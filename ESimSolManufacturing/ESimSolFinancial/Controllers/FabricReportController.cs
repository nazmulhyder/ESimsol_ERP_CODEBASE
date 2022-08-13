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



    public class FabricReportController : Controller
    {
        #region Declare
        List<FabricExecutionOrder> _oFEOs = new List<FabricExecutionOrder>();
        #endregion

        #region FDO List Report
        public ActionResult View_FDOListReports(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<FDOListReport> oFDOListReports = new List<FDOListReport>();
            //oFDOListReports = FDOListReport.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            //ViewBag.DOTypes = DOTypeObj.Gets().Where(o => o.id != (int)EnumDOType.YarnTransfer_DO && o.id != (int)EnumDOType.YarnTransferForDEO);
            //ViewBag.DOStatus = DOStatusObj.Gets();
           string sSQL = "SELECT * FROM View_Employee WHERE EmployeeDesignationType = " + (int)EnumEmployeeDesignationType.MarketPerson;
            ViewBag.MktPersons = Employee.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(oFDOListReports);
        }
        [HttpPost]
        public ActionResult PrintFDOLR(FormCollection DataCollection)
        {
            string sFEOIDs = DataCollection["txtFDOLR"];
            List<FDOListReport> oFDOListReports = new List<FDOListReport>();
            if (!string.IsNullOrEmpty(sFEOIDs))
            {
                oFDOListReports = FDOListReport.GetsBysp(sFEOIDs, ((User)Session[SessionInfo.CurrentUser]).UserID);
                //string sSQL = "SELECT * FROM View_RPT_FDOListReport WHERE FEOID IN (" + sFEOIDs + ")";
                //oFDOListReports = FDOListReport.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            string Messge = "FDO List Report";
            rptFDOListReport oReport = new rptFDOListReport();
            byte[] abytes = oReport.PrepareReport(oFDOListReports, oCompany, Messge);
            return File(abytes, "application/pdf");
        }
        //public void ExcelFDOLR(FormCollection fc)
        //{
        //    string sParams = fc["Params"];
        //    List<FDOListReport> oFDOListReports = new List<FDOListReport>();
        //    if (!string.IsNullOrEmpty(sParams))
        //    {
        //        //string sSQL = "SELECT * FROM View_RPT_FDOListReport WHERE FEOID IN (" + sParams + ") ";
        //        //oFDOListReports = FDOListReport.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //        oFDOListReports = FDOListReport.GetsBysp(sParams, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    }

        //    Company oCompany = new Company();
        //    oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    oCompany.CompanyLogo = GetCompanyLogo(oCompany);

        //    string sTitle = "FDO List Report";
        //    rptFDOListReport oReport = new rptFDOListReport();
        //    PdfPTable oPdfPTable = new PdfPTable(1);
        //    oPdfPTable = oReport.PrepareExcel(oFDOListReports, oCompany, sTitle);

        //    #region Response Part
        //    Response.ClearContent();
        //    Response.BinaryWrite(GlobalExcel.ConvertToExcel(oPdfPTable, oCompany, sTitle));
        //    Response.AddHeader("content-disposition", "attachment; filename=" + sTitle + ".xlsx");
        //    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //    Response.Flush();
        //    Response.End();
        //    #endregion
        //}

        #region Searching
        [HttpPost]
        public JsonResult GetsAdvSearchFDOLR(FDOListReport oFDOLR)
        {
            List<FDOListReport> oFDOLRs = new List<FDOListReport>();
            try
            {
              
               // string sSQL = this.MakeSQLAdvSearchFDOLR(oFDOLR);
                if (!string.IsNullOrEmpty(oFDOLR.Params))
                {
                    //oFDOLRs = FDOListReport.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oFDOLRs = FDOListReport.GetsBysp(oFDOLR.Params, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oFDOLR = new FDOListReport();
                    oFDOLR.ErrorMessage = "Give a Searching Criteria.";
                    oFDOLRs.Add(oFDOLR);
                }
            }
            catch (Exception ex)
            {
                oFDOLR = new FDOListReport();
                oFDOLR.ErrorMessage = ex.Message;
                oFDOLRs.Add(oFDOLR);
            }
            var jsonResult = Json(oFDOLRs, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        private string MakeSQLAdvSearchFDOLR(FDOListReport oFDOLR)
        {
            string sSQL = "";

            if (!string.IsNullOrEmpty(oFDOLR.Params))
            {
                string sReturn1 = "SELECT * FROM View_RPT_FDOListReport ";
                string sReturn = "";
                int nDOType = Convert.ToInt32(oFDOLR.Params.Split('~')[0]);
                string sLCNo = Convert.ToString(oFDOLR.Params.Split('~')[1]);
                string sPINo = Convert.ToString(oFDOLR.Params.Split('~')[2]);
                string sFEONo = Convert.ToString(oFDOLR.Params.Split('~')[3]);
                string sBuyerName = Convert.ToString(oFDOLR.Params.Split('~')[4]);
                string sStatus = Convert.ToString(oFDOLR.Params.Split('~')[5]);

                bool bIsDelStartDate = Convert.ToBoolean(oFDOLR.Params.Split('~')[6]);
                DateTime dFromDelStartDate = Convert.ToDateTime(oFDOLR.Params.Split('~')[7]);
                DateTime dToDelStartDate = Convert.ToDateTime(oFDOLR.Params.Split('~')[8]);

                bool bIsDelEndDate = Convert.ToBoolean(oFDOLR.Params.Split('~')[9]);
                DateTime dFromDelEndDate = Convert.ToDateTime(oFDOLR.Params.Split('~')[10]);
                DateTime dToDelEndDate = Convert.ToDateTime(oFDOLR.Params.Split('~')[11]);

                bool bIsPendingDelivery = Convert.ToBoolean(oFDOLR.Params.Split('~')[12]);
                DateTime dFromPendingDeliveryDate = Convert.ToDateTime(oFDOLR.Params.Split('~')[13]);
                DateTime dToPendingDeliveryDate = Convert.ToDateTime(oFDOLR.Params.Split('~')[14]);

                bool bIsDeliveryComplete = Convert.ToBoolean(oFDOLR.Params.Split('~')[15]);
                DateTime dFromDeliveryCompleteDate = Convert.ToDateTime(oFDOLR.Params.Split('~')[16]);
                DateTime dToDeliveryCompleteDate = Convert.ToDateTime(oFDOLR.Params.Split('~')[17]);

                bool bIsDelivery = Convert.ToBoolean(oFDOLR.Params.Split('~')[18]);
                DateTime dFromDeliveryDate = Convert.ToDateTime(oFDOLR.Params.Split('~')[19]);
                DateTime dToDeliveryDate = Convert.ToDateTime(oFDOLR.Params.Split('~')[20]);

                bool bIsDODate = Convert.ToBoolean(oFDOLR.Params.Split('~')[21]);
                DateTime dFromDODate = Convert.ToDateTime(oFDOLR.Params.Split('~')[22]);
                DateTime dToDODate = Convert.ToDateTime(oFDOLR.Params.Split('~')[23]);

                bool bIsChallanDate = Convert.ToBoolean(oFDOLR.Params.Split('~')[24]);
                DateTime dFromChallanDate = Convert.ToDateTime(oFDOLR.Params.Split('~')[25]);
                DateTime dToChallanDate = Convert.ToDateTime(oFDOLR.Params.Split('~')[26]);

                bool bIsUndelivered = Convert.ToBoolean(oFDOLR.Params.Split('~')[27]);

                bool bIsDelCompleteDate = Convert.ToBoolean(oFDOLR.Params.Split('~')[28]);
                DateTime dFromDelCompleteDate = Convert.ToDateTime(oFDOLR.Params.Split('~')[29]);
                DateTime dToDelCompleteDate = Convert.ToDateTime(oFDOLR.Params.Split('~')[30]);


                #region Only FabricDeliveryOrder's FEO
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "  FEOID IN (SELECT FDOD.FEOID FROM FabricDeliveryOrderDetail AS FDOD) ";
                #endregion

                #region DO Type
                if (nDOType > (int)EnumDOType.None)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " FDOType = " + nDOType;
                }
                #endregion

                #region LC No
                if (!string.IsNullOrEmpty(sLCNo))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " LCNo LIKE '%" + sLCNo + "%' ";
                }
                #endregion

                #region PI No
                if (!string.IsNullOrEmpty(sPINo))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " PINo LIKE '%" + sPINo + "%' ";
                }
                #endregion

                #region FEO No
                if (!string.IsNullOrEmpty(sFEONo))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " FEONo LIKE '%" + sFEONo + "%' ";
                }
                #endregion

                #region Buyer Name
                if (!string.IsNullOrEmpty(sBuyerName))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " BuyerName LIKE '%" + sBuyerName + "%' ";
                }
                #endregion

                #region Is Del End Date
                if (bIsDelEndDate)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DelEndDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDelEndDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToDelEndDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                #endregion

                #region DO Date
                if (bIsDODate)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),FDODate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDODate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToDODate.ToString("dd MMM yyyy") + "',106)) ";
                }
                #endregion

                #region Yet not create challan (Undelivered)
                if (bIsUndelivered)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CountChallan = 0 ";
                }
                #endregion

                #region Del Complete Date
                if (bIsDelCompleteDate)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " FDODQty>0 AND FDOCQty>0 AND FDODQty=FDOCQty AND CONVERT(DATE,CONVERT(VARCHAR(12),DelCompleteDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDelCompleteDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToDelCompleteDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                #endregion
                sSQL = sReturn1 + " " + sReturn + " ORDER BY FEONo";
            }
            return sSQL;
        }
        #endregion
        #endregion

        #region Weaving Production Order Status
        public ActionResult View_WeavingProductionOrderStatus(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<FDOListReport> oFDOListReports = new List<FDOListReport>();
            return View(oFDOListReports);
        }

        #region Customer wise bulk order in hand status (Solid & Yarn Dyed Fabrics)
        private List<FabricExecutionOrder> GetFEOListForCWBO(string sParams)
        {
            List<FabricExecutionOrder> oFEOs = new List<FabricExecutionOrder>();

            DateTime dFromDate = Convert.ToDateTime(sParams.Split('~')[0]);
            DateTime dToDate = Convert.ToDateTime(sParams.Split('~')[1]);
            string sBuyerIDs = sParams.Split('~')[2];

            string sReturn1 = "SELECT * FROM View_FabricExecutionOrder ";
            string sReturn = "";

            #region Order Type
            Global.TagSQL(ref sReturn);
            //sReturn = sReturn + " OrderType = " + (int)EnumOrderType.Bulk;
            #endregion

            #region Order Date
            Global.TagSQL(ref sReturn);
            sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),OrderDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToDate.ToString("dd MMM yyyy") + "',106)) ";
            #endregion

            #region Buyer IDs
            if (!string.IsNullOrEmpty(sBuyerIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BuyerID IN (" + sBuyerIDs + ") ";
            }
            #endregion

            string sSQL = sReturn1 + " " + sReturn + " ORDER BY FEONo";
            oFEOs = FabricExecutionOrder.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            return oFEOs;
        }
        public ActionResult PrintCWBO(string sParams, double nts)
        {
            List<FabricExecutionOrder> oFEOs = new List<FabricExecutionOrder>();
            oFEOs = this.GetFEOListForCWBO(sParams);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            string sMessage = "CUSTOMER WISE BULK ORDER IN HAND STATUS (SOLID & YARN DYED FABRICS)";

            rptCustomerWiseBulkOrderReport oReport = new rptCustomerWiseBulkOrderReport();
            byte[] abytes = oReport.PrepareReport(oFEOs, oCompany, sMessage);
            return File(abytes, "application/pdf");
        }

        #region Testing
        public void ExcelCWBO1(string sParams)
        {
            List<FabricExecutionOrder> oFEOs = new List<FabricExecutionOrder>();
            oFEOs = this.GetFEOListForCWBO(sParams);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            string sMessage = "CUSTOMER WISE BULK ORDER IN HAND STATUS (SOLID & YARN DYED FABRICS)";

            rptCustomerWiseBulkOrderReport oReport = new rptCustomerWiseBulkOrderReport();
            PdfPTable oPdfPTable = new PdfPTable(5);
            oPdfPTable = oReport.PrepareExcel(oFEOs, oCompany, sMessage);

            this.ConvertToExcel(oPdfPTable);
        }
        #endregion
        public void ExcelCWBO(string sParams)
        {
            _oFEOs = new List<FabricExecutionOrder>();
            _oFEOs = this.GetFEOListForCWBO(sParams);


            int nMaxColumn = 0;
            int colIndex = 2;
            int rowIndex = 2;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("CUSTOMER WISE BULK ORDER IN HAND STATUS (SOLID & YARN DYED FABRICS)");
                sheet.Name = "CUSTOMER WISE BULK ORDER IN HAND STATUS (SOLID & YARN DYED FABRICS)";

                sheet.Column(2).Width = 8; //SL
                sheet.Column(3).Width = 40; //CUSTOMER
                sheet.Column(4).Width = 18; //YARN DYED ORDER
                sheet.Column(5).Width = 18; //SOLID DYED ORDER
                sheet.Column(6).Width = 16; //TOTAL QTY

                nMaxColumn = 6;

                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);

                #region Report Header
                sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = "CUSTOMER WISE BULK ORDER IN HAND STATUS (SOLID & YARN DYED FABRICS)"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 2;
                #endregion

                #region Table Header 01
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++, rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Unit in Mtr"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Date : " + DateTime.Now.ToString("dd MMM yyyy"); cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                rowIndex++;
                #endregion

                #region Table Header 02
                colIndex = 2;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "SL"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "CUSTOMER"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "YARN DYED ORDER"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "SOLID DYED ORDER"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "TOTAL QTY"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                rowIndex++;
                #endregion

                #region Table Body
                string sBuyerName = "";
                double nYarnDyedOrder = 0,
                       nSolidDyedOrder = 0,
                       nTotalQty = 0,
                       nTotalYarnDyedOrder = 0,
                       nTotalSolidDyedOrder = 0,
                       nGrandTotalQty = 0;

                string[] sBuyerIDs = string.Join(",", _oFEOs.Select(o => o.BuyerID).Distinct()).Split(',');
                List<FabricExecutionOrder> oFEOs = new List<FabricExecutionOrder>();

                int nSL = 0;
                foreach (string sBuyerID in sBuyerIDs)
                {
                    #region Calculations
                    int nBuyerID = Convert.ToInt32(sBuyerID);
                    oFEOs = new List<FabricExecutionOrder>();
                    oFEOs = _oFEOs.Where(o => o.BuyerID == nBuyerID).ToList();
                    sBuyerName = oFEOs[0].BuyerName;
                    nYarnDyedOrder = oFEOs.Where(o => o.ProcessType == (int)EnumFabricProcess.Process && o.IsYarnDyed == true).Select(o => Global.GetMeter(o.Qty, 2)).Sum();
                    nSolidDyedOrder = oFEOs.Where(o => o.ProcessType == 2 && o.IsYarnDyed == false).Select(o => Global.GetMeter(o.Qty, 2)).Sum(); //2 = solid dyed
                    nTotalQty = nYarnDyedOrder + nSolidDyedOrder;

                    nTotalYarnDyedOrder += nYarnDyedOrder;
                    nTotalSolidDyedOrder += nSolidDyedOrder;
                    nGrandTotalQty += nTotalQty;
                    #endregion

                    nSL++;
                    colIndex = 2;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nSL; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = sBuyerName; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nYarnDyedOrder; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nSolidDyedOrder; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nTotalQty; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    rowIndex++;
                }
                #endregion

                #region Total
                colIndex = 2;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Green);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Grand Total : "; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Green);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nTotalYarnDyedOrder; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Green);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nTotalSolidDyedOrder; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Green);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nGrandTotalQty; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Green);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                #endregion


                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=CUSTOMER WISE BULK ORDER IN HAND.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }
        
        
        #endregion

        #region BUYER & ORDER NO WISE  ATMLS BULK FABRICS PRODUCTION & DELIVERY UPDATE STATUS
        private List<FabricExecutionOrder> GetFEOListForBOWA(string sParams)
        {
            List<FabricExecutionOrder> oFEOs = new List<FabricExecutionOrder>();

            int nOrderType = Convert.ToInt32(sParams.Split('~')[0]);
            DateTime dFromDate = Convert.ToDateTime(sParams.Split('~')[1]);
            DateTime dToDate = Convert.ToDateTime(sParams.Split('~')[2]);
            string sBuyerIDs = sParams.Split('~')[3];

            string sReturn1 = "SELECT * FROM View_FabricExecutionOrder ";
            string sReturn = "";

            #region Process Type
            Global.TagSQL(ref sReturn);
            if (nOrderType == 1)
            {
                sReturn = sReturn + " ProcessType = " + (int)EnumFabricProcess.Process + " AND IsYarnDyed = 1";
            }
            else
            {
                sReturn = sReturn + " ProcessType = 2 AND IsYarnDyed = 0"; //2 = solid dyed
            }
            #endregion

            #region Order Type
            Global.TagSQL(ref sReturn);
            //sReturn = sReturn + " OrderType = " + (int)EnumOrderType.Bulk;
            #endregion

            #region Order Date
            Global.TagSQL(ref sReturn);
            sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),OrderDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToDate.ToString("dd MMM yyyy") + "',106)) ";
            #endregion

            #region Buyer IDs
            if (!string.IsNullOrEmpty(sBuyerIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BuyerID IN (" + sBuyerIDs + ") ";
            }
            #endregion

            string sSQL = sReturn1 + " " + sReturn + " ORDER BY FEONo";
            oFEOs = FabricExecutionOrder.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            return oFEOs;
        }
        //public ActionResult PrintBOWA(string sParams, double nts)
        //{
        //    List<FabricExecutionOrder> oFEOs = new List<FabricExecutionOrder>();
        //    oFEOs = this.GetFEOListForBOWA(sParams);

        //    Company oCompany = new Company();
        //    oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    oCompany.CompanyLogo = GetCompanyLogo(oCompany);

        //    int nOrderType = Convert.ToInt32(sParams.Split('~')[0]);
        //    string sMessage = "BUYER & ORDER NO WISE BULK FABRICS PRODUCTION & DELIVERY UPDATE STATUS (ONLY " + (nOrderType == 1 ? "YARN" : "SOLID") + " DYED ORDER)";

        //    rptProductionAndDeliveryUpdateStatus oReport = new rptProductionAndDeliveryUpdateStatus();
        //    byte[] abytes = oReport.PrepareReport(oFEOs, oCompany, sMessage);
        //    return File(abytes, "application/pdf");
        //}
        //public void ExcelBOWA(string sParams)
        //{
        //    _oFEOs = new List<FabricExecutionOrder>();
        //    _oFEOs = this.GetFEOListForBOWA(sParams);

        //    int nMaxColumn = 0;
        //    int colIndex = 2;
        //    int rowIndex = 2;
        //    ExcelRange cell;
        //    ExcelFill fill;
        //    OfficeOpenXml.Style.Border border;

        //    using (var excelPackage = new ExcelPackage())
        //    {
        //        int nOrderType = Convert.ToInt32(sParams.Split('~')[0]);
        //        excelPackage.Workbook.Properties.Author = "ESimSol";
        //        excelPackage.Workbook.Properties.Title = "Export from ESimSol";
        //        var sheet = excelPackage.Workbook.Worksheets.Add("BUYER & ORDER NO WISE  ATMLS BULK FABRICS PRODUCTION & DELIVERY UPDATE STATUS (ONLY " + (nOrderType == 1 ? "YARN" : "SOLID") + " DYED ORDER)");
        //        sheet.Name = "BUYER & ORDER NO WISE  ATMLS BULK FABRICS PRODUCTION & DELIVERY UPDATE STATUS (ONLY " + (nOrderType == 1 ? "YARN" : "SOLID") + " DYED ORDER)";

        //        sheet.Column(2).Width = 8; //SL
        //        sheet.Column(3).Width = 16; //CONSTRUCTION
        //        sheet.Column(4).Width = 16; //FABRIC TYPE
        //        sheet.Column(5).Width = 20; //BUYER
        //        sheet.Column(6).Width = 15; //ORDER NO

        //        sheet.Column(7).Width = 15; //ORDER QTY(Y)
        //        sheet.Column(8).Width = 15; //ORDER QTY(M)
        //        sheet.Column(9).Width = 15; //GREY TARGET
        //        sheet.Column(10).Width = 15; //GREY DONE
        //        sheet.Column(11).Width = 15; //GREY BALANCE
        //        sheet.Column(12).Width = 15; //Grey Issued to Process
        //        sheet.Column(13).Width = 15; //Grey Issue balance
        //        sheet.Column(14).Width = 15; //ORDER RECEIVED DT
        //        sheet.Column(15).Width = 15; //BUYER ASKING DT
        //        sheet.Column(16).Width = 15; //ATMLS GREY DELIVERY DT(lead Time wise)
        //        sheet.Column(17).Width = 15; //Grey Issue start & Complete dt
        //        sheet.Column(18).Width = 15; //LOOM RUNNING STATUS

        //        nMaxColumn = 18;

        //        Company oCompany = new Company();
        //        oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //        oCompany.CompanyLogo = GetCompanyLogo(oCompany);

        //        #region Report Header
        //        sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
        //        cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
        //        cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        rowIndex = rowIndex + 1;

        //        sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
        //        cell = sheet.Cells[rowIndex, 2]; cell.Value = "BUYER & ORDER NO WISE  ATMLS BULK FABRICS PRODUCTION & DELIVERY UPDATE STATUS (ONLY " + (nOrderType == 1 ? "YARN" : "SOLID") + " DYED ORDER)"; cell.Style.Font.Bold = true;
        //        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        rowIndex = rowIndex + 2;
        //        #endregion

        //        #region Table Header
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "SL"; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "CONSTRUCTION"; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "FABRIC TYPE"; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "BUYER"; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "ORDER NO"; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "ORDER QTY(Y)"; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "ORDER QTY(M)"; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "GREY TARGET"; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "GREY DONE"; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "GREY BALANCE"; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Grey Issued to Process"; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Grey Issue balance"; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "ORDER RECEIVED DT"; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "BUYER ASKING DT"; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "ATMLS GREY DELIVERY DT(lead Time wise)"; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Grey Issue start & Complete dt"; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "LOOM RUNNING STATUS"; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        rowIndex++;
        //        #endregion

        //        #region Table Body

        //        int nSL = 0;

        //        double nGrandTotalOrderQtyInYard = 0,
        //        nGrandTotalOrderQtyInMeter = 0,
        //        nGrandTotalGrayTarget = 0,
        //        nGrandTotalGrayDone = 0,
        //        nGrandTotalGrayBalance = 0,
        //        nGrandTotalGreyIssuedToProcess = 0,
        //        nGrandTotalGreyIssueBalance = 0;

        //        double nTotalOrderQtyInYard = 0,
        //        nTotalOrderQtyInMeter = 0,
        //        nTotalGrayTarget = 0,
        //        nTotalGrayDone = 0,
        //        nTotalGrayBalance = 0,
        //        nTotalGreyIssuedToProcess = 0,
        //        nTotalGreyIssueBalance = 0;

        //        string[] sBuyerIDs = string.Join(",", _oFEOs.Select(o => o.BuyerID).Distinct()).Split(',');
        //        List<FabricExecutionOrder> oFEOs = new List<FabricExecutionOrder>();

        //        foreach (string sBuyerID in sBuyerIDs)
        //        {
        //            int nBuyerID = Convert.ToInt32(sBuyerID);

        //            nSL = 0;
        //            nTotalOrderQtyInYard = 0;
        //            nTotalOrderQtyInMeter = 0;
        //            nTotalGrayTarget = 0;
        //            nTotalGrayDone = 0;
        //            nTotalGrayBalance = 0;
        //            nTotalGreyIssuedToProcess = 0;
        //            nTotalGreyIssueBalance = 0;

        //            oFEOs = new List<FabricExecutionOrder>();
        //            oFEOs = _oFEOs.Where(o => o.BuyerID == nBuyerID).ToList();
        //            int nTotalFEOCount = oFEOs.Count;
        //            if (nTotalFEOCount > 0)
        //            {
        //                foreach (FabricExecutionOrder oItem in oFEOs)
        //                {
        //                    nSL++;

        //                    colIndex = 2;
        //                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nSL; cell.Style.Font.Bold = false;
        //                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Construction; cell.Style.Font.Bold = false;
        //                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.FinishTypeName; cell.Style.Font.Bold = false;
        //                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (nSL == 1 ? oItem.BuyerName : "``"); cell.Style.Font.Bold = false;
        //                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.OrderNo.Replace("EXE-", ""); cell.Style.Font.Bold = false;
        //                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                    nTotalOrderQtyInYard += oItem.OrderQtyInYard;
        //                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.OrderQtyInYard; cell.Style.Font.Bold = false;
        //                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                    nTotalOrderQtyInMeter += oItem.OrderQtyInMeter;
        //                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.OrderQtyInMeter; cell.Style.Font.Bold = false;
        //                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                    nTotalGrayTarget += 0;
        //                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = 0; cell.Style.Font.Bold = false;
        //                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                    nTotalGrayDone += 0;
        //                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = 0; cell.Style.Font.Bold = false;
        //                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                    nTotalGrayBalance += 0;
        //                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = 0; cell.Style.Font.Bold = false;
        //                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                    nTotalGreyIssuedToProcess += 0;
        //                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = 0; cell.Style.Font.Bold = false;
        //                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                    nTotalGreyIssueBalance += 0;
        //                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = 0; cell.Style.Font.Bold = false;
        //                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ApproveDateInStr; cell.Style.Font.Bold = false;
        //                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
        //                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
        //                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
        //                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
        //                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                    rowIndex++;

        //                    nGrandTotalOrderQtyInYard += nTotalOrderQtyInYard;
        //                    nGrandTotalOrderQtyInMeter += nTotalOrderQtyInMeter;
        //                    nGrandTotalGrayTarget += nTotalGrayTarget;
        //                    nGrandTotalGrayDone += nTotalGrayDone;
        //                    nGrandTotalGrayBalance += nTotalGrayBalance;
        //                    nGrandTotalGreyIssuedToProcess += nTotalGreyIssuedToProcess;
        //                    nGrandTotalGreyIssueBalance += nTotalGreyIssueBalance;

        //                    #region Total
        //                    if (nSL == nTotalFEOCount)
        //                    {
        //                        colIndex = 2;
        //                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Total : "; cell.Style.Font.Bold = true;
        //                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nTotalOrderQtyInYard; cell.Style.Font.Bold = true;
        //                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nTotalOrderQtyInMeter; cell.Style.Font.Bold = true;
        //                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nTotalGrayTarget; cell.Style.Font.Bold = true;
        //                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nTotalGrayDone; cell.Style.Font.Bold = true;
        //                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nTotalGrayBalance; cell.Style.Font.Bold = true;
        //                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nTotalGreyIssuedToProcess; cell.Style.Font.Bold = true;
        //                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nTotalGreyIssueBalance; cell.Style.Font.Bold = true;
        //                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                        rowIndex++;
        //                    }
        //                    #endregion
        //                }
        //            }
        //        }

        //        #endregion

        //        #region Grand Total
        //        colIndex = 2;
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Grand Total : "; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nGrandTotalOrderQtyInYard; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nGrandTotalOrderQtyInMeter; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nGrandTotalGrayTarget; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nGrandTotalGrayDone; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nGrandTotalGrayBalance; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nGrandTotalGreyIssuedToProcess; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nGrandTotalGreyIssueBalance; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        rowIndex++;
        //        #endregion


        //        Response.ClearContent();
        //        Response.BinaryWrite(excelPackage.GetAsByteArray());
        //        Response.AddHeader("content-disposition", "attachment; filename=CUSTOMER WISE BULK ORDER IN HAND.xlsx");
        //        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //        Response.Flush();
        //        Response.End();
        //    }
        //}
        #endregion

        #endregion

        #region Piece to Piece Inspection Record Summery of Grey Inspection
        //public ActionResult View_PieceToPieceInspectionRecord(int menuid)
        //{
        //    this.Session.Remove(SessionInfo.MenuID);
        //    this.Session.Add(SessionInfo.MenuID, menuid);
        //    List<PieceToPieceInspectionRecord> oPTPIRs = new List<PieceToPieceInspectionRecord>();
        //    //oPTPIRs = PieceToPieceInspectionRecord.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
        //    return View(oPTPIRs);
        //}
        //[HttpPost]
        //public ActionResult PrintPTPIR(FormCollection DataCollection)
        //{
        //    string sFEOIDs = DataCollection["txtFEOIDs"];
        //    string sBuyerIDs = DataCollection["txtBuyerIDs"];
        //    string sQCDate = DataCollection["txtDatePrint"];
        //    string sQCDoneStatus = DataCollection["txtQCDoneStatus"];

        //    string sParams = sFEOIDs + "~" + sBuyerIDs + "~" + sQCDate + "~" + sQCDoneStatus;
        //    List<PieceToPieceInspectionRecord> oPTPIRs = new List<PieceToPieceInspectionRecord>();
        //    oPTPIRs = this.GetsPTPIR(sParams);

        //    Company oCompany = new Company();
        //    oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    oCompany.CompanyLogo = GetCompanyLogo(oCompany);

        //    string Messge = "Piece to Piece Inspection Record Summery of Grey Inspection";
        //    rptPieceToPieceInspectionRecord oReport = new rptPieceToPieceInspectionRecord();
        //    byte[] abytes = oReport.PrepareReport(oPTPIRs, oCompany, Messge);
        //    return File(abytes, "application/pdf");
        //}
        //public void ExcelPTPIR(string sParamFEOIDs, string sParamBuyerIDs, string sQCDate, int nQCDoneStatus)
        //{
        //    List<PieceToPieceInspectionRecord> oPTPIRs = new List<PieceToPieceInspectionRecord>();
        //    string sParams = sParamFEOIDs + "~" + sParamBuyerIDs + "~" + sQCDate + "~" + nQCDoneStatus;
        //    oPTPIRs = this.GetsPTPIR(sParams);

        //    int nMaxColumn = 0;
        //    int colIndex = 2;
        //    int rowIndex = 2;
        //    ExcelRange cell;
        //    ExcelFill fill;
        //    OfficeOpenXml.Style.Border border;

        //    using (var excelPackage = new ExcelPackage())
        //    {
        //        excelPackage.Workbook.Properties.Author = "ESimSol";
        //        excelPackage.Workbook.Properties.Title = "Export from ESimSol";
        //        var sheet = excelPackage.Workbook.Worksheets.Add("Piece to Piece Inspection Record Summery of Grey Inspection");
        //        sheet.Name = "Piece to Piece Inspection Record Summery of Grey Inspection";

        //        sheet.Column(2).Width = 8;  //SL
        //        sheet.Column(3).Width = 20; //Exe No
        //        sheet.Column(4).Width = 18; //Construction
        //        sheet.Column(5).Width = 30; //Buyer
        //        sheet.Column(6).Width = 16; //Fabrics Type
        //        sheet.Column(7).Width = 16; //Total Inspected 'A' Grade (Mtr)
        //        sheet.Column(8).Width = 14; //No of Than
        //        sheet.Column(9).Width = 16; //Total Inspected 'B'Grade (Mtr)
        //        sheet.Column(10).Width = 14; //No of Than
        //        sheet.Column(11).Width = 16; //Total Inspected Reject (Mtr)
        //        sheet.Column(12).Width = 14; //No of Than
        //        sheet.Column(13).Width = 16; //Total Inspected Received (A+B+R) (Mtr)
        //        sheet.Column(14).Width = 18; //Recording Start date
        //        sheet.Column(15).Width = 18; //Recording Finish date
        //        sheet.Column(16).Width = 18; //Grey Transferred date Finishing
        //        sheet.Column(17).Width = 16; //Remarks

        //        nMaxColumn = 17;

        //        Company oCompany = new Company();
        //        oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //        oCompany.CompanyLogo = GetCompanyLogo(oCompany);

        //        #region Report Header
        //        sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
        //        cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
        //        cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        rowIndex = rowIndex + 1;

        //        sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
        //        cell = sheet.Cells[rowIndex, 2]; cell.Value = "Piece to Piece Inspection Record Summery of Grey Inspection"; cell.Style.Font.Bold = true;
        //        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        rowIndex = rowIndex + 2;
        //        #endregion

        //        #region Table Header 01
        //        colIndex = 2;
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "SL"; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Exe No"; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Construction"; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Buyer"; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Fabrics Type"; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Total Inspected 'A' Grade (Mtr)"; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "No of Than"; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Total Inspected 'B'Grade (Mtr)"; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "No of Than"; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Total Inspected Reject (Mtr)"; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "No of Than"; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Total Inspected Received (A+B+R) (Mtr)"; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Recording Start date"; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Recording Finish date"; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Grey Transferred date Finishing"; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Remarks"; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        rowIndex++;
        //        #endregion

        //        #region Table Body
        //        if (oPTPIRs.Count > 0)
        //        {
        //            string[] sFEOIDs = string.Join(",", oPTPIRs.Select(x => x.FEOID).Distinct()).Split(',');
        //            List<PieceToPieceInspectionRecord> oTempPTPIRs = new List<PieceToPieceInspectionRecord>();
        //            int nSL = 0;

        //            foreach (string sFEOID in sFEOIDs)
        //            {
        //                nSL++;
        //                colIndex = 2;

        //                oTempPTPIRs = new List<PieceToPieceInspectionRecord>();
        //                oTempPTPIRs = oPTPIRs.Where(x => x.FEOID == Convert.ToInt32(sFEOID)).ToList();

        //                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nSL; cell.Style.Font.Bold = false;
        //                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oTempPTPIRs[0].OrderNo.Replace("EXE-", ""); cell.Style.Font.Bold = false;
        //                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oTempPTPIRs[0].Construction; cell.Style.Font.Bold = false;
        //                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oTempPTPIRs[0].BuyerName; cell.Style.Font.Bold = false;
        //                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oTempPTPIRs[0].FabricType; cell.Style.Font.Bold = false;
        //                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                double nAGradeQty = Math.Round(oTempPTPIRs.Sum(x => Global.GetMeter(x.AGradeQty, 2)));
        //                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nAGradeQty; cell.Style.Font.Bold = false;
        //                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                int nAThan = oTempPTPIRs.Sum(x => x.AThan);
        //                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nAThan; cell.Style.Font.Bold = false;
        //                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                double nBGradeQty = Math.Round(oTempPTPIRs.Sum(x => Global.GetMeter(x.BGradeQty, 2)));
        //                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nBGradeQty; cell.Style.Font.Bold = false;
        //                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                int nBThan = oTempPTPIRs.Sum(x => x.BThan);
        //                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nBThan; cell.Style.Font.Bold = false;
        //                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                double nRejectGradeQty = Math.Round(oTempPTPIRs.Sum(x => Global.GetMeter(x.RejectGradeQty, 2)));
        //                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nRejectGradeQty; cell.Style.Font.Bold = false;
        //                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                int nRejectThan = oTempPTPIRs.Sum(x => x.RejectThan);
        //                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nRejectThan; cell.Style.Font.Bold = false;
        //                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                double nTotalInspectedReceived = Math.Round(oTempPTPIRs.Sum(x => x.TotalInspectedReceived));
        //                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nTotalInspectedReceived; cell.Style.Font.Bold = false;
        //                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oTempPTPIRs[0].StartDateSt; cell.Style.Font.Bold = false;
        //                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                var nLastIndex = (oTempPTPIRs.Count > 1 ? oTempPTPIRs.Count - 1 : 0);
        //                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oTempPTPIRs[nLastIndex].EndDateSt; cell.Style.Font.Bold = false;
        //                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oTempPTPIRs[0].GreyTransferredDateSt; cell.Style.Font.Bold = false;
        //                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oTempPTPIRs[0].Remark; cell.Style.Font.Bold = false;
        //                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                rowIndex++;
        //            }
        //        }
        //        #endregion

        //        #region Total
        //        if (oPTPIRs.Count > 0)
        //        {

        //            colIndex = 2;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Total : "; cell.Style.Font.Bold = true;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            double nTotalAQty = Math.Round(oPTPIRs.Sum(x => Global.GetMeter(x.AGradeQty, 2)));
        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nTotalAQty; cell.Style.Font.Bold = true;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            int nANoOfThan = oPTPIRs.Sum(x => x.AThan);
        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nANoOfThan; cell.Style.Font.Bold = true;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            double nTotalBQty = Math.Round(oPTPIRs.Sum(x => Global.GetMeter(x.BGradeQty, 2)));
        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nTotalBQty; cell.Style.Font.Bold = true;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            int nBNoOfThan = oPTPIRs.Sum(x => x.BThan);
        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nBNoOfThan; cell.Style.Font.Bold = true;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            double nTotalRejectQty = Math.Round(oPTPIRs.Sum(x => Global.GetMeter(x.RejectGradeQty, 2)));
        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nTotalRejectQty; cell.Style.Font.Bold = true;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            int nRejectNoOfThan = oPTPIRs.Sum(x => x.RejectThan);
        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nRejectNoOfThan; cell.Style.Font.Bold = true;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            double nTotal = Math.Round(oPTPIRs.Sum(x => x.TotalInspectedReceived));
        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nTotal; cell.Style.Font.Bold = true;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //        }
        //        #endregion

        //        Response.ClearContent();
        //        Response.BinaryWrite(excelPackage.GetAsByteArray());
        //        Response.AddHeader("content-disposition", "attachment; filename=Piece to Piece Inspection Record Summery of Grey Inspection.xlsx");
        //        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //        Response.Flush();
        //        Response.End();
        //    }
        //}
        //private List<PieceToPieceInspectionRecord> GetsPTPIR(string sParams)
        //{
        //    string sReturn1 = "SELECT * FROM View_RPT_PieceToPieceInspectionRecord ";
        //    string sReturn = "";

        //    string sFEOIDs = sParams.Split('~')[0];
        //    string sBuyerIDs = sParams.Split('~')[1];
        //    string sQCDate = sParams.Split('~')[2];
        //    int nQCDoneStatus = Convert.ToInt32(sParams.Split('~')[3]);

        //    #region FEO Wise
        //    if (!string.IsNullOrEmpty(sFEOIDs))
        //    {
        //        Global.TagSQL(ref sReturn);
        //        sReturn = sReturn + " FEOID IN (" + sFEOIDs + ") ";
        //    }
        //    #endregion

        //    #region Buyer Wise
        //    if (!string.IsNullOrEmpty(sBuyerIDs))
        //    {
        //        Global.TagSQL(ref sReturn);
        //        sReturn = sReturn + " BuyerID IN (" + sBuyerIDs + ") ";
        //    }
        //    #endregion

        //    #region QC Date
        //    bool bIsQCDate = Convert.ToBoolean(sQCDate.Split('>')[0]);
        //    if (bIsQCDate)
        //    {
        //        DateTime dFromDate = Convert.ToDateTime(sQCDate.Split('>')[1]);
        //        DateTime dToDate = Convert.ToDateTime(sQCDate.Split('>')[2]);

        //        Global.TagSQL(ref sReturn);
        //        sReturn = sReturn + " ('" + dFromDate.ToString("dd MMM yyyy") + "' Between Convert(date,StartDate) And Convert(date,EndDate) Or '" + dToDate.ToString("dd MMM yyyy") + "' Between Convert(date,StartDate) And Convert(date,EndDate))";
        //    }
        //    #endregion

        //    #region QC Done Status
        //    if (nQCDoneStatus > 0)
        //    {
        //        if (nQCDoneStatus == 1)
        //        {
        //            Global.TagSQL(ref sReturn);
        //            sReturn = sReturn + " (FEOQty <= TotalQcDetailQty) ";
        //        }
        //        else if (nQCDoneStatus == 2)
        //        {
        //            Global.TagSQL(ref sReturn);
        //            sReturn = sReturn + " (FEOQty > TotalQcDetailQty) ";
        //        }
        //    }
        //    #endregion

        //    string sSQL = sReturn1 + " " + sReturn + " ORDER BY FBQCID";
        //    List<PieceToPieceInspectionRecord> oPTPIRs = new List<PieceToPieceInspectionRecord>();
        //    oPTPIRs = PieceToPieceInspectionRecord.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    return oPTPIRs;
        //}

        #endregion
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

        #region Product Report
        //public ActionResult View_DailyProductionReport(int menuid)
        //{
        //    this.Session.Remove(SessionInfo.MenuID);
        //    this.Session.Add(SessionInfo.MenuID, menuid);
        //    List<RptProductReport> oRptProductReports = new List<RptProductReport>();

        //    string sSQL = "Select * from View_TextileSubUnit Where ISNULL(InactiveBy,0)=0";
        //    List<TextileSubUnit> oTextileSubUnits = new List<TextileSubUnit>();
        //    oTextileSubUnits = TextileSubUnit.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    List<HRMShift> oHRMShifts = new List<HRMShift>();
        //    oHRMShifts = HRMShift.Gets("SELECT * FROM HRM_Shift WHERE IsActive=1", ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    ViewBag.HRMShifts = oHRMShifts;
        //    ViewBag.TextileSubUnits = oTextileSubUnits;
        //    List<FabricProcess> oFabricProcesss = new List<FabricProcess>();
        //    oFabricProcesss = FabricProcess.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);

        //    ViewBag.FinishTypes = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Finish).ToList();
        //    ViewBag.ProcessTypes = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Process).ToList();
        //    ViewBag.FabricWeaves = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Weave).ToList();

        //    return View(oRptProductReports);
        //}
        //public void ExcelProductionReport(int tsuid, string Params)
        //{


        //    using (var excelPackage = GenerateDailyProductionReportInExcel(tsuid, Params, false))
        //    {
        //        #region Response Part
        //        Response.ClearContent();
        //        Response.BinaryWrite(excelPackage.GetAsByteArray());
        //        Response.AddHeader("content-disposition", "attachment; filename=Production Report.xlsx");
        //        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //        Response.Flush();
        //        Response.End();
        //        #endregion
        //    }
        //}
        private List<dynamic> GetDynamicListForLoomProductionSummery(DataTable oDataTable)
        {
            List<dynamic> oDynamiObjects = new List<dynamic>();
            //List<object> oDynamiObjects = new List<object>();
            foreach (DataRow oDataRow in oDataTable.Rows)
            {

                dynamic obj = new ExpandoObject();
                var expobj = obj as IDictionary<string, object>;

                expobj.Add("MachineName", (oDataRow["MachineName"] == DBNull.Value) ? "" : oDataRow["MachineName"]);
                expobj.Add("AShiftProdQty", (oDataRow["AShiftProdQty"] == DBNull.Value) ? 0 : Convert.ToDouble(oDataRow["AShiftProdQty"]));
                expobj.Add("BShiftProdQty", (oDataRow["BShiftProdQty"] == DBNull.Value) ? 0 : Convert.ToDouble(oDataRow["BShiftProdQty"]));
                expobj.Add("CShiftProdQty", (oDataRow["CShiftProdQty"] == DBNull.Value) ? 0 : Convert.ToDouble(oDataRow["CShiftProdQty"]));
                expobj.Add("AShiftLoomEfficiency", (oDataRow["AShiftLoomEfficiency"] == DBNull.Value) ? 0 : Convert.ToDouble(oDataRow["AShiftLoomEfficiency"]));
                expobj.Add("BShiftLoomEfficiency", (oDataRow["BShiftLoomEfficiency"] == DBNull.Value) ? 0 : Convert.ToDouble(oDataRow["BShiftLoomEfficiency"]));
                expobj.Add("CShiftLoomEfficiency", (oDataRow["CShiftLoomEfficiency"] == DBNull.Value) ? 0 : Convert.ToDouble(oDataRow["CShiftLoomEfficiency"]));
                expobj.Add("AShiftLoom", (oDataRow["AShiftLoom"] == DBNull.Value) ? 0 : Convert.ToInt32(oDataRow["AShiftLoom"]));
                expobj.Add("BShiftLoom", (oDataRow["BShiftLoom"] == DBNull.Value) ? 0 : Convert.ToInt32(oDataRow["BShiftLoom"]));
                expobj.Add("CShiftLoom", (oDataRow["CShiftLoom"] == DBNull.Value) ? 0 : Convert.ToInt32(oDataRow["CShiftLoom"]));
                expobj.Add("AShiftRPM", (oDataRow["AShiftRPM"] == DBNull.Value) ? 0 : Convert.ToInt32(oDataRow["AShiftRPM"]));
                expobj.Add("BShiftRPM", (oDataRow["BShiftRPM"] == DBNull.Value) ? 0 : Convert.ToInt32(oDataRow["BShiftRPM"]));
                expobj.Add("CShiftRPM", (oDataRow["CShiftRPM"] == DBNull.Value) ? 0 : Convert.ToInt32(oDataRow["CShiftRPM"]));
                oDynamiObjects.Add(expobj);

            }
            return oDynamiObjects;
        }

        //private List<RptProductReport> GetRptProductReports(DataTable oDataTable)
        //{
        //    List<RptProductReport> oRptProductReports = new List<RptProductReport>();

        //    foreach (DataRow oDataRow in oDataTable.Rows)
        //    {
        //        RptProductReport oRptProductReport = new RptProductReport();
        //        oRptProductReport.Construction = oDataRow.Field<string>("Construction");
        //        oRptProductReport.BuyerID = oDataRow.Field<Int32>("BuyerID");
        //        oRptProductReport.BuyerName = oDataRow.Field<string>("BuyerName");
        //        oRptProductReport.FEOID = oDataRow.Field<Int32>("FEOID");
        //        oRptProductReport.FEONo = oDataRow.Field<string>("FEONo");
        //        oRptProductReport.IsInHouse = oDataRow.Field<Boolean>("IsInHouse");
        //        oRptProductReport.OrderType = (EnumOrderType)oDataRow.Field<Int16>("OrderType");
        //        oRptProductReport.FEOQty =Convert.ToDouble(oDataRow["FEOQty"]);
        //        oRptProductReport.OrderReceiptDate = oDataRow.Field<DateTime>("OrderReceiptDate");
        //        oRptProductReport.AskingDeliveryDate = oDataRow.Field<DateTime>("AskingDeliveryDate");
        //        oRptProductReport.FabricsType = oDataRow.Field<string>("FabricsType"); 
        //        oRptProductReport.MaxLoomRPM = oDataRow.Field<Int32>("MaxLoomRPM");
        //        oRptProductReport.MinLoomRPM = oDataRow.Field<Int32>("MinLoomRPM"); 
        //        oRptProductReport.AShiftLoom = oDataRow.Field<Int32>("AShiftLoom"); 
        //        oRptProductReport.BShiftLoom = oDataRow.Field<Int32>("BShiftLoom"); 
        //        oRptProductReport.CShiftLoom = oDataRow.Field<Int32>("CShiftLoom");
        //        oRptProductReport.AShiftQty =Convert.ToDouble(oDataRow["AShiftQty"]);
        //        oRptProductReport.BShiftQty =Convert.ToDouble( oDataRow["BShiftQty"]);
        //        oRptProductReport.CShiftQty = Convert.ToDouble(oDataRow["CShiftQty"]);
        //        oRptProductReport.AShiftLoomEfficiency =Convert.ToDouble( oDataRow["AShiftLoomEfficiency"]);
        //        oRptProductReport.BShiftLoomEfficiency = Convert.ToDouble(oDataRow["BShiftLoomEfficiency"]);
        //        oRptProductReport.CShiftLoomEfficiency =Convert.ToDouble( oDataRow["CShiftLoomEfficiency"]);
        //        oRptProductReport.GreyTarget =Convert.ToDouble( oDataRow["GreyTarget"]);
        //        oRptProductReport.BeamQty = Convert.ToDouble(oDataRow["UpToProductionQty"]); 
        //        oRptProductReport.Remark = oDataRow.Field<string>("Remark"); 
        //        oRptProductReport.IsYarnDyed = oDataRow.Field<Boolean>("IsYarnDyed");
        //        oRptProductReport.TSUID = oDataRow.Field<Int32>("TSUID"); 
        //        oRptProductReport.TSUName = oDataRow.Field<string>("TSUName");
        //        oRptProductReport.TtlRPM = oDataRow.Field<Int32>("TtlRPM");
        //        oRptProductReports.Add(oRptProductReport);
        //    }
        //    return oRptProductReports;
        //}
        //public ExcelPackage GenerateDailyProductionReportInExcel(int tsuid, string sParams, bool bIsMail)
        //{

          
        //    Company oCompany = new Company();
        //    oCompany = oCompany.Get(1, (bIsMail) ? 0 : ((User)Session[SessionInfo.CurrentUser]).UserID);
    
        //    string sTexSubUnit = string.Empty; 
        //    RptProductReport oRptProductReport = new RptProductReport();
        //    oRptProductReport.Date = Convert.ToDateTime(sParams.Split('~')[0]);
        //    oRptProductReport.EndDate = Convert.ToDateTime(sParams.Split('~')[1]);
        //    oRptProductReport.sBuyerIDs = sParams.Split('~')[2];
        //    oRptProductReport.sFEOIDs = sParams.Split('~')[3];
        //    oRptProductReport.FabricType = Convert.ToInt32(sParams.Split('~')[4]);
        //    oRptProductReport.ProcessType = Convert.ToInt32(sParams.Split('~')[5]);
        //    oRptProductReport.Construction =sParams.Split('~')[6];
        //    oRptProductReport.sFMIDs = sParams.Split('~')[7];
        //    oRptProductReport.chkNewLoom = Convert.ToBoolean(sParams.Split('~')[8]);
        //    oRptProductReport.chkRunOut = Convert.ToBoolean(sParams.Split('~')[9]);
        //    oRptProductReport.chkLoomDone = Convert.ToBoolean(sParams.Split('~')[10]);
        //    List<RptProductReport> oRptProductReports = new List<RptProductReport>();
        //    List<dynamic> oDynamicLoomProductionSumms = new List<dynamic>();
        //    DataSet oLoadDataSets = RptProductReport.GetDailyProductionReport(tsuid, oRptProductReport, (bIsMail) ? 0 : ((User)Session[SessionInfo.CurrentUser]).UserID);
        //     oRptProductReports = this.GetRptProductReports(oLoadDataSets.Tables[0]);
        //    oDynamicLoomProductionSumms = this.GetDynamicListForLoomProductionSummery(oLoadDataSets.Tables[1]);

        //    List<RptProductReport> oYarnDyedRptProductReports = new List<RptProductReport>();
        //    List<RptProductReport> oSolidDyedRptProductReports = new List<RptProductReport>();
        //    List<RptProductReport> oScwYarnDyedRptProductReports = new List<RptProductReport>();
        //    List<RptProductReport> oScwSolidDyedRptProductReports = new List<RptProductReport>();
        //    if (oRptProductReports.Count > 0)
        //    {
        //        sTexSubUnit = (tsuid>0)? oRptProductReports.First().TSUName : "";
        //        oYarnDyedRptProductReports = oRptProductReports.Where(x => x.IsYarnDyed && x.IsInHouse).ToList();
        //        oSolidDyedRptProductReports = oRptProductReports.Where(x => !x.IsYarnDyed && x.IsInHouse).ToList();
        //        oScwYarnDyedRptProductReports = oRptProductReports.Where(x => x.IsYarnDyed && !x.IsInHouse).ToList();
        //        oScwSolidDyedRptProductReports = oRptProductReports.Where(x => !x.IsYarnDyed && !x.IsInHouse).ToList();
        //    }
        //    List<RptProductReport> oTempItems = new List<RptProductReport>();

        //    var excelPackage = new ExcelPackage();

        //    int nMaxColumn = 0;
        //    int colIndex = 2;
        //    int rowIndex = 2;
        //    ExcelRange cell;
        //    ExcelFill fill;
        //    OfficeOpenXml.Style.Border border;

        //    excelPackage.Workbook.Properties.Author = "ESimSol";
        //    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
        //    var sheet = excelPackage.Workbook.Worksheets.Add("Production Report");
        //    sheet.Name = "Production Report";
        //    var color = Color.White;

        //    #region Columns
        //    sheet.Column(2).Width = 5; //SL.
        //    sheet.Column(3).Width = 15; //Construction
        //    sheet.Column(4).Width = 18; //BuyerName
        //    sheet.Column(5).Width = 18; //OrderNo
        //    sheet.Column(6).Width = 18; //OrderQty
        //    sheet.Column(7).Width = 14; //OrderReceiptDate
        //    sheet.Column(8).Width = 14; //AskingDeliveryDate
        //    sheet.Column(9).Width = 14; //FabricsType
        //    sheet.Column(10).Width = 15; //Avg. LoomRPM
        //    sheet.Column(11).Width = 10;//Max. ~ Min. value
        //    sheet.Column(12).Width = 10;//AShiftLoom
        //    sheet.Column(13).Width = 10;//BShiftLoom
        //    sheet.Column(14).Width = 10;//CShiftLoom
        //    sheet.Column(15).Width = 10;//ShiftLoomAvg
        //    sheet.Column(16).Width = 18;//AShiftQty
        //    sheet.Column(17).Width = 18;//BShiftQty
        //    sheet.Column(18).Width = 18;//CShiftQty
        //    sheet.Column(19).Width = 18;//ShiftQtyMtrTotal
        //    sheet.Column(20).Width = 18;//AShiftLoomEfficiency
        //    sheet.Column(21).Width = 18;//AShiftEfficiency
        //    sheet.Column(22).Width = 18;//BShiftLoomEfficiency
        //    sheet.Column(23).Width = 18;//BShiftEfficiency
        //    sheet.Column(24).Width = 18;//CShiftLoomEfficiency
        //    sheet.Column(25).Width = 18;//CShiftEfficiency
        //    sheet.Column(26).Width = 18;//ShiftLoomEfficiencyAvg
        //    sheet.Column(27).Width = 18;//GreyTarget
        //    sheet.Column(28).Width = 18;//BeamQty
           
        //    nMaxColumn = 31;
        //    #endregion

        //    #region Report Header
        //    sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
        //    cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
        //    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    rowIndex = rowIndex + 1;
        //    string rptDateRange = string.Empty;
        //    rptDateRange = oRptProductReport.Date.ToString("dd MMM yyyy");
        //    if (oRptProductReport.Date.ToString("dd MMM yyyy") != oRptProductReport.EndDate.ToString("dd MMM yyyy"))
        //        rptDateRange = oRptProductReport.Date.ToString("dd MMM yyyy") + " To " + oRptProductReport.EndDate.ToString("dd MMM yyyy");
        //    sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
        //    cell = sheet.Cells[rowIndex, 2]; cell.Value = "Production Report (" + rptDateRange + ")"; cell.Style.Font.Bold = true;
        //    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    rowIndex = rowIndex + 1;

        //    string sShadeInfo = "";
        //    if (tsuid == 0)
        //        sShadeInfo = "Shade-1 & Shade-2";
        //    if (tsuid == 1)
        //        sShadeInfo = "Shade-1";
        //    if (tsuid == 2)
        //        sShadeInfo = "Shade-2";
        //    sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
        //    cell = sheet.Cells[rowIndex, 2]; cell.Value = "Production Report For " + sShadeInfo; cell.Style.Font.Bold = true;
        //    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    rowIndex = rowIndex + 1;

        //    if (sTexSubUnit != string.Empty)
        //    {
        //        sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
        //        cell = sheet.Cells[rowIndex, 2]; cell.Value = sTexSubUnit; cell.Style.Font.Bold = true;
        //        cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //    }
        //    rowIndex = rowIndex + 1;

        //    #endregion

        //    #region Table Header
        //    colIndex = 2;
           
        //    #region SL
        //    cell = sheet.Cells[5, colIndex, 6, colIndex];
        //    cell.Merge = true;
        //    cell.Value = "SL"; cell.Style.Font.Bold = true;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    fill = cell.Style.Fill;
        //    fill.PatternType = ExcelFillStyle.Solid;
        //    fill.BackgroundColor.SetColor(Color.LimeGreen);
        //    border = cell.Style.Border;
        //    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //    #endregion

        //    #region Construction
        //    colIndex = colIndex + 1;
        //    cell = sheet.Cells[5, colIndex, 6, colIndex];
        //    cell.Merge = true;
        //    cell.Value = "Construction";
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.Font.Bold = true;
        //    fill = cell.Style.Fill;
        //    fill.PatternType = ExcelFillStyle.Solid;
        //    fill.BackgroundColor.SetColor(Color.LimeGreen);
        //    border = cell.Style.Border;
        //    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //    #endregion

        //    #region Buyer
        //    colIndex = colIndex + 1;
        //    cell = sheet.Cells[5, colIndex, 6, colIndex];
        //    cell.Merge = true;
        //    cell.Value = "Buyer";
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.Font.Bold = true;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
        //    fill.BackgroundColor.SetColor(Color.LimeGreen);
        //    border = cell.Style.Border;
        //    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //    #endregion

        //    #region Order No
        //    colIndex = colIndex + 1;
        //    cell = sheet.Cells[5, colIndex, 6, colIndex];
        //    cell.Merge = true;
        //    cell.Value = "Order No";
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.Font.Bold = true;
        //    fill = cell.Style.Fill;
        //    fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LimeGreen);
        //    border = cell.Style.Border;
        //    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //    #endregion

        //    #region Order Qty (Yds)
        //    colIndex = colIndex + 1;
        //    cell = sheet.Cells[5, colIndex, 6, colIndex];
        //    cell.Merge = true;
        //    cell.Value = "Order Qty (Yds)";
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.Font.Bold = true;
        //    fill = cell.Style.Fill;
        //    fill.PatternType = ExcelFillStyle.Solid;
        //    fill.BackgroundColor.SetColor(Color.LimeGreen);
        //    border = cell.Style.Border;
        //    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //    #endregion

        //    #region Order Receipt Date
        //    colIndex = colIndex + 1;
        //    cell = sheet.Cells[5, colIndex, 6, colIndex];
        //    cell.Merge = true;
        //    cell.Value = "Order Receipt Date";
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.Font.Bold = true;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
        //    fill.BackgroundColor.SetColor(Color.LimeGreen);
        //    border = cell.Style.Border;
        //    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //    #endregion

        //    #region Asking Delivery Date
        //    colIndex = colIndex + 1;
        //    cell = sheet.Cells[5, colIndex, 6, colIndex];
        //    cell.Merge = true;
        //    cell.Value = "Asking Delivery Date";
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.Font.Bold = true;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
        //    fill.BackgroundColor.SetColor(Color.LimeGreen);
        //    border = cell.Style.Border;
        //    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //    #endregion

        //    #region Fabrics Type
        //    colIndex = colIndex + 1;
        //    cell = sheet.Cells[5, colIndex, 6, colIndex];
        //    cell.Merge = true;
        //    cell.Value = "Fabrics Type";
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.Font.Bold = true;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
        //    fill.BackgroundColor.SetColor(Color.LimeGreen);
        //    border = cell.Style.Border;
        //    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //    #endregion

        //    #region AVG. LOOM RPM
        //    colIndex = colIndex + 1;
        //    cell = sheet.Cells[5, colIndex, 6, colIndex];
        //    cell.Merge = true;
        //    cell.Value = "AVG. LOOM RPM";
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.Font.Bold = true;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
        //    fill.BackgroundColor.SetColor(Color.LimeGreen);
        //    border = cell.Style.Border;
        //    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //    #endregion

        //    #region Max. ~ Min. value
        //    colIndex = colIndex + 1;
        //    cell = sheet.Cells[5, colIndex, 6, colIndex];
        //    cell.Merge = true;
        //    cell.Value = "Max. ~ Min. value";
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.Font.Bold = true;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
        //    fill.BackgroundColor.SetColor(Color.LimeGreen);
        //    border = cell.Style.Border;
        //    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //    #endregion

        //    #region Per Shift Running Loom

        //    colIndex = colIndex + 4;

        //    cell = sheet.Cells[5, 12, 5, 15];
        //    cell.Merge = true;
        //    cell.Value = "Per Shift Running Loom";
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.Font.Bold = true;
        //    fill = cell.Style.Fill;
        //    fill.PatternType = ExcelFillStyle.Solid;
        //    fill.BackgroundColor.SetColor(Color.LimeGreen);
        //    border = cell.Style.Border;
        //    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[6, 12];
        //    cell.Value = "A";
        //    cell.Style.Font.Bold = true;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    fill = cell.Style.Fill;
        //    fill.PatternType = ExcelFillStyle.Solid;
        //    fill.BackgroundColor.SetColor(Color.LimeGreen);
        //    border = cell.Style.Border;
        //    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[6, 13];
        //    cell.Value = "B";
        //    cell.Style.Font.Bold = true;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    fill = cell.Style.Fill;
        //    fill.PatternType = ExcelFillStyle.Solid;
        //    fill.BackgroundColor.SetColor(Color.LimeGreen);
        //    border = cell.Style.Border;
        //    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[6, 14];
        //    cell.Value = "C";
        //    cell.Style.Font.Bold = true;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    fill = cell.Style.Fill;
        //    fill.PatternType = ExcelFillStyle.Solid;
        //    fill.BackgroundColor.SetColor(Color.LimeGreen);
        //    border = cell.Style.Border;
        //    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[6, 15];
        //    cell.Value = "Avg";
        //    cell.Style.Font.Bold = true;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    fill = cell.Style.Fill;
        //    fill.PatternType = ExcelFillStyle.Solid;
        //    fill.BackgroundColor.SetColor(Color.LimeGreen);
        //    border = cell.Style.Border;
        //    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    #endregion

        //    #region Per Shift Production mtr.
        //    colIndex = colIndex + 4;

        //    cell = sheet.Cells[5, 16, 5, 19];
        //    cell.Merge = true;
        //    cell.Value = "Per Shift Production mtr.";
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.Font.Bold = true;
        //    fill = cell.Style.Fill;
        //    fill.PatternType = ExcelFillStyle.Solid;
        //    fill.BackgroundColor.SetColor(Color.LimeGreen);
        //    border = cell.Style.Border;
        //    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[6, 16];
        //    cell.Value = "A";
        //    cell.Style.Font.Bold = true;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    fill = cell.Style.Fill;
        //    fill.PatternType = ExcelFillStyle.Solid;
        //    fill.BackgroundColor.SetColor(Color.LimeGreen);
        //    border = cell.Style.Border;
        //    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[6, 17];
        //    cell.Value = "B";
        //    cell.Style.Font.Bold = true;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    fill = cell.Style.Fill;
        //    fill.PatternType = ExcelFillStyle.Solid;
        //    fill.BackgroundColor.SetColor(Color.LimeGreen);
        //    border = cell.Style.Border;
        //    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[6, 18];
        //    cell.Value = "C";
        //    cell.Style.Font.Bold = true;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    fill = cell.Style.Fill;
        //    fill.PatternType = ExcelFillStyle.Solid;
        //    fill.BackgroundColor.SetColor(Color.LimeGreen);
        //    border = cell.Style.Border;
        //    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[6, 19];
        //    cell.Value = "Total";
        //    cell.Style.Font.Bold = true;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    fill = cell.Style.Fill;
        //    fill.PatternType = ExcelFillStyle.Solid;
        //    fill.BackgroundColor.SetColor(Color.LimeGreen);
        //    border = cell.Style.Border;
        //    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //    #endregion

        //    #region Per Shift Efficiency
        //    colIndex = colIndex + 7;

        //    cell = sheet.Cells[5, 20, 5, 23];
        //    cell.Merge = true;
        //    cell.Value = "Per Shift Efficiency";
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.Font.Bold = true;
        //    fill = cell.Style.Fill;
        //    fill.PatternType = ExcelFillStyle.Solid;
        //    fill.BackgroundColor.SetColor(Color.LimeGreen);
        //    border = cell.Style.Border;
        //    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    //cell = sheet.Cells[6, 20];
        //    //cell.Value = "A Shift Total Loom Efficiency";
        //    //cell.Style.Font.Bold = true;
        //    //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    //cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    //fill = cell.Style.Fill;
        //    //fill.PatternType = ExcelFillStyle.Solid;
        //    //fill.BackgroundColor.SetColor(Color.LimeGreen);
        //    //border = cell.Style.Border;
        //    //border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[6, 20];
        //    cell.Value = "A";
        //    cell.Style.Font.Bold = true;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    fill = cell.Style.Fill;
        //    fill.PatternType = ExcelFillStyle.Solid;
        //    fill.BackgroundColor.SetColor(Color.LimeGreen);
        //    border = cell.Style.Border;
        //    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    //cell = sheet.Cells[6, 22];
        //    //cell.Value = "B Shift Total Loom Efficiency";
        //    //cell.Style.Font.Bold = true;
        //    //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    //cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    //fill = cell.Style.Fill;
        //    //fill.PatternType = ExcelFillStyle.Solid;
        //    //fill.BackgroundColor.SetColor(Color.LimeGreen);
        //    //border = cell.Style.Border;
        //    //border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[6, 21];
        //    cell.Value = "B";
        //    cell.Style.Font.Bold = true;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    fill = cell.Style.Fill;
        //    fill.PatternType = ExcelFillStyle.Solid;
        //    fill.BackgroundColor.SetColor(Color.LimeGreen);
        //    border = cell.Style.Border;
        //    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    //cell = sheet.Cells[6, 24];
        //    //cell.Value = "C Shift Total Loom Efficiency";
        //    //cell.Style.Font.Bold = true;
        //    //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    //cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    //fill = cell.Style.Fill;
        //    //fill.PatternType = ExcelFillStyle.Solid;
        //    //fill.BackgroundColor.SetColor(Color.LimeGreen);
        //    //border = cell.Style.Border;
        //    //border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[6, 22];
        //    cell.Value = "C";
        //    cell.Style.Font.Bold = true;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    fill = cell.Style.Fill;
        //    fill.PatternType = ExcelFillStyle.Solid;
        //    fill.BackgroundColor.SetColor(Color.LimeGreen);
        //    border = cell.Style.Border;
        //    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[6, 23];
        //    cell.Value = "Avg";
        //    cell.Style.Font.Bold = true;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    fill = cell.Style.Fill;
        //    fill.PatternType = ExcelFillStyle.Solid;
        //    fill.BackgroundColor.SetColor(Color.LimeGreen);
        //    border = cell.Style.Border;
        //    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //    #endregion

        //    #region Grey Target mtr.
        //    //colIndex = colIndex + 1;
        //    colIndex = 24;
        //    cell = sheet.Cells[5, colIndex, 6, colIndex];
        //    cell.Merge = true;
        //    cell.Value = "Grey Target mtr.";
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.Font.Bold = true;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
        //    fill.BackgroundColor.SetColor(Color.LimeGreen);
        //    border = cell.Style.Border;
        //    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //    #endregion

        //    #region Up to Date Production mtr.
        //    colIndex = colIndex + 1;
        //    cell = sheet.Cells[5, colIndex, 6, colIndex];
        //    cell.Merge = true;
        //    cell.Value = "Up to Date Production mtr.";
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.Font.Bold = true;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
        //    fill.BackgroundColor.SetColor(Color.LimeGreen);
        //    border = cell.Style.Border;
        //    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //    #endregion

        //    #region Balance to weave mtr.
        //    colIndex = colIndex + 1;
        //    cell = sheet.Cells[5, colIndex, 6, colIndex];
        //    cell.Merge = true;
        //    cell.Value = "Balance to weave mtr.";
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.Font.Bold = true;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
        //    fill.BackgroundColor.SetColor(Color.LimeGreen);
        //    border = cell.Style.Border;
        //    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //    #endregion

        //    #region Remarks
        //    colIndex = colIndex + 1;
        //    cell = sheet.Cells[5, colIndex, 6, colIndex];
        //    cell.Merge = true;
        //    cell.Value = "Remarks";
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.Font.Bold = true;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
        //    fill.BackgroundColor.SetColor(Color.LimeGreen);
        //    border = cell.Style.Border;
        //    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //    #endregion

        //    rowIndex = rowIndex + 2;
        //    #endregion

        //    #region Yarn Dyed
        //    double ttlAvgLoom = 0;
        //    if (oYarnDyedRptProductReports.Count > 0)
        //    {
        //        #region Yarn dyed title
        //        color = Color.Cyan;
        //        colIndex = 2;
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "EXE-Y/D"; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
               
        //        rowIndex++;
        //        #endregion

        //        #region Yarn Dyed
        //        int nSL = 0;
        //        string[] sFEOIDs = string.Join(",", oYarnDyedRptProductReports.Select(x => x.FEOID).Distinct()).Split(',');
        //        foreach (string sFEOID in sFEOIDs)
        //        {
        //            nSL++;
        //            oTempItems = new List<RptProductReport>();
        //            oTempItems = oYarnDyedRptProductReports.Where(x => x.FEOID == Convert.ToInt32(sFEOID)).ToList();

        //            colIndex = 2;
        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nSL.ToString(); cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oTempItems[0].Construction; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oTempItems[0].BuyerName; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oTempItems[0].OrderNo; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oTempItems[0].FEOQty; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oTempItems[0].OrderReceiptDateSt; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oTempItems[0].AskingDeliveryDateSt; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oTempItems[0].FabricsType; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            int nAShiftLoom = oTempItems.Sum(x => x.AShiftLoom);
        //            int nBShiftLoom = oTempItems.Sum(x => x.BShiftLoom);
        //            int nCShiftLoom = oTempItems.Sum(x => x.CShiftLoom);
        //            double nLoomRPMAvg = (nAShiftLoom + nBShiftLoom + nCShiftLoom > 0) ? Math.Round((double)(oTempItems[0].TtlRPM / (nAShiftLoom + nBShiftLoom + nCShiftLoom)), 2) : 0;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value =nLoomRPMAvg; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oTempItems[0].LoomRpmMaxMin; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //           // var nAShiftLoom = oTempItems.Sum(x => x.AShiftLoom);
        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oTempItems[0].AShiftLoom; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //           // var nBShiftLoom = oTempItems.Sum(x => x.BShiftLoom);
        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oTempItems[0].BShiftLoom; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //           // var nCShiftLoom = oTempItems.Sum(x => x.CShiftLoom);
        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oTempItems[0].CShiftLoom; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            int LoomAvgCount = 0;
        //            if (nAShiftLoom != 0)
        //                LoomAvgCount++;
        //            if (nBShiftLoom != 0)
        //                LoomAvgCount++;
        //            if (nCShiftLoom != 0)
        //                LoomAvgCount++;

        //            double nShiftLoomAvg = (LoomAvgCount != 0) ? Math.Round((double)(nAShiftLoom + nBShiftLoom + nCShiftLoom) / LoomAvgCount, 0) : 0;
        //            ttlAvgLoom += nShiftLoomAvg;
        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nShiftLoomAvg; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            var nAShiftQtyMtr = oTempItems.Sum(x => x.AShiftQtyMtr);
        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nAShiftQtyMtr; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            var nBShiftQtyMtr = oTempItems.Sum(x => x.BShiftQtyMtr);
        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nBShiftQtyMtr; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            var nCShiftQtyMtr = oTempItems.Sum(x => x.CShiftQtyMtr);
        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nCShiftQtyMtr; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            var nShiftQtyMtrTotal = oTempItems.Sum(x => x.ShiftQtyMtrTotal);
        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nShiftQtyMtrTotal; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            var nAShiftEfficiency = oTempItems.Sum(x => x.AShiftLoomEfficiency);
        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nAShiftEfficiency; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            var nBShiftEfficiency = oTempItems.Sum(x => x.BShiftLoomEfficiency);
        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nBShiftEfficiency; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            var nCShiftEfficiency = oTempItems.Sum(x => x.CShiftLoomEfficiency);
        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nCShiftEfficiency; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //            //int EffAvgCount = 0;
        //            //if (nAShiftEfficiency != 0)
        //            //    EffAvgCount++;
        //            //if (nBShiftLoom != 0)
        //            //    EffAvgCount++;
        //            //if (nCShiftLoom != 0)
        //            //    EffAvgCount++;

        //            //int nShiftEfficiencyAvg = (EffAvgCount != 0) ? Convert.ToInt32((nAShiftEfficiency + nBShiftEfficiency + nCShiftEfficiency) / EffAvgCount) : 0;

        //            int ttlLoomCount = Convert.ToInt32(nAShiftLoom + nBShiftLoom + nCShiftLoom);
        //            int ttlShiftEff=  Convert.ToInt32((nAShiftEfficiency*nAShiftLoom + nBShiftEfficiency*nBShiftLoom + nCShiftEfficiency*nCShiftLoom));
        //            int nShiftEfficiencyAvg = (ttlLoomCount != 0) ?  Convert.ToInt32(ttlShiftEff / ttlLoomCount) : 0;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nShiftEfficiencyAvg; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            var nGreyTargetMtr = oTempItems.Sum(x => x.GreyTargetMtr);
        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nGreyTargetMtr; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            var nBeamQtyMtr = oTempItems.Sum(x => x.BeamQtyMtr);
        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nBeamQtyMtr; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            var nBalanceToWeaveMtr = oTempItems.Sum(x => x.BalanceToWeaveMtr);
        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nBalanceToWeaveMtr; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oTempItems[0].Remark; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            rowIndex++;
        //        }

        //        #endregion

        //        #region Yarn Dyed Sub Total
        //        colIndex = 2;
        //        color = Color.Yellow;
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Sub Total"; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        double nAShiftQtyMtrYD = oYarnDyedRptProductReports.Sum(x => x.AShiftQtyMtr);
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nAShiftQtyMtrYD; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        double nBShiftQtyMtrYD = oYarnDyedRptProductReports.Sum(x => x.BShiftQtyMtr);
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nBShiftQtyMtrYD; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        double nCShiftQtyMtrYD = oYarnDyedRptProductReports.Sum(x => x.CShiftQtyMtr);
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nCShiftQtyMtrYD; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        double nTotalShiftQtyMtrYD = nAShiftQtyMtrYD + nBShiftQtyMtrYD + nCShiftQtyMtrYD;
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nTotalShiftQtyMtrYD; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        double nTotalAShiftLoomEfficiencyYD = oYarnDyedRptProductReports.Sum(x => x.AShiftLoomEfficiency);
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        double nTotalAShiftEfficiencyYD = oYarnDyedRptProductReports.Sum(x => x.AShiftEfficiency);
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        double nTotalBShiftLoomEfficiencyYD = oYarnDyedRptProductReports.Sum(x => x.BShiftLoomEfficiency);
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        double nTotalBShiftEfficiencyYD = oYarnDyedRptProductReports.Sum(x => x.BShiftEfficiency);
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        double nTotalCShiftLoomEfficiencyYD = oYarnDyedRptProductReports.Sum(x => x.CShiftLoomEfficiency);
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        double nTotalCShiftEfficiencyYD = oYarnDyedRptProductReports.Sum(x => x.CShiftEfficiency);
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        double nTotalShiftEfficiencyAvgYD = (nTotalAShiftEfficiencyYD + nTotalBShiftEfficiencyYD + nTotalCShiftEfficiencyYD) / 3;
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        //cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        //cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        //cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        rowIndex++;
        //        #endregion
        //    }
        //    #endregion

        //    #region Solid Dyed
        //    if (oSolidDyedRptProductReports.Count > 0)
        //    {
        //        #region Solid dyed title
        //        color = Color.Gray;
        //        colIndex = 2;
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "EXE-S/D"; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        //cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        //cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        //cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        rowIndex++;
        //        #endregion

        //        #region Solid Dyed
        //        int nSL1 = 0;
        //        string[] sFEOIDs = string.Join(",", oSolidDyedRptProductReports.Select(x => x.FEOID).Distinct()).Split(',');
        //        foreach (string sFEOID in sFEOIDs)
        //        {
        //            nSL1++;
        //            oTempItems = new List<RptProductReport>();
        //            oTempItems = oSolidDyedRptProductReports.Where(x => x.FEOID == Convert.ToInt32(sFEOID)).ToList();

        //            colIndex = 2;
        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nSL1.ToString(); cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oTempItems[0].Construction; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oTempItems[0].BuyerName; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oTempItems[0].OrderNo; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oTempItems[0].FEOQty; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oTempItems[0].OrderReceiptDateSt; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oTempItems[0].AskingDeliveryDateSt; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oTempItems[0].FabricsType; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                  

        //            int nAShiftLoom = oTempItems.Sum(x => x.AShiftLoom);
        //            int nBShiftLoom = oTempItems.Sum(x => x.BShiftLoom);
        //            int nCShiftLoom = oTempItems.Sum(x => x.CShiftLoom);
        //            double nLoomRPMAvg = (nAShiftLoom + nBShiftLoom + nCShiftLoom > 0) ? Math.Round((double)(oTempItems[0].TtlRPM / (nAShiftLoom + nBShiftLoom + nCShiftLoom)), 2) : 0;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nLoomRPMAvg; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oTempItems[0].LoomRpmMaxMin; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                  
        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nAShiftLoom; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                   
        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nBShiftLoom; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                   
        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nCShiftLoom; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            int LoomAvgCount = 0;
        //            if (nAShiftLoom != 0)
        //                LoomAvgCount++;
        //            if (nBShiftLoom != 0)
        //                LoomAvgCount++;
        //            if (nCShiftLoom != 0)
        //                LoomAvgCount++;
        //            double nShiftLoomAvg = Math.Round((double)(nAShiftLoom + nBShiftLoom + nCShiftLoom) / LoomAvgCount,0);
        //            ttlAvgLoom += nShiftLoomAvg;
        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nShiftLoomAvg; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            var nAShiftQtyMtr = oTempItems.Sum(x => x.AShiftQtyMtr);
        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nAShiftQtyMtr; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            var nBShiftQtyMtr = oTempItems.Sum(x => x.BShiftQtyMtr);
        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nBShiftQtyMtr; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            var nCShiftQtyMtr = oTempItems.Sum(x => x.CShiftQtyMtr);
        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nCShiftQtyMtr; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            var nShiftQtyMtrTotal = oTempItems.Sum(x => x.ShiftQtyMtrTotal);
        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nShiftQtyMtrTotal; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            var nAShiftEfficiency = oTempItems.Sum(x => x.AShiftLoomEfficiency);
        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nAShiftEfficiency; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            var nBShiftEfficiency = oTempItems.Sum(x => x.BShiftLoomEfficiency);
        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nBShiftEfficiency; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            var nCShiftEfficiency = oTempItems.Sum(x => x.CShiftLoomEfficiency);
        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nCShiftEfficiency; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            int ttlLoomCount = Convert.ToInt32(nAShiftLoom + nBShiftLoom + nCShiftLoom);
        //            int ttlShiftEff=  Convert.ToInt32((nAShiftEfficiency*nAShiftLoom + nBShiftEfficiency*nBShiftLoom + nCShiftEfficiency*nCShiftLoom));
              

        //            int nShiftEfficiencyAvg = (ttlLoomCount != 0) ?  Convert.ToInt32(ttlShiftEff / ttlLoomCount) : 0;


        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nShiftEfficiencyAvg; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            var nGreyTargetMtr = oTempItems.Sum(x => x.GreyTargetMtr);
        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nGreyTargetMtr; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            var nBeamQtyMtr = oTempItems.Sum(x => x.BeamQtyMtr);
        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nBeamQtyMtr; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            var nBalanceToWeaveMtr = oTempItems.Sum(x => x.BalanceToWeaveMtr);
        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nBalanceToWeaveMtr; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oTempItems[0].Remark; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            rowIndex++;
        //        }
        //        #endregion

        //        #region Solid Dyed Sub Total
        //        colIndex = 2;
        //        color = Color.Yellow;
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Sub Total"; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        double nAShiftQtyMtrSD = oSolidDyedRptProductReports.Sum(x => x.AShiftQtyMtr);
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nAShiftQtyMtrSD; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        double nBShiftQtyMtrSD = oSolidDyedRptProductReports.Sum(x => x.BShiftQtyMtr);
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nBShiftQtyMtrSD; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        double nCShiftQtyMtrSD = oSolidDyedRptProductReports.Sum(x => x.CShiftQtyMtr);
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nCShiftQtyMtrSD; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        double nTotalShiftQtyMtrSD = nAShiftQtyMtrSD + nBShiftQtyMtrSD + nCShiftQtyMtrSD;
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nTotalShiftQtyMtrSD; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        double nTotalAShiftLoomEfficiencySD = oYarnDyedRptProductReports.Sum(x => x.AShiftLoomEfficiency);
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        double nTotalAShiftEfficiencySD = oYarnDyedRptProductReports.Sum(x => x.BShiftLoomEfficiency);
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        double nTotalBShiftLoomEfficiencySD = oYarnDyedRptProductReports.Sum(x => x.BShiftLoomEfficiency);
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        double nTotalBShiftEfficiencySD = oYarnDyedRptProductReports.Sum(x => x.BShiftEfficiency);
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        double nTotalCShiftLoomEfficiencySD = oYarnDyedRptProductReports.Sum(x => x.CShiftLoomEfficiency);
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        double nTotalCShiftEfficiencySD = oYarnDyedRptProductReports.Sum(x => x.CShiftEfficiency);
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        double nTotalShiftEfficiencyAvgSD = (nTotalAShiftEfficiencySD + nTotalBShiftEfficiencySD + nTotalCShiftEfficiencySD) / 3;
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        rowIndex++;
        //        #endregion
        //    }
        //    else
        //    {
        //        rowIndex++;
        //        cell = sheet.Cells[rowIndex, 2, rowIndex, 30]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = false;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
        //        rowIndex++;
        //    }
        //    #endregion

        //    #region Yarn Dyed For SCW
        //    if (oScwYarnDyedRptProductReports.Count > 0)
        //    {
        //        #region Solid dyed title
        //        color = Color.Gray;
        //        colIndex = 2;
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "SCW-Y/D"; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        rowIndex++;
        //        #endregion

        //        #region Yarn Dyed For SCW
        //        int nSL1 = 0;
        //        string[] sFEOIDs = string.Join(",", oScwYarnDyedRptProductReports.Select(x => x.FEOID).Distinct()).Split(',');
        //        foreach (string sFEOID in sFEOIDs)
        //        {
        //            nSL1++;
        //            oTempItems = new List<RptProductReport>();
        //            oTempItems = oScwYarnDyedRptProductReports.Where(x => x.FEOID == Convert.ToInt32(sFEOID)).ToList();

        //            colIndex = 2;
        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nSL1.ToString(); cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oTempItems[0].Construction; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oTempItems[0].BuyerName; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oTempItems[0].OrderNo; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oTempItems[0].FEOQty; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oTempItems[0].OrderReceiptDateSt; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oTempItems[0].AskingDeliveryDateSt; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oTempItems[0].FabricsType; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



        //            int nAShiftLoom = oTempItems.Sum(x => x.AShiftLoom);
        //            int nBShiftLoom = oTempItems.Sum(x => x.BShiftLoom);
        //            int nCShiftLoom = oTempItems.Sum(x => x.CShiftLoom);
        //            double nLoomRPMAvg = (nAShiftLoom + nBShiftLoom + nCShiftLoom > 0) ? Math.Round((double)(oTempItems[0].TtlRPM / (nAShiftLoom + nBShiftLoom + nCShiftLoom)), 2) : 0;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nLoomRPMAvg; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oTempItems[0].LoomRpmMaxMin; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nAShiftLoom; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nBShiftLoom; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nCShiftLoom; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            int LoomAvgCount = 0;
        //            if (nAShiftLoom != 0)
        //                LoomAvgCount++;
        //            if (nBShiftLoom != 0)
        //                LoomAvgCount++;
        //            if (nCShiftLoom != 0)
        //                LoomAvgCount++;
        //            double nShiftLoomAvg = Math.Round((double)(nAShiftLoom + nBShiftLoom + nCShiftLoom) / LoomAvgCount, 0);
        //            ttlAvgLoom += nShiftLoomAvg;
        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nShiftLoomAvg; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            var nAShiftQtyMtr = oTempItems.Sum(x => x.AShiftQtyMtr);
        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nAShiftQtyMtr; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            var nBShiftQtyMtr = oTempItems.Sum(x => x.BShiftQtyMtr);
        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nBShiftQtyMtr; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            var nCShiftQtyMtr = oTempItems.Sum(x => x.CShiftQtyMtr);
        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nCShiftQtyMtr; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            var nShiftQtyMtrTotal = oTempItems.Sum(x => x.ShiftQtyMtrTotal);
        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nShiftQtyMtrTotal; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            var nAShiftEfficiency = oTempItems.Sum(x => x.AShiftLoomEfficiency);
        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nAShiftEfficiency; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            var nBShiftEfficiency = oTempItems.Sum(x => x.BShiftLoomEfficiency);
        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nBShiftEfficiency; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            var nCShiftEfficiency = oTempItems.Sum(x => x.CShiftLoomEfficiency);
        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nCShiftEfficiency; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            int ttlLoomCount = Convert.ToInt32(nAShiftLoom + nBShiftLoom + nCShiftLoom);
        //            int ttlShiftEff = Convert.ToInt32((nAShiftEfficiency * nAShiftLoom + nBShiftEfficiency * nBShiftLoom + nCShiftEfficiency * nCShiftLoom));


        //            int nShiftEfficiencyAvg = (ttlLoomCount != 0) ? Convert.ToInt32(ttlShiftEff / ttlLoomCount) : 0;


        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nShiftEfficiencyAvg; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            var nGreyTargetMtr = oTempItems.Sum(x => x.GreyTargetMtr);
        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nGreyTargetMtr; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            var nBeamQtyMtr = oTempItems.Sum(x => x.BeamQtyMtr);
        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nBeamQtyMtr; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            var nBalanceToWeaveMtr = oTempItems.Sum(x => x.BalanceToWeaveMtr);
        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nBalanceToWeaveMtr; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oTempItems[0].Remark; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            rowIndex++;
        //        }
        //        #endregion

        //        #region Yarn Dyed SCW Sub Total
        //        colIndex = 2;
        //        color = Color.Yellow;
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Sub Total"; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        double nAShiftQtyMtrSD = oScwYarnDyedRptProductReports.Sum(x => x.AShiftQtyMtr);
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nAShiftQtyMtrSD; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        double nBShiftQtyMtrSD = oScwYarnDyedRptProductReports.Sum(x => x.BShiftQtyMtr);
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nBShiftQtyMtrSD; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        double nCShiftQtyMtrSD = oScwYarnDyedRptProductReports.Sum(x => x.CShiftQtyMtr);
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nCShiftQtyMtrSD; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        double nTotalShiftQtyMtrSD = nAShiftQtyMtrSD + nBShiftQtyMtrSD + nCShiftQtyMtrSD;
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nTotalShiftQtyMtrSD; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        double nTotalAShiftLoomEfficiencySD = oScwYarnDyedRptProductReports.Sum(x => x.AShiftLoomEfficiency);
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        double nTotalAShiftEfficiencySD = oScwYarnDyedRptProductReports.Sum(x => x.BShiftLoomEfficiency);
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        double nTotalBShiftLoomEfficiencySD = oScwYarnDyedRptProductReports.Sum(x => x.BShiftLoomEfficiency);
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        double nTotalBShiftEfficiencySD = oScwYarnDyedRptProductReports.Sum(x => x.BShiftEfficiency);
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        double nTotalCShiftLoomEfficiencySD = oScwYarnDyedRptProductReports.Sum(x => x.CShiftLoomEfficiency);
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        double nTotalCShiftEfficiencySD = oScwYarnDyedRptProductReports.Sum(x => x.CShiftEfficiency);
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        double nTotalShiftEfficiencyAvgSD = (nTotalAShiftEfficiencySD + nTotalBShiftEfficiencySD + nTotalCShiftEfficiencySD) / 3;
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        rowIndex++;
        //        #endregion
        //    }
        //    else
        //    {
        //        rowIndex++;
        //        cell = sheet.Cells[rowIndex, 2, rowIndex, 30]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = false;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
        //        rowIndex++;
        //    }
        //    #endregion

        //    #region Solid Dyed For SCW
        //    if (oScwSolidDyedRptProductReports.Count > 0)
        //    {
        //        #region Solid dyed title
        //        color = Color.Gray;
        //        colIndex = 2;
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "SCW-S/D"; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        rowIndex++;
        //        #endregion

        //        #region Solid Dyed For SCW
        //        int nSL1 = 0;
        //        string[] sFEOIDs = string.Join(",", oScwSolidDyedRptProductReports.Select(x => x.FEOID).Distinct()).Split(',');
        //        foreach (string sFEOID in sFEOIDs)
        //        {
        //            nSL1++;
        //            oTempItems = new List<RptProductReport>();
        //            oTempItems = oScwSolidDyedRptProductReports.Where(x => x.FEOID == Convert.ToInt32(sFEOID)).ToList();

        //            colIndex = 2;
        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nSL1.ToString(); cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oTempItems[0].Construction; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oTempItems[0].BuyerName; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oTempItems[0].OrderNo; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oTempItems[0].FEOQty; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oTempItems[0].OrderReceiptDateSt; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oTempItems[0].AskingDeliveryDateSt; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oTempItems[0].FabricsType; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



        //            int nAShiftLoom = oTempItems.Sum(x => x.AShiftLoom);
        //            int nBShiftLoom = oTempItems.Sum(x => x.BShiftLoom);
        //            int nCShiftLoom = oTempItems.Sum(x => x.CShiftLoom);
        //            double nLoomRPMAvg = (nAShiftLoom + nBShiftLoom + nCShiftLoom > 0) ? Math.Round((double)(oTempItems[0].TtlRPM / (nAShiftLoom + nBShiftLoom + nCShiftLoom)), 2) : 0;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nLoomRPMAvg; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oTempItems[0].LoomRpmMaxMin; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nAShiftLoom; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nBShiftLoom; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nCShiftLoom; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            int LoomAvgCount = 0;
        //            if (nAShiftLoom != 0)
        //                LoomAvgCount++;
        //            if (nBShiftLoom != 0)
        //                LoomAvgCount++;
        //            if (nCShiftLoom != 0)
        //                LoomAvgCount++;
        //            double nShiftLoomAvg = Math.Round((double)(nAShiftLoom + nBShiftLoom + nCShiftLoom) / LoomAvgCount, 0);
        //            ttlAvgLoom += nShiftLoomAvg;
        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nShiftLoomAvg; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            var nAShiftQtyMtr = oTempItems.Sum(x => x.AShiftQtyMtr);
        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nAShiftQtyMtr; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            var nBShiftQtyMtr = oTempItems.Sum(x => x.BShiftQtyMtr);
        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nBShiftQtyMtr; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            var nCShiftQtyMtr = oTempItems.Sum(x => x.CShiftQtyMtr);
        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nCShiftQtyMtr; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            var nShiftQtyMtrTotal = oTempItems.Sum(x => x.ShiftQtyMtrTotal);
        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nShiftQtyMtrTotal; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            var nAShiftEfficiency = oTempItems.Sum(x => x.AShiftLoomEfficiency);
        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nAShiftEfficiency; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            var nBShiftEfficiency = oTempItems.Sum(x => x.BShiftLoomEfficiency);
        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nBShiftEfficiency; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            var nCShiftEfficiency = oTempItems.Sum(x => x.CShiftLoomEfficiency);
        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nCShiftEfficiency; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            int ttlLoomCount = Convert.ToInt32(nAShiftLoom + nBShiftLoom + nCShiftLoom);
        //            int ttlShiftEff = Convert.ToInt32((nAShiftEfficiency * nAShiftLoom + nBShiftEfficiency * nBShiftLoom + nCShiftEfficiency * nCShiftLoom));


        //            int nShiftEfficiencyAvg = (ttlLoomCount != 0) ? Convert.ToInt32(ttlShiftEff / ttlLoomCount) : 0;


        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nShiftEfficiencyAvg; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            var nGreyTargetMtr = oTempItems.Sum(x => x.GreyTargetMtr);
        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nGreyTargetMtr; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            var nBeamQtyMtr = oTempItems.Sum(x => x.BeamQtyMtr);
        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nBeamQtyMtr; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            var nBalanceToWeaveMtr = oTempItems.Sum(x => x.BalanceToWeaveMtr);
        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nBalanceToWeaveMtr; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oTempItems[0].Remark; cell.Style.Font.Bold = false;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            rowIndex++;
        //        }
        //        #endregion

        //        #region Solid Dyed SCW Sub Total
        //        colIndex = 2;
        //        color = Color.Yellow;
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Sub Total"; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        double nAShiftQtyMtrSD = oScwSolidDyedRptProductReports.Sum(x => x.AShiftQtyMtr);
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nAShiftQtyMtrSD; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        double nBShiftQtyMtrSD = oScwSolidDyedRptProductReports.Sum(x => x.BShiftQtyMtr);
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nBShiftQtyMtrSD; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        double nCShiftQtyMtrSD = oScwSolidDyedRptProductReports.Sum(x => x.CShiftQtyMtr);
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nCShiftQtyMtrSD; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        double nTotalShiftQtyMtrSD = nAShiftQtyMtrSD + nBShiftQtyMtrSD + nCShiftQtyMtrSD;
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nTotalShiftQtyMtrSD; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        double nTotalAShiftLoomEfficiencySD = oScwSolidDyedRptProductReports.Sum(x => x.AShiftLoomEfficiency);
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        double nTotalAShiftEfficiencySD = oScwSolidDyedRptProductReports.Sum(x => x.BShiftLoomEfficiency);
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        double nTotalBShiftLoomEfficiencySD = oScwSolidDyedRptProductReports.Sum(x => x.BShiftLoomEfficiency);
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        double nTotalBShiftEfficiencySD = oScwSolidDyedRptProductReports.Sum(x => x.BShiftEfficiency);
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        double nTotalCShiftLoomEfficiencySD = oScwSolidDyedRptProductReports.Sum(x => x.CShiftLoomEfficiency);
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        double nTotalCShiftEfficiencySD = oScwSolidDyedRptProductReports.Sum(x => x.CShiftEfficiency);
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        double nTotalShiftEfficiencyAvgSD = (nTotalAShiftEfficiencySD + nTotalBShiftEfficiencySD + nTotalCShiftEfficiencySD) / 3;
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        rowIndex++;
        //        #endregion
        //    }
        //    else
        //    {
        //        rowIndex++;
        //        cell = sheet.Cells[rowIndex, 2, rowIndex, 30]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = false;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
        //        rowIndex++;
        //    }
        //    #endregion

        //    #region Grand Calculation (Green Part)


        //    color = Color.LightGreen;
        //    #region Row 01
        //    colIndex = 2;
        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    var nAShiftLoomTotal = oYarnDyedRptProductReports.Sum(x => x.AShiftLoom) + oSolidDyedRptProductReports.Sum(x => x.AShiftLoom);
        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nAShiftLoomTotal; cell.Style.Font.Bold = false;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    var nBShiftLoomTotal = oYarnDyedRptProductReports.Sum(x => x.BShiftLoom) + oSolidDyedRptProductReports.Sum(x => x.BShiftLoom);
        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nBShiftLoomTotal; cell.Style.Font.Bold = false;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    var nCShiftLoomTotal = oYarnDyedRptProductReports.Sum(x => x.CShiftLoom) + oSolidDyedRptProductReports.Sum(x => x.CShiftLoom);
        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nCShiftLoomTotal; cell.Style.Font.Bold = false;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    var nShiftLoomAvgTotal = Math.Round(Convert.ToDouble((nAShiftLoomTotal + nBShiftLoomTotal + nCShiftLoomTotal) / 3));
        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round(ttlAvgLoom,0); cell.Style.Font.Bold = false;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


        //    var nAShiftQtyMtrTotal = oYarnDyedRptProductReports.Sum(x => x.AShiftQtyMtr) + oSolidDyedRptProductReports.Sum(x => x.AShiftQtyMtr) + oScwYarnDyedRptProductReports.Sum(x => x.AShiftQtyMtr) + oScwSolidDyedRptProductReports.Sum(x => x.AShiftQtyMtr);
        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round((double)nAShiftQtyMtrTotal,0); cell.Style.Font.Bold = false;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    var nBShiftQtyMtrTotal = oYarnDyedRptProductReports.Sum(x => x.BShiftQtyMtr) + oSolidDyedRptProductReports.Sum(x => x.BShiftQtyMtr) + oScwYarnDyedRptProductReports.Sum(x => x.BShiftQtyMtr) + oScwSolidDyedRptProductReports.Sum(x => x.BShiftQtyMtr);
        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round((double)nBShiftQtyMtrTotal, 0); cell.Style.Font.Bold = false;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    var nCShiftQtyMtrTotal = oYarnDyedRptProductReports.Sum(x => x.CShiftQtyMtr) + oSolidDyedRptProductReports.Sum(x => x.CShiftQtyMtr) + oScwYarnDyedRptProductReports.Sum(x => x.CShiftQtyMtr) + oScwSolidDyedRptProductReports.Sum(x => x.CShiftQtyMtr);
        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round((double)nCShiftQtyMtrTotal,0); cell.Style.Font.Bold = false;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round((double)(nAShiftQtyMtrTotal + nBShiftQtyMtrTotal+nCShiftQtyMtrTotal), 0); cell.Style.Font.Bold = false;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    var nAShiftLoomEfficiencyTotal = (oYarnDyedRptProductReports.Sum(x => x.AShiftLoomEfficiency) + oSolidDyedRptProductReports.Sum(x => x.AShiftLoomEfficiency)) / 2;
        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    var nAShiftEfficiencyTotal = (oYarnDyedRptProductReports.Sum(x => x.AShiftEfficiency) + oSolidDyedRptProductReports.Sum(x => x.AShiftEfficiency)) / 2;
        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    var nBShiftLoomEfficiencyTotal = (oYarnDyedRptProductReports.Sum(x => x.BShiftLoomEfficiency) + oSolidDyedRptProductReports.Sum(x => x.BShiftLoomEfficiency)) / 2;
        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    var nBShiftEfficiencyTotal = (oYarnDyedRptProductReports.Sum(x => x.BShiftEfficiency) + oSolidDyedRptProductReports.Sum(x => x.BShiftEfficiency)) / 2;
        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    var nCShiftLoomEfficiencyTotal = (oYarnDyedRptProductReports.Sum(x => x.CShiftLoomEfficiency) + oSolidDyedRptProductReports.Sum(x => x.CShiftLoomEfficiency)) / 2;
        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    var nCShiftEfficiencyTotal = (oYarnDyedRptProductReports.Sum(x => x.CShiftEfficiency) + oSolidDyedRptProductReports.Sum(x => x.CShiftEfficiency)) / 2;
        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    var nShiftEfficiencyAvgTotal = (oYarnDyedRptProductReports.Sum(x => x.ShiftEfficiencyAvg) + oSolidDyedRptProductReports.Sum(x => x.ShiftEfficiencyAvg)) / 2;
        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //    rowIndex++;
        //    #endregion

        //    #region Row 02
        //    colIndex = 2;
        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    colIndex = colIndex + 2;
        //    cell = sheet.Cells[rowIndex, colIndex - 2, rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Actual Avg. run Loom "; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value =Math.Round( ttlAvgLoom,0); cell.Style.Font.Bold = false;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    colIndex = colIndex + 2;
        //    cell = sheet.Cells[rowIndex, colIndex - 2, rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Total Production In(mtr.) : "; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round((double)(nAShiftQtyMtrTotal + nBShiftQtyMtrTotal + nCShiftQtyMtrTotal), 0); cell.Style.Font.Bold = false;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(color);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //   rowIndex++;
        //    #endregion

        //    #endregion
           
        //    #region Summery PRoduction
        //    rowIndex = rowIndex + 2;
        //    colIndex = 3;

        //    #region FirstHeader
        //    cell = sheet.Cells[rowIndex, colIndex];
        //    cell.Value = "Summery";
        //    cell.Style.Font.Bold = true;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    fill = cell.Style.Fill;
        //    fill.PatternType = ExcelFillStyle.Solid;
        //    fill.BackgroundColor.SetColor(Color.LimeGreen);
        //    border = cell.Style.Border;
        //    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, 4, rowIndex, 7];
        //    cell.Merge = true;
        //    cell.Value = "Production";
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.Font.Bold = true;
        //    fill = cell.Style.Fill;
        //    fill.PatternType = ExcelFillStyle.Solid;
        //    fill.BackgroundColor.SetColor(Color.LimeGreen);
        //    border = cell.Style.Border;
        //    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, 8, rowIndex, 11];
        //    cell.Merge = true;
        //    cell.Value = "Efficiency";
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.Font.Bold = true;
        //    fill = cell.Style.Fill;
        //    fill.PatternType = ExcelFillStyle.Solid;
        //    fill.BackgroundColor.SetColor(Color.LimeGreen);
        //    border = cell.Style.Border;
        //    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, 12, rowIndex, 15];
        //    cell.Merge = true;
        //    cell.Value = "RPM";
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.Font.Bold = true;
        //    fill = cell.Style.Fill;
        //    fill.PatternType = ExcelFillStyle.Solid;
        //    fill.BackgroundColor.SetColor(Color.LimeGreen);
        //    border = cell.Style.Border;
        //    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, 16, rowIndex, 19];
        //    cell.Merge = true;
        //    cell.Value = "Run Loom";
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.Font.Bold = true;
        //    fill = cell.Style.Fill;
        //    fill.PatternType = ExcelFillStyle.Solid;
        //    fill.BackgroundColor.SetColor(Color.LimeGreen);
        //    border = cell.Style.Border;
        //    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, 20, rowIndex+1, 20];
        //    cell.Merge = true;
        //    cell.Value = "Avg. Pick";
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.Font.Bold = true;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
        //    fill.BackgroundColor.SetColor(Color.LimeGreen);
        //    border = cell.Style.Border;
        //    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


        //    rowIndex++;
        //    #endregion

        //    #region SecondHeader
        //    cell = sheet.Cells[rowIndex, colIndex];
        //    cell.Value = "Shift";
        //    cell.Style.Font.Bold = true;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    fill = cell.Style.Fill;
        //    fill.PatternType = ExcelFillStyle.Solid;
        //    fill.BackgroundColor.SetColor(Color.LimeGreen);
        //    border = cell.Style.Border;
        //    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex,++colIndex];
        //    cell.Value = "A";
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.Font.Bold = true;
        //    fill = cell.Style.Fill;
        //    fill.PatternType = ExcelFillStyle.Solid;
        //    fill.BackgroundColor.SetColor(Color.LimeGreen);
        //    border = cell.Style.Border;
        //    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, ++colIndex];
        //    cell.Value = "B";
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.Font.Bold = true;
        //    fill = cell.Style.Fill;
        //    fill.PatternType = ExcelFillStyle.Solid;
        //    fill.BackgroundColor.SetColor(Color.LimeGreen);
        //    border = cell.Style.Border;
        //    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, ++colIndex];
        //    cell.Value = "C";
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.Font.Bold = true;
        //    fill = cell.Style.Fill;
        //    fill.PatternType = ExcelFillStyle.Solid;
        //    fill.BackgroundColor.SetColor(Color.LimeGreen);
        //    border = cell.Style.Border;
        //    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, ++colIndex];
        //    cell.Value = "Total";
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.Font.Bold = true;
        //    fill = cell.Style.Fill;
        //    fill.PatternType = ExcelFillStyle.Solid;
        //    fill.BackgroundColor.SetColor(Color.LimeGreen);
        //    border = cell.Style.Border;
        //    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, ++colIndex];
        //    cell.Value = "A";
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.Font.Bold = true;
        //    fill = cell.Style.Fill;
        //    fill.PatternType = ExcelFillStyle.Solid;
        //    fill.BackgroundColor.SetColor(Color.LimeGreen);
        //    border = cell.Style.Border;
        //    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, ++colIndex];
        //    cell.Value = "B";
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.Font.Bold = true;
        //    fill = cell.Style.Fill;
        //    fill.PatternType = ExcelFillStyle.Solid;
        //    fill.BackgroundColor.SetColor(Color.LimeGreen);
        //    border = cell.Style.Border;
        //    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, ++colIndex];
        //    cell.Value = "C";
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.Font.Bold = true;
        //    fill = cell.Style.Fill;
        //    fill.PatternType = ExcelFillStyle.Solid;
        //    fill.BackgroundColor.SetColor(Color.LimeGreen);
        //    border = cell.Style.Border;
        //    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, ++colIndex];
        //    cell.Value = "Avg.";
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.Font.Bold = true;
        //    fill = cell.Style.Fill;
        //    fill.PatternType = ExcelFillStyle.Solid;
        //    fill.BackgroundColor.SetColor(Color.LimeGreen);
        //    border = cell.Style.Border;
        //    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, ++colIndex];
        //    cell.Value = "A";
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.Font.Bold = true;
        //    fill = cell.Style.Fill;
        //    fill.PatternType = ExcelFillStyle.Solid;
        //    fill.BackgroundColor.SetColor(Color.LimeGreen);
        //    border = cell.Style.Border;
        //    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, ++colIndex];
        //    cell.Value = "B";
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.Font.Bold = true;
        //    fill = cell.Style.Fill;
        //    fill.PatternType = ExcelFillStyle.Solid;
        //    fill.BackgroundColor.SetColor(Color.LimeGreen);
        //    border = cell.Style.Border;
        //    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, ++colIndex];
        //    cell.Value = "C";
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.Font.Bold = true;
        //    fill = cell.Style.Fill;
        //    fill.PatternType = ExcelFillStyle.Solid;
        //    fill.BackgroundColor.SetColor(Color.LimeGreen);
        //    border = cell.Style.Border;
        //    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, ++colIndex];
        //    cell.Value = "Avg.";
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.Font.Bold = true;
        //    fill = cell.Style.Fill;
        //    fill.PatternType = ExcelFillStyle.Solid;
        //    fill.BackgroundColor.SetColor(Color.LimeGreen);
        //    border = cell.Style.Border;
        //    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, ++colIndex];
        //    cell.Value = "A";
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.Font.Bold = true;
        //    fill = cell.Style.Fill;
        //    fill.PatternType = ExcelFillStyle.Solid;
        //    fill.BackgroundColor.SetColor(Color.LimeGreen);
        //    border = cell.Style.Border;
        //    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, ++colIndex];
        //    cell.Value = "B";
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.Font.Bold = true;
        //    fill = cell.Style.Fill;
        //    fill.PatternType = ExcelFillStyle.Solid;
        //    fill.BackgroundColor.SetColor(Color.LimeGreen);
        //    border = cell.Style.Border;
        //    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, ++colIndex];
        //    cell.Value = "C";
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.Font.Bold = true;
        //    fill = cell.Style.Fill;
        //    fill.PatternType = ExcelFillStyle.Solid;
        //    fill.BackgroundColor.SetColor(Color.LimeGreen);
        //    border = cell.Style.Border;
        //    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, ++colIndex];
        //    cell.Value = "Avg.";
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.Font.Bold = true;
        //    fill = cell.Style.Fill;
        //    fill.PatternType = ExcelFillStyle.Solid;
        //    fill.BackgroundColor.SetColor(Color.LimeGreen);
        //    border = cell.Style.Border;
        //    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    rowIndex++;

        //    #endregion

        //    #region Summery Body
        //    int TempRowIndex = rowIndex;
        //    int tempLstRowindex = 0;
        //    double grandtotalQty = 0;
        //    double grandTotalLoom = 0;
        //    double grandTotalEff=0;
        //    double grandTotalRPM =0;
        //    double ttlLoom=0;
        //    double ttlAShiftQty = 0;
        //    double ttlBShiftQty = 0;
        //    double ttlCShiftQty = 0;
        //    double ttlAShiftLoom = 0;
        //    double ttlBShiftLoom = 0;
        //    double ttlCShiftLoom = 0;
        //    double ttlAShiftEff = 0;
        //    double ttlBShiftEff = 0;
        //    double ttlCShiftEff = 0;
        //    double ttlAshiftRPM = 0;
        //    double ttlBshiftRPM = 0;
        //    double ttlCshiftRPM = 0;
          
        //    foreach(var oitem in oDynamicLoomProductionSumms)
        //    {
        //        IDictionary<string, object> dic = oitem;
        //        colIndex = 2;
        //        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = dic["MachineName"]; cell.Style.Font.Bold = false;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        double nAshiftProdQty =Convert.ToDouble(dic["AShiftProdQty"]);
        //        ttlAShiftQty += nAshiftProdQty;
        //        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = Global.GetMeter(nAshiftProdQty, 2); cell.Style.Font.Bold = false;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        double nBShiftProdQty = Convert.ToDouble(dic["BShiftProdQty"]);
        //        ttlBShiftQty += nBShiftProdQty;
        //        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = Global.GetMeter(nBShiftProdQty, 2); cell.Style.Font.Bold = false;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        double nCShiftProdQty = Convert.ToDouble(dic["CShiftProdQty"]);
        //        ttlCShiftQty += nCShiftProdQty;
        //        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = Global.GetMeter(nCShiftProdQty, 2); cell.Style.Font.Bold = false;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        double totalQty = nAshiftProdQty + nBShiftProdQty + nCShiftProdQty;
        //        grandtotalQty+=totalQty;
        //        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = Global.GetMeter(totalQty, 2); cell.Style.Font.Bold = false;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = dic["AShiftLoomEfficiency"]; cell.Style.Font.Bold = false;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = dic["BShiftLoomEfficiency"]; cell.Style.Font.Bold = false;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = dic["CShiftLoomEfficiency"]; cell.Style.Font.Bold = false;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        double ttlLoomCount = Convert.ToDouble(dic["AShiftLoom"]) + Convert.ToDouble(dic["BShiftLoom"]) + Convert.ToDouble(dic["CShiftLoom"]);

        //        double avgLoomEff = (ttlLoomCount > 0) ? Math.Round((Convert.ToDouble(dic["AShiftLoomEfficiency"]) * Convert.ToDouble(dic["AShiftLoom"]) + Convert.ToDouble(dic["BShiftLoomEfficiency"]) * Convert.ToDouble(dic["BShiftLoom"]) + Convert.ToDouble(dic["CShiftLoomEfficiency"]) * Convert.ToDouble(dic["CShiftLoom"])) / ttlLoomCount, 2) : 0;
              
                
        //       // grandTotalEff += Convert.ToDouble(dic["AShiftLoomEfficiency"]) + Convert.ToDouble(dic["BShiftLoomEfficiency"]) + Convert.ToDouble(dic["CShiftLoomEfficiency"]);
                

        //        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = avgLoomEff; cell.Style.Font.Bold = false;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        double AshiftRPM = 0, BshiftRPM = 0, CshiftRPM=0;
        //        if (Convert.ToDouble(dic["AShiftLoom"]) != 0)
        //            AshiftRPM = Math.Round((Convert.ToDouble(Convert.ToDouble(dic["AShiftRPM"]) / (Convert.ToDouble(dic["AShiftLoom"])))));
        //        if (Convert.ToDouble(dic["BShiftLoom"]) != 0)
        //            BshiftRPM = Math.Round((Convert.ToDouble(Convert.ToDouble(dic["BShiftRPM"]) / (Convert.ToDouble(dic["BShiftLoom"])))));
        //        if (Convert.ToDouble(dic["CShiftLoom"]) != 0)
        //            CshiftRPM = Math.Round((Convert.ToDouble(Convert.ToDouble(dic["CShiftRPM"]) / (Convert.ToDouble(dic["CShiftLoom"])))));
        //        grandTotalRPM += Convert.ToDouble(dic["AShiftRPM"]) + Convert.ToDouble(dic["BShiftRPM"]) + Convert.ToDouble(dic["CShiftRPM"]);

        //        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = AshiftRPM; cell.Style.Font.Bold = false;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = BshiftRPM; cell.Style.Font.Bold = false;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = CshiftRPM; cell.Style.Font.Bold = false;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


        //        double avgLoomRPM = (ttlLoomCount > 0) ? Math.Round((Convert.ToDouble(dic["AShiftRPM"]) + Convert.ToDouble(dic["BShiftRPM"]) + Convert.ToDouble(dic["CShiftRPM"])) / ttlLoomCount, 2) : 0;
        //        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = avgLoomRPM; cell.Style.Font.Bold = false;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        ttlAShiftLoom +=Convert.ToDouble(dic["AShiftLoom"]);
        //        ttlBShiftLoom += Convert.ToDouble(dic["BShiftLoom"]);
        //        ttlCShiftLoom += Convert.ToDouble(dic["CShiftLoom"]);

        //        ttlAShiftEff += (Convert.ToDouble(dic["AShiftLoom"]) * Convert.ToDouble(dic["AShiftLoomEfficiency"]));
        //        ttlBShiftEff += (Convert.ToDouble(dic["BShiftLoom"]) * Convert.ToDouble(dic["BShiftLoomEfficiency"]));
        //        ttlCShiftEff += (Convert.ToDouble(dic["CShiftLoom"]) * Convert.ToDouble(dic["CShiftLoomEfficiency"]));

        //        ttlAshiftRPM += Convert.ToDouble(dic["AShiftRPM"]);
        //        ttlBshiftRPM += Convert.ToDouble(dic["BShiftRPM"]);
        //        ttlCshiftRPM += Convert.ToDouble(dic["CShiftRPM"]);

        //        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = dic["AShiftLoom"]; cell.Style.Font.Bold = false;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = dic["BShiftLoom"]; cell.Style.Font.Bold = false;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = dic["CShiftLoom"]; cell.Style.Font.Bold = false;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //        int LoomAvgCount = 0;
        //        if (Convert.ToDouble(dic["AShiftLoom"]) != 0)
        //            LoomAvgCount++;
        //        if (Convert.ToDouble(dic["BShiftLoom"]) != 0)
        //            LoomAvgCount++;
        //        if (Convert.ToDouble(dic["CShiftLoom"]) != 0)
        //            LoomAvgCount++;
           
        //        double avgLoomRun = Math.Round((Convert.ToDouble(dic["AShiftLoom"]) + Convert.ToDouble(dic["BShiftLoom"]) + Convert.ToDouble(dic["CShiftLoom"])) / LoomAvgCount, 0);
        //        grandTotalLoom += avgLoomRun;

        //        ttlLoom+=(Convert.ToDouble(dic["AShiftLoom"]) + Convert.ToDouble(dic["BShiftLoom"]) + Convert.ToDouble(dic["CShiftLoom"]));
        //        grandTotalEff += Convert.ToDouble(dic["AShiftLoom"]) * Convert.ToDouble(dic["AShiftLoomEfficiency"]) + Convert.ToDouble(dic["BShiftLoom"]) * Convert.ToDouble(dic["BShiftLoomEfficiency"]) + Convert.ToDouble(dic["CShiftLoom"]) * Convert.ToDouble(dic["CShiftLoomEfficiency"]);
        //        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = avgLoomRun; cell.Style.Font.Bold = false;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //        tempLstRowindex = rowIndex;
             
        //        rowIndex++;
        //    }
        //    if (oRptProductReports.Any())
        //    {
               
        //        cell = sheet.Cells[TempRowIndex, 20, tempLstRowindex, 20];
        //        cell.Merge = true;
        //        cell.Value = Math.Round(Convert.ToDouble(oRptProductReports.Sum(x => x.PPI)) /( oRptProductReports).Count(), 0);
        //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //        cell.Style.Font.Bold = true;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
        //        fill.BackgroundColor.SetColor(Color.White);
        //        border = cell.Style.Border;
        //        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //    }
        //    #endregion
        
        //    colIndex = 2;
        //    cell = sheet.Cells[rowIndex, 3]; cell.Value = "Grand Total"; cell.Style.Font.Bold = false;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGreen);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, 4, rowIndex, 4]; cell.Value = Math.Round(Global.GetMeter(ttlAShiftQty, 2), 0); cell.Style.Font.Bold = false;
        //    cell.Merge = true;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGreen);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, 5, rowIndex, 5]; cell.Value = Math.Round(Global.GetMeter(ttlBShiftQty, 2), 0); cell.Style.Font.Bold = false;
        //    cell.Merge = true;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGreen);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, 6, rowIndex, 6]; cell.Value = Math.Round(Global.GetMeter(ttlCShiftQty, 2), 0); cell.Style.Font.Bold = false;
        //    cell.Merge = true;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGreen);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
           
             
        //    cell = sheet.Cells[rowIndex, 7 ,rowIndex,7]; cell.Value =Math.Round( Global.GetMeter(grandtotalQty, 2),0); cell.Style.Font.Bold = false;
        //    cell.Merge = true;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGreen);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, 8, rowIndex, 8]; cell.Value = (ttlAShiftLoom != 0) ? Math.Round(ttlAShiftEff / ttlAShiftLoom) : 0; cell.Style.Font.Bold = false;
        //    cell.Merge = true;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGreen);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, 9, rowIndex, 9]; cell.Value = (ttlBShiftLoom != 0) ? Math.Round(ttlBShiftEff / ttlBShiftLoom) : 0; cell.Style.Font.Bold = false;
        //    cell.Merge = true;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGreen);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, 10, rowIndex, 10]; cell.Value = (ttlCShiftLoom != 0) ? Math.Round(ttlCShiftEff / ttlCShiftLoom) : 0; cell.Style.Font.Bold = false;
        //    cell.Merge = true;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGreen);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


        //    cell = sheet.Cells[rowIndex, 11, rowIndex, 11]; cell.Value =(ttlLoom!=0)? Math.Round(grandTotalEff/ttlLoom):0; cell.Style.Font.Bold = false;
        //    cell.Merge = true;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGreen);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, 12, rowIndex, 12]; cell.Value = (ttlAShiftLoom != 0) ? Math.Round(ttlAshiftRPM / ttlAShiftLoom) : 0; cell.Style.Font.Bold = false;
        //    cell.Merge = true;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGreen);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, 13, rowIndex, 13]; cell.Value = (ttlBShiftLoom != 0) ? Math.Round(ttlBshiftRPM / ttlBShiftLoom) : 0; cell.Style.Font.Bold = false;
        //    cell.Merge = true;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGreen);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, 14, rowIndex, 14]; cell.Value = (ttlCShiftLoom != 0) ? Math.Round(ttlCshiftRPM / ttlCShiftLoom) : 0; cell.Style.Font.Bold = false;
        //    cell.Merge = true;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGreen);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, 15, rowIndex, 15]; cell.Value =(ttlLoom!=0)? Math.Round(grandTotalRPM/ttlLoom):0; cell.Style.Font.Bold = false;
        //    cell.Merge = true;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGreen);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, 16, rowIndex, 16]; cell.Value = Math.Round(ttlAShiftLoom); cell.Style.Font.Bold = false;
        //    cell.Merge = true;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGreen);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, 17, rowIndex, 17]; cell.Value = Math.Round(ttlBShiftLoom); cell.Style.Font.Bold = false;
        //    cell.Merge = true;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGreen);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, 18, rowIndex, 18]; cell.Value = Math.Round(ttlCShiftLoom); cell.Style.Font.Bold = false;
        //    cell.Merge = true;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGreen);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, 19, rowIndex, 19]; cell.Value =Math.Round(grandTotalLoom); cell.Style.Font.Bold = false;
        //    cell.Merge = true;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGreen);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, 20]; cell.Value = ""; cell.Style.Font.Bold = false;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGreen);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
           
        //    rowIndex++;
        //    #endregion
           
        //    return excelPackage;
        //}

        //public object[] MailDailyProductionReport()
        //{
        //    object[] objArr = new object[1];
        //    var excelPackage = GenerateDailyProductionReportInExcel(0, DateTime.Today.ToString("dd MMM yyyy"), true);

        //    Attachment oAttachment = null;
        //    List<Attachment> oAttachments = new List<Attachment>();

        //    Stream stream = null;
        //    stream = new MemoryStream(excelPackage.GetAsByteArray());
        //    ContentType ct = new ContentType("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        //    oAttachment = new Attachment(stream, ct);
        //    oAttachment.Name = "DailyProductionReport(" + DateTime.Today.ToString("dd MMM yyyy") + ").xlsx";
        //    oAttachments.Add(oAttachment);

        //    objArr[0] = oAttachments;
        //    return objArr;
        //}

        
        #endregion

        #region Daily Log Report
        //public ActionResult View_DailyLogReport(int menuid)
        //{
        //    this.Session.Remove(SessionInfo.MenuID);
        //    this.Session.Add(SessionInfo.MenuID, menuid);
        //    List<RptDailyLogReport> oRptDailyLogReports = new List<RptDailyLogReport>();

        //    List<FabricProcess> oFabricProcesss = new List<FabricProcess>();
        //    oFabricProcesss = FabricProcess.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);

        //    ViewBag.FinishTypes = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Finish).ToList();
        //    ViewBag.ProcessTypes = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Process).ToList();
        //    ViewBag.FabricWeaves = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Weave).ToList();

        //    string sSQL = "Select * from View_TextileSubUnit Where ISNULL(InactiveBy,0)=0";
        //    List<TextileSubUnit> oTextileSubUnits = new List<TextileSubUnit>();
        //    oTextileSubUnits = TextileSubUnit.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    ViewBag.TextileSubUnits = oTextileSubUnits;

        //    return View(oRptDailyLogReports);
        //}

        //public void ExcelDailyLogReportDayWise(int tsuid, string sParams)
        //{
        //    using (var excelPackage = DailyLogsReportInExcelDayWise(tsuid, sParams, false))
        //    {
        //        Response.ClearContent();
        //        Response.BinaryWrite(excelPackage.GetAsByteArray());
        //        Response.AddHeader("content-disposition", "attachment; filename=DailyLogReport.xlsx");
        //        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //        Response.Flush();
        //        Response.End();

        //    }
        //}

        //public ExcelPackage DailyLogsReportInExcelDayWise(int tsuid, string sParams, bool bIsMail)
        //{
        //    //Log Report Production Summery Will be calculated 01 date back as per ATML Req.(05 Apr 2018)
        //    long nUserID = (bIsMail) ? 0 : ((User)Session[SessionInfo.CurrentUser]).UserID;
        //    Company oCompany = new Company();
        //    oCompany = oCompany.Get(1, nUserID);

        //    #region Get List
        //    List<RptDailyLogReport> oRptDailyLogReports = new List<RptDailyLogReport>();
        //    List<FabricBatchProductionBatchMan> oShiftWiseSummerys = new List<FabricBatchProductionBatchMan>();
        //    List<FabricBatchProductionBatchMan> orptSUmmerys = new List<FabricBatchProductionBatchMan>();
        //    List<FabricBatchProduction> oSummeryProductions = new List<FabricBatchProduction>();
        //    List<FabricBatchProductionBeam> oCommingBeams = new List<FabricBatchProductionBeam>();

        //    List<FabricBatchProductionBatchMan> orptSUmmeryForProds = new List<FabricBatchProductionBatchMan>();

        //    DateTime dDate = Convert.ToDateTime(sParams.Split('~')[0]);
        //    string sFEOIDs = Convert.ToString(sParams.Split('~')[1]);
        //    string sBuyerIDs = Convert.ToString(sParams.Split('~')[2]);
        //    int nFabricWeave = Convert.ToInt32(sParams.Split('~')[3]);
        //    int nProcessType = Convert.ToInt32(sParams.Split('~')[4]);
        //    string sConstruction = Convert.ToString(sParams.Split('~')[5]);
        //    double nReedCount = 0;
        //    if (!string.IsNullOrEmpty(sParams.Split('~')[6]))
        //        nReedCount = Convert.ToDouble(sParams.Split('~')[6]);
        //    double nYetToWeqave = Convert.ToDouble(sParams.Split('~')[7]);

        //    oRptDailyLogReports = RptDailyLogReport.Gets(dDate, sFEOIDs, sBuyerIDs, nFabricWeave, nProcessType, sConstruction, tsuid, nReedCount, nUserID);
        //    #endregion
        //    string sSql = string.Empty;
        //    #region Get Coming Loom
        //    if (oRptDailyLogReports.Any())
        //    {
        //        string sQuery = "";
            
        //        #region Buyer ID
        //        if (!string.IsNullOrEmpty(sBuyerIDs)) 
        //            sQuery = sQuery + " AND BuyerID IN (" + sBuyerIDs + ") ";
        //        #endregion

        //        #region textile subunit
        //        if (tsuid > 0)
        //            sQuery = sQuery + " AND TSUID =" + tsuid;
        //        #endregion
        //        sQuery = sQuery + " AND FEOID IN (" + string.Join(",", oRptDailyLogReports.Select(x => x.FEOID)) + ") ";

        //        sSql = "SELECT * FROM (SELECT * FROM View_FabricBatchProductionBeam ReadyLBeam WHERE  ReadyLBeam.FBStatus != 14 "//AND ReadyLBeam.MachineStatus= 1
        //                + sQuery
        //                + " AND ReadyLBeam.IsFinish=1 AND ReadyLBeam.WeavingProcessType=1 AND ReadyLBeam.BeamID NOT IN ("
        //                + " SELECT LBeam.BeamID FROM View_FabricBatchProductionBeam LBeam WHERE LBeam.WeavingProcessType=3 AND LBeam.FBID=ReadyLBeam.FBID)"
        //                + " UNION"
        //                + " SELECT * FROM View_FabricBatchProductionBeam RunningBeam WHERE  RunningBeam.FBStatus != 14 "//AND  RunningBeam.MachineStatus= 1
        //                + sQuery
        //                + " AND RunningBeam.WeavingProcessType=3 AND RunningBeam.MachineStatus=" + (int)EnumMachineStatus.Running + " And RunningBeam.FBPID IN ("
        //                + " SELECT FBPID FROM FabricBatchProduction WHERE EndTime IS NULL AND WeavingProcess=3)) AS F ORDER BY CONVERT(INT,dbo.fnGetNumberFromString(F.MachineCode)) ASC";


           
        //        oCommingBeams = FabricBatchProductionBeam.Gets(sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //        oCommingBeams = oCommingBeams.Where(x => x.YetWeaveQtyInMtr <= nYetToWeqave && (int)x.WeavingProcessType == 3).ToList();
        //    }
        //    #endregion
        //    #region Get ShiftWiseSummerys
        //    if (oRptDailyLogReports.Any())
        //    {
        //        string subQuery = "SELECT FBPID FROM FabricBatchProduction WHERE WeavingProcess=3";
        //        if (tsuid > 0)
        //            subQuery += " AND TSUID=" + tsuid;

        //        sSql = "SELECT * FROM View_FabricBatchProductionBatchMan WHERE FinishDate='" + dDate.AddDays(-1).ToString("dd MMM yyyy") + "' AND FBPID IN (" + subQuery + ")";
        //        oShiftWiseSummerys = FabricBatchProductionBatchMan.GetsBySql(sSql, (int)((User)Session[SessionInfo.CurrentUser]).UserID);
        //    }

        //    #endregion

        //    #region Get All Machine
        //    if (oRptDailyLogReports.Any())
        //    {
        //        sSql = "SELECT (SELECT MAX(RPM) FROM FabricBatchProductionBatchMan WHERE FBPID = FBP.FBPID) AS RPM,(SELECT MAX(Efficiency) FROM FabricBatchProductionBatchMan WHERE FBPID = FBP.FBPID) AS Efficiency,(SELECT SUM(Qty) FROM FabricBatchProductionBatchMan WHERE FBPID = FBP.FBPID) AS Qty,TSUID FROM FabricBatchProduction AS FBP WHERE WeavingProcess=3 AND EndTime IS NULL AND  FBPID IN (" + string.Join(",", oRptDailyLogReports.Select(x => x.FBPID)) + ")";

        //        oSummeryProductions = FabricBatchProduction.GetsSummery(sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    }
        //    string sSQL = "Select * from View_FabricMachine Where WeavingProcess=3 And IsBeam=0 And IsActive=1";
        //    if (tsuid > 0)
        //        sSQL += " AND TSUID=" + tsuid;

        //    List<FabricMachine> oFabricMachines = new List<FabricMachine>();
        //    oFabricMachines = FabricMachine.Gets(sSQL, nUserID);

        //    List<FabricMachine> oPatternOuts = new List<FabricMachine>();
        //    if (oRptDailyLogReports.Any())
        //    {

        //        sSql = "SELECT * FROM View_FabricMachine WHERE WeavingProcess=3 AND IsActive=1 AND IsBeam=0";
        //        if (!string.IsNullOrEmpty(oRptDailyLogReports[0].StopLoomFMIDs))
        //        {
        //            sSql += "AND FMID NOT IN (" + oRptDailyLogReports[0].StopLoomFMIDs + ")";
        //        }

        //        if (!string.IsNullOrEmpty(oRptDailyLogReports[0].RunLoomFMIDs))
        //        {
        //            sSql += "AND FMID NOT IN (" + oRptDailyLogReports[0].RunLoomFMIDs + ")";
        //        }
        //        if (tsuid > 0)
        //            sSql += " AND TSUID=" + tsuid;
        //        oPatternOuts = FabricMachine.Gets(sSql, nUserID);
        //    }



        //    #endregion
        //    #region Summery
            
        //    if (oRptDailyLogReports.Any())
        //    {
              
        //        string Subquery = "SELECT FBPID FROM FabricBatchProduction WHERE WeavingProcess=3";
        //        if (tsuid > 0)
        //            Subquery += " AND TSUID=" + tsuid;

        //        sSql = "SELECT * FROM View_FabricBatchProductionBatchMan WHERE FinishDate='" + dDate.AddDays(-1).ToString("dd MMM yyyy") + "' AND ShiftID IN (1,2,3) AND FBPID IN (" + Subquery + ")";
        //        orptSUmmeryForProds = FabricBatchProductionBatchMan.GetsBySql(sSql, (int)((User)Session[SessionInfo.CurrentUser]).UserID);
        //    }

        //    #endregion
        //    int nMaxColumn = 0;
        //    int colIndex = 2;
        //    int rowIndex = 2;
        //    ExcelRange cell;
        //    ExcelFill fill;
        //    OfficeOpenXml.Style.Border border;

        //    var excelPackage = new ExcelPackage();

        //    excelPackage.Workbook.Properties.Author = "ESimSol";
        //    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
        //    var sheet = excelPackage.Workbook.Worksheets.Add("Daily Log Report");
        //    sheet.Name = "Daily Log Report";

        //    sheet.Column(2).Width = 8; //SL
        //    sheet.Column(3).Width = 18; //Construction
        //    sheet.Column(4).Width = 20; //Buyer
        //    sheet.Column(5).Width = 10; //Total Ends
        //    sheet.Column(6).Width = 10; //Ref.no
        //    sheet.Column(7).Width = 28; //Order No
        //    sheet.Column(8).Width = 14; //Weave
        //    sheet.Column(9).Width = 10; //Reed-Count
        //    sheet.Column(10).Width = 25; //Running Loom
        //    sheet.Column(11).Width = 25; //Stop Loom
        //    sheet.Column(12).Width = 10; //Total-Loom
        //    sheet.Column(13).Width = 10; //Run-Loom
        //    sheet.Column(14).Width = 10; //Stop-Loom
        //    sheet.Column(15).Width = 15; //Remarks
        //    sheet.Column(16).Width = 15; //Remarks
        //    sheet.Column(17).Width = 20; //Remarks
        //    nMaxColumn = 17;

        //    #region Report Header
        //    sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
        //    cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
        //    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    rowIndex = rowIndex + 1;

        //    sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
        //    cell = sheet.Cells[rowIndex, 2]; cell.Value = "Daily Log Report"; cell.Style.Font.Bold = true;
        //    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    rowIndex = rowIndex + 1;

        //    sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
        //    cell = sheet.Cells[rowIndex, 2]; cell.Value = "Date : " + dDate.ToString("dd MMM yyyy"); cell.Style.Font.Bold = true;
        //    cell.Style.Font.Size = 14; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    rowIndex = rowIndex + 2;

        //    if (tsuid > 0 && oRptDailyLogReports.Any() && oRptDailyLogReports.First().TSUID > 0)
        //    {
        //        sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
        //        cell = sheet.Cells[rowIndex, 2]; cell.Value = oRptDailyLogReports.First().TSUName; cell.Style.Font.Bold = true;
        //        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //        rowIndex++;
        //    }
        //    #endregion

        //    #region Table Header
        //    colIndex = 2;
        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "SL"; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Construction"; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Buyer"; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Total Ends"; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Ref.no"; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Order No"; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Weave"; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Reed-Count"; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Running Loom"; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Beam Finish"; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Total-Loom"; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Run-Loom"; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Stop-Loom"; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Warp Lot"; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Weft Lot"; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Remarks"; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    rowIndex++;
        //    #endregion

        //    #region Table Body
        //    int nTotalInt = 0;
        //    int nSL = 0;
      
        //    var summary = oRptDailyLogReports.GroupBy(x => x.FEOID, (key, grp) => new RptDailyLogReport
        //    {
        //        FEOID = key,
        //        FEONo = grp.First().FEONo,
        //        OrderType = grp.First().OrderType,
        //        IsInHouse = grp.First().IsInHouse,
        //        Construction = grp.First().Construction,
        //        BuyerID = grp.First().BuyerID,
        //        BuyerName = grp.First().BuyerName,
        //        IsYarnDyed = grp.First().IsYarnDyed,
        //        FabricWeave = grp.First().FabricWeave,
        //        FabricWeaveName = grp.First().FabricWeaveName,
        //        ProcessType = grp.First().ProcessType,
        //        TotalEnds = grp.First().TotalEnds,
        //        WarpColor = grp.First().WarpColor,
        //        WeftColor = grp.First().WeftColor,
        //        WarpLot = grp.First().WarpLot,
        //        WeftLot = grp.First().WeftLot,
        //        Remark = grp.First().Remark,

        //        StopLoomNo = MachineCodeSetup.GenerateCode(grp.First().StopLoomNo.Split('~').ToList().Where(m => !string.IsNullOrEmpty(m.Trim())).ToArray()),
        //        StopLoom = grp.First().StopLoomNo.Split('~').ToList().Where(m => !string.IsNullOrEmpty(m.Trim())).Count(),
        //        RunLoom = grp.Select(p => p.MachineCode).Distinct().Count(),
        //        StopLoomString = grp.First().StopLoomNo,
        //        DailyLogs = grp.GroupBy(p => new { p.ReedCount, p.Dent }).Select(g => new RptDailyLogReport
        //        {
        //            ReedCount = g.Key.ReedCount,
        //            Dent = g.Key.Dent,
        //            MachineCode = string.Join("~", g.Select(m => m.MachineCode).Distinct()),
        //            RunLoom = g.Select(m => m.MachineCode).Distinct().Count()
        //        }).ToList()

        //    }).ToList().OrderBy(p => p.IsInHouse);


        //    #region Exe Order
        //    var oExeYarnDyed = summary.Where(x => x.IsInHouse && x.IsYarnDyed).OrderBy(x => x.BuyerName).ToList();

        //    if (oExeYarnDyed.Any())
        //    {
        //        cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn]; cell.Merge = true; cell.Value = "Exe-Y/D"; cell.Style.Font.Bold = true;
        //        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //        rowIndex++;
        //        GenerateDailyLogsReportDayWise(oExeYarnDyed, ref nSL, ref rowIndex, ref sheet, ref cell);

        //    }


        //    var oExeSolidDyed = summary.Where(x => x.IsInHouse && !x.IsYarnDyed).OrderBy(x => x.BuyerName).ToList();

        //    if (oExeSolidDyed.Any())
        //    {
        //        cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn]; cell.Merge = true; cell.Value = "Exe-S/D"; cell.Style.Font.Bold = true;
        //        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //        rowIndex++;
        //        GenerateDailyLogsReportDayWise(oExeSolidDyed, ref nSL, ref rowIndex, ref sheet, ref cell);

        //    }


        //    #endregion

        //    #region Sub Contact
        //    var oScwYarnDyed = summary.Where(x => !x.IsInHouse && x.IsYarnDyed).OrderBy(x => x.BuyerName).ToList();
        //    if (oScwYarnDyed.Any())
        //    {
        //        cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn]; cell.Merge = true; cell.Value = "SCW-Y/D"; cell.Style.Font.Bold = true;
        //        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //        rowIndex++;
        //        GenerateDailyLogsReportDayWise(oScwYarnDyed, ref nSL, ref rowIndex, ref sheet, ref cell);

        //    }

        //    var oScwSolidDyed = summary.Where(x => !x.IsInHouse && !x.IsYarnDyed).OrderBy(x => x.BuyerName).ToList();
        //    if (oScwSolidDyed.Any())
        //    {
        //        cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn]; cell.Merge = true; cell.Value = "SCW-S/D"; cell.Style.Font.Bold = true;
        //        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //        rowIndex++;
        //        GenerateDailyLogsReportDayWise(oScwSolidDyed, ref nSL, ref rowIndex, ref sheet, ref cell);

        //    }
        //    #endregion


        //    #endregion

        //    #region Exe Total
        //    colIndex = 2;

        //    cell = sheet.Cells[rowIndex, colIndex, rowIndex, 11]; cell.Merge = true; cell.Value = "Total(EXE-Y/D & EXE-S/D) "; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    colIndex = 12;
        //    nTotalInt = summary.Sum(x => x.TotalLoom);
        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nTotalInt; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    nTotalInt = summary.Sum(x => x.RunLoom);
        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nTotalInt; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    nTotalInt = summary.Sum(x => x.StopLoom);
        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nTotalInt; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    rowIndex++;
        //    #endregion

        //    #region Beam Finished
        //    colIndex = 2;


        //    cell = sheet.Cells[rowIndex, colIndex, rowIndex, 11]; cell.Merge = true; cell.Value = "Beam Finished "; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    colIndex = 12;
        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nTotalInt; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex, rowIndex, 17]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    rowIndex++;
        //    #endregion

        //    #region Beam Finished
        //    colIndex = 2;
        //    cell = sheet.Cells[rowIndex, colIndex, rowIndex, 11]; cell.Merge = true; cell.Value = "Pattern Out "; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    colIndex = 12;
        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oPatternOuts.Count(); cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex, rowIndex, 17]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    rowIndex++;
        //    #endregion

        //    #region Grand Total
        //    colIndex = 2;

        //    cell = sheet.Cells[rowIndex, colIndex, rowIndex, 11]; cell.Merge = true; cell.Value = "Grand Total "; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    colIndex = 12;
        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = summary.Sum(x => x.TotalLoom); cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oFabricMachines.Count(); cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = summary.Sum(x => x.StopLoom); cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex, rowIndex, 17]; cell.Merge = true; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    rowIndex++;
        //    #endregion

        //    #region Summary
    
        //    var summaryForTSU1 = oRptDailyLogReports.Where(x => x.TSUID == 1).ToList().GroupBy(x => x.FEOID, (key, grp) => new RptDailyLogReport
        //    {
        //        FEOID = key,
        //        FEONo = grp.First().FEONo,
        //        OrderType = grp.First().OrderType,
        //        IsInHouse = grp.First().IsInHouse,
        //        Construction = grp.First().Construction,
        //        BuyerID = grp.First().BuyerID,
        //        BuyerName = grp.First().BuyerName,
        //        IsYarnDyed = grp.First().IsYarnDyed,
        //        FabricWeave = grp.First().FabricWeave,
        //        FabricWeaveName = grp.First().FabricWeaveName,
        //        ProcessType = grp.First().ProcessType,
        //        TotalEnds = grp.First().TotalEnds,
        //        WarpColor = grp.First().WarpColor,
        //        WeftColor = grp.First().WeftColor,
        //        StopLoomNo = MachineCodeSetup.GenerateCode(grp.First().StopLoomNo.Split('~').ToList().Where(m => !string.IsNullOrEmpty(m.Trim())).ToArray()),
        //        StopLoom = grp.First().StopLoomNo.Split('~').ToList().Where(m => !string.IsNullOrEmpty(m.Trim())).Count(),
        //        RunLoom = grp.Select(p => p.MachineCode).Distinct().Distinct().Count(),
        //        DailyLogs = grp.GroupBy(p => new { p.ReedCount, p.Dent }).Select(g => new RptDailyLogReport
        //        {
        //            ReedCount = g.Key.ReedCount,
        //            Dent = g.Key.Dent,
        //            MachineCode = string.Join("~", g.Select(m => m.MachineCode).Distinct()),
        //            RunLoom = g.Select(m => m.MachineCode).Distinct().Count()
        //        }).ToList()

        //    }).ToList();

        //    var summaryForTSU2 = oRptDailyLogReports.Where(x => x.TSUID == 2).ToList().GroupBy(x => x.FEOID, (key, grp) => new RptDailyLogReport
        //    {
        //        FEOID = key,
        //        FEONo = grp.First().FEONo,
        //        OrderType = grp.First().OrderType,
        //        IsInHouse = grp.First().IsInHouse,
        //        Construction = grp.First().Construction,
        //        BuyerID = grp.First().BuyerID,
        //        BuyerName = grp.First().BuyerName,
        //        IsYarnDyed = grp.First().IsYarnDyed,
        //        FabricWeave = grp.First().FabricWeave,
        //        FabricWeaveName = grp.First().FabricWeaveName,
        //        ProcessType = grp.First().ProcessType,
        //        TotalEnds = grp.First().TotalEnds,
        //        WarpColor = grp.First().WarpColor,
        //        WeftColor = grp.First().WeftColor,
        //        StopLoomNo = MachineCodeSetup.GenerateCode(grp.First().StopLoomNo.Split('~').ToList().Where(m => !string.IsNullOrEmpty(m.Trim())).ToArray()),
        //        StopLoom = grp.First().StopLoomNo.Split('~').ToList().Where(m => !string.IsNullOrEmpty(m.Trim())).Distinct().Count(),
        //        RunLoom = grp.Select(p => p.MachineCode).Distinct().Count(),
        //        DailyLogs = grp.GroupBy(p => new { p.ReedCount, p.Dent }).Select(g => new RptDailyLogReport
        //        {
        //            ReedCount = g.Key.ReedCount,
        //            Dent = g.Key.Dent,
        //            MachineCode = string.Join("~", g.Select(m => m.MachineCode).Distinct()),
        //            RunLoom = g.Select(m => m.MachineCode).Distinct().Count()
        //        }).ToList()

        //    }).ToList();


        //    colIndex = 4;
        //    cell = sheet.Cells[rowIndex, colIndex, rowIndex, 8]; cell.Merge = true; cell.Value = "Summary For Production( " + dDate.AddDays(-1).ToString("dd MMM yyyy") + " )"; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGreen);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //    rowIndex++;

        //    colIndex = 4;
        //    cell = sheet.Cells[rowIndex, colIndex, rowIndex, 5]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, 6, rowIndex, 6]; cell.Merge = true; cell.Value = "Shade-1"; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, 7, rowIndex, 7]; cell.Merge = true; cell.Value = "Shade-2"; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, 8, rowIndex, 8]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //    rowIndex++;

        //    colIndex = 4;
        //    cell = sheet.Cells[rowIndex, colIndex, rowIndex, 5]; cell.Merge = true; cell.Value = "Total Production (M) ="; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, 6, rowIndex, 6]; cell.Merge = false; cell.Value = Convert.ToInt32(oShiftWiseSummerys.Where(x => x.TSUID == 1).Sum(x => x.QtyInM)); ; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, 7, rowIndex, 7]; cell.Merge = false; cell.Value = Convert.ToInt32(oShiftWiseSummerys.Where(x => x.TSUID == 2).Sum(x => x.QtyInM)); cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, 8, rowIndex, 8]; cell.Merge = false; cell.Value = Convert.ToInt32(oShiftWiseSummerys.Sum(x => x.QtyInM)); cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //    rowIndex++;

        //    double val1 = summaryForTSU1.Sum(x => x.RunLoom);
        //    double val2 = summaryForTSU2.Sum(x => x.RunLoom);
        //    int nShiftCountForShade1 = orptSUmmeryForProds.Where(x => x.TSUID == 1).Select(x => x.ShiftID).Distinct().Count();
        //    int nShiftCountForShade2 = orptSUmmeryForProds.Where(x => x.TSUID == 2).Select(x => x.ShiftID).Distinct().Count();
        //    int nShiftCountForShade12 = orptSUmmeryForProds.Select(x => x.ShiftID).Distinct().Count();
        //    double Shade1AvgRunLoom = (nShiftCountForShade1 > 0) ? Math.Round((double)orptSUmmeryForProds.Where(x => x.TSUID == 1).ToList().Count() / (double)nShiftCountForShade1, 0) : 0;
        //    double Shade2AvgRunLoom = (nShiftCountForShade2 > 0) ? Math.Round((double)orptSUmmeryForProds.Where(x => x.TSUID == 2).ToList().Count() / (double)nShiftCountForShade2, 0) : 0;
        //    double Shade12AvgRunLoom = (nShiftCountForShade12 > 0) ? Math.Round((double)orptSUmmeryForProds.ToList().Count() / (double)nShiftCountForShade12, 0) : 0;
        //    int nShade1RunningLoom = orptSUmmeryForProds.Where(x => x.TSUID == 1).ToList().Count();
        //    int nShade2RunningLoom = orptSUmmeryForProds.Where(x => x.TSUID == 2).ToList().Count();
        //    int nShade12RunningLoom = orptSUmmeryForProds.ToList().Count();
        //    colIndex = 4;
        //    cell = sheet.Cells[rowIndex, colIndex, rowIndex, 5]; cell.Merge = true; cell.Value = "Per Shift avg. Run Loom ="; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, 6, rowIndex, 6]; cell.Merge = false; cell.Value = Shade1AvgRunLoom; ; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, 7, rowIndex, 7]; cell.Merge = false; cell.Value = Shade2AvgRunLoom; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, 8, rowIndex, 8]; cell.Merge = false; cell.Value = Shade12AvgRunLoom; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //    rowIndex++;

        //    colIndex = 4;
        //    cell = sheet.Cells[rowIndex, colIndex, rowIndex, 5]; cell.Merge = true; cell.Value = "Avg. Pick ="; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //    double avgPickTSU1 = (val1 != 0) ? Math.Round(summaryForTSU1.Sum(x => x.PPI * x.RunLoom) / val1, 0) : 0;
        //    cell = sheet.Cells[rowIndex, 6, rowIndex, 6]; cell.Merge = false; cell.Value = avgPickTSU1; ; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //    double avgPickTSU2 = (val2 != 0) ? Math.Round(summaryForTSU2.Sum(x => x.PPI * x.RunLoom) / val2, 0) : 0;
        //    cell = sheet.Cells[rowIndex, 7, rowIndex, 7]; cell.Merge = false; cell.Value = avgPickTSU2; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


        //    double avgPick = (summary.Sum(x => x.RunLoom) != 0) ? summary.Sum(x => x.PPI * x.RunLoom) / summary.Sum(x => x.RunLoom) : 0;

        //    cell = sheet.Cells[rowIndex, 8, rowIndex, 8]; cell.Merge = false; cell.Value = avgPick; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //    rowIndex++;


        //    colIndex = 4;
        //    cell = sheet.Cells[rowIndex, colIndex, rowIndex, 5]; cell.Merge = true; cell.Value = "Avg. Eff ="; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, 6, rowIndex, 6]; cell.Merge = false; cell.Value = (nShade1RunningLoom > 0) ? Math.Round(orptSUmmeryForProds.Where(x => x.TSUID == 1).ToList().Sum(x => x.Efficiency) / nShade1RunningLoom, 0) : 0; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, 7, rowIndex, 7]; cell.Merge = false; cell.Value = (nShade2RunningLoom > 0) ? Math.Round(orptSUmmeryForProds.Where(x => x.TSUID == 2).ToList().Sum(x => x.Efficiency) / nShade2RunningLoom, 0) : 0; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, 8, rowIndex, 8]; cell.Merge = false; cell.Value = (nShade12RunningLoom > 0) ? Math.Round(orptSUmmeryForProds.Sum(x => x.Efficiency) / nShade12RunningLoom, 0) : 0; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //    rowIndex++;




        //    colIndex = 4;
        //    cell = sheet.Cells[rowIndex, colIndex, rowIndex, 5]; cell.Merge = true; cell.Value = "Avg. RPM ="; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, 6, rowIndex, 6]; cell.Merge = false; cell.Value = (nShade1RunningLoom > 0) ? Math.Round((double)orptSUmmeryForProds.Where(x => x.TSUID == 1).ToList().Sum(x => x.RPM) / nShade1RunningLoom, 0) : 0; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


        //    cell = sheet.Cells[rowIndex, 7, rowIndex, 7]; cell.Merge = false; cell.Value = (nShade2RunningLoom > 0) ? Math.Round((double)orptSUmmeryForProds.Where(x => x.TSUID == 2).ToList().Sum(x => x.RPM) / nShade2RunningLoom, 0) : 0; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, 8, rowIndex, 8]; cell.Merge = false; cell.Value = (nShade12RunningLoom > 0) ? Math.Round((double)orptSUmmeryForProds.Sum(x => x.RPM) / nShade12RunningLoom, 0) : 0; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //    rowIndex++;

        //    rowIndex++;

        //    #endregion

        //    return excelPackage;
        //}
        //public void ExcelDailyLogReport(int tsuid, string sParams)
        //{
        //    using (var excelPackage = DailyLogsReportInExcel(tsuid, sParams, false))
        //    {
        //        Response.ClearContent();
        //        Response.BinaryWrite(excelPackage.GetAsByteArray());
        //        Response.AddHeader("content-disposition", "attachment; filename=DailyLogReport.xlsx");
        //        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //        Response.Flush();
        //        Response.End();

        //    }
        //}

        //public ExcelPackage GenerateDailyLogReportInExcel(string sParams, bool bIsMail)
        //{

        //    Company oCompany = new Company();
        //    oCompany = oCompany.Get(1, (bIsMail) ? 0 : ((User)Session[SessionInfo.CurrentUser]).UserID);

        //    #region Get List

        //    List<RptDailyLogReport> oRptDailyLogReports = new List<RptDailyLogReport>();
        //    string sSQL = "SELECT * FROM View_RPT_DailyLogReport";

        //    DateTime dDate = Convert.ToDateTime(sParams.Split('~')[0]);
        //    string sFEOIDs = Convert.ToString(sParams.Split('~')[1]);
        //    string sBuyerIDs = Convert.ToString(sParams.Split('~')[2]);
        //    int nFabricWeave = Convert.ToInt32(sParams.Split('~')[3]);
        //    int nProcessType = Convert.ToInt32(sParams.Split('~')[4]);
        //    string sConstruction = Convert.ToString(sParams.Split('~')[5]);

        //    sSQL = sSQL + " WHERE CONVERT(DATE,CONVERT(VARCHAR(12), StartTime,106)) <= CONVERT(DATE,CONVERT(VARCHAR(12),'" + dDate.ToString("dd MMM yyyy") + "',106)) AND ISNULL(EndTime,0)=0 ";

        //    if (!string.IsNullOrEmpty(sFEOIDs))
        //    {
        //        sSQL = sSQL + " AND FEOID IN (" + sFEOIDs + ") ";
        //    }
        //    if (!string.IsNullOrEmpty(sBuyerIDs))
        //    {
        //        sSQL = sSQL + " AND BuyerID IN (" + sBuyerIDs + ") ";
        //    }
        //    if (nFabricWeave > 0)
        //    {
        //        sSQL = sSQL + " AND FabricWeave = " + nFabricWeave;
        //    }
        //    if (nProcessType > 0)
        //    {
        //        sSQL = sSQL + " AND ProcessType = " + nProcessType;
        //    }
        //    if (!string.IsNullOrEmpty(sConstruction))
        //    {
        //        sSQL = sSQL + " AND Construction LIKE '%" + sConstruction + "%' ";
        //    }
        //    sSQL = sSQL + " And RunLoom>0 ORDER BY BuyerName,FEOID";
        //    oRptDailyLogReports = RptDailyLogReport.Gets(sSQL, (bIsMail) ? 0 : ((User)Session[SessionInfo.CurrentUser]).UserID);

        //    #endregion

        //    #region Get All Machine

        //    sSQL = "Select * from View_FabricMachine Where WeavingProcess=3 And IsBeam=0 And IsActive=1";

        //    List<FabricMachine> oFabricMachines = new List<FabricMachine>();
        //    oFabricMachines = FabricMachine.Gets(sSQL, (bIsMail) ? 0 : ((User)Session[SessionInfo.CurrentUser]).UserID);

        //    #endregion

        //    int nMaxColumn = 0;
        //    int colIndex = 2;
        //    int rowIndex = 2;
        //    ExcelRange cell;
        //    ExcelFill fill;
        //    OfficeOpenXml.Style.Border border;

        //    var excelPackage = new ExcelPackage();

        //    excelPackage.Workbook.Properties.Author = "ESimSol";
        //    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
        //    var sheet = excelPackage.Workbook.Worksheets.Add("Daily Log Report");
        //    sheet.Name = "Daily Log Report";

        //    sheet.Column(2).Width = 8; //SL
        //    sheet.Column(3).Width = 18; //Construction
        //    //sheet.Column(4).Width = 10; //PPI
        //    //sheet.Column(5).Width = 10; //TOTAL PICK
        //    sheet.Column(4).Width = 20; //Buyer
        //    sheet.Column(5).Width = 10; //Total Ends
        //    sheet.Column(6).Width = 10; //Ref.no
        //    sheet.Column(7).Width = 28; //Order No
        //    sheet.Column(8).Width = 14; //Weave
        //    sheet.Column(9).Width = 10; //Reed-Count
        //    sheet.Column(10).Width = 20; //Loom-No
        //    sheet.Column(11).Width = 10; //Total-Loom
        //    sheet.Column(12).Width = 10; //Run-Loom
        //    sheet.Column(13).Width = 10; //Stop-Loom
        //    sheet.Column(14).Width = 20; //Remarks
        //    nMaxColumn = 14;

        //    #region Report Header
        //    sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
        //    cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
        //    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    rowIndex = rowIndex + 1;

        //    sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
        //    cell = sheet.Cells[rowIndex, 2]; cell.Value = "Daily Log Report"; cell.Style.Font.Bold = true;
        //    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    rowIndex = rowIndex + 1;

        //    sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
        //    cell = sheet.Cells[rowIndex, 2]; cell.Value = "Date : " + dDate.ToString("dd MMM yyyy"); cell.Style.Font.Bold = true;
        //    cell.Style.Font.Size = 14; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    rowIndex = rowIndex + 2;
        //    #endregion

        //    #region Table Header
        //    colIndex = 2;
        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "SL"; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Construction"; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    /*
        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "PPI"; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "TOTAL PICK"; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //    */

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Buyer"; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Total Ends"; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Ref.no"; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Order No"; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Weave"; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Reed-Count"; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Loom-No"; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Total-Loom"; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Run-Loom"; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Stop-Loom"; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Remarks"; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    rowIndex++;
        //    #endregion

        //    #region Table
        //    int nTotalInt = 0;
        //    List<RptDailyLogReport> oTempRptDailyLogReports = new List<RptDailyLogReport>();
        //    oRptDailyLogReports = oRptDailyLogReports.GroupBy(x => x.FEOID).Select(x => x.First()).ToList();

        //    for (int i = 1; i <= 2; i++)
        //    {
        //        int nSL = 0;
        //        oTempRptDailyLogReports = new List<RptDailyLogReport>();
        //        oTempRptDailyLogReports = oRptDailyLogReports.Where(x => (i == 1 ? x.IsYarnDyed == true : x.IsYarnDyed == false)).ToList();

        //        sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
        //        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //        cell = sheet.Cells[rowIndex, 2]; cell.Value = (i == 1 ? "EXE-Y/D" : "EXE-S/D"); cell.Style.Font.Bold = true;
        //        cell.Style.Font.Size = 14; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        rowIndex = rowIndex + 1;

        //        #region Body
        //        foreach (RptDailyLogReport oItem in oTempRptDailyLogReports)
        //        {
        //            nSL++;
        //            colIndex = 2;
        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nSL; cell.Style.Font.Bold = false;
        //            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Construction; cell.Style.Font.Bold = false;
        //            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            /*
        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.PPI; cell.Style.Font.Bold = false;
        //            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.TotalPick; cell.Style.Font.Bold = false;
        //            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //            */

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.BuyerName; cell.Style.Font.Bold = false;
        //            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.TotalEnds; cell.Style.Font.Bold = false;
        //            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.RefNo; cell.Style.Font.Bold = false;
        //            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.OrderNo; cell.Style.Font.Bold = false;
        //            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.FabricWeaveName; cell.Style.Font.Bold = false;
        //            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ReedCountWithDent; cell.Style.Font.Bold = false;
        //            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.MachineCodesSt; cell.Style.Font.Bold = false;
        //            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.TotalLoom; cell.Style.Font.Bold = false;
        //            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.RunLoom; cell.Style.Font.Bold = false;
        //            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.StopLoom; cell.Style.Font.Bold = false;
        //            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Remark; cell.Style.Font.Bold = false;
        //            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            rowIndex++;
        //        }
        //        #endregion

        //        #region Total
        //        colIndex = 2;
        //        cell = sheet.Cells[rowIndex, colIndex, rowIndex, 10]; cell.Merge = true; cell.Value = "Total : "; cell.Style.Font.Bold = true;
        //        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        colIndex = 11;
        //        nTotalInt = oTempRptDailyLogReports.Sum(x => x.TotalLoom);
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nTotalInt; cell.Style.Font.Bold = true;
        //        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        nTotalInt = oTempRptDailyLogReports.Sum(x => x.RunLoom);
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nTotalInt; cell.Style.Font.Bold = true;
        //        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        nTotalInt = oTempRptDailyLogReports.Sum(x => x.StopLoom);
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nTotalInt; cell.Style.Font.Bold = true;
        //        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        rowIndex++;
        //        #endregion
        //    }
        //    #endregion

        //    #region Exe Total
        //    colIndex = 2;

        //    cell = sheet.Cells[rowIndex, colIndex, rowIndex, 10]; cell.Merge = true; cell.Value = "Total(EXE-Y/D & EXE-S/D) "; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    colIndex = 11;
        //    nTotalInt = oRptDailyLogReports.Sum(x => x.TotalLoom);
        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nTotalInt; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    nTotalInt = oRptDailyLogReports.Sum(x => x.RunLoom);
        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nTotalInt; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    nTotalInt = oRptDailyLogReports.Sum(x => x.StopLoom);
        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nTotalInt; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    rowIndex++;
        //    #endregion

        //    #region Beam Finished
        //    colIndex = 2;

        //    cell = sheet.Cells[rowIndex, colIndex, rowIndex, 10]; cell.Merge = true; cell.Value = "Beam Finished "; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    colIndex = 11;
        //    nTotalInt = oFabricMachines.Where(x => x.MachineStatus == EnumMachineStatus.Free).Count();
        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nTotalInt; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nTotalInt; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    nTotalInt = 0;
        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nTotalInt; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    rowIndex++;
        //    #endregion

        //    #region Grand Total
        //    colIndex = 2;

        //    cell = sheet.Cells[rowIndex, colIndex, rowIndex, 10]; cell.Merge = true; cell.Value = "Grand Total "; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    colIndex = 11;
        //    nTotalInt = oFabricMachines.Where(x => x.MachineStatus == EnumMachineStatus.Free).Count() + oRptDailyLogReports.Sum(x => x.TotalLoom);
        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nTotalInt; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nTotalInt; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    nTotalInt = oRptDailyLogReports.Sum(x => x.StopLoom);
        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nTotalInt; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    rowIndex++;
        //    #endregion

        //    #region Summary

        //    colIndex = 2;

        //    cell = sheet.Cells[rowIndex, colIndex, rowIndex, 3]; cell.Merge = true; cell.Value = "Under Process"; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, 4, rowIndex, nMaxColumn]; cell.Merge = true; cell.Value = 0; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    rowIndex++;


        //    cell = sheet.Cells[rowIndex, colIndex, rowIndex, 3]; cell.Merge = true; cell.Value = "Beam Finish"; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, 4, rowIndex, nMaxColumn]; cell.Merge = true; cell.Value = MachineCodeSetup.GenerateCode(oFabricMachines.Where(x => x.MachineStatus == EnumMachineStatus.Free).Select(x => x.Code).ToArray()); cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //    rowIndex++;
        //    #endregion

        //    return excelPackage;
        //}

        //public object[] MailDailyLogReport()
        //{
        //    object[] objArr = new object[1];

        //    DateTime current = DateTime.Today;
        //    string sParams = current.ToString("dd MMM yyyy") + "~" + "" + "~" + "" + "~"+ "" + "~" + "" + "~" + "" + "~"+ "";

        //    //var excelPackage = GenerateDailyLogReportInExcel(sParams, true);
        //    var excelPackage = DailyLogsReportInExcel(0, sParams, true);
        //    Attachment oAttachment = null;
        //    List<Attachment> oAttachments = new List<Attachment>();

        //    Stream stream = null;
        //    stream = new MemoryStream(excelPackage.GetAsByteArray());
        //    ContentType ct = new ContentType("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        //    oAttachment = new Attachment(stream, ct);
        //    oAttachment.Name = "DailyLogReport(" + DateTime.Today.ToString("dd MMM yyyy") + ")";
        //    oAttachments.Add(oAttachment);

        //    objArr[0] = oAttachments;
        //    return objArr;
        //}


        //private void GenerateDailyLogsReport(List<RptDailyLogReport> oRptDailyLogReports, ref int nSL, ref int rowIndex, ref ExcelWorksheet sheet, ref ExcelRange cell)
        //{
        //    ExcelFill fill;
        //    OfficeOpenXml.Style.Border border;
        //    int nNextRowIndex = 0;
        //    int colIndex = 2;
        //    int tempRowIndex = 0;
        //    int tempColIndex = 0;
        //    #region
        //    foreach (RptDailyLogReport oItem in oRptDailyLogReports)
        //    {
        //        nNextRowIndex = (oItem.DailyLogs.Any()) ? rowIndex + oItem.DailyLogs.Count() - 1 : rowIndex;

        //        nSL++;
        //        colIndex = 2;
        //        cell = sheet.Cells[rowIndex, colIndex, nNextRowIndex, colIndex++]; cell.Merge = true; cell.Value = nSL; cell.Style.Font.Bold = false;
        //        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex, nNextRowIndex, colIndex++]; cell.Merge = true; cell.Value = oItem.Construction; cell.Style.Font.Bold = false;
        //        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex, nNextRowIndex, colIndex++]; cell.Merge = true; cell.Value = oItem.BuyerName; cell.Style.Font.Bold = false;
        //        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex, nNextRowIndex, colIndex++]; cell.Merge = true; cell.Value = oItem.TotalEnds; cell.Style.Font.Bold = false;
        //        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex, nNextRowIndex, colIndex++]; cell.Merge = true; cell.Value = oItem.RefNo; cell.Style.Font.Bold = false;
        //        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex, nNextRowIndex, colIndex++]; cell.Merge = true; cell.Value = oItem.OrderNo; cell.Style.Font.Bold = false;
        //        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex, nNextRowIndex, colIndex++]; cell.Merge = true; cell.Value = oItem.FabricWeaveName; cell.Style.Font.Bold = false;
        //        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        tempRowIndex = rowIndex;
                
        //        foreach (RptDailyLogReport data in oItem.DailyLogs)
        //        {
        //            tempColIndex = colIndex;
        //            cell = sheet.Cells[tempRowIndex, tempColIndex++]; cell.Value = data.ReedCountWithDent; cell.Style.Font.Bold = false;
        //            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
       
        //            cell = sheet.Cells[tempRowIndex, tempColIndex++]; cell.Value = data.MachineCodesSt; cell.Style.Font.Bold = false;
        //            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //            tempRowIndex++;
        //        }
        //        colIndex += 2;
        //        cell = sheet.Cells[rowIndex, colIndex, nNextRowIndex, colIndex++]; cell.Merge = true; cell.Value = oItem.StopLoomNo; cell.Style.Font.Bold = false;
        //        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex, nNextRowIndex, colIndex++]; cell.Merge = true; cell.Value = oItem.TotalLoom; cell.Style.Font.Bold = false;
        //        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                
        //        cell = sheet.Cells[rowIndex, colIndex, nNextRowIndex, colIndex++]; cell.Merge = true; cell.Value = oItem.RunLoom; cell.Style.Font.Bold = false;
        //        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex, nNextRowIndex, colIndex++]; cell.Merge = true; cell.Value = oItem.StopLoom; cell.Style.Font.Bold = false;
        //        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex, nNextRowIndex, colIndex++]; cell.Merge = true; cell.Value = oItem.WarpLot; cell.Style.Font.Bold = false;
        //        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex, nNextRowIndex, colIndex++]; cell.Merge = true; cell.Value = oItem.WeftLot; cell.Style.Font.Bold = false;
        //        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex, nNextRowIndex, colIndex++]; cell.Merge = true; cell.Value = oItem.Remark; cell.Style.Font.Bold = false;
        //        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        rowIndex = ++nNextRowIndex;
        //    }
        //    #endregion

        //    #region Total
        //    colIndex = 2;
        //    cell = sheet.Cells[rowIndex, colIndex, rowIndex, 11]; cell.Merge = true; cell.Value = "Total : "; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    colIndex = 12;
        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oRptDailyLogReports.Sum(x => x.TotalLoom); cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oRptDailyLogReports.Sum(x => x.RunLoom); cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oRptDailyLogReports.Sum(x => x.StopLoom); cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex, rowIndex, 17]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    rowIndex++;
        //    #endregion
        //}

        //private void GenerateDailyLogsReportDayWise(List<RptDailyLogReport> oRptDailyLogReports, ref int nSL, ref int rowIndex, ref ExcelWorksheet sheet, ref ExcelRange cell)
        //{
        //    ExcelFill fill;
        //    OfficeOpenXml.Style.Border border;
        //    int nNextRowIndex = 0;
        //    int colIndex = 2;
        //    int tempRowIndex = 0;
        //    int tempColIndex = 0;
        //    #region
        //    foreach (RptDailyLogReport oItem in oRptDailyLogReports)
        //    {
        //        nNextRowIndex = (oItem.DailyLogs.Any()) ? rowIndex + oItem.DailyLogs.Count() - 1 : rowIndex;

        //        nSL++;
        //        colIndex = 2;
        //        cell = sheet.Cells[rowIndex, colIndex, nNextRowIndex, colIndex++]; cell.Merge = true; cell.Value = nSL; cell.Style.Font.Bold = false;
        //        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex, nNextRowIndex, colIndex++]; cell.Merge = true; cell.Value = oItem.Construction; cell.Style.Font.Bold = false;
        //        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex, nNextRowIndex, colIndex++]; cell.Merge = true; cell.Value = oItem.BuyerName; cell.Style.Font.Bold = false;
        //        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex, nNextRowIndex, colIndex++]; cell.Merge = true; cell.Value = oItem.TotalEnds; cell.Style.Font.Bold = false;
        //        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex, nNextRowIndex, colIndex++]; cell.Merge = true; cell.Value = oItem.RefNo; cell.Style.Font.Bold = false;
        //        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex, nNextRowIndex, colIndex++]; cell.Merge = true; cell.Value = oItem.OrderNo; cell.Style.Font.Bold = false;
        //        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex, nNextRowIndex, colIndex++]; cell.Merge = true; cell.Value = oItem.FabricWeaveName; cell.Style.Font.Bold = false;
        //        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        tempRowIndex = rowIndex;

        //        foreach (RptDailyLogReport data in oItem.DailyLogs)
        //        {
        //            tempColIndex = colIndex;
        //            cell = sheet.Cells[tempRowIndex, tempColIndex++]; cell.Value = data.ReedCountWithDent; cell.Style.Font.Bold = false;
        //            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = sheet.Cells[tempRowIndex, tempColIndex++]; cell.Value = data.MachineCodesSt; cell.Style.Font.Bold = false;
        //            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //            tempRowIndex++;
        //        }
        //        colIndex += 2;
        //        cell = sheet.Cells[rowIndex, colIndex, nNextRowIndex, colIndex++]; cell.Merge = true; cell.Value = oItem.StopLoomNo; cell.Style.Font.Bold = false;
        //        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex, nNextRowIndex, colIndex++]; cell.Merge = true; cell.Value = oItem.TotalLoom; cell.Style.Font.Bold = false;
        //        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex, nNextRowIndex, colIndex++]; cell.Merge = true; cell.Value = oItem.RunLoom; cell.Style.Font.Bold = false;
        //        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex, nNextRowIndex, colIndex++]; cell.Merge = true; cell.Value = oItem.StopLoom; cell.Style.Font.Bold = false;
        //        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex, nNextRowIndex, colIndex++]; cell.Merge = true; cell.Value = oItem.WarpLot; cell.Style.Font.Bold = false;
        //        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex, nNextRowIndex, colIndex++]; cell.Merge = true; cell.Value = oItem.WeftLot; cell.Style.Font.Bold = false;
        //        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, colIndex, nNextRowIndex, colIndex++]; cell.Merge = true; cell.Value = oItem.Remark; cell.Style.Font.Bold = false;
        //        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        rowIndex = ++nNextRowIndex;
        //    }
        //    #endregion

        //    #region Total
        //    colIndex = 2;
        //    cell = sheet.Cells[rowIndex, colIndex, rowIndex, 11]; cell.Merge = true; cell.Value = "Total : "; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    colIndex = 12;
        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oRptDailyLogReports.Sum(x => x.TotalLoom); cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oRptDailyLogReports.Sum(x => x.RunLoom); cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oRptDailyLogReports.Sum(x => x.StopLoom); cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex, rowIndex, 17]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    rowIndex++;
        //    #endregion
        //}
        //public ActionResult PrintDailyLogsReport(int tsuid, string sParams)
        //{
        //    bool bIsMail = false;
        //    long nUserID = (bIsMail) ? 0 : ((User)Session[SessionInfo.CurrentUser]).UserID;
        //    Company oCompany = new Company();
        //    oCompany = oCompany.Get(1, nUserID);

        //    List<RptDailyLogReport> oRptDailyLogReports = new List<RptDailyLogReport>();
        //    List<FabricBatchProductionBatchMan> oShiftWiseSummerys = new List<FabricBatchProductionBatchMan>();
        //    List<FabricBatchProductionBatchMan> orptSUmmerys = new List<FabricBatchProductionBatchMan>();
        //    List<FabricBatchProduction> oSummeryProductions = new List<FabricBatchProduction>();
        //    List<FabricBatchProductionBeam> oCommingBeams = new List<FabricBatchProductionBeam>();
        //    List<FabricBatchProductionBatchMan> orptSUmmeryForProds = new List<FabricBatchProductionBatchMan>();

        //    DateTime dDate = Convert.ToDateTime(sParams.Split('~')[0]);
        //    string sFEOIDs = Convert.ToString(sParams.Split('~')[1]);
        //    string sBuyerIDs = Convert.ToString(sParams.Split('~')[2]);
        //    int nFabricWeave = Convert.ToInt32(sParams.Split('~')[3]);
        //    int nProcessType = Convert.ToInt32(sParams.Split('~')[4]);
        //    string sConstruction = Convert.ToString(sParams.Split('~')[5]);
        //    double nReedCount = 0;
        //    if (!string.IsNullOrEmpty(sParams.Split('~')[6]))
        //        nReedCount = Convert.ToDouble(sParams.Split('~')[6]);
        //    double nYetToWeqave = Convert.ToDouble(sParams.Split('~')[7]);

        //    oRptDailyLogReports = RptDailyLogReport.Gets(dDate, sFEOIDs, sBuyerIDs, nFabricWeave, nProcessType, sConstruction, tsuid, nReedCount, nUserID);


        //    string sSql = string.Empty;
        //    #region Get Coming Loom
        //    if (oRptDailyLogReports.Any())
        //    {
        //        string sQuery = "";
                
        //        #region Buyer ID
        //        if (!string.IsNullOrEmpty(sBuyerIDs)) // BuyerName used for carrying BuyerIDs
        //            sQuery = sQuery + " AND BuyerID IN (" + sBuyerIDs + ") ";
        //        #endregion

        //        #region textile subunit
        //        if (tsuid > 0)
        //            sQuery = sQuery + " AND TSUID =" + tsuid;
        //        #endregion
        //        sQuery = sQuery + " AND FEOID IN (" + string.Join(",", oRptDailyLogReports.Select(x => x.FEOID)) + ") ";

        //        sSql = "SELECT * FROM (SELECT * FROM View_FabricBatchProductionBeam ReadyLBeam WHERE  ReadyLBeam.FBStatus != 14 "//AND ReadyLBeam.MachineStatus= 1
        //                + sQuery
        //                + " AND ReadyLBeam.IsFinish=1 AND ReadyLBeam.WeavingProcessType=1 AND ReadyLBeam.BeamID NOT IN ("
        //                + " SELECT LBeam.BeamID FROM View_FabricBatchProductionBeam LBeam WHERE LBeam.WeavingProcessType=3 AND LBeam.FBID=ReadyLBeam.FBID)"
        //                + " UNION"
        //                + " SELECT * FROM View_FabricBatchProductionBeam RunningBeam WHERE  RunningBeam.FBStatus != 14 "//AND  RunningBeam.MachineStatus= 1
        //                + sQuery
        //                + " AND RunningBeam.WeavingProcessType=3 AND RunningBeam.MachineStatus=" + (int)EnumMachineStatus.Running + " And RunningBeam.FBPID IN ("
        //                + " SELECT FBPID FROM FabricBatchProduction WHERE EndTime IS NULL AND WeavingProcess=3)) AS F ORDER BY CONVERT(INT,dbo.fnGetNumberFromString(F.MachineCode)) ASC";

        //        oCommingBeams = FabricBatchProductionBeam.Gets(sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //        oCommingBeams = oCommingBeams.Where(x => x.YetWeaveQtyInMtr <= nYetToWeqave && (int)x.WeavingProcessType == 3).ToList();
        //    }
        //    #endregion
        //    #region Get ShiftWiseSummerys
        //    if (oRptDailyLogReports.Any())
        //    {
        //        string subQuery = "SELECT FBPID FROM FabricBatchProduction WHERE WeavingProcess=3";
        //        if (tsuid > 0)
        //            subQuery += " AND TSUID=" + tsuid;

        //        sSql = "SELECT * FROM View_FabricBatchProductionBatchMan WHERE FinishDate='" + dDate.AddDays(-1).ToString("dd MMM yyyy") + "' AND FBPID IN (" + subQuery + ")";
        //        oShiftWiseSummerys = FabricBatchProductionBatchMan.GetsBySql(sSql, (int)((User)Session[SessionInfo.CurrentUser]).UserID);
        //    }

        //    #endregion

        //    #region Get All Machine
        //    if (oRptDailyLogReports.Any())
        //    {
        //        sSql = "SELECT (SELECT MAX(RPM) FROM FabricBatchProductionBatchMan WHERE FBPID = FBP.FBPID) AS RPM,(SELECT MAX(Efficiency) FROM FabricBatchProductionBatchMan WHERE FBPID = FBP.FBPID) AS Efficiency,(SELECT SUM(Qty) FROM FabricBatchProductionBatchMan WHERE FBPID = FBP.FBPID) AS Qty,TSUID FROM FabricBatchProduction AS FBP WHERE WeavingProcess=3 AND EndTime IS NULL AND  FBPID IN (" + string.Join(",", oRptDailyLogReports.Select(x => x.FBPID)) + ")";

        //        oSummeryProductions = FabricBatchProduction.GetsSummery(sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    }
        //    string sSQL = "Select * from View_FabricMachine Where WeavingProcess=3 And IsBeam=0 And IsActive=1";
        //    if (tsuid > 0)
        //        sSQL += " AND TSUID=" + tsuid;

        //    List<FabricMachine> oFabricMachines = new List<FabricMachine>();
        //    oFabricMachines = FabricMachine.Gets(sSQL, nUserID);

        //    List<FabricMachine> oPatternOuts = new List<FabricMachine>();
        //    if (oRptDailyLogReports.Any())
        //    {

        //        sSql = "SELECT * FROM View_FabricMachine WHERE WeavingProcess=3 AND IsActive=1 AND IsBeam=0";
        //        if (!string.IsNullOrEmpty(oRptDailyLogReports[0].StopLoomFMIDs))
        //        {
        //            sSql += "AND FMID NOT IN (" + oRptDailyLogReports[0].StopLoomFMIDs + ")";
        //        }

        //        if (!string.IsNullOrEmpty(oRptDailyLogReports[0].RunLoomFMIDs))
        //        {
        //            sSql += "AND FMID NOT IN (" + oRptDailyLogReports[0].RunLoomFMIDs + ")";
        //        }
        //        if (tsuid > 0)
        //            sSql += " AND TSUID=" + tsuid;
        //        oPatternOuts = FabricMachine.Gets(sSql, nUserID);
        //    }



        //    #endregion
        //    #region Summery
           
        //    if (oRptDailyLogReports.Any())
        //    {
                
        //        string Subquery = "SELECT FBPID FROM FabricBatchProduction WHERE WeavingProcess=3";
        //        if (tsuid > 0)
        //            Subquery += " AND TSUID=" + tsuid;

        //        sSql = "SELECT * FROM View_FabricBatchProductionBatchMan WHERE FinishDate='" + dDate.AddDays(-1).ToString("dd MMM yyyy") + "' AND ShiftID IN (1,2,3) AND FBPID IN (" + Subquery + ")";
        //        orptSUmmeryForProds = FabricBatchProductionBatchMan.GetsBySql(sSql, (int)((User)Session[SessionInfo.CurrentUser]).UserID);
        //    }

        //    #endregion

        //   List<RptDailyLogReport> summarys = oRptDailyLogReports.GroupBy(x => x.FEOID, (key, grp) => new RptDailyLogReport
        //    {
        //        FEOID = key,
        //        FEONo = grp.First().FEONo,
        //        OrderType = grp.First().OrderType,
        //        IsInHouse = grp.First().IsInHouse,
        //        Construction = grp.First().Construction,
        //        BuyerID = grp.First().BuyerID,
        //        BuyerName = grp.First().BuyerName,
        //        IsYarnDyed = grp.First().IsYarnDyed,
        //        FabricWeave = grp.First().FabricWeave,
        //        FabricWeaveName = grp.First().FabricWeaveName,
        //        ProcessType = grp.First().ProcessType,
        //        TotalEnds = grp.First().TotalEnds,
        //        WarpColor = grp.First().WarpColor,
        //        WeftColor = grp.First().WeftColor,
        //        WarpLot = grp.First().WarpLot,
        //        WeftLot = grp.First().WeftLot,
        //        Remark = grp.First().Remark,

        //        StopLoomNo = MachineCodeSetup.GenerateCode(grp.First().StopLoomNo.Split('~').ToList().Where(m => !string.IsNullOrEmpty(m.Trim())).ToArray()),
        //        StopLoom = grp.First().StopLoomNo.Split('~').ToList().Where(m => !string.IsNullOrEmpty(m.Trim())).Count(),
        //        RunLoom = grp.Select(p => p.MachineCode).Distinct().Count(),
        //        StopLoomString = grp.First().StopLoomNo,
        //        DailyLogs = grp.GroupBy(p => new { p.ReedCount, p.Dent }).Select(g => new RptDailyLogReport
        //        {
        //            ReedCount = g.Key.ReedCount,
        //            Dent = g.Key.Dent,
        //            MachineCode = string.Join("~", g.Select(m => m.MachineCode).Distinct()),
        //            RunLoom = g.Select(m => m.MachineCode).Distinct().Count()
        //        }).ToList()

        //    }).ToList();
          

        //    rptDailyLogsReportPrintPDF oReport = new rptDailyLogsReportPrintPDF();
        //    byte[] abytes = oReport.PrepareReport(summarys, oCompany, oPatternOuts, oFabricMachines, oCommingBeams, dDate, oShiftWiseSummerys, orptSUmmeryForProds,oRptDailyLogReports, "Daily Log Report");
        //    return File(abytes, "application/pdf");
        //}

        //public ExcelPackage DailyLogsReportInExcel(int tsuid, string sParams, bool bIsMail)
        //{
        //    //Log Report Production Summery Will be calculated 01 date back as per ATML Req.(05 Apr 2018)
        //    long nUserID = (bIsMail) ? 0 : ((User)Session[SessionInfo.CurrentUser]).UserID;
        //    Company oCompany = new Company();
        //    oCompany = oCompany.Get(1, nUserID);

        //    #region Get List
        //    List<RptDailyLogReport> oRptDailyLogReports = new List<RptDailyLogReport>();
        //    //List<FabricBatchProduction> oCommingBeams = new List<FabricBatchProduction>();
        //    List<FabricBatchProductionBatchMan> oShiftWiseSummerys= new List<FabricBatchProductionBatchMan>();
        //    List<FabricBatchProductionBatchMan> orptSUmmerys = new List<FabricBatchProductionBatchMan>();
        //    List<FabricBatchProduction> oSummeryProductions = new List<FabricBatchProduction>();
        //    List<FabricBatchProductionBeam> oCommingBeams = new List<FabricBatchProductionBeam>();

        //    //List<FabricBatchProduction> oSummeryProductionShift1 = new List<FabricBatchProduction>();
        //    //List<FabricBatchProduction> oSummeryProductionShift2 = new List<FabricBatchProduction>();
        //    //List<FabricBatchProduction> oSummeryProductionShift3 = new List<FabricBatchProduction>();

        //    List<FabricBatchProductionBatchMan> orptSUmmeryForProds = new List<FabricBatchProductionBatchMan>();

        //    DateTime dDate = Convert.ToDateTime(sParams.Split('~')[0]);
        //    string sFEOIDs = Convert.ToString(sParams.Split('~')[1]);
        //    string sBuyerIDs = Convert.ToString(sParams.Split('~')[2]);
        //    int nFabricWeave = Convert.ToInt32(sParams.Split('~')[3]);
        //    int nProcessType = Convert.ToInt32(sParams.Split('~')[4]);
        //    string sConstruction = Convert.ToString(sParams.Split('~')[5]);
        //    double nReedCount = 0;
        //    if(!string.IsNullOrEmpty(sParams.Split('~')[6]))
        //        nReedCount = Convert.ToDouble(sParams.Split('~')[6]);
        //    double nYetToWeqave = Convert.ToDouble(sParams.Split('~')[7]);

        //    oRptDailyLogReports = RptDailyLogReport.Gets(dDate, sFEOIDs, sBuyerIDs, nFabricWeave, nProcessType, sConstruction, tsuid,nReedCount, nUserID);
        //    #endregion
        //    string sSql =string.Empty;
        //    #region Get Coming Loom
        //    if (oRptDailyLogReports.Any())
        //    {
        //        string sQuery = "";
        //        //sSql = "SELECT tab.* FROM (SELECT FBP.*,(SELECT SUM(QTY) FROM FabricBatchProductionBeam WHERE FBPID=FBP.FBPID) AS InQty,ISNULL((SELECT SUM(ISNULL(QTY,0)) FROM FabricBatchProductionBatchMan WHERE FBPID=FBP.FBPID),0) AS OutQty FROM view_FabricBatchProduction FBP WHERE FBP.WeavingProcess=3 AND FBP.EndTime IS NULL AND FBID IN (SELECT FBID FROM FabricBatch WHERE FEOID IN (" + string.Join(",", oRptDailyLogReports.Select(x => x.FEOID)) + ")) )tab WHERE (tab.InQty-tab.OutQty)<=" + nYetToWeqave;
        //        //oCommingBeams = FabricBatchProduction.Gets(sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);

        //        #region Buyer ID
        //        if (!string.IsNullOrEmpty(sBuyerIDs)) // BuyerName used for carrying BuyerIDs
        //            sQuery = sQuery + " AND BuyerID IN (" + sBuyerIDs + ") ";
        //        #endregion

        //        #region textile subunit
        //        if (tsuid > 0)
        //            sQuery = sQuery + " AND TSUID =" + tsuid;
        //        #endregion
        //        sQuery = sQuery + " AND FEOID IN (" + string.Join(",", oRptDailyLogReports.Select(x => x.FEOID)) + ") ";

        //        sSql = "SELECT * FROM (SELECT * FROM View_FabricBatchProductionBeam ReadyLBeam WHERE  ReadyLBeam.FBStatus != 14 "//AND ReadyLBeam.MachineStatus= 1
        //                + sQuery
        //                + " AND ReadyLBeam.IsFinish=1 AND ReadyLBeam.WeavingProcessType=1 AND ReadyLBeam.BeamID NOT IN ("
        //                + " SELECT LBeam.BeamID FROM View_FabricBatchProductionBeam LBeam WHERE LBeam.WeavingProcessType=3 AND LBeam.FBID=ReadyLBeam.FBID)"
        //                + " UNION"
        //                + " SELECT * FROM View_FabricBatchProductionBeam RunningBeam WHERE  RunningBeam.FBStatus != 14 "//AND  RunningBeam.MachineStatus= 1
        //                + sQuery
        //                + " AND RunningBeam.WeavingProcessType=3 AND RunningBeam.MachineStatus=" + (int)EnumMachineStatus.Running + " And RunningBeam.FBPID IN ("
        //                + " SELECT FBPID FROM FabricBatchProduction WHERE EndTime IS NULL AND WeavingProcess=3)) AS F ORDER BY CONVERT(INT,dbo.fnGetNumberFromString(F.MachineCode)) ASC";


        //    //   sSql = "SELECT * FROM View_FabricBatchProductionBeam WHERE WeavingProcess=3 AND FEOID IN (" + string.Join(",", oRptDailyLogReports.Select(x => x.FEOID)) + ")";
        //        oCommingBeams = FabricBatchProductionBeam.Gets(sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //         oCommingBeams = oCommingBeams.Where(x => x.YetWeaveQtyInMtr <= nYetToWeqave && (int)x.WeavingProcessType==3).ToList();
        //    }
        //    #endregion
        //    #region Get ShiftWiseSummerys
        //    if (oRptDailyLogReports.Any())
        //    {
        //        string subQuery = "SELECT FBPID FROM FabricBatchProduction WHERE WeavingProcess=3";
        //        if (tsuid > 0)
        //            subQuery += " AND TSUID=" + tsuid;

        //        sSql = "SELECT * FROM View_FabricBatchProductionBatchMan WHERE FinishDate='" + dDate.AddDays(-1).ToString("dd MMM yyyy") + "' AND FBPID IN (" + subQuery + ")";
        //        oShiftWiseSummerys = FabricBatchProductionBatchMan.GetsBySql(sSql, (int)((User)Session[SessionInfo.CurrentUser]).UserID);
        //    }
           
        //    #endregion

        //    #region Get All Machine
        //    if (oRptDailyLogReports.Any())
        //    {
        //        sSql = "SELECT (SELECT MAX(RPM) FROM FabricBatchProductionBatchMan WHERE FBPID = FBP.FBPID) AS RPM,(SELECT MAX(Efficiency) FROM FabricBatchProductionBatchMan WHERE FBPID = FBP.FBPID) AS Efficiency,(SELECT SUM(Qty) FROM FabricBatchProductionBatchMan WHERE FBPID = FBP.FBPID) AS Qty,TSUID FROM FabricBatchProduction AS FBP WHERE WeavingProcess=3 AND EndTime IS NULL AND  FBPID IN (" + string.Join(",", oRptDailyLogReports.Select(x => x.FBPID)) + ")";

        //        oSummeryProductions = FabricBatchProduction.GetsSummery(sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    }
        //    string sSQL = "Select * from View_FabricMachine Where WeavingProcess=3 And IsBeam=0 And IsActive=1";
        //    if (tsuid > 0)
        //        sSQL += " AND TSUID=" + tsuid;

        //    List<FabricMachine> oFabricMachines = new List<FabricMachine>();
        //    oFabricMachines = FabricMachine.Gets(sSQL, nUserID);

        //    List<FabricMachine> oPatternOuts = new List<FabricMachine>();
        //    if(oRptDailyLogReports.Any()){
               
        //        sSql = "SELECT * FROM View_FabricMachine WHERE WeavingProcess=3 AND IsActive=1 AND IsBeam=0";
        //        if(!string.IsNullOrEmpty(oRptDailyLogReports[0].StopLoomFMIDs))
        //        {
        //            sSql += "AND FMID NOT IN (" + oRptDailyLogReports[0].StopLoomFMIDs+")";
        //        }

        //        if (!string.IsNullOrEmpty(oRptDailyLogReports[0].RunLoomFMIDs))
        //        {
        //            sSql += "AND FMID NOT IN (" + oRptDailyLogReports[0].RunLoomFMIDs + ")";
        //        }
        //        if (tsuid > 0)
        //            sSql += " AND TSUID=" + tsuid;
        //    oPatternOuts = FabricMachine.Gets(sSql, nUserID);
        //    }
           
          

        //    #endregion
        //    #region Summery   
        //    //if (oRptDailyLogReports.Any())
        //    //{
        //    //    sSql = "SELECT * FROM View_FabricBatchProductionBatchMan WHERE FBPID IN (SELECT FBPID FROM FabricBatchProduction WHERE  EndTime is null AND WeavingProcess=3) AND FinishDate ='"+dDate.AddDays(-1).ToString("dd MMM yyyy")+"'";
        //    //    orptSUmmerys = FabricBatchProductionBatchMan.GetsBySql(sSql, (int)((User)Session[SessionInfo.CurrentUser]).UserID);
        //    //}
        //    if (oRptDailyLogReports.Any())
        //    {
        //        //sSql = "SELECT * FROM View_FabricBatchProduction WHERE WeavingProcess=3 AND EndTime is null AND FBPID IN (SELECT FBPID FROM FabricBatchProductionBatchMan WHERE FinishDate='" + dDate.AddDays(-1).ToString("dd MMM yyyy") + "' AND ShiftID=1)";
        //        //if (tsuid > 0)
        //        //    sSql = sSql + " AND TSUID =" + tsuid;
        //        //oSummeryProductionShift1 = FabricBatchProduction.Gets(sSql, (int)((User)Session[SessionInfo.CurrentUser]).UserID);

        //        //sSql = "SELECT * FROM View_FabricBatchProduction WHERE WeavingProcess=3 AND EndTime is null AND FBPID IN (SELECT FBPID FROM FabricBatchProductionBatchMan WHERE FinishDate='" + dDate.AddDays(-1).ToString("dd MMM yyyy") + "' AND ShiftID=2)";
        //        //if (tsuid > 0)
        //        //    sSql = sSql + " AND TSUID =" + tsuid;
        //        //oSummeryProductionShift2 = FabricBatchProduction.Gets(sSql, (int)((User)Session[SessionInfo.CurrentUser]).UserID);

        //        //sSql = "SELECT * FROM View_FabricBatchProduction WHERE WeavingProcess=3 AND EndTime is null AND FBPID IN (SELECT FBPID FROM FabricBatchProductionBatchMan WHERE FinishDate='" + dDate.AddDays(-1).ToString("dd MMM yyyy") + "' AND ShiftID=3)";
        //        //if (tsuid > 0)
        //        //    sSql = sSql + " AND TSUID =" + tsuid;

              
        //       // oSummeryProductionShift3 = FabricBatchProduction.Gets(sSql, (int)((User)Session[SessionInfo.CurrentUser]).UserID);


        //        //    SELECT * FROM View_FabricBatchProductionBatchMan WHERE FinishDate='04 Apr 2018' AND FBPID IN (
        //        //SELECT FBPID FROM FabricBatchProduction WHERE WeavingProcess=3 AND TSUID=1) 
        //        //AND ShiftID IN (1,2,3)
        //        string Subquery ="SELECT FBPID FROM FabricBatchProduction WHERE WeavingProcess=3";
        //         if (tsuid > 0)
        //            Subquery += " AND TSUID=" + tsuid;

        //         sSql = "SELECT * FROM View_FabricBatchProductionBatchMan WHERE FinishDate='" + dDate.AddDays(-1).ToString("dd MMM yyyy") + "' AND ShiftID IN (1,2,3) AND FBPID IN (" + Subquery + ")";
        //        orptSUmmeryForProds = FabricBatchProductionBatchMan.GetsBySql(sSql, (int)((User)Session[SessionInfo.CurrentUser]).UserID);
        //    }
           
        //    #endregion
        //    int nMaxColumn = 0;
        //    int colIndex = 2;
        //    int rowIndex = 2;
        //    ExcelRange cell;
        //    ExcelFill fill;
        //    OfficeOpenXml.Style.Border border;

        //    var excelPackage = new ExcelPackage();

        //    excelPackage.Workbook.Properties.Author = "ESimSol";
        //    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
        //    var sheet = excelPackage.Workbook.Worksheets.Add("Daily Log Report");
        //    sheet.Name = "Daily Log Report";

        //    sheet.Column(2).Width = 8; //SL
        //    sheet.Column(3).Width = 18; //Construction
        //    sheet.Column(4).Width = 20; //Buyer
        //    sheet.Column(5).Width = 10; //Total Ends
        //    sheet.Column(6).Width = 10; //Ref.no
        //    sheet.Column(7).Width = 28; //Order No
        //    sheet.Column(8).Width = 14; //Weave
        //    sheet.Column(9).Width = 10; //Reed-Count
        //    sheet.Column(10).Width = 25; //Running Loom
        //    sheet.Column(11).Width = 25; //Stop Loom
        //    sheet.Column(12).Width = 10; //Total-Loom
        //    sheet.Column(13).Width = 10; //Run-Loom
        //    sheet.Column(14).Width = 10; //Stop-Loom
        //    sheet.Column(15).Width = 15; //Remarks
        //    sheet.Column(16).Width = 15; //Remarks
        //    sheet.Column(17).Width = 20; //Remarks
        //    nMaxColumn = 17;

        //    #region Report Header
        //    sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
        //    cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
        //    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    rowIndex = rowIndex + 1;

        //    sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
        //    cell = sheet.Cells[rowIndex, 2]; cell.Value = "Daily Log Report"; cell.Style.Font.Bold = true;
        //    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    rowIndex = rowIndex + 1;

        //    sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
        //    cell = sheet.Cells[rowIndex, 2]; cell.Value = "Date : " + dDate.ToString("dd MMM yyyy"); cell.Style.Font.Bold = true;
        //    cell.Style.Font.Size = 14; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    rowIndex = rowIndex + 2;

        //    if (tsuid > 0 && oRptDailyLogReports.Any() && oRptDailyLogReports.First().TSUID > 0)
        //    {
        //        sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
        //        cell = sheet.Cells[rowIndex, 2]; cell.Value = oRptDailyLogReports.First().TSUName; cell.Style.Font.Bold = true;
        //        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //        rowIndex++;
        //    }
        //    #endregion

        //    #region Table Header
        //    colIndex = 2;
        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "SL"; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Construction"; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Buyer"; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Total Ends"; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Ref.no"; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Order No"; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Weave"; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Reed-Count"; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Running Loom"; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Beam Finish"; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Total-Loom"; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Run-Loom"; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Stop-Loom"; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Warp Lot"; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Weft Lot"; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Remarks"; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    rowIndex++;
        //    #endregion

        //    #region Table Body
        //    int nTotalInt = 0;
        //    int nSL = 0;
        //    int totalPPI = 0;
        //    var summary = oRptDailyLogReports.GroupBy(x => x.FEOID, (key, grp) => new RptDailyLogReport
        //    {
        //        FEOID=key,
        //        FEONo = grp.First().FEONo,
        //        OrderType = grp.First().OrderType,
        //        IsInHouse = grp.First().IsInHouse,
        //        Construction = grp.First().Construction,
        //        BuyerID = grp.First().BuyerID,
        //        BuyerName = grp.First().BuyerName,
        //        IsYarnDyed = grp.First().IsYarnDyed,
        //        FabricWeave = grp.First().FabricWeave,
        //        FabricWeaveName = grp.First().FabricWeaveName,
        //        ProcessType = grp.First().ProcessType,
        //        TotalEnds = grp.First().TotalEnds,
        //        WarpColor = grp.First().WarpColor,
        //        WeftColor = grp.First().WeftColor,
        //        WarpLot=grp.First().WarpLot,
        //        WeftLot = grp.First().WeftLot,
        //        Remark=grp.First().Remark,

        //        StopLoomNo = MachineCodeSetup.GenerateCode(grp.First().StopLoomNo.Split('~').ToList().Where(m=>!string.IsNullOrEmpty(m.Trim())).ToArray()),
        //        StopLoom=grp.First().StopLoomNo.Split('~').ToList().Where(m=>!string.IsNullOrEmpty(m.Trim())).Count(),
        //        RunLoom = grp.Select(p=>p.MachineCode).Distinct().Count(),
        //        StopLoomString=grp.First().StopLoomNo,
        //        DailyLogs = grp.GroupBy(p => new {p.ReedCount, p.Dent}).Select(g => new RptDailyLogReport
        //        {
        //            ReedCount = g.Key.ReedCount,
        //            Dent = g.Key.Dent,
        //            MachineCode = string.Join("~", g.Select(m=>m.MachineCode).Distinct()),
        //            RunLoom = g.Select(m => m.MachineCode).Distinct().Count()
        //        }).ToList()

        //    }).ToList().OrderBy(p=>p.IsInHouse);


        //    #region Exe Order
        //    var oExeYarnDyed = summary.Where(x => x.IsInHouse && x.IsYarnDyed).OrderBy(x=>x.BuyerName).ToList();

        //    if (oExeYarnDyed.Any())
        //    {
        //        cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn]; cell.Merge = true; cell.Value = "Exe-Y/D"; cell.Style.Font.Bold = true;
        //        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; 
        //        rowIndex++;
        //        GenerateDailyLogsReport(oExeYarnDyed, ref nSL, ref rowIndex, ref sheet, ref cell);

        //    }


        //    var oExeSolidDyed = summary.Where(x => x.IsInHouse && !x.IsYarnDyed).OrderBy(x => x.BuyerName).ToList();

        //    if (oExeSolidDyed.Any())
        //    {
        //        cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn]; cell.Merge = true; cell.Value = "Exe-S/D"; cell.Style.Font.Bold = true;
        //        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //        rowIndex++;
        //        GenerateDailyLogsReport(oExeSolidDyed, ref nSL, ref rowIndex, ref sheet, ref cell);

        //    }


        //    #endregion

        //    #region Sub Contact
        //    var oScwYarnDyed = summary.Where(x => !x.IsInHouse && x.IsYarnDyed).OrderBy(x => x.BuyerName).ToList();
        //    if (oScwYarnDyed.Any())
        //    {
        //        cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn]; cell.Merge = true; cell.Value = "SCW-Y/D"; cell.Style.Font.Bold = true;
        //        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //        rowIndex++;
        //        GenerateDailyLogsReport(oScwYarnDyed, ref nSL, ref rowIndex, ref sheet, ref cell);

        //    }

        //    var oScwSolidDyed = summary.Where(x => !x.IsInHouse && !x.IsYarnDyed).OrderBy(x => x.BuyerName).ToList();
        //    if (oScwSolidDyed.Any())
        //    {
        //        cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn]; cell.Merge = true; cell.Value = "SCW-S/D"; cell.Style.Font.Bold = true;
        //        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //        rowIndex++;
        //        GenerateDailyLogsReport(oScwSolidDyed, ref nSL, ref rowIndex, ref sheet, ref cell);

        //    }
        //    #endregion


        //    #endregion

        //    #region Exe Total
        //    colIndex = 2;

        //    cell = sheet.Cells[rowIndex, colIndex, rowIndex, 11]; cell.Merge = true; cell.Value = "Total(EXE-Y/D & EXE-S/D) "; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    colIndex = 12;
        //    nTotalInt = summary.Sum(x => x.TotalLoom);
        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nTotalInt; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    nTotalInt = summary.Sum(x => x.RunLoom);
        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nTotalInt; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    nTotalInt = summary.Sum(x => x.StopLoom);
        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nTotalInt; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    rowIndex++;
        //    #endregion

        //    #region Beam Finished
        //    colIndex = 2;

           
        //     cell = sheet.Cells[rowIndex, colIndex, rowIndex, 11]; cell.Merge = true; cell.Value = "Beam Finished "; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    colIndex = 12;
        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nTotalInt; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex, rowIndex, 17]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    rowIndex++;
        //    #endregion

        //    #region Beam Finished
        //    colIndex = 2;
        //    cell = sheet.Cells[rowIndex, colIndex, rowIndex, 11]; cell.Merge = true; cell.Value = "Pattern Out "; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    colIndex = 12;
        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oPatternOuts.Count(); cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex, rowIndex, 17]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    rowIndex++;
        //    #endregion
           
        //    #region Grand Total
        //    colIndex = 2;

        //    cell = sheet.Cells[rowIndex, colIndex, rowIndex, 11]; cell.Merge = true; cell.Value = "Grand Total "; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    colIndex = 12;
        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value =summary.Sum(x => x.TotalLoom); cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oFabricMachines.Count(); cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = summary.Sum(x => x.StopLoom); cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, colIndex, rowIndex, 17]; cell.Merge = true; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    rowIndex++;
        //    #endregion

        //    #region Summary
        //    colIndex = 2;

        //    cell = sheet.Cells[rowIndex, colIndex, rowIndex, 3]; cell.Merge = true; cell.Value = "Coming Beam Finished ( " + oCommingBeams.Select(x=>x.MachineCode).ToList().Distinct().Count() + " )"; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, 4, rowIndex, nMaxColumn]; cell.Merge = true; cell.Value = MachineCodeSetup.GenerateCode(oCommingBeams.Select(x => x.MachineCode).ToList().Distinct().ToArray()); cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    rowIndex++;
          
        //    colIndex = 2;

        //    cell = sheet.Cells[rowIndex, colIndex, rowIndex, 3]; cell.Merge = true; cell.Value = "Beam Finish ( " +summary.Sum(x=>x.StopLoom)  + " )"; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
   
        //    cell = sheet.Cells[rowIndex, 4, rowIndex, nMaxColumn]; cell.Merge = true; cell.Value = string.Join(" ", MachineCodeSetup.GenerateCode(string.Join("~", summary.Select(x => x.StopLoomString)).Split('~').ToList().OrderBy(x => x.Trim()).Where(m => !string.IsNullOrEmpty(m.Trim())).ToArray())); cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //    rowIndex++;

        //    colIndex = 2;

        //    cell = sheet.Cells[rowIndex, colIndex, rowIndex, 3]; cell.Merge = true; cell.Value = "Pattern Out: ( " + oPatternOuts.Count()+ " )"; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, 4, rowIndex, nMaxColumn]; cell.Merge = true; cell.Value = MachineCodeSetup.GenerateCode(oPatternOuts.Select(x => x.Code).ToArray()); cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
         
          
        //    rowIndex++;
           
        //    colIndex = 2;

        //    cell = sheet.Cells[rowIndex, colIndex, rowIndex, 3]; cell.Merge = true; cell.Value =dDate.AddDays(-1).ToString("dd MMM yyyy")+ "  Shift wise Production (M) R/A-R/B-R/C:"; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, 4, rowIndex, 4]; cell.Value = oShiftWiseSummerys.Where(x => x.ShiftID == 1).Select(x => x.QtyinMeter).Sum(); cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, 5, rowIndex, 5]; cell.Value = oShiftWiseSummerys.Where(x => x.ShiftID == 2).Select(x => x.QtyinMeter).Sum(); cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, 6, rowIndex, 6]; cell.Value = oShiftWiseSummerys.Where(x => x.ShiftID == 3).Select(x => x.QtyinMeter).Sum(); cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //    rowIndex++;


        //    colIndex = 2;

        //    cell = sheet.Cells[rowIndex, colIndex, rowIndex, nMaxColumn]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //    rowIndex++;
        //    cell = sheet.Cells[rowIndex, colIndex, rowIndex, nMaxColumn]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //    rowIndex++;
        //    var summaryForTSU1 = oRptDailyLogReports.Where(x => x.TSUID == 1).ToList().GroupBy(x => x.FEOID, (key, grp) => new RptDailyLogReport
        //    {
        //        FEOID = key,
        //        FEONo = grp.First().FEONo,
        //        OrderType = grp.First().OrderType,
        //        IsInHouse = grp.First().IsInHouse,
        //        Construction = grp.First().Construction,
        //        BuyerID = grp.First().BuyerID,
        //        BuyerName = grp.First().BuyerName,
        //        IsYarnDyed = grp.First().IsYarnDyed,
        //        FabricWeave = grp.First().FabricWeave,
        //        FabricWeaveName = grp.First().FabricWeaveName,
        //        ProcessType = grp.First().ProcessType,
        //        TotalEnds = grp.First().TotalEnds,
        //        WarpColor = grp.First().WarpColor,
        //        WeftColor = grp.First().WeftColor,
        //        StopLoomNo = MachineCodeSetup.GenerateCode(grp.First().StopLoomNo.Split('~').ToList().Where(m => !string.IsNullOrEmpty(m.Trim())).ToArray()),
        //        StopLoom = grp.First().StopLoomNo.Split('~').ToList().Where(m => !string.IsNullOrEmpty(m.Trim())).Count(),
        //        RunLoom = grp.Select(p => p.MachineCode).Distinct().Distinct().Count(),
        //        DailyLogs = grp.GroupBy(p => new { p.ReedCount, p.Dent }).Select(g => new RptDailyLogReport
        //        {
        //            ReedCount = g.Key.ReedCount,
        //            Dent = g.Key.Dent,
        //            MachineCode = string.Join("~", g.Select(m => m.MachineCode).Distinct()),
        //            RunLoom = g.Select(m => m.MachineCode).Distinct().Count()
        //        }).ToList()

        //    }).ToList();

        //    var summaryForTSU2 = oRptDailyLogReports.Where(x => x.TSUID == 2).ToList().GroupBy(x => x.FEOID, (key, grp) => new RptDailyLogReport
        //    {
        //        FEOID = key,
        //        FEONo = grp.First().FEONo,
        //        OrderType = grp.First().OrderType,
        //        IsInHouse = grp.First().IsInHouse,
        //        Construction = grp.First().Construction,
        //        BuyerID = grp.First().BuyerID,
        //        BuyerName = grp.First().BuyerName,
        //        IsYarnDyed = grp.First().IsYarnDyed,
        //        FabricWeave = grp.First().FabricWeave,
        //        FabricWeaveName = grp.First().FabricWeaveName,
        //        ProcessType = grp.First().ProcessType,
        //        TotalEnds = grp.First().TotalEnds,
        //        WarpColor = grp.First().WarpColor,
        //        WeftColor = grp.First().WeftColor,
        //        StopLoomNo = MachineCodeSetup.GenerateCode(grp.First().StopLoomNo.Split('~').ToList().Where(m => !string.IsNullOrEmpty(m.Trim())).ToArray()),
        //        StopLoom = grp.First().StopLoomNo.Split('~').ToList().Where(m => !string.IsNullOrEmpty(m.Trim())).Distinct().Count(),
        //        RunLoom = grp.Select(p => p.MachineCode).Distinct().Count(),
        //        DailyLogs = grp.GroupBy(p => new { p.ReedCount, p.Dent }).Select(g => new RptDailyLogReport
        //        {
        //            ReedCount = g.Key.ReedCount,
        //            Dent = g.Key.Dent,
        //            MachineCode = string.Join("~", g.Select(m => m.MachineCode).Distinct()),
        //            RunLoom = g.Select(m => m.MachineCode).Distinct().Count()
        //        }).ToList()

        //    }).ToList();


        //    colIndex = 4;
        //    cell = sheet.Cells[rowIndex, colIndex, rowIndex,8]; cell.Merge = true; cell.Value = "Summary For Production( "+dDate.AddDays(-1).ToString("dd MMM yyyy")+" )"; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGreen);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //    rowIndex++;

        //    colIndex = 4;
        //    cell = sheet.Cells[rowIndex, colIndex, rowIndex, 5]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, 6, rowIndex, 6]; cell.Merge = true; cell.Value = "Shade-1"; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, 7, rowIndex, 7]; cell.Merge = true; cell.Value = "Shade-2"; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, 8, rowIndex, 8]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //    rowIndex++;
          
        //    colIndex = 4;
        //    cell = sheet.Cells[rowIndex, colIndex, rowIndex, 5]; cell.Merge = true; cell.Value = "Total Production (M) ="; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, 6, rowIndex, 6]; cell.Merge = false; cell.Value = Convert.ToInt32(oShiftWiseSummerys.Where(x => x.TSUID == 1).Sum(x => x.QtyInM)); ; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, 7, rowIndex, 7]; cell.Merge = false; cell.Value = Convert.ToInt32(oShiftWiseSummerys.Where(x => x.TSUID == 2).Sum(x => x.QtyInM)); cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, 8, rowIndex, 8]; cell.Merge = false; cell.Value = Convert.ToInt32(oShiftWiseSummerys.Sum(x => x.QtyInM)); cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //    rowIndex++;
            


        //    //int TSU1ShiftCount = 0, TSU2ShiftCount = 0;
        //    //int AShiftRunLoomTSU1 = oSummeryProductionShift1.Where(x => x.TSUID == 1).ToList().Count();
        //    //int BShiftRunLoomTSU1 = oSummeryProductionShift2.Where(x => x.TSUID == 1 ).ToList().Count();
        //    //int CShiftRunLoomTSU1 = oSummeryProductionShift3.Where(x => x.TSUID == 1 ).ToList().Count();

        //    //int AShiftRunLoomTSU2 = oSummeryProductionShift1.Where(x => x.TSUID == 2).ToList().Count();
        //    //int BShiftRunLoomTSU2 = oSummeryProductionShift2.Where(x => x.TSUID == 2).ToList().Count();
        //    //int CShiftRunLoomTSU2 = oSummeryProductionShift3.Where(x => x.TSUID == 2 ).ToList().Count();
          
        //    //if (AShiftRunLoomTSU1 > 0)
        //    //    TSU1ShiftCount = TSU1ShiftCount + 1;
        //    //if (BShiftRunLoomTSU1 > 0)
        //    //    TSU1ShiftCount = TSU1ShiftCount + 1;
        //    //if (CShiftRunLoomTSU1 > 0)
        //    //    TSU1ShiftCount = TSU1ShiftCount + 1;

        //    //if (AShiftRunLoomTSU2 > 0)
        //    //    TSU2ShiftCount = TSU2ShiftCount + 1;
        //    //if (BShiftRunLoomTSU2 > 0)
        //    //    TSU2ShiftCount = TSU2ShiftCount + 1;
        //    //if (CShiftRunLoomTSU2 > 0)
        //    //    TSU2ShiftCount = TSU2ShiftCount + 1;
        //    //int Shade1avgRUnLoom = 0,Shade2avgRUnLoom=0;
        //    //if(TSU1ShiftCount>0)
        //    //    Shade1avgRUnLoom =Convert.ToInt32((AShiftRunLoomTSU1+BShiftRunLoomTSU1+CShiftRunLoomTSU1)/TSU1ShiftCount);
        //    //if(TSU2ShiftCount>0)
        //    //    Shade2avgRUnLoom =Convert.ToInt32((AShiftRunLoomTSU2+BShiftRunLoomTSU2+CShiftRunLoomTSU2)/TSU2ShiftCount);

        //    //double ttlRLoom1 = AShiftRunLoomTSU1 + BShiftRunLoomTSU1 + CShiftRunLoomTSU1;
        //    //double ttlRLoom2 = AShiftRunLoomTSU2 + BShiftRunLoomTSU2 + CShiftRunLoomTSU2;
        //    double val1 = summaryForTSU1.Sum(x => x.RunLoom);
        //    double val2 = summaryForTSU2.Sum(x => x.RunLoom);
        //    int nShiftCountForShade1 = orptSUmmeryForProds.Where(x => x.TSUID == 1).Select(x => x.ShiftID).Distinct().Count();
        //    int nShiftCountForShade2 = orptSUmmeryForProds.Where(x => x.TSUID == 2).Select(x => x.ShiftID).Distinct().Count();
        //    int nShiftCountForShade12 = orptSUmmeryForProds.Select(x => x.ShiftID).Distinct().Count();
        //    double Shade1AvgRunLoom=(nShiftCountForShade1>0)?Math.Round((double)orptSUmmeryForProds.Where(x => x.TSUID == 1).ToList().Count()/(double)nShiftCountForShade1,0):0;
        //    double Shade2AvgRunLoom=(nShiftCountForShade2>0)?Math.Round((double)orptSUmmeryForProds.Where(x => x.TSUID == 2).ToList().Count()/(double)nShiftCountForShade2,0):0;
        //     double Shade12AvgRunLoom=(nShiftCountForShade12>0)?Math.Round((double)orptSUmmeryForProds.ToList().Count()/(double)nShiftCountForShade12,0):0;
        //     int nShade1RunningLoom = orptSUmmeryForProds.Where(x => x.TSUID == 1).ToList().Count();
        //     int nShade2RunningLoom = orptSUmmeryForProds.Where(x => x.TSUID == 2).ToList().Count();
        //     int nShade12RunningLoom = orptSUmmeryForProds.ToList().Count();
        //    colIndex = 4;
        //    cell = sheet.Cells[rowIndex, colIndex, rowIndex, 5]; cell.Merge = true; cell.Value = "Per Shift avg. Run Loom ="; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
           
        //    cell = sheet.Cells[rowIndex, 6, rowIndex, 6]; cell.Merge = false; cell.Value = Shade1AvgRunLoom; ; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
           
        //    cell = sheet.Cells[rowIndex, 7, rowIndex, 7]; cell.Merge = false; cell.Value = Shade2AvgRunLoom; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
    
        //    cell = sheet.Cells[rowIndex, 8, rowIndex, 8]; cell.Merge = false; cell.Value = Shade12AvgRunLoom; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //    rowIndex++;
           
        //    colIndex = 4;
        //    cell = sheet.Cells[rowIndex, colIndex, rowIndex, 5]; cell.Merge = true; cell.Value = "Avg. Pick ="; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //    double avgPickTSU1 = (val1 != 0) ? Math.Round(summaryForTSU1.Sum(x => x.PPI * x.RunLoom) / val1,0) : 0;
        //    cell = sheet.Cells[rowIndex, 6, rowIndex, 6]; cell.Merge = false; cell.Value = avgPickTSU1; ; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //    double avgPickTSU2 = (val2 != 0) ? Math.Round(summaryForTSU2.Sum(x => x.PPI*x.RunLoom) / val2,0) : 0;
        //    cell = sheet.Cells[rowIndex, 7, rowIndex, 7]; cell.Merge = false; cell.Value = avgPickTSU2; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


        //    double avgPick = (summary.Sum(x => x.RunLoom) != 0) ? summary.Sum(x => x.PPI * x.RunLoom) / summary.Sum(x => x.RunLoom) : 0;

        //    cell = sheet.Cells[rowIndex, 8, rowIndex, 8]; cell.Merge = false; cell.Value = avgPick; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //    rowIndex++;

         
        //    colIndex = 4;
        //    cell = sheet.Cells[rowIndex, colIndex, rowIndex, 5]; cell.Merge = true; cell.Value = "Avg. Eff ="; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, 6, rowIndex, 6]; cell.Merge = false; cell.Value = (nShade1RunningLoom > 0) ? Math.Round(orptSUmmeryForProds.Where(x => x.TSUID == 1).ToList().Sum(x=>x.Efficiency) / nShade1RunningLoom, 0) : 0;  cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, 7, rowIndex, 7]; cell.Merge = false; cell.Value = (nShade2RunningLoom > 0) ? Math.Round(orptSUmmeryForProds.Where(x => x.TSUID == 2).ToList().Sum(x => x.Efficiency) / nShade2RunningLoom, 0) : 0; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, 8, rowIndex, 8]; cell.Merge = false; cell.Value = (nShade12RunningLoom > 0) ? Math.Round(orptSUmmeryForProds.Sum(x => x.Efficiency) / nShade12RunningLoom, 0) : 0; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //    rowIndex++;




        //    colIndex =4;
        //    cell = sheet.Cells[rowIndex, colIndex, rowIndex, 5]; cell.Merge = true; cell.Value = "Avg. RPM ="; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, 6, rowIndex, 6]; cell.Merge = false; cell.Value = (nShade1RunningLoom > 0) ? Math.Round((double)orptSUmmeryForProds.Where(x => x.TSUID == 1).ToList().Sum(x => x.RPM) / nShade1RunningLoom, 0) : 0; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


        //    cell = sheet.Cells[rowIndex, 7, rowIndex, 7]; cell.Merge = false; cell.Value = (nShade2RunningLoom > 0) ? Math.Round((double)orptSUmmeryForProds.Where(x => x.TSUID == 2).ToList().Sum(x => x.RPM) / nShade2RunningLoom, 0) : 0; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //    cell = sheet.Cells[rowIndex, 8, rowIndex, 8]; cell.Merge = false; cell.Value = (nShade12RunningLoom > 0) ? Math.Round((double)orptSUmmeryForProds.Sum(x => x.RPM) / nShade12RunningLoom, 0) : 0; cell.Style.Font.Bold = true;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //    rowIndex++;

        //    rowIndex++;
         
        //    #endregion

        //    return excelPackage;
        //}

        #endregion

        #region Excel Convertion (Ratin)
        public void ConvertToExcel(PdfPTable oPdfPTable)
        {
            #region Variables
            int nMaxColumn = 0;
            int colIndex = 2;
            int rowIndex = 2;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;
            #endregion

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Name");
                sheet.Name = "Name";

                int nTotalRows = oPdfPTable.Rows.Count;
                if (nTotalRows > 0)
                {
                    var oFirstRowCells = oPdfPTable.Rows[0].GetCells();
                    int nTotalCellOfARow = oFirstRowCells.Length;

                    #region Cell Width Declaration
                    int nCellIndex = rowIndex - 1;
                    for (int i = 0; i < nTotalCellOfARow; i++)
                    {
                        nCellIndex++;
                        sheet.Column(nCellIndex).Width = oPdfPTable.AbsoluteWidths[i] / 4;
                    }
                    nMaxColumn = nCellIndex;
                    #endregion

                    #region Table
                    colIndex = 2;
                    string sCellValue = "";
                    //bool bIsHeader = true;
                    int nMaxColspan = oPdfPTable.Rows[0].GetCells().Count();
                    for (int i = 0; i < nTotalRows; i++)
                    {
                        colIndex = 1;
                        var oRowCells = oPdfPTable.Rows[i].GetCells();
                        for (int j = 0; j < nTotalCellOfARow; j++)
                        {
                            var oCurrentRowCells = oRowCells[j];
                            if (oCurrentRowCells != null)
                            {
                                sCellValue = (oCurrentRowCells.Phrase.Count > 0 ? oCurrentRowCells.Phrase[0].ToString() : "");
                                sCellValue = sCellValue.Replace(",", ""); //May be no need

                                #region Colspan
                                int nConspan = oCurrentRowCells.Colspan;
                                if (nConspan == 1)
                                {
                                    colIndex++;
                                    cell = sheet.Cells[rowIndex, colIndex];
                                }
                                else if (nConspan == nMaxColspan) //Check
                                {
                                    cell = sheet.Cells[rowIndex, rowIndex, rowIndex, nMaxColspan];
                                    cell.Merge = true;
                                }
                                else
                                {
                                    colIndex = colIndex + nConspan;
                                    cell = sheet.Cells[rowIndex, nConspan, rowIndex, colIndex];
                                    cell.Merge = true;
                                }
                                #endregion


                                cell.Value = sCellValue;
                                cell.Style.HorizontalAlignment = this.GetHorizontalAlignment(oCurrentRowCells.HorizontalAlignment);
                                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                cell.Style.Font.Bold = false;

                                fill = cell.Style.Fill;
                                fill.PatternType = ExcelFillStyle.Solid;
                                fill.BackgroundColor.SetColor(this.GetColorCode(oCurrentRowCells.BackgroundColor));
                                border = cell.Style.Border;
                                border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = this.GetBorder(oCurrentRowCells.Border);

                                /*
                                 Pending :
                                 * Font bold
                                 * Rowspan
                                 * Company Informations
                                 * Image Load
                                 */
                            }
                        }
                        //bIsHeader = false;
                        rowIndex++;
                    }
                    #endregion

                    #region Response Part
                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=Excel001.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                    #endregion
                }
            }
        }

        #region Functions For Excels
        private ExcelBorderStyle GetBorder(int nBorder)
        {
            if (nBorder == 15) //None
            {
                return ExcelBorderStyle.Thin;
            }
            else if (nBorder == -1) //bold not found in pdfTable
            {
                return ExcelBorderStyle.Medium;
            }
            return ExcelBorderStyle.None;
        }
        private Color GetColorCode(BaseColor myColor)
        {
            string hex = myColor.R.ToString("X2") + myColor.G.ToString("X2") + myColor.B.ToString("X2"); //In Hex
            Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#" + hex);
            return colFromHex;
        }
        private ExcelHorizontalAlignment GetHorizontalAlignment(int nHorizontalAlignment)
        {
            if (nHorizontalAlignment == 1) //Center
            {
                return ExcelHorizontalAlignment.Center;
            }
            else if (nHorizontalAlignment == 2) //Right
            {
                return ExcelHorizontalAlignment.Right;
            }
            return ExcelHorizontalAlignment.Left;
        }
        #endregion
        #endregion

        #region RptMonthlyProductionAnalyse
        //public ActionResult View_RptMonthlyProductionAnalyse(int menuid)
        //{
        //    this.Session.Remove(SessionInfo.MenuID);
        //    this.Session.Add(SessionInfo.MenuID, menuid);
        //    List<RptMonthlyProductionAnalyse> oRptMonthlyProductionAnalyses = new List<RptMonthlyProductionAnalyse>();
        //    return View(oRptMonthlyProductionAnalyses);
        //}
        //public void ExcelMonthlyProductionAnalyse(string sParams)
        //{
        //    #region Get List
        //    List<RptMonthlyProductionAnalyse> oRptMonthlyProductionAnalyses = new List<RptMonthlyProductionAnalyse>();
        //    DateTime dFromDate = Convert.ToDateTime(sParams.Split('~')[0]);
        //    DateTime dToDate = Convert.ToDateTime(sParams.Split('~')[1]);

        //    oRptMonthlyProductionAnalyses = RptMonthlyProductionAnalyse.Gets(dFromDate, dToDate, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    #endregion

        //    int nMaxColumn = 0;
        //    int colIndex = 2;
        //    int rowIndex = 2;
        //    ExcelRange cell;
        //    ExcelFill fill;
        //    OfficeOpenXml.Style.Border border;

        //    using (var excelPackage = new ExcelPackage())
        //    {
        //        excelPackage.Workbook.Properties.Author = "ESimSol";
        //        excelPackage.Workbook.Properties.Title = "Export from ESimSol";
        //        var sheet = excelPackage.Workbook.Worksheets.Add("Monthly Production Analyse Report");
        //        sheet.Name = "Monthly Production Analyse Report";

        //        int nColumnWidth = 18;
        //        sheet.Column(2).Width = nColumnWidth; //DateOfReceived
        //        sheet.Column(3).Width = nColumnWidth; //AGradeReceived
        //        sheet.Column(4).Width = nColumnWidth; //BGradeReceived
        //        sheet.Column(5).Width = nColumnWidth; //RejectReceived
        //        sheet.Column(6).Width = nColumnWidth; //TotalReceivedSt
        //        sheet.Column(7).Width = nColumnWidth; //YDGoodExeReceived
        //        sheet.Column(8).Width = nColumnWidth; //YDRejectExeReceived
        //        sheet.Column(9).Width = nColumnWidth; //SDGoodExeReceived
        //        sheet.Column(10).Width = nColumnWidth; //SDRejectExeReceived
        //        sheet.Column(11).Width = nColumnWidth; //YDGoodSample
        //        sheet.Column(12).Width = nColumnWidth; //YDRejectSample
        //        sheet.Column(13).Width = nColumnWidth; //SDGoodSample
        //        sheet.Column(14).Width = nColumnWidth; //SDRejectSample
        //        sheet.Column(15).Width = nColumnWidth; //YDGoodScw
        //        sheet.Column(16).Width = nColumnWidth; //YDRejectScw
        //        sheet.Column(17).Width = nColumnWidth; //SDGoodScw
        //        sheet.Column(18).Width = nColumnWidth; //SDRejectScw
        //        sheet.Column(19).Width = nColumnWidth; //YDGoodSampleScw
        //        sheet.Column(20).Width = nColumnWidth; //YDRejectSampleScw
        //        sheet.Column(21).Width = nColumnWidth; //SDGoodSampleScw
        //        sheet.Column(22).Width = nColumnWidth; //SDRejectSampleScw
        //        sheet.Column(23).Width = nColumnWidth; //SDAkijExtra
        //        sheet.Column(24).Width = nColumnWidth; //DevelopmentPCFabrics
        //        sheet.Column(25).Width = nColumnWidth; //Remarks
        //        nMaxColumn = 25;

        //        Company oCompany = new Company();
        //        oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //        oCompany.CompanyLogo = GetCompanyLogo(oCompany);

        //        #region Report Header
        //        sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
        //        cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
        //        cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        rowIndex = rowIndex + 1;

        //        sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
        //        cell = sheet.Cells[rowIndex, 2]; cell.Value = "Weaving Unit"; cell.Style.Font.Bold = true;
        //        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        rowIndex = rowIndex + 1;

        //        sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
        //        cell = sheet.Cells[rowIndex, 2]; cell.Value = "Monthly Production Analyse Report :"+dToDate.ToString("MMMM")+"-"+dToDate.Year; cell.Style.Font.Bold = true;
        //        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        rowIndex = rowIndex + 1;

        //        sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
        //        cell = sheet.Cells[rowIndex, 2]; cell.Value = "Date : " + dFromDate.ToString("dd MMM yyyy") + " to " + dToDate.ToString("dd MMM yyyy"); cell.Style.Font.Bold = true;
        //        cell.Style.Font.Size = 14; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        rowIndex = rowIndex + 2;
        //        #endregion

        //        #region Table Header
        //        colIndex = 2;
        //        string sVal = "";
        //        for (int i = 1; i <= nMaxColumn; i++)
        //        {
        //            if (i == 1) { sVal = "Date of Received"; }
        //            else if (i == 2) { sVal = "A-Grade Received"; }
        //            else if (i == 3) { sVal = "B-Grade Received"; }
        //            else if (i == 4) { sVal = "Reject Received"; }
        //            else if (i == 5) { sVal = "Total (Qty) Received "; }
        //            else if (i == 6) { sVal = "Good Exe(Y/D) Received"; }
        //            else if (i == 7) { sVal = "Reject Exe(Y/D) Received"; }
        //            else if (i == 8) { sVal = "Good Exe(S/D) Received"; }
        //            else if (i == 9) { sVal = "Reject Exe(S/D) Received"; }
        //            else if (i == 10) { sVal = "Good Sample (Y/D)"; }
        //            else if (i == 11) { sVal = "Reject Sample (Y/D)"; }
        //            else if (i == 12) { sVal = "Good Sample (S/D)"; }
        //            else if (i == 13) { sVal = "Reject Sample (S/D)"; }
        //            else if (i == 14) { sVal = "Good (Qty) Scw(Y/D)"; }
        //            else if (i == 15) { sVal = "Reject Scw(Y/D)"; }
        //            else if (i == 16) { sVal = "Good (Qty) Scw(S/D)"; }
        //            else if (i == 17) { sVal = "Reject Scw(S/D)"; }
        //            else if (i == 18) { sVal = "Good Sample Scw(Y/D)"; }
        //            else if (i == 19) { sVal = "Reject Sample Scw(Y/D)"; }
        //            else if (i == 20) { sVal = "Good Sample Scw(S/D)"; }
        //            else if (i == 21) { sVal = "Reject Sample Scw(S/D)"; }
        //            else if (i == 22) { sVal = "Akij Extra (S/D)"; }
        //            else if (i == 23) { sVal = "Development P.C Fabrics"; }
        //            else if (i == 24) { sVal = "Total (Qty) Received "; }
        //            else if (i == 25) { sVal = "Remarks"; }
        //            else { sVal = ""; }

        //            cell = sheet.Cells[rowIndex, colIndex++];
        //            cell.Value = sVal; 
        //            cell.Style.Font.Bold = true;
        //            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            fill = cell.Style.Fill; 
        //            fill.PatternType = ExcelFillStyle.Solid; 
        //            fill.BackgroundColor.SetColor(Color.Cyan);
        //            border = cell.Style.Border; 
        //            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //        }
        //        rowIndex++;
        //        #endregion

        //        #region Table body
        //        int nHorizontalAlignment = 0;
        //        bool IsNumberField = false;
        //        double nValue = 0;
        //        int nTotalDays = 0;
        //        if (oRptMonthlyProductionAnalyses.Count > 0)
        //        {

        //            oRptMonthlyProductionAnalyses.ForEach(x =>
        //            {
        //                x.AGradeReceived = Math.Round(Global.GetMeter(x.AGradeReceived,2));
        //                x.BGradeReceived = Math.Round(Global.GetMeter(x.BGradeReceived, 2));
        //                x.RejectReceived = Math.Round(Global.GetMeter(x.RejectReceived, 2));
        //                x.YDGoodExeReceived = Math.Round(Global.GetMeter(x.YDGoodExeReceived, 2));
        //                x.YDRejectExeReceived = Math.Round(Global.GetMeter(x.YDRejectExeReceived, 2));
        //                x.SDGoodExeReceived = Math.Round(Global.GetMeter(x.SDGoodExeReceived, 2));
        //                x.SDRejectExeReceived = Math.Round(Global.GetMeter(x.SDRejectExeReceived, 2));
        //                x.YDGoodSample = Math.Round(Global.GetMeter(x.YDGoodSample, 2));
        //                x.YDRejectSample = Math.Round(Global.GetMeter(x.YDRejectSample, 2));
        //                x.SDGoodSample = Math.Round(Global.GetMeter(x.SDGoodSample, 2));
        //                x.SDRejectSample = Math.Round(Global.GetMeter(x.SDRejectSample, 2));
        //                x.YDGoodScw = Math.Round(Global.GetMeter(x.YDGoodScw, 2));
        //                x.YDRejectScw = Math.Round(Global.GetMeter(x.YDRejectScw, 2));
        //                x.SDGoodScw = Math.Round(Global.GetMeter(x.SDGoodScw, 2));
        //                x.SDRejectScw = Math.Round(Global.GetMeter(x.SDRejectScw, 2));
        //                x.YDGoodSampleScw = Math.Round(Global.GetMeter(x.YDGoodSampleScw, 2));
        //                x.YDRejectSampleScw = Math.Round(Global.GetMeter(x.YDRejectSampleScw, 2));
        //                x.SDGoodSampleScw = Math.Round(Global.GetMeter(x.SDGoodSampleScw, 2));
        //                x.SDRejectSampleScw = Math.Round(Global.GetMeter(x.SDRejectSampleScw, 2));
        //                x.SDAkijExtra = Math.Round(Global.GetMeter(x.SDAkijExtra, 2));
        //                x.DevelopmentPCFabrics = Math.Round(Global.GetMeter(x.DevelopmentPCFabrics, 2)); 

        //            });

        //            foreach (RptMonthlyProductionAnalyse oItem in oRptMonthlyProductionAnalyses)
        //            {

        //                colIndex = 2;
        //                if (oItem.TotalReceived > 0)
        //                    nTotalDays++;
        //                for (int i = 1; i <= nMaxColumn; i++)
        //                {
        //                    nHorizontalAlignment = 0;
        //                    IsNumberField = false;
        //                    nValue = 0;
                           
        //                    if (i == 1) { sVal = oItem.DateOfReceived.ToString("dd MMM yyyy"); nHorizontalAlignment = 1; }
        //                    else if (i == 2) { nValue = oItem.AGradeReceived; IsNumberField = true; }
        //                    else if (i == 3) { nValue = oItem.BGradeReceived; IsNumberField = true; }
        //                    else if (i == 4) { nValue = oItem.RejectReceived; IsNumberField = true; }
        //                    else if (i == 5) { nValue = oItem.TotalReceived; IsNumberField = true; }
        //                    else if (i == 6) { nValue = oItem.YDGoodExeReceived; IsNumberField = true; }
        //                    else if (i == 7) { nValue = oItem.YDRejectExeReceived; IsNumberField = true; }
        //                    else if (i == 8) { nValue = oItem.SDGoodExeReceived; IsNumberField = true; }
        //                    else if (i == 9) { nValue = oItem.SDRejectExeReceived; IsNumberField = true; }
        //                    else if (i == 10) { nValue = oItem.YDGoodSample; IsNumberField = true; }
        //                    else if (i == 11) { nValue = oItem.YDRejectSample; IsNumberField = true; }
        //                    else if (i == 12) { nValue = oItem.SDGoodSample; IsNumberField = true; }
        //                    else if (i == 13) { nValue = oItem.SDRejectSample; IsNumberField = true; }
        //                    else if (i == 14) { nValue = oItem.YDGoodScw; IsNumberField = true; }
        //                    else if (i == 15) { nValue = oItem.YDRejectScw; IsNumberField = true; }
        //                    else if (i == 16) { nValue = oItem.SDGoodScw; IsNumberField = true; }
        //                    else if (i == 17) { nValue = oItem.SDRejectScw; IsNumberField = true; }
        //                    else if (i == 18) { nValue = oItem.YDGoodSampleScw; IsNumberField = true; }
        //                    else if (i == 19) { nValue = oItem.YDRejectSampleScw; IsNumberField = true; }
        //                    else if (i == 20) { nValue = oItem.SDGoodSampleScw; IsNumberField = true; }
        //                    else if (i == 21) { nValue = oItem.SDRejectSampleScw; IsNumberField = true; }
        //                    else if (i == 22) { nValue = oItem.SDAkijExtra; IsNumberField = true; }
        //                    else if (i == 23) { nValue = oItem.DevelopmentPCFabrics; IsNumberField = true; }
        //                    else if (i == 24) { nValue = oItem.GrandTotalReceived; IsNumberField = true; }
        //                    else if (i == 25) { sVal = oItem.Remarks; }
        //                    else { sVal = ""; }

        //                    cell = sheet.Cells[rowIndex, colIndex++];
        //                    if (IsNumberField)
        //                    {
        //                        cell.Value = nValue;
                                
        //                    }
        //                    else
        //                    {
        //                        cell.Value = sVal;
        //                    }
        //                    cell.Style.Font.Bold = false;
        //                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //                    if (nHorizontalAlignment == 0) {
        //                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //                    }
        //                    else if (nHorizontalAlignment == 1){
        //                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                    }
        //                    else if (nHorizontalAlignment == 2){
        //                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //                    }
        //                    fill = cell.Style.Fill;
        //                    fill.PatternType = ExcelFillStyle.Solid;
        //                    fill.BackgroundColor.SetColor(Color.White);
        //                    border = cell.Style.Border;
        //                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                }
        //                rowIndex++;
        //            }
        //        }
        //        #endregion

        //        #region Total
        //        colIndex = 2;
        //        double GrandTotalReceived =0;
        //        double RejectReceived =0;
        //        double TotalSDReceived =0;
        //        for (int i = 1; i <= nMaxColumn; i++)
        //        {
        //            nHorizontalAlignment = 0;
        //            IsNumberField = false;
        //            nValue = 0;
        //            if (i == 1) { sVal = "Total : "; nHorizontalAlignment = 1; }
        //            else if (i == 2) { nValue = oRptMonthlyProductionAnalyses.Sum(x => x.AGradeReceived); IsNumberField = true; }
        //            else if (i == 3) { nValue = oRptMonthlyProductionAnalyses.Sum(x => x.BGradeReceived); IsNumberField = true; }
        //            else if (i == 4) {
        //                nValue = oRptMonthlyProductionAnalyses.Sum(x => x.RejectReceived); IsNumberField = true;
        //                RejectReceived=nValue;
        //            }
        //            else if (i == 5) { nValue = oRptMonthlyProductionAnalyses.Sum(x => x.TotalReceived); IsNumberField = true; }
        //            else if (i == 6) { nValue = oRptMonthlyProductionAnalyses.Sum(x => x.YDGoodExeReceived); IsNumberField = true; }
        //            else if (i == 7) { nValue = oRptMonthlyProductionAnalyses.Sum(x => x.YDRejectExeReceived); IsNumberField = true; }
        //            else if (i == 8) { nValue = oRptMonthlyProductionAnalyses.Sum(x => x.SDGoodExeReceived); IsNumberField = true;
        //                            TotalSDReceived+= nValue;                   
        //            }
        //            else if (i == 9) { nValue = oRptMonthlyProductionAnalyses.Sum(x => x.SDRejectExeReceived); IsNumberField = true;
        //                                TotalSDReceived+=nValue;
                    
        //            }
        //            else if (i == 10) { nValue = oRptMonthlyProductionAnalyses.Sum(x => x.YDGoodSample); IsNumberField = true; }
        //            else if (i == 11) { nValue = oRptMonthlyProductionAnalyses.Sum(x => x.YDRejectSample); IsNumberField = true; }
        //            else if (i == 12) { nValue = oRptMonthlyProductionAnalyses.Sum(x => x.SDGoodSample); IsNumberField = true;
        //                     TotalSDReceived+=nValue;
        //            }
        //            else if (i == 13) { nValue = oRptMonthlyProductionAnalyses.Sum(x => x.SDRejectSample); IsNumberField = true; 
        //             TotalSDReceived+=nValue;}
        //            else if (i == 14) { nValue = oRptMonthlyProductionAnalyses.Sum(x => x.YDGoodScw); IsNumberField = true; }
        //            else if (i == 15) { nValue = oRptMonthlyProductionAnalyses.Sum(x => x.YDRejectScw); IsNumberField = true; }
        //            else if (i == 16) { nValue = oRptMonthlyProductionAnalyses.Sum(x => x.SDGoodScw); IsNumberField = true; 
        //             TotalSDReceived+=nValue;
                    
        //            }
        //            else if (i == 17) { nValue = oRptMonthlyProductionAnalyses.Sum(x => x.SDRejectScw); IsNumberField = true;
        //             TotalSDReceived+=nValue;
        //            }
        //            else if (i == 18) { nValue = oRptMonthlyProductionAnalyses.Sum(x => x.YDGoodSampleScw); IsNumberField = true; }
        //            else if (i == 19) { nValue = oRptMonthlyProductionAnalyses.Sum(x => x.YDRejectSampleScw); IsNumberField = true; }
        //            else if (i == 20) { nValue = oRptMonthlyProductionAnalyses.Sum(x => x.SDGoodSampleScw); IsNumberField = true;
        //             TotalSDReceived+=nValue;
        //            }
        //            else if (i == 21) { nValue = oRptMonthlyProductionAnalyses.Sum(x => x.SDRejectSampleScw); IsNumberField = true;
        //             TotalSDReceived+=nValue;
        //            }
        //            else if (i == 22) { nValue = oRptMonthlyProductionAnalyses.Sum(x => x.SDAkijExtra); IsNumberField = true;
        //             TotalSDReceived+=nValue;
        //            }
        //            else if (i == 23) { nValue = oRptMonthlyProductionAnalyses.Sum(x => x.DevelopmentPCFabrics); IsNumberField = true; }
        //            else if (i == 24) { 
        //                nValue = oRptMonthlyProductionAnalyses.Sum(x => x.GrandTotalReceived); IsNumberField = true; 
        //                GrandTotalReceived=nValue;
        //            }
        //            else if (i == 25) { sVal = ""; }
        //            else { sVal = ""; }

        //            cell = sheet.Cells[rowIndex, colIndex++];
        //            if (IsNumberField)
        //            {
        //                cell.Value = nValue;
                       
        //            }
        //            else
        //            {
        //                cell.Value = sVal;
        //            }
        //            cell.Style.Font.Bold = true;
        //            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //            if (nHorizontalAlignment == 0)
        //            {
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //            }
        //            else if (nHorizontalAlignment == 1)
        //            {
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            }
        //            else if (nHorizontalAlignment == 2)
        //            {
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //            }
        //            fill = cell.Style.Fill;
        //            fill.PatternType = ExcelFillStyle.Solid;
        //            fill.BackgroundColor.SetColor(Color.White);
        //            border = cell.Style.Border;
        //            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //        }
        //        rowIndex++;
        //        #endregion
        //        #region Summery
        //        sheet.Cells[rowIndex,10, rowIndex, 13].Merge = true;
        //        cell = sheet.Cells[rowIndex, 10, rowIndex, 13]; cell.Value = ""; cell.Style.Font.Bold = true;
            
        //        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        rowIndex = rowIndex + 1;

        //        sheet.Cells[rowIndex, 10, rowIndex, 13].Merge = true;
        //        cell = sheet.Cells[rowIndex, 10, rowIndex, 13]; cell.Value = "Total Production Summery"; cell.Style.Font.Bold = true;
        //        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        rowIndex = rowIndex + 1;
               
        //        colIndex = 10;

        //        sheet.Cells[rowIndex, 10, rowIndex, 10].Merge = false;
        //        cell = sheet.Cells[rowIndex, 10, rowIndex, 10]; cell.Value = "Total Production"; cell.Style.Font.Bold = false;
        //        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //        cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        //        sheet.Cells[rowIndex, 11, rowIndex, 11].Merge = false;
        //        cell = sheet.Cells[rowIndex, 11, rowIndex, 11]; cell.Value = GrandTotalReceived; cell.Style.Font.Bold = false;
        //        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //        cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        //        sheet.Cells[rowIndex, 12, rowIndex, 12].Merge = false;
        //        cell = sheet.Cells[rowIndex, 12, rowIndex, 12]; cell.Value = "Total Prod. S/D"; cell.Style.Font.Bold = false;
        //        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //        cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        //        sheet.Cells[rowIndex, 13, rowIndex, 13].Merge = false;
        //        cell = sheet.Cells[rowIndex, 13, rowIndex, 13]; cell.Value = TotalSDReceived; cell.Style.Font.Bold = false;
        //        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //        cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        //        rowIndex = rowIndex + 1;


        //        sheet.Cells[rowIndex, 10, rowIndex, 10].Merge = false;
        //        cell = sheet.Cells[rowIndex, 10, rowIndex, 10]; cell.Value = "Avg. Production"; cell.Style.Font.Bold = false;
        //        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //        cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        //        sheet.Cells[rowIndex, 11, rowIndex, 11].Merge = false;
        //        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //        cell = sheet.Cells[rowIndex, 11, rowIndex, 11]; cell.Value = (nTotalDays > 0) ? Math.Round(GrandTotalReceived / nTotalDays) : 0; cell.Style.Font.Bold = false;
        //        cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        //        sheet.Cells[rowIndex, 12, rowIndex, 12].Merge = false;
        //        cell = sheet.Cells[rowIndex, 12, rowIndex, 12]; cell.Value = "Total Prod. Y/D"; cell.Style.Font.Bold = false;
        //        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //        cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        //        sheet.Cells[rowIndex, 13, rowIndex, 13].Merge = false;
        //        cell = sheet.Cells[rowIndex, 13, rowIndex, 13]; cell.Value = (GrandTotalReceived - TotalSDReceived); cell.Style.Font.Bold = false;
        //        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //        cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        //        rowIndex = rowIndex + 1;


        //        sheet.Cells[rowIndex, 10, rowIndex, 10].Merge = false;
        //        cell = sheet.Cells[rowIndex, 10, rowIndex, 10]; cell.Value = "Total Reject"; cell.Style.Font.Bold = false;
        //        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //        cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        //        sheet.Cells[rowIndex, 11, rowIndex, 11].Merge = false;
        //        cell = sheet.Cells[rowIndex, 11, rowIndex, 11]; cell.Value = RejectReceived; cell.Style.Font.Bold = false;
        //        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //        cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        //        sheet.Cells[rowIndex, 12, rowIndex, 12].Merge = false;
        //        cell = sheet.Cells[rowIndex, 12, rowIndex, 12]; cell.Value = " S/D Percentage"; cell.Style.Font.Bold = false;
        //        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //        cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


        //        sheet.Cells[rowIndex, 13, rowIndex, 13].Merge = false;
        //        cell = sheet.Cells[rowIndex, 13, rowIndex, 13]; cell.Value = Math.Round((TotalSDReceived * 100) / GrandTotalReceived, 2); cell.Style.Font.Bold = false;
        //        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //        cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        //        rowIndex = rowIndex + 1;

        //        sheet.Cells[rowIndex, 10, rowIndex, 10].Merge = false;
        //        cell = sheet.Cells[rowIndex, 10, rowIndex, 10]; cell.Value = "AVG Reject"; cell.Style.Font.Bold = false;
        //        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //        cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        //        sheet.Cells[rowIndex, 11, rowIndex, 11].Merge = false;
        //        cell = sheet.Cells[rowIndex, 11, rowIndex, 11]; cell.Value = Math.Round((RejectReceived * 100) / GrandTotalReceived, 2); cell.Style.Font.Bold = false;
        //        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //        cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        //        sheet.Cells[rowIndex, 12, rowIndex, 12].Merge = false;
        //        cell = sheet.Cells[rowIndex, 12, rowIndex, 12]; cell.Value = "Y/D Percentage"; cell.Style.Font.Bold = false;
        //        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //        cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        //        sheet.Cells[rowIndex, 13, rowIndex, 13].Merge = false;
        //        cell = sheet.Cells[rowIndex, 13, rowIndex, 13]; cell.Value = Math.Round(((GrandTotalReceived - TotalSDReceived) * 100) / GrandTotalReceived, 2); cell.Style.Font.Bold = false;
        //        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //        cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        //        rowIndex = rowIndex + 1;


        //        #endregion

        //        #region Signatory

        //        rowIndex = rowIndex + 5;
        //         colIndex = 3;
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "_________________"; cell.Style.Font.Bold = false;
        //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
        //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "_________________"; cell.Style.Font.Bold = false;
        //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
        //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "_________________"; cell.Style.Font.Bold = false;
        //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
        //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "_________________"; cell.Style.Font.Bold = false;
        //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
        //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "_________________"; cell.Style.Font.Bold = false;
        //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
        //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "_________________"; cell.Style.Font.Bold = false;
        //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
        //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "_________________"; cell.Style.Font.Bold = false;
        //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
        //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "_________________"; cell.Style.Font.Bold = false;
        //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
        //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "_________________"; cell.Style.Font.Bold = false;
        //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //        rowIndex++;
        //        colIndex = 3;
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Prepared BY"; cell.Style.Font.Bold = false;
        //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                
        //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "AM(Fabric Ins)"; cell.Style.Font.Bold = false;
        //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
        //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "AM(Production)"; cell.Style.Font.Bold = false;
        //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
        //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Q.A.D(Weaving)"; cell.Style.Font.Bold = false;
        //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
        //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "M/C Dept."; cell.Style.Font.Bold = false;
        //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
        //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "E/E Dept."; cell.Style.Font.Bold = false;
        //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
        //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Prepatory Dept."; cell.Style.Font.Bold = false;
        //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
        //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Manager"; cell.Style.Font.Bold = false;
        //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
        //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "D.G.M"; cell.Style.Font.Bold = false;
        //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //        rowIndex++;
        //        #endregion

        //        Response.ClearContent();
        //        Response.BinaryWrite(excelPackage.GetAsByteArray());
        //        Response.AddHeader("content-disposition", "attachment; filename=Monthly Production Analyse Report.xlsx");
        //        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //        Response.Flush();
        //        Response.End();
        //    }
        //}
        #endregion

        #region ProductionMonthlyInspection
        public ActionResult View_RptProductionMonthlyInspection(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<RptProductionMonthlyInspection> oRptProductionMonthlyInspections = new List<RptProductionMonthlyInspection>();
            string sSQL = "Select * from View_TextileSubUnit Where ISNULL(InactiveBy,0)=0";
            List<TextileSubUnit> oTextileSubUnits = new List<TextileSubUnit>();
            oTextileSubUnits = TextileSubUnit.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.TextileSubUnits = oTextileSubUnits;
            return View(oRptProductionMonthlyInspections);
        }
        public void ExcelProductionMonthlyInspection(string sParams, double nts)
        {
            #region Get List
            List<RptProductionMonthlyInspection> oRptProductionMonthlyInspections = new List<RptProductionMonthlyInspection>();
            DateTime dFromDate = Convert.ToDateTime(sParams.Split('~')[0]);
            DateTime dToDate = Convert.ToDateTime(sParams.Split('~')[1]);
            string sFEOIDs = (string.IsNullOrEmpty(sParams.Split('~')[2])) ? "" : sParams.Split('~')[2].Trim();
            string sBuyerIDs = (string.IsNullOrEmpty(sParams.Split('~')[3])) ? "" : sParams.Split('~')[3].Trim();
            string sFMIDs = (string.IsNullOrEmpty(sParams.Split('~')[4])) ? "" : sParams.Split('~')[4].Trim();

            oRptProductionMonthlyInspections = RptProductionMonthlyInspection.Gets(dFromDate, dToDate, sFEOIDs, sBuyerIDs, sFMIDs, ((User)Session[SessionInfo.CurrentUser]).UserID);
            #endregion

            int nMaxColumn = 0;
            int colIndex = 2;
            int rowIndex = 2;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Monthly Production Analyse Report");
                sheet.Name = "Monthly Production Analyse Report";

                int nColumnWidth = 18;
                sheet.Column(2).Width = nColumnWidth; //Date
                sheet.Column(3).Width = nColumnWidth; //Total Fol. Production
                sheet.Column(4).Width = nColumnWidth; //A-Grade
                sheet.Column(5).Width = nColumnWidth; //B- Grade
                sheet.Column(6).Width = nColumnWidth; // Total Reject
                sheet.Column(7).Width = nColumnWidth; //Reject %
                sheet.Column(8).Width = nColumnWidth; //R/A shift
                sheet.Column(9).Width = nColumnWidth; //R/B shift
                sheet.Column(10).Width = nColumnWidth; //R/C shift
                sheet.Column(11).Width = nColumnWidth; //Yarn Fault
                sheet.Column(12).Width = nColumnWidth; //Y/D Fault.
                sheet.Column(13).Width = nColumnWidth; //weaving
                sheet.Column(14).Width = nColumnWidth; //Remarks
                nMaxColumn = 14;

                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);

                #region Report Header
                sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = "Date : " + dFromDate.ToString("dd MMM yyyy") + " to " + dToDate.ToString("dd MMM yyyy"); cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 14; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 2;
                #endregion

                #region Table Header
                colIndex = 2;
                var sVal = "";
                for (int i = 1; i < nMaxColumn; i++)
                {
                    switch (i)
                    {
                        case 1: sVal = "Date"; break;
                        case 2: sVal = "Total Fol. Production"; break;
                        case 3: sVal = "A-Grade"; break;
                        case 4: sVal = "B- Grade"; break;
                        case 5: sVal = "Total Reject"; break;
                        case 6: sVal = "Reject %"; break;
                        case 7: sVal = "R/A shift"; break;
                        case 8: sVal = "R/B shift"; break;
                        case 9: sVal = "R/C shift"; break;
                        case 10: sVal = "Yarn Fault"; break;
                        case 11: sVal = "Y/D Fault"; break;
                        case 12: sVal = "Weaving"; break;
                        case 13: sVal = "Remarks"; break;
                        default: sVal = ""; break;
                    }

                    cell = sheet.Cells[rowIndex, colIndex++];
                    cell.Value = sVal;
                    cell.Style.Font.Bold = true;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                }
                rowIndex++;
                #endregion

                #region Table Body
                int nHorizontalAlignment = 0;
                bool IsNumberField = false;
                double nValue = 0;
                if (oRptProductionMonthlyInspections.Count > 0)
                {
                    foreach (RptProductionMonthlyInspection oItem in oRptProductionMonthlyInspections)
                    {
                        colIndex = 2;
                        sVal = "";

                        for (int i = 1; i < nMaxColumn; i++)
                        {
                            nHorizontalAlignment = 0;
                            IsNumberField = false;
                            nValue = 0;
                            switch (i)
                            {
                                case 1: sVal = oItem.LockDateSt; nHorizontalAlignment = 1; break;
                                case 2: nValue = Math.Round(oItem.TotalFolProductionInMtr); IsNumberField = true; break;
                                case 3: nValue = Math.Round(oItem.AGradeInMtr); IsNumberField = true; break;
                                case 4: nValue = Math.Round(oItem.BGradeInMtr); IsNumberField = true; break;
                                case 5: nValue = Math.Round(oItem.TotalRejectInMtr); IsNumberField = true; break;
                                case 6: sVal = oItem.RejectPercentageInMtr; break;
                                case 7: nValue = Math.Round(oItem.RAShiftInMtr); IsNumberField = true; break;
                                case 8: nValue = Math.Round(oItem.RBShiftInMtr); IsNumberField = true; break;
                                case 9: nValue = Math.Round(oItem.RCShiftInMtr); IsNumberField = true; break;
                                case 10: nValue = Math.Round(oItem.YarnFaultInMtr); IsNumberField = true; break;
                                case 11: nValue = Math.Round(oItem.YDFaultInMtr); IsNumberField = true; break;
                                case 12: nValue = Math.Round(oItem.WeavingInMtr); IsNumberField = true; break;
                                case 13: sVal = oItem.Remarks.ToString(); break;
                                default: sVal = ""; break;
                            }
                            cell = sheet.Cells[rowIndex, colIndex++];

                            cell.Style.Font.Bold = false;
                            if (IsNumberField)
                            {
                                cell.Value = nValue;

                            }
                            else
                            {
                                cell.Value = sVal;
                            }
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            if (nHorizontalAlignment == 0)
                            {
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            }
                            else if (nHorizontalAlignment == 1)
                            {
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            }
                            else if (nHorizontalAlignment == 2)
                            {
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            }
                            fill = cell.Style.Fill;
                            fill.PatternType = ExcelFillStyle.Solid;
                            fill.BackgroundColor.SetColor(Color.White);
                            border = cell.Style.Border;
                            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        rowIndex++;
                    }
                }
                #endregion

                #region Total
                colIndex = 2;
                sVal = "";
                for (int i = 1; i < nMaxColumn; i++)
                {
                    IsNumberField = false;
                    nValue = 0;
                    switch (i)
                    {
                        case 1: sVal = "Total : "; break;
                        case 2: nValue = oRptProductionMonthlyInspections.Sum(x => Math.Round(x.TotalFolProductionInMtr)); IsNumberField = true; break;
                        case 3: nValue = oRptProductionMonthlyInspections.Sum(x => Math.Round(x.AGradeInMtr)); IsNumberField = true; break;
                        case 4: nValue = oRptProductionMonthlyInspections.Sum(x => Math.Round(x.BGradeInMtr)); IsNumberField = true; break;
                        case 5: nValue = oRptProductionMonthlyInspections.Sum(x => Math.Round(x.TotalRejectInMtr)); IsNumberField = true; break;
                        case 6:
                            #region Calculation
                            double nTotalRejectInMtr = oRptProductionMonthlyInspections.Sum(x => Math.Round(x.TotalRejectInMtr));
                            double nTotalInMtr = oRptProductionMonthlyInspections.Sum(x => Math.Round(x.AGradeInMtr))
                                            + oRptProductionMonthlyInspections.Sum(x => Math.Round(x.BGradeInMtr))
                                            + nTotalRejectInMtr;
                            double nCal = (100 * nTotalRejectInMtr / nTotalInMtr);
                            if (double.IsNaN(nCal))
                            {
                                nCal = 0;
                            }
                            #endregion
                            sVal = nCal.ToString("0.00") + "%";
                            break;
                        case 7: nValue = oRptProductionMonthlyInspections.Sum(x => Math.Round(x.RAShiftInMtr)); IsNumberField = true; break;
                        case 8: nValue = oRptProductionMonthlyInspections.Sum(x => Math.Round(x.RBShiftInMtr)); IsNumberField = true; break;
                        case 9: nValue = oRptProductionMonthlyInspections.Sum(x => Math.Round(x.RCShiftInMtr)); IsNumberField = true; break;
                        case 10: nValue = oRptProductionMonthlyInspections.Sum(x => Math.Round(x.YarnFaultInMtr)); IsNumberField = true; break;
                        case 11: nValue = oRptProductionMonthlyInspections.Sum(x => Math.Round(x.YDFaultInMtr)); IsNumberField = true; break;
                        case 12: nValue = oRptProductionMonthlyInspections.Sum(x => Math.Round(x.WeavingInMtr)); IsNumberField = true; break;
                        case 13: sVal = ""; break;
                        default: sVal = ""; break;
                    }
                    cell = sheet.Cells[rowIndex, colIndex++];
                    if (IsNumberField)
                    {
                        cell.Value = nValue;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    }
                    else
                    {
                        cell.Value = sVal;
                    }
                    cell.Style.Font.Bold = true;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                }
                rowIndex = rowIndex + 2;
                #endregion

                #region Summary

                for (int j = 1; j <= 5; j++)
                {
                    colIndex = 2;
                    for (int i = 1; i <= 4; i++) //Null Cells
                    {
                        cell = sheet.Cells[rowIndex, colIndex++];
                        cell.Value = "";
                        cell.Style.Font.Bold = true;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                    }

                    if (j == 1)
                    {
                        cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex + 3];
                        cell.Value = "Summary";
                        cell.Merge = true;
                        cell.Style.Font.Bold = true;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        colIndex = colIndex + 4;
                    }
                    else if (j == 2)
                    {
                        cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex + 1];
                        cell.Value = "Total Production";
                        cell.Merge = true;
                        cell.Style.Font.Bold = false;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        colIndex = colIndex + 2;


                        cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex + 1];
                        cell.Value = oRptProductionMonthlyInspections.Sum(x => Math.Round(x.TotalFolProductionInMtr));

                        cell.Merge = true;
                        cell.Style.Font.Bold = false;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        colIndex = colIndex + 2;
                    }
                    else if (j == 3)
                    {
                        cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex + 1];
                        cell.Value = "AVG Production";
                        cell.Merge = true;
                        cell.Style.Font.Bold = false;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        colIndex = colIndex + 2;
                        int hasProductionCountDays = oRptProductionMonthlyInspections.Where(x => x.TotalFolProductionInMtr > 0).ToList().Count();
                        cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex + 1];
                        cell.Value = (hasProductionCountDays > 0) ? Math.Round(oRptProductionMonthlyInspections.Sum(x => Math.Round(x.TotalFolProductionInMtr)) / (double)hasProductionCountDays, 2) : 0;
                        cell.Merge = true;
                        cell.Style.Font.Bold = false;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        colIndex = colIndex + 2;
                    }
                    else if (j == 4)
                    {
                        cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex + 1];
                        cell.Value = "Total Reject";
                        cell.Merge = true;
                        cell.Style.Font.Bold = false;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        colIndex = colIndex + 2;

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex + 1];
                        cell.Value = oRptProductionMonthlyInspections.Sum(x => Math.Round(x.TotalRejectInMtr));

                        cell.Merge = true;
                        cell.Style.Font.Bold = false;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        colIndex = colIndex + 2;
                    }
                    else if (j == 5)
                    {
                        cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex + 1];
                        cell.Value = "AVG Reject %";
                        cell.Merge = true;
                        cell.Style.Font.Bold = false;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        colIndex = colIndex + 2;

                        #region Calculcation
                        double nTotalRejectInMtr = oRptProductionMonthlyInspections.Sum(x => Math.Round(x.TotalRejectInMtr));
                        double nTotalInMtr = oRptProductionMonthlyInspections.Sum(x => Math.Round(x.AGradeInMtr))
                                        + oRptProductionMonthlyInspections.Sum(x => Math.Round(x.BGradeInMtr))
                                        + nTotalRejectInMtr;
                        double nCal = (100 * nTotalRejectInMtr / nTotalInMtr);
                        if (double.IsNaN(nCal))
                        {
                            nCal = 0;
                        }
                        sVal = nCal.ToString("0.00") + "%";
                        #endregion

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex + 1];
                        cell.Value = sVal;
                        cell.Merge = true;
                        cell.Style.Font.Bold = false;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        colIndex = colIndex + 2;
                    }

                    for (int i = 1; i <= 5; i++) //Null Cells
                    {
                        cell = sheet.Cells[rowIndex, colIndex++];
                        cell.Value = "";
                        cell.Style.Font.Bold = true;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                    }
                    rowIndex++;
                }

                #endregion

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=MonthlyProductionAnalyseReport.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }



        private void SummaryDailyProduction(List<WUProductionDailyInspection> oWUProductionDailyInspections, ref int rowIndex, ref ExcelWorksheet sheet, ref ExcelRange cell, string stitle, ref ExcelFill fill, double nTotalInspection, double nGradeA, double nGradeB, double nReject)
        {
            #region Summary
            OfficeOpenXml.Style.Border border;
            //double nTotalInpectionQty = oWUProductionDailyInspections.Sum(x => x.TotalInspection);
            double nTotalInpectionQty = nTotalInspection;
            double nTotalInpectionQtyTSU1 = oWUProductionDailyInspections.Where(x => x.TSUID == 1).Sum(x => x.TotalInspection);
            double nTotalInpectionQtyTSU2 = oWUProductionDailyInspections.Where(x => x.TSUID == 2).Sum(x => x.TotalInspection);
            double nGradeAQty = nGradeA;
            double nGradeATSU1 = oWUProductionDailyInspections.Where(x => x.TSUID == 1).Sum(x => x.GradeA);
            double nGradeATSU2 = oWUProductionDailyInspections.Where(x => x.TSUID == 2).Sum(x => x.GradeA);
            double nGradeBQty = nGradeB;
            double nGradeBTSU1 = oWUProductionDailyInspections.Where(x => x.TSUID == 1).Sum(x => x.GradeB);
            double nGradeBTSU2 = oWUProductionDailyInspections.Where(x => x.TSUID == 2).Sum(x => x.GradeB);

            double nRejectQty = nReject;
            double nRejectTSU1 = oWUProductionDailyInspections.Where(x => x.TSUID == 1).Sum(x => x.Reject);
            double nRejectTSU2 = oWUProductionDailyInspections.Where(x => x.TSUID == 2).Sum(x => x.Reject);

            double nMtrGradeA = 0, nMtrGradeATSU1 = 0, nMtrGradeATSU2 = 0;
            double nMtrGradeB = 0, nMtrGradeBTSU1 = 0, nMtrGradeBTSU2 = 0;
            double nMtrGradeReject = 0, nMtrGradeRejectTSU1 = 0, nMtrGradeRejectTSU2 = 0;
            if (nTotalInpectionQty > 0)
            {
                nMtrGradeA = Math.Round((nGradeAQty * 100) / nTotalInpectionQty, 2);
                nMtrGradeB = Math.Round((nGradeBQty * 100) / nTotalInpectionQty, 2);
                nMtrGradeReject = Math.Round((nRejectQty * 100) / nTotalInpectionQty, 2);
            }
            if (nTotalInpectionQtyTSU1 > 0)
            {
                nMtrGradeATSU1 = Math.Round((nGradeATSU1 * 100) / nTotalInpectionQtyTSU1, 2);
                nMtrGradeBTSU1 = Math.Round((nGradeBTSU1 * 100) / nTotalInpectionQtyTSU1, 2);
                nMtrGradeRejectTSU1 = Math.Round((nRejectTSU1 * 100) / nTotalInpectionQtyTSU1, 2);
            }
            if (nTotalInpectionQtyTSU2 > 0)
            {
                nMtrGradeATSU2 = Math.Round((nGradeATSU2 * 100) / nTotalInpectionQtyTSU2, 2);
                nMtrGradeBTSU2 = Math.Round((nGradeBTSU2 * 100) / nTotalInpectionQtyTSU2, 2);
                nMtrGradeRejectTSU2 = Math.Round((nRejectTSU2 * 100) / nTotalInpectionQtyTSU2, 2);
            }




            int colIndex = 6;
            cell = sheet.Cells[rowIndex, 6, rowIndex, 8]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = false;
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            rowIndex++;

            colIndex = 6;
            cell = sheet.Cells[rowIndex, 6, rowIndex, 12]; cell.Merge = true; cell.Value = stitle + " Summery"; cell.Style.Font.Bold = false;
            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
            rowIndex++;


            colIndex = 6;
            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Summary"; cell.Style.Font.Bold = false;
            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Shade-01"; cell.Style.Font.Bold = false;
            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "mtr(%)"; cell.Style.Font.Bold = false;
            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Shade-02"; cell.Style.Font.Bold = false;
            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "mtr(%)"; cell.Style.Font.Bold = false;
            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Shade-01 & Shade -02"; cell.Style.Font.Bold = false;
            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "mtr(%)"; cell.Style.Font.Bold = false;
            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            rowIndex++;


            colIndex = 6;
            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Total Production:"; cell.Style.Font.Bold = false;
            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round(nTotalInpectionQtyTSU1, 0); cell.Style.Font.Bold = false;
            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "-"; cell.Style.Font.Bold = false;
            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round(nTotalInpectionQtyTSU2, 0); cell.Style.Font.Bold = false;
            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "-"; cell.Style.Font.Bold = false;
            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round(nTotalInpectionQty, 0); cell.Style.Font.Bold = false;
            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "-"; cell.Style.Font.Bold = false;
            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;


            rowIndex++;

            colIndex = 6;
            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "A-Grade:"; cell.Style.Font.Bold = false;
            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round(nGradeATSU1, 0); cell.Style.Font.Bold = false;
            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nMtrGradeATSU1; cell.Style.Font.Bold = false;
            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round(nGradeATSU2, 0); cell.Style.Font.Bold = false;
            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nMtrGradeATSU2; cell.Style.Font.Bold = false;
            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round(nGradeAQty, 0); cell.Style.Font.Bold = false;
            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nMtrGradeA; cell.Style.Font.Bold = false;
            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;


            rowIndex++;

            colIndex = 6;
            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "B-Grade:"; cell.Style.Font.Bold = false;
            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round(nGradeBTSU1, 0); cell.Style.Font.Bold = false;
            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nMtrGradeBTSU1; cell.Style.Font.Bold = false;
            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round(nGradeBTSU2, 0); cell.Style.Font.Bold = false;
            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nMtrGradeBTSU2; cell.Style.Font.Bold = false;
            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round(nGradeBQty, 0); cell.Style.Font.Bold = false;
            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nMtrGradeB; cell.Style.Font.Bold = false;
            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            rowIndex++;
            colIndex = 6;
            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Reject:"; cell.Style.Font.Bold = false;
            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round(nRejectTSU1, 0); cell.Style.Font.Bold = false;
            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nMtrGradeRejectTSU1; cell.Style.Font.Bold = false;
            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round(nRejectTSU2, 0); cell.Style.Font.Bold = false;
            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nMtrGradeRejectTSU2; cell.Style.Font.Bold = false;
            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round(nReject, 0); cell.Style.Font.Bold = false;
            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nMtrGradeReject; cell.Style.Font.Bold = false;
            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            #endregion


        }

        public double GetValue(double nValue){
          double floor = Math.Floor(nValue);
          return (nValue - floor <= 0.5) ? floor : floor + 1;

         }

        public void ExcelProductionDailyInspection(string sParams, double nts)
        {
            #region Get List
            List<WUProductionDailyInspection> oWUProductionDailyInspections = new List<WUProductionDailyInspection>();
            DateTime dFromDate = Convert.ToDateTime(sParams.Split('~')[0]);
            string sFEOIDs = (string.IsNullOrEmpty(sParams.Split('~')[1])) ? "" : sParams.Split('~')[1].Trim();
            string sBuyerIDs = (string.IsNullOrEmpty(sParams.Split('~')[2])) ? "" : sParams.Split('~')[2].Trim();
            string sFMIDs = (string.IsNullOrEmpty(sParams.Split('~')[3])) ? "" : sParams.Split('~')[3].Trim();
            DateTime dtTO = Convert.ToDateTime(sParams.Split('~')[4]);
            //int TSUID = Convert.ToInt32(sParams.Split('~')[5]);
            int TSUID = 0;

            oWUProductionDailyInspections = WUProductionDailyInspection.Gets(dFromDate, sFEOIDs, sBuyerIDs, sFMIDs, dtTO, TSUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            #endregion

            int rowIndex = 1;
            int nMaxColumn = 0;
            int colIndex = 2;
            ExcelRange cell;
            ExcelFill fill;

            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Daily Inspection & Folding Production Report");
                sheet.Name = "Daily Inspection & Folding Production Report";

                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);

                string[] columnHead = new string[] { "#SL", "Production Date", "Exe No", "Buyer Name", "Construction", "Reed Count", "Grey Width", "Weave", "A-Grade", "B-Grade", "Reject", "Total Ins. Qty", "Remarks" };
                int[] colWidth = new int[] { 5, 15, 15, 18, 17, 20, 10, 15, 12, 12, 12, 12, 12, 12, 12, 25 };
                nMaxColumn = columnHead.Length + 1;


                #region Report Header
                cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn]; cell.Merge = true; cell.Style.Font.Bold = true; cell.Style.Font.Size = 15;
                cell.Value = oCompany.Name; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex++;

                cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn]; cell.Merge = true; cell.Style.Font.Bold = false; cell.Style.Font.Size = 12;
                cell.Value = "Weaving Unit"; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex++;

                cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn]; cell.Merge = true; cell.Style.Font.Bold = false; cell.Style.Font.Size = 12;
                cell.Value = "Daily Inspection & Folding Production Report"; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex++;

                cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn]; cell.Merge = true; cell.Style.Font.Bold = false; cell.Style.Font.Size = 12;
                cell.Value = " Production Date:" + dFromDate.ToString("dd MMM yyyy") + " To " + dtTO.ToString("dd MMM yyyy"); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex++;
                #endregion

                #region Report Header
                cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn]; cell.Merge = true; cell.Style.Font.Bold = false; cell.Style.Font.Size = 12;
                cell.Value = " Reporting Date:" + DateTime.Now.ToString("dd MMM yyyy"); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex++;
                #endregion

                #region Table Header

                colIndex = 2;
                for (int i = 0; i < colWidth.Length; i++)
                {
                    sheet.Column(colIndex).Width = colWidth[i];
                    colIndex++;
                }
                nMaxColumn = colIndex;

                colIndex = 2;
                for (int i = 0; i < 8; i++)
                {
                    cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex]; cell.Merge = true; cell.Value = columnHead[i]; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    colIndex++;
                }
                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex += 2]; cell.Merge = true; cell.Value = "Shade -1"; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                colIndex++;
                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex += 2]; cell.Merge = true; cell.Value = "Shade -2"; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                colIndex++;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex]; cell.Merge = true; cell.Value = "Total Ins. Qty"; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                colIndex++;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex]; cell.Merge = true; cell.Value = "Remarks"; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                colIndex++;


                rowIndex++;
                colIndex = 10;

                for (int i = 8; i < 11; i++)
                {
                    cell = sheet.Cells[rowIndex, colIndex]; cell.Merge = false; cell.Value = columnHead[i]; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    colIndex++;

                }
                for (int i = 8; i < 11; i++)
                {
                    cell = sheet.Cells[rowIndex, colIndex]; cell.Merge = false; cell.Value = columnHead[i]; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    colIndex++;

                }
                rowIndex++;


                #endregion

                #region Table Body
                if (oWUProductionDailyInspections.Any() && oWUProductionDailyInspections.FirstOrDefault().FEOID > 0)
                {

                    var SummeryByFEOs = oWUProductionDailyInspections.GroupBy(x => new { x.FEOID, x.ProductionDate }).Select(grp => new WUProductionDailyInspection
                    {
                        FEOID = grp.Key.FEOID,
                        FEONo = grp.First().FEONo,
                        IsInHouse = grp.First().IsInHouse,
                        OrderType = grp.First().OrderType,
                        BuyerName = grp.First().BuyerName,
                        ReedCount = grp.First().ReedCount,
                        GreyWidth = grp.First().GreyWidth,
                        ProcessType = grp.First().ProcessType,
                        Remarks = grp.First().Remarks,
                        Construction = grp.First().Construction,
                        ProductionDate = grp.Key.ProductionDate,
                        FabricWeaveName = grp.First().FabricWeaveName


                    }).ToList();

                    var summary = SummeryByFEOs.GroupBy(x => x.ProcessType).Select(grp => new
                    {
                        ProcessType = grp.Key,
                        Result = grp.ToList()
                    }).ToList().OrderBy(x => x.ProcessType);


                    double nTotalInspection = 0;
                    double nGradeA = 0, nGradeB = 0, Reject = 0;
                    foreach (var data in summary)
                    {
                        int nCount = 0;

                        cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn]; cell.Merge = true; cell.Style.Font.Bold = true; cell.Style.Font.Size = 12;
                        cell.Value = data.ProcessType; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        rowIndex++;
                        nTotalInspection = 0;
                        nGradeA = 0; nGradeB = 0; Reject = 0;
                        foreach (var oItem in data.Result.OrderByDescending(x => x.OrderType).ThenBy(x => x.ProductionDate))
                        {
                            colIndex = 2;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (++nCount).ToString(); cell.Style.Font.Bold = false;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ProductionDateStr; cell.Style.Font.Bold = false;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.OrderNo; cell.Style.Font.Bold = false;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.BuyerName; cell.Style.Font.Bold = false;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Construction; cell.Style.Font.Bold = false;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ReedCount; cell.Style.Font.Bold = false;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.GreyWidth; cell.Style.Font.Bold = false;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.FabricWeaveName; cell.Style.Font.Bold = false;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            double Shade1GradeA = GetValue(oWUProductionDailyInspections.Where(x => x.TSUID == 1 && x.FEOID == oItem.FEOID).Sum(x => x.GradeA));
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round(Shade1GradeA, 0); cell.Style.Font.Bold = false;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            double Shade1GradeB = GetValue(oWUProductionDailyInspections.Where(x => x.TSUID == 1 && x.FEOID == oItem.FEOID).Sum(x => x.GradeB));
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round(Shade1GradeB, 0); cell.Style.Font.Bold = false;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            double Shade1Reject = GetValue(oWUProductionDailyInspections.Where(x => x.TSUID == 1 && x.FEOID == oItem.FEOID).Sum(x => x.Reject));
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round(Shade1Reject, 0); cell.Style.Font.Bold = false;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            double Shade2GradeA = GetValue(oWUProductionDailyInspections.Where(x => x.TSUID == 2 && x.FEOID == oItem.FEOID).Sum(x => x.GradeA));
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round(Shade2GradeA, 0); cell.Style.Font.Bold = false;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            double Shade2GradeB = GetValue(oWUProductionDailyInspections.Where(x => x.TSUID == 2 && x.FEOID == oItem.FEOID).Sum(x => x.GradeB));
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round(Shade2GradeB, 0); cell.Style.Font.Bold = false;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            double Shade2Reject = GetValue(oWUProductionDailyInspections.Where(x => x.TSUID == 2 && x.FEOID == oItem.FEOID).Sum(x => x.Reject));
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round(Shade2Reject, 0); cell.Style.Font.Bold = false;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            double Total = Shade1GradeA + Shade1GradeB + Shade1Reject + Shade2GradeA + Shade2GradeB + Shade2Reject;
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round(Total, 0); cell.Style.Font.Bold = false;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Remarks; cell.Style.Font.Bold = false;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            nTotalInspection += Total;
                            nGradeA += Shade1GradeA + Shade2GradeA;
                            nGradeB += Shade1GradeB + Shade2GradeB;
                            Reject += Shade1Reject + Shade2Reject;
                            rowIndex++;
                        }

                    }

                }
                #endregion

                #region Total Summary

                rowIndex = rowIndex + 2;
                double nTotalInpectionQtyinMtr = oWUProductionDailyInspections.Sum(x => x.TotalInspection);
                double gredeA = oWUProductionDailyInspections.Sum(x => x.GradeA);
                double gredeB = oWUProductionDailyInspections.Sum(x => x.GradeB);
                double reject = oWUProductionDailyInspections.Sum(x => x.Reject);

                SummaryDailyProduction(oWUProductionDailyInspections, ref rowIndex, ref sheet, ref cell, "Total Production", ref fill, nTotalInpectionQtyinMtr, gredeA, gredeB, reject);

                #region Signatory

                rowIndex = rowIndex + 5;
                colIndex = 3;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "_________________"; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "_________________"; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "_________________"; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "_________________"; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "_________________"; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "_________________"; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "_________________"; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "_________________"; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "_________________"; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                rowIndex++;
                colIndex = 3;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Prepared BY"; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "AM(Fabric Ins)"; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "AM(Production)"; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Q.A.D(Weave)"; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "M/C Dept."; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "E/E Dept."; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Prepatory Dept."; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Manager"; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "D.G.M"; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                rowIndex++;
                #endregion
                #endregion

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=DailyProductionAnalyseReport.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }
       
        

        #endregion

        #region RptDailyBeamStockReport
        //public ActionResult View_RptDailyBeamStockReport(int menuid)
        //{
        //    this.Session.Remove(SessionInfo.MenuID);
        //    this.Session.Add(SessionInfo.MenuID, menuid);
        //    List<RptDailyBeamStockReport> oRptDailyBeamStockReports = new List<RptDailyBeamStockReport>();

        //    string sSQL = "Select * from View_TextileSubUnit Where ISNULL(InactiveBy,0)=0";
        //    List<TextileSubUnit> oTextileSubUnits = new List<TextileSubUnit>();
        //    oTextileSubUnits = TextileSubUnit.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    ViewBag.TextileSubUnits = oTextileSubUnits;

        //    return View(oRptDailyBeamStockReports);
        //}
        #endregion

        #region WUStockReport
        public ActionResult View_WUStockReports(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<WUStockReport> oWUStockReports = new List<WUStockReport>();
            //ViewBag.OrderTypes = Enum.GetValues(typeof(EnumOrderType)).Cast<EnumOrderType>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            string sSQL = "SELECT * FROM FabricProcess WHERE ProcessType=" + (int)EnumFabricProcess.Process;
            ViewBag.FabricProcesss = FabricProcess.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(oWUStockReports);
        }
        public void ExcelWUStockReport(string param)
        {

            short orderType = Convert.ToInt16(param.Split('~')[0]);
            int processType = Convert.ToInt32(param.Split('~')[1]);
            string buyerIds = (string.IsNullOrEmpty(param.Split('~')[2])) ? "" : param.Split('~')[2];
            string feoIds = (string.IsNullOrEmpty(param.Split('~')[3])) ? "" : param.Split('~')[3];
            bool bIsCurrentStock = Convert.ToBoolean(param.Split('~')[4]);

            List<WUStockReport> oWUStockReports = new List<WUStockReport>();
            oWUStockReports = WUStockReport.Gets(orderType, processType, buyerIds, feoIds, bIsCurrentStock, ((User)Session[SessionInfo.CurrentUser]).UserID);

            int nMaxColumn = 0;
            int colIndex = 2;
            int rowIndex = 2;
            ExcelRange cell;
            ExcelFill fill;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Stock Report");
                sheet.Name = "Stock Report";

                List<string> stores = new List<string>();
                if (oWUStockReports.Any() && oWUStockReports.FirstOrDefault().FEOID > 0)
                {
                    oWUStockReports.Select(x => x.StoreWiseReceive).ToList().ForEach(x => stores.AddRange(x.Split('~').ToList()));
                    stores = stores.Select(x => x.Split('^')[0]).ToList();
                    stores = stores.Distinct().ToList();
                }

                List<string> columnHead = new List<string> { "SL", "Buyer Name", "FEO No", "Construction", "Order Type", "Process Type", "Order Qty", "Stock Qty" };
                List<int> colWidth = new List<int> { 8, 35, 25, 25, 15, 20, 18, 18 };

                if (stores.Any())
                {
                    foreach (string val in stores)
                    {
                        columnHead.Add(val);
                        colWidth.Add(25);
                    }

                }
                columnHead.Add("Transfer Qty");
                colWidth.Add(18);



                colIndex = 2;
                for (int i = 0; i < colWidth.Count(); i++)
                {
                    sheet.Column(colIndex).Width = colWidth[i];
                    colIndex++;
                }
                nMaxColumn = colIndex - 1;

                #region Company

                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);

                sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 2;
                #endregion

                #region Column Header
                colIndex = 2;

                foreach (string column in columnHead)
                {
                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = column; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                    colIndex++;
                }

                rowIndex++;
                #endregion

                #region Table Body

                Dictionary<string, double> summary = new Dictionary<string, double>();
                summary.Add("OrderQty", 0);
                summary.Add("StockQty", 0);
                summary.Add("TransferQty", 0);
                stores.ForEach(x => summary.Add(x, 0));


                if (oWUStockReports.Any() && oWUStockReports.FirstOrDefault().FEOID > 0)
                {
                    oWUStockReports = oWUStockReports.OrderBy(x => x.BuyerName).ToList();
                    int count = 0;
                    List<string> storeReceive = new List<string>();
                    while (oWUStockReports.Any())
                    {
                        var oWUSRs = oWUStockReports.Where(x => x.BuyerID == oWUStockReports.FirstOrDefault().BuyerID).ToList();
                        oWUStockReports.RemoveAll(x => x.BuyerID == oWUSRs.FirstOrDefault().BuyerID);
                        bool hasSpan = true;
                        foreach (WUStockReport oItem in oWUSRs)
                        {
                            storeReceive = new List<string>();
                            if (!string.IsNullOrEmpty(oItem.StoreWiseReceive))
                                storeReceive.AddRange(oItem.StoreWiseReceive.Split('~'));


                            colIndex = 2;
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (++count).ToString(); cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            if (hasSpan)
                            {
                                cell = sheet.Cells[rowIndex, colIndex, rowIndex + oWUSRs.Count() - 1, colIndex++]; cell.Merge = hasSpan; cell.Value = oItem.BuyerName; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                hasSpan = false;
                            }
                            else
                            {
                                colIndex++;
                            }

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.OrderNo; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Construction; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.OrderType.ToString(); cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ProcessTypeName; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.OrderQty; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.CurrentStockQty; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                            if (stores.Any())
                            {
                                foreach (string column in stores)
                                {
                                    bool hasFound = false;
                                    double nQty = 0;
                                    foreach (string val in storeReceive)
                                    {
                                        if (val.Split('^')[0] == column)
                                        {
                                            hasFound = true;
                                            Double.TryParse(val.Split('^')[1], out nQty);
                                            summary[column] = summary[column] + nQty;
                                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nQty; cell.Style.Font.Bold = false;
                                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                            break;
                                        }
                                    }
                                    if (!hasFound)
                                    {
                                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nQty; cell.Style.Font.Bold = false;
                                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                    }
                                }
                            }

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.TransferQty; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                            summary["OrderQty"] = summary["OrderQty"] + oItem.OrderQty;
                            summary["StockQty"] = summary["StockQty"] + oItem.CurrentStockQty;
                            summary["TransferQty"] = summary["TransferQty"] + oItem.TransferQty;
                            rowIndex++;
                        }
                    }

                    #region Total
                    colIndex = 7;
                    cell = sheet.Cells[rowIndex, 2, rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Total"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = summary["OrderQty"]; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = summary["StockQty"]; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";


                    foreach (string column in stores)
                    {
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = summary[column]; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    }

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = summary["TransferQty"]; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                    #endregion

                }

                else
                {
                    foreach (string column in columnHead)
                    {
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    }
                }
                #endregion



                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=Stock Report.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }
        #endregion

        #region Report Daily Machine Log
        public void ExcelMachineLogReport(string sParam)
        {
            if (string.IsNullOrEmpty(sParam))
                throw new Exception("Search parameter required");


            DateTime dDate = Convert.ToDateTime(sParam.Split('~')[0]);
            int nTSUID = Convert.ToInt32(sParam.Split('~')[1]);
            int nShiftID = Convert.ToInt32(sParam.Split('~')[2]);
            DateTime dToDate = Convert.ToDateTime(sParam.Split('~')[3]);
            string sBuyerIDs= sParam.Split('~')[4];
            string sFEOIDs= sParam.Split('~')[5];
            int nWeaveType = Convert.ToInt32(sParam.Split('~')[6]);
            int nProcessType = Convert.ToInt32(sParam.Split('~')[7]);
            string sConstruction= sParam.Split('~')[8];
            string sFMIDs= sParam.Split('~')[9];

            string subQuery = string.Empty;
            string subQuery1 = string.Empty;
            List<FabricBatchProductionBatchMan> oFBPBMs = new List<FabricBatchProductionBatchMan>();
            List<FabricBatchProduction> oFBPs = new List<FabricBatchProduction>();
            if (nTSUID > 0)
                subQuery += " AND TSUID = " +nTSUID;
            if (!string.IsNullOrEmpty(sFMIDs))
                subQuery += " AND FMID IN ( " + sFMIDs+")";
            if (!string.IsNullOrEmpty(sFEOIDs))
                subQuery1 += " AND FEOID IN ( " + sFEOIDs + ")";
            if (!string.IsNullOrEmpty(sBuyerIDs))
                subQuery1 += " AND FEOID IN (SELECT FEOID FROM FabricExecutionOrder WHERE BuyerID IN (  " + sBuyerIDs + "))";
            if (nProcessType > 0)
                subQuery1 += " AND FEOID IN (SELECT FEOID FROM FabricExecutionOrder WHERE ProcessType IN (  " + nProcessType + "))";
            if (nWeaveType > 0)
                subQuery1 += " AND FEOID IN (SELECT FEOID FROM FabricExecutionOrder WHERE FabricWeave IN (  " + nWeaveType + "))";
            if (!string.IsNullOrEmpty(sConstruction))
                subQuery1 += " AND FEOID IN (SELECT FEOID FROM View_FabricExecutionOrder WHERE Construction ='" + sConstruction + "')";


            subQuery += "AND FBID IN (SELECT FBID FROM FabricBatch WHERE FBID<>0"+subQuery1+")";
            string sSQL = "SELECT * FROM View_FabricBatchProductionBatchMan WHERE FBPID IN (SELECT FBPID FROM FabricBatchProduction WHERE WeavingProcess=3 " + subQuery + ") AND FinishDate BETWEEN '" + dDate.ToString("dd MMM yyyy") + "' AND '" + dToDate.ToString("dd MMM yyyy") + "'";
            if (nShiftID > 0)
                sSQL += " AND ShiftID = " + nShiftID;
            oFBPBMs = FabricBatchProductionBatchMan.GetsBySql(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if(oFBPBMs.Any()){
                 string sSQL1 = "SELECT * FROM View_FabricBatchProduction WHERE FBPID IN ("+string.Join(",",oFBPBMs.Select(x=>x.FBPID))+")";
                 oFBPs = FabricBatchProduction.Gets(sSQL1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
           
            int nMaxColumn = 0;
            int colIndex = 2;
            int rowIndex = 2;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Daily Machine Log Report");
                sheet.Name = "Daily Machine Log Report";

                int nColumnWidth = 18;
                sheet.Column(2).Width = nColumnWidth; //SL  
                sheet.Column(3).Width = nColumnWidth; //Date
                sheet.Column(4).Width = nColumnWidth; //Machine No
                sheet.Column(5).Width = nColumnWidth; //Beam No
                sheet.Column(6).Width = nColumnWidth; //Order No
                sheet.Column(7).Width = nColumnWidth; //Construction
                sheet.Column(8).Width = nColumnWidth; //BuyerName

                sheet.Column(9).Width = nColumnWidth; //Color
                sheet.Column(10).Width = nColumnWidth; //Shift
                sheet.Column(11).Width = nColumnWidth; //RPM
                sheet.Column(12).Width = nColumnWidth; //Prod Qty
                sheet.Column(13).Width = nColumnWidth; //Efficiency
                sheet.Column(14).Width = nColumnWidth; //Production Date
                sheet.Column(15).Width = nColumnWidth; //Start Date
                sheet.Column(16).Width = nColumnWidth;//Start Time
                sheet.Column(17).Width = nColumnWidth;//End Date
                sheet.Column(18).Width = nColumnWidth;//End Shift
                sheet.Column(19).Width = nColumnWidth;//End Time
                sheet.Column(20).Width = nColumnWidth;//Status
                nMaxColumn = 20;

                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);

                #region Report Header
                sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = "Golora,Charkhanda,Manikgonj"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = "Daily Machine Log Report"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = "(Weaving Unit)"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value =  "Reporting Date : " + DateTime.Now.ToString("dd MMM yyyy"); cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 2;


                #endregion

                #region Table Header
                colIndex = 2;
                string sVal = "";
                for (int i = 1; i < nMaxColumn; i++)
                {
                    switch (i)
                    {
                        case 1: sVal = "SL#"; break;
                        case 2: sVal = "Production Date"; break;
                        case 3: sVal = "Machine No"; break;
                        case 4: sVal = "Beam No"; break;
                        case 5: sVal = "Order No"; break;
                        case 6: sVal = "Construction"; break;
                        case 7: sVal = "Buyer Name"; break;
                        case 8: sVal = "Color"; break;
                        case 9: sVal = "Shift"; break;
                        case 10: sVal = "RPM"; break;
                        case 11: sVal = "Prod.Qty(mtr)"; break;
                        case 12: sVal = "Efficiency"; break;
                        case 13: sVal = "Production Date"; break;
                        case 14: sVal = "Start Date"; break;
                        case 15: sVal = "Start Time"; break;
                        case 16: sVal = "End Date "; break;
                        case 17: sVal = "End Shift "; break;
                        case 18: sVal = "End Time "; break;
                        case 19: sVal = "Status"; break;

                        default: sVal = ""; break;
                    }

                    cell = sheet.Cells[rowIndex, colIndex++];
                    cell.Value = sVal;
                    cell.Style.Font.Bold = true;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                }
                rowIndex++;
                #endregion

                #region Table body

                if (oFBPBMs.Any())
                {
                        int SL = 0;
                        foreach (FabricBatchProductionBatchMan oitem in oFBPBMs)
                        {
                            FabricBatchProduction oFBP = oFBPs.Where(x=>x.FBPID==oitem.FBPID).ToList().First();
                            colIndex = 2;
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = ++SL;
                            cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = oitem.FinishDateInString;
                            cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = oFBP.MachineCode;
                            cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = oitem.BeamNo;
                            cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                          
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = oFBP.OrderNo;
                            cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = oFBP.Construction;
                            cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                           
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = oFBP.BuyerName;
                            cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = oFBP.ColorName;
                            cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = oitem.ShiftName;
                            cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value =oitem.RPM;
                            cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                          
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = Global.MillionFormat(oitem.QtyinMeter,2);
                            cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = oitem.Efficiency;
                            cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "";
                            cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = oFBP.StartDateSt;
                            cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = oFBP.StartTime.ToString("hh:mm tt");
                            cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = (oFBP.EndTime==DateTime.MinValue)?"-":oFBP.EndTime.ToString("dd MMM yyyy");
                            cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "";
                            cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value =(oFBP.EndTime==DateTime.MinValue)?"-":oFBP.EndTime.ToString("hh:mm tt");
                            cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = (oFBP.EndTime == DateTime.MinValue) ? "Running" : "Out";
                            cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;



                            rowIndex++;
                        }
                        #region Total
                        rowIndex++;
                        colIndex = 2;
                        cell = sheet.Cells[rowIndex, 2, rowIndex, 11]; cell.Merge = true; cell.Value = "Total"; cell.Style.Font.Bold = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[rowIndex,12]; cell.Value = oFBPBMs.Sum(x => Global.GetMeter(x.Qty, 2)); cell.Style.Font.Bold = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[rowIndex, 13]; cell.Value = ""; cell.Style.Font.Bold = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[rowIndex, 14]; cell.Value = ""; cell.Style.Font.Bold = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[rowIndex, 15]; cell.Value = ""; cell.Style.Font.Bold = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[rowIndex, 16]; cell.Value = ""; cell.Style.Font.Bold = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[rowIndex, 17]; cell.Value = ""; cell.Style.Font.Bold = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[rowIndex, 18]; cell.Value = ""; cell.Style.Font.Bold = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[rowIndex, 19]; cell.Value = ""; cell.Style.Font.Bold = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        cell = sheet.Cells[rowIndex, 20]; cell.Value = ""; cell.Style.Font.Bold = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;



                        rowIndex++;
                        #endregion


                    }
                         
                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=DDailyMachineLog.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
           
                }

                #endregion

              

          


        }
        #endregion

        #region Daily Loom Production Report 
        //public void ExcelDailyProductionReport(string sParam)
        //{
        //    if (string.IsNullOrEmpty(sParam))
        //        throw new Exception("Search parameter required");
        //    DateTime dDate = Convert.ToDateTime(sParam.Split('~')[0]);
        //    int nTSUID = Convert.ToInt32(sParam.Split('~')[1]);
        //    int nShiftID = Convert.ToInt32(sParam.Split('~')[2]);
        //    DateTime dToDate = Convert.ToDateTime(sParam.Split('~')[3]);
        //    string sBuyerIDs = sParam.Split('~')[4];
        //    string sFEOIDs = sParam.Split('~')[5];
        //    int nWeaveType = Convert.ToInt32(sParam.Split('~')[6]);
        //    int nProcessType = Convert.ToInt32(sParam.Split('~')[7]);
        //    string sConstruction = sParam.Split('~')[8];
        //    string sFMIDs = sParam.Split('~')[9];
        //    bool chkNewLoom = Convert.ToBoolean(sParam.Split('~')[10]);
        //    bool chkRunOut = Convert.ToBoolean(sParam.Split('~')[11]);
        //    bool chkLoomDone = Convert.ToBoolean(sParam.Split('~')[12]);
        //    string subQuery = string.Empty;
        //    string subQuery1 = string.Empty;
        //    List<FabricBatchProductionBatchMan> oFBPBMs = new List<FabricBatchProductionBatchMan>();
        //    List<FabricBatchProduction> oFBPs = new List<FabricBatchProduction>();
        //    List<HRMShift> oShifts = new List<HRMShift>();
        //    if (nTSUID > 0)
        //        subQuery += " AND TSUID = " + nTSUID;
        //    if (!string.IsNullOrEmpty(sFMIDs))
        //        subQuery += " AND FMID IN ( " + sFMIDs + ")";
        //    if (!string.IsNullOrEmpty(sFEOIDs))
        //        subQuery1 += " AND FEOID IN ( " + sFEOIDs + ")";
        //    if (!string.IsNullOrEmpty(sBuyerIDs))
        //        subQuery1 += " AND FEOID IN (SELECT FEOID FROM FabricExecutionOrder WHERE BuyerID IN (  " + sBuyerIDs + "))";
        //    if (nProcessType > 0)
        //        subQuery1 += " AND FEOID IN (SELECT FEOID FROM FabricExecutionOrder WHERE ProcessType IN (  " + nProcessType + "))";
        //    if (nWeaveType > 0)
        //        subQuery1 += " AND FEOID IN (SELECT FEOID FROM FabricExecutionOrder WHERE FabricWeave IN (  " + nWeaveType + "))";
        //    if (!string.IsNullOrEmpty(sConstruction))
        //        subQuery1 += " AND FEOID IN (SELECT FEOID FROM View_FabricExecutionOrder WHERE Construction LIKE'%" + sConstruction + "%')";
        //    if (chkLoomDone)
        //        subQuery1 += " AND FEOQty<=(SELECT ISNULL(SUM(Qty),0) FROM View_FabricBatchProductionBatchMan TT WHERE TT.FEOID=FEOID )";

        //    subQuery += "AND FBID IN (SELECT FBID FROM FabricBatch WHERE FBID<>0" + subQuery1 + ")";

        //    string sqlquery = "";
        //    if (chkNewLoom)
        //        sqlquery += " AND CONVERT(DATE,StartTime)='" + DateTime.Now.ToString("dd MMM yyyy") + "'";
        //    if (chkRunOut)
        //        sqlquery += " AND CONVERT(DATE,EndTime)='" + DateTime.Now.ToString("dd MMM yyyy") + "'";
           

        //    string sSQL = "SELECT * FROM View_FabricBatchProductionBatchMan WHERE FBPID IN (SELECT FBPID FROM FabricBatchProduction WHERE WeavingProcess=3 " + subQuery + sqlquery+") AND ApproveBy > 0 AND FinishDate BETWEEN '" + dDate.ToString("dd MMM yyyy") + "' AND '" + dToDate.ToString("dd MMM yyyy") + "'"; ;
        //    if (nShiftID > 0)
        //        sSQL += " AND ShiftID = " + nShiftID;
        //    if (chkLoomDone)
        //        sSQL += " AND (SELECT ISNULL(SUM(Qty),0) FROM View_FabricBatchProductionBatchMan TT WHERE TT.FEOID=FEOID ) >= FEOQty";
        //    oFBPBMs = FabricBatchProductionBatchMan.GetsBySql(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    if (oFBPBMs.Any())
        //    {
        //        string sSQL1 = "SELECT * FROM View_FabricBatchProduction WHERE FBPID IN (" + string.Join(",", oFBPBMs.Select(x => x.FBPID)) + ")  ORDER BY CONVERT(INT,dbo.fnGetNumberFromString(Code)) ASC";
        //        oFBPs = FabricBatchProduction.Gets(sSQL1, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    }

        //    List<FabricBreakage> oFabricBreakages = new List<FabricBreakage>();
        //    List<FabricBatchProductionBreakage> oFabricBatchProductionBreakages = new List<FabricBatchProductionBreakage>();

        //    if (oFBPBMs.Count > 0)
        //    {
        //        oFabricBreakages = FabricBreakage.Gets("SELECT * FROM FabricBreakage", ((User)Session[SessionInfo.CurrentUser]).UserID);
        //        oFabricBatchProductionBreakages = FabricBatchProductionBreakage.Gets("SELECT * FROM View_FabricBatchProductionBreakage WHERE FBPBID IN(" + string.Join(",", oFBPBMs.Select(x => x.FBPBID)) + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    }

        //    var FBPB = oFabricBatchProductionBreakages.GroupBy(x => new {x.FBPID,x.FabricBreakageName,x.ShiftID }, (key, grp) => new
        //    {
        //        FBPID = key.FBPID,
        //        //ShiftID =  grp.Select(x=>x.ShiftID).FirstOrDefault(),
        //        ShiftID = key.ShiftID,
        //        FabricBreakageName = key.FabricBreakageName,
        //        NoOfBreakage = grp.Sum(x=>x.NoOfBreakage),

        //    }).ToList();

        //    List<string> ColBreakages = new List<string>();
        //    ColBreakages = oFabricBatchProductionBreakages.Select(x => x.FabricBreakageName).ToList();//.Where(x => x.NoOfBreakage > 0)
        //    ColBreakages = ColBreakages.Distinct().ToList();

        //    //List<string> ColBreakages = FBPB.GroupBy(p => p.FabricBreakageName).Select(g => g.First()).ToList();


        //    oShifts = HRMShift.Gets("SELECT * FROM HRM_Shift WHERE IsActive=1", ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    int nMaxColumn = 0;
        //    int colIndex = 2;
        //    int rowIndex = 2;
        //    ExcelRange cell;
        //    ExcelFill fill;
        //    OfficeOpenXml.Style.Border border;

        //    using (var excelPackage = new ExcelPackage())
        //    {
        //        excelPackage.Workbook.Properties.Author = "ESimSol";
        //        excelPackage.Workbook.Properties.Title = "Export from ESimSol";
        //        var sheet = excelPackage.Workbook.Worksheets.Add("Daily Production Report");
        //        sheet.Name = "Daily Production Report";

        //        int nCol = 0;

        //        int nColumnWidth = 18;
        //        sheet.Column(2).Width = nColumnWidth; //SL  
        //        sheet.Column(3).Width = nColumnWidth; //Buyer
        //        sheet.Column(4).Width = nColumnWidth; //Order No
        //        sheet.Column(5).Width = nColumnWidth; //Construction
        //        sheet.Column(6).Width = nColumnWidth; //Fabric Type
        //        sheet.Column(7).Width = nColumnWidth; //Texture
        //        sheet.Column(8).Width = nColumnWidth; //No of weft color

        //        sheet.Column(9).Width = nColumnWidth; //M/C No
        //        sheet.Column(10).Width = nColumnWidth; //Shift
        //        sheet.Column(11).Width = nColumnWidth; //RPM
        //        sheet.Column(12).Width = nColumnWidth; //Prod Meter
        //        sheet.Column(13).Width = nColumnWidth; //Eff%
        //        //sheet.Column(14).Width = nColumnWidth; //weft stop
        //        //sheet.Column(15).Width = nColumnWidth; //weft complex
        //        //sheet.Column(16).Width = nColumnWidth; //warp stop
        //        nCol = 13;
        //        //sheet.Column(17).Width = nColumnWidth; //warp complex
        //        for (int i = 0; i < ColBreakages.Count; i++)
        //        {
        //            sheet.Column(nCol++).Width = nColumnWidth; //warp complex
        //        }
        //        sheet.Column(nCol).Width = nColumnWidth; //RunTime

        //        nMaxColumn = 18;

        //        Company oCompany = new Company();
        //        oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //        oCompany.CompanyLogo = GetCompanyLogo(oCompany);

        //        #region Report Header
        //        sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
        //        cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
        //        cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        rowIndex = rowIndex + 1;

        //        sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
        //        cell = sheet.Cells[rowIndex, 2]; cell.Value = "Golora,Charkhanda,Manikgonj"; cell.Style.Font.Bold = true;
        //        cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        rowIndex = rowIndex + 1;

        //        sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
        //        cell = sheet.Cells[rowIndex, 2]; cell.Value = "Daily Production Report"; cell.Style.Font.Bold = true;
        //        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        rowIndex = rowIndex + 1;

        //        sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
        //        cell = sheet.Cells[rowIndex, 2]; cell.Value = "(Weaving Unit)"; cell.Style.Font.Bold = true;
        //        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        rowIndex = rowIndex + 1;

        //        sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
        //        cell = sheet.Cells[rowIndex, 2]; cell.Value = "Production Date : " + DateTime.Now.ToString("dd MMM yyyy"); cell.Style.Font.Bold = true;
        //        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        rowIndex = rowIndex + 2;


        //        #endregion

        //        #region Table Header
        //        colIndex = 1;
        //        string sVal = "";
        //        int tempLastColindex = 0;
        //        for (int i = 1; i < 13; i++)
        //        {
        //            switch (i)
        //            {
        //                case 1: sVal = "SL#"; break;
        //                case 2: sVal = "Buyer"; break;
        //                case 3: sVal = "Order No"; break;
        //                case 4: sVal = "Construction"; break;
                     
        //                case 5: sVal = "Fabric Type"; break;
        //                case 6: sVal = "Texture"; break;
        //                case 7: sVal = "No of weft color"; break;
        //                case 8: sVal = "M/C No"; break;
        //                case 9: sVal = "Shift"; break;
        //                case 10: sVal = "RPM"; break;
        //                case 11: sVal = "Prod Meter"; break;
        //                case 12: sVal = "Eff%"; break;
        //                default: sVal = ""; break;
        //            }
        //            colIndex++;
        //            cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex];
        //            cell.Merge = true;
        //            cell.Value = sVal;
        //            cell.Style.Font.Bold = true;
        //            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            fill = cell.Style.Fill;
        //            fill.PatternType = ExcelFillStyle.Solid;
        //            fill.BackgroundColor.SetColor(Color.Cyan);
        //            border = cell.Style.Border;
        //            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //           tempLastColindex =colIndex+1;
        //        }
        //        #region For Breakage
        //        //cell = sheet.Cells[rowIndex, tempLastColindex, rowIndex, tempLastColindex+3];

        //        int nColBreakage = ColBreakages.Count - 1;
        //        if (ColBreakages.Count > 0)
        //        {
        //            cell = sheet.Cells[rowIndex, tempLastColindex, rowIndex, tempLastColindex + nColBreakage];
        //            cell.Merge = true;
        //            cell.Value = "Breakage";
        //            cell.Style.Font.Bold = true;
        //            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            fill = cell.Style.Fill;
        //            fill.PatternType = ExcelFillStyle.Solid;
        //            fill.BackgroundColor.SetColor(Color.Cyan);
        //            border = cell.Style.Border;
        //            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            foreach (string sItem in ColBreakages)
        //            {
        //                cell = sheet.Cells[rowIndex + 1, tempLastColindex++];
        //                cell.Value = sItem;
        //                cell.Style.Font.Bold = true;
        //                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                fill = cell.Style.Fill;
        //                fill.PatternType = ExcelFillStyle.Solid;
        //                fill.BackgroundColor.SetColor(Color.LightGreen);
        //                border = cell.Style.Border;
        //                border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //            }
                  
        //        }
        //        #endregion

        //        cell = sheet.Cells[rowIndex, tempLastColindex, rowIndex + 1, tempLastColindex];
        //        cell.Merge = true;
        //        cell.Value = "Run Time";
        //        cell.Style.Font.Bold = true;
        //        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        fill = cell.Style.Fill;
        //        fill.PatternType = ExcelFillStyle.Solid;
        //        fill.BackgroundColor.SetColor(Color.Cyan);
        //        border = cell.Style.Border;
        //        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


        //        rowIndex=rowIndex+2;
        //        #endregion

        //        #region Table body

        //        if (oFBPs.Any())
        //        {
                 
        //            var oDistinctMachines = oFBPs.Select(x => x.MachineCode).ToList().Distinct().OrderBy(x => Convert.ToInt32(x));
        //            int SL = 0;
        //            double nGrandTotal = 0;
        //            foreach (var nFMID in oDistinctMachines)
        //            {
                       
        //                List<FabricBatchProduction> otempFBPs = oFBPs.Where(x => x.MachineCode == nFMID).ToList();
        //                int nActiveShiftCount= oShifts.Count()  ;// add one for avg of shift

        //                foreach (FabricBatchProduction oFBP in otempFBPs)
        //                {
        //                    colIndex = 1;
        //                    int startRowindex = rowIndex;
        //                    int lastRowIndex = rowIndex + nActiveShiftCount;
        //                    cell = sheet.Cells[rowIndex, ++colIndex, lastRowIndex, colIndex]; cell.Merge = true; cell.Value = ++SL;
        //                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                    cell = sheet.Cells[rowIndex, ++colIndex, lastRowIndex, colIndex]; cell.Merge = true; cell.Value = oFBP.BuyerName;
        //                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                    cell = sheet.Cells[rowIndex, ++colIndex, lastRowIndex, colIndex]; cell.Merge = true; cell.Value = oFBP.OrderNo;
        //                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                    cell = sheet.Cells[rowIndex, ++colIndex, lastRowIndex, colIndex]; cell.Merge = true; cell.Value = oFBP.Construction;
        //                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                    cell = sheet.Cells[rowIndex, ++colIndex, lastRowIndex, colIndex]; cell.Merge = true; cell.Value = "";
        //                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                    cell = sheet.Cells[rowIndex, ++colIndex, lastRowIndex, colIndex]; cell.Merge = true; cell.Value = oFBP.Texture;
        //                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                    cell = sheet.Cells[rowIndex, ++colIndex, lastRowIndex, colIndex]; cell.Merge = true; cell.Value = "";
        //                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                    cell = sheet.Cells[rowIndex, ++colIndex, lastRowIndex, colIndex]; cell.Merge = true; cell.Value = oFBP.MachineCode;
        //                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //                    int nStartColindex = colIndex;
        //                    foreach (HRMShift oShift in oShifts.OrderBy(x=>x.ShiftID))
        //                    {
                               
        //                        List<FabricBatchProductionBatchMan> otempFBPBMs = oFBPBMs.Where(x => x.ShiftID == oShift.ShiftID && x.FBPID == oFBP.FBPID).ToList();
        //                        cell = sheet.Cells[rowIndex, ++nStartColindex]; cell.Value = oShift.Name;
        //                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

        //                        cell = sheet.Cells[rowIndex, ++nStartColindex]; cell.Value = otempFBPBMs.Sum(x => x.RPM);
        //                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

        //                        cell = sheet.Cells[rowIndex, ++nStartColindex]; cell.Value = Math.Round(otempFBPBMs.Sum(x => x.QtyinMeter), 0);
        //                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

        //                        cell = sheet.Cells[rowIndex, ++nStartColindex]; cell.Value = otempFBPBMs.Sum(x => x.Efficiency);
        //                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

        //                        if (ColBreakages.Count > 0)
        //                        {
        //                            foreach (string item in ColBreakages)
        //                            {
        //                                int nBrackageCount = Convert.ToInt32(FBPB.Where(x => x.ShiftID == oShift.ShiftID && x.FBPID == oFBP.FBPID && x.FabricBreakageName == item).Select(x => x.NoOfBreakage).FirstOrDefault());
        //                                cell = sheet.Cells[rowIndex, ++nStartColindex]; cell.Value = nBrackageCount;
        //                                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //                            }
        //                        }
                                
        //                        nStartColindex = colIndex;
        //                        rowIndex++;
        //                    }
        //                    //For Avg COlumn ShiftWise

        //                    cell = sheet.Cells[rowIndex, ++nStartColindex]; cell.Value = "AVG";
        //                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                           
        //                   cell = sheet.Cells[rowIndex, ++nStartColindex]; cell.Value = (oFBPBMs.Where(x => x.FBPID == oFBP.FBPID).Where(x => x.RPM > 0).Count() > 0) ?Math.Round(Convert.ToDouble((oFBPBMs.Where(x => x.FBPID == oFBP.FBPID).Where(x => x.RPM > 0).ToList().Sum(x => x.RPM)) / (oFBPBMs.Where(x => x.FBPID == oFBP.FBPID).Where(x => x.RPM > 0).Count())),0) : 0;
        //                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //                     nGrandTotal+=( oFBPBMs.Where(x => x.FBPID == oFBP.FBPID).Sum(x => x.QtyinMeter));
        //                    cell = sheet.Cells[rowIndex, ++nStartColindex]; cell.Value = oFBPBMs.Where(x => x.FBPID == oFBP.FBPID).Sum(x => x.QtyinMeter);
        //                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

        //                    cell = sheet.Cells[rowIndex, ++nStartColindex]; cell.Value = (oFBPBMs.Where(x => x.FBPID == oFBP.FBPID).Where(x => x.Efficiency > 0).Count() > 0) ? Math.Round(Convert.ToDouble((oFBPBMs.Where(x => x.FBPID == oFBP.FBPID).Where(x => x.Efficiency > 0).ToList().Sum(x => x.Efficiency)) / (oFBPBMs.Where(x => x.FBPID == oFBP.FBPID).Where(x => x.Efficiency > 0).Count())), 0) : 0;
        //                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;


        //                    foreach (string item in ColBreakages)
        //                    {
        //                        double nBrackageCount = Convert.ToDouble(FBPB.Where(x => x.FBPID == oFBP.FBPID && x.FabricBreakageName == item).Sum(x => x.NoOfBreakage));
        //                        double nBreakasgeShiftCount = FBPB.Where(x => x.FBPID == oFBP.FBPID && x.FabricBreakageName == item).Count();
        //                        cell = sheet.Cells[rowIndex, ++nStartColindex]; cell.Value = (nBreakasgeShiftCount>0)?Math.Round(nBrackageCount/nBreakasgeShiftCount,0):0;
        //                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //                    }

                            
        //                    rowIndex++;

        //                    cell = sheet.Cells[startRowindex, ++nStartColindex, lastRowIndex, nStartColindex]; cell.Merge = true; cell.Value = "";
        //                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                }
                        


        //            }

        //            #region Summary

        //            cell = sheet.Cells[rowIndex, 2, rowIndex, 10]; cell.Merge = true; cell.Value = "Total"; cell.Style.Font.Bold = true;
        //            cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //            fill = cell.Style.Fill;
        //            fill.PatternType = ExcelFillStyle.Solid;
        //            fill.BackgroundColor.SetColor(Color.YellowGreen);
        //            border = cell.Style.Border;
        //            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                  
        //            cell = sheet.Cells[rowIndex, 11, rowIndex, 11]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true;
        //            cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //            fill = cell.Style.Fill;
        //            fill.PatternType = ExcelFillStyle.Solid;
        //            fill.BackgroundColor.SetColor(Color.YellowGreen);
        //            border = cell.Style.Border;
        //            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                   
        //            cell = sheet.Cells[rowIndex, 12, rowIndex, 12]; cell.Merge = true; cell.Value = nGrandTotal; cell.Style.Font.Bold = true;
        //            cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //            fill = cell.Style.Fill;
        //            fill.PatternType = ExcelFillStyle.Solid;
        //            fill.BackgroundColor.SetColor(Color.YellowGreen);
        //            border = cell.Style.Border;
        //            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                  
        //            cell = sheet.Cells[rowIndex, 13, rowIndex, 13]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true;
        //            cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //            fill = cell.Style.Fill;
        //            fill.PatternType = ExcelFillStyle.Solid;
        //            fill.BackgroundColor.SetColor(Color.YellowGreen);
        //            border = cell.Style.Border;
        //            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //            tempLastColindex = 14;
        //            nColBreakage = ColBreakages.Count - 1;
        //            if (ColBreakages.Count > 0)
        //            {
        //                cell = sheet.Cells[rowIndex, tempLastColindex, rowIndex, tempLastColindex + nColBreakage];
        //                cell.Merge = true;
        //                cell.Value = "";
        //                cell.Style.Font.Bold = true;
        //                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                fill = cell.Style.Fill;
        //                fill.PatternType = ExcelFillStyle.Solid;
        //                fill.BackgroundColor.SetColor(Color.YellowGreen);
        //                border = cell.Style.Border;
        //                border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                //foreach (string sItem in ColBreakages)
        //                //{
        //                //    cell = sheet.Cells[rowIndex + 1, tempLastColindex++];
        //                //    cell.Value = sItem;
        //                //    cell.Style.Font.Bold = true;
        //                //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //                //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                //    fill = cell.Style.Fill;
        //                //    fill.PatternType = ExcelFillStyle.Solid;
        //                //    fill.BackgroundColor.SetColor(Color.YellowGreen);
        //                //    border = cell.Style.Border;
        //                //    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                //}

        //            }


        //            rowIndex = rowIndex + 1;
        //            #endregion
                  

        //        }

        //        Response.ClearContent();
        //        Response.BinaryWrite(excelPackage.GetAsByteArray());
        //        Response.AddHeader("content-disposition", "attachment; filename=DailyProduction.xlsx");
        //        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //        Response.Flush();
        //        Response.End();

        //    }

        //        #endregion






        //}
        #endregion
    }
}
