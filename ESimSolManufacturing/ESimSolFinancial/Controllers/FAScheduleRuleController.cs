using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.IO;
using System.Drawing.Imaging;
using ESimSol.Reports;
using System.Drawing;
using OfficeOpenXml.Style;
using OfficeOpenXml;

namespace ESimSolFinancial.Controllers
{
    public class FAScheduleRuleController : Controller
    {
        #region Declaration
        FAScheduleRule _oFAScheduleRule = new FAScheduleRule();
        List<FAScheduleRule> _oFAScheduleRules = new List<FAScheduleRule>();
        FAScheduleReport _oFAScheduleReport = new FAScheduleReport();
        List<FAScheduleReport> _oFAScheduleReports = new List<FAScheduleReport>();
        #endregion

        #region Actions
        public ActionResult ViewFAScheduleRules(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            _oFAScheduleRules = new List<FAScheduleRule>();
            _oFAScheduleRules = FAScheduleRule.Gets((int)Session[SessionInfo.currentUserID]);
            return View(_oFAScheduleRules);
        }
        public ActionResult ViewFAScheduleRule(int id)
        {
            _oFAScheduleRule = new FAScheduleRule();
            if (id > 0)
            {
                _oFAScheduleRule = _oFAScheduleRule.Get(id, (int)Session[SessionInfo.currentUserID]);
            }
            ViewBag.FAMethods = EnumObject.jGets(typeof(EnumFAMethod));
            ViewBag.DEPCalculateOns = EnumObject.jGets(typeof(EnumDateDisplayPart));
            ViewBag.Currencys = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            return View(_oFAScheduleRule);
        }
        [HttpPost]
        public JsonResult Save(FAScheduleRule oFAScheduleRule)
        {
            _oFAScheduleRule = new FAScheduleRule();
            try
            {
                _oFAScheduleRule = oFAScheduleRule;
                _oFAScheduleRule = _oFAScheduleRule.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFAScheduleRule = new FAScheduleRule();
                _oFAScheduleRule.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFAScheduleRule);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Delete(FAScheduleRule oFAScheduleRule)
        {
            string sFeedBackMessage = "";
            try
            {
                _oFAScheduleRule = new FAScheduleRule();
                sFeedBackMessage = oFAScheduleRule.Delete(oFAScheduleRule.FAScheduleRuleID, (int)Session[SessionInfo.currentUserID]);
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
        public JsonResult GetsProductByType(Product oProduct)
        {
            List<Product> oProducts = new List<Product>();
            try
            {
                string sSQL = "SELECT * FROM Product WHERE ProductType=2 ";

                if (!string.IsNullOrEmpty(oProduct.ProductName))
                    sSQL += " AND ProductName Like '%" + oProduct.ProductName + "%'";

                oProducts = Product.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFAScheduleRule = new FAScheduleRule();
                _oFAScheduleRule.ErrorMessage = ex.Message;
            }
            var jsonResult = Json(oProducts, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #endregion

        #region FASchedule & Report
        public ActionResult ViewFAScheduleReport(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            _oFAScheduleReports = new List<FAScheduleReport>();
            //_oFAScheduleReports = FAScheduleReport.Gets(DateTime.Now.Year, (int)Session[SessionInfo.currentUserID]);

            ViewBag.BUID = buid;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));

            ViewBag.FAMethods = EnumObject.jGets(typeof(EnumFAMethod));
            ViewBag.DEPCalculateOns = EnumObject.jGets(typeof(EnumDateDisplayPart));
            ViewBag.Currencys = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            return View(_oFAScheduleReports);
        }
        [HttpPost]
        public JsonResult GetsGridData(FAScheduleReport oFAScheduleReport)
        {
            List<FAScheduleReport> oFAScheduleReportList = new List<FAScheduleReport>();
            oFAScheduleReportList = new List<FAScheduleReport>();
            try
            {
                //oFAScheduleReportList = FAScheduleReport.Gets(oFAScheduleReport.EndDate.Year, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFAScheduleReportList = new List<FAScheduleReport>();
                oFAScheduleReport.ErrorMessage = ex.Message;
                oFAScheduleReportList.Add(oFAScheduleReport);
            }
            var jSonResult = Json(oFAScheduleReportList, JsonRequestBehavior.AllowGet);
            jSonResult.MaxJsonLength = int.MaxValue;
            return jSonResult;
        }
        public void ExportToExcel_Report(string sTempString)
        {
            _oFAScheduleReports = new List<FAScheduleReport>();
            _oFAScheduleReport = new FAScheduleReport();
            BusinessUnit oBusinessUnit = new BusinessUnit();

            DateTime dDateYear = DateTime.Now; int BUID = 0;
            string _sDateRange = "", _sErrorMesage = "";
            if (!string.IsNullOrEmpty(sTempString))
            {
                dDateYear = Convert.ToDateTime(sTempString.Split('~')[0]);
                BUID = Convert.ToInt32(sTempString.Split('~')[1]);
            }

            try
            {
                oBusinessUnit = oBusinessUnit.Get(_oFAScheduleReport.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                _sDateRange = "Date: " + dDateYear.ToString("yyyy");

                //_oFAScheduleReports = FAScheduleReport.Gets(dDateYear.Year, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oFAScheduleReports.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oFAScheduleReports = new List<FAScheduleReport>();
                _sErrorMesage = ex.Message;
            }

            if (_sErrorMesage == "")
            {
                #region Header
                List<TableHeader> table_header = new List<TableHeader>();
                table_header.Add(new TableHeader { Header = "#SL", Width = 10f, IsRotate = false, Align = TextAlign.Center });
                table_header.Add(new TableHeader { Header = "Purchase Date", Width = 35f, IsRotate = false, Align = TextAlign.Center });
                table_header.Add(new TableHeader { Header = "Product Category", Width = 35f, IsRotate = false, Align = TextAlign.Center });
                table_header.Add(new TableHeader { Header = "Product Name", Width = 35f, IsRotate = false, Align = TextAlign.Center });

                table_header.Add(new TableHeader { Header = "Openning Cost", Width = 35f, IsRotate = false, Align = TextAlign.Center });
                table_header.Add(new TableHeader { Header = "Salvage Value", Width = 35f, IsRotate = false, Align = TextAlign.Center });
                table_header.Add(new TableHeader { Header = "Percentage", Width = 35f, IsRotate = false, Align = TextAlign.Center });
                table_header.Add(new TableHeader { Header = "UsefulLifetime", Width = 35f, IsRotate = false, Align = TextAlign.Center });
                table_header.Add(new TableHeader { Header = "Depreciation/Year", Width = 35f, IsRotate = false, Align = TextAlign.Center });
                table_header.Add(new TableHeader { Header = "Total DEP Cost", Width = 35f, IsRotate = false, Align = TextAlign.Center });
                table_header.Add(new TableHeader { Header = "Closing Cost", Width = 35f, IsRotate = false, Align = TextAlign.Center });

                //table_header.Add(new TableHeader
                //{
                //    Header = "Qty",
                //    ChildHeader = new List<TableHeader>()
                //     {
                //        new TableHeader { Header = "Unit", Width = 10f, IsRotate = false, Align = TextAlign.Center },
                //        new TableHeader { Header = "Openning Qty", Width = 10f, IsRotate = false, Align = TextAlign.Center },
                //        new TableHeader { Header = "In Qty", Width = 10f, IsRotate = false, Align = TextAlign.Center },
                //        new TableHeader { Header = "Out Qty", Width = 10f, IsRotate = false, Align = TextAlign.Center },
                //        new TableHeader { Header = "Closing Qty", Width = 10f, IsRotate = false, Align = TextAlign.Center }
                //     },
                //    Width = 50f,
                //    IsRotate = false,
                //    Align = TextAlign.Center
                //});

                #endregion

                #region Export Excel
                int nRowIndex = 2, nStartCol = 2, nEndCol = 12;
                ExcelRange cell; ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Fixed Assest Ledger");
                    sheet.Name = "Fixed Assest Ledger";

                    ExcelTool.SetColumnWidth(table_header, ref sheet, ref nStartCol, ref nEndCol);
                    nEndCol = 14;
                    #region Report Header
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, 7]; cell.Merge = true;
                    cell.Value = "Fixed Assest Ledger"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, 8, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = _sDateRange; cell.Style.Font.Bold = true;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Address & Date
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, 7]; cell.Merge = true;
                    cell.Value = oBusinessUnit.Address; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, 8, nRowIndex, 10]; cell.Merge = true;
                    cell.Value = ""; cell.Style.Font.Bold = false;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Data

                    nRowIndex++;

                    nStartCol = 2;

                    ExcelTool.GenerateHeader(table_header, ref sheet, ref nRowIndex, 2, nEndCol, 10, true, true);

                    int nCount = 0; nEndCol = 14;
                    foreach (var obj in _oFAScheduleReports)
                    {
                        nStartCol = 2;
                        ExcelTool.Formatter = "";
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, (++nCount).ToString(), true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.PurchaseDateSt, false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.ProductCategoryName, false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.ProductName, false);


                        ExcelTool.Formatter = " #,##0;(#,##0)";
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.OpenningCost.ToString(), true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.SalvageValue.ToString(), true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.DEPPercentage.ToString(), true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.UsefulLifetime.ToString(), true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.DepreciationperYear.ToString(), true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.TotalAccumulatedCost.ToString(), true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.ClosingCost.ToString(), true);

                        nRowIndex++;
                    }
                    #endregion

                    nRowIndex++;

                    cell = sheet.Cells[1, 1, nRowIndex, 14];

                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=FixedAssestLedger.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
        }
        #endregion
    }
}
