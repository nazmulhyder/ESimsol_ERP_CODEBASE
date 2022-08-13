using System;
using System.Data;
using ESimSol.BusinessObjects;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using iTextSharp.text;
using ICS.Core.Utility;
using iTextSharp.text.pdf;

namespace ESimSol.Reports
{

    public class rptMoneyReceipt
    {

        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPTable oPdfPTable = new PdfPTable(4);

        PdfPCell _oPdfPCell;
        MemoryStream _oMemoryStream = new MemoryStream();
        TradingPayment _oTradingPayment = new TradingPayment();

        Company _oCompany = new Company();
        Contractor _oContractor = new Contractor();

        string _sTradingPaymentOf = "N/A";
        string _sTradingPaymentBy = "";
        string _sReceivedFrom = "";
        string _sReceivedBy = "";
        string _sReceivedByCaption = "";



        #endregion

        public byte[] PrepareReport(TradingPayment oTradingPayment, Company oCompany)
        {
            _oTradingPayment = oTradingPayment;
            _oCompany = oCompany;
            //_oContractor = oTradingPayment.Contractor;

            if (_oTradingPayment.PaymentMethod == EnumPaymentMethod.Cash)
            {
                _sTradingPaymentBy = "Cash: " + Global.TakaFormat(_oTradingPayment.Amount);
            }
            else
            {
                _sTradingPaymentBy = "Cheque No: " + _oTradingPayment.ChequeNo + ";  Bank Name:-" + _oTradingPayment.BankName;
            }

            if (_oTradingPayment.ReferenceType == EnumPaymentRefType.ReceivedPayment)
            {
                _sReceivedFrom = "Received From";
                _sReceivedBy = "Received By";
                _sReceivedByCaption = "Received By";
            }
            else
            {
                _sReceivedFrom = "Paid From";
                _sReceivedBy = "Paid By";
                _sReceivedByCaption = "Paid By";
            }



            if (_oTradingPayment.Note != "")
            {
                _sTradingPaymentOf = _oTradingPayment.Note;
            }

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            _oDocument.SetMargins(40f, 40f, 40f, 30f);
            _oPdfPTable.WidthPercentage = 100;
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();

            _oPdfPTable.SetWidths(new float[] { 515f });
            #endregion

            this.PrintBody("Client Copy");

            #region Blank Space
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 100f;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            this.PrintBody("Office Copy");
            _oPdfPTable.HeaderRows = 0;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }



        #region Report Body
        private void PrintBody(string sCopymessage)
        {
            #region Blank Space
            oPdfPTable = new PdfPTable(4);
            oPdfPTable.SetWidths(new float[] { 132f, 200f, 177f, 10f });
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Colspan = 4;
            _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.FixedHeight = 5f;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            _oFontStyle = FontFactory.GetFont("Tahoma", 14f, iTextSharp.text.Font.BOLD);

            #region TradingPayment
            #region 1st Row

            _oPdfPCell = new PdfPCell(new Phrase(_oTradingPayment.BUName, _oFontStyle));
            _oPdfPCell.ExtraParagraphSpace = 10f; _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(sCopymessage, _oFontStyle));
            _oPdfPCell.ExtraParagraphSpace = 10f; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.ExtraParagraphSpace = 10f; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();


            #endregion

            #region 2nd  Row
            _oFontStyle = FontFactory.GetFont("Tahoma", 14f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("Money Receipt", _oFontStyle));
            _oPdfPCell.Colspan = 4; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();


            #endregion

            #region 3rd  Row
            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(" M.R.NO: " + _oTradingPayment.RefNo, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.ExtraParagraphSpace = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Date :" + _oTradingPayment.PaymentDateSt, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.ExtraParagraphSpace = 10f; _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.ExtraParagraphSpace = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();


            #endregion

            #region 4th   Row
            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(" " + _sReceivedFrom, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.MinimumHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oTradingPayment.ContractorName, _oFontStyle));
            _oPdfPCell.Colspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.MinimumHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.MinimumHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            #region Small Blank Space

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 4;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.FixedHeight = 4f;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion


            #region 5th   Row

            _oPdfPCell = new PdfPCell(new Phrase(" For Payment Of", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.MinimumHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_sTradingPaymentOf, _oFontStyle));
            _oPdfPCell.Colspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.MinimumHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.MinimumHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion
            #region Small Blank Space

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 4;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.FixedHeight = 4f;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region 6th   Row

            _oPdfPCell = new PdfPCell(new Phrase(" " + _sReceivedBy, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.MinimumHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_sTradingPaymentBy, _oFontStyle));
            _oPdfPCell.Colspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.MinimumHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.MinimumHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();

            #endregion

            #region Small Blank Space

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 4;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.FixedHeight = 4f;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region 7th   Row

            _oPdfPCell = new PdfPCell(new Phrase(" Amount In Word", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.MinimumHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.TakaWords(_oTradingPayment.Amount), _oFontStyle));
            _oPdfPCell.Colspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.MinimumHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.MinimumHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion
            #region Small Blank Space

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 4;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.FixedHeight = 4f;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region 8th   Row

            _oPdfPCell = new PdfPCell(new Phrase(" Amount", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.MinimumHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.TakaFormat(_oTradingPayment.Amount), _oFontStyle));
            _oPdfPCell.Colspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.MinimumHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.MinimumHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion


            #region Blank Space

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Colspan = 4;
            _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.FixedHeight = 35f;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region 9th   Row
            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(" " + _sReceivedByCaption, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.ExtraParagraphSpace = 3f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLDITALIC);
            _oPdfPCell = new PdfPCell(new Phrase("Thanks", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.ExtraParagraphSpace = 3f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("-------------------------------------", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.ExtraParagraphSpace = 3f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.ExtraParagraphSpace = 3f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();


            #endregion

            #region 10th   Row
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(" " + _oTradingPayment.ApprovedByName, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.ExtraParagraphSpace = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.ExtraParagraphSpace = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("Authorized Signature", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.ExtraParagraphSpace = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.ExtraParagraphSpace = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();


            #endregion



            #endregion


            #region Footer  Detail
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(_oTradingPayment.BUID, 0);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(oBusinessUnit.Address + oBusinessUnit.Phone + ";  " + oBusinessUnit.Email + ";  " + oBusinessUnit.WebAddress, _oFontStyle));
            _oPdfPCell.Colspan = 4;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.FixedHeight = 10;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.ExtraParagraphSpace = 7f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

        }
        #endregion
    }


}
