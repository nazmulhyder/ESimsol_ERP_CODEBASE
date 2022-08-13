using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ESimSol.BusinessObjects.ReportingObject;

using System.Web.Script.Serialization;
using ReportManagement;
using ICS.Core.Utility;
using iTextSharp.text;
using ESimSol.Reports;
using System.Data;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using System.Xml.Serialization;

using OfficeOpenXml;
using OfficeOpenXml.Style;
using iTextSharp.text.pdf;
using System.Net.Mail;
using System.Net.Mime;
using System.Dynamic;

namespace ESimSolFinancial.Controllers
{
    public class ExportFundAllocationController : Controller
    {
        public ActionResult ViewExportFundAllocationHeads(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.ExportFundAllocationHead).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            List<ExportFundAllocationHead> _oExportFundAllocationHeads = new List<ExportFundAllocationHead>();
            string sSQL = "SELECT * FROM View_ExportFundAllocationHead ORDER BY Sequence ASC";
            _oExportFundAllocationHeads = ExportFundAllocationHead.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            return View(_oExportFundAllocationHeads);
        }
        public ActionResult ViewExportFundAllocationHead(int id)
        {
            ExportFundAllocationHead _oExportFundAllocationHead = new ExportFundAllocationHead();
            if (id > 0)
            {
                _oExportFundAllocationHead = _oExportFundAllocationHead.Get(id, (int)Session[SessionInfo.currentUserID]);
            }
            return View(_oExportFundAllocationHead);
        }
        public ActionResult ViewExportFundAllocation(int menuid, int nid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<ExportFundAllocation> oExportFundAllocations = new List<ExportFundAllocation>();
            string sSQL = "SELECT * FROM View_ExportFundAllocation WHERE ExportLCID=" + nid + "";
            oExportFundAllocations = ExportFundAllocation.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            ExportLC oExportLC = new ExportLC();
            oExportLC = oExportLC.Get(nid, (int)Session[SessionInfo.currentUserID]);
            List<ExportFundAllocationHead> _oExportFundAllocationHeads = new List<ExportFundAllocationHead>();
            sSQL = "SELECT * FROM View_ExportFundAllocationHead ORDER BY Sequence ASC";
            _oExportFundAllocationHeads = ExportFundAllocationHead.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            ViewBag.ExportLC = oExportLC;
            ViewBag.ExportFundAllocationHead = _oExportFundAllocationHeads;
            return View(oExportFundAllocations);

        }

