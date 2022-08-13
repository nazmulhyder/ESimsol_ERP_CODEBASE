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
    public class rptPurchaseOrder
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyle_Bold;
        iTextSharp.text.Font _oFontStyle_UnLine;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        //top:20 +botom:20+signature:80
        double nTableHeight = 120;
        MemoryStream _oMemoryStream = new MemoryStream();
        Company _oCompany = new Company();
        PurchaseOrder _oPurchaseOrder = new PurchaseOrder();
        List<PurchaseOrderDetail> _oPurchaseOrderDetails = new List<PurchaseOrderDetail>();
        List<PurchaseOrderDetail> _oPurchaseOrderDetails_Property = new List<PurchaseOrderDetail>();
        Contractor _oContractor = new Contractor();
        Contractor _oDeliveryTo = new Contractor();
        Contractor _oBillTo = new Contractor();
        BusinessUnit _oBusinessUnit = new BusinessUnit();
        ClientOperationSetting _oClientOperationSetting = new ClientOperationSetting();

        List<SignatureSetup> _oSignatureSetups = new List<SignatureSetup>();
        #endregion

        #region PurchaseOrder LC
        public byte[] PrepareReport(PurchaseOrder oPurchaseOrder, Company oCompany, Contractor oContractor, BusinessUnit oBusinessUnit, ClientOperationSetting oClientOperationSetting, List<SignatureSetup> oSignatureSetups)
        {
            _oPurchaseOrder = oPurchaseOrder;
            _oPurchaseOrderDetails = oPurchaseOrder.PurchaseOrderDetails;
            _oBusinessUnit = oBusinessUnit;
            _oContractor=oContractor;
            _oClientOperationSetting = oClientOperationSetting;
            _oCompany = oCompany;
            _oSignatureSetups = oSignatureSetups;

            #region Page Setup
            _oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
            _oDocument.SetMargins(20f, 20f, 20f, 20f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            //_oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 
                                                    595f //Articale
                                              });
            #endregion
         
            this.PrintHeader();
            nTableHeight += CalculatePdfPTableHeight(_oPdfPTable);
            this.PrintBody();
            _oPdfPTable.HeaderRows = 3;
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
                _oImag.ScaleAbsolute(60f, 25f);
                _oPdfPCell = new PdfPCell(_oImag);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            }
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Rowspan = 3; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 18f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Name, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Rowspan = 3; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.PringReportHead, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Colspan = 6; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

            #region ReportHeader
            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
            _oPdfPCell = new PdfPCell(new Phrase("Purchase Order", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.FixedHeight = 20f; _oPdfPCell.Colspan = 6; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        #endregion

        #region Report Body

    
        private void PrintBody()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 9.0f, iTextSharp.text.Font.NORMAL);
            _oFontStyle_Bold = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
             #region PO No,Purchaser, PO Date
            PdfPTable oPdfPTable = new PdfPTable(7);
             oPdfPTable.SetWidths(new float[] {53f,5f, 80f, 100f, 120f, 100f, 97f });

            
             _oPdfPCell = new PdfPCell(new Phrase("PO No     ", _oFontStyle_Bold));
             _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

             _oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle_Bold));
             _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

             _oPdfPCell = new PdfPCell(new Phrase(_oPurchaseOrder.PONo, _oFontStyle));
             _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

             _oPdfPCell = new PdfPCell(new Phrase("Purchaser :", _oFontStyle_Bold));
             _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

             _oPdfPCell = new PdfPCell(new Phrase(_oPurchaseOrder.ConcernPersonName, _oFontStyle));
             _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

             _oPdfPCell = new PdfPCell(new Phrase("PO Date :", _oFontStyle_Bold));
             _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

             _oPdfPCell = new PdfPCell(new Phrase(_oPurchaseOrder.PODateSt, _oFontStyle));
             _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
             oPdfPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(oPdfPTable); _oPdfPCell.Border = 0;
            _oPdfPTable.AddCell(_oPdfPCell); 
            _oPdfPTable.CompleteRow();

            #endregion
            _oContractor = _oContractor.Get(_oPurchaseOrder.ContractorID, 0);
            _oDeliveryTo = _oContractor.Get(_oPurchaseOrder.DeliveryTo, 0);
            _oBillTo = _oContractor.Get(_oPurchaseOrder.BillTo, 0);

            #region supplier
            oPdfPTable = new PdfPTable(3);
            oPdfPTable.SetWidths(new float[] {227f, 128f,200f});

            PdfPTable oPdfTempPTable = new PdfPTable(3);
            oPdfTempPTable.SetWidths(new float[] { 47f,5f, 148f });

            #region Name and address

            _oPdfPCell = new PdfPCell(new Phrase("Supplier ", _oFontStyle_Bold));
            _oPdfPCell.Border= 0;_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfTempPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle_Bold));
            _oPdfPCell.Border = 0;  _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfTempPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 9.0f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oContractor.Name, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfTempPTable.AddCell(_oPdfPCell);
            oPdfTempPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase("          ", _oFontStyle_Bold));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfTempPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle_Bold));
            _oPdfPCell.Border = 0;  _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfTempPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oContractor.Address, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfTempPTable.AddCell(_oPdfPCell);
            oPdfTempPTable.CompleteRow();
            #endregion

            _oPdfPCell = new PdfPCell(oPdfTempPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle_Bold));
            _oPdfPCell.Border = 0;  _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            #region Contact and Tel with Fex
            oPdfTempPTable = new PdfPTable(2);
            oPdfTempPTable.SetWidths(new float[] { 100f, 200f });
            _oFontStyle = FontFactory.GetFont("Tahoma", 9.0f, iTextSharp.text.Font.NORMAL);

            _oPdfPCell = new PdfPCell(new Phrase("Contact ", _oFontStyle_Bold));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfTempPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(": "+_oPurchaseOrder.ContactPersonName, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfTempPTable.AddCell(_oPdfPCell);
            oPdfTempPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Tel ", _oFontStyle_Bold));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfTempPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(": " + _oContractor.Phone, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfTempPTable.AddCell(_oPdfPCell);
            oPdfTempPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Fax ", _oFontStyle_Bold));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfTempPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(": " + _oContractor.Fax, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfTempPTable.AddCell(_oPdfPCell);
            oPdfTempPTable.CompleteRow();
            #endregion

            _oPdfPCell = new PdfPCell(oPdfTempPTable);
            _oPdfPCell.Border = 0;  _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable); _oPdfPCell.Border = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Payment
            oPdfPTable = new PdfPTable(9);
            oPdfPTable.SetWidths(new float[] { 53f, 5f, 128f, 50, 65f, 45f, 70f, 60f, 75f,});

            _oPdfPCell = new PdfPCell(new Phrase("Payment ", _oFontStyle_Bold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle_Bold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oPurchaseOrder.PaymentTermText, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("Currency:", _oFontStyle_Bold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oPurchaseOrder.CurrencyBFDP, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("Ship By:", _oFontStyle_Bold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oPurchaseOrder.ShipBy, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("Trade Term:", _oFontStyle_Bold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oPurchaseOrder.TradeTerm, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPTable.CompleteRow();
            _oPdfPCell = new PdfPCell(oPdfPTable); _oPdfPCell.Border = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Delivery 
            oPdfPTable = new PdfPTable(3);
            oPdfPTable.SetWidths(new float[] { 227f, 128f, 200f });

            oPdfTempPTable = new PdfPTable(3);
            oPdfTempPTable.SetWidths(new float[] { 47f,5f, 148f });
            _oFontStyle = FontFactory.GetFont("Tahoma", 9.0f, iTextSharp.text.Font.NORMAL);
            #region Name and address
            _oPdfPCell = new PdfPCell(new Phrase("Delivery ", _oFontStyle_Bold));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfTempPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle_Bold));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfTempPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oDeliveryTo.Name, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfTempPTable.AddCell(_oPdfPCell);
            oPdfTempPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase("Address ", _oFontStyle_Bold));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfTempPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle_Bold));
            _oPdfPCell.Border = 0;  _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfTempPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oDeliveryTo.Address, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfTempPTable.AddCell(_oPdfPCell);
            oPdfTempPTable.CompleteRow();
            #endregion

            _oPdfPCell = new PdfPCell(oPdfTempPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle_Bold));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            #region Contact and Tel with Fex
            oPdfTempPTable = new PdfPTable(2);
            oPdfTempPTable.SetWidths(new float[] { 100f, 200f });

            _oPdfPCell = new PdfPCell(new Phrase("Contact ", _oFontStyle_Bold));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfTempPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(": "+_oPurchaseOrder.DeliveryToContactPersonName, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfTempPTable.AddCell(_oPdfPCell);
            oPdfTempPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Tel ", _oFontStyle_Bold));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfTempPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(": " + _oDeliveryTo.Phone, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfTempPTable.AddCell(_oPdfPCell);
            oPdfTempPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Fax ", _oFontStyle_Bold));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfTempPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(": " + _oDeliveryTo.Fax, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfTempPTable.AddCell(_oPdfPCell);
            oPdfTempPTable.CompleteRow();
            #endregion

            _oPdfPCell = new PdfPCell(oPdfTempPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            oPdfPTable.CompleteRow();
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0;_oPdfPTable.AddCell(_oPdfPCell); 
            _oPdfPTable.CompleteRow();
            #endregion

            #region Blank Space
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle_Bold));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 15f;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Bill
            oPdfPTable = new PdfPTable(3);
            oPdfPTable.SetWidths(new float[] { 227f, 128f, 200f });

            oPdfTempPTable = new PdfPTable(3);
            oPdfTempPTable.SetWidths(new float[] { 47f,5f, 148f });

            #region Name and address
            _oPdfPCell = new PdfPCell(new Phrase("Bill          ", _oFontStyle_Bold));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfTempPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle_Bold));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfTempPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oBillTo.Name, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfTempPTable.AddCell(_oPdfPCell);
            oPdfTempPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase("Address  ", _oFontStyle_Bold));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfTempPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle_Bold));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfTempPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oBillTo.Address, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfTempPTable.AddCell(_oPdfPCell);
            oPdfTempPTable.CompleteRow();
            #endregion

            _oPdfPCell = new PdfPCell(oPdfTempPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle_Bold));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            #region Contact and Tel with Fex
            oPdfTempPTable = new PdfPTable(2);
            oPdfTempPTable.SetWidths(new float[] { 100f, 200f });

            _oPdfPCell = new PdfPCell(new Phrase("Contact ", _oFontStyle_Bold));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfTempPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(": " + _oPurchaseOrder.BIllToContactPersonName, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfTempPTable.AddCell(_oPdfPCell);
            oPdfTempPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Tel ", _oFontStyle_Bold));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfTempPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(": " + _oBillTo.Phone, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfTempPTable.AddCell(_oPdfPCell);
            oPdfTempPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Fax ", _oFontStyle_Bold));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfTempPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(": " + _oBillTo.Fax, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfTempPTable.AddCell(_oPdfPCell);
            oPdfTempPTable.CompleteRow();
            #endregion

            _oPdfPCell = new PdfPCell(oPdfTempPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            oPdfPTable.CompleteRow();
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0;
            _oPdfPTable.AddCell(_oPdfPCell); 
            _oPdfPTable.CompleteRow();
            #endregion

            #region PO Details
            #region Detail Column Header
            int nTempColumn = 9;
            if (_oClientOperationSetting.Value == "1")//Is  Apply Procurement
            {
                oPdfPTable = new PdfPTable(9);
                oPdfPTable.SetWidths(new float[] { 20f, 125f, 90f, 80f, 80f, 50f, 50f, 60f, 60f });
            }
            else
            {
                nTempColumn = 6;
                oPdfPTable = new PdfPTable(6);
                oPdfPTable.SetWidths(new float[] { 40f, 225f,  80f, 80f, 100f, 100f });
            }
            string sMUName = "";
            if (_oPurchaseOrderDetails.Count > 0)
            {
                sMUName = _oPurchaseOrderDetails[0].UnitName;
            }

            _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle_Bold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
         
            _oPdfPCell = new PdfPCell(new Phrase("DESCRIPTION OF GOODS", _oFontStyle_Bold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            if (_oClientOperationSetting.Value == "1")//Is  Apply Procurement
            {
                _oPdfPCell = new PdfPCell(new Phrase("Buyer Name", _oFontStyle_Bold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Style No", _oFontStyle_Bold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("PO No", _oFontStyle_Bold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            }
            _oPdfPCell = new PdfPCell(new Phrase("UNIT", _oFontStyle_Bold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("QTY", _oFontStyle_Bold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("UNIT PRICE", _oFontStyle_Bold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("AMOUNT("+_oPurchaseOrder.CurrencySymbol+")", _oFontStyle_Bold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPTable.AddCell(_oPdfPCell); 
            _oPdfPTable.CompleteRow();
            #endregion
            sMUName = "";
            #region Details Data
            PurchaseOrderDetail oPurchaseOrderDetail=new PurchaseOrderDetail();

            _oPurchaseOrderDetails = _oPurchaseOrderDetails.OrderBy(a => a.ProductID).ToList();
            int nCount = 0; double nTotalQty = 0; double nTotalValue = 0;        
            foreach (PurchaseOrderDetail oItem in _oPurchaseOrderDetails)
            {
                nTempColumn = 9;
                if (_oClientOperationSetting.Value == "1")//Is  Apply Procurement
                {
                    oPdfPTable = new PdfPTable(9);
                    oPdfPTable.SetWidths(new float[] { 20f, 125f, 90f, 80f, 80f, 50f, 50f, 60f, 60f });
                }
                else
                {
                    nTempColumn = 6;
                    oPdfPTable = new PdfPTable(6);
                    oPdfPTable.SetWidths(new float[] { 40f, 225f, 80f, 80f, 100f, 100f });
                }
                
                    nCount++;
                    _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
     
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName, _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    if (_oClientOperationSetting.Value == "1")//Is  Apply Procurement
                    {
                        _oPdfPCell = new PdfPCell(new Phrase(oItem.BuyerName, _oFontStyle));
                        _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.StyleNo, _oFontStyle));
                        _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.OrderRecapNo, _oFontStyle));
                        _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    }
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.UnitName, _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(oItem.Qty), _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    nTotalQty = nTotalQty + oItem.Qty;

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.UnitPrice.ToString("0.0000"), _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase((oItem.Qty * oItem.UnitPrice).ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft =0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _oPurchaseOrderDetails_Property = new List<PurchaseOrderDetail>();
                    
                nTotalValue = nTotalValue + oItem.Qty * oItem.UnitPrice;
                sMUName = oItem.UnitName;
                oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
         


            int nRequiredRow = 2- (_oPurchaseOrderDetails.Count);
            for (int i = 1; i <= nRequiredRow; i++)
            {
                #region Blank Row
                nTempColumn = 9;
                if (_oClientOperationSetting.Value == "1")//Is  Apply Procurement
                {
                    oPdfPTable = new PdfPTable(9);
                    oPdfPTable.SetWidths(new float[] { 20f, 125f, 90f, 80f, 80f, 50f, 50f, 60f, 60f });
                }
                else
                {
                    nTempColumn = 6;
                    oPdfPTable = new PdfPTable(6);
                    oPdfPTable.SetWidths(new float[] { 40f, 225f, 80f, 80f, 100f, 100f });
                }

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                if (_oClientOperationSetting.Value == "1")//Is  Apply Procurement
                {
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                }
                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                  _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                  _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion
            }

            #region Total Summary
            nTempColumn = 9;
            if (_oClientOperationSetting.Value == "1")//Is  Apply Procurement
            {
                oPdfPTable = new PdfPTable(9);
                oPdfPTable.SetWidths(new float[] { 20f, 125f, 90f, 80f, 80f, 50f, 50f, 60f, 60f });
            }
            else
            {
                nTempColumn = 6;
                oPdfPTable = new PdfPTable(6);
                oPdfPTable.SetWidths(new float[] { 40f, 225f, 80f, 80f, 100f, 100f });
            }

            _oPdfPCell = new PdfPCell(new Phrase("Total : ", _oFontStyle_Bold)); _oPdfPCell.FixedHeight = 15;
            _oPdfPCell.Border = 0; _oPdfPCell.Colspan = nTempColumn-3; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            Chunk oChunk = new Chunk();
            Phrase oPhrase = new Phrase();
            oChunk = new Chunk(nTotalQty.ToString(), _oFontStyle);
            oChunk.SetUnderline(0.7f, -3f);
            oChunk.SetUnderline(0.7f, -5f);
            oPhrase.Add(oChunk);
            _oPdfPCell = new PdfPCell(oPhrase); _oPdfPCell.FixedHeight = 15;
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle_Bold)); _oPdfPCell.FixedHeight = 15;
            _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


            oChunk = new Chunk();
            oPhrase = new Phrase();
            oChunk = new Chunk(_oPurchaseOrder.CurrencySymbol + " " + nTotalValue.ToString("#,##0.00;(#,##0.00)"), _oFontStyle);
            oChunk.SetUnderline(0.7f, -3f);
            oChunk.SetUnderline(0.7f, -5f);
            oPhrase.Add(oChunk);
            _oPdfPCell = new PdfPCell(oPhrase); _oPdfPCell.FixedHeight = 15;
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);    
            oPdfPTable.CompleteRow();

            if (_oPurchaseOrder.PurchaseCosts.Count > 0)
            {
                foreach (PurchaseCost oPC in _oPurchaseOrder.PurchaseCosts)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(oPC.Name + " : ", _oFontStyle_Bold));
                    _oPdfPCell.Border = 0; _oPdfPCell.Colspan = nTempColumn - 3; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle_Bold));
                    _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oPC.ValueInAmount.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    oPdfPTable.CompleteRow();
                }
                nTotalValue = nTotalValue + (_oPurchaseOrder.PurchaseCosts.Where(x => (int)x.CostHeadType == 1).ToList().Sum(x => x.ValueInAmount) - _oPurchaseOrder.PurchaseCosts.Where(x => (int)x.CostHeadType == 2).ToList().Sum(x => x.ValueInAmount));
            }


            _oPdfPCell = new PdfPCell(new Phrase("Total Payable Amount : ", _oFontStyle_Bold));
            _oPdfPCell.Border = 0; _oPdfPCell.Colspan = nTempColumn - 3; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle_Bold));
            _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oChunk = new Chunk();
            oPhrase = new Phrase();
            oChunk = new Chunk(_oPurchaseOrder.CurrencySymbol + " " + nTotalValue.ToString("#,##0.00;(#,##0.00)"), _oFontStyle);
            oChunk.SetUnderline(0.7f, -3f);
            oChunk.SetUnderline(0.7f, -5f);
            oPhrase.Add(oChunk);
            _oPdfPCell = new PdfPCell(oPhrase);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            string sAmountInWords = "";
            if (_oPurchaseOrder.CurrencySymbol == "$") 
            { sAmountInWords = Global.DollarWords(nTotalValue); }
            else if (_oPurchaseOrder.CurrencySymbol == "TK") { sAmountInWords = Global.TakaWords(nTotalValue); }
            else if (_oPurchaseOrder.CurrencySymbol == "EU") { sAmountInWords = Global.EuroWords(nTotalValue); }
            else if (_oPurchaseOrder.CurrencySymbol == "GBP") { sAmountInWords = Global.PoundWords(nTotalValue); } 

            _oPdfPCell = new PdfPCell(new Phrase("Amount In Word : "+sAmountInWords, _oFontStyle_Bold));
            _oPdfPCell.Border = 0; _oPdfPCell.Colspan = nTempColumn; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(oPdfPTable); _oPdfPCell.Border = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion
            #endregion
            #endregion

            #region Terms & Condition

            oPdfPTable = new PdfPTable(1);
            oPdfPTable.SetWidths(new float[] { 300f });
            int nClauseType = 0;
            _oFontStyle_Bold = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle_Bold));
            _oPdfPCell.FixedHeight = 5f; _oPdfPCell.Border = 0; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            _oPdfPCell = new PdfPCell(new Phrase("Terms & Conditions:", FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE)));
            _oPdfPCell.Border = 0; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            nCount = 0;
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
           
            POTandCClause oPOTandCClause = new POTandCClause();
            List<POTandCClause> oPOTandCClauses = new List<POTandCClause>();
            oPOTandCClauses = _oPurchaseOrder.POTandCClauses;            
            oPOTandCClauses = oPOTandCClauses.OrderBy(a => a.POTandCClauseID).ToList();
            oPOTandCClauses = oPOTandCClauses.OrderBy(a => a.ClauseType).ToList();
          
            /// From PO TnC Setup terms
            nCount = 0;
            foreach (POTandCClause oItem in oPOTandCClauses)
            {
                 nCount++;
               _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString()+". "+oItem.TermsAndCondition, _oFontStyle));
               _oPdfPCell.Border = 0; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
                nClauseType = oItem.ClauseType;
            }
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            
            #endregion

            nTableHeight += CalculatePdfPTableHeight(_oPdfPTable);
            #region Blank Space
            if ((float)(842 - nTableHeight) > 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle_Bold));
                _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = (float)(842 - nTableHeight);
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion




            #region Signature

            oPdfPTable = new PdfPTable(2);
            oPdfPTable.SetWidths(new float[] {150f, 150f});

            _oPdfPCell = new PdfPCell(new Phrase("FOR AND ON BEHALF OF", _oFontStyle_Bold));
            _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("ACCEPTED AND CONFIRMED BY", _oFontStyle_Bold));
            _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase(_oPurchaseOrder.BillToName, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oPurchaseOrder.ContractorName, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Purchaser :", _oFontStyle_Bold));
            _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("In charge of Supplier(Processer):", _oFontStyle_Bold));
            _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle_Bold));
            _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.FixedHeight = 30f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase("In Charge :", _oFontStyle_Bold));
            _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Date:", _oFontStyle_Bold));
            _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(oPdfPTable); _oPdfPCell.Border = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }

        #endregion
        #endregion

        public static float CalculatePdfPTableHeight(PdfPTable table)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (Document doc = new Document(PageSize.TABLOID))
                {
                    using (PdfWriter w = PdfWriter.GetInstance(doc, ms))
                    {
                        doc.Open();
                        table.TotalWidth = 555f;
                        table.WriteSelectedRows(0, table.Rows.Count, 0, 0, w.DirectContent);

                        doc.Close();
                        return table.TotalHeight;
                    }
                }
            }
        }
    }
}
