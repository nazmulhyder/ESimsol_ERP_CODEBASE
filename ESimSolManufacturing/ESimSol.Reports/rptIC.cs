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

    public class rptIC
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        Phrase _oPhrase = new Phrase();
        Chunk _oChunk = new Chunk();
        iTextSharp.text.Image _oImag;
        PdfWriter _oWriter;
        MemoryStream _oMemoryStream = new MemoryStream();

        Company _oCompany = new Company();
        InspectionCertificate _oInspectionCertificate = new InspectionCertificate();
        List<InspectionCertificateDetail> _oInspectionCertificateDetails = new List<InspectionCertificateDetail>();

        double _nTotalOrderQty = 0;
        double _nTotalShippedQty = 0;
        double _nTotalCartonQty = 0;
       
        #endregion

        public byte[] PrepareReport(InspectionCertificate oInspectionCertificate, Company oCompany)
        {
            _oInspectionCertificate = oInspectionCertificate;
            _oInspectionCertificateDetails = oInspectionCertificate.InspectionCertificateDetails;
            _oCompany = oCompany;

            #region Page Setup
            _oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
            _oDocument.SetMargins(30f, 30f, 30f, 30f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            //_oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _oWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();

            _oPdfPTable.SetWidths(new float[] { 595f});
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

                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.ExtraParagraphSpace = 0;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Address + "\n" + _oCompany.Phone + ";  " + _oCompany.Email + ";  " + _oCompany.WebAddress, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;

            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 5f;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            #endregion

            #region Line
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.FixedHeight = 5;
            _oPdfPCell.Border = 1;
            _oPdfPCell.BorderWidth = 1f;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Report Caption
            _oFontStyle = FontFactory.GetFont("Tahoma", 14f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Inspection Certificate", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Colspan = 7;
            _oPdfPCell.FixedHeight = 35;
            _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        #endregion

        #region Report Body
        private void PrintBody()
        {
            #region Shipper and Manufactturer 

            PdfPTable oHeaderPdfPTable = new PdfPTable(4);
            oHeaderPdfPTable.WidthPercentage = 100;
            oHeaderPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oHeaderPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            oHeaderPdfPTable.SetWidths(new float[] { 100f, 290f, 45f,160f});

            #region 1st Row 
            _oFontStyle = FontFactory.GetFont("Tahoma", 9.5f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Shipper :", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;_oPdfPCell.BackgroundColor = BaseColor.WHITE;_oPdfPCell.Border = 0;oHeaderPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 9.5f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oInspectionCertificate.ShipperName, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;_oPdfPCell.BackgroundColor = BaseColor.WHITE;_oPdfPCell.Border = 0;oHeaderPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Date:", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;_oPdfPCell.BackgroundColor = BaseColor.WHITE;_oPdfPCell.Border = 0; oHeaderPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase(_oInspectionCertificate.ICDate.ToString("dd - MM - yyyy"), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;_oPdfPCell.BackgroundColor = BaseColor.WHITE;_oPdfPCell.Border = 0;oHeaderPdfPTable.AddCell(_oPdfPCell);
            oHeaderPdfPTable.CompleteRow();
            #endregion

            #region 2nd  Row
            _oFontStyle = FontFactory.GetFont("Tahoma", 9.5f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Manufacturer :", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; oHeaderPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 9.5f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oInspectionCertificate.MenufacturerName, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; oHeaderPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("Ref.No.", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; oHeaderPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oInspectionCertificate.RefNo, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; oHeaderPdfPTable.AddCell(_oPdfPCell);
            oHeaderPdfPTable.CompleteRow();
            #endregion

            _oPdfPCell = new PdfPCell(oHeaderPdfPTable);
            _oPdfPCell.Border = 0;  _oPdfPCell.ExtraParagraphSpace = 8f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

            #region Blank Row
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region 2nd Informatin
            oHeaderPdfPTable = new PdfPTable(4);
            oHeaderPdfPTable.SetWidths(new float[] { 150f, 120f, 150f, 170f });

            #region Table Header 
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Invoice No. & Date:", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oHeaderPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Invoice Value", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oHeaderPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("L/C No. & Date", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oHeaderPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("Bill of Lading No. & Date", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oHeaderPdfPTable.AddCell(_oPdfPCell);
            oHeaderPdfPTable.CompleteRow();
            #endregion

            #region Table Value 

            #region 1st Row 
            _oFontStyle = FontFactory.GetFont("Tahoma", 9.5f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oInspectionCertificate.InvoiceNo, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oHeaderPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oInspectionCertificate.InvoiceValue) , _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oHeaderPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oInspectionCertificate.MasterLCNo, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oHeaderPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase(_oInspectionCertificate.BillOfLadingNo, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oHeaderPdfPTable.AddCell(_oPdfPCell);
            oHeaderPdfPTable.CompleteRow();
            #endregion

            #region 2nd  Row
            _oFontStyle = FontFactory.GetFont("Tahoma", 9.5f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("Date :"+_oInspectionCertificate.InvoiceDate.ToString("dd - MM - yyyy"), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oHeaderPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oHeaderPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Date :" + _oInspectionCertificate.MasterLCDate.ToString("dd - MM - yyyy"), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oHeaderPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("Date :" + _oInspectionCertificate.BillOfLadingDate.ToString("dd - MM - yyyy"), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oHeaderPdfPTable.AddCell(_oPdfPCell);
            oHeaderPdfPTable.CompleteRow();
            #endregion

            _oPdfPCell = new PdfPCell(oHeaderPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.ExtraParagraphSpace = 8f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #endregion

            #region Blank Row
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region ORGinal Print Table 
            oHeaderPdfPTable = new PdfPTable(4);

            #region 1st Row
            _oFontStyle = FontFactory.GetFont("Tahoma", 9.5f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Vessel :", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oHeaderPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 9.5f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oInspectionCertificate.Vessel, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Colspan = 2; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oHeaderPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 22f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("ORGINAL", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.Rowspan = 3; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oHeaderPdfPTable.AddCell(_oPdfPCell);

            oHeaderPdfPTable.CompleteRow();
            #endregion

            #region 2nd  Row
            _oFontStyle = FontFactory.GetFont("Tahoma", 9.5f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Port Of Loading :", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oHeaderPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 9.5f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oInspectionCertificate.PortOfLoading, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oHeaderPdfPTable.AddCell(_oPdfPCell);

            oHeaderPdfPTable.CompleteRow();
            #endregion

            #region 3rd Row
            _oFontStyle = FontFactory.GetFont("Tahoma", 9.5f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Final Destination:", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oHeaderPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 9.5f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oInspectionCertificate.FinalDestination, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Colspan = 2; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oHeaderPdfPTable.AddCell(_oPdfPCell);

            oHeaderPdfPTable.CompleteRow();
            #endregion

            _oPdfPCell = new PdfPCell(oHeaderPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.ExtraParagraphSpace = 8f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion


            #region Blank Row
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region IC Detail 
            oHeaderPdfPTable = new PdfPTable(5);
            oHeaderPdfPTable.SetWidths(new float[] { 40f,225f, 100f, 105f, 125f });

            #region Detail Table header
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("SL No.", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oHeaderPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Style No. /Order No.", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oHeaderPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Order Quantity (Pcs)", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oHeaderPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Shipped Quantity (Pcs)", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oHeaderPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("Carton Quantity", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oHeaderPdfPTable.AddCell(_oPdfPCell);
            oHeaderPdfPTable.CompleteRow();
            #endregion

           int nCount = 0;
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
            foreach (InspectionCertificateDetail oItem in _oInspectionCertificateDetails)
            {
                nCount++;

                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oHeaderPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.StyleNoOrderNo, _oFontStyle));
                 _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oHeaderPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.OrderQty), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oHeaderPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.ShipedQty), _oFontStyle));
                 _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oHeaderPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.CartonQty), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oHeaderPdfPTable.AddCell(_oPdfPCell);

                _nTotalOrderQty += oItem.OrderQty;
                _nTotalShippedQty += oItem.ShipedQty;
                _nTotalCartonQty += oItem.CartonQty;

                oHeaderPdfPTable.CompleteRow();
            }

            #region Total Print 

            _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Total Quantity", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Colspan = 2; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oHeaderPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalOrderQty), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oHeaderPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalShippedQty), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oHeaderPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalCartonQty), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oHeaderPdfPTable.AddCell(_oPdfPCell);
            oHeaderPdfPTable.CompleteRow();

            #endregion

            _oPdfPCell = new PdfPCell(oHeaderPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.ExtraParagraphSpace = 8f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
           
            #endregion

            #region Blank Rows
            _oPhrase = new Phrase();
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            _oChunk = new Chunk(" ", _oFontStyle);
            _oPhrase.Add(_oChunk);

            _oPdfPCell = new PdfPCell(_oPhrase);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 10;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Description 
            _oFontStyle = FontFactory.GetFont("Tahoma", 9.5f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("We hereby certify that the above mentioned goods were inspected at random and found to fonfirm of your requirements. This certificate, however, does not stand for a final acceptance of the goods by the buyer who remains entitled to inspects the goods upon receipt. Furthermore, the manufacturer would be sloely liable for any claim resulting from the shipmnet of poor quantity/badly executed goods. We confirm goods manufactured in accordance with applicant's specifications.", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Remarks 
         
            _oPhrase = new Phrase();
            _oFontStyle = FontFactory.GetFont("Tahoma", 9.5f, iTextSharp.text.Font.BOLD);
            _oChunk = new Chunk("Remarks:", _oFontStyle);
            _oPhrase.Add(_oChunk);
            _oFontStyle = FontFactory.GetFont("Tahoma", 9.5f, iTextSharp.text.Font.NORMAL);
            _oChunk = new Chunk("Goods have been inspected and found merchandise in good Order and Condition.", _oFontStyle);
            _oPhrase.Add(_oChunk);

            _oPdfPCell = new PdfPCell(_oPhrase);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;

            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

            #region Blank Row
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 40f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Authorized Signature
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("Authorized Signature", _oFontStyle));
            _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));
            _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);



            #endregion

        }

        #endregion
    }
    
    
}
