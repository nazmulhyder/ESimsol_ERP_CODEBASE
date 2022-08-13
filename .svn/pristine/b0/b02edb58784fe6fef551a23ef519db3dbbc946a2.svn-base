using System;
using System.Data;
using ESimSol.BusinessObjects;
using ICS.Core;
using System.Linq;
using ICS.Core.Utility;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;


namespace ESimSol.Reports
{
    public class rptWeavingStatement
    {
        #region Declaration
        int _nTotalColumn = 1;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyleBold;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        FabricBatch _oFabricBatch = new FabricBatch();
        FabricSalesContractDetail _oFabricSalesContractDetail = new FabricSalesContractDetail();
        FabricExecutionOrderSpecification _oFEOS = new FabricExecutionOrderSpecification();
        Company _oCompany = new Company();
        List<FabricWarpPlan> _oFabricWarpPlans = new List<FabricWarpPlan>();
        List<FabricBatchProduction> _oWarpingExecutions = new List<FabricBatchProduction>();
        List<FabricSizingPlan> _oFabricSizingPlans = new List<FabricSizingPlan>();
        List<FabricBatchProduction> _oSizingExecutions = new List<FabricBatchProduction>();
        List<FabricBatchProduction> _oDrawingExecutions = new List<FabricBatchProduction>();
        List<FabricBatchProduction> _oLeasingExecutions = new List<FabricBatchProduction>();
        List<FabricLoomPlan> _oFabricLoomPlans = new List<FabricLoomPlan>();
        List<FabricBatchLoom> _oFabricBatchLooms = new List<FabricBatchLoom>();
        List<FabricBatch> _oFabricBatchs = new List<FabricBatch>();
        List<FabricBatchQC> _oFabricBatchQCs = new List<FabricBatchQC>();
        #endregion

        public byte[] PrepareReport(List<FabricBatch> oFabricBatchs, FabricSalesContractDetail oFabricSalesContractDetail, FabricExecutionOrderSpecification oFEOS, Company oCompany, List<FabricWarpPlan> oFabricWarpPlans, List<FabricBatchProduction> oWarpingExecutions, List<FabricSizingPlan> oFabricSizingPlans, List<FabricBatchProduction> oSizingExecutions, List<FabricBatchProduction> oDrawingExecutions, List<FabricBatchProduction> oLeasingExecutions, List<FabricLoomPlan> oFabricLoomPlans, List<FabricBatchLoom> oFabricBatchLooms, List<FabricBatchQC> oFabricBatchQCs)
        {
            _oFabricBatchs = oFabricBatchs;
            _oFabricSalesContractDetail = oFabricSalesContractDetail;
            _oFEOS = oFEOS;
            _oCompany = oCompany;
            _oFabricWarpPlans = oFabricWarpPlans;
            _oWarpingExecutions = oWarpingExecutions;
            _oFabricSizingPlans = oFabricSizingPlans;
            _oSizingExecutions = oSizingExecutions;
            _oDrawingExecutions = oDrawingExecutions;
            _oLeasingExecutions = oLeasingExecutions;
            _oFabricLoomPlans = oFabricLoomPlans;
            _oFabricBatchLooms = oFabricBatchLooms;
            _oFabricBatchQCs = oFabricBatchQCs;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            _oDocument.SetMargins(25f, 25f, 5f, 25f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 595f });
            #endregion

            this.PrintHeader();
            this.PrintBody();
            //_oPdfPTable.HeaderRows = 4;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Header

        private void PrintHeader()
        {
            #region CompanyHeader


            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 80f, 300.5f, 80f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            if (_oCompany.CompanyLogo != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(70f, 40f);
                _oPdfPCell = new PdfPCell(_oImag);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            }
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Rowspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oFontStyle = FontFactory.GetFont("Tahoma", 20f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));

            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.PringReportHead, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            #region ReportHeader
            #region Blank Space
            _oFontStyle = FontFactory.GetFont("Tahoma", 5f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion
            #endregion

            #endregion
        }
        private void PrintHeaderDetail(string sReportHead, string sOrderType, string sDate)
        {
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.SetWidths(new float[] { 170f, 200f, 170f });
            #region
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, sReportHead, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, sDate, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, sOrderType, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, FontFactory.GetFont("Tahoma", 11f, iTextSharp.text.Font.BOLDITALIC));
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Reporting Date:" + DateTime.Now.ToString("dd MMM yyyy hh:mm tt"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, false, 0, _oFontStyle);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            #endregion


        }
        #endregion

