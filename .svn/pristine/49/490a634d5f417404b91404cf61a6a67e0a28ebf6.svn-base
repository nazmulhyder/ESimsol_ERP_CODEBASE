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
using ICS.Core.Framework;
using System.Linq;
namespace ESimSol.Reports
{
   public class rptSalesBankQuotation
    {
        #region Declaration
        Document _oDocument;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        MemoryStream _oMemoryStream = new MemoryStream();
        SalesQuotation _oSalesQuotation = new SalesQuotation();
        #endregion

        public byte[] PrepareReport(SalesQuotation oSalesQuotation,PartyWiseBank oPartyWiseBank, string PrintDate, Company oCompany)
        {
            _oSalesQuotation = oSalesQuotation;
            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetMargins(20f, 20f, 10f, 10f);

            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            _oDocument.Open();

            _oPdfPTable.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.SetWidths(new float[] { 595f });
            #endregion

            this.PrintHeader(oCompany);
            this.PrintBody(oPartyWiseBank, PrintDate);
            _oPdfPTable.HeaderRows = 1;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Header
        public void PrintHeader(Company oCompany)
        {
            PdfPTable oPdfPTableComDetail = new PdfPTable(3);
            oPdfPTableComDetail.SetWidths(new float[] { 120f, 355f, 120f });

            #region 1st Row

            ESimSolPdfHelper.SetCustomFont("AudiType-ExtendedBold", 12);
            ESimSolPdfHelper.AddCell(ref oPdfPTableComDetail, "Audi Dhaka", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0,0,3,0);

            ESimSolPdfHelper.AddCell(ref oPdfPTableComDetail, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTableComDetail, "Progress Motors Imports Limited", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0);

            #region CompanyLogo
            iTextSharp.text.Image oImag = iTextSharp.text.Image.GetInstance(oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);

            if (oImag != null)
            {
                oImag.ScaleAbsolute(65f, 30f);
                _oPdfPCell = new PdfPCell(oImag);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 40f;
                oPdfPTableComDetail.AddCell(_oPdfPCell);
            }
            else
                ESimSolPdfHelper.AddCell(ref oPdfPTableComDetail, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0);
            #endregion

            #endregion

            #region 2nd Row

            ESimSolPdfHelper.FontStyle.IsUnderlined();
            ESimSolPdfHelper.AddCell(ref oPdfPTableComDetail, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTableComDetail, "Offer/Quotation", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTableComDetail, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0);
            #endregion

            ESimSolPdfHelper.AddCell(ref oPdfPTableComDetail, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0,0,3,0);

            #region Insert Into Main Table
            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTableComDetail, iTextSharp.text.Rectangle.NO_BORDER, 0, 0);
            #endregion
        }
        #endregion

        #region Report Body
        private void PrintBody(PartyWiseBank oPartyWiseBank, string sPrintDate)
        {
            PdfPTable oPdfPTable_Body = new PdfPTable(2);
            oPdfPTable_Body.SetWidths(new float[] { 475, 100f });

            #region 1st Table
            PdfPTable oPdfPTable = new PdfPTable(6);
            oPdfPTable.SetWidths(new float[] { 40f, 100f, 220f, 50f,65f,100f});

            ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0, 0, 6, 10);
            
