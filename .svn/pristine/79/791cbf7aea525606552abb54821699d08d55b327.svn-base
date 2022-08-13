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
    public class ExportLCRegisterController : Controller
    {
        string _sDateRange ="";
        int _nReportLayout = 0;
        string _sErrorMesage = "";
        string sExportLCType = "";
        List<ExportLCRegister> _oExportLCRegisters = new List<ExportLCRegister>();

        public ActionResult ViewExportLCRegisters(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            ExportLCRegister oExportLCRegister = new ExportLCRegister();

            BusinessUnit oBusinessUnit = new BusinessUnit();
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            if (buid > 0)
            {
                oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oBusinessUnits.Add(oBusinessUnit);
            }
            else
            {
                oBusinessUnits = BusinessUnit.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            #region Approval User
            string sSQL = "SELECT * FROM View_User AS HH WHERE HH.UserID IN (SELECT DISTINCT MM.DBServerUserID FROM ExportLC AS MM WHERE ISNULL(MM.DBServerUserID,0)!=0) ORDER BY HH.UserName";
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
                if ((EnumReportLayout)oItem.id == EnumReportLayout.ProductWise || (EnumReportLayout)oItem.id == EnumReportLayout.LCWithAmendment|| (EnumReportLayout)oItem.id == EnumReportLayout.PartyWise || (EnumReportLayout)oItem.id == EnumReportLayout.BankWise)
                {
                    oReportLayouts.Add(oItem);
                }
            }

            ViewBag.ReportLayouts = oReportLayouts;
            #endregion
            ViewBag.BankBranchs = BankBranch.GetsByDeptAndBU(((int)EnumOperationalDept.Export_Own).ToString(), buid, "", ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.Currencys = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.LCTerms = LCTerm.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.LCStatus = EnumObject.jGets(typeof(EnumExportLCStatus));
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.PaymentInstructions = EnumObject.jGets(typeof(EnumPaymentInstruction));
            ViewBag.NotifyBy = EnumObject.jGets(typeof(EnumNotifyBy));
            ViewBag.BUID = buid;
            ViewBag.BusinessUnits = oBusinessUnits;
            ViewBag.LCTypes = EnumObject.jGets(typeof(EnumExportLCType));
            return View(oExportLCRegister);
        }
        [HttpPost]
        public ActionResult SetSessionSearchCriteria(ExportLCRegister oExportLCRegister)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oExportLCRegister);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PrintExportLCRegister(double ts)
        {
            ExportLCRegister oExportLCRegister = new ExportLCRegister();
            try
            {
                _sErrorMesage = "";
                _oExportLCRegisters = new List<ExportLCRegister>();
                oExportLCRegister = (ExportLCRegister)Session[SessionInfo.ParamObj];
                try
                {
                    string sSQL = GetSQL(oExportLCRegister.ErrorMessage,true,false);
                    _oExportLCRegisters = ExportLCRegister.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                catch (Exception ex)
                {
                    _sErrorMesage = ex.Message;
                }

                if (_oExportLCRegisters.Count <= 0 && string.IsNullOrEmpty(_sErrorMesage))
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oExportLCRegisters = new List<ExportLCRegister>();
                _sErrorMesage = ex.Message;
            }

            if (_sErrorMesage == "")
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                BusinessUnit oBU = new BusinessUnit();

                int nBUID = Convert.ToInt32(oExportLCRegister.ErrorMessage.Split('~')[22]);
                oBU = oBU.Get(nBUID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBU);

                rptExportLCRegisters oReport = new rptExportLCRegisters();
                byte[] abytes = oReport.PrepareReport(_oExportLCRegisters, oCompany, _nReportLayout, _sDateRange,sExportLCType);
                return File(abytes, "application/pdf");
            }
            else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport(_sErrorMesage);
                return File(abytes, "application/pdf");
            }
        }

        public ActionResult PrintExportLCRegisterSummary(double ts)
        {
            ExportLCRegister oExportLCRegister = new ExportLCRegister();
            try
            {
                _sErrorMesage = "";
                _oExportLCRegisters = new List<ExportLCRegister>();
                oExportLCRegister = (ExportLCRegister)Session[SessionInfo.ParamObj];
                try
                {
                    string sSQL = GetSQL(oExportLCRegister.ErrorMessage, true, true);
                    _oExportLCRegisters = ExportLCRegister.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                catch (Exception ex)
                {
                    _sErrorMesage = ex.Message;
                }

                if (_oExportLCRegisters.Count <= 0 && string.IsNullOrEmpty(_sErrorMesage))
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oExportLCRegisters = new List<ExportLCRegister>();
                _sErrorMesage = ex.Message;
            }

            if (_sErrorMesage == "")
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                BusinessUnit oBU = new BusinessUnit();

                int nBUID = Convert.ToInt32(oExportLCRegister.ErrorMessage.Split('~')[22]);
                oBU = oBU.Get(nBUID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBU);

                rptExportLCRegisters oReport = new rptExportLCRegisters();
                byte[] abytes = oReport.PrepareReportSUmmary(_oExportLCRegisters, oCompany, _nReportLayout, _sDateRange,sExportLCType);
                return File(abytes, "application/pdf");
            }
            else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport(_sErrorMesage);
                return File(abytes, "application/pdf");
            }
        }
        public void Print_ExportLCRegisterXL(double ts)
        {
            ExportLCRegister oExportLCRegister = new ExportLCRegister();
            try
            {
                _sErrorMesage = "";
                _oExportLCRegisters = new List<ExportLCRegister>();
                oExportLCRegister = (ExportLCRegister)Session[SessionInfo.ParamObj];

                string sSQL = GetSQL(oExportLCRegister.ErrorMessage,false,false);
                _oExportLCRegisters = ExportLCRegister.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            
                if (_oExportLCRegisters.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oExportLCRegisters = new List<ExportLCRegister>();
                _sErrorMesage = ex.Message;
            }

            if (_sErrorMesage == "")
            {
                int nBUID = Convert.ToInt32(oExportLCRegister.ErrorMessage.Split('~')[22]);
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                BusinessUnit oBU = new BusinessUnit();
                oBU = oBU.Get(nBUID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBU);

                #region Print XL

                int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 17, nColumn = 1, nCount = 0, nExportLCID = 0;
                ExcelRange cell;
                ExcelRange HeaderCell;
                ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("ExportLCRegister");
                    sheet.Name = "Export L/C Register";
                    sheet.Column(++nColumn).Width = 10; //SL No
                    sheet.Column(++nColumn).Width = 30; //ApplicantName
                    sheet.Column(++nColumn).Width = 15; //LC No
                    sheet.Column(++nColumn).Width = 15; //PINo
                    sheet.Column(++nColumn).Width = 30; //Negotiation Bank
                    sheet.Column(++nColumn).Width = 35; //ProductName
                    sheet.Column(++nColumn).Width = 15; //Qty
                    sheet.Column(++nColumn).Width = 15; //Value
                    sheet.Column(++nColumn).Width = 15; //ShipmentDate
                    sheet.Column(++nColumn).Width = 15; //ExpireDate
                    sheet.Column(++nColumn).Width = 20; //LC Status
                    sheet.Column(++nColumn).Width = 10; //Acc
                    sheet.Column(++nColumn).Width = 10; //ACC
                    sheet.Column(++nColumn).Width = 10; //UD
                    sheet.Column(++nColumn).Width = 15; //DOC Send To Bank
                    sheet.Column(++nColumn).Width = 15; //C/P

                    //   nEndCol = 17;

                    #region Report Header
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 8]; cell.Merge = true;
                    cell.Value = "TEST"+oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, nStartCol + 9, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = "ExportLC Register"; cell.Style.Font.Bold = true;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Address & Date
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 8]; cell.Merge = true;
                    cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        
                    cell = sheet.Cells[nRowIndex, nStartCol + 9, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = _sDateRange; cell.Style.Font.Bold = false;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    nRowIndex = nRowIndex + 1;

                    if (sExportLCType != "")
                    {
                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 8]; cell.Merge = true;
                        cell.Value = "Export LC Type: "+sExportLCType; cell.Style.Font.Bold = false;
                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }
                    nRowIndex++;
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

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Buyer Name"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "LC No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "L/C Value"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Negotiation Bank"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "PI No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Commodity"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Qty"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Value"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Shipment Date"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Expire Date"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "LC Status"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Acc"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Acc"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "UD"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Doc Send To Bank"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "C/P"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nRowIndex++;
                    #endregion
                    double nValue = 0;
                    #region Data
                    int nImportPIID = 0; double nGrandTotalQty = 0, nGrandTotalAmount = 0;
                    foreach (ExportLCRegister oItem in _oExportLCRegisters)
                    {
                        nColumn = 1;
                        if (nExportLCID != oItem.ExportLCID)
                        {
                            nCount++;
                            int nRowSpan = _oExportLCRegisters.Where(PIR => PIR.ExportLCID == oItem.ExportLCID).ToList().Count-1;
                            cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex + nRowSpan, nColumn]; cell.Value = nCount; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            
                            cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex + nRowSpan, nColumn]; cell.Value = oItem.ApplicantName.ToString(); cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex + nRowSpan, nColumn]; cell.Value = oItem.LCNo.ToString(); cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                             nValue = _oExportLCRegisters.Where(PIR => PIR.ExportLCID == oItem.ExportLCID).ToList().Select(c => c.Qty * c.UnitPrice).Sum();
                             cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex + nRowSpan, nColumn]; cell.Value = nValue; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            cell.Style.Numberformat.Format = oItem.Currency+ "#,##0.00";
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex + nRowSpan, nColumn]; cell.Value = oItem.NegoBankName; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        }

                        nColumn = 6;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.PINo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.Qty; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "# #,##0.00";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.Qty*oItem.UnitPrice; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "$ #,##0.00";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.ShipmentDateSt; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.ExpiryDateSt; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value ="--"; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.Acc_Party+ " Part"; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.Acc_Bank + " Part"; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        string sUDRcv = (oItem.UDRcvType == EnumUDRcvType.No_Receive ? "No" : "Yes");
                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = sUDRcv; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.NoteQuery; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.MKTPersonName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        
                        nExportLCID = oItem.ExportLCID;
                        nGrandTotalQty = nGrandTotalQty + oItem.Qty;
                        nGrandTotalAmount = nGrandTotalAmount + oItem.Amount;
                        nEndRow = nRowIndex;
                        nRowIndex+=1;
                    }
                    #endregion

                    #region Total
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 10]; cell.Value = "" + "Total"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Merge = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                     nValue = _oExportLCRegisters.Select(c => c.Qty).Sum();
                    cell = sheet.Cells[nRowIndex, nEndCol - 9]; cell.Value = nValue; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "# #,##0.00";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nValue = _oExportLCRegisters.Select(c => c.LCValue).Sum();
                    cell = sheet.Cells[nRowIndex, nEndCol - 8]; cell.Value = nValue; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "$ #,##0.00";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    for (int i = 7; i >= 0; i--) 
                    {
                        cell = sheet.Cells[nRowIndex, nEndCol-i]; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    
                    nRowIndex = nRowIndex + 1;
                    #endregion
                    #endregion

                    cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=ExportLCRegister.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion

            }
            else
            {
                #region Print XL

                #endregion
            }
        }
        
        #region Support Functions
        private string GetSQL(string sTemp, bool isPDF,bool isSummary)
        {
            string sReturn1 = "SELECT ExportPIID,PINo,PIDate,SUM(LCValue) AS LCValue,BuyerID,MKTEmpID,PIStatus,VersionNo,LCOpenDate,AmendmentDate,LCReceiveDate,UDRecDate,UDRcvType,ExportLCID,LCNo,ApplicantID,LCStatus,NegoBankBranchID,IssueBankBranchID,NoteQuery,NoteUD,HaveQuery,GetOriginalCopy,Currency,ApplicantName,BuyerName,MKTPersonName,NegoBankName,IssueBankName,SUM(Value_DO) AS Value_DO,SUM(Value_DC) AS Value_DC,SUM(Qty_Invoice*UnitPrice) AS Value_Invoice,ProductName,SUM(Qty) AS Qty,Avg(UnitPrice) AS UnitPrice,Acc_Bank,Acc_Party,ShipmentDate,[ExpiryDate],ExportLCType "
                               +"FROM View_ExportLCRegister ";

            if(isSummary)
            {
                sReturn1 = " SELECT  Count(Distinct(ExportLCID)) as Qty, ApplicantName,SUM(Qty*UnitPrice) AS LCValue,ApplicantID,SUM(Value_Invoice) AS Qty_Invoice,Currency FROM View_ExportLCRegister";
            }

            string sReturn = "";

            if (!string.IsNullOrEmpty(sTemp))
            {
                #region Set Values

                string sExportPINo = Convert.ToString(sTemp.Split('~')[0]);
                string sFileNo = Convert.ToString(sTemp.Split('~')[1]);
                string sContractorIDs = Convert.ToString(sTemp.Split('~')[2]);
                int nBankBranch_Nego = Convert.ToInt32(sTemp.Split('~')[3]);
                int nBankBranch_Advise = Convert.ToInt32(sTemp.Split('~')[4]);
                string sBBranchIDs_IssueBank = Convert.ToString(sTemp.Split('~')[5]);

                int nCboLCOpenDate = Convert.ToInt32(sTemp.Split('~')[6]);
                DateTime dFromLCOpenDate = DateTime.Now;
                DateTime dToLCOpenDate = DateTime.Now;
                if (nCboLCOpenDate > 0)
                {
                    dFromLCOpenDate = Convert.ToDateTime(sTemp.Split('~')[7]);
                    dToLCOpenDate = Convert.ToDateTime(sTemp.Split('~')[8]);
                }

                int ncboReceiveDate = Convert.ToInt32(sTemp.Split('~')[9]);
                DateTime dFromLCReceiveDate = DateTime.Now;
                DateTime dToLCReceiveDate = DateTime.Now;
                if (ncboReceiveDate > 0)
                {
                    dFromLCReceiveDate = Convert.ToDateTime(sTemp.Split('~')[10]);
                    dToLCReceiveDate = Convert.ToDateTime(sTemp.Split('~')[11]);
                }

                int nCboShipmentDate = Convert.ToInt32(sTemp.Split('~')[12]);
                DateTime dFromShipmentDate = DateTime.Now;
                DateTime dToShipmentDate = DateTime.Now;
                if (nCboShipmentDate > 0)
                {
                    dFromShipmentDate = Convert.ToDateTime(sTemp.Split('~')[13]);
                    dToShipmentDate = Convert.ToDateTime(sTemp.Split('~')[14]);
                }

                int nCboExpireDate = Convert.ToInt32(sTemp.Split('~')[15]);
                DateTime dFromExpireDate = DateTime.Now;
                DateTime dToExpireDate = DateTime.Now;
                if (nCboExpireDate > 0)
                {
                    dFromExpireDate = Convert.ToDateTime(sTemp.Split('~')[16]);
                    dToExpireDate = Convert.ToDateTime(sTemp.Split('~')[17]);
                }

                int ncboAmendmentDate = Convert.ToInt32(sTemp.Split('~')[18]);
                DateTime dFromAmendmentDate = DateTime.Now;
                DateTime dToAmendmentDate = DateTime.Now;
                if (ncboAmendmentDate > 0)
                {
                    dFromAmendmentDate = Convert.ToDateTime(sTemp.Split('~')[19]);
                    dToAmendmentDate = Convert.ToDateTime(sTemp.Split('~')[20]);
                }
                string sLCStatus = Convert.ToString(sTemp.Split('~')[21]);
                int nBUID = Convert.ToInt32(sTemp.Split('~')[22]);

                int nUDRecd = Convert.ToInt32(sTemp.Split('~')[23]);
                int ncboHaveQuery = Convert.ToInt32(sTemp.Split('~')[24]);
                int ncboGetOriginalCopy = Convert.ToInt32(sTemp.Split('~')[25]);
                bool bIsExportDoIsntCreateYet = Convert.ToBoolean(sTemp.Split('~')[26]);
                bool bDeliveryChallanIssueButBillNotCreated = Convert.ToBoolean(sTemp.Split('~')[27]);
                string  sLCNo = Convert.ToString(sTemp.Split('~')[28]);
                int nReportLayout = Convert.ToInt32(sTemp.Split('~')[29]);
                string sBuyerIDs = Convert.ToString(sTemp.Split('~')[30]);
                string sDeliveryToIDs = Convert.ToString(sTemp.Split('~')[31]);
                int nExportLC= Convert.ToInt32(sTemp.Split('~')[32]);
                if (nExportLC == 0)
                {
                    sExportLCType = "";
                }
                

                _nReportLayout = nReportLayout;

                #endregion

                #region Make Query

                #region Export PI NO
                if (!string.IsNullOrEmpty(sExportPINo))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ExportLCID IN (SELECT EPI.LCID FROM ExportPI AS EPI WHERE EPI.PINo LIKE '%" + sExportPINo + "%') ";
                }
                #endregion

                #region FileNo
                //if (!string.IsNullOrEmpty(sFileNo))
                //{
                //    Global.TagSQL(ref sReturn);
                //    sReturn = sReturn + " FileNo = '" + sFileNo + "' ";
                //}
                #endregion

                #region Applicant
                if (!String.IsNullOrEmpty(sContractorIDs))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ApplicantID in( " + sContractorIDs + ") ";
                }
                #endregion
                #region Buyer
                if (!String.IsNullOrEmpty(sBuyerIDs))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " BuyerID in( " + sBuyerIDs + ") ";
                }
                #endregion
                #region DeliveryTo
                if (!String.IsNullOrEmpty(sDeliveryToIDs))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " DeliveryToID  in( " + sDeliveryToIDs + ") ";
                }
                #endregion

                #region BankBranch_Nego
                if (nBankBranch_Nego > 0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " NegoBankBranchID =" + nBankBranch_Nego;
                }
                #endregion

                #region BankBranch_Advise
                //if (nBankBranch_Advise > 0)
                //{
                //    Global.TagSQL(ref sReturn);
                //    sReturn = sReturn + " BankBranchID_Advice =" + nBankBranch_Advise;
                //}
                #endregion

                #region Issue Bank
                //if (!String.IsNullOrEmpty(sBBranchIDs_IssueBank))
                //{
                //    Global.TagSQL(ref sReturn);
                //    sReturn = sReturn + " BankBranchID_Issue in(" + sBBranchIDs_IssueBank + ") ";
                //}
                #endregion


                #region LC Open Date
                if (nCboLCOpenDate != (int)EnumCompareOperator.None)
                {
                    Global.TagSQL(ref sReturn);
                    if (nCboLCOpenDate == (int)EnumCompareOperator.EqualTo)
                    {
                        _sDateRange = "LC Date : "+dFromLCOpenDate.ToString("dd MMM yyyy");
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),LCOpenDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromLCOpenDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (nCboLCOpenDate == (int)EnumCompareOperator.NotEqualTo)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),LCOpenDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromLCOpenDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (nCboLCOpenDate == (int)EnumCompareOperator.GreaterThan)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),LCOpenDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromLCOpenDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (nCboLCOpenDate == (int)EnumCompareOperator.SmallerThan)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),LCOpenDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromLCOpenDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (nCboLCOpenDate == (int)EnumCompareOperator.Between)
                    {
                        _sDateRange = "LC Date  " + dFromLCOpenDate.ToString("dd MMM yyyy") + " to " + dToLCOpenDate.ToString("dd MMM yyyy");
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),LCOpenDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromLCOpenDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToLCOpenDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (nCboLCOpenDate == (int)EnumCompareOperator.NotBetween)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),LCOpenDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromLCOpenDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToLCOpenDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                }
                #endregion
                #region L/C Receive Date
                if (ncboReceiveDate != (int)EnumCompareOperator.None)
                {
                    Global.TagSQL(ref sReturn);
                    if (ncboReceiveDate == (int)EnumCompareOperator.EqualTo)
                    {
                        _sDateRange = "L/C Receive Date " + dFromLCOpenDate.ToString("dd MMM yyyy");
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),LCReceiveDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromLCReceiveDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (ncboReceiveDate == (int)EnumCompareOperator.NotEqualTo)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),LCReceiveDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromLCReceiveDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (ncboReceiveDate == (int)EnumCompareOperator.GreaterThan)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),LCReceiveDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromLCReceiveDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (ncboReceiveDate == (int)EnumCompareOperator.SmallerThan)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),LCReceiveDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromLCReceiveDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (ncboReceiveDate == (int)EnumCompareOperator.Between)
                    {
                        _sDateRange = "L/C Receive Date  " + dFromLCOpenDate.ToString("dd MMM yyyy") + " to " + dToLCOpenDate.ToString("dd MMM yyyy");
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),LCReceiveDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromLCReceiveDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToLCReceiveDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (ncboReceiveDate == (int)EnumCompareOperator.NotBetween)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),LCReceiveDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromLCReceiveDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToLCReceiveDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                }
                #endregion
                //#region Shipment Date
                //if (nCboShipmentDate != (int)EnumCompareOperator.None)
                //{
                //    Global.TagSQL(ref sReturn);
                //    if (ncboReceiveDate == (int)EnumCompareOperator.EqualTo)
                //    {
                //        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ShipmentDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromShipmentDate.ToString("dd MMM yyyy") + "',106)) ";
                //    }
                //    else if (ncboReceiveDate == (int)EnumCompareOperator.NotEqualTo)
                //    {
                //        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ShipmentDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromShipmentDate.ToString("dd MMM yyyy") + "',106)) ";
                //    }
                //    else if (ncboReceiveDate == (int)EnumCompareOperator.GreaterThen)
                //    {
                //        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ShipmentDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromShipmentDate.ToString("dd MMM yyyy") + "',106)) ";
                //    }
                //    else if (ncboReceiveDate == (int)EnumCompareOperator.SmallerThen)
                //    {
                //        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ShipmentDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromShipmentDate.ToString("dd MMM yyyy") + "',106)) ";
                //    }
                //    else if (ncboReceiveDate == (int)EnumCompareOperator.Between)
                //    {
                //        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ShipmentDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromShipmentDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToShipmentDate.ToString("dd MMM yyyy") + "',106)) ";
                //    }
                //    else if (ncboReceiveDate == (int)EnumCompareOperator.NotBetween)
                //    {
                //        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ShipmentDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromShipmentDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToShipmentDate.ToString("dd MMM yyyy") + "',106)) ";
                //    }
                //}
                //#endregion
                //#region Expire Date
                //if (nCboExpireDate != (int)EnumCompareOperator.None)
                //{
                //    Global.TagSQL(ref sReturn);
                //    if (nCboExpireDate == (int)EnumCompareOperator.EqualTo)
                //    {
                //        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ExpireDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromExpireDate.ToString("dd MMM yyyy") + "',106)) ";
                //    }
                //    else if (nCboExpireDate == (int)EnumCompareOperator.NotEqualTo)
                //    {
                //        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ExpireDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromExpireDate.ToString("dd MMM yyyy") + "',106)) ";
                //    }
                //    else if (nCboExpireDate == (int)EnumCompareOperator.GreaterThen)
                //    {
                //        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ExpireDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromExpireDate.ToString("dd MMM yyyy") + "',106)) ";
                //    }
                //    else if (nCboExpireDate == (int)EnumCompareOperator.SmallerThen)
                //    {
                //        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ExpireDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromExpireDate.ToString("dd MMM yyyy") + "',106)) ";
                //    }
                //    else if (nCboExpireDate == (int)EnumCompareOperator.Between)
                //    {
                //        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ExpireDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromExpireDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToExpireDate.ToString("dd MMM yyyy") + "',106)) ";
                //    }
                //    else if (nCboExpireDate == (int)EnumCompareOperator.NotBetween)
                //    {
                //        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ExpireDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromExpireDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToExpireDate.ToString("dd MMM yyyy") + "',106)) ";
                //    }
                //}
                //#endregion

                #region Amendment Date
                if (ncboAmendmentDate != (int)EnumCompareOperator.None)
                {
                    Global.TagSQL(ref sReturn);

                    if (ncboAmendmentDate == (int)EnumCompareOperator.EqualTo)
                    {
                        sReturn = sReturn + "ExportLCID in (Select ExportLCID from ExportPILCMapping where Activity=1 and  CONVERT(DATE,CONVERT(VARCHAR(12),[Date] ,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromAmendmentDate.ToString("dd MMM yyyy") + "',106)) )";
                    }

                    else if (ncboAmendmentDate == (int)EnumCompareOperator.NotEqualTo)
                    {
                        sReturn = sReturn + "ExportLCID in (Select ExportLCID from ExportPILCMapping where Activity=1 and CONVERT(DATE,CONVERT(VARCHAR(12),[Date] ,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromAmendmentDate.ToString("dd MMM yyyy") + "',106)) )";
                    }
                    else if (ncboAmendmentDate == (int)EnumCompareOperator.GreaterThan)
                    {
                        sReturn = sReturn + "ExportLCID in (Select ExportLCID from ExportPILCMapping where Activity=1 and CONVERT(DATE,CONVERT(VARCHAR(12),[Date] ,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromAmendmentDate.ToString("dd MMM yyyy") + "',106))  )";
                    }
                    else if (ncboAmendmentDate == (int)EnumCompareOperator.SmallerThan)
                    {
                        sReturn = sReturn + "ExportLCID in (Select ExportLCID from ExportPILCMapping where Activity=1 and CONVERT(DATE,CONVERT(VARCHAR(12),[Date] ,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromAmendmentDate.ToString("dd MMM yyyy") + "',106))  )";
                    }
                    else if (ncboAmendmentDate == (int)EnumCompareOperator.Between)
                    {
                        sReturn = sReturn + "ExportLCID in (Select ExportLCID from ExportPILCMapping where Activity=1 and CONVERT(DATE,CONVERT(VARCHAR(12),[Date] ,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromAmendmentDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToAmendmentDate.ToString("dd MMM yyyy") + "',106))  )";
                    }
                    else if (ncboAmendmentDate == (int)EnumCompareOperator.NotBetween)
                    {
                        sReturn = sReturn + "ExportLCID in (Select ExportLCID from ExportPILCMapping where Activity=1 and CONVERT(DATE,CONVERT(VARCHAR(12),[Date] ,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromAmendmentDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToAmendmentDate.ToString("dd MMM yyyy") + "',106))  )";
                    }

                }
                #endregion

                #region Get Original Copy
                if (ncboGetOriginalCopy > 0)
                {
                    Global.TagSQL(ref sReturn);
                    if (ncboGetOriginalCopy == 1)
                    {
                        sReturn = sReturn + " GetOriginalCopy = 0 "; // Yet Not Receive
                    }
                    if (ncboGetOriginalCopy == 2)
                    {
                        sReturn = sReturn + " GetOriginalCopy = 1 ";// Receive
                    }
                }
                #endregion

                #region Get Export LC Type
                if (nExportLC > 0)
                {
                    Global.TagSQL(ref sReturn);
                    if (nExportLC == 1)
                    {
                        sExportLCType = "LC";
                        sReturn = sReturn + " ExportLCType = 1 "; // LC
                    }
                    if (nExportLC == 2)
                    {
                        sExportLCType = "FDD";
                        sReturn = sReturn + " ExportLCType = 2 ";// FDD
                    }
                    if (nExportLC == 3)
                    {
                        sExportLCType = "TT";
                        sReturn = sReturn + " ExportLCType = 3 ";// TT
                    }
                }
                #endregion

                #region HaveQuery
                if (ncboHaveQuery > 0)
                {
                    Global.TagSQL(ref sReturn);
                    if (ncboHaveQuery == 1)
                    {
                        sReturn = sReturn + " HaveQuery = 1 "; // Yes
                    }
                    if (ncboHaveQuery == 2)
                    {
                        sReturn = sReturn + " HaveQuery = 0 ";// No
                    }
                }
                #endregion
                #region UD Receive
                if (nUDRecd > 0)
                {
                    Global.TagSQL(ref sReturn);
                    if (nUDRecd == 1)
                    {
                        sReturn = sReturn + " ExportLCID in (Select ExportLCID from ExportPILCMapping where activity=1 and isnull(UDRcvType,0)=0  )"; // Yet Not Receive
                    }
                    if (nUDRecd == 2)
                    {
                        sReturn = sReturn + "ExportLCID in (Select ExportLCID from ExportPILCMapping where activity=1 and isnull(UDRcvType,0)>0  ) ";// Receive
                    }
                }
                #endregion

                #region Business Unit
                if (nBUID > 0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " BUID = " + nBUID + " ";
                }
                #endregion

                #region Export Do Isn't Create Yet
                if (bIsExportDoIsntCreateYet)
                {
                    Global.TagSQL(ref sReturn);

                    if (nBUID == (int)EnumBusinessUnitType.Integrated)
                    {
                        sReturn = sReturn + " ExportLCID IN (SELECT LCID FROM ExportPI WHERE ExportPIID NOT IN (SELECT ExportPIID FROM SUDeliveryOrder WHERE DOType = " + (int)EnumDOType.Export + ")) ";
                    }
                    else if (nBUID == (int)EnumBusinessUnitType.Plastic)
                    {
                        sReturn = sReturn + " ExportLCID IN (SELECT LCID FROM ExportPI WHERE ExportPIID NOT IN (SELECT ExportPIID FROM FabricDeliveryOrder)) ";
                    }
                    else if (nBUID == (int)EnumBusinessUnitType.None)
                    {
                        sReturn = sReturn + " ExportLCID IN (SELECT LCID FROM ExportPI WHERE ExportPIID NOT IN (SELECT ExportPIID FROM FabricDeliveryOrder)) AND ExportLCID IN (SELECT LCID FROM ExportPI WHERE ExportPIID NOT IN (SELECT ExportPIID FROM SUDeliveryOrder WHERE DOType = " + (int)EnumDOType.Export + "))";
                    }
                }
                #endregion

                #region Status Wise
                if (!string.IsNullOrEmpty(sLCStatus))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " LCStatus IN (" + sLCStatus + ") ";
                }
                #endregion

                #region Delivery Challan Issue But Bill Not Created
                if (bDeliveryChallanIssueButBillNotCreated)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ExportLCID IN (SELECT LCID FROM ExportPI WHERE ExportPIID IN (SELECT ExportPIID FROM SUDeliveryOrderDetail WHERE SUDeliveryOrderDetailID IN (SELECT SUDeliveryOrderDetailID FROM SUDeliveryChallanDetail))) AND ExportLCID NOT IN (SELECT ExportLCID FROM ExportBill) ";
                }
                #endregion

                #endregion

            }
             string sOrderBy = "";
            if (isSummary)
            {
                sReturn = sReturn1 + sReturn + " GROUP BY ApplicantID,ApplicantName,Currency ";
        
                sOrderBy = " Order By LCValue DESC";
            }
            else
            {
                sReturn = sReturn1 + sReturn + " GROUP BY ProductName,ExportPIID,PINo,BuyerID,PIDate,BuyerID,MKTEmpID,UDRcvType,ExportLCID,ApplicantID,NegoBankBranchID,IssueBankBranchID,NoteQuery,NoteUD,HaveQuery,GetOriginalCopy,Currency,ApplicantName,BuyerName,MKTPersonName,NegoBankName,IssueBankName,PIStatus,VersionNo,LCOpenDate,AmendmentDate,LCReceiveDate,UDRecDate,LCNo,LCStatus,Acc_Bank,Acc_Party,ShipmentDate,[ExpiryDate],ExportLCType ";
        
                if (_nReportLayout == (int)EnumReportLayout.PartyWise)
                    sOrderBy = "Order By ApplicantName,ApplicantID,ExportLCID,ProductName,NegoBankName";
                else if (_nReportLayout == (int)EnumReportLayout.BankWise)
                    sOrderBy = "Order By NegoBankName,NegoBankBranchID,ExportLCID,ProductName,ApplicantName";
                else if (_nReportLayout == (int)EnumReportLayout.ProductWise)
                    sOrderBy = "Order By ProductName,ExportLCID,ApplicantName,NegoBankName";
                else if (_nReportLayout == (int)EnumReportLayout.LCWithAmendment)
                    sOrderBy = "ORDER BY AmendmentDate, ExportLCID,ExportPIID ASC";
            }
            
            if(isPDF)
                sReturn += sOrderBy;
            else
                sReturn += "Order By ExportLCID,ProductName,ApplicantName,NegoBankName";

            return sReturn;
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

