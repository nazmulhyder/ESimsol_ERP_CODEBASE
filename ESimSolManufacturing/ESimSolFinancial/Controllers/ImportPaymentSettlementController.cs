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
    public class ImportPaymentSettlementController : Controller
    {
        #region Declaration
        ImportPaymentSettlement _oImportPaymentSettlement = new ImportPaymentSettlement();
        List<ImportPaymentSettlement> _oImportPaymentSettlements = new List<ImportPaymentSettlement>();
        List<ImportInvoice> _oImportInvoices = new List<ImportInvoice>();
        #endregion

        #region Actions
        public ActionResult ViewImportPaymentSettlement(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.ImportPayment).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            _oImportPaymentSettlements = new List<ImportPaymentSettlement>();
            _oImportPaymentSettlements = ImportPaymentSettlement.Gets(buid, (int)EnumInvoiceBankStatus.Payment_Request, (int)Session[SessionInfo.currentUserID]);


            #region Get User
            string sSQL = "SELECT * FROM Users AS HH WHERE HH.UserID IN (SELECT NN.ApprovedBy FROM View_ImportPayment AS NN WHERE NN.BUID=" + buid.ToString() + ") ORDER BY UserName ASC";
            List<User> oApprovedUsers = new List<ESimSol.BusinessObjects.User>();
            ESimSol.BusinessObjects.User oApprovedUser = new ESimSol.BusinessObjects.User();
            oApprovedUser.UserID = 0; oApprovedUser.UserName = "--Select Approved User--";
            oApprovedUsers.Add(oApprovedUser);
            oApprovedUsers.AddRange(ESimSol.BusinessObjects.User.GetsBySql(sSQL, (int)Session[SessionInfo.currentUserID]));
            #endregion

            ViewBag.ApprovedUsers = oApprovedUsers;
            ViewBag.CompareOperatorObjs = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.LiabilityTypes = EnumObject.jGets(typeof(EnumLiabilityType));
            ViewBag.ForExGainLoss = EnumObject.jGets(typeof(EnumForExGainLoss));
            ViewBag.BankAccounts = BankAccount.GetsByDeptAndBU(((int)EnumOperationalDept.Import_Own).ToString(), buid, (int)Session[SessionInfo.currentUserID]);
            sSQL = "SELECT * FROM View_ChartsOfAccount AS HH WHERE HH.AccountHeadID IN (SELECT MM.AccountHeadID FROM ExpenditureHead AS MM WHERE MM.ExpenditureHeadID IN((SELECT HH.ExpenditureHeadID FROM ExpenditureHeadMapping AS HH WHERE HH.OperationType=" + ((int)EnumExpenditureType.ImportInvoice).ToString() + "))) ORDER BY AccountHeadName ASC";
            ViewBag.AccountHeads = ChartsOfAccount.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            List<EnumObject> oImportPITypes = new List<EnumObject>();
            List<EnumObject> oEnums = EnumObject.jGets(typeof(EnumImportPIType));
            foreach (var oObj in oEnums)
            {
                if (oObj.id == (int)EnumImportPIType.Foreign || oObj.id == (int)EnumImportPIType.NonLC || oObj.id == (int)EnumImportPIType.TTForeign || oObj.id == (int)EnumImportPIType.TTLocal)
                {
                    oImportPITypes.Add(oObj);
                }
            }
            ViewBag.ImportPITypeObj = oImportPITypes;
            return View(_oImportPaymentSettlements);
        }
        public ActionResult ViewPaymentDoneByLiability(int id, decimal ts)
        {
            ImportPayment oImportPayment = new ImportPayment();
            ImportInvoice oImportInvoice = new ImportInvoice();
            ImportInvoiceHistory oImportInvoiceHistory = new ImportInvoiceHistory();
            if (id > 0)
            {
                oImportInvoice = oImportInvoice.Get(id, (int)Session[SessionInfo.currentUserID]);
                oImportPayment = oImportPayment.GetBy(id, (int)Session[SessionInfo.currentUserID]);

                ImportPaymentRequest oImportPaymentRequest = new ImportPaymentRequest();
                oImportPaymentRequest = oImportPaymentRequest.GetByPInvoice(id, (int)Session[SessionInfo.currentUserID]);
                oImportInvoice.LiabilityType = oImportPaymentRequest.LiabilityType;
                oImportInvoice.LiabilityTypeInt = oImportPaymentRequest.LiabilityTypeInt;

                if (oImportPayment.ImportPaymentID <= 0)
                {
                    oImportPayment.BankAccountID = oImportPaymentRequest.BankAccountID;
                }
                oImportPayment.EHTransactions = EHTransaction.Gets(oImportPayment.ImportInvoiceID, EnumExpenditureType.ImportInvoice, (int)Session[SessionInfo.currentUserID]);
                Company oCompany = new Company();
                oImportInvoice.Company = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
                oImportInvoice.BankStatus = EnumInvoiceBankStatus.Payment_Done;
                oImportInvoice.Currencys = Currency.Gets((int)Session[SessionInfo.currentUserID]);
                oImportInvoice.CCRate = oImportInvoice.CRate_Acceptance;
            }
            ViewBag.ImportPayment = oImportPayment;
            ViewBag.ForExGainLoss = EnumObject.jGets(typeof(EnumForExGainLoss));
            ViewBag.BankAccounts = BankAccount.GetsByBankBranch(oImportInvoice.BankBranchID_Nego, (int)Session[SessionInfo.currentUserID]);
            string sSQL = "SELECT * FROM View_ChartsOfAccount AS HH WHERE HH.AccountHeadID IN (SELECT MM.AccountHeadID FROM ExpenditureHead AS MM WHERE MM.ExpenditureHeadID IN((SELECT HH.ExpenditureHeadID FROM ExpenditureHeadMapping AS HH WHERE HH.OperationType=" + ((int)EnumExpenditureType.ImportInvoice).ToString() + "))) ORDER BY AccountHeadName ASC";
            ViewBag.AccountHeads = ChartsOfAccount.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            return View(oImportInvoice);
        }

        [HttpPost]
        public JsonResult Save(ImportPayment oImportPayment)
        {
            try
            {
                _oImportPaymentSettlement = _oImportPaymentSettlement.Save(oImportPayment, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oImportPaymentSettlement = new ImportPaymentSettlement();
                _oImportPaymentSettlement.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oImportPaymentSettlement);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Approved(ImportPayment oImportPayment)
        {
            try
            {
                _oImportPaymentSettlement = _oImportPaymentSettlement.Approved(oImportPayment, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oImportPaymentSettlement = new ImportPaymentSettlement();
                _oImportPaymentSettlement.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oImportPaymentSettlement);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SetSessionData(ImportPaymentSettlement oImportPaymentSettlement)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oImportPaymentSettlement);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PrintList(double ts)
        {
            ImportPaymentSettlement oImportPaymentSettlement = new ImportPaymentSettlement();
            List<ImportPaymentSettlement> oImportPaymentSettlements = new List<ImportPaymentSettlement>();         
            try
            {
                oImportPaymentSettlement = (ImportPaymentSettlement)Session[SessionInfo.ParamObj];                
                string sSQL = "SELECT * FROM View_ImportPaymentsettlement AS HH WHERE HH.ImportInvoiceID IN (" + oImportPaymentSettlement.ErrorMessage + ") ORDER BY HH.ImportInvoiceID ASC";
                oImportPaymentSettlements = ImportPaymentSettlement.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oImportPaymentSettlement = new ImportPaymentSettlement();
                throw new Exception(oImportPaymentSettlement.ErrorMessage);
            }


            this.Session.Remove(SessionInfo.ParamObj);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            rptImportPaymentSettlements rptImportPaymentSettlement = new rptImportPaymentSettlements();
            byte[] abytes = rptImportPaymentSettlement.PrepareReport(oImportPaymentSettlements, oCompany);
            return File(abytes, "application/pdf");

        }
        #endregion

        public void PrintExcel(double ts)
        {
            Company oCompany = new Company();
            List<ImportPaymentSettlement> oImportPaymentSettlements = new List<ImportPaymentSettlement>();
            ImportPaymentSettlement oImportPaymentSettlement = new ImportPaymentSettlement();

            try
            {
                oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
                oImportPaymentSettlement = (ImportPaymentSettlement)Session[SessionInfo.ParamObj];
                string sSQL = "SELECT * FROM View_ImportPaymentsettlement AS HH WHERE HH.ImportInvoiceID IN (" + oImportPaymentSettlement.ErrorMessage + ") ORDER BY HH.ImportInvoiceID ASC";
                oImportPaymentSettlements = ImportPaymentSettlement.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

                if (oImportPaymentSettlements.Count() > 0)
                {
                    ExportToExcel(oImportPaymentSettlements, oCompany);
                }
                else
                {
                    throw new Exception(oImportPaymentSettlement.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                #region Errormessage
                ExcelRange cell;
                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Import_Payment_Settlement");
                    sheet.Name = "Import_Payment_Settlement";

                    cell = sheet.Cells[2, 2, 2, 5]; cell.Merge = true; cell.Value = ex.Message; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=Import_Payment_Settlement.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
        }

        private void ExportToExcel(List<ImportPaymentSettlement> oImportPaymentSettlements, Company oCompany)
        {
            int rowIndex = 2;
            int colIndex = 0;
            ExcelRange cell;
            Border border;
            ExcelFill fill;

            using (var excelPackage = new ExcelPackage())
            {

                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Import_Payment_Settlement");
                sheet.Name = "Import_Payment_Settlement";
                sheet.View.FreezePanes(4,1);

                #region Declare Column
                sheet.Column(++colIndex).Width = 8;  //SL
                sheet.Column(++colIndex).Width = 16;  //Ref No
                sheet.Column(++colIndex).Width = 28; //Invoice No
                sheet.Column(++colIndex).Width = 16; //Invoice Amount
                sheet.Column(++colIndex).Width = 18; //LC No
                sheet.Column(++colIndex).Width = 16; //Maturity Date
                sheet.Column(++colIndex).Width = 18; //Settlement Type
                sheet.Column(++colIndex).Width = 16; //Payment Date
                sheet.Column(++colIndex).Width = 20; //Loan No
                sheet.Column(++colIndex).Width = 16; //Loan Date
                sheet.Column(++colIndex).Width = 16; //Loan/Deduct Amt
                sheet.Column(++colIndex).Width = 20; //Approved By
                sheet.Column(++colIndex).Width = 12; //ForEx
                sheet.Column(++colIndex).Width = 30; //Bank Account
                sheet.Column(++colIndex).Width = 30; //Bank Name
                sheet.Column(++colIndex).Width = 30; //Branch Name

                #endregion
                #region Report Header
                cell = sheet.Cells[rowIndex, 2, rowIndex, 14]; cell.Merge = true; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex++;
                #endregion

                #region Column Header
                colIndex = 1;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex++]; cell.Merge = true; cell.Value = "SL"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Ref No"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Invoice No"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Invoice Amount"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex++]; cell.Merge = true; cell.Value = "LC No"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Maturity Date"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Settlement Type"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Payment Date"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Loan No"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Loan Date"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Loan/Deduct Amt"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Approved By"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex++]; cell.Merge = true; cell.Value = "ForEx"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Bank Account"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Bank Name"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Branch Name"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                rowIndex++;
                #endregion

                int nCount = 0;
                #region Report Body
                //double nGrandTotal = 0;
                var nStartRow = rowIndex;
                foreach (ImportPaymentSettlement oItem in oImportPaymentSettlements)
                {
                    colIndex = 1;
                    nCount++;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nCount.ToString();
                    fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.RefNo;
                    fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ImportInvoiceNo;
                    fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Amount; cell.Style.Numberformat.Format = "#,##,##0.00;(#,##,##0.00)";
                    fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                   // nGrandTotal = nGrandTotal + oItem.Amount;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ImportLCNo;
                    fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.DateofMaturityST;
                    fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.LiabilityTypeSt;
                    fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.PaymentDateSt;
                    fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.PMTLiabilityNo;
                    fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.PMTLoanOpenDateST;
                    fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.PMTAmount; cell.Style.Numberformat.Format = "#,##,##0.00;(#,##,##0.00)";
                    fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.PMTApprovedByName;
                    fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ForExGainLossSt;
                    fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.AccountNo;
                    fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.BankName;
                    fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.BranchName;
                    fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    rowIndex++;
                }
                var nEndRow = rowIndex - 1;
                cell = sheet.Cells[rowIndex, 1, rowIndex, 3]; cell.Merge = true; cell.Value = "Grand Total:"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                
                colIndex = 4;
                var sStartCell = Global.GetExcelCellName(nStartRow, colIndex);
                var sEndCell = Global.GetExcelCellName(nEndRow, colIndex);
                cell = sheet.Cells[rowIndex, colIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##,##0.00;(#,##,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 5, rowIndex, 16]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                #endregion

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=Import_Payment_Settlement.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }

        #region Advance Search
        [HttpPost]
        public JsonResult SearchByNo(ImportPayment oImportPayment)
        {
            List<ImportPaymentSettlement> oImportPaymentSettlements = new List<ImportPaymentSettlement>();
            try
            {
                string sSQL = "SELECT * FROM View_ImportPaymentsettlement AS HH WHERE HH.BUID=" + oImportPayment.BUID.ToString() + " AND (ISNULL(HH.PMTLiabilityNo,'')+ISNULL(HH.ImportInvoiceNo,'')+ISNULL(HH.ImportLCNo,'')) LIKE '%" + oImportPayment.ImportInvoiceNo + "%' ORDER BY ImportInvoiceID ASC";
                oImportPaymentSettlements = ImportPaymentSettlement.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oImportPaymentSettlements = new List<ImportPaymentSettlement>();
            }

            var jsonResult = Json(oImportPaymentSettlements, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult WaitForApproved(ImportPayment oImportPayment)
        {
            _oImportPaymentSettlements = new List<ImportPaymentSettlement>();
            try
            {
                string sSQL = "SELECT * FROM View_ImportPaymentsettlement WHERE BUID = " + oImportPayment.BUID.ToString() + " AND ISNULL(PMTApprovedBy,0)=0 ORDER BY ImportInvoiceID ASC";
                _oImportPaymentSettlements = ImportPaymentSettlement.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oImportPaymentSettlement = new ImportPaymentSettlement();
                _oImportPaymentSettlements = new List<ImportPaymentSettlement>();
                _oImportPaymentSettlement.ErrorMessage = ex.Message;
                _oImportPaymentSettlements.Add(_oImportPaymentSettlement);
            }

            var jsonResult = Json(_oImportPaymentSettlements, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult AdvanceSearch(ImportPayment oImportPayment)
        {
            _oImportPaymentSettlements = new List<ImportPaymentSettlement>();
            try
            {
                string sSQL = this.GetSQL(oImportPayment);
                _oImportPaymentSettlements = ImportPaymentSettlement.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oImportPaymentSettlement = new ImportPaymentSettlement();
                _oImportPaymentSettlements = new List<ImportPaymentSettlement>();
                _oImportPaymentSettlement.ErrorMessage = ex.Message;
                _oImportPaymentSettlements.Add(_oImportPaymentSettlement);

            }

            var jsonResult = Json(_oImportPaymentSettlements, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        private string GetSQL(ImportPayment oImportPayment)
        {
            string sSearchingData = oImportPayment.Remarks;
            EnumCompareOperator ePaymentDate =(EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[0]);
            DateTime dPaymentStartDate = Convert.ToDateTime(sSearchingData.Split('~')[1]);
            DateTime dPaymentEndDate =  Convert.ToDateTime(sSearchingData.Split('~')[2]);
            EnumCompareOperator ePaymentAmount = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[3]);
            double nPaymentAmountStart = Convert.ToDouble(sSearchingData.Split('~')[4]);
            double nPaymentAmountEnd = Convert.ToDouble(sSearchingData.Split('~')[5]);
            int nCostHead = Convert.ToInt32(sSearchingData.Split('~')[6]);
            string sRemarks = Convert.ToString(sSearchingData.Split('~')[7]);
            bool bApproved = Convert.ToBoolean(sSearchingData.Split('~')[8]);
            bool bUnApproved = Convert.ToBoolean(sSearchingData.Split('~')[9]);
            int nImportPIType = Convert.ToInt32(sSearchingData.Split('~')[10]);

            string sReturn1 = "SELECT * FROM View_ImportPaymentsettlement";
            string sReturn = "";

            #region BUID
            if (oImportPayment.BUID != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BUID = " + oImportPayment.BUID.ToString();
            }
            #endregion

            #region LiabilityNo
            if (oImportPayment.LiabilityNo == null) oImportPayment.LiabilityNo = "";
            if (oImportPayment.LiabilityNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " PMTLiabilityNo LIKE '%" + oImportPayment.LiabilityNo + "%'";
            }
            #endregion

            #region InvoiceNo
            if (oImportPayment.ImportInvoiceNo == null) oImportPayment.ImportInvoiceNo = "";
            if (oImportPayment.ImportInvoiceNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ImportInvoiceNo LIKE '%" + oImportPayment.ImportInvoiceNo + "%'";
            }
            #endregion

            #region ImportLCNo
            if (oImportPayment.ImportLCNo == null) oImportPayment.ImportLCNo = "";
            if (oImportPayment.ImportLCNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ImportLCNo LIKE '%" + oImportPayment.ImportLCNo + "%'";
            }
            #endregion

            #region LiabilityType
            if ((EnumLiabilityType)oImportPayment.LiabilityTypeInt !=  EnumLiabilityType.None)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ISNULL(LiabilityType,0) = " + oImportPayment.LiabilityTypeInt.ToString();
            }
            #endregion

            #region ForExGainLoss
            if ((EnumForExGainLoss)oImportPayment.ForExGainLossInt != EnumForExGainLoss.None)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ISNULL(ForExGainLoss,0) = " + oImportPayment.ForExGainLossInt.ToString();
            }
            #endregion

            #region ApprovedBy
            if (oImportPayment.ApprovedBy != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ISNULL(PMTApprovedBy,0) = " + oImportPayment.ApprovedBy.ToString();
            }
            #endregion

            #region BankAccountID
            if (oImportPayment.BankAccountID != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ISNULL(PMTBankAccountID,0) = " + oImportPayment.BankAccountID.ToString();
            }
            #endregion

            #region MarginAccountID
            if (oImportPayment.MarginAccountID != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ISNULL(PMTMarginAccountID,0) = " + oImportPayment.MarginAccountID.ToString();
            }
            #endregion
            
            #region PaymentDate Date
            if (ePaymentDate != EnumCompareOperator.None)
            {
                if (ePaymentDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),PaymentDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dPaymentStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (ePaymentDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),PaymentDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dPaymentStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (ePaymentDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),PaymentDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dPaymentStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (ePaymentDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),PaymentDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dPaymentStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (ePaymentDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),PaymentDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dPaymentStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dPaymentEndDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (ePaymentDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),PaymentDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dPaymentStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dPaymentEndDate.ToString("dd MMM yyyy") + "',106))";
                }
            }
            #endregion

            #region PaymentAmount
            if (ePaymentAmount != EnumCompareOperator.None)
            {
                if (ePaymentAmount == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " PMTAmount = " + nPaymentAmountStart.ToString("0.00");
                }
                else if (ePaymentAmount == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " PMTAmount != " + nPaymentAmountStart.ToString("0.00");
                }
                else if (ePaymentAmount == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " PMTAmount < " + nPaymentAmountStart.ToString("0.00");
                }
                else if (ePaymentAmount == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " PMTAmount > " + nPaymentAmountStart.ToString("0.00");
                }
                else if (ePaymentAmount == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " PMTAmount BETWEEN " + nPaymentAmountStart.ToString("0.00") + " AND " + nPaymentAmountEnd.ToString("0.00");
                }
                else if (ePaymentAmount == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " PMTAmount NOT BETWEEN " + nPaymentAmountStart.ToString("0.00") + " AND " + nPaymentAmountEnd.ToString("0.00");
                }
            }
            #endregion

            #region Remarks
            if (sRemarks != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " PMTRemarks LIKE '%" + sRemarks + "%'";
            }
            #endregion

            #region Approved
            if (bApproved == true)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ISNULL(PMTApprovedBy,0) != 0 ";
            }
            #endregion

            #region Un-Approved
            if (bUnApproved == true)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ISNULL(PMTApprovedBy,0) = 0 ";
            }
            #endregion

            #region Import PI Type
            if (nImportPIType > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ImportInvoiceID IN (SELECT ImportInvoiceID FROM ImportInvoice WHERE InvoiceType = " + nImportPIType + ")";
            }
            #endregion

            sReturn = sReturn1 + sReturn + " ORDER BY ImportInvoiceID ASC";
            return sReturn;
        }
        #endregion

        public Image GetCompanyLogo(Company oCompany)
        {
            if (oCompany.OrganizationLogo != null)
            {
                MemoryStream m = new MemoryStream(oCompany.OrganizationLogo);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(Response.OutputStream, ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }
    }
}
