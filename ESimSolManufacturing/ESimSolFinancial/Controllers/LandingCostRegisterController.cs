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
using System.Collections;

namespace ESimSolFinancial.Controllers
{
    public class LandingCostRegisterController : Controller
    {   
        string _sDateRange = "";
        string _sErrorMesage = "";
        string _sFormatter = "";
        List<LandingCostRegister> _oLandingCostRegisters = new List<LandingCostRegister>();        

        #region Actions
        public ActionResult ViewLandingCostRegisters(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            LandingCostRegister oLandingCostRegister = new LandingCostRegister();

            #region Approval User
            string sSQL = "SELECT * FROM View_User AS HH WHERE HH.UserID IN (SELECT DISTINCT MM.ApprovedBy FROM PurchaseInvoice AS MM WHERE ISNULL(MM.ApprovedBy,0)!=0 AND MM.BUID IN(" + buid.ToString() + ") AND MM.RefType IN (3,5)) ORDER BY HH.UserName";
            List<User> oApprovalUsers = new List<ESimSol.BusinessObjects.User>();
            ESimSol.BusinessObjects.User oApprovalUser = new ESimSol.BusinessObjects.User();
            oApprovalUser.UserID = 0; oApprovalUser.UserName = "--Select Approval User--";
            oApprovalUsers.Add(oApprovalUser);
            oApprovalUsers.AddRange(ESimSol.BusinessObjects.User.GetsBySql(sSQL, (int)Session[SessionInfo.currentUserID]));
            #endregion

            List<ImportProduct> oImportProducts = new List<ImportProduct>();            
            oImportProducts = ImportProduct.GetByBU(buid, (int)Session[SessionInfo.currentUserID]);
            ViewBag.ImportProducts = oImportProducts;
            ViewBag.BUID = buid;
            ViewBag.ApprovalUsers = oApprovalUsers;            
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));                 
            return View(oLandingCostRegister);
        }

        [HttpPost]
        public ActionResult SetSessionSearchCriteria(LandingCostRegister oLandingCostRegister)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oLandingCostRegister);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public void ExportToExcelLandingCostRegister(double ts)
        {
            LandingCostRegister oLandingCostRegister = new LandingCostRegister();
            try
            {
                _sErrorMesage = "";
                _oLandingCostRegisters = new List<LandingCostRegister>();
                oLandingCostRegister = (LandingCostRegister)Session[SessionInfo.ParamObj];
                string sSQL = this.GetSQL(oLandingCostRegister);
                _oLandingCostRegisters = LandingCostRegister.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (_oLandingCostRegisters.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oLandingCostRegisters = new List<LandingCostRegister>();
                _sErrorMesage = ex.Message;
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = null;// GetCompanyLogo(oCompany);
            BusinessUnit oBU = new BusinessUnit();
            if (_oLandingCostRegisters.Count > 0)
            {
                oBU = oBU.Get(_oLandingCostRegisters[0].BUID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBU);
            }

            #region Excel Body
            double nGrandTotalAmount = 0;
            int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 0;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;
            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Export PI Report");
                sheet.Name = "Landing Cost Register"; 

                #region SHEET COLUMN WIDTH
                int nColumn = 1;
                sheet.Column(++nColumn).Width = 6;   // SL NO    
                sheet.Column(++nColumn).Width = 18;  // Import LCNo
                sheet.Column(++nColumn).Width = 18;  // Import LCDate             
                sheet.Column(++nColumn).Width = 15;  // Import LCAmount       
                sheet.Column(++nColumn).Width = 30;   // Import InvoiceNo
                sheet.Column(++nColumn).Width = 15;  // Import InvoiceDate
                sheet.Column(++nColumn).Width = 15;  // Import InvoiceAmount
                sheet.Column(++nColumn).Width = 15;  // Bill No
                sheet.Column(++nColumn).Width = 15;  // Bill Date
                sheet.Column(++nColumn).Width = 15;  // Payment Type(Method)
                sheet.Column(++nColumn).Width = 40;  // Supplier Name
                sheet.Column(++nColumn).Width = 50;  // Cost Head Name
                sheet.Column(++nColumn).Width = 15;  // Cost Amount                
                nEndCol = nColumn;

                #endregion

                #region Report Header

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oCompany.Address; cell.Style.Font.Bold = false;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oCompany.Phone + ";  " + oCompany.Email + ";  " + oCompany.WebAddress; cell.Style.Font.Bold = false;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 2;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "Landing Cost Register"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                nRowIndex = nRowIndex + 1;
                #endregion

                string sStartCell = "", sEndCell = "";
                int nLCID = 0; int nCount = 0; bool IsTotalPrint = false;
                foreach (LandingCostRegister oItem in _oLandingCostRegisters)
                {
                    nCount++;
                    if (nLCID != oItem.LCID )
                    {
                        if (IsTotalPrint == true)
                        {
                            #region Total
                            int nCellIndex = 13;
                            cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nCellIndex];

                            cell.Merge = true;
                            cell.Value = "Sub Total :"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            
                            //Amount
                            sStartCell = Global.GetExcelCellName(nStartRow + 1, ++nCellIndex);
                            sEndCell = Global.GetExcelCellName(nEndRow, nCellIndex);
                            cell = sheet.Cells[nRowIndex, nCellIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            nRowIndex++;
                            #endregion
                        }
                        nCount = 1;

                        #region Column Header
                        //nRowIndex = nRowIndex + 1;
                        nStartRow = nRowIndex;
                        int nHeaderIndex = 1;
                        cell = sheet.Cells[nRowIndex, ++nHeaderIndex]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nHeaderIndex]; cell.Value = "Import LCNo"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nHeaderIndex]; cell.Value = "Import LCDate"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nHeaderIndex]; cell.Value = "LC Amount"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, ++nHeaderIndex]; cell.Value = "Import InvoiceNo"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, ++nHeaderIndex]; cell.Value = "Invoice Date"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nHeaderIndex]; cell.Value = "Invoice Amount"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nHeaderIndex]; cell.Value = "Bill No"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nHeaderIndex]; cell.Value = "Bill Date"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nHeaderIndex]; cell.Value = "Payment Type"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nHeaderIndex]; cell.Value = "Supplier Name"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nHeaderIndex]; cell.Value = "Cost Head Name"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nHeaderIndex]; cell.Value = "Cost Amount"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nRowIndex = nRowIndex + 1;
                        IsTotalPrint = true;
                        nLCID = oItem.LCID;
                        #endregion
                    }

                    #region Data
                    int nDataIndex = 1;
                    cell = sheet.Cells[nRowIndex, ++nDataIndex]; cell.Value = nCount.ToString(); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                    cell.Style.Numberformat.Format = "###0;(###0)";
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    
                    cell = sheet.Cells[nRowIndex, ++nDataIndex]; cell.Value = oItem.ImportLCNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nDataIndex]; cell.Value = oItem.ImportLCDate.ToString("dd MMM yyyy"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nDataIndex]; cell.Value = oItem.ImportLCAmount; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nDataIndex]; cell.Value = oItem.ImportInvoiceNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nDataIndex]; cell.Value = oItem.ImportInvoiceDate.ToString("dd MMM yyyy"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                       
                    cell = sheet.Cells[nRowIndex, ++nDataIndex]; cell.Value = oItem.ImportInvoiceAmount; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, ++nDataIndex]; cell.Value = oItem.BillNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nDataIndex]; cell.Value = oItem.DateofBill.ToString("dd MMM yyyy"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nDataIndex]; cell.Value = oItem.PaymentMethod.ToString(); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nDataIndex]; cell.Value = oItem.SupplierName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nDataIndex]; cell.Value = oItem.CostHeadName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                       
                    cell = sheet.Cells[nRowIndex, ++nDataIndex]; cell.Value = oItem.Amount; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    #endregion

                    nEndRow = nRowIndex;
                    nRowIndex++;

                    //Grand total
                    nGrandTotalAmount = nGrandTotalAmount + oItem.Amount;
                }

                #region Total
                int nTotalIndex = 13;
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nTotalIndex]; cell.Merge = true;
                cell.Value = "Sub Total :"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                //Amount
                sStartCell = Global.GetExcelCellName(nStartRow + 1, ++nTotalIndex);
                sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                cell = sheet.Cells[nRowIndex, nTotalIndex]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nRowIndex++;
                #endregion


                #region Grand Total Total

                nTotalIndex = 13;
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nTotalIndex];
                cell.Merge = true;
                cell.Value = "Grand Total :"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                //Amount
                cell = sheet.Cells[nRowIndex, ++nTotalIndex]; cell.Value = nGrandTotalAmount; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nRowIndex++;
                #endregion

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=LandingCostRegister.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion


        }
        #endregion

        #region Support Functions
        private string GetSQL(LandingCostRegister oLandingCostRegister)
        {
            _sDateRange = "";
            string sSearchingData = oLandingCostRegister.SearchingData;
            EnumCompareOperator eBillDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[0]);
            DateTime dBillStartDate = Convert.ToDateTime(sSearchingData.Split('~')[1]);
            DateTime dBillEndDate = Convert.ToDateTime(sSearchingData.Split('~')[2]);

            EnumCompareOperator eInvoiceDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[3]);
            DateTime dInvoiceDateStartDate = Convert.ToDateTime(sSearchingData.Split('~')[4]);
            DateTime dInvoiceDateEndDate = Convert.ToDateTime(sSearchingData.Split('~')[5]);

            EnumCompareOperator eApprovedDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[6]);
            DateTime dApprovedStartDate = Convert.ToDateTime(sSearchingData.Split('~')[7]);
            DateTime dApprovedEndDate = Convert.ToDateTime(sSearchingData.Split('~')[8]);

            EnumCompareOperator eBillAmount = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[9]);
            double nBillAmountStsrt = Convert.ToDouble(sSearchingData.Split('~')[10]);
            double nBillAmountEnd = Convert.ToDouble(sSearchingData.Split('~')[11]);


            string sSQLQuery = "", sWhereCluse = "";

            #region BusinessUnit
            if (oLandingCostRegister.BUID > 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " Inv.BUID =" + oLandingCostRegister.BUID.ToString();
            }
            #endregion

            #region ApprovedBy
            if (oLandingCostRegister.ApprovedBy != 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " Inv.ApprovedBy =" + oLandingCostRegister.ApprovedBy.ToString();
            }
            #endregion

            #region BillNo
            if (oLandingCostRegister.BillNo != null && oLandingCostRegister.BillNo != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " Inv.BillNo LIKE '%" + oLandingCostRegister.BillNo + "%'";
            }
            #endregion

            #region ImportLCNo
            if (oLandingCostRegister.ImportLCNo != null && oLandingCostRegister.ImportLCNo != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " InvDtl.LCID IN (SELECT MM.ImportLCID FROM ImportLC AS MM WHERE MM.ImportLCNo LIKE '%" + oLandingCostRegister.ImportLCNo + "%')";
            }
            #endregion

            #region ImportInvoiceNo
            if (oLandingCostRegister.ImportInvoiceNo != null && oLandingCostRegister.ImportInvoiceNo != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " InvDtl.InvoiceID IN (SELECT MM.ImportInvoiceID FROM ImportInvoice AS MM WHERE MM.ImportInvoiceNo LIKE '%" + oLandingCostRegister.ImportInvoiceNo + "%')";
            }
            #endregion

            #region Supplier
            if (oLandingCostRegister.SupplierName != null && oLandingCostRegister.SupplierName != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " Inv.ContractorID IN(" + oLandingCostRegister.SupplierName + ")";
            }
            #endregion
            
            #region ProductType
            if (oLandingCostRegister.ProductType != EnumProductNature.Dyeing)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " InvDtl.InvoiceID IN (SELECT MM.ImportInvoiceID FROM ImportInvoiceDetail AS MM WHERE MM.ProductID IN ( SELECT ProductID FROM Product AS HH WHERE HH.Activity = 1 and  HH.ProductCategoryID IN (SELECT ProductCategoryID FROM [dbo].[fn_GetProductCategoryBU](" + oLandingCostRegister.BUID + ", " + (int)oLandingCostRegister.ProductType + ")))) ";
            }
            #endregion
                       
            #region Bill Date
            if (eBillDate != EnumCompareOperator.None)
            {
                if (eBillDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),Inv.DateofBill,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dBillStartDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "Bill Date @ " + dBillStartDate.ToString("dd MMM yyyy");
                }
                else if (eBillDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),Inv.DateofBill,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dBillStartDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "Bill Date Not Equal @ " + dBillStartDate.ToString("dd MMM yyyy");
                }
                else if (eBillDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),Inv.DateofBill,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dBillStartDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "Bill Date Greater Then @ " + dBillStartDate.ToString("dd MMM yyyy");
                }
                else if (eBillDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),Inv.DateofBill,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dBillStartDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "Bill Date Smaller Then @ " + dBillStartDate.ToString("dd MMM yyyy");
                }
                else if (eBillDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),Inv.DateofBill,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dBillStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dBillEndDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "Bill Date Between " + dBillStartDate.ToString("dd MMM yyyy") + " To " + dBillEndDate.ToString("dd MMM yyyy");
                }
                else if (eBillDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),Inv.DateofBill,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dBillStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dBillEndDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "Bill Date NOT Between " + dBillStartDate.ToString("dd MMM yyyy") + " To " + dBillEndDate.ToString("dd MMM yyyy");
                }
            }
            #endregion

            #region Invoice Date
            if (eInvoiceDate != EnumCompareOperator.None)
            {
                if (eInvoiceDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),Inv.DateofInvoice,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dInvoiceDateStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eInvoiceDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),Inv.DateofInvoice,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dInvoiceDateStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eInvoiceDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),Inv.DateofInvoice,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dInvoiceDateStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eInvoiceDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),Inv.DateofInvoice,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dInvoiceDateStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eInvoiceDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),Inv.DateofInvoice,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dInvoiceDateStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dInvoiceDateEndDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eInvoiceDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),Inv.DateofInvoice,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dInvoiceDateStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dInvoiceDateEndDate.ToString("dd MMM yyyy") + "', 106))";
                }
            }
            #endregion

            #region Approved Date
            if (eApprovedDate != EnumCompareOperator.None)
            {
                if (eApprovedDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),Inv.ApprovedDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApprovedStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eApprovedDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),Inv.ApprovedDate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApprovedStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eApprovedDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),Inv.ApprovedDate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApprovedStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eApprovedDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),Inv.ApprovedDate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApprovedStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eApprovedDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),Inv.ApprovedDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApprovedStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApprovedEndDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eApprovedDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),Inv.ApprovedDate,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApprovedStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApprovedEndDate.ToString("dd MMM yyyy") + "', 106))";
                }
            }
            #endregion

            #region Amount
            if (eBillAmount != EnumCompareOperator.None)
            {
                if (eBillAmount == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " InvDtl.Amount = " + nBillAmountStsrt.ToString("0.00");
                }
                else if (eBillAmount == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " InvDtl.Amount != " + nBillAmountStsrt.ToString("0.00"); ;
                }
                else if (eBillAmount == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " InvDtl.Amount > " + nBillAmountStsrt.ToString("0.00"); ;
                }
                else if (eBillAmount == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " InvDtl.Amount < " + nBillAmountStsrt.ToString("0.00"); ;
                }
                else if (eBillAmount == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " InvDtl.Amount BETWEEN " + nBillAmountStsrt.ToString("0.00") + " AND " + nBillAmountEnd.ToString("0.00");
                }
                else if (eBillAmount == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " InvDtl.Amount NOT BETWEEN " + nBillAmountStsrt.ToString("0.00") + " AND " + nBillAmountEnd.ToString("0.00");
                }
            }
            #endregion

            sSQLQuery = sSQLQuery + sWhereCluse;
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

