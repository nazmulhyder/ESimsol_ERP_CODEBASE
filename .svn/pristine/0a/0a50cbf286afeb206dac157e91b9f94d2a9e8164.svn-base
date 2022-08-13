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
    public class rptSaleInvoice
    {
        #region Font Declaration
        Document _oDocument;
        iTextSharp.text.Image _oImag;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyleUBodyDetails;
        iTextSharp.text.Font _oFontStyleURowHeader;
        iTextSharp.text.Font _oFontStyleBodyDetails;
        iTextSharp.text.Font _oFontStyleCompanyDetails;
        iTextSharp.text.Font _oFontStyleRowHeader;
        iTextSharp.text.Font _oFontStyleColumnHeader;
        #endregion

        #region Declaration
        PdfPTable _oPdfPTable = new PdfPTable(3);
        PdfPCell _oPdfPCell;
        Phrase _oPhrase;
        MemoryStream _oMemoryStream = new MemoryStream();
        Company _oCompany = new Company();

        float _fCloumnHeader= 120f;
        float _fBodyWidth = 370f;
        float _fCompanyWidth = 100f;
        List _checkmark = new ZapfDingbatsList(52);

        SaleInvoice _oSaleInvoice = new SaleInvoice();
        #endregion

        public byte[] PrepareReport(Company oCompany, SaleInvoice oSaleInvoice)
        {
            _oCompany = oCompany;
            _oSaleInvoice = oSaleInvoice;
            //SetFontStyle For CustomFonts
            this.SetFontStyle();

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
            _oDocument.SetMargins(45f, 15f, 55f, 30f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

            _oFontStyle = _oFontStyleBodyDetails; //FontFactory.GetFont("Audi Type", 7f, 1);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { _fCloumnHeader, // To
                                                _fBodyWidth, //body, 
                                                _fCompanyWidth //Company
                                                });
            # endregion

            this.PrintCompanyHeader();
            this.PrintBody();

            _oPdfPTable.HeaderRows = 1;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();

            return _oMemoryStream.ToArray();;
        }

        public void PrintCompanyHeader()
        {
            PrintEmptyCell();
            PrintEmptyCell();
            PdfPTable oPdfPTableComDetail = new PdfPTable(2);
            oPdfPTableComDetail.WidthPercentage = 100;
            oPdfPTableComDetail.SetWidths(new float[] { 10f, _fCompanyWidth-10f });
            oPdfPTableComDetail.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTableComDetail.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

            #region Header Into Main Table
            _oFontStyleRowHeader = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("NOTICE OF VEHICLE SALE", _oFontStyleRowHeader));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 3;
            _oPdfPTable.CompleteRow(); 
            _oPdfPTable.AddCell(_oPdfPCell); 
            _oFontStyleRowHeader = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            #endregion

            #region CompanyLogo
            PrintEmptyCell(ref oPdfPTableComDetail);
            _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
            _oImag.ScaleAbsolute(40f, 25f);
            _oPdfPCell = new PdfPCell(_oImag);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 25f;
            oPdfPTableComDetail.AddCell(_oPdfPCell);
            oPdfPTableComDetail.CompleteRow();

            PrintEmptyCell(ref oPdfPTableComDetail);//1stCell
            _oFontStyleBodyDetails = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.RED);
            _oPdfPCell = new PdfPCell(new Phrase("Audi", _oFontStyleBodyDetails));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            oPdfPTableComDetail.AddCell(_oPdfPCell);
            oPdfPTableComDetail.CompleteRow();
            _oFontStyleBodyDetails = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            #endregion

            //AddTable(oPdfPTable,3);
            //_oPdfPTable.CompleteRow();
            //PrintEmptyRow(10);

            #region CompanyDetails

            PrintEmptyCell(ref oPdfPTableComDetail);//1stCell
            _oPdfPCell = new PdfPCell(new Phrase("")); _oPdfPCell.FixedHeight = 50f;
            _oPdfPCell.Border = 0; oPdfPTableComDetail.AddCell(_oPdfPCell); oPdfPTableComDetail.CompleteRow();
            _oFontStyle = _oFontStyleCompanyDetails;

            #region 1stPart
            PrintEmptyCell(ref oPdfPTableComDetail);//1stCell
            _oPdfPCell = new PdfPCell(new Phrase("Progress Motors Imports Limited", _oFontStyle)); _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTableComDetail.AddCell(_oPdfPCell); oPdfPTableComDetail.CompleteRow();

            PrintEmptyCell(ref oPdfPTableComDetail);//1stCell
            _oPdfPCell = new PdfPCell(new Phrase("225, Tejgaon C/A", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTableComDetail.AddCell(_oPdfPCell); oPdfPTableComDetail.CompleteRow();

            PrintEmptyCell(ref oPdfPTableComDetail);//1stCell
            _oPdfPCell = new PdfPCell(new Phrase("Dhaka 1208, Bangladesh", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTableComDetail.AddCell(_oPdfPCell); oPdfPTableComDetail.CompleteRow();

            PrintEmptyCell(ref oPdfPTableComDetail);//1stCell
            _oPdfPCell = new PdfPCell(new Phrase("Phone +880 2 9899872", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTableComDetail.AddCell(_oPdfPCell); oPdfPTableComDetail.CompleteRow();

            PrintEmptyCell(ref oPdfPTableComDetail);//1stCell
            _oPdfPCell = new PdfPCell(new Phrase("info@pmilbd.com", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTableComDetail.AddCell(_oPdfPCell); oPdfPTableComDetail.CompleteRow();

            PrintEmptyCell(ref oPdfPTableComDetail);//1stCell
            _oPdfPCell = new PdfPCell(new Phrase("www.audi.com.bd", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTableComDetail.AddCell(_oPdfPCell); oPdfPTableComDetail.CompleteRow();
            #endregion

            _oPdfPCell = new PdfPCell();
            _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2;
            oPdfPTableComDetail.AddCell(_oPdfPCell);

            #region 2ndstPart
            PrintEmptyCell(ref oPdfPTableComDetail);//1stCell
            _oPdfPCell = new PdfPCell(new Phrase("Audi Dhaka", _oFontStyle)); _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTableComDetail.AddCell(_oPdfPCell); oPdfPTableComDetail.CompleteRow();

            PrintEmptyCell(ref oPdfPTableComDetail);//1stCell
            _oPdfPCell = new PdfPCell(new Phrase("242 Gulshan Tejgaon Link Road", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTableComDetail.AddCell(_oPdfPCell); oPdfPTableComDetail.CompleteRow();

            PrintEmptyCell(ref oPdfPTableComDetail);//1stCell
            _oPdfPCell = new PdfPCell(new Phrase("Dhaka 1208, Bangladesh", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTableComDetail.AddCell(_oPdfPCell); oPdfPTableComDetail.CompleteRow();

            PrintEmptyCell(ref oPdfPTableComDetail);//1stCell
            _oPdfPCell = new PdfPCell(new Phrase("Phone +88 02 8878056", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTableComDetail.AddCell(_oPdfPCell); oPdfPTableComDetail.CompleteRow();

            PrintEmptyCell(ref oPdfPTableComDetail);//1stCell
            _oPdfPCell = new PdfPCell(new Phrase("Hotline +88 019 SHOP AUDI", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTableComDetail.AddCell(_oPdfPCell); oPdfPTableComDetail.CompleteRow();

            PrintEmptyCell(ref oPdfPTableComDetail);//1stCell
            _oPdfPCell = new PdfPCell(new Phrase("customer.service@pmilbd.com", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTableComDetail.AddCell(_oPdfPCell); oPdfPTableComDetail.CompleteRow();
            #endregion

            _oPdfPCell = new PdfPCell();
            _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2;
            oPdfPTableComDetail.AddCell(_oPdfPCell);

            #region 3rdstPart
            PrintEmptyCell(ref oPdfPTableComDetail);//1stCell
            _oPdfPCell = new PdfPCell(new Phrase("Audi Service", _oFontStyle)); _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTableComDetail.AddCell(_oPdfPCell); oPdfPTableComDetail.CompleteRow();

            PrintEmptyCell(ref oPdfPTableComDetail);//1stCell
            _oPdfPCell = new PdfPCell(new Phrase("429/432 , Tejgaon Industrial", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTableComDetail.AddCell(_oPdfPCell); oPdfPTableComDetail.CompleteRow();

            PrintEmptyCell(ref oPdfPTableComDetail);//1stCell
            _oPdfPCell = new PdfPCell(new Phrase("Area, Dhaka 1208, Bangladesh", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTableComDetail.AddCell(_oPdfPCell); oPdfPTableComDetail.CompleteRow();

            PrintEmptyCell(ref oPdfPTableComDetail);//1stCell
            _oPdfPCell = new PdfPCell(new Phrase("Phone +88 02 8891243", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTableComDetail.AddCell(_oPdfPCell); oPdfPTableComDetail.CompleteRow();

            PrintEmptyCell(ref oPdfPTableComDetail);//1stCell
            _oPdfPCell = new PdfPCell(new Phrase("Hotline +88 019 CALL AUDI", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTableComDetail.AddCell(_oPdfPCell); oPdfPTableComDetail.CompleteRow();

            PrintEmptyCell(ref oPdfPTableComDetail);//1stCell
            _oPdfPCell = new PdfPCell(new Phrase("service@pmilbd.com", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTableComDetail.AddCell(_oPdfPCell); oPdfPTableComDetail.CompleteRow();
            #endregion

            #endregion

            PrintEmptyCell();
            PrintEmptyCell();
            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTableComDetail);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            _oPdfPCell.Rowspan = 30;
            _oPdfPTable.AddCell(_oPdfPCell);
            #endregion

        }

        public void PrintBody()
        {
            #region CLIENT DETAILS
            this.PrintColumnHeader("CLIENT DETAILS");
            this.PrintRow("Name of Purchase", _oSaleInvoice.CustomerName);
            this.PrintRow("Company Name",_oSaleInvoice.CustomerName);
            this.PrintRow("Address",_oSaleInvoice.CustomerAddress);
            this.PrintRow("City",_oSaleInvoice.CustomerCity);
            this.PrintRow("Office/Home Landline",_oSaleInvoice.CustomerLandline);
            this.PrintRow("Cell Phone",_oSaleInvoice.CustomerCellPhone);
            #endregion

            #region VEHICLE DETAILS
            this.PrintColumnHeader("VEHICLE DETAILS");

            this.PrintRowHeader("New Order");
            this.NewOrder(_oSaleInvoice.IsNewOrder);
            this.PrintRowHeader("Vehicle location");
            this.VehicleLocation(_oSaleInvoice.VehicleLocation);
            this.PrintRow("Vehicle Chassis Number",_oSaleInvoice.ChassisNo);
            this.PrintRow("Vehicle Engine Number",_oSaleInvoice.EngineNo);
            this.PrintRow("Vehicle Komm Number",_oSaleInvoice.KommNo);
            this.PrintRow("Vehicle Model",_oSaleInvoice.ModelNo);
            this.PrintRow("Exterior Color",_oSaleInvoice.ExteriorColorName);
            this.PrintRow("Interior Color",_oSaleInvoice.InteriorColorName);
            this.PrintRow("Special order Pr-No",_oSaleInvoice.PRNo);
            this.PrintRow("Special Instruction",_oSaleInvoice.SpecialInstruction);
            #endregion
            
            #region VEHICLE DELIVERY AGGREMENT
            this.PrintColumnHeader("VEHICLE DELIVERY AGGREMENT");
            this.PrintRowHeader("ETA Agreement");
            this.PrintETAAgreement();
            #endregion

            #region PRICE
            this.PrintColumnHeader("PRICE");
            this.PrintRowHeader("Vehicle sale amount");
            this.PrintVechileSaleAmount();
            #endregion

            #region VEHICLE DELIVERY AGGREMENT
            this.PrintColumnHeader("PAYMENT TERMS & DETAILS");
            this.PrintRow("Purchase order number",_oSaleInvoice.SQNo);
            this.PrintRowHeader("Advance payment");
            this.PrintAdvanceAmount();
            this.PrintRow("Advance payment date",_oSaleInvoice.AdvanceDateST);
            this.PrintRow("Advance payment amount", Global.MillionFormat(_oSaleInvoice.AdvanceAmount) + "/-");
            //this.PrintRow("Balance payment amount",_oSaleInvoice.);
            this.PrintRow("Balance payment due", Global.MillionFormat((_oSaleInvoice.NetAmount-_oSaleInvoice.AdvanceAmount)) + "/-");
            this.PrintRow("Cheque/Payorder No",_oSaleInvoice.ChassisNo);
            this.PrintRow("Cheque/Payorder Date",_oSaleInvoice.ChequeDateST);
            this.PrintRow("Bank",_oSaleInvoice.BankName);
            this.PrintRow("Received on",_oSaleInvoice.ReceivedOn);
            this.PrintRow("Received by",_oSaleInvoice.ReceivedByName);
            this.PrintRow("Vehicle sold by",_oSaleInvoice.MarketingAccountName);
            this.PrintRow("Approved by",_oSaleInvoice.ApprovedByName);
            this.PrintRow("Comments",_oSaleInvoice.Remarks);
            //this.PrintRow("",_oSaleInvoice.);
           #endregion

            this.PrintEmptyRow(15f);
            this.PrintAttachments();
        }

        #region PRINT Functions
        public void PrintETAAgreement()
        {
            PdfPTable oPdfPTable = new PdfPTable(4);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
    
            float nSetWidths = (_fBodyWidth - 10f) / 4;
            oPdfPTable.SetWidths(new float[] { 10f, nSetWidths*2+30f , nSetWidths-30f, nSetWidths});

            AddRowData(ref oPdfPTable, ": ", false);
            AddRowData(ref oPdfPTable, _oSaleInvoice.ETAAgreement, false);
            AddRowHeader(ref oPdfPTable, "ETA Weeks:");
            AddRowData(ref oPdfPTable, _oSaleInvoice.ETAWeeks+"Weeks ", true);
            oPdfPTable.CompleteRow();
            AddTable(oPdfPTable, 0);
        }
        public void PrintVechileSaleAmount()
        {
            PdfPTable oPdfPTable = new PdfPTable(5);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

            float nSetWidths = (_fBodyWidth - 10f) / 3;
            oPdfPTable.SetWidths(new float[] { 10f, nSetWidths / 2, nSetWidths / 2, nSetWidths - 70f, nSetWidths + 70f });

            AddRowData(ref oPdfPTable, ": ", false);
            AddRowData(ref oPdfPTable, _oSaleInvoice.CurrencyName, true);
            AddRowHeader(ref oPdfPTable, "Currency  ");
            AddRowHeader(ref oPdfPTable, "      Amount:");
            AddRowData(ref oPdfPTable, Global.MillionFormat(_oSaleInvoice.NetAmount) + "/-", true);

            oPdfPTable.CompleteRow();
            AddTable(oPdfPTable, 0);
        }
        public void PrintAdvanceAmount()
        {
            PdfPTable oPdfPTable = new PdfPTable(7);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

            float nSetWidths = (_fBodyWidth - 10f) / 2;
            oPdfPTable.SetWidths(new float[] { 10f, nSetWidths / 3, nSetWidths / 3, nSetWidths / 3-8f, 8f, nSetWidths-95f, nSetWidths +95f });

            AddRowData(ref oPdfPTable, ": ", false);
            AddRowData(ref oPdfPTable, _oSaleInvoice.CurrencyName, true);
            AddRowHeader(ref oPdfPTable, "Currency  ");
            AddRowData(ref oPdfPTable, Global.MillionFormat_Round((_oSaleInvoice.AdvanceAmount / _oSaleInvoice.OTRAmount) * 100) + " ", true);
            AddRowData(ref oPdfPTable," %   ", false);
            AddRowHeader(ref oPdfPTable, "      Amount:");
            AddRowData(ref oPdfPTable, Global.MillionFormat(_oSaleInvoice.AdvanceAmount) + "/-", true);

            oPdfPTable.CompleteRow();

            AddTable(oPdfPTable,0);
            _oPdfPTable.CompleteRow();
        }
        public void NewOrder(bool IsNewOrder)
        {
            PdfPTable oPdfPTable = new PdfPTable(9);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

            float nSetWidths = (_fBodyWidth - 10f) / 4;
            oPdfPTable.SetWidths(new float[] { 10f, nSetWidths - 70f, 70f, nSetWidths - 70f, 70f, nSetWidths - 70f, 70f, nSetWidths - 70f, 70f });

            AddRowData(ref oPdfPTable, ": ", false);

            #region Yes
            if(IsNewOrder)
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBodyDetails));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                _checkmark.Add(" "); _oPdfPCell.FixedHeight=12.7f;
                _oPdfPCell.AddElement(_checkmark);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBodyDetails));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            }
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell);

            AddRowData(ref oPdfPTable, "Yes", false);

            #endregion

            #region No
            if (!IsNewOrder)
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBodyDetails));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                _checkmark.Add(" "); _oPdfPCell.FixedHeight = 12.7f;
                _oPdfPCell.AddElement(_checkmark);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBodyDetails));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            }
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell);

            AddRowData(ref oPdfPTable, "No", false);
            #endregion

            oPdfPTable.CompleteRow();
            AddTable(oPdfPTable, 0);
            _oPdfPTable.CompleteRow();
        }
        public void VehicleLocation(int nLocation)
        {
            PdfPTable oPdfPTable = new PdfPTable(9);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            float nSetWidths = (_fBodyWidth - 10f) / 4;
            oPdfPTable.SetWidths(new float[] { 10f, nSetWidths - 70f, 70f, nSetWidths - 70f, 70f, nSetWidths - 70f, 70f, nSetWidths - 70f, 70f });


            AddRowData(ref oPdfPTable, ": ", false);

            #region Facotry
            if (nLocation==(int)EnumVehicleLocation.Factory)
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBodyDetails));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                _checkmark.Add(" "); _oPdfPCell.FixedHeight = 12.7f;
                _oPdfPCell.AddElement(_checkmark);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBodyDetails));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            }
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell);

            AddRowData(ref oPdfPTable, "Factory", false);

            #endregion

            #region In-transit
            if (nLocation == (int)EnumVehicleLocation.In_transit)
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBodyDetails));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                _checkmark.Add(" "); _oPdfPCell.FixedHeight = 12.7f;
                _oPdfPCell.AddElement(_checkmark);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBodyDetails));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            }
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell);

            AddRowData(ref oPdfPTable, "In-transit", false);

            #endregion

            #region Showroorm
            if (nLocation == (int)EnumVehicleLocation.Showroom)
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBodyDetails));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                _checkmark.Add(" "); _oPdfPCell.FixedHeight = 12.7f;
                _oPdfPCell.AddElement(_checkmark);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBodyDetails));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            }
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell);

            AddRowData(ref oPdfPTable, "Showroom", false);

            #endregion

            #region In_Production
            if (nLocation == (int)EnumVehicleLocation.In_Production)
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBodyDetails));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                _checkmark.Add(" "); _oPdfPCell.FixedHeight = 12.7f;
                _oPdfPCell.AddElement(_checkmark);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBodyDetails));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            }
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell);

            AddRowData(ref oPdfPTable, "In Production", false);

            #endregion

            oPdfPTable.CompleteRow();
            AddTable(oPdfPTable, 0);
            _oPdfPTable.CompleteRow();
        }
        public void PrintAttachments() 
        {
            _oPdfPCell = new PdfPCell(new Phrase("Attachments", _oFontStyleURowHeader));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);

            PdfPTable oPdfPTable = new PdfPTable(9);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

            float nSetWidths = (_fBodyWidth - 10f) / 4;
            oPdfPTable.SetWidths(new float[] { 10f, nSetWidths - 70f, 70f, nSetWidths - 70f, 70f, nSetWidths - 70f, 70f, nSetWidths - 70f, 70f });

            AddRowData(ref oPdfPTable, ": ", false);

            #region PI
            if (_oSaleInvoice.AttachmentDoc==1)
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBodyDetails));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                _checkmark.Add(" "); _oPdfPCell.FixedHeight = 12.7f;
                _oPdfPCell.AddElement(_checkmark);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBodyDetails));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            }
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell);

            AddRowData(ref oPdfPTable, "Proforma Invoice", false);

            #endregion

            #region Cheque/PO
            if (_oSaleInvoice.AttachmentDoc == 2)
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBodyDetails));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                _checkmark.Add(" "); _oPdfPCell.FixedHeight = 12.7f;
                _oPdfPCell.AddElement(_checkmark);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBodyDetails));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            }
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell);

            AddRowData(ref oPdfPTable, "Cheque/PO", false);
            #endregion

            oPdfPTable.CompleteRow();
            AddTable(oPdfPTable, 0);
            _oPdfPTable.CompleteRow();
        }
        #endregion

        #region PDF HELPER
        public void PrintRow(string sHeader,string sData)
        {
            this.PrintRowHeader(sHeader);
            this.PrintRowData(sData);
            this.PrintEmptyCell();
            _oPdfPTable.CompleteRow();
        }
        public void PrintRowHeader(string sHeader)
        {
            _oPdfPCell = new PdfPCell(new Phrase(sHeader, _oFontStyleRowHeader));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
        }
        public void PrintRowData(string sData)
        {
            PdfPTable oPdfPTable = new PdfPTable(2);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            oPdfPTable.SetWidths(new float[] { 10f,_fBodyWidth-10f});
            AddRowData(ref oPdfPTable, ": ",false);
            AddRowData(ref oPdfPTable, sData,true);
            AddTable(oPdfPTable,0);
        }
        public void PrintEmptyCell()
        {
            _oPdfPCell = new PdfPCell();
            _oPdfPCell.Border = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
        }
        public void PrintEmptyCell(ref PdfPTable oPdfPTable)
        {
            _oPdfPCell = new PdfPCell();
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);
        }
        public void PrintColumnHeader(string sHeader)
        {
            this.PrintEmptyRow(10);
             
            #region HEADER
                _oPhrase = new Phrase();
                _oPhrase.Add(new Chunk(sHeader, _oFontStyleRowHeader));
                _oPdfPCell = new PdfPCell(new Phrase(sHeader, _oFontStyleColumnHeader));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.Colspan = 3;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 25f;
                _oPdfPTable.AddCell(_oPdfPCell);
                #endregion
         
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
        public void AddRowHeader(ref PdfPTable oPdfPTable, string sHeader)
        {
            _oPdfPCell = new PdfPCell(new Phrase(sHeader, _oFontStyleRowHeader));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell);
        }
        public void AddRowData(ref PdfPTable oPdfPTable, string sData,bool isBorder)
        {
            _oPdfPCell = new PdfPCell(new Phrase(sData, _oFontStyleBodyDetails));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            _oPdfPCell.BorderWidthTop = _oPdfPCell.BorderWidthLeft = _oPdfPCell.BorderWidthRight = _oPdfPCell.BorderWidthBottom=0;
            if(isBorder)
                _oPdfPCell.BorderWidthBottom = 1f;
         
             _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell);
        }
        public void AddTable(PdfPTable oPdfPTable,int nColspan)
        {
            _oPdfPCell = new PdfPCell();
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            _oPdfPCell.AddElement(oPdfPTable);
            _oPdfPCell.Colspan = nColspan;
            _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();
        }
        #endregion

        public void SetFontStyle()
        {
            _oFontStyleUBodyDetails = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL | iTextSharp.text.Font.UNDERLINE);
            _oFontStyleBodyDetails = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            _oFontStyleCompanyDetails = FontFactory.GetFont("Tahoma", 5f, iTextSharp.text.Font.NORMAL);
            _oFontStyleRowHeader = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            _oFontStyleURowHeader = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
            _oFontStyleColumnHeader = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
        }
        public iTextSharp.text.Font GetCustomFont(string fontName, float fFontSize)
        {
            string fontpath = System.Web.Hosting.HostingEnvironment.MapPath("~/fonts/");
            BaseFont customfont = BaseFont.CreateFont(fontpath + fontName + ".ttf", BaseFont.CP1252, BaseFont.EMBEDDED);
            iTextSharp.text.Font oFont = new iTextSharp.text.Font(customfont, fFontSize);
            //iTextSharp.text.FontFactory.Register(fontpath + fontName + ".ttf");
            return oFont;
        }
    }
}
