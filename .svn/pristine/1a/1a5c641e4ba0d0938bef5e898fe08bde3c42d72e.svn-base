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
    public class QCRegisterController : Controller
    {   
        string _sDateRange = "";
        string _sErrorMesage = "";
        string _sFormatter = "";
        List<QCRegister> _oQCRegisters = new List<QCRegister>();        

        #region Actions
        public ActionResult ViewQCRegisters(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            QCRegister oQCRegister = new QCRegister();

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
                if ((EnumReportLayout)oItem.id == EnumReportLayout.DateWiseDetails || (EnumReportLayout)oItem.id == EnumReportLayout.PartyWiseDetails || (EnumReportLayout)oItem.id == EnumReportLayout.ProductWise || (EnumReportLayout)oItem.id == EnumReportLayout.QC_Follow_Up)
                {
                    oReportLayouts.Add(oItem);
                }
            }
            #endregion

            ViewBag.BUID = buid;
            ViewBag.ApprovalUsers = oApprovalUsers;
            ViewBag.ReportLayouts = oReportLayouts;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.QCStates = EnumObject.jGets(typeof(EnumQCStatus));
            ViewBag.Stores = WorkingUnit.GetsPermittedStore(buid, EnumModuleName.QC, EnumStoreType.ReceiveStore, ((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(oQCRegister);
        }

        [HttpPost]
        public ActionResult SetSessionSearchCriteria(QCRegister oQCRegister)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oQCRegister);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintQCRegister(double ts)
        {
            QCRegister oQCRegister = new QCRegister();
            try
            {
                _sErrorMesage = "";
                _oQCRegisters = new List<QCRegister>();
                oQCRegister = (QCRegister)Session[SessionInfo.ParamObj];

                if (oQCRegister.ReportLayout == EnumReportLayout.QC_Follow_Up)
                {
                    string sSearchingData = oQCRegister.SearchingData;
                    DateTime dCStartDate = Convert.ToDateTime(sSearchingData.Split('~')[1]);
                    DateTime dCEndDate = Convert.ToDateTime(sSearchingData.Split('~')[2]);
                    oQCRegister.ConsumptionStartDate = dCStartDate;
                    oQCRegister.ConsumptionEndDate = dCEndDate;
                    _sDateRange = "QC Date Between " + oQCRegister.ConsumptionStartDateInStr + " To " + oQCRegister.ConsumptionEndDateInStr;
                    _oQCRegisters = oQCRegister.GetsByQCFollowUp((int)Session[SessionInfo.currentUserID]);
                }
                else
                {
                    string sSQL = this.GetSQL(oQCRegister);
                    _oQCRegisters = QCRegister.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                }

                if (_oQCRegisters.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oQCRegisters = new List<QCRegister>();
                _sErrorMesage = ex.Message;
            }

            if (_sErrorMesage == "")
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                BusinessUnit oBU = new BusinessUnit();
                oBU = oBU.Get(_oQCRegisters.Max(x=>x.BUID), (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBU);

                if (oQCRegister.ReportLayout == EnumReportLayout.QC_Follow_Up)
                {
                    rptQCFURegisters oReport = new rptQCFURegisters();
                    byte[] abytes = oReport.PrepareReport(_oQCRegisters, oCompany, _sDateRange);
                    return File(abytes, "application/pdf");
                }
                else
                {
                    rptQCRegisters oReport = new rptQCRegisters();
                    byte[] abytes = oReport.PrepareReport(_oQCRegisters, oCompany, oQCRegister.ReportLayout, _sDateRange);
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
        public void ExportToExcelQCRegister(double ts)
        {
            int ProductionSheetID=-999;
            string Header = "", HeaderColumn = "";

            Company oCompany = new Company();
            QCRegister oQCRegister = new QCRegister();
            try
            {
                _sErrorMesage = "";

                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oQCRegisters = new List<QCRegister>();
                oQCRegister = (QCRegister)Session[SessionInfo.ParamObj];

                if (oQCRegister.ReportLayout == EnumReportLayout.QC_Follow_Up)
                {
                    string sSearchingData = oQCRegister.SearchingData;
                    DateTime dCStartDate = Convert.ToDateTime(sSearchingData.Split('~')[1]);
                    DateTime dCEndDate = Convert.ToDateTime(sSearchingData.Split('~')[2]);
                    oQCRegister.ConsumptionStartDate = dCStartDate;
                    oQCRegister.ConsumptionEndDate = dCEndDate;
                    _sDateRange = "QC Date Between " + oQCRegister.ConsumptionStartDateInStr + " To " + oQCRegister.ConsumptionEndDateInStr;
                    _oQCRegisters = oQCRegister.GetsByQCFollowUp((int)Session[SessionInfo.currentUserID]);
                }
                else
                {
                    string sSQL = this.GetSQL(oQCRegister);
                    _oQCRegisters = QCRegister.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                }

                if (_oQCRegisters.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oQCRegisters = new List<QCRegister>();
                _sErrorMesage = ex.Message;
            }

            if (_sErrorMesage == "") 
            {
                if (oQCRegister.ReportLayout == EnumReportLayout.QC_Follow_Up)
                {
                    ExportToExcelForQCFUWise(_oQCRegisters, oCompany, _sDateRange);
                }
                else
                {
                    #region Header
                    List<TableHeader> table_header = new List<TableHeader>();
                    table_header.Add(new TableHeader { Header = "#SL", Width = 10f, IsRotate = false });
                    table_header.Add(new TableHeader { Header = "Sheet No", Width = 15f, IsRotate = false });

                    if (oQCRegister.ReportLayout == EnumReportLayout.DateWiseDetails)
                        table_header.Add(new TableHeader { Header = "Store Name", Width = 45f, IsRotate = false });
                    else
                        table_header.Add(new TableHeader { Header = "QC Date", Width = 25f, IsRotate = false });

                    table_header.Add(new TableHeader { Header = "QC Person", Width = 25f, IsRotate = false });

                    if (oQCRegister.ReportLayout == EnumReportLayout.ProductWise)
                    {
                        table_header.Add(new TableHeader { Header = "Lot No", Width = 20f, IsRotate = false });
                        table_header.Add(new TableHeader { Header = "Store Name", Width = 45f, IsRotate = false });
                    }
                    else
                    {
                        table_header.Add(new TableHeader { Header = "Store Name", Width = 20f, IsRotate = false });
                        table_header.Add(new TableHeader { Header = "Product Name", Width = 45f, IsRotate = false });
                    }

                    table_header.Add(new TableHeader { Header = "M Unit", Width = 10f, IsRotate = false });
                    table_header.Add(new TableHeader { Header = "Rej. Qty", Width = 10f, IsRotate = false });
                    table_header.Add(new TableHeader { Header = "Pass Qty", Width = 10f, IsRotate = false });
                    table_header.Add(new TableHeader { Header = "Amount", Width = 10f, IsRotate = false });
                    table_header.Add(new TableHeader { Header = "Unit Price", Width = 10f, IsRotate = false });
                    #endregion

                    #region Layout Wise Header
                    if (oQCRegister.ReportLayout == EnumReportLayout.ProductWise)
                    {
                        Header = "Product Wise"; HeaderColumn = "Product Name : ";
                    }
                    else if (oQCRegister.ReportLayout == EnumReportLayout.PartyWiseDetails)
                    {
                        Header = "Party Wise"; HeaderColumn = "Party Name : ";
                    }
                    else if (oQCRegister.ReportLayout == EnumReportLayout.DateWiseDetails)
                    {
                        Header = "Date Wise"; HeaderColumn = "QC Date : ";
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
                        var sheet = excelPackage.Workbook.Worksheets.Add("QC Register");
                        sheet.Name = "QC Register";

                        foreach (TableHeader listItem in table_header)
                        {
                            sheet.Column(nStartCol++).Width = listItem.Width;
                        }

                        #region Report Header
                        cell = sheet.Cells[nRowIndex, 2, nRowIndex, 6]; cell.Merge = true;
                        cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        cell = sheet.Cells[nRowIndex, 7, nRowIndex, nEndCol]; cell.Merge = true;
                        cell.Value = "QC Register (" + Header + ") "; cell.Style.Font.Bold = true;
                        cell.Style.WrapText = true;
                        cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        nRowIndex = nRowIndex + 1;
                        #endregion

                        #region Address & Date
                        cell = sheet.Cells[nRowIndex, 2, nRowIndex, 6]; cell.Merge = true;
                        cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        cell = sheet.Cells[nRowIndex, 7, nRowIndex, 10]; cell.Merge = true;
                        cell.Value = ""; cell.Style.Font.Bold = false;
                        cell.Style.WrapText = true;
                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        nRowIndex = nRowIndex + 1;
                        #endregion

                        #region Group By Layout Wise
                        var data = _oQCRegisters.GroupBy(x => new { x.BuyerName, x.BuyerID }, (key, grp) => new
                        {
                            HeaderName = key.BuyerName,
                            TotalRejQty = grp.Sum(x => x.RejectQuantity),
                            TotalPassQty = grp.Sum(x => x.PassQuantity),
                            TotalAmount = grp.Sum(x => x.Amount),
                            Results = grp.ToList()
                        });

                        if (oQCRegister.ReportLayout == EnumReportLayout.ProductWise)
                        {
                            data = _oQCRegisters.GroupBy(x => new { x.ProductName, x.ProductID }, (key, grp) => new
                            {
                                HeaderName = key.ProductName,
                                TotalRejQty = grp.Sum(x => x.RejectQuantity),
                                TotalPassQty = grp.Sum(x => x.PassQuantity),
                                TotalAmount = grp.Sum(x => x.Amount),
                                Results = grp.ToList()
                            });
                        }
                        else if (oQCRegister.ReportLayout == EnumReportLayout.DateWiseDetails)
                        {
                            data = _oQCRegisters.GroupBy(x => new { x.OperationTimeInString }, (key, grp) => new
                            {
                                HeaderName = key.OperationTimeInString,
                                TotalRejQty = grp.Sum(x => x.RejectQuantity),
                                TotalPassQty = grp.Sum(x => x.PassQuantity),
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
                            FillCellMerge(ref sheet, HeaderColumn + oItem.HeaderName, nRowIndex, nRowIndex, nStartCol, nEndCol + 1, true, ExcelHorizontalAlignment.Left);

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
                                #region Order Wise Merge
                                if (ProductionSheetID != obj.ProductionSheetID)
                                {
                                    if (nCount > 0)
                                    {
                                        nStartCol = 6;
                                        FillCellMerge(ref sheet, "Sub Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 2, true, ExcelHorizontalAlignment.Right);

                                        _sFormatter = " #,##0;(#,##0)";
                                        FillCell(sheet, nRowIndex, ++nStartCol, oItem.Results.Where(x => x.ProductionSheetID == ProductionSheetID).Sum(x => x.RejectQuantity).ToString(), true, true);
                                        FillCell(sheet, nRowIndex, ++nStartCol, oItem.Results.Where(x => x.ProductionSheetID == ProductionSheetID).Sum(x => x.PassQuantity).ToString(), true, true);
                                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);

                                        _sFormatter = obj.CurrencySymbol + " #,##0.00;(#,##0.00)";
                                        FillCell(sheet, nRowIndex, ++nStartCol, oItem.Results.Where(x => x.ProductionSheetID == ProductionSheetID).Sum(x => x.Amount).ToString(), true, true);
                                        nRowIndex++;
                                    }

                                    nStartCol = 2;
                                    nRowSpan = oItem.Results.Where(OrderR => OrderR.ProductionSheetID == obj.ProductionSheetID).ToList().Count;

                                    FillCellMerge(ref sheet, (++nCount).ToString(), nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
                                    FillCellMerge(ref sheet, obj.SheetNo, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
                                    FillCellMerge(ref sheet, obj.QCPersonName, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);

                                    if (oQCRegister.ReportLayout == EnumReportLayout.DateWiseDetails)
                                        FillCellMerge(ref sheet, obj.StoreName, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
                                    else
                                        FillCellMerge(ref sheet, obj.OperationTimeInString, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);

                                }
                                #endregion

                                nStartCol = 6;

                                if (oQCRegister.ReportLayout == EnumReportLayout.ProductWise)
                                {
                                    FillCell(sheet, nRowIndex, nStartCol++, obj.LotNo, false);
                                    FillCell(sheet, nRowIndex, nStartCol++, obj.StoreName, false);
                                }
                                else
                                {
                                    FillCell(sheet, nRowIndex, nStartCol++, obj.ProductCode, false);
                                    FillCell(sheet, nRowIndex, nStartCol++, obj.ProductName, false);
                                }

                                FillCell(sheet, nRowIndex, nStartCol++, obj.MUName, false);
                                _sFormatter = " #,##0;(#,##0)";
                                FillCell(sheet, nRowIndex, nStartCol++, obj.RejectQuantity.ToString(), true);
                                FillCell(sheet, nRowIndex, nStartCol++, obj.PassQuantity.ToString(), true);
                                _sFormatter = obj.CurrencySymbol + " #,##0.00;(#,##0.00)";
                                FillCell(sheet, nRowIndex, nStartCol++, obj.UnitPrice.ToString(), true);
                                FillCell(sheet, nRowIndex, nStartCol++, obj.TotalQty.ToString(), true);
                                nRowIndex++;

                                ProductionSheetID = obj.ProductionSheetID;
                                sCurrencySymbol = obj.CurrencySymbol;
                            }
                            #region SubTotal
                            nStartCol = 6;
                            FillCellMerge(ref sheet, "Sub Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 2, true, ExcelHorizontalAlignment.Right);
                            _sFormatter = " #,##0;(#,##0)";
                            FillCell(sheet, nRowIndex, ++nStartCol, oItem.Results.Where(x => x.ProductionSheetID == ProductionSheetID).Sum(x => x.RejectQuantity).ToString(), true, true);
                            FillCell(sheet, nRowIndex, ++nStartCol, oItem.Results.Where(x => x.ProductionSheetID == ProductionSheetID).Sum(x => x.PassQuantity).ToString(), true, true);
                            FillCell(sheet, nRowIndex, ++nStartCol, "", false);

                            _sFormatter = sCurrencySymbol + " #,##0.00;(#,##0.00)";
                            FillCell(sheet, nRowIndex, ++nStartCol, oItem.Results.Where(x => x.ProductionSheetID == ProductionSheetID).Sum(x => x.Amount).ToString(), true, true);
                            nRowIndex++;
                            #endregion

                            #region SubTotal (layout Wise)
                            nStartCol = 2; _sFormatter = " #,##0;(#,##0)";
                            FillCellMerge(ref sheet, Header + " Sub Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 6, true, ExcelHorizontalAlignment.Right);
                            FillCell(sheet, nRowIndex, ++nStartCol, oItem.TotalRejQty.ToString(), true, true);
                            FillCell(sheet, nRowIndex, ++nStartCol, oItem.TotalPassQty.ToString(), true, true);
                            FillCell(sheet, nRowIndex, ++nStartCol, "", false);

                            _sFormatter = sCurrencySymbol + " #,##0.00;(#,##0.00)";
                            FillCell(sheet, nRowIndex, ++nStartCol, oItem.TotalAmount.ToString(), true, true);
                            nRowIndex++;
                            #endregion
                        }
                        #region Grand Total
                        nStartCol = 2; _sFormatter = " #,##0;(#,##0)";
                        FillCellMerge(ref sheet, "Grand Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 6, true, ExcelHorizontalAlignment.Right);
                        FillCell(sheet, nRowIndex, ++nStartCol, _oQCRegisters.Sum(x => x.RejectQuantity).ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, _oQCRegisters.Sum(x => x.PassQuantity).ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        _sFormatter = sCurrencySymbol + " #,##0.00;(#,##0.00)";
                        FillCell(sheet, nRowIndex, ++nStartCol, _oQCRegisters.Sum(x => x.Amount).ToString(), true, true);
                        nRowIndex++;
                        #endregion

                        cell = sheet.Cells[1, 1, nRowIndex, 12];
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        #endregion

                        Response.ClearContent();
                        Response.BinaryWrite(excelPackage.GetAsByteArray());
                        Response.AddHeader("content-disposition", "attachment; filename=QCRegister(" + Header + ").xlsx");
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Flush();
                        Response.End();
                    }
                    #endregion
                }
            }
        }
        private void ExportToExcelForQCFUWise(List<QCRegister> _oQCRegisters, Company oCompany, string _sDateRange)
        {
            #region Print XL

            int nRowIndex = 2, nStartCol = 2, nEndCol = 14, nColumn = 1, nCount = 0;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("ExportQCRegister");
                sheet.Name = "Export QC Register";
                sheet.Column(++nColumn).Width = 10; //SL No
                sheet.Column(++nColumn).Width = 15; //Sheet No
                sheet.Column(++nColumn).Width = 35; //PI No
                sheet.Column(++nColumn).Width = 30; //Buyer Name
                sheet.Column(++nColumn).Width = 20; //Sheet Issue Date
                sheet.Column(++nColumn).Width = 20; //Product Name
                sheet.Column(++nColumn).Width = 15; //Color Name
                sheet.Column(++nColumn).Width = 10; //M.Unit
                sheet.Column(++nColumn).Width = 20; //Sheet Qty
                sheet.Column(++nColumn).Width = 20; //Raw Consumption
                sheet.Column(++nColumn).Width = 15; //QC Pass Qty
                sheet.Column(++nColumn).Width = 15; //Reject Qty
                sheet.Column(++nColumn).Width = 15; //Yet to QC

                #region Report Header
                cell = sheet.Cells[nRowIndex, nStartCol + 2, nRowIndex, nEndCol - 6]; cell.Merge = true;
                cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                cell = sheet.Cells[nRowIndex, nStartCol + 8, nRowIndex, nEndCol - 1]; cell.Merge = true;
                cell.Value = "Export QC Register(QC Follow Up wise)"; cell.Style.Font.Bold = true;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Address & Date
                cell = sheet.Cells[nRowIndex, nStartCol + 2, nRowIndex, nEndCol - 6]; cell.Merge = true;
                cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                cell = sheet.Cells[nRowIndex, nStartCol + 8, nRowIndex, nEndCol - 1]; cell.Merge = true;
                cell.Value = _sDateRange; cell.Style.Font.Bold = false;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Report Data

                #region Blank
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = ""; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region
                nColumn = 1;
                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Sheet No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "PI No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Buyer Name"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Issue Date"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Product Name"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Color Name"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "M.Unit"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "SheetQty"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Raw Consumption"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "QC Pass Qty"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Reject Qty"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Yet to QC"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nRowIndex++;
                #endregion

                #region Data
                double nTotalSheetQty = 0, nTotalConsumptionQty = 0, nTotalQCPassQty = 0, nTotalRejectQty = 0, nTotalYetToQC = 0;
                foreach (QCRegister oItem in _oQCRegisters)
                {
                    nCount++;
                    nColumn = 1;
                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = nCount; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.SheetNo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.ExportPINo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.BuyerName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.PSIssueDateInString; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.ColorName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.MUName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.SheetQty; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.ConsumptionQty; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.QCPassQty; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.RejectQty; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.YetToQCQty; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nTotalSheetQty = nTotalSheetQty + oItem.SheetQty;
                    nTotalConsumptionQty = nTotalConsumptionQty + oItem.ConsumptionQty;
                    nTotalQCPassQty = nTotalQCPassQty + oItem.QCPassQty;
                    nTotalRejectQty = nTotalRejectQty + oItem.RejectQty;
                    nTotalYetToQC = nTotalYetToQC + oItem.YetToQCQty;

                    nRowIndex += 1;
                }
                #endregion

                #region Grand Total
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 5]; cell.Merge = true; cell.Value = "Grand Total:"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nColumn = 9;
                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = nTotalSheetQty; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = nTotalConsumptionQty; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = nTotalQCPassQty; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = nTotalRejectQty; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = nTotalYetToQC; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nRowIndex++;
                #endregion

                #endregion

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=ExportQCRegister.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
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
        private string GetSQL(QCRegister oQCRegister)
        {
            _sDateRange = "";
            string sSearchingData = oQCRegister.SearchingData;
            EnumCompareOperator eOperationTime = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[0]);
            DateTime dQCStartDate = Convert.ToDateTime(sSearchingData.Split('~')[1]);
            DateTime dQCEndDate = Convert.ToDateTime(sSearchingData.Split('~')[2]);

            string sSQLQuery = "", sWhereCluse = "", sGroupBy = "", sOrderBy = "";

            #region BusinessUnit
            if (oQCRegister.BUID > 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " BUID =" + oQCRegister.BUID.ToString();
            }
            #endregion

            #region SheetNo
            if (oQCRegister.SheetNo != null && oQCRegister.SheetNo != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " SheetNo LIKE'%" + oQCRegister.SheetNo + "%'";
            }
            #endregion

            #region LotNo
            if (oQCRegister.LotNo != null && oQCRegister.LotNo != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " LotNo LIKE'%" + oQCRegister.LotNo + "%'";
            }
            #endregion

            #region QCPerson
            if (oQCRegister.QCPerson != 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " QCPerson =" + oQCRegister.QCPerson;
            }
            #endregion

            #region ApproveBy
            DateObject.CompareDateQuery(ref sWhereCluse, " OperationTime ", (int)eOperationTime, dQCStartDate, dQCEndDate);
            #endregion

            #region QCStatus
            if (oQCRegister.QCStatus > 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " QCStatus =" + oQCRegister.QCStatus;
            }
            #endregion

            #region BuyerName
            if (oQCRegister.BuyerName != null && oQCRegister.BuyerName != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " BuyerID IN(" + oQCRegister.BuyerName + ")";
            }
            #endregion

            #region Product
            if (oQCRegister.ProductName != null && oQCRegister.ProductName != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " ProductID IN(" + oQCRegister.ProductName + ")";
            }
            #endregion

            #region StoreName
            if (oQCRegister.StoreName != null && oQCRegister.StoreName != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " WorkingUnitID IN(" + oQCRegister.StoreName + ")";
            }
            #endregion

            #region Report Layout
            if (oQCRegister.ReportLayout == EnumReportLayout.DateWiseDetails)
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT * FROM View_QCRegister ";
                sOrderBy = " ORDER BY  OperationTime, ProductionSheetID ASC";
            }
            else if (oQCRegister.ReportLayout == EnumReportLayout.PartyWiseDetails)
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT * FROM View_QCRegister ";
                sOrderBy = " ORDER BY  BuyerName,BuyerID, ProductionSheetID ASC";
            }
            else if (oQCRegister.ReportLayout == EnumReportLayout.ProductWise)
            {
               sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
               sSQLQuery = "SELECT * FROM View_QCRegister ";
               sOrderBy = " ORDER BY  ProductName,ProductID, ProductionSheetID ASC";
            }
            else
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT * FROM View_QCRegister ";
                sOrderBy = " ORDER BY OperationTime, ProductionSheetID ASC";
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

