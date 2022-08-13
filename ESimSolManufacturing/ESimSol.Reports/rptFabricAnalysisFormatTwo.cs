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
    public class rptFabricAnalysisFormatTwo
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
        Fabric _oFabric = new Fabric();
        FabricSCReport _oFabricSCReport = new FabricSCReport();
        FabricRnD _oFabricRnD = new FabricRnD();
        FabricPOSetup _oFabricPOSetup = new FabricPOSetup();
        Company _oCompany = new Company();
        #endregion

        public byte[] PrepareReport(Fabric oFabric, FabricSCReport oFabricSCReport, FabricRnD oFabricRnD, FabricPOSetup oFabricPOSetup, Company oCompany)
        {
            _oFabric = oFabric;
            _oFabricSCReport = oFabricSCReport;
            _oFabricRnD = oFabricRnD;
            _oFabricPOSetup = oFabricPOSetup;
            _oCompany = oCompany;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            _oDocument.SetMargins(25f, 25f, 5f, 25f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyleBold = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
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
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
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
            this.PrintHeaderDetail("Fabric Analysis Report", " ", " ");
            this.SetData();
        }
        #endregion

        private void SetData()
        {
            TopPart();
            FirstPart();
            SecondPart();
            ThirdPart();
            FourthPart();
        }

        private void FourthPart()
        {
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            PdfPTable oPdfPTable = new PdfPTable(2);
            oPdfPTable.SetTotalWidth(new float[] { 215f, 280f });
            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER, 0, 2, 10);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Suggested Finish Construction : ", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, " ", Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable, 0, 0, _oPdfPTable.NumberOfColumns);

            oPdfPTable = new PdfPTable(4);
            oPdfPTable.SetTotalWidth(new float[] { 20f, 30f, 20f, 30f });

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Construction:", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricRnD.ConstructionSuggest, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Fabric Composition:", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricRnD.ProductWarpRnd_Suggest, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Weave Type:", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabric.FabricWeaveName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Declared Weight:", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(_oFabricRnD.WeightDec), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            oPdfPTable.CompleteRow();
            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable, 0, 0, _oPdfPTable.NumberOfColumns);

            oPdfPTable = new PdfPTable(4);
            oPdfPTable.SetTotalWidth(new float[] { 35f, 15f, 35f, 15f });

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Considered Fabric Grey Width:", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricRnD.WidthGrey, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Considered Fabric Finish Width:", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricRnD.FabricWidth, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            oPdfPTable.CompleteRow();


            Phrase oPhrase = new Phrase();
            Chunk oChunk2 = new Chunk("Note: ", _oFontStyleBold); Chunk oChunk3 = new Chunk(_oFabricRnD.Note, _oFontStyle);
            oPhrase.Add(oChunk2); oPhrase.Add(oChunk3);
            _oPdfPCell = new PdfPCell(oPhrase); _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 15; _oPdfPCell.Colspan = oPdfPTable.NumberOfColumns; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Note: " + _oFabricRnD.Note, 0, 4, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            //oPdfPTable.CompleteRow();

            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable, 0, 0, _oPdfPTable.NumberOfColumns);


            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0, 0, 2, 15);
            #region Signature

            List<SignatureSetup> _oSignatureSetups = new List<SignatureSetup>();
            _oSignatureSetups.Add(new SignatureSetup() { DisplayDataColumn = "", SignatureCaption = "" + "\nPrepared By \n" }); //_oFabric.PrepareByName+ "\n
            _oSignatureSetups.Add(new SignatureSetup() { DisplayDataColumn = "", SignatureCaption = "" + "\nChecked By\n" }); //_oFabric.MKTPersonName + " \n
            _oSignatureSetups.Add(new SignatureSetup() { DisplayDataColumn = "", SignatureCaption = "" + "\nApproved By (Head of Q.A)\n" }); //_oFabric.ApprovedByNameSt + "\n

            _oPdfPCell = new PdfPCell(new Phrase(""));
            _oPdfPCell.Colspan = 9;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 600 - ESimSolPdfHelper.CalculatePdfPTableHeight(_oPdfPTable);
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #region Authorized Signature
            _oPdfPCell = new PdfPCell(ESimSolSignature.GetSignature(60f, _oFabric, _oSignatureSetups, 25.0f));
            _oPdfPCell.Colspan = 9;
            _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_BOTTOM;
            _oPdfPCell.FixedHeight = 80f;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            #endregion
        }

        private void ThirdPart()
        {
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            PdfPTable oPdfPTable = new PdfPTable(2);
            oPdfPTable.SetTotalWidth(new float[] { 215f, 280f });
            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER, 0, 2, 10);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Analysis Construction : ", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, " ", Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable, 0, 0, _oPdfPTable.NumberOfColumns);

            oPdfPTable = new PdfPTable(4);
            oPdfPTable.SetTotalWidth(new float[] { 25f, 25f, 25f, 25f });

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "EPI:", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricRnD.EPI, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Calculated Weight:", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(_oFabricRnD.WeightCal), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "PPI:", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricRnD.PPI, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Weave Type:", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricRnD.FabricWeaveName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Fabric Width:", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricRnD.FabricWidth, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Finish Type:", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricRnD.FinishTypeName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Crimp Warp:", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricRnD.CrimpWP, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Crimp Weft:", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricRnD.CrimpWF, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "For Lycra Fabric:", 2, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Growth", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Recovery", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Elongation", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricRnD.Growth, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricRnD.Recovy, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricRnD.Elongation, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "For Slub Dimension:", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Slub Length(CM)", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Push Length(CM)", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Slub Dia", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "WP:", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricRnD.SlubLengthWP, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricRnD.PauseLengthWP, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricRnD.SlubDiaWP, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "WF:", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricRnD.SlubLengthWF, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricRnD.PauseLengthWF, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricRnD.SlubDiaWF, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "For Polyester (No of Filament):", 0, 2, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            oPdfPTable.CompleteRow();
            
            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable, 0, 0, _oPdfPTable.NumberOfColumns);


        }

        private void SecondPart()
        {
            int LeftColumn = 90;
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            PdfPTable oPdfPTable = new PdfPTable(2);
            oPdfPTable.SetTotalWidth(new float[] { 215f, 280f });
            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER, 0, 2, 10);
            if (!string.IsNullOrEmpty(_oFabricSCReport.ExeNo)) { _oFabric.Code = _oFabricSCReport.ExeNo; }
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Fabric Analysis Report : " + _oFabric.Code, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0);

            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Lab Received Date:" + _oFabricRnD.ReceiveDateSt, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable, 0, 0, _oPdfPTable.NumberOfColumns);

            #region YARN DETAILS
            oPdfPTable = new PdfPTable(4);
            oPdfPTable.SetTotalWidth(new float[] { 150, 80, 350 - LeftColumn, 200 });

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Size Of Swatch (CM)", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Shade", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Wash Position", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricRnD.WashTypeInString, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Dye Type", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricRnD.ShadeTypeInString, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
            oPdfPTable.CompleteRow();
            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable, 0, 0, _oPdfPTable.NumberOfColumns);

            oPdfPTable = new PdfPTable(4);
            oPdfPTable.SetTotalWidth(new float[] { 150, 80, 350 - LeftColumn, 200 });
            #region Table Header
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLDITALIC);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "YARN DETAILS", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX);
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            Print_TableHeader(new string[] { "Count", "Yarn composition", "" }, ref oPdfPTable);

            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
            Print_YarnDetailRnD(ref oPdfPTable, 1, "Warp yarn count", true);
            //ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0, 0, 7, 8);
            Print_YarnDetailRnD(ref oPdfPTable, 1, "Weft yarn count", false);
            #endregion

            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable, 0, 0, _oPdfPTable.NumberOfColumns);
            #endregion
        }

        public void Print_TableHeader(string[] Headers, ref PdfPTable oPdfPTable)
        {
            foreach (string Header in Headers)
                ESimSolPdfHelper.AddCell(ref oPdfPTable, Header, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX);
        }
        public void Print_YarnDetailRnD(ref PdfPTable oPdfPTable, int Count, string sData, bool isWarp)
        {
            int nCount = 0;
            for (int i = 0; i < Count; i++)
            {
                ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, sData + "(" + (++nCount) + ")", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX);
                if (isWarp)
                    Print_TableHeader(new string[] { _oFabricRnD.WarpCount, (string.IsNullOrEmpty(_oFabricRnD.ProductNameWarp)) ? "" : _oFabricRnD.ProductNameWarp, "Yarn Ply: " + _oFabricRnD.YarnFly }, ref oPdfPTable);
                else
                    Print_TableHeader(new string[] { _oFabricRnD.WeftCount, (string.IsNullOrEmpty(_oFabricRnD.ProductNameWeft)) ? "" : _oFabricRnD.ProductNameWeft, "Yarn Quality: " + _oFabricRnD.YarnQuality }, ref oPdfPTable);
            }
        } 
        private void FirstPart()
        {
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
            #region Head
            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.SetTotalWidth(new float[] { 100, 150, 200 });

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "BUYER: ", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricSCReport.BuyerName, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Swatch: ", 5, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Tracking No: ", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricSCReport.FabricNo, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Style/Season: ", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricSCReport.StyleNo, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Department: ", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "-", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Seeking Date: ", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabric.SeekingSubmissionDateInString, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            //ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable, 0, 0, 2);
            #endregion

            #region Signature
            List<SignatureSetup> _oSignatureSetups = new List<SignatureSetup>();
            _oSignatureSetups.Add(new SignatureSetup() { DisplayDataColumn = "", SignatureCaption = _oFabricSCReport.PreapeByName + "\nPrepared" });
            _oSignatureSetups.Add(new SignatureSetup() { DisplayDataColumn = "", SignatureCaption = _oFabricSCReport.MKTGroup + " \nTeam Leader \n " });
            _oSignatureSetups.Add(new SignatureSetup() { DisplayDataColumn = "", SignatureCaption = _oFabricSCReport.ApproveByName + "\nApproved By\n " });

            #region Authorized Signature
            _oPdfPCell = new PdfPCell(ESimSolSignature.GetSignature(50f, _oFabric, _oSignatureSetups, 15.0f));
            _oPdfPCell.Colspan = _oPdfPTable.NumberOfColumns;
            _oPdfPCell.BorderWidthBottom = 1; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 1; _oPdfPCell.BorderWidthRight = 1;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.FixedHeight = 60f;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #endregion
        }


        private void TopPart()
        {
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            #region Date
            PdfPTable oPdfPTable = new PdfPTable(2);
            oPdfPTable.SetTotalWidth(new float[] { 415f, 80f });
            //ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.TOP_BORDER, 0, 2, 0);

            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Request From Marketing:  ", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Date:  " + _oFabricSCReport.SCDateSt, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
            //ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable, 0, 0, 2);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

            #endregion
        }

    }
}
