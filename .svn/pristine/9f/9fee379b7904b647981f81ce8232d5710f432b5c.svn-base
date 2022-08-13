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
using ICS.Core.Framework;

namespace ESimSol.Reports
{
    public class rptNotice
    {
        #region Declaration

        iTextSharp.text.Image _oImag;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(3);
        PdfPCell _oPdfPCell;
        MemoryStream _oMemoryStream = new MemoryStream();
        Notice _oNotice = new Notice();
        Company _oCompany = new Company();
        Phrase _oPhrase;
        DateTime _dstartDate = DateTime.Now;
        DateTime _dEndDate = DateTime.Now;

        #endregion

        public byte[] PrepareReport(Notice oNotice, Company oCompany)
        {
            _oNotice = oNotice;
            _oCompany = oCompany;

            #region Page Setup
            //_oDocument = new Document(new iTextSharp.text.Rectangle(_nPageWidth, _npageHeight), 0f, 0f, 0f, 0f);
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            _oDocument.SetMargins(40f, 40f, 5f, 40f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 240f, 20f, 240f });
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

            PdfPTable oPdfPTableHeader = new PdfPTable(2);
            oPdfPTableHeader.WidthPercentage = 100;
            oPdfPTableHeader.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTableHeader.SetWidths(new float[] { 150f, 250f });
            PdfPCell  oPdfPCellHearder;

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            if (_oCompany.CompanyLogo != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(90f, 35f);
                oPdfPCellHearder = new PdfPCell(_oImag);
                oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPCellHearder.VerticalAlignment = Element.ALIGN_BOTTOM;
                oPdfPCellHearder.Border = 0;
                oPdfPCellHearder.FixedHeight = 35;
                //_oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                //oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_LEFT; 
                oPdfPCellHearder.Border = 0; oPdfPCellHearder.BackgroundColor = BaseColor.WHITE;
                oPdfPTableHeader.AddCell(oPdfPCellHearder);
            }
            else
            {

                oPdfPCellHearder = new PdfPCell(new Phrase("", _oFontStyle));
                oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPCellHearder.VerticalAlignment = Element.ALIGN_BOTTOM;
                oPdfPCellHearder.Border = 0;
                oPdfPCellHearder.FixedHeight = 35;
                //_oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                //oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_LEFT; 
                oPdfPCellHearder.Border = 0; oPdfPCellHearder.BackgroundColor = BaseColor.WHITE; oPdfPTableHeader.AddCell(oPdfPCellHearder);

            }
            _oFontStyle = FontFactory.GetFont("Tahoma", 14f, 1);
            oPdfPCellHearder = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));
            oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellHearder.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCellHearder.Border = 0; oPdfPCellHearder.BackgroundColor = BaseColor.WHITE; oPdfPTableHeader.AddCell(oPdfPCellHearder);


            oPdfPTableHeader.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            oPdfPCellHearder = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCellHearder.Border = 0;
            oPdfPCellHearder.BackgroundColor = BaseColor.WHITE;
            oPdfPCellHearder.ExtraParagraphSpace = 0;
            oPdfPTableHeader.AddCell(oPdfPCellHearder);

            _oPdfPCell = new PdfPCell(oPdfPTableHeader);
            _oPdfPCell.Colspan = 3;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Address, _oFontStyle));
            _oPdfPCell.Colspan = 3;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 5;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            //_oPdfPCell = new PdfPCell(new Phrase(" "));
            //_oPdfPCell.Colspan = 3;
            //_oPdfPCell.Border = 0;
            //_oPdfPCell.FixedHeight = 5;
            //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();


            #endregion

        }
        #endregion

        private void PrintBody()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 20;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Colspan = 3; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
            _oPdfPCell = new PdfPCell(new Phrase("NOTICE", _oFontStyle));
            _oPdfPCell.Colspan = 3;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 40;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Colspan = 3; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            _oPhrase = new Phrase();
            iTextSharp.text.Font oFontStyleRowHeader = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            iTextSharp.text.Font oFontStyleData = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            _oPhrase.Add(new Chunk("Notice No : ", oFontStyleRowHeader));
            _oPhrase.Add(new Chunk(_oNotice.NoticeNo, oFontStyleData)); _oPdfPCell = new PdfPCell(_oPhrase);
            _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Colspan = 3; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPhrase = new Phrase();
            _oPhrase.Add(new Chunk("Issue Date : ", oFontStyleRowHeader));
            _oPhrase.Add(new Chunk(_oNotice.IssueDateInStr, oFontStyleData)); _oPdfPCell = new PdfPCell(_oPhrase);
            _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Colspan = 3; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPhrase = new Phrase();
            _oPhrase.Add(new Chunk("Expire Date : ", oFontStyleRowHeader));
            _oPhrase.Add(new Chunk(_oNotice.ExpireDateInStr, oFontStyleData)); _oPdfPCell = new PdfPCell(_oPhrase);
            _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Colspan = 3; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            
            _oPdfPTable.CompleteRow();


            
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 20;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Colspan = 3; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPhrase = new Phrase();
            _oPhrase.Add(new Chunk("Subject: ", oFontStyleRowHeader));
            _oPhrase.Add(new Chunk(_oNotice.Title, oFontStyleData)); _oPdfPCell = new PdfPCell(_oPhrase);
            _oPdfPCell.Colspan = 3; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 25;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Colspan = 3; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oNotice.Description, _oFontStyle)); _oPdfPCell.Colspan = 3; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Colspan = 3;
            _oPdfPCell.FixedHeight = 250;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            //_oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            //_oPdfPCell = new PdfPCell(new Phrase("Sign: ", _oFontStyle)); _oPdfPCell.Border = 0;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Colspan = 3; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();

            //_oPdfPCell = new PdfPCell(new Phrase("Name: ", _oFontStyle)); _oPdfPCell.Border = 0;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Colspan = 3; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();

            //_oPdfPCell = new PdfPCell(new Phrase("Designation: ", _oFontStyle)); _oPdfPCell.Border = 0;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Colspan = 3; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();

        }

    }

}

