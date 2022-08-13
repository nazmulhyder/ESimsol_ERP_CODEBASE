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
    public class rptSalesQuotation_F2
    {
        #region Font Declaration
        Document _oDocument;
        iTextSharp.text.Image _oImag;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyleBangla;
        iTextSharp.text.Font _oFontStyleCompanyTitle ;
        iTextSharp.text.Font _oFontStyleToFrom ;
        iTextSharp.text.Font _oFontStyleToFromDetails;
        iTextSharp.text.Font _oFontStyleLetterBody ;
        iTextSharp.text.Font _oFontStyleCompanyDetails ;
        iTextSharp.text.Font _oFontStyleHeaderTitle;
        iTextSharp.text.Font _oFontStylePageNo;
        iTextSharp.text.Font _oFontStyleRowHeader;
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
        List<KommFileImage> _oKommFileImages = new List<KommFileImage>();
        #endregion

        public List<Object> PrepareReport(Company oCompany, SalesQuotation oSalesQuotation, List<KommFileImage> oKommFileImages, List<ModelFeature> oModelFeatures, int PageTotal)
        {
            _oCompany = oCompany;
            _oSalesQuotation = oSalesQuotation;
            _oModelFeatures = oModelFeatures;
            //if(oSalesQuotationImages.Count > 0) 
            //{
            //    _oSalesQuotationImages = oSalesQuotationImages;
            //}else if(oVehicleOrderImages.Count>0)
            //{
            //    _oVehicleOrderImages = oVehicleOrderImages;
            //}
            _oKommFileImages = oKommFileImages;

            //SetFontStyle For CustomFonts
            this.SetFontStyle();

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
            _oDocument.SetMargins(45f, 5f, 30f, 15f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

            _oFontStyle = _oFontStyleToFromDetails; //FontFactory.GetFont("Audi Type", 7f, 1);
            //PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oPDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            //rptSaleQuotationFooter PageEventHandler = new rptSaleQuotationFooter(_oSalesQuotation.RefNo);
            //_oPDFWriter.PageEvent = PageEventHandler;
         
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { _fToWidth, // To
                                                _fBodyWidth, //body, 
                                                _fCompanyWidth //Company
                                                });
            # endregion
                      
            this.PrintPages(PageTotal);
            //_oPdfPTable.HeaderRows = 1;
            _oDocument.Close();

            List<Object> oObjects = new List<Object>();
            Object aa = _oMemoryStream.ToArray();
            Object bb = _oDocument.PageNumber - 1;
            oObjects.Add(aa);
            oObjects.Add(bb);
            return oObjects;
        }

        private void PrintPages(int nTotalPage) 
        {
            //this.PrintLetterPage(nTotalPage);
            //this.NewPageDeclaration();
            //ESimSolPdfHelper.PrintHeader_Audi(ref _oPdfPTable, new BusinessUnit(), _oCompany, new string[] { "Spare Parts Request Form", "Audi Service Dhaka", "Audi", "429/432, Tejgaon I/A, Dhaka-1208", "Service" }, 3);

            this.PrintHeader();

            this.PrintOfferPage();
            _oPdfPTable.HeaderRows =1;
            this.NewPageDeclaration();

            this.PrintFeaturePage();
            _oPdfPTable.HeaderRows =1;
            this.NewPageDeclaration();

            this.PrintPriceCalculation();
            _oPdfPTable.HeaderRows =1;
            this.NewPageDeclaration();

            this.PrintExteriorImage();
            _oPdfPTable.HeaderRows =1;
            this.NewPageDeclaration();

            this.PrintInteriorImage();
            _oPdfPTable.HeaderRows =1;
            this.NewPageDeclaration();
            //_oPdfPTable.HeaderRows =1;
            //_oPdfPTable.HeaderRows = 1;
            //PrintPriceAndSalesTerm();
            //this.NewPageDeclaration();
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
            _oFontStyleCompanyTitle = this.GetCustomFont("AudiType-ExtendedBold",12);//SolaimanLipi_29-05-06
            _oFontStyleBangla = this.GetCustomFontBangla("SolaimanLipi_29-05-06", 12);//
            _oFontStyleToFrom = this.GetCustomFont("AudiType-Normal", 7f); //FontFactory.GetFont("Audi Type", 7f, iTextSharp.text.Font.NORMAL);
            _oFontStyleToFromDetails = this.GetCustomFont("AudiType-Normal", 7f);// FontFactory.GetFont("Audi Type", 7f, iTextSharp.text.Font.NORMAL);
            _oFontStyleLetterBody = this.GetCustomFont("AudiType-Normal", 9f);// FontFactory.GetFont("Audi Type", 9f, iTextSharp.text.Font.NORMAL);
            _oFontStyleCompanyDetails = this.GetCustomFont("AudiType-Normal", 7f);// FontFactory.GetFont("Audi Type", 7f, iTextSharp.text.Font.NORMAL);
            _oFontStyleHeaderTitle = this.GetCustomFont("AudiType-Bold", 10f);// FontFactory.GetFont("Audi Type", 10f, iTextSharp.text.Font.BOLD);
            _oFontStylePageNo = this.GetCustomFont("AudiType-Normal", 7f);// FontFactory.GetFont("Audi Type", 7f, 2);
            _oFontStyleRowHeader = this.GetCustomFont("AudiType-Bold", 10f);// FontFactory.GetFont("Audi Type", 9f, iTextSharp.text.Font.BOLD);
            _oFontStyleImageFooter = this.GetCustomFont("AudiType-Bold", 7.5f);
        }

        #region Print Functions
        private void PrintHeader()
        {
            PrintCompanyHeader();
        }
        public void PrintOfferPage() 
        {
            PrintEmptyRow(70);

            #region BUYER
            _oPdfPCell = new PdfPCell(); //ToFromColumn
            _oPdfPCell.Border = 0;
            _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oSalesQuotation.BuyerName, _oFontStyleHeaderTitle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(); //ToFromColumn
            _oPdfPCell.Border = 0;
            _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oSalesQuotation.BuyerAddress, _oFontStyleHeaderTitle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            PrintEmptyRow(120);

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
            PrintEmptyRow(40);

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
            _oPdfPCell.BackgroundColor =  BaseColor.WHITE;
            _oPdfPCell.AddElement(oPdfPTable);
            _oPdfPTable.AddCell(_oPdfPCell);

            #endregion

            _oPdfPCell = new PdfPCell(); //ToFromColumn
            _oPdfPCell.Border = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
        }
        public void PrintFeaturePage()
        {
            //_oPdfPCell = new PdfPCell(new Phrase("", this.GetCustomFont("AudiType-Bold", 9f, true))); _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.FixedHeight = 22f;
            //_oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.Colspan = 3; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();
            PrintEmptyRow(10);
            #region PRICE CALCULATION

            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(); //ToFromColumn
            _oPdfPCell.Border = 0;
            _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("PRICE CALCULATION", this.GetCustomFont("AudiType-Bold", 9f, true)));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.Colspan=2;
            _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
                        
            //PrintFeatureName("Order Quotation Reference: " + _oSalesQuotation.RefNo, this.GetCustomFont("AudiType-Bold", 8.5f));

            PdfPTable oPdfPTable = new PdfPTable(2);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            oPdfPTable.SetWidths(new float[] { 100f ,_fBodyWidth-100f});

            ESimSolPdfHelper.FontStyle = this.GetCustomFont("AudiType-Bold", 8.5f);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Order Quotation Reference:", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, _oSalesQuotation.RefNo, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);

            ESimSolPdfHelper.FontStyle = this.GetCustomFont("AudiType-Normal", 8f);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Model", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, _oSalesQuotation.ModelNo, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Code", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, _oSalesQuotation.ModelCode, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
            //ESimSolPdfHelper.AddCell(ref oPdfPTable, "Chassis No", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
            //ESimSolPdfHelper.AddCell(ref oPdfPTable, _oSalesQuotation.ChassisNo, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
            //ESimSolPdfHelper.AddCell(ref oPdfPTable, "Engine No", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
            //ESimSolPdfHelper.AddCell(ref oPdfPTable, _oSalesQuotation.EngineNo, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Interior Color", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, _oSalesQuotation.InteriorColorName, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Exterior Color", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, _oSalesQuotation.ExteriorColorName, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Deliver", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
            //ESimSolPdfHelper.AddCell(ref oPdfPTable, _oSalesQuotation.DeliveryDate, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, (_oSalesQuotation.ETAValue > 0) ? _oSalesQuotation.PossibleDateInString : "In Stock", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
            
            PrintEmptyCell();
            _oPdfPCell = new PdfPCell();
            _oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 2;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.AddElement(oPdfPTable);
            _oPdfPTable.AddCell(_oPdfPCell);
            #endregion

            #region Print Features
            PrintEmptyRow(10);
            //PrintFeatureName(_oSalesQuotation.ModelShortName + "  " + _oSalesQuotation.ModelNo, this.GetCustomFont("AudiType-Bold", 8f));
            PrintFeatureName(" " +_oSalesQuotation.ModelNo, this.GetCustomFont("AudiType-Bold", 8f));
            foreach (ModelFeature oModelFeature in _oModelFeatures) 
            {
                if (oModelFeature.FeatureType != EnumFeatureType.OptionalFeature)
                {
                    AddFeature(oModelFeature);
                }
                
            }
            PrintEmptyRow(15f);

            #endregion


            
        }

        public void PrintPriceCalculation()
        {

            PrintEmptyRow(20);

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

            PrintEmptyCell();
            _oPdfPCell = new PdfPCell();
            _oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 2;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.AddElement(oPdfPTable);
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

          
            #endregion
            
            
            
            oPdfPTable = new PdfPTable(2);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            oPdfPTable.SetWidths(new float[] { _fBodyWidth-130f, // Code No
                                               130f });

            PrintEmptyRow(30);

            _oPdfPCell = new PdfPCell(new Phrase("Price Calculation : ", this.GetCustomFont("AudiType-Bold", 10.5f,true))); _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.FixedHeight = 30f;
            _oPdfPCell.Colspan = 2; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable = PrintValue(oPdfPTable, "Unit Price",_oSalesQuotation.NewOfferPrice);
            oPdfPTable = PrintValue(oPdfPTable, "Optional Item Total Price", _oSalesQuotation.OptionTotal );
            oPdfPTable = PrintValue(oPdfPTable, "Unit Price With Optional Item Total Price", _oSalesQuotation.OptionTotal + _oSalesQuotation.NewOfferPrice);
            oPdfPTable = PrintValue(oPdfPTable, "VAT", _oSalesQuotation.VatAmount);
            oPdfPTable = PrintValue(oPdfPTable, "TDS Amount", _oSalesQuotation.TDSAmount);
            oPdfPTable = PrintValue(oPdfPTable, "Registration Cost", _oSalesQuotation.RegistrationFee);
            if (_oSalesQuotation.DiscountPrice > 0)
            {
                oPdfPTable = PrintValue(oPdfPTable, "Proforma Price", _oSalesQuotation.OptionTotal + _oSalesQuotation.NewOfferPrice + _oSalesQuotation.VatAmount +_oSalesQuotation.VatAmount+ _oSalesQuotation.RegistrationFee);
                oPdfPTable = PrintValue(oPdfPTable, "Discount", _oSalesQuotation.DiscountPrice);
                oPdfPTable = PrintValue(oPdfPTable, "Total On the Road (OTR) Price", (_oSalesQuotation.OptionTotal + _oSalesQuotation.NewOfferPrice + _oSalesQuotation.VatAmount + _oSalesQuotation.TDSAmount + _oSalesQuotation.RegistrationFee) - _oSalesQuotation.DiscountPrice);
            }
            else
            {
                oPdfPTable = PrintValue(oPdfPTable, "Total On the Road (OTR) Price", _oSalesQuotation.OptionTotal + _oSalesQuotation.NewOfferPrice + _oSalesQuotation.VatAmount +_oSalesQuotation.TDSAmount+ _oSalesQuotation.RegistrationFee);
            }
            
            PrintEmptyCell();

            _oPdfPCell = new PdfPCell();
            _oPdfPCell.Border = 0;_oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.AddElement(oPdfPTable);            _oPdfPTable.AddCell(_oPdfPCell);

            PrintEmptyCell();
            _oPdfPTable.CompleteRow();
        }
        public void PrintPriceAndSalesTerm() 
        {
            PrintHeaderTitler("PRICE AND SALES TERMS");

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

            oPdfPTable = AddColumnBold(oPdfPTable, "Unit Price (Ex-Showroom): ", _oSalesQuotation.NewOfferPrice + " (" + Global.TakaWords(_oSalesQuotation.UnitPrice) + ") BDT ");

            if (_oSalesQuotation.DiscountPrice > 0)
            {
                oPdfPTable = AddColumnBold(oPdfPTable, "Discount Price : ", _oSalesQuotation.DiscountPrice + " (" + Global.TakaWords(_oSalesQuotation.DiscountPrice) + ") BDT ");
                oPdfPTable = AddColumnBold(oPdfPTable, "Unit Price : ", _oSalesQuotation.UnitPrice + " (" + Global.TakaWords(_oSalesQuotation.UnitPrice) + ") BDT ");
            }

            oPdfPTable = AddColumnBold(oPdfPTable,"",_oSalesQuotation.PaymentTerm);
            oPdfPTable = AddColumnBold(oPdfPTable, "", "");
            oPdfPTable = AddColumnBold(oPdfPTable, "VAT: ", _oSalesQuotation.VatAmount +" (" + Global.TakaWords(_oSalesQuotation.VatAmount) + ") BDT ");
            oPdfPTable = AddColumnBold(oPdfPTable, "Registration Cost: ", _oSalesQuotation.RegistrationFee + " (" + Global.TakaWords(_oSalesQuotation.RegistrationFee) + ") BDT ");
            oPdfPTable = AddColumnBold(oPdfPTable, "Total OTR: ", _oSalesQuotation.OTRAmount + " (" + Global.TakaWords(_oSalesQuotation.OTRAmount) + ") BDT ");
             
            oPdfPTable = AddColumn(oPdfPTable, "Warranty: ", _oSalesQuotation.Warranty,true);

            oPdfPTable = AddColumn(oPdfPTable, "Delivery: ", _oSalesQuotation.DeliveryDate, true);
            oPdfPTable = AddColumn(oPdfPTable, "Payment:  ", _oSalesQuotation.AdvancePayment + " (" + Global.TakaWords(_oSalesQuotation.AdvancePayment) + ") BDT ", true);
            oPdfPTable = AddColumnBold(oPdfPTable, "", "");
            oPdfPTable = AddColumn(oPdfPTable, "Validity of Offer: ", _oSalesQuotation.ValidityOfOffer, true);
            oPdfPTable = AddColumnBold(oPdfPTable, "", "");
            oPdfPTable = AddColumn(oPdfPTable, "After Sales Service: ", _oSalesQuotation.AfterSalesService, true);
           
            //oPdfPTable = AddColumn(oPdfPTable, "Model: ", "bbbb");
            #endregion
        
            _oPdfPCell = new PdfPCell();
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.AddElement(oPdfPTable);
            _oPdfPTable.AddCell(_oPdfPCell);

            #endregion

            PrintEmptyRow(20);

            #region Paragraphs
            string sHeader = "Acceptance: ";
            AddParagraphRow(sHeader, _oSalesQuotation.Acceptance);//sHeader,sData

            sHeader="Offer Validity: ";
            //sData="The enclosed offer is valid for 7(seven) banking days from the date of issuance";
            AddParagraphRow(sHeader, _oSalesQuotation.OfferValidity);

            sHeader="Order Specifications: ";
            //sData="The selected options/specifications in the order are binding and cannot be changed as the production process at the factory is initiated as soon as the order is placed within the Audi ordering-system.";
            AddParagraphRow(sHeader,_oSalesQuotation.OrderSpecifications);

            sHeader="Vehicle Inspection: ";
            //sData="The vehicle inspection carried out by Audi AG at their respective plants shall be final for all purposes. Audi AG reserves the right to change names of options/accessories without prior notice; however, with no financial loss to the customer.";
            AddParagraphRow(sHeader,_oSalesQuotation.VehicleInspection);

            sHeader="Cancelling or Changing the Order: ";
            AddParagraphRow(sHeader,_oSalesQuotation.CancelOrChangeOrder);

            sHeader="Vehicle Specifications: ";
            //string sData= "If any of the optional equipment/accessory is not in conformity with the vehicle order form, the Purchaser’s sole remedy shall be limited to Seller. Making good any shortage by replacing such equipment/accessory or, if Seller shall choose, by refunding a proportionate part of the price.";
            AddParagraphRow(sHeader,_oSalesQuotation.VehicleSpecification);

            sHeader="Mode of Payment: ";
            //sData="Advance payment upon receipt of signed Proforma offer and order confirmation. For the price to be valid, payment amount needs to be made on/before the date as mentioned in your Proforma Offer. A cheque will not be treated as payment until it has been cleared. The goods shall remain the property of the Seller until the price has been discharged in full. Any exchange of funds given by the Purchaser in payment shall not be treated as a discharge until the same has been cleared.";
            AddParagraphRow(sHeader,_oSalesQuotation.PaymentMode);

            if (!string.IsNullOrEmpty(_oSalesQuotation.Complementary))
                AddParagraphRow("Complementary: ", _oSalesQuotation.Complementary);

            sHeader="Delivery: ";
            //sData= "The Seller will endeavor to secure delivery of the goods by the estimated delivery date (if any) but does not guarantee the time of delivery and shall not be liable for any damages or claims of any kind in respect of delay in delivery (The Seller shall not be obliged to fulfil orders in the sequence in which they are placed). Delivery of the vehicle is estimated (non-binding) between 16-18 weeks after receipt signed Proforma Invoice and clearance of down payment. We will endeavor to adhere to stipulated delivery deadline. In the event of a delay, we will contact you to agree an alternative estimates delivery period. However, delivery deadlines will not be binding unless expressly agreed otherwise. If non-compliance with the delivery date is due to force majeure or to other disturbances beyond our control e.g. war, port’s strikes, workers strike, rail/trucks strikes, vessel/ship delays import or export restrictions, including such disturbances affecting subcontractors, the delivery dates agreed upon shall be extended by the period of time of the disturbance. We will not be liable for any claim for compensation of any description arising out of a delay in delivery. We reserve the right to make deliveries before or after the respective deliver periods. ";
            AddParagraphRow(sHeader,_oSalesQuotation.DeliveryDescription);

            sHeader="Price Fluctuation Clause: ";
            AddParagraphRow(sHeader,_oSalesQuotation.PriceFluctuationClause);

            sHeader="Customs Clearance:";
            //sData="The Seller will arrange customs clearance of vehicle if payment is done locally in Bangladeshi Taka, and in case of direct remittance to Audi AG (only valid for Audi Security vehicles) it is optional that clearance assistance from the Seller may be obtained at actual custom clearance cost. For diplomatic sales, or duty free imports the Seller will only assist in vehicle clearance, however any charges occurred during the process of clearing will be borne by the customer.";
            AddParagraphRow(sHeader,_oSalesQuotation.CustomsClearance);

            sHeader="Insurance: ";
           // sData="Insurance of the vehicle until our respective dealerships in Bangladesh is seller’s responsibility.";
            AddParagraphRow(sHeader,_oSalesQuotation.Insurance);

            sHeader="Force Majeure: ";
            //sData="Neither party shall be liable in damages or have the right to terminate this Agreement for any delay or default in performing hereunder if such delay or default is caused by conditions beyond its control including, but not limited to Acts of God, Government restrictions (including the denial or cancellation of any export or other necessary license), wars, insurrections and/or any other cause beyond the reasonable control of the party whose performance is affected.";
            AddParagraphRow(sHeader,_oSalesQuotation.ForceMajeure);

            sHeader="Fuel Quality: ";
            //sData="With the signature below the purchaser agrees to fuel his/her vehicle only with HOBC fuel only. Failure to do so, can possibility void the manufacturer warranty if proven during a possible fault or during a fuel sample analysis.";
            AddParagraphRow(sHeader,_oSalesQuotation.FuelQuality);

            sHeader="Warranty: ";
            //sData="Standard terms of warranty of Audi AG are applicable. 2 (two) years with unlimited mileage";
            AddParagraphRow(sHeader,_oSalesQuotation.WarrantyTerms);

            sHeader="Special Instruction: ";
            //sData = "It is very important for the purchaser to read thoroughly the owner’s manual to better understand their vehicle.";
            AddParagraphRow(sHeader,_oSalesQuotation.SpecialInstruction);
            #endregion

            PrintEmptyRow(60);
            #region Customer Signature
            PrintEmptyCell();
            oPdfPTable = new PdfPTable(2); //460f
            oPdfPTable.WidthPercentage = 100;oPdfPTable.WidthPercentage = 100;
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

            _oPdfPCell = new PdfPCell();
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.AddElement(oPdfPTable);
            _oPdfPTable.AddCell(_oPdfPCell);
            PrintEmptyCell();

            _oPdfPTable.CompleteRow();
            #endregion
        }
        #endregion

        #region Add Column
        public PdfPTable AddColumn(PdfPTable oPdfPTable, string sHeader, string sValue, bool bIsLastCol)  //table,header,data
        {
            _oPhrase = new Phrase();
            _oPhrase.Add(new Chunk(sHeader, this.GetCustomFont("AudiType-Bold", 9.5f)));
            _oPhrase.Add(new Chunk(sValue, this.GetCustomFont("AudiType-Normal", 9.5f)));
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
            if(!(string.IsNullOrEmpty(sHeader) && string.IsNullOrEmpty(sValue)))
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
            oPdfPTable.SetWidths(new float[] { 40f,440f,115f });

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

            _oPdfPCell = new PdfPCell(new Phrase((oModelFeature.FeatureType == EnumFeatureType.OptionalFeature ? "BDT "+Global.TakaFormat(oModelFeature.Price) : "Included" ),_oFontStyle));
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
        public PdfPTable PrintOptionTotalValue(PdfPTable oPdfPTable, string sHeader, double dTotalValue)
        {
            _oPdfPCell = new PdfPCell(new Phrase(sHeader, this.GetCustomFont("AudiType-Bold", 9f)));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.FixedHeight = 20f;
            _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("BDT " + Global.MillionFormat(dTotalValue), this.GetCustomFont("AudiType-Bold", 9f)));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            return oPdfPTable;
        }

        public PdfPTable PrintValue(PdfPTable oPdfPTable, string sHeader, double dTotalValue)
        {
          
            PdfPTable oPdfPTable_BDT = new PdfPTable(2);
            oPdfPTable_BDT.SetWidths(new float[] { 50, 160 });

            _oPdfPCell = new PdfPCell(new Phrase(sHeader, this.GetCustomFont("AudiType-Bold", 9f)));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.FixedHeight = 20f;
            _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell);

            #region BDT
            _oPdfPCell = new PdfPCell(new Phrase("BDT ", this.GetCustomFont("AudiType-Bold", 9f)));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.FixedHeight = 20f;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthRight = 0;
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
        public void PrintExteriorImage()
        {
            //if (_oVehicleOrderImages.Count > 0)
            //{
            //    _oPdfPCell = new PdfPCell();
            //    _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 50f;
            //    _oPdfPTable.AddCell(_oPdfPCell);
            //    _oPdfPTable.CompleteRow();


            //    PrintVehicleOrderImage(_oVehicleOrderImages[0]);
            //    _oPdfPCell = new PdfPCell();
            //    _oPdfPCell.Border = 0;
            //    _oPdfPTable.AddCell(_oPdfPCell);
            //    _oPdfPTable.CompleteRow();

            //    _oPdfPCell = new PdfPCell();
            //    _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 20f;
            //    _oPdfPTable.AddCell(_oPdfPCell);
            //    _oPdfPTable.CompleteRow();

            //    _oPdfPCell = new PdfPCell();
            //    _oPdfPCell.Border = 0;
            //    _oPdfPTable.AddCell(_oPdfPCell);
            //    PrintVehicleOrderImage(_oVehicleOrderImages[1]);
            //    _oPdfPCell = new PdfPCell();
            //    _oPdfPCell.Border = 0;
            //    _oPdfPTable.AddCell(_oPdfPCell);
            //    _oPdfPTable.CompleteRow();
            //}
            //else if(_oSalesQuotationImages.Count>0)
            //{
            //    _oPdfPCell = new PdfPCell();
            //    _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 50f;
            //    _oPdfPTable.AddCell(_oPdfPCell);
            //    _oPdfPTable.CompleteRow();

            //    PrintSalesQuotationImage(_oSalesQuotationImages[0]);
            //    _oPdfPCell = new PdfPCell();
            //    _oPdfPCell.Border = 0;
            //    _oPdfPTable.AddCell(_oPdfPCell);
            //    _oPdfPTable.CompleteRow();

            //    _oPdfPCell = new PdfPCell();
            //    _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 20f;
            //    _oPdfPTable.AddCell(_oPdfPCell);
            //    _oPdfPTable.CompleteRow();

            //    _oPdfPCell = new PdfPCell();
            //    _oPdfPCell.Border = 0;
            //    _oPdfPTable.AddCell(_oPdfPCell);
            //    PrintSalesQuotationImage(_oSalesQuotationImages[1]);
            //    _oPdfPCell = new PdfPCell();
            //    _oPdfPCell.Border = 0;
            //    _oPdfPTable.AddCell(_oPdfPCell);
            //    _oPdfPTable.CompleteRow();
            //}

            PrintEmptyRow(20);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleHeaderTitle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 50f; _oPdfPCell.Colspan = 3;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            
            #region first image
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleHeaderTitle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 200f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPTable.AddCell(_oPdfPCell);
            if (_oKommFileImages[0].TSImage != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oKommFileImages[0].TSImage, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(_fBodyWidth, 250f);
                _oPdfPCell = new PdfPCell(_oImag);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 200f;
                _oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleHeaderTitle)); //TitleColumn
                _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 200f;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPTable.AddCell(_oPdfPCell);                
            }
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleHeaderTitle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 200f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleHeaderTitle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 20f; _oPdfPCell.Colspan = 3;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();



            PrintEmptyRow(30);
            #region 2nd image
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleHeaderTitle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 200f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPTable.AddCell(_oPdfPCell);
            if (_oKommFileImages[0].TSImage != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oKommFileImages[1].TSImage, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(_fBodyWidth, 250f);
                _oPdfPCell = new PdfPCell(_oImag);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 200f;
                _oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleHeaderTitle)); //TitleColumn
                _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 200f;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPTable.AddCell(_oPdfPCell);
            }
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleHeaderTitle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 200f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleHeaderTitle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 15f; _oPdfPCell.Colspan = 3;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();



            //_oPdfPCell = new PdfPCell();
            //_oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 50f;
            //_oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();

            //PrintKommFileImage(_oKommFileImages[0]);
            //_oPdfPCell = new PdfPCell();
            //_oPdfPCell.Border = 0;
            //_oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();

            //_oPdfPCell = new PdfPCell();
            //_oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 20f;
            //_oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();

            //_oPdfPCell = new PdfPCell();
            //_oPdfPCell.Border = 0;
            //_oPdfPTable.AddCell(_oPdfPCell);
            //PrintKommFileImage(_oKommFileImages[1]);
            //_oPdfPCell = new PdfPCell();
            //_oPdfPCell.Border = 0;
            //_oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();

            PrintFeatureName("*Illustrated Models Shown, Actual Vehicle May Differ in Color and Specification Accuracy", _oFontStyleImageFooter);

        }
        public void PrintInteriorImage()
        {
            //if (_oVehicleOrderImages.Count > 0)
            //{
            //    _oPdfPCell = new PdfPCell();
            //    _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 50f;
            //    _oPdfPTable.AddCell(_oPdfPCell);
            //    _oPdfPTable.CompleteRow();

            //    PrintVehicleOrderImage(_oVehicleOrderImages[2]);
            //    _oPdfPCell = new PdfPCell();
            //    _oPdfPCell.Border = 0;
            //    _oPdfPTable.AddCell(_oPdfPCell);
            //    _oPdfPTable.CompleteRow();

            //    _oPdfPCell = new PdfPCell();
            //    _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 20f;
            //    _oPdfPTable.AddCell(_oPdfPCell);
            //    _oPdfPTable.CompleteRow();

            //    _oPdfPCell = new PdfPCell();
            //    _oPdfPCell.Border = 0;
            //    _oPdfPTable.AddCell(_oPdfPCell);
            //    PrintVehicleOrderImage(_oVehicleOrderImages[3]);
            //    _oPdfPCell = new PdfPCell();
            //    _oPdfPCell.Border = 0;
            //    _oPdfPTable.AddCell(_oPdfPCell);
            //    _oPdfPTable.CompleteRow();

            //}
            //else if(_oSalesQuotationImages.Count>0)
            //{
            //    _oPdfPCell = new PdfPCell();
            //    _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 50f;
            //    _oPdfPTable.AddCell(_oPdfPCell);
            //    _oPdfPTable.CompleteRow();

            //    PrintSalesQuotationImage(_oSalesQuotationImages[2]);
            //    _oPdfPCell = new PdfPCell();
            //    _oPdfPCell.Border = 0;
            //    _oPdfPTable.AddCell(_oPdfPCell);
            //    _oPdfPTable.CompleteRow();

            //    _oPdfPCell = new PdfPCell();
            //    _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 20f;
            //    _oPdfPTable.AddCell(_oPdfPCell);
            //    _oPdfPTable.CompleteRow();

            //    _oPdfPCell = new PdfPCell();
            //    _oPdfPCell.Border = 0;
            //    _oPdfPTable.AddCell(_oPdfPCell);
            //    PrintSalesQuotationImage(_oSalesQuotationImages[3]);
            //    _oPdfPCell = new PdfPCell();
            //    _oPdfPCell.Border = 0;
            //    _oPdfPTable.AddCell(_oPdfPCell);
            //    _oPdfPTable.CompleteRow();
            //}

            PrintEmptyRow(20);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleHeaderTitle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 50f; _oPdfPCell.Colspan = 3;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #region 3rd image
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleHeaderTitle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 200f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPTable.AddCell(_oPdfPCell);
            if (_oKommFileImages[2].TSImage != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oKommFileImages[2].TSImage, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(_fBodyWidth, 250f);
                _oPdfPCell = new PdfPCell(_oImag);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 200f;
                _oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleHeaderTitle)); //TitleColumn
                _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 200f;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPTable.AddCell(_oPdfPCell);
            }
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleHeaderTitle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 200f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleHeaderTitle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 20f; _oPdfPCell.Colspan = 3;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            PrintEmptyRow(40);
            #region 4th image
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleHeaderTitle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 200f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPTable.AddCell(_oPdfPCell);
            if (_oKommFileImages[3].TSImage != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oKommFileImages[3].TSImage, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(_fBodyWidth, 250f);
                _oPdfPCell = new PdfPCell(_oImag);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 200f;
                _oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleHeaderTitle)); //TitleColumn
                _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 200f;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPTable.AddCell(_oPdfPCell);
            }
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleHeaderTitle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 200f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleHeaderTitle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 15f; _oPdfPCell.Colspan = 3;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            //_oPdfPCell = new PdfPCell();
            //_oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 50f;
            //_oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();

            //PrintKommFileImage(_oKommFileImages[2]);
            //_oPdfPCell = new PdfPCell();
            //_oPdfPCell.Border = 0;
            //_oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();

            //_oPdfPCell = new PdfPCell();
            //_oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 20f;
            //_oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();

            //_oPdfPCell = new PdfPCell();
            //_oPdfPCell.Border = 0;
            //_oPdfPTable.AddCell(_oPdfPCell);
            //PrintKommFileImage(_oKommFileImages[3]);
            //_oPdfPCell = new PdfPCell();
            //_oPdfPCell.Border = 0;
            //_oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();

            PrintFeatureName("*Illustrated Models Shown, Actual Vehicle May Differ in Color and Specification Accuracy", _oFontStyleImageFooter);

        }
        public void PrintVehicleOrderImage(VehicleOrderImage oVehicleOrderImage)
        {
            PrintEmptyRow(55f);

            _oPdfPCell = new PdfPCell(new Phrase(""));
            if (oVehicleOrderImage.TSImage != null) 
            { 
                _oImag = iTextSharp.text.Image.GetInstance(oVehicleOrderImage.TSImage, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oPdfPCell = new PdfPCell(_oImag);
            }
            _oImag.ScaleToFit(_fBodyWidth, 250f);
           
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_BOTTOM;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 200f;
            _oPdfPTable.AddCell(_oPdfPCell);
        }
        public void PrintSalesQuotationImage(SalesQuotationImage oSalesQuotationImage)
        {           
            PrintEmptyRow(55f);

            _oPdfPCell = new PdfPCell(new Phrase(""));
            if (oSalesQuotationImage.TSImage != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(oSalesQuotationImage.TSImage, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oPdfPCell = new PdfPCell(_oImag);
            } 
            _oImag.ScaleToFit(_fBodyWidth, 250f);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_BOTTOM;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 200f;
            _oPdfPTable.AddCell(_oPdfPCell);
        }

        public void PrintKommFileImage(KommFileImage oKommFileImage)
        {
            PrintEmptyRow(55f);

            _oPdfPCell = new PdfPCell(new Phrase(""));
            if (oKommFileImage.TSImage != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(oKommFileImage.TSImage, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oPdfPCell = new PdfPCell(_oImag);
            }
            _oImag.ScaleToFit(180, 100f);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_BOTTOM;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 200f;
            _oPdfPTable.AddCell(_oPdfPCell);
        }
        #endregion

        #region PDF HELPER
        public Paragraph AddParagraphRow(string sHeader, string sData) 
        {
            PrintEmptyRow(15);
            //PrintFeatureName(sHeader, _oFontStyleRowHeader);
            //PrintEmptyCell();

            PdfPTable oTempPdfPTable = new PdfPTable(1);
            oTempPdfPTable.WidthPercentage = 100;

            Paragraph _oPdfParagraph;
            _oPdfPCell = new PdfPCell(new Phrase(sHeader, _oFontStyleRowHeader));
            _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oTempPdfPTable.AddCell(_oPdfPCell);
            oTempPdfPTable.CompleteRow();

            _oPdfParagraph = new Paragraph(new Phrase(sData, _oFontStyleLetterBody));
            _oPdfParagraph.SetLeading(0.5f, 2.2f);
            _oPdfParagraph.Alignment = Element.ALIGN_JUSTIFIED;
            _oPdfPCell.AddElement(_oPdfParagraph);
            _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oTempPdfPTable.AddCell(_oPdfPCell);
            oTempPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oTempPdfPTable);
            _oPdfPCell.Colspan = 2; _oPdfPCell.Border = 0;
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
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 40f;
            _oPdfPTable.AddCell(_oPdfPCell);
            #endregion

            _oPdfPTable.CompleteRow();
            _oPdfPCell = new PdfPCell(); //TitleColumn
            _oPdfPCell.Border = 0; 
            _oPdfPTable.AddCell(_oPdfPCell);

            //_oFontStyleHeaderTitle = FontFactory.GetFont("Tahoma", 14f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.RED);
            //_oPdfPCell = new PdfPCell(new Phrase("Audi",_oFontStyleHeaderTitle)); //TitleColumn
            //_oPdfPCell.Border = 0;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //_oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;

            _oFontStyleHeaderTitle = this.GetCustomFont("AudiType-Bold", 16f);
            _oPdfPCell = new PdfPCell(new Phrase("Progress Motors Imports Limited", _oFontStyleHeaderTitle)); //TitleColumn
            _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.Colspan = 2;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyleHeaderTitle = this.GetCustomFont("AudiType-Bold", 10f);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleHeaderTitle)); //TitleColumn
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 25f;
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
