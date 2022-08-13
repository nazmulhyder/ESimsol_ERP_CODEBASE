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
    public class PurchaseRequisitionRegisterController : Controller
    {   
        string _sDateRange = "";
        string _sErrorMesage = "";
        string _sFormatter = "";
        List<PurchaseRequisitionRegister> _oPurchaseRequisitionRegisters = new List<PurchaseRequisitionRegister>();        

        #region Actions
        public ActionResult ViewPurchaseRequisitionRegisters(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            PurchaseRequisitionRegister oPurchaseRequisitionRegister = new PurchaseRequisitionRegister();

            #region Approval User
            string sSQL = "SELECT * FROM View_User AS HH WHERE HH.UserID IN (SELECT DISTINCT MM.ApprovedBy FROM PurchaseInvoice AS MM WHERE ISNULL(MM.ApprovedBy,0)!=0) ORDER BY HH.UserName";
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
                if ((EnumReportLayout)oItem.id == EnumReportLayout.DateWiseDetails || (EnumReportLayout)oItem.id == EnumReportLayout.DepartmentWise || (EnumReportLayout)oItem.id == EnumReportLayout.ProductWise)
                {
                    oReportLayouts.Add(oItem);
                }
            }
            #endregion

            ViewBag.BUID = buid;
            ViewBag.ApprovalUsers = oApprovalUsers;
            ViewBag.ReportLayouts = oReportLayouts;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.PurchaseRequisitionStates = EnumObject.jGets(typeof(EnumPurchaseRequisitionStatus));
            
            return View(oPurchaseRequisitionRegister);
        }

        [HttpPost]
        public ActionResult SetSessionSearchCriteria(PurchaseRequisitionRegister oPurchaseRequisitionRegister)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oPurchaseRequisitionRegister);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintPurchaseRequisitionRegister(double ts)
        {
            PurchaseRequisitionRegister oPurchaseRequisitionRegister = new PurchaseRequisitionRegister();
            try
            {
                _sErrorMesage = "";
                _oPurchaseRequisitionRegisters = new List<PurchaseRequisitionRegister>();                
                oPurchaseRequisitionRegister = (PurchaseRequisitionRegister)Session[SessionInfo.ParamObj];
                string sSQL = this.GetSQL(oPurchaseRequisitionRegister);
                _oPurchaseRequisitionRegisters = PurchaseRequisitionRegister.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (_oPurchaseRequisitionRegisters.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oPurchaseRequisitionRegisters = new List<PurchaseRequisitionRegister>();
                _sErrorMesage = ex.Message;
            }

            if (_sErrorMesage == "")
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                BusinessUnit oBU = new BusinessUnit();
                oBU = oBU.Get(_oPurchaseRequisitionRegisters.Max(x=>x.BUID), (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBU);

                rptPurchaseRequisitionRegisters oReport = new rptPurchaseRequisitionRegisters();
                byte[] abytes = oReport.PrepareReport(_oPurchaseRequisitionRegisters, oCompany, oPurchaseRequisitionRegister.ReportLayout, _sDateRange);
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
    
        #region Excel
        private ExcelRange FillCell(ExcelWorksheet sheet, int nRowIndex, int nStartCol, string sVal, bool IsNumber)
        {
           return FillCell( sheet, nRowIndex, nStartCol, sVal, IsNumber, false);
        }
        private ExcelRange FillCell(ExcelWorksheet sheet, int nRowIndex, int nStartCol, string sVal, bool IsNumber, bool IsBold)
        {
            ExcelRange cell;
            OfficeOpenXml.Style.Border border;

            cell = sheet.Cells[nRowIndex, nStartCol++];
            if (IsNumber)
                cell.Value = Convert.ToDouble(sVal);
            else
                cell.Value = sVal;
            cell.Style.Font.Bold = IsBold;
            cell.Style.WrapText = true;
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

            if (IsNumber)
            {
                cell.Style.Numberformat.Format = _sFormatter;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            }
            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            return cell;
        }
        public void ExportToExcelPurchaseRequisitionRegister(double ts)
        {
            int PRID=-999;
            string Header = "", HeaderColumn = "";

            Company oCompany = new Company();
            PurchaseRequisitionRegister oPurchaseRequisitionRegister = new PurchaseRequisitionRegister();
            try
            {
                _sErrorMesage = "";

                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oPurchaseRequisitionRegisters = new List<PurchaseRequisitionRegister>();
                oPurchaseRequisitionRegister = (PurchaseRequisitionRegister)Session[SessionInfo.ParamObj];
                string sSQL = this.GetSQL(oPurchaseRequisitionRegister);
                _oPurchaseRequisitionRegisters = PurchaseRequisitionRegister.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (_oPurchaseRequisitionRegisters.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oPurchaseRequisitionRegisters = new List<PurchaseRequisitionRegister>();
                _sErrorMesage = ex.Message;
            }

            if (_sErrorMesage == "") 
            {
                #region Header
                List<TableHeader> table_header = new List<TableHeader>();
                table_header.Add(new TableHeader { Header = "#SL", Width = 10f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "PR No", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "PR Status", Width = 15f, IsRotate = false });

                if (oPurchaseRequisitionRegister.ReportLayout == EnumReportLayout.DateWiseDetails)
                    table_header.Add(new TableHeader { Header = "Department Name", Width = 45f, IsRotate = false });
                else
                    table_header.Add(new TableHeader { Header = "PR Date", Width = 25f, IsRotate = false });

                table_header.Add(new TableHeader { Header = "Requirement Date", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Approved By Name", Width = 25f, IsRotate = false });

                if (oPurchaseRequisitionRegister.ReportLayout == EnumReportLayout.ProductWise)
                {
                    table_header.Add(new TableHeader { Header = "Required Date", Width = 20f, IsRotate = false });
                    table_header.Add(new TableHeader { Header = "Department Name", Width = 45f, IsRotate = false });
                }
                else
                {
                    table_header.Add(new TableHeader { Header = "Product Code", Width = 20f, IsRotate = false });
                    table_header.Add(new TableHeader { Header = "Product Name", Width = 45f, IsRotate = false });
                }

                table_header.Add(new TableHeader { Header = "M Unit", Width = 10f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Qty", Width = 10f, IsRotate = false });
                #endregion

                #region Layout Wise Header
                if (oPurchaseRequisitionRegister.ReportLayout == EnumReportLayout.ProductWise)
                {
                    Header = "Product Wise"; HeaderColumn = "Product Name : ";
                }
                else if (oPurchaseRequisitionRegister.ReportLayout == EnumReportLayout.DepartmentWise)
                {
                    Header = "Department Wise"; HeaderColumn = "Department Name : ";
                }
                else if (oPurchaseRequisitionRegister.ReportLayout == EnumReportLayout.DateWiseDetails)
                {
                    Header = "Date Wise"; HeaderColumn = "PO Date : ";
                }
                #endregion

                #region Export Excel
                int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count;
                ExcelRange cell; ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Purchase Reuisition Register");
                    sheet.Name = "Purchase Reuisition Register";

                    foreach (TableHeader listItem in table_header)
                    {
                        sheet.Column(nStartCol++).Width = listItem.Width;
                    }

                    #region Report Header
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, 6]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, 7, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = "Purchase Reuisition Register (" + Header+") "; cell.Style.Font.Bold = true;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Address & Date
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, 6]; cell.Merge = true;
                    cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, 7, nRowIndex, 10]; cell.Merge = true;
                    cell.Value = ""; cell.Style.Font.Bold = false;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Group By Layout Wise
                    var data = _oPurchaseRequisitionRegisters.GroupBy(x => new { x.DepartmentName, x.DepartmentID}, (key, grp) => new
                    {
                        HeaderName = key.DepartmentName,
                        TotalQty = grp.Sum(x => x.Qty),
                        Results = grp.ToList()
                    });

                    if (oPurchaseRequisitionRegister.ReportLayout == EnumReportLayout.ProductWise)
                    {
                        data = _oPurchaseRequisitionRegisters.GroupBy(x => new { x.ProductName, x.ProductID }, (key, grp) => new
                        {
                            HeaderName = key.ProductName,
                            TotalQty = grp.Sum(x => x.Qty),
                            Results = grp.ToList()
                        });
                    }
                    else if (oPurchaseRequisitionRegister.ReportLayout == EnumReportLayout.DateWiseDetails)
                    {
                        data = _oPurchaseRequisitionRegisters.GroupBy(x => new { x.PRDateSt }, (key, grp) => new
                        {
                            HeaderName = key.PRDateSt,
                            TotalQty = grp.Sum(x => x.Qty),
                            Results = grp.ToList()
                        });
                    }
                    #endregion

                    #region Data
                    foreach (var oItem in data)
                    {
                        nRowIndex++;

                        nStartCol = 2;
                        FillCellMerge(ref sheet, HeaderColumn + oItem.HeaderName, nRowIndex, nRowIndex, nStartCol, nEndCol + 1, true, ExcelHorizontalAlignment.Left);

                        nRowIndex++;
                        foreach (TableHeader listItem in table_header)
                        {
                            cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = listItem.Header; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }

                        PRID = 0;
                        nRowIndex++; int nCount = 0, nRowSpan = 0;
                        foreach (var obj in oItem.Results)
                        {
                            #region Order Wise Merge
                            if (PRID != obj.PRID)
                            {
                                if (nCount > 0)
                                {
                                    nStartCol = 8;
                                    FillCellMerge(ref sheet, "Sub Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 2, true, ExcelHorizontalAlignment.Right);

                                    _sFormatter = " #,##0;(#,##0)";
                                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.Results.Where(x => x.PRID == PRID).Sum(x => x.Qty).ToString(), true, true);
                                    nRowIndex++;
                                }

                                nStartCol = 2;
                                nRowSpan = oItem.Results.Where(OrderR => OrderR.PRID == obj.PRID).ToList().Count;

                                FillCellMerge(ref sheet, (++nCount).ToString(), nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
                                FillCellMerge(ref sheet, obj.PRNo, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
                                FillCellMerge(ref sheet, obj.StatusSt, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);

                                if (oPurchaseRequisitionRegister.ReportLayout == EnumReportLayout.DateWiseDetails)
                                    FillCellMerge(ref sheet, obj.DepartmentName, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
                                else
                                    FillCellMerge(ref sheet, obj.PRDateSt, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);

                                FillCellMerge(ref sheet, obj.RequirementDateInString, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
                                FillCellMerge(ref sheet, obj.ApprovedByName, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
                            }
                            #endregion

                            nStartCol = 8;

                            if (oPurchaseRequisitionRegister.ReportLayout == EnumReportLayout.ProductWise)
                            {
                                FillCell(sheet, nRowIndex, nStartCol++, obj.ApproveDateSt, false);
                                FillCell(sheet, nRowIndex, nStartCol++, obj.DepartmentName, false);
                            }
                            else
                            {
                                FillCell(sheet, nRowIndex, nStartCol++, obj.ProductCode, false);
                                FillCell(sheet, nRowIndex, nStartCol++, obj.ProductName, false);
                            }

                            FillCell(sheet, nRowIndex, nStartCol++, obj.UnitName, false);
                            _sFormatter = " #,##0;(#,##0)";
                            FillCell(sheet, nRowIndex, nStartCol++, obj.Qty.ToString(), true);
                            nRowIndex++;

                            PRID = obj.PRID;
                        }

                        nStartCol = 8;
                        FillCellMerge(ref sheet, "Sub Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 2, true, ExcelHorizontalAlignment.Right);
                        _sFormatter = " #,##0;(#,##0)";
                        FillCell(sheet, nRowIndex, ++nStartCol, oItem.Results.Where(x => x.PRID == PRID).Sum(x => x.Qty).ToString(), true, true);
                        nRowIndex++;

                        nStartCol = 2; _sFormatter = " #,##0;(#,##0)";
                        FillCellMerge(ref sheet, Header + " Sub Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 8, true, ExcelHorizontalAlignment.Right);
                        FillCell(sheet, nRowIndex, ++nStartCol, oItem.TotalQty.ToString(), true, true);
                        nRowIndex++;
                    }

                    #region Grand Total
                    nStartCol = 2; _sFormatter = " #,##0;(#,##0)";
                    FillCellMerge(ref sheet, "Grand Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 8, true, ExcelHorizontalAlignment.Right);
                    FillCell(sheet, nRowIndex, ++nStartCol, data.Sum(x => x.TotalQty).ToString(), true, true);
                    nRowIndex++;
                    #endregion

                    cell = sheet.Cells[1, 1, nRowIndex, 11];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    #endregion
                    
                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=PurchaseRequisitionRegister(" + Header + ").xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
        }
        private void FillCellMerge(ref ExcelWorksheet sheet, int startRowIndex, int endRowIndex, int startColIndex, int endColIndex, string sVal)
        {
            ExcelRange cell;
            OfficeOpenXml.Style.Border border;

            cell = sheet.Cells[startRowIndex, startColIndex, endRowIndex, endColIndex];
            cell.Merge = true;
            cell.Value = sVal;
            cell.Style.Font.Bold = false;
            cell.Style.WrapText = true;
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
        }
        private void FillCellMerge(ref ExcelWorksheet sheet,string sVal, int startRowIndex, int endRowIndex, int startColIndex, int endColIndex)
        {
            FillCellMerge(ref sheet, sVal, startRowIndex, endRowIndex, startColIndex, endColIndex, false, ExcelHorizontalAlignment.Left);
        }
        private void FillCellMerge(ref ExcelWorksheet sheet, string sVal, int startRowIndex, int endRowIndex, int startColIndex, int endColIndex, bool isBold, ExcelHorizontalAlignment HoriAlign)
        {
            ExcelRange cell;
            OfficeOpenXml.Style.Border border;

            cell = sheet.Cells[startRowIndex, startColIndex, endRowIndex, endColIndex];
            cell.Merge = true;
            cell.Value = sVal;
            cell.Style.Font.Bold = isBold;
            cell.Style.WrapText = true;
            cell.Style.HorizontalAlignment = HoriAlign;
            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        }
        #endregion

        #region Support Functions
        private string GetSQL(PurchaseRequisitionRegister oPurchaseRequisitionRegister)
        {
            _sDateRange = "";
            string sSearchingData = oPurchaseRequisitionRegister.SearchingData;
            EnumCompareOperator ePRDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[0]);
            DateTime dPurchaseRequisitionStartDate = Convert.ToDateTime(sSearchingData.Split('~')[1]);
            DateTime dPurchaseRequisitionEndDate = Convert.ToDateTime(sSearchingData.Split('~')[2]);

            EnumCompareOperator eRequirementDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[3]);
            DateTime dRequirementDateStartDate = Convert.ToDateTime(sSearchingData.Split('~')[4]);
            DateTime dRequirementDateEndDate = Convert.ToDateTime(sSearchingData.Split('~')[5]);

            EnumCompareOperator eApproveDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[6]);
            DateTime dApprovedStartDate = Convert.ToDateTime(sSearchingData.Split('~')[7]);
            DateTime dApprovedEndDate = Convert.ToDateTime(sSearchingData.Split('~')[8]);

            string sSQLQuery = "", sWhereCluse = "", sGroupBy = "", sOrderBy = "";

            #region BusinessUnit
            if (oPurchaseRequisitionRegister.BUID > 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " BUID =" + oPurchaseRequisitionRegister.BUID.ToString();
            }
            #endregion

            #region PRNo
            if (oPurchaseRequisitionRegister.PRNo != null && oPurchaseRequisitionRegister.PRNo != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " PRNo LIKE'%" + oPurchaseRequisitionRegister.PRNo + "%'";
            }
            #endregion

            #region RequisitionBy
            if (oPurchaseRequisitionRegister.RequisitionBy != 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " RequisitionBy =" + oPurchaseRequisitionRegister.RequisitionBy;
            }
            #endregion

            #region ApproveBy
            if (oPurchaseRequisitionRegister.ApproveBy != 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " ApproveBy =" + oPurchaseRequisitionRegister.ApproveBy;
            }
            #endregion

            #region PurchaseRequisitionStatus
            if (oPurchaseRequisitionRegister.Status>=0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " Status =" + oPurchaseRequisitionRegister.Status;
            }
            #endregion

            #region Remarks
            if (oPurchaseRequisitionRegister.Remarks != null && oPurchaseRequisitionRegister.Remarks != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " Note LIKE'%" + oPurchaseRequisitionRegister.Remarks + "%'";
            }
            #endregion

            #region DepartmentName
            if (oPurchaseRequisitionRegister.DepartmentName != null && oPurchaseRequisitionRegister.DepartmentName != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " DepartmentID IN(" + oPurchaseRequisitionRegister.DepartmentName + ")";
            }
            #endregion

            #region Product
            if (oPurchaseRequisitionRegister.ProductName != null && oPurchaseRequisitionRegister.ProductName != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " ProductID IN(" + oPurchaseRequisitionRegister.ProductName + ")";
            }
            #endregion

            #region PRDate
            if (ePRDate != EnumCompareOperator.None)
            {
                if (ePRDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),PRDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPurchaseRequisitionStartDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "PO Date @ " + dPurchaseRequisitionStartDate.ToString("dd MMM yyyy");
                }
                else if (ePRDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),PRDate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPurchaseRequisitionStartDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "PO Date Not Equal @ " + dPurchaseRequisitionStartDate.ToString("dd MMM yyyy");
                }
                else if (ePRDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),PRDate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPurchaseRequisitionStartDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "PO Date Greater Then @ " + dPurchaseRequisitionStartDate.ToString("dd MMM yyyy");
                }
                else if (ePRDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),PRDate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPurchaseRequisitionStartDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "PO Date Smaller Then @ " + dPurchaseRequisitionStartDate.ToString("dd MMM yyyy");
                }
                else if (ePRDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),PRDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPurchaseRequisitionStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPurchaseRequisitionEndDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "PO Date Between " + dPurchaseRequisitionStartDate.ToString("dd MMM yyyy") + " To " + dPurchaseRequisitionEndDate.ToString("dd MMM yyyy");
                }
                else if (ePRDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),PRDate,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPurchaseRequisitionStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dPurchaseRequisitionEndDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "PO Date NOT Between " + dPurchaseRequisitionStartDate.ToString("dd MMM yyyy") + " To " + dPurchaseRequisitionEndDate.ToString("dd MMM yyyy");
                }
            }
            #endregion

            #region Requisition Date
            if (eRequirementDate != EnumCompareOperator.None)
            {
                if (eRequirementDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),RequirementDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dRequirementDateStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eRequirementDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),RequirementDate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dRequirementDateStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eRequirementDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),RequirementDate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dRequirementDateStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eRequirementDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),RequirementDate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dRequirementDateStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eRequirementDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),RequirementDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dRequirementDateStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dRequirementDateEndDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eRequirementDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),RequirementDate,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dRequirementDateStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dRequirementDateEndDate.ToString("dd MMM yyyy") + "', 106))";
                }
            }
            #endregion

            #region Approved Date
            if (eApproveDate != EnumCompareOperator.None)
            {
                if (eApproveDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApprovedStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eApproveDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApprovedStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eApproveDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApprovedStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eApproveDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApprovedStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eApproveDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApprovedStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApprovedEndDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eApproveDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApprovedStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApprovedEndDate.ToString("dd MMM yyyy") + "', 106))";
                }
            }
            #endregion

            #region Report Layout
            if (oPurchaseRequisitionRegister.ReportLayout == EnumReportLayout.DateWiseDetails)
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT * FROM View_PurchaseRequisitionRegister ";
                sOrderBy = " ORDER BY  PRDate, PRID ASC";
            }
            else if (oPurchaseRequisitionRegister.ReportLayout == EnumReportLayout.DepartmentWise)
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT * FROM View_PurchaseRequisitionRegister ";
                sOrderBy = " ORDER BY  DepartmentName,DepartmentID, PRID ASC";
            }
            else if (oPurchaseRequisitionRegister.ReportLayout == EnumReportLayout.ProductWise)
            {
               sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
               sSQLQuery = "SELECT * FROM View_PurchaseRequisitionRegister ";
               sOrderBy = " ORDER BY  ProductName,ProductID, PRID ASC";
            }
            else
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT * FROM View_PurchaseRequisitionRegister ";
                sOrderBy = " ORDER BY PRDate, PRID ASC";
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

