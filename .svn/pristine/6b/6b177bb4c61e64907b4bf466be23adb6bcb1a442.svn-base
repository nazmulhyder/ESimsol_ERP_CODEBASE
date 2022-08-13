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
using ICS.Core.Utility;
using System.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Style;


namespace ESimSolFinancial.Controllers
{
    public class AccountingRatioSetupController : Controller
    {
        #region Declaration
        AccountingRatioSetup _oAccountingRatioSetup = new AccountingRatioSetup();
        List<AccountingRatioSetup> _oAccountingRatioSetups = new List<AccountingRatioSetup>();

        TLocation _oTLocation = new TLocation();
        List<TLocation> _oTLocations = new List<TLocation>();
        List<Location> _oLocations = new List<Location>();
        TChartsOfAccount _oTChartsOfAccount = new TChartsOfAccount();
        List<TChartsOfAccount> _oTChartsOfAccounts = new List<TChartsOfAccount>();
        List<ChartsOfAccount> _oChartsOfAccounts = new List<ChartsOfAccount>();

        TBusinessLocation _oTBusinessLocation = new TBusinessLocation();
        List<TBusinessLocation> _oTBusinessLocations = new List<TBusinessLocation>();
        List<BusinessLocation> _oBusinessLocations = new List<BusinessLocation>();

        string _sErrorMessage = "";
        string _sSQL = "";
        #endregion
        #region Functions
        private void MakeSQL(string Arguments)
        {
            _sSQL = "";
            _sSQL = "SELECT * FROM View_AccountingRatioSetup";
            string sCode = (Arguments.Split(';')[1].Split('~')[0] == null) ? "" : Arguments.Split(';')[1].Split('~')[0];
            string sName = (Arguments.Split(';')[1].Split('~')[1] == null) ? "" : Arguments.Split(';')[1].Split('~')[1];
            //string sAccountingRatioSetupIDs = _bForPrint ? (Arguments.Split(';')[1].Split('~')[1] == null) ? "" : Arguments.Split(';')[1].Split('~')[1] : "";
            string sSQL = "";


            #region Code
            if (sCode != null)
            {
                if (sCode != "")
                {
                    Global.TagSQL(ref sSQL);
                    sSQL = sSQL + " Code LIKE '%" + sCode + "%' ";
                }
            }
            #endregion
            #region Name
            if (sName != null)
            {
                if (sName != "")
                {
                    Global.TagSQL(ref sSQL);
                    sSQL = sSQL + " Name LIKE '%" + sName + "%' ";
                }
            }
            #endregion

            if (sSQL != "")
            { _sSQL = _sSQL + sSQL; }

        }
        #endregion

        #region Used Code
        public ActionResult ViewAccountingRatioSetups(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            
            _oAccountingRatioSetups = new List<AccountingRatioSetup>();
            _oAccountingRatioSetups = AccountingRatioSetup.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);

            return View(_oAccountingRatioSetups);
        }

