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

namespace ESimSolFinancial.Controllers
{
    public class ImportPIRegisterController : Controller
    {   
        string _sDateRange = "";
        string _sErrorMesage = "";
        List<ImportPIRegister> _oImportPIRegisters = new List<ImportPIRegister>();        

        #region Actions
        public ActionResult ViewImportPIRegisters(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            ImportPIRegister oImportPIRegister = new ImportPIRegister();

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
                if ((EnumReportLayout)oItem.id == EnumReportLayout.PIWise || (EnumReportLayout)oItem.id == EnumReportLayout.DateWise || (EnumReportLayout)oItem.id == EnumReportLayout.DateWiseDetails || (EnumReportLayout)oItem.id == EnumReportLayout.PartyWise || (EnumReportLayout)oItem.id == EnumReportLayout.PartyWiseDetails || (EnumReportLayout)oItem.id == EnumReportLayout.ProductWise)
                {
                    oReportLayouts.Add(oItem);
                }
            }
            #endregion

            List<ImportProduct> oImportProducts = new List<ImportProduct>();
            oImportProducts=ImportProduct.GetByBU(buid, (int)Session[SessionInfo.currentUserID]);

            ViewBag.BUID = buid;
            ViewBag.ImportProducts = oImportProducts;
            ViewBag.ApprovalUsers = oApprovalUsers;
            ViewBag.ReportLayouts = oReportLayouts;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.ImportPIStates = EnumObject.jGets(typeof(EnumImportPIState));
            ViewBag.ImportPITypes = EnumObject.jGets(typeof(EnumImportPIType));
            return View(oImportPIRegister);
        }

