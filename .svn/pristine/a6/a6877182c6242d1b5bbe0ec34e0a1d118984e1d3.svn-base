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
    public class rptDispoWiseStatementPrint
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
        //FabricExecutionOrderSpecification _oFabricExecutionOrderSpecification = new FabricExecutionOrderSpecification();
        List<FabricExecutionOrderYarnReceive> _oFabricEOYarnReceives = new List<FabricExecutionOrderYarnReceive>();
        List<FabricWarpPlan> _oFabricWarpPlans = new List<FabricWarpPlan>();
        List<FabricSizingPlan> _oFabricSizingPlans = new List<FabricSizingPlan>();
        List<FabricBatch> _oFabricBatchs = new List<FabricBatch>();
        List<FabricBatchRawMaterial> _oFabricBatchRawMaterials = new List<FabricBatchRawMaterial>();
        RPT_Dispo _oRPT_Dispo = new RPT_Dispo();
        List<FabricTransferPackingList> _oFabricTransferPackingLists = new List<FabricTransferPackingList>();
        Company _oCompany = new Company();

        #endregion

        public byte[] PrepareReport(RPT_Dispo oRPT_Dispo, List<FabricExecutionOrderYarnReceive> oFabricEOYarnReceives, List<FabricWarpPlan> oFabricWarpPlans, List<FabricSizingPlan> oFabricSizingPlans, List<FabricBatchRawMaterial> oFabricBatchRawMaterials, List<FabricBatch> oFabricBatchs, List<FabricTransferPackingList> oFabricTransferPackingLists, Company oCompany)
        {
            //_oFabricExecutionOrderSpecification = oFabricExecutionOrderSpecification;
            _oRPT_Dispo = oRPT_Dispo;
            _oFabricEOYarnReceives = oFabricEOYarnReceives;
            _oFabricWarpPlans = oFabricWarpPlans;
            _oFabricSizingPlans = oFabricSizingPlans;
            _oFabricBatchs = oFabricBatchs;
            _oFabricBatchRawMaterials = oFabricBatchRawMaterials;
            _oFabricTransferPackingLists = oFabricTransferPackingLists;
            _oCompany = oCompany;

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
            _oPdfPTable.HeaderRows = 4;
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
            this.PrintHeaderDetail("Dispo Wise Statement", " ", " ");
            this.SetDispo();
            this.SetData();
            this.SetConsumptionData();
            this.SetFabricTransferPackingList();
        }
        #endregion


        private void SetDispo()
        {
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            PdfPTable oPdfPTable = new PdfPTable(4);
            oPdfPTable.SetWidths(new float[] { 15f, 35f, 15f, 35f });

            #region Heder Info
            oPdfPTable = new PdfPTable(4);
            oPdfPTable.SetWidths(new float[] { 15f, 35f, 15f, 35f });

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Dispo No", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oRPT_Dispo.ExeNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Buyer", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oRPT_Dispo.BuyerName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Construction", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oRPT_Dispo.Construction, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "No Of Color", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Warp(" + _oRPT_Dispo.ColorWarp + ") Weft(" + _oRPT_Dispo.ColorWeft + ")", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Req. Length", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oRPT_Dispo.WarpLength.ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "T/F Length", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oRPT_Dispo.WarpLengthRecd.ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            #endregion

            #region Blank
            oPdfPTable = new PdfPTable(4);
            oPdfPTable.SetWidths(new float[] { 15f, 35f, 15f, 35f });

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, oPdfPTable.NumberOfColumns, Element.ALIGN_RIGHT, BaseColor.WHITE, false, 20f, _oFontStyle);

            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            #endregion

        }
        private void SetData()
        {
            int nSLNo = 0;
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            PdfPTable oPdfPTable = new PdfPTable(9);
            oPdfPTable.SetWidths(new float[] { 20f, 60f, 90f, 50f, 50f, 70f, 70f, 70f, 70f });

            if (_oFabricWarpPlans.Count > 0)
            {
                #region warping

                #region Heder Info
                oPdfPTable = new PdfPTable(9);
                oPdfPTable.SetWidths(new float[] { 20f, 60f, 90f, 50f, 50f, 70f, 70f, 70f, 70f });

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Warping Plan", 0, oPdfPTable.NumberOfColumns, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
                oPdfPTable.CompleteRow();
                _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "SL#", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Date", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Sizing M/C", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Warp Color", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Weft Color", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Reed No", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "R. Width", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Warp Length", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Sizing Beam", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                oPdfPTable.CompleteRow();

                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
                #endregion

                #region data
                foreach (var oItem in _oFabricWarpPlans)
                {
                    nSLNo++;
                    oPdfPTable = new PdfPTable(9);
                    oPdfPTable.SetWidths(new float[] { 20f, 60f, 90f, 50f, 50f, 70f, 70f, 70f, 70f });

                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, nSLNo.ToString("00"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.StartTimeSt, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.MachineName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.WarpColor.ToString(), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.WeftColor.ToString(), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.ReedNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);


                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.REEDWidth), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.WarpLength), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.WarpBeam, 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);

                    oPdfPTable.CompleteRow();
                    ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
                }
                #endregion

                #region Total Info
                #endregion

                #region Blank
                oPdfPTable = new PdfPTable(9);
                oPdfPTable.SetWidths(new float[] { 20f, 60f, 90f, 50f, 50f, 70f, 70f, 70f, 70f });

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, oPdfPTable.NumberOfColumns, Element.ALIGN_RIGHT, BaseColor.WHITE, false, 20f, _oFontStyle);

                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
                #endregion

                #endregion
            }

            if (_oFabricSizingPlans.Count > 0)
            {
                #region sizing

                #region Heder Info
                oPdfPTable = new PdfPTable(9);
                oPdfPTable.SetWidths(new float[] { 20f, 60f, 90f, 50f, 50f, 70f, 70f, 70f, 70f });
                _oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Sizing Plan", 0, oPdfPTable.NumberOfColumns, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
                oPdfPTable.CompleteRow();
                _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "SL#", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Date", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Sizing M/C", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Warp Color", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Weft Color", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Reed No", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "R. Width", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Warp Length", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Sizing Beam", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                oPdfPTable.CompleteRow();

                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
                #endregion

                #region data
                foreach (var oItem in _oFabricSizingPlans)
                {
                    nSLNo++;
                    oPdfPTable = new PdfPTable(9);
                    oPdfPTable.SetWidths(new float[] { 20f, 60f, 90f, 50f, 50f, 70f, 70f, 70f, 70f });

                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, nSLNo.ToString("00"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.StartTimeSt, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.MachineName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.WarpColor.ToString(), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.WeftColor.ToString(), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.ReedNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.REEDWidth), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.WarpLength), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.WarpBeam.ToString(), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);

                    oPdfPTable.CompleteRow();
                    ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
                }
                #endregion

                #region Total Info
                #endregion

                #region Blank
                oPdfPTable = new PdfPTable(9);
                oPdfPTable.SetWidths(new float[] { 20f, 60f, 90f, 50f, 50f, 70f, 70f, 70f, 70f });

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, oPdfPTable.NumberOfColumns, Element.ALIGN_RIGHT, BaseColor.WHITE, false, 20f, _oFontStyle);

                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
                #endregion

                #endregion
            }
            

        }

        private void SetConsumptionData()
        {
            if (_oFabricBatchRawMaterials.Count > 0)
            {
                int nSLNo = 0;
                _oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

                PdfPTable oPdfPTable = new PdfPTable(7);
                oPdfPTable.SetWidths(new float[] { 20f, 60f, 60f, 120f, 120f, 60f, 70f });

                #region Consumption

                #region Heder Info
                oPdfPTable = new PdfPTable(7);
                oPdfPTable.SetWidths(new float[] { 20f, 60f, 60f, 120f, 120f, 60f, 70f });

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Consumption Information", 0, oPdfPTable.NumberOfColumns, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
                oPdfPTable.CompleteRow();
                _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "SL#", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Batch No", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Date", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Product", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Store", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Unit", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Qty", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                oPdfPTable.CompleteRow();

                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
                #endregion

                var data = _oFabricBatchRawMaterials.GroupBy(x => new { x.FBID }, (key, grp) => new
                {
                    nFBID = key.FBID,
                    Results = grp.ToList()
                });

                int nFBID = -999;
                foreach (var oData in data)
                {
                    oPdfPTable = new PdfPTable(7);
                    oPdfPTable.SetWidths(new float[] { 20f, 60f, 60f, 120f, 120f, 60f, 70f });
                    foreach (var oItem in oData.Results)
                    {
                        if (nFBID != oData.nFBID)
                        {
                            ESimSolItexSharp.SetCellValue(ref oPdfPTable, (++nSLNo).ToString("00"), oData.Results.Count, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricBatchs.Where(x => x.FBID == oData.nFBID).Select(y => y.BatchNo).FirstOrDefault(), oData.Results.Count, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                        }
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.ReceiveByDateInString, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.ProductName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.StoreName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.UnitName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.Qty.ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);

                        nFBID = oData.nFBID;
                        oPdfPTable.CompleteRow();
                    }
                    ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

                }

                #region Total Info
                oPdfPTable = new PdfPTable(7);
                oPdfPTable.SetWidths(new float[] { 20f, 60f, 60f, 120f, 120f, 60f, 70f });

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total: ", 0, 6, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricBatchRawMaterials.Sum(x => x.Qty).ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);

                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
                #endregion

                #region Blank
                oPdfPTable = new PdfPTable(7);
                oPdfPTable.SetWidths(new float[] { 20f, 60f, 60f, 120f, 120f, 60f, 70f });

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, oPdfPTable.NumberOfColumns, Element.ALIGN_RIGHT, BaseColor.WHITE, false, 20f, _oFontStyle);

                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
                #endregion

                #endregion
            }
            

        }

        private void SetFabricTransferPackingList()
        {
            if (_oFabricTransferPackingLists.Count > 0)
            {
                int nSLNo = 0;
                _oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

                PdfPTable oPdfPTable = new PdfPTable(4);
                oPdfPTable.SetWidths(new float[] { 20f, 70f, 150f, 70f });

                #region Fabric Transfer  Packing List

                #region Heder Info
                oPdfPTable = new PdfPTable(4);
                oPdfPTable.SetWidths(new float[] { 20f, 70f, 150f, 70f });

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Fabric Transfer Packing List", 0, oPdfPTable.NumberOfColumns, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
                oPdfPTable.CompleteRow();
                _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "SL#", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Note Date", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Buyer", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Qty", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                
                oPdfPTable.CompleteRow();

                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
                #endregion

                #region Data
                //var data = _oFabricTransferPackingLists.GroupBy(x => new { x.FBID }, (key, grp) => new
                //{
                //    nFBID = key.FBID,
                //    Results = grp.ToList()
                //});

                //int nFBID = -999;
                //foreach (var oData in data)
                //{
                //    oPdfPTable = new PdfPTable(7);
                //    oPdfPTable.SetWidths(new float[] { 20f, 60f, 60f, 120f, 120f, 60f, 70f });
                //    foreach (var oItem in oData.Results)
                //    {
                //        if (nFBID != oData.nFBID)
                //        {
                //            ESimSolItexSharp.SetCellValue(ref oPdfPTable, (++nSLNo).ToString("00"), oData.Results.Count, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                //            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricBatchs.Where(x => x.FBID == oData.nFBID).Select(y => y.BatchNo).FirstOrDefault(), oData.Results.Count, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                //        }
                //        ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.ReceiveByDateInString, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                //        ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.ProductName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                //        ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.StoreName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                //        ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.UnitName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                //        ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.Qty.ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);

                //        nFBID = oData.nFBID;
                //        oPdfPTable.CompleteRow();
                //    }
                //    ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

                //}
                #endregion

                #region Data
                foreach (var oItem in _oFabricTransferPackingLists)
                {
                    oPdfPTable = new PdfPTable(4);
                    oPdfPTable.SetWidths(new float[] { 20f, 70f, 150f, 70f });

                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, (++nSLNo).ToString("00"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);                    
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.NoteDateSt, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.BuyerName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.DetailQty.ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);

                    oPdfPTable.CompleteRow();
                    ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
                }

                #endregion

                #region Total Info
                oPdfPTable = new PdfPTable(4);
                oPdfPTable.SetWidths(new float[] { 20f, 70f, 150f, 70f });

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total: ", 0, 3, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricTransferPackingLists.Sum(x => x.DetailQty).ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);

                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
                #endregion

                #region Blank
                oPdfPTable = new PdfPTable(4);
                oPdfPTable.SetWidths(new float[] { 20f, 70f, 150f, 70f });

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, oPdfPTable.NumberOfColumns, Element.ALIGN_RIGHT, BaseColor.WHITE, false, 10f, _oFontStyle);

                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
                #endregion

                #endregion
            }


        }

    }
}
