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
    public class rptPrintPOS
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        int _nColumn = 6;
        PdfPTable _oPdfPTable = new PdfPTable(6);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        TradingSaleInvoice _oTradingSaleInvoice = new TradingSaleInvoice();
        List<TradingSaleInvoiceDetail> _oTradingSaleInvoiceDetails = new List<TradingSaleInvoiceDetail>();

        Company _oCompany = new Company();

        #endregion

        public byte[] PrepareReport(TradingSaleInvoice oTradingSaleInvoice, Company oCompany)
        {
            _oCompany = oCompany;
            _oTradingSaleInvoice = oTradingSaleInvoice;
            _oTradingSaleInvoiceDetails = oTradingSaleInvoice.TradingSaleInvoiceDetails;

            #region Page Setup
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPTable.SetWidths(new float[] { 7f, 18f, 10f, 10f, 12f, 15f });
            this.PrintHeader();
            this.PrintBody();

            float nWidth = 210f;
            float nHeight = 30f;
            if (_oTradingSaleInvoiceDetails.Count > 0)
            {
                nHeight = nHeight + CalculatePdfPTableHeight(_oPdfPTable, (nWidth - 10));
            }
            _oDocument = new Document(new iTextSharp.text.Rectangle(nWidth, nHeight));  //A7 = 210 x 298
            _oDocument.SetMargins(5f, 5f, 5f, 5f);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            #endregion

            _oPdfPTable.HeaderRows = 0;
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
                _oPdfPCell.Colspan = _nColumn;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 35;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            else
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 1);
                _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));
                _oPdfPCell.Colspan = _nColumn;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.ExtraParagraphSpace = 0;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Address + "\n" + _oCompany.Phone + ";  " + _oCompany.Email + ";  " + _oCompany.WebAddress, _oFontStyle));
            _oPdfPCell.Colspan = _nColumn;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 2f;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        #endregion

        #region Report Body


        private void ColumnHead()
        {
            #region Grid Column
            _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Items", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Qty", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Unit", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Price", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Amount", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();
            #endregion
        }

        private void DiscountColumnHead()
        {
            #region Grid Column
            _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Items", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Amount", _oFontStyle));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Dis(%)", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Discount-Amount", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();
            #endregion
        }
        private void PrintBody()
        {

            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 1);

            #region Invoice Headder
            _oPdfPCell = new PdfPCell(new Phrase("Invoice No: " + _oTradingSaleInvoice.InvoiceNo, _oFontStyle)); // show from start date to end date
            _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Inovice Date: " + _oTradingSaleInvoice.InvoiceDateInString, _oFontStyle)); // show current date time
            _oPdfPCell.Colspan = _nColumn - 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Customer : " + _oTradingSaleInvoice.BuyerName, _oFontStyle));
            _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Contact No: " + _oTradingSaleInvoice.BuyerPhone, _oFontStyle));
            _oPdfPCell.Colspan = _nColumn - 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            if (_oTradingSaleInvoice.BuyerAddress != "")
            {
                _oPdfPCell = new PdfPCell(new Phrase("Address : " + _oTradingSaleInvoice.BuyerAddress, _oFontStyle));
                _oPdfPCell.Colspan = _nColumn; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion

            this.ColumnHead();

            if (_oTradingSaleInvoiceDetails.Count > 0)
            {
                int nCount = 0;
                double nGrandTotalPriceInString = 0;
                double nTotalDiscount = 0;
                _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 1);

                var oDiscountItems = _oTradingSaleInvoiceDetails.Where(x => x.Discount > 0).ToList();

                #region Sold Items
                foreach (TradingSaleInvoiceDetail oItem in _oTradingSaleInvoiceDetails)
                {
                    nCount++;
                    _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Convert.ToString(oItem.ProductName), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(oItem.InvoiceQty), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.Symbol, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.UnitPrice).ToString(), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Convert.ToString(Global.MillionFormat(oItem.Amount)), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    nGrandTotalPriceInString += oItem.Amount;
                    _oPdfPTable.CompleteRow();


                }

                _oPdfPCell = new PdfPCell(new Phrase("Total Amount", _oFontStyle));
                _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nGrandTotalPriceInString), _oFontStyle));
                _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                #region DIscount Item
                if (oDiscountItems.Count() > 0)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("----------------- Discount Items -----------------", _oFontStyle));
                    _oPdfPCell.Colspan = _nColumn; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 16f; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();

                    this.DiscountColumnHead();

                    nCount = 0;
                    foreach (TradingSaleInvoiceDetail oItem in oDiscountItems)
                    {
                        nCount++;
                        _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Convert.ToString(oItem.ProductName), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(oItem.Amount), _oFontStyle));
                        _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.DiscountInPercentage).ToString(), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Convert.ToString(Global.MillionFormat(oItem.Discount)), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        nTotalDiscount += oItem.Discount;
                        _oPdfPTable.CompleteRow();
                    }
                    _oPdfPCell = new PdfPCell(new Phrase("Total Discount", _oFontStyle));
                    _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalDiscount), _oFontStyle));
                    _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.Colspan = _nColumn; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                }
                #endregion


                _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 1);
                #region VAT
                _oPdfPCell = new PdfPCell(new Phrase("Vat", _oFontStyle));
                _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oTradingSaleInvoice.VatAmount), _oFontStyle));
                _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPTable.CompleteRow();
                #endregion

                #region Service Charge

                _oPdfPCell = new PdfPCell(new Phrase("Service Charge", _oFontStyle));
                _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oTradingSaleInvoice.ServiceCharge), _oFontStyle));
                _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPTable.CompleteRow();
                #endregion

                #region Less/ Adjustment

                _oPdfPCell = new PdfPCell(new Phrase("Less/ Adjustment", _oFontStyle));
                _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oTradingSaleInvoice.DiscountAmount), _oFontStyle));
                _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPTable.CompleteRow();
                #endregion

                #region NetAmount

                _oPdfPCell = new PdfPCell(new Phrase("Net Amount", _oFontStyle));
                _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oTradingSaleInvoice.NetAmount), _oFontStyle));
                _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPTable.CompleteRow();
                #endregion


                #region In Words

                _oPdfPCell = new PdfPCell(new Phrase("In words: " + Global.TakaWords(_oTradingSaleInvoice.NetAmount), _oFontStyle));
                _oPdfPCell.Colspan = _nColumn; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPCell.ExtraParagraphSpace = 15f; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                #endregion

                #region Served By

                _oPdfPCell = new PdfPCell(new Phrase("Served By : " + _oTradingSaleInvoice.ApprovedByName + " @ " + DateTime.Now.ToString("dd MMM yyyy hh:mm tt"), _oFontStyle));
                _oPdfPCell.Colspan = _nColumn; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                #endregion

                #region Greetings

                _oPdfPCell = new PdfPCell(new Phrase("Thank You For Shopping At " + _oCompany.Name, _oFontStyle));
                _oPdfPCell.Colspan = _nColumn; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                #endregion

                #region Vendor

                _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 0);
                _oPdfPCell = new PdfPCell(new Phrase("Developed By www.infocratsolutions.com", _oFontStyle));
                _oPdfPCell.Colspan = _nColumn; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                #endregion

            }
        }

        public static float CalculatePdfPTableHeight(PdfPTable table, float nWidth)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (Document doc = new Document(PageSize.TABLOID))
                {
                    using (PdfWriter w = PdfWriter.GetInstance(doc, ms))
                    {
                        doc.Open();
                        table.TotalWidth = nWidth;
                        table.WriteSelectedRows(0, table.Rows.Count, 0, 0, w.DirectContent);

                        doc.Close();
                        return table.TotalHeight;
                    }
                }
            }
        }
        #endregion
    }
}
