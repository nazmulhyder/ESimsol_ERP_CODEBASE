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
    public class PurchaseOrderRegisterController : Controller
    {   
        string _sDateRange = "";
        string _sErrorMesage = "";
        string _sFormatter = "";
        List<PurchaseOrderRegister> _oPurchaseOrderRegisters = new List<PurchaseOrderRegister>();        

        #region Actions
        public ActionResult ViewPurchaseOrderRegisters(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            PurchaseOrderRegister oPurchaseOrderRegister = new PurchaseOrderRegister();

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
            ViewBag.PurchaseOrderStates = EnumObject.jGets(typeof(EnumPOStatus));
            
            return View(oPurchaseOrderRegister);
        }

        [HttpPost]
        public ActionResult SetSessionSearchCriteria(PurchaseOrderRegister oPurchaseOrderRegister)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oPurchaseOrderRegister);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintPurchaseOrderRegister(double ts)
        {
            PurchaseOrderRegister oPurchaseOrderRegister = new PurchaseOrderRegister();
            try
            {
                _sErrorMesage = "";
                _oPurchaseOrderRegisters = new List<PurchaseOrderRegister>();                
                oPurchaseOrderRegister = (PurchaseOrderRegister)Session[SessionInfo.ParamObj];
                string sSQL = this.GetSQL(oPurchaseOrderRegister);
                _oPurchaseOrderRegisters = PurchaseOrderRegister.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (_oPurchaseOrderRegisters.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oPurchaseOrderRegisters = new List<PurchaseOrderRegister>();
                _sErrorMesage = ex.Message;
            }

            if (_sErrorMesage == "")
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                BusinessUnit oBU = new BusinessUnit();
                oBU = oBU.Get(_oPurchaseOrderRegisters[0].BUID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBU);

                rptPurchaseOrderRegisters oReport = new rptPurchaseOrderRegisters();
                byte[] abytes = oReport.PrepareReport(_oPurchaseOrderRegisters, oCompany, oPurchaseOrderRegister.ReportLayout, _sDateRange);
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
        public void ExportToExcelPurchaseOrderRegister(double ts)
        {
            int POID=-999;
            string Header = "", HeaderColumn = "";

            Company oCompany = new Company();
            PurchaseOrderRegister oPurchaseOrderRegister = new PurchaseOrderRegister();
            try
            {
                _sErrorMesage = "";

                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oPurchaseOrderRegisters = new List<PurchaseOrderRegister>();
                oPurchaseOrderRegister = (PurchaseOrderRegister)Session[SessionInfo.ParamObj];
                string sSQL = this.GetSQL(oPurchaseOrderRegister);
                _oPurchaseOrderRegisters = PurchaseOrderRegister.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (_oPurchaseOrderRegisters.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oPurchaseOrderRegisters = new List<PurchaseOrderRegister>();
                _sErrorMesage = ex.Message;
            }

            if (_sErrorMesage == "") 
            {
                #region Header
                List<TableHeader> table_header = new List<TableHeader>();
                table_header.Add(new TableHeader { Header = "#SL", Width = 10f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "PO No", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "PO Status", Width = 15f, IsRotate = false });
                
                if (oPurchaseOrderRegister.ReportLayout == EnumReportLayout.DateWiseDetails)
                    table_header.Add(new TableHeader { Header = "Contractor Name", Width = 45f, IsRotate = false });
                else
                    table_header.Add(new TableHeader { Header = "PO Date", Width = 25f, IsRotate = false });
               
                table_header.Add(new TableHeader { Header = "Date Of Ref", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Approved By Name", Width = 25f, IsRotate = false });

                if (oPurchaseOrderRegister.ReportLayout == EnumReportLayout.ProductWise)
                {
                    table_header.Add(new TableHeader { Header = "ApproveDate", Width = 20f, IsRotate = false });
                    table_header.Add(new TableHeader { Header = "Supplier Name", Width = 45f, IsRotate = false });
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
                if (oPurchaseOrderRegister.ReportLayout == EnumReportLayout.ProductWise) 
                {
                    Header = "Product Wise"; HeaderColumn="Product Name : "; 
                }
                else if (oPurchaseOrderRegister.ReportLayout == EnumReportLayout.PartyWiseDetails)
                {
                    Header = "Party Wise"; HeaderColumn="Party Name : "; 
                }
                else if (oPurchaseOrderRegister.ReportLayout == EnumReportLayout.DateWiseDetails)
                {
                    Header = "Date Wise"; HeaderColumn="PO Date : ";
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
                    var sheet = excelPackage.Workbook.Worksheets.Add("Purchase Order Register");
                    sheet.Name = "Purchase Order Register";

                    foreach (TableHeader listItem in table_header)
                    {
                        sheet.Column(nStartCol++).Width = listItem.Width;
                    }

                    #region Report Header
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, 7]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, 8, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = "Purchase Order Register (" + Header+") "; cell.Style.Font.Bold = true;
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
                    var data = _oPurchaseOrderRegisters.GroupBy(x => new { x.ContractorName, x.ContractorID}, (key, grp) => new
                    {
                        HeaderName = key.ContractorName,
                        TotalQty = grp.Sum(x => x.Qty),
                        TotalAmount= grp.Sum(x => x.Amount),
                        Results = grp.ToList()
                    });

                    if (oPurchaseOrderRegister.ReportLayout == EnumReportLayout.ProductWise)
                    {
                        data = _oPurchaseOrderRegisters.GroupBy(x => new { x.ProductName, x.ProductID }, (key, grp) => new
                        {
                            HeaderName = key.ProductName,
                            TotalQty = grp.Sum(x => x.Qty),
                            TotalAmount = grp.Sum(x => x.Amount),
                            Results = grp.ToList()
                        });
                    }
                    else if (oPurchaseOrderRegister.ReportLayout == EnumReportLayout.DateWiseDetails)
                    {
                        data = _oPurchaseOrderRegisters.GroupBy(x => new { x.PODateSt }, (key, grp) => new
                        {
                            HeaderName = key.PODateSt,
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

                        POID = 0; 
                        nRowIndex++; int nCount = 0, nRowSpan=0;
                        foreach (var obj in oItem.Results)
                        {
                            #region Order Wise Merge
                            if (POID != obj.POID) 
                            {
                                if (nCount > 0) 
                                {
                                    nStartCol = 8;
                                    FillCellMerge(ref sheet, "Sub Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 2, true, ExcelHorizontalAlignment.Right);

                                    _sFormatter = " #,##0;(#,##0)";
                                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.Results.Where(x => x.POID == POID).Sum(x => x.Qty).ToString(), true, true);
                                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);

                                    _sFormatter = obj.CurrencySymbol + " #,##0.00;(#,##0.00)";
                                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.Results.Where(x => x.POID == POID).Sum(x => x.Amount).ToString(), true, true);
                                    nRowIndex++;
                                }

                                nStartCol = 2;
                                nRowSpan = oItem.Results.Where(OrderR => OrderR.POID == obj.POID).ToList().Count;

                                FillCellMerge(ref sheet,(++nCount).ToString(), nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
                                FillCellMerge(ref sheet, obj.PONo, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
                                FillCellMerge(ref sheet, obj.Status.ToString(), nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
                                
                                if (oPurchaseOrderRegister.ReportLayout != EnumReportLayout.DateWiseDetails)
                                    FillCellMerge(ref sheet, obj.ContractorName, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
                                else
                                    FillCellMerge(ref sheet, obj.PODateSt, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);

                                FillCellMerge(ref sheet, obj.RefDateSt, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
                                FillCellMerge(ref sheet, obj.ApprovedByName, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
                            }
                            #endregion

                            nStartCol = 8;

                            if (oPurchaseOrderRegister.ReportLayout == EnumReportLayout.ProductWise) 
                            {
                                FillCell(sheet, nRowIndex, nStartCol++, obj.ApproveDateSt, false);
                                FillCell(sheet, nRowIndex, nStartCol++, obj.ContractorName, false);
                            }
                            else
                            {
                                FillCell(sheet, nRowIndex, nStartCol++, obj.ProductCode, false);
                                FillCell(sheet, nRowIndex, nStartCol++, obj.ProductName, false);
                            }

                            FillCell(sheet, nRowIndex, nStartCol++, obj.UnitName, false);
                            _sFormatter = " #,##0;(#,##0)";
                            FillCell(sheet, nRowIndex, nStartCol++, obj.Qty.ToString(), true);
                            _sFormatter = obj.CurrencySymbol + " #,##0.00;(#,##0.00)";
                            FillCell(sheet, nRowIndex, nStartCol++, obj.UnitPrice.ToString(), true);
                            FillCell(sheet, nRowIndex, nStartCol++, obj.Amount.ToString(), true);
                            nRowIndex++;

                            POID = obj.POID;
                            sCurrencySymbol = obj.CurrencySymbol;
                        }

                        nStartCol = 8;
                        FillCellMerge(ref sheet, "Sub Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 2, true, ExcelHorizontalAlignment.Right);
                        _sFormatter = " #,##0;(#,##0)";
                        FillCell(sheet, nRowIndex, ++nStartCol, oItem.Results.Where(x => x.POID == POID).Sum(x => x.Qty).ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        _sFormatter = sCurrencySymbol + " #,##0.00;(#,##0.00)";
                        FillCell(sheet, nRowIndex, ++nStartCol, oItem.Results.Where(x => x.POID == POID).Sum(x => x.Amount).ToString(), true, true);
                        nRowIndex++;

                        nStartCol = 2; _sFormatter = " #,##0;(#,##0)";
                        FillCellMerge(ref sheet, Header + " Sub Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 8, true, ExcelHorizontalAlignment.Right);
                        FillCell(sheet, nRowIndex, ++nStartCol, oItem.TotalQty.ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false, true); _sFormatter = sCurrencySymbol + " #,##0.00;(#,##0.00)";
                        FillCell(sheet, nRowIndex, ++nStartCol, oItem.TotalAmount.ToString(), true, true);
                        nRowIndex++;
                    }

                    nStartCol = 2; _sFormatter = " #,##0;(#,##0)";
                    FillCellMerge(ref sheet, "Grand Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 8, true, ExcelHorizontalAlignment.Right);
                    FillCell(sheet, nRowIndex, ++nStartCol, data.Sum(x => x.TotalQty).ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false, true); _sFormatter = sCurrencySymbol + " #,##0.00;(#,##0.00)";
                    FillCell(sheet, nRowIndex, ++nStartCol, data.Sum(x=>x.TotalAmount).ToString(), true, true);
                    nRowIndex++;

                    #endregion

                    cell = sheet.Cells[1, 1, nRowIndex, 13];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=PurchaseOrderRegister(" + Header + ").xlsx");
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
        private string GetSQL(PurchaseOrderRegister oPurchaseOrderRegister)
        {
            _sDateRange = "";
            string sSearchingData = oPurchaseOrderRegister.SearchingData;
            EnumCompareOperator ePODate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[0]);
            DateTime dPurchaseOrderStartDate = Convert.ToDateTime(sSearchingData.Split('~')[1]);
            DateTime dPurchaseOrderEndDate = Convert.ToDateTime(sSearchingData.Split('~')[2]);

            EnumCompareOperator eRefDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[3]);
            DateTime dRefDateStartDate = Convert.ToDateTime(sSearchingData.Split('~')[4]);
            DateTime dRefDateEndDate = Convert.ToDateTime(sSearchingData.Split('~')[5]);

            EnumCompareOperator eApproveDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[6]);
            DateTime dApprovedStartDate = Convert.ToDateTime(sSearchingData.Split('~')[7]);
            DateTime dApprovedEndDate = Convert.ToDateTime(sSearchingData.Split('~')[8]);

            EnumCompareOperator ePOAmount = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[9]);
            double nPOAmountStsrt = Convert.ToDouble(sSearchingData.Split('~')[10]);
            double nPOAmountEnd = Convert.ToDouble(sSearchingData.Split('~')[11]);

            string sSQLQuery = "", sWhereCluse = "", sGroupBy = "", sOrderBy = "";

            #region BusinessUnit
            if (oPurchaseOrderRegister.BUID > 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " BUID =" + oPurchaseOrderRegister.BUID.ToString();
            }
            #endregion

            #region PONo
            if (oPurchaseOrderRegister.PONo != null && oPurchaseOrderRegister.PONo != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " PONo LIKE'%" + oPurchaseOrderRegister.PONo + "%'";
            }
            #endregion

            #region RefNo
            if (oPurchaseOrderRegister.RefNo != null && oPurchaseOrderRegister.RefNo != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " ReflNo LIKE'%" + oPurchaseOrderRegister.RefNo + "%'";
            }
            #endregion

            #region ApprovedBy
            if (oPurchaseOrderRegister.ApprovedBy != 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " ApproveBy =" + oPurchaseOrderRegister.ApprovedBy.ToString();
            }
            #endregion

            #region PurchaseOrderStatus
            if (oPurchaseOrderRegister.StatusInt>=0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " Status =" + oPurchaseOrderRegister.StatusInt;
            }
            #endregion

            #region Remarks
            if (oPurchaseOrderRegister.Remarks != null && oPurchaseOrderRegister.Remarks != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " Note LIKE'%" + oPurchaseOrderRegister.Remarks + "%'";
            }
            #endregion

            #region Supplier
            if (oPurchaseOrderRegister.ContractorName != null && oPurchaseOrderRegister.ContractorName != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " ContractorID IN(" + oPurchaseOrderRegister.ContractorName + ")";
            }
            #endregion

            #region Product
            if (oPurchaseOrderRegister.ProductName != null && oPurchaseOrderRegister.ProductName != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " ProductID IN(" + oPurchaseOrderRegister.ProductName + ")";
            }
            #endregion

            #region PODate
            if (ePODate != EnumCompareOperator.None)
            {
                if (ePODate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),PODate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPurchaseOrderStartDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "PO Date @ " + dPurchaseOrderStartDate.ToString("dd MMM yyyy");
                }
                else if (ePODate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),PODate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPurchaseOrderStartDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "PO Date Not Equal @ " + dPurchaseOrderStartDate.ToString("dd MMM yyyy");
                }
                else if (ePODate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),PODate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPurchaseOrderStartDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "PO Date Greater Then @ " + dPurchaseOrderStartDate.ToString("dd MMM yyyy");
                }
                else if (ePODate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),PODate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPurchaseOrderStartDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "PO Date Smaller Then @ " + dPurchaseOrderStartDate.ToString("dd MMM yyyy");
                }
                else if (ePODate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),PODate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPurchaseOrderStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPurchaseOrderEndDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "PO Date Between " + dPurchaseOrderStartDate.ToString("dd MMM yyyy") + " To " + dPurchaseOrderEndDate.ToString("dd MMM yyyy");
                }
                else if (ePODate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),PODate,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPurchaseOrderStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPurchaseOrderEndDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "PO Date NOT Between " + dPurchaseOrderStartDate.ToString("dd MMM yyyy") + " To " + dPurchaseOrderEndDate.ToString("dd MMM yyyy");
                }
            }
            #endregion

            #region RefDate Date
            if (eRefDate != EnumCompareOperator.None)
            {
                if (eRefDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),RefDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dRefDateStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eRefDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),RefDate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dRefDateStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eRefDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),RefDate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dRefDateStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eRefDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),RefDate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dRefDateStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eRefDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),RefDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dRefDateStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dRefDateEndDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eRefDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),RefDate,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dRefDateStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dRefDateEndDate.ToString("dd MMM yyyy") + "', 106))";
                }
            }
            #endregion

            #region Approved Date
            if (eApproveDate != EnumCompareOperator.None)
            {
                if (eApproveDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApprovedStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eApproveDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApprovedStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eApproveDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApprovedStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eApproveDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApprovedStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eApproveDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApprovedStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApprovedEndDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eApproveDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApprovedStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApprovedEndDate.ToString("dd MMM yyyy") + "', 106))";
                }
            }
            #endregion

            #region Amount
            if (ePOAmount != EnumCompareOperator.None)
            {
                if (ePOAmount == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " Amount = " + nPOAmountStsrt.ToString("0.00");
                }
                else if (ePOAmount == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " Amount != " + nPOAmountStsrt.ToString("0.00"); ;
                }
                else if (ePOAmount == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " Amount > " + nPOAmountStsrt.ToString("0.00"); ;
                }
                else if (ePOAmount == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " Amount < " + nPOAmountStsrt.ToString("0.00"); ;
                }
                else if (ePOAmount == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " Amount BETWEEN " + nPOAmountStsrt.ToString("0.00") + " AND " + nPOAmountEnd.ToString("0.00");
                }
                else if (ePOAmount == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " Amount NOT BETWEEN " + nPOAmountStsrt.ToString("0.00") + " AND " + nPOAmountEnd.ToString("0.00");
                }
            }
            #endregion

            #region Report Layout
           if (oPurchaseOrderRegister.ReportLayout == EnumReportLayout.DateWiseDetails)
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT * FROM View_PurchaseOrderRegister ";
                sOrderBy = " ORDER BY  PODate, POID ASC";
            }
            else if (oPurchaseOrderRegister.ReportLayout == EnumReportLayout.PartyWiseDetails)
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT * FROM View_PurchaseOrderRegister ";
                sOrderBy = " ORDER BY  ContractorName, POID ASC";
            }
            else if (oPurchaseOrderRegister.ReportLayout == EnumReportLayout.ProductWise)
            {
               sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
               sSQLQuery = "SELECT * FROM View_PurchaseOrderRegister ";
               sOrderBy = " ORDER BY  ProductName,ProductID, POID ASC";
            }
            else
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT * FROM View_PurchaseOrderRegister ";
                sOrderBy = " ORDER BY PODate, POID ASC";
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

