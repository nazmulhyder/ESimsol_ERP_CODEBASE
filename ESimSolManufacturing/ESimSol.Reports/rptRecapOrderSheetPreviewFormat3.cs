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
    public class rptRecapOrderSheetPreviewFormat3
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        OrderRecap _oOrderRecap = new OrderRecap();
        List<OrderRecap> _oOrderRecaps = new List<OrderRecap>();
        List<OrderRecapDetail> _oOrderRecapDetails = new List<OrderRecapDetail>();
        List<SizeCategory> _oSizeCategories = new List<SizeCategory>();
        List<ColorCategory> _oColorCategories = new List<ColorCategory>();
        List<RecapBillOfMaterial> _oRecapBillOfMaterials = new List<RecapBillOfMaterial>();
        List<ORAssortment> _oORAssortments = new List<ORAssortment>();
        Company _oCompany = new Company();
         int _nRowSpan = 18;//for Defult format
         double _nTotalAssortmentQty = 0;
           
        #endregion

     
         public byte[] PrepareReportFormat3(OrderRecap oOrderRecap)
         {
             _oOrderRecap = oOrderRecap;
             _oOrderRecapDetails = oOrderRecap.OrderRecapDetails;
             _oSizeCategories = oOrderRecap.SizeCategories;
             _oColorCategories = oOrderRecap.ColorCategories;
             _oCompany = oOrderRecap.Company;
             _oRecapBillOfMaterials = oOrderRecap.RecapBillOfMaterials;
             _oORAssortments = oOrderRecap.ORAssortments;

             #region Page Setup
             _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
             _oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
             _oDocument.SetMargins(25f, 25f, 15f, 15f);
             _oPdfPTable.WidthPercentage = 100;
             _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

             _oFontStyle = FontFactory.GetFont("Tahoma", 13f, 1);
             PdfWriter.GetInstance(_oDocument, _oMemoryStream);
             _oDocument.Open();

             _oPdfPTable.SetWidths(new float[] { 495f });
             #endregion

             this.PrintHeader();
             this.PrintBody();
             _oDocument.Add(_oPdfPTable);
             _oDocument.Close();
             return _oMemoryStream.ToArray();
         }


         #region Report Header
         private void PrintHeader()
         {
             #region ReportHeader
             _oPdfPCell = new PdfPCell(new Phrase("ORDER SHEET", _oFontStyle));
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
             _oPdfPCell.Border = 0;
             _oPdfPCell.BackgroundColor = BaseColor.WHITE;
             _oPdfPCell.ExtraParagraphSpace = 5f;
             _oPdfPTable.AddCell(_oPdfPCell);
             _oPdfPTable.CompleteRow();

             #endregion
         }
         #endregion

         private void PrintBody()
         {
             PdfPTable oPdfPTable = new PdfPTable(3);
             oPdfPTable.WidthPercentage = 100;
             oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
             _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
             oPdfPTable.SetWidths(new float[] { 290f, 252f, 250f });

             #region Company Supplier Information

             #region 1st Row
             #region company Logo print

             if (_oCompany.CompanyLogo != null)
             {
                 PdfPTable oCompanyPdfPTable = new PdfPTable(2);
                 oCompanyPdfPTable.WidthPercentage = 100;
                 oCompanyPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                 _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
                 oCompanyPdfPTable.SetWidths(new float[] { 100f, 190f });

                 _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                 _oImag.ScaleAbsolute(80f, 30f);

                 _oPdfPCell = new PdfPCell(_oImag);
                 _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                 _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                 _oPdfPCell.Border = 0;
                 oCompanyPdfPTable.AddCell(_oPdfPCell);

                 _oFontStyle = FontFactory.GetFont("Tahoma", 11f, 1);
                 _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));
                 _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                 _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                 _oPdfPCell.Border = 0;
                 _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                 oCompanyPdfPTable.AddCell(_oPdfPCell);

                 _oPdfPCell = new PdfPCell(oCompanyPdfPTable);
                 _oPdfPCell.Rowspan = 3;
                 _oPdfPCell.BorderWidthLeft = 0.5f;
                 _oPdfPCell.BorderWidthBottom = 0;
                 _oPdfPCell.BorderWidthRight = 0;
                 _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE;
                 oPdfPTable.AddCell(_oPdfPCell);

             }
             else
             {
                 _oFontStyle = FontFactory.GetFont("Tahoma", 11f, 1);
                 _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));
                 _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                 _oPdfPCell.BorderWidthLeft = .5f;
                 _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                 _oPdfPCell.Rowspan = 3;
                 oPdfPTable.AddCell(_oPdfPCell);

             }
             #endregion
             _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL);
             _oPdfPCell = new PdfPCell(new Phrase("SUPPLIER", _oFontStyle));
             _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f;
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Rowspan = 2; oPdfPTable.AddCell(_oPdfPCell);
             _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
             _oPdfPCell = new PdfPCell(new Phrase("ORDER DATE :" + _oOrderRecap.OrderDateInString, _oFontStyle));
             _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

             oPdfPTable.CompleteRow();
             #endregion

             #region 2nd Row
             _oPdfPCell = new PdfPCell(new Phrase("SHIPMENT DATE: " + _oOrderRecap.FactoryShipmentDateInString  /*_oOrderRecap.ShipmentDate.Subtract(TimeSpan.FromDays(_oOrderRecap.ClientOperationSetting.OrderSheetShipmentDateCount)).ToString("dd MMM yyyy")*/, _oFontStyle));
             _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
             oPdfPTable.CompleteRow();
             #endregion

             #region 3rd Row
             _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
             _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthTop = 0f; _oPdfPCell.BorderWidthRight = 0f; _oPdfPCell.BorderWidthBottom = 0f;
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

             _oPdfPCell = new PdfPCell(new Phrase("TERMS OF DELIVERY: " + _oOrderRecap.DeliveryTerm, _oFontStyle));
             _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
             oPdfPTable.CompleteRow();
             #endregion

             #region 4th Row
             _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Address, _oFontStyle));
             _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthRight = 0f; _oPdfPCell.BorderWidthBottom = 0f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Rowspan = 2; oPdfPTable.AddCell(_oPdfPCell);

             _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.NORMAL);
             _oPdfPCell = new PdfPCell(new Phrase(_oOrderRecap.FactoryName, _oFontStyle));
             _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthTop = 0f; _oPdfPCell.BorderWidthRight = 0f; _oPdfPCell.BorderWidthBottom = 0f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

             _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
             _oPdfPCell = new PdfPCell(new Phrase("PAYMENTS TERMS: " + _oOrderRecap.PaymentTerm, _oFontStyle));
             _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
             oPdfPTable.CompleteRow();
             #endregion

             #region 5th Row
             _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.NORMAL);
             _oPdfPCell = new PdfPCell(new Phrase(_oOrderRecap.FactoryAddress, _oFontStyle));
             _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = 0f; _oPdfPCell.BorderWidthBottom = 0f; _oPdfPCell.BorderWidthTop = 0f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Rowspan = 3; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

             _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
             _oPdfPCell = new PdfPCell(new Phrase("REQUIRED SAMPLES: " + _oOrderRecap.RequiredSample, _oFontStyle));
             _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
             oPdfPTable.CompleteRow();
             #endregion

             #region 6th Row
             _oPdfPCell = new PdfPCell(new Phrase("Tel #" + _oCompany.Phone, _oFontStyle));
             _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthRight = 0f; _oPdfPCell.BorderWidthBottom = 0f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

             _oPdfPCell = new PdfPCell(new Phrase("PACKING INSTRUCTION: " + _oOrderRecap.PackingInstruction, _oFontStyle));
             _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
             oPdfPTable.CompleteRow();
             #endregion

             #region 7th Row
             _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
             _oPdfPCell = new PdfPCell(new Phrase("E-mail: " + _oCompany.Email, _oFontStyle));
             _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthRight = 0f; _oPdfPCell.BorderWidthBottom = 0f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

             _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
             _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
             _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
             oPdfPTable.CompleteRow();
             #endregion

             _oPdfPCell = new PdfPCell(oPdfPTable);
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
             _oPdfPCell.Border = 1;
             _oPdfPCell.BackgroundColor = BaseColor.WHITE;
             _oPdfPCell.Colspan = 1;
             _oPdfPTable.AddCell(_oPdfPCell);
             _oPdfPTable.CompleteRow();
             #endregion

             #region Yarn and Accessorie Print
             oPdfPTable = new PdfPTable(2);
             _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
             oPdfPTable.SetWidths(new float[] { 580f, 200f });

             #region Accessoreis Caption Print
             _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

             _oPdfPCell = new PdfPCell(new Phrase("ACCESSORIES", _oFontStyle));
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

             oPdfPTable.CompleteRow();

             #endregion

             #region Yarn, size And Accessoreis Table Insert
             if (_oColorCategories.Count > 7)
             {
                 _nRowSpan += (_oColorCategories.Count - 7);
             }
             _oPdfPCell = new PdfPCell(GetYarnAndSizeTable());
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

             _oPdfPCell = new PdfPCell(GetAccessoriesTable());
             _oPdfPCell.Rowspan = _nRowSpan; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
             oPdfPTable.CompleteRow();

             _oPdfPCell = new PdfPCell(oPdfPTable);
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
             _oPdfPCell.Border = 1;
             _oPdfPCell.BackgroundColor = BaseColor.WHITE;
             _oPdfPCell.Colspan = 1;
             _oPdfPTable.AddCell(_oPdfPCell);
             _oPdfPTable.CompleteRow();

             #endregion

             #endregion


             #region Print Style Image
             oPdfPTable = new PdfPTable(3);
             _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.NORMAL);
             oPdfPTable.SetWidths(new float[] { 400f, 180f, 200f });

             #region Assortment & Image Caption print

             _oFontStyle = FontFactory.GetFont("Tahoma", 11f, iTextSharp.text.Font.NORMAL);

             if ((int)_oOrderRecap.AssortmentType == 0)//Assort Color Assort Size
             {
                 _oPdfPCell = new PdfPCell(new Phrase("Assortment (Carton Qty: " + Global.MillionFormat(_oOrderRecap.CartonQty) + ")", _oFontStyle));
             }
             else if ((int)_oOrderRecap.AssortmentType == 1) //Solid Color Assort Size
             {
                 _oPdfPCell = new PdfPCell(new Phrase("Assortment (Carton Qty: " + Global.MillionFormat(GetSolidColorSolidSizeCartonQty()) + ")", _oFontStyle));
             }
             else if ((int)_oOrderRecap.AssortmentType == 2) //Solid Color Solid Size
             {
                 double nTotalCartonQty = GetTotalCartonQty();
                 _oPdfPCell = new PdfPCell(new Phrase("Assortment (Carton Qty: " + Global.MillionFormat(nTotalCartonQty) + ")", _oFontStyle));
             }

             _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
             _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
             _oPdfPCell.BorderWidthTop = .5f;
             _oPdfPCell.BorderWidthLeft = .5f;
             _oPdfPCell.BorderWidthBottom = 0.5f;
             _oPdfPCell.BorderWidthRight = 0.5f;
             _oPdfPCell.BackgroundColor = BaseColor.WHITE;
             oPdfPTable.AddCell(_oPdfPCell);

             _oPdfPCell = new PdfPCell(new Phrase("PICTURE IMAGE", _oFontStyle));
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
             _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
             _oPdfPCell.BorderWidthLeft = 0.5f;
             _oPdfPCell.BorderWidthTop = .5f;
             _oPdfPCell.BorderWidthBottom = 0.5f;
             _oPdfPCell.BorderWidthRight = 0.5f;
             _oPdfPCell.BackgroundColor = BaseColor.WHITE;
             oPdfPTable.AddCell(_oPdfPCell);

             _oFontStyle = FontFactory.GetFont("Tahoma", 11f, iTextSharp.text.Font.BOLD);
             _oPdfPCell = new PdfPCell(new Phrase("COMMENTS", _oFontStyle));
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
             _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
             _oPdfPCell.BorderWidthLeft = .5f;
             _oPdfPCell.BorderWidthBottom = 0.5f;
             _oPdfPCell.BackgroundColor = BaseColor.WHITE;
             oPdfPTable.AddCell(_oPdfPCell);

             oPdfPTable.CompleteRow();


             #endregion

             #region Assortment print
             _oFontStyle = FontFactory.GetFont("Tahoma", 11f, 1);
             if ((int)_oOrderRecap.AssortmentType == 0)//Assort Color Assort Size
             {
                 _oPdfPCell = new PdfPCell(GetAssortColorAssortSizeColumnName());
             }
             else if ((int)_oOrderRecap.AssortmentType == 1) //Solid Color Assort Size
             {
                 _oPdfPCell = new PdfPCell(GetSolidColorAssortSizeColumnName());
             }
             else if ((int)_oOrderRecap.AssortmentType == 2) //Solid Color Solid Size
             {
                 _oPdfPCell = new PdfPCell(GetAssortmentSizeColumnName(true));
             }
            
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
             _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
             _oPdfPCell.BorderWidthLeft = .5f;
             _oPdfPCell.BorderWidthRight = 0f;
             _oPdfPCell.FixedHeight = 160f;
             _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
             _oPdfPCell.BackgroundColor = BaseColor.WHITE;
             oPdfPTable.AddCell(_oPdfPCell);

             #endregion

             #region Front Image print
             if (_oOrderRecap.TechnicalSheetFrontImage.FrontImage != null)
             {
                 _oImag = iTextSharp.text.Image.GetInstance(_oOrderRecap.TechnicalSheetFrontImage.FrontImage, System.Drawing.Imaging.ImageFormat.Jpeg);
                 _oImag.ScaleAbsolute(175f, 155f);
                 _oPdfPCell = new PdfPCell(_oImag);
                 _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                 _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                 _oPdfPCell.BorderWidthLeft = 0.5f;
                 _oPdfPCell.BorderWidthRight = 0f;
                 _oPdfPCell.BorderWidthBottom = 0.5f;
                 _oPdfPCell.BorderWidthTop = 0f;
                 _oPdfPCell.FixedHeight = 160f;
                 oPdfPTable.AddCell(_oPdfPCell);

             }
             else
             {
                 _oFontStyle = FontFactory.GetFont("Tahoma", 11f, 1);
                 _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                 _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                 _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                 _oPdfPCell.BorderWidthLeft = 0f;
                 _oPdfPCell.BorderWidthTop = 0f;
                 _oPdfPCell.BorderWidthRight = 0f;
                 _oPdfPCell.FixedHeight = 160f;
                 _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                 oPdfPTable.AddCell(_oPdfPCell);

             }
             #endregion

             _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
             _oPdfPCell = new PdfPCell(new Phrase(GetComments(), _oFontStyle));
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
             _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
             _oPdfPCell.BackgroundColor = BaseColor.WHITE;
             oPdfPTable.AddCell(_oPdfPCell);
             oPdfPTable.CompleteRow();

             _oPdfPCell = new PdfPCell(new Phrase("Remarks :" + _oOrderRecap.Description, _oFontStyle));
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
             _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
             _oPdfPCell.Colspan = 3;
             _oPdfPCell.FixedHeight = 15;
             _oPdfPCell.BackgroundColor = BaseColor.WHITE;
             oPdfPTable.AddCell(_oPdfPCell);
             oPdfPTable.CompleteRow();

             _oPdfPCell = new PdfPCell(oPdfPTable);
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
             _oPdfPCell.Border = 1;
             _oPdfPCell.BackgroundColor = BaseColor.WHITE;
             _oPdfPCell.Colspan = 1;
             _oPdfPTable.AddCell(_oPdfPCell);
             _oPdfPTable.CompleteRow();
             #endregion

             #region space Print
             _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.NORMAL);
             _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
             _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
             _oPdfPCell.FixedHeight = 20;
             _oPdfPCell.Border = 0;
             _oPdfPCell.BackgroundColor = BaseColor.WHITE;
             _oPdfPTable.AddCell(_oPdfPCell);
             _oPdfPTable.CompleteRow();
             #endregion

             #region Signature Print
             _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.NORMAL);
             _oPdfPCell = new PdfPCell(new Phrase("Marchandiser", _oFontStyle));
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
             _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
             _oPdfPCell.Border = 0;
             _oPdfPCell.BackgroundColor = BaseColor.WHITE;
             _oPdfPTable.AddCell(_oPdfPCell);
             _oPdfPTable.CompleteRow();
             #endregion

         }
         private PdfPTable GetAccessoriesTable()
         {
             PdfPTable oPdfPTable = new PdfPTable(2);
             oPdfPTable.WidthPercentage = 100;
             oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
             _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.NORMAL);
             oPdfPTable.SetWidths(new float[] { 80f, 120f });

             #region caption
             _oPdfPCell = new PdfPCell(new Phrase("Type", _oFontStyle));
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

             _oPdfPCell = new PdfPCell(new Phrase("Description", _oFontStyle));
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
             oPdfPTable.CompleteRow();
             #endregion

             int nCount = 0;
             foreach (RecapBillOfMaterial oItem in _oRecapBillOfMaterials)
             {
                 if (_nRowSpan > nCount)
                 {
                     #region  Row Print
                     _oPdfPCell = new PdfPCell(new Phrase(_oRecapBillOfMaterials.Count > nCount ? _oRecapBillOfMaterials[nCount].ProductName : "", _oFontStyle));
                     _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                     _oPdfPCell = new PdfPCell(new Phrase(_oRecapBillOfMaterials.Count > nCount ? _oRecapBillOfMaterials[nCount].ItemDescription : "", _oFontStyle));
                     _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                     oPdfPTable.CompleteRow();
                     #endregion
                 }
                 nCount++;
             }
             return oPdfPTable;
         }
         private double GetTotalCartonQty()
         {
             double nRetrunvalue = 0;

             foreach (ColorCategory oItem in _oColorCategories)
             {

                 double nColorWiseTotal = GetColorWiseTotalQty(oItem.ColorCategoryID, true, true);
                 if (nColorWiseTotal > 0)
                 {
                     nColorWiseTotal = Math.Ceiling(nColorWiseTotal / _oOrderRecap.QtyPerCarton);
                     nRetrunvalue += nColorWiseTotal;
                 }
             }
             return nRetrunvalue;
         }

        private PdfPTable GetAssortmentSizeColumnName(bool bIsSolidAssortment)
        {
            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 70f, 240f, 50f });
            #region Caption
            _oPdfPCell = new PdfPCell(new Phrase("Color", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Rowspan = 2; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Size", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Rowspan = 2; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            #region size print
            _oPdfPCell = new PdfPCell(GetSizeColumnName(240));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            int nCount = 0; double nColorWiseTotalQty = 0;
            foreach (ColorCategory oItem in _oColorCategories)
            {
                nCount++;

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ColorName, _oFontStyle));
                _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(GetSizeBreakDownValue(oItem.ColorCategoryID, 240, true, bIsSolidAssortment,false));
                _oPdfPCell.Border = 1; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                double nColorWiseTotal = GetColorWiseTotalQty(oItem.ColorCategoryID, true, bIsSolidAssortment);
                if (_oOrderRecap.QtyPerCarton > 0 && bIsSolidAssortment == true && nColorWiseTotal>0)
                {
                    nColorWiseTotal = Math.Ceiling(nColorWiseTotal / _oOrderRecap.QtyPerCarton);
                    nColorWiseTotalQty += nColorWiseTotal;
                }
                _oPdfPCell = new PdfPCell(new Phrase(nColorWiseTotal.ToString() + _oOrderRecapDetails[0].UnitSymbol, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();
            }

            while (nCount<10)
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(GetSizeBreakDownValue(0, 240, true, bIsSolidAssortment,false));
                 _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
                nCount++;
            }

            #region Total Print

            _oPdfPCell = new PdfPCell(new Phrase("Total:", _oFontStyle));
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(GetSizeBreakDownValue(0, 240, true,bIsSolidAssortment, true));
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

             _oPdfPCell = new PdfPCell(new Phrase((_oOrderRecap.QtyPerCarton > 0 && bIsSolidAssortment == true && nColorWiseTotalQty>0)?nColorWiseTotalQty.ToString(): _nTotalAssortmentQty.ToString() +" "+ _oOrderRecapDetails[0].UnitSymbol, _oFontStyle));
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            #endregion
           
            return oPdfPTable;
        }

        private PdfPTable GetSolidColorAssortSizeColumnName() 
        {
            PdfPTable oPdfPTable = new PdfPTable(4);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 70f, 240f, 50f, 40f });
            #region Caption
            _oPdfPCell = new PdfPCell(new Phrase("Color", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Rowspan = 2; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Size", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Rowspan = 2; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Carton", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Rowspan = 2; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            #region size print
            _oPdfPCell = new PdfPCell(GetSizeColumnName(240));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            int nCount = 0; double nCartonQty = 0; double ntotalCartonQty = 0;
            foreach (ColorCategory oItem in _oColorCategories)
            {
                nCount++;
                nCartonQty = 0;//reset
                _oPdfPCell = new PdfPCell(new Phrase(oItem.ColorName, _oFontStyle));
                _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(GetSizeBreakDownValue(oItem.ColorCategoryID, 240, true,false,  false));
                _oPdfPCell.Border = 1; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase(GetColorWiseTotalQty(oItem.ColorCategoryID, true, false).ToString() + _oOrderRecapDetails[0].UnitSymbol, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                double nTotalColorQty = GetColorWiseTotalQty(oItem.ColorCategoryID, false, false);
                double nTotalAssortmentColorQty = GetColorWiseTotalQty(oItem.ColorCategoryID, true, false);
                if(nTotalAssortmentColorQty>0 && nTotalColorQty>0)
                {
                    nCartonQty = Math.Ceiling(nTotalColorQty / nTotalAssortmentColorQty);
                    ntotalCartonQty += nCartonQty;
                }
                _oPdfPCell = new PdfPCell(new Phrase(nCartonQty.ToString() + _oOrderRecapDetails[0].UnitSymbol, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }

            while (nCount < 10)
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(GetSizeBreakDownValue(0, 240, true, false, false));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


                oPdfPTable.CompleteRow();
                nCount++;
            }

            #region Total Print

            _oPdfPCell = new PdfPCell(new Phrase("Total:", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(GetSizeBreakDownValue(0, 240, true,false, true));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_nTotalAssortmentQty.ToString() + " " + _oOrderRecapDetails[0].UnitSymbol, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(ntotalCartonQty.ToString() + " " + _oOrderRecapDetails[0].UnitSymbol, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();


            #endregion
          
            return oPdfPTable;
        }


        private PdfPTable GetAssortColorAssortSizeColumnName()
        {
            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 70f, 280f, 50f });
            #region Caption
            _oPdfPCell = new PdfPCell(new Phrase("Color", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Rowspan = 2; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Size", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Rowspan = 2; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            #region size print
            _oPdfPCell = new PdfPCell(GetSizeColumnName(240));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            int nCount = 0; 
            foreach (ColorCategory oItem in _oColorCategories)
            {
                nCount++;

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ColorName, _oFontStyle));
                _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(GetSizeBreakDownValue(oItem.ColorCategoryID, 240, true, false, false));
                _oPdfPCell.Border = 1; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase(GetColorWiseTotalQty(oItem.ColorCategoryID, true, false).ToString() + _oOrderRecapDetails[0].UnitSymbol, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

              
                oPdfPTable.CompleteRow();
            }

            while (nCount < 10)
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(GetSizeBreakDownValue(0, 240, true, false, false));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();
                nCount++;
            }

            #region Total Print

            _oPdfPCell = new PdfPCell(new Phrase("Total:", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(GetSizeBreakDownValue(0, 240, true, false, true));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_nTotalAssortmentQty.ToString() + " " + _oOrderRecapDetails[0].UnitSymbol, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

             oPdfPTable.CompleteRow();


            #endregion

            return oPdfPTable;
        }

        private PdfPTable GetSizeColumnName(int nWidth)
        {

            PdfPTable oPdfPTable = new PdfPTable(_oSizeCategories.Count);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            float[] sizeLength = new float[_oSizeCategories.Count];
            for (int i = 0; i < _oSizeCategories.Count; i++)
            {
                sizeLength[i] = (nWidth / _oSizeCategories.Count);
            }
            oPdfPTable.SetWidths(sizeLength);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);

            foreach (SizeCategory oItem in _oSizeCategories)
            {

                _oPdfPCell = new PdfPCell(new Phrase(oItem.SizeCategoryName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            }
            oPdfPTable.CompleteRow();
            return oPdfPTable;
        }

        private PdfPTable GetSizeBreakDownValue(int ColorID, int nWidth, bool bIsORAssortment, bool bIsSolidAssortment, bool bIsCalCulateSizeTotal)
        {

            PdfPTable oPdfPTable = new PdfPTable(_oSizeCategories.Count);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            float[] sizeLength = new float[_oSizeCategories.Count];
            for (int i = 0; i < _oSizeCategories.Count; i++)
            {
                sizeLength[i] = (nWidth / _oSizeCategories.Count);
            }
            oPdfPTable.SetWidths(sizeLength);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            if (bIsCalCulateSizeTotal)
            {
                foreach (SizeCategory oItem in _oSizeCategories)
                {
                    double nSizeWiseTotal = GetTotalSizeValue(oItem.SizeCategoryID, bIsORAssortment, bIsSolidAssortment);

                    if (_oOrderRecap.QtyPerCarton > 0 && bIsSolidAssortment == true && nSizeWiseTotal>0)
                    {
                        nSizeWiseTotal =  Math.Ceiling(nSizeWiseTotal / _oOrderRecap.QtyPerCarton);
                    }
                    _oPdfPCell = new PdfPCell(new Phrase(nSizeWiseTotal + " " + _oOrderRecapDetails[0].UnitSymbol, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                }
            }
            else
            {
                foreach (SizeCategory oItem in _oSizeCategories)
                {

                    double nSizeValue =GetSizeValue(ColorID, oItem.SizeCategoryID, bIsORAssortment, bIsSolidAssortment);
                    string sizeValueInSring = "";
                    if (_oOrderRecap.QtyPerCarton > 0 && bIsSolidAssortment == true && nSizeValue > 0)
                    {
                        nSizeValue = Math.Ceiling(nSizeValue / _oOrderRecap.QtyPerCarton);
                    }
                    if(nSizeValue>0)
                    {
                        sizeValueInSring = nSizeValue.ToString();
                    }else
                    {
                        sizeValueInSring = "-";
                    }

                    _oPdfPCell = new PdfPCell(new Phrase((ColorID > 0) ? sizeValueInSring : " ", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                }
            }
           
            oPdfPTable.CompleteRow();
            return oPdfPTable;
        }

        private double GetSizeValue(int ColorID, int SizeCategoryID, bool bIsORAssortment, bool bIsSolidAssortment)
        {
            if (bIsORAssortment == true && bIsSolidAssortment==false)
            {
                foreach (ORAssortment oItem in _oORAssortments)
                {
                    if (oItem.ColorID == ColorID && oItem.SizeID == SizeCategoryID)
                    {
                        return oItem.Qty;
                    }
                }
            }
            else
            {
                foreach (OrderRecapDetail oItem in _oOrderRecapDetails)
                {
                    if (oItem.ColorID == ColorID && oItem.SizeID == SizeCategoryID)
                    {
                        return oItem.Quantity;
                    }
                }
            }

            return 0;
        }

        private double GetTotalSizeValue(int SizeCategoryID, bool bIsORAssortment, bool bIsSolidAssortment)
        {
            double nTotalQty = 0;
            if (bIsORAssortment == true && bIsSolidAssortment==false)
            {
                foreach (ORAssortment oItem in _oORAssortments)
                {
                    if (oItem.SizeID == SizeCategoryID)
                    {
                        nTotalQty += oItem.Qty;
                    }
                }
            }
            else
            {
                foreach (OrderRecapDetail oItem in _oOrderRecapDetails)
                {
                    if ( oItem.SizeID == SizeCategoryID)
                    {
                        nTotalQty += oItem.Quantity;
                    }
                }
            }
          
            return nTotalQty;
        }

        private double GetColorWiseTotalQty(int nColorID, bool bIsORAssortment, bool bIsSolidAssortment)
        {
            double nTotalQty = 0;
            if (bIsORAssortment == true && bIsSolidAssortment==false)
            {
                foreach (ORAssortment oItem in _oORAssortments)
                {
                    if (oItem.ColorID == nColorID)
                    {
                        nTotalQty += oItem.Qty;
                    }
                }
                _nTotalAssortmentQty += nTotalQty;
               
            }
            else
            {
                foreach (OrderRecapDetail oItem in _oOrderRecapDetails)
                {
                    if (oItem.ColorID == nColorID)
                    {
                        nTotalQty += oItem.Quantity;
                    }
                }
            }

            return nTotalQty;
        }

        private string GetComments()
        {
            string sComments = ""; int nCount = 0;
            foreach (ImageComment oItem in _oOrderRecap.ImageComments)
            {
                nCount++;
                sComments = sComments + nCount.ToString() + ") " + oItem.Comments + ".\n";
            }

            return sComments;
        }

        private PdfPTable GetYarnAndSizeTable()
        {
            PdfPTable oPdfPTable = new PdfPTable(1);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            oPdfPTable.SetWidths(new float[] { 580f });

            _oPdfPCell = new PdfPCell(GetYarnTableForFormat3());
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(GetColorSizeTable());
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            return oPdfPTable;
        }

        private PdfPTable GetYarnTableForFormat3()
        {
            PdfPTable oPdfPTable = new PdfPTable(8);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            oPdfPTable.SetWidths(new float[] { 80f, 60f, 80f, 60f, 50f, 70f, 90f, 90f });

            #region Customer Ref
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("CUSTOMER REF", _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("Brand:", _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("STYLE NO:", _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("ORDER NO:", _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("RECAP S.L NO:", _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Colspan = 2; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("REVISED ON: " + _oOrderRecap.DBDate.ToString("dd MMM yyyy"), _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("RESPONSIBLE PERSON", _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            #region  RECAP SL No
            _oPdfPCell = new PdfPCell(new Phrase(_oOrderRecap.BuyerName, _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oOrderRecap.BrandName, _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            
            _oPdfPCell = new PdfPCell(new Phrase(_oOrderRecap.StyleNo, _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oOrderRecap.OrderRecapNo, _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oOrderRecap.SLNo, _oFontStyle));
            _oPdfPCell.BorderWidthLeft = 1f; _oPdfPCell.BorderWidthRight = 1f; _oPdfPCell.BorderWidthBottom = 1f; _oPdfPCell.BorderWidthTop = 1f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Colspan = 2; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("COUNTRY OF ORIGIN:", _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("NAME", _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            #region  FABRICATION/YARN/COUNT/PLY
            _oPdfPCell = new PdfPCell(new Phrase("FABRICATION/YARN/COUNT/PLY", _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f;
            _oPdfPCell.Colspan = 3;  _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        
            _oPdfPCell = new PdfPCell(new Phrase("Collection Name", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Order Type", _oFontStyle)); //Bar Code
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            
            _oPdfPCell = new PdfPCell(new Phrase("BANGLADESH", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Rowspan = 4; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            
            _oPdfPCell = new PdfPCell(new Phrase("01." + _oOrderRecap.ClientOperationSetting.Value, _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthRight = 0f; _oPdfPCell.BorderWidthBottom = 0f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            #region  yarn Print 1
            _oPdfPCell = new PdfPCell(new Phrase(_oOrderRecap.FabricName, _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f;
            _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        
            _oPdfPCell = new PdfPCell(new Phrase(_oOrderRecap.CollectionNo, _oFontStyle));
            _oPdfPCell.Rowspan = 3; _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oOrderRecap.TSType.ToString(), _oFontStyle));
            _oPdfPCell.Colspan = 2; _oPdfPCell.Rowspan = 3; _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("02." + _oOrderRecap.ApproveByName, _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthRight = 0f; _oPdfPCell.BorderWidthBottom = 0f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            #region  yarn Print 2
            _oPdfPCell = new PdfPCell(new Phrase(((_oOrderRecap.OrderRecapYarns.Count > 0) ? _oOrderRecap.OrderRecapYarns[0].YarnName : "") + " " + ((_oOrderRecap.OrderRecapYarns.Count > 0) ? _oOrderRecap.OrderRecapYarns[0].YarnPly : ""), _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f;
            _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                       
            _oPdfPCell = new PdfPCell(new Phrase("03." + _oOrderRecap.MerchandiserName, _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthRight = 0f; _oPdfPCell.BorderWidthBottom = 0f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


            oPdfPTable.CompleteRow();
            #endregion

            #region  yarn Print 3
            _oPdfPCell = new PdfPCell(new Phrase( ((_oOrderRecap.OrderRecapYarns.Count > 1) ? _oOrderRecap.OrderRecapYarns[1].YarnName : "") + " " + ((_oOrderRecap.OrderRecapYarns.Count > 1) ? _oOrderRecap.OrderRecapYarns[1].YarnPly : ""), _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f;
            _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            
            _oPdfPCell = new PdfPCell(new Phrase("04. ", _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthRight = 0f; _oPdfPCell.BorderWidthBottom = 0f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            #region  yarn Print 4
            _oPdfPCell = new PdfPCell(new Phrase(((_oOrderRecap.OrderRecapYarns.Count > 2) ? _oOrderRecap.OrderRecapYarns[2].YarnName : "") + " " + ((_oOrderRecap.OrderRecapYarns.Count > 2) ? _oOrderRecap.OrderRecapYarns[2].YarnPly : ""), _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f;
            _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("DESCRIPTION OF GOODS : " + _oOrderRecap.StyleDescription, _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Colspan = 4; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            #endregion

            #region  Quantity and Kjnitting Machine
            _oPdfPCell = new PdfPCell(new Phrase("QUANTITY: " + _oOrderRecap.TotalQuantity.ToString("0"), _oFontStyle));
            _oPdfPCell.Colspan = 2; _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("KNITTING MACHINE :" + _oOrderRecap.KnittingPatternName, _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Colspan = 2; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("GG/GSM: " + _oOrderRecap.GG, _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("WEIGHT: " + _oOrderRecap.Weight  /*_oOrderRecap.ShipmentDate.Subtract(TimeSpan.FromDays(_oOrderRecap.ClientOperationSetting.OrderSheetShipmentDateCount)).ToString("dd MMM yyyy")*/, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Colspan = 3; _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            #region  Color Size Break Down Caption
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("COLOR & SIZE BREAKDOWN: ", _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Colspan = 8; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            return oPdfPTable;
        }

        private PdfPTable GetColorSizeTable()
        {
            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            oPdfPTable.SetWidths(new float[] { 130f, 360f, 90f });

            #region 1st Row
            _oPdfPCell = new PdfPCell(new Phrase("COLOR:", _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthRight = 0f; _oPdfPCell.BorderWidthBottom = 0f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.Rowspan = 2; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("SIZE", _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("TOTAL", _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.Rowspan = 2; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();

            #endregion

            #region Size Name Print
            _oPdfPCell = new PdfPCell(GetSizeColumnName(360));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion


            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            int nCount = 0;
            foreach (ColorCategory oItem in _oColorCategories)
            {
                nCount++;
                
                _oPdfPCell = new PdfPCell(new Phrase(oItem.ColorName, _oFontStyle));
                _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(GetSizeBreakDownValue(oItem.ColorCategoryID,360,false,false,false));
                _oPdfPCell.Border = 1; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase(GetColorWiseTotalQty(oItem.ColorCategoryID, false, false) + _oOrderRecapDetails[0].UnitSymbol, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }

            #region Blank Rows
            if (nCount < 6)
            {
                for (int i = nCount; i <= 6; i++)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                    _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(GetSizeBreakDownValue(0,360,false,false, false));
                    _oPdfPCell.Border = 1; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    oPdfPTable.CompleteRow();
                }
            }
            #endregion

            #region total Print
            
            _oPdfPCell = new PdfPCell(new Phrase(" Total", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(GetSizeBreakDownValue(0, 360, false,false,  true));
            _oPdfPCell.Border = 1; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oOrderRecap.TotalQuantity.ToString() + " PCS", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
             oPdfPTable.CompleteRow();

            #endregion


            return oPdfPTable;
        }

        private double GetSolidColorSolidSizeCartonQty()
        {
            double ntotalCartonQty = 0, nCartonQty = 0;

            foreach(ColorCategory oItem in _oColorCategories)
            {
                double nTotalColorQty = GetColorWiseTotalQty(oItem.ColorCategoryID, false, false);
                double nTotalAssortmentColorQty = GetColorWiseTotalQty(oItem.ColorCategoryID, true, false);
                if (nTotalAssortmentColorQty > 0 && nTotalColorQty > 0)
                {
                    nCartonQty = Math.Ceiling(nTotalColorQty / nTotalAssortmentColorQty);
                    ntotalCartonQty += nCartonQty;
                }
            }
            return ntotalCartonQty;
        }
      
       
    }
}
