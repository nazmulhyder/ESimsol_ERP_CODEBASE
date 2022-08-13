﻿using System;
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
    public class rptMultiVoucherMultiCurrency
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

        BusinessUnit _oBusinessUnit = new BusinessUnit();
        Contractor _oContractor = new Contractor();
        ContactPersonnel _oContactPersonnel = new ContactPersonnel();
        List<SignatureSetup> _oSignatureSetups = new List<SignatureSetup>();

        double _nSumCreditAmount = 0;
        double _nSumDebitAmount = 0;       
        PdfWriter _oPdfWriter = null;
        #endregion

        public byte[] PrepareReport(List<Voucher> oVouchers, bool bLogoPrint, List<SignatureSetup> oSignatureSetups)
        {
            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
            _oDocument.SetMargins(45f, 25f, 20f, 20f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _oPdfWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 525f });
            #endregion
            
            foreach (Voucher oItem in oVouchers)
            {
                _nSumCreditAmount = 0;
                _nSumDebitAmount = 0; 
                _oBusinessUnit = new BusinessUnit();
                _oVoucher = new Voucher();
                _oVDObjs = new List<VDObj>();

                _oBusinessUnit = oItem.BusinessUnit;
                _oVoucher = oItem;
                _oVDObjs = oItem.VDObjs;
                _oSignatureSetups = oSignatureSetups;

                this.PrintHeader(bLogoPrint);
                this.PrintBody();
                //_oPdfPTable.HeaderRows = 1;                
                _oDocument.Add(_oPdfPTable);
                _oDocument.NewPage();
                _oPdfPTable.DeleteBodyRows();                
            }
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Header
        private void PrintHeader(bool bLogoPrint)
        {
            if (bLogoPrint)
            {
                #region Company Header With Logo
                PdfPTable oPdfPTable = new PdfPTable(4);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.SetWidths(new float[] { 70f, 267.5f, 60f, 137.5f });

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase("Print Date: " + DateTime.Now.ToString("dd MMM yyyy :hh:mm"), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Colspan = 4; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();

                #region Business Unit
                PdfPTable oPdfPTableBU = new PdfPTable(2);
                oPdfPTableBU.WidthPercentage = 100;
                oPdfPTableBU.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTableBU.SetWidths(new float[] { 70f, 267.5f });

                if (_oBusinessUnit.BULogo != null)
                {
                    iTextSharp.text.Image oImag = iTextSharp.text.Image.GetInstance(_oBusinessUnit.BULogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                    oImag.ScaleAbsolute(70f, 55f);
                    _oPdfPCell = new PdfPCell(oImag);
                    _oPdfPCell.Rowspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.Border = 0; _oPdfPCell.PaddingRight = 0f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableBU.AddCell(_oPdfPCell);
                }
                else
                {
                    _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.Rowspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.Border = 0; _oPdfPCell.PaddingRight = 0f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableBU.AddCell(_oPdfPCell);
                }

                _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Name, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableBU.AddCell(_oPdfPCell);
                oPdfPTableBU.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Address, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableBU.AddCell(_oPdfPCell);
                oPdfPTableBU.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase("Tel: " + _oBusinessUnit.Phone, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableBU.AddCell(_oPdfPCell);
                oPdfPTableBU.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase("Email: " + _oBusinessUnit.Email + " " + _oBusinessUnit.WebAddress, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableBU.AddCell(_oPdfPCell);
                oPdfPTableBU.CompleteRow();
                #endregion

                _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(oPdfPTableBU);
                //_oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Name, _oFontStyle));
                _oPdfPCell.Colspan = 2; _oPdfPCell.Rowspan = 5; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase("Business Unit", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase(_oVoucher.BUName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();


                //_oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                //_oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Address, _oFontStyle));
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Voucher Type", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase(_oVoucher.VoucherName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();

                //_oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                //_oPdfPCell = new PdfPCell(new Phrase("Tel: " + _oBusinessUnit.Phone, _oFontStyle));
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Voucher No", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase(_oVoucher.VoucherNo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();

                //_oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                //_oPdfPCell = new PdfPCell(new Phrase("Email: " + _oBusinessUnit.Email + " " + _oBusinessUnit.WebAddress, _oFontStyle));
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Voucher Date", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase(_oVoucher.VoucherDateInString, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();

                //_oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Batch No", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase(_oVoucher.VoucherBatchNO, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.ExtraParagraphSpace = 10f; _oPdfPCell.Colspan = 4; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion
            }
            else
            {
                #region Company Header without Logo
                PdfPTable oPdfPTable = new PdfPTable(3);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.SetWidths(new float[] { 267.5f, 100f, 167.5f });

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase("Print Date: " + DateTime.Now.ToString("dd MMM yyyy :hh:mm"), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Colspan = 3; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();

                #region Business Unit
                PdfPTable oPdfPTableBU = new PdfPTable(1);
                oPdfPTableBU.WidthPercentage = 100;
                oPdfPTableBU.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTableBU.SetWidths(new float[] { 267.5f });

                _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Name, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableBU.AddCell(_oPdfPCell);
                oPdfPTableBU.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Address, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableBU.AddCell(_oPdfPCell);
                oPdfPTableBU.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase("Tel: " + _oBusinessUnit.Phone, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableBU.AddCell(_oPdfPCell);
                oPdfPTableBU.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase("Email: " + _oBusinessUnit.Email + " " + _oBusinessUnit.WebAddress, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableBU.AddCell(_oPdfPCell);
                oPdfPTableBU.CompleteRow();
                #endregion

                _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(oPdfPTableBU);
                //_oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Name, _oFontStyle));
                _oPdfPCell.Rowspan = 5; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase("Business Unit", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase(_oVoucher.BUName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();


                //_oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                //_oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Address, _oFontStyle));
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Voucher Type", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase(_oVoucher.VoucherName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();

                //_oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                //_oPdfPCell = new PdfPCell(new Phrase("Tel: " + _oBusinessUnit.Phone, _oFontStyle));
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Voucher No", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase(_oVoucher.VoucherNo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();

                //_oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                //_oPdfPCell = new PdfPCell(new Phrase("Email: " + _oBusinessUnit.Email + " " + _oBusinessUnit.WebAddress, _oFontStyle));
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Voucher Date", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase(_oVoucher.VoucherDateInString, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();

                //_oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Batch No", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase(_oVoucher.VoucherBatchNO, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.ExtraParagraphSpace = 10f; _oPdfPCell.Colspan = 3; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion
            }
        }
        #endregion

        #region Report Body
        private void PrintBody()
        {
            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 175f, 175f, 175f });

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

            _oPdfPTableTemp = new PdfPTable(1);
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
                    if (oItem.AHID > 0)
                    {
                        #region Voucher Detail
                        oPdfPTable = new PdfPTable(3);
                        oPdfPTable.WidthPercentage = 100;
                        oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPTable.SetWidths(new float[] { 370f, 77.5f, 77.5f });

                        _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
                        _oPdfPCell = new PdfPCell(new Phrase("[" + oItem.AHCode + "]" + oItem.AHName, _oFontStyle));
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
                    if (oItem.CCID == 0)
                    {
                        oPdfPTable = new PdfPTable(3);
                        oPdfPTable.WidthPercentage = 100;
                        oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPTable.SetWidths(new float[] { 370f, 77.5f, 77.5f });

                        PdfPTable oPdfPTablett = new PdfPTable(4);
                        oPdfPTablett.WidthPercentage = 100;
                        oPdfPTablett.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPTablett.SetWidths(new float[] { 10f, 190f, 150f, 10f });

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
                    }
                    else
                    {
                        oPdfPTable = new PdfPTable(3);
                        oPdfPTable.WidthPercentage = 100;
                        oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPTable.SetWidths(new float[] { 370f, 77.5f, 77.5f });

                        PdfPTable oPdfPTablett = new PdfPTable(4);
                        oPdfPTablett.WidthPercentage = 100;
                        oPdfPTablett.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPTablett.SetWidths(new float[] { 10f, 190f, 150f, 10f });

                        _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                        _oPdfPCellTwo = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCellTwo.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCellTwo.BorderWidthLeft = 0; _oPdfPCellTwo.BorderWidthRight = 0; _oPdfPCellTwo.BorderWidthTop = 0; _oPdfPCellTwo.BorderWidthBottom = 0; _oPdfPCellTwo.BackgroundColor = BaseColor.WHITE; oPdfPTablett.AddCell(_oPdfPCellTwo);

                        _oPdfPCellTwo = new PdfPCell(new Phrase(oItem.CCName, _oFontStyle));
                        _oPdfPCellTwo.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCellTwo.BorderWidthLeft = 0; _oPdfPCellTwo.BorderWidthRight = 0; _oPdfPCellTwo.BorderWidthTop = 0; _oPdfPCellTwo.BorderWidthBottom = 0; _oPdfPCellTwo.BackgroundColor = BaseColor.WHITE; oPdfPTablett.AddCell(_oPdfPCellTwo);

                        _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                        _oPdfPCellTwo = new PdfPCell(new Phrase((_oVoucher.BaseCurrencyID == oItem.CID ? oItem.CSymbol + " " + Global.MillionFormat(oItem.Amount) : oItem.CSymbol + " " + Global.MillionFormat(oItem.CAmount) + " @ " + Global.MillionFormatActualDigit(oItem.CRate) + " " + _oVoucher.BaseCUSymbol + " " + Global.MillionFormat(oItem.AmountBC)) + " " + (oItem.IsDr ? "Dr" : "Cr"), _oFontStyle));
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
                    }

                    if (oItem.CCID != 0)
                    {
                        if (oItem.Detail != "")
                        {
                            #region Subledger Wise Narration
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
                    #endregion                    
                }
                else if (oItem.ObjType == EnumBreakdownType.BillReference || oItem.ObjType == EnumBreakdownType.SubledgerBill)
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

                    _oPdfPCellTwo = new PdfPCell(new Phrase(oItem.TrTypeStr + " : " + oItem.BillNo+",  Dt : "+ oItem.BillDateStr, _oFontStyle));
                    _oPdfPCellTwo.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCellTwo.BorderWidthLeft = 0; _oPdfPCellTwo.BorderWidthRight = 0; _oPdfPCellTwo.BorderWidthTop = 0; _oPdfPCellTwo.BorderWidthBottom = 0; _oPdfPCellTwo.BackgroundColor = BaseColor.WHITE; oPdfPTablett.AddCell(_oPdfPCellTwo);

                    _oPdfPCellTwo = new PdfPCell(new Phrase((_oVoucher.BaseCurrencyID == oItem.CID ? oItem.CSymbol + " " + Global.MillionFormat(oItem.Amount) : oItem.CSymbol + " " + Global.MillionFormat(oItem.CAmount) + " @ " + Global.MillionFormatActualDigit(oItem.CRate) + " " + _oVoucher.BaseCUSymbol + " " + Global.MillionFormat(oItem.CAmount * oItem.CRate)) + " " + (oItem.IsDr ? "Dr" : "Cr"), _oFontStyle));
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
                else if (oItem.ObjType == EnumBreakdownType.ChequeReference || oItem.ObjType == EnumBreakdownType.SubledgerCheque)
                {
                    #region Cheque Reference
                    oPdfPTable = new PdfPTable(3);
                    oPdfPTable.WidthPercentage = 100;
                    oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.SetWidths(new float[] { 370f, 77.5f, 77.5f });

                    PdfPTable oPdfPTablett = new PdfPTable(5);
                    oPdfPTablett.WidthPercentage = 100;
                    oPdfPTablett.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTablett.SetWidths(new float[] { 10f, 113.33f, 113.33f, 113.33f, 10f });
                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

                    _oPdfPCellTwo = new PdfPCell(new Phrase(" ", _oFontStyle));
                    _oPdfPCellTwo.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCellTwo.BorderWidthLeft = 0; _oPdfPCellTwo.BorderWidthRight = 0; _oPdfPCellTwo.BorderWidthTop = 0; _oPdfPCellTwo.BorderWidthBottom = 0; _oPdfPCellTwo.BackgroundColor = BaseColor.WHITE; oPdfPTablett.AddCell(_oPdfPCellTwo);

                    _oPdfPCellTwo = new PdfPCell(new Phrase(oItem.DR_CR, _oFontStyle));
                    _oPdfPCellTwo.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCellTwo.BorderWidthLeft = 0; _oPdfPCellTwo.BorderWidthRight = 0; _oPdfPCellTwo.BorderWidthTop = 0; _oPdfPCellTwo.BorderWidthBottom = 0; _oPdfPCellTwo.BackgroundColor = BaseColor.WHITE; oPdfPTablett.AddCell(_oPdfPCellTwo);

                    if (oItem.ChequeDateStr.ToLower() == "date")
                    {
                        _oPdfPCellTwo = new PdfPCell(new Phrase("", _oFontStyle));
                    }
                    else
                    {
                        _oPdfPCellTwo = new PdfPCell(new Phrase("Date-" + oItem.ChequeDateStr, _oFontStyle));
                    }                    
                    _oPdfPCellTwo.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCellTwo.BorderWidthLeft = 0; _oPdfPCellTwo.BorderWidthRight = 0; _oPdfPCellTwo.BorderWidthTop = 0; _oPdfPCellTwo.BorderWidthBottom = 0; _oPdfPCellTwo.BackgroundColor = BaseColor.WHITE; oPdfPTablett.AddCell(_oPdfPCellTwo);

                    _oPdfPCellTwo = new PdfPCell(new Phrase(Global.MillionFormat(oItem.CAmount), _oFontStyle));
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
                else if (oItem.ObjType == EnumBreakdownType.OrderReference || oItem.ObjType == EnumBreakdownType.SLOrderReference)
                {
                    #region Order Reference
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

                    string sObjType = "Order Ref"; if (oItem.ObjType == EnumBreakdownType.SLOrderReference) { sObjType = "SLOrder ref "; }
                    _oPdfPCellTwo = new PdfPCell(new Phrase(sObjType + " : " + oItem.RefNo + "[" + oItem.OrderNo + "], " + oItem.ORemarks, _oFontStyle));
                    _oPdfPCellTwo.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCellTwo.BorderWidthLeft = 0; _oPdfPCellTwo.BorderWidthRight = 0; _oPdfPCellTwo.BorderWidthTop = 0; _oPdfPCellTwo.BorderWidthBottom = 0; _oPdfPCellTwo.BackgroundColor = BaseColor.WHITE; oPdfPTablett.AddCell(_oPdfPCellTwo);

                    _oPdfPCellTwo = new PdfPCell(new Phrase((_oVoucher.BaseCurrencyID == oItem.CID ? oItem.CSymbol + " " + Global.MillionFormat(oItem.Amount) : oItem.CSymbol + " " + Global.MillionFormat(oItem.CAmount) + " @ " + Global.MillionFormatActualDigit(oItem.CRate) + " " + _oVoucher.BaseCUSymbol + " " + Global.MillionFormat(oItem.CAmount * oItem.CRate)) + " " + (oItem.IsDr ? "Dr" : "Cr"), _oFontStyle));
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

            _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("In Word : " + Global.TakaWords(_nSumDebitAmount), _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            Chunk oChunk = new Chunk();
            Phrase oPhrase = new Phrase();            
            oChunk = new Chunk(_oVoucher.BaseCUSymbol + " " + Global.MillionFormat(_nSumDebitAmount), _oFontStyle);
            oChunk.SetUnderline(0.7f, -3f);
            oChunk.SetUnderline(0.7f, -5f);
            oPhrase.Add(oChunk);
            _oPdfPCell = new PdfPCell(oPhrase);            
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oChunk = new Chunk();
            oPhrase = new Phrase();
            oChunk = new Chunk(_oVoucher.BaseCUSymbol + " " + Global.MillionFormat(_nSumCreditAmount), _oFontStyle);
            oChunk.SetUnderline(0.7f, -3f);
            oChunk.SetUnderline(0.7f, -5f);
            oPhrase.Add(oChunk);
            _oPdfPCell = new PdfPCell(oPhrase);
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
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(ESimSolSignature.GetSignature(525f, (object)_oVoucher, _oSignatureSetups, 0.0f));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            #endregion
        }
        #endregion
    }
}
