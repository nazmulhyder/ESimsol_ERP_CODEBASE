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
    public class rptTradingSaleInvoice
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        TradingSaleInvoice _oTradingSaleInvoice = new TradingSaleInvoice();
        List<TradingSaleInvoiceDetail> _oTradingSaleInvoiceDetails = new List<TradingSaleInvoiceDetail>();
        List<TradingSaleInvoice> _oTradingSaleInvoices = new List<TradingSaleInvoice>();
        Company _oCompany = new Company();
        #endregion

        public byte[] PrepareReport(TradingSaleInvoice oTradingSaleInvoice, Company oCompany, List<TradingSaleInvoice> oTradingSaleInvoices)
        {
            _oTradingSaleInvoice = oTradingSaleInvoice;
            _oTradingSaleInvoiceDetails = oTradingSaleInvoice.TradingSaleInvoiceDetails;
            _oTradingSaleInvoices = oTradingSaleInvoices;
            _oCompany = oCompany;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            _oDocument.SetMargins(30f, 30f, 5f, 30f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();

            _oPdfPTable.SetWidths(new float[] { 535f });
            #endregion

            this.PrintHeader();
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

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Address + "\n" + _oCompany.Phone + ";  " + _oCompany.Email + ";  " + _oCompany.WebAddress, _oFontStyle));

            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region ReportHeader
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));

            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 5f;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region ReportHeader
            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Sale Invoice", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
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
            #region Purchase Invoice Information

            PdfPTable oPdfPTable = new PdfPTable(6);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            oPdfPTable.SetWidths(new float[] { 70f, 151f, 70f, 123f, 60f, 60f });
            _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.NORMAL);

            #region 1st Row
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Invocie No ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(": " + _oTradingSaleInvoice.InvoiceNo, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Sales Type:", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oTradingSaleInvoice.SalesTypeSt, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Invocie Date :", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oTradingSaleInvoice.InvoiceDateInString, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region 2nd Row
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Customer Name", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(": " + _oTradingSaleInvoice.BuyerName, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Contact Person:", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oTradingSaleInvoice.ContactPersonName, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Currency :", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oTradingSaleInvoice.CurrencyName, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region 3rd Row
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Remarks ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(": " + _oTradingSaleInvoice.Note, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 5; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.ExtraParagraphSpace = 7f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Purchase Invoice Detail
            oPdfPTable = new PdfPTable(8);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            oPdfPTable.SetWidths(new float[]{ 
                                                20f, //SL No
                                                50f, //Product Code
                                                120f, //Product Name 
                                                120f, //Item Description
                                                50f, //Unit
                                                50f, //Invocie Qty
                                                50f, //Unit Price
                                                60f //Amount
                                            });



            #region Hader
            #region Title
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Colspan = 8;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            _oPdfPCell.FixedHeight = 8;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Item Descriptions", _oFontStyle));
            _oPdfPCell.Colspan = 8;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            _oPdfPCell.FixedHeight = 12;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Code", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Product Name", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Item Description", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Unit", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Invocie Qty", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("Unit Price", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Amount", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Detail Data
            int nCount = 0;
            _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.NORMAL);
            foreach (TradingSaleInvoiceDetail oItem in _oTradingSaleInvoiceDetails)
            {
                nCount++;
                #region Table Initialize
                oPdfPTable = new PdfPTable(8);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                oPdfPTable.SetWidths(new float[]{ 
                                                20f, //SL No
                                                50f, //Product Code
                                                120f, //Product Name 
                                                120f, //Item Description
                                                50f, //Unit
                                                50f, //Invocie Qty
                                                50f, //Unit Price
                                                60f //Amount
                                            });
                #endregion

                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductCode, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ItemDescription, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.UnitName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.InvoiceQty), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.UnitPrice), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Amount), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion

            #region Summery
            #region Table Initialize
            oPdfPTable = new PdfPTable(8);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            oPdfPTable.SetWidths(new float[]{ 
                                                20f, //SL No
                                                50f, //Product Code
                                                120f, //Product Name 
                                                120f, //Item Description
                                                50f, //Unit
                                                50f, //Invocie Qty
                                                50f, //Unit Price
                                                60f //Amount
                                            });
            #endregion

            #region Gross Amount
            _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Gross Amount :", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 7; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oTradingSaleInvoice.GrossAmount), _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion
            if (_oTradingSaleInvoices.Count > 0)
            {
                #region Discount Amount
                _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase("Discount Amount :", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 7; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oTradingSaleInvoice.DiscountAmount), _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
                #endregion

                #region Service Charge
                _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase("Vat Amount:", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 7; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oTradingSaleInvoice.VatAmount), _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
                #endregion

                #region Service Charge
                _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase("Service Charge:", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 7; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oTradingSaleInvoice.ServiceCharge), _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
                #endregion

                #region Line
                _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("-----------------------------------------------", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 8; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
                #endregion

                #region Net Amount
                _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("Net Amount :", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 7; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oTradingSaleInvoice.NetAmount), _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
                #endregion

                #region Previous Due
                double nTotalDueAmount = 0;
                foreach (TradingSaleInvoice oItem in _oTradingSaleInvoices)
                {
                    nTotalDueAmount = nTotalDueAmount + oItem.DueAmount;
                }

                _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("Previous Due :", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 7; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalDueAmount), _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
                #endregion

                #region Line
                _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("-----------------------------------------------", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 8; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
                #endregion

                #region Total Due

                _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("Total Due :", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 7; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat((nTotalDueAmount + _oTradingSaleInvoice.NetAmount)), _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
                #endregion
            }

            #region Approved By
            _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(_oTradingSaleInvoice.ApprovedByName + "\n---------------------\nApproved By", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 8; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #endregion

            #region Previous Dues
            //if (_oTradingSaleInvoices.Count > 0)
            //{
            //    double nTotalInvocieAmount = 0, nTotalPaymentAmount = 0, nTotalDueAmount = 0;
            //    oPdfPTable = new PdfPTable(7);
            //    //oPdfPTable.WidthPercentage = 100;
            //    oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            //    oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //    oPdfPTable.SetWidths(new float[]{ 
            //                                    20f, //SL No
            //                                    50f, //Invoice No
            //                                    50f, //Invoice Date
            //                                    60f, //Invoice Amount
            //                                    70f, //Payment Amount
            //                                    60f, //Due Amount
            //                                    200f // Blank
            //                                });


            //    #region Blank Rows
            //    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            //    _oPdfPCell.Colspan = 7;
            //    _oPdfPCell.FixedHeight = 40f;
            //    _oPdfPCell.Border = 0;
            //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //    _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            //    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //    oPdfPTable.AddCell(_oPdfPCell);
            //    oPdfPTable.CompleteRow();
            //    #endregion

            //    #region Table Caption
            //    _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            //    _oPdfPCell = new PdfPCell(new Phrase("Previous Due(s)", _oFontStyle));
            //    _oPdfPCell.Colspan = 6;
            //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //    _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            //    _oPdfPCell.FixedHeight = 12;
            //    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //    oPdfPTable.AddCell(_oPdfPCell);

            //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //    _oPdfPCell.Border = 0;
            //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //    _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            //    _oPdfPCell.FixedHeight = 12;
            //    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //    oPdfPTable.AddCell(_oPdfPCell);
            //    oPdfPTable.CompleteRow();
            //    #endregion

            //    #region Table Header
            //    _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            //    _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle));
            //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            //    _oPdfPCell = new PdfPCell(new Phrase("Invoice No", _oFontStyle));
            //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            //    _oPdfPCell = new PdfPCell(new Phrase("Invoice Date", _oFontStyle));
            //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            //    _oPdfPCell = new PdfPCell(new Phrase("Invoice Amount", _oFontStyle));
            //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            //    _oPdfPCell = new PdfPCell(new Phrase("Payment Amount", _oFontStyle));
            //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            //    _oPdfPCell = new PdfPCell(new Phrase("Due Amount", _oFontStyle));
            //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            //    oPdfPTable.CompleteRow();
            //    #endregion

            //    nCount = 1;
            //    _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            //    foreach (TradingSaleInvoice oItem in _oTradingSaleInvoices)
            //    {
            //        _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
            //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //        _oPdfPCell = new PdfPCell(new Phrase(oItem.InvoiceNo, _oFontStyle));
            //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //        _oPdfPCell = new PdfPCell(new Phrase(oItem.InvoiceDateInString, _oFontStyle));
            //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.NetAmount), _oFontStyle));
            //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat((oItem.NetAmount - oItem.DueAmount)), _oFontStyle));
            //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.DueAmount), _oFontStyle));
            //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            //        oPdfPTable.CompleteRow();

            //        nTotalInvocieAmount = nTotalInvocieAmount + oItem.NetAmount;
            //        nTotalPaymentAmount = nTotalPaymentAmount + (oItem.NetAmount - oItem.DueAmount);
            //        nTotalDueAmount = nTotalDueAmount + oItem.DueAmount;
            //    }

            //    #region Total Print
            //    _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            //    _oPdfPCell = new PdfPCell(new Phrase("Grand Total :", _oFontStyle));
            //    _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalInvocieAmount), _oFontStyle));
            //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalPaymentAmount), _oFontStyle));
            //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalDueAmount), _oFontStyle));
            //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            //    oPdfPTable.CompleteRow();
            //    #endregion

            //    _oPdfPCell = new PdfPCell(oPdfPTable);
            //    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //    _oPdfPTable.CompleteRow();
            //}
            #endregion

        }
        #endregion
    }
}
