using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ICS.Core.Framework;
using System.Web.Script.Serialization;

using ICS.Core.Utility;
using System.IO;
using System.Drawing.Imaging;
using ESimSol.Reports;
using System.Drawing;
using iTextSharp.text.pdf;
using System.Web.Services;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ESimSolFinancial.Controllers
{
    public class ExportIncentiveController : Controller
    {
        #region Declaration
        ExportIncentive _oExportIncentive = new ExportIncentive();
        List<ExportIncentive> _oExportIncentives = new List<ExportIncentive>();
        string _sErrorMessage = "";
        #endregion

        public ActionResult ViewExportIncentives(int menuid, int buid)//test
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
           
            _oExportIncentives = new List<ExportIncentive>();
            _oExportIncentives = ExportIncentive.Gets("SELECT top(100)* FROM View_ExportLC_Incentive WHERE ISNULL(ApplicationBy,0)=0 order by BillRelizationDate DESC", ((User)Session[SessionInfo.CurrentUser]).UserID);

            ViewBag.BUID = buid;
            ViewBag.CompareOperators = Enum.GetValues(typeof(EnumCompareOperator)).Cast<EnumCompareOperator>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            return View(_oExportIncentives);
        }
        public ActionResult ViewExportIncentive(int id, int nExportLCID, int nBillID, int nBUID)
        {
            _oExportIncentive = new ExportIncentive();
            List<MasterLC> oMasterLCs = new List<MasterLC>();
            if (id == 0)
            {
                _oExportIncentive.ExportLCID = nExportLCID;
                _oExportIncentive.ExportBillID = nBillID;
                _oExportIncentive = _oExportIncentive.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            else 
            {
                _oExportIncentive = _oExportIncentive.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            ViewBag.BUID = nBUID;
            oMasterLCs = MasterLC.GetsByLCID(nExportLCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oExportIncentive.MasterLCNo = string.Join(", ", oMasterLCs.Select(x => (x.MasterLCNo + " DT:" + x.MasterLCDateSt)));

            ViewBag.ExportBills = ExportBill.Gets("SELECT DD.* FROM View_ExportBill AS DD WHERE DD.ExportLCID=" + nExportLCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.Currencys = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(_oExportIncentive);
        }
        [HttpPost]
        public JsonResult Save(ExportIncentive oExportIncentive)
        {
            _oExportIncentive = new ExportIncentive();
            try
            {
                _oExportIncentive = oExportIncentive;
                _oExportIncentive = _oExportIncentive.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oExportIncentive = new ExportIncentive();
                _oExportIncentive.ErrorMessage = ex.Message;
            }
            
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportIncentive);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                ExportIncentive oExportIncentive = new ExportIncentive();
                sFeedBackMessage = oExportIncentive.Delete(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Gets_Incentive(ExportIncentive oExportIncentive)
        {
            List<ExportIncentive> oExportIncentives = new List<ExportIncentive>();
            string SQL = "SELECT * FROM View_ExportLC_Incentive WHERE ExportLCID>0 ";
            if (!string.IsNullOrEmpty(oExportIncentive.ExportLCNo))
                SQL += " AND ExportLCNo LIKE '%" + oExportIncentive.ExportLCNo + "%'";

            oExportIncentives = ExportIncentive.Gets(SQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oExportIncentives);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region Adv Search
        public ActionResult AdvanceSearch()
        {
            _oExportIncentive = new ExportIncentive();
            return PartialView(_oExportIncentive);
        }
        [HttpPost]
        public JsonResult AdvSearch(ExportIncentive oExportIncentive)
        {
            List<ExportIncentive> oExportIncentives = new List<ExportIncentive>();
            string sSQL = MakeSQL(oExportIncentive);
            if (sSQL == "Error")
            {
                _oExportIncentive = new ExportIncentive();
                _oExportIncentive.ErrorMessage = "Please select a searching critaria.";
                oExportIncentives = new List<ExportIncentive>();
            }
            else
            {
                oExportIncentives = new List<ExportIncentive>();
                oExportIncentives = ExportIncentive.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oExportIncentives.Count == 0)
                {
                    oExportIncentives = new List<ExportIncentive>();
                }
            }
            var jsonResult = Json(oExportIncentives, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        private string MakeSQL(ExportIncentive oExportIncentive)
        {
            string sParams = oExportIncentive.ErrorMessage;

            int nDateCriteria_BillRelization = 0;
            DateTime dStart_BillRelizationDate = DateTime.Today;
            DateTime dEnd_BillRelizationDate = DateTime.Today;

            int nDateCriteria_PRC = 0;
            DateTime dStart_PRC = DateTime.Today;
            DateTime dEnd_PRC = DateTime.Today;
            int nDateCriteria_APP = 0;
            DateTime dStart_APP = DateTime.Today;
            DateTime dEnd_APP = DateTime.Today;
            int nDateCriteria_BTMA = 0;
            DateTime dStart_BTMA = DateTime.Today;
            DateTime dEnd_BTMA = DateTime.Today;
            int nDateCriteria_AuditCert = 0;
            DateTime dStart_AuditCert = DateTime.Today;
            DateTime dEnd_AuditCert = DateTime.Today;
            int nDateCriteria_Realized = 0;
            DateTime dStart_Realized = DateTime.Today;
            DateTime dEnd_Realized = DateTime.Today;

            int nTimeLag = 0;
            bool YetToPRC = false;
            bool YetToApp = false;
            bool YetToBTMA = false;
            bool YetToAudit = false;
            bool YetToRealize = false;
            if (!String.IsNullOrEmpty(sParams))
            {
                int nCount = 0;
                nDateCriteria_BillRelization = Convert.ToInt32(sParams.Split('~')[nCount++]);
                dStart_BillRelizationDate = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                dEnd_BillRelizationDate = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                nDateCriteria_PRC = Convert.ToInt32(sParams.Split('~')[nCount++]);
                dStart_PRC = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                dEnd_PRC = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                nDateCriteria_APP = Convert.ToInt32(sParams.Split('~')[nCount++]);
                dStart_APP = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                dEnd_APP = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                nDateCriteria_BTMA = Convert.ToInt32(sParams.Split('~')[nCount++]);
                dStart_BTMA = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                dEnd_BTMA = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                nDateCriteria_AuditCert = Convert.ToInt32(sParams.Split('~')[nCount++]);
                dStart_AuditCert = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                dEnd_AuditCert = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                nDateCriteria_Realized = Convert.ToInt32(sParams.Split('~')[nCount++]);
                dStart_Realized = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                dEnd_Realized = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                nTimeLag = Convert.ToInt32(sParams.Split('~')[nCount++]);
                YetToPRC = Convert.ToBoolean(sParams.Split('~')[nCount++]);
                YetToApp = Convert.ToBoolean(sParams.Split('~')[nCount++]);
                YetToBTMA = Convert.ToBoolean(sParams.Split('~')[nCount++]);
                YetToAudit = Convert.ToBoolean(sParams.Split('~')[nCount++]);
                YetToRealize = Convert.ToBoolean(sParams.Split('~')[nCount++]);
            }

            string sReturn1 = "SELECT * FROM View_ExportLC_Incentive AS EB";
            string sReturn = "";

            #region DATE SEARCH
            //BillRelizationDate,PRCDate,ApplicationDate,BTMAIssueDate,RealizedDate,AuditCertDate
            MakeSQLByDate(ref sReturn, "BillRelizationDate", nDateCriteria_BillRelization, dStart_BillRelizationDate, dEnd_BillRelizationDate);
            MakeSQLByDate(ref sReturn, "PRCDate", nDateCriteria_PRC, dStart_PRC, dEnd_PRC);
            MakeSQLByDate(ref sReturn, "ApplicationDate", nDateCriteria_APP, dStart_APP, dEnd_APP);
            MakeSQLByDate(ref sReturn, "BTMAIssueDate", nDateCriteria_BTMA, dStart_AuditCert, dEnd_AuditCert);
            MakeSQLByDate(ref sReturn, "AuditCertDate", nDateCriteria_AuditCert, dStart_AuditCert, dEnd_AuditCert);
            MakeSQLByDate(ref sReturn, "RealizedDate", nDateCriteria_Realized, dStart_Realized, dEnd_Realized);
            #endregion

            #region Time_Lag
            if (nTimeLag >= 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.Time_Lag >=" + nTimeLag;
            }
            #endregion

            #region YET TO
            this.MakeSQLByBool(ref sReturn,"PRCCollectBy",YetToPRC);
            this.MakeSQLByBool(ref sReturn,"ApplicationBy",YetToApp);
            this.MakeSQLByBool(ref sReturn,"BTMAIssueBy",YetToBTMA);
            this.MakeSQLByBool(ref sReturn,"AuditCertBy",YetToAudit);
            this.MakeSQLByBool(ref sReturn,"RealizedBy",YetToRealize);
            #endregion
            sReturn = sReturn1 + sReturn;

            return sReturn;
        }
        private void MakeSQLByDate(ref string sReturn,string sSearchDate, int nDateCriteria, DateTime dStartDate, DateTime dEndDate) 
        {
            #region ChalllanDate
            if (nDateCriteria > 0)
            {
                Global.TagSQL(ref sReturn);
                if (nDateCriteria == (int)EnumCompareOperator.EqualTo)
                {
                    sReturn = sReturn + " EB." + sSearchDate + "= '" + dStartDate.ToString("dd MMM yyy") + "' ";
                }
                else if (nDateCriteria == (int)EnumCompareOperator.NotEqualTo)
                {
                    sReturn = sReturn + " EB." + sSearchDate + " != '" + dStartDate.ToString("dd MMM yyy") + "' ";
                }
                else if (nDateCriteria == (int)EnumCompareOperator.GreaterThan)
                {
                    sReturn = sReturn + " EB." + sSearchDate + " > '" + dStartDate.ToString("dd MMM yyy") + "' ";
                }
                else if (nDateCriteria == (int)EnumCompareOperator.SmallerThan)
                {
                    sReturn = sReturn + " EB." + sSearchDate + " < '" + dStartDate.ToString("dd MMM yyy") + "' ";
                }
                else if (nDateCriteria == (int)EnumCompareOperator.Between)
                {
                    sReturn = sReturn + " EB." + sSearchDate + " BETWEEN '" + dStartDate.ToString("dd MMM yyy") + "' AND '" + dEndDate.ToString("dd MMM yyy") + "' ";
                }
                else if (nDateCriteria == (int)EnumCompareOperator.NotBetween)
                {
                    sReturn = sReturn + " EB." + sSearchDate + " NOT BETWEEN '" + dStartDate.ToString("dd MMM yyy") + "' AND '" + dEndDate.ToString("dd MMM yyy") + "' ";
                }
            }
            #endregion
        }
        private void MakeSQLByBool(ref string sReturn, string sColumn, bool bYetTo) 
        {
            if (bYetTo)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ISNULL(EB."+sColumn+",0)=0 " ;
            }
        }
        #endregion

        #region Update
        [HttpPost]
        public JsonResult Update_Incentive(ExportIncentive oExportIncentive) 
        {
            //Update_PRCDate,Update_ApplicationDate,Update_BTMAIssueDate,Update_AuditCertDate,Update_RealizedDate
            _oExportIncentive = new ExportIncentive();
            try
            {
                _oExportIncentive = oExportIncentive;
                if (oExportIncentive.UpdateOperation == 1)
                    _oExportIncentive = _oExportIncentive.Update_PRCDate(((User)Session[SessionInfo.CurrentUser]).UserID);
                else if (oExportIncentive.UpdateOperation == 2)
                    _oExportIncentive = _oExportIncentive.Update_ApplicationDate(((User)Session[SessionInfo.CurrentUser]).UserID);
                else if (oExportIncentive.UpdateOperation == 3)
                    _oExportIncentive = _oExportIncentive.Update_BTMAIssueDate(((User)Session[SessionInfo.CurrentUser]).UserID);
                else if (oExportIncentive.UpdateOperation == 4)
                    _oExportIncentive = _oExportIncentive.Update_AuditCertDate(((User)Session[SessionInfo.CurrentUser]).UserID);
                else if (oExportIncentive.UpdateOperation == 5)
                    _oExportIncentive = _oExportIncentive.Update_RealizedDate(((User)Session[SessionInfo.CurrentUser]).UserID);
                else if (oExportIncentive.UpdateOperation == 6)
                    _oExportIncentive = _oExportIncentive.Update_BankSubDate(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oExportIncentive = new ExportIncentive();
                _oExportIncentive.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportIncentive);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region PRINT
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

        public ActionResult PrintIncentive(int id, int Authorized, int nBUID)
        {
            List<MasterLC> oMasterLCs = new List<MasterLC>();
            ExportIncentive oExportIncentive = new ExportIncentive();
            oExportIncentive = oExportIncentive.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);

            if (oExportIncentive.ExportLCID > 0)
            {
                oMasterLCs = MasterLC.GetsByLCID(oExportIncentive.ExportLCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oExportIncentive.MasterLCNo = string.Join(", ", oMasterLCs.Select(x => (x.MasterLCNo + " DT:" + x.MasterLCDateSt)));
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            ImportLetterSetup oImportLetterSetup = new ImportLetterSetup();
            ImportLetterSetup oImportLetterSetupFord = new ImportLetterSetup();
            oImportLetterSetup = oImportLetterSetup.Get((int)EnumImportLetterType.ExportLCIncentive, (int)EnumImportLetterIssueTo.Bank, oExportIncentive.ExportLCID, "", ((User)Session[SessionInfo.CurrentUser]).UserID);
            oImportLetterSetupFord = oImportLetterSetupFord.Get((int)EnumImportLetterType.ExportLCIncentiveFord, (int)EnumImportLetterIssueTo.IssueBank, oExportIncentive.ExportLCID, "", ((User)Session[SessionInfo.CurrentUser]).UserID);

            if (Authorized == 1)
                oImportLetterSetup.Authorize2IsAuto = false;
            else if (Authorized == 2)
                oImportLetterSetup.Authorize1IsAuto = false;

            rptExportIncentive oReport = new rptExportIncentive();
            byte[] abytes = oReport.PreparePrintLetter(oExportIncentive, oImportLetterSetup, oImportLetterSetupFord, oCompany);

            return File(abytes, "application/pdf");
        }
        #endregion

        #region PRINT TO EXCEL
        public void ExportToExcel(string sIDs)
        {
            ExportIncentive oExportIncentive = new ExportIncentive();
            string _sErrorMesage;
            try
            {
                _sErrorMesage = "";
                _oExportIncentives = new List<ExportIncentive>();
                _oExportIncentives = ExportIncentive.Gets("SELECT * FROM View_ExportLC_Incentive WHERE ExportLCID IN (" + sIDs + ") order by BillRelizationDate DESC", (int)Session[SessionInfo.currentUserID]);
               
                if (_oExportIncentives.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oExportIncentives = new List<ExportIncentive>();
                _sErrorMesage = ex.Message;
            }

            if (_sErrorMesage == "")
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
     
            
                #region Print XL

                int nRowIndex = 2, nStartRow = 2, nStartCol = 2, nEndCol = 12, nColumn = 1, nCount = 0;
                ExcelRange cell;
                ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("ExportIncentive");
                    sheet.Name = "Export Incentive";
                    sheet.Column(++nColumn).Width = 10; //SL
                    //sheet.Column(++nColumn).Width = 10; //TextileUnitST
                    sheet.Column(++nColumn).Width = 15; //ExportLCNo
                    sheet.Column(++nColumn).Width = 10; //Version No
                    sheet.Column(++nColumn).Width = 15; //OpeningDateST
                    sheet.Column(++nColumn).Width = 15; //LCRecivedDateST
                    sheet.Column(++nColumn).Width = 10; //FileNo
                    sheet.Column(++nColumn).Width = 20; //MasterLCNo
                    sheet.Column(++nColumn).Width = 15; //MLC Dates
                    sheet.Column(++nColumn).Width = 35; //ApplicantName
                    sheet.Column(++nColumn).Width = 15; //AmendmentDateSt
                    sheet.Column(++nColumn).Width = 35; //BankName_Issue
                    sheet.Column(++nColumn).Width = 25; //BBranchName_Issue
                    sheet.Column(++nColumn).Width = 15; //Amount_ST
                    sheet.Column(++nColumn).Width = 15; //Amount_BillRealST
                    sheet.Column(++nColumn).Width = 15; //Amount_Realized_PVST
                    sheet.Column(++nColumn).Width = 15; //BillRelizationDateST
                    sheet.Column(++nColumn).Width = 10; //Time_Lag
                    sheet.Column(++nColumn).Width = 15; //ApplicationDateST
                    sheet.Column(++nColumn).Width = 15; //PRCDateST
                    sheet.Column(++nColumn).Width = 15; //BTMAIssueDateST
                    sheet.Column(++nColumn).Width = 15; //BankSubDateST
                    sheet.Column(++nColumn).Width = 15; //AuditCertDateST
                    sheet.Column(++nColumn).Width = 15; //Amount_RealizedST
                    sheet.Column(++nColumn).Width = 15; //RealizedDateST
                    nEndCol=nColumn;

                    #region Report Header
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 6]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, nEndCol-5, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = "Export IncentiveReport"; cell.Style.Font.Bold = true;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Address & Date
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 6]; cell.Merge = true;
                    cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    //cell = sheet.Cells[nRowIndex, nStartCol + 10, nRowIndex, nEndCol]; cell.Merge = true;
                    //cell.Value = ""; cell.Style.Font.Bold = false;
                    //cell.Style.WrapText = true;
                    //cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
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
                   
                    string [] sHeader= new string[] {"SLNo","LC No","A No","LC Date","LC Receive Date","File No", "Master L/C No","Master L/C Date", "Applicant","Amendment Date","Bank","Bank Branch","LC Value","Invoice Value","Incentive Value","Bill Relization DT"
                                                    ,"Time Lag","Application Date","PRC Date","BTMA Date","Bank Sub. Date","Audit Date", "Realized Value", "Realized Date"};

                    foreach (string Header in sHeader) 
                    {
                        this.AddExcelHeader(ref cell, sheet,Header, nRowIndex, ++nColumn);
                    }
                    
                    nRowIndex++;
                    #endregion

                    string sCurrencySymbol = _oExportIncentives.Select(x=>x.CurrencySymbol).FirstOrDefault();

                    #region Data
                    foreach (ExportIncentive oItem in _oExportIncentives)
                    {
                        nCount++;
                        nColumn = 1;
                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = nCount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.ExportLCNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.VersionNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.OpeningDateST; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.LCRecivedDateST; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.FileNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.MasterLCNos; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.MasterLCDates; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.ApplicantName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.AmendmentDateSt; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.BankName_Issue; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.BBranchName_Issue; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.Amount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = sCurrencySymbol+" ##,##0.00";
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.Amount_BillReal; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = sCurrencySymbol + " ##,##0.00";
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.Amount_Realized_PV; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = sCurrencySymbol + " ##,##0.00";
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.BillRelizationDateST; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.Time_Lag; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = " ##,##0.00";
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.ApplicationDateST; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.PRCDateST; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.BTMAIssueDateST; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.BankSubDateST; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.AuditCertDateST; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.Amount_Realized; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = oItem.CurrencySymbol_Real+ " ##,##0.00";
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.RealizedDateST; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nRowIndex++;
                    }
                    #endregion

                    #region Total
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol -12]; cell.Value = "" + "Total"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Merge = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    double nValue = _oExportIncentives.Select(c => c.Amount).Sum();
                    cell = sheet.Cells[nRowIndex, nEndCol - 11]; cell.Value = nValue; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = sCurrencySymbol+" ##,##0.00";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nValue = _oExportIncentives.Select(c => c.Amount_BillReal).Sum();
                    cell = sheet.Cells[nRowIndex, nEndCol - 10]; cell.Value = nValue; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = sCurrencySymbol + " ##,##0.00";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nValue = _oExportIncentives.Select(c => c.Amount_Realized_PV).Sum();
                    cell = sheet.Cells[nRowIndex, nEndCol - 9]; cell.Value = nValue; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = sCurrencySymbol+" ##,##0.00";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    int nColumnBlank=8;
                    while (nColumnBlank > 1) 
                    {
                        cell = sheet.Cells[nRowIndex, nEndCol - nColumnBlank--]; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }

                    nValue = _oExportIncentives.Select(c => c.Amount_Realized).Sum();
                    cell = sheet.Cells[nRowIndex, nEndCol - 1]; cell.Value = nValue; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = sCurrencySymbol + " ##,##0.00";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nEndCol]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nRowIndex = nRowIndex + 1;
                    #endregion
                    #endregion

                    cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=ExportIncentive.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
        }
        public void AddExcelHeader(ref ExcelRange cell, ExcelWorksheet sheet, string sHeader,int nRowIndex,int nColumn) 
        {
            OfficeOpenXml.Style.Border border;
            cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = sHeader; cell.Style.Font.Bold = true;
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        }
        #endregion

        //#region TIPS
        //public ActionResult View_AddTipsnSpeedsIncentive(int nId, double ts)
        //{
        //    TipsnSpeed oTipsnSpeed = new TipsnSpeed();
        //    List<TipsnSpeed> oTipsnSpeeds = new List<TipsnSpeed>();
        //    ExportIncentive oExportIncentive = new ExportIncentive();
        //    oExportIncentive.ExportLCID = nId;

        //    string sSQL = "";
        //    if (nId > 0)
        //    {
        //        sSQL = "Select * from View_TipsnSpeed Where ReferenceType = " + (int)EnumReferenceType.LC + " AND ReferenceID = " + nId;
        //        oTipsnSpeeds = TipsnSpeed.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    }
        //    sSQL = "Select * From TipsType Where ReferenceType = " + (int)EnumReferenceType.LC;
        //    List<TipsType> oTipsTypes = TipsType.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    ViewBag.TipsTypes = oTipsTypes;
        //    ViewBag.TipsnSpeeds = oTipsnSpeeds;
        //    ViewBag.Currencys = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
        //    Company oCompany = new Company();
        //    oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    ViewBag.BaseCurrencyID = oCompany.BaseCurrencyID;
        //    ViewBag.PrepareByName = ((User)Session[SessionInfo.CurrentUser]).UserName;
        //    return View(oExportIncentive);
        //}
        //#endregion
    }
}

