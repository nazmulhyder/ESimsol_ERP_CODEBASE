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
using System.Drawing.Printing;
using System.Reflection;
using OfficeOpenXml;
using OfficeOpenXml.Style;


namespace ESimSolFinancial.Controllers
{
    public class RPT_LotTrackingsController : Controller
    {
        #region Declaration
        RPT_LotTrackings _oRPT_LotTrackings = new RPT_LotTrackings();
        List<RPT_LotTrackings> _oRPT_LotTrackingss = new List<RPT_LotTrackings>();
        string sFeedBackMessage = "";

        #endregion

        public ActionResult ViewRPT_LotTrackingss(int menuid, int buid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<RPT_LotTrackings> oRPT_LotTrackingss = new List<RPT_LotTrackings>();

            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.BUID = buid;
            return View(oRPT_LotTrackingss);
        }

        [HttpPost]
        public JsonResult AdvSearch(RPT_LotTrackings oRPT_LotTrackings)
        {
            List<RPT_LotTrackings> oRPT_LotTrackingss = new List<RPT_LotTrackings>();
            try
            {
                int nBUID = Convert.ToInt32(oRPT_LotTrackings.Params.Split('~')[7]);
                int nReportType = Convert.ToInt32(oRPT_LotTrackings.Params.Split('~')[8]);
                string sSQL = MakeSQL(oRPT_LotTrackings.Params);
                oRPT_LotTrackingss = RPT_LotTrackings.Gets(sSQL, nReportType, nBUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oRPT_LotTrackings.ErrorMessage = ex.Message;
                oRPT_LotTrackingss.Add(oRPT_LotTrackings);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRPT_LotTrackingss);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        private string MakeSQL(string sString)
        {
            int nCount = 0;
            string sLCNo = "";
            string sInvNo = "";
            int nGRNDate = 0;
            DateTime dGRNDate_Start = DateTime.MinValue;
            DateTime dGRNDate_End = DateTime.MinValue;

            string sProductIDs = "";
            string sLotIDs = "";

            if (!String.IsNullOrEmpty(sString))
            {
                sLCNo = Convert.ToString(sString.Split('~')[nCount++]);
                sInvNo = Convert.ToString(sString.Split('~')[nCount++]);
                nGRNDate = Convert.ToInt16(sString.Split('~')[nCount++]);
                dGRNDate_Start = Convert.ToDateTime(sString.Split('~')[nCount++]);
                dGRNDate_End = Convert.ToDateTime(sString.Split('~')[nCount++]);
                sProductIDs = Convert.ToString(sString.Split('~')[nCount++]);
                sLotIDs = Convert.ToString(sString.Split('~')[nCount++]);
            }

            string sReturn = " ";/// and 

            #region Product
            if (!string.IsNullOrEmpty(sProductIDs))
            {
                sReturn = sReturn + "AND ProductID IN (" + sProductIDs + ")";
            }
            #endregion

            #region Lot
            if (!string.IsNullOrEmpty(sLotIDs))
            {
                sReturn = sReturn + "AND LotID IN (" + sLotIDs + ")";
            }
            #endregion
            #region nGRNDate Date
            if (nGRNDate != (int)EnumCompareOperator.None)
            {
                Global.TagSQL(ref sReturn);
                if (nGRNDate == (int)EnumCompareOperator.EqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DBServerDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dGRNDate_Start.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nGRNDate == (int)EnumCompareOperator.NotEqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DBServerDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dGRNDate_Start.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nGRNDate == (int)EnumCompareOperator.GreaterThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DBServerDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dGRNDate_Start.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nGRNDate == (int)EnumCompareOperator.SmallerThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DBServerDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dGRNDate_Start.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nGRNDate == (int)EnumCompareOperator.Between)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DBServerDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dGRNDate_Start.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dGRNDate_End.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nGRNDate == (int)EnumCompareOperator.NotBetween)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DBServerDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dGRNDate_Start.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dGRNDate_End.ToString("dd MMM yyyy") + "',106)) ";
                }
            }
            #endregion
            #region Invoice No
            if (!string.IsNullOrEmpty(sInvNo))
            {
                sReturn = sReturn + " and LotID in (Select LotID from ITransaction where TriggerParentType=103 and TriggerParentID in (Select GRNDetailID from GRNDetail where GRNID in (Select GRNID from GRN where GRN.GRNType>0 and GRN.RefObjectID in (Select ImportInvoiceID from ImportInvoice where ImportInvoiceNo like '%" + sInvNo + "%' ))))";
            }
            #endregion

            #region L/C No
            if (!string.IsNullOrEmpty(sLCNo))
            {
                sReturn = sReturn + " and LotID in (Select LotID from ITransaction where TriggerParentType=103 and TriggerParentID in (Select GRNDetailID from GRNDetail where GRNID in (Select GRNID from GRN where GRN.GRNType=2 and GRN.RefObjectID in (Select ImportInvoiceID from View_ImportInvoice where ImportLCNo like '%" + sLCNo + "%' ))))";
            }
            #endregion
            return sReturn;
        }
        public void ExportToExcel(string sParams, int nReportType, int nBUID)
        {
            List<RPT_LotTrackings> oRPT_LotTrackingss = new List<RPT_LotTrackings>();
            string sString = MakeSQL(sParams);
            oRPT_LotTrackingss = RPT_LotTrackings.Gets(sString, nReportType, nBUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(nBUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            #region Header
            List<TableHeader> table_header = new List<TableHeader>();
            table_header.Add(new TableHeader { Header = "SL.",                   Width = 6f,    IsRotate = false,   Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "Lot No",                Width = 18f,   IsRotate = false,   Align = TextAlign.Left });
            table_header.Add(new TableHeader { Header = "Code",          Width = 15f,   IsRotate = false,   Align = TextAlign.Left });
            table_header.Add(new TableHeader { Header = "Product Name", Width = 35f, IsRotate = false, Align = TextAlign.Left });
            table_header.Add(new TableHeader { Header = "Supplier Name",         Width = 50f,   IsRotate = false,   Align = TextAlign.Left });
            table_header.Add(new TableHeader { Header = "L/C No",                 Width = 25f,   IsRotate = false,   Align = TextAlign.Left });
            table_header.Add(new TableHeader { Header = "Invoice No", Width = 25f, IsRotate = false, Align = TextAlign.Left });
            table_header.Add(new TableHeader { Header = "Unit",                  Width = 8f,    IsRotate = false,   Align = TextAlign.Left });
            table_header.Add(new TableHeader { Header = "Raw Received", Width = 15f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "Raw Issue to Soft Cone", Width = 15f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "Raw Issue To Dyeing Floor", Width = 15f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "Dyed Yarn Received", Width = 15f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "Recycle (Qty)",         Width = 15f,   IsRotate = false,   Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "Westage (Qty)",         Width = 15f,   IsRotate = false,   Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "Production Loss/(Gain)", Width = 15f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "Total",          Width = 15f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "WIP",        Width = 15f,   IsRotate = false,   Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "Delivery Qty", Width = 15f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "Transfer From Delivery Store", Width = 15f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "Dyed Yarn(Stock)", Width = 15f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "Raw Yarn(Stock)", Width = 15f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "Soft Cone Store", Width = 15f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "Recycle Received", Width = 15f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "Recycle  to Soft Cone", Width = 15f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "Recycle Issue To Dyeing Floor", Width = 15f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "Dyed Yarn Received", Width = 15f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "Recycle (Qty)", Width = 15f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "Westage (Qty)", Width = 15f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "Production Loss/Gain", Width = 15f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "Total", Width = 15f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "WIP(C-H)", Width = 15f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "Delivery(Qty)", Width = 15f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "Dyed Yarn", Width = 15f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "Recycle  Yarn ", Width = 15f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "Soft Cone Store", Width = 15f, IsRotate = false, Align = TextAlign.Right });


            #endregion

            #region Export Excel
            int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count() + nStartCol;
            ExcelRange cell; ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Lot Usage History Report";
                var sheet = excelPackage.Workbook.Worksheets.Add("Lot Usage History Report");
                sheet.Name = "Lot Usage History Report";

                ExcelTool.SetColumnWidth(table_header, ref sheet, ref nStartCol, ref nEndCol);
                nEndCol = 25;
                #region Report Header
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oBusinessUnit.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex++;

                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "Lot Usage History Report"; cell.Style.Font.Bold = true;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Address & Date
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oBusinessUnit.Address; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex++;
                #endregion

                #region Data
                nRowIndex++;
                nStartCol = 2;
                int nSL = 1;
                nEndCol = table_header.Count() + nStartCol;
                ExcelTool.GenerateHeader(table_header, ref sheet, ref nRowIndex, 2, nEndCol, 11, true, true);
                RPT_LotTrackings oRPT_LotTrackings = new RPT_LotTrackings();
                //nRowIndex++;
                nStartCol = 2;
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Right, false, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++,"", false, ExcelHorizontalAlignment.Left, false, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++,"", false, ExcelHorizontalAlignment.Left, false, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++,"", false, ExcelHorizontalAlignment.Left, false, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);

                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "A", false, ExcelHorizontalAlignment.Right, false, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "B", false, ExcelHorizontalAlignment.Right, false, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "C", false, ExcelHorizontalAlignment.Right, false, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "D", false, ExcelHorizontalAlignment.Right, false, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "E", false, ExcelHorizontalAlignment.Right, false, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "F", false, ExcelHorizontalAlignment.Right, false, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "G", false, ExcelHorizontalAlignment.Right, false, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "H(D+E+F+G)", false, ExcelHorizontalAlignment.Right, false, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "I(C-H)", false, ExcelHorizontalAlignment.Right, false, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "J", false, ExcelHorizontalAlignment.Right, false, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "K", false, ExcelHorizontalAlignment.Right, false, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "L(D-J-K)", false, ExcelHorizontalAlignment.Right, false, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "M(A-C)", false, ExcelHorizontalAlignment.Right, false, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "N(B-C)", false, ExcelHorizontalAlignment.Right, false, false);

                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "A", false, ExcelHorizontalAlignment.Right, false, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "B", false, ExcelHorizontalAlignment.Right, false, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "C", false, ExcelHorizontalAlignment.Right, false, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "D", false, ExcelHorizontalAlignment.Right, false, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "E", false, ExcelHorizontalAlignment.Right, false, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "F", false, ExcelHorizontalAlignment.Right, false, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "G", false, ExcelHorizontalAlignment.Right, false, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "H(D+E+F+G)", false, ExcelHorizontalAlignment.Right, false, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "I", false, ExcelHorizontalAlignment.Right, false, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "J", false, ExcelHorizontalAlignment.Right, false, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "K(D-J)", false, ExcelHorizontalAlignment.Right, false, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "L(A-C)", false, ExcelHorizontalAlignment.Right, false, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "M(B-C)", false, ExcelHorizontalAlignment.Right, false, false); 

