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
    public class rptKnittingOrder
    {

        #region Declaration

        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyleBold;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        int styleIDCount = 0;
        double nTableHeight = 87;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        Company _oCompany = new Company();
        BusinessUnit _oBusinessUnit = new BusinessUnit();

        List<KnittingOrderDetail> _oKnittingOrderDetails = new List<KnittingOrderDetail>();
        List<KnittingComposition> _oKnittingCompositions = new List<KnittingComposition>();
        List<KnittingOrderTermsAndCondition> _oKnittingOrderTermsAndConditions = new List<KnittingOrderTermsAndCondition>();
        //List<ApprovalHead> _oApprovalHeads = new List<ApprovalHead>();
        //List<ApprovalHistory> _oApprovalHistorys = new List<ApprovalHistory>();
        List<SignatureSetup> _oSignatureSetups = new List<SignatureSetup>();
        KnittingOrder _oKnittingOrder = new KnittingOrder();

        #endregion

        private void PrintHeader()
        {
            #region Company & Report Header
            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 200f, 400f, 200f });

            #region Company Name & Report Header
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            if (_oCompany.CompanyLogo != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(95f, 40f);
                _oPdfPCell = new PdfPCell(_oImag);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            }
            _oPdfPCell.Border = 0; _oPdfPCell.Rowspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 15f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Name, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Company Address & Date Range
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Address, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Company Phone Number
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Phone + ";  " + _oBusinessUnit.Email + ";  " + _oBusinessUnit.WebAddress, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion


            #endregion
        }

        public byte[] PrepareReport(KnittingOrder oKnittingOrder, List<KnittingComposition> oKnittingCompositions, Company oCompany, BusinessUnit oBusinessUnit, List<SignatureSetup> oSignatureSetups)//List<ApprovalHead> oApprovalHeads, List<ApprovalHistory> oApprovalHistorys
        {
            _oCompany = oCompany;
            _oKnittingOrder = oKnittingOrder;
            _oKnittingOrderDetails = oKnittingOrder.KnittingOrderDetails;
            _oKnittingCompositions = oKnittingCompositions;
            _oKnittingOrderTermsAndConditions = oKnittingOrder.KnittingOrderTermsAndConditions;
            _oBusinessUnit = oBusinessUnit;
            //_oApprovalHeads = oApprovalHeads;
            //_oApprovalHistorys = oApprovalHistorys;
            _oSignatureSetups = oSignatureSetups;
            styleIDCount = _oKnittingOrderDetails.Select(x => x.StyleID).Distinct().Count();

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetMargins(30f, 30f, 40f, 30f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            _oFontStyle = FontFactory.GetFont("Tahoma", 15f, 1);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 595 });
            #endregion

            #region 1st
            PrintHeader();
            PrintUnderLine();
            //PrintEmptyRow("", 3, 18, false);
            PrintEmptyRow("KNITTING PROGRAM", 20, 18, false);
            PrintKnittingOrder("Office Copy");
            PrintEmptyRow("", 15, 18, false);
            if (styleIDCount == 1)
            {
                this.PrintKnittingOrderDetailsSameStyle();
            }
            else
            {
                this.PrintKnittingOrderDetailsDifferentStyle();
            }
            PrintEmptyRow("", 15, 18, false);
            PrintKnittingConsumption();
            PrintEmptyRow("", 15, 18, false);
            if (_oKnittingCompositions.Count > 0) PrintComment();
            PrintEmptyRow("", 15, 18, false);
            if (_oKnittingOrderTermsAndConditions.Count > 0) PrintTerms();
            PrintEmptyRow("", 15, 18, false);
            PrintSignature();
            #endregion
            NewPageDeclaration();
            #region 2nd
            PrintHeader();
            PrintUnderLine();
            //PrintEmptyRow("", 3, 18, false);
            PrintEmptyRow("KNITTING PROGRAM", 20, 18, false);
            PrintKnittingOrder("Audit Copy");
            PrintEmptyRow("", 15, 18, false);
            if (styleIDCount == 1)
            {
                this.PrintKnittingOrderDetailsSameStyle();
            }
            else
            {
                this.PrintKnittingOrderDetailsDifferentStyle();
            }
            PrintEmptyRow("", 15, 18, false);
            PrintKnittingConsumption();
            PrintEmptyRow("", 15, 18, false);
            if (_oKnittingCompositions.Count > 0) PrintComment();
            PrintEmptyRow("", 15, 18, false);
            if (_oKnittingOrderTermsAndConditions.Count > 0) PrintTerms();
            PrintEmptyRow("", 15, 18, false);
            PrintSignature();
            #endregion
            NewPageDeclaration();
            #region 3rd
            PrintHeader();
            PrintUnderLine();
            //PrintEmptyRow("", 3, 18, false);
            PrintEmptyRow("KNITTING PROGRAM", 20, 18, false);
            PrintKnittingOrder("Yarn Store Copy");
            PrintEmptyRow("", 15, 18, false);
            if (styleIDCount == 1)
            {
                this.PrintKnittingOrderDetailsSameStyle();
            }
            else
            {
                this.PrintKnittingOrderDetailsDifferentStyle();
            }
            PrintEmptyRow("", 15, 18, false);
            PrintKnittingConsumption();
            PrintEmptyRow("", 15, 18, false);
            if (_oKnittingCompositions.Count > 0) PrintComment();
            PrintEmptyRow("", 15, 18, false);
            if (_oKnittingOrderTermsAndConditions.Count > 0) PrintTerms();
            PrintEmptyRow("", 15, 18, false);
            PrintSignature();
            #endregion
            NewPageDeclaration();
            #region 4th
            PrintHeader();
            PrintUnderLine();
            //PrintEmptyRow("", 3, 18, false);
            PrintEmptyRow("KNITTING PROGRAM", 20, 18, false);
            PrintKnittingOrder("Grey Store Copy");
            PrintEmptyRow("", 15, 18, false);
            if (styleIDCount == 1)
            {
                this.PrintKnittingOrderDetailsSameStyle();
            }
            else
            {
                this.PrintKnittingOrderDetailsDifferentStyle();
            }
            PrintEmptyRow("", 15, 18, false);
            PrintKnittingConsumption();
            PrintEmptyRow("", 15, 18, false);
            if (_oKnittingCompositions.Count > 0) PrintComment();
            PrintEmptyRow("", 15, 18, false);
            if (_oKnittingOrderTermsAndConditions.Count > 0) PrintTerms();
            PrintEmptyRow("", 15, 18, false);
            PrintSignature();
            #endregion
            NewPageDeclaration();
            #region 5th
            PrintHeader();
            PrintUnderLine();
            //PrintEmptyRow("", 3, 18, false);
            PrintEmptyRow("KNITTING PROGRAM", 20, 18, false);
            PrintKnittingOrder("Party Copy");
            PrintEmptyRow("", 15, 18, false);
            if (styleIDCount == 1)
            {
                this.PrintKnittingOrderDetailsSameStyle();
            }
            else
            {
                this.PrintKnittingOrderDetailsDifferentStyle();
            }
            PrintEmptyRow("", 15, 18, false);
            PrintKnittingConsumption();
            PrintEmptyRow("", 15, 18, false);
            if (_oKnittingCompositions.Count > 0) PrintComment();
            PrintEmptyRow("", 15, 18, false);
            if (_oKnittingOrderTermsAndConditions.Count > 0) PrintTerms();
            PrintEmptyRow("", 15, 18, false);
            PrintSignature();
            #endregion

            _oPdfPTable.HeaderRows = 4;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        public void PrintKnittingOrder(string sMsg)
        {
            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            oPdfPTable.SetWidths(new float[] { 60, 4, 36 });
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);

            #region print copy msg
            _oPdfPCell = new PdfPCell(new Phrase(sMsg, _oFontStyleBold)); _oPdfPCell.Colspan = 3;
            _oPdfPCell.Border = iTextSharp.text.Rectangle.NO_BORDER; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold)); _oPdfPCell.Colspan = 3; _oPdfPCell.FixedHeight = 10f;
            _oPdfPCell.Border = iTextSharp.text.Rectangle.NO_BORDER; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            #region Master Table
            PdfPTable oPdfPTableMasterTable = new PdfPTable(2);
            oPdfPTableMasterTable.WidthPercentage = 100;
            oPdfPTableMasterTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTableMasterTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            oPdfPTableMasterTable.SetWidths(new float[] { 30f, 70f });


            _oPdfPCell = new PdfPCell(new Phrase("ORDER NO :", _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableMasterTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" " + _oKnittingOrder.OrderNo, _oFontStyle));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableMasterTable.AddCell(_oPdfPCell);

            oPdfPTableMasterTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("ORDER DATE :", _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableMasterTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" " + _oKnittingOrder.OrderDateInString, _oFontStyle));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableMasterTable.AddCell(_oPdfPCell);

            oPdfPTableMasterTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase("FACTORY NAME :", _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableMasterTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" " + _oKnittingOrder.FactoryName, FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD)));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableMasterTable.AddCell(_oPdfPCell);

            oPdfPTableMasterTable.CompleteRow();


            if (styleIDCount == 1)
            {
                _oPdfPCell = new PdfPCell(new Phrase("BUYER NAME :", _oFontStyleBold));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableMasterTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" " + _oKnittingOrderDetails[0].BuyerName, _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableMasterTable.AddCell(_oPdfPCell);
                oPdfPTableMasterTable.CompleteRow();


                _oPdfPCell = new PdfPCell(new Phrase("SYLE NO :", _oFontStyleBold));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableMasterTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" " + _oKnittingOrderDetails[0].StyleNo, _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableMasterTable.AddCell(_oPdfPCell);
                oPdfPTableMasterTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase("PAM :", _oFontStyleBold));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableMasterTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" " + _oKnittingOrderDetails[0].PAM.ToString("##"), _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableMasterTable.AddCell(_oPdfPCell);
                oPdfPTableMasterTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase("QTY PCS :", _oFontStyleBold));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableMasterTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" " + _oKnittingOrderDetails[0].StylePcsQty.ToString("#,###;(#,###)"), _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableMasterTable.AddCell(_oPdfPCell);
                oPdfPTableMasterTable.CompleteRow();
            }

            _oPdfPCell = new PdfPCell(new Phrase("REMARKS :", _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableMasterTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" " + _oKnittingOrder.Remarks, _oFontStyle));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableMasterTable.AddCell(_oPdfPCell);

            oPdfPTableMasterTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTableMasterTable);
            _oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 1;
            _oPdfPCell.ExtraParagraphSpace = 3f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            #endregion

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));//_oPdfPCell.Rowspan = _oKnittingYarns.Count - 1;
            _oPdfPCell.Border = iTextSharp.text.Rectangle.NO_BORDER; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            #region yarn and date table
            PdfPTable oPdfPTableRequiredYarn = new PdfPTable(3);
            oPdfPTableRequiredYarn.WidthPercentage = 100;
            oPdfPTableRequiredYarn.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTableRequiredYarn.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            oPdfPTableRequiredYarn.SetWidths(new float[] { 8f, 60f, 32f });


            _oPdfPCell = new PdfPCell(new Phrase("KNITTING START DATE :", _oFontStyleBold)); _oPdfPCell.Colspan = 2;
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableRequiredYarn.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" " + _oKnittingOrder.StartDateInString, _oFontStyle));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableRequiredYarn.AddCell(_oPdfPCell);

            oPdfPTableRequiredYarn.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("KNITTING COMPLETE DATE :", _oFontStyleBold)); _oPdfPCell.Colspan = 2;
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableRequiredYarn.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" " + _oKnittingOrder.ApproxCompleteDateInString, _oFontStyle));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableRequiredYarn.AddCell(_oPdfPCell);

            oPdfPTableRequiredYarn.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold)); _oPdfPCell.Colspan = 2;
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableRequiredYarn.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableRequiredYarn.AddCell(_oPdfPCell);

            oPdfPTableRequiredYarn.CompleteRow();


            _oPdfPCell = new PdfPCell(oPdfPTableRequiredYarn);
            _oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 1;
            _oPdfPCell.ExtraParagraphSpace = 3f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            #endregion

            oPdfPTable.CompleteRow();
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 1;
            _oPdfPCell.ExtraParagraphSpace = 5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        public void PrintKnittingOrderDetailsSameStyle()
        {
            PdfPTable oPdfPTable = new PdfPTable(11);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            oPdfPTable.SetWidths(new float[] { 30, 120, 45, 55, 45, 45, 60, 40, 70, 60, 80 });
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);

            _oPdfPCell = new PdfPCell(new Phrase("FABRICS CONSTRUCTION", _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.Colspan = 11; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            #region header
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Fabric/ Color", _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("GSM", _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("MIC Dia", _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("F/N Dia", _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("S/L", _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Brand", _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("M.Unit", _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Qty", _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Price(" + _oKnittingOrder.CurrencyName + ")", _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Amount(" + _oKnittingOrder.CurrencyName + ")", _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region knitting order details data
            int nCount = 0;
            double totalQty = 0;
            double totalAmt = 0;

            foreach (KnittingOrderDetail oItem in _oKnittingOrderDetails)
            {
                nCount++;
                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.FabricName + Environment.NewLine + oItem.ColorName, _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.GSM.ToString(), _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.MICDia.ToString(), _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.FinishDia.ToString(), _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.StratchLength.ToString(), _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.BrandName.ToString(), _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.MUSymbol.ToString(), _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Qty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.UnitPrice.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Amount.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();
                totalQty = totalQty + oItem.Qty;
                totalAmt = totalAmt + oItem.Amount;
            }
            #endregion

            _oPdfPCell = new PdfPCell(new Phrase("Grand Total:", _oFontStyleBold)); _oPdfPCell.Colspan = 8;
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(totalQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(totalAmt.ToString("#,##0.00;(#,##0.00)"), _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 1;
            _oPdfPCell.ExtraParagraphSpace = 5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        public void PrintKnittingOrderDetailsDifferentStyle()
        {
            PdfPTable oPdfPTable = new PdfPTable(13);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            oPdfPTable.SetWidths(new float[] { 30, 130, 35, 90, 45, 55, 60, 45, 45, 50, 60, 50, 80 });
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);

            _oPdfPCell = new PdfPCell(new Phrase("FABRICS CONSTRUCTION", _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.Colspan = 13; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            #region header
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Buyer/Style/Piece Qty", _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("PAM", _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Fabric/ Color", _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("GSM", _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("MIC Dia", _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("F/N Dia", _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("S/L", _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Brand", _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("M.Unit", _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Qty", _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Price(" + _oKnittingOrder.CurrencyName + ")", _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Amount(" + _oKnittingOrder.CurrencyName + ")", _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region knitting order details data
            int nCount = 0;
            double totalQty = 0;
            double totalAmt = 0;

            foreach (KnittingOrderDetail oItem in _oKnittingOrderDetails)
            {
                nCount++;
                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.BuyerName + Environment.NewLine + oItem.StyleNo + Environment.NewLine + oItem.StylePcsQty.ToString("#,###;(#,###)") + " " + oItem.OrderUnitName, _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.PAM.ToString(), _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.FabricName + Environment.NewLine + oItem.ColorName, _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.GSM.ToString(), _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.MICDia.ToString(), _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.FinishDia.ToString(), _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.StratchLength.ToString(), _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.BrandName.ToString(), _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.MUSymbol.ToString(), _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Qty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.UnitPrice.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Amount.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();
                totalQty = totalQty + oItem.Qty;
                totalAmt = totalAmt + oItem.Amount;
            }
            #endregion

            _oPdfPCell = new PdfPCell(new Phrase("Grand Total:", _oFontStyleBold)); _oPdfPCell.Colspan = 10;
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(totalQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(totalAmt.ToString("#,##0.00;(#,##0.00)"), _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 1;
            _oPdfPCell.ExtraParagraphSpace = 5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        public void PrintKnittingConsumption()
        {
            PdfPTable oPdfPTable = new PdfPTable(10);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            oPdfPTable.SetWidths(new float[] { 20, 130, 50, 150, 60, 60, 60, 45, 50, 35 });
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);

            _oPdfPCell = new PdfPCell(new Phrase("Knitting Order Composition", _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.Colspan = 10; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            #region header
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Style/ Fabric", _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Yarn Code", _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Yarn Name", _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Lot No", _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Color", _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Brand", _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Ratio", _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Qty", _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("M. Unit", _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            #region Knitting Consumption
            int nCount = 0;
            int nRowSpan = 0;
            double totalQty = 0;

            List<KnittingComposition> oTempKnittingCompositions = new List<KnittingComposition>();
            foreach (KnittingOrderDetail oItem in _oKnittingOrderDetails)
            {
                nCount++;
                oTempKnittingCompositions = _oKnittingCompositions.Where(x => x.KnittingOrderDetailID == oItem.KnittingOrderDetailID).ToList();
                nRowSpan = oTempKnittingCompositions.Count();
                if (nRowSpan == 0) nRowSpan = 1;
                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle)); _oPdfPCell.Rowspan = nRowSpan;
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.StyleNo + Environment.NewLine + oItem.FabricName, _oFontStyle)); _oPdfPCell.Rowspan = nRowSpan;
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                foreach (KnittingComposition oTemp in oTempKnittingCompositions)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(oTemp.YarnCode, _oFontStyle));
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oTemp.YarnName, _oFontStyle));
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oTemp.LotNo, _oFontStyle));
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oTemp.ColorName, _oFontStyle));
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oTemp.BrandName, _oFontStyle));
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oTemp.RatioInPercent.ToString("#,##0.00;(#,##0.00)") + " %", _oFontStyle));
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oTemp.Qty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.MUSymbol, _oFontStyle));
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                }
                if (oTempKnittingCompositions.Count == 0)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                }
                oPdfPTable.CompleteRow();
                totalQty = totalQty + oItem.Qty;

                _oPdfPCell = new PdfPCell(new Phrase("Sub Total:", _oFontStyleBold));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.Colspan = 8; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oKnittingCompositions.Where(x => x.KnittingOrderDetailID == oItem.KnittingOrderDetailID).Sum(x => x.Qty).ToString("#,##0.00;(#,##0.00)"), _oFontStyleBold));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            }
            oPdfPTable.CompleteRow();
            _oPdfPCell = new PdfPCell(new Phrase("Grand Total:", _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.Colspan = 8; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oKnittingCompositions.Sum(x => x.Qty).ToString("#,##0.00;(#,##0.00)"), _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0;
            _oPdfPCell.ExtraParagraphSpace = 5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

        }
        public void PrintSignature()
        {
            #region print Signature Captions
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.FixedHeight = 20f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(ESimSolSignature.GetSignature(525f, (object)_oKnittingOrder, _oSignatureSetups, 15f));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        public void NewPageDeclaration()
        {
            #region New Page Declare
            _oDocument.Add(_oPdfPTable);
            _oDocument.NewPage();
            _oPdfPTable.DeleteBodyRows();
            #endregion
        }
        public void PrintTerms()
        {
            PdfPTable oPdfPTable = new PdfPTable(1);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            oPdfPTable.SetWidths(new float[] { 100 });
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);

            _oPdfPCell = new PdfPCell(new Phrase("TERMS & CONDITIONS", _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.NO_BORDER; _oPdfPCell.Colspan = 1; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            #region terms & conditions
            int nCount = 0;

            foreach (KnittingOrderTermsAndCondition oItem in _oKnittingOrderTermsAndConditions)
            {
                nCount++;

                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString("00") + ". " + oItem.TermsAndCondition.ToString(), _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.NO_BORDER; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();
            }

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 1;
            _oPdfPCell.ExtraParagraphSpace = 5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion
        }
        public void PrintComment()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD);
            Phrase oPhrase = new Phrase();
            Chunk oChunk1 = new Chunk("Comments: ", _oFontStyle);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            Chunk oChunk2 = new Chunk(_oKnittingOrder.KnittingInstruction, _oFontStyle);
            oPhrase.Add(oChunk1);
            oPhrase.Add(oChunk2);
            _oPdfPCell = new PdfPCell(oPhrase);
            //_oPdfPCell = new PdfPCell(new Phrase("Comments: " + _oKnittingOrder.KnittingInstruction, _oFontStyle));
            _oPdfPCell.Border = 15;
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.MinimumHeight = 35; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        public void PrintKnitOrder()
        {
            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            oPdfPTable.SetWidths(new float[] { 182, 3, 90 });
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

            if (styleIDCount > 1)
            {
                #region Master Table
                PdfPTable oPdfPTableMasterTable = new PdfPTable(6);
                oPdfPTableMasterTable.WidthPercentage = 100;
                oPdfPTableMasterTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTableMasterTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                oPdfPTableMasterTable.SetWidths(new float[] { 53, 30, 33, 60, 38, 35 });

                _oPdfPCell = new PdfPCell(new Phrase("   ", _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.NO_BORDER; _oPdfPCell.Colspan = 6; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableMasterTable.AddCell(_oPdfPCell);
                oPdfPTableMasterTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase("Knitting Order No :", _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableMasterTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oKnittingOrder.OrderNo, _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableMasterTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Order Date:", _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableMasterTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oKnittingOrder.OrderDateInString, _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableMasterTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Knitting Start:", _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableMasterTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oKnittingOrder.StartDateInString, _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableMasterTable.AddCell(_oPdfPCell);
                oPdfPTableMasterTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase("Factory Name :", _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableMasterTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oKnittingOrder.FactoryName, _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableMasterTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Knitting End:", _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableMasterTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oKnittingOrder.ActualCompleteDateInString, _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableMasterTable.AddCell(_oPdfPCell);
                oPdfPTableMasterTable.CompleteRow();

                int nRowspan = 1;

                _oPdfPCell = new PdfPCell(new Phrase("Remarks:", _oFontStyle)); _oPdfPCell.MinimumHeight = 10 * nRowspan;
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableMasterTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oKnittingOrder.Remarks, _oFontStyle)); _oPdfPCell.MinimumHeight = 10 * nRowspan;
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.Colspan = 5; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableMasterTable.AddCell(_oPdfPCell);
                oPdfPTableMasterTable.CompleteRow();


                //_oPdfPCell = new PdfPCell(new Phrase("Knitting Instruction:", _oFontStyle));//_oPdfPCell.Rowspan = nRowspan;
                //_oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.MinimumHeight = 10 * nRowspan; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableMasterTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase(_oKnittingOrder.KnittingInstruction, _oFontStyle));
                //_oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.MinimumHeight = 10 * nRowspan; _oPdfPCell.Colspan = 5; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableMasterTable.AddCell(_oPdfPCell);
                //oPdfPTableMasterTable.CompleteRow();

                _oPdfPCell = new PdfPCell(oPdfPTableMasterTable);
                _oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = 1;
                _oPdfPCell.ExtraParagraphSpace = 3f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                #endregion
            }
            else
            {
                #region Master Table
                PdfPTable oPdfPTableMasterTable = new PdfPTable(6);
                oPdfPTableMasterTable.WidthPercentage = 100;
                oPdfPTableMasterTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTableMasterTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                oPdfPTableMasterTable.SetWidths(new float[] { 53, 30, 33, 60, 38, 35 });

                _oPdfPCell = new PdfPCell(new Phrase("   ", _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.NO_BORDER; _oPdfPCell.Colspan = 6; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableMasterTable.AddCell(_oPdfPCell);
                oPdfPTableMasterTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase("Knitting Order No :", _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableMasterTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oKnittingOrder.OrderNo, _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableMasterTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Order Date:", _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableMasterTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oKnittingOrder.OrderDateInString, _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableMasterTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Knitting Start:", _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableMasterTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oKnittingOrder.StartDateInString, _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableMasterTable.AddCell(_oPdfPCell);
                oPdfPTableMasterTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase("Factory Name :", _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableMasterTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oKnittingOrder.FactoryName, _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableMasterTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Knitting End:", _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableMasterTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oKnittingOrder.ApproxCompleteDateInString, _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableMasterTable.AddCell(_oPdfPCell);
                oPdfPTableMasterTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase("Buyer Name :", _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableMasterTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oKnittingOrderDetails[0].BuyerName, _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableMasterTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Style No :", _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableMasterTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oKnittingOrderDetails[0].StyleNo, _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableMasterTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Order Qty :", _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableMasterTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oKnittingOrderDetails[0].StylePcsQty.ToString("#,###;(#,###)"), _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableMasterTable.AddCell(_oPdfPCell);
                oPdfPTableMasterTable.CompleteRow();

                int nRowspan = 1;

                _oPdfPCell = new PdfPCell(new Phrase("Remarks:", _oFontStyle)); _oPdfPCell.MinimumHeight = 10 * nRowspan;
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableMasterTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oKnittingOrder.Remarks, _oFontStyle)); _oPdfPCell.MinimumHeight = 10 * nRowspan;
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.Colspan = 5; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableMasterTable.AddCell(_oPdfPCell);
                oPdfPTableMasterTable.CompleteRow();


                //_oPdfPCell = new PdfPCell(new Phrase("Knitting Instruction:", _oFontStyle)); _oPdfPCell.MinimumHeight = 10 * nRowspan;
                //_oPdfPCell.Border = iTextSharp.text.Rectangle.BOX;  _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableMasterTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase(_oKnittingOrder.KnittingInstruction, _oFontStyle)); _oPdfPCell.MinimumHeight = 10 * nRowspan;
                //_oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.Colspan = 5; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableMasterTable.AddCell(_oPdfPCell);
                //oPdfPTableMasterTable.CompleteRow();





                _oPdfPCell = new PdfPCell(oPdfPTableMasterTable);
                _oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = 1;
                _oPdfPCell.ExtraParagraphSpace = 3f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                #endregion
            }

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.NO_BORDER; _oPdfPCell.Rowspan = 1 - 1; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);



            oPdfPTable.CompleteRow();
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 1;
            _oPdfPCell.ExtraParagraphSpace = 5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        public void PrintEmptyRow(string sText, int nHeight, int nFontSize, bool IsBorder)
        {
            PdfPTable oPdfPTable = new PdfPTable(1);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            oPdfPTable.SetWidths(new float[] { 842 });

            #region Row
            _oPdfPCell = new PdfPCell(new Phrase(sText, FontFactory.GetFont("Tahoma", nFontSize, iTextSharp.text.Font.BOLD))); _oPdfPCell.FixedHeight = nHeight;
            if (IsBorder)
            {
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX;
            }
            else
            {
                _oPdfPCell.Border = 0;
            }
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0;
            _oPdfPCell.ExtraParagraphSpace = 30f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        public void PrintUnderLine()
        {
            PdfPTable oPdfPTable = new PdfPTable(1);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            oPdfPTable.SetWidths(new float[] { 842 });

            #region Row
            _oPdfPCell = new PdfPCell(new Phrase("", FontFactory.GetFont("Tahoma", 18, iTextSharp.text.Font.BOLD))); _oPdfPCell.FixedHeight = 5;
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            _oPdfPCell.ExtraParagraphSpace = 30f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }




    }
}
