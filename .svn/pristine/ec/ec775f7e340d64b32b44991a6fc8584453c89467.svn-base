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
    public class rptGRNWithRate
    {
        #region Declaration
        private int _nColumn = 7;
        private Document _oDocument;
        private iTextSharp.text.Font _oFontStyle;
        private PdfPTable _oPdfPTable = new PdfPTable(7);
        private PdfPCell _oPdfPCell;
        PdfWriter PDFWriter;
        private iTextSharp.text.Image _oImag;
        private MemoryStream _oMemoryStream = new MemoryStream();
        private GRN _oGRN = new GRN();
        private List<GRNDetail> _oGRNDetails = new List<GRNDetail>();
        List<SignatureSetup> _oSignatureSetups = new List<SignatureSetup>();
        List<ApprovalHead> _oApprovalHeads = new List<ApprovalHead>();
        private Company _oCompany = new Company();
        Phrase _oPhrase = new Phrase();
        Chunk _oChunk = new Chunk();       
        #endregion

        public byte[] PrepareReport(GRN oGRN, Company oCompany, bool bIsA4, List<SignatureSetup> oSignatureSetup)
        {
            _oGRN = oGRN;
            _oGRNDetails = oGRN.GRNDetails;
            _oCompany = oCompany;
            _oSignatureSetups = oSignatureSetup;
            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            if (bIsA4)
            {
                
                _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            }
            else
            {
                _oDocument = new Document(new iTextSharp.text.Rectangle(595f, 421f), 0f, 0f, 0f, 0f); //A4:842*595            
            }
            _oDocument.SetMargins(10f, 10f, 10f, 10f);
            _oPdfPTable.WidthPercentage = 95;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_CENTER;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 30f,//SL
                                                150f, //Product
                                                100f, //Specification
                                                50f, //unit
                                                50f, //  qty
                                                60f,//rate
                                                70f //Amount
                                                  });//535
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
            if (_oCompany.CompanyLogo != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(160f, 35f);
                _oPdfPCell = new PdfPCell(_oImag);
                _oPdfPCell.Colspan = _nColumn;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 35;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            else
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 11f, 1);
                _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));
                _oPdfPCell.Colspan = _nColumn;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.ExtraParagraphSpace = 0;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Address + "\n" + _oCompany.Phone + ";  " + _oCompany.Email + ";  " + _oCompany.WebAddress, _oFontStyle));
            _oPdfPCell.Colspan = _nColumn;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region ReportHeader
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
            _oPdfPCell = new PdfPCell(new Phrase("MRR(Bill)", _oFontStyle));
            _oPdfPCell.Colspan = _nColumn;
            _oPdfPCell.FixedHeight = 25f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 5f;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        #endregion


        #region Report Body
        private void PrintBody()
        {
            Phrase oPhrase = new Phrase();

            #region GRN
            PdfPTable oTempPdfPTable = new PdfPTable(4);
            oTempPdfPTable.SetWidths(new float[] { 70f, 280f, 60f, 130f });//535

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("MRR No", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oTempPdfPTable.AddCell(_oPdfPCell);

            oPhrase = new Phrase();
            oPhrase.Add(new Chunk(": ", FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL)));
            oPhrase.Add(new Chunk(_oGRN.GRNNo+"                                                         ", FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD)));
            oPhrase.Add(new Chunk("MRF No  :", FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL)));
            oPhrase.Add(new Chunk(_oGRN.MRFNo, FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD)));

            _oPdfPCell = new PdfPCell(oPhrase);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oTempPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("Date", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oTempPdfPTable.AddCell(_oPdfPCell);

            oPhrase = new Phrase();
            oPhrase.Add(new Chunk(": ", FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL)));
            oPhrase.Add(new Chunk(_oGRN.GRNDateSt, FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD)));
            _oPdfPCell = new PdfPCell(oPhrase);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oTempPdfPTable.AddCell(_oPdfPCell);
            oTempPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("Supplier", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oTempPdfPTable.AddCell(_oPdfPCell);

            oPhrase = new Phrase();
            oPhrase.Add(new Chunk(": ", FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL)));
            oPhrase.Add(new Chunk(_oGRN.ContractorName, FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD)));
            _oPdfPCell = new PdfPCell(oPhrase);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oTempPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("Ref Type", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oTempPdfPTable.AddCell(_oPdfPCell);

            oPhrase = new Phrase();
            oPhrase.Add(new Chunk(": ", FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL)));
            oPhrase.Add(new Chunk(_oGRN.GRNTypeSt, FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD)));
            _oPdfPCell = new PdfPCell(oPhrase);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oTempPdfPTable.AddCell(_oPdfPCell);
            oTempPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("Received Store", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oTempPdfPTable.AddCell(_oPdfPCell);

            oPhrase = new Phrase();
            oPhrase.Add(new Chunk(": ", FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL)));
            oPhrase.Add(new Chunk(_oGRN.StoreName, FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD)));
            _oPdfPCell = new PdfPCell(oPhrase);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oTempPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oGRN.GRNTypeSt+ " No", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oTempPdfPTable.AddCell(_oPdfPCell);

            oPhrase = new Phrase();
            oPhrase.Add(new Chunk(": ", FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL)));
            oPhrase.Add(new Chunk(_oGRN.RefObjectNo, FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD)));
            _oPdfPCell = new PdfPCell(oPhrase);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oTempPdfPTable.AddCell(_oPdfPCell);
            oTempPdfPTable.CompleteRow();

            /// gate in No
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("Challan No", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oTempPdfPTable.AddCell(_oPdfPCell);

            oPhrase = new Phrase();
            oPhrase.Add(new Chunk(": ", FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL)));
            oPhrase.Add(new Chunk(_oGRN.ChallanNo, FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD)));
            _oPdfPCell = new PdfPCell(oPhrase);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oTempPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("Req No", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oTempPdfPTable.AddCell(_oPdfPCell);

            oPhrase = new Phrase();
            oPhrase.Add(new Chunk(": ", FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL)));
            oPhrase.Add(new Chunk("", FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD)));
            _oPdfPCell = new PdfPCell(oPhrase);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oTempPdfPTable.AddCell(_oPdfPCell);
            oTempPdfPTable.CompleteRow();

            /// gate in No
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("Gate Entry No", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oTempPdfPTable.AddCell(_oPdfPCell);

            oPhrase = new Phrase();
            oPhrase.Add(new Chunk(": ", FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL)));
            oPhrase.Add(new Chunk(_oGRN.GatePassNo, FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD)));
            _oPdfPCell = new PdfPCell(oPhrase);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oTempPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("Req Date", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oTempPdfPTable.AddCell(_oPdfPCell);

            oPhrase = new Phrase();
            oPhrase.Add(new Chunk(": ", FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL)));
            oPhrase.Add(new Chunk("", FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD)));
            _oPdfPCell = new PdfPCell(oPhrase);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oTempPdfPTable.AddCell(_oPdfPCell);
            oTempPdfPTable.CompleteRow();

          
            /// gate in No
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("Gate Entry Date", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oTempPdfPTable.AddCell(_oPdfPCell);

            oPhrase = new Phrase();
            oPhrase.Add(new Chunk(": ", FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL)));
            oPhrase.Add(new Chunk(_oGRN.ChallanDateSt, FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD)));
            _oPdfPCell = new PdfPCell(oPhrase);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oTempPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("Bill No", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oTempPdfPTable.AddCell(_oPdfPCell);

            oPhrase = new Phrase();
            oPhrase.Add(new Chunk(": ", FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL)));
            oPhrase.Add(new Chunk(_oGRN.RefObjectNo, FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD)));
            _oPdfPCell = new PdfPCell(oPhrase);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oTempPdfPTable.AddCell(_oPdfPCell);
            oTempPdfPTable.CompleteRow();

            ///

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("Remarks", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oTempPdfPTable.AddCell(_oPdfPCell);

            oPhrase = new Phrase();
            oPhrase.Add(new Chunk(": ", FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL)));
            oPhrase.Add(new Chunk(_oGRN.Remarks, FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD)));
            _oPdfPCell = new PdfPCell(oPhrase);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oTempPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("Bill Date", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oTempPdfPTable.AddCell(_oPdfPCell);

            oPhrase = new Phrase();
            oPhrase.Add(new Chunk(": ", FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL)));
            oPhrase.Add(new Chunk(_oGRN.RefObjectDateSt, FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD)));
            _oPdfPCell = new PdfPCell(oPhrase);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oTempPdfPTable.AddCell(_oPdfPCell);
            oTempPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oTempPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Colspan = _nColumn; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            #endregion

            #region Blank Rows
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.FixedHeight = 15f; _oPdfPCell.Colspan = _nColumn; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region GRNDetail
            _oPdfPCell = new PdfPCell(new Phrase("Item Details Description", _oFontStyle));
            _oPdfPCell.Colspan = _nColumn; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("SL No", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Product", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Specification", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);
                       
            _oPdfPCell = new PdfPCell(new Phrase("Unit", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Quantity", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Unit Price(" + _oGRN.Currency+")", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Amount(" + _oGRN.Currency + ")", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            int nCount = 0;
            double nTotalQty = 0, nTotalAmount = 0;
            foreach (GRNDetail oItem in _oGRNDetails)
            {
                nCount++;
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle)); _oPdfPCell.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName, _oFontStyle)); _oPdfPCell.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.TechnicalSpecification, _oFontStyle)); _oPdfPCell.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                             
                _oPdfPCell = new PdfPCell(new Phrase(oItem.MUName, _oFontStyle)); _oPdfPCell.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.ReceivedQty), _oFontStyle)); _oPdfPCell.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.UnitPrice.ToString("#,##0.00"), _oFontStyle)); _oPdfPCell.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Amount.ToString("#,##0.00"), _oFontStyle)); _oPdfPCell.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                nTotalQty += oItem.ReceivedQty;
                nTotalAmount += oItem.Amount;
            }
            
            #region Total
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Total :", _oFontStyle)); _oPdfPCell.Colspan = _nColumn - 3; _oPdfPCell.Border = iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalQty), _oFontStyle)); 
            _oPdfPCell.Border = iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle)); _oPdfPCell.Border = iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(nTotalAmount.ToString("#,##0.00"), _oFontStyle)); _oPdfPCell.Border = iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.BOTTOM_BORDER;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Discount
            double nTempDiscount = 0;
            if (_oGRN.RefDisCount > 0)
            {
                nTempDiscount = (_oGRN.RefDisCount / _oGRN.RefAmount) * (nTotalAmount);
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle)); _oPdfPCell.Colspan = _nColumn - 3;
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Discount :", _oFontStyle)); _oPdfPCell.Colspan = 2;
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(nTempDiscount.ToString("#,##0.00"), _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion

            #region SErvice Charge
            double nTempServiceCharge = 0;
            if (_oGRN.RefServiceCharge> 0)
            {
                nTempServiceCharge = (_oGRN.RefServiceCharge / _oGRN.RefAmount) * (nTotalAmount);
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle)); _oPdfPCell.Colspan = _nColumn - 3;
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Service Charge :", _oFontStyle)); _oPdfPCell.Colspan =2; 
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(nTempServiceCharge.ToString("#,##0.00"), _oFontStyle)); 
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion

            #region Net Amount

            if (_oGRN.RefServiceCharge > 0 || _oGRN.RefDisCount> 0)
            {
                nTempServiceCharge = (_oGRN.RefServiceCharge / _oGRN.RefAmount) * (nTotalAmount);
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle)); _oPdfPCell.Colspan = _nColumn - 3; _oPdfPCell.Border =  iTextSharp.text.Rectangle.BOTTOM_BORDER;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Net Amount:", _oFontStyle)); _oPdfPCell.Colspan = 2; _oPdfPCell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(((nTotalAmount + nTempServiceCharge) - nTempDiscount).ToString("#,##0.00"), _oFontStyle)); _oPdfPCell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                 _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion

            #region Amount in word
            string sAmountInWords = "";
            if (_oGRN.Currency == "$") { sAmountInWords = Global.DollarWords((nTotalAmount + nTempServiceCharge) - nTempDiscount); } else if (_oGRN.Currency == "TK" || _oGRN.Currency == "Tk") { sAmountInWords = Global.TakaWords((nTotalAmount + nTempServiceCharge) - nTempDiscount); } else if (_oGRN.Currency == "EU") { sAmountInWords = Global.EuroWords((nTotalAmount + nTempServiceCharge) - nTempDiscount); } 
            _oPdfPCell = new PdfPCell(new Phrase("In Word :"+sAmountInWords, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.Colspan = _nColumn; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #endregion

            if (_oSignatureSetups.Count <= 0)
            {
                _oApprovalHeads = new List<ApprovalHead>();
                _oApprovalHeads.Add(new ApprovalHead { Name = "Purchase/Submitted By" });
                _oApprovalHeads.Add(new ApprovalHead { Name = "Deputy Manager \n(Accounts & Finance)" });
                _oApprovalHeads.Add(new ApprovalHead { Name = "AGM \n(Sourcing)" });
                _oApprovalHeads.Add(new ApprovalHead { Name = "Internal Audit" });
                PrintAuthorizationTRole(_oApprovalHeads);

                _oApprovalHeads = new List<ApprovalHead>();
                _oApprovalHeads.Add(new ApprovalHead { Name = "GM \n(Accounts & Finance)" });
                _oApprovalHeads.Add(new ApprovalHead { Name = "GM \n(Internal Audit)" });
                _oApprovalHeads.Add(new ApprovalHead { Name = "ED" });
                PrintAuthorizationTRole(_oApprovalHeads);
            }
            else
            { PrintAuthorizationTRoleTwo(); }

        }

        private void PrintAuthorizationTRole(List<ApprovalHead> _oApprovalHeads)
        {
               #region Authorization
            if (_oApprovalHeads.Count > 0)
            {
                int signNumber = _oApprovalHeads.Count;
                PdfPTable oTempPdfPTable = new PdfPTable(signNumber);

                float colWidth = 535 / signNumber;
                float[] widths = new float[signNumber];
                for (int i = 0; i < _oApprovalHeads.Count; i++)
                {
                    widths[i] = colWidth;
                }
                oTempPdfPTable.SetWidths(widths);//535

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

                foreach (ApprovalHead oItem in _oApprovalHeads)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("____________________", _oFontStyle)); _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);

                }
                oTempPdfPTable.CompleteRow();

                foreach (ApprovalHead oItem in _oApprovalHeads)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.Name, _oFontStyle)); _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);

                }
                oTempPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(oTempPdfPTable); 
                _oPdfPCell.Colspan = _nColumn; _oPdfPCell.FixedHeight = 50f; _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion
        }
        private void PrintAuthorizationTRoleTwo()
        {
            #region print Signature Captions
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Colspan = _nColumn;
            _oPdfPCell.FixedHeight = 20f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(ESimSolSignature.GetSignature(525f, (object)_oGRN, _oSignatureSetups, 20.0f)); _oPdfPCell.Colspan = _nColumn;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

        }
        #endregion
    }
}