        public ActionResult ViewAccountingRatioSetup(int id)
        {
            _oAccountingRatioSetup = new AccountingRatioSetup();
            if (id > 0)
            {
                _oAccountingRatioSetup = _oAccountingRatioSetup.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            _oAccountingRatioSetup.Divisibles = AccountingRatioSetupDetail.GetsForARS(_oAccountingRatioSetup.AccountingRatioSetupID, true, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oAccountingRatioSetup.Dividers = AccountingRatioSetupDetail.GetsForARS(_oAccountingRatioSetup.AccountingRatioSetupID, false, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.RatioFormats = EnumObject.jGets(typeof(EnumRatioFormat));
            ViewBag.RatioSetups = EnumObject.jGets(typeof(EnumRatioSetupType));
            return View(_oAccountingRatioSetup);
        }

        [HttpPost]
        public JsonResult PrepareARSDs(AccountingRatioSetupDetail oAccountingRatioSetupDetail)
        {
            List<AccountingRatioSetupDetail> oAccountingRatioSetupDetails = new List<AccountingRatioSetupDetail>();
            oAccountingRatioSetupDetail.SubGroupName = oAccountingRatioSetupDetail.SubGroupName == null ? "" : oAccountingRatioSetupDetail.SubGroupName;
          
            if(oAccountingRatioSetupDetail.RatioSetupTypeInt==(int)EnumRatioSetupType.GenrealSetup)
            {
                ChartsOfAccount oChartsOfAccount = new ChartsOfAccount();
                oChartsOfAccount.AccountType = EnumAccountType.SubGroup;
                oChartsOfAccount.AccountHeadCodeName = oAccountingRatioSetupDetail.SubGroupName;
                List<ChartsOfAccount> oChartsOfAccounts = new List<ChartsOfAccount>();
                oChartsOfAccounts = ChartsOfAccount.GetsByComponentAndCodeName(oChartsOfAccount, ((User)Session[SessionInfo.CurrentUser]).UserID);

                foreach (ChartsOfAccount oItem in oChartsOfAccounts)
                {
                    AccountingRatioSetupDetail oARSD = new AccountingRatioSetupDetail();
                    oARSD.SubGroupID = oItem.AccountHeadID;
                    oARSD.SubGroupName = oItem.AccountHeadName;
                    oARSD.SubGroupCode = oItem.AccountCode;
                    oARSD.IsDivisible = oAccountingRatioSetupDetail.IsDivisible;
                    oAccountingRatioSetupDetails.Add(oARSD);
                }
            }
            else
            {
               List<EnumObject> oComponentsRatios = new List<EnumObject>();
                oComponentsRatios = EnumObject.jGets(typeof(EnumRatioComponent));
                foreach(EnumObject oItem in oComponentsRatios)
                {
                    if (oItem.id > 0)
                    {
                        AccountingRatioSetupDetail oARSD = new AccountingRatioSetupDetail();
                        oARSD.SubGroupName = oItem.Value;
                        oARSD.RatioComponent = (EnumRatioComponent)oItem.id;
                        oARSD.RatioComponentInt = oItem.id;
                        oARSD.IsDivisible = oAccountingRatioSetupDetail.IsDivisible;
                        oAccountingRatioSetupDetails.Add(oARSD);
                    }
                }
            }
           
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oAccountingRatioSetupDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Gets(AccountingRatioSetup oAccountingRatioSetup)
        {
            List<AccountingRatioSetup> oAccountingRatioSetups = new List<AccountingRatioSetup>();
            oAccountingRatioSetup.ErrorMessage = oAccountingRatioSetup.ErrorMessage == null ? "" : oAccountingRatioSetup.ErrorMessage;
            this.MakeSQL(oAccountingRatioSetup.ErrorMessage);

            oAccountingRatioSetups = AccountingRatioSetup.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oAccountingRatioSetups);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Get(AccountingRatioSetup oAccountingRatioSetup)
        {
            _oAccountingRatioSetup = new AccountingRatioSetup();
            try
            {
                if (oAccountingRatioSetup.AccountingRatioSetupID > 0)
                {
                    _oAccountingRatioSetup = _oAccountingRatioSetup.Get(oAccountingRatioSetup.AccountingRatioSetupID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                _oAccountingRatioSetup = new AccountingRatioSetup();
                _oAccountingRatioSetup.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oAccountingRatioSetup);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        public Image GetDBCompanyLogo(int id)
        {
            #region From DB
            Company oCompany = new Company();
            oCompany = oCompany.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
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
            #endregion
        }


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



        [HttpPost]
        public JsonResult Save(AccountingRatioSetup oAccountingRatioSetup)
        {
            _oAccountingRatioSetup = new AccountingRatioSetup();
            oAccountingRatioSetup.Name = oAccountingRatioSetup.Name == null ? "" : oAccountingRatioSetup.Name;
            oAccountingRatioSetup.DivisibleName = oAccountingRatioSetup.DivisibleName == null ? "" : oAccountingRatioSetup.DivisibleName;
            oAccountingRatioSetup.DividerName = oAccountingRatioSetup.DividerName == null ? "" : oAccountingRatioSetup.DividerName;
            oAccountingRatioSetup.Remarks = oAccountingRatioSetup.Remarks == null ? "" : oAccountingRatioSetup.Remarks;
            try
            {
                _oAccountingRatioSetup = oAccountingRatioSetup;
                _oAccountingRatioSetup = _oAccountingRatioSetup.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oAccountingRatioSetup = new AccountingRatioSetup();
                _oAccountingRatioSetup.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oAccountingRatioSetup);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(AccountingRatioSetup oAccountingRatioSetup)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oAccountingRatioSetup.Delete(oAccountingRatioSetup.AccountingRatioSetupID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }



        #region Set Ratio SessionData
        [HttpPost]
        public ActionResult SetARSSessionData(SP_RatioSetup oSP_RatioSetup)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oSP_RatioSetup);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Ratio
        public ActionResult ViewRatioAnalysis(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            AccountingSession oAccountingSession = new AccountingSession();
            SP_RatioSetup oSP_RatioSetup = new SP_RatioSetup();
            List<SP_RatioSetup> oSP_RatioSetups = new List<SP_RatioSetup>();
            oSP_RatioSetup = (SP_RatioSetup)Session[SessionInfo.ParamObj];
            BusinessUnit oBusinessUnit = new BusinessUnit();
            IncomeStatement oIncomeStatement = new IncomeStatement();
            if (oSP_RatioSetup != null)
            {
                if (oSP_RatioSetup.IsForCurrentDate)
                {
                    oAccountingSession = AccountingSession.GetRunningAccountingYear(((User)Session[SessionInfo.CurrentUser]).UserID);
                    oSP_RatioSetup.StartDate = oAccountingSession.StartDate;
                    oSP_RatioSetup.EndDate = DateTime.Now;
                }
                oSP_RatioSetups = SP_RatioSetup.Gets(oSP_RatioSetup.AccountingRatioSetupID, oSP_RatioSetup.StartDate, oSP_RatioSetup.EndDate, oSP_RatioSetup.BusinessUnitID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oSP_RatioSetups.Where(x=>x.RatioSetupType==EnumRatioSetupType.BasedOnStatement).ToList().Count>0)
                {
                    //if (oBusinessUnit.BusinessUnitID > 0) { oCompany.Name = oBusinessUnit.Name; }
                    List<CIStatementSetup> oCIStatementSetups = new List<CIStatementSetup>();
                    oCIStatementSetups = (List<CIStatementSetup>)Session[SessionInfo.SearchData]; //oTempController.PrepareComprehensiveIncome(oIncomeStatement, oBusinessUnit);
                    this.Session.Add(SessionInfo.SearchData, "");
                    foreach (SP_RatioSetup oIem in oSP_RatioSetups)
                    {
                        if (oIem.RatioSetupType == EnumRatioSetupType.BasedOnStatement)
                        {
                            oIem.DivisibleAmount = oCIStatementSetups.Where(x=>x.RatioComponent==(EnumRatioComponent)oIem.DivisibleComponent).FirstOrDefault().AccountHeadValue;
                            oIem.DividerAmount = oCIStatementSetups.Where(x => x.RatioComponent == (EnumRatioComponent)oIem.DividerComponent).FirstOrDefault().AccountHeadValue; 
                        }
                    }
                }
                oSP_RatioSetup.SP_RatioSetups = new List<SP_RatioSetup>();
                oSP_RatioSetup.SP_RatioSetups = oSP_RatioSetups;
            }
            else
            {
                oSP_RatioSetup = new SP_RatioSetup();
                oSP_RatioSetup.SP_RatioSetups = new List<SP_RatioSetup>();
            }
            ViewBag.Ratios = AccountingRatioSetup.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);            
         
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oBusinessUnit = oBusinessUnit.Get(oSP_RatioSetup.BusinessUnitID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (oBusinessUnit.BusinessUnitID > 0) { oCompany.Name = oBusinessUnit.Name; }
            ViewBag.Company = oCompany;

            ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
            oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            ViewBag.BUs = BusinessUnit.GetsPermittedBU(oUser, (int)Session[SessionInfo.currentUserID]);

            this.Session.Remove(SessionInfo.ParamObj);
            return View(oSP_RatioSetup);
        }


        public ActionResult PrintRatioAnalysis(string Params)
        {
            int nAccountingRatioSetupID = Params.Split('~')[0] == null ? 0 : Params.Split('~')[0] == "" ? 0 : Convert.ToInt32(Params.Split('~')[0]);
            DateTime dStartDate = Params.Split('~')[1] == null ? DateTime.Now : Params.Split('~')[1] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[1]);
            DateTime dEndDate = Params.Split('~')[2] == null ? DateTime.Now : Params.Split('~')[2] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[2]);
            int nBusinessUnitID = Params.Split('~')[3] == null ? 0 : Params.Split('~')[3] == "" ? 0 : Convert.ToInt32(Params.Split('~')[3]);
            byte[] abytes = null;

            #region Check Authorize Business Unit
            if (!BusinessUnit.IsPermittedBU(nBusinessUnitID, (int)Session[SessionInfo.currentUserID]))
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] aErrorMessagebytes = oReport.PrepareReport("You are not authorize for selected business unit!");
                return File(aErrorMessagebytes, "application/pdf");
            }
            #endregion

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);


            List<SP_RatioSetup> oSP_RatioSetups = new List<SP_RatioSetup>();
            SP_RatioSetup oSP_RatioSetup = new SP_RatioSetup();
            oSP_RatioSetup.AccountingRatioSetupID = nAccountingRatioSetupID;
            oSP_RatioSetup.BusinessUnitID = nBusinessUnitID;
            oSP_RatioSetup.StartDate = dStartDate;
            oSP_RatioSetup.EndDate = dEndDate;
            oSP_RatioSetups = SP_RatioSetup.Gets(nAccountingRatioSetupID, dStartDate, dEndDate, nBusinessUnitID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oSP_RatioSetup.SP_RatioSetups = oSP_RatioSetups;

            rptRatioAnalysis orptRatioAnalysis = new rptRatioAnalysis();
            abytes = orptRatioAnalysis.PrepareReport(oSP_RatioSetup, oCompany);

            return File(abytes, "application/pdf");
        }

        public void ExportRatioAnalysisToExcel(string Params)
        {
            #region Dataget
            int nAccountingRatioSetupID = Params.Split('~')[0] == null ? 0 : Params.Split('~')[0] == "" ? 0 : Convert.ToInt32(Params.Split('~')[0]);
            DateTime dStartDate = Params.Split('~')[1] == null ? DateTime.Now : Params.Split('~')[1] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[1]);
            DateTime dEndDate = Params.Split('~')[2] == null ? DateTime.Now : Params.Split('~')[2] == "" ? DateTime.Now : Convert.ToDateTime(Params.Split('~')[2]);
            int nBusinessUnitID = Params.Split('~')[3] == null ? 0 : Params.Split('~')[3] == "" ? 0 : Convert.ToInt32(Params.Split('~')[3]);
            byte[] abytes = null;

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);


            List<SP_RatioSetup> oSP_RatioSetups = new List<SP_RatioSetup>();
            SP_RatioSetup oSP_RatioSetup = new SP_RatioSetup();
            oSP_RatioSetup.AccountingRatioSetupID = nAccountingRatioSetupID;
            oSP_RatioSetup.BusinessUnitID = nBusinessUnitID;
            oSP_RatioSetup.StartDate = dStartDate;
            oSP_RatioSetup.EndDate = dEndDate;
            oSP_RatioSetups = SP_RatioSetup.Gets(nAccountingRatioSetupID, dStartDate, dEndDate, nBusinessUnitID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            #endregion
            #region Export Excel
            int nRowIndex = 2, nStartRow = 2, nEndRow = 0;
            ExcelRange cell;
            ExcelRange HeaderCell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;
            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Ratio Analysis");
                sheet.Name = "Ratio Analysis";
                sheet.Column(2).Width = 25;
                sheet.Column(3).Width = 35;
                sheet.Column(4).Width = 8;
                sheet.Column(5).Width = 35;
                sheet.Column(6).Width = 25;
                


                #region Report Header
                sheet.Cells[nRowIndex, 2, nRowIndex, 6].Merge = true;
                cell = sheet.Cells[nRowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                sheet.Cells[nRowIndex, 2, nRowIndex, 6].Merge = true;
                cell = sheet.Cells[nRowIndex, 2]; cell.Value = oCompany.Address; cell.Style.Font.Bold = false;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                sheet.Cells[nRowIndex, 2, nRowIndex, 6].Merge = true;
                cell = sheet.Cells[nRowIndex, 2]; cell.Value = oCompany.Phone + ";  " + oCompany.Email + ";  " + oCompany.WebAddress; cell.Style.Font.Bold = false;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 2;

                sheet.Cells[nRowIndex, 2, nRowIndex, 6].Merge = true;
                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "Ratio Analysis"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                sheet.Cells[nRowIndex, 2, nRowIndex, 6].Merge = true;
                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "Date Range : \""+ dStartDate.ToString("dd MMM yyyy")+"\" to \""+dEndDate.ToString("dd MMM yyyy")+"\""; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 2;
                #endregion

                

                #region Report Data
               foreach (SP_RatioSetup oItem in oSP_RatioSetups)
                {
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex + 1, 2]; cell.Merge = true;
                    cell.Value = oItem.Name+" = "; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "General";
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 3];
                    cell.Value = oItem.DividerName; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "Text";
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thick; border.Top.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 4, nRowIndex + 1, 4]; cell.Merge = true;
                    cell.Value = oItem.Separator; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 5];
                    cell.Value = oItem.DivisibleAmount; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thick; border.Top.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6, nRowIndex + 1, 6]; cell.Merge = true;
                    if (oItem.RatioFormat == EnumRatioFormat.Ratio)
                    {
                        cell.Value = oItem.RatioSt;
                    }
                    else
                    {
                        cell.Value = oItem.PercentageSt;
                    }
                    cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nRowIndex = nRowIndex + 1;