        #region Report Body
        private void PrintBody()
        {
            this.PrintHeaderDetail("Weaving Statement", " ", _oFabricSalesContractDetail.SCNoFull);
            this.SetBasicInfo();
            this.SetSummary();

            foreach (FabricBatch oFB in _oFabricBatchs)
            {
                SetFabricBatchInfo(oFB);
                //List<FabricWarpPlan> oTempFabricWarpPlans = new List<FabricWarpPlan>();
                //oTempFabricWarpPlans = _oFabricWarpPlans.Where(x => x.FBID == oFB.FBID).ToList();
                //SetWarpingPlan(oFB, oTempFabricWarpPlans);

                List<FabricBatchProduction> oTempWarpingExecutions = new List<FabricBatchProduction>();
                oTempWarpingExecutions = _oWarpingExecutions.Where(x => x.FBID == oFB.FBID).ToList();
                SetWarpingExecution(oFB, oTempWarpingExecutions);

                //List<FabricSizingPlan> oTempFabricSizingPlans = new List<FabricSizingPlan>();
                //oTempFabricSizingPlans = _oFabricSizingPlans.Where(x => x.FBID == oFB.FBID).ToList();
                //SetSizingPlan(oFB, oTempFabricSizingPlans);

                List<FabricBatchProduction> oTempSizingExecutions = new List<FabricBatchProduction>();
                oTempSizingExecutions = _oSizingExecutions.Where(x => x.FBID == oFB.FBID).ToList();
                SetSizingExecution(oFB, oTempSizingExecutions);

                List<FabricBatchProduction> oTempDrawingExecutions = new List<FabricBatchProduction>();
                oTempDrawingExecutions = _oDrawingExecutions.Where(x => x.FBID == oFB.FBID).ToList();
                SetDrawingExecution(oFB, oTempDrawingExecutions);

                List<FabricBatchProduction> oTempLeasingExecutions = new List<FabricBatchProduction>();
                oTempLeasingExecutions = _oLeasingExecutions.Where(x => x.FBID == oFB.FBID).ToList();
                SetLeasingExecution(oFB, oTempLeasingExecutions);

                List<FabricBatchLoom> oTempFabricBatchLooms = new List<FabricBatchLoom>();
                oTempFabricBatchLooms = _oFabricBatchLooms.Where(x => x.FBID == oFB.FBID).ToList();
                SetLoomExecution(oFB, oTempFabricBatchLooms);
            }

        }
        #endregion

