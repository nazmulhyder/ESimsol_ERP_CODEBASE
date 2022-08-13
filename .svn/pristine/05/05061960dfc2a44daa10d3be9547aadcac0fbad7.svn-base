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
    public class CommercialInvoiceRegisterController : Controller
    {
        #region Declaration
        string _sDateRange = "";
        string _sErrorMesage = "";
        CommercialInvoiceRegister _oCommercialInvoiceRegister = new CommercialInvoiceRegister();
        List<CommercialInvoiceRegister> _oCommercialInvoiceRegisters = new List<CommercialInvoiceRegister>();
        #endregion

        #region Actions
        public ActionResult ViewCommercialInvoiceRegister(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            CommercialInvoiceRegister oCommercialInvoiceRegister = new CommercialInvoiceRegister();

            #region Report Layout
            List<EnumObject> oReportLayouts = new List<EnumObject>();
            List<EnumObject> oTempReportLayouts = new List<EnumObject>();
            oTempReportLayouts = EnumObject.jGets(typeof(EnumReportLayout));
            foreach (EnumObject oItem in oTempReportLayouts)
            {
                if ((EnumReportLayout)oItem.id == EnumReportLayout.DateWiseDetails || (EnumReportLayout)oItem.id == EnumReportLayout.PartyWiseDetails || (EnumReportLayout)oItem.id == EnumReportLayout.InvoiceWise)
                {
                    oReportLayouts.Add(oItem);
                }
            }
            #endregion

            ViewBag.BUID = buid;
            ViewBag.ReportLayouts = oReportLayouts;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            return View(oCommercialInvoiceRegister);
        }
        public ActionResult ViewCommercialInvoiceRegisterMgt(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            CommercialInvoiceRegister oCommercialInvoiceRegister = new CommercialInvoiceRegister();
            ViewBag.BUID = buid;
            ViewBag.BusinessUnits = BusinessUnit.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.Buyers = Contractor.GetsByNamenType("",((int)EnumContractorType.Buyer).ToString(),buid, (int)Session[SessionInfo.currentUserID]);
            return View(oCommercialInvoiceRegister);
        }
        [HttpPost]
        public ActionResult SetSessionSearchCriteria(CommercialInvoiceRegister oCommercialInvoiceRegister)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oCommercialInvoiceRegister);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Print(double ts)
        {
            CommercialInvoiceRegister oCommercialInvoiceRegister = new CommercialInvoiceRegister();
            try
            {
                _sErrorMesage = "";
                _oCommercialInvoiceRegisters = new List<CommercialInvoiceRegister>();
                oCommercialInvoiceRegister = (CommercialInvoiceRegister)Session[SessionInfo.ParamObj];

                string sSQL = this.GetSQL(oCommercialInvoiceRegister);
                _oCommercialInvoiceRegisters = CommercialInvoiceRegister.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

                if (_oCommercialInvoiceRegisters.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oCommercialInvoiceRegisters = new List<CommercialInvoiceRegister>();
                _sErrorMesage = ex.Message;
            }

            if (_sErrorMesage == "")
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                BusinessUnit oBU = new BusinessUnit();
                oBU = oBU.Get(_oCommercialInvoiceRegisters.Max(x => x.BUID), (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBU);

                rptCommercialInvoiceRegisters oReport = new rptCommercialInvoiceRegisters();
                byte[] abytes = oReport.PrepareReport(_oCommercialInvoiceRegisters, oCompany, oCommercialInvoiceRegister.ReportLayout, _sDateRange);
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

        #region SearchDetails
        [HttpPost]
        public JsonResult SearchDetails(CommercialInvoiceRegister oCommercialInvoiceRegister)
        {
            _oCommercialInvoiceRegisters = new List<CommercialInvoiceRegister>();
          
            try
            {
                _oCommercialInvoiceRegisters = CommercialInvoiceRegister.Gets(MakeSQL(oCommercialInvoiceRegister), (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oCommercialInvoiceRegister = new CommercialInvoiceRegister();
                _oCommercialInvoiceRegister.ErrorMessage = ex.Message;
            }
            var jsonResult = Json(_oCommercialInvoiceRegisters, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public string MakeSQL(CommercialInvoiceRegister oCommercialInvoiceRegister)
        {
            string sSearchingData = oCommercialInvoiceRegister.SearchingData;
            bool diSearcingDate= Convert.ToBoolean(sSearchingData.Split('~')[0]);
            DateTime dInvoiceStartDate = Convert.ToDateTime(sSearchingData.Split('~')[1]);
            DateTime dInoviceEndDate = Convert.ToDateTime(sSearchingData.Split('~')[2]);
            bool bIsYesGSP = Convert.ToBoolean(sSearchingData.Split('~')[3]);
            bool bIsNoGSP = Convert.ToBoolean(sSearchingData.Split('~')[4]);
            bool bIsYesBL = Convert.ToBoolean(sSearchingData.Split('~')[5]);
            bool bIsNoBL = Convert.ToBoolean(sSearchingData.Split('~')[6]);
            bool bIsYesIC = Convert.ToBoolean(sSearchingData.Split('~')[7]);
            bool bIsNoIC = Convert.ToBoolean(sSearchingData.Split('~')[8]);
            string sSQL = "SELECT * FROM View_CommercialInvoiceRegister ", sWhereCluse = "";
            #region BusinessUnit
            if (oCommercialInvoiceRegister.BUID > 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " BUID =" + oCommercialInvoiceRegister.BUID.ToString();
            }
            #endregion

            #region OrderRecapNo
            if (oCommercialInvoiceRegister.OrderRecapNo != null && oCommercialInvoiceRegister.OrderRecapNo != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " OrderRecapNo LIKE'%" + oCommercialInvoiceRegister.OrderRecapNo + "%'";
            }
            #endregion

            #region StyleNo
            if (oCommercialInvoiceRegister.StyleNo != null && oCommercialInvoiceRegister.StyleNo != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " StyleNo LIKE'%" + oCommercialInvoiceRegister.StyleNo + "%'";
            }
            #endregion
            #region MasterLCNo
            if (oCommercialInvoiceRegister.MasterLCNo != null && oCommercialInvoiceRegister.MasterLCNo != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " MasterLCNo LIKE'%" + oCommercialInvoiceRegister.MasterLCNo + "%'";
            }
            #endregion

            #region InvoiceNo
            if (oCommercialInvoiceRegister.InvoiceNo != null && oCommercialInvoiceRegister.InvoiceNo != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " InvoiceNo LIKE'%" + oCommercialInvoiceRegister.InvoiceNo + "%'";
            }
            #endregion

            #region BuyerID
            if (oCommercialInvoiceRegister.BuyerID != 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " BuyerID IN (" + oCommercialInvoiceRegister.BuyerID + ")";
            }
            #endregion

            #region InvoiceDate
            if (diSearcingDate)
            {
                DateObject.CompareDateQuery(ref sWhereCluse, "InvoiceDate ", 5, dInvoiceStartDate, dInoviceEndDate);
            }
            #endregion
            #region GSP
            if (bIsYesGSP!=bIsNoGSP)//not matched one eaither true another is false
            {
                Global.TagSQL(ref sWhereCluse);
                if (bIsYesGSP){ sWhereCluse = sWhereCluse + "ISNULL(GSP,0)=1"; }//for yes Ceck
                if (bIsNoGSP){ sWhereCluse = sWhereCluse + "ISNULL(GSP,0)=0"; }//for false check
            }
            #endregion
            #region BL
            if (bIsYesBL != bIsNoBL)//not matched one eaither true another is false
            {
                Global.TagSQL(ref sWhereCluse);
                if (bIsYesBL) { sWhereCluse = sWhereCluse + "ISNULL(BL,0)=1"; }//for yes Ceck
                if (bIsNoBL) { sWhereCluse = sWhereCluse + "ISNULL(BL,0)=0"; }//for false check
            }
            #endregion

            #region BL
            if (bIsYesIC != bIsNoIC)//not matched one eaither true another is false
            {
                Global.TagSQL(ref sWhereCluse);
                if (bIsYesIC) { sWhereCluse = sWhereCluse + "ISNULL(IC,0)=1"; }//for yes Ceck
                if (bIsNoIC) { sWhereCluse = sWhereCluse + "ISNULL(IC,0)=0"; }//for false check
            }
            #endregion

            sSQL = sSQL + sWhereCluse;

            return sSQL;
        }

        #endregion

        #region Print
        public ActionResult PrintRegister(double ts)
        {
            CommercialInvoiceRegister oCommercialInvoiceRegister = (CommercialInvoiceRegister)Session[SessionInfo.ParamObj];
            _oCommercialInvoiceRegisters = CommercialInvoiceRegister.Gets(MakeSQL(oCommercialInvoiceRegister), (int)Session[SessionInfo.currentUserID]);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            rptCInvoiceRegister oReport = new rptCInvoiceRegister();
            byte[] abytes = oReport.PrepareReport(_oCommercialInvoiceRegisters, oCompany);
            return File(abytes, "application/pdf");
        }

        public void PrintRegisterInXL(double ts)
        {
                    
            Company oCompany = new Company();

            try
            {
                _sErrorMesage = "";
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oCommercialInvoiceRegisters = new List<CommercialInvoiceRegister>();
                CommercialInvoiceRegister oCommercialInvoiceRegister = (CommercialInvoiceRegister)Session[SessionInfo.ParamObj];

                _oCommercialInvoiceRegisters = CommercialInvoiceRegister.Gets(MakeSQL(oCommercialInvoiceRegister), (int)Session[SessionInfo.currentUserID]);

                if (_oCommercialInvoiceRegisters.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oCommercialInvoiceRegisters = new List<CommercialInvoiceRegister>();
                _sErrorMesage = ex.Message;
            }
            if (_sErrorMesage == "")
            {

                #region Print XL

                int nRowIndex = 2, nStartCol = 2, nEndCol = 18, nColumn = 1, nCount = 0;
                ExcelRange cell;
                ExcelFill fill;
                OfficeOpenXml.Style.Border border;
                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("CIRegister");
                    sheet.Name = "Invoice Register";
                    sheet.Column(++nColumn).Width = 10; //SL No
                    sheet.Column(++nColumn).Width = 15; //MasterLC No
                    sheet.Column(++nColumn).Width = 15; //Buyer
                    sheet.Column(++nColumn).Width = 30; //Unit
                    sheet.Column(++nColumn).Width = 20; //Style Number
                    sheet.Column(++nColumn).Width = 20; //PO Numbe
                    sheet.Column(++nColumn).Width = 20; //Invoice No
                    sheet.Column(++nColumn).Width = 20; //Ship Mode
                    sheet.Column(++nColumn).Width = 15; //value
                    sheet.Column(++nColumn).Width = 15; //Pcs
                    sheet.Column(++nColumn).Width = 15; //CTN
                    sheet.Column(++nColumn).Width = 15; //Month
                    sheet.Column(++nColumn).Width = 25; //Sending Date
                    sheet.Column(++nColumn).Width = 25; //Accept Date
                    sheet.Column(++nColumn).Width = 25; //Maturity Date
                    sheet.Column(++nColumn).Width = 25; //Realization Date
                    sheet.Column(++nColumn).Width = 25; //Encashment Date


                    #region Report Header
                    cell = sheet.Cells[nRowIndex, nStartCol + 3, nRowIndex, nEndCol - 6]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, nStartCol + 12, nRowIndex, nEndCol - 1]; cell.Merge = true;
                    cell.Value = "Invoice Register"; cell.Style.Font.Bold = true;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Address & Date
                    cell = sheet.Cells[nRowIndex, nStartCol + 4, nRowIndex, nEndCol - 6]; cell.Merge = true;
                    cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, nStartCol + 12, nRowIndex, nEndCol - 1]; cell.Merge = true;
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

                    #region Header
                    nColumn = 1;
                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "L/C Or TT Number"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Buyer"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Unit"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Style Number"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "PO Number"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Invoice No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Ship Mode"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Value"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Pcs"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "CTN"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Month"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Sending Date"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Accept Date"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Maturity Date"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Realization Date"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Encashment Date"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nRowIndex++;
                    #endregion

                    #region Data
                    double nGrandTotalNetAmount = 0;
                                      

                        foreach (CommercialInvoiceRegister oItem in _oCommercialInvoiceRegisters)
                        {
                          
                                nCount++;
                                nColumn = 1;

                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = nCount; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.MasterLCNo; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.BuyerName; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.BUName; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.StyleNo; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.OrderRecapNo; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.InvoiceNo; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.ShipmentModeInString; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.Amount; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                cell.Style.Numberformat.Format = "#,##0.00";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.Qty; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.CartonQty; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.SendingMonth; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = ""; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = ""; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = ""; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = ""; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value =""; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                nGrandTotalNetAmount += oItem.Amount;
                                nRowIndex += 1;
                           
                        }
                    #endregion

                    #region Grand Total
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 9]; cell.Merge = true;
                    cell.Value = "Grand Total:"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 10];
                    cell.Value = nGrandTotalNetAmount; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 11, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nRowIndex++;
                    #endregion

                    #endregion

                    cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=CIRegister.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion

            }
        }
        #endregion

        #region Excel
        public void ExportToExcel(double ts)
        {
            Company oCompany = new Company();
            CommercialInvoiceRegister oCommercialInvoiceRegister = new CommercialInvoiceRegister();
            try
            {
                _sErrorMesage = "";

                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oCommercialInvoiceRegisters = new List<CommercialInvoiceRegister>();
                oCommercialInvoiceRegister = (CommercialInvoiceRegister)Session[SessionInfo.ParamObj];

                string sSQL = this.GetSQL(oCommercialInvoiceRegister);
                _oCommercialInvoiceRegisters = CommercialInvoiceRegister.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

                if (_oCommercialInvoiceRegisters.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oCommercialInvoiceRegisters = new List<CommercialInvoiceRegister>();
                _sErrorMesage = ex.Message;
            }
            if (_sErrorMesage == "")
            {
                if (oCommercialInvoiceRegister.ReportLayout == EnumReportLayout.PartyWiseDetails)
                {
                    ExportToExcelBuyerWise(_oCommercialInvoiceRegisters, oCompany, _sDateRange);
                }
                else if (oCommercialInvoiceRegister.ReportLayout == EnumReportLayout.DateWiseDetails)
                {
                    ExportToExcelDateWise(_oCommercialInvoiceRegisters, oCompany, _sDateRange);
                }
                else
                {
                    ExportToExcelInvoiceWise(_oCommercialInvoiceRegisters, oCompany, _sDateRange);
                }
            }
        }

        public void ExportToExcelInvoiceWise(List<CommercialInvoiceRegister>_oCommercialInvoiceRegisters, Company oCompany, string _sDateRange)
        {
            #region Print XL

            int nRowIndex = 2, nStartCol = 2, nEndCol = 21, nColumn = 1, nCount = 0;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("ExportCIRegister");
                sheet.Name = "Export CI Register";
                sheet.Column(++nColumn).Width = 10; //SL No
                sheet.Column(++nColumn).Width = 15; //MasterLC No
                sheet.Column(++nColumn).Width = 15; //Invoice No
                sheet.Column(++nColumn).Width = 30; //Buyer Name
                sheet.Column(++nColumn).Width = 20; //Invoice Date
                sheet.Column(++nColumn).Width = 20; //Shipment Date
                sheet.Column(++nColumn).Width = 20; //Maturity Date
                sheet.Column(++nColumn).Width = 20; //Relization Date
                sheet.Column(++nColumn).Width = 20; //Encashment Date
                sheet.Column(++nColumn).Width = 15; //Invoice Status
                sheet.Column(++nColumn).Width = 15; //Style No
                sheet.Column(++nColumn).Width = 15; //Order Recap No
                sheet.Column(++nColumn).Width = 15; //M.Unit
                sheet.Column(++nColumn).Width = 15; //Qty
                sheet.Column(++nColumn).Width = 15; //Unit Price
                sheet.Column(++nColumn).Width = 25; //Amount
                sheet.Column(++nColumn).Width = 25; //Discount Amount
                sheet.Column(++nColumn).Width = 25; //Addition Amount
                sheet.Column(++nColumn).Width = 25; //Descripancy Charge
                sheet.Column(++nColumn).Width = 25; //Net Invoice Amount


                #region Report Header
                cell = sheet.Cells[nRowIndex, nStartCol + 4, nRowIndex, nEndCol - 6]; cell.Merge = true;
                cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                cell = sheet.Cells[nRowIndex, nStartCol + 14, nRowIndex, nEndCol - 1]; cell.Merge = true;
                cell.Value = "Export CI Register(Invoice wise)"; cell.Style.Font.Bold = true;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Address & Date
                cell = sheet.Cells[nRowIndex, nStartCol + 4, nRowIndex, nEndCol - 6]; cell.Merge = true;
                cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                cell = sheet.Cells[nRowIndex, nStartCol + 14, nRowIndex, nEndCol - 1]; cell.Merge = true;
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

                #region Header
                nColumn = 1;
                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "LC No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Invoice No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Buyer Name"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Invoice Date"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Shipment Date"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Maturity Date"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Relization Date"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Encashment Date"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Status"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Style No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Recap No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "M.Unit"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Qty"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "UnitPrice"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Amount"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Discount"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Addition"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Des.Charge"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Net Amount"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nRowIndex++;
                #endregion

                #region Data
                double nTotalNetAmount = 0; int nCommercialInvoiceID = 0; int nRowSpan = 0;
                foreach (CommercialInvoiceRegister oItem in _oCommercialInvoiceRegisters)
                {
                    if (oItem.CommercialInvoiceID != nCommercialInvoiceID)
                    {
                        nCount++;
                        nColumn = 1;
                        nRowSpan = _oCommercialInvoiceRegisters.Where(CIR => CIR.CommercialInvoiceID == oItem.CommercialInvoiceID).ToList().Count - 1;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex + nRowSpan, nColumn]; cell.Merge = true;
                        cell.Value = nCount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex + nRowSpan, nColumn]; cell.Merge = true;
                        cell.Value = oItem.MasterLCNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex + nRowSpan, nColumn]; cell.Merge = true; 
                        cell.Value = oItem.InvoiceNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex + nRowSpan, nColumn]; cell.Merge = true; 
                        cell.Value = oItem.BuyerName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex + nRowSpan, nColumn]; cell.Merge = true; 
                        cell.Value = oItem.InvoiceDateInString; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex + nRowSpan, nColumn]; cell.Merge = true;
                        cell.Value = oItem.ShipmentDateInString; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex + nRowSpan, nColumn]; cell.Merge = true;
                        cell.Value = ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex + nRowSpan, nColumn]; cell.Merge = true;
                        cell.Value = ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex + nRowSpan, nColumn]; cell.Merge = true;
                        cell.Value =""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex + nRowSpan, nColumn]; cell.Merge = true;
                        cell.Value = oItem.InvoiceStatusInString; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }

                    nColumn = 11;
                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.StyleNo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.OrderRecapNo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.MUnit; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.InvoiceQty; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.UnitPrice; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.Amount; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    if (oItem.CommercialInvoiceID != nCommercialInvoiceID)
                    {
                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex + nRowSpan, nColumn]; cell.Merge = true;
                        cell.Value = oItem.DiscountAmount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex + nRowSpan, nColumn]; cell.Merge = true;
                        cell.Value = oItem.AdditionAmount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex + nRowSpan, nColumn]; cell.Merge = true; 
                        cell.Value = oItem.DiscrepancyCharge; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex + nRowSpan, nColumn]; cell.Merge = true;
                        cell.Value = oItem.NetInvoiceAmount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }

                    nCommercialInvoiceID = oItem.CommercialInvoiceID;
                    nTotalNetAmount = nTotalNetAmount + oItem.NetInvoiceAmount;

                    nRowIndex += 1;
                }
                #endregion

                #region Grand Total
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 1]; cell.Merge = true; 
                cell.Value = "Grand Total:"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, nEndCol];
                cell.Value = nTotalNetAmount; cell.Style.Font.Bold = false;
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
                Response.AddHeader("content-disposition", "attachment; filename=ExportCIRegister.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }

        public void ExportToExcelBuyerWise(List<CommercialInvoiceRegister> _oCommercialInvoiceRegisters, Company oCompany, string _sDateRange)
        {
            #region Print XL

            int nRowIndex = 2, nStartCol = 2, nEndCol = 17, nColumn = 1, nCount = 0;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("ExportCIRegister");
                sheet.Name = "Export CI Register";
                sheet.Column(++nColumn).Width = 10; //SL No
                sheet.Column(++nColumn).Width = 15; //MasterLC No
                sheet.Column(++nColumn).Width = 15; //Invoice No
                //sheet.Column(++nColumn).Width = 30; //Buyer Name
                sheet.Column(++nColumn).Width = 20; //Invoice Date
                sheet.Column(++nColumn).Width = 20; //Shipment Date
                sheet.Column(++nColumn).Width = 20; //Maturity Date
                sheet.Column(++nColumn).Width = 20; //Relization Date
                sheet.Column(++nColumn).Width = 20; //Encashment Date
                sheet.Column(++nColumn).Width = 15; //Invoice Status
                sheet.Column(++nColumn).Width = 15; //M.Unit
                sheet.Column(++nColumn).Width = 15; //Qty
                sheet.Column(++nColumn).Width = 25; //Amount
                sheet.Column(++nColumn).Width = 25; //Discount Amount
                sheet.Column(++nColumn).Width = 25; //Addition Amount
                sheet.Column(++nColumn).Width = 25; //Descripancy Charge
                sheet.Column(++nColumn).Width = 25; //Net Invoice Amount


                #region Report Header
                cell = sheet.Cells[nRowIndex, nStartCol + 3, nRowIndex, nEndCol - 6]; cell.Merge = true;
                cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                cell = sheet.Cells[nRowIndex, nStartCol + 12, nRowIndex, nEndCol - 1]; cell.Merge = true;
                cell.Value = "Export CI Register(Buyer wise)"; cell.Style.Font.Bold = true;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Address & Date
                cell = sheet.Cells[nRowIndex, nStartCol + 4, nRowIndex, nEndCol - 6]; cell.Merge = true;
                cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                cell = sheet.Cells[nRowIndex, nStartCol + 12, nRowIndex, nEndCol - 1]; cell.Merge = true;
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

                #region Header
                nColumn = 1;
                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "LC No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Invoice No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Invoice Date"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Shipment Date"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Maturity Date"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Relization Date"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Encashment Date"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Status"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "M.Unit"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Qty"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Amount"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Discount"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Addition"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Des.Charge"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Net Amount"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nRowIndex++;
                #endregion

                #region Data
                double nGrandTotalNetAmount = 0; double nSubTotalNetAmount = 0; int nCommercialInvoiceID = 0; bool IsBuyerNameAssigned = false;
                var GroupResults = from CIR in _oCommercialInvoiceRegisters group CIR by CIR.BuyerID;
                foreach (var oItems in GroupResults)
                {
                    nCount = 0;
                    nSubTotalNetAmount = 0;
                    nCommercialInvoiceID = 0;
                    IsBuyerNameAssigned = false;

                    foreach (var oItem in oItems)
                    {
                        if (oItem.CommercialInvoiceID != nCommercialInvoiceID)
                        {
                            if (!IsBuyerNameAssigned)
                            {
                                IsBuyerNameAssigned = true;
                                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                                cell.Value = "Buyer Name : " + oItem.BuyerName; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                nRowIndex++;
                            }

                            nCommercialInvoiceID = oItem.CommercialInvoiceID;

                            nCount++;
                            nColumn = 1;

                            cell = sheet.Cells[nRowIndex, ++nColumn];cell.Value = nCount; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.MasterLCNo; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.InvoiceNo; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.InvoiceDateInString; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.ShipmentDateInString; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = ""; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = ""; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value =""; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.InvoiceStatusInString; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.MUnit; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.Qty; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.Amount; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "#,##0.00";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.DiscountAmount; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "#,##0.00";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.AdditionAmount; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "#,##0.00";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.DiscrepancyCharge; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "#,##0.00";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.NetInvoiceAmount; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "#,##0.00";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            nSubTotalNetAmount = nSubTotalNetAmount + oItem.NetInvoiceAmount;

                            nRowIndex += 1;
                        }
                    }

                    nGrandTotalNetAmount = nGrandTotalNetAmount + nSubTotalNetAmount;

                    #region Sub Total
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 1]; cell.Merge = true;
                    cell.Value = "Sub Total:"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nEndCol];
                    cell.Value = nSubTotalNetAmount; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nRowIndex++;
                    #endregion
                }
                #endregion

                #region Grand Total
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 1]; cell.Merge = true;
                cell.Value = "Grand Total:"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, nEndCol];
                cell.Value = nGrandTotalNetAmount; cell.Style.Font.Bold = false;
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
                Response.AddHeader("content-disposition", "attachment; filename=ExportCIRegister.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }

        public void ExportToExcelDateWise(List<CommercialInvoiceRegister> _oCommercialInvoiceRegisters, Company oCompany, string _sDateRange)
        {
            #region Print XL

            int nRowIndex = 2, nStartCol = 2, nEndCol = 17, nColumn = 1, nCount = 0;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("ExportCIRegister");
                sheet.Name = "Export CI Register";
                sheet.Column(++nColumn).Width = 10; //SL No
                sheet.Column(++nColumn).Width = 15; //MasterLC No
                sheet.Column(++nColumn).Width = 15; //Invoice No
                sheet.Column(++nColumn).Width = 30; //Buyer Name
                //sheet.Column(++nColumn).Width = 20; //Invoice Date
                sheet.Column(++nColumn).Width = 20; //Shipment Date
                sheet.Column(++nColumn).Width = 20; //Maturity Date
                sheet.Column(++nColumn).Width = 20; //Relization Date
                sheet.Column(++nColumn).Width = 20; //Encashment Date
                sheet.Column(++nColumn).Width = 15; //Invoice Status
                sheet.Column(++nColumn).Width = 15; //M.Unit
                sheet.Column(++nColumn).Width = 15; //Qty
                sheet.Column(++nColumn).Width = 25; //Amount
                sheet.Column(++nColumn).Width = 25; //Discount Amount
                sheet.Column(++nColumn).Width = 25; //Addition Amount
                sheet.Column(++nColumn).Width = 25; //Descripancy Charge
                sheet.Column(++nColumn).Width = 25; //Net Invoice Amount


                #region Report Header
                cell = sheet.Cells[nRowIndex, nStartCol + 3, nRowIndex, nEndCol - 6]; cell.Merge = true;
                cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                cell = sheet.Cells[nRowIndex, nStartCol + 12, nRowIndex, nEndCol - 1]; cell.Merge = true;
                cell.Value = "Export CI Register(Date wise)"; cell.Style.Font.Bold = true;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Address & Date
                cell = sheet.Cells[nRowIndex, nStartCol + 4, nRowIndex, nEndCol - 6]; cell.Merge = true;
                cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                cell = sheet.Cells[nRowIndex, nStartCol + 12, nRowIndex, nEndCol - 1]; cell.Merge = true;
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

                #region Header
                nColumn = 1;
                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "LC No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Invoice No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Buyer Name"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Shipment Date"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Maturity Date"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Relization Date"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Encashment Date"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Status"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "M.Unit"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Qty"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Amount"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Discount"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Addition"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Des.Charge"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Net Amount"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nRowIndex++;
                #endregion

                #region Data
                double nGrandTotalNetAmount = 0; double nSubTotalNetAmount = 0; int nCommercialInvoiceID = 0; bool IsInvoiceDateAssigned = false;
                var GroupResults = from CIR in _oCommercialInvoiceRegisters group CIR by CIR.InvoiceDateInString;
                foreach (var oItems in GroupResults)
                {
                    nCount = 0;
                    nSubTotalNetAmount = 0;
                    nCommercialInvoiceID = 0;
                    IsInvoiceDateAssigned = false;

                    foreach (var oItem in oItems)
                    {
                        if (oItem.CommercialInvoiceID != nCommercialInvoiceID)
                        {
                            if (!IsInvoiceDateAssigned)
                            {
                                IsInvoiceDateAssigned = true;
                                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                                cell.Value = "Invoice Date : " + oItem.InvoiceDateInString; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                nRowIndex++;
                            }

                            nCommercialInvoiceID = oItem.CommercialInvoiceID;

                            nCount++;
                            nColumn = 1;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = nCount; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.MasterLCNo; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.InvoiceNo; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.BuyerName; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.ShipmentDateInString; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = ""; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = ""; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value =""; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.InvoiceStatusInString; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.MUnit; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.Qty; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.Amount; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "#,##0.00";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.DiscountAmount; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "#,##0.00";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.AdditionAmount; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "#,##0.00";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.DiscrepancyCharge; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "#,##0.00";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.NetInvoiceAmount; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "#,##0.00";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            nSubTotalNetAmount = nSubTotalNetAmount + oItem.NetInvoiceAmount;

                            nRowIndex += 1;
                        }
                    }

                    nGrandTotalNetAmount = nGrandTotalNetAmount + nSubTotalNetAmount;

                    #region Sub Total
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 1]; cell.Merge = true;
                    cell.Value = "Sub Total:"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nEndCol];
                    cell.Value = nSubTotalNetAmount; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nRowIndex++;
                    #endregion
                }
                #endregion

                #region Grand Total
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 1]; cell.Merge = true;
                cell.Value = "Grand Total:"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, nEndCol];
                cell.Value = nGrandTotalNetAmount; cell.Style.Font.Bold = false;
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
                Response.AddHeader("content-disposition", "attachment; filename=ExportCIRegister.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }

        #endregion

        #region Support Functions
        private string GetSQL(CommercialInvoiceRegister oCommercialInvoiceRegister)
        {
            _sDateRange = "";
            string sSearchingData = oCommercialInvoiceRegister.SearchingData;
            EnumCompareOperator eInvoiceOperationTime = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[0]);
            EnumCompareOperator eShipmentOperationTime = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[3]);
            //EnumCompareOperator eReceiveOperationTime = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[6]);
            //EnumCompareOperator eExpireOperationTime = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[9]);

            string sSQLQuery = "", sWhereCluse = "", sGroupBy = "", sOrderBy = "";

            #region BusinessUnit
            if (oCommercialInvoiceRegister.BUID > 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " BUID =" + oCommercialInvoiceRegister.BUID.ToString();
            }
            #endregion

            #region InvoiceNo
            if (oCommercialInvoiceRegister.InvoiceNo != null && oCommercialInvoiceRegister.InvoiceNo != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " InvoiceNo LIKE'%" + oCommercialInvoiceRegister.InvoiceNo + "%'";
            }
            #endregion

            #region OrderRecapNo
            if (oCommercialInvoiceRegister.OrderRecapNo != null && oCommercialInvoiceRegister.OrderRecapNo != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " InvoiceNo LIKE'%" + oCommercialInvoiceRegister.OrderRecapNo + "%'";
            }
            #endregion

            #region StyleNo
            if (oCommercialInvoiceRegister.StyleNo != null && oCommercialInvoiceRegister.StyleNo != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " InvoiceNo LIKE'%" + oCommercialInvoiceRegister.StyleNo + "%'";
            }
            #endregion

            #region BuyerID
            if (oCommercialInvoiceRegister.BuyerID != 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " BuyerID IN (" + oCommercialInvoiceRegister.BuyerID + ")";
            }
            #endregion

            #region InvoiceDate
            if ((int)eInvoiceOperationTime > 0)
            {
                if ((int)eInvoiceOperationTime == 5 || (int)eInvoiceOperationTime == 6)
                {
                    DateTime dInvoiceStartDate = Convert.ToDateTime(sSearchingData.Split('~')[1]);
                    DateTime dInoviceEndDate = Convert.ToDateTime(sSearchingData.Split('~')[2]);
                    DateObject.CompareDateQuery(ref sWhereCluse, " InvoiceDate ", (int)eInvoiceOperationTime, dInvoiceStartDate, dInoviceEndDate);
                }
                else
                {
                    DateTime dInvoiceStartDate = Convert.ToDateTime(sSearchingData.Split('~')[1]);
                    DateTime dInoviceEndDate = DateTime.Now;
                    DateObject.CompareDateQuery(ref sWhereCluse, " InvoiceDate ", (int)eInvoiceOperationTime, dInvoiceStartDate, dInoviceEndDate);
                }
            }
            #endregion

            #region ShipmentDate
            if ((int)eShipmentOperationTime > 0)
            {
                if ((int)eShipmentOperationTime == 5 || (int)eShipmentOperationTime == 6)
                {
                    DateTime dShipmentStartDate = Convert.ToDateTime(sSearchingData.Split('~')[4]);
                    DateTime dShipmentEndDate = Convert.ToDateTime(sSearchingData.Split('~')[5]);
                    DateObject.CompareDateQuery(ref sWhereCluse, " ShipmentDate ", (int)eShipmentOperationTime, dShipmentStartDate, dShipmentEndDate);
                }
                else
                {
                    DateTime dShipmentStartDate = Convert.ToDateTime(sSearchingData.Split('~')[4]);
                    DateTime dShipmentEndDate = DateTime.Now;
                    DateObject.CompareDateQuery(ref sWhereCluse, " ShipmentDate ", (int)eShipmentOperationTime, dShipmentStartDate, dShipmentEndDate);
                }
            }
            #endregion

          
            #region Report Layout
            if (oCommercialInvoiceRegister.ReportLayout == EnumReportLayout.DateWiseDetails)
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT * FROM View_CommercialInvoiceRegister ";
                sOrderBy = " ORDER BY CommercialInvoiceID ASC";
            }
            else if (oCommercialInvoiceRegister.ReportLayout == EnumReportLayout.PartyWiseDetails)
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT * FROM View_CommercialInvoiceRegister ";
                sOrderBy = " ORDER BY  BuyerName, BuyerID, CommercialInvoiceID ASC";
            }
            else
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT * FROM View_CommercialInvoiceRegister ";
                sOrderBy = " ORDER BY CommercialInvoiceDetailID ASC";
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
        
        #endregion
    }
}