using System;
using System.Data;
using ESimSol.BusinessObjects;
using ICS.Core;
using ICS.Core.Utility;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;


namespace ESimSol.Reports
{
    public class rptRouteSheetLabel
    {
        #region Declaration
        Document _oDocument;
        int _nColumns = 3;
        iTextSharp.text.Font _oFontStyle;
        Phrase _oPhrase = new Phrase();
        public iTextSharp.text.Image _oImag { get; set; }
        PdfPTable _oPdfPTable = new PdfPTable(3);
        PdfPCell _oPdfPCell;
        MemoryStream _oMemoryStream = new MemoryStream();
        RouteSheet _oRS = new RouteSheet();
        List<RouteSheetPacking> _oRouteSheetPackings = new  List<RouteSheetPacking>();
        RouteSheetSetup _oRouteSheetSetup = new RouteSheetSetup();
        BusinessUnit _oBusinessUnit = new BusinessUnit();
        int _nCountLabel = 0;
        double _nQty = 0;
        #endregion


        #region Constructor
        public rptRouteSheetLabel() { }
        #endregion
        #region Print Label
        public byte[] PrepareReport(RouteSheet oRS, List<RouteSheetPacking> oRouteSheetPackings, BusinessUnit oBusinessUnit, RouteSheetSetup oRouteSheetSetup)
        {

            _oRS = oRS;
            _oRouteSheetPackings=oRouteSheetPackings;
            _nCountLabel = _oRouteSheetPackings.Count;
            _oBusinessUnit = oBusinessUnit;
            _oRouteSheetSetup =oRouteSheetSetup;
            #region Page Setup
            _oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
            _oDocument.SetMargins(20f, 20f, 20f, 20f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);

            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 270f, 30f, 270f });
            #endregion

            //this.PrintHeader();
            this.PrintBody();
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Body
        private void PrintBody()
        {
            int nCount = 0;
            //PdfPTable oPdfPTable = new PdfPTable(3);
            //PdfPCell oPdfPCell;
            //oPdfPTable.SetWidths(new float[] { 200f,30f, 200f });

            foreach(RouteSheetPacking oItem in _oRouteSheetPackings)
            {
                nCount++;
                if (nCount % 2 != 0)
                {
                    _oPdfPCell = new PdfPCell(this.PrintLabel(oItem));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", FontFactory.GetFont("Tahoma", 13f, iTextSharp.text.Font.BOLD)));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPTable.AddCell(_oPdfPCell);
                }
                else
                {
                    _oPdfPCell = new PdfPCell(this.PrintLabel(oItem));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                }
            }
            if (_nCountLabel % 2 != 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase("", FontFactory.GetFont("Tahoma", 13f, iTextSharp.text.Font.BOLD)));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPTable.AddCell(_oPdfPCell);

            }
            