        private void SetBasicInfo()
        {
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            PdfPTable oPdfPTable = new PdfPTable(6);
            oPdfPTable.SetWidths(new float[] { 15f, 18f, 15f, 18f, 15f, 18f });

            #region Basic Info
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Buyer: ", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricSalesContractDetail.BuyerName, 0, 3, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "MKT Concern: ", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricSalesContractDetail.ContractorName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Construction: ", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricSalesContractDetail.Construction, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Composition: ", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFEOS.Composition, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "SC Date: ", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricSalesContractDetail.SCDateSt, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Design: ", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricSalesContractDetail.FabricWeaveName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Reed No: ", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFEOS.Reed + "/" + _oFEOS.Dent, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total Ends: ", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFEOS.TotalEnds.ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, oPdfPTable.NumberOfColumns, Element.ALIGN_LEFT, BaseColor.WHITE, false, 12, _oFontStyle);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            #endregion


        }

        private void SetSummary()
        {
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            PdfPTable oPdfPTable = new PdfPTable(8);
            oPdfPTable.SetWidths(new float[] { 14f, 12f, 12f, 12f, 12f, 12f, 12f, 14f });

            #region Basic Info
            oPdfPTable = new PdfPTable(8);
            oPdfPTable.SetWidths(new float[] { 14f, 12f, 12f, 12f, 12f, 12f, 12f, 14f });

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Req. Warp Length", 0, 0, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Warping Plan", 0, 0, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Warping", 0, 0, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Sizing Plan", 0, 0, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Sizing", 0, 0, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Loom Plan", 0, 0, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Loom", 0, 0, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Inspection", 0, 0, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFEOS.RequiredWarpLength.ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricWarpPlans.Sum(x=>x.QtyInM).ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oWarpingExecutions.Sum(x => x.QtyInMeterSt).ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricSizingPlans.Sum(x => x.QtyInM).ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oSizingExecutions.Sum(x => x.QtyInMeterSt).ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oDrawingExecutions.Sum(x => x.Qty).ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oLeasingExecutions.Sum(x => x.Qty).ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricLoomPlans.Sum(x => x.QtyInM).ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricBatchLooms.Sum(x => x.QtyInMtr).ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricBatchQCs.Sum(x => x.LoomQtyInMtr).ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Pending", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, (_oFEOS.RequiredWarpLength - _oFabricWarpPlans.Sum(x => x.QtyInM)).ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, (_oFEOS.RequiredWarpLength - _oWarpingExecutions.Sum(x => x.QtyInMeterSt)).ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, (_oFEOS.RequiredWarpLength - _oFabricSizingPlans.Sum(x => x.QtyInM)).ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, (_oFEOS.RequiredWarpLength - _oSizingExecutions.Sum(x => x.QtyInMeterSt)).ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, (_oFEOS.RequiredWarpLength - _oFabricLoomPlans.Sum(x => x.QtyInM)).ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, (_oFEOS.RequiredWarpLength - _oFabricBatchLooms.Sum(x => x.QtyInMtr)).ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, (_oFabricBatchLooms.Sum(x => x.QtyInMtr) - _oFabricBatchQCs.Sum(x => x.LoomQtyInMtr)).ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, 12, _oFontStyle);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            #endregion


        }

        private void SetFabricBatchInfo(FabricBatch oFB)
        {
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            PdfPTable oPdfPTable = new PdfPTable(8);
            oPdfPTable.SetWidths(new float[] { 15f, 12f, 12f, 12f, 12f, 12f, 12f, 12f });

            #region Basic Info
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Program No: ", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, oFB.BatchNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Date: ", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, oFB.IssueDateSt, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Qty (Y): ", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, oFB.Qty.ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Qty (M): ", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, oFB.QtyInM.ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, oPdfPTable.NumberOfColumns, Element.ALIGN_LEFT, BaseColor.WHITE, false, 12, _oFontStyle);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            #endregion


        }

        private void SetWarpingPlan(FabricBatch oFB, List<FabricWarpPlan> oTempFabricWarpPlans)
        {
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            PdfPTable oPdfPTable = new PdfPTable(6);
            oPdfPTable.SetWidths(new float[] { 10f, 20f, 20f, 15f, 15f, 15f });

            #region Basic Info
            oPdfPTable = new PdfPTable(6);
            oPdfPTable.SetWidths(new float[] { 10f, 20f, 20f, 15f, 15f, 15f });

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Warping Plan", 0, oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "SL", 0, 0, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Date", 0, 0, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Beam No", 0, 0, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Qty (Y)", 0, 0, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Qty (M)", 0, 0, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Yet Qty (M)", 0, 0, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

            int nSL = 0;
            foreach (FabricWarpPlan oItem in oTempFabricWarpPlans)
            {
                oPdfPTable = new PdfPTable(6);
                oPdfPTable.SetWidths(new float[] { 10f, 20f, 20f, 15f, 15f, 15f });
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (++nSL).ToString(), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.StartDateSt, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.WarpBeam, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.Qty.ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.QtyInM.ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oFB.QtyInM - oItem.QtyInM).ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            }

            oPdfPTable = new PdfPTable(6);
            oPdfPTable.SetWidths(new float[] { 10f, 20f, 20f, 15f, 15f, 15f });
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total: ", 0, 3, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, oTempFabricWarpPlans.Sum(x => x.Qty).ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, oTempFabricWarpPlans.Sum(x => x.QtyInM).ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, oTempFabricWarpPlans.Sum(x => (oFB.QtyInM - x.QtyInM)).ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, 10, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            #endregion


        }

        private void SetWarpingExecution(FabricBatch oFB, List<FabricBatchProduction> oTempWarpingExecutions)
        {
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            PdfPTable oPdfPTable = new PdfPTable(6);
            oPdfPTable.SetWidths(new float[] { 10f, 20f, 20f, 15f, 15f, 15f });

            #region Basic Info
            oPdfPTable = new PdfPTable(6);
            oPdfPTable.SetWidths(new float[] { 10f, 20f, 20f, 15f, 15f, 15f });

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Warping Execution", 0, oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "SL", 0, 0, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Date", 0, 0, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Beam No", 0, 0, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Qty (Y)", 0, 0, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Qty (M)", 0, 0, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Yet Qty (M)", 0, 0, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

            int nSL = 0;
            foreach (FabricBatchProduction oItem in oTempWarpingExecutions)
            {
                oPdfPTable = new PdfPTable(6);
                oPdfPTable.SetWidths(new float[] { 10f, 20f, 20f, 15f, 15f, 15f });
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (++nSL).ToString(), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.StartDateSt, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.BeamNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.Qty.ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.QtyInMeterSt.ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oFB.QtyInM - oItem.QtyInMeterSt).ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            }

            oPdfPTable = new PdfPTable(6);
            oPdfPTable.SetWidths(new float[] { 10f, 20f, 20f, 15f, 15f, 15f });
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total: ", 0, 3, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, oTempWarpingExecutions.Sum(x => x.Qty).ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, oTempWarpingExecutions.Sum(x => x.QtyInMeterSt).ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, oTempWarpingExecutions.Sum(x => (oFB.QtyInM - x.QtyInMeterSt)).ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, 10, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            #endregion


        }

        private void SetSizingPlan(FabricBatch oFB, List<FabricSizingPlan> oTempFabricSizingPlans)
        {
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            PdfPTable oPdfPTable = new PdfPTable(6);
            oPdfPTable.SetWidths(new float[] { 10f, 20f, 20f, 15f, 15f, 15f });

            #region Basic Info
            oPdfPTable = new PdfPTable(6);
            oPdfPTable.SetWidths(new float[] { 10f, 20f, 20f, 15f, 15f, 15f });

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Sizing Plan", 0, oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "SL", 0, 0, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Date", 0, 0, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Beam No", 0, 0, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Qty (Y)", 0, 0, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Qty (M)", 0, 0, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Yet Qty (M)", 0, 0, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

            int nSL = 0;
            foreach (FabricSizingPlan oItem in oTempFabricSizingPlans)
            {
                oPdfPTable = new PdfPTable(6);
                oPdfPTable.SetWidths(new float[] { 10f, 20f, 20f, 15f, 15f, 15f });
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (++nSL).ToString(), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.StartDateSt, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.BeamName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.Qty.ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.QtyInM.ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oFB.QtyInM - oItem.QtyInM).ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            }

            oPdfPTable = new PdfPTable(6);
            oPdfPTable.SetWidths(new float[] { 10f, 20f, 20f, 15f, 15f, 15f });
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total: ", 0, 3, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, oTempFabricSizingPlans.Sum(x => x.Qty).ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, oTempFabricSizingPlans.Sum(x => x.QtyInM).ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, oTempFabricSizingPlans.Sum(x => (oFB.QtyInM - x.QtyInM)).ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, 10, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            #endregion


        }

        private void SetSizingExecution(FabricBatch oFB, List<FabricBatchProduction> oTempSizingExecutions)
        {
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            PdfPTable oPdfPTable = new PdfPTable(6);
            oPdfPTable.SetWidths(new float[] { 10f, 20f, 20f, 15f, 15f, 15f });

            #region Basic Info
            oPdfPTable = new PdfPTable(6);
            oPdfPTable.SetWidths(new float[] { 10f, 20f, 20f, 15f, 15f, 15f });

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Sizing Execution", 0, oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "SL", 0, 0, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Date", 0, 0, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Beam No", 0, 0, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Qty (Y)", 0, 0, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Qty (M)", 0, 0, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Yet Qty (M)", 0, 0, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

            int nSL = 0;
            foreach (FabricBatchProduction oItem in oTempSizingExecutions)
            {
                oPdfPTable = new PdfPTable(6);
                oPdfPTable.SetWidths(new float[] { 10f, 20f, 20f, 15f, 15f, 15f });
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (++nSL).ToString(), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.StartDateSt, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.BeamNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.Qty.ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.QtyInMeterSt.ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oFB.QtyInM - oItem.QtyInMeterSt).ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            }

            oPdfPTable = new PdfPTable(6);
            oPdfPTable.SetWidths(new float[] { 10f, 20f, 20f, 15f, 15f, 15f });
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total: ", 0, 3, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, oTempSizingExecutions.Sum(x => x.Qty).ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, oTempSizingExecutions.Sum(x => x.QtyInMeterSt).ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, oTempSizingExecutions.Sum(x => (oFB.QtyInM - x.QtyInMeterSt)).ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, 10, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            #endregion


        }

        private void SetDrawingExecution(FabricBatch oFB, List<FabricBatchProduction> oTempDrawingExecutions)
        {
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            PdfPTable oPdfPTable = new PdfPTable(6);
            oPdfPTable.SetWidths(new float[] { 10f, 20f, 20f, 15f, 15f, 15f });

            #region Basic Info
            oPdfPTable = new PdfPTable(6);
            oPdfPTable.SetWidths(new float[] { 10f, 20f, 20f, 15f, 15f, 15f });

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Drawing Execution", 0, oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "SL", 0, 0, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Date", 0, 0, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Beam No", 0, 0, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Qty (Y)", 0, 0, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Qty (M)", 0, 0, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Yet Qty (M)", 0, 0, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

            int nSL = 0;
            foreach (FabricBatchProduction oItem in oTempDrawingExecutions)
            {
                oPdfPTable = new PdfPTable(6);
                oPdfPTable.SetWidths(new float[] { 10f, 20f, 20f, 15f, 15f, 15f });
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (++nSL).ToString(), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.StartDateSt, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.BeamNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.Qty.ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.QtyInMeterSt.ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oFB.QtyInM - oItem.QtyInMeterSt).ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            }

            oPdfPTable = new PdfPTable(6);
            oPdfPTable.SetWidths(new float[] { 10f, 20f, 20f, 15f, 15f, 15f });
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total: ", 0, 3, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, oTempDrawingExecutions.Sum(x => x.Qty).ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, oTempDrawingExecutions.Sum(x => x.QtyInMeterSt).ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, oTempDrawingExecutions.Sum(x => (oFB.QtyInM - x.QtyInMeterSt)).ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, 10, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            #endregion


        }

        private void SetLeasingExecution(FabricBatch oFB, List<FabricBatchProduction> oTempLeasingExecutions)
        {
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            PdfPTable oPdfPTable = new PdfPTable(6);
            oPdfPTable.SetWidths(new float[] { 10f, 20f, 20f, 15f, 15f, 15f });

            #region Basic Info
            oPdfPTable = new PdfPTable(6);
            oPdfPTable.SetWidths(new float[] { 10f, 20f, 20f, 15f, 15f, 15f });

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Leasing Execution", 0, oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "SL", 0, 0, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Date", 0, 0, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Beam No", 0, 0, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Qty (Y)", 0, 0, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Qty (M)", 0, 0, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Yet Qty (M)", 0, 0, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

            int nSL = 0;
            foreach (FabricBatchProduction oItem in oTempLeasingExecutions)
            {
                oPdfPTable = new PdfPTable(6);
                oPdfPTable.SetWidths(new float[] { 10f, 20f, 20f, 15f, 15f, 15f });
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (++nSL).ToString(), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.StartDateSt, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.BeamNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.Qty.ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.QtyInMeterSt.ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oFB.QtyInM - oItem.QtyInMeterSt).ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            }

            oPdfPTable = new PdfPTable(6);
            oPdfPTable.SetWidths(new float[] { 10f, 20f, 20f, 15f, 15f, 15f });
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total: ", 0, 3, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, oTempLeasingExecutions.Sum(x => x.Qty).ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, oTempLeasingExecutions.Sum(x => x.QtyInMeterSt).ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, oTempLeasingExecutions.Sum(x => (oFB.QtyInM - x.QtyInMeterSt)).ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, 10, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            #endregion


        }

        private void SetLoomExecution(FabricBatch oFB, List<FabricBatchLoom> oTempFabricBatchLooms)
        {
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            PdfPTable oPdfPTable = new PdfPTable(6);
            oPdfPTable.SetWidths(new float[] { 10f, 20f, 20f, 15f, 15f, 15f });

            #region Basic Info
            oPdfPTable = new PdfPTable(6);
            oPdfPTable.SetWidths(new float[] { 10f, 20f, 20f, 15f, 15f, 15f });

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Loom Execution", 0, oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "SL", 0, 0, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Date", 0, 0, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Beam No", 0, 0, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Qty (Y)", 0, 0, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Qty (M)", 0, 0, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Yet Qty (M)", 0, 0, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

            int nSL = 0;
            foreach (FabricBatchLoom oItem in oTempFabricBatchLooms)
            {
                oPdfPTable = new PdfPTable(6);
                oPdfPTable.SetWidths(new float[] { 10f, 20f, 20f, 15f, 15f, 15f });
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (++nSL).ToString(), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.StartDateSt, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.BeamNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.Qty.ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.QtyInMtr.ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oFB.QtyInM - oItem.QtyInMtr).ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            }

            oPdfPTable = new PdfPTable(6);
            oPdfPTable.SetWidths(new float[] { 10f, 20f, 20f, 15f, 15f, 15f });
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total: ", 0, 3, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, oTempFabricBatchLooms.Sum(x => x.Qty).ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, oTempFabricBatchLooms.Sum(x => x.QtyInMtr).ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, oTempFabricBatchLooms.Sum(x => (oFB.QtyInM - x.QtyInMtr)).ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, 10, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            #endregion


        }

    }
}