        public ActionResult ViewExportFundAllocationReport(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            ExportFundAllocation oExportFundAllocation = new ExportFundAllocation();
            List<ExportFundAllocationHead> _oExportFundAllocationHeads = new List<ExportFundAllocationHead>();
            string sSQL = "SELECT * FROM View_ExportFundAllocationHead ORDER BY Sequence ASC";
            _oExportFundAllocationHeads = ExportFundAllocationHead.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            ViewBag.ExportFundAllocationHead = _oExportFundAllocationHeads;
            ViewBag.BUID = buid;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.BankBranchs = BankBranch.GetsByDeptAndBU(((int)EnumOperationalDept.Export_Own).ToString(), buid, "", ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.LCStatus = EnumObject.jGets(typeof(EnumExportLCStatus));
            return View(oExportFundAllocation);
        }
        public ActionResult SetSessionSearchCriteria(ExportFundAllocation oExportFundAllocation)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oExportFundAllocation);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public void AdvReportExportExcel(double ts)
        {
            Company oCompany = new Company();
            ExportFundAllocation oExportFundAllocation = new ExportFundAllocation();
            try
            {
                oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
                oExportFundAllocation = (ExportFundAllocation)Session[SessionInfo.ParamObj];
                if (oExportFundAllocation == null)
                {
                    throw new Exception("Invalid Searching Criteria!");
                }

                ExportFundAllocation oTempExportFundAllocation = new ExportFundAllocation();

                oTempExportFundAllocation.SQL = this.GetSQL(oExportFundAllocation.ErrorMessage);

                DataSet oDataSet = ExportFundAllocation.Gets(oTempExportFundAllocation, (int)Session[SessionInfo.currentUserID]);

                ExportFundAllocationXL(oDataSet, oCompany);
               
            }
            catch (Exception ex)
            {
                #region Errormessage
                ExcelRange cell;
                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Sales & Marketing");
                    sheet.Name = "Sales & Marketing";

                    cell = sheet.Cells[2, 2, 2, 5]; cell.Merge = true; cell.Value = ex.Message; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=Buyer-MonthWiseSalesReport.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
        }
        private void ExportFundAllocationXL(DataSet oDataSet, Company oCompany)
        {
            DataTable oEFAs = oDataSet.Tables[0];
            DataTable oFollowUpHeads = oDataSet.Tables[1];

            #region Report Body
            string sStartCell = "", sEndCell = "", sFormula = "";
            int nStartRow = 0, nEndRow = 0, nStartCol = 0, nEndCol = 0;
            int rowIndex = 2;
            int nMaxColumn;
            int colIndex = 2;
            ExcelRange cell;
            ExcelFill fill;
            float nFontSize = 10f;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Fund Allocation Report");
                sheet.Name = "Fund Allocation Report";
                sheet.View.FreezePanes(5, 7);

                #region Coloums
                sheet.Column(colIndex++).Width = 8;//SL
                sheet.Column(colIndex++).Width = 18;//File No
                sheet.Column(colIndex++).Width = 15;//LC Date
                sheet.Column(colIndex++).Width = 15;//LC value
                sheet.Column(colIndex++).Width = 15;//Payment Recived

                if (oFollowUpHeads != null && oFollowUpHeads.Rows.Count > 0)
                {
                    foreach (DataRow oItem in oFollowUpHeads.Rows)
                    {
                        sheet.Column(colIndex++).Width = 15;//Dynamic Followup Head
                        sheet.Column(colIndex++).Width = 15;//Dynamic Followup Head Percent
                    }
                }
                nMaxColumn = colIndex - 1;
                #endregion
                #region Report Header
                cell = sheet.Cells[rowIndex, 2, rowIndex, 6]; cell.Merge = true; cell.Value = "BB LC Fund Allocation Report"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15f; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 2;
                #endregion


                #region Column Header
                colIndex = 2;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "SL No."; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "File No."; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "LC date"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "LC value"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Payment Received"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                if (oFollowUpHeads != null && oFollowUpHeads.Rows.Count > 0)
                {
                    foreach (DataRow oItem in oFollowUpHeads.Rows)
                    {
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (oItem["Name"] == DBNull.Value) ? "" : Convert.ToString(oItem["Name"]); cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (oItem["Name"] == DBNull.Value) ? "" : Convert.ToString(oItem["Name"])+" (%)"; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                }
                rowIndex++;
                #endregion
                int nCount = 0;
                #region Report Body
                if (oEFAs.Rows.Count > 0)
                {

                    foreach (DataRow oItem in oEFAs.Rows)
                    {
                        colIndex = 2;
                        nCount++;
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nCount.ToString();
                        cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (oItem["FileNo"] == DBNull.Value) ? "" : Convert.ToString(oItem["FileNo"]); cell.Style.WrapText = true;
                        cell.Style.Font.Size = nFontSize; cell.Style.Font.Bold = false; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (oItem["OpeningDate"] == DBNull.Value) ? "" : Convert.ToDateTime(oItem["OpeningDate"]).ToString("dd MMM yyyy"); cell.Style.Font.Bold = false;
                        cell.Style.Font.Size = nFontSize; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        if (nCount == 1)
                        {
                            nStartCol = colIndex-1;
                            nStartRow = rowIndex;
                        }

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (oItem["LCValue"] == DBNull.Value) ? 0 : Convert.ToDouble(oItem["LCValue"]); cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Font.Size = nFontSize; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (oItem["PaymentReceived"] == DBNull.Value) ? 0 : Convert.ToDouble(oItem["PaymentReceived"]); cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Font.Size = nFontSize; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;



                        if (oFollowUpHeads != null && oFollowUpHeads.Rows.Count > 0)
                        {
                            foreach (DataRow oFollowUpItem in oFollowUpHeads.Rows)
                            {
                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (oItem["AllocationHead" + Convert.ToInt32(oFollowUpItem["ExportFundAllocationHeadID"]).ToString()] == DBNull.Value) ? 0 : Convert.ToDouble(oItem["AllocationHead" + Convert.ToInt32(oFollowUpItem["ExportFundAllocationHeadID"]).ToString()]);
                                cell.Style.Font.Size = nFontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.WrapText = true;
                                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; 

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (oItem["AllocationHeadPercent" + Convert.ToInt32(oFollowUpItem["ExportFundAllocationHeadID"]).ToString()] == DBNull.Value) ? 0 : Convert.ToDouble(oItem["AllocationHeadPercent" + Convert.ToInt32(oFollowUpItem["ExportFundAllocationHeadID"]).ToString()]);
                                cell.Style.Font.Size = nFontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.WrapText = true;
                                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; 
                            }
                        }
                        rowIndex++;
                    }
                    nEndCol = colIndex - 1;
                    nEndRow = rowIndex - 1;
                    colIndex = 5;
                    #region GrandTotal
                    sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                    sEndCell = Global.GetExcelCellName(nEndRow, nStartCol++);

                    cell = sheet.Cells[rowIndex, 2, rowIndex, 4]; cell.Merge = true; cell.Value = "Grand Total:"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                    sEndCell = Global.GetExcelCellName(nEndRow, nStartCol++);
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Style.Font.Bold = false; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                    sEndCell = Global.GetExcelCellName(nEndRow, nStartCol++);
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Style.Font.Bold = false; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    if (oFollowUpHeads != null && oFollowUpHeads.Rows.Count > 0)
                    {
                        foreach (DataRow oFollowUpItem in oFollowUpHeads.Rows)
                        {
                            sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                            sEndCell = Global.GetExcelCellName(nEndRow, nStartCol++);
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Style.Font.Bold = false; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                            sEndCell = Global.GetExcelCellName(nEndRow, nStartCol++);
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                    }

                    #endregion
                }
                #endregion


                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=Fund_Allocation_Report.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }
        private string GetSQL(string sSearchingData)
        {
            string sSQL = "";
            int Count = 0;
            string sPINo = Convert.ToString(sSearchingData.Split('~')[Count++]);
            string sFileNo = Convert.ToString(sSearchingData.Split('~')[Count++]);
            EnumCompareOperator ecboShipmentDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[Count++]);
            DateTime dromShipmentDate = Convert.ToDateTime(sSearchingData.Split('~')[Count++]);
            DateTime dToShipmentDate = Convert.ToDateTime(sSearchingData.Split('~')[Count++]);
            EnumCompareOperator ecboLCRecivedDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[Count++]);
            DateTime dFromLCRecivedDate = Convert.ToDateTime(sSearchingData.Split('~')[Count++]);
            DateTime dToLCRecivedDate = Convert.ToDateTime(sSearchingData.Split('~')[Count++]);
            EnumCompareOperator ecboDateOpening = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[Count++]);
            DateTime dFromOpeningDate = Convert.ToDateTime(sSearchingData.Split('~')[Count++]);
            DateTime dToOpeningDate = Convert.ToDateTime(sSearchingData.Split('~')[Count++]);
            EnumCompareOperator ecboAmendmentDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[Count++]);
            DateTime dFromAmendmentDate = Convert.ToDateTime(sSearchingData.Split('~')[Count++]);
            DateTime dToAmendmentDate = Convert.ToDateTime(sSearchingData.Split('~')[Count++]);
            EnumCompareOperator ecboExpireDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[Count++]);
            DateTime dFromExpireDate = Convert.ToDateTime(sSearchingData.Split('~')[Count++]);
            DateTime dToExpireDate = Convert.ToDateTime(sSearchingData.Split('~')[Count++]);
            string sIssueBankIDs = Convert.ToString(sSearchingData.Split('~')[Count++]);
            string sBuyerIDs = Convert.ToString(sSearchingData.Split('~')[Count++]);
            int nAdviseBankType = Convert.ToInt32(sSearchingData.Split('~')[Count++]);
            int nNegoBankType = Convert.ToInt32(sSearchingData.Split('~')[Count++]);
            int nStatusType = Convert.ToInt32(sSearchingData.Split('~')[Count++]);
            int nHeadNameType = Convert.ToInt32(sSearchingData.Split('~')[Count++]);


            #region PINo
            if (sPINo!="")
            {
                Global.TagSQL(ref sSQL);
                sSQL = sSQL + " ELC.ExportLCID IN (SELECT EPILC.ExportLCID FROM ExportPILCMapping AS EPILC WHERE EPILC.Activity =1 AND  EPILC.ExportPIID IN  (SELECT ExPI.ExportPIID FROM ExportPI AS ExPI WHERE ExPI.PINo LIKE '%" + sPINo + "%'))";
            }
            #endregion
            #region FileNo
            if (sFileNo != "")
            {
                Global.TagSQL(ref sSQL);
                sSQL = sSQL + " ELC.FileNo LIKE '%" + sFileNo + "%'";
            }
            #endregion

            #region IssueBank
            if (sIssueBankIDs != "")
            {
                Global.TagSQL(ref sSQL);
                sSQL = sSQL + " ELC.BankBranchID_Issue IN (" + sIssueBankIDs + ")";
            }
            #endregion
            #region Buyer
            if (sBuyerIDs != "")
            {
                Global.TagSQL(ref sSQL);
                sSQL = sSQL + " ELC.ContactPersonalID IN (" + sBuyerIDs + ")";
            }
            #endregion
            #region nAdviseBankType
            if (nAdviseBankType != 0)
            {
                Global.TagSQL(ref sSQL);
                sSQL = sSQL + " ISNULL(ELC.BankBranchID_Advice,0) = " + nAdviseBankType.ToString();
            }
            #endregion
            #region nNegoBankType
            if (nNegoBankType != 0)
            {
                Global.TagSQL(ref sSQL);
                sSQL = sSQL + " ISNULL(ELC.BankBranchID_Negotiation,0) = " + nNegoBankType.ToString();
            }
            #endregion

            #region nStatusType
            if (nStatusType != 0)
            {
                Global.TagSQL(ref sSQL);
                sSQL = sSQL + " ISNULL(ELC.CurrentStatus,0) = " + nStatusType.ToString();
            }
            #endregion

            #region HeadName Type
            if (nHeadNameType != 0)
            {
                Global.TagSQL(ref sSQL);
                sSQL = sSQL + " ELC.ExportLCID IN (SELECT EFA.ExportLCID FROM ExportFundAllocation AS EFA WHERE EFA.ExportFundAllocationHeadID =" + nHeadNameType + " )";
            }
            #endregion
            #region ShipmentDate Date
            if (ecboShipmentDate != EnumCompareOperator.None)
            {
                if (ecboShipmentDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sSQL);
                    sSQL = sSQL + " CONVERT(DATE,CONVERT(VARCHAR(12),ELC.ShipmentDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dromShipmentDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (ecboShipmentDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sSQL);
                    sSQL = sSQL + " CONVERT(DATE,CONVERT(VARCHAR(12),ELC.ShipmentDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dromShipmentDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (ecboShipmentDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sSQL);
                    sSQL = sSQL + " CONVERT(DATE,CONVERT(VARCHAR(12),ELC.ShipmentDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dromShipmentDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (ecboShipmentDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sSQL);
                    sSQL = sSQL + " CONVERT(DATE,CONVERT(VARCHAR(12),ELC.ShipmentDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dromShipmentDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (ecboShipmentDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sSQL);
                    sSQL = sSQL + " CONVERT(DATE,CONVERT(VARCHAR(12),ELC.ShipmentDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dromShipmentDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToShipmentDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (ecboShipmentDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sSQL);
                    sSQL = sSQL + " CONVERT(DATE,CONVERT(VARCHAR(12),ELC.ShipmentDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dromShipmentDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToShipmentDate.ToString("dd MMM yyyy") + "',106))";
                }
            }
            #endregion

            #region LCRecived Date
            if (ecboLCRecivedDate != EnumCompareOperator.None)
            {
                if (ecboLCRecivedDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sSQL);
                    sSQL = sSQL + " CONVERT(DATE,CONVERT(VARCHAR(12),ELC.LCRecivedDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromLCRecivedDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (ecboLCRecivedDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sSQL);
                    sSQL = sSQL + " CONVERT(DATE,CONVERT(VARCHAR(12),ELC.LCRecivedDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromLCRecivedDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (ecboLCRecivedDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sSQL);
                    sSQL = sSQL + " CONVERT(DATE,CONVERT(VARCHAR(12),ELC.LCRecivedDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromLCRecivedDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (ecboLCRecivedDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sSQL);
                    sSQL = sSQL + " CONVERT(DATE,CONVERT(VARCHAR(12),ELC.LCRecivedDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromLCRecivedDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (ecboLCRecivedDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sSQL);
                    sSQL = sSQL + " CONVERT(DATE,CONVERT(VARCHAR(12),ELC.LCRecivedDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromLCRecivedDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToLCRecivedDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (ecboLCRecivedDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sSQL);
                    sSQL = sSQL + " CONVERT(DATE,CONVERT(VARCHAR(12),ELC.LCRecivedDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromLCRecivedDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToLCRecivedDate.ToString("dd MMM yyyy") + "',106))";
                }
            }
            #endregion

            #region LC Opening Date
            if (ecboDateOpening != EnumCompareOperator.None)
            {
                if (ecboDateOpening == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sSQL);
                    sSQL = sSQL + " CONVERT(DATE,CONVERT(VARCHAR(12),ELC.OpeningDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromOpeningDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (ecboDateOpening == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sSQL);
                    sSQL = sSQL + " CONVERT(DATE,CONVERT(VARCHAR(12),ELC.OpeningDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromOpeningDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (ecboDateOpening == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sSQL);
                    sSQL = sSQL + " CONVERT(DATE,CONVERT(VARCHAR(12),ELC.OpeningDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromOpeningDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (ecboDateOpening == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sSQL);
                    sSQL = sSQL + " CONVERT(DATE,CONVERT(VARCHAR(12),ELC.OpeningDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromOpeningDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (ecboDateOpening == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sSQL);
                    sSQL = sSQL + " CONVERT(DATE,CONVERT(VARCHAR(12),ELC.OpeningDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromOpeningDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToOpeningDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (ecboDateOpening == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sSQL);
                    sSQL = sSQL + " CONVERT(DATE,CONVERT(VARCHAR(12),ELC.OpeningDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromOpeningDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToOpeningDate.ToString("dd MMM yyyy") + "',106))";
                }
            }
            #endregion
            #region Amendment Date
            if (ecboAmendmentDate != EnumCompareOperator.None)
            {
                if (ecboAmendmentDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sSQL);
                    sSQL = sSQL + " CONVERT(DATE,CONVERT(VARCHAR(12),ELC.AmendmentDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromAmendmentDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (ecboAmendmentDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sSQL);
                    sSQL = sSQL + " CONVERT(DATE,CONVERT(VARCHAR(12),ELC.AmendmentDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromAmendmentDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (ecboAmendmentDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sSQL);
                    sSQL = sSQL + " CONVERT(DATE,CONVERT(VARCHAR(12),ELC.AmendmentDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromAmendmentDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (ecboAmendmentDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sSQL);
                    sSQL = sSQL + " CONVERT(DATE,CONVERT(VARCHAR(12),ELC.AmendmentDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromAmendmentDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (ecboAmendmentDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sSQL);
                    sSQL = sSQL + " CONVERT(DATE,CONVERT(VARCHAR(12),ELC.AmendmentDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromAmendmentDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToAmendmentDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (ecboAmendmentDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sSQL);
                    sSQL = sSQL + " CONVERT(DATE,CONVERT(VARCHAR(12),ELC.AmendmentDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromAmendmentDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToAmendmentDate.ToString("dd MMM yyyy") + "',106))";
                }
            }
            #endregion

            #region Expire Date
            if (ecboExpireDate != EnumCompareOperator.None)
            {
                if (ecboExpireDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sSQL);
                    sSQL = sSQL + " CONVERT(DATE,CONVERT(VARCHAR(12),ELC.ExpiryDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromExpireDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (ecboExpireDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sSQL);
                    sSQL = sSQL + " CONVERT(DATE,CONVERT(VARCHAR(12),ELC.ExpiryDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromExpireDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (ecboExpireDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sSQL);
                    sSQL = sSQL + " CONVERT(DATE,CONVERT(VARCHAR(12),ELC.ExpiryDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromExpireDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (ecboExpireDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sSQL);
                    sSQL = sSQL + " CONVERT(DATE,CONVERT(VARCHAR(12),ELC.ExpiryDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromExpireDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (ecboExpireDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sSQL);
                    sSQL = sSQL + " CONVERT(DATE,CONVERT(VARCHAR(12),ELC.ExpiryDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromExpireDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToExpireDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (ecboExpireDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sSQL);
                    sSQL = sSQL + " CONVERT(DATE,CONVERT(VARCHAR(12),ELC.ExpiryDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromExpireDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToExpireDate.ToString("dd MMM yyyy") + "',106))";
                }
            }
            #endregion
            return sSQL;
        }
        [HttpPost]
        public JsonResult Save(ExportFundAllocationHead oExportFundAllocationHead)
        {
            ExportFundAllocationHead _oExportFundAllocationHead = new ExportFundAllocationHead();
            try
            {

                _oExportFundAllocationHead = oExportFundAllocationHead.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oExportFundAllocationHead = new ExportFundAllocationHead();
                _oExportFundAllocationHead.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportFundAllocationHead);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveExportFundAllocation(ExportFundAllocation oExportFundAllocation)
        {
            ExportFundAllocation _oExportFundAllocation = new ExportFundAllocation();
            try
            {

                _oExportFundAllocation = oExportFundAllocation.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oExportFundAllocation = new ExportFundAllocation();
                _oExportFundAllocation.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportFundAllocation);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteExportFundAllocation(ExportFundAllocation oExportFundAllocation)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oExportFundAllocation.Delete(oExportFundAllocation.ExportFundAllocationID, (int)Session[SessionInfo.currentUserID]);
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
        public JsonResult Delete(ExportFundAllocationHead oExportFundAllocationHead)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oExportFundAllocationHead.Delete(oExportFundAllocationHead.ExportFundAllocationHeadID, (int)Session[SessionInfo.currentUserID]);
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
        public JsonResult ApprovedExportFundAllocation(List<ExportFundAllocation> oExportFundAllocations)
        {
            ExportFundAllocation oExportFundAllocation = new ExportFundAllocation();
            List<ExportFundAllocation> _oExportFundAllocations = new List<ExportFundAllocation>();
            try
            {
                _oExportFundAllocations = oExportFundAllocation.ApprovedExportFundAllocation(oExportFundAllocations, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oExportFundAllocations = new List<ExportFundAllocation>();
                oExportFundAllocation = new ExportFundAllocation();
                oExportFundAllocation.ErrorMessage = ex.Message;
                _oExportFundAllocations.Add(oExportFundAllocation);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportFundAllocations);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UndoApprovedExportFundAllocation(List<ExportFundAllocation> oExportFundAllocations)
        {
            ExportFundAllocation oExportFundAllocation = new ExportFundAllocation();
            List<ExportFundAllocation> _oExportFundAllocations = new List<ExportFundAllocation>();
            try
            {
                _oExportFundAllocations = oExportFundAllocation.UndoApprovedExportFundAllocation(oExportFundAllocations, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oExportFundAllocations = new List<ExportFundAllocation>();
                oExportFundAllocation = new ExportFundAllocation();
                oExportFundAllocation.ErrorMessage = ex.Message;
                _oExportFundAllocations.Add(oExportFundAllocation);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportFundAllocations);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult RefreshSequence(List<ExportFundAllocationHead> oExportFundAllocationHeads)
        {
            ExportFundAllocationHead oExportFundAllocationHead = new ExportFundAllocationHead();
            List<ExportFundAllocationHead> _ExportFundAllocationHeads = new List<ExportFundAllocationHead>();
            try
            {
                _ExportFundAllocationHeads = oExportFundAllocationHead.RefreshSequence(oExportFundAllocationHeads, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _ExportFundAllocationHeads = new List<ExportFundAllocationHead>();
                oExportFundAllocationHead = new ExportFundAllocationHead();
                oExportFundAllocationHead.ErrorMessage = ex.Message;
                _ExportFundAllocationHeads.Add(oExportFundAllocationHead);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_ExportFundAllocationHeads);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

    }
}