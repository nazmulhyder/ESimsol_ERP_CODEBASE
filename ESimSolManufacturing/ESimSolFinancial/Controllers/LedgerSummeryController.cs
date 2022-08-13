using System;
using System.Linq;
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
using System.Threading;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using ESimSolFinancial.Hubs;


namespace ESimSolFinancial.Controllers
{
    public class LedgerSummeryController : Controller
    {
        #region Declaration
        List<LedgerSummery> _oLedgerSummerys = new List<LedgerSummery>();
        LedgerSummery _oLedgerSummery = new LedgerSummery();
        CostCenterBreakdown _oCostCenterBreakdown = new CostCenterBreakdown();
        VoucherBillBreakDown _oVoucherBillBreakDown = new VoucherBillBreakDown();
        #endregion


        private bool ValidateInputForLedgerSummery(LedgerSummery oLedgerSummery)
        {
            _oLedgerSummery = new LedgerSummery();
            if (oLedgerSummery.AccountHeadID <= 0)
            {
                _oLedgerSummery.ErrorMessage = "Please select an account head";
                return false;
            }
            if (oLedgerSummery.EndDate < oLedgerSummery.StartDate)
            {
                _oLedgerSummery.ErrorMessage = "End Date must be grater/equal then start date";
                return false;
            }
            return true;
        }
        [HttpPost]
        public ActionResult SetSessionData(LedgerSummery oLedgerSummery)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oLedgerSummery);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        