                nRowIndex++;

                foreach (var obj in oRPT_LotTrackingss)
                {
                    nStartCol = 2;
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, nSL++.ToString(), false, ExcelHorizontalAlignment.Right, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.LotNo, false, ExcelHorizontalAlignment.Left, false, false);
                    //ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.ProductCategoryName, false, ExcelHorizontalAlignment.Left, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.ProductCode, false, ExcelHorizontalAlignment.Left, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.ProductName, false, ExcelHorizontalAlignment.Left, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.ContractorName, false, ExcelHorizontalAlignment.Left, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.LCNo, false, ExcelHorizontalAlignment.Left, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.InvoiceNo, false, ExcelHorizontalAlignment.Left, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.MUnit, false, ExcelHorizontalAlignment.Left, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.RawReced, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false); oRPT_LotTrackings.QtyGRN = obj.QtyGRN + oRPT_LotTrackings.QtyGRN; oRPT_LotTrackings.QtyAdjIn = obj.QtyAdjIn + oRPT_LotTrackings.QtyAdjIn; oRPT_LotTrackings.QtyAdjOut = obj.QtyAdjOut + oRPT_LotTrackings.QtyAdjOut;
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.QtySoft, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false); oRPT_LotTrackings.QtySoft = obj.QtySoft + oRPT_LotTrackings.QtySoft;
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.QtyProIn, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false); oRPT_LotTrackings.QtyProIn = obj.QtyProIn + oRPT_LotTrackings.QtyProIn;
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.QtyFinish, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false); oRPT_LotTrackings.QtyFinish = obj.QtyFinish + oRPT_LotTrackings.QtyFinish;
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.QtyRecycle, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false); oRPT_LotTrackings.QtyRSCancel = obj.QtyRSCancel + oRPT_LotTrackings.QtyRSCancel; oRPT_LotTrackings.QtyRecycle = obj.QtyRecycle + oRPT_LotTrackings.QtyRecycle;
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.QtyWestage, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false); oRPT_LotTrackings.QtyWestage = obj.QtyWestage + oRPT_LotTrackings.QtyWestage;
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.QtyShort, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false); oRPT_LotTrackings.QtyShort = obj.QtyShort + oRPT_LotTrackings.QtyShort;
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.Total, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.QtyWIP, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.QtyActualDelivery, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false); oRPT_LotTrackings.QtyDelivery = obj.QtyDelivery + oRPT_LotTrackings.QtyDelivery; oRPT_LotTrackings.QtyReturn = obj.QtyReturn + oRPT_LotTrackings.QtyReturn;
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.QtyTR, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false); oRPT_LotTrackings.QtyTR = obj.QtyTR + oRPT_LotTrackings.QtyTR; 
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.DyedYarnStock, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.RawYarnStock, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Right, false, false); //oRPT_LotTrackings.QtyProProcess = obj.QtyProProcess + oRPT_LotTrackings.QtyProProcess;
                    //22
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.Recycle_Recd, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false);  oRPT_LotTrackings.Recycle_Recd = obj.Recycle_Recd + oRPT_LotTrackings.Recycle_Recd;
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.QtySoft_Dye, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false); oRPT_LotTrackings.QtySoft_Dye = obj.QtySoft_Dye + oRPT_LotTrackings.QtySoft_Dye;
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.QtyProInDye, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false); oRPT_LotTrackings.QtyProInDye = obj.QtyProInDye + oRPT_LotTrackings.QtyProInDye;
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.QtyFinish_Dye, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false); oRPT_LotTrackings.QtyFinish_Dye = obj.QtyFinish_Dye + oRPT_LotTrackings.QtyFinish_Dye;
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.QtyRecycle_Dye + obj.QtyRSCancel_Dye, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false); oRPT_LotTrackings.QtyRecycle_Dye = obj.QtyRecycle_Dye + oRPT_LotTrackings.QtyRecycle_Dye; oRPT_LotTrackings.QtyRSCancel_Dye = obj.QtyRSCancel_Dye + oRPT_LotTrackings.QtyRSCancel_Dye;
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.QtyWestage_Dye, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false); oRPT_LotTrackings.QtyWestage_Dye = obj.QtyWestage_Dye + oRPT_LotTrackings.QtyWestage_Dye;
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.QtyShort_Dye, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false); oRPT_LotTrackings.QtyShort_Dye = obj.QtyShort_Dye + oRPT_LotTrackings.QtyShort_Dye;
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.TotalDye, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false); 
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.QtyWIPDye, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false); //oRPT_LotTrackings.QtyShort = obj.QtyShort + oRPT_LotTrackings.QtyShort;
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.QtyActualDeliveryDye, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false); oRPT_LotTrackings.QtyDelivery_Dye = obj.QtyDelivery_Dye + oRPT_LotTrackings.QtyDelivery_Dye; oRPT_LotTrackings.QtyReturn_Dye = obj.QtyReturn_Dye + oRPT_LotTrackings.QtyReturn_Dye;
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.DyedYarnStockDye, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Right, false, false);// oRPT_LotTrackings.CurrentBalanceDye = obj.CurrentBalanceDye + oRPT_LotTrackings.CurrentBalanceDye;
                    nRowIndex++;
                }
                #region total
                nStartCol = 2;
              //  return this.QtyDelivery - this.QtyReturn;
               //  this.QtyFinish + this.QtyRecycle  + this.QtyWestage+this.QtyShort
              //  this.QtyProIn - (this.QtyFinish + this.QtyRecycle + this.QtyWestage + this.QtyShort);
                //ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                //ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oRPT_LotTrackings.LotNo, false, ExcelHorizontalAlignment.Left, false, false);
                //ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oRPT_LotTrackings.ProductCategoryName, false, ExcelHorizontalAlignment.Left, false, false);
                //ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oRPT_LotTrackings.ProductName, false, ExcelHorizontalAlignment.Left, false, false);
                //ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oRPT_LotTrackings.ProductCode, false, ExcelHorizontalAlignment.Left, false, false);
                //ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oRPT_LotTrackings.ContractorName, false, ExcelHorizontalAlignment.Left, false, false);
                //ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oRPT_LotTrackings.LCNo, false, ExcelHorizontalAlignment.Left, false, false);
                ExcelTool.FillCellMerge(ref sheet, "Total :", nRowIndex, nRowIndex, nStartCol, 8, true, ExcelHorizontalAlignment.Right, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, 10, Math.Round(oRPT_LotTrackings.RawReced, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, 11, Math.Round(oRPT_LotTrackings.QtySoft, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, 12, Math.Round(oRPT_LotTrackings.QtyProIn, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, 13, Math.Round(oRPT_LotTrackings.QtyFinish, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);

                ExcelTool.FillCellBasic(sheet, nRowIndex, 14, Math.Round(oRPT_LotTrackings.QtyRecycle, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, 15, Math.Round(oRPT_LotTrackings.QtyWestage, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, 16, Math.Round(oRPT_LotTrackings.QtyShort, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, 17, Math.Round(oRPT_LotTrackings.Total, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, 18, Math.Round(oRPT_LotTrackings.QtyWIP, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, 19, Math.Round(oRPT_LotTrackings.QtyActualDelivery, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, 20, Math.Round(oRPT_LotTrackings.QtyTR, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, 21, Math.Round(oRPT_LotTrackings.DyedYarnStock, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, 22, Math.Round(oRPT_LotTrackings.RawYarnStock, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, 23, "", false, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, 24, Math.Round(oRPT_LotTrackings.Recycle_Recd, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, 25, Math.Round(oRPT_LotTrackings.QtySoft_Dye, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, 26, Math.Round(oRPT_LotTrackings.QtyProInDye, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, 27, Math.Round(oRPT_LotTrackings.QtyFinish_Dye, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, 28, Math.Round(oRPT_LotTrackings.QtyRecycle_Dye + oRPT_LotTrackings.QtyRSCancel_Dye, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, 29, Math.Round(oRPT_LotTrackings.QtyWestage_Dye, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, 30, Math.Round(oRPT_LotTrackings.QtyShort_Dye, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, 31, Math.Round(oRPT_LotTrackings.TotalDye, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, 32, Math.Round(oRPT_LotTrackings.QtyWIPDye, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, 33, Math.Round(oRPT_LotTrackings.QtyActualDeliveryDye, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, 34, Math.Round(oRPT_LotTrackings.DyedYarnStockDye, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, 35, "", false, ExcelHorizontalAlignment.Right, true, false);
                //ExcelTool.FillCellBasic(sheet, nRowIndex, 24, Math.Round(oRPT_LotTrackings.CurrentBalanceDye, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                #endregion
                #endregion
                nRowIndex++;
                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 2];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=LotUsageHistoryReport.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
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
        public ActionResult PrintStockLotTracking(string sTempString, double nts)
        {
            int nBUID = Convert.ToInt32(sTempString.Split('~')[0]);
            int nLotID = Convert.ToInt32(sTempString.Split('~')[1]);
         
            _oRPT_LotTrackingss = RPT_LotTrackings.Gets("AND LotID IN (" + nLotID + ")", 0, nBUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<LotTraking> oLotTrakings = new List<LotTraking>();
            oLotTrakings = LotTraking.Gets_Lot(nBUID, nLotID.ToString(), (int)Session[SessionInfo.currentUserID]);

            List<DUOrderRS> oDUOrderRSs = new List<DUOrderRS>();
            oDUOrderRSs = DUOrderRS.GetsQCByRaqLot(nLotID,0, (int)Session[SessionInfo.currentUserID]);


            oLotTrakings = oLotTrakings.OrderBy(x => x.WorkingUnitID).ToList();
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(nBUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            rptLotTraking oReport = new rptLotTraking();
            byte[] abytes = oReport.PrepareReportRS(oDUOrderRSs,_oRPT_LotTrackingss, oLotTrakings, oCompany, oBusinessUnit);
            return File(abytes, "application/pdf");

        }
        
    }
}