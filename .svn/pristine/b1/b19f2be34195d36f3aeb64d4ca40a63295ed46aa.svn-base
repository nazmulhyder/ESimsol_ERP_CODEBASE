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
using System.Linq;
namespace ESimSol.Reports
{
    public class rptMRIRPrint
    {
        #region Declaration
        private int _nColumn = 9;
        private Document _oDocument;
        private iTextSharp.text.Font _oFontStyle;
        private PdfPTable _oPdfPTable = new PdfPTable(9);
        private PdfPCell _oPdfPCell;
        PdfWriter PDFWriter;
        private iTextSharp.text.Image _oImag;
        private MemoryStream _oMemoryStream = new MemoryStream();
        private GRN _oGRN = new GRN();
        private List<GRNDetail> _oGRNDetails = new List<GRNDetail>();
        private Company _oCompany = new Company();
        Phrase _oPhrase = new Phrase();
        Chunk _oChunk = new Chunk();
        #endregion

        public byte[] PrepareReport(GRN oGRN, Company oCompany)
        {
            _oGRN = oGRN;
            _oGRNDetails = oGRN.GRNDetails;
            _oCompany = oCompany;
            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
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
                                                200f, //Product
                                                55f, // Lot No
                                                70f, // Technical Specification
                                                60f, // Chllan qty
                                                60f, // RCV qty
                                                60f, // Yet qty
                                                60f,//Return Qty
                                                50f //Remark
                                              });
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
                _oImag.ScaleAbsolute(60f, 35f);
                _oPdfPCell = new PdfPCell(_oImag);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            }
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.Rowspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oFontStyle = FontFactory.GetFont("Tahoma", 18f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));

            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.PringReportHead, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.Colspan = _nColumn; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion


            #region Space
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Colspan = _nColumn;_oPdfPCell.FixedHeight = 7f;_oPdfPCell.Border = 0;_oPdfPCell.BorderWidthBottom = 1f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;_oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;_oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion


            #region ReportHeader
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
            //_oPdfPCell = new PdfPCell(new Phrase("Purchase Return", _oFontStyle));
            _oPdfPCell = new PdfPCell(new Phrase("Material Receiving Inspection Report(MRIR)", _oFontStyle)); //requiremnt by Sabuj for PTL
            _oPdfPCell.Colspan = _nColumn;
            _oPdfPCell.FixedHeight = 25f;
            _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
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
            oTempPdfPTable.SetWidths(new float[] { 70f, 295f, 70f, 100f });//535

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("MRIR No", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oTempPdfPTable.AddCell(_oPdfPCell);

            string sMRINo = _oGRN.MRIRNo;
            if (sMRINo == null || sMRINo == "")
            {
                if (_oCompany.BaseAddress.ToUpper() == "AUDI")
                {
                    sMRINo = _oGRN.GRNNo;
                }
            }

            oPhrase = new Phrase();
            oPhrase.Add(new Chunk(": ", FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL)));
            oPhrase.Add(new Chunk(sMRINo + "                                                         ", FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD)));
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
            oPhrase.Add(new Chunk(_oGRN.MRIRDateSt, FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD)));
            _oPdfPCell = new PdfPCell(oPhrase);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oTempPdfPTable.AddCell(_oPdfPCell);
            oTempPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("Receive Store", _oFontStyle));
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
            _oPdfPCell = new PdfPCell(new Phrase("GRN Date", _oFontStyle));
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
            _oPdfPCell = new PdfPCell(new Phrase("Ref Type", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oTempPdfPTable.AddCell(_oPdfPCell);

            oPhrase = new Phrase();
            oPhrase.Add(new Chunk(": ", FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL)));
            oPhrase.Add(new Chunk(_oGRN.GRNTypeSt+" ("+_oGRN.RefObjectNo+" )", FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD)));
            _oPdfPCell = new PdfPCell(oPhrase);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oTempPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("GRN No", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oTempPdfPTable.AddCell(_oPdfPCell);

            oPhrase = new Phrase();
            oPhrase.Add(new Chunk(": ", FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL)));
            oPhrase.Add(new Chunk(_oGRN.GRNNo, FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD)));
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
            _oPdfPCell = new PdfPCell(new Phrase("Challan No", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
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
            oTempPdfPTable.CompleteRow();


            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("Address", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oTempPdfPTable.AddCell(_oPdfPCell);

            oPhrase = new Phrase();
            oPhrase.Add(new Chunk(": ", FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL)));
            oPhrase.Add(new Chunk(_oGRN.Address, FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD)));
            _oPdfPCell = new PdfPCell(oPhrase);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oTempPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("L/C No", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oTempPdfPTable.AddCell(_oPdfPCell);

            oPhrase = new Phrase();
            oPhrase.Add(new Chunk(": ", FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL)));
            oPhrase.Add(new Chunk(_oGRN.ImportLCNo, FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD)));
            _oPdfPCell = new PdfPCell(oPhrase);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oTempPdfPTable.AddCell(_oPdfPCell);
            oTempPdfPTable.CompleteRow();


            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oGRN.GRNType.ToString() + " Date", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
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
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oTempPdfPTable.AddCell(_oPdfPCell);

            oPhrase = new Phrase();
            oPhrase.Add(new Chunk(" ", FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL)));
            oPhrase.Add(new Chunk("", FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD)));
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
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("SL No", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Descriptoin Of Goods", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Lot No", _oFontStyle)); //Req by Umor
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Specification", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Challan Qty", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("GRN/Rcv Qty", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Yet Qty", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Return Qty", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Remarks", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            int nCount = 0;
            double nTotalQty = 0, nTotalAmount = 0;
            foreach (GRNDetail oItem in _oGRNDetails)
            {
                nCount++;
                _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle)); 
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName, _oFontStyle)); 
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.LotNo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.TechnicalSpecification, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.RefQty) + " " + oItem.MUSymbol, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.ReceivedQty) + " " + oItem.MUSymbol, _oFontStyle));  
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.RefQty-oItem.ReceivedQty) + " " + oItem.MUSymbol, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.RejectQty) + " " + oItem.MUSymbol, _oFontStyle));  
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Remarks, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                nTotalQty += oItem.ReceivedQty;
                nTotalAmount += oItem.Amount;
            }

            int n = 5;
            for (int i = nCount; i < n; i++)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle)); 
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle)); 
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle)); 
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle)); 
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle)); 
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }

            #endregion

            #region Total
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyle));
            _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oGRNDetails.Sum(x => x.RefQty)) + " " + _oGRNDetails[0].MUSymbol, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oGRNDetails.Sum(x => x.ReceivedQty)) + " " + _oGRNDetails[0].MUSymbol, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oGRNDetails.Sum(x => x.RefQty-x.ReceivedQty)) + " " + _oGRNDetails[0].MUSymbol, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oGRNDetails.Sum(x => x.RejectQty)) + " " + _oGRNDetails[0].MUSymbol, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            List<ApprovalHead> _oApprovalHeads=new List<ApprovalHead>();
            _oApprovalHeads.Add(new ApprovalHead { Name = "Prepared By" });
            _oApprovalHeads.Add(new ApprovalHead { Name = "Section Incharge " });
            _oApprovalHeads.Add(new ApprovalHead { Name = "QC Check By" });
            _oApprovalHeads.Add(new ApprovalHead { Name = "Authorized" });
            PrintAuthorizationTRole(_oApprovalHeads);


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
        #endregion
    }
}
