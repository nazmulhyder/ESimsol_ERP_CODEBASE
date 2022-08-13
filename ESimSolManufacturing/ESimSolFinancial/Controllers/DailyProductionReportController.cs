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
using System.Linq;
using ICS.Core.Utility;
using System.Dynamic;
using System.Data;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Drawing;

namespace ESimSolFinancial.Controllers
{
  
    public class DailyProductionReportController:Controller
    {
        #region Declaration
        PTUTransection _oPTUTransection = new PTUTransection();
        List<PTUTransection> _oPTUTransections = new List<PTUTransection>();
        #endregion

        #region Produciotn Plan
        public ActionResult ViewDailyProductionReport(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            GUPReportSetUp oGUPReportSetUp = new GUPReportSetUp();
            oGUPReportSetUp.GUPReportSetUps = GUPReportSetUp.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.BusinessUnits = BusinessUnit.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.ProductionUnits = ProductionUnit.Gets((int)Session[SessionInfo.currentUserID]);
            return View(oGUPReportSetUp);
        }

        [HttpPost]
        public JsonResult GetReports(PTUTransection oPTUTransection)
        {
            List<dynamic> oDynamics = new List<dynamic>();
            _oPTUTransection = new PTUTransection();
            
            try
            {
                DataSet oLotsDataSet = PTUTransection.GetDailyProductionReport(oPTUTransection.StartDate, oPTUTransection.EndDate, oPTUTransection.BUID, oPTUTransection.ProductionUnitID, (int)Session[SessionInfo.currentUserID]);
                DataTable oDataTable = oLotsDataSet.Tables[0];
                oDynamics = GetDynamicList(oDataTable);

            }
            catch (Exception ex)
            {
                dynamic obj = new ExpandoObject();
                var expobj = obj as IDictionary<string, object>;
                expobj.Add("Message", ex.Message);
                oDynamics.Add(expobj);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDynamics);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        private List<dynamic> GetDynamicList(DataTable oDataTable)
        {
            List<dynamic> oDynamiObjects = new List<dynamic>();
            //oLot.LotID = (oDataRow["LotID"] == DBNull.Value) ? 0 : Convert.ToInt32(oDataRow["LotID"]);
            int nTodaySwingValue = 0, nTodayQCValue = 0, SMV = 0, ActualWorkingHOur = 0, UseOperator = 0, TotalSAH = 0, TotalWorkingHour=0; 
            List<GUPReportSetUp> oGUPReportSetUps = GUPReportSetUp.Gets((int)Session[SessionInfo.currentUserID]);
            foreach (DataRow oDataRow in oDataTable.Rows)
            {
                dynamic obj = new ExpandoObject();
                var expobj = obj as IDictionary<string, object>;
                expobj.Add("PLineConfigureID", (oDataRow["PLineConfigureID"] == DBNull.Value) ? 0 : Convert.ToInt32(oDataRow["PLineConfigureID"]));
                expobj.Add("LineNo", (oDataRow["LineNo"] == DBNull.Value) ? "" : oDataRow["LineNo"]);
                expobj.Add("ProductionUnitID", (oDataRow["ProductionUnitID"] == DBNull.Value) ? 0 : Convert.ToInt32(oDataRow["ProductionUnitID"]));
                expobj.Add("ProductionUnitName", (oDataRow["ProductionUnitName"] == DBNull.Value) ? "" : oDataRow["ProductionUnitName"]);
                expobj.Add("OrderRecapID", (oDataRow["OrderRecapID"] == DBNull.Value) ? 0 : Convert.ToInt32(oDataRow["OrderRecapID"]));
                expobj.Add("BuyerName", (oDataRow["BuyerName"] == DBNull.Value) ? "" : oDataRow["BuyerName"]);
                expobj.Add("StyleNo", (oDataRow["StyleNo"] == DBNull.Value) ? "" : oDataRow["StyleNo"]);
                expobj.Add("RecapQty", (oDataRow["RecapQty"] == DBNull.Value) ? 0 : Convert.ToDouble(oDataRow["RecapQty"]));
                expobj.Add("ColorName", (oDataRow["ColorName"] == DBNull.Value) ? "" : oDataRow["ColorName"]);
                expobj.Add("GarmentsName", (oDataRow["GarmentsName"] == DBNull.Value) ? "" : oDataRow["GarmentsName"]);
                expobj.Add("ShipmentDate", (oDataRow["ShipmentDate"] == DBNull.Value) ? "" : Convert.ToDateTime(oDataRow["ShipmentDate"]).ToString("dd MMM yyyy"));
                expobj.Add("InputDate", (oDataRow["InputDate"] == DBNull.Value) ? "" : Convert.ToDateTime(oDataRow["InputDate"]).ToString("dd MMM yyyy"));
                expobj.Add("PlanQty", (oDataRow["PlanQty"] == DBNull.Value) ? 0 : Convert.ToInt32(oDataRow["PlanQty"]));
                expobj.Add("PlanWorkingHour", (oDataRow["PlanWorkingHour"] == DBNull.Value) ? 0 : Convert.ToInt32(oDataRow["PlanWorkingHour"]));
                expobj.Add("ActualWorkingHour", (oDataRow["ActualWorkingHour"] == DBNull.Value) ? 0 : Convert.ToInt32(oDataRow["ActualWorkingHour"])); ActualWorkingHOur = (oDataRow["ActualWorkingHour"] == DBNull.Value) ? 0 : Convert.ToInt32(oDataRow["ActualWorkingHour"]);
                expobj.Add("UseHelper", (oDataRow["UseHelper"] == DBNull.Value) ? 0 : Convert.ToInt32(oDataRow["UseHelper"]));
                expobj.Add("UseOperator", (oDataRow["UseOperator"] == DBNull.Value) ? 0 : Convert.ToInt32(oDataRow["UseOperator"]));UseOperator = (oDataRow["UseOperator"] == DBNull.Value) ? 0 : Convert.ToInt32(oDataRow["UseOperator"]);
                expobj.Add("SMV", (oDataRow["SMV"] == DBNull.Value) ? 0 : Convert.ToInt32(oDataRow["SMV"]));SMV = (oDataRow["SMV"] == DBNull.Value) ? 0 : Convert.ToInt32(oDataRow["SMV"]);
                expobj.Add("Remarks", (oDataRow["Remarks"] == DBNull.Value) ? "" : oDataRow["Remarks"]);
                foreach(GUPReportSetUp oReport in oGUPReportSetUps) 
                {
                    expobj.Add("PreCol" + oReport.ProductionStepID, (oDataRow["PreCol" + oReport.ProductionStepID] == DBNull.Value) ? 0 : Convert.ToDouble(oDataRow["PreCol" + oReport.ProductionStepID]));
                    expobj.Add("TodayCol" + oReport.ProductionStepID, (oDataRow["TodayCol" + oReport.ProductionStepID] == DBNull.Value) ? 0 : Convert.ToDouble(oDataRow["TodayCol" + oReport.ProductionStepID]));
                    nTodaySwingValue = (nTodaySwingValue == 0 && oReport.ProductionStepType == EnumProductionStepType.Sewing && oDataRow["TodayCol" + oReport.ProductionStepID] != DBNull.Value) ? Convert.ToInt32(oDataRow["TodayCol" + oReport.ProductionStepID]) : 0;
                    nTodayQCValue = (nTodayQCValue == 0 && oReport.ProductionStepType == EnumProductionStepType.QCPass && oDataRow["TodayCol" + oReport.ProductionStepID] != DBNull.Value) ? Convert.ToInt32(oDataRow["TodayCol" + oReport.ProductionStepID]) : 0;
                    expobj.Add("TotalCol" + oReport.ProductionStepID, (oDataRow["TotalCol" + oReport.ProductionStepID] == DBNull.Value) ? 0 : Convert.ToDouble(oDataRow["TotalCol" + oReport.ProductionStepID])); 
                }
                if(SMV>0 && nTodaySwingValue>0&& ActualWorkingHOur>0 && UseOperator>0)
                {
                    expobj.Add("DailyLineEfficiency", Global.MillionFormatActualDigit(((SMV * nTodaySwingValue)/60)/(ActualWorkingHOur*UseOperator))+"%");
                }else{
                    expobj.Add("DailyLineEfficiency", 0);
                }
                if (SMV > 0 && nTodayQCValue > 0)
                {
                    expobj.Add("TotalSAH", Global.MillionFormatActualDigit((SMV * nTodayQCValue) / 60) + "%");
                    TotalSAH = (SMV * nTodayQCValue) / 60;
                }
                else
                {
                    expobj.Add("TotalSAH", 0);
                }
                if (ActualWorkingHOur > 0 && UseOperator > 0)
                {
                    expobj.Add("TotalWorkingHour", Global.MillionFormatActualDigit(ActualWorkingHOur * UseOperator));
                    TotalWorkingHour = ActualWorkingHOur * UseOperator;
                }
                else
                {
                    expobj.Add("TotalWorkingHour", 0);
                }
                expobj.Add("DailyFloorEfficiency",(TotalSAH>0 && TotalWorkingHour>0?Global.MillionFormatActualDigit(TotalSAH/TotalWorkingHour)+"%":""));
                
                oDynamiObjects.Add(expobj);
            }
            return oDynamiObjects;
        }

        public void ExportToExcel(DateTime StartDate, DateTime EndDate, int BUID, int ProductionUnitID, double ts)
        {
            
            List<GUPReportSetUp> oGUPReportSetUps = new List<GUPReportSetUp>();
            oGUPReportSetUps = GUPReportSetUp.Gets((int)Session[SessionInfo.currentUserID]);
            DataSet oLotsDataSet = PTUTransection.GetDailyProductionReport(StartDate, EndDate, BUID, ProductionUnitID, (int)Session[SessionInfo.currentUserID]);
            DataTable oDataTable = oLotsDataSet.Tables[0];
           

            if (oDataTable.Rows.Count > 0)
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
             

                int rowIndex = 2, nColumnCount=0;
                ExcelRange cell;
                ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Daily Production Report");
                    sheet.Name = "Daily Production Report";

                    sheet.Column(2).Width = 8; //Floor
                    sheet.Column(3).Width = 15; //Line
                    sheet.Column(4).Width = 20; //Buyer
                    sheet.Column(5).Width = 20; //Style NO
                    sheet.Column(6).Width = 10; //Order Qty
                    sheet.Column(7).Width = 15; //Color
                    sheet.Column(8).Width = 15; //Item
                    sheet.Column(9).Width = 15; //Del.date
                    sheet.Column(10).Width = 15; //Input date
                    sheet.Column(11).Width = 12; //Plan target/hr
                    sheet.Column(12).Width = 10; //PLan W/Hr
                    sheet.Column(13).Width = 15; //Actual Working Hour
                    sheet.Column(14).Width = 15; //Day QC Pass
                    sheet.Column(15).Width = 15; //Sweing Ach%
                    nColumnCount = 16;
                    foreach (GUPReportSetUp oItem in oGUPReportSetUps)
                    {
                        sheet.Column(nColumnCount).Width = 15; nColumnCount++; //Today step qty
                        sheet.Column(nColumnCount).Width = 15; nColumnCount++; //Privoius step qty
                        sheet.Column(nColumnCount).Width = 15; nColumnCount++; //Total step qty
                    }
                    sheet.Column(nColumnCount).Width = 10; nColumnCount++; //User Operator
                    sheet.Column(nColumnCount).Width = 10; nColumnCount++; //User Helper
                    sheet.Column(nColumnCount).Width = 10; nColumnCount++; //SMV
                    sheet.Column(nColumnCount).Width = 15; nColumnCount++; //Day Line Eff%
                    sheet.Column(nColumnCount).Width = 15; nColumnCount++; //DAy Floor Eff%
                    sheet.Column(nColumnCount).Width = 15;  //Remarks

                    #region Report Header
                    sheet.Cells[rowIndex, 2, rowIndex, nColumnCount].Merge = true;
                    cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex = rowIndex + 1;

                    sheet.Cells[rowIndex, 2, rowIndex, nColumnCount].Merge = true;
                    cell = sheet.Cells[rowIndex, 2]; cell.Value = "Daily Productoin Report ("+StartDate.ToString("dd MMM yyyy")+" To "+EndDate.ToString("dd MMM yyyy"); cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex = rowIndex + 2;
                    #endregion

                    #region Column Header

                    cell = sheet.Cells[rowIndex, 2]; cell.Value = "Floor"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 3]; cell.Value = "Line No"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 4]; cell.Value = "Buyer"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 5]; cell.Value = "Style No/Name"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 6]; cell.Value = "Order Qty"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 7]; cell.Value = "Color"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 8]; cell.Value = "Item"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 9]; cell.Value = "Del.Date"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 10]; cell.Value = "Input Date"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 11]; cell.Value = "Plan Tgt(Hr)"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 12]; cell.Value = "Plan W.Hr"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 13]; cell.Value = "Actual W.Hr"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 14]; cell.Value = "Day QC Pass Tgt"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[rowIndex, 15]; cell.Value = "Sewing Ach.%"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nColumnCount = 16;
                    foreach (GUPReportSetUp oItem in oGUPReportSetUps)
                    {

                        cell = sheet.Cells[rowIndex, nColumnCount]; nColumnCount++; cell.Value = "Today (" + oItem.StepName + ")"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, nColumnCount];nColumnCount++; cell.Value = "Privious Total (" + oItem.StepName + ")"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, nColumnCount]; nColumnCount++; cell.Value = "Total (" + oItem.StepName + ")"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    }

                    cell = sheet.Cells[rowIndex, nColumnCount]; nColumnCount++; cell.Value = "Use Operator"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, nColumnCount]; nColumnCount++; cell.Value = "Use Helper"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, nColumnCount]; nColumnCount++; cell.Value = "SMV"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, nColumnCount]; nColumnCount++; cell.Value = "Day Line Eff%"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, nColumnCount]; nColumnCount++; cell.Value = "Day Floor Eff%"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, nColumnCount]; cell.Value = "Remarks"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    rowIndex = rowIndex + 1;
                    #endregion

                    #region Report Data
                    int nRowHeight = 50, PUID = 0, PLineConfigureID = 0, TotalDayTargetQC=0, TotalTodyDayQC =0;
                    foreach (DataRow oDataRow in oDataTable.Rows)
                    {
                        int nTempTodaySwingQty = 0, nTempTodayQCQty = 0, TempDayQCPass = 0, SMV = 0, ActualWorkingHOur = 0, UseOperator = 0, TotalSAH = 0, TotalWorkingHour = 0, PURowSpan = 0, LineRowSpan = 0;
                        string DailyLineEfficiency = "0", DailyFloorEfficiency = "0"; 
                        
                        if (PUID != Convert.ToInt32(oDataRow["ProductionUnitID"]))
                        {
                            #region Total Print
                            if (PUID!=0 && PUID != Convert.ToInt32(oDataRow["ProductionUnitID"]))
                            {
                                sheet.Row(rowIndex).Height = nRowHeight;
                                cell = sheet.Cells[rowIndex, 2, rowIndex, 11]; cell.Value = "Total"; cell.Style.Font.Bold = false;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true; cell.Merge = true;

                                sheet.Row(rowIndex).Height = nRowHeight;
                                cell = sheet.Cells[rowIndex, 12, rowIndex, 13]; cell.Value = TotalDayTargetQC > TotalTodyDayQC ? "Target Less" : " "; cell.Style.Font.Bold = false;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.WrapText = true; cell.Merge = true;

                                cell = sheet.Cells[rowIndex, 14]; cell.Value = TotalDayTargetQC; cell.Style.Font.Bold = false;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true;
                                rowIndex++;
                                TotalTodyDayQC =TotalDayTargetQC = 0;
                            }
                            #endregion
                            PUID = Convert.ToInt32(oDataRow["ProductionUnitID"]);
                            sheet.Row(rowIndex).Height = nRowHeight; PURowSpan = oDataTable.Rows.Cast<DataRow>().Where(x => x.Field<int>("ProductionUnitID") == PUID).Count();
                            cell = sheet.Cells[rowIndex, 2, (rowIndex + PURowSpan-1), 2]; cell.Value = (oDataRow["ProductionUnitName"] == DBNull.Value) ? "" : Convert.ToString(oDataRow["ProductionUnitName"]); cell.Style.Font.Bold = false;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.TextRotation = 90; cell.Style.WrapText = true; cell.Merge = true;
                            
                        }

                        if (PLineConfigureID != Convert.ToInt32(oDataRow["PLineConfigureID"]))
                        {
                            PLineConfigureID = Convert.ToInt32(oDataRow["PLineConfigureID"]);
                            sheet.Row(rowIndex).Height = nRowHeight; LineRowSpan = oDataTable.Rows.Cast<DataRow>().Where(x => x.Field<int>("PLineConfigureID") == PLineConfigureID).Count();
                            cell = sheet.Cells[rowIndex, 3, (rowIndex + LineRowSpan-1), 3]; cell.Value = (oDataRow["LineNo"] == DBNull.Value) ? "" : Convert.ToString(oDataRow["LineNo"]); cell.Style.Font.Bold = false;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.WrapText = true; cell.Merge = true;
                        }
                        sheet.Row(rowIndex).Height = nRowHeight;
                        cell = sheet.Cells[rowIndex, 4]; cell.Value = (oDataRow["BuyerName"] == DBNull.Value) ? "" : Convert.ToString(oDataRow["BuyerName"]); cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.WrapText = true;

                        sheet.Row(rowIndex).Height = nRowHeight;
                        cell = sheet.Cells[rowIndex, 5]; cell.Value = (oDataRow["StyleNo"] == DBNull.Value) ? "" : Convert.ToString(oDataRow["StyleNo"]); cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.WrapText = true;

                        sheet.Row(rowIndex).Height = nRowHeight;
                        cell = sheet.Cells[rowIndex, 6]; cell.Value = (oDataRow["RecapQty"] == DBNull.Value) ? 0 : Convert.ToDouble(oDataRow["RecapQty"]); cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.WrapText = true;

                        sheet.Row(rowIndex).Height = nRowHeight;
                        cell = sheet.Cells[rowIndex, 7]; cell.Value = (oDataRow["ColorName"] == DBNull.Value) ? "" : Convert.ToString(oDataRow["ColorName"]); cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.WrapText = true;

                        sheet.Row(rowIndex).Height = nRowHeight;
                        cell = sheet.Cells[rowIndex, 8]; cell.Value = (oDataRow["GarmentsName"] == DBNull.Value) ? "" : Convert.ToString(oDataRow["GarmentsName"]); cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.WrapText = true;

                        sheet.Row(rowIndex).Height = nRowHeight;
                        cell = sheet.Cells[rowIndex, 9]; cell.Value = (oDataRow["ShipmentDate"] == DBNull.Value) ? "-" : Convert.ToDateTime(oDataRow["ShipmentDate"]).ToString("dd MMM yyyy"); cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.WrapText = true;

                        sheet.Row(rowIndex).Height = nRowHeight;
                        cell = sheet.Cells[rowIndex, 10]; cell.Value = (oDataRow["InputDate"] == DBNull.Value) ? "" : Convert.ToDateTime(oDataRow["InputDate"]).ToString("dd MMM yyyy"); cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.WrapText = true;

                        sheet.Row(rowIndex).Height = nRowHeight;
                        cell = sheet.Cells[rowIndex, 11]; cell.Value = (oDataRow["PlanQty"] == DBNull.Value) ? 0 : Convert.ToDouble(oDataRow["PlanQty"]); cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.WrapText = true;

                        sheet.Row(rowIndex).Height = nRowHeight;
                        cell = sheet.Cells[rowIndex, 12]; cell.Value = (oDataRow["PlanWorkingHour"] == DBNull.Value) ? 0 : Convert.ToDouble(oDataRow["PlanWorkingHour"]); cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.WrapText = true;

                        ActualWorkingHOur = (oDataRow["ActualWorkingHour"] == DBNull.Value) ? 0 : Convert.ToInt32(oDataRow["ActualWorkingHour"]);
                        sheet.Row(rowIndex).Height = nRowHeight;
                        cell = sheet.Cells[rowIndex, 13]; cell.Value = ActualWorkingHOur; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.WrapText = true;

                        sheet.Row(rowIndex).Height = nRowHeight; TempDayQCPass = (oDataRow["DayQCPass"] == DBNull.Value) ? 0 : Convert.ToInt32(oDataRow["DayQCPass"]); TotalDayTargetQC += TempDayQCPass;
                        cell = sheet.Cells[rowIndex, 14]; cell.Value = TempDayQCPass; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.WrapText = true;


                        nColumnCount = 16;
                        foreach (GUPReportSetUp oReport in oGUPReportSetUps)
                        {

                            sheet.Row(rowIndex).Height = nRowHeight;
                            cell = sheet.Cells[rowIndex, nColumnCount]; nColumnCount++; cell.Value = (oDataRow["PreCol" + oReport.ProductionStepID] == DBNull.Value) ? 0 : Convert.ToDouble(oDataRow["PreCol" + oReport.ProductionStepID]); cell.Style.Font.Bold = false;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.WrapText = true;

                            sheet.Row(rowIndex).Height = nRowHeight;
                            cell = sheet.Cells[rowIndex, nColumnCount]; nColumnCount++; cell.Value = (oDataRow["TodayCol" + oReport.ProductionStepID] == DBNull.Value) ? 0 : Convert.ToDouble(oDataRow["TodayCol" + oReport.ProductionStepID]); cell.Style.Font.Bold = false;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.WrapText = true;
                            nTempTodaySwingQty = (nTempTodaySwingQty==0&&oReport.ProductionStepType == EnumProductionStepType.Sewing && oDataRow["TodayCol" + oReport.ProductionStepID] != DBNull.Value) ? Convert.ToInt32(oDataRow["TodayCol" + oReport.ProductionStepID]) : 0;

                            nTempTodayQCQty = (nTempTodayQCQty == 0 && oReport.ProductionStepType == EnumProductionStepType.QCPass && oDataRow["TodayCol" + oReport.ProductionStepID] != DBNull.Value) ? Convert.ToInt32(oDataRow["TodayCol" + oReport.ProductionStepID]) : 0;
                            TotalTodyDayQC += nTempTodayQCQty;
                            sheet.Row(rowIndex).Height = nRowHeight;
                            cell = sheet.Cells[rowIndex, nColumnCount]; nColumnCount++; cell.Value = (oDataRow["TotalCol" + oReport.ProductionStepID] == DBNull.Value) ? 0 : Convert.ToDouble(oDataRow["TotalCol" + oReport.ProductionStepID]); cell.Style.Font.Bold = false;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.WrapText = true;
                            
                        }

                        sheet.Row(rowIndex).Height = nRowHeight;//don't change/remove it
                        cell = sheet.Cells[rowIndex, 15]; cell.Value = (nTempTodaySwingQty > 0 && TempDayQCPass > 0) ? (100 * nTempTodaySwingQty) / TempDayQCPass : 0; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.WrapText = true;

                        UseOperator = (oDataRow["UseOperator"] == DBNull.Value) ? 0 : Convert.ToInt32(oDataRow["UseOperator"]);
                        sheet.Row(rowIndex).Height = nRowHeight;
                        cell = sheet.Cells[rowIndex, nColumnCount]; nColumnCount++; cell.Value = UseOperator; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.WrapText = true;

                        
                        sheet.Row(rowIndex).Height = nRowHeight;
                        cell = sheet.Cells[rowIndex, nColumnCount]; nColumnCount++; cell.Value = (oDataRow["UseHelper"] == DBNull.Value) ? 0 : Convert.ToDouble(oDataRow["UseHelper"]); cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.WrapText = true;

                        SMV = (oDataRow["SMV"] == DBNull.Value) ? 0 : Convert.ToInt32(oDataRow["SMV"]);
                        sheet.Row(rowIndex).Height = nRowHeight;
                        cell = sheet.Cells[rowIndex, nColumnCount]; nColumnCount++; cell.Value = SMV; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.WrapText = true;

                        if (SMV > 0 && nTempTodaySwingQty > 0 && ActualWorkingHOur > 0 && UseOperator > 0)
                        {
                            DailyLineEfficiency =  Global.MillionFormatActualDigit(((SMV * nTempTodaySwingQty) / 60) / (ActualWorkingHOur * UseOperator)) + "%";
                        }
                       
                        if (SMV > 0 && nTempTodayQCQty > 0)
                        {
                            TotalSAH = (SMV * nTempTodayQCQty) / 60;
                        }
                       
                        if (ActualWorkingHOur > 0 && UseOperator > 0)
                        {
                            
                            TotalWorkingHour = ActualWorkingHOur * UseOperator;
                        }

                        DailyFloorEfficiency = (TotalSAH > 0 && TotalWorkingHour > 0 ? Global.MillionFormatActualDigit(TotalSAH / TotalWorkingHour) + "%" : "");

                        sheet.Row(rowIndex).Height = nRowHeight;
                        cell = sheet.Cells[rowIndex, nColumnCount]; nColumnCount++; cell.Value = DailyLineEfficiency; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.WrapText = true;

                        sheet.Row(rowIndex).Height = nRowHeight;
                        cell = sheet.Cells[rowIndex, nColumnCount]; nColumnCount++; cell.Value = DailyFloorEfficiency; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.WrapText = true;

                        sheet.Row(rowIndex).Height = nRowHeight;
                        cell = sheet.Cells[rowIndex, nColumnCount]; nColumnCount++; cell.Value = (oDataRow["Remarks"] == DBNull.Value) ? "" : Convert.ToString(oDataRow["Remarks"]); cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.WrapText = true;
                        rowIndex++;
                        
                    }

                    #region Total Print
                        sheet.Row(rowIndex).Height = nRowHeight;
                        cell = sheet.Cells[rowIndex, 2, rowIndex, 11]; cell.Value = "Total"; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.WrapText = true; cell.Merge = true;

                        sheet.Row(rowIndex).Height = nRowHeight;
                        cell = sheet.Cells[rowIndex, 12, rowIndex, 13]; cell.Value = TotalDayTargetQC > TotalTodyDayQC ? "Target Less" : " "; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.WrapText = true; cell.Merge = true;

                        cell = sheet.Cells[rowIndex, 14]; cell.Value = TotalDayTargetQC; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.WrapText = true;
                        
                    #endregion
                    #endregion

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=Order Recap List.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }



            }

        }

        public Image GetCompanyLogo(Company oCompany)
        {
            if (oCompany.OrganizationLogo != null)
            {
                MemoryStream m = new MemoryStream(oCompany.OrganizationLogo);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                //img.Save(Response.OutputStream, ImageFormat.Jpeg);
                img.Save(Server.MapPath("~/Content/") + "companyLogo.jpg", ImageFormat.Jpeg);
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