        [HttpPost]
        public ActionResult SetSessionSearchCriteria(ImportPIRegister oImportPIRegister)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oImportPIRegister);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintImportPISummary(double ts)
        {
            ImportPIRegister oImportPIRegister = new ImportPIRegister();
            List<ImportPI> _oImportPIs = new List<ImportPI>(); 
            try
            {
                _sErrorMesage = "";               
                oImportPIRegister = (ImportPIRegister)Session[SessionInfo.ParamObj];
                string sSQL = this.GetSqlImportPISummary(oImportPIRegister);
                _oImportPIs = ImportPI.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (_oImportPIs.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oImportPIRegisters = new List<ImportPIRegister>();
                _sErrorMesage = ex.Message;
            }

            if (_sErrorMesage == "")
            {
                Company oCompany = new Company();
                BusinessUnit oBusinessUnit = new BusinessUnit();
                oBusinessUnit = oBusinessUnit.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBusinessUnit);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);

                rptImpotPIPreviewSummary oReport = new rptImpotPIPreviewSummary();
                byte[] abytes = oReport.PrepareReport(_oImportPIs, oCompany,_sDateRange);
                return File(abytes, "application/pdf");
            }
            else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport(_sErrorMesage);
                return File(abytes, "application/pdf");
            }
        }



        public ActionResult PrintImportPIRegister(double ts)
        {
            ImportPIRegister oImportPIRegister = new ImportPIRegister();
            try
            {
                _sErrorMesage = "";
                _oImportPIRegisters = new List<ImportPIRegister>();                
                oImportPIRegister = (ImportPIRegister)Session[SessionInfo.ParamObj];
                string sSQL = this.GetSQL(oImportPIRegister);
                _oImportPIRegisters = ImportPIRegister.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (_oImportPIRegisters.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oImportPIRegisters = new List<ImportPIRegister>();
                _sErrorMesage = ex.Message;
            }

            if (_sErrorMesage == "")
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                BusinessUnit oBU = new BusinessUnit();
                oBU = oBU.Get(_oImportPIRegisters[0].BUID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBU);

                rptImportPIRegisters oReport = new rptImportPIRegisters();
                byte[] abytes = oReport.PrepareReport(_oImportPIRegisters, oCompany, oImportPIRegister.ReportLayout, _sDateRange);
                return File(abytes, "application/pdf");
            }
            else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport(_sErrorMesage);
                return File(abytes, "application/pdf");
            }
        }

        public void ExportToExcelImportPIRegister(double ts)
        {
            ImportPIRegister oImportPIRegister = new ImportPIRegister();
            try
            {
                _sErrorMesage = "";
                _oImportPIRegisters = new List<ImportPIRegister>();
                oImportPIRegister = (ImportPIRegister)Session[SessionInfo.ParamObj];
                string sSQL = this.GetSQL(oImportPIRegister);
                _oImportPIRegisters = ImportPIRegister.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (_oImportPIRegisters.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oImportPIRegisters = new List<ImportPIRegister>();
                _sErrorMesage = ex.Message;
            }

            byte[] abytes = null;
            if (_sErrorMesage == "")
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = null;// GetCompanyLogo(oCompany);
                BusinessUnit oBU = new BusinessUnit();
                oBU = oBU.Get(_oImportPIRegisters[0].BUID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBU);

                rptImportPIRegisters oReport = new rptImportPIRegisters();
                PdfPTable oPdfPTable = oReport.PrepareExcel(_oImportPIRegisters, oCompany, oImportPIRegister.ReportLayout, _sDateRange);

                ExportToExcel.WorksheetName = "ImportPIRegister";
                abytes = ExportToExcel.ConvertToExcel(oPdfPTable);
            }
            else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                PdfPTable oPdfPTable = oReport.PrepareExcel(_sErrorMesage);
                abytes = ExportToExcel.ConvertToExcel(oPdfPTable);
            }

            Response.ClearContent();
            Response.BinaryWrite(abytes);
            Response.AddHeader("content-disposition", "attachment; filename=Excel001.xlsx");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Flush();
            Response.End();
        }

        public void ExportToExcelImportPIRegisterTwo(double ts)
        {
            ImportPIRegister oImportPIRegister = new ImportPIRegister();
            List<ImportInvoice> oImportInvoices = new List<ImportInvoice>();
            try
            {
                _sErrorMesage = "";
                _oImportPIRegisters = new List<ImportPIRegister>();
                oImportPIRegister = (ImportPIRegister)Session[SessionInfo.ParamObj];
                string sSQL = this.GetSQLForPIRegisterTwo(oImportPIRegister);
                _oImportPIRegisters = ImportPIRegister.GetsForPIRegTwo(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (_oImportPIRegisters.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
                else
                {
                    string ids = String.Join(",", _oImportPIRegisters.Select(x => x.LCID));
                    oImportInvoices = ImportInvoice.Gets("SELECT * FROM View_ImportInvoice WHERE ImportLCID IN ("+ids+")", (int)Session[SessionInfo.currentUserID]);
                }
            }
            catch (Exception ex)
            {
                _oImportPIRegisters = new List<ImportPIRegister>();
                _sErrorMesage = ex.Message;
            }

            byte[] abytes = null;
            if (_sErrorMesage == "")
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                //oCompany.CompanyLogo = null;// GetCompanyLogo(oCompany);
                BusinessUnit oBU = new BusinessUnit();
                oBU = oBU.Get(oImportPIRegister.BUID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBU);


                #region Header
                List<TableHeader> table_header = new List<TableHeader>();

                table_header.Add(new TableHeader { Header = "#SL", Width = 10f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "PI No", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "PI Date", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Commodity", Width = 30f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "M. Unit", Width = 10f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "PI Qty", Width = 13f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "PI Rate", Width = 12f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "PI Amount", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Total Amount", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Party", Width = 35f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Invoice Qty", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "GRN Qty", Width = 15f, IsRotate = false });

                table_header.Add(new TableHeader { Header = "LC No", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "LC Date", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "SC No", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "SC Date", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Advising Bank", Width = 30f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "LC Amount", Width = 15f, IsRotate = false });

                table_header.Add(new TableHeader { Header = "Invoice No", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "invoice Date", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Invoice Value", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Acceptence Date", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Maturity Date", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Payment Type", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Payment Date", Width = 15f, IsRotate = false });
                #endregion


                #region Export Excel
                int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count;
                ExcelRange cell; ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Import PI Register");
                    sheet.Name = "Import_PI_Register";

                    foreach (TableHeader listItem in table_header)
                    {
                        sheet.Column(nStartCol++).Width = listItem.Width;
                    }

                    #region Report Header
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol + 1]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    #endregion
                    nRowIndex++;
                    #region Address & Date
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol + 1]; cell.Merge = true;
                    cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    #endregion
                    nRowIndex++;
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol + 1]; cell.Merge = true;
                    cell.Value = "Import PI Register"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex += 2;
                    #region Column Header
                    nStartCol = 2;
                    foreach (TableHeader listItem in table_header)
                    {
                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = listItem.Header; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    #endregion

                    #region Data

                    nRowIndex++;

                    ExcelTool.Formatter = " #,##0.00;(#,##0.00)";
                    int nCount = 0;

                    var data = _oImportPIRegisters.GroupBy(x => new { x.LCID }, (key, grp) => new
                    {
                        LCID = key.LCID,
                        Results = grp.ToList()
                    });
                    int nSL = 0;
                    double ntotalLCAmount = 0;
                    foreach (var oData in data)
                    {
                        List<ImportInvoice> oTempImportInvoices = new List<ImportInvoice>();
                        oTempImportInvoices = oImportInvoices.Where(x => x.ImportLCID == oData.LCID).ToList();
                        //int nRowSpan = (Math.Max(oData.Results.Count(), oTempImportInvoices.Count())) - 1;
                        //if (nRowSpan < 0) nRowSpan = 0;
                        int nMaxCount = Math.Max(oData.Results.Count(), oTempImportInvoices.Count());
                        int nPIID = -999; int nRowSpan = 0;
                        int nTempRowIndex = nRowIndex; int nRowCnt = 0;

                        nStartCol = 2;
                        ExcelTool.FillCellMerge(ref sheet, (++nSL).ToString(), nRowIndex, nRowIndex + (nMaxCount-1), nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, false);

                        #region PI
                        foreach (var oItem in oData.Results)
                        {
                            nRowCnt++;
                            nStartCol = 3;
                            if (nPIID != oItem.ImportPIID)
                            {
                                nRowSpan = oData.Results.Where(x => x.ImportPIID == oItem.ImportPIID).Count() - 1;
                                if (nRowSpan < 0) nRowSpan = 0;
                                ExcelTool.FillCellMerge(ref sheet, oItem.ImportPINo, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, false);
                                ExcelTool.FillCellMerge(ref sheet, oItem.ImportPIDateSt, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, false);
                            }
                            else nStartCol = 5;

                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.ProductName, false, ExcelHorizontalAlignment.Left, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.MUnitName, false, ExcelHorizontalAlignment.Left, false, false);
                            ExcelTool.Formatter = " #,##0.00;(#,##0.00)";
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.PIQty.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.UnitPrice.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, (oItem.UnitPrice * oItem.PIQty).ToString(), true, ExcelHorizontalAlignment.Right, false, false);

                            if (nPIID != oItem.ImportPIID)
                            {
                                //nRowSpan = oData.Results.Where(x => x.ImportPIID == oItem.ImportPIID).Count() - 1;
                                //if (nRowSpan < 0) nRowSpan = 0;
                                ExcelTool.Formatter = " #,##0.00;(#,##0.00)";
                                //ExcelTool.FillCellMergeForNumber(ref sheet, oData.Results.Where(x => x.ImportPIID == oItem.ImportPIID).Sum(y => y.Amount), nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, false);
                                ExcelTool.FillCellMergeForNumber(ref sheet, oItem.PIAmount, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, false);
                                ExcelTool.FillCellMerge(ref sheet, oItem.ContractorName, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, false);
                                ExcelTool.FillCellMergeForNumber(ref sheet, oItem.InvoiceQty, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, false);
                                ExcelTool.FillCellMergeForNumber(ref sheet, oItem.GRNQty, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, false);
                            }
                            else nStartCol = 14;

                            nPIID = oItem.ImportPIID;
                            nRowIndex++;
                        }
                        if (nRowCnt < nMaxCount)
                        {
                            while (nRowCnt != nMaxCount)
                            {
                                nRowCnt++; nStartCol = 3;
                                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, " ", false, ExcelHorizontalAlignment.Left, false, false);
                                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, " ", false, ExcelHorizontalAlignment.Left, false, false);
                                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, " ", false, ExcelHorizontalAlignment.Left, false, false);
                                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, " ", false, ExcelHorizontalAlignment.Left, false, false);
                                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, " ", false, ExcelHorizontalAlignment.Left, false, false);
                                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, " ", false, ExcelHorizontalAlignment.Left, false, false);
                                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, " ", false, ExcelHorizontalAlignment.Left, false, false);
                                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, " ", false, ExcelHorizontalAlignment.Left, false, false);
                                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, " ", false, ExcelHorizontalAlignment.Left, false, false);
                                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, " ", false, ExcelHorizontalAlignment.Left, false, false);
                                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, " ", false, ExcelHorizontalAlignment.Left, false, false);
                                nRowIndex++;
                            }
                        }
                        #endregion

                        #region LC
                        nStartCol = 14;
                        ExcelTool.FillCellMerge(ref sheet, oData.Results[0].LCNo, nTempRowIndex, nTempRowIndex + (nMaxCount - 1), nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, false);
                        ExcelTool.FillCellMerge(ref sheet, oData.Results[0].LCDateSt, nTempRowIndex, nTempRowIndex + (nMaxCount - 1), nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, false);
                        ExcelTool.FillCellMerge(ref sheet, oData.Results[0].SalesContractNo, nTempRowIndex, nTempRowIndex + (nMaxCount - 1), nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, false);
                        ExcelTool.FillCellMerge(ref sheet, oData.Results[0].SalesContractDateSt, nTempRowIndex, nTempRowIndex + (nMaxCount - 1), nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, false);
                        ExcelTool.FillCellMerge(ref sheet, oData.Results[0].BankName, nTempRowIndex, nTempRowIndex + (nMaxCount - 1), nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, false);
                        ExcelTool.Formatter = " #,##0.00;(#,##0.00)";
                        ExcelTool.FillCellMergeForNumber(ref sheet, oData.Results[0].LCAmount, nTempRowIndex, nTempRowIndex + (nMaxCount - 1), nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, false);
                        ntotalLCAmount += oData.Results[0].LCAmount;
                        #endregion

                        #region invoice
                        nRowCnt = 0;
                        foreach (ImportInvoice oII in oTempImportInvoices)
                        {
                            nStartCol = 20; nRowCnt++;
                            ExcelTool.FillCellBasic(sheet, nTempRowIndex, nStartCol++, oII.ImportInvoiceNo, false, ExcelHorizontalAlignment.Left, false, false);
                            ExcelTool.FillCellBasic(sheet, nTempRowIndex, nStartCol++, oII.InvoiceDateInString, false, ExcelHorizontalAlignment.Center, false, false);
                            ExcelTool.Formatter = " #,##0.00;(#,##0.00)";
                            ExcelTool.FillCellBasic(sheet, nTempRowIndex, nStartCol++, oII.Amount.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                            ExcelTool.FillCellBasic(sheet, nTempRowIndex, nStartCol++, oII.DateofAcceptanceSt, false, ExcelHorizontalAlignment.Center, false, false);
                            ExcelTool.FillCellBasic(sheet, nTempRowIndex, nStartCol++, oII.DateofMaturityST, false, ExcelHorizontalAlignment.Center, false, false);
                            ExcelTool.FillCellBasic(sheet, nTempRowIndex, nStartCol++, oII.LCPaymentType, false, ExcelHorizontalAlignment.Left, false, false);
                            ExcelTool.FillCellBasic(sheet, nTempRowIndex, nStartCol++, oII.DateofPaymentSt, false, ExcelHorizontalAlignment.Center, false, false);
                            nTempRowIndex++;
                        }
                        if (nRowCnt < nMaxCount)
                        {
                            while (nRowCnt != nMaxCount)
                            {
                                nRowCnt++; nStartCol = 20;
                                ExcelTool.FillCellBasic(sheet, nTempRowIndex, nStartCol++, " ", false, ExcelHorizontalAlignment.Left, false, false);
                                ExcelTool.FillCellBasic(sheet, nTempRowIndex, nStartCol++, " ", false, ExcelHorizontalAlignment.Left, false, false);
                                ExcelTool.FillCellBasic(sheet, nTempRowIndex, nStartCol++, " ", false, ExcelHorizontalAlignment.Left, false, false);
                                ExcelTool.FillCellBasic(sheet, nTempRowIndex, nStartCol++, " ", false, ExcelHorizontalAlignment.Left, false, false);
                                ExcelTool.FillCellBasic(sheet, nTempRowIndex, nStartCol++, " ", false, ExcelHorizontalAlignment.Left, false, false);
                                ExcelTool.FillCellBasic(sheet, nTempRowIndex, nStartCol++, " ", false, ExcelHorizontalAlignment.Left, false, false);
                                ExcelTool.FillCellBasic(sheet, nTempRowIndex, nStartCol++, " ", false, ExcelHorizontalAlignment.Left, false, false);
                                nTempRowIndex++;
                            }
                        }
                        #endregion


                        //nRowIndex++;

                    }

                    #region Total
                    nStartCol = 2; ExcelTool.Formatter = " #,##0.00;(#,##0.00)";

                    ExcelTool.FillCellMerge(ref sheet, "Total : ", nRowIndex, nRowIndex, nStartCol, nStartCol += 4, true, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, ++nStartCol, _oImportPIRegisters.Select(x => x.PIQty).Sum().ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, ++nStartCol, " ", false, ExcelHorizontalAlignment.Left, true, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, ++nStartCol, _oImportPIRegisters.Select(x => (x.UnitPrice*x.PIQty)).Sum().ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, ++nStartCol, _oImportPIRegisters.Select(x => (x.UnitPrice * x.PIQty)).Sum().ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                    ExcelTool.FillCellMerge(ref sheet, " ", nRowIndex, nRowIndex, ++nStartCol, nStartCol += 7, true, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, ++nStartCol, ntotalLCAmount.ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                    ExcelTool.FillCellMerge(ref sheet, " ", nRowIndex, nRowIndex, ++nStartCol, nStartCol += 1, true, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, ++nStartCol, oImportInvoices.Select(x => x.Amount).Sum().ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                    ExcelTool.FillCellMerge(ref sheet, " ", nRowIndex, nRowIndex, ++nStartCol, nStartCol += 3, true, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, false);
                    //nRowIndex++;
                    #endregion

                    cell = sheet.Cells[1, 1, nRowIndex, table_header.Count + 2];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    #endregion

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=Import_PI_Register.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion





            }
            else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                ExportToExcel.WorksheetName = "ImportPIRegister";
                PdfPTable oPdfPTable = oReport.PrepareExcel(_sErrorMesage);
                abytes = ExportToExcel.ConvertToExcel(oPdfPTable);
                Response.ClearContent();
                Response.BinaryWrite(abytes);
                Response.AddHeader("content-disposition", "attachment; filename=Excel001.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }

        }
        #endregion

        #region Support Functions
        private string GetSQL(ImportPIRegister oImportPIRegister)
        {
            _sDateRange = "";
            string sSearchingData = oImportPIRegister.SearchingData;
            EnumCompareOperator ePIIssueDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[0]);
            DateTime dPIIssueStartDate = Convert.ToDateTime(sSearchingData.Split('~')[1]);
            DateTime dPIIssueEndDate = Convert.ToDateTime(sSearchingData.Split('~')[2]);

            EnumCompareOperator ePIValidityDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[3]);
            DateTime dPIValidityStartDate = Convert.ToDateTime(sSearchingData.Split('~')[4]);
            DateTime dPIValidityEndDate = Convert.ToDateTime(sSearchingData.Split('~')[5]);

            EnumCompareOperator ePIApprovedDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[6]);
            DateTime dPIApprovedStartDate = Convert.ToDateTime(sSearchingData.Split('~')[7]);
            DateTime dPIApprovedEndDate = Convert.ToDateTime(sSearchingData.Split('~')[8]);

            EnumCompareOperator ePIAmount = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[9]);
            double nPIAmountStsrt = Convert.ToDouble(sSearchingData.Split('~')[10]);
            double nPIAmountEnd = Convert.ToDouble(sSearchingData.Split('~')[11]);

            string sImportLCNo = sSearchingData.Split('~')[12];
            string sImportInvoiceNo = sSearchingData.Split('~')[13];

            EnumCompareOperator ePostingDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[14]);
            DateTime dPostingStartDate = Convert.ToDateTime(sSearchingData.Split('~')[15]);
            DateTime dPostingEndDate = Convert.ToDateTime(sSearchingData.Split('~')[16]);

            bool bIsWthoutCancel = Convert.ToBoolean(sSearchingData.Split('~')[17]);

            string sSQLQuery = "", sWhereCluse = "", sGroupBy = "", sOrderBy = "";

            #region POSTING DATE (DBServerDateTime)
            DateObject.CompareDateQuery(ref sWhereCluse, "DBServerDateTime", (int)ePostingDate, dPostingStartDate, dPostingEndDate);
            #endregion
          
            #region BusinessUnit
            if (oImportPIRegister.BUID > 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " BUID =" + oImportPIRegister.BUID.ToString();
            }
            #endregion

            #region ImportPINo
            if (oImportPIRegister.ImportPINo != null && oImportPIRegister.ImportPINo != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " ImportPINo LIKE'%" + oImportPIRegister.ImportPINo + "%'";
            }
            #endregion

            #region ApprovedBy
            if (oImportPIRegister.ApprovedBy != 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " ApprovedBy =" + oImportPIRegister.ApprovedBy.ToString();
            }
            #endregion

            #region ImportPIStatus
            if (oImportPIRegister.ImportPIStatus != EnumImportPIState.Initialized)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " ImportPIStatus =" + ((int)oImportPIRegister.ImportPIStatus).ToString();
            }
            #endregion

            #region Wthout Cancel
            if (bIsWthoutCancel == true)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " ImportPIStatus != " + (int)EnumImportPIState.Cancel;
            }
            #endregion

            #region ImportPIType
            if (oImportPIRegister.ImportPIType != EnumImportPIType.None)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " ImportPIType =" + ((int)oImportPIRegister.ImportPIType).ToString();
            }
            #endregion
            #region ProductType
            if (oImportPIRegister.ProductType != EnumProductNature.Dyeing)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " ProductType =" + ((int)oImportPIRegister.ProductType).ToString();
            }
            #endregion

            #region Remarks
            if (oImportPIRegister.Remarks != null && oImportPIRegister.Remarks != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " Remarks LIKE'%" + oImportPIRegister.Remarks + "%'";
            }
            #endregion

            #region Supplier
            if (oImportPIRegister.SupplierName != null && oImportPIRegister.SupplierName != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " SupplierID IN(" + oImportPIRegister.SupplierName + ")";
            }
            #endregion

            #region Product
            if (oImportPIRegister.ProductName != null && oImportPIRegister.ProductName != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " ProductID IN(" + oImportPIRegister.ProductName + ")";
            }
            #endregion

            #region Bank Branch
            if (oImportPIRegister.BranchName != null && oImportPIRegister.BranchName != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " AdviseBBID IN(" + oImportPIRegister.BranchName + ")";
            }
            #endregion

            #region PI Issue Date
            if (ePIIssueDate != EnumCompareOperator.None)
            {
                if (ePIIssueDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIIssueStartDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "PI Date @ " + dPIIssueStartDate.ToString("dd MMM yyyy");
                }
                else if (ePIIssueDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIIssueStartDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "PI Date Not Equal @ " + dPIIssueStartDate.ToString("dd MMM yyyy");
                }
                else if (ePIIssueDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIIssueStartDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "PI Date Greater Then @ " + dPIIssueStartDate.ToString("dd MMM yyyy");
                }
                else if (ePIIssueDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIIssueStartDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "PI Date Smaller Then @ " + dPIIssueStartDate.ToString("dd MMM yyyy");
                }
                else if (ePIIssueDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIIssueStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIIssueEndDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "PI Date Between " + dPIIssueStartDate.ToString("dd MMM yyyy") + " To " + dPIIssueEndDate.ToString("dd MMM yyyy");
                }
                else if (ePIIssueDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIIssueStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIIssueEndDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "PI Date NOT Between " + dPIIssueStartDate.ToString("dd MMM yyyy") + " To " + dPIIssueEndDate.ToString("dd MMM yyyy");
                }
            }
            #endregion

            #region PIValidity Date
            if (ePIValidityDate != EnumCompareOperator.None)
            {
                if (ePIValidityDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ValidityDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIValidityStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (ePIValidityDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ValidityDate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIValidityStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (ePIValidityDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ValidityDate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIValidityStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (ePIValidityDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ValidityDate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIValidityStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (ePIValidityDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ValidityDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIValidityStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIValidityEndDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (ePIValidityDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ValidityDate,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIValidityStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIValidityEndDate.ToString("dd MMM yyyy") + "', 106))";
                }
            }
            #endregion

            #region PIApproved Date
            if (ePIApprovedDate != EnumCompareOperator.None)
            {
                if (ePIApprovedDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),DateOfApproved,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIApprovedStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (ePIApprovedDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),DateOfApproved,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIApprovedStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (ePIApprovedDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),DateOfApproved,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIApprovedStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (ePIApprovedDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),DateOfApproved,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIApprovedStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (ePIApprovedDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),DateOfApproved,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIApprovedStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIApprovedEndDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (ePIApprovedDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),DateOfApproved,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIApprovedStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIApprovedEndDate.ToString("dd MMM yyyy") + "', 106))";
                }
            }
            #endregion

            #region PIAmount
            if (ePIAmount != EnumCompareOperator.None)
            {
                if (ePIAmount == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " TotalValue = " + nPIAmountStsrt.ToString("0.00");
                }
                else if (ePIAmount == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " TotalValue != " + nPIAmountStsrt.ToString("0.00"); ;
                }
                else if (ePIAmount == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " TotalValue > " + nPIAmountStsrt.ToString("0.00"); ;
                }
                else if (ePIAmount == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " TotalValue < " + nPIAmountStsrt.ToString("0.00"); ;
                }
                else if (ePIAmount == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " TotalValue BETWEEN " + nPIAmountStsrt.ToString("0.00") + " AND " + nPIAmountEnd.ToString("0.00");
                }
                else if (ePIAmount == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " TotalValue NOT BETWEEN " + nPIAmountStsrt.ToString("0.00") + " AND " + nPIAmountEnd.ToString("0.00");
                }
            }
            #endregion

            #region ImportLCNo
            if (sImportLCNo != null && sImportLCNo != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " ImportPIID IN (SELECT MM.ImportPIID FROM ImportLCDetail AS MM WHERE MM.ImportLCID IN (SELECT NN.ImportLCID FROM ImportLC AS NN WHERE NN.ImportLCNo LIKE '%" + sImportLCNo + "%'))";
            }
            #endregion

            #region ImportInvoiceNo
            if (sImportInvoiceNo != null && sImportInvoiceNo != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " ImportPIID IN (SELECT MM.ImportPIID FROM ImportLCDetail AS MM WHERE MM.ImportLCID IN (SELECT NN.ImportLCID FROM ImportInvoice AS NN WHERE NN.ImportInvoiceNo LIKE '%" + sImportInvoiceNo + "%'))";
            }
            #endregion

            #region Report Layout
            if (oImportPIRegister.ReportLayout == EnumReportLayout.PIWise)
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT * FROM View_ImportPIRegister ";
                sOrderBy = " ORDER BY  IssueDate, ImportPIID, ImportPIDetailID ASC";
            }
            else if (oImportPIRegister.ReportLayout == EnumReportLayout.DateWise)
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT ImportPIID, BUID, ImportPINo, IssueDate, ImportPIStatus, SupplierName, SCPName, LCTermName, PaymentInstructionType, ApprovedByName, DateOfApproved, ValidityDate, Remarks, ImportLCNo, TotalValue   FROM View_ImportPIRegister ";
                sGroupBy = "GROUP BY ImportPIID, BUID, ImportPINo, IssueDate, ImportPIStatus, SupplierName, SCPName, LCTermName, PaymentInstructionType, ApprovedByName, DateOfApproved, ValidityDate, Remarks, ImportLCNo, TotalValue";
                sOrderBy = " ORDER BY IssueDate ASC";
            }
            else if (oImportPIRegister.ReportLayout == EnumReportLayout.DateWiseDetails)
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT * FROM View_ImportPIRegister ";
                sOrderBy = " ORDER BY  IssueDate, ImportPIID, ImportPIDetailID ASC";
            }
            else if (oImportPIRegister.ReportLayout == EnumReportLayout.PartyWise)
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT ImportPIID, BUID, ImportPINo, IssueDate, ImportPIStatus, SupplierName, SCPName, LCTermName, PaymentInstructionType, ApprovedByName, DateOfApproved, ValidityDate, Remarks, ImportLCNo, TotalValue   FROM View_ImportPIRegister ";
                sGroupBy = "GROUP BY ImportPIID, BUID, ImportPINo, IssueDate, ImportPIStatus, SupplierName, SCPName, LCTermName, PaymentInstructionType, ApprovedByName, DateOfApproved, ValidityDate, Remarks, ImportLCNo, TotalValue";
                sOrderBy = " ORDER BY SupplierName ASC";
            }
            else if (oImportPIRegister.ReportLayout == EnumReportLayout.PartyWiseDetails)
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT * FROM View_ImportPIRegister ";
                sOrderBy = " ORDER BY  SupplierName, ImportPIID, ImportPIDetailID ASC";
            }
            else if (oImportPIRegister.ReportLayout == EnumReportLayout.ProductWise)
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT * FROM View_ImportPIRegister ";
                sOrderBy = " ORDER BY  ProductName ASC";
            }
            else
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT * FROM View_ImportPIRegister ";
                sOrderBy = " ORDER BY IssueDate, ImportPIID, ImportPIDetailID ASC";
            }
            #endregion

            sSQLQuery = sSQLQuery + sWhereCluse + sGroupBy + sOrderBy;
            return sSQLQuery;
        }

        public string GetSqlImportPISummary(ImportPIRegister oImportPIRegister)
        {
            string sReturn1 = "SELECT HH.SupplierNameCode,HH.SupplierID, HH.SupplierName,HH.CurrencySymbol,SUM(HH.TotalValue)AS TotalValue,ISNULL(HH.ProductType,0) AS ProductType,Count(DISTINCT(HH.ImportPIID)) AS Count FROM View_ImportPI AS HH ";
            string sReturn = "";

            _sDateRange = "";
            string sSearchingData = oImportPIRegister.SearchingData;
            EnumCompareOperator ePIIssueDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[0]);
            DateTime dPIIssueStartDate = Convert.ToDateTime(sSearchingData.Split('~')[1]);
            DateTime dPIIssueEndDate = Convert.ToDateTime(sSearchingData.Split('~')[2]);

            EnumCompareOperator ePIValidityDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[3]);
            DateTime dPIValidityStartDate = Convert.ToDateTime(sSearchingData.Split('~')[4]);
            DateTime dPIValidityEndDate = Convert.ToDateTime(sSearchingData.Split('~')[5]);

            EnumCompareOperator ePIApprovedDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[6]);
            DateTime dPIApprovedStartDate = Convert.ToDateTime(sSearchingData.Split('~')[7]);
            DateTime dPIApprovedEndDate = Convert.ToDateTime(sSearchingData.Split('~')[8]);

            EnumCompareOperator ePIAmount = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[9]);
            double nPIAmountStsrt = Convert.ToDouble(sSearchingData.Split('~')[10]);
            double nPIAmountEnd = Convert.ToDouble(sSearchingData.Split('~')[11]);

            string sImportLCNo = sSearchingData.Split('~')[12];
            string sImportInvoiceNo = sSearchingData.Split('~')[13];

            EnumCompareOperator ePostingDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[14]);
            DateTime dPostingStartDate = Convert.ToDateTime(sSearchingData.Split('~')[15]);
            DateTime dPostingEndDate = Convert.ToDateTime(sSearchingData.Split('~')[16]);

            bool bIsWthoutCancel = Convert.ToBoolean(sSearchingData.Split('~')[17]);

            string sSQLQuery = "", sWhereCluse = "", sGroupBy = "", sOrderBy = "";

            #region ImportPINo
            if (oImportPIRegister.ImportPINo != null && oImportPIRegister.ImportPINo != "")
            {
                Global.TagSQL(ref sReturn);
                sWhereCluse = sWhereCluse + " HH.ImportPINo LIKE'%" + oImportPIRegister.ImportPINo + "%'";
            }
            #endregion
            #region ImportLCNo 
            if (sImportLCNo != null && sImportLCNo != "")
            {
                Global.TagSQL(ref sReturn);
                sWhereCluse = sWhereCluse + "  HH.ImportLCNo LIKE %" + sImportLCNo + "% ";
            }
            #endregion
            #region ImportPIType
            if (oImportPIRegister.ImportPIType != EnumImportPIType.None)
            {
                Global.TagSQL(ref sReturn);
                sWhereCluse = sWhereCluse + "  HH.ImportPIType =" + ((int)oImportPIRegister.ImportPIType).ToString();
            }
            #endregion
            #region ProductType
            if (oImportPIRegister.ProductType != EnumProductNature.Dyeing)
            {
                Global.TagSQL(ref sReturn);
                sWhereCluse = sWhereCluse + " HH.ProductType =" + ((int)oImportPIRegister.ProductType).ToString();
            }
            #endregion
            #region PIIssueDate
            if (ePIIssueDate != EnumCompareOperator.None)
            {
                if (ePIIssueDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(DATE,CONVERT(VARCHAR(12),HH.IssueDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dPIIssueStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (ePIIssueDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(DATE,CONVERT(VARCHAR(12),HH.IssueDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dPIIssueStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (ePIIssueDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(DATE,CONVERT(VARCHAR(12),HH.IssueDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dPIIssueStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (ePIIssueDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(DATE,CONVERT(VARCHAR(12),HH.IssueDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dPIIssueStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (ePIIssueDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(DATE,CONVERT(VARCHAR(12),HH.IssueDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dPIIssueStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dPIIssueEndDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (ePIIssueDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(DATE,CONVERT(VARCHAR(12),HH.IssueDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dPIIssueStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dPIIssueEndDate.ToString("dd MMM yyyy") + "',106))";
                }
            }
            #endregion
            #region ePIValidityDate
            if (ePIValidityDate != EnumCompareOperator.None)
            {
                if (ePIValidityDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(DATE,CONVERT(VARCHAR(12),HH.ValidityDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dPIValidityStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (ePIValidityDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(DATE,CONVERT(VARCHAR(12),HH.ValidityDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dPIValidityStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (ePIValidityDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(DATE,CONVERT(VARCHAR(12),HH.ValidityDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dPIValidityStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (ePIValidityDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(DATE,CONVERT(VARCHAR(12),HH.ValidityDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dPIValidityStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (ePIValidityDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(DATE,CONVERT(VARCHAR(12),HH.ValidityDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dPIValidityStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dPIValidityEndDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (ePIValidityDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(DATE,CONVERT(VARCHAR(12),HH.ValidityDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dPIValidityStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dPIValidityEndDate.ToString("dd MMM yyyy") + "',106))";
                }
            }
            #endregion
            #region ePIApprovedDate
            if (ePIApprovedDate != EnumCompareOperator.None)
            {
                if (ePIApprovedDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(DATE,CONVERT(VARCHAR(12),HH.DateOfApproved,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dPIApprovedStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (ePIApprovedDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(DATE,CONVERT(VARCHAR(12),HH.DateOfApproved,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dPIApprovedStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (ePIApprovedDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(DATE,CONVERT(VARCHAR(12),HH.DateOfApproved,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dPIApprovedStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (ePIApprovedDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(DATE,CONVERT(VARCHAR(12),HH.DateOfApproved,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dPIApprovedStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (ePIApprovedDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(DATE,CONVERT(VARCHAR(12),HH.DateOfApproved,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dPIApprovedStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dPIApprovedEndDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (ePIApprovedDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(DATE,CONVERT(VARCHAR(12),HH.DateOfApproved,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dPIApprovedStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dPIApprovedEndDate.ToString("dd MMM yyyy") + "',106))";
                }
            }
            #endregion
            #region PIAmount
            if (ePIAmount != EnumCompareOperator.None)
            {
                if (ePIAmount == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " TotalValue = " + nPIAmountStsrt.ToString("0.00");
                }
                else if (ePIAmount == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " TotalValue != " + nPIAmountStsrt.ToString("0.00"); 
                }
                else if (ePIAmount == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " TotalValue > " + nPIAmountStsrt.ToString("0.00"); ;
                }
                else if (ePIAmount == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " TotalValue < " + nPIAmountStsrt.ToString("0.00"); ;
                }
                else if (ePIAmount == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " TotalValue BETWEEN " + nPIAmountStsrt.ToString("0.00") + " AND " + nPIAmountEnd.ToString("0.00");
                }
                else if (ePIAmount == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " TotalValue NOT BETWEEN " + nPIAmountStsrt.ToString("0.00") + " AND " + nPIAmountEnd.ToString("0.00");
                }
            }
            #endregion
            #region eReceive Date
            if (ePostingDate != EnumCompareOperator.None)
            {
                if (ePostingDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(DATE,CONVERT(VARCHAR(12),HH.ReceiveDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dPostingStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (ePostingDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(DATE,CONVERT(VARCHAR(12),HH.ReceiveDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dPostingStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (ePostingDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(DATE,CONVERT(VARCHAR(12),HH.ReceiveDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dPostingStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (ePostingDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(DATE,CONVERT(VARCHAR(12),HH.ReceiveDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dPostingStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (ePostingDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(DATE,CONVERT(VARCHAR(12),HH.ReceiveDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dPostingStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dPostingEndDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (ePostingDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(DATE,CONVERT(VARCHAR(12),HH.ReceiveDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dPostingStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dPostingEndDate.ToString("dd MMM yyyy") + "',106))";
                }
            }

            #endregion
            #region Supplier
            if (oImportPIRegister.SupplierName != null && oImportPIRegister.SupplierName != "")
            {
                Global.TagSQL(ref sReturn);
                sWhereCluse = sWhereCluse + " HH.SupplierID IN(" + oImportPIRegister.SupplierName + ")";
            }
            #endregion           
            #region ImportPIStatus
            if (oImportPIRegister.ImportPIStatus != EnumImportPIState.Initialized)
            {
                Global.TagSQL(ref sReturn);
                sWhereCluse = sWhereCluse + " ImportPIStatus =" + ((int)oImportPIRegister.ImportPIStatus).ToString();
            }
            #endregion

            #region ImportInvoiceNo
            if (sImportInvoiceNo != null && sImportInvoiceNo != "")
            {
                Global.TagSQL(ref sReturn);
                sWhereCluse = sWhereCluse + "  HH.ImportPIType LIKE %" + sImportInvoiceNo.ToString()+"% ";
            }
            #endregion

            sReturn = sReturn1 + sReturn + " Group BY CurrencySymbol,ISNULL(HH.ProductType,0),SupplierName,SupplierID,SupplierNameCode ORDER BY SupplierName ";
            return sReturn;

        }

        private string GetSQLForPIRegisterTwo(ImportPIRegister oImportPIRegister)
        {
            _sDateRange = "";
            string sSearchingData = oImportPIRegister.SearchingData;
            EnumCompareOperator ePIIssueDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[0]);
            DateTime dPIIssueStartDate = Convert.ToDateTime(sSearchingData.Split('~')[1]);
            DateTime dPIIssueEndDate = Convert.ToDateTime(sSearchingData.Split('~')[2]);

            EnumCompareOperator ePIValidityDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[3]);
            DateTime dPIValidityStartDate = Convert.ToDateTime(sSearchingData.Split('~')[4]);
            DateTime dPIValidityEndDate = Convert.ToDateTime(sSearchingData.Split('~')[5]);

            EnumCompareOperator ePIApprovedDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[6]);
            DateTime dPIApprovedStartDate = Convert.ToDateTime(sSearchingData.Split('~')[7]);
            DateTime dPIApprovedEndDate = Convert.ToDateTime(sSearchingData.Split('~')[8]);

            EnumCompareOperator ePIAmount = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[9]);
            double nPIAmountStsrt = Convert.ToDouble(sSearchingData.Split('~')[10]);
            double nPIAmountEnd = Convert.ToDouble(sSearchingData.Split('~')[11]);

            string sImportLCNo = sSearchingData.Split('~')[12];
            string sImportInvoiceNo = sSearchingData.Split('~')[13];

            EnumCompareOperator ePostingDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[14]);
            DateTime dPostingStartDate = Convert.ToDateTime(sSearchingData.Split('~')[15]);
            DateTime dPostingEndDate = Convert.ToDateTime(sSearchingData.Split('~')[16]);

            bool bIsWthoutCancel = Convert.ToBoolean(sSearchingData.Split('~')[17]);

            string sSQLQuery = "", sWhereCluse = "", sGroupBy = "", sOrderBy = "";

            #region POSTING DATE (DBServerDateTime)
            DateObject.CompareDateQuery(ref sWhereCluse, "DBServerDateTime", (int)ePostingDate, dPostingStartDate, dPostingEndDate);
            #endregion

            #region BusinessUnit
            if (oImportPIRegister.BUID > 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " BUID =" + oImportPIRegister.BUID.ToString();
            }
            #endregion

            #region ImportPINo
            if (oImportPIRegister.ImportPINo != null && oImportPIRegister.ImportPINo != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " ImportPINo LIKE'%" + oImportPIRegister.ImportPINo + "%'";
            }
            #endregion

            #region ApprovedBy
            if (oImportPIRegister.ApprovedBy != 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " ApprovedBy =" + oImportPIRegister.ApprovedBy.ToString();
            }
            #endregion

            #region ImportPIStatus
            if (oImportPIRegister.ImportPIStatus != EnumImportPIState.Initialized)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " ImportPIStatus =" + ((int)oImportPIRegister.ImportPIStatus).ToString();
            }
            #endregion

            #region Wthout Cancel
            if (bIsWthoutCancel == true)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " ImportPIStatus != " + (int)EnumImportPIState.Cancel;
            }
            #endregion

            #region ImportPIType
            if (oImportPIRegister.ImportPIType != EnumImportPIType.None)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " ImportPIType =" + ((int)oImportPIRegister.ImportPIType).ToString();
            }
            #endregion
            #region ProductType
            if (oImportPIRegister.ProductType != EnumProductNature.Dyeing)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " ProductType =" + ((int)oImportPIRegister.ProductType).ToString();
            }
            #endregion

            #region Remarks
            if (oImportPIRegister.Remarks != null && oImportPIRegister.Remarks != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " Remarks LIKE'%" + oImportPIRegister.Remarks + "%'";
            }
            #endregion

            #region Supplier
            if (oImportPIRegister.SupplierName != null && oImportPIRegister.SupplierName != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " SupplierID IN(" + oImportPIRegister.SupplierName + ")";
            }
            #endregion

            #region Product
            if (oImportPIRegister.ProductName != null && oImportPIRegister.ProductName != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " ProductID IN(" + oImportPIRegister.ProductName + ")";
            }
            #endregion

            #region Bank Branch
            if (oImportPIRegister.BranchName != null && oImportPIRegister.BranchName != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " AdviseBBID IN(" + oImportPIRegister.BranchName + ")";
            }
            #endregion

            #region PI Issue Date
            if (ePIIssueDate != EnumCompareOperator.None)
            {
                if (ePIIssueDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIIssueStartDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "PI Date @ " + dPIIssueStartDate.ToString("dd MMM yyyy");
                }
                else if (ePIIssueDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIIssueStartDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "PI Date Not Equal @ " + dPIIssueStartDate.ToString("dd MMM yyyy");
                }
                else if (ePIIssueDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIIssueStartDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "PI Date Greater Then @ " + dPIIssueStartDate.ToString("dd MMM yyyy");
                }
                else if (ePIIssueDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIIssueStartDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "PI Date Smaller Then @ " + dPIIssueStartDate.ToString("dd MMM yyyy");
                }
                else if (ePIIssueDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIIssueStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIIssueEndDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "PI Date Between " + dPIIssueStartDate.ToString("dd MMM yyyy") + " To " + dPIIssueEndDate.ToString("dd MMM yyyy");
                }
                else if (ePIIssueDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIIssueStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIIssueEndDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "PI Date NOT Between " + dPIIssueStartDate.ToString("dd MMM yyyy") + " To " + dPIIssueEndDate.ToString("dd MMM yyyy");
                }
            }
            #endregion

            #region PIValidity Date
            if (ePIValidityDate != EnumCompareOperator.None)
            {
                if (ePIValidityDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ValidityDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIValidityStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (ePIValidityDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ValidityDate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIValidityStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (ePIValidityDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ValidityDate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIValidityStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (ePIValidityDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ValidityDate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIValidityStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (ePIValidityDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ValidityDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIValidityStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIValidityEndDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (ePIValidityDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ValidityDate,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIValidityStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIValidityEndDate.ToString("dd MMM yyyy") + "', 106))";
                }
            }
            #endregion

            #region PIApproved Date
            if (ePIApprovedDate != EnumCompareOperator.None)
            {
                if (ePIApprovedDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),DateOfApproved,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIApprovedStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (ePIApprovedDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),DateOfApproved,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIApprovedStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (ePIApprovedDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),DateOfApproved,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIApprovedStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (ePIApprovedDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),DateOfApproved,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIApprovedStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (ePIApprovedDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),DateOfApproved,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIApprovedStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIApprovedEndDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (ePIApprovedDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),DateOfApproved,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIApprovedStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPIApprovedEndDate.ToString("dd MMM yyyy") + "', 106))";
                }
            }
            #endregion

            #region PIAmount
            if (ePIAmount != EnumCompareOperator.None)
            {
                if (ePIAmount == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " TotalValue = " + nPIAmountStsrt.ToString("0.00");
                }
                else if (ePIAmount == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " TotalValue != " + nPIAmountStsrt.ToString("0.00"); ;
                }
                else if (ePIAmount == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " TotalValue > " + nPIAmountStsrt.ToString("0.00"); ;
                }
                else if (ePIAmount == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " TotalValue < " + nPIAmountStsrt.ToString("0.00"); ;
                }
                else if (ePIAmount == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " TotalValue BETWEEN " + nPIAmountStsrt.ToString("0.00") + " AND " + nPIAmountEnd.ToString("0.00");
                }
                else if (ePIAmount == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " TotalValue NOT BETWEEN " + nPIAmountStsrt.ToString("0.00") + " AND " + nPIAmountEnd.ToString("0.00");
                }
            }
            #endregion

            #region ImportLCNo
            if (sImportLCNo != null && sImportLCNo != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " ImportPIID IN (SELECT MM.ImportPIID FROM ImportLCDetail AS MM WHERE MM.ImportLCID IN (SELECT NN.ImportLCID FROM ImportLC AS NN WHERE NN.ImportLCNo LIKE '%" + sImportLCNo + "%'))";
            }
            #endregion

            #region ImportInvoiceNo
            if (sImportInvoiceNo != null && sImportInvoiceNo != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " ImportPIID IN (SELECT MM.ImportPIID FROM ImportLCDetail AS MM WHERE MM.ImportLCID IN (SELECT NN.ImportLCID FROM ImportInvoice AS NN WHERE NN.ImportInvoiceNo LIKE '%" + sImportInvoiceNo + "%'))";
            }
            #endregion



            sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
            sSQLQuery = "SELECT ImportPIDetailID FROM View_ImportPIRegister ";
            sOrderBy = " ";

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