            _oPdfPTable.CompleteRow();

        }
        #endregion
        private PdfPTable PrintLabel(RouteSheetPacking oRouteSheetPacking)
        {
            //_oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, 1);

            PdfPTable oPdfPTable = new PdfPTable(2);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 100, 240f });

            oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Name,  FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD)));
            oPdfPCell.Border = 0;
            oPdfPCell.Colspan = 2;
            oPdfPCell.Border = 0;
            oPdfPCell.BorderWidthTop = 0.5f;
            oPdfPCell.BorderWidthLeft = 0.5f;
            oPdfPCell.BorderWidthRight = 0.5f;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.PringReportHead, FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD)));
            oPdfPCell.Border = 0;
            oPdfPCell.Colspan = 2;
            oPdfPCell.BorderWidthTop = 0;
            oPdfPCell.BorderWidthLeft = 0.5f;
            oPdfPCell.BorderWidthRight = 0.5f;
            oPdfPCell.BorderWidthBottom= 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("CUSTOMER", FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL)));
           // oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(_oRS.PTU.ContractorName, FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD)));
            // oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("DATE", FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL)));
            // oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(oRouteSheetPacking.Date.ToString("dd MMM yyyy"), FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD)));
            // oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("Order No", FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL)));
            // oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(_oRS.PTU.OrderNo, FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD)));
            // oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("SHEFT", FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL)));
            // oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("", FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD)));
            // oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("LOT No", FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL)));
            // oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(_oRS.RouteSheetNo, FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD)));
            // oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("BALE No", FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL)));
            // oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(oRouteSheetPacking.BagNo.ToString(), FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD)));
            // oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("COLOR NAME", FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL)));
            // oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(_oRS.PTU.ColorName, FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD)));
            // oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("YARN", FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL)));
            // oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(_oRS.ProductName, FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD)));
            // oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("NET WEIGHT", FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL)));
            // oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oRouteSheetPacking.Weight), FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD)));
            // oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("CUTION", FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD)));
            oPdfPCell.Border = 0;
            oPdfPCell.Colspan = 2;
            oPdfPCell.BorderWidthLeft = 0.5f;
            oPdfPCell.BorderWidthRight = 0.5f;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("A) KEEP IN DRY & SHADED PLACE, SUNLIGHT MAY DEEPACE THE SHINING.", FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.BOLD)));
            oPdfPCell.Border = 0;
            oPdfPCell.Colspan = 2;
            oPdfPCell.BorderWidthLeft = 0.5f;
            oPdfPCell.BorderWidthRight = 0.5f;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("B) SIMILAR BATCH & COLOR BE USED IN ONE OPERATION.", FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.BOLD)));
            oPdfPCell.Border = 0;
            oPdfPCell.BorderWidthTop = 0;
            oPdfPCell.BorderWidthLeft = 0.5f;
            oPdfPCell.BorderWidthRight = 0.5f;
            oPdfPCell.BorderWidthBottom = 0.5f;
            oPdfPCell.Colspan = 2;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("", FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD)));
            oPdfPCell.Border = 0;
            oPdfPCell.Colspan = 2;
            oPdfPCell.FixedHeight = 20f;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            return oPdfPTable;
        }
        #endregion

        #region Print CARD
        public byte[] PrepareReportCARD(RouteSheet oRS, List<RouteSheetPacking> oRouteSheetPackings, BusinessUnit oBusinessUnit, RouteSheetSetup oRouteSheetSetup)
        {

            _oRS = oRS;
            _oRouteSheetPackings = oRouteSheetPackings;
            _nCountLabel = _oRouteSheetPackings.Count;
            _oBusinessUnit = oBusinessUnit;
            _oRouteSheetSetup = oRouteSheetSetup;

            #region Page Setup
            _oDocument = new Document(new iTextSharp.text.Rectangle(180f, 300f), 0f, 0f, 0f, 0f);
            //_oDocument.SetPageSize(iTextSharp.text.PageSize.B6);//842*595
            _oDocument.SetMargins(10f, 10f, 10f, 10f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);

            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 100f, 88f, 100f });
            #endregion

            //this.PrintHeader();
            this.PrintBody_Card();
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        #region Report Body
        private void PrintBody_Card()
        {
            foreach (RouteSheetPacking oItem in _oRouteSheetPackings)
            {
                _nQty = _nQty + oItem.Weight;
            }

            _oPdfPCell = new PdfPCell(new Phrase("PACKING CARD", FontFactory.GetFont("Tahoma", 15f, iTextSharp.text.Font.NORMAL)));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 3;
            //_oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Date:"+DateTime.Today.ToString("dd MMM yyyy"), FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL)));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 3;
            //_oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            _oPhrase = new Phrase();
            _oPhrase.Add(new Chunk("Yarn Description: ", FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL)));
            _oPhrase.Add(new Chunk(_oRS.ProductName, FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD)));

            _oPdfPCell = new PdfPCell(_oPhrase);
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 3;
            //_oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPhrase = new Phrase();
            _oPhrase.Add(new Chunk("Customer: ", FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL)));
            _oPhrase.Add(new Chunk(_oRS.PTU.ContractorName, FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD)));

            _oPdfPCell = new PdfPCell(_oPhrase);
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 3;
            //_oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPhrase = new Phrase();
            _oPhrase.Add(new Chunk("Order No: ", FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL)));
            _oPhrase.Add(new Chunk(_oRS.PTU.OrderNo, FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD)));

            _oPdfPCell = new PdfPCell(_oPhrase);
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 3;
            //_oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPhrase = new Phrase();
            _oPhrase.Add(new Chunk("Lot No: ", FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL)));
            _oPhrase.Add(new Chunk(_oRS.RouteSheetNo, FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD)));

            _oPdfPCell = new PdfPCell(_oPhrase);
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 3;
            //_oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPhrase = new Phrase();
            _oPhrase.Add(new Chunk("Color: ", FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL)));
            _oPhrase.Add(new Chunk(_oRS.PTU.ColorName, FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD)));

            _oPdfPCell = new PdfPCell(_oPhrase);
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 3;
            //_oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            _oPhrase = new Phrase();
            _oPhrase.Add(new Chunk("Dyeing QTY: ", FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL)));
            _oPhrase.Add(new Chunk(Global.MillionFormat(_nQty) + " " + _oRouteSheetSetup.MUnit, FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD)));

            _oPdfPCell = new PdfPCell(_oPhrase);
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 3;
            //_oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPhrase = new Phrase();
            _oPhrase.Add(new Chunk("Packing QTY: ", FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL)));
            _oPhrase.Add(new Chunk(Global.MillionFormat(_nQty) + " "+ _oRouteSheetSetup.MUnit, FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD)));

            _oPdfPCell = new PdfPCell(_oPhrase);
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 3;
            //_oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase("", FontFactory.GetFont("Tahoma", 15f, iTextSharp.text.Font.NORMAL)));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 3;
            _oPdfPCell.FixedHeight = 20f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Sample", FontFactory.GetFont("Tahoma", 15f, iTextSharp.text.Font.NORMAL)));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 3;
            _oPdfPCell.FixedHeight = 20f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase("", FontFactory.GetFont("Tahoma", 15f, iTextSharp.text.Font.NORMAL)));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 3;
            _oPdfPCell.FixedHeight =50f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("______  __________ _________", FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL)));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 3;
            _oPdfPCell.FixedHeight = 20f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Q.C Sig.   Floor Incharge Sign.   Dyeing Master/P.M", FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL)));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 3;
            _oPdfPCell.FixedHeight = 20f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

        }
        #endregion
        #endregion
    }
}
