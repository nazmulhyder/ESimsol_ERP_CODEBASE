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
    public class AdjustmentRequisitionSlipRegisterController : Controller
    {   
        string _sDateRange = "";
        string _sErrorMesage = "";
        List<AdjustmentRequisitionSlipRegister> _oAdjustmentRequisitionSlipRegisters = new List<AdjustmentRequisitionSlipRegister>();        
        #region Actions
        public ActionResult ViewAdjustmentRequisitionSlipRegisters(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            AdjustmentRequisitionSlipRegister oAdjustmentRequisitionSlipRegister = new AdjustmentRequisitionSlipRegister();

            #region Request User
            string sSQL = "SELECT * FROM View_User AS HH WHERE HH.UserID IN (SELECT DISTINCT MM.RequestedByID FROM AdjustmentRequisitionSlip AS MM WHERE ISNULL(MM.RequestedByID,0)!=0) ORDER BY HH.UserName";
            List<User> oRequestUsers = new List<ESimSol.BusinessObjects.User>();
            ESimSol.BusinessObjects.User oApprovalUser = new ESimSol.BusinessObjects.User();
            oApprovalUser.UserID = 0; oApprovalUser.UserName = "--Select Request User--";
            oRequestUsers.Add(oApprovalUser);
            oRequestUsers.AddRange(ESimSol.BusinessObjects.User.GetsBySql(sSQL, (int)Session[SessionInfo.currentUserID]));
            #endregion

            #region Report Layout
            List<EnumObject> oReportLayouts = new List<EnumObject>();
            List<EnumObject> oTempReportLayouts = new List<EnumObject>();
            oTempReportLayouts = EnumObject.jGets(typeof(EnumReportLayout));
            foreach (EnumObject oItem in oTempReportLayouts)
            {
                if ((EnumReportLayout)oItem.id == EnumReportLayout.ProductWise )
                {
                    oReportLayouts.Add(oItem);
                }
            }
            #endregion

            ViewBag.BUID = buid;
            ViewBag.ApprovalUsers = oRequestUsers;
            ViewBag.ReportLayouts = oReportLayouts;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.Stores = WorkingUnit.GetsPermittedStore(buid, EnumModuleName.Adjustment, EnumStoreType.IssueStore, ((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(oAdjustmentRequisitionSlipRegister);
        }
        [HttpPost]
        public ActionResult SetSessionSearchCriteria(AdjustmentRequisitionSlipRegister oAdjustmentRequisitionSlipRegister)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oAdjustmentRequisitionSlipRegister);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PrintARSRegister(double ts)
        {
            AdjustmentRequisitionSlipRegister oAdjustmentRequisitionSlipRegister = new AdjustmentRequisitionSlipRegister();
            try
            {
                _sErrorMesage = "";
                _oAdjustmentRequisitionSlipRegisters = new List<AdjustmentRequisitionSlipRegister>();
                oAdjustmentRequisitionSlipRegister = (AdjustmentRequisitionSlipRegister)Session[SessionInfo.ParamObj];
                string sSQL = this.GetSQL(oAdjustmentRequisitionSlipRegister);
                _oAdjustmentRequisitionSlipRegisters = AdjustmentRequisitionSlipRegister.Gets(sSQL,(int)Session[SessionInfo.currentUserID]);
                if (_oAdjustmentRequisitionSlipRegisters.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oAdjustmentRequisitionSlipRegisters = new List<AdjustmentRequisitionSlipRegister>();
                _sErrorMesage = ex.Message;
            }

            if (_sErrorMesage == "")
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                BusinessUnit oBU = new BusinessUnit();
                oBU = oBU.Get(oAdjustmentRequisitionSlipRegister.BUID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBU);

                rptARSRegisters oReport = new rptARSRegisters();
                byte[] abytes = oReport.PrepareReport(_oAdjustmentRequisitionSlipRegisters, oCompany, oAdjustmentRequisitionSlipRegister.ReportLayout, _sDateRange);
                return File(abytes, "application/pdf");
            }
            else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport(_sErrorMesage);
                return File(abytes, "application/pdf");
            }
        }

        #region Print XlX
        public void ExportToExcelRSRegister(double ts)
        {
            AdjustmentRequisitionSlipRegister oAdjustmentRequisitionSlipRegister = new AdjustmentRequisitionSlipRegister();
            _oAdjustmentRequisitionSlipRegisters = new List<AdjustmentRequisitionSlipRegister>();
            oAdjustmentRequisitionSlipRegister = (AdjustmentRequisitionSlipRegister)Session[SessionInfo.ParamObj];
            string sSQL = this.GetSQL(oAdjustmentRequisitionSlipRegister);
            _oAdjustmentRequisitionSlipRegisters = AdjustmentRequisitionSlipRegister.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            if (_oAdjustmentRequisitionSlipRegisters.Count <= 0)
            {
                _sErrorMesage = "There is no data for print!";
            }
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            BusinessUnit oBU = new BusinessUnit();
            oBU = oBU.Get(oAdjustmentRequisitionSlipRegister.BUID, (int)Session[SessionInfo.currentUserID]);
            oCompany = GlobalObject.BUTOCompany(oCompany, oBU);

            int nSL = 0;
            #region Export Excel
            int nRowIndex = 2, nEndRow = 0, nStartCol = 2, nEndCol = 20, nTempCol = 2;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Adjustment Requisition Register");
                string sReportHeader = " ";
                #region Report Body & Header
                if (oAdjustmentRequisitionSlipRegister.ReportLayout == EnumReportLayout.ProductWise)
                {
                    sReportHeader = "Product Wise Adjustment  Register";
                }
                sheet.Column(nTempCol).Width = 15; nTempCol++;//ref no
                sheet.Column(nTempCol).Width = 35; nTempCol++;//Store Name
                sheet.Column(nTempCol).Width = 20; nTempCol++;//Lot NO
                sheet.Column(nTempCol).Width = 20; nTempCol++;//Approved By
                sheet.Column(nTempCol).Width = 20; nTempCol++;//Date             
                sheet.Column(nTempCol).Width = 15; nTempCol++;//m unit
                sheet.Column(nTempCol).Width = 15; nTempCol++;//In Qty
                sheet.Column(nTempCol).Width = 15;//Out qty   
                #endregion

                nEndCol = nTempCol + 1;

                #region Report Header
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex,nEndCol-4]; cell.Merge = true;
                cell.Value = oBU.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                cell = sheet.Cells[nRowIndex,nEndCol-3, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = sReportHeader; cell.Style.Font.Bold = true; cell.Style.Font.UnderLine = true;
                cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex,nEndCol - 4]; cell.Merge = true;
                cell.Value = oBU.PringReportHead; cell.Style.Font.Bold = false; 
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                cell = sheet.Cells[nRowIndex,nEndCol - 3, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = _sDateRange; cell.Style.Font.Bold = true; cell.Style.Font.UnderLine = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Blank
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = ""; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Report Data
                
              if (oAdjustmentRequisitionSlipRegister.ReportLayout == EnumReportLayout.ProductWise)
                {
                    #region Data (Product Wise)
                    double nInTotalQty = 0, nOutTotalQty = 0, nInGrandTotalQty = 0, nOutGrandTotalQty = 0; int nTableRow = 0;
                    string sProductName = "";
                    foreach (AdjustmentRequisitionSlipRegister oItem in _oAdjustmentRequisitionSlipRegisters)
                    {

                        if (sProductName != oItem.ProductName)
                        {
                            if (nTableRow > 0)
                            {
                                #region SubTotal
                                cell = sheet.Cells[nRowIndex, 1, nRowIndex, 7]; cell.Value = "Product Wise Sub Total"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 8]; cell.Value = Global.MillionFormatActualDigit(nInTotalQty); cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                                cell = sheet.Cells[nRowIndex, 9]; cell.Value = Global.MillionFormatActualDigit(nOutTotalQty); cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                #endregion

                                nInTotalQty = 0; nOutTotalQty = 0; nTableRow = 0;
                            }
                            #region Header

                            #region Blank Row
                            nRowIndex++;
                            #endregion


                            #region Date Heading
                            nRowIndex++;
                            cell = sheet.Cells[nRowIndex, 1, nRowIndex,9]; cell.Value = "Product Name : " + oItem.ProductName+"["+oItem.ProductCode+"]"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Merge = true; cell.Style.Font.UnderLine = true;
                            cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            #endregion

                            #region Header Row
                            nRowIndex++;

                            cell = sheet.Cells[nRowIndex, 1]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            cell = sheet.Cells[nRowIndex, 2]; cell.Value = "Req No"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Store Name"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Lot No"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Approved By"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Date"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex,7]; cell.Value = "MUnit"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            cell = sheet.Cells[nRowIndex, 8]; cell.Value = "In Qty"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 9]; cell.Value = "Out Qty"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            nRowIndex++;
                            #endregion

                            #endregion
                            nSL = 0;
                            
                        }
                        nTableRow++;
                         nSL++;
                        cell = sheet.Cells[nRowIndex, 1]; cell.Value = "" + nSL; cell.Style.Font.Bold = false; 
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 2]; cell.Value = "" + oItem.ARSlipNo; cell.Style.Font.Bold = false; 
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.WUName; cell.Style.Font.Bold = false; 
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.LotNo; cell.Style.Font.Bold = false; 
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.ApproveByName; cell.Style.Font.Bold = false; 
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.RequestedDateTimeInString; cell.Style.Font.Bold = false; 
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 7]; cell.Value = oItem.MUSymbol; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 8]; cell.Value = oItem.InOutType == EnumInOutType.Receive ? oItem.Qty:0; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 9]; cell.Value = oItem.InOutType == EnumInOutType.Disburse ? oItem.Qty:0; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        sProductName = oItem.ProductName;
                        if (oItem.InOutType == EnumInOutType.Receive)
                        {
                            nInTotalQty = nInTotalQty + oItem.Qty;
                            nInGrandTotalQty = nInGrandTotalQty + oItem.Qty;
                        }
                        else
                        {
                            nOutTotalQty = nOutTotalQty + oItem.Qty;
                            nOutGrandTotalQty = nOutGrandTotalQty + oItem.Qty;
                        }

                        nEndRow = nRowIndex;
                        nRowIndex++;

                    }

                    #region SubTotal
                    cell = sheet.Cells[nRowIndex, 1, nRowIndex, 7]; cell.Value = "Product Wise Sub Total"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = Global.MillionFormatActualDigit(nInTotalQty); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 9]; cell.Value = Global.MillionFormatActualDigit(nInTotalQty); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    #endregion
                    
                    #region Grand Total
                    nRowIndex++;
                    cell = sheet.Cells[nRowIndex, 1, nRowIndex,7]; cell.Value = "Grand Total"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Merge = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = Global.MillionFormatActualDigit(nInGrandTotalQty); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 9]; cell.Value = Global.MillionFormatActualDigit(nOutGrandTotalQty); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    #endregion
                    #endregion
                }
              
                #endregion

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=AdjustmentRequisitionRegister(Bulk).xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion


        }
        #endregion

        #endregion
        #region Support Functions        
        private string GetSQL(AdjustmentRequisitionSlipRegister oAdjustmentRequisitionSlipRegister)
        {
            _sDateRange = "";
            string sSearchingData = oAdjustmentRequisitionSlipRegister.SearchingData;
            EnumCompareOperator nDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[0]);
            DateTime dStartDate = Convert.ToDateTime(sSearchingData.Split('~')[1]);
            DateTime dEndDate = Convert.ToDateTime(sSearchingData.Split('~')[2]);
            //EnumCompareOperator ePIApprovedDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[3]);
            //DateTime dPIApprovedStartDate = Convert.ToDateTime(sSearchingData.Split('~')[4]);
            //DateTime dPIApprovedEndDate = Convert.ToDateTime(sSearchingData.Split('~')[5]);
            int nWorkingUnitID = Convert.ToInt32(sSearchingData.Split('~')[3]);
            string sLotNo = sSearchingData.Split('~')[4];
 
            string sSQLQuery = "", sWhereCluse = "", sGroupBy = "", sOrderBy = "";

            #region BusinessUnit
            if (oAdjustmentRequisitionSlipRegister.BUID > 0)
            {
                Global.TagSQL(ref sWhereCluse);  
                sWhereCluse = sWhereCluse + " BUID =" + oAdjustmentRequisitionSlipRegister.BUID.ToString();
               

            }
            #endregion

            #region RequestedByID
            if (oAdjustmentRequisitionSlipRegister.RequestedByID != 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " RequestedByID =" + oAdjustmentRequisitionSlipRegister.RequestedByID.ToString();
            }
            #endregion
            #region WorkingUnitID
            if ((oAdjustmentRequisitionSlipRegister.WorkingUnitID) > 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " WorkingUnitID =" + oAdjustmentRequisitionSlipRegister.WorkingUnitID;
            }
            #endregion
            #region Product
            if (oAdjustmentRequisitionSlipRegister.ProductName != null && oAdjustmentRequisitionSlipRegister.ProductName != "")
            {
                Global.TagSQL(ref sWhereCluse);  
                sWhereCluse = sWhereCluse + " ProductID IN(" + oAdjustmentRequisitionSlipRegister.ProductName + ")";

            }
            #endregion
            #region Date
            if (nDate != EnumCompareOperator.None)
            {
                if (nDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);  
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),RequestedDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dStartDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "Date @ " + dStartDate.ToString("dd MMM yyyy");
                }
                else if (nDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),RequestedDate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dStartDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "Date Not Equal @ " + dStartDate.ToString("dd MMM yyyy");
                }
                else if (nDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),RequestedDate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dStartDate.ToString("dd MMM yyyy") + "', 106))";
                   
                    _sDateRange = "Date Greater Then @ " + dStartDate.ToString("dd MMM yyyy");
                }
                else if (nDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),RequestedDate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dStartDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "Date Smaller Then @ " + dStartDate.ToString("dd MMM yyyy");
                }
                else if (nDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),RequestedDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dEndDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "Date Between " + dStartDate.ToString("dd MMM yyyy") + " To " + dEndDate.ToString("dd MMM yyyy");
                }
                else if (nDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),RequestedDate,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dEndDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "Date NOT Between " + dStartDate.ToString("dd MMM yyyy") + " To " + dEndDate.ToString("dd MMM yyyy");
                }
            }
            #endregion
            #region sLotNo
            if (sLotNo != null && sLotNo != "")
            {
                Global.TagSQL(ref sWhereCluse);  
                sWhereCluse = sWhereCluse + " LotNo LIKE'%" + sLotNo+ "%'"; ;
            }
            #endregion
           
            #region Report Layout
             if (oAdjustmentRequisitionSlipRegister.ReportLayout == EnumReportLayout.ProductWise)
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT * FROM View_AdjustmentRequisitionSlipRegister ";
                sOrderBy = " ORDER BY  ProductName, RequestedDate ASC";
            }
            else
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT * FROM View_AdjustmentRequisitionSlipRegister ";
                sOrderBy = " ORDER BY Date, AdjustmentRequisitionSlipID, AdjustmentRequisitionSlipDetailID ASC";
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