                    cell = sheet.Cells[nRowIndex, 3];
                    cell.Value = oItem.DividerName; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "Text";
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 5];
                    cell.Value = oItem.DivisibleAmount; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin;

                    nRowIndex = nRowIndex + 3;

                }
                #endregion

                

                cell = sheet.Cells[1, 1, nRowIndex, 7];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=Ratio_Analysis.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }
        #endregion

        #region Ration Notes
        public ActionResult ViewNotesOfRationAnalysis(int buid, string sdt, string edt, int rasid, bool bisdivisible)
        {

            #region Check Authorize Business Unit
            if (!BusinessUnit.IsPermittedBU(buid, (int)Session[SessionInfo.currentUserID]))
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] aErrorMessagebytes = oReport.PrepareReport("You are not authorize for selected business unit!");
                return File(aErrorMessagebytes, "application/pdf");
            }
            #endregion

            //buid means businessunitid
            //sdt means start date
            //edt means end date
            //rasid means ration analysis setup id
            if (sdt == null || sdt == null) { sdt = DateTime.Today.ToString("dd MMM yyyy"); }
            if (edt == null || edt == null) { edt = DateTime.Today.ToString("dd MMM yyyy"); }
            DateTime dStartDate = Convert.ToDateTime(sdt);
            DateTime dEndDate = Convert.ToDateTime(edt);

            Company oCompany = new Company();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            BalanceSheet oBalanceSheet = new BalanceSheet();
            List<BalanceSheet> oBalanceSheets = new List<BalanceSheet>();
            AccountingRatioSetup oAccountingRatioSetup = new AccountingRatioSetup();
            oBalanceSheets = BalanceSheet.GetsForRationAnalysis(buid, dEndDate, rasid, bisdivisible, 0, (int)Session[SessionInfo.currentUserID]);
            oBalanceSheet.BalanceSheets = oBalanceSheets;

            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oBusinessUnit = oBusinessUnit.Get(buid, (int)Session[SessionInfo.currentUserID]);
            oAccountingRatioSetup = oAccountingRatioSetup.Get(rasid, (int)Session[SessionInfo.currentUserID]);
            if (oBusinessUnit.BusinessUnitID > 0)
            {
                oCompany.Name = oBusinessUnit.Name;
            }
            oBalanceSheet.BUID = buid;
            oBalanceSheet.Company = oCompany;
            oBalanceSheet.StartDate = dStartDate;
            oBalanceSheet.EndDate = dEndDate;
            if (bisdivisible)
            {
                oBalanceSheet.ReportCaption = oAccountingRatioSetup.Name + "=>" + oAccountingRatioSetup.DivisibleName;
            }
            else
            {
                oBalanceSheet.ReportCaption = oAccountingRatioSetup.Name + "=>" + oAccountingRatioSetup.DividerName;
            }
            return View(oBalanceSheet);
        }
        #endregion

        #endregion



    }
}
