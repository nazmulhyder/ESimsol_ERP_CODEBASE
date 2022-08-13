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
    public class rptPreInvoice
    {
        #region Font Declaration
        Document _oDocument;
        iTextSharp.text.Image _oImag;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyleBangla;
        iTextSharp.text.Font _oFontStyleCompanyTitle;
        iTextSharp.text.Font _oFontStyleToFrom;
        iTextSharp.text.Font _oFontStyleToFromDetails;
        iTextSharp.text.Font _oFontStyleLetterBody;
        iTextSharp.text.Font _oFontStyleCompanyDetails;
        iTextSharp.text.Font _oFontStyleHeaderTitle;
        iTextSharp.text.Font _oFontStylePageNo;
        iTextSharp.text.Font _oFontStyleRowHeader;
        iTextSharp.text.Font _oFontStyleCondition;
        iTextSharp.text.Font _oFontStyleImageFooter;

        #endregion

        #region Declaration
        PdfPTable _oPdfPTable = new PdfPTable(3);
        PdfPCell _oPdfPCell;
        Phrase _oPhrase;
        MemoryStream _oMemoryStream = new MemoryStream();
        Company _oCompany = new Company();

        double _dOptionItemTotalAmmount = 0;
        float _fToWidth = 25f;
        float _fBodyWidth = 430f;
        float _fCompanyWidth = 130f;

        PdfWriter _oPDFWriter;
        SalesQuotation _oSalesQuotation = new SalesQuotation();

        List<ModelFeature> _oModelFeatures = new List<ModelFeature>();
        List<VehicleOrderImage> _oVehicleOrderImages = new List<VehicleOrderImage>();
        List<SalesQuotationImage> _oSalesQuotationImages = new List<SalesQuotationImage>();
        List<PreInvoice> _oPreInvoices = new List<PreInvoice>();
        PreInvoice _oPreInvoice = new PreInvoice();
        #endregion

        public List<Object> PrepareReport(Company oCompany, PreInvoice oPreInvoice, List<ModelFeature> oModelFeatures)
        {
            _oPreInvoice = oPreInvoice;
            _oCompany = oCompany;
            _oSalesQuotation = oPreInvoice.SalesQuotation;
            _oModelFeatures = oModelFeatures;
            
            //SetFontStyle For CustomFonts
            this.SetFontStyle();

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
            _oDocument.SetMargins(45f, 33f, 55f, 60f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

            _oFontStyle = _oFontStyleToFromDetails; //FontFactory.GetFont("Audi Type", 7f, 1);
            //PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            //_oPDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PdfWriter PdfWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            //rptSaleQuotationFooter PageEventHandler = new rptSaleQuotationFooter(_oSalesQuotation.RefNo);
            //_oPDFWriter.PageEvent = PageEventHandler;

            ESimSolFooter PageEventHandler = new ESimSolFooter("Date: _________________________________             Customer Signature: _________________________________");
            PageEventHandler.nFontSize = 6; PageEventHandler.PrintDocumentGenerator = false; PageEventHandler.PrintPrintingDateTime = false;
            PdfWriter.PageEvent = PageEventHandler;

            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { _fToWidth, // To
                                                _fBodyWidth, //body, 
                                                _fCompanyWidth //Company
                                                });
            # endregion

            this.PrintPages();
            _oDocument.Close();

            List<Object> oObjects = new List<Object>();
            Object aa = _oMemoryStream.ToArray();
            Object bb = _oDocument.PageNumber - 1;
            oObjects.Add(aa);
            oObjects.Add(bb);
            return oObjects;
        }

        private void PrintPages()
        {
            this.PrintHeader();
            //this.PrintCompanyHeader2();
            this.PrintLetterPage();
            this.NewPageDeclaration();
            _oPdfPTable.HeaderRows = 2;
            
            //ESimSolPdfHelper.PrintHeader_Audi(ref _oPdfPTable, new BusinessUnit(), _oCompany, new string[] { "Spare Parts Request Form", "Audi Service Dhaka", "Audi", "429/432, Tejgaon I/A, Dhaka-1208", "Service" }, 3);
            this.PrintCompanyHeader2();
            this.PrintOfferPage();
            //this.NewPageDeclaration();
            //_oPdfPTable.HeaderRows = 3;

            //this.PrintCompanyHeader2();
            this.PrintFeaturePage();
            this.NewPageDeclaration();
            _oPdfPTable.HeaderRows = 4;

            //this.PrintExteriorImage();
            //this.NewPageDeclaration();

            //this.PrintInteriorImage();
            //this.NewPageDeclaration();
            //this.PrintHeader();
            PrintPriceAndSalesTerm();
            this.NewPageDeclaration();
            _oPdfPTable.HeaderRows = 2;

            _oPdfPCell = new PdfPCell(GetConfirmationTable()); _oPdfPCell.Colspan = 3; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; //_oPdfPCell.FixedHeight = 150;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(footer()); _oPdfPCell.Colspan = 3; _oPdfPCell.Border = 0;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; //_oPdfPCell.FixedHeight = 150;
            //_oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);            
            this.NewPageDeclaration();
            _oPdfPTable.HeaderRows = 2;
        }
        public void NewPageDeclaration()
        {
            #region New Page Declare
            _oDocument.Add(_oPdfPTable);
            _oDocument.NewPage();
            _oPdfPTable.DeleteBodyRows();
            #endregion
        }
        public void SetFontStyle()
        {
            _oFontStyleCompanyTitle = this.GetCustomFont("AudiType-ExtendedBold", 12);//SolaimanLipi_29-05-06
            _oFontStyleBangla = this.GetCustomFontBangla("SolaimanLipi_29-05-06", 12);//
            _oFontStyleToFrom = this.GetCustomFont("AudiType-Normal", 7f); //FontFactory.GetFont("Audi Type", 7f, iTextSharp.text.Font.NORMAL);
            _oFontStyleToFromDetails = this.GetCustomFont("AudiType-Normal", 7f);// FontFactory.GetFont("Audi Type", 7f, iTextSharp.text.Font.NORMAL);
            _oFontStyleLetterBody = this.GetCustomFont("AudiType-Normal", 9f);// FontFactory.GetFont("Audi Type", 9f, iTextSharp.text.Font.NORMAL);
            _oFontStyleCompanyDetails = this.GetCustomFont("AudiType-Normal", 7f);// FontFactory.GetFont("Audi Type", 7f, iTextSharp.text.Font.NORMAL);
            _oFontStyleHeaderTitle = this.GetCustomFont("AudiType-Bold", 10f);// FontFactory.GetFont("Audi Type", 10f, iTextSharp.text.Font.BOLD);
            _oFontStylePageNo = this.GetCustomFont("AudiType-Normal", 7f);// FontFactory.GetFont("Audi Type", 7f, 2);
            _oFontStyleRowHeader = this.GetCustomFont("AudiType-Bold", 10f);// FontFactory.GetFont("Audi Type", 9f, iTextSharp.text.Font.BOLD);
            _oFontStyleCondition = this.GetCustomFont("AudiType-Bold", 7f);
            _oFontStyleImageFooter = this.GetCustomFont("AudiType-Bold", 7.5f);
        }

        #region Print Functions
        private void PrintHeader()
        {
            PrintCompanyHeader();
        }

        private PdfPTable GetTopHeaderTable()
        {
            PdfPTable oTopHeaderTable = new PdfPTable(1);
            oTopHeaderTable.WidthPercentage = 100;
            oTopHeaderTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oTopHeaderTable.SetWidths(new float[] { 100f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);

            Phrase oPhrase = new Phrase();
            Chunk oChunk1 = new Chunk("", _oFontStyle); //Client's Information: 
            _oFontStyle = FontFactory.GetFont("Tahoma", 11f, 0);
            Chunk oChunk2 = new Chunk(_oPreInvoice.CustomerName, _oFontStyle);
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            oPhrase.Add(oChunk1);
            oPhrase.Add(oChunk2);
            _oPdfPCell = new PdfPCell(oPhrase);
            //_oPdfPCell = new PdfPCell(new Phrase("Customer Name: " + _oPreInvoice.CustomerName, _oFontStyle)); 
            _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopHeaderTable.AddCell(_oPdfPCell);
            oTopHeaderTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oPreInvoice.CustomerAddress, _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopHeaderTable.AddCell(_oPdfPCell);
            oTopHeaderTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oPreInvoice.CustomerLandline, _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopHeaderTable.AddCell(_oPdfPCell);
            oTopHeaderTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oPreInvoice.CustomerEmail, _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopHeaderTable.AddCell(_oPdfPCell);
            oTopHeaderTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopHeaderTable.AddCell(_oPdfPCell);
            oTopHeaderTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopHeaderTable.AddCell(_oPdfPCell);
            oTopHeaderTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopHeaderTable.AddCell(_oPdfPCell);
            oTopHeaderTable.CompleteRow();

            
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;_oPdfPCell.FixedHeight = 30;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTopHeaderTable.AddCell(_oPdfPCell);
            oTopHeaderTable.CompleteRow();


            return oTopHeaderTable;
        }
        
        #endregion

        private PdfPTable footer()
        {
            #region Customer Signature

            PdfPTable oPdfPTable = new PdfPTable(2); //460f
            oPdfPTable.WidthPercentage = 100; oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            oPdfPTable.SetWidths(new float[] { _fBodyWidth / 2, _fBodyWidth / 2 });

            _oFontStyle = _oFontStyleLetterBody; ///this.GetCustomFont("ARIALUNI", 10);//

            _oPhrase = new Phrase();
            _oPhrase.Add(new Chunk("Date: ", _oFontStyle));
            _oPhrase.Add(new Chunk("__________________________", _oFontStyle)); //মুক্তি নাই
            _oPdfPCell = new PdfPCell(_oPhrase); _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Customer Signature: ______________________", _oFontStyle)); _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell); oPdfPTable.CompleteRow();

            return oPdfPTable;

            //_oPdfPCell = new PdfPCell();
            //_oPdfPCell.Border = 0; _oPdfPCell.Colspan = 3;
            //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.AddElement(oPdfPTable);
            //_oPdfPTable.AddCell(_oPdfPCell);
            //PrintEmptyCell();

            //_oPdfPTable.CompleteRow();
            #endregion
        }

        private PdfPTable GetConfirmationTable()
        {
            PdfPTable oConfirmationTable = new PdfPTable(2);
            oConfirmationTable.WidthPercentage = 100;
            oConfirmationTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oConfirmationTable.SetWidths(new float[] {               40f,                                                  
                                                                60f
                                                          });

            #region Order confirmation

            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.FixedHeight = 20; _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oConfirmationTable.AddCell(_oPdfPCell);

            oConfirmationTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("ORDER CONFIRMATION", _oFontStyle)); _oPdfPCell.Colspan = 2; _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oConfirmationTable.AddCell(_oPdfPCell);

            oConfirmationTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.FixedHeight = 20; _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oConfirmationTable.AddCell(_oPdfPCell);

            oConfirmationTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL);
            Phrase oPhrase = new Phrase();
            Chunk oChunk2 = new Chunk("We, the bellow undersigned here with confirm the above offer, and would like to place irrevocably an order as per the order ", _oFontStyle);
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            Chunk oChunk3 = new Chunk("Quotation Reference AD" + _oSalesQuotation.ModelShortName + "KOMM" + _oSalesQuotation.KommNo + _oPreInvoice.CustomerShortName, _oFontStyle);
            oPhrase.Add(oChunk2);
            oPhrase.Add(oChunk3);
            _oPdfPCell = new PdfPCell(oPhrase); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oConfirmationTable.AddCell(_oPdfPCell);

            oConfirmationTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.FixedHeight = 30; _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oConfirmationTable.AddCell(_oPdfPCell);

            oConfirmationTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Customer Name", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oConfirmationTable.AddCell(_oPdfPCell);

            //iTextSharp.text.Font.UNDERLINE
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL);

            _oPdfPCell = new PdfPCell(new Phrase(_oPreInvoice.CustomerName, _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 1;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oConfirmationTable.AddCell(_oPdfPCell);

            oConfirmationTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.FixedHeight = 20; _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oConfirmationTable.AddCell(_oPdfPCell);

            oConfirmationTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL);

            _oPdfPCell = new PdfPCell(new Phrase("Authorized Signatory", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oConfirmationTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 1;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oConfirmationTable.AddCell(_oPdfPCell);

            oConfirmationTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.FixedHeight = 20; _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oConfirmationTable.AddCell(_oPdfPCell);

            oConfirmationTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Date/Place", _oFontStyle));  _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oConfirmationTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL);

            _oPdfPCell = new PdfPCell(new Phrase( _oPreInvoice.InvoiceDateST, _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 1;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oConfirmationTable.AddCell(_oPdfPCell);

            oConfirmationTable.CompleteRow();


            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.FixedHeight = 15; _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oConfirmationTable.AddCell(_oPdfPCell);

            oConfirmationTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Sales Advisor", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oConfirmationTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 1;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oConfirmationTable.AddCell(_oPdfPCell);

            oConfirmationTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oConfirmationTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oSalesQuotation.MarketingAccountName, _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oConfirmationTable.AddCell(_oPdfPCell);

            oConfirmationTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.FixedHeight = 15; _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oConfirmationTable.AddCell(_oPdfPCell);

            oConfirmationTable.CompleteRow();
            
            //_oPdfPCell = new PdfPCell(new Phrase("Enclosed Cheque No", _oFontStyle));  _oPdfPCell.Border = 0;
            //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oConfirmationTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 1;
            //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oConfirmationTable.AddCell(_oPdfPCell);

            //oConfirmationTable.CompleteRow();

            //_oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);

            //_oPdfPCell = new PdfPCell(new Phrase("(" + ((_oPreInvoice.AdvanceAmount / _oPreInvoice.OTRAmount) * 100).ToString("###,0.") + "% Advance payment)", _oFontStyle)); _oPdfPCell.Border = 0;
            //_oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oConfirmationTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
            //_oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oConfirmationTable.AddCell(_oPdfPCell);

            //oConfirmationTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2; _oPdfPCell.FixedHeight = 30;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oConfirmationTable.AddCell(_oPdfPCell);

            oConfirmationTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);

            _oPdfPCell = new PdfPCell(new Phrase("Payment Info:", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2; _oPdfPCell.FixedHeight = 30;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oConfirmationTable.AddCell(_oPdfPCell);

            oConfirmationTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL);

          

            if (_oPreInvoice.AdvanceAmount <= 0)
            {
              

                _oPdfPCell = new PdfPCell(new Phrase("1. " + _oPreInvoice.FirstTerm, _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oConfirmationTable.AddCell(_oPdfPCell);
                oConfirmationTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase("2. " + _oPreInvoice.SecondTerm, _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oConfirmationTable.AddCell(_oPdfPCell);

                oConfirmationTable.CompleteRow();
            }
            else 
            {
                _oPdfPCell = new PdfPCell(new Phrase("1. " + ((_oPreInvoice.AdvanceAmount / _oPreInvoice.OTRAmount) * 100).ToString("###,0.") + "% advance payment (i.e. BDT " + _oPreInvoice.AdvanceAmount.ToString("###,0.00") + " ) to be made at the time of booking and order confirmation", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oConfirmationTable.AddCell(_oPdfPCell);
                oConfirmationTable.CompleteRow();

                oPhrase = new Phrase();
                Chunk oChunk7 = new Chunk("2. " + ((_oPreInvoice.AdvanceAmount / _oPreInvoice.OTRAmount) * 100).ToString("###,0") + "% remaining balance payment (i.e. BDT" + _oPreInvoice.AdvanceAmount.ToString("###,0.00") + ") shall be made before registration. ", _oFontStyle);
                _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL | iTextSharp.text.Font.UNDEFINED);
                Chunk oChunk8 = new Chunk("(Only pay orders\n    will be accepted)", _oFontStyle);
                oPhrase.Add(oChunk7);
                oPhrase.Add(oChunk8);

                _oPdfPCell = new PdfPCell(oPhrase); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oConfirmationTable.AddCell(_oPdfPCell);

                oConfirmationTable.CompleteRow();
            }
           
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2; _oPdfPCell.FixedHeight = 40;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oConfirmationTable.AddCell(_oPdfPCell);

            oConfirmationTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            oPhrase = new Phrase();
            Chunk oChunk4 = new Chunk("Note: ", _oFontStyle);
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL);
            Chunk oChunk5 = new Chunk("Please note that payment has to be made in Bangladeshi Taka in favor of ", _oFontStyle);
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            Chunk oChunk6 = new Chunk(" Progress Motors Imports Limited.", _oFontStyle);
            oPhrase.Add(oChunk4);
            oPhrase.Add(oChunk5);
            oPhrase.Add(oChunk6);
            _oPdfPCell = new PdfPCell(oPhrase); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oConfirmationTable.AddCell(_oPdfPCell);

            oConfirmationTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2; _oPdfPCell.FixedHeight = 30;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oConfirmationTable.AddCell(_oPdfPCell);

            oConfirmationTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("The Buyer Commits himself to secrecy to a third part regarding prices and terms of delivery. In case of failure to comply with the condition AUDI AG is free to stop the direct delivery at special rates or is free to cancel the contract. M/S Progress Motors Imports Limited reserves the right increase the agreed price due to devaluation of BDT/increase or addtion in Government Duties/Taxes/Surcharges. The Customer hereby agrees to pay the addtional amount against revised performa invoice/offer.", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oConfirmationTable.AddCell(_oPdfPCell);

            oConfirmationTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2; _oPdfPCell.FixedHeight = 40;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oConfirmationTable.AddCell(_oPdfPCell);

            oConfirmationTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Delivery Address of Vehicle: ", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oConfirmationTable.AddCell(_oPdfPCell);

            oConfirmationTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2; _oPdfPCell.FixedHeight = 15;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oConfirmationTable.AddCell(_oPdfPCell);

            oConfirmationTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL);

            _oPdfPCell = new PdfPCell(new Phrase("Audi Bangladesh", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2; 
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oConfirmationTable.AddCell(_oPdfPCell);

            oConfirmationTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2; _oPdfPCell.FixedHeight = 40;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oConfirmationTable.AddCell(_oPdfPCell);

            oConfirmationTable.CompleteRow();

            #endregion

            return oConfirmationTable;
           

        }


        public void PrintLetterPage()
        {

            #region To & EMAIL With Logo & Details

            #region To
            _oFontStyle = _oFontStyleToFrom;
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 60f;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);

            #region Details
            PdfPTable oPdfPTable = new PdfPTable(2);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

            oPdfPTable.SetWidths(new float[] { _fBodyWidth-160f, // Code No
                                               160f });
            #region To Whom
            PdfPTable oPdfPTableToWhom = new PdfPTable(1);
            oPdfPTableToWhom.WidthPercentage = 100;
            oPdfPTableToWhom.WidthPercentage = 100;
            oPdfPTableToWhom.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTableToWhom.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

     
            #endregion

            #endregion

            _oPdfPTable.CompleteRow();

            #endregion


            #endregion

            #region Letter Body
            _oPdfPCell = new PdfPCell(); //ToFromColumn
            _oPdfPCell.Border = 0;
            _oPdfPTable.AddCell(_oPdfPCell);

            #region Body
            oPdfPTable = new PdfPTable(1);
            oPdfPTable.WidthPercentage = 90;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

            _oPdfPCell = new PdfPCell(GetTopHeaderTable()); _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell); oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("")); _oPdfPCell.FixedHeight = 35f;
            _oPdfPCell.Border = 0; oPdfPTable.AddCell(_oPdfPCell); oPdfPTable.CompleteRow();

            _oFontStyle = _oFontStyleLetterBody;

            _oPdfPCell = new PdfPCell(new Phrase(_oPreInvoice.ModelNo + " " + _oPreInvoice.ExteriorColorName + " ", _oFontStyleRowHeader)); _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell); oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell); oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Dear Sir,", _oFontStyle)); _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell); oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.FixedHeight = 15f;
            _oPdfPCell.Border = 0; oPdfPTable.AddCell(_oPdfPCell); oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Please find enclosed in the following pages the full and final offer towards", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell); oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase(_oPreInvoice.ModelNo + " " + _oPreInvoice.ExteriorColorName + ".", _oFontStyle)); _oPdfPCell.FixedHeight = 15f;
            _oPdfPCell.Border = 0; oPdfPTable.AddCell(_oPdfPCell); oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.FixedHeight = 15f;
            _oPdfPCell.Border = 0; oPdfPTable.AddCell(_oPdfPCell); oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Incase you need any further information, please feel free to constact us accordingly.", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0; _oPdfPCell.Rowspan = 2; _oPdfPCell.FixedHeight = 25f;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell); oPdfPTable.CompleteRow();

            //this.AddParagraphRow("", "Please find the attached file for the amended pro forma of the 2017 Audi Q7 2.0 TFSI (252 Bhp) with the revised price.");

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.FixedHeight = 25f;
            _oPdfPCell.Border = 0; oPdfPTable.AddCell(_oPdfPCell); oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell); oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell); oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("With best regards,", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell); oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Progress Motors Imports Limited", _oFontStyleRowHeader));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 40;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell); oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell); oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell); oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase(_oSalesQuotation.MarketingAccountName, _oFontStyleRowHeader)); _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell); oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Sales Advisor", _oFontStyle)); _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell); oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Audi Dhaka", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell); oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell();
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.AddElement(oPdfPTable);
            _oPdfPTable.AddCell(_oPdfPCell);
            #endregion

            #region CompanyDetails
            oPdfPTable = new PdfPTable(1);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

           // _oFontStyle = _oFontStyleCompanyDetails;
            _oFontStyle = _oFontStyleCondition;//bold
            _oPdfPCell = new PdfPCell(new Phrase("AD" + _oSalesQuotation.ModelShortName + "KOMM" + _oSalesQuotation.KommNo + _oPreInvoice.CustomerShortName, _oFontStyle)); 
            _oPdfPCell.Border = 0; oPdfPTable.AddCell(_oPdfPCell); oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase(_oSalesQuotation.ModelNo, _oFontStyle));
            _oPdfPCell.Border = 0; oPdfPTable.AddCell(_oPdfPCell); oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase(_oPreInvoice.Phone, _oFontStyle));
            _oPdfPCell.Border = 0; oPdfPTable.AddCell(_oPdfPCell); oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase(_oPreInvoice.Email, _oFontStyle));
            _oPdfPCell.Border = 0; oPdfPTable.AddCell(_oPdfPCell); oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase(_oPreInvoice.InvoiceDateST, _oFontStyle));
            _oPdfPCell.Border = 0; oPdfPTable.AddCell(_oPdfPCell); oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; oPdfPTable.AddCell(_oPdfPCell); oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; oPdfPTable.AddCell(_oPdfPCell); oPdfPTable.CompleteRow();

            #region 1stPart
            _oFontStyle = _oFontStyleCondition;//bold
            _oPdfPCell = new PdfPCell(new Phrase("Progress Motors Imports Limited", _oFontStyle)); _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell); oPdfPTable.CompleteRow();

            _oFontStyle = _oFontStyleCompanyDetails;
            _oPdfPCell = new PdfPCell(new Phrase("225, Tejgaon C/A", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell); oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Dhaka 1208, Bangladesh", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell); oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Phone +880 2 9899872", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell); oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("info@pmilbd.com", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell); oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("www.audi.com.bd", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell); oPdfPTable.CompleteRow();
            #endregion

            _oPdfPCell = new PdfPCell(new Phrase("")); _oPdfPCell.FixedHeight = 10f;
            _oPdfPCell.Border = 0; oPdfPTable.AddCell(_oPdfPCell); oPdfPTable.CompleteRow();

            #region 2ndstPart
            _oFontStyle = _oFontStyleCondition;//bold
            _oPdfPCell = new PdfPCell(new Phrase("Audi Dhaka", _oFontStyle)); _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell); oPdfPTable.CompleteRow();

            _oFontStyle = _oFontStyleCompanyDetails;
            _oPdfPCell = new PdfPCell(new Phrase("242 Gulshan Tejgaon Link Road", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell); oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Dhaka 1208, Bangladesh", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell); oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Phone +88 02 8878056", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell); oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Hotline +88 019 SHOP AUDI", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell); oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("customer.service@pmilbd.com", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell); oPdfPTable.CompleteRow();
            #endregion

            _oPdfPCell = new PdfPCell(new Phrase("")); _oPdfPCell.FixedHeight = 10f;
            _oPdfPCell.Border = 0; oPdfPTable.AddCell(_oPdfPCell); oPdfPTable.CompleteRow();

            #region 3rdstPart
            _oFontStyle = _oFontStyleCondition;//bold
            _oPdfPCell = new PdfPCell(new Phrase("Audi Service", _oFontStyle)); _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell); oPdfPTable.CompleteRow();

            _oFontStyle = _oFontStyleCompanyDetails;
            _oPdfPCell = new PdfPCell(new Phrase("429/432 , Tejgaon Industrial", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell); oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Area, Dhaka 1208, Bangladesh", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell); oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Phone +88 02 8891243", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell); oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Hotline +88 019 CALL AUDI", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell); oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("service@pmilbd.com", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell); oPdfPTable.CompleteRow();
            #endregion

            _oPdfPCell = new PdfPCell();
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.AddElement(oPdfPTable);
            _oPdfPTable.AddCell(_oPdfPCell);
            #endregion

            #endregion
            _oPdfPTable.CompleteRow();
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL);
        }

        public void PrintOfferPage()
        {
          

            PrintEmptyRow(50);

            #region HEADER
            _oPdfPCell = new PdfPCell(); //ToFromColumn
            _oPdfPCell.Border = 0;
            _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("TECHNICAL DETAILS", this.GetCustomFont("AudiType-Bold", 10f, true)));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();
            #endregion

            _oPdfPCell = new PdfPCell(); //ToFromColumn
            _oPdfPCell.Border = 0;
            _oPdfPTable.AddCell(_oPdfPCell);

            #region Body
            PdfPTable oPdfPTable = new PdfPTable(2);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.SetWidths(new float[] { 90f, // Caption
                                               455f //value, 
                                                });
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

            _oPdfPCell = new PdfPCell(new Phrase("")); _oPdfPCell.FixedHeight = 10f;
            _oPdfPCell.Border = 0; oPdfPTable.AddCell(_oPdfPCell); oPdfPTable.CompleteRow();

            #region Model
            PrintEmptyRow(20);
            oPdfPTable = AddColumn(oPdfPTable, "Model", "", false); oPdfPTable = AddColumn(oPdfPTable, "", ": " + _oSalesQuotation.ModelNo, true);
            oPdfPTable = AddColumn(oPdfPTable, "Model Year", "", false); oPdfPTable = AddColumn(oPdfPTable, "", ": " + _oSalesQuotation.YearOfModel, true);
            oPdfPTable = AddColumn(oPdfPTable, "Manufacture Year", "", false); oPdfPTable = AddColumn(oPdfPTable, "", ": " + _oSalesQuotation.YearOfManufacture, true);
            oPdfPTable = AddColumn(oPdfPTable, "Code", "", false); oPdfPTable = AddColumn(oPdfPTable, "", ": " + _oSalesQuotation.ModelCode, true);
            oPdfPTable = AddColumn(oPdfPTable, "Engine Type", "", false); oPdfPTable = AddColumn(oPdfPTable, "", ": " + _oSalesQuotation.EngineType, true);
            oPdfPTable = AddColumn(oPdfPTable, "Displacement", "", false); oPdfPTable = AddColumn(oPdfPTable, "", ": " + _oSalesQuotation.DisplacementCC, true);
            oPdfPTable = AddColumn(oPdfPTable, "Max Output", "", false); oPdfPTable = AddColumn(oPdfPTable, "", ": " + _oSalesQuotation.MaxPowerOutput, true);
            oPdfPTable = AddColumn(oPdfPTable, "Max Torque", "", false); oPdfPTable = AddColumn(oPdfPTable, "", ": " + _oSalesQuotation.MaximumTorque, true);
            oPdfPTable = AddColumn(oPdfPTable, "Transmission", "", false); oPdfPTable = AddColumn(oPdfPTable, "", ": " + _oSalesQuotation.Transmission, true);
            oPdfPTable = AddColumn(oPdfPTable, "Acceleration", "", false); oPdfPTable = AddColumn(oPdfPTable, "", ": " + _oSalesQuotation.Acceleration, true);
            oPdfPTable = AddColumn(oPdfPTable, "Type of drive", "", false); oPdfPTable = AddColumn(oPdfPTable, "", ": " + _oSalesQuotation.DriveTypeInString, true);
            oPdfPTable = AddColumn(oPdfPTable, "Top Speed", "", false); oPdfPTable = AddColumn(oPdfPTable, "", ": " + _oSalesQuotation.TopSpeed, true);
            #endregion

            PrintEmptyCell();
            _oPdfPCell = new PdfPCell();
            _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.AddElement(oPdfPTable);
            _oPdfPTable.AddCell(_oPdfPCell);

            #endregion

            _oPdfPCell = new PdfPCell(); //ToFromColumn
            _oPdfPCell.Border = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
        }
        public void PrintFeaturePage()
        {
            #region PRICE CALCULATION

            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(); //ToFromColumn
            _oPdfPCell.Border = 0;
            _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", this.GetCustomFont("AudiType-Bold", 8.5f, true)));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.FixedHeight = 10;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.Colspan = 2;
            _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

     

            this.NewPageDeclaration();
            _oPdfPTable.HeaderRows = 2;

            //PrintFeatureName("Order Quoatation Reference: AD" + _oSalesQuotation.ModelShortName + "KOMM" + _oSalesQuotation.KommNo + _oPreInvoice.CustomerShortName, this.GetCustomFont("AudiType-Bold", 9f));

            PdfPTable oPdfPTable2 = new PdfPTable(2);
            oPdfPTable2.WidthPercentage = 100;
            oPdfPTable2.WidthPercentage = 100;
            oPdfPTable2.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable2.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            oPdfPTable2.SetWidths(new float[] { 105f, _fBodyWidth - 105f });

            ESimSolPdfHelper.FontStyle = this.GetCustomFont("AudiType-Bold", 8.5f);
            ESimSolPdfHelper.AddCell(ref oPdfPTable2, "Order Quotation Reference:", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
            ESimSolPdfHelper.AddCell(ref oPdfPTable2, "AD" + _oSalesQuotation.ModelShortName + "KOMM" + _oSalesQuotation.KommNo + _oPreInvoice.CustomerShortName, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);

            ESimSolPdfHelper.FontStyle = this.GetCustomFont("AudiType-Normal", 9f);
            ESimSolPdfHelper.AddCell(ref oPdfPTable2, "Model", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
            ESimSolPdfHelper.AddCell(ref oPdfPTable2, _oSalesQuotation.ModelNo, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
            ESimSolPdfHelper.AddCell(ref oPdfPTable2, "Code", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
            ESimSolPdfHelper.AddCell(ref oPdfPTable2, _oSalesQuotation.ModelCode, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
            ESimSolPdfHelper.AddCell(ref oPdfPTable2, "Chassis No", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
            ESimSolPdfHelper.AddCell(ref oPdfPTable2, _oSalesQuotation.ChassisNo, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
            ESimSolPdfHelper.AddCell(ref oPdfPTable2, "Engine No", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
            ESimSolPdfHelper.AddCell(ref oPdfPTable2, _oSalesQuotation.EngineNo, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
            ESimSolPdfHelper.AddCell(ref oPdfPTable2, "Interior Color", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
            ESimSolPdfHelper.AddCell(ref oPdfPTable2, _oSalesQuotation.InteriorColorName, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
            ESimSolPdfHelper.AddCell(ref oPdfPTable2, "Exterior Color", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
            ESimSolPdfHelper.AddCell(ref oPdfPTable2, _oSalesQuotation.ExteriorColorName, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
            ESimSolPdfHelper.AddCell(ref oPdfPTable2, "Deliver", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
            //ESimSolPdfHelper.AddCell(ref oPdfPTable2, _oSalesQuotation.DeliveryDate, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
            ESimSolPdfHelper.AddCell(ref oPdfPTable2, (_oSalesQuotation.ETAValue > 0) ? _oSalesQuotation.PossibleDateInString : "In Stock", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);

            PrintEmptyCell();
            _oPdfPCell = new PdfPCell();
            _oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 2;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.AddElement(oPdfPTable2);
            _oPdfPTable.AddCell(_oPdfPCell);
            #endregion


            #region Print Features
            PrintEmptyRow(10);
            PrintFeatureName(_oSalesQuotation.ModelShortName + "  " + _oSalesQuotation.ModelNo, this.GetCustomFont("AudiType-Bold", 8f));
            foreach (ModelFeature oModelFeature in _oModelFeatures)
            {
                if (oModelFeature.FeatureType != EnumFeatureType.OptionalFeature)
                {
                    AddFeature(oModelFeature);
                }
            }
            PrintEmptyRow(30f);

            #endregion

            #region OptionalItem
            PdfPTable oPdfPTable = new PdfPTable(2);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            oPdfPTable.SetWidths(new float[] { _fBodyWidth-100f, // Code No
                                               100f });

            foreach (SalesQuotationDetail oSalesQuotationDetail in _oSalesQuotation.SalesQuotationDetails)
            {
                AddFeature(new ModelFeature()
                {
                    FeatureCode = oSalesQuotationDetail.FeatureCode,
                    Price = oSalesQuotationDetail.Price,
                    FeatureName = oSalesQuotationDetail.FeatureName,
                    FeatureType = EnumFeatureType.OptionalFeature
                });
            }
            

            _oPdfPCell = new PdfPCell(new Phrase(" ", this.GetCustomFont("AudiType-Bold", 9f)));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.FixedHeight = 60f;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase("Price Calculation : ", this.GetCustomFont("AudiType-Bold", 10.5f, true))); _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.FixedHeight = 30f;
            _oPdfPCell.Colspan = 2; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPTable = PrintValue(oPdfPTable, "Proforma Price(Excluding Registration, VAT, AIT & Source Tax)", _oSalesQuotation.OptionTotal + _oSalesQuotation.NewOfferPrice);//AmountWithDiscount
            oPdfPTable = PrintValue(oPdfPTable, "VAT", _oSalesQuotation.VatAmount);
            oPdfPTable = PrintValue(oPdfPTable, "TDS Amount", _oSalesQuotation.TDSAmount);
            oPdfPTable = PrintValue(oPdfPTable, "Registration Cost", _oSalesQuotation.RegistrationFee);

            if (_oSalesQuotation.DiscountPrice > 0)
            {
                oPdfPTable = PrintValue(oPdfPTable, "Proforma Price", _oSalesQuotation.OptionTotal + _oSalesQuotation.NewOfferPrice + _oSalesQuotation.VatAmount + _oSalesQuotation.RegistrationFee + _oSalesQuotation.TDSAmount);

                oPdfPTable = PrintValue(oPdfPTable, "Discount Price", _oSalesQuotation.DiscountPrice);
                oPdfPTable = PrintValue(oPdfPTable, "Total Proforma Price", _oSalesQuotation.OptionTotal + _oSalesQuotation.NewOfferPrice + _oSalesQuotation.VatAmount + _oSalesQuotation.TDSAmount + _oSalesQuotation.RegistrationFee - _oSalesQuotation.DiscountPrice);
            }
            else
                oPdfPTable = PrintValue(oPdfPTable, "Total Proforma Price", _oSalesQuotation.OptionTotal + _oSalesQuotation.NewOfferPrice + _oSalesQuotation.VatAmount + _oSalesQuotation.TDSAmount + _oSalesQuotation.RegistrationFee);            
            
            oPdfPTable = PrintOptionTotalValue(oPdfPTable, "Please note that payment has to be made in Bangladeshi Taka.", 0, 2);


            PrintEmptyCell();
            _oPdfPCell = new PdfPCell();
            _oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 2;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.AddElement(oPdfPTable);
            _oPdfPTable.AddCell(_oPdfPCell);

            #endregion

         
        }
     

        public void PrintPriceAndSalesTerm()
        {
            PrintHeaderTitler("CONDITION OF SALE");

            #region Price Table Row
            _oPdfPCell = new PdfPCell(); //ToFromColumn
            _oPdfPCell.Border = 0;
            _oPdfPTable.AddCell(_oPdfPCell);

            PdfPTable oPdfPTable = new PdfPTable(1);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

            _oPdfPCell = new PdfPCell(new Phrase("")); _oPdfPCell.FixedHeight = 10f;
            _oPdfPCell.Border = 0; oPdfPTable.AddCell(_oPdfPCell); oPdfPTable.CompleteRow();

            #region Column

            //oPdfPTable = AddColumnBold(oPdfPTable, "Unit Price (Ex-Showroom): ", _oSalesQuotation.NewOfferPrice + " (" + Global.TakaWords(_oSalesQuotation.UnitPrice) + ") BDT ");

            //if (_oSalesQuotation.DiscountPrice > 0)
            //{
            //    oPdfPTable = AddColumnBold(oPdfPTable, "Discount Price : ", _oSalesQuotation.DiscountPrice + " (" + Global.TakaWords(_oSalesQuotation.DiscountPrice) + ") BDT ");
            //    oPdfPTable = AddColumnBold(oPdfPTable, "Unit Price : ", _oSalesQuotation.UnitPrice + " (" + Global.TakaWords(_oSalesQuotation.UnitPrice) + ") BDT ");
            //}

            //oPdfPTable = AddColumnBold(oPdfPTable, "", _oSalesQuotation.PaymentTerm);
            //oPdfPTable = AddColumnBold(oPdfPTable, "", "");
            //oPdfPTable = AddColumnBold(oPdfPTable, "VAT: ", _oSalesQuotation.VatAmount + " (" + Global.TakaWords(_oSalesQuotation.VatAmount) + ") BDT ");
            //oPdfPTable = AddColumnBold(oPdfPTable, "Registration Cost: ", _oSalesQuotation.RegistrationFee + " (" + Global.TakaWords(_oSalesQuotation.RegistrationFee) + ") BDT ");
            //oPdfPTable = AddColumnBold(oPdfPTable, "Total OTR: ", _oSalesQuotation.OTRAmount + " (" + Global.TakaWords(_oSalesQuotation.OTRAmount) + ") BDT ");

            //oPdfPTable = AddColumn(oPdfPTable, "Warranty: ", _oSalesQuotation.Warranty);

            //oPdfPTable = AddColumn(oPdfPTable, "Delivery: ", _oSalesQuotation.DeliveryDate);
            //oPdfPTable = AddColumn(oPdfPTable, "Payment:  ", _oSalesQuotation.AdvancePayment + " (" + Global.TakaWords(_oSalesQuotation.AdvancePayment) + ") BDT ");
            //oPdfPTable = AddColumnBold(oPdfPTable, "", "");
            //oPdfPTable = AddColumn(oPdfPTable, "Validity of Offer: ", _oSalesQuotation.ValidityOfOffer);
            //oPdfPTable = AddColumnBold(oPdfPTable, "", "");
            //oPdfPTable = AddColumn(oPdfPTable, "After Sales Service: ", _oSalesQuotation.AfterSalesService);

            #endregion

            //_oPdfPCell = new PdfPCell();
            //_oPdfPCell.Border = 0;
            //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.AddElement(oPdfPTable);
            //_oPdfPTable.AddCell(_oPdfPCell);

            #endregion

            PrintEmptyRow(10);

            #region Paragraphs
            string sHeader = "Acceptance: ";
            AddParagraphRow(sHeader, _oSalesQuotation.Acceptance);//sHeader,sData

            sHeader = "Offer Validity: ";
            //sData="The enclosed offer is valid for 7(seven) banking days from the date of issuance";
            AddParagraphRow(sHeader, _oSalesQuotation.OfferValidity);

            sHeader = "Order Specifications: ";
            //sData="The selected options/specifications in the order are binding and cannot be changed as the production process at the factory is initiated as soon as the order is placed within the Audi ordering-system.";
            AddParagraphRow(sHeader, _oSalesQuotation.OrderSpecifications);

            sHeader = "Vehicle Inspection: ";
            //sData="The vehicle inspection carried out by Audi AG at their respective plants shall be final for all purposes. Audi AG reserves the right to change names of options/accessories without prior notice; however, with no financial loss to the customer.";
            AddParagraphRow(sHeader, _oSalesQuotation.VehicleInspection);

            sHeader = "Cancelling or Changing the Order: ";
            AddParagraphRow(sHeader, _oSalesQuotation.CancelOrChangeOrder);

            sHeader = "Vehicle Specifications: ";
            //string sData= "If any of the optional equipment/accessory is not in conformity with the vehicle order form, the Purchaser’s sole remedy shall be limited to Seller. Making good any shortage by replacing such equipment/accessory or, if Seller shall choose, by refunding a proportionate part of the price.";
            AddParagraphRow(sHeader, _oSalesQuotation.VehicleSpecification);

            sHeader = "Mode of Payment: ";
            //sData="Advance payment upon receipt of signed Proforma offer and order confirmation. For the price to be valid, payment amount needs to be made on/before the date as mentioned in your Proforma Offer. A cheque will not be treated as payment until it has been cleared. The goods shall remain the property of the Seller until the price has been discharged in full. Any exchange of funds given by the Purchaser in payment shall not be treated as a discharge until the same has been cleared.";
            AddParagraphRow(sHeader, _oSalesQuotation.PaymentMode);

            if (!string.IsNullOrEmpty(_oSalesQuotation.Complementary))
                AddParagraphRow("Complementary: ", _oSalesQuotation.Complementary);

            sHeader = "Delivery: ";
            //sData= "The Seller will endeavor to secure delivery of the goods by the estimated delivery date (if any) but does not guarantee the time of delivery and shall not be liable for any damages or claims of any kind in respect of delay in delivery (The Seller shall not be obliged to fulfil orders in the sequence in which they are placed). Delivery of the vehicle is estimated (non-binding) between 16-18 weeks after receipt signed Proforma Invoice and clearance of down payment. We will endeavor to adhere to stipulated delivery deadline. In the event of a delay, we will contact you to agree an alternative estimates delivery period. However, delivery deadlines will not be binding unless expressly agreed otherwise. If non-compliance with the delivery date is due to force majeure or to other disturbances beyond our control e.g. war, port’s strikes, workers strike, rail/trucks strikes, vessel/ship delays import or export restrictions, including such disturbances affecting subcontractors, the delivery dates agreed upon shall be extended by the period of time of the disturbance. We will not be liable for any claim for compensation of any description arising out of a delay in delivery. We reserve the right to make deliveries before or after the respective deliver periods. ";
            AddParagraphRow(sHeader, _oSalesQuotation.DeliveryDescription);

            sHeader = "Price Fluctuation Clause: ";
            AddParagraphRow(sHeader, _oSalesQuotation.PriceFluctuationClause);

            sHeader = "Customs Clearance:";
            //sData="The Seller will arrange customs clearance of vehicle if payment is done locally in Bangladeshi Taka, and in case of direct remittance to Audi AG (only valid for Audi Security vehicles) it is optional that clearance assistance from the Seller may be obtained at actual custom clearance cost. For diplomatic sales, or duty free imports the Seller will only assist in vehicle clearance, however any charges occurred during the process of clearing will be borne by the customer.";
            AddParagraphRow(sHeader, _oSalesQuotation.CustomsClearance);

            sHeader = "Insurance: ";
            // sData="Insurance of the vehicle until our respective dealerships in Bangladesh is seller’s responsibility.";
            AddParagraphRow(sHeader, _oSalesQuotation.Insurance);

            sHeader = "Force Majeure: ";
            //sData="Neither party shall be liable in damages or have the right to terminate this Agreement for any delay or default in performing hereunder if such delay or default is caused by conditions beyond its control including, but not limited to Acts of God, Government restrictions (including the denial or cancellation of any export or other necessary license), wars, insurrections and/or any other cause beyond the reasonable control of the party whose performance is affected.";
            AddParagraphRow(sHeader, _oSalesQuotation.ForceMajeure);

            sHeader = "Fuel Quality: ";
            //sData="With the signature below the purchaser agrees to fuel his/her vehicle only with HOBC fuel only. Failure to do so, can possibility void the manufacturer warranty if proven during a possible fault or during a fuel sample analysis.";
            AddParagraphRow(sHeader, _oSalesQuotation.FuelQuality);

            sHeader = "Warranty: ";
            //sData="Standard terms of warranty of Audi AG are applicable. 2 (two) years with unlimited mileage";
            AddParagraphRow(sHeader, _oSalesQuotation.WarrantyTerms);

            sHeader = "Special Instruction: ";
            //sData = "It is very important for the purchaser to read thoroughly the owner’s manual to better understand their vehicle.";
            AddParagraphRow(sHeader, _oSalesQuotation.SpecialInstruction);
            #endregion

          
        }
        

        #region Add Column
        public PdfPTable AddColumn(PdfPTable oPdfPTable, string sHeader, string sValue, bool bIsLastCol)  //table,header,data
        {
            _oPhrase = new Phrase();
            _oPhrase.Add(new Chunk(sHeader, this.GetCustomFont("AudiType-Bold", 8.5f)));
            _oPhrase.Add(new Chunk(sValue, this.GetCustomFont("AudiType-Normal", 8.5f)));
            _oPdfPCell = new PdfPCell(_oPhrase); _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 30f;
            oPdfPTable.AddCell(_oPdfPCell);
            if (bIsLastCol == true) { oPdfPTable.CompleteRow(); }
            return oPdfPTable;
        }
        public PdfPTable AddColumnBold(PdfPTable oPdfPTable, string sHeader, string sValue)
        {
            _oPhrase = new Phrase();
            _oPhrase.Add(new Chunk(sHeader, _oFontStyleRowHeader));
            _oPhrase.Add(new Chunk(sValue, _oFontStyleRowHeader));

            _oPdfPCell = new PdfPCell(_oPhrase); _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            if (!(string.IsNullOrEmpty(sHeader) && string.IsNullOrEmpty(sValue)))
                _oPdfPCell.FixedHeight = 20f;
            oPdfPTable.AddCell(_oPdfPCell); oPdfPTable.CompleteRow();
            return oPdfPTable;
        }
        #endregion

        #region Feature's Funtion
        public void PrintFeatureName(string sHeader, iTextSharp.text.Font oFontStyle)
        {
            _oPdfPCell = new PdfPCell(); //TOFromColumn
            _oPdfPCell.Border = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase(sHeader, oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.Colspan = 2;
            _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();
        }
        public void AddFeature(ModelFeature oModelFeature)
        {
            PrintEmptyCell();

            PdfPTable oPdfPTable = new PdfPTable(3); //460f
            oPdfPTable.WidthPercentage = 100; oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            oPdfPTable.SetWidths(new float[] { 40f, 440f, 115f });

            _oFontStyle = this.GetCustomFont("AudiType-Normal", 8f);

            _oPdfPCell = new PdfPCell(new Phrase(oModelFeature.FeatureCode, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(oModelFeature.FeatureName, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase((oModelFeature.FeatureType == EnumFeatureType.OptionalFeature ? "BDT " + Global.TakaFormat(oModelFeature.Price) : "Included"), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell();
            _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.AddElement(oPdfPTable);
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        public PdfPTable AddOptionalItem(PdfPTable oPdfPTable, string sHeader, double dAmmount)
        {
            _oPdfPCell = new PdfPCell(new Phrase(sHeader, _oFontStyleLetterBody));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.FixedHeight = 20f;
            _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("BDT " + Global.MillionFormat(dAmmount), _oFontStyleLetterBody));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell);

            _dOptionItemTotalAmmount += dAmmount;
            oPdfPTable.CompleteRow();
            return oPdfPTable;
        }
        public PdfPTable PrintOptionTotalValue(PdfPTable oPdfPTable, string sHeader, double dTotalValue, int IsUnderline)
        {
            PdfPTable oPdfPTable_BDT = new PdfPTable(2);
            oPdfPTable_BDT.SetWidths(new float[]{50,140});

            if(IsUnderline == 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase(sHeader, this.GetCustomFont("AudiType-Bold", 9f)));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; 
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.FixedHeight = 20f;
                _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(_oPdfPCell);

                #region BDT
                _oPdfPCell = new PdfPCell(new Phrase("BDT ", this.GetCustomFont("AudiType-Bold", 9f)));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable_BDT.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(dTotalValue), this.GetCustomFont("AudiType-Bold", 9f)));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable_BDT.AddCell(_oPdfPCell);
                #endregion

                _oPdfPCell = new PdfPCell(oPdfPTable_BDT);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            else if (IsUnderline == 1)
            {
                _oPdfPCell = new PdfPCell(new Phrase(sHeader, this.GetCustomFont("AudiType-Bold", 9f)));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 1;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.FixedHeight = 20f;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable_BDT = new PdfPTable(2);
                oPdfPTable_BDT.SetWidths(new float[] { 50, 140 });

                #region BDT
                _oPdfPCell = new PdfPCell(new Phrase("BDT ", this.GetCustomFont("AudiType-Bold", 9f)));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 1;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable_BDT.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(dTotalValue), this.GetCustomFont("AudiType-Bold", 9f)));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 1;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable_BDT.AddCell(_oPdfPCell);
                #endregion


                _oPdfPCell = new PdfPCell(oPdfPTable_BDT);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 1;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            else if (IsUnderline == 2)
            {
                _oPdfPCell = new PdfPCell(new Phrase(sHeader, this.GetCustomFont("AudiType-Normal", 7.5f)));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.FixedHeight = 20f;
                _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", this.GetCustomFont("AudiType-Normal", 7.5f)));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            
            return oPdfPTable;
        }

        public PdfPTable PrintValue(PdfPTable oPdfPTable, string sHeader, double dTotalValue)
        {
            PdfPTable oPdfPTable_BDT = new PdfPTable(2);
            oPdfPTable_BDT.SetWidths(new float[] { 50, 140 });

            _oPdfPCell = new PdfPCell(new Phrase(sHeader, this.GetCustomFont("AudiType-Bold", 9f)));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.FixedHeight = 20f;
            _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell);

            #region BDT
            _oPdfPCell = new PdfPCell(new Phrase("BDT ", this.GetCustomFont("AudiType-Bold", 9f)));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.FixedHeight = 20f;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.Border =0; _oPdfPCell.BorderWidthRight = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable_BDT.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.TakaFormat(dTotalValue), this.GetCustomFont("AudiType-Bold", 9f)));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.FixedHeight = 20f;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.Border = 0;
            _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable_BDT.AddCell(_oPdfPCell);
            #endregion

            _oPdfPCell = new PdfPCell(oPdfPTable_BDT);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.FixedHeight = 20f;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            return oPdfPTable;
        }
        #endregion

        #region Print Exterior & Interior Image
        //public void PrintExteriorImage()
        //{
        //    if (_oVehicleOrderImages.Count > 0)
        //    {
        //        _oPdfPCell = new PdfPCell();
        //        _oPdfPCell.Border = 0;
        //        _oPdfPTable.AddCell(_oPdfPCell);

        //        PrintVehicleOrderImage(_oVehicleOrderImages[0]);
        //        _oPdfPCell = new PdfPCell();
        //        _oPdfPCell.Border = 0;
        //        _oPdfPTable.AddCell(_oPdfPCell);
        //        _oPdfPTable.CompleteRow();

        //        _oPdfPCell = new PdfPCell();
        //        _oPdfPCell.Border = 0;
        //        _oPdfPTable.AddCell(_oPdfPCell);
        //        PrintVehicleOrderImage(_oVehicleOrderImages[1]);
        //        _oPdfPCell = new PdfPCell();
        //        _oPdfPCell.Border = 0;
        //        _oPdfPTable.AddCell(_oPdfPCell);
        //        _oPdfPTable.CompleteRow();
        //    }
        //    else if (_oSalesQuotationImages.Count > 0)
        //    {
        //        _oPdfPCell = new PdfPCell();
        //        _oPdfPCell.Border = 0;
        //        _oPdfPTable.AddCell(_oPdfPCell);

        //        PrintSalesQuotationImage(_oSalesQuotationImages[0]);
        //        _oPdfPCell = new PdfPCell();
        //        _oPdfPCell.Border = 0;
        //        _oPdfPTable.AddCell(_oPdfPCell);
        //        _oPdfPTable.CompleteRow();

        //        _oPdfPCell = new PdfPCell();
        //        _oPdfPCell.Border = 0;
        //        _oPdfPTable.AddCell(_oPdfPCell);
        //        PrintSalesQuotationImage(_oSalesQuotationImages[1]);
        //        _oPdfPCell = new PdfPCell();
        //        _oPdfPCell.Border = 0;
        //        _oPdfPTable.AddCell(_oPdfPCell);
        //        _oPdfPTable.CompleteRow();
        //    }
        //    PrintFeatureName("*Illustrated Models Shown, Actual Vehicle May Differ in Color and Specification Accuracy", _oFontStyleImageFooter);

        //}
        //public void PrintInteriorImage()
        //{
        //    if (_oVehicleOrderImages.Count > 0)
        //    {
        //        _oPdfPCell = new PdfPCell();
        //        _oPdfPCell.Border = 0;
        //        _oPdfPTable.AddCell(_oPdfPCell);
        //        PrintVehicleOrderImage(_oVehicleOrderImages[2]);
        //        _oPdfPCell = new PdfPCell();
        //        _oPdfPCell.Border = 0;
        //        _oPdfPTable.AddCell(_oPdfPCell);
        //        _oPdfPTable.CompleteRow();

        //        _oPdfPCell = new PdfPCell();
        //        _oPdfPCell.Border = 0;
        //        _oPdfPTable.AddCell(_oPdfPCell);
        //        PrintVehicleOrderImage(_oVehicleOrderImages[3]);
        //        _oPdfPCell = new PdfPCell();
        //        _oPdfPCell.Border = 0;
        //        _oPdfPTable.AddCell(_oPdfPCell);
        //        _oPdfPTable.CompleteRow();
        //    }
        //    else if (_oSalesQuotationImages.Count > 0)
        //    {
        //        _oPdfPCell = new PdfPCell();
        //        _oPdfPCell.Border = 0;
        //        _oPdfPTable.AddCell(_oPdfPCell);
        //        PrintSalesQuotationImage(_oSalesQuotationImages[2]);
        //        _oPdfPCell = new PdfPCell();
        //        _oPdfPCell.Border = 0;
        //        _oPdfPTable.AddCell(_oPdfPCell);
        //        _oPdfPTable.CompleteRow();

        //        _oPdfPCell = new PdfPCell();
        //        _oPdfPCell.Border = 0;
        //        _oPdfPTable.AddCell(_oPdfPCell);
        //        PrintSalesQuotationImage(_oSalesQuotationImages[3]);
        //        _oPdfPCell = new PdfPCell();
        //        _oPdfPCell.Border = 0;
        //        _oPdfPTable.AddCell(_oPdfPCell);
        //        _oPdfPTable.CompleteRow();
        //    }
        //    PrintFeatureName("*Illustrated Models Shown, Actual Vehicle May Differ in Color and Specification Accuracy", _oFontStyleImageFooter);

        //}
        //public void PrintVehicleOrderImage(VehicleOrderImage oVehicleOrderImage)
        //{
        //    PrintEmptyRow(55f);

        //    _oPdfPCell = new PdfPCell(new Phrase(""));
        //    if (oVehicleOrderImage.TSImage != null)
        //    {
        //        _oImag = iTextSharp.text.Image.GetInstance(oVehicleOrderImage.TSImage, System.Drawing.Imaging.ImageFormat.Jpeg);
        //        _oPdfPCell = new PdfPCell(_oImag);
        //    }
        //    _oImag.ScaleToFit(_fBodyWidth, 250f);

        //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_BOTTOM;
        //    _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
        //    _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 200f;
        //    _oPdfPTable.AddCell(_oPdfPCell);
        //}
        //public void PrintSalesQuotationImage(SalesQuotationImage oSalesQuotationImage)
        //{
        //    PrintEmptyRow(55f);

        //    _oPdfPCell = new PdfPCell(new Phrase(""));
        //    if (oSalesQuotationImage.TSImage != null)
        //    {
        //        _oImag = iTextSharp.text.Image.GetInstance(oSalesQuotationImage.TSImage, System.Drawing.Imaging.ImageFormat.Jpeg);
        //        _oPdfPCell = new PdfPCell(_oImag);
        //    }
        //    _oImag.ScaleToFit(_fBodyWidth, 250f);
        //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_BOTTOM;
        //    _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
        //    _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 200f;
        //    _oPdfPTable.AddCell(_oPdfPCell);
        //}
        #endregion

        #region PDF HELPER
        public Paragraph AddParagraphRow(string sHeader, string sData)
        {
            PrintEmptyRow(4);
            //PrintFeatureName(sHeader, _oFontStyleRowHeader);
            //PrintEmptyCell();
            iTextSharp.text.Font _oFontTerms = new iTextSharp.text.Font();
            PdfPTable oTempPdfPTable = new PdfPTable(1);
            oTempPdfPTable.WidthPercentage = 100;

            _oFontTerms = this.GetCustomFont("AudiType-Bold", 6f);
            Paragraph _oPdfParagraph;
            _oPdfPCell = new PdfPCell(new Phrase(sHeader, _oFontTerms));
            _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oTempPdfPTable.AddCell(_oPdfPCell);
            oTempPdfPTable.CompleteRow();

            _oFontTerms = this.GetCustomFont("AudiType-Normal", 6f);
            _oPdfParagraph = new Paragraph(new Phrase(sData, _oFontTerms));
            _oPdfParagraph.SetLeading(0.1f, 1f);
            _oPdfParagraph.Alignment = Element.ALIGN_JUSTIFIED;
            _oPdfPCell.AddElement(_oPdfParagraph);
            _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oTempPdfPTable.AddCell(_oPdfPCell);
            oTempPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oTempPdfPTable);
            _oPdfPCell.Colspan = 3; _oPdfPCell.Border = 0;
            _oPdfPTable.AddCell(_oPdfPCell);

            PrintEmptyCell();
            _oPdfPTable.CompleteRow();
            return null;
        }
        public PdfPTable AddChunkRow(PdfPTable oPdfPTable, string sHeader, string sValue, iTextSharp.text.Font oFontStyleRowHeader, iTextSharp.text.Font oFontStyleData)  //table,header,data,headerfont,datafont
        {
            _oPhrase = new Phrase();
            _oPhrase.Add(new Chunk(sHeader, oFontStyleRowHeader));
            _oPhrase.Add(new Chunk(sValue, oFontStyleData));
            _oPdfPCell = new PdfPCell(_oPhrase); _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell); oPdfPTable.CompleteRow();
            return oPdfPTable;
        }
        public void PrintEmptyCell()
        {
            _oPdfPCell = new PdfPCell();
            _oPdfPCell.Border = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
        }
        public void PrintCompanyHeader()
        {
            _oPdfPCell = new PdfPCell(); //TOFromColumn
            _oPdfPCell.Border = 0;
            _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(); //TitleColumn
            _oPdfPCell.Border = 0;
            _oPdfPTable.AddCell(_oPdfPCell);

            #region CompanyLogo
            _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
            _oImag.ScaleAbsolute(73f, 25f);
            _oPdfPCell = new PdfPCell(_oImag);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2;
            _oPdfPCell.FixedHeight = 15f;
            _oPdfPTable.AddCell(_oPdfPCell);
            #endregion

            _oPdfPTable.CompleteRow();
            //_oPdfPCell = new PdfPCell(); //TitleColumn
            //_oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2;
            //_oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyleHeaderTitle = FontFactory.GetFont("Tahoma", 16f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);
            _oPdfPCell = new PdfPCell(new Phrase("Progress Motors Imports Limited", _oFontStyleHeaderTitle)); //TitleColumn
            _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.Colspan = 3;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            

            _oFontStyleHeaderTitle = this.GetCustomFont("AudiType-Bold", 10f);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleHeaderTitle)); //TitleColumn
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 40;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }

        public void PrintCompanyHeader2()
        {
            _oPdfPCell = new PdfPCell(); //TOFromColumn
            _oPdfPCell.Border = 0;
            _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(); //TitleColumn
            _oPdfPCell.Border = 0;
            _oPdfPTable.AddCell(_oPdfPCell);

            #region CompanyLogo
            _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
            _oImag.ScaleAbsolute(73f, 25f);
            _oPdfPCell = new PdfPCell(_oImag);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2;
            _oPdfPCell.FixedHeight = 15f;
            _oPdfPTable.AddCell(_oPdfPCell);
            #endregion

            _oPdfPTable.CompleteRow();
            //_oPdfPCell = new PdfPCell(); //TitleColumn
            //_oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2;
            //_oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyleHeaderTitle = FontFactory.GetFont("Tahoma", 16f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.LIGHT_GRAY);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleHeaderTitle)); //TitleColumn
            _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.Colspan = 3;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

      

            _oFontStyleHeaderTitle = this.GetCustomFont("AudiType-Bold", 10f);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleHeaderTitle)); //TitleColumn
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 30;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        public void PrintHeaderTitler(string sHeader)
        {
            _oPdfPCell = new PdfPCell(); //ToFromColumn
            _oPdfPCell.Border = 0;
            _oPdfPTable.AddCell(_oPdfPCell);

            if (!string.IsNullOrEmpty(sHeader))
            {
                #region HEADER
                _oPdfPCell = new PdfPCell(new Phrase(sHeader, _oFontStyleHeaderTitle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                #endregion
            }
            else
            {
                _oPdfPCell = new PdfPCell(); //ToFromColumn
                _oPdfPCell.Border = 0;
                _oPdfPTable.AddCell(_oPdfPCell);
            }

            _oPdfPTable.CompleteRow();
        }
        public void PrintEmptyRow(float nHeight)
        {
            _oPdfPCell = new PdfPCell();
            _oPdfPCell.Colspan = 3;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = nHeight;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }

        #endregion
        public iTextSharp.text.Font GetCustomFont(string fontName, float fFontSize)
        {
            string fontpath = System.Web.Hosting.HostingEnvironment.MapPath("~/fonts/");
            BaseFont customfont = BaseFont.CreateFont(fontpath + fontName + ".ttf", BaseFont.CP1252, BaseFont.EMBEDDED);
            iTextSharp.text.Font oFont = new iTextSharp.text.Font(customfont, fFontSize);
            //iTextSharp.text.FontFactory.Register(fontpath + fontName + ".ttf");
            return oFont;
        }
        public iTextSharp.text.Font GetCustomFont(string fontName, float fFontSize, bool isUnderLined)
        {
            string fontpath = System.Web.Hosting.HostingEnvironment.MapPath("~/fonts/");
            BaseFont customfont = BaseFont.CreateFont(fontpath + fontName + ".ttf", BaseFont.CP1252, BaseFont.EMBEDDED);
            iTextSharp.text.Font oFont = new iTextSharp.text.Font(customfont, fFontSize, iTextSharp.text.Font.UNDERLINE);
            //iTextSharp.text.FontFactory.Register(fontpath + fontName + ".ttf");
            return oFont;
        }
        public iTextSharp.text.Font GetCustomFontBangla(string fontName, float fFontSize)
        {
            string fontpath = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/AudiFont/");
            BaseFont customfont = BaseFont.CreateFont(fontpath + fontName + ".ttf", BaseFont.CP1252, BaseFont.EMBEDDED);
            iTextSharp.text.Font oFont = new iTextSharp.text.Font(customfont, fFontSize);
            //iTextSharp.text.FontFactory.Register(fontpath + fontName + ".ttf");
            return oFont;
        }
    }
}