        [HttpPost]
        public JsonResult GetSessionData(LedgerSummery oLedgerSummery)
        {
            List<LedgerSummery> oLedgerSummerys = new List<LedgerSummery>();
            try
            {
                oLedgerSummerys = (List<LedgerSummery>)Session[SessionInfo.SearchData];
            }
            catch (Exception ex)
            {
                oLedgerSummerys = new List<LedgerSummery>();
            }
            var jsonResult = Json(oLedgerSummerys, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public ActionResult ViewLedgerSummery(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            ChartsOfAccount oChartsOfAccount = new ChartsOfAccount();
            AccountingSession oAccountingSession = new AccountingSession();
            LedgerSummery oLedgerSummery = new LedgerSummery();
            List<LedgerSummery> oLedgerSummerys = new List<LedgerSummery>();
           
            oLedgerSummery.Currencies = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);

            //ViewBag.COA = oChartsOfAccount;

            #region Business Unit
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
            oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            oBusinessUnits = BusinessUnit.GetsPermittedBU(oUser, (int)Session[SessionInfo.currentUserID]);
            ViewBag.BusinessUnits = oBusinessUnits;
            #endregion
            
            return View(oLedgerSummery);
        }

        [HttpPost]
        public JsonResult GetLedgerSummerys(LedgerSummery oLedgerSummery)
        {
            _oLedgerSummerys = new List<LedgerSummery>();
            _oLedgerSummerys = LedgerSummery.Gets(oLedgerSummery, (int)Session[SessionInfo.currentUserID]);

            var jSonResult = Json(_oLedgerSummerys, JsonRequestBehavior.AllowGet);
            jSonResult.MaxJsonLength = int.MaxValue;
            return jSonResult;
        }


        public ActionResult PrintLedgerSummerys()
        {
            _oLedgerSummery = new LedgerSummery();
            try
            {
                _oLedgerSummery = (LedgerSummery)Session[SessionInfo.ParamObj];
                //string sSQL = "SELECT * FROM View_LedgerSummery WHERE LedgerSummeryID IN (" + _oLedgerSummery.ErrorMessage + ") Order By LedgerSummeryID";
                _oLedgerSummerys = LedgerSummery.Gets(_oLedgerSummery, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oLedgerSummery = new LedgerSummery();
                _oLedgerSummerys = new List<LedgerSummery>();
            }

            Company oCompany = new Company();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            if (_oLedgerSummery.BUID <= 0)
            {
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            else
            {
                oBusinessUnit = oBusinessUnit.Get(_oLedgerSummery.BUID, (int)Session[SessionInfo.currentUserID]);
                oCompany = new Company();
                oCompany = GlobalObject.BUTOCompany(oCompany, oBusinessUnit);
            }
            
            if (_oLedgerSummerys.Count > 0)
            {
                rptLedgerSummerys oReport = new rptLedgerSummerys();
                byte[] abytes = oReport.PrepareReport(_oLedgerSummery, _oLedgerSummerys, oCompany);
                return File(abytes, "application/pdf");
            }
            else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport("No Data Found!!");
                return File(abytes, "application/pdf");
            }
        }

        public void ExcelLedgerSummerys()
        {
            _oLedgerSummery = new LedgerSummery();
            try
            {
                _oLedgerSummery = (LedgerSummery)Session[SessionInfo.ParamObj];
                _oLedgerSummerys = LedgerSummery.Gets(_oLedgerSummery, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oLedgerSummery = new LedgerSummery();
                _oLedgerSummerys = new List<LedgerSummery>();
            }

            Company oCompany = new Company();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            if (_oLedgerSummery.BUID <= 0)
            {
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            else
            {
                oBusinessUnit = oBusinessUnit.Get(_oLedgerSummery.BUID, (int)Session[SessionInfo.currentUserID]);
                oCompany = new Company();
                oCompany = GlobalObject.BUTOCompany(oCompany, oBusinessUnit);
            }

            if (_oLedgerSummerys.Count > 0)
            {

                #region full excel
                int rowIndex = 2;
                ExcelRange cell;
                ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Ledger Summery");
                    sheet.Name = "Ledger Summery";
                    sheet.Column(2).Width = 5; //SL
                    sheet.Column(3).Width = 25; //code
                    sheet.Column(4).Width = 25; //name
                    sheet.Column(5).Width = 18; //debit
                    sheet.Column(6).Width = 18; //credit

                    #region Report Header
                    sheet.Cells[rowIndex, 2, rowIndex, 6].Merge = true;
                    cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex = rowIndex + 1;

                    sheet.Cells[rowIndex, 2, rowIndex, 6].Merge = true;
                    cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex = rowIndex + 1;

                    sheet.Cells[rowIndex, 2, rowIndex, 6].Merge = true; cell.Style.WrapText = true;
                    cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Phone + ";  " + oCompany.Email + ";  " + oCompany.WebAddress; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex = rowIndex + 1;

                    sheet.Cells[rowIndex, 2, rowIndex, 6].Merge = true; cell.Style.WrapText = true;
                    cell = sheet.Cells[rowIndex, 2]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex = rowIndex + 1;

                    sheet.Cells[rowIndex, 2, rowIndex, 6].Merge = true; cell.Style.WrapText = true; cell.Style.Font.UnderLine = true;
                    cell = sheet.Cells[rowIndex, 2]; cell.Value = _oLedgerSummery.AccountHeadName; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex = rowIndex + 1;

                    sheet.Cells[rowIndex, 2, rowIndex, 6].Merge = true;
                    cell = sheet.Cells[rowIndex, 2]; cell.Value = _oLedgerSummery.StartDateInString + " to " + _oLedgerSummery.EndDateInString; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex = rowIndex + 2;
                    #endregion

                    #region Column Header

                    cell = sheet.Cells[rowIndex, 2]; cell.Value = "#SL"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 3]; cell.Value = "Code"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 4]; cell.Value = "Name"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 5]; cell.Value = "Debit"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 6]; cell.Value = "Credit"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    rowIndex = rowIndex + 1;

                    #endregion

                    #region Report Data
                    int nSL = 0;
                    double dGrandDrAmount = 0, dGrandCrAmount = 0;
                    foreach (LedgerSummery oItem in _oLedgerSummerys)
                    {
                        nSL++;
                        cell = sheet.Cells[rowIndex, 2]; cell.Value = nSL; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 3]; cell.Value = oItem.AccountHeadCode; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 4]; cell.Value = oItem.AccountHeadName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 5]; cell.Value = oItem.DrAmountInString; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        dGrandDrAmount += oItem.DrAmount;

                        cell = sheet.Cells[rowIndex, 6]; cell.Value = oItem.CrAmountInString; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        dGrandCrAmount += oItem.CrAmount;

                        rowIndex++;
                    }
                    #endregion

                    cell = sheet.Cells[rowIndex, 2, rowIndex, 4]; cell.Merge = true; cell.Value = "Grand Total"; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 5]; cell.Value = dGrandDrAmount; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    cell.Style.Numberformat.Format = "###,0.00";

                    cell = sheet.Cells[rowIndex, 6]; cell.Value = dGrandCrAmount; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    cell.Style.Numberformat.Format = "###,0.00";

                    rowIndex++;

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=Ledger_Summery.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion

            }
        }

        public Image GetCompanyLogo(Company oCompany)
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
    }
}