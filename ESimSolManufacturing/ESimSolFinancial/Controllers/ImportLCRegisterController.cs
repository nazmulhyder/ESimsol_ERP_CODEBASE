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
    public class ImportLCRegisterController : Controller
    {   
        string _sDateRange = "";
        string _sErrorMesage = "";
        string sLCType = "";
        List<ImportLCRegister> _oImportLCRegisters = new List<ImportLCRegister>();        

        #region Actions
        public ActionResult ViewImportLCRegisters(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            ImportLCRegister oImportLCRegister = new ImportLCRegister();

            #region Approval User
            string sSQL = "SELECT * FROM View_User AS HH WHERE HH.UserID IN (SELECT DISTINCT MM.ReceivedBy FROM ImportLC AS MM WHERE ISNULL(MM.ReceivedBy,0)!=0) ORDER BY HH.UserName";
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
                if ((EnumReportLayout)oItem.id == EnumReportLayout.LCWise || (EnumReportLayout)oItem.id == EnumReportLayout.LCWithAmendment || (EnumReportLayout)oItem.id == EnumReportLayout.DateWise || (EnumReportLayout)oItem.id == EnumReportLayout.DateWiseDetails || (EnumReportLayout)oItem.id == EnumReportLayout.PartyWise || (EnumReportLayout)oItem.id == EnumReportLayout.PartyWiseDetails || (EnumReportLayout)oItem.id == EnumReportLayout.ProductWise||(EnumReportLayout)oItem.id==EnumReportLayout.Export_LC_Details)
                {
                    oReportLayouts.Add(oItem);
                }
            }
            #endregion
            ViewBag.ReportLayouts = oReportLayouts;
            ViewBag.BUID = buid;
            ViewBag.ApprovalUsers = oApprovalUsers;

            ViewBag.ImportPITypeObj = EnumObject.jGets(typeof(EnumImportPIType)).Where(x => x.id == (int)EnumImportPIType.Foreign || x.id == (int)EnumImportPIType.Local || x.id == (int)EnumImportPIType.NonLC || x.id == (int)EnumImportPIType.TTForeign || x.id == (int)EnumImportPIType.TTLocal);
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.ImportLCStates = EnumObject.jGets(typeof(EnumLCCurrentStatus));
            ViewBag.ImportLCPaymentTypes = EnumObject.jGets(typeof(EnumLCPaymentType));
            ViewBag.LCTypes = EnumObject.jGets(typeof(EnumExportLCType));
            return View(oImportLCRegister);
        }
        #region GetImportProductProduct
        [HttpPost]
        public JsonResult GetByImportProduct(ImportProduct oImportProduct)
        {
            List<ImportProduct> oImportProducts = new List<ImportProduct>();
            try
            {
                oImportProducts = ImportProduct.GetByBU(oImportProduct.BUID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oImportProducts = new List<ImportProduct>();
                oImportProduct = new ImportProduct();
                oImportProduct.ErrorMessage = ex.Message;
                oImportProducts.Add(oImportProduct);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oImportProducts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
            
        }
        #endregion
        [HttpPost]
        public ActionResult SetSessionSearchCriteria(ImportLCRegister oImportLCRegister)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oImportLCRegister);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintImportLCRegister(double ts)
        {
            ImportLCRegister oImportLCRegister = new ImportLCRegister();
            try
            {
          
                _sErrorMesage = "";
                _oImportLCRegisters = new List<ImportLCRegister>();                
                oImportLCRegister = (ImportLCRegister)Session[SessionInfo.ParamObj];
                string sSQL = this.GetSQL(oImportLCRegister);
                _oImportLCRegisters = ImportLCRegister.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (_oImportLCRegisters.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oImportLCRegisters = new List<ImportLCRegister>();
                _sErrorMesage = ex.Message;
            }

            if (_sErrorMesage == "")
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                BusinessUnit oBU = new BusinessUnit();
                oBU = oBU.Get(_oImportLCRegisters[0].BUID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBU);

                rptImportLCRegisters oReport = new rptImportLCRegisters();
                byte[] abytes = oReport.PrepareReport(_oImportLCRegisters, oCompany, oImportLCRegister.ReportLayout, _sDateRange, sLCType);
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
        #region Print_ImportLCRegisterXL
        public void Print_ImportLCRegisterXL(double ts)
        {
            ImportLCRegister oImportLCRegister = new ImportLCRegister();
            try
            {
               
                _sErrorMesage = "";
                _oImportLCRegisters = new List<ImportLCRegister>();
                oImportLCRegister = (ImportLCRegister)Session[SessionInfo.ParamObj];
                string sSQL = this.GetSQL(oImportLCRegister);
                _oImportLCRegisters = ImportLCRegister.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (_oImportLCRegisters.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oImportLCRegisters = new List<ImportLCRegister>();
                _sErrorMesage = ex.Message;
            }

            if (_sErrorMesage == "")
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                BusinessUnit oBU = new BusinessUnit();
                oBU = oBU.Get(_oImportLCRegisters[0].BUID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBU);
                if (oImportLCRegister.ReportLayout == EnumReportLayout.LCWithAmendment)
                {
                    #region Print LC with amendment
                    int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 9, nColumn = 1, nCount = 0, nImportLCID = 0;
                    ExcelRange cell;
                    ExcelRange HeaderCell;
                    ExcelFill fill;
                    OfficeOpenXml.Style.Border border;

                    using (var excelPackage = new ExcelPackage())
                    {
                        excelPackage.Workbook.Properties.Author = "ESimSol";
                        excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                        var sheet = excelPackage.Workbook.Worksheets.Add("Import LC Register");
                        sheet.Name = "Import LC Register";
                        sheet.Column(++nColumn).Width = 10; //SL No
                        sheet.Column(++nColumn).Width = 15; //LC No
                        sheet.Column(++nColumn).Width = 15; //LC Date
                        sheet.Column(++nColumn).Width = 17; //amendment date
                        sheet.Column(++nColumn).Width = 30; //Supplier
                        sheet.Column(++nColumn).Width = 20; //PI no
                        sheet.Column(++nColumn).Width = 20; //Lc amount
                        sheet.Column(++nColumn).Width = 25; //amendment amount


                        #region Report Header
                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol-4]; cell.Merge = true;
                        cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        cell = sheet.Cells[nRowIndex, nStartCol + 4, nRowIndex, nEndCol]; cell.Merge = true;
                        cell.Value = "ImportLC Register"; cell.Style.Font.Bold = true;
                        cell.Style.WrapText = true;
                        cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        nRowIndex = nRowIndex + 1;

                        if (sLCType!="")
                        {
                            cell = sheet.Cells[nRowIndex, nStartCol + 4, nRowIndex, nEndCol]; cell.Merge = true;
                            cell.Value = "L/C Type: " + sLCType; cell.Style.Font.Bold = true;
                            cell.Style.WrapText = true;
                            cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            nRowIndex = nRowIndex + 1;

                        }
                       

                        #endregion

                        #region Address & Date
                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 4]; cell.Merge = true;
                        cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        cell = sheet.Cells[nRowIndex, nStartCol + 4, nRowIndex, nEndCol]; cell.Merge = true;
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

                        #region Data
                         double nTotalAmendmendAmount = 0, nGrandTotalAmount = 0; DateTime dIssueDate = DateTime.MinValue; bool bIsSubtotalPrint = false;
                        foreach (ImportLCRegister oItem in _oImportLCRegisters)
                        {
                            nColumn = 1;
                            if (dIssueDate.Month != oItem.AmendmentDate.Month)
                            {
                                #region Sub total print
                                if (bIsSubtotalPrint)
                                {
                                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 1]; cell.Value = "Sub Total :"; cell.Style.Font.Bold = true;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Merge = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                                    cell = sheet.Cells[nRowIndex, nEndCol]; cell.Value = nTotalAmendmendAmount; cell.Style.Font.Bold = true;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                    cell.Style.Numberformat.Format = "$ #,##0.00";
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    nRowIndex++;

                                    nCount = 0; nTotalAmendmendAmount = 0;
                                }
                                #endregion

                                #region  DAte print
                                nRowIndex++;//extend row
                                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Value = "Month @ " + oItem.AmendmentDate.ToString("MMM yyyy"); cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Merge = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                nRowIndex++;
                                #endregion

                                #region header print
                                nColumn = 1;
                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Import LC No"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "LC Date"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Amendment Date"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Supplier"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "PI No"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "LC Amount"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "New LC/Amendment Amount"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                nRowIndex++;
                                #endregion

                            }
                            bIsSubtotalPrint = true;
                            nCount++;
                            nColumn = 1;
                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = nCount.ToString(); cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.ImportLCNo; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.ImportLCDateSt; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.AmendmentDate != oItem.ImportLCDate ? oItem.AmendmentDate.ToString("dd MMM yyyy") : ""; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.SupplierName; cell.Style.Font.Bold = false; // new change
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.ImportPINo; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.Amount; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "$ #,##0.00";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value =oItem.AmmendmentAmount; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "$ #,##0.00";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            dIssueDate = oItem.AmendmentDate;
                            nTotalAmendmendAmount = nTotalAmendmendAmount + oItem.AmmendmentAmount;
                            nGrandTotalAmount = nGrandTotalAmount + Math.Round(oItem.AmmendmentAmount, 2);
                            nEndRow = nRowIndex;
                            nRowIndex++;
                        }
                        #endregion


                        #region Sub total print

                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 1]; cell.Value = "Sub Total :"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, nEndCol]; cell.Value = nTotalAmendmendAmount; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "$ #,##0.00";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex++;


                        #endregion

                        #region Grand total print
                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 1]; cell.Value = "Grand Total :"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, nEndCol]; cell.Value = nGrandTotalAmount; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "$ #,##0.00";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex++;


                        #endregion

                        #endregion

                        cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);

                        Response.ClearContent();
                        Response.BinaryWrite(excelPackage.GetAsByteArray());
                        Response.AddHeader("content-disposition", "attachment; filename=ImportLCRegister.xlsx");
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Flush();
                        Response.End();
                    #endregion
                    }
                }
                else
                {

                    #region Print XL

                    int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 21, nColumn = 1, nCount = 0, nImportLCID = 0;
                    ExcelRange cell;
                    ExcelRange HeaderCell;
                    ExcelFill fill;
                    OfficeOpenXml.Style.Border border;

                    using (var excelPackage = new ExcelPackage())
                    {
                        excelPackage.Workbook.Properties.Author = "ESimSol";
                        excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                        var sheet = excelPackage.Workbook.Worksheets.Add("Import LC Register");
                        sheet.Name = "Import LC Register";
                        sheet.Column(++nColumn).Width = 10; //SL No
                        if (oImportLCRegister.ReportLayout == EnumReportLayout.DateWise || oImportLCRegister.ReportLayout == EnumReportLayout.LCWise || oImportLCRegister.ReportLayout == EnumReportLayout.PartyWise)
                        {
                            sheet.Column(++nColumn).Width = 0; //File No
                        }
                        else
                        {
                            sheet.Column(++nColumn).Width = 15; //File No
                        }
                        sheet.Column(++nColumn).Width = 15; //LC No
                        sheet.Column(++nColumn).Width = 15; //LC Date
                        sheet.Column(++nColumn).Width = 30; //Supplier
                        sheet.Column(++nColumn).Width = 30; //Negotiation Bank
                        sheet.Column(++nColumn).Width = 20; //LC Terms
                        sheet.Column(++nColumn).Width = 15; //ShipmentDate
                        sheet.Column(++nColumn).Width = 15; //ExpireDate
                        sheet.Column(++nColumn).Width = 20; //PI No
                        if (oImportLCRegister.ReportLayout == EnumReportLayout.DateWise || oImportLCRegister.ReportLayout == EnumReportLayout.PartyWise || oImportLCRegister.ReportLayout == EnumReportLayout.LCWise)
                        {
                            sheet.Column(++nColumn).Width = 0; //Product Name
                            sheet.Column(++nColumn).Width = 0; //Qty
                            sheet.Column(++nColumn).Width = 0; //Rate
                        }
                        else
                        {
                            sheet.Column(++nColumn).Width = 44; //Product Name
                            sheet.Column(++nColumn).Width = 15; //Qty
                            sheet.Column(++nColumn).Width = 15; //Rate
                        }
                        sheet.Column(++nColumn).Width = 25; //Amount
                        if (oImportLCRegister.ReportLayout == EnumReportLayout.DateWise || oImportLCRegister.ReportLayout == EnumReportLayout.PartyWise)
                        {
                            sheet.Column(++nColumn).Width = 0; //InvoiceQty
                            sheet.Column(++nColumn).Width = 0; //InvoiceAmount
                            sheet.Column(++nColumn).Width = 0; //Yet2Invoice
                            sheet.Column(++nColumn).Width = 0; //Yet2InvoiceQty
                            sheet.Column(++nColumn).Width = 0; //LC App Type
                            sheet.Column(++nColumn).Width = 0; //Import PI TYpe
                        }
                        else
                        {
                            sheet.Column(++nColumn).Width = 15; //InvoiceQty
                            sheet.Column(++nColumn).Width = 15; //InvoiceAmount
                            sheet.Column(++nColumn).Width = 15; //Yet2Invoice
                            sheet.Column(++nColumn).Width = 15; //Yet2InvoiceQty
                            sheet.Column(++nColumn).Width = 15; //LC App Type
                            sheet.Column(++nColumn).Width = 15; //Import PI TYpe
                        }




                        //   nEndCol = 10;

                        #region Report Header
                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 11]; cell.Merge = true;
                        cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        cell = sheet.Cells[nRowIndex, nStartCol + 12, nRowIndex, nEndCol]; cell.Merge = true;
                        cell.Value = "ImportLC Register"; cell.Style.Font.Bold = true;
                        cell.Style.WrapText = true;
                        cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        nRowIndex = nRowIndex + 1;
                        #endregion

                        #region Address & Date
                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 11]; cell.Merge = true;
                        cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        cell = sheet.Cells[nRowIndex, nStartCol + 12, nRowIndex, nEndCol]; cell.Merge = true;
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
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "File No"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "LC No"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "LC Date"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Supplier"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Negotiation Bank"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "LC Terms"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Shipment Date"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Expire Date"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "PI No"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Product Name"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Qty"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Rate"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Amount"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Invoice Qty"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Invoice Value"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Yet To Qty"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Yet To Value"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "LC App Type"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Import PI Type"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex++;
                        #endregion

                        #region Data
                        int nImportPIID = 0; double nGrandTotalQty = 0, nGrandTotalAmount = 0; double dAmount = 0;
                        foreach (ImportLCRegister oItem in _oImportLCRegisters)
                        {
                            nColumn = 1;
                            if (nImportLCID != oItem.ImportLCID)
                            {
                                nCount++;
                                int nRowSpan = _oImportLCRegisters.Where(PIR => PIR.ImportLCID == oItem.ImportLCID).ToList().Count - 1;
                                cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex + nRowSpan, nColumn]; cell.Value = nCount; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                //cell.Merge = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex + nRowSpan, nColumn]; cell.Value = oItem.FileNo.ToString(); cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex + nRowSpan, nColumn]; cell.Value = oItem.ImportLCNo.ToString(); cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex + nRowSpan, nColumn]; cell.Value = oItem.ImportLCDateSt; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex + nRowSpan, nColumn]; cell.Value = oItem.SupplierName; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex + nRowSpan, nColumn]; cell.Value = oItem.BankName; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex + nRowSpan, nColumn]; cell.Value = oItem.LCTermName; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex + nRowSpan, nColumn]; cell.Value = oItem.ShipmentDatet; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex + nRowSpan, nColumn]; cell.Value = oItem.ExpireDateSt; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            }

                            nColumn = 10;
                            if (nImportPIID != oItem.ImportPIID)
                            {
                                int nRowSpan = _oImportLCRegisters.Where(PIR => PIR.ImportLCID == oItem.ImportLCID && PIR.ImportPIID == oItem.ImportPIID).ToList().Count - 1;
                                cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex + nRowSpan, nColumn]; cell.Value = oItem.ImportPINo; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                if (nRowSpan > 0)
                                    //cell.Merge = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            }
                            nColumn = 11;
                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            cell.Style.Numberformat.Format = "# #,##0.00";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.Qty; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "# #,##0.00";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.UnitPrice; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "$ #,##0.00";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            if (oImportLCRegister.ReportLayout == EnumReportLayout.PartyWise || oImportLCRegister.ReportLayout == EnumReportLayout.DateWise || oImportLCRegister.ReportLayout == EnumReportLayout.LCWise)
                            {
                                dAmount = oItem.Amount;
                            }
                            else
                            {
                                dAmount = oItem.LCAmount;
                            }
                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = Math.Round(dAmount, 2); cell.Style.Font.Bold = false; // change here
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "$ #,##0.00";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.InvoiceQty; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "# #,##0.00";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.InvoiceValue; cell.Style.Font.Bold = false; // new change
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "$ #,##0.00";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.YetToInvoiceQty; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "# #,##0.00";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.YetToInvoiceAmount; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "$ #,##0.00";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "" + oItem.LCAppType; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                            //cell.Merge = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "" + oItem.ImportPIType; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                            //cell.Merge = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            nImportPIID = oItem.ImportPIID;
                            nImportLCID = oItem.ImportLCID;
                            nGrandTotalQty = nGrandTotalQty + oItem.Qty;
                            nGrandTotalAmount = nGrandTotalAmount + Math.Round(dAmount, 2);
                            nEndRow = nRowIndex;
                            nRowIndex++;
                        }
                        #endregion

                        #region Total
                        int endIndex = 9;  // Enter The Remind Column Number For TOTAL
                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - endIndex--]; cell.Value = "" + "Total"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        double nValue = _oImportLCRegisters.Select(c => c.Qty).Sum();
                        cell = sheet.Cells[nRowIndex, nEndCol - endIndex--]; cell.Value = nValue; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "# #,##0.00";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, nEndCol - endIndex--]; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                        if (oImportLCRegister.ReportLayout == EnumReportLayout.PartyWise || oImportLCRegister.ReportLayout == EnumReportLayout.DateWise || oImportLCRegister.ReportLayout == EnumReportLayout.LCWise)
                        {
                            nValue = _oImportLCRegisters.Select(c => c.Amount).Sum();
                            cell = sheet.Cells[nRowIndex, nEndCol - endIndex--]; cell.Value = nValue; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "$ #,##0.00";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        else
                        {
                            nValue = _oImportLCRegisters.Select(c => c.LCAmount).Sum();
                            cell = sheet.Cells[nRowIndex, nEndCol - endIndex--]; cell.Value = nValue; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "$ #,##0.00";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        nValue = _oImportLCRegisters.Select(c => c.InvoiceQty).Sum();
                        cell = sheet.Cells[nRowIndex, nEndCol - endIndex--]; cell.Value = nValue; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "# #,##0.00";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nValue = _oImportLCRegisters.Select(c => c.InvoiceAmount).Sum();
                        cell = sheet.Cells[nRowIndex, nEndCol - endIndex--]; cell.Value = nValue; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "$ #,##0.00";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nValue = _oImportLCRegisters.Select(c => c.YetToInvoiceQty).Sum();
                        cell = sheet.Cells[nRowIndex, nEndCol - endIndex--]; cell.Value = nValue; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "# #,##0.00";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nValue = _oImportLCRegisters.Select(c => c.YetToInvoiceAmount).Sum();
                        cell = sheet.Cells[nRowIndex, nEndCol - endIndex--]; cell.Value = nValue; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "$ #,##0.00";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, nEndCol - endIndex--]; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "$ #,##0.00";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, nEndCol - endIndex--]; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "$ #,##0.00";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        nRowIndex = nRowIndex + 1;
                        #endregion
                        #endregion

                        cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);

                        Response.ClearContent();
                        Response.BinaryWrite(excelPackage.GetAsByteArray());
                        Response.AddHeader("content-disposition", "attachment; filename=ImportLCRegister.xlsx");
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Flush();
                        Response.End();
                    }
                    #endregion
                }
            }
            else
            {
                #region Print XL
                
                #endregion
            }
        }
        #endregion
        #region Support Functions
        private string GetSQL(ImportLCRegister oImportLCRegister)
        {
            _sDateRange = "";
            string sSearchingData = oImportLCRegister.SearchingData;
            EnumCompareOperator eLCImportLCDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[0]);
            DateTime dLCIssueStartDate = Convert.ToDateTime(sSearchingData.Split('~')[1]);
            DateTime dLCIssueEndDate = Convert.ToDateTime(sSearchingData.Split('~')[2]);

            EnumCompareOperator eLCExpiredDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[3]);
            DateTime dLCExpiredStartDate = Convert.ToDateTime(sSearchingData.Split('~')[4]);
            DateTime dLCExpiredEndDate = Convert.ToDateTime(sSearchingData.Split('~')[5]);

            EnumCompareOperator eLCReceiveDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[6]);
            DateTime dLCReceivedStartDate = Convert.ToDateTime(sSearchingData.Split('~')[7]);
            DateTime dLCReceivedEndDate = Convert.ToDateTime(sSearchingData.Split('~')[8]);

            EnumCompareOperator eLCAmount = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[9]);
            double nLCAmountStsrt = Convert.ToDouble(sSearchingData.Split('~')[10]);
            double nLCAmountEnd = Convert.ToDouble(sSearchingData.Split('~')[11]);
            string sImportInvoiceNo = sSearchingData.Split('~')[12];
            int nLCPaymentType = Convert.ToInt32(sSearchingData.Split('~')[13]);

            EnumCompareOperator eRequestDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[14]);
            DateTime dRequestDate = Convert.ToDateTime(sSearchingData.Split('~')[15]);
            DateTime dRequestEndDate = Convert.ToDateTime(sSearchingData.Split('~')[16]);

            bool bIsWthoutCancel = Convert.ToBoolean(sSearchingData.Split('~')[17]);

            string sSQLQuery = "", sWhereCluse = "", sGroupBy = "", sOrderBy = "";

            #region BusinessUnit
            if (oImportLCRegister.BUID > 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " BUID =" + oImportLCRegister.BUID.ToString();
            }
            #endregion

            #region ImportLCNo
            if (oImportLCRegister.ImportLCNo != null && oImportLCRegister.ImportLCNo != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " ImportLCNo LIKE'%" + oImportLCRegister.ImportLCNo + "%'";
            }
            #endregion
            #region ImportLCNo
            if (nLCPaymentType != 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " LCPaymentType =" + nLCPaymentType;
            }
            #endregion

            #region ApprovedBy
            if (oImportLCRegister.ReceivedBy != 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " ReceivedBy =" + oImportLCRegister.ReceivedBy.ToString();
            }
            #endregion

            #region ImportLCStatus
            if (oImportLCRegister.LCCurrentStatus != EnumLCCurrentStatus.None)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " LCCurrentStatus =" + ((int)oImportLCRegister.LCCurrentStatus).ToString();
            }
            #endregion

            #region Wthout Cancel
            if (bIsWthoutCancel != false)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " LCCurrentStatus != " + (int)EnumLCCurrentStatus.LCCancel;
            }
            #endregion

            #region Supplier
            if (oImportLCRegister.SupplierName != null && oImportLCRegister.SupplierName != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " ContractorID IN(" + oImportLCRegister.SupplierName + ")";
            }
            #endregion
            #region ProductType
            if (oImportLCRegister.ImportProductName != null && oImportLCRegister.ImportProductName != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " ImportPIID IN (SELECT ImportPIID FROM ImportPI WHERE  ProductType IN(" + oImportLCRegister.ImportProductName + "))";
            }
            #endregion
           

            #region Product
            if (oImportLCRegister.ProductName != null && oImportLCRegister.ProductName != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " ProductID IN(" + oImportLCRegister.ProductName + ")";
            }
            #endregion

            #region Bank Branch
            if (oImportLCRegister.BranchName != null && oImportLCRegister.BranchName != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " BankBranchID_Nego IN(" + oImportLCRegister.BranchName + ")";
            }
            #endregion

            #region LC Issue Date
            if (eLCImportLCDate != EnumCompareOperator.None)
            {
                string sDateFieldName = "ImportLCDate";
                string sDateFullName = "Import LC Date";
                if (oImportLCRegister.ReportLayout == EnumReportLayout.LCWithAmendment)
                {
                    sDateFullName = "LC Amendment Date";
                }
                
                if (eLCImportLCDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12)," + sDateFieldName + ",106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCIssueStartDate.ToString("dd MMM yyyy") + "', 106))";
                    if (oImportLCRegister.ReportLayout == EnumReportLayout.LCWithAmendment) { sDateFieldName = "AmendmentDate"; sWhereCluse = sWhereCluse + " OR CONVERT(DATE,CONVERT(VARCHAR(12)," + sDateFieldName + ",106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCIssueStartDate.ToString("dd MMM yyyy") + "', 106))"; }
                    _sDateRange = sDateFullName+" @ " + dLCIssueStartDate.ToString("dd MMM yyyy");
                }
                else if (eLCImportLCDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12)," + sDateFieldName + ",106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCIssueStartDate.ToString("dd MMM yyyy") + "', 106))";
                    if (oImportLCRegister.ReportLayout == EnumReportLayout.LCWithAmendment) { sDateFieldName = "AmendmentDate"; sWhereCluse = sWhereCluse + " OR CONVERT(DATE,CONVERT(VARCHAR(12)," + sDateFieldName + ",106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCIssueStartDate.ToString("dd MMM yyyy") + "', 106))"; }
                    _sDateRange = sDateFullName + " Not Equal @ " + dLCIssueStartDate.ToString("dd MMM yyyy");
                }
                else if (eLCImportLCDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12)," + sDateFieldName + ",106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCIssueStartDate.ToString("dd MMM yyyy") + "', 106))";
                    if (oImportLCRegister.ReportLayout == EnumReportLayout.LCWithAmendment) { sDateFieldName = "AmendmentDate"; sWhereCluse = sWhereCluse + " OR CONVERT(DATE,CONVERT(VARCHAR(12)," + sDateFieldName + ",106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCIssueStartDate.ToString("dd MMM yyyy") + "', 106))"; }
                    _sDateRange = sDateFullName + " Greater Then @ " + dLCIssueStartDate.ToString("dd MMM yyyy");
                }
                else if (eLCImportLCDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12)," + sDateFieldName + ",106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCIssueStartDate.ToString("dd MMM yyyy") + "', 106))";
                    if (oImportLCRegister.ReportLayout == EnumReportLayout.LCWithAmendment) { sDateFieldName = "AmendmentDate"; sWhereCluse = sWhereCluse + " OR CONVERT(DATE,CONVERT(VARCHAR(12)," + sDateFieldName + ",106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCIssueStartDate.ToString("dd MMM yyyy") + "', 106))"; }
                    _sDateRange = sDateFullName + " Smaller Then @ " + dLCIssueStartDate.ToString("dd MMM yyyy");
                }
                else if (eLCImportLCDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12)," + sDateFieldName + ",106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCIssueStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCIssueEndDate.ToString("dd MMM yyyy") + "', 106))";
                    if (oImportLCRegister.ReportLayout == EnumReportLayout.LCWithAmendment) { sDateFieldName = "AmendmentDate"; sWhereCluse = sWhereCluse + " OR CONVERT(DATE,CONVERT(VARCHAR(12)," + sDateFieldName + ",106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCIssueStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCIssueEndDate.ToString("dd MMM yyyy") + "', 106))"; }
                    _sDateRange = sDateFullName + " Between " + dLCIssueStartDate.ToString("dd MMM yyyy") + " To " + dLCIssueEndDate.ToString("dd MMM yyyy");
                }
                else if (eLCImportLCDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12)," + sDateFieldName + ",106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCIssueStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCIssueEndDate.ToString("dd MMM yyyy") + "', 106))";
                    if (oImportLCRegister.ReportLayout == EnumReportLayout.LCWithAmendment) { sDateFieldName = "AmendmentDate"; sWhereCluse = sWhereCluse + " OR CONVERT(DATE,CONVERT(VARCHAR(12)," + sDateFieldName + ",106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCIssueStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCIssueEndDate.ToString("dd MMM yyyy") + "', 106))"; }
                    _sDateRange = sDateFullName + " NOT Between " + dLCIssueStartDate.ToString("dd MMM yyyy") + " To " + dLCIssueEndDate.ToString("dd MMM yyyy");
                }
            }
            #endregion

            #region LCExpired Date
            if (eLCExpiredDate != EnumCompareOperator.None)
            {
                if (eLCExpiredDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ExpireDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCExpiredStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eLCExpiredDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ExpireDate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCExpiredStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eLCExpiredDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ExpireDate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCExpiredStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eLCExpiredDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ExpireDate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCExpiredStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eLCExpiredDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ExpireDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCExpiredStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCExpiredEndDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eLCExpiredDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ExpireDate,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCExpiredStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCExpiredEndDate.ToString("dd MMM yyyy") + "', 106))";
                }
            }
            #endregion

            #region LC Received Date
            if (eLCReceiveDate != EnumCompareOperator.None)
            {
                if (eLCReceiveDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ReceiveDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCReceivedStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eLCReceiveDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ReceiveDate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCReceivedStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eLCReceiveDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ReceiveDate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCReceivedStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eLCReceiveDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ReceiveDate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCReceivedStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eLCReceiveDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ReceiveDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCReceivedStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCReceivedEndDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eLCReceiveDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ReceiveDate,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCReceivedStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCReceivedEndDate.ToString("dd MMM yyyy") + "', 106))";
                }
            }
            #endregion

            #region Request Date
            if (eRequestDate != EnumCompareOperator.None)
            {
                if (eRequestDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),LCRequestDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dRequestDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eRequestDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),LCRequestDate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dRequestDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eRequestDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),LCRequestDate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dRequestDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eRequestDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),LCRequestDate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dRequestDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eRequestDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),LCRequestDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dRequestDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dRequestEndDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eRequestDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),LCRequestDate,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dRequestDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dRequestEndDate
                        .ToString("dd MMM yyyy") + "', 106))";
                }
            }
            #endregion

            #region LCAmount
            if (eLCAmount != EnumCompareOperator.None)
            {
                if (eLCAmount == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " Amount = " + nLCAmountStsrt.ToString("0.00");
                }
                else if (eLCAmount == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " Amount != " + nLCAmountStsrt.ToString("0.00"); ;
                }
                else if (eLCAmount == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " Amount > " + nLCAmountStsrt.ToString("0.00"); ;
                }
                else if (eLCAmount == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " Amount < " + nLCAmountStsrt.ToString("0.00"); ;
                }
                else if (eLCAmount == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " Amount BETWEEN " + nLCAmountStsrt.ToString("0.00") + " AND " + nLCAmountEnd.ToString("0.00");
                }
                else if (eLCAmount == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " Amount NOT BETWEEN " + nLCAmountStsrt.ToString("0.00") + " AND " + nLCAmountEnd.ToString("0.00");
                }
            }
            #endregion

            #region ImportInvoiceNo
            if (sImportInvoiceNo != null && sImportInvoiceNo != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " ImportLCID IN (SELECT NN.ImportLCID FROM ImportInvoice AS NN WHERE NN.ImportInvoiceNo LIKE '%" + sImportInvoiceNo + "%')";
            }
            #endregion

            #region Import PI TYpe
            if ((int)oImportLCRegister.ImportPIType > 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " ImportPIID IN (SELECT ImportPIID FROM ImportPI WHERE ImportPIType = " + (int)oImportLCRegister.ImportPIType + ") ";
                sLCType = oImportLCRegister.ImportPITypeST;
            }
            #endregion

            #region Activity
            if (oImportLCRegister.ReportLayout == EnumReportLayout.LCWithAmendment)
            {
                sWhereCluse +=" AND ISNULL(Activity,0)=1";//only Active Item
            }
            #endregion

            #region Report Layout
            if (oImportLCRegister.ReportLayout == EnumReportLayout.LCWise)
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT  ImportPIType, ImportPIID, ImportPINo, BankName,LCTermName, ImportLCID, BUID, ImportLCNo, ImportLCDate,  SupplierName,  ReceiveDate, ExpireDate, ImportLCNo, PIValue, PIDate, ShipmentDate, CurrencySymbol, Amount, Sum(InvoiceQty*UnitPrice) as InvoiceValue FROM View_ImportLCRegister "
                            ;// +"(SELECT SUM(ISNULL(IID.Qty,0)) FROM ImportInvoiceDetail AS IID WHERE IID.ImportInvoiceID In (SELECT ImportInvoiceID FROM ImportInvoice wHERE ImportLCID = View_ImportLCRegister.ImportLCID) AND IID.ProductID = View_ImportLCRegister.ProductID) AS InvoiceQty ";
                sGroupBy = " GROUP BYImportPIType, ImportPIID, ImportPINo,BankName,LCTermName, BUID, ImportLCID, ImportLCNo, ImportLCDate,  SupplierName,  ReceiveDate, ExpireDate, ImportLCNo, PIValue, PIDate, ShipmentDate, CurrencySymbol, Amount";
                sOrderBy = " ORDER BY ImportLCID ASC";
            }
            else if (oImportLCRegister.ReportLayout == EnumReportLayout.LCWithAmendment)
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT * FROM View_ImportLCRegister ";
                //sSQLQuery = "SELECT  ImportLCID,BUID, ImportLCNo, ImportLCDate,  SupplierName, ImportPINo,  AmendmentDate,  Amount, AmmendmentAmount , CurrencySymbol   FROM View_ImportLCRegister ";
                //sGroupBy = "GROUP BY ImportLCID,ImportLCDate, AmendmentDate, BUID,  ImportLCNo,  ImportPINo, SupplierName, Amount, AmmendmentAmount, CurrencySymbol";
                sOrderBy = " ORDER BY AmendmentDate, ImportLCID, ImportLCDetailID ASC";
            }
            else if (oImportLCRegister.ReportLayout == EnumReportLayout.DateWise)
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT ImportPIType, ImportLCID,BankName,LCTermName,  BUID, ImportLCNo, ImportLCDate,  SupplierName,  ReceiveDate, ExpireDate, ImportLCNo,   ShipmentDate,  Amount,CurrencySymbol   FROM View_ImportLCRegister ";
                sGroupBy = " GROUP BY ImportPIType,ImportLCID, BankName,LCTermName, BUID,  ImportLCNo, ImportLCDate,  SupplierName, ReceiveDate, ExpireDate, ImportLCNo, ShipmentDate, Amount,CurrencySymbol";
                sOrderBy = " ORDER BY ImportLCDate ASC";
            }
            else if (oImportLCRegister.ReportLayout == EnumReportLayout.DateWiseDetails)
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT * FROM View_ImportLCRegister ";
                sOrderBy = " ORDER BY  ImportLCDate, ImportLCID, ImportLCDetailID ASC";
            }
            else if (oImportLCRegister.ReportLayout == EnumReportLayout.PartyWise)
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = " SELECT ImportPIType, ImportLCID,BankName,LCTermName,  BUID, ImportLCNo, ImportLCDate,  SupplierName,  ReceiveDate, ExpireDate, ImportLCNo,   ShipmentDate,  Amount, CurrencySymbol, FileNo   FROM View_ImportLCRegister ";
                sGroupBy = " GROUP BY ImportPIType, ImportLCID, BankName,LCTermName, BUID,  ImportLCNo, ImportLCDate,  SupplierName, ReceiveDate, ExpireDate, ImportLCNo,   ShipmentDate,  Amount, CurrencySymbol, FileNo ";
                sOrderBy = " ORDER BY SupplierName ASC";
            }
            else if (oImportLCRegister.ReportLayout == EnumReportLayout.PartyWiseDetails)
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT * FROM View_ImportLCRegister ";
                sOrderBy = " ORDER BY  SupplierName, ImportLCID, ImportLCDetailID ASC";
            }
            else if (oImportLCRegister.ReportLayout == EnumReportLayout.Export_LC_Details)
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT * FROM View_ImportLCRegister ";
                sOrderBy = " ORDER BY  ExportLCNo, ImportLCID, ImportLCDetailID ASC";
            }
            else if (oImportLCRegister.ReportLayout == EnumReportLayout.ProductWise)
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT * FROM View_ImportLCRegister ";
                sOrderBy = " ORDER BY  ProductName ASC";
            }
            else
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT * FROM View_ImportLCRegister ";
                sOrderBy = " ORDER BY ImportLCDate, ImportLCID, ImportLCDetailID ASC";
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

