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
    public class ImportInvoiceRegisterController : Controller
    {   
        string _sDateRange = "";
        string _sErrorMesage = "";
        string sLCType = "";
        List<ImportInvoiceRegister> _oImportInvoiceRegisters = new List<ImportInvoiceRegister>();        

        #region Actions
        public ActionResult ViewImportInvoiceRegisters(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            ImportInvoiceRegister oImportInvoiceRegister = new ImportInvoiceRegister();

            #region Approval User
            string sSQL = "SELECT * FROM View_User AS HH WHERE HH.UserID IN (SELECT DISTINCT MM.AcceptedBy FROM ImportInvoice AS MM WHERE ISNULL(MM.AcceptedBy,0)!=0) ORDER BY HH.UserName";
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
                if ((EnumReportLayout)oItem.id == EnumReportLayout.InvoiceWise || (EnumReportLayout)oItem.id == EnumReportLayout.DateWise || (EnumReportLayout)oItem.id == EnumReportLayout.DateWiseDetails || (EnumReportLayout)oItem.id == EnumReportLayout.PartyWise || (EnumReportLayout)oItem.id == EnumReportLayout.PartyWiseDetails || (EnumReportLayout)oItem.id == EnumReportLayout.ProductWise)
                {
                    oReportLayouts.Add(oItem);
                }
            }
            #endregion

            List<ImportProduct> oImportProducts = new List<ImportProduct>();
            oImportProducts = ImportProduct.GetByBU(buid, (int)Session[SessionInfo.currentUserID]);
            ViewBag.ImportProducts = oImportProducts;
            ViewBag.BUID = buid;
            ViewBag.ApprovalUsers = oApprovalUsers;
            ViewBag.ReportLayouts = oReportLayouts;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.ImportInvoiceStates = EnumObject.jGets(typeof(EnumInvoiceEvent));
            ViewBag.ImportInvoiceTypes = EnumObject.jGets(typeof(EnumImportPIType));
            ViewBag.ImportPITypeObj = EnumObject.jGets(typeof(EnumImportPIType)).Where(x => x.id == (int)EnumImportPIType.Foreign || x.id == (int)EnumImportPIType.Local || x.id == (int)EnumImportPIType.NonLC || x.id == (int)EnumImportPIType.TTForeign || x.id == (int)EnumImportPIType.TTLocal);
            ViewBag.InvoiceBankStatusObjs = EnumObject.jGets(typeof(EnumInvoiceBankStatus));
            return View(oImportInvoiceRegister);
        }

        [HttpPost]
        public ActionResult SetSessionSearchCriteria(ImportInvoiceRegister oImportInvoiceRegister)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oImportInvoiceRegister);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintImportInvoiceRegister(double ts,int nLCType)
        {
            ImportInvoiceRegister oImportInvoiceRegister = new ImportInvoiceRegister();
            try
            {
                if (nLCType != null)
                {
                    if (nLCType == 0)
                    {
                        sLCType = "";
                    }
                    if (nLCType == 1)
                    {
                        sLCType = "Foreign";
                    }
                    if (nLCType == 2)
                    {
                        sLCType = "non L/C";
                    }
                    if (nLCType == 3)
                    {
                        sLCType = "TT Foreign";
                    }
                    if (nLCType == 4)
                    {
                        sLCType = "Local";
                    }
                    if (nLCType == 5)
                    {
                        sLCType = "TT Local";
                    }
                }
                _sErrorMesage = "";
                _oImportInvoiceRegisters = new List<ImportInvoiceRegister>();                
                oImportInvoiceRegister = (ImportInvoiceRegister)Session[SessionInfo.ParamObj];
                string sSQL = this.GetSQL(oImportInvoiceRegister);
                _oImportInvoiceRegisters = ImportInvoiceRegister.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (_oImportInvoiceRegisters.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oImportInvoiceRegisters = new List<ImportInvoiceRegister>();
                _sErrorMesage = ex.Message;
            }

            if (_sErrorMesage == "")
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                BusinessUnit oBU = new BusinessUnit();
                oBU = oBU.Get(_oImportInvoiceRegisters[0].BUID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBU);

                rptImportInvoiceRegisters oReport = new rptImportInvoiceRegisters();
                byte[] abytes = oReport.PrepareReport(_oImportInvoiceRegisters, oCompany, oImportInvoiceRegister.ReportLayout, _sDateRange, sLCType);
                return File(abytes, "application/pdf");
            }
            else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport(_sErrorMesage);
                return File(abytes, "application/pdf");
            }
        }

        public void ExportToExcelImportInvoiceRegister(double ts,int nLCType)
        {
            ImportInvoiceRegister oImportInvoiceRegister = new ImportInvoiceRegister();
            try
            {
                if (nLCType != null)
                {
                    if (nLCType == 0)
                    {
                        sLCType = "";
                    }
                    if (nLCType == 1)
                    {
                        sLCType = "Foreign";
                    }
                    if (nLCType == 2)
                    {
                        sLCType = "non L/C";
                    }
                    if (nLCType == 3)
                    {
                        sLCType = "TT Foreign";
                    }
                    if (nLCType == 4)
                    {
                        sLCType = "Local";
                    }
                    if (nLCType == 5)
                    {
                        sLCType = "TT Local";
                    }
                }
                _sErrorMesage = "";
                _oImportInvoiceRegisters = new List<ImportInvoiceRegister>();
                oImportInvoiceRegister = (ImportInvoiceRegister)Session[SessionInfo.ParamObj];
                string sSQL = this.GetSQL(oImportInvoiceRegister);
                _oImportInvoiceRegisters = ImportInvoiceRegister.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (_oImportInvoiceRegisters.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oImportInvoiceRegisters = new List<ImportInvoiceRegister>();
                _sErrorMesage = ex.Message;
            }

            byte[] abytes = null;
            if (_sErrorMesage == "")
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = null;// GetCompanyLogo(oCompany);
                BusinessUnit oBU = new BusinessUnit();
                oBU = oBU.Get(_oImportInvoiceRegisters[0].BUID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBU);

                //rptImportInvoiceRegisters oReport = new rptImportInvoiceRegisters();
                //PdfPTable oPdfPTable = oReport.PrepareExcel(_oImportInvoiceRegisters, oCompany, oImportInvoiceRegister.ReportLayout, _sDateRange);
                //ExportToExcel.WorksheetName = "ImportInvoiceRegister";
                //abytes = ExportToExcel.ConvertToExcel(oPdfPTable);

                #region Print XL

                int nRowIndex = 2, nStartRow = 2, nStartCol = 2, nEndCol = 19, nColumn = 1, nCount = 0, nImportLCID = 0;
                ExcelRange cell;
                ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("ImportInvoiceRegister");
                    sheet.Name = "Sample OutStanding ";
                    sheet.Column(++nColumn).Width = 10; //SL No
                    sheet.Column(++nColumn).Width = 15; //Invoice No
                    sheet.Column(++nColumn).Width = 15; //Invoice Date
                    sheet.Column(++nColumn).Width = 20; //Master LC no
                    sheet.Column(++nColumn).Width = 15; //File No
                    sheet.Column(++nColumn).Width = 15; //LC No
                    sheet.Column(++nColumn).Width = 15; //LC Date
                    sheet.Column(++nColumn).Width = 30; //Supplier
                    sheet.Column(++nColumn).Width = 30; //Negotiation Bank
                    sheet.Column(++nColumn).Width = 15; //BL No
                    sheet.Column(++nColumn).Width = 15; //BL Date
                    sheet.Column(++nColumn).Width = 15; //BL Entry No
                    sheet.Column(++nColumn).Width = 15; //BL Entry Date
                    sheet.Column(++nColumn).Width = 15; //Status
                    sheet.Column(++nColumn).Width = 44; //Product Name
                    sheet.Column(++nColumn).Width = 15; //Qty
                    sheet.Column(++nColumn).Width = 15; //Rate
                    sheet.Column(++nColumn).Width = 15; //Amount
                    //nEndCol = nColumn;

                    #region Report Header
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 10]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, nStartCol + 11, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = "Import Invocice Register"; cell.Style.Font.Bold = true;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Address & Date
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 9]; cell.Merge = true;
                    cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, nStartCol + 10, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = _sDateRange; cell.Style.Font.Bold = false;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;

                    if (sLCType != "")
                    {
                        cell = sheet.Cells[nRowIndex, nStartCol + 10, nRowIndex, nEndCol]; cell.Merge = true;
                        cell.Value = "L/C Type "+sLCType; cell.Style.Font.Bold = false;
                        cell.Style.WrapText = true;
                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        nRowIndex = nRowIndex + 1;
                    }
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

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Invoice No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Invoice Date"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Sales Contract No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "File No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "L/C No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "LC Date"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Supplier"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Bank Name"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "BL No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "BL Date"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "BL Entry No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "BL Entry Date"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Status"; cell.Style.Font.Bold = true;
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

                    nRowIndex++;
                    #endregion

                    #region Data
                    int nImportPIID = 0; double nGrandTotalQty = 0, nGrandTotalAmount = 0;
                    string sImportLCNo = "`",sImportInvoiceNo="`";
                    _oImportInvoiceRegisters = _oImportInvoiceRegisters.OrderBy(x => x.ImportLCNo).ThenBy(x => x.ImportInvoiceNo).ToList();

                    foreach (ImportInvoiceRegister oItem in _oImportInvoiceRegisters)
                    {
                        nColumn = 1;
                        
                        nCount++;
                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = nCount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        if (sImportInvoiceNo != oItem.ImportInvoiceNo)//sImportLCNo.Equals(oItem.ImportLCNo)
                        {
                            int nEndRow = (_oImportInvoiceRegisters.Where(PIR => PIR.ImportInvoiceNo.Equals(oItem.ImportInvoiceNo)).ToList().Count) + nRowIndex-1;
                            cell = sheet.Cells[nRowIndex, ++nColumn, nEndRow, nColumn]; cell.Value = oItem.ImportInvoiceNo.ToString(); cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Merge = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        nColumn = 3;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.DateofInvoiceSt; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.MasterLCNos; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.WrapText = true;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.FileNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        if(sImportLCNo != oItem.ImportLCNo)  ////!sImportLCNo.Equals(oItem.ImportLCNo) 
                        {
                            int nEndRow = (_oImportInvoiceRegisters.Where(PIR => PIR.ImportInvoiceNo.Equals(oItem.ImportInvoiceNo)).ToList().Count) + nRowIndex-1;
                            cell = sheet.Cells[nRowIndex, ++nColumn, nEndRow, nColumn]; cell.Value = oItem.ImportLCNo; cell.Style.Font.Bold = false;
                            try
                            {
                                cell.Merge = true;
                            }
                            catch (Exception e) { } 
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; 
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        nColumn = 7;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.ImportLCDateSt; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.SupplierName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.WrapText = true;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = (oItem.BankName_Nego.Equals("") ? "-" : oItem.BankWithBranch); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.BLNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.BLDateSt; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.BillofEntryNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.BillOfEntryDateSt; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                       
                        if (string.IsNullOrEmpty(oItem.ImportLCNo))
                        {
                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value ="Wating For L/C No"; cell.Style.Font.Bold = false;
                        }
                        else
                        {
                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.ImportInvoiceStatusSt; cell.Style.Font.Bold = false;
                        }
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    
                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.Qty; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "# #,##0.00";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.UnitPrice; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "$ #,##0.00";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.Amount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "$ #,##0.00";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        sImportLCNo = oItem.ImportLCNo;
                        sImportInvoiceNo = oItem.ImportInvoiceNo;
                        //nImportLCID = oItem.ImportLCID;
                        nGrandTotalQty = nGrandTotalQty + oItem.Qty;
                        nGrandTotalAmount = nGrandTotalAmount + oItem.Amount;
                        //nEndRow = nRowIndex;
                        nRowIndex++;
                    }
                    #endregion

                    #region Total
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 3]; cell.Value = "" + "Total"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Merge = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    double nValue = _oImportInvoiceRegisters.Select(c => c.Qty).Sum();
                    cell = sheet.Cells[nRowIndex, nEndCol - 2]; cell.Value = nValue; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "# #,##0.00";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nEndCol - 1]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nValue = _oImportInvoiceRegisters.Select(c => c.Amount).Sum();
                    cell = sheet.Cells[nRowIndex, nEndCol]; cell.Value = nValue; cell.Style.Font.Bold = true;
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
                    Response.AddHeader("content-disposition", "attachment; filename=ImportInvoiceRegister.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion

            }
        }
        #endregion

        #region Support Functions
        private string GetSQL(ImportInvoiceRegister oImportInvoiceRegister)
        {
            _sDateRange = "";
            string sSearchingData = oImportInvoiceRegister.SearchingData;
            EnumCompareOperator eDateofInvoiceDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[0]);
            DateTime dDateofInvoiceStartDate = Convert.ToDateTime(sSearchingData.Split('~')[1]);
            DateTime dDateofInvoiceEndDate = Convert.ToDateTime(sSearchingData.Split('~')[2]);

            EnumCompareOperator eDateofAcceptance = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[3]);
            DateTime dInvoiceDateofAcceptanceStartDate = Convert.ToDateTime(sSearchingData.Split('~')[4]);
            DateTime dInvoiceDateofAcceptanceEndDate = Convert.ToDateTime(sSearchingData.Split('~')[5]);

            EnumCompareOperator eDateofApplication = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[6]);
            DateTime dDateofApplicationStartDate = Convert.ToDateTime(sSearchingData.Split('~')[7]);
            DateTime dDateofApplicationEndDate = Convert.ToDateTime(sSearchingData.Split('~')[8]);

            EnumCompareOperator eDateofMaturity = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[9]);
            DateTime dDateofMaturityStartDate = Convert.ToDateTime(sSearchingData.Split('~')[10]);
            DateTime dDateofMaturityEndDate = Convert.ToDateTime(sSearchingData.Split('~')[11]);

            EnumCompareOperator ePIAmount = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[12]);
            double nPIAmountStsrt = Convert.ToDouble(sSearchingData.Split('~')[13]);
            double nPIAmountEnd = Convert.ToDouble(sSearchingData.Split('~')[14]);

            string sImportLCNo = sSearchingData.Split('~')[15];
            string sImportInvoiceNo = sSearchingData.Split('~')[16];

            string sImportDateString = sSearchingData.Split('~')[17];
            int nImportDate = Convert.ToInt32(sSearchingData.Split('~')[18]);
            DateTime dImportStartDate = Convert.ToDateTime(sSearchingData.Split('~')[19]);
            DateTime dImportEndDate = Convert.ToDateTime(sSearchingData.Split('~')[20]);

            bool bIsWthoutCancel = Convert.ToBoolean(sSearchingData.Split('~')[21]);
            int nImportPIType = Convert.ToInt32(sSearchingData.Split('~')[22]);
            string sBankStatusIDs = "";
            if (sSearchingData.Split('~').Length > 23)
                sBankStatusIDs = sSearchingData.Split('~')[23];

            string sSQLQuery = "", sWhereCluse = "", sGroupBy = "", sOrderBy = "";

            #region Import Date String

            if (!string.IsNullOrEmpty(sImportDateString))
            {
                DateObject.CompareDateQuery(ref sWhereCluse, sImportDateString, nImportDate, dImportStartDate, dImportEndDate);
            }

            #endregion

            #region BusinessUnit
            if (oImportInvoiceRegister.BUID > 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " BUID =" + oImportInvoiceRegister.BUID.ToString();
            }
            #endregion

            #region ImportInvoiceNo
            if (oImportInvoiceRegister.ImportInvoiceNo != null && oImportInvoiceRegister.ImportInvoiceNo != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " ImportInvoiceNo LIKE'%" + oImportInvoiceRegister.ImportInvoiceNo + "%'";
            }
            #endregion

            #region ApprovedBy
            if (oImportInvoiceRegister.AcceptedBy != 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " AcceptedBy =" + oImportInvoiceRegister.AcceptedBy.ToString();
            }
            #endregion

            #region InvoiceStatus
            if (oImportInvoiceRegister.InvoiceStatus != EnumInvoiceEvent.None)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " InvoiceStatus =" + ((int)oImportInvoiceRegister.InvoiceStatus).ToString();
            }
            #endregion

            #region Wthout Cancel
            if (bIsWthoutCancel != false)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " InvoiceStatus != " + (int)EnumInvoiceEvent.Cancel_Invoice + " ";
            }
            #endregion

            #region ImportInvoiceType
            if (oImportInvoiceRegister.InvoiceType != EnumImportPIType.None)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " InvoiceType =" + ((int)oImportInvoiceRegister.InvoiceType).ToString();
            }
            #endregion

            #region ProductType
            if (oImportInvoiceRegister.ProductType != EnumProductNature.Dyeing)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + "  ImportPIDetailID IN (SELECT ImportPIDetailID FROM ImportPIDetail WHERE  ImportPIID IN (SELECT ImportPIID FROM ImportPI WHERE  ProductType =" + ((int)oImportInvoiceRegister.ProductType).ToString() + "))";
            }
            #endregion

            #region Remarks
            if (oImportInvoiceRegister.Remarks != null && oImportInvoiceRegister.Remarks != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " Remarks LIKE'%" + oImportInvoiceRegister.Remarks + "%'";
            }
            #endregion

            #region Supplier
            if (oImportInvoiceRegister.SupplierName != null && oImportInvoiceRegister.SupplierName != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " ContractorID IN(" + oImportInvoiceRegister.SupplierName + ")";
            }
            #endregion

            #region Product
            if (oImportInvoiceRegister.ProductName != null && oImportInvoiceRegister.ProductName != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " ProductID IN(" + oImportInvoiceRegister.ProductName + ")";
            }
            #endregion

            #region Invoice Date
            if (eDateofInvoiceDate != EnumCompareOperator.None)
            {
                if (eDateofInvoiceDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),DateofInvoice,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofInvoiceStartDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "Invoice Date @ " + dDateofInvoiceStartDate.ToString("dd MMM yyyy");
                }
                else if (eDateofInvoiceDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),DateofInvoice,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofInvoiceStartDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "Invoice Date Not Equal @ " + dDateofInvoiceStartDate.ToString("dd MMM yyyy");
                }
                else if (eDateofInvoiceDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),DateofInvoice,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofInvoiceStartDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "Invoice Date Greater Then @ " + dDateofInvoiceStartDate.ToString("dd MMM yyyy");
                }
                else if (eDateofInvoiceDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),DateofInvoice,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofInvoiceStartDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "Invoice Date Smaller Then @ " + dDateofInvoiceStartDate.ToString("dd MMM yyyy");
                }
                else if (eDateofInvoiceDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),DateofInvoice,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofInvoiceStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofInvoiceEndDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "Invoice Date Between " + dDateofInvoiceStartDate.ToString("dd MMM yyyy") + " To " + dDateofInvoiceEndDate.ToString("dd MMM yyyy");
                }
                else if (eDateofInvoiceDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),DateofInvoice,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofInvoiceStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofInvoiceEndDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "Invoice Date NOT Between " + dDateofInvoiceStartDate.ToString("dd MMM yyyy") + " To " + dDateofInvoiceEndDate.ToString("dd MMM yyyy");
                }
            }
            #endregion

            #region InvoiceDateofAcceptance
            if (eDateofAcceptance != EnumCompareOperator.None)
            {
                if (eDateofAcceptance == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),DateofAcceptance,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dInvoiceDateofAcceptanceStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eDateofAcceptance == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),DateofAcceptance,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dInvoiceDateofAcceptanceStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eDateofAcceptance == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),DateofAcceptance,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dInvoiceDateofAcceptanceStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eDateofAcceptance == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),DateofAcceptance,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dInvoiceDateofAcceptanceStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eDateofAcceptance == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),DateofAcceptance,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dInvoiceDateofAcceptanceStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dInvoiceDateofAcceptanceEndDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eDateofAcceptance == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),DateofAcceptance,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dInvoiceDateofAcceptanceStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dInvoiceDateofAcceptanceEndDate.ToString("dd MMM yyyy") + "', 106))";
                }
            }
            #endregion

            #region DateofApplication Date
            if (eDateofApplication != EnumCompareOperator.None)
            {
                if (eDateofApplication == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),DateofApplication,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofApplicationStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eDateofApplication == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),DateofApplication,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofApplicationStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eDateofApplication == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),DateofApplication,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofApplicationStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eDateofApplication == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),DateofApplication,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofApplicationStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eDateofApplication == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),DateofApplication,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofApplicationStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofApplicationEndDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eDateofApplication == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),DateofApplication,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofApplicationStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofApplicationEndDate.ToString("dd MMM yyyy") + "', 106))";
                }
            }
            #endregion

            #region DateofMaturity
            if (eDateofMaturity != EnumCompareOperator.None)
            {
                if (eDateofMaturity == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),DateofMaturity,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofMaturityStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eDateofMaturity == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),DateofMaturity,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofMaturityStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eDateofMaturity == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),DateofMaturity,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofMaturityStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eDateofMaturity == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),DateofMaturity,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofMaturityStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eDateofMaturity == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),DateofMaturity,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofMaturityStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofMaturityEndDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eDateofMaturity == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),DateofMaturity,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofMaturityStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDateofMaturityEndDate.ToString("dd MMM yyyy") + "', 106))";
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
                //sWhereCluse = sWhereCluse + " ImportInvoiceID IN (SELECT MM.ImportInvoiceID FROM ImportLCDetail AS MM WHERE MM.ImportLCID IN (SELECT NN.ImportLCID FROM ImportLC AS NN WHERE NN.ImportLCNo LIKE '%" + sImportLCNo + "%'))";
                sWhereCluse = sWhereCluse + " ImportLCID IN (SELECT NN.ImportLCID FROM ImportLC AS NN WHERE NN.ImportLCNo LIKE '%" + sImportLCNo + "%')";
            }
            #endregion

            #region ImportInvoiceNo
            if (sImportInvoiceNo != null && sImportInvoiceNo != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " ImportInvoiceID IN (SELECT MM.ImportInvoiceID FROM ImportLCDetail AS MM WHERE MM.ImportLCID IN (SELECT NN.ImportLCID FROM ImportInvoice AS NN WHERE NN.ImportInvoiceNo LIKE '%" + sImportInvoiceNo + "%'))";
            }
            #endregion

            #region Import PI Type
            if (nImportPIType > 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " InvoiceType = " + nImportPIType;
            }
            #endregion

            #region Bank status
            if (!string.IsNullOrEmpty(sBankStatusIDs))
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " BankStatus IN(" + sBankStatusIDs + ")";
            }
            #endregion

            #region Report Layout
            if (oImportInvoiceRegister.ReportLayout == EnumReportLayout.InvoiceWise)
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT * FROM View_ImportInvoiceRegister ";
                sOrderBy = " ORDER BY  DateofInvoice, ImportInvoiceID, ImportInvoiceDetailID ASC";
            }
            else if (oImportInvoiceRegister.ReportLayout == EnumReportLayout.DateWise)
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT ImportInvoiceID, BUID, ImportInvoiceNo, DateofInvoice, InvoiceStatus, SupplierName, ApprovedByName, DateofApplication, DateofAcceptance, DateofMaturity, Remarks, ImportLCNo, InvoiceAmount, BLNo, BLDate, ImportLCDate, MasterLCNos   FROM View_ImportInvoiceRegister ";
                sGroupBy = " GROUP BY ImportInvoiceID, BUID, ImportInvoiceNo, DateofInvoice, InvoiceStatus, SupplierName, ApprovedByName, DateofApplication, DateofAcceptance, DateofMaturity, Remarks, ImportLCNo,  InvoiceAmount,BLNo,BLDate, ImportLCDate, MasterLCNos";
                sOrderBy = " ORDER BY DateofInvoice ASC";
            }
            else if (oImportInvoiceRegister.ReportLayout == EnumReportLayout.DateWiseDetails)
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT * FROM View_ImportInvoiceRegister ";
                sOrderBy = " ORDER BY  DateofInvoice, ImportInvoiceID, ImportInvoiceDetailID ASC";
            }
            else if (oImportInvoiceRegister.ReportLayout == EnumReportLayout.PartyWise)
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT ImportInvoiceID, BUID, ImportInvoiceNo, DateofInvoice, InvoiceStatus, SupplierName, ApprovedByName, DateofApplication, DateofAcceptance, DateofMaturity, Remarks, ImportLCNo, BLNo, BLDate, ImportLCDate, InvoiceAmount, FileNo   FROM View_ImportInvoiceRegister ";//MasterLCNos
                sGroupBy = "GROUP BY ImportInvoiceID, BUID, ImportInvoiceNo, DateofInvoice, InvoiceStatus, SupplierName, ApprovedByName, DateofApplication, DateofAcceptance, DateofMaturity, Remarks, ImportLCNo, InvoiceAmount, BLNo, BLDate, ImportLCDate, FileNo";
                sOrderBy = " ORDER BY SupplierName ASC";
            }
            else if (oImportInvoiceRegister.ReportLayout == EnumReportLayout.PartyWiseDetails)
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT * FROM View_ImportInvoiceRegister ";
                sOrderBy = " ORDER BY  SupplierName, ImportInvoiceID, ImportInvoiceDetailID ASC";
            }
            else if (oImportInvoiceRegister.ReportLayout == EnumReportLayout.ProductWise)
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT * FROM View_ImportInvoiceRegister ";
                sOrderBy = " ORDER BY  ProductName ASC";
            }
            else
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT * FROM View_ImportInvoiceRegister ";
                sOrderBy = " ORDER BY DateofInvoice, ImportInvoiceID, ImportInvoiceDetailID ASC";
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

