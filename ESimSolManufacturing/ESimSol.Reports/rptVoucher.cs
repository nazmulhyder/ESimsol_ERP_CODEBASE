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
    public class rptVoucher
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyleTwo;
        iTextSharp.text.Font _oFontStyleForCC;
        iTextSharp.text.Font _oFontStyleForCCCategory;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPTable _oPdfPTableTemp = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        PdfPCell _oPdfPCellTwo;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        Voucher _oVoucher = new Voucher();
        List<VDObj> _oVDObjs = new List<VDObj>();

        Company _oCompany = new Company();
        Contractor _oContractor = new Contractor();
        ContactPersonnel _oContactPersonnel = new ContactPersonnel();

        double _nSumCreditAmount = 0;
        double _nSumDebitAmount = 0;
        double _nCount = 0;
        PdfWriter _oPdfWriter = null;
        #endregion

        public byte[] PrepareReport(Voucher oVoucher, int nBUID)
        {
            _oCompany = oVoucher.Company;
            _oVoucher = oVoucher;
            _oVDObjs = oVoucher.VDObjs;
            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
            _oDocument.SetMargins(35f, 35f, 20f, 20f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _oPdfWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 525f });
            #endregion

            this.PrintHeader();
            this.PrintBody();
            _oPdfPTable.HeaderRows = 1;
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
            oPdfPTable.SetWidths(new float[] { 100f, 325f, 100f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 11f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 0);
            if (_oVoucher.PrintCount < 1)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Print Date:" + DateTime.Now.ToString("dd MMM yy :HH:mm") + "\n Orginal", _oFontStyle));
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("Print Date:" + DateTime.Now.ToString("dd MMM yy :HH:mm") + "\n Duplicate Copy:" + _oVoucher.PrintCount.ToString(), _oFontStyle));
            }
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Logo & Address
            #region Logo
            if (_oCompany.CompanyLogo != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(60f, 35f);
                _oPdfPCell = new PdfPCell(_oImag);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 35;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Rowspan = 3; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 35;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Rowspan = 3; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            }
            #endregion

            #region address
            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Address, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTableTemp.AddCell(_oPdfPCell);
            _oPdfPTableTemp.CompleteRow();
            #endregion
            #endregion

            #region Blank Space
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Colspan = 3; _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 15f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTableTemp.AddCell(_oPdfPCell);
            _oPdfPTableTemp.CompleteRow();
            #endregion

            #region VoucherName
            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.UNDERLINE);
            _oPdfPCell = new PdfPCell(new Phrase(_oVoucher.VoucherName, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Colspan = 3;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTableTemp.AddCell(_oPdfPCell);
            _oPdfPTableTemp.CompleteRow();
            #endregion

            #region AuthorizedBy
            if (_oVoucher.AuthorizedBy == 0)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.ITALIC);
                _oPdfPCell = new PdfPCell(new Phrase("Initial Voucher", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Colspan = 3;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTableTemp.AddCell(_oPdfPCell);
                _oPdfPTableTemp.CompleteRow();
            }
            else
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.ITALIC);
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Colspan = 3;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTableTemp.AddCell(_oPdfPCell);
                _oPdfPTableTemp.CompleteRow();
            }
            #endregion

            #region Blank Space
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Colspan = 3; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTableTemp.AddCell(_oPdfPCell);
            _oPdfPTableTemp.CompleteRow();
            #endregion
        }
        #endregion

        #region Report Body
        private void PrintBody()
        {
            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 175f, 175f, 175f });

            #region Report Vocher No & Voucher Date
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Voucher No : " + _oVoucher.VoucherNo, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" Dated :" + _oVoucher.VoucherDateInString, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTableTemp.AddCell(_oPdfPCell);
            _oPdfPTableTemp.CompleteRow();
            #endregion

            #region Column Header
            oPdfPTable = new PdfPTable(3);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            oPdfPTable.SetWidths(new float[] { 371.5f, 77.5f, 77.5f });
            _oPdfPCell = new PdfPCell(new Phrase("Ledger Description", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Debit (" + _oVoucher.BaseCUSymbol + ")", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" Credit (" + _oVoucher.BaseCUSymbol + ")", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTableTemp.AddCell(_oPdfPCell);
            _oPdfPTableTemp.CompleteRow();
            #endregion

            #region Add Header Table
            _oPdfPCell = new PdfPCell(_oPdfPTableTemp);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            int nCount = 0; int nTotalRowCount = _oVDObjs.Count; int nRemainningCount = 0;
            nRemainningCount = nTotalRowCount;
            foreach (VDObj oItem in _oVDObjs)
            {
                nCount++;
                if (oItem.ObjType == EnumBreakdownType.VoucherDetail)
                {
                    if (oItem.VDObjID > 0)
                    {
                        #region Voucher Detail
                        oPdfPTable = new PdfPTable(3);
                        oPdfPTable.WidthPercentage = 100;
                        oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPTable.SetWidths(new float[] { 370f, 77.5f, 77.5f });

                        _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
                        _oPdfPCell = new PdfPCell(new Phrase(oItem.AHName, _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        if (oItem.DrAmount == 0)
                        {
                            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        }
                        else
                        {
                            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.DrAmount), _oFontStyle));
                        }
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        if (oItem.CrAmount == 0)
                        {
                            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        }
                        else
                        {
                            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.CrAmount), _oFontStyle));
                        }
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 1; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        oPdfPTable.CompleteRow();

                        _oPdfPCell = new PdfPCell(oPdfPTable);
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();

                        _nSumDebitAmount = _nSumDebitAmount + oItem.DrAmount;
                        _nSumCreditAmount = _nSumCreditAmount + oItem.CrAmount;
                        #endregion
                    }
                    else
                    {
                        if (oItem.CFormat != "")
                        {
                            #region Account Head Wise Conversion Rate Display
                            oPdfPTable = new PdfPTable(3);
                            oPdfPTable.WidthPercentage = 100;
                            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                            oPdfPTable.SetWidths(new float[] { 370f, 77.5f, 77.5f });

                            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                            _oPdfPCellTwo = new PdfPCell(new Phrase(oItem.CFormat, _oFontStyle));
                            _oPdfPCellTwo.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCellTwo.BorderWidthLeft = 1; _oPdfPCellTwo.BorderWidthRight = 0; _oPdfPCellTwo.BorderWidthTop = 0; _oPdfPCellTwo.BorderWidthBottom = 0; _oPdfPCellTwo.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCellTwo);

                            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase("  ", _oFontStyle));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 1; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                            oPdfPTable.CompleteRow();

                            _oPdfPCell = new PdfPCell(oPdfPTable);
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                            _oPdfPTable.CompleteRow();
                            #endregion
                        }
                        if (oItem.Detail != "")
                        {
                            #region Account Head Wise Narration
                            oPdfPTable = new PdfPTable(3);
                            oPdfPTable.WidthPercentage = 100;
                            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                            oPdfPTable.SetWidths(new float[] { 370f, 77.5f, 77.5f });

                            PdfPTable oPdfPTablett = new PdfPTable(3);
                            oPdfPTablett.WidthPercentage = 100;
                            oPdfPTablett.HorizontalAlignment = Element.ALIGN_LEFT;
                            oPdfPTablett.SetWidths(new float[] { 10f, 320f, 30f });
                            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

                            _oPdfPCellTwo = new PdfPCell(new Phrase(" ", _oFontStyle));
                            _oPdfPCellTwo.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCellTwo.BorderWidthLeft = 0; _oPdfPCellTwo.BorderWidthRight = 0; _oPdfPCellTwo.BorderWidthTop = 0; _oPdfPCellTwo.BorderWidthBottom = 0; _oPdfPCellTwo.BackgroundColor = BaseColor.WHITE; oPdfPTablett.AddCell(_oPdfPCellTwo);

                            _oPdfPCellTwo = new PdfPCell(new Phrase("(" + oItem.Detail + ")", _oFontStyle));
                            _oPdfPCellTwo.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCellTwo.BorderWidthLeft = 0; _oPdfPCellTwo.BorderWidthRight = 0; _oPdfPCellTwo.BorderWidthTop = 0; _oPdfPCellTwo.BorderWidthBottom = 0; _oPdfPCellTwo.BackgroundColor = BaseColor.WHITE; oPdfPTablett.AddCell(_oPdfPCellTwo);

                            _oPdfPCellTwo = new PdfPCell(new Phrase("", _oFontStyle));
                            _oPdfPCellTwo.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCellTwo.BorderWidthLeft = 0; _oPdfPCellTwo.BorderWidthRight = 0; _oPdfPCellTwo.BorderWidthTop = 0; _oPdfPCellTwo.BorderWidthBottom = 0; _oPdfPCellTwo.BackgroundColor = BaseColor.WHITE; oPdfPTablett.AddCell(_oPdfPCellTwo);
                            oPdfPTablett.CompleteRow();

                            _oPdfPCellTwo = new PdfPCell(oPdfPTablett);
                            _oPdfPCellTwo.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCellTwo.BorderWidthLeft = 1; _oPdfPCellTwo.BorderWidthRight = 0; _oPdfPCellTwo.BorderWidthTop = 0; _oPdfPCellTwo.BorderWidthBottom = 0; _oPdfPCellTwo.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCellTwo);

                            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase("  ", _oFontStyle));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 1; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                            oPdfPTable.CompleteRow();

                            _oPdfPCell = new PdfPCell(oPdfPTable);
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                            _oPdfPTable.CompleteRow();
                            #endregion
                        }
                    }
                }
                else if (oItem.ObjType == EnumBreakdownType.CostCenter)
                {
                    #region Cost Center
                    oPdfPTable = new PdfPTable(3);
                    oPdfPTable.WidthPercentage = 100;
                    oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.SetWidths(new float[] { 370f, 77.5f, 77.5f });

                    PdfPTable oPdfPTablett = new PdfPTable(4);
                    oPdfPTablett.WidthPercentage = 100;
                    oPdfPTablett.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTablett.SetWidths(new float[] { 10f, 190f, 150f, 10f });
                    if (oItem.CCID == 0)
                    {
                        _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                        _oPdfPCellTwo = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCellTwo.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCellTwo.BorderWidthLeft = 0; _oPdfPCellTwo.BorderWidthRight = 0; _oPdfPCellTwo.BorderWidthTop = 0; _oPdfPCellTwo.BorderWidthBottom = 0; _oPdfPCellTwo.BackgroundColor = BaseColor.WHITE; oPdfPTablett.AddCell(_oPdfPCellTwo);

                        _oPdfPCellTwo = new PdfPCell(new Phrase(oItem.CCName, _oFontStyle));
                        _oPdfPCellTwo.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCellTwo.BorderWidthLeft = 0; _oPdfPCellTwo.BorderWidthRight = 0; _oPdfPCellTwo.BorderWidthTop = 0; _oPdfPCellTwo.BorderWidthBottom = 0; _oPdfPCellTwo.BackgroundColor = BaseColor.WHITE; oPdfPTablett.AddCell(_oPdfPCellTwo);

                        _oPdfPCellTwo = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCellTwo.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCellTwo.BorderWidthLeft = 0; _oPdfPCellTwo.BorderWidthRight = 0; _oPdfPCellTwo.BorderWidthTop = 0; _oPdfPCellTwo.BorderWidthBottom = 0; _oPdfPCellTwo.BackgroundColor = BaseColor.WHITE; oPdfPTablett.AddCell(_oPdfPCellTwo);

                        _oPdfPCellTwo = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCellTwo.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCellTwo.BorderWidthLeft = 0; _oPdfPCellTwo.BorderWidthRight = 0; _oPdfPCellTwo.BorderWidthTop = 0; _oPdfPCellTwo.BorderWidthBottom = 0; _oPdfPCellTwo.BackgroundColor = BaseColor.WHITE; oPdfPTablett.AddCell(_oPdfPCellTwo);
                        oPdfPTablett.CompleteRow();
                    }
                    else
                    {
                        _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                        _oPdfPCellTwo = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCellTwo.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCellTwo.BorderWidthLeft = 0; _oPdfPCellTwo.BorderWidthRight = 0; _oPdfPCellTwo.BorderWidthTop = 0; _oPdfPCellTwo.BorderWidthBottom = 0; _oPdfPCellTwo.BackgroundColor = BaseColor.WHITE; oPdfPTablett.AddCell(_oPdfPCellTwo);

                        _oPdfPCellTwo = new PdfPCell(new Phrase(oItem.CCName, _oFontStyle));
                        _oPdfPCellTwo.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCellTwo.BorderWidthLeft = 0; _oPdfPCellTwo.BorderWidthRight = 0; _oPdfPCellTwo.BorderWidthTop = 0; _oPdfPCellTwo.BorderWidthBottom = 0; _oPdfPCellTwo.BackgroundColor = BaseColor.WHITE; oPdfPTablett.AddCell(_oPdfPCellTwo);

                        _oPdfPCellTwo = new PdfPCell(new Phrase((_oVoucher.BaseCurrencyID == oItem.CID ? oItem.CSymbol + " " + Global.MillionFormat(oItem.Amount) : oItem.CSymbol + " " + Global.MillionFormat(oItem.CAmount) + " @ " + Global.MillionFormat(oItem.CRate) + " " + _oVoucher.BaseCUSymbol + " " + Global.MillionFormat(oItem.CAmount * oItem.CRate)) + " " + (oItem.IsDr ? "Dr" : "Cr"), _oFontStyle));
                        _oPdfPCellTwo.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCellTwo.BorderWidthLeft = 0; _oPdfPCellTwo.BorderWidthRight = 0; _oPdfPCellTwo.BorderWidthTop = 0; _oPdfPCellTwo.BorderWidthBottom = 0; _oPdfPCellTwo.BackgroundColor = BaseColor.WHITE; oPdfPTablett.AddCell(_oPdfPCellTwo);

                        _oPdfPCellTwo = new PdfPCell(new Phrase("", _oFontStyle));
                        _oPdfPCellTwo.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCellTwo.BorderWidthLeft = 0; _oPdfPCellTwo.BorderWidthRight = 0; _oPdfPCellTwo.BorderWidthTop = 0; _oPdfPCellTwo.BorderWidthBottom = 0; _oPdfPCellTwo.BackgroundColor = BaseColor.WHITE; oPdfPTablett.AddCell(_oPdfPCellTwo);
                        oPdfPTablett.CompleteRow();
                    }
                    _oPdfPCellTwo = new PdfPCell(oPdfPTablett);
                    _oPdfPCellTwo.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCellTwo.BorderWidthLeft = 1; _oPdfPCellTwo.BorderWidthRight = 0; _oPdfPCellTwo.BorderWidthTop = 0; _oPdfPCellTwo.BorderWidthBottom = 0; _oPdfPCellTwo.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCellTwo);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("  ", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 1; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    oPdfPTable.CompleteRow();

                    _oPdfPCell = new PdfPCell(oPdfPTable);
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion
                }
                else if (oItem.ObjType == EnumBreakdownType.BillReference || oItem.ObjType==EnumBreakdownType.SubledgerBill)
                {
                    #region Voucher Bill
                    oPdfPTable = new PdfPTable(3);
                    oPdfPTable.WidthPercentage = 100;
                    oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.SetWidths(new float[] { 370f, 77.5f, 77.5f });

                    PdfPTable oPdfPTablett = new PdfPTable(4);
                    oPdfPTablett.WidthPercentage = 100;
                    oPdfPTablett.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTablett.SetWidths(new float[] { 10f, 190f, 150f, 10f });
                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

                    _oPdfPCellTwo = new PdfPCell(new Phrase(" ", _oFontStyle));
                    _oPdfPCellTwo.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCellTwo.BorderWidthLeft = 0; _oPdfPCellTwo.BorderWidthRight = 0; _oPdfPCellTwo.BorderWidthTop = 0; _oPdfPCellTwo.BorderWidthBottom = 0; _oPdfPCellTwo.BackgroundColor = BaseColor.WHITE; oPdfPTablett.AddCell(_oPdfPCellTwo);

                    _oPdfPCellTwo = new PdfPCell(new Phrase(oItem.TrTypeStr + ":" + oItem.BillNo, _oFontStyle));
                    _oPdfPCellTwo.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCellTwo.BorderWidthLeft = 0; _oPdfPCellTwo.BorderWidthRight = 0; _oPdfPCellTwo.BorderWidthTop = 0; _oPdfPCellTwo.BorderWidthBottom = 0; _oPdfPCellTwo.BackgroundColor = BaseColor.WHITE; oPdfPTablett.AddCell(_oPdfPCellTwo);

                    _oPdfPCellTwo = new PdfPCell(new Phrase((_oVoucher.BaseCurrencyID == oItem.CID ? oItem.CSymbol + " " + Global.MillionFormat(oItem.Amount) : oItem.CSymbol + " " + Global.MillionFormat(oItem.CAmount) + " @ " + Global.MillionFormat(oItem.CRate) + " " + _oVoucher.BaseCUSymbol + " " + Global.MillionFormat(oItem.CAmount * oItem.CRate)) + " " + (oItem.IsDr ? "Dr" : "Cr"), _oFontStyle));
                    _oPdfPCellTwo.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCellTwo.BorderWidthLeft = 0; _oPdfPCellTwo.BorderWidthRight = 0; _oPdfPCellTwo.BorderWidthTop = 0; _oPdfPCellTwo.BorderWidthBottom = 0; _oPdfPCellTwo.BackgroundColor = BaseColor.WHITE; oPdfPTablett.AddCell(_oPdfPCellTwo);

                    _oPdfPCellTwo = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCellTwo.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCellTwo.BorderWidthLeft = 0; _oPdfPCellTwo.BorderWidthRight = 0; _oPdfPCellTwo.BorderWidthTop = 0; _oPdfPCellTwo.BorderWidthBottom = 0; _oPdfPCellTwo.BackgroundColor = BaseColor.WHITE; oPdfPTablett.AddCell(_oPdfPCellTwo);
                    oPdfPTablett.CompleteRow();

                    _oPdfPCellTwo = new PdfPCell(oPdfPTablett);
                    _oPdfPCellTwo.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCellTwo.BorderWidthLeft = 1; _oPdfPCellTwo.BorderWidthRight = 0; _oPdfPCellTwo.BorderWidthTop = 0; _oPdfPCellTwo.BorderWidthBottom = 0; _oPdfPCellTwo.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCellTwo);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("  ", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 1; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    oPdfPTable.CompleteRow();

                    _oPdfPCell = new PdfPCell(oPdfPTable);
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion
                }
                else if (oItem.ObjType == EnumBreakdownType.InventoryReference)
                {
                    #region Inventory Reference
                    oPdfPTable = new PdfPTable(3);
                    oPdfPTable.WidthPercentage = 100;
                    oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.SetWidths(new float[] { 370f, 77.5f, 77.5f });

                    PdfPTable oPdfPTablett = new PdfPTable(4);
                    oPdfPTablett.WidthPercentage = 100;
                    oPdfPTablett.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTablett.SetWidths(new float[] { 10f, 190f, 150f, 10f });
                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

                    _oPdfPCellTwo = new PdfPCell(new Phrase(" ", _oFontStyle));
                    _oPdfPCellTwo.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCellTwo.BorderWidthLeft = 0; _oPdfPCellTwo.BorderWidthRight = 0; _oPdfPCellTwo.BorderWidthTop = 0; _oPdfPCellTwo.BorderWidthBottom = 0; _oPdfPCellTwo.BackgroundColor = BaseColor.WHITE; oPdfPTablett.AddCell(_oPdfPCellTwo);

                    _oPdfPCellTwo = new PdfPCell(new Phrase(oItem.PName, _oFontStyle));
                    _oPdfPCellTwo.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCellTwo.BorderWidthLeft = 0; _oPdfPCellTwo.BorderWidthRight = 0; _oPdfPCellTwo.BorderWidthTop = 0; _oPdfPCellTwo.BorderWidthBottom = 0; _oPdfPCellTwo.BackgroundColor = BaseColor.WHITE; oPdfPTablett.AddCell(_oPdfPCellTwo);

                    _oPdfPCellTwo = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty) + " " + oItem.MUName + " @ " + Global.MillionFormat(oItem.UPrice) + " " + oItem.CSymbol + " " + Global.MillionFormat(oItem.Amount) + " " + (oItem.IsDr ? "Dr" : "Cr"), _oFontStyle));
                    //_oPdfPCellTwo = new PdfPCell(new Phrase((_oVoucher.BaseCurrencyId == oItem.CID ? oItem.CSymbol + " " + Global.MillionFormat(oItem.Amount) : oItem.CSymbol + " " + Global.MillionFormat(oItem.CAmount) + " @ " + Global.MillionFormat(oItem.CRate) + " " + _oVoucher.BaseCurrencySymbol + " " + Global.MillionFormat(oItem.CAmount * oItem.CRate)) + " " + (oItem.IsDr ? "Dr" : "Cr"), _oFontStyle));
                    _oPdfPCellTwo.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCellTwo.BorderWidthLeft = 0; _oPdfPCellTwo.BorderWidthRight = 0; _oPdfPCellTwo.BorderWidthTop = 0; _oPdfPCellTwo.BorderWidthBottom = 0; _oPdfPCellTwo.BackgroundColor = BaseColor.WHITE; oPdfPTablett.AddCell(_oPdfPCellTwo);

                    _oPdfPCellTwo = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCellTwo.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCellTwo.BorderWidthLeft = 0; _oPdfPCellTwo.BorderWidthRight = 0; _oPdfPCellTwo.BorderWidthTop = 0; _oPdfPCellTwo.BorderWidthBottom = 0; _oPdfPCellTwo.BackgroundColor = BaseColor.WHITE; oPdfPTablett.AddCell(_oPdfPCellTwo);
                    oPdfPTablett.CompleteRow();

                    _oPdfPCellTwo = new PdfPCell(oPdfPTablett);
                    _oPdfPCellTwo.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCellTwo.BorderWidthLeft = 1; _oPdfPCellTwo.BorderWidthRight = 0; _oPdfPCellTwo.BorderWidthTop = 0; _oPdfPCellTwo.BorderWidthBottom = 0; _oPdfPCellTwo.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCellTwo);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("  ", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 1; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    oPdfPTable.CompleteRow();

                    _oPdfPCell = new PdfPCell(oPdfPTable);
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion
                }
                else if (oItem.ObjType == EnumBreakdownType.ChequeReference)
                {
                    #region Voucher Reference
                    oPdfPTable = new PdfPTable(3);
                    oPdfPTable.WidthPercentage = 100;
                    oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.SetWidths(new float[] { 370f, 77.5f, 77.5f });

                    PdfPTable oPdfPTablett = new PdfPTable(4);
                    oPdfPTablett.WidthPercentage = 100;
                    oPdfPTablett.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTablett.SetWidths(new float[] { 10f, 190f, 150f, 10f });
                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

                    _oPdfPCellTwo = new PdfPCell(new Phrase(" ", _oFontStyle));
                    _oPdfPCellTwo.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCellTwo.BorderWidthLeft = 0; _oPdfPCellTwo.BorderWidthRight = 0; _oPdfPCellTwo.BorderWidthTop = 0; _oPdfPCellTwo.BorderWidthBottom = 0; _oPdfPCellTwo.BackgroundColor = BaseColor.WHITE; oPdfPTablett.AddCell(_oPdfPCellTwo);

                    _oPdfPCellTwo = new PdfPCell(new Phrase(oItem.Detail, _oFontStyle));
                    _oPdfPCellTwo.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCellTwo.BorderWidthLeft = 0; _oPdfPCellTwo.BorderWidthRight = 0; _oPdfPCellTwo.BorderWidthTop = 0; _oPdfPCellTwo.BorderWidthBottom = 0; _oPdfPCellTwo.BackgroundColor = BaseColor.WHITE; oPdfPTablett.AddCell(_oPdfPCellTwo);

                    _oPdfPCellTwo = new PdfPCell(new Phrase((_oVoucher.BaseCurrencyID == oItem.CID ? oItem.CSymbol + " " + Global.MillionFormat(oItem.Amount) : oItem.CSymbol + " " + Global.MillionFormat(oItem.CAmount) + " @ " + Global.MillionFormat(oItem.CRate) + " " + _oVoucher.BaseCUSymbol + " " + Global.MillionFormat(oItem.CAmount * oItem.CRate)) + " " + (oItem.IsDr ? "Dr" : "Cr"), _oFontStyle));
                    _oPdfPCellTwo.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCellTwo.BorderWidthLeft = 0; _oPdfPCellTwo.BorderWidthRight = 0; _oPdfPCellTwo.BorderWidthTop = 0; _oPdfPCellTwo.BorderWidthBottom = 0; _oPdfPCellTwo.BackgroundColor = BaseColor.WHITE; oPdfPTablett.AddCell(_oPdfPCellTwo);

                    _oPdfPCellTwo = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCellTwo.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCellTwo.BorderWidthLeft = 0; _oPdfPCellTwo.BorderWidthRight = 0; _oPdfPCellTwo.BorderWidthTop = 0; _oPdfPCellTwo.BorderWidthBottom = 0; _oPdfPCellTwo.BackgroundColor = BaseColor.WHITE; oPdfPTablett.AddCell(_oPdfPCellTwo);
                    oPdfPTablett.CompleteRow();

                    _oPdfPCellTwo = new PdfPCell(oPdfPTablett);
                    _oPdfPCellTwo.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCellTwo.BorderWidthLeft = 1; _oPdfPCellTwo.BorderWidthRight = 0; _oPdfPCellTwo.BorderWidthTop = 0; _oPdfPCellTwo.BorderWidthBottom = 0; _oPdfPCellTwo.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCellTwo);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("  ", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 1; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    oPdfPTable.CompleteRow();

                    _oPdfPCell = new PdfPCell(oPdfPTable);
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion
                }

                //_oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
                //_oPdfPCell = new PdfPCell(new Phrase(i.ToString(), _oFontStyle));
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 1; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                //_oPdfPTable.CompleteRow();

                if (nRemainningCount == 51)
                {
                    nRemainningCount = 50;
                }
                if (nCount >= 43 && nRemainningCount < 51)
                {
                    #region Continue
                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                    _oPdfPCell = new PdfPCell(new Phrase("Continue..", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 1; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion

                    _oDocument.Add(_oPdfPTable);
                    _oDocument.NewPage();
                    _oPdfPTable.DeleteBodyRows();
                    #region Add Header Table
                    _oPdfPCell = new PdfPCell(_oPdfPTableTemp);
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion
                    nRemainningCount = nRemainningCount - nCount;

                    nCount = 0;
                }
                else if (nCount >= 51)
                {
                    #region Continue
                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                    _oPdfPCell = new PdfPCell(new Phrase("Continue..", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 1; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion

                    _oDocument.Add(_oPdfPTable);
                    _oDocument.NewPage();
                    _oPdfPTable.DeleteBodyRows();
                    #region Add Header Table
                    _oPdfPCell = new PdfPCell(_oPdfPTableTemp);
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion
                    nRemainningCount = nRemainningCount - nCount;
                    nCount = 0;
                }
            }

            #region Blank Row
            if (nTotalRowCount < 43)
            {
                for (int i = nTotalRowCount + 1; i <= 43; i++)
                {
                    oPdfPTable = new PdfPTable(3);
                    oPdfPTable.WidthPercentage = 100;
                    oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.SetWidths(new float[] { 370f, 77.5f, 77.5f });

                    _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 1; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    oPdfPTable.CompleteRow();

                    _oPdfPCell = new PdfPCell(oPdfPTable);
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                }
            }
            #endregion

            #region Footer Part
            #region Total Voucher Amount
            oPdfPTable = new PdfPTable(4);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 50f, 320f, 77.5f, 77.5f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("In words : " + Global.TakaWords(_nSumDebitAmount), _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oVoucher.BaseCUSymbol + " " + Global.MillionFormat(_nSumDebitAmount), _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oVoucher.BaseCUSymbol + " " + Global.MillionFormat(_nSumCreditAmount), _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 1; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Blank Space
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Narration Print
            oPdfPTable = new PdfPTable(2);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 50f, 475f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Narration :", _oFontStyle));
            _oPdfPCell.FixedHeight = 50f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oVoucher.Narration, _oFontStyle));
            _oPdfPCell.FixedHeight = 50f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region print Signature Captions
            oPdfPTable = new PdfPTable(4);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 131.25f, 131.25f, 131.25f, 131.25f });

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("__________________\nPrepared By\n" + _oVoucher.PreparedByName, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("__________________ \n Checked By", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("__________________ \n Varified By", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("__________________ \n Approved By", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            #endregion


            #region Old Program

            //int nCount = 0;
            //int nRawCount= 0;
            //_oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            //_oFontStyleForCC = FontFactory.GetFont("Tahoma", 8f, 0);
            //_oFontStyleForCCCategory = FontFactory.GetFont("Tahoma", 8f, 1);
            //foreach (VoucherDetail oItem in _oVoucherDetails)
            //{
            //    oPdfPTable = new PdfPTable(3);
            //    oPdfPTable.WidthPercentage = 100;
            //    oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            //    oPdfPTable.SetWidths(new float[] { 339.7f, 77.5f, 77.5f });
            //    nCount++;

            //    #region Header Insertion
            //    _oPdfPCell = new PdfPCell(new Phrase("["+oItem.AccountCode+"]"+oItem.AccountHeadName, _oFontStyleTwo));
            //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            //    if (oItem.DebitAmount == 0)
            //    {
            //        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //    }
            //    else
            //    {
            //        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.DebitAmount), _oFontStyle));
            //    }
            //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            //    if (oItem.CreditAmount == 0)
            //    {
            //        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //    }
            //    else
            //    {
            //        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.CreditAmount), _oFontStyle));
            //    }
            //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 1; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //    if (!oItem.IsDr)
            //    {
            //        _nSumCreditAmount += oItem.Amount;
            //    }
            //    else
            //    {
            //        _nSumDebitAmount += oItem.Amount;
            //    }
            //    oPdfPTable.CompleteRow();
            //    #endregion

            //    _oPdfPCell = new PdfPCell(oPdfPTable);
            //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //    _oPdfPTable.CompleteRow();
            //    #region Report Vocher Reference
            //    if (oItem.VRs != null)
            //    {
            //        foreach (VoucherReference OVR in oItem.VRs)
            //        {

            //            oPdfPTable = new PdfPTable(3);
            //            oPdfPTable.WidthPercentage = 100;
            //            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            //            oPdfPTable.SetWidths(new float[] { 339.7f, 77.5f, 77.5f });

            //            nCount++;

            //            _oPdfPCell = new PdfPCell(new Phrase("   " + OVR.Description + "  "+oItem.CurrencySymbol +" " + Global.MillionFormat(OVR.Amount), _oFontStyle));
            //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //            _oPdfPCell = new PdfPCell(new Phrase("  ", _oFontStyle));
            //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //            _oPdfPCell = new PdfPCell(new Phrase("  ", _oFontStyle));
            //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 1; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            //            oPdfPTable.CompleteRow();


            //            _oPdfPCell = new PdfPCell(oPdfPTable);
            //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //            _oPdfPTable.CompleteRow();
            //        }
            //    }
            //    #endregion
            //    #region  Vocher Bill Reference
            //    if (oItem.VoucherBillTrs != null)
            //    {
            //        foreach (VoucherBillTransaction oVBillTr in oItem.VoucherBillTrs)
            //        {

            //            oPdfPTable = new PdfPTable(3);
            //            oPdfPTable.WidthPercentage = 100;
            //            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            //            oPdfPTable.SetWidths(new float[] { 339.7f, 77.5f, 77.5f });

            //            nCount++;


            //            _oPdfPCell = new PdfPCell(new Phrase("   " +oVBillTr.TrTypeST+" "+oVBillTr.BillNo + "    "+oItem.CurrencySymbol+" " + Global.MillionFormat(oVBillTr.Amount), _oFontStyle));
            //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //            _oPdfPCell = new PdfPCell(new Phrase("  ", _oFontStyle));
            //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //            _oPdfPCell = new PdfPCell(new Phrase("  ", _oFontStyle));
            //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 1; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            //            oPdfPTable.CompleteRow();

            //            _oPdfPCell = new PdfPCell(oPdfPTable);
            //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //            _oPdfPTable.CompleteRow();

            //            #region For Page Brack
            //            if (_oPdfPTable.Rows.Count >= 45)
            //            {
            //                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 1; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //                _oPdfPTable.CompleteRow();

            //                _oPdfPCell = new PdfPCell(new Phrase("Continued ...", _oFontStyle));
            //                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //                _oPdfPTable.CompleteRow();

            //                _oDocument.Add(_oPdfPTable);
            //                _oDocument.NewPage();

            //                _oPdfPTable.DeleteBodyRows();

            //                _oPdfPCell = new PdfPCell(_oPdfPTableTemp);
            //                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //                _oPdfPTable.CompleteRow();
            //                nCount = 0;
            //            }
            //            #endregion
            //        }
            //    }
            //    #endregion
            //    #region Print Vocher Cost center
            //    PdfPTable oPdfPTablett = new PdfPTable(2);
            //    if (oItem.CCTs != null)

            //    {
            //        int nCCD = 0;
            //        foreach (CostCenterTransaction CCT in oItem.CCTs)
            //        {

            //            oPdfPTable = new PdfPTable(3);
            //            oPdfPTable.WidthPercentage = 100;
            //            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            //            oPdfPTable.SetWidths(new float[] { 339.7f, 77.5f, 77.5f });

            //            nCount++;

            //             oPdfPTablett = new PdfPTable(3);
            //            oPdfPTablett.WidthPercentage = 100;
            //            oPdfPTablett.HorizontalAlignment = Element.ALIGN_LEFT;
            //            oPdfPTablett.SetWidths(new float[] { 150f, 80.2f, 60f });
            //            if (nCCD==0)
            //            {
            //                _oPdfPCellTwo = new PdfPCell(new Phrase(" " + CCT.CategoryName, _oFontStyleForCCCategory));
            //            _oPdfPCellTwo.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCellTwo.BorderWidthLeft = 0; _oPdfPCellTwo.BorderWidthRight = 0; _oPdfPCellTwo.BorderWidthTop = 0; _oPdfPCellTwo.BorderWidthBottom = 0; _oPdfPCellTwo.BackgroundColor = BaseColor.WHITE; oPdfPTablett.AddCell(_oPdfPCellTwo);

            //            _oPdfPCellTwo = new PdfPCell(new Phrase(" " , _oFontStyleForCC));
            //            _oPdfPCellTwo.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCellTwo.BorderWidthLeft = 0; _oPdfPCellTwo.BorderWidthRight = 0; _oPdfPCellTwo.BorderWidthTop = 0; _oPdfPCellTwo.BorderWidthBottom = 0; _oPdfPCellTwo.BackgroundColor = BaseColor.WHITE; oPdfPTablett.AddCell(_oPdfPCellTwo);

            //            _oPdfPCellTwo = new PdfPCell(new Phrase("", _oFontStyleForCC));
            //            _oPdfPCellTwo.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCellTwo.BorderWidthLeft = 0; _oPdfPCellTwo.BorderWidthRight = 0; _oPdfPCellTwo.BorderWidthTop = 0; _oPdfPCellTwo.BorderWidthBottom = 0; _oPdfPCellTwo.BackgroundColor = BaseColor.WHITE; oPdfPTablett.AddCell(_oPdfPCellTwo);

            //            oPdfPTablett.CompleteRow();
            //            }


            //            _oPdfPCellTwo = new PdfPCell(new Phrase("   " + CCT.CostCenterName, _oFontStyleForCC));
            //            _oPdfPCellTwo.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCellTwo.BorderWidthLeft = 0; _oPdfPCellTwo.BorderWidthRight = 0; _oPdfPCellTwo.BorderWidthTop = 0; _oPdfPCellTwo.BorderWidthBottom = 0; _oPdfPCellTwo.BackgroundColor = BaseColor.WHITE; oPdfPTablett.AddCell(_oPdfPCellTwo);

            //            _oPdfPCellTwo = new PdfPCell(new Phrase(oItem.CurrencySymbol + " " + Global.MillionFormat(CCT.Amount) + " " + oItem.IsDRSt, _oFontStyleForCC));
            //            _oPdfPCellTwo.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCellTwo.BorderWidthLeft = 0; _oPdfPCellTwo.BorderWidthRight = 0; _oPdfPCellTwo.BorderWidthTop = 0; _oPdfPCellTwo.BorderWidthBottom = 0; _oPdfPCellTwo.BackgroundColor = BaseColor.WHITE; oPdfPTablett.AddCell(_oPdfPCellTwo);

            //            _oPdfPCellTwo = new PdfPCell(new Phrase("", _oFontStyleForCC));
            //            _oPdfPCellTwo.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCellTwo.BorderWidthLeft = 0; _oPdfPCellTwo.BorderWidthRight = 0; _oPdfPCellTwo.BorderWidthTop = 0; _oPdfPCellTwo.BorderWidthBottom = 0; _oPdfPCellTwo.BackgroundColor = BaseColor.WHITE; oPdfPTablett.AddCell(_oPdfPCellTwo);

            //            oPdfPTablett.CompleteRow();

            //            nCCD = CCT.CCID;

            //            _oPdfPCellTwo = new PdfPCell(oPdfPTablett);
            //            _oPdfPCellTwo.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCellTwo.BorderWidthLeft = 1; _oPdfPCellTwo.BorderWidthRight = 0; _oPdfPCellTwo.BorderWidthTop = 0; _oPdfPCellTwo.BorderWidthBottom = 0; _oPdfPCellTwo.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCellTwo);



            //            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


            //            _oPdfPCell = new PdfPCell(new Phrase("  ", _oFontStyle));
            //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 1; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //           oPdfPTable.CompleteRow();


            //            _oPdfPCell = new PdfPCell(oPdfPTable);
            //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //            _oPdfPTable.CompleteRow();
            //            #region For Page Brack
            //            if (_oPdfPTable.Rows.Count >= 45)
            //            {
            //                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 1; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //                _oPdfPTable.CompleteRow();

            //                _oPdfPCell = new PdfPCell(new Phrase("Continued ...", _oFontStyle));
            //                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //                _oPdfPTable.CompleteRow();

            //                _oDocument.Add(_oPdfPTable);
            //                _oDocument.NewPage();

            //                _oPdfPTable.DeleteBodyRows();

            //                _oPdfPCell = new PdfPCell(_oPdfPTableTemp);
            //                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //                _oPdfPTable.CompleteRow();
            //                nCount = 0;
            //            }
            //            #endregion
            //        }
            //    }
            //    #endregion

            //    #region  Vocher Transaction
            //    if (oItem.VPTransactions != null)
            //    {
            //        foreach (VPTransaction oVPT in oItem.VPTransactions)
            //        {

            //            oPdfPTable = new PdfPTable(3);
            //            oPdfPTable.WidthPercentage = 100;
            //            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            //            oPdfPTable.SetWidths(new float[] { 339.7f, 77.5f, 77.5f });

            //            nCount++;

            //            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);

            //            _oPdfPCell = new PdfPCell(new Phrase("   " + oVPT.ProductName + " " + Global.MillionFormat(oVPT.Qty) + "" + oVPT.MUnitName + "@" + Global.MillionFormat(oVPT.UnitPrice)+ " "+oItem.CurrencySymbol+" "+ Global.MillionFormat(oVPT.Amount), _oFontStyle));
            //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //            _oPdfPCell = new PdfPCell(new Phrase("  ", _oFontStyle));
            //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //            _oPdfPCell = new PdfPCell(new Phrase("  ", _oFontStyle));
            //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 1; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            //            oPdfPTable.CompleteRow();

            //            _oPdfPCell = new PdfPCell(oPdfPTable);
            //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //            _oPdfPTable.CompleteRow();

            //            #region For Page Brack
            //            if (_oPdfPTable.Rows.Count >= 45)
            //            {
            //                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 1; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //                _oPdfPTable.CompleteRow();

            //                _oPdfPCell = new PdfPCell(new Phrase("Continued ...", _oFontStyle));
            //                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //                _oPdfPTable.CompleteRow();

            //                _oDocument.Add(_oPdfPTable);
            //                _oDocument.NewPage();

            //                _oPdfPTable.DeleteBodyRows();

            //                _oPdfPCell = new PdfPCell(_oPdfPTableTemp);
            //                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //                _oPdfPTable.CompleteRow();
            //                nCount = 0;
            //            }
            //            #endregion
            //        }
            //    }
            //    #endregion
            //    //// For print Narration ----------------
            //    oPdfPTable = new PdfPTable(3);
            //    oPdfPTable.WidthPercentage = 100;
            //    oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            //    oPdfPTable.SetWidths(new float[] { 339.7f, 77.5f, 77.5f });

            //    nCount++;


            //    if (oItem.Narration.Length > 0)
            //    {
            //        _oPdfPCell = new PdfPCell(new Phrase("(" + oItem.Narration + ")", _oFontStyle));
            //    }
            //    else
            //    {
            //        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //    }

            //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 1; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


            //    oPdfPTable.CompleteRow();

            //    _oPdfPCell = new PdfPCell(oPdfPTable);
            //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //    _oPdfPTable.CompleteRow();

            //    #region For Page brack if raw count >45

            //    if (_oPdfPTable.Rows.Count >= 45)
            //    {
            //       // nRawCount = _oPdfPTable.Rows.Count;
            //        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 1; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //        _oPdfPTable.CompleteRow();

            //        _oPdfPCell = new PdfPCell(new Phrase("Continued ...", _oFontStyle));
            //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //        _oPdfPTable.CompleteRow();

            //        _oDocument.Add(_oPdfPTable);
            //        _oDocument.NewPage();
            //        // _oPdfPTable = new PdfPTable(1);
            //        _oPdfPTable.DeleteBodyRows();

            //        _oPdfPCell = new PdfPCell(_oPdfPTableTemp);
            //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //        _oPdfPTable.CompleteRow();


            //        nCount = 0;


            //    }
            //    #endregion Page brack
            //}
            // nRawCount = _oPdfPTable.Rows.Count;
            //#region For Print blank raw if (40-raw count)>0
            // for (int i = 0; i <= (40 - nRawCount); i++)
            //{

            //    oPdfPTable = new PdfPTable(3);
            //    oPdfPTable.WidthPercentage = 100;
            //    oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            //    oPdfPTable.SetWidths(new float[] { 339.7f, 77.5f, 77.5f });



            //    _oPdfPCell = new PdfPCell(new Phrase("  ", _oFontStyle));
            //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //    _oPdfPCell = new PdfPCell(new Phrase("  ", _oFontStyle));
            //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //    _oPdfPCell = new PdfPCell(new Phrase("  ", _oFontStyle));
            //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 1; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //    oPdfPTable.CompleteRow();

            //    _oPdfPCell = new PdfPCell(oPdfPTable);
            //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //    _oPdfPTable.CompleteRow();
            //}
            //for (int i = 1; i <= 1; i++)
            //{

            //    oPdfPTable = new PdfPTable(3);
            //    oPdfPTable.WidthPercentage = 100;
            //    oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            //    oPdfPTable.SetWidths(new float[] { 339.7f, 77.5f, 77.5f });

            //    nCount++;


            //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 1; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 1; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 1; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 1; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //    oPdfPTable.CompleteRow();

            //    _oPdfPCell = new PdfPCell(oPdfPTable);
            //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //    _oPdfPTable.CompleteRow();
            //}
            //#endregion 
            #endregion
        }
        #endregion
    }
}
