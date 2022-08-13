using System;
using System.Collections.Generic;
using System.Dynamic;
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
using System.Collections;

using OfficeOpenXml;
using OfficeOpenXml.Style;
using ESimSol.BusinessObjects.ReportingObject;

namespace ESimSolFinancial.Controllers
{
    public class FabricExecutionOrderController : PdfViewController
    {
        //#region LightSource
        //public ActionResult ViewLightSources(int menuid)
        //{
        //    this.Session.Remove(SessionInfo.MenuID);
        //    this.Session.Add(SessionInfo.MenuID, menuid);

        //    List<LightSource> oLightSources = new List<LightSource>();
        //    oLightSources = LightSource.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
        //    return View(oLightSources);
        //}

        //[HttpPost]
        //public JsonResult SaveLightSource(LightSource oLightSource)
        //{
        //    try
        //    {
        //        oLightSource = oLightSource.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
        //    }
        //    catch (Exception ex)
        //    {
        //        oLightSource = new LightSource();
        //        oLightSource.ErrorMessage = ex.Message;
        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(oLightSource);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}

        //[HttpPost]
        //public JsonResult DeleteLightSource(LightSource oLightSource)
        //{
        //    string sFeedBackMessage = "";
        //    try
        //    {
        //        sFeedBackMessage = oLightSource.Delete(oLightSource.LightSourceID, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    }
        //    catch (Exception ex)
        //    {
        //        sFeedBackMessage = ex.Message;
        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(sFeedBackMessage);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}

        //#endregion

        public ActionResult ViewFEOYarnReceivesByChallan(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            //List<DeliveryChallan> oDeliveryChallans = new List<DeliveryChallan>();
            //string sSQL = this.GetSQLChallanForReceive();
            //sSQL=sSQL+" And ChallanID In (Select ChallanID from ChallanDetail Where ChallanDetailID Not In (Select ChallanDetailID from FabricExecutionOrderYarnReceive))";
            //oDeliveryChallans = DeliveryChallan.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            bool bIsDateSearch = false;
            DateTime dtFrom = DateTime.Today;
            Company oCompany = new Company();
            oCompany = oCompany.Get(((User)(Session[SessionInfo.CurrentUser])).CompanyID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (!string.IsNullOrEmpty(oCompany.BaseAddress) && oCompany.BaseAddress == "atml")
            {
                bIsDateSearch = true;
                dtFrom = new DateTime(dtFrom.Year, 3, 10);
            }


            List<FabricInHouseChallan> oFabricInHouseChallans = new List<FabricInHouseChallan>();
            oFabricInHouseChallans = FabricInHouseChallan.Gets("", "", "", bIsDateSearch, dtFrom, DateTime.Today, false, 0, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            oFabricInHouseChallans = oFabricInHouseChallans.Where(x => x.IsALLRcv == false).ToList();
            return View(oFabricInHouseChallans);
        }
        public ActionResult View_FEOSalesStatement(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<RptFEOSalesStatement> oFEOSalesStatements = new List<RptFEOSalesStatement>();
            ViewBag.OrderTypes = Enum.GetValues(typeof(EnumOrderType)).Cast<EnumOrderType>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            return View(oFEOSalesStatements);
        }

    
        public void ExportExcelFEOSalesStatus(string sParam)
        {
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<string> columnHead = new List<string>();
            List<int> colWidth = new List<int>();


            RptFEOSalesStatement oRptFEOSalesStatement = new RptFEOSalesStatement();
            oRptFEOSalesStatement.Params = sParam;


            List<RptFEOSalesStatement> oRptFEOSalesStatements = new List<RptFEOSalesStatement>();
            oRptFEOSalesStatements = GenerateFEOSalesStatement(oRptFEOSalesStatement);

            if (oRptFEOSalesStatements.Any() && oRptFEOSalesStatements.First().OrderDate != DateTime.MinValue)
            {
                //Get Distinct Month
                List<string> monthYear = oRptFEOSalesStatements.OrderBy(x => x.OrderDate).Select(x => x.OrderDateStr).Distinct().ToList();
                string sMonthRange = monthYear.FirstOrDefault() + " to " + monthYear.LastOrDefault();

                int rowIndex = 2;
                int nMaxColumn = 0;
                int colIndex = 2;
                ExcelRange cell;
                Border border;
                ExcelFill fill;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Fabric Order, Production & Sales Status";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Fabric Order, Production & Sales Status");
                    sheet.Name = "Fabric Order, Production & Sales Status";

                    columnHead = new List<string>(new string[] { "#SL", "Month", "Total Order Qty(Yards)", "Total Production Qty(Yards)", "Total Sales Qty(Yards)", "Total Sales Value(BDT)" });
                    colWidth = new List<int>(new int[] { 8, 10, 20, 20, 20, 20 });

                    #region Coloums

                    for (int i = 0; i < colWidth.Count(); i++)
                    {
                        sheet.Column(colIndex).Width = colWidth[i];
                        colIndex++;
                    }
                    nMaxColumn = colIndex - 1;

                    #endregion

                    #region Report Header
                    cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn]; cell.Merge = true; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    rowIndex++;

                    cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn - 2]; cell.Merge = true; cell.Style.Font.Bold = true; cell.Style.Font.Size = 12;
                    cell.Value = "Fabric Order, Production & Sales Status" + "(" + sMonthRange + ")"; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex++;
                    #endregion

                    #region Column Header

                    //Row 1

                    colIndex = 2;
                    for (int i = 0; i < colWidth.Count(); i++)
                    {
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = columnHead[i]; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                    }
                    rowIndex++;
                    #endregion

                    #region Body

                    int nCount = 0;
                    foreach (RptFEOSalesStatement oItem in oRptFEOSalesStatements)
                    {
                        colIndex = 2;
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (++nCount).ToString(); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.OrderDateStr; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.TotalExeQty; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.TotalProduction; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.TotalSalesQty; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.TotalSalesValue; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                        rowIndex++;
                    }

                    #endregion

                    #region Bottom Summary

                    colIndex = 3;
                    cell = sheet.Cells[rowIndex, 2, rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Total"; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oRptFEOSalesStatements.Sum(x => x.TotalExeQty); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oRptFEOSalesStatements.Sum(x => x.TotalProduction); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oRptFEOSalesStatements.Sum(x => x.TotalSalesQty); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oRptFEOSalesStatements.Sum(x => x.TotalSalesValue); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                    rowIndex++;

                    //Avg Per Month
                    int nMonth = oRptFEOSalesStatements.Count();
                    colIndex = 3;
                    cell = sheet.Cells[rowIndex, 2, rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Avg/ Month"; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oRptFEOSalesStatements.Sum(x => x.TotalExeQty) / nMonth; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oRptFEOSalesStatements.Sum(x => x.TotalProduction) / nMonth; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oRptFEOSalesStatements.Sum(x => x.TotalSalesQty) / nMonth; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oRptFEOSalesStatements.Sum(x => x.TotalSalesValue) / nMonth; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                    rowIndex++;
                    #endregion

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=FabricSalesStatement.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
            }
            else
            {
                ExcelRange cell;
                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Fabric Order, Production & Sales Status";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Fabric Order, Production & Sales Status");
                    sheet.Name = "Fabric Order, Production & Sales Status";

                    cell = sheet.Cells[2, 2, 2, 5]; cell.Merge = true; cell.Value = "No Data found"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=FabricOrderProductionSales Status.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }

            }

        }
        List<RptFEOSalesStatement> GenerateFEOSalesStatement(RptFEOSalesStatement oRptFEOSalesStatement)
        {
            List<RptFEOSalesStatement> oRptFEOSalesStatements = new List<RptFEOSalesStatement>();
            try
            {
                Int16 nExeType = Convert.ToInt16(oRptFEOSalesStatement.Params.Split('~')[0]);
                DateTime dtFrom = Convert.ToDateTime(oRptFEOSalesStatement.Params.Split('~')[1]);
                DateTime dtTo = Convert.ToDateTime(oRptFEOSalesStatement.Params.Split('~')[2]);

                oRptFEOSalesStatements = RptFEOSalesStatement.Gets(dtFrom, dtTo, nExeType, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oRptFEOSalesStatements = new List<RptFEOSalesStatement>();
                oRptFEOSalesStatement = new RptFEOSalesStatement();
                oRptFEOSalesStatement.ErrorMessage = ex.Message;
                oRptFEOSalesStatements.Add(oRptFEOSalesStatement);
            }
            return oRptFEOSalesStatements;
        }
        List<RptFEOSalesSummary> GenerateFEOSalesSummary(RptFEOSalesSummary oRptFEOSalesSummary, bool bIsOrderTypeWise)
        {
            List<RptFEOSalesSummary> oRptFEOSalesSummarys = new List<RptFEOSalesSummary>();
            try
            {
                Int16 nOrderType = Convert.ToInt16(oRptFEOSalesSummary.Params.Split('~')[0]);
                DateTime dtFrom = Convert.ToDateTime(oRptFEOSalesSummary.Params.Split('~')[1]);
                DateTime dtTo = Convert.ToDateTime(oRptFEOSalesSummary.Params.Split('~')[2]);
                Int16 nReportType = Convert.ToInt16(oRptFEOSalesSummary.Params.Split('~')[3]);
                Int16 nExeType = Convert.ToInt16(oRptFEOSalesSummary.Params.Split('~')[4]);

                bool bIsBuyerWise = (nReportType == 1 || nReportType == 2) ? true : false;
                oRptFEOSalesSummarys = RptFEOSalesSummary.Gets(nOrderType, dtFrom, dtTo, bIsBuyerWise, nExeType, bIsOrderTypeWise, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oRptFEOSalesSummarys = new List<RptFEOSalesSummary>();
                oRptFEOSalesSummary = new RptFEOSalesSummary();
                oRptFEOSalesSummary.ErrorMessage = ex.Message;
                oRptFEOSalesSummarys.Add(oRptFEOSalesSummary);
            }
            return oRptFEOSalesSummarys;
        }
        void BuyerWiseFEOSales(ref int nCount, int nMaxRow, List<string> columnHead, string sFirstLetters, List<RptFEOSalesSummary> oSummarys, int rowIndex, int colIndex, ref ExcelRange cell, ref ExcelWorksheet sheet)
        {
            Border border;
            ExcelFill fill;
            int startColIndex = colIndex;
            #region Column Header
            for (int i = 0; i < columnHead.Count(); i++)
            {
                cell = sheet.Cells[rowIndex, colIndex]; cell.Value = columnHead[i]; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                colIndex++;
            }
            rowIndex++;

            cell = sheet.Cells[rowIndex, startColIndex, rowIndex, startColIndex + columnHead.Count() - 1]; cell.Merge = true; cell.Value = sFirstLetters; cell.Style.Font.Bold = false;
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.WhiteSmoke);
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
            rowIndex++;

            #endregion

            #region Body
            foreach (var oItem in oSummarys)
            {
                colIndex = startColIndex;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (++nCount).ToString(); cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.BuyerName; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.TotalQty; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                rowIndex++;
            }

            while (nMaxRow > oSummarys.Count())
            {
                colIndex = startColIndex;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                rowIndex++;
                nMaxRow--;
            }
            #endregion
        }
        public void ExportExcelFEOSalesSummary(string sParam)
        {
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<string> columnHead = new List<string>();
            List<int> colWidth = new List<int>();


            RptFEOSalesSummary oRptFEOSS = new RptFEOSalesSummary();
            oRptFEOSS.Params = sParam;

            EnumOrderType orderType = (EnumOrderType)Convert.ToInt16(sParam.Split('~')[0]);
            string sOrderTypeName = orderType == (EnumOrderType.None) ? "" : orderType.ToString() + " Order";
            int nReportType = Convert.ToInt16(sParam.Split('~')[3]); //1.Buyer Wise, 2. Buyer & Month Wise, 3. With PI Value 4. Fabric Type Wise(Solid & Yarn Dyed)

            List<RptFEOSalesSummary> oRptFEOSalesSummarys = new List<RptFEOSalesSummary>();
            oRptFEOSalesSummarys = GenerateFEOSalesSummary(oRptFEOSS, false);

            if (oRptFEOSalesSummarys.Any() && (oRptFEOSalesSummarys.First().BuyerID >= 0 || oRptFEOSalesSummarys.First().MktPersonID >= 0))
            {
                //Get Distinct Month
                List<string> monthYear = oRptFEOSalesSummarys.OrderBy(x => x.OrderDate).Select(x => x.OrderDateStr).Distinct().ToList();
                string sMonthRange = monthYear.FirstOrDefault() + " - " + monthYear.LastOrDefault();

                if (nReportType == 1)
                {
                    #region
                    var buyerWiseSummary = oRptFEOSalesSummarys.GroupBy(x => x.BuyerName).Select(grp => new RptFEOSalesSummary
                    {
                        BuyerName = grp.Key,
                        SolidDyedQty = grp.Sum(x => x.SolidDyedQty),
                        YarnDyedQty = grp.Sum(x => x.YarnDyedQty)
                    }).OrderBy(x => x.BuyerName).ToList();


                    #region Character Wise Conversion
                    List<char> firstLetter = new List<char>();
                    firstLetter = buyerWiseSummary.Select(x => Char.ToUpper(Convert.ToChar(x.BuyerName[0]))).Distinct().ToList();


                    List<char> region1 = new List<char>();
                    List<char> region2 = new List<char>();
                    List<char> region3 = new List<char>();
                    List<char> region4 = new List<char>();

                    for (int i = 0; i < firstLetter.Count(); i++)
                    {
                        if (i < 5)
                        {
                            region1.Add(firstLetter[i]);
                        }
                        else if (i >= 5 && i < 10)
                        {
                            region2.Add(firstLetter[i]);
                        }
                        else if (i >= 10 && i < 15)
                        {
                            region3.Add(firstLetter[i]);
                        }
                        else
                        {
                            region4.Add(firstLetter[i]);
                        }
                    }
                    #endregion

                    int rowIndex = 2;
                    int nMaxColumn = 0;
                    int colIndex = 2;
                    ExcelRange cell;
                    Border border;

                    using (var excelPackage = new ExcelPackage())
                    {
                        excelPackage.Workbook.Properties.Author = "ESimSol";
                        excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                        var sheet = excelPackage.Workbook.Worksheets.Add("Sales & Marketing");
                        sheet.Name = "Sales & Marketing";

                        columnHead = new List<string>(new string[] { "#SL", "Buying House Name", "Order Qty(Y)" });
                        colWidth = new List<int>(new int[] { 8, 30, 15 });

                        #region Coloums

                        int nRegion = ((region1.Any()) ? 1 : 0) + ((region2.Any()) ? 1 : 0) + ((region3.Any()) ? 1 : 0) + ((region4.Any()) ? 1 : 0);

                        for (int j = 0; j < nRegion; j++) // Number of Region Character Wise
                        {
                            for (int i = 0; i < colWidth.Count(); i++)
                            {
                                sheet.Column(colIndex).Width = colWidth[i];
                                colIndex++;
                            }
                        }
                        nMaxColumn = colIndex - 1;

                        #endregion

                        #region Report Header
                        cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn]; cell.Merge = true; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex++;

                        cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn]; cell.Merge = true; cell.Style.Font.Bold = true; cell.Style.Font.Size = 15;
                        cell.Value = "Sales & Marketing"; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex++;

                        cell = sheet.Cells[rowIndex, 2, rowIndex, 3]; cell.Merge = true; cell.Style.Font.Bold = true; cell.Style.Font.Size = 12;
                        cell.Value = sOrderTypeName + "(" + sMonthRange + ") - Buyer Wise Fabric Order"; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        cell = sheet.Cells[rowIndex, nMaxColumn, rowIndex, nMaxColumn]; cell.Merge = true; cell.Style.Font.Bold = false;
                        cell.Value = DateTime.Now.ToString("dd MMM yyyy"); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex++;
                        #endregion

                        #region Body
                        int nMaxRowLen = 0;
                        int nCount = 0;
                        List<RptFEOSalesSummary> oSummarys1 = new List<RptFEOSalesSummary>();
                        List<RptFEOSalesSummary> oSummarys2 = new List<RptFEOSalesSummary>();
                        List<RptFEOSalesSummary> oSummarys3 = new List<RptFEOSalesSummary>();
                        List<RptFEOSalesSummary> oSummarys4 = new List<RptFEOSalesSummary>();
                        if (region1.Any())
                        {
                            oSummarys1 = buyerWiseSummary.Where(x => region1.Contains(Char.ToUpper(Convert.ToChar(x.BuyerName[0])))).ToList();
                            nMaxRowLen = oSummarys1.Count();
                        }
                        if (region2.Any())
                        {
                            oSummarys2 = buyerWiseSummary.Where(x => region2.Contains(Char.ToUpper(Convert.ToChar(x.BuyerName[0])))).ToList();
                            nMaxRowLen = (nMaxRowLen < oSummarys2.Count()) ? oSummarys2.Count() : nMaxRowLen;
                        }
                        if (region3.Any())
                        {
                            oSummarys3 = buyerWiseSummary.Where(x => region3.Contains(Char.ToUpper(Convert.ToChar(x.BuyerName[0])))).ToList();
                            nMaxRowLen = (nMaxRowLen < oSummarys3.Count()) ? oSummarys3.Count() : nMaxRowLen;
                        }
                        if (region4.Any())
                        {
                            oSummarys4 = buyerWiseSummary.Where(x => region4.Contains(Char.ToUpper(Convert.ToChar(x.BuyerName[0])))).ToList();
                            nMaxRowLen = (nMaxRowLen < oSummarys4.Count()) ? oSummarys4.Count() : nMaxRowLen;
                        }

                        if (region1.Any())
                            BuyerWiseFEOSales(ref nCount, nMaxRowLen, columnHead, string.Join(",", region1), oSummarys1, rowIndex, 2, ref cell, ref sheet);
                        if (region2.Any())
                            BuyerWiseFEOSales(ref nCount, nMaxRowLen, columnHead, string.Join(",", region2), oSummarys2, rowIndex, 5, ref cell, ref sheet);
                        if (region3.Any())
                            BuyerWiseFEOSales(ref nCount, nMaxRowLen, columnHead, string.Join(",", region3), oSummarys3, rowIndex, 8, ref cell, ref sheet);
                        if (region4.Any())
                            BuyerWiseFEOSales(ref nCount, nMaxRowLen, columnHead, string.Join(",", region4), oSummarys4, rowIndex, 11, ref cell, ref sheet);

                        #endregion

                        #region Bottom Summary
                        rowIndex = rowIndex + nMaxRowLen + 2;
                        cell = sheet.Cells[rowIndex, 2, rowIndex, 3]; cell.Merge = true; cell.Style.Font.Bold = true;
                        cell.Value = "Total"; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                        cell = sheet.Cells[rowIndex, 4]; cell.Value = buyerWiseSummary.Sum(x => x.TotalQty); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                        rowIndex++;

                        cell = sheet.Cells[rowIndex, 2, rowIndex, 3]; cell.Merge = true; cell.Style.Font.Bold = true;
                        cell.Value = "Avg/Month"; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                        cell = sheet.Cells[rowIndex, 4]; cell.Value = buyerWiseSummary.Sum(x => x.TotalQty) / monthYear.Count(); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                        rowIndex++;

                        cell = sheet.Cells[rowIndex, 2, rowIndex, 3]; cell.Merge = true; cell.Style.Font.Bold = true;
                        cell.Value = "Total Yarn Dyed"; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                        cell = sheet.Cells[rowIndex, 4]; cell.Value = oRptFEOSalesSummarys.Sum(x => x.YarnDyedQty); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                        rowIndex++;

                        cell = sheet.Cells[rowIndex, 2, rowIndex, 3]; cell.Merge = true; cell.Style.Font.Bold = true;
                        cell.Value = "Total Solid Dyed"; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                        cell = sheet.Cells[rowIndex, 4]; cell.Value = oRptFEOSalesSummarys.Sum(x => x.SolidDyedQty); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                        rowIndex++;
                        #endregion

                        Response.ClearContent();
                        Response.BinaryWrite(excelPackage.GetAsByteArray());
                        Response.AddHeader("content-disposition", "attachment; filename=BuyerWiseFabricsOrder.xlsx");
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Flush();
                        Response.End();
                    }
                    #endregion

                }
                else
                {
                    #region
                    columnHead = new List<string>(new string[] { "#SL", "Month" });
                    colWidth = new List<int>(new int[] { 8, 10 });

                    var mktPersonWiseSum = oRptFEOSalesSummarys.GroupBy(x => x.MktPersonName).Select(grp => new RptFEOSalesSummary
                    {
                        MktPersonName = grp.Key,
                        //Qty
                        YarnDyedQty = grp.Sum(x => x.YarnDyedQty),
                        SolidDyedQty = grp.Sum(x => x.SolidDyedQty),
                        AvgYDQtyPerMonth = grp.Sum(x => x.YarnDyedQty) / monthYear.Count(),
                        AvgSDQtyPerMonth = grp.Sum(x => x.SolidDyedQty) / monthYear.Count(),
                        YDRatioQty = grp.Sum(x => x.YarnDyedQty) * 100 / oRptFEOSalesSummarys.Sum(x => x.YarnDyedQty + x.SolidDyedQty),
                        SDRatioQty = grp.Sum(x => x.SolidDyedQty) * 100 / oRptFEOSalesSummarys.Sum(x => x.YarnDyedQty + x.SolidDyedQty),

                        // Value
                        YarnDyedValue = grp.Sum(x => x.YarnDyedValue),
                        SolidDyedValue = grp.Sum(x => x.SolidDyedValue),
                        AvgYDValuePerMonth = grp.Sum(x => x.YarnDyedValue) / monthYear.Count(),
                        AvgSDValuePerMonth = grp.Sum(x => x.SolidDyedValue) / monthYear.Count(),
                        YDRatioValue = grp.Sum(x => x.YarnDyedValue) * 100 / oRptFEOSalesSummarys.Sum(x => x.YarnDyedValue + x.SolidDyedValue),
                        SDRatioValue = grp.Sum(x => x.SolidDyedValue) * 100 / oRptFEOSalesSummarys.Sum(x => x.YarnDyedValue + x.SolidDyedValue),


                    }).OrderBy(x => x.MktPersonName).ToList();



                    if (nReportType == 4)//  Fabric Type Wise(Solid & Yarn Dyed)
                        mktPersonWiseSum.ForEach(x =>
                        {
                            colWidth.Add(14);
                            colWidth.Add(14);
                        });
                    else
                        mktPersonWiseSum.ForEach(x => colWidth.Add(20));

                    colWidth.Add(18);
                    columnHead.AddRange(mktPersonWiseSum.Select(x => x.MktPersonName));
                    columnHead.Add("Total Order Qty(Y)");



                    if (nReportType == 3)// With Value
                    {
                        if (nReportType == 4)//  Fabric Type Wise(Solid & Yarn Dyed)
                            mktPersonWiseSum.ForEach(x =>
                            {
                                colWidth.Add(14);
                                colWidth.Add(14);
                            });
                        else
                            mktPersonWiseSum.ForEach(x => colWidth.Add(20));

                        colWidth.Add(15);
                        columnHead.AddRange(mktPersonWiseSum.Select(x => x.MktPersonName));
                        columnHead.Add("Total Value");
                    }




                    int rowIndex = 2;
                    int nMaxColumn = 0;
                    int colIndex = 2;
                    ExcelRange cell;
                    Border border;
                    ExcelFill fill;


                    using (var excelPackage = new ExcelPackage())
                    {
                        excelPackage.Workbook.Properties.Author = "ESimSol";
                        excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                        var sheet = excelPackage.Workbook.Worksheets.Add("Marketing Person Wise");
                        sheet.Name = "Marketing Person Wise";

                        #region Coloums

                        for (int i = 0; i < colWidth.Count(); i++)
                        {
                            sheet.Column(colIndex).Width = colWidth[i];
                            colIndex++;
                        }
                        nMaxColumn = colIndex;

                        #endregion

                        #region Report Header
                        cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn]; cell.Merge = true; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex = rowIndex + 1;

                        cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn]; cell.Merge = true; cell.Style.Font.Bold = true; cell.Style.Font.Size = 15;
                        cell.Value = sOrderTypeName + "(" + sMonthRange + ") - Marketing Person Wise"; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex = rowIndex + 2;
                        #endregion

                        #region Column Header
                        colIndex = 2;

                        int nSpan = (nReportType == 4) ? 2 : 1;

                        #region Header Row 1
                        cell = sheet.Cells[rowIndex, colIndex, rowIndex + nSpan, colIndex++]; cell.Merge = true; cell.Value = columnHead[0]; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex + nSpan, colIndex++]; cell.Merge = true; cell.Value = columnHead[1]; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                        int preIndex = colIndex;
                        colIndex += mktPersonWiseSum.Select(x => x.MktPersonName).Count() * nSpan - 1;
                        cell = sheet.Cells[rowIndex, preIndex, rowIndex, colIndex++]; cell.Merge = true; cell.Value = sOrderTypeName + " (Qty. Yards) "; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex + nSpan, colIndex++]; cell.Merge = true; cell.Value = columnHead[2 + (mktPersonWiseSum.Count())]; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;


                        if (nReportType == 3)
                        {
                            preIndex = colIndex;
                            colIndex += mktPersonWiseSum.Select(x => x.MktPersonName).Count() * nSpan - 1;
                            cell = sheet.Cells[rowIndex, preIndex, rowIndex, colIndex++]; cell.Merge = true; cell.Value = sOrderTypeName + " (Value In USD) "; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                            cell = sheet.Cells[rowIndex, colIndex, rowIndex + nSpan, colIndex++]; cell.Merge = true; cell.Value = columnHead.LastOrDefault(); cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                        }
                        rowIndex++;
                        #endregion

                        #region Header Row 2
                        colIndex = 4;

                        foreach (var oItem in mktPersonWiseSum)
                        {
                            if (nReportType == 4)
                            {
                                preIndex = colIndex;
                                colIndex++;
                                cell = sheet.Cells[rowIndex, preIndex, rowIndex, colIndex++];
                                cell.Merge = true;

                            }
                            else { cell = sheet.Cells[rowIndex, colIndex++]; }

                            cell.Value = oItem.MktPersonName; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                        }

                        if (nReportType == 3)// With Value
                        {
                            colIndex++; // Escape Total Qty Column
                            foreach (var oItem in mktPersonWiseSum)
                            {
                                cell = sheet.Cells[rowIndex, colIndex++];
                                cell.Value = oItem.MktPersonName; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                            }
                        }

                        rowIndex++;
                        #endregion

                        #region Header Row 3
                        if (nReportType == 4)
                        {
                            colIndex = 4;
                            for (int i = 2; i < columnHead.Count() - 1; i++)
                            {
                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Yarn Dyed"; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Solid Dyed"; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                            }
                            rowIndex++;
                        }
                        #endregion

                        #endregion

                        #region Body
                        int nCount = 0;
                        foreach (string sValue in monthYear)
                        {

                            var monthWiseSummary = oRptFEOSalesSummarys.Where(x => x.OrderDateStr == sValue).GroupBy(x => x.MktPersonName).Select(grp => new RptFEOSalesSummary
                            {
                                MktPersonName = grp.Key,
                                YarnDyedQty = grp.Sum(o => o.YarnDyedQty),
                                SolidDyedQty = grp.Sum(o => o.SolidDyedQty),
                                YarnDyedValue = grp.Sum(o => o.YarnDyedValue),
                                SolidDyedValue = grp.Sum(o => o.SolidDyedValue)

                            }).OrderBy(x => x.MktPersonName);

                            colIndex = 2;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (++nCount).ToString(); cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = sValue; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                            foreach (var sMktPerson in mktPersonWiseSum.Select(x => x.MktPersonName).ToList())
                            {
                                var oresult = monthWiseSummary.Where(x => x.MktPersonName == sMktPerson).ToList();

                                if (nReportType == 4)
                                {
                                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (oresult != null && oresult.Any()) ? oresult.First().YarnDyedQty : 0; cell.Style.Font.Bold = false;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (oresult != null && oresult.Any()) ? oresult.First().SolidDyedQty : 0; cell.Style.Font.Bold = false;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                                }
                                else
                                {
                                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (oresult != null && oresult.Any()) ? oresult.First().TotalQty : 0; cell.Style.Font.Bold = false;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                                }

                            }
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = monthWiseSummary.Sum(x => x.TotalQty); cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                            if (nReportType == 3)
                            {
                                foreach (var sMktPerson in mktPersonWiseSum.Select(x => x.MktPersonName).ToList())
                                {
                                    var oresult = monthWiseSummary.Where(x => x.MktPersonName == sMktPerson).ToList();

                                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (oresult != null && oresult.Any()) ? oresult.First().TotalValue : 0; cell.Style.Font.Bold = false;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                                }
                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = monthWiseSummary.Sum(x => x.TotalValue); cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                            }

                            rowIndex++;
                        }
                        #endregion

                        #region Bottom Summary

                        #region Total
                        colIndex = 3;
                        cell = sheet.Cells[rowIndex, 2, rowIndex, colIndex++]; cell.Merge = true; cell.Style.Font.Bold = true;
                        cell.Value = "Total"; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                        foreach (var oItem in mktPersonWiseSum)
                        {
                            if (nReportType == 4)
                            {
                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.YarnDyedQty; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.SolidDyedQty; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                            }
                            else
                            {
                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.TotalQty; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                            }

                        }
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = mktPersonWiseSum.Sum(x => x.TotalQty); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;


                        if (nReportType == 3)
                        {
                            foreach (var oItem in mktPersonWiseSum)
                            {
                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.TotalValue; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                            }
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = mktPersonWiseSum.Sum(x => x.TotalValue); cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                        }
                        rowIndex++;
                        #endregion

                        #region Avg/Month
                        colIndex = 3;
                        cell = sheet.Cells[rowIndex, 2, rowIndex, colIndex++]; cell.Merge = true; cell.Style.Font.Bold = true;
                        cell.Value = "Avg/Month"; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                        foreach (var oItem in mktPersonWiseSum)
                        {
                            if (nReportType == 4)
                            {
                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.AvgYDQtyPerMonth; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.AvgSDQtyPerMonth; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                            }
                            else
                            {
                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.AvgQtyPerMonth; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                            }

                        }
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = mktPersonWiseSum.Sum(x => x.AvgQtyPerMonth); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                        if (nReportType == 3)
                        {
                            foreach (var oItem in mktPersonWiseSum)
                            {
                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.AvgValuePerMonth; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                            }
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = mktPersonWiseSum.Sum(x => x.AvgValuePerMonth); cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                        }
                        rowIndex++;
                        #endregion

                        #region Ratio
                        colIndex = 3;
                        cell = sheet.Cells[rowIndex, 2, rowIndex, colIndex++]; cell.Merge = true; cell.Style.Font.Bold = true;
                        cell.Value = "Ratio(%)"; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                        foreach (var oItem in mktPersonWiseSum)
                        {
                            if (nReportType == 4)
                            {
                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.YDRatioQty; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.SDRatioQty; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                            }
                            else
                            {
                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.RatioQty; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                            }
                        }
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = mktPersonWiseSum.Sum(x => x.RatioQty); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;


                        if (nReportType == 3)
                        {
                            foreach (var oItem in mktPersonWiseSum)
                            {
                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.RatioValue; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                            }
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = mktPersonWiseSum.Sum(x => x.RatioValue); cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                        }
                        rowIndex++;
                        #endregion



                        #endregion

                        Response.ClearContent();
                        Response.BinaryWrite(excelPackage.GetAsByteArray());
                        Response.AddHeader("content-disposition", "attachment; filename=FEOSalesOrder.xlsx");
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Flush();
                        Response.End();
                    }
                    #endregion
                }
            }
            else
            {
                ExcelRange cell;
                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Marketing Person Wise");
                    sheet.Name = "Marketing Person Wise";

                    cell = sheet.Cells[2, 2, 2, 5]; cell.Merge = true; cell.Value = "No Data found"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=FEOSalesOrder.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }

            }

        }
        public void ExportExcelFEOSalesStatement(string sParam)
        {
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<string> columnHead = new List<string>();
            List<int> colWidth = new List<int>();


            RptFEOSalesStatement oRptFEOSalesStatement = new RptFEOSalesStatement();
            oRptFEOSalesStatement.Params = sParam;


            List<RptFEOSalesStatement> oRptFEOSalesStatements = new List<RptFEOSalesStatement>();
            oRptFEOSalesStatements = GenerateFEOSalesStatement(oRptFEOSalesStatement);

            if (oRptFEOSalesStatements.Any() && oRptFEOSalesStatements.First().OrderDate != DateTime.MinValue)
            {
                //Get Distinct Month
                List<string> monthYear = oRptFEOSalesStatements.OrderBy(x => x.OrderDate).Select(x => x.OrderDateStr).Distinct().ToList();
                string sMonthRange = monthYear.FirstOrDefault() + " to " + monthYear.LastOrDefault();

                int rowIndex = 2;
                int nMaxColumn = 0;
                int colIndex = 2;
                ExcelRange cell;
                Border border;
                ExcelFill fill;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Fabric Sales Statement";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Fabric Sales Statement");
                    sheet.Name = "Fabric Sales Statement";

                    columnHead = new List<string>(new string[] { "#SL", "Month", "In House", "Out side", "Total(Yards)", "In House", "Out side", "Total(BDT)", "In House", "Out side", "Total(Yards)", "Total Production" });
                    colWidth = new List<int>(new int[] { 8, 10, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15 });

                    #region Coloums

                    for (int i = 0; i < colWidth.Count(); i++)
                    {
                        sheet.Column(colIndex).Width = colWidth[i];
                        colIndex++;
                    }
                    nMaxColumn = colIndex - 1;

                    #endregion

                    #region Report Header
                    cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn]; cell.Merge = true; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex++;

                    cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn]; cell.Merge = true; cell.Style.Font.Bold = true; cell.Style.Font.Size = 15;
                    cell.Value = "Fabric Sales Statement"; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex++;

                    cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn - 2]; cell.Merge = true; cell.Style.Font.Bold = true; cell.Style.Font.Size = 12;
                    cell.Value = "Fabric Sales Statement " + "(" + sMonthRange + ")"; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[rowIndex, nMaxColumn - 1, rowIndex, nMaxColumn]; cell.Merge = true; cell.Style.Font.Bold = false;
                    cell.Value = DateTime.Now.ToString("dd MMM yyyy"); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex++;
                    #endregion

                    #region Column Header

                    //Row 1

                    colIndex = 2;
                    cell = sheet.Cells[rowIndex, 2, rowIndex + 1, 2]; cell.Merge = true; cell.Value = columnHead[0]; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                    cell = sheet.Cells[rowIndex, 3, rowIndex + 1, 3]; cell.Merge = true; cell.Value = columnHead[1]; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                    cell = sheet.Cells[rowIndex, 4, rowIndex, 5]; cell.Merge = true; cell.Value = "Sales(Yard)"; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                    cell = sheet.Cells[rowIndex, 6, rowIndex + 1, 6]; cell.Merge = true; cell.Value = "Total(Yards)"; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                    cell = sheet.Cells[rowIndex, 7, rowIndex, 8]; cell.Merge = true; cell.Value = "Sales(BDT)"; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                    cell = sheet.Cells[rowIndex, 9, rowIndex + 1, 9]; cell.Merge = true; cell.Value = "Total (BDT)"; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                    cell = sheet.Cells[rowIndex, 10, rowIndex, 11]; cell.Merge = true; cell.Value = "Execution"; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                    cell = sheet.Cells[rowIndex, 12, rowIndex + 1, 12]; cell.Merge = true; cell.Value = columnHead[columnHead.Count() - 2]; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                    cell = sheet.Cells[rowIndex, 13, rowIndex + 1, 13]; cell.Merge = true; cell.Value = columnHead.Last(); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                    rowIndex++;

                    //Row 2
                    //Sales Yards
                    colIndex = 4;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = columnHead[2]; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = columnHead[3]; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                    //Sales BDT
                    colIndex = 7;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = columnHead[5]; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = columnHead[6]; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                    //Exe
                    colIndex = 10;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = columnHead[8]; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = columnHead[9]; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                    rowIndex++;

                    #endregion

                    #region Body

                    int nCount = 0;
                    foreach (RptFEOSalesStatement oItem in oRptFEOSalesStatements)
                    {
                        colIndex = 2;
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (++nCount).ToString(); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.OrderDateStr; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                        //Sales Qty
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.InHouseSalesQty; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.OutsideSalesQty; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.TotalSalesQty; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                        //Sales Value
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.InHouseSalesValue; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.OutsideSalesValue; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.TotalSalesValue; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                        //Exe Qty
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.InHouseExeQty; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.OutsideExeQty; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.TotalExeQty; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                        //Production
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.TotalProduction; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                        rowIndex++;
                    }

                    #endregion

                    #region Bottom Summary
                    colIndex = 3;
                    cell = sheet.Cells[rowIndex, 2, rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Total"; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                    //Sales Qty
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oRptFEOSalesStatements.Sum(x => x.InHouseSalesQty); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oRptFEOSalesStatements.Sum(x => x.OutsideSalesQty); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oRptFEOSalesStatements.Sum(x => x.TotalSalesQty); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                    //Sales Value
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oRptFEOSalesStatements.Sum(x => x.InHouseSalesValue); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oRptFEOSalesStatements.Sum(x => x.OutsideSalesValue); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oRptFEOSalesStatements.Sum(x => x.TotalSalesValue); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                    //Exe Qty
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oRptFEOSalesStatements.Sum(x => x.InHouseExeQty); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oRptFEOSalesStatements.Sum(x => x.OutsideExeQty); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oRptFEOSalesStatements.Sum(x => x.TotalExeQty); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                    //Production
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oRptFEOSalesStatements.Sum(x => x.TotalProduction); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                    rowIndex++;
                    #endregion

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=FabricSalesStatement.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
            }
            else
            {
                ExcelRange cell;
                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Fabric Sales Statement";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Fabric Sales Statement");
                    sheet.Name = "Fabric Sales Statement";

                    cell = sheet.Cells[2, 2, 2, 5]; cell.Merge = true; cell.Value = "No Data found"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=FabricSalesStatement.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }

            }

        }

    }
}