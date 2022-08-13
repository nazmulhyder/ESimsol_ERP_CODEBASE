using System;
using System.Collections.Generic;
using System.Data;
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
using ReportManagement;
using ESimSol.BusinessObjects.ReportingObject;
using OfficeOpenXml;
using OfficeOpenXml.Style;


namespace ESimSolFinancial.Controllers
{
    public class RSInQCManageController : Controller
    {
        #region 
        string _sErrorMessage = "";
        string _sDateRange = "";
        RSInQCManage _oRSInQCManage = new RSInQCManage();
        List<RSInQCManage> _oRSInQCManages = new List<RSInQCManage>();

        public ActionResult ViewRSInQCManages(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.FabricPO).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oRSInQCManages = new List<RSInQCManage>();
            _oRSInQCManages = RSInQCManage.Gets("Select top(100)* from View_RSInQCDetail_Manage where isnull(ManagedLotID,0)<=0 order by DBServerDateTime", ((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<WorkingUnit> oWUs = new List<WorkingUnit>();
            oWUs = WorkingUnit.BUWiseGets(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            //List<Location> oLocations = new List<Location>();
            //oLocations = Location.GetsIncludingStore(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.WorkingUnits = oWUs;

            string sSQL = "Select * from View_RSInQCSetup Where Activity=1 and YarnType not in (" + (int)EnumYarnType.FreshDyedYarn + "," + (int)EnumYarnType.DyedYarnOne + "," + (int)EnumYarnType.DyedYarnTwo+"," + (int)EnumYarnType.DyedYarnThree + ")";
            ViewBag.RSInQCSetups = RSInQCSetup.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.DUOrderSetups = DUOrderSetup.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.BUID = buid;
            ViewBag.BusinessUnit = oBusinessUnit;
            return View(_oRSInQCManages);
        }
        [HttpPost]
        public JsonResult Save(List<RSInQCManage> oRSInQCManages)
        {
            _oRSInQCManages = new List<RSInQCManage>();
            try
            {
                _oRSInQCManages = RSInQCManage.IUD(oRSInQCManages, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                _oRSInQCManage = new RSInQCManage();
                _oRSInQCManage.ErrorMessage = ex.Message;
                _oRSInQCManages = new List<RSInQCManage>();
                _oRSInQCManages.Add(_oRSInQCManage);
                    
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oRSInQCManages);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region Get Company Logo
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
        #endregion

        #endregion

        #region Advance Search

        #region HttpGet For Search
        [HttpPost]
        public JsonResult AdvanchSearch(RSInQCManage oRSInQCManage)
        {
            _oRSInQCManages = new List<RSInQCManage>();
            try
            {
                string sSQL = GetSQL(oRSInQCManage.ErrorMessage);
                _oRSInQCManages = RSInQCManage.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oRSInQCManage = new RSInQCManage();
                _oRSInQCManage.ErrorMessage = ex.Message;
            }

            var jsonResult = Json(_oRSInQCManages, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        private string GetSQL(string sParams)
        {
            string sReturn1 = "SELECT * FROM View_RSInQCDetail_Manage ";
            string sReturn = "";

            if (!string.IsNullOrEmpty(sParams) && sParams.Split('~').Length>0)
            {
               #region Set Values

                int nCboRSDate = 0;
                DateTime dFromRSDate = DateTime.Today;
                DateTime dToRSDate = DateTime.Today;
                int nCboQCDate = 0;
                DateTime dFromQCDate = DateTime.Today;
                DateTime dToQCDate = DateTime.Today;
                int nCboManageDate = 0;
                DateTime dFromManageDate = DateTime.Today;
                DateTime dToManageDate = DateTime.Today;
                string sOrderNo = Convert.ToString(sParams.Split('~')[0]);
                string sRSNo = Convert.ToString(sParams.Split('~')[1]);
                int nOrderType = Convert.ToInt32(sParams.Split('~')[2]);
                int nWorkingUnitID = Convert.ToInt32(sParams.Split('~')[3]);
                nCboRSDate = Convert.ToInt32(sParams.Split('~')[4]);
                if (nCboRSDate > 0)
                {
                    dFromRSDate = Convert.ToDateTime(sParams.Split('~')[5]);
                    dToRSDate = Convert.ToDateTime(sParams.Split('~')[6]);
                }
                nCboQCDate = Convert.ToInt32(sParams.Split('~')[7]);
                if (nCboQCDate > 0)
                {
                    dFromQCDate = Convert.ToDateTime(sParams.Split('~')[8]);
                    dToQCDate = Convert.ToDateTime(sParams.Split('~')[9]);
                }
                nCboManageDate = Convert.ToInt32(sParams.Split('~')[10]);
                if (nCboManageDate > 0)
                {
                    dFromManageDate = Convert.ToDateTime(sParams.Split('~')[11]);
                    dToManageDate = Convert.ToDateTime(sParams.Split('~')[12]);
                }
                string sQCTypeIDs = Convert.ToString(sParams.Split('~')[13]);
               
               #endregion
               #region Make Query
                #region sOrder NO
                if (!string.IsNullOrEmpty(sOrderNo))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "OrderNo like '%" + sOrderNo + "%'";
                }
                #endregion
                #region RS NO
                if (!string.IsNullOrEmpty(sRSNo))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "RouteSheetNo like '%" + sRSNo + "%'";
                }
                #endregion
                #region Order Type
                if (!String.IsNullOrEmpty(nOrderType.ToString()) && nOrderType>0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "OrderType ="+ nOrderType;
                }
                #endregion
                #region nWorkingUnitID
                if (!String.IsNullOrEmpty(nWorkingUnitID.ToString()) && nWorkingUnitID > 0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "WorkingUnitID =" + nWorkingUnitID;
                }
                #endregion
                #region RouteSheetDate Date (RSDate)
                if (nCboRSDate != (int)EnumCompareOperator.None)
                {
                    Global.TagSQL(ref sReturn);
                    if (nCboRSDate == (int)EnumCompareOperator.EqualTo)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),RouteSheetDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromRSDate.ToString("dd MMM yyyy") + "',106)) ";
                        _sDateRange = "Date: " + dFromRSDate.ToString("dd MMM yyyy");
                    }
                    else if (nCboRSDate == (int)EnumCompareOperator.NotEqualTo)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),RouteSheetDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromRSDate.ToString("dd MMM yyyy") + "',106)) ";
                        _sDateRange = "Date: NotEqualTo->" + dFromRSDate.ToString("dd MMM yyyy");
                    }
                    else if (nCboRSDate == (int)EnumCompareOperator.GreaterThan)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),RouteSheetDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromRSDate.ToString("dd MMM yyyy") + "',106)) ";
                        _sDateRange = "Date: GreaterThen->" + dFromRSDate.ToString("dd MMM yyyy");
                    }
                    else if (nCboRSDate == (int)EnumCompareOperator.SmallerThan)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),RouteSheetDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromRSDate.ToString("dd MMM yyyy") + "',106)) ";
                        _sDateRange = "Date: SmallerThen->" + dFromRSDate.ToString("dd MMM yyyy");
                    }
                    else if (nCboRSDate == (int)EnumCompareOperator.Between)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),RouteSheetDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromRSDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToRSDate.ToString("dd MMM yyyy") + "',106)) ";
                        _sDateRange = "Date: From " + dFromRSDate.ToString("dd MMM yyyy") + " To " + dToRSDate.ToString("dd MMM yyyy");
                    }
                    else if (nCboRSDate == (int)EnumCompareOperator.NotBetween)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),RouteSheetDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromRSDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToRSDate.ToString("dd MMM yyyy") + "',106)) ";
                        _sDateRange = "Date: NotBetween " + dFromRSDate.ToString("dd MMM yyyy") + " To " + dToRSDate.ToString("dd MMM yyyy");
                    }
                }
                #endregion
                #region QC Date
                if (nCboQCDate != (int)EnumCompareOperator.None)
                {
                    string stemp = "RouteSheetID in (Select RouteSheetID from RouteSheetHistory where CurrentStatus=13 and";

                    Global.TagSQL(ref sReturn);
                    if (nCboQCDate == (int)EnumCompareOperator.EqualTo)
                    {
                        sReturn = sReturn + stemp+ " CONVERT(DATE,CONVERT(VARCHAR(12),EventTime,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromQCDate.ToString("dd MMM yyyy") + "',106)) )";
                    }
                    else if (nCboQCDate == (int)EnumCompareOperator.NotEqualTo)
                    {
                        sReturn = sReturn + stemp + " CONVERT(DATE,CONVERT(VARCHAR(12),EventTime,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromQCDate.ToString("dd MMM yyyy") + "',106)) ) ";
                    }
                    else if (nCboQCDate == (int)EnumCompareOperator.GreaterThan)
                    {
                        sReturn = sReturn + stemp + " CONVERT(DATE,CONVERT(VARCHAR(12),EventTime,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromQCDate.ToString("dd MMM yyyy") + "',106))) ";
                    }
                    else if (nCboQCDate == (int)EnumCompareOperator.SmallerThan)
                    {
                        sReturn = sReturn + stemp + " CONVERT(DATE,CONVERT(VARCHAR(12),EventTime,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromQCDate.ToString("dd MMM yyyy") + "',106)) )";
                    }
                    else if (nCboQCDate == (int)EnumCompareOperator.Between)
                    {
                        sReturn = sReturn + stemp + " CONVERT(DATE,CONVERT(VARCHAR(12),EventTime,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromQCDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToQCDate.ToString("dd MMM yyyy") + "',106))) ";
                    }
                    else if (nCboQCDate == (int)EnumCompareOperator.NotBetween)
                    {
                        sReturn = sReturn + stemp + " CONVERT(DATE,CONVERT(VARCHAR(12),EventTime,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromQCDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToQCDate.ToString("dd MMM yyyy") + "',106)) )";
                    }
                }
            #endregion
                #region Manage Date
                if (nCboManageDate != (int)EnumCompareOperator.None)
                {
                    Global.TagSQL(ref sReturn);
                    if (nCboManageDate == (int)EnumCompareOperator.EqualTo)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ManageDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromManageDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (nCboManageDate == (int)EnumCompareOperator.NotEqualTo)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ManageDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromManageDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (nCboManageDate == (int)EnumCompareOperator.GreaterThan)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ManageDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromManageDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (nCboManageDate == (int)EnumCompareOperator.SmallerThan)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ManageDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromManageDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (nCboManageDate == (int)EnumCompareOperator.Between)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ManageDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromManageDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToManageDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (nCboManageDate == (int)EnumCompareOperator.NotBetween)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ManageDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromManageDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToManageDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                }
                #endregion
                #region RSInQCSetupID (QCTypes)
                if (!string.IsNullOrEmpty(sQCTypeIDs))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "RSInQCSetupID IN (" + sQCTypeIDs + ")";
                }
                #endregion
                #endregion
            }
            sReturn = sReturn1 + sReturn;
            return sReturn;
        }
        #endregion

        #endregion
           
        #region Print
  
        //public ActionResult PrintPreview(int nID, double nts)
        //{
        //    #region Company Info
        //    Company oCompany = new Company();
        //    oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    oCompany.CompanyLogo = GetCompanyLogo(oCompany);
        //    _oRSInQCManage = _oRSInQCManage.Get(nID, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    _oRSInQCManage.RSInQCManageDetails = RSInQCManageDetail.Gets(nID, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    List<FebricDeliverySchedule> _oFebricDeliverySchedules = FebricDeliverySchedule.GetsFSCID(nID, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    List<RSInQCManageNote> oRSInQCManageNotes = RSInQCManageNote.Gets(nID, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    _oRSInQCManage.PINo = this.GetPINos(_oRSInQCManage.RSInQCManageDetails);

        //    AttachDocument oAttachDocument = new AttachDocument();
        //    oAttachDocument = oAttachDocument.GetUserSignature(_oRSInQCManage.ContractorID, (int)Session[SessionInfo.currentUserID]);
        //    _oRSInQCManage.ConImage = this.GetContractorImage(oAttachDocument);

        //    #endregion
        //    BusinessUnit oBusinessUnit = new BusinessUnit();
        //    oBusinessUnit = oBusinessUnit.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    string _sMessage = "";
        //    rptRSInQCManage oReport = new rptRSInQCManage();
        //    byte[] abytes = oReport.PrepareReport(_oRSInQCManage, oCompany, oBusinessUnit, _oFebricDeliverySchedules, oRSInQCManageNotes);
        //    return File(abytes, "application/pdf");

        //}
        #endregion

        #region Excel
        public void PrintXL_RSInQCManage(string sTempString)
        {
            string _sErrorMesage = "";
            try
            {
                _oRSInQCManages = new List<RSInQCManage>();
                try
                {
                    string sSQL = GetSQL(sTempString);
                    _oRSInQCManages = RSInQCManage.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                catch (Exception ex)
                {
                    _oRSInQCManage = new RSInQCManage();
                    _oRSInQCManage.ErrorMessage = ex.Message;
                }
                if (_oRSInQCManages.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oRSInQCManages = new List<RSInQCManage>();
                _sErrorMesage = ex.Message;
            }

            if (_sErrorMesage == "")
            {
              
                BusinessUnit oBU = new BusinessUnit();
                oBU = oBU.GetByType((int)EnumBusinessUnitType.Dyeing, (int)Session[SessionInfo.currentUserID]);
               

                #region Print XL

                  int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 9, nColumn=1,  nCount=0, nImportLCID=0;
                  ExcelRange cell;
                  ExcelRange HeaderCell;
                  ExcelFill fill;
                  OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("YarnManage");
                    sheet.Name = "Yarn Manage";
                    sheet.Column(++nColumn).Width = 10; //SL No
                    sheet.Column(++nColumn).Width = 20; //DL No
                    sheet.Column(++nColumn).Width = 20; //OrderNo
                    sheet.Column(++nColumn).Width = 30; //YranType
                    sheet.Column(++nColumn).Width = 15; //Qty
                    sheet.Column(++nColumn).Width = 20; //Type
                    sheet.Column(++nColumn).Width = 20; //Store
                    sheet.Column(++nColumn).Width = 15; //ManageDate
                    //   nEndCol = 9;

                    #region Report Header
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol-4]; cell.Merge = true;
                    cell.Value = oBU.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                   
                    cell = sheet.Cells[nRowIndex, nStartCol+5, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = "Yarn Manage"; cell.Style.Font.Bold = true;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Address & Date
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol-4]; cell.Merge = true;
                    cell.Value = oBU.PringReportHead; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                   
                    cell = sheet.Cells[nRowIndex, nStartCol+5, nRowIndex, nEndCol]; cell.Merge = true;
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
                    nColumn=1;
                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "DL No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Order No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Yran Type"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Qty"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Type"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Store"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Manage Date"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nRowIndex++;
                    #endregion

                    #region Data
                    int nImportPIID=0; double nGrandTotalQty=0,nGrandTotalAmount=0;
                    foreach (RSInQCManage oItem in _oRSInQCManages)
                    {
                        nColumn = 1;
                        nCount++;
                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = nCount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.OrderNo.ToString(); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.RouteSheetNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn];  cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn];  cell.Value = oItem.Qty; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "# #,##0.00";
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn];  cell.Value = oItem.QCSetupName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "" + oItem.WUName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "" + oItem.ManageDateStr; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex++;
                    }
                    #endregion

                    #region Total
                    cell = sheet.Cells[nRowIndex, nStartCol,nRowIndex,nEndCol-4]; cell.Value = "" + "Total"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Merge = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    double nValue = _oRSInQCManages.Select(c => c.Qty).Sum();
                    cell = sheet.Cells[nRowIndex, nEndCol-3]; cell.Value = nValue; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "# #,##0.00";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nEndCol - 2]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nEndCol - 1]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
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
                    Response.AddHeader("content-disposition", "attachment; filename=YarnManage.xlsx");
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
        #endregion
    }
}