            #region Ref & Date
            ESimSolPdfHelper.SetCustomFont("AudiType-Bold", 9);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Ref", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0);
            ESimSolPdfHelper.SetCustomFont("AudiType-Normal", 9);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, _oSalesQuotation.RefNo);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0);

            ESimSolPdfHelper.SetCustomFont("AudiType-Bold", 9);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Date: ", Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0);
            ESimSolPdfHelper.SetCustomFont("AudiType-Normal", 9);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, sPrintDate, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
          //  ESimSolPdfHelper.AddCell(ref oPdfPTable,_oSalesQuotation.QuotationDateInString, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
            
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0);

            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable, iTextSharp.text.Rectangle.NO_BORDER, 0, 0);
            #endregion

            oPdfPTable = new PdfPTable(5);
            oPdfPTable.SetWidths(new float[] { 40f, 190f, 100f, 50f, 95f });

            int nHeight = 60;
            #region To & Bank
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0, 0, 5, 8);
            ESimSolPdfHelper.SetCustomFont("AudiType-Bold", 9f);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "To", Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 0, 0, 0, nHeight);
            ESimSolPdfHelper.SetCustomFont("AudiType-Normal", 9);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, oPartyWiseBank.BankName+'\n'+oPartyWiseBank.BranchName, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15, 0, 0, nHeight);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0, 0, 3, nHeight);
            #endregion

            ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0, 0, 5, 8);
            #region A/C
            ESimSolPdfHelper.SetCustomFont("AudiType-Bold", 9f);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "A/C", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0, 0, 0, 0);
            ESimSolPdfHelper.SetCustomFont("AudiType-Normal", 9);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, oPartyWiseBank.AccountName+" ["+oPartyWiseBank.AccountNo+']', Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0, 0, 0, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0, 0, 3, 0);
            #endregion

            ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0, 0, 5, 8);
            #region Dear Sir/Madam
            ESimSolPdfHelper.SetCustomFont("AudiType-Bold", 9f);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Dear Sir/Madam,", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0, 0, 5, 25);

            ESimSolPdfHelper.SetCustomFont("AudiType-Normal", 9);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "In reference to our conversation earlier, please find our best offer for the AUDI you have inquired", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0, 0, 5, 0);

            ESimSolPdfHelper.SetCustomFont("AudiType-Bold", 10f);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "BRAND NEW VEHICLE", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0, 0, 5, 0);
            #endregion

            ESimSolPdfHelper.AddTable(ref oPdfPTable_Body, oPdfPTable, 0);

            #region CompanyDetails
            PdfPTable oPdfPTable_CompanyDetails = new PdfPTable(1);
            oPdfPTable_CompanyDetails.SetWidths(new float[] { 100f });
            ESimSolPdfHelper.SetCustomFont("AudiType-Normal", 6f);
            

            ESimSolPdfHelper.AddCell(ref oPdfPTable_CompanyDetails, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0,0,0,25);
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 5.5f, iTextSharp.text.Font.BOLD);
            ESimSolPdfHelper.AddCell(ref oPdfPTable_CompanyDetails, "Progress Motors Imports Limited", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0);
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL);
            ESimSolPdfHelper.AddCell(ref oPdfPTable_CompanyDetails, "225, Tejgaon C/A", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable_CompanyDetails, "Dhaka 1208, Bangladesh", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable_CompanyDetails, "Phone +880 2 9899872", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable_CompanyDetails, "info@pmilbd.com", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable_CompanyDetails, "www.audi.com.bd", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0);

            ESimSolPdfHelper.AddCell(ref oPdfPTable_CompanyDetails, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0);
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 5.5f, iTextSharp.text.Font.BOLD);
            ESimSolPdfHelper.AddCell(ref oPdfPTable_CompanyDetails, "Audi Dhaka", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0);
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL);
            ESimSolPdfHelper.AddCell(ref oPdfPTable_CompanyDetails, "242 Gulshan Tejgaon Link Road", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable_CompanyDetails, "Dhaka 1208, Bangladesh", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable_CompanyDetails, "Phone +88 02 8878056", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable_CompanyDetails, "Hotline +88 019 SHOP AUDI", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable_CompanyDetails, "customer.service@pmilbd.com", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0);


            ESimSolPdfHelper.AddCell(ref oPdfPTable_CompanyDetails, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0);
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 5.5f, iTextSharp.text.Font.BOLD);
            ESimSolPdfHelper.AddCell(ref oPdfPTable_CompanyDetails, "Audi Service", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0);
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL);
            ESimSolPdfHelper.AddCell(ref oPdfPTable_CompanyDetails, "429/432 , Tejgaon Industrial", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable_CompanyDetails, "Area, Dhaka 1208, Bangladesh", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable_CompanyDetails, "Phone +88 02 8891243", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable_CompanyDetails, "Hotline +88 019 CALL AUDI", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable_CompanyDetails, "service@pmilbd.com", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0);
            #endregion

            ESimSolPdfHelper.AddTable(ref oPdfPTable_Body, oPdfPTable_CompanyDetails, 0, 0, 0);
            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable_Body, 0, 0, 0);
            #endregion

            ESimSolPdfHelper.PrintHeaderInfo(ref _oPdfPTable, "", "", new string[][] {
                                    new string[] {"Make","Model","Model Year", "Year Of Manufacture","Engine Type","Transmission","C.C","Seating Capacity"},
                                    new string[] {"AUDI",_oSalesQuotation.ModelNo,_oSalesQuotation.YearOfModel,  _oSalesQuotation.YearOfManufacture, _oSalesQuotation.EngineType,_oSalesQuotation.Transmission, _oSalesQuotation.Capacity,_oSalesQuotation.SeatingCapacity}
                                });

            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0);

            #region Chassis, Engine & Color
            oPdfPTable = new PdfPTable(6);
            oPdfPTable.SetWidths(new float[] { 40f, 180f, 60f, 200f, 35f, 80f });

            int nFontSize = 9;
            ESimSolPdfHelper.SetCustomFont("AudiType-Bold", nFontSize);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Chassis", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0);
            ESimSolPdfHelper.SetCustomFont("AudiType-Normal", nFontSize);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, _oSalesQuotation.ChassisNo, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);

            ESimSolPdfHelper.SetCustomFont("AudiType-Bold", nFontSize);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "  Engine No", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0);
            ESimSolPdfHelper.SetCustomFont("AudiType-Normal", nFontSize);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, _oSalesQuotation.EngineNo, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);

            ESimSolPdfHelper.SetCustomFont("AudiType-Bold", nFontSize);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "  Color", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0);
            ESimSolPdfHelper.SetCustomFont("AudiType-Normal", nFontSize);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, _oSalesQuotation.ExteriorColorName, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
            #endregion

            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable, 0);

            #region Details
            oPdfPTable_Body = new PdfPTable(3);
            oPdfPTable_Body.SetWidths(new float[] { 235f, 10f, 310f });

            oPdfPTable = new PdfPTable(2);
            oPdfPTable.SetWidths(new float[] { 75f, 180f});

            ESimSolPdfHelper.SetCustomFont("AudiType-Bold", 9f);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0,0,4,15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, " ", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0);
            ESimSolPdfHelper.SetCustomFont("AudiType-Normal", 9f);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, " ", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0);
        
            ESimSolPdfHelper.SetCustomFont("AudiType-Bold", 9f);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0, 0, 4, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, " ", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0);
            ESimSolPdfHelper.SetCustomFont("AudiType-Normal", 9f);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, " ", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0);
     
            ESimSolPdfHelper.SetCustomFont("AudiType-Bold", 9f);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0, 0, 4, 0,15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Total Tk.", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0,0,0,0,15);
            ESimSolPdfHelper.SetCustomFont("AudiType-Normal", 9f);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.TakaFormat(_oSalesQuotation.PrintOTRAmount), Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15,0,0,0,15);
            
            ESimSolPdfHelper.AddTable(ref oPdfPTable_Body, oPdfPTable, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable_Body, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0);

            oPdfPTable = new PdfPTable(1);
            oPdfPTable.SetWidths(new float[] { 310f });
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0, 0, 0, 15);
            string sPaymentTerm = "100% Payment to be made at the time of taking delivery by Cash/Pay Order";//_oSalesQuotation.PaymentTerm
            this.AddChunkRow(ref oPdfPTable, "Payment: ", sPaymentTerm, ESimSolPdfHelper.GetCustomFont("AudiType-Bold", 9f), ESimSolPdfHelper.GetCustomFont("AudiType-Normal", 9f));
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0, 0, 0, 0);
            //_oSalesQuotation.DeliveryDescription
            string sDelivery = " Ready stock.";
            this.AddChunkRow(ref oPdfPTable, "Delivery: ", sDelivery, ESimSolPdfHelper.GetCustomFont("AudiType-Bold", 9f), ESimSolPdfHelper.GetCustomFont("AudiType-Normal", 9f));
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0, 0, 0, 0);
            string sValidity = "This offer is valid for 21 days from the date of offer (subject to avilability -from ready stock)";//_oSalesQuotation.ValidityOfOffer
            this.AddChunkRow(ref oPdfPTable, "Validity: ", sValidity, ESimSolPdfHelper.GetCustomFont("AudiType-Bold", 9f), ESimSolPdfHelper.GetCustomFont("AudiType-Normal", 9f));
            ESimSolPdfHelper.AddTable(ref oPdfPTable_Body, oPdfPTable, 0);
            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable_Body, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, 0);

            oPdfPTable = new PdfPTable(2);
            oPdfPTable.SetWidths(new float[] { 75f, 520f });
            ESimSolPdfHelper.SetCustomFont("AudiType-Bold", 9f);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0, 0, 2, 0,25);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Total in Words", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0);
            ESimSolPdfHelper.SetCustomFont("AudiType-Normal", 9f);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, (_oSalesQuotation.TotalAmount > 0 ? Global.TakaWords(_oSalesQuotation.TotalAmount) : ""), Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15, 0, 0, 0);

            ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0, 0, 2, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0);
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8.0f, iTextSharp.text.Font.ITALIC | iTextSharp.text.Font.UNDERLINE);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, ("this quoted price is exclusive of insurANCE, sOURCE TAX/ ait etc OF VEHICLE").ToUpper(), Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER, 0, 0, 0);

            ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0, 0, 2, 18);

            ESimSolPdfHelper.SetCustomFont("AudiType-Normal", 10f);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "With best regards,", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0, 0, 2, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Progress Motors Imports Limited", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0, 0, 2, 0);

            ESimSolPdfHelper.SetCustomFont("AudiType-Bold", 9f);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "_______________________________", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0, 0, 2, 25);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Authorized Signature", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0, 0, 2, 0);


            #endregion

            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable, 0);
        }
        #endregion

        public string MilionFormat(double value) 
        {
            return (value>0?Global.MillionFormat(value):"");
        } 
        public void AddChunkRow(ref PdfPTable oPdfPTable, string sHeader, string sValue, iTextSharp.text.Font oFontStyleRowHeader, iTextSharp.text.Font oFontStyleData)  //table,header,data,headerfont,datafont
        {
            Phrase _oPhrase = new Phrase();
            _oPhrase.Add(new Chunk(sHeader, oFontStyleRowHeader));
            _oPhrase.Add(new Chunk(sValue, oFontStyleData));
            _oPdfPCell = new PdfPCell(_oPhrase); _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 15; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.MinimumHeight = 30f;
            oPdfPTable.AddCell(_oPdfPCell); oPdfPTable.CompleteRow();
        }
       
    }
}
