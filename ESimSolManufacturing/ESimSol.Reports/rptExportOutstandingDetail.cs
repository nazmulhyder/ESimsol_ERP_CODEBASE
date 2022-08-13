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
using ESimSol.Reports;

namespace ESimSol.Reports
{
    public class rptExportOutstandingDetails
    {
        #region Declaration
        PdfWriter _oWriter;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyleBold;
        iTextSharp.text.Font _oFontStyleBoldUnderLine;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();

        List<ExportOutstandingDetail> _oExportOutstandingDetails = new List<ExportOutstandingDetail>();
        Company _oCompany = new Company();
        List<Currency> _oCurrencys = new List<Currency>();
        List<BankBranch> _oBankBranchs = new List<BankBranch>();
        BusinessUnit _oBusinessUnit= new BusinessUnit();
        int _nReportType = 0;
        int _nCount = 0;
        int _nTempBBranch_LegoID = 0;
        #endregion

        public byte[] PrepareReport(ExportOutstandingDetail oExportOutstandingDetail, int nReportType)
        {
            _nReportType = nReportType;

            _oCompany = oExportOutstandingDetail.Company;
            _oExportOutstandingDetails = oExportOutstandingDetail.ExportOutstandingDetails.OrderBy(c => c.BBranchID_Nego).ToList();

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(PageSize.A4.Rotate());

            _oDocument.SetMargins(40f, 40f, 20f, 40f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 842 });//height:842   width:595
            #endregion

            //this.PrintHeader();
            this.PrintBody();
            _oPdfPTable.HeaderRows = 6;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        #region Report Body
        private void PrintBody()
        {
            #region LC In Hand
            if (_nReportType == 1)
            {
                #region Variable Declaration
                int nCountDuplicate = 0;
                int nMaxCountDuplicate = 0;

                double nTotalLCValue = 0;
                double nTotalBillValue = 0;
                double nTotalLCInHandValue = 0;
                double nDeliveryValue = 0;

                double nGrandTotalLCValue = 0;
                double nGrandTotalBillValue = 0;
                double nGrandTotalLCInHandValue = 0;
                double nGrandDeliveryValue = 0;
                #endregion

                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
                PdfPTable oPdfPTable = new PdfPTable(8);
                oPdfPTable = new PdfPTable(8);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.SetWidths(new float[] { 35f, 200f, 200f, 100f, 100f, 100f, 100f, 100f });
                foreach (ExportOutstandingDetail oItem in _oExportOutstandingDetails)
                {
                    #region Title

                    if (_nTempBBranch_LegoID != oItem.BBranchID_Nego)
                    {
                        nMaxCountDuplicate = _oExportOutstandingDetails.Where(item => item.BBranchID_Nego == oItem.BBranchID_Nego).Count();

                        _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
                        _oPdfPCell = new PdfPCell(new Phrase(oItem.BankName_Nego, FontFactory.GetFont("Tahoma", 13f, iTextSharp.text.Font.BOLD)));
                        _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.Colspan = 8; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        oPdfPTable.CompleteRow();

                        _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("Party Name", _oFontStyle)); //ApplicantName
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("LC No", _oFontStyle)); //ExportLCNo
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("LC Value", _oFontStyle)); //Amount_LCSt
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("Total Bill Value", _oFontStyle)); //AmountSt
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("LC In Hand Value", _oFontStyle)); //LCinHandSt
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("Delivered Value", _oFontStyle)); //DeliveryValueSt
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("Current Status", _oFontStyle)); //LCStatusSt
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);
                        oPdfPTable.CompleteRow();
                    }
                    #endregion

                    #region Details
                    _nCount++;
                    nCountDuplicate++;
                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
                    _oPdfPCell = new PdfPCell(new Phrase(_nCount.ToString(), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ContractorName, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ExportLCNo, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.Amount_LCSt, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.AmountSt, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.LCinHandSt, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.DeliveryValueSt, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.LCStatusSt, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    oPdfPTable.CompleteRow();

                    nTotalLCValue += oItem.Amount_LC;
                    nTotalBillValue += oItem.Amount;
                    nTotalLCInHandValue = nTotalLCInHandValue + (oItem.Amount_LC - oItem.Amount);
                    nDeliveryValue += oItem.DeliveryValue;


                    #endregion

                    #region Total
                    if (nCountDuplicate == nMaxCountDuplicate)
                    {
                        nGrandTotalLCValue += nTotalLCValue;
                        nGrandTotalBillValue += nTotalBillValue;
                        nGrandTotalLCInHandValue += nTotalLCInHandValue;
                        nGrandDeliveryValue += nDeliveryValue;

                        _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
                        _oPdfPCell = new PdfPCell(new Phrase("Total : ", _oFontStyle));
                        _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalLCValue), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalBillValue), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalLCInHandValue), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nDeliveryValue), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);
                        oPdfPTable.CompleteRow();


                        _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        _oPdfPCell.Colspan = 8;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        _oPdfPCell.Border = 0;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPCell.FixedHeight = 15f;
                        oPdfPTable.AddCell(_oPdfPCell);
                        oPdfPTable.CompleteRow();

                        nMaxCountDuplicate = 0;
                        nCountDuplicate = 0;

                        _nCount = 0;
                        nTotalLCValue = 0;
                        nTotalBillValue = 0;
                        nTotalLCInHandValue = 0;
                        nDeliveryValue = 0;
                    }
                    #endregion

                    _nTempBBranch_LegoID = oItem.BBranchID_Nego;
                }
                #region Grand Total
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
                _oPdfPCell = new PdfPCell(new Phrase("Grand Total :", _oFontStyle));
                _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nGrandTotalLCValue), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nGrandTotalBillValue), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nGrandTotalLCInHandValue), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nGrandDeliveryValue), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); //LCStatusSt
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
                #endregion

                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion

            #region Pending Party Acceptance
            else if (_nReportType == 3)
            {
                #region Variable Declaration
                int nCountDuplicate = 0;
                int nMaxCountDuplicate = 0;

                double nTotalValue = 0;
                double nGrandTotalValue = 0;
                #endregion

                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
                PdfPTable oPdfPTable = new PdfPTable(11);
                oPdfPTable = new PdfPTable(11);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.SetWidths(new float[] { 35f, 200f, 200f, 200f, 100f, 200f, 100f, 150f, 80f, 150f, 150f });

                foreach (ExportOutstandingDetail oItem in _oExportOutstandingDetails)
                {
                    #region Title

                    if (_nTempBBranch_LegoID != oItem.BBranchID_Nego)
                    {
                        nMaxCountDuplicate = _oExportOutstandingDetails.Where(item => item.BBranchID_Nego == oItem.BBranchID_Nego).Count();

                        _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
                        _oPdfPCell = new PdfPCell(new Phrase(oItem.BankName_Nego, FontFactory.GetFont("Tahoma", 13f, iTextSharp.text.Font.BOLD)));
                        _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.Colspan = 11; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        oPdfPTable.CompleteRow();

                        _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("Bill No", _oFontStyle)); //ExportBillNo
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("Party Name", _oFontStyle)); //ApplicantName
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("LC No", _oFontStyle)); //ExportLCNo
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("PI No", _oFontStyle)); //PINo
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("Value", _oFontStyle)); //AmountSt
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("MKT Person", _oFontStyle)); //MKTPName
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("Send Party Date", _oFontStyle)); //SendToPartySt
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("Due Days", _oFontStyle)); //TimeLag3
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("Last Delivered Date", _oFontStyle)); //DeliveryDateSt
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("Due Date", _oFontStyle)); //???
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);
                        oPdfPTable.CompleteRow();
                    }
                    #endregion

                    #region Details
                    _nCount++;
                    nCountDuplicate++;
                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
                    _oPdfPCell = new PdfPCell(new Phrase(_nCount.ToString(), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ExportBillNo, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ContractorName, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ExportLCNo, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.PINo, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Amount), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.MKTPName, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.SendToPartySt, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.TimeLag3.ToString(), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.DeliveryDateSt, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); //oItem.Due Date
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    oPdfPTable.CompleteRow();

                    nTotalValue += oItem.Amount;

                    #endregion

                    #region Total
                    if (nCountDuplicate == nMaxCountDuplicate)
                    {
                        nGrandTotalValue += nTotalValue;

                        _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
                        _oPdfPCell = new PdfPCell(new Phrase("Total : ", _oFontStyle));
                        _oPdfPCell.Colspan = 5; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalValue), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); //oItem.Due Date
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); //oItem.Due Date
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); //oItem.Due Date
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); //oItem.Due Date
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); //oItem.Due Date
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);
                        oPdfPTable.CompleteRow();

                        _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        _oPdfPCell.Colspan = 11;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        _oPdfPCell.Border = 0;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPCell.FixedHeight = 15f;
                        oPdfPTable.AddCell(_oPdfPCell);
                        oPdfPTable.CompleteRow();

                        nMaxCountDuplicate = 0;
                        nCountDuplicate = 0;

                        _nCount = 0;
                        nTotalValue = 0;
                    }
                    #endregion

                    _nTempBBranch_LegoID = oItem.BBranchID_Nego;
                }
                #region Grand Total
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
                _oPdfPCell = new PdfPCell(new Phrase("Grand Total : ", _oFontStyle));
                _oPdfPCell.Colspan = 5; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nGrandTotalValue), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
                #endregion

                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion

            #region Pending Submit To Bank
            else if (_nReportType == 4)
            {
                #region Variable Declaration
                int nCountDuplicate = 0;
                int nMaxCountDuplicate = 0;

                double nTotalValue = 0;
                double nGrandTotalValue = 0;
                #endregion

                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
                PdfPTable oPdfPTable = new PdfPTable(11);
                oPdfPTable = new PdfPTable(11);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.SetWidths(new float[] { 35f, 200f, 200f, 200f, 100f, 200f, 200f, 150f, 150f, 180f, 80f });

                foreach (ExportOutstandingDetail oItem in _oExportOutstandingDetails)
                {
                    #region Title

                    if (_nTempBBranch_LegoID != oItem.BBranchID_Nego)
                    {
                        nMaxCountDuplicate = _oExportOutstandingDetails.Where(item => item.BBranchID_Nego == oItem.BBranchID_Nego).Count();

                        _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
                        _oPdfPCell = new PdfPCell(new Phrase(oItem.BankName_Nego, FontFactory.GetFont("Tahoma", 13f, iTextSharp.text.Font.BOLD)));
                        _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.Colspan = 11; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        oPdfPTable.CompleteRow();

                        _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("Bill No", _oFontStyle)); //ExportBillNo
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("Party Name", _oFontStyle)); //ApplicantName
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("LC No", _oFontStyle)); //ExportLCNo
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("PI No", _oFontStyle)); //PINo
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("Value", _oFontStyle)); //AmountSt
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("Issue Bank", _oFontStyle)); //BankName_Issue
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("Prepare Date", _oFontStyle)); //Prepare Date ???
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("Send Party Date", _oFontStyle)); //SendToPartySt
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("Receive from party", _oFontStyle)); //RecdFromPartySt
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("Due Days", _oFontStyle)); //TimeLag3
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        oPdfPTable.CompleteRow();
                    }
                    #endregion

                    #region Details
                    _nCount++;
                    nCountDuplicate++;
                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
                    _oPdfPCell = new PdfPCell(new Phrase(_nCount.ToString(), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ExportBillNo, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ContractorName, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ExportLCNo, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.PINo, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Amount), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.BankName_Issue, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.SendToPartySt, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("".ToString(), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.TimeLag3.ToString(), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    oPdfPTable.CompleteRow();

                    nTotalValue += oItem.Amount;
                    #endregion

                    #region Total
                    if (nCountDuplicate == nMaxCountDuplicate)
                    {
                        nGrandTotalValue += nTotalValue;

                        _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
                        _oPdfPCell = new PdfPCell(new Phrase("Total : ", _oFontStyle));
                        _oPdfPCell.Colspan = 5; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalValue), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);
                        oPdfPTable.CompleteRow();

                        _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        _oPdfPCell.Colspan = 11;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        _oPdfPCell.Border = 0;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPCell.FixedHeight = 15f;
                        oPdfPTable.AddCell(_oPdfPCell);
                        oPdfPTable.CompleteRow();

                        nMaxCountDuplicate = 0;
                        nCountDuplicate = 0;

                        _nCount = 0;
                        nTotalValue = 0;
                    }
                    #endregion

                    _nTempBBranch_LegoID = oItem.BBranchID_Nego;
                }

                #region Grand Total
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
                _oPdfPCell = new PdfPCell(new Phrase("Grand Total : ", _oFontStyle));
                _oPdfPCell.Colspan = 5; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nGrandTotalValue), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
                #endregion

                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion

            #region Pending LDBC
            else if (_nReportType == 5)
            {
                #region Variable Declaration
                int nCountDuplicate = 0;
                int nMaxCountDuplicate = 0;

                double nTotalValue = 0;
                double nGrandTotalValue = 0;
                #endregion

                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
                PdfPTable oPdfPTable = new PdfPTable(12);
                oPdfPTable = new PdfPTable(12);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.SetWidths(new float[] { 35f, 200f, 180f, 200f, 100f, 200f, 200f, 150f, 150f, 180f, 150f, 100f });

                foreach (ExportOutstandingDetail oItem in _oExportOutstandingDetails)
                {
                    #region Title

                    if (_nTempBBranch_LegoID != oItem.BBranchID_Nego)
                    {
                        nMaxCountDuplicate = _oExportOutstandingDetails.Where(item => item.BBranchID_Nego == oItem.BBranchID_Nego).Count();

                        _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
                        _oPdfPCell = new PdfPCell(new Phrase(oItem.BankName_Nego, FontFactory.GetFont("Tahoma", 13f, iTextSharp.text.Font.BOLD)));
                        _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.Colspan = 12; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        oPdfPTable.CompleteRow();

                        _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("Bill No", _oFontStyle)); //ExportBillNo
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("Party Name", _oFontStyle)); //ApplicantName
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("LC No", _oFontStyle)); //ExportLCNo
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("PI No", _oFontStyle)); //PINo
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("Value", _oFontStyle)); //AmountSt
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("Issue Bank", _oFontStyle)); //BankName_Issue
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("Prepare Date", _oFontStyle)); //Prepare Date ???
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("Send Party Date", _oFontStyle)); //SendToPartySt
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("Receive from party", _oFontStyle)); //RecdFromPartySt
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("Send To Bank", _oFontStyle)); //SendToBankDateSt
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("Due Days", _oFontStyle)); //TimeLag3
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        oPdfPTable.CompleteRow();
                    }
                    #endregion

                    #region Details
                    _nCount++;
                    nCountDuplicate++;
                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
                    _oPdfPCell = new PdfPCell(new Phrase(_nCount.ToString(), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ExportBillNo, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ContractorName, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ExportLCNo, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.PINo, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Amount), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.BankName_Issue, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.SendToPartySt, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.RecdFromPartySt, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.SendToBankDateSt, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.TimeLag3.ToString(), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    oPdfPTable.CompleteRow();

                    nTotalValue += oItem.Amount;
                    #endregion

                    #region Total
                    if (nCountDuplicate == nMaxCountDuplicate)
                    {
                        nGrandTotalValue += nTotalValue;

                        _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
                        _oPdfPCell = new PdfPCell(new Phrase("Total : ", _oFontStyle));
                        _oPdfPCell.Colspan = 5; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalValue), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);
                        oPdfPTable.CompleteRow();

                        _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        _oPdfPCell.Colspan = 12;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        _oPdfPCell.Border = 0;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPCell.FixedHeight = 15f;
                        oPdfPTable.AddCell(_oPdfPCell);
                        oPdfPTable.CompleteRow();

                        nMaxCountDuplicate = 0;
                        nCountDuplicate = 0;

                        _nCount = 0;
                        nTotalValue = 0;
                    }
                    #endregion

                    _nTempBBranch_LegoID = oItem.BBranchID_Nego;
                }
                #region Grand Total
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
                _oPdfPCell = new PdfPCell(new Phrase("Grand Total : ", _oFontStyle));
                _oPdfPCell.Colspan = 5; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nGrandTotalValue), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
                #endregion

                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion

            #region Pending Maturity
            else if (_nReportType == 6)
            {
                #region Variable Declaration
                int nCountDuplicate = 0;
                int nMaxCountDuplicate = 0;

                double nTotalValue = 0;
                double nGrandTotalValue = 0;
                #endregion

                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
                PdfPTable oPdfPTable = new PdfPTable(13);
                oPdfPTable = new PdfPTable(13);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.SetWidths(new float[] { 65f, 200f, 180f, 200f, 100f, 200f, 250f, 100f, 150f, 200f, 150f, 150f, 150f });

                foreach (ExportOutstandingDetail oItem in _oExportOutstandingDetails)
                {
                    #region Title

                    if (_nTempBBranch_LegoID != oItem.BBranchID_Nego)
                    {
                        nMaxCountDuplicate = _oExportOutstandingDetails.Where(item => item.BBranchID_Nego == oItem.BBranchID_Nego).Count();

                        _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
                        _oPdfPCell = new PdfPCell(new Phrase(oItem.BankName_Nego, FontFactory.GetFont("Tahoma", 13f, iTextSharp.text.Font.BOLD)));
                        _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.Colspan = 16; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        oPdfPTable.CompleteRow();

                        _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("Bill No", _oFontStyle)); //ExportBillNo
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("Party Name", _oFontStyle)); //ApplicantName
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("LC No", _oFontStyle)); //ExportLCNo
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("PI No", _oFontStyle)); //PINo
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("Value", _oFontStyle)); //AmountSt
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("Issue Bank", _oFontStyle)); //BankName_Issue
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("LDBC No", _oFontStyle)); //LDBCNo
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("LDBC Date", _oFontStyle)); //LDBCDateSt
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        //_oPdfPCell = new PdfPCell(new Phrase("Maturity Rec Date", _oFontStyle)); //???
                        //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        //_oPdfPCell = new PdfPCell(new Phrase("Maturity Date", _oFontStyle)); //???
                        //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("O.Rate", _oFontStyle)); //???
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("O.Due", _oFontStyle)); //???
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("Last Del. Date", _oFontStyle)); //DeliveryDateSt
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("Due Days", _oFontStyle)); //TimeLag3
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);
                        oPdfPTable.CompleteRow();
                    }
                    #endregion

                    #region Details
                    _nCount++;
                    nCountDuplicate++;
                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
                    _oPdfPCell = new PdfPCell(new Phrase(_nCount.ToString(), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ExportBillNo, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ContractorName, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ExportLCNo, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.PINo, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Amount), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.BankName_Issue, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.LDBCNo, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.LDBCDateSt, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.DeliveryDateSt, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.TimeLag3.ToString(), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    oPdfPTable.CompleteRow();

                    nTotalValue += oItem.Amount;
                    #endregion

                    #region Total
                    if (nCountDuplicate == nMaxCountDuplicate)
                    {
                        nGrandTotalValue += nTotalValue;

                        _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
                        _oPdfPCell = new PdfPCell(new Phrase("Total : ", _oFontStyle));
                        _oPdfPCell.Colspan = 5; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalValue), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        oPdfPTable.CompleteRow();

                        _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        _oPdfPCell.Colspan = 15;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        _oPdfPCell.Border = 0;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPCell.FixedHeight = 15f;
                        oPdfPTable.AddCell(_oPdfPCell);
                        oPdfPTable.CompleteRow();

                        nMaxCountDuplicate = 0;
                        nCountDuplicate = 0;

                        _nCount = 0;
                        nTotalValue = 0;
                    }
                    #endregion

                    _nTempBBranch_LegoID = oItem.BBranchID_Nego;
                }
                #region Grand Total
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
                _oPdfPCell = new PdfPCell(new Phrase("Grand Total : ", _oFontStyle));
                _oPdfPCell.Colspan = 5; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nGrandTotalValue), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
                #endregion


                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion

            #region Over due Maturity
            else if (_nReportType == 7)
            {
                #region Variable Declaration
                int nCountDuplicate = 0;
                int nMaxCountDuplicate = 0;

                double nTotalValue = 0;
                double nGrandTotalValue = 0;
                #endregion

                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
                PdfPTable oPdfPTable = new PdfPTable(16);
                oPdfPTable = new PdfPTable(16);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.SetWidths(new float[] { 65f, 200f, 180f, 200f, 100f, 200f, 250f, 100f, 150f, 150f, 200f, 200f, 150f, 150f, 150f, 150f });

                foreach (ExportOutstandingDetail oItem in _oExportOutstandingDetails)
                {
                    #region Title

                    if (_nTempBBranch_LegoID != oItem.BBranchID_Nego)
                    {
                        nMaxCountDuplicate = _oExportOutstandingDetails.Where(item => item.BBranchID_Nego == oItem.BBranchID_Nego).Count();

                        _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
                        _oPdfPCell = new PdfPCell(new Phrase(oItem.BankName_Nego, FontFactory.GetFont("Tahoma", 13f, iTextSharp.text.Font.BOLD)));
                        _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.Colspan = 16; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        oPdfPTable.CompleteRow();

                        _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("Bill No", _oFontStyle)); //ExportBillNo
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("Party Name", _oFontStyle)); //ApplicantName
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("LC No", _oFontStyle)); //ExportLCNo
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("PI No", _oFontStyle)); //PINo
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("Value", _oFontStyle)); //AmountSt
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("Issue Bank", _oFontStyle)); //BankName_Issue
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("Status", _oFontStyle)); //??
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("LDBC No", _oFontStyle)); //LDBCNo
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("LDBC Date", _oFontStyle)); //LDBCDateSt
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("Maturity Rec Date", _oFontStyle)); //???
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("Maturity Date", _oFontStyle)); //???
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("O.Rate", _oFontStyle)); //???
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("O.Due", _oFontStyle)); //???
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("Last Del. Date", _oFontStyle)); //DeliveryDateSt
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("Due Days", _oFontStyle)); //TimeLag3
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);
                        oPdfPTable.CompleteRow();
                    }
                    #endregion

                    #region Details
                    _nCount++;
                    nCountDuplicate++;
                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
                    _oPdfPCell = new PdfPCell(new Phrase(_nCount.ToString(), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ExportBillNo, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ContractorName, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ExportLCNo, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.PINo, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Amount), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.BankName_Issue, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));//Status
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.LDBCNo, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.LDBCDateSt, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); //Maturity Rec Date
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); //Maturity Date
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); //O.Rate
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); //O.Due
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.DeliveryDateSt, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.TimeLag3.ToString(), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    oPdfPTable.CompleteRow();

                    nTotalValue += oItem.Amount;
                    #endregion

                    #region Total
                    if (nCountDuplicate == nMaxCountDuplicate)
                    {
                        nGrandTotalValue += nTotalValue;

                        _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
                        _oPdfPCell = new PdfPCell(new Phrase("Total : ", _oFontStyle));
                        _oPdfPCell.Colspan = 5; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalValue), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);
                        oPdfPTable.CompleteRow();

                        _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        _oPdfPCell.Colspan = 16;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        _oPdfPCell.Border = 0;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPCell.FixedHeight = 15f;
                        oPdfPTable.AddCell(_oPdfPCell);
                        oPdfPTable.CompleteRow();

                        nMaxCountDuplicate = 0;
                        nCountDuplicate = 0;

                        _nCount = 0;
                        nTotalValue = 0;
                    }
                    #endregion

                    _nTempBBranch_LegoID = oItem.BBranchID_Nego;
                }

                #region Grand Total
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
                _oPdfPCell = new PdfPCell(new Phrase("Grand Total : ", _oFontStyle));
                _oPdfPCell.Colspan = 5; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nGrandTotalValue), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
                #endregion

                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion

            #region Pending Payment
            else if (_nReportType == 8)
            {
                #region Variable Declaration
                int nCountDuplicate = 0;
                int nMaxCountDuplicate = 0;

                double nTotalValue = 0;
                double nGrandTotalValue = 0;
                #endregion

                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
                PdfPTable oPdfPTable = new PdfPTable(16);
                oPdfPTable = new PdfPTable(16);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.SetWidths(new float[] { 65f, 200f, 180f, 200f, 100f, 200f, 250f, 100f, 150f, 150f, 200f, 200f, 150f, 150f, 150f, 150f });

                foreach (ExportOutstandingDetail oItem in _oExportOutstandingDetails)
                {
                    #region Title

                    if (_nTempBBranch_LegoID != oItem.BBranchID_Nego)
                    {
                        nMaxCountDuplicate = _oExportOutstandingDetails.Where(item => item.BBranchID_Nego == oItem.BBranchID_Nego).Count();

                        _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
                        _oPdfPCell = new PdfPCell(new Phrase(oItem.BankName_Nego, FontFactory.GetFont("Tahoma", 13f, iTextSharp.text.Font.BOLD)));
                        _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.Colspan = 16; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        oPdfPTable.CompleteRow();

                        _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("Bill No", _oFontStyle)); //ExportBillNo
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("Party Name", _oFontStyle)); //ApplicantName
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("LC No", _oFontStyle)); //ExportLCNo
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("PI No", _oFontStyle)); //PINo
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("Value", _oFontStyle)); //AmountSt
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("Issue Bank", _oFontStyle)); //BankName_Issue
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("Status", _oFontStyle)); //??
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("LDBC No", _oFontStyle)); //LDBCNo
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("LDBC Date", _oFontStyle)); //LDBCDateSt
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("Maturity Rec Date", _oFontStyle)); //???
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("Maturity Date", _oFontStyle)); //???
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("O.Rate", _oFontStyle)); //???
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("O.Due", _oFontStyle)); //???
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("Last Del. Date", _oFontStyle)); //DeliveryDateSt
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("Due Days", _oFontStyle)); //TimeLag3
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);
                        oPdfPTable.CompleteRow();
                    }
                    #endregion

                    #region Details
                    _nCount++;
                    nCountDuplicate++;
                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
                    _oPdfPCell = new PdfPCell(new Phrase(_nCount.ToString(), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ExportBillNo, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ContractorName, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ExportLCNo, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.PINo, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Amount), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.BankName_Issue, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.LDBCNo, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); //Status
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.LDBCDateSt, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); //Maturity Rec Date
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); //Maturity Date
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); //O.Rate
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); //O.Due
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.DeliveryDateSt, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.TimeLag3.ToString(), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    oPdfPTable.CompleteRow();

                    nTotalValue += oItem.Amount;
                    #endregion

                    #region Total
                    if (nCountDuplicate == nMaxCountDuplicate)
                    {
                        nGrandTotalValue += nTotalValue;

                        _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
                        _oPdfPCell = new PdfPCell(new Phrase("Total : ", _oFontStyle));
                        _oPdfPCell.Colspan = 5; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalValue), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);
                        oPdfPTable.CompleteRow();

                        _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        _oPdfPCell.Colspan = 16;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        _oPdfPCell.Border = 0;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPCell.FixedHeight = 15f;
                        oPdfPTable.AddCell(_oPdfPCell);
                        oPdfPTable.CompleteRow();

                        nMaxCountDuplicate = 0;
                        nCountDuplicate = 0;

                        _nCount = 0;
                        nTotalValue = 0;
                    }
                    #endregion

                    _nTempBBranch_LegoID = oItem.BBranchID_Nego;
                }

                #region Grand Total
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
                _oPdfPCell = new PdfPCell(new Phrase("Grand Total : ", _oFontStyle));
                _oPdfPCell.Colspan = 5; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nGrandTotalValue), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
                #endregion

                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion
        }
        #endregion
        public byte[] PrepareReportMgt(List<ExportOutstandingDetail> oExportOutstandingDetails, int nReportType, Company oCompany, EnumReportLayout eReportLayout, string sReportHead, string sDateRange, BusinessUnit oBusinessUnit)
        {
            _nReportType = nReportType;
            _oExportOutstandingDetails = oExportOutstandingDetails;
            _oCompany = oCompany;
            _oBusinessUnit = oBusinessUnit;
            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate()); //595X842
            _oDocument.SetMargins(20f, 20f, 5f, 20f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _oWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PageEventHandler.PrintDocumentGenerator = true;
            PageEventHandler.PrintPrintingDateTime = true;
            _oWriter.PageEvent = PageEventHandler; //Footer print wiht page event handeler            
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 802 });
            #endregion
            #region Report Body & Header
            this.PrintHeader();
            ///AdvanceDelivery
            ///
            if (nReportType == (int)EnumOperationStage.AdvanceDelivery)
            {
                if (eReportLayout == EnumReportLayout.PartyWise)
                {
                    this.ReporttHeader(sReportHead, sDateRange);
                    this.PrintBody_AdvanceDelivery_PartyWise();
                }
                else if (eReportLayout == EnumReportLayout.BankWise)
                {
                    this.ReporttHeader(sReportHead, sDateRange);
                    this.PrintBody_AdvanceDelivery_BankWise();
                }
            }

            ///AdvanceDelivery
            if (nReportType == (int)EnumOperationStage.LCInHand_Delivery_Done )
            {
                if (eReportLayout == EnumReportLayout.PartyWise)
                {
                    this.ReporttHeader(sReportHead, sDateRange);
                    this.PrintBody_LCInHand_PendingDelivery_PartyWise();
                }
                else if (eReportLayout == EnumReportLayout.BankWise)
                {
                    this.ReporttHeader(sReportHead, sDateRange);
                    this.PrintBody_LCInHand_PendingDelivery_BankWise();
                }
            }

            ///BO In Hand
            if (nReportType == (int)EnumOperationStage.BOINHand)
            {
                if (eReportLayout == EnumReportLayout.PartyWise)
                {
                    this.ReporttHeader(sReportHead, sDateRange);
                    this.PrintBody_BOInHand_PartyWise();
                }
                else if (eReportLayout == EnumReportLayout.BankWise)
                {
                    this.ReporttHeader(sReportHead, sDateRange);
                    this.PrintBody_BOInHand_BankWise();
                }
            }
            //BO In Hand Customer Hand ==5-BillState=1
            if (nReportType == (int)EnumOperationStage.BOINCUS_Hand)
            {
                if (eReportLayout == EnumReportLayout.PartyWise)
                {
                    this.ReporttHeader(sReportHead, sDateRange);
                    this.PrintBody_BOInHandCustomerHand_PartyWise();
                }
                else if (eReportLayout == EnumReportLayout.BankWise)
                {
                    this.ReporttHeader(sReportHead, sDateRange);
                    this.PrintBody_BOInHandCustomerHand_BankWise();
                }
            }
            //-Party Acceptance Bill ==6--Bill State=2
            if (nReportType == (int)EnumOperationStage.PendingBankSubmition)
            {
                if (eReportLayout == EnumReportLayout.PartyWise)
                {
                    this.ReporttHeader(sReportHead, sDateRange);
                    this.PrintBody_PartyAcceptanceBill_PartyWise();
                }
                else if (eReportLayout == EnumReportLayout.BankWise)
                {
                    this.ReporttHeader(sReportHead, sDateRange);
                    this.PrintBody_PartyAcceptanceBill_BankWise();
                }
            }
            //-Panding Bank Acceptance( Nego_Transit = 3,  NegotiatedBill = 4,)==7--Bill State=3,4
            if (nReportType == (int)EnumOperationStage.PendingBankAcceptance)
            {
                if (eReportLayout == EnumReportLayout.PartyWise)
                {
                    this.ReporttHeader(sReportHead, sDateRange);
                    this.PrintBody_PendingBankAcceptance_PartyWise();
                }
                else if (eReportLayout == EnumReportLayout.BankWise)
                {
                    this.ReporttHeader(sReportHead, sDateRange);
                    this.PrintBody_PendingBankAcceptance_BankWise();
                }
            }
            if (nReportType == (int)EnumOperationStage.OverduePayment)
            {
                if (eReportLayout == EnumReportLayout.PartyWise)
                {
                    this.ReporttHeader(sReportHead, sDateRange);
                    this.PrintBody_OverduePayment_PartyWise();
                }
                else if (eReportLayout == EnumReportLayout.BankWise)
                {
                    this.ReporttHeader(sReportHead, sDateRange);
                    this.PrintBody_OverduePayment_BankWise();
                }
            }
            if (nReportType == (int)EnumOperationStage.OnlyDuePayment)
            {
                if (eReportLayout == EnumReportLayout.PartyWise)
                {
                    this.ReporttHeader(sReportHead, sDateRange);
                    this.PrintBody_DuePayment_PartyWise();
                }
                else if (eReportLayout == EnumReportLayout.BankWise)
                {
                    this.ReporttHeader(sReportHead, sDateRange);
                    this.PrintBody_DuePayment_BankWise();
                }
            }
            #endregion

            _oPdfPTable.HeaderRows = 3;
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
            oPdfPTable.SetWidths(new float[] { 80f, 300.5f, 80f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            if (_oCompany.CompanyLogo != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(80f, 55f);
                _oPdfPCell = new PdfPCell(_oImag);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            }
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.Rowspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oFontStyle = FontFactory.GetFont("Tahoma", 18f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Name, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));

            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.PringReportHead, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            #region ReportHeader
            #region Blank Space
            _oFontStyle = FontFactory.GetFont("Tahoma", 5f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion
            #endregion

            #endregion
        }
        private void ReporttHeader(string sReportHead, string sDateRange)
        {

            #region Proforma Invoice Heading Print

            _oPdfPCell = new PdfPCell(new Phrase(sReportHead, FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD)));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(sDateRange, FontFactory.GetFont("Tahoma", 11f, iTextSharp.text.Font.NORMAL)));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();


            #endregion
        }
        #endregion
        
        #region  4Body BOInHand_PartyWise
        private void PrintBody_BOInHand_PartyWise()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
            _oFontStyleBoldUnderLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
            _oExportOutstandingDetails = _oExportOutstandingDetails.OrderBy(c => c.ContractorName).ToList();
            #region Initialize Table
            PdfPTable oPdfPTable = new PdfPTable(12);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                    30f,  //SL 
                                                    60f,  //Bill No
                                                    140f,  //Bank/Party Name
                                                    70f, //L/C No
                                                    50f,  //L/C Date                                                    
                                                    90f,  //PI No                                                   
                                                    50f,  //Value
                                                    90f,  //Concern Person 
                                                    55f,  //Last Delivery Date                                                    
                                                    30f,  //Due Days
                                                    50f,  //Bill Date
                                                    30f,  //Due Days
                                             });
            #endregion

            string sContractorName = ""; int nCount = 0; string sCurrencySymbol = "";
            double nTotalAmount = 0, nGrandTotalAmount = 0;
            foreach (ExportOutstandingDetail oItem in _oExportOutstandingDetails)
            {
                sCurrencySymbol = oItem.Currency;
                if (sContractorName != oItem.ContractorName)
                {
                    #region Supplier Wise Sub Total
                    if (oPdfPTable.Rows.Count > 0)
                    {
                        #region Initialize Table
                        oPdfPTable = new PdfPTable(12);
                        oPdfPTable.WidthPercentage = 100;
                        oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPTable.SetWidths(new float[] { 
                                                    30f,  //SL 
                                                    60f,  //Bill No
                                                    140f,  //Bank/Party Name
                                                    70f, //L/C No
                                                    50f,  //L/C Date                                                    
                                                    90f,  //PI No                                                   
                                                    50f,  //Value
                                                    90f,  //Concern Person 
                                                    55f,  //Last Delivery Date                                                    
                                                    30f,  //Due Days
                                                    50f,  //Bill Date
                                                    30f,  //Due Days
                                             });
                        #endregion

                        #region  Sub Total
                        _oPdfPCell = new PdfPCell(new Phrase("Sub Total :", _oFontStyleBold));
                        _oPdfPCell.Colspan = 10; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nTotalAmount), _oFontStyleBold));
                        _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        oPdfPTable.CompleteRow();
                        #endregion

                        #region Insert Into Main Table
                        _oPdfPCell = new PdfPCell(oPdfPTable);
                        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();
                        #endregion

                        nTotalAmount = 0; nCount = 0;
                    }
                    #endregion

                    #region Header
                    #region Blank Row
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion

                    #region Table Initialize
                    oPdfPTable = new PdfPTable(12);
                    oPdfPTable.WidthPercentage = 100;
                    oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.SetWidths(new float[] { 
                                                   30f,  //SL 
                                                    60f,  //Bill No
                                                    140f,  //Bank/Party Name
                                                    70f, //L/C No
                                                    50f,  //L/C Date                                                    
                                                    90f,  //PI No                                                   
                                                    50f,  //Value
                                                    90f,  //Concern Person 
                                                    55f,  //Last Delivery Date                                                    
                                                    30f,  //Due Days
                                                    50f,  //Bill Date
                                                    30f,  //Due Days
                                             });
                    #endregion

                    #region Supplier Heading
                    _oPdfPCell = new PdfPCell(new Phrase("Party Name : " + oItem.ContractorName, _oFontStyleBoldUnderLine));
                    _oPdfPCell.Colspan = 12; _oPdfPCell.FixedHeight = 14f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    oPdfPTable.CompleteRow();
                    #endregion

                    #region Header Row
                    _oPdfPCell = new PdfPCell(new Phrase("SL No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Bill No ", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Bank Name", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("L/C No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("L/C Date", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("PI No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);


                    _oPdfPCell = new PdfPCell(new Phrase("Value", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Concern Person", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Last Delivery", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Due", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Bill Date", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Due", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    oPdfPTable.CompleteRow();
                    #endregion

                    #region Insert Into Main Table
                    _oPdfPCell = new PdfPCell(oPdfPTable);
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion
                    #endregion
                }


                #region Initialize Table
                oPdfPTable = new PdfPTable(12);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.SetWidths(new float[] { 
                                                    30f,  //SL 
                                                    60f,  //Bill No
                                                    140f,  //Bank/Party Name
                                                    70f, //L/C No
                                                    50f,  //L/C Date                                                    
                                                    90f,  //PI No                                                   
                                                    50f,  //Value
                                                    90f,  //Concern Person 
                                                    55f,  //Last Delivery Date                                                    
                                                    30f,  //Due Days
                                                    50f,  //Bill Date
                                                    30f,  //Due Days
                                             });
                #endregion

                nCount++;
                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ExportBillNo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.BankName_Nego, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ExportLCNo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.LCOpeningDatest, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.PINo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.AmountSt, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.MKTPName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.DeliveryDateSt, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                if (oItem.DeliveryDate == DateTime.MinValue)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(("").ToString(), _oFontStyle));
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase((Global.DateDiff("D", oItem.DeliveryDate, DateTime.Today)).ToString(), _oFontStyle));
                }
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.StartDateSt, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((Global.DateDiff("D", oItem.StartDate, DateTime.Today)).ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();

                #region Insert Into Main Table
                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                sContractorName = oItem.ContractorName;
                sCurrencySymbol = oItem.Currency;
                nTotalAmount = nTotalAmount + oItem.Amount;
                nGrandTotalAmount = nGrandTotalAmount + oItem.Amount;
            }

            #region Initialize Table
            oPdfPTable = new PdfPTable(12);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                    30f,  //SL 
                                                    60f,  //Bill No
                                                    140f,  //Bank/Party Name
                                                    70f, //L/C No
                                                    50f,  //L/C Date                                                    
                                                    90f,  //PI No                                                   
                                                    50f,  //Value
                                                    90f,  //Concern Person 
                                                    55f,  //Last Delivery Date                                                    
                                                    30f,  //Due Days
                                                    50f,  //Bill Date
                                                    30f,  //Due Days
                                             });
            #endregion

            #region Date Wise Sub Total
            _oPdfPCell = new PdfPCell(new Phrase("Sub Total :", _oFontStyleBold));
            _oPdfPCell.Colspan = 10; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nTotalAmount), _oFontStyleBold));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Grand Total
            _oPdfPCell = new PdfPCell(new Phrase("Grand Total :", _oFontStyleBold));
            _oPdfPCell.Colspan = 10; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nGrandTotalAmount), _oFontStyleBold));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        #endregion
        #region  4Body BOInHand_Bank Wise
        private void PrintBody_BOInHand_BankWise()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
            _oFontStyleBoldUnderLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
            _oExportOutstandingDetails = _oExportOutstandingDetails.OrderBy(c => c.BankName_Nego).ToList();
            #region Initialize Table
            PdfPTable oPdfPTable = new PdfPTable(12);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                    30f,  //SL 
                                                    60f,  //Bill No
                                                    140f,  //Bank/Party Name
                                                    70f, //L/C No
                                                    50f,  //L/C Date                                                    
                                                    90f,  //PI No                                                   
                                                    50f,  //Value
                                                    90f,  //Concern Person 
                                                    55f,  //Last Delivery Date                                                    
                                                    30f,  //Due Days
                                                    50f,  //Bill Date
                                                    30f,  //Due Days
                                             });
            #endregion

            string sBankName_Nego = ""; int nCount = 0; string sCurrencySymbol = "";
            double nTotalAmount = 0, nGrandTotalAmount = 0;
            foreach (ExportOutstandingDetail oItem in _oExportOutstandingDetails)
            {
                if (sBankName_Nego != oItem.BankName_Nego)
                {
                    #region Supplier Wise Sub Total
                    if (oPdfPTable.Rows.Count > 0)
                    {
                        #region Initialize Table
                        oPdfPTable = new PdfPTable(12);
                        oPdfPTable.WidthPercentage = 100;
                        oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPTable.SetWidths(new float[] { 
                                                     30f,  //SL 
                                                    60f,  //Bill No
                                                    140f,  //Bank/Party Name
                                                    70f, //L/C No
                                                    50f,  //L/C Date                                                    
                                                    90f,  //PI No                                                   
                                                    50f,  //Value
                                                    90f,  //Concern Person 
                                                    55f,  //Last Delivery Date                                                    
                                                    30f,  //Due Days
                                                    50f,  //Bill Date
                                                    30f,  //Due Days
                                             });
                        #endregion

                        #region  Sub Total
                        _oPdfPCell = new PdfPCell(new Phrase("Sub Total :", _oFontStyleBold));
                        _oPdfPCell.Colspan = 10; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nTotalAmount), _oFontStyleBold));
                        _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        oPdfPTable.CompleteRow();
                        #endregion

                        #region Insert Into Main Table
                        _oPdfPCell = new PdfPCell(oPdfPTable);
                        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();
                        #endregion

                        nTotalAmount = 0; nCount = 0;
                    }
                    #endregion

                    #region Header
                    #region Blank Row
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion

                    #region Table Initialize
                    oPdfPTable = new PdfPTable(12);
                    oPdfPTable.WidthPercentage = 100;
                    oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.SetWidths(new float[] { 
                                                    30f,  //SL 
                                                    60f,  //Bill No
                                                    140f,  //Bank/Party Name
                                                    70f, //L/C No
                                                    50f,  //L/C Date                                                    
                                                    90f,  //PI No                                                   
                                                    50f,  //Value
                                                    90f,  //Concern Person 
                                                    55f,  //Last Delivery Date                                                    
                                                    30f,  //Due Days
                                                    50f,  //Bill Date
                                                    30f,  //Due Days
                                             });
                    #endregion

                    #region Supplier Heading
                    _oPdfPCell = new PdfPCell(new Phrase("Bank Name : " + oItem.BankName_Nego, _oFontStyleBoldUnderLine));
                    _oPdfPCell.Colspan = 12; _oPdfPCell.FixedHeight = 14f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    oPdfPTable.CompleteRow();
                    #endregion

                    #region Header Row
                    _oPdfPCell = new PdfPCell(new Phrase("SL No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Bill No ", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Buyer Name", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("L/C No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("L/C Date", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("PI No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);


                    _oPdfPCell = new PdfPCell(new Phrase("Value", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Concern Person", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Last Delivery", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Due", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Bill Date", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Due", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    oPdfPTable.CompleteRow();
                    #endregion

                    #region Insert Into Main Table
                    _oPdfPCell = new PdfPCell(oPdfPTable);
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion
                    #endregion
                }


                #region Initialize Table
                oPdfPTable = new PdfPTable(12);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.SetWidths(new float[] { 
                                                    30f,  //SL 
                                                    60f,  //Bill No
                                                    140f,  //Bank/Party Name
                                                    70f, //L/C No
                                                    50f,  //L/C Date                                                    
                                                    90f,  //PI No                                                   
                                                    50f,  //Value
                                                    90f,  //Concern Person 
                                                    55f,  //Last Delivery Date                                                    
                                                    30f,  //Due Days
                                                    50f,  //Bill Date
                                                    30f,  //Due Days
                                             });
                #endregion

                nCount++;
                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ExportBillNo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ContractorName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ExportLCNo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.LCOpeningDatest, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.PINo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.AmountSt, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.MKTPName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.DeliveryDateSt, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                if (oItem.DeliveryDate == DateTime.MinValue)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(("").ToString(), _oFontStyle));
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase((Global.DateDiff("D", oItem.DeliveryDate, DateTime.Today)).ToString(), _oFontStyle));
                }
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.StartDateSt, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((Global.DateDiff("D", oItem.StartDate, DateTime.Today)).ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();

                #region Insert Into Main Table
                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                sBankName_Nego = oItem.BankName_Nego;
                sCurrencySymbol = oItem.Currency;
                nTotalAmount = nTotalAmount + oItem.Amount;
                nGrandTotalAmount = nGrandTotalAmount + oItem.Amount;
            }

            #region Initialize Table
            oPdfPTable = new PdfPTable(12);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                   30f,  //SL 
                                                    60f,  //Bill No
                                                    140f,  //Bank/Party Name
                                                    70f, //L/C No
                                                    50f,  //L/C Date                                                    
                                                    90f,  //PI No                                                   
                                                    50f,  //Value
                                                    90f,  //Concern Person 
                                                    55f,  //Last Delivery Date                                                    
                                                    30f,  //Due Days
                                                    50f,  //Bill Date
                                                    30f,  //Due Days
                                             });
            #endregion

            #region Date Wise Sub Total
            _oPdfPCell = new PdfPCell(new Phrase("Sub Total :", _oFontStyleBold));
            _oPdfPCell.Colspan = 10; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nTotalAmount), _oFontStyleBold));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Grand Total
            _oPdfPCell = new PdfPCell(new Phrase("Grand Total :", _oFontStyleBold));
            _oPdfPCell.Colspan = 10; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nGrandTotalAmount), _oFontStyleBold));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        #endregion

        #region  5 Body BOInHandCustomerHand_PartyWise
        private void PrintBody_BOInHandCustomerHand_PartyWise()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
            _oFontStyleBoldUnderLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
            _oExportOutstandingDetails = _oExportOutstandingDetails.OrderBy(c => c.ContractorName).ToList();
            #region Initialize Table
            PdfPTable oPdfPTable = new PdfPTable(12);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                    30f,  //SL 
                                                    60f,  //Bill No
                                                    140f,  //Bank/Party Name
                                                    70f, //L/C No
                                                    50f,  //L/C Date                                                    
                                                    90f,  //PI No                                                   
                                                    50f,  //Value
                                                    90f,  //Concern Person 
                                                    55f,  //Last Delivery Date                                                    
                                                    30f,  //Due Days
                                                    50f,  //Bill Date
                                                    30f,  //Due Days
                                             });
            #endregion

            string sContractorName = ""; int nCount = 0; string sCurrencySymbol = "";
            double nTotalAmount = 0, nGrandTotalAmount = 0;
            foreach (ExportOutstandingDetail oItem in _oExportOutstandingDetails)
            {
                sCurrencySymbol = oItem.Currency;
                if (sContractorName != oItem.ContractorName)
                {
                    #region Supplier Wise Sub Total
                    if (oPdfPTable.Rows.Count > 0)
                    {
                        #region Initialize Table
                        oPdfPTable = new PdfPTable(12);
                        oPdfPTable.WidthPercentage = 100;
                        oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPTable.SetWidths(new float[] { 
                                                    30f,  //SL 
                                                    60f,  //Bill No
                                                    140f,  //Bank/Party Name
                                                    70f, //L/C No
                                                    50f,  //L/C Date                                                    
                                                    90f,  //PI No                                                   
                                                    50f,  //Value
                                                    90f,  //Concern Person 
                                                    55f,  //Last Delivery Date                                                    
                                                    30f,  //Due Days
                                                    50f,  //Bill Date
                                                    30f,  //Due Days
                                             });
                        #endregion

                        #region  Sub Total
                        _oPdfPCell = new PdfPCell(new Phrase("Sub Total :", _oFontStyleBold));
                        _oPdfPCell.Colspan = 10; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nTotalAmount), _oFontStyleBold));
                        _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        oPdfPTable.CompleteRow();
                        #endregion

                        #region Insert Into Main Table
                        _oPdfPCell = new PdfPCell(oPdfPTable);
                        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();
                        #endregion

                        nTotalAmount = 0; nCount = 0;
                    }
                    #endregion

                    #region Header
                    #region Blank Row
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion

                    #region Table Initialize
                    oPdfPTable = new PdfPTable(12);
                    oPdfPTable.WidthPercentage = 100;
                    oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.SetWidths(new float[] { 
                                                   30f,  //SL 
                                                    60f,  //Bill No
                                                    140f,  //Bank/Party Name
                                                    70f, //L/C No
                                                    50f,  //L/C Date                                                    
                                                    90f,  //PI No                                                   
                                                    50f,  //Value
                                                    90f,  //Concern Person 
                                                    55f,  //Last Delivery Date                                                    
                                                    30f,  //Due Days
                                                    50f,  //Bill Date
                                                    30f,  //Due Days
                                             });
                    #endregion

                    #region Supplier Heading
                    _oPdfPCell = new PdfPCell(new Phrase("Party Name : " + oItem.ContractorName, _oFontStyleBoldUnderLine));
                    _oPdfPCell.Colspan = 12; _oPdfPCell.FixedHeight = 14f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    oPdfPTable.CompleteRow();
                    #endregion

                    #region Header Row
                    _oPdfPCell = new PdfPCell(new Phrase("SL No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Bill No ", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Bank Name", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("L/C No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("L/C Date", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("PI No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);


                    _oPdfPCell = new PdfPCell(new Phrase("Value", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Concern Person", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Last Delivery", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Due", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Send DT", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Due", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    oPdfPTable.CompleteRow();
                    #endregion

                    #region Insert Into Main Table
                    _oPdfPCell = new PdfPCell(oPdfPTable);
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion
                    #endregion
                }


                #region Initialize Table
                oPdfPTable = new PdfPTable(12);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.SetWidths(new float[] { 
                                                    30f,  //SL 
                                                    60f,  //Bill No
                                                    140f,  //Bank/Party Name
                                                    70f, //L/C No
                                                    50f,  //L/C Date                                                    
                                                    90f,  //PI No                                                   
                                                    50f,  //Value
                                                    90f,  //Concern Person 
                                                    55f,  //Last Delivery Date                                                    
                                                    30f,  //Due Days
                                                    50f,  //Bill Date
                                                    30f,  //Due Days
                                             });
                #endregion

                nCount++;
                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ExportBillNo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.BankName_Nego, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ExportLCNo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.LCOpeningDatest, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.PINo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.AmountSt, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.MKTPName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.DeliveryDateSt, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                if (oItem.DeliveryDate == DateTime.MinValue)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(("").ToString(), _oFontStyle));
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase((Global.DateDiff("D", oItem.DeliveryDate, DateTime.Today)).ToString(), _oFontStyle));
                }
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.SendToPartySt, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                if (oItem.SendToParty == DateTime.MinValue)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(("").ToString(), _oFontStyle));
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase((Global.DateDiff("D", oItem.SendToParty, DateTime.Today)).ToString(), _oFontStyle));
                }
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();

                #region Insert Into Main Table
                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                sContractorName = oItem.ContractorName;
                sCurrencySymbol = oItem.Currency;
                nTotalAmount = nTotalAmount + oItem.Amount;
                nGrandTotalAmount = nGrandTotalAmount + oItem.Amount;
            }

            #region Initialize Table
            oPdfPTable = new PdfPTable(12);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                    30f,  //SL 
                                                    60f,  //Bill No
                                                    140f,  //Bank/Party Name
                                                    70f, //L/C No
                                                    50f,  //L/C Date                                                    
                                                    90f,  //PI No                                                   
                                                    50f,  //Value
                                                    90f,  //Concern Person 
                                                    55f,  //Last Delivery Date                                                    
                                                    30f,  //Due Days
                                                    50f,  //Bill Date
                                                    30f,  //Due Days
                                             });
            #endregion

            #region Date Wise Sub Total
            _oPdfPCell = new PdfPCell(new Phrase("Sub Total :", _oFontStyleBold));
            _oPdfPCell.Colspan = 10; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nTotalAmount), _oFontStyleBold));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Grand Total
            _oPdfPCell = new PdfPCell(new Phrase("Grand Total :", _oFontStyleBold));
            _oPdfPCell.Colspan = 10; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nGrandTotalAmount), _oFontStyleBold));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        #endregion
        #region  5 Body BOInHandCustomerHand_Bank Wise
        private void PrintBody_BOInHandCustomerHand_BankWise()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
            _oFontStyleBoldUnderLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
            _oExportOutstandingDetails = _oExportOutstandingDetails.OrderBy(c => c.BankName_Nego).ToList();
            #region Initialize Table
            PdfPTable oPdfPTable = new PdfPTable(12);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                    30f,  //SL 
                                                    60f,  //Bill No
                                                    140f,  //Bank/Party Name
                                                    70f, //L/C No
                                                    50f,  //L/C Date                                                    
                                                    90f,  //PI No                                                   
                                                    50f,  //Value
                                                    90f,  //Concern Person 
                                                    55f,  //Last Delivery Date                                                    
                                                    30f,  //Due Days
                                                    50f,  //Bill Date
                                                    30f,  //Due Days
                                             });
            #endregion

            string sBankName_Nego = ""; int nCount = 0; string sCurrencySymbol = "";
            double nTotalAmount = 0, nGrandTotalAmount = 0;
            foreach (ExportOutstandingDetail oItem in _oExportOutstandingDetails)
            {
                if (sBankName_Nego != oItem.BankName_Nego)
                {
                    #region Supplier Wise Sub Total
                    if (oPdfPTable.Rows.Count > 0)
                    {
                        #region Initialize Table
                        oPdfPTable = new PdfPTable(12);
                        oPdfPTable.WidthPercentage = 100;
                        oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPTable.SetWidths(new float[] { 
                                                     30f,  //SL 
                                                    60f,  //Bill No
                                                    140f,  //Bank/Party Name
                                                    70f, //L/C No
                                                    50f,  //L/C Date                                                    
                                                    90f,  //PI No                                                   
                                                    50f,  //Value
                                                    90f,  //Concern Person 
                                                    55f,  //Last Delivery Date                                                    
                                                    30f,  //Due Days
                                                    50f,  //Bill Date
                                                    30f,  //Due Days
                                             });
                        #endregion

                        #region  Sub Total
                        _oPdfPCell = new PdfPCell(new Phrase("Sub Total :", _oFontStyleBold));
                        _oPdfPCell.Colspan = 10; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nTotalAmount), _oFontStyleBold));
                        _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        oPdfPTable.CompleteRow();
                        #endregion

                        #region Insert Into Main Table
                        _oPdfPCell = new PdfPCell(oPdfPTable);
                        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();
                        #endregion

                        nTotalAmount = 0; nCount = 0;
                    }
                    #endregion

                    #region Header
                    #region Blank Row
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion

                    #region Table Initialize
                    oPdfPTable = new PdfPTable(12);
                    oPdfPTable.WidthPercentage = 100;
                    oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.SetWidths(new float[] { 
                                                    30f,  //SL 
                                                    60f,  //Bill No
                                                    140f,  //Bank/Party Name
                                                    70f, //L/C No
                                                    50f,  //L/C Date                                                    
                                                    90f,  //PI No                                                   
                                                    50f,  //Value
                                                    90f,  //Concern Person 
                                                    55f,  //Last Delivery Date                                                    
                                                    30f,  //Due Days
                                                    50f,  //Bill Date
                                                    30f,  //Due Days
                                             });
                    #endregion

                    #region Supplier Heading
                    _oPdfPCell = new PdfPCell(new Phrase("Bank Name : " + oItem.BankName_Nego, _oFontStyleBoldUnderLine));
                    _oPdfPCell.Colspan = 12; _oPdfPCell.FixedHeight = 14f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    oPdfPTable.CompleteRow();
                    #endregion

                    #region Header Row
                    _oPdfPCell = new PdfPCell(new Phrase("SL No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Bill No ", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Buyer Name", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("L/C No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("L/C Date", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("PI No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);


                    _oPdfPCell = new PdfPCell(new Phrase("Value", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Concern Person", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Last Delivery", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Due", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Send DT", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Due", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    oPdfPTable.CompleteRow();
                    #endregion

                    #region Insert Into Main Table
                    _oPdfPCell = new PdfPCell(oPdfPTable);
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion
                    #endregion
                }


                #region Initialize Table
                oPdfPTable = new PdfPTable(12);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.SetWidths(new float[] { 
                                                    30f,  //SL 
                                                    60f,  //Bill No
                                                    140f,  //Bank/Party Name
                                                    70f, //L/C No
                                                    50f,  //L/C Date                                                    
                                                    90f,  //PI No                                                   
                                                    50f,  //Value
                                                    90f,  //Concern Person 
                                                    55f,  //Last Delivery Date                                                    
                                                    30f,  //Due Days
                                                    50f,  //Bill Date
                                                    30f,  //Due Days
                                             });
                #endregion

                nCount++;
                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ExportBillNo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ContractorName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ExportLCNo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.LCOpeningDatest, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.PINo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.AmountSt, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.MKTPName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.DeliveryDateSt, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                if (oItem.DeliveryDate == DateTime.MinValue)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(("").ToString(), _oFontStyle));
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase((Global.DateDiff("D", oItem.DeliveryDate, DateTime.Today)).ToString(), _oFontStyle));
                }
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.SendToPartySt, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                if (oItem.SendToParty == DateTime.MinValue)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(("").ToString(), _oFontStyle));
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase((Global.DateDiff("D", oItem.SendToParty, DateTime.Today)).ToString(), _oFontStyle));
                }
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();

                #region Insert Into Main Table
                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                sBankName_Nego = oItem.BankName_Nego;
                sCurrencySymbol = oItem.Currency;
                nTotalAmount = nTotalAmount + oItem.Amount;
                nGrandTotalAmount = nGrandTotalAmount + oItem.Amount;
            }

            #region Initialize Table
            oPdfPTable = new PdfPTable(12);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                   30f,  //SL 
                                                    60f,  //Bill No
                                                    140f,  //Bank/Party Name
                                                    70f, //L/C No
                                                    50f,  //L/C Date                                                    
                                                    90f,  //PI No                                                   
                                                    50f,  //Value
                                                    90f,  //Concern Person 
                                                    55f,  //Last Delivery Date                                                    
                                                    30f,  //Due Days
                                                    50f,  //Bill Date
                                                    30f,  //Due Days
                                             });
            #endregion

            #region Date Wise Sub Total
            _oPdfPCell = new PdfPCell(new Phrase("Sub Total :", _oFontStyleBold));
            _oPdfPCell.Colspan = 10; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nTotalAmount), _oFontStyleBold));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Grand Total
            _oPdfPCell = new PdfPCell(new Phrase("Grand Total :", _oFontStyleBold));
            _oPdfPCell.Colspan = 10; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nGrandTotalAmount), _oFontStyleBold));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        #endregion

        #region  6 Body PartyAcceptanceBill_PartyWise
        private void PrintBody_PartyAcceptanceBill_PartyWise()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
            _oFontStyleBoldUnderLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
            _oExportOutstandingDetails = _oExportOutstandingDetails.OrderBy(c => c.ContractorName).ToList();
            #region Initialize Table
            PdfPTable oPdfPTable = new PdfPTable(12);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                    30f,  //SL 
                                                    60f,  //Bill No
                                                    140f,  //Bank/Party Name
                                                    70f, //L/C No
                                                    50f,  //L/C Date                                                    
                                                    90f,  //PI No                                                   
                                                    50f,  //Value
                                                    90f,  //Concern Person 
                                                    55f,  //Last Delivery Date                                                    
                                                    30f,  //Due Days
                                                    50f,  //Bill Date
                                                    30f,  //Due Days
                                             });
            #endregion

            string sContractorName = ""; int nCount = 0; string sCurrencySymbol = "";
            double nTotalAmount = 0, nGrandTotalAmount = 0;
            foreach (ExportOutstandingDetail oItem in _oExportOutstandingDetails)
            {
                sCurrencySymbol = oItem.Currency;
                if (sContractorName != oItem.ContractorName)
                {
                    #region Supplier Wise Sub Total
                    if (oPdfPTable.Rows.Count > 0)
                    {
                        #region Initialize Table
                        oPdfPTable = new PdfPTable(12);
                        oPdfPTable.WidthPercentage = 100;
                        oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPTable.SetWidths(new float[] { 
                                                    30f,  //SL 
                                                    60f,  //Bill No
                                                    140f,  //Bank/Party Name
                                                    70f, //L/C No
                                                    50f,  //L/C Date                                                    
                                                    90f,  //PI No                                                   
                                                    50f,  //Value
                                                    90f,  //Concern Person 
                                                    55f,  //Last Delivery Date                                                    
                                                    30f,  //Due Days
                                                    50f,  //Bill Date
                                                    30f,  //Due Days
                                             });
                        #endregion

                        #region  Sub Total
                        _oPdfPCell = new PdfPCell(new Phrase("Sub Total :", _oFontStyleBold));
                        _oPdfPCell.Colspan = 10; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nTotalAmount), _oFontStyleBold));
                        _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        oPdfPTable.CompleteRow();
                        #endregion

                        #region Insert Into Main Table
                        _oPdfPCell = new PdfPCell(oPdfPTable);
                        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();
                        #endregion

                        nTotalAmount = 0; nCount = 0;
                    }
                    #endregion

                    #region Header
                    #region Blank Row
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion

                    #region Table Initialize
                    oPdfPTable = new PdfPTable(12);
                    oPdfPTable.WidthPercentage = 100;
                    oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.SetWidths(new float[] { 
                                                   30f,  //SL 
                                                    60f,  //Bill No
                                                    140f,  //Bank/Party Name
                                                    70f, //L/C No
                                                    50f,  //L/C Date                                                    
                                                    90f,  //PI No                                                   
                                                    50f,  //Value
                                                    90f,  //Concern Person 
                                                    55f,  //Last Delivery Date                                                    
                                                    30f,  //Due Days
                                                    50f,  //Bill Date
                                                    30f,  //Due Days
                                             });
                    #endregion

                    #region Supplier Heading
                    _oPdfPCell = new PdfPCell(new Phrase("Party Name : " + oItem.ContractorName, _oFontStyleBoldUnderLine));
                    _oPdfPCell.Colspan = 12; _oPdfPCell.FixedHeight = 14f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    oPdfPTable.CompleteRow();
                    #endregion

                    #region Header Row
                    _oPdfPCell = new PdfPCell(new Phrase("SL No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Bill No ", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Bank Name", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("L/C No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("L/C Date", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("PI No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);


                    _oPdfPCell = new PdfPCell(new Phrase("Value", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Concern Person", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Last Delivery", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Due", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Receive DT", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Due", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    oPdfPTable.CompleteRow();
                    #endregion

                    #region Insert Into Main Table
                    _oPdfPCell = new PdfPCell(oPdfPTable);
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion
                    #endregion
                }


                #region Initialize Table
                oPdfPTable = new PdfPTable(12);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.SetWidths(new float[] { 
                                                    30f,  //SL 
                                                    60f,  //Bill No
                                                    140f,  //Bank/Party Name
                                                    70f, //L/C No
                                                    50f,  //L/C Date                                                    
                                                    90f,  //PI No                                                   
                                                    50f,  //Value
                                                    90f,  //Concern Person 
                                                    55f,  //Last Delivery Date                                                    
                                                    30f,  //Due Days
                                                    50f,  //Bill Date
                                                    30f,  //Due Days
                                             });
                #endregion

                nCount++;
                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ExportBillNo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.BankName_Nego, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ExportLCNo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.LCOpeningDatest, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.PINo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.AmountSt, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.MKTPName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.DeliveryDateSt, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                if (oItem.DeliveryDate == DateTime.MinValue)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(("").ToString(), _oFontStyle));
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase((Global.DateDiff("D", oItem.DeliveryDate, DateTime.Today)).ToString(), _oFontStyle));
                }
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.RecdFromPartySt, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                if (oItem.RecdFromParty == DateTime.MinValue)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(("").ToString(), _oFontStyle));
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase((Global.DateDiff("D", oItem.RecdFromParty, DateTime.Today)).ToString(), _oFontStyle));
                }
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();

                #region Insert Into Main Table
                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                sContractorName = oItem.ContractorName;
                sCurrencySymbol = oItem.Currency;
                nTotalAmount = nTotalAmount + oItem.Amount;
                nGrandTotalAmount = nGrandTotalAmount + oItem.Amount;
            }

            #region Initialize Table
            oPdfPTable = new PdfPTable(12);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                    30f,  //SL 
                                                    60f,  //Bill No
                                                    140f,  //Bank/Party Name
                                                    70f, //L/C No
                                                    50f,  //L/C Date                                                    
                                                    90f,  //PI No                                                   
                                                    50f,  //Value
                                                    90f,  //Concern Person 
                                                    55f,  //Last Delivery Date                                                    
                                                    30f,  //Due Days
                                                    50f,  //Bill Date
                                                    30f,  //Due Days
                                             });
            #endregion

            #region Date Wise Sub Total
            _oPdfPCell = new PdfPCell(new Phrase("Sub Total :", _oFontStyleBold));
            _oPdfPCell.Colspan = 10; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nTotalAmount), _oFontStyleBold));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Grand Total
            _oPdfPCell = new PdfPCell(new Phrase("Grand Total :", _oFontStyleBold));
            _oPdfPCell.Colspan = 10; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nGrandTotalAmount), _oFontStyleBold));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        #endregion
        #region 6 Body PartyAcceptanceBill_Bank Wise
        private void PrintBody_PartyAcceptanceBill_BankWise()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
            _oFontStyleBoldUnderLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
            _oExportOutstandingDetails = _oExportOutstandingDetails.OrderBy(c => c.BankName_Nego).ToList();
            #region Initialize Table
            PdfPTable oPdfPTable = new PdfPTable(12);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                    30f,  //SL 
                                                    60f,  //Bill No
                                                    140f,  //Bank/Party Name
                                                    70f, //L/C No
                                                    50f,  //L/C Date                                                    
                                                    90f,  //PI No                                                   
                                                    50f,  //Value
                                                    90f,  //Concern Person 
                                                    55f,  //Last Delivery Date                                                    
                                                    30f,  //Due Days
                                                    50f,  //Bill Date
                                                    30f,  //Due Days
                                             });
            #endregion

            string sBankName_Nego = ""; int nCount = 0; string sCurrencySymbol = "";
            double nTotalAmount = 0, nGrandTotalAmount = 0;
            foreach (ExportOutstandingDetail oItem in _oExportOutstandingDetails)
            {
                if (sBankName_Nego != oItem.BankName_Nego)
                {
                    #region Supplier Wise Sub Total
                    if (oPdfPTable.Rows.Count > 0)
                    {
                        #region Initialize Table
                        oPdfPTable = new PdfPTable(12);
                        oPdfPTable.WidthPercentage = 100;
                        oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPTable.SetWidths(new float[] { 
                                                     30f,  //SL 
                                                    60f,  //Bill No
                                                    140f,  //Bank/Party Name
                                                    70f, //L/C No
                                                    50f,  //L/C Date                                                    
                                                    90f,  //PI No                                                   
                                                    50f,  //Value
                                                    90f,  //Concern Person 
                                                    55f,  //Last Delivery Date                                                    
                                                    30f,  //Due Days
                                                    50f,  //Bill Date
                                                    30f,  //Due Days
                                             });
                        #endregion

                        #region  Sub Total
                        _oPdfPCell = new PdfPCell(new Phrase("Sub Total :", _oFontStyleBold));
                        _oPdfPCell.Colspan = 10; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nTotalAmount), _oFontStyleBold));
                        _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        oPdfPTable.CompleteRow();
                        #endregion

                        #region Insert Into Main Table
                        _oPdfPCell = new PdfPCell(oPdfPTable);
                        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();
                        #endregion

                        nTotalAmount = 0; nCount = 0;
                    }
                    #endregion

                    #region Header
                    #region Blank Row
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion

                    #region Table Initialize
                    oPdfPTable = new PdfPTable(12);
                    oPdfPTable.WidthPercentage = 100;
                    oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.SetWidths(new float[] { 
                                                    30f,  //SL 
                                                    60f,  //Bill No
                                                    140f,  //Bank/Party Name
                                                    70f, //L/C No
                                                    50f,  //L/C Date                                                    
                                                    90f,  //PI No                                                   
                                                    50f,  //Value
                                                    90f,  //Concern Person 
                                                    55f,  //Last Delivery Date                                                    
                                                    30f,  //Due Days
                                                    50f,  //Bill Date
                                                    30f,  //Due Days
                                             });
                    #endregion

                    #region Supplier Heading
                    _oPdfPCell = new PdfPCell(new Phrase("Bank Name : " + oItem.BankName_Nego, _oFontStyleBoldUnderLine));
                    _oPdfPCell.Colspan = 12; _oPdfPCell.FixedHeight = 14f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    oPdfPTable.CompleteRow();
                    #endregion

                    #region Header Row
                    _oPdfPCell = new PdfPCell(new Phrase("SL No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Bill No ", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Buyer Name", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("L/C No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("L/C Date", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("PI No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);


                    _oPdfPCell = new PdfPCell(new Phrase("Value", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Concern Person", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Last Delivery", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Due", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Receive DT", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Due", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    oPdfPTable.CompleteRow();
                    #endregion

                    #region Insert Into Main Table
                    _oPdfPCell = new PdfPCell(oPdfPTable);
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion
                    #endregion
                }


                #region Initialize Table
                oPdfPTable = new PdfPTable(12);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.SetWidths(new float[] { 
                                                    30f,  //SL 
                                                    60f,  //Bill No
                                                    140f,  //Bank/Party Name
                                                    70f, //L/C No
                                                    50f,  //L/C Date                                                    
                                                    90f,  //PI No                                                   
                                                    50f,  //Value
                                                    90f,  //Concern Person 
                                                    55f,  //Last Delivery Date                                                    
                                                    30f,  //Due Days
                                                    50f,  //Bill Date
                                                    30f,  //Due Days
                                             });
                #endregion

                nCount++;
                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ExportBillNo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ContractorName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ExportLCNo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.LCOpeningDatest, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.PINo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.AmountSt, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.MKTPName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.DeliveryDateSt, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                if (oItem.DeliveryDate == DateTime.MinValue)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(("").ToString(), _oFontStyle));
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase((Global.DateDiff("D", oItem.DeliveryDate, DateTime.Today)).ToString(), _oFontStyle));
                }
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.RecdFromPartySt, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                if (oItem.RecdFromParty == DateTime.MinValue)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase((Global.DateDiff("D", oItem.RecdFromParty, DateTime.Today)).ToString(), _oFontStyle));
                }
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();

                #region Insert Into Main Table
                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                sBankName_Nego = oItem.BankName_Nego;
                sCurrencySymbol = oItem.Currency;
                nTotalAmount = nTotalAmount + oItem.Amount;
                nGrandTotalAmount = nGrandTotalAmount + oItem.Amount;
            }

            #region Initialize Table
            oPdfPTable = new PdfPTable(12);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                   30f,  //SL 
                                                    60f,  //Bill No
                                                    140f,  //Bank/Party Name
                                                    70f, //L/C No
                                                    50f,  //L/C Date                                                    
                                                    90f,  //PI No                                                   
                                                    50f,  //Value
                                                    90f,  //Concern Person 
                                                    55f,  //Last Delivery Date                                                    
                                                    30f,  //Due Days
                                                    50f,  //Bill Date
                                                    30f,  //Due Days
                                             });
            #endregion

            #region Date Wise Sub Total
            _oPdfPCell = new PdfPCell(new Phrase("Sub Total :", _oFontStyleBold));
            _oPdfPCell.Colspan = 10; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nTotalAmount), _oFontStyleBold));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Grand Total
            _oPdfPCell = new PdfPCell(new Phrase("Grand Total :", _oFontStyleBold));
            _oPdfPCell.Colspan = 10; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nGrandTotalAmount), _oFontStyleBold));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        #endregion

        #region  7 Panding Bank Acceptance( Nego_Transit = 3,  NegotiatedBill = 4,)==7--Bill State=3,4
        private void PrintBody_PendingBankAcceptance_PartyWise()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
            _oFontStyleBoldUnderLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
            _oExportOutstandingDetails = _oExportOutstandingDetails.OrderBy(c => c.ContractorName).ToList();
            #region Initialize Table
            PdfPTable oPdfPTable = new PdfPTable(12);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                    30f,  //SL 
                                                    60f,  //Bill No
                                                    140f,  //Bank/Party Name
                                                    75f, //L/C No
                                                    50f,  //L/C Date                                                    
                                                    //90f,  //PI No                                                   
                                                    50f,  //Value
                                                    95f,  //Concern Person 
                                                    55f,  //Party Send Date                                                    
                                                    55f,  //Party Rec Date
                                                    50f,  //Submit Date
                                                    50f,  //LDBC No
                                                    50f,  //LDBC Date
                                             });
            #endregion

            string sContractorName = ""; int nCount = 0; string sCurrencySymbol = "";
            double nTotalAmount = 0, nGrandTotalAmount = 0;
            foreach (ExportOutstandingDetail oItem in _oExportOutstandingDetails)
            {
                sCurrencySymbol = oItem.Currency;
                if (sContractorName != oItem.ContractorName)
                {
                    #region Supplier Wise Sub Total
                    if (oPdfPTable.Rows.Count > 0)
                    {
                        #region Initialize Table
                        oPdfPTable = new PdfPTable(12);
                        oPdfPTable.WidthPercentage = 100;
                        oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPTable.SetWidths(new float[] { 
                                                   30f,  //SL 
                                                    60f,  //Bill No
                                                    140f,  //Bank/Party Name
                                                    75f, //L/C No
                                                    50f,  //L/C Date                                                    
                                                    //90f,  //PI No                                                   
                                                    50f,  //Value
                                                    95f,  //Concern Person 
                                                    55f,  //Party Send Date                                                    
                                                    55f,  //Party Rec Date
                                                    50f,  //Submit Date
                                                    50f,  //LDBC No
                                                    50f,  //LDBC Date
                                             });
                        #endregion

                        #region  Sub Total
                        _oPdfPCell = new PdfPCell(new Phrase("Sub Total :", _oFontStyleBold));
                        _oPdfPCell.Colspan = 10; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nTotalAmount), _oFontStyleBold));
                        _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        oPdfPTable.CompleteRow();
                        #endregion

                        #region Insert Into Main Table
                        _oPdfPCell = new PdfPCell(oPdfPTable);
                        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();
                        #endregion

                        nTotalAmount = 0; nCount = 0;
                    }
                    #endregion

                    #region Header
                    #region Blank Row
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion

                    #region Table Initialize
                    oPdfPTable = new PdfPTable(12);
                    oPdfPTable.WidthPercentage = 100;
                    oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.SetWidths(new float[] { 
                                                    30f,  //SL 
                                                    60f,  //Bill No
                                                    140f,  //Bank/Party Name
                                                    75f, //L/C No
                                                    50f,  //L/C Date                                                    
                                                    //90f,  //PI No                                                   
                                                    50f,  //Value
                                                    95f,  //Concern Person 
                                                    55f,  //Party Send Date                                                    
                                                    55f,  //Party Rec Date
                                                    50f,  //Submit Date
                                                    50f,  //LDBC No
                                                    50f,  //LDBC Date
                                             });
                    #endregion

                    #region Supplier Heading
                    _oPdfPCell = new PdfPCell(new Phrase("Party Name : " + oItem.ContractorName, _oFontStyleBoldUnderLine));
                    _oPdfPCell.Colspan = 12; _oPdfPCell.FixedHeight = 14f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    oPdfPTable.CompleteRow();
                    #endregion

                    #region Header Row
                    _oPdfPCell = new PdfPCell(new Phrase("SL No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Bill No ", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Bank Name", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("L/C No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("L/C Date", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    //_oPdfPCell = new PdfPCell(new Phrase("PI No", _oFontStyleBold));
                    //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);


                    _oPdfPCell = new PdfPCell(new Phrase("Value", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Concern Person", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Party Send DT", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Recive DT", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Submit DT", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("LDBC No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("LDBC Date", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    oPdfPTable.CompleteRow();
                    #endregion

                    #region Insert Into Main Table
                    _oPdfPCell = new PdfPCell(oPdfPTable);
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion
                    #endregion
                }


                #region Initialize Table
                oPdfPTable = new PdfPTable(12);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.SetWidths(new float[] { 
                                                   30f,  //SL 
                                                    60f,  //Bill No
                                                    140f,  //Bank/Party Name
                                                    75f, //L/C No
                                                    50f,  //L/C Date                                                    
                                                    //90f,  //PI No                                                   
                                                    50f,  //Value
                                                    95f,  //Concern Person 
                                                    55f,  //Party Send Date                                                    
                                                    55f,  //Party Rec Date
                                                    50f,  //Submit Date
                                                    50f,  //LDBC No
                                                    50f,  //LDBC Date
                                             });
                #endregion

                nCount++;
                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ExportBillNo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.BankName_Nego, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ExportLCNo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.LCOpeningDatest, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase(oItem.PINo, _oFontStyle));
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.AmountSt, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.MKTPName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.SendToPartySt, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase(oItem.RecdFromPartySt, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.SendToBankDateSt, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.LDBCNo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.LDBCDateSt, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();

                #region Insert Into Main Table
                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                sContractorName = oItem.ContractorName;
                sCurrencySymbol = oItem.Currency;
                nTotalAmount = nTotalAmount + oItem.Amount;
                nGrandTotalAmount = nGrandTotalAmount + oItem.Amount;
            }

            #region Initialize Table
            oPdfPTable = new PdfPTable(12);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                    30f,  //SL 
                                                    60f,  //Bill No
                                                    140f,  //Bank/Party Name
                                                    75f, //L/C No
                                                    50f,  //L/C Date                                                    
                                                    //90f,  //PI No                                                   
                                                    50f,  //Value
                                                    95f,  //Concern Person 
                                                    55f,  //Party Send Date                                                    
                                                    55f,  //Party Rec Date
                                                    50f,  //Submit Date
                                                    50f,  //LDBC No
                                                    50f,  //LDBC Date
                                             });
            #endregion

            #region Date Wise Sub Total
            _oPdfPCell = new PdfPCell(new Phrase("Sub Total :", _oFontStyleBold));
            _oPdfPCell.Colspan = 10; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nTotalAmount), _oFontStyleBold));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Grand Total
            _oPdfPCell = new PdfPCell(new Phrase("Grand Total :", _oFontStyleBold));
            _oPdfPCell.Colspan = 10; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nGrandTotalAmount), _oFontStyleBold));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        #endregion
        #region 7 BodyPendingBankAcceptance_Bank Wise
        private void PrintBody_PendingBankAcceptance_BankWise()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
            _oFontStyleBoldUnderLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
            _oExportOutstandingDetails = _oExportOutstandingDetails.OrderBy(c => c.BankName_Nego).ToList();
            #region Initialize Table
            PdfPTable oPdfPTable = new PdfPTable(12);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                   30f,  //SL 
                                                    60f,  //Bill No
                                                    140f,  //Bank/Party Name
                                                    75f, //L/C No
                                                    50f,  //L/C Date                                                    
                                                    //90f,  //PI No                                                   
                                                    50f,  //Value
                                                    95f,  //Concern Person 
                                                    55f,  //Party Send Date                                                    
                                                    55f,  //Party Rec Date
                                                    50f,  //Submit Date
                                                    50f,  //LDBC No
                                                    50f,  //LDBC Date
                                             });
            #endregion

            string sBankName_Nego = ""; int nCount = 0; string sCurrencySymbol = "";
            double nTotalAmount = 0, nGrandTotalAmount = 0;
            foreach (ExportOutstandingDetail oItem in _oExportOutstandingDetails)
            {
                if (sBankName_Nego != oItem.BankName_Nego)
                {
                    #region Supplier Wise Sub Total
                    if (oPdfPTable.Rows.Count > 0)
                    {
                        #region Initialize Table
                        oPdfPTable = new PdfPTable(12);
                        oPdfPTable.WidthPercentage = 100;
                        oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPTable.SetWidths(new float[] { 
                                                     30f,  //SL 
                                                    60f,  //Bill No
                                                    140f,  //Bank/Party Name
                                                    75f, //L/C No
                                                    50f,  //L/C Date                                                    
                                                    //90f,  //PI No                                                   
                                                    50f,  //Value
                                                    95f,  //Concern Person 
                                                    55f,  //Party Send Date                                                    
                                                    55f,  //Party Rec Date
                                                    50f,  //Submit Date
                                                    50f,  //LDBC No
                                                    50f,  //LDBC Date
                                             });
                        #endregion

                        #region  Sub Total
                        _oPdfPCell = new PdfPCell(new Phrase("Sub Total :", _oFontStyleBold));
                        _oPdfPCell.Colspan = 10; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nTotalAmount), _oFontStyleBold));
                        _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        oPdfPTable.CompleteRow();
                        #endregion

                        #region Insert Into Main Table
                        _oPdfPCell = new PdfPCell(oPdfPTable);
                        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();
                        #endregion

                        nTotalAmount = 0; nCount = 0;
                    }
                    #endregion

                    #region Header
                    #region Blank Row
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion

                    #region Table Initialize
                    oPdfPTable = new PdfPTable(12);
                    oPdfPTable.WidthPercentage = 100;
                    oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.SetWidths(new float[] { 
                                                    30f,  //SL 
                                                    60f,  //Bill No
                                                    140f,  //Bank/Party Name
                                                    75f, //L/C No
                                                    50f,  //L/C Date                                                    
                                                    //90f,  //PI No                                                   
                                                    50f,  //Value
                                                    95f,  //Concern Person 
                                                    55f,  //Party Send Date                                                    
                                                    55f,  //Party Rec Date
                                                    50f,  //Submit Date
                                                    50f,  //LDBC No
                                                    50f,  //LDBC Date
                                             });
                    #endregion

                    #region Supplier Heading
                    _oPdfPCell = new PdfPCell(new Phrase("Bank Name : " + oItem.BankName_Nego, _oFontStyleBoldUnderLine));
                    _oPdfPCell.Colspan = 12; _oPdfPCell.FixedHeight = 14f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    oPdfPTable.CompleteRow();
                    #endregion

                    #region Header Row
                    _oPdfPCell = new PdfPCell(new Phrase("SL No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Bill No ", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Buyer Name", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("L/C No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("L/C Date", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    //_oPdfPCell = new PdfPCell(new Phrase("PI No", _oFontStyleBold));
                    //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Value", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Concern Person", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Party Send DT", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Recive DT", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Submit DT", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("LDBC No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("LDBC Date", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    oPdfPTable.CompleteRow();
                    #endregion

                    #region Insert Into Main Table
                    _oPdfPCell = new PdfPCell(oPdfPTable);
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion
                    #endregion
                }


                #region Initialize Table
                oPdfPTable = new PdfPTable(12);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.SetWidths(new float[] { 
                                                    30f,  //SL 
                                                    60f,  //Bill No
                                                    140f,  //Bank/Party Name
                                                    75f, //L/C No
                                                    50f,  //L/C Date                                                    
                                                    //90f,  //PI No                                                   
                                                    50f,  //Value
                                                    95f,  //Concern Person 
                                                    55f,  //Party Send Date                                                    
                                                    55f,  //Party Rec Date
                                                    50f,  //Submit Date
                                                    50f,  //LDBC No
                                                    50f,  //LDBC Date
                                             });
                #endregion

                nCount++;
                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ExportBillNo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ContractorName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ExportLCNo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.LCOpeningDatest, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase(oItem.PINo, _oFontStyle));
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.AmountSt, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.MKTPName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.SendToPartySt, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase(oItem.RecdFromPartySt, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.SendToBankDateSt, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.LDBCNo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.LDBCDateSt, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();

                #region Insert Into Main Table
                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                sBankName_Nego = oItem.BankName_Nego;
                sCurrencySymbol = oItem.Currency;
                nTotalAmount = nTotalAmount + oItem.Amount;
                nGrandTotalAmount = nGrandTotalAmount + oItem.Amount;
            }

            #region Initialize Table
            oPdfPTable = new PdfPTable(12);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                   30f,  //SL 
                                                    60f,  //Bill No
                                                    140f,  //Bank/Party Name
                                                    75f, //L/C No
                                                    50f,  //L/C Date                                                    
                                                    //90f,  //PI No                                                   
                                                    50f,  //Value
                                                    95f,  //Concern Person 
                                                    55f,  //Party Send Date                                                    
                                                    55f,  //Party Rec Date
                                                    50f,  //Submit Date
                                                    50f,  //LDBC No
                                                    50f,  //LDBC Date
                                             });
            #endregion

            #region Date Wise Sub Total
            _oPdfPCell = new PdfPCell(new Phrase("Sub Total :", _oFontStyleBold));
            _oPdfPCell.Colspan = 10; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nTotalAmount), _oFontStyleBold));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Grand Total
            _oPdfPCell = new PdfPCell(new Phrase("Grand Total :", _oFontStyleBold));
            _oPdfPCell.Colspan = 10; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nGrandTotalAmount), _oFontStyleBold));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        #endregion

        #region  8 OverduePayment( BAcceptance = 5,  Discounted = 7,)==7--Bill State=5,7
        private void PrintBody_OverduePayment_PartyWise()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
            _oFontStyleBoldUnderLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
            _oExportOutstandingDetails = _oExportOutstandingDetails.OrderBy(c => c.ContractorName).ToList();
            #region Initialize Table
            PdfPTable oPdfPTable = new PdfPTable(12);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                     30f,  //SL 
                                                    60f,  //L/C No
                                                    130f,  //Bank/Party Name
                                                    85f, //PI No   
                                                    100f,  //Issue Bank   
                                                    60f,  //Issue Bank   
                                                    25f,  //Issue Bank   
                                                    50f,  //Value
                                                    60f,  //LDBC  
                                                    55f,  //Maturity                                                    
                                                    25f,  //Dues
                                                    //25f,  //O.Due Rate
                                                    85f,  //Contact Person
                                             });
            #endregion

            string sContractorName = ""; int nCount = 0; string sCurrencySymbol = "";
            double nTotalAmount = 0, nGrandTotalAmount = 0;
            foreach (ExportOutstandingDetail oItem in _oExportOutstandingDetails)
            {
                sCurrencySymbol = oItem.Currency;
                if (sContractorName != oItem.ContractorName)
                {
                    #region Supplier Wise Sub Total
                    if (oPdfPTable.Rows.Count > 0)
                    {
                        #region Initialize Table
                        oPdfPTable = new PdfPTable(12);
                        oPdfPTable.WidthPercentage = 100;
                        oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPTable.SetWidths(new float[] { 
                                                    30f,  //SL 
                                                    60f,  //L/C No
                                                    130f,  //Bank/Party Name
                                                    85f, //PI No   
                                                    100f,  //Issue Bank   
                                                    60f,  //Issue Bank   
                                                    25f,  //Issue Bank   
                                                    50f,  //Value
                                                    60f,  //LDBC  
                                                    55f,  //Maturity                                                    
                                                    25f,  //Dues
                                                    85f,  //Contact Person
                                             });
                        #endregion

                        #region  Sub Total
                        _oPdfPCell = new PdfPCell(new Phrase("Sub Total :", _oFontStyleBold));
                        _oPdfPCell.Colspan = 10; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nTotalAmount), _oFontStyleBold));
                        _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        oPdfPTable.CompleteRow();
                        #endregion

                        #region Insert Into Main Table
                        _oPdfPCell = new PdfPCell(oPdfPTable);
                        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();
                        #endregion

                        nTotalAmount = 0; nCount = 0;
                    }
                    #endregion

                    #region Header
                    #region Blank Row
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion

                    #region Table Initialize
                    oPdfPTable = new PdfPTable(12);
                    oPdfPTable.WidthPercentage = 100;
                    oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.SetWidths(new float[] { 
                                                       30f,  //SL 
                                                    60f,  //L/C No
                                                    130f,  //Bank/Party Name
                                                    85f, //PI No   
                                                    100f,  //Issue Bank   
                                                    60f,  //Issue Bank   
                                                    25f,  //Issue Bank   
                                                    50f,  //Value
                                                    60f,  //LDBC  
                                                    55f,  //Maturity                                                    
                                                    25f,  //Dues
                                                    85f,  //Contact Person
                                                   
                                             });
                    #endregion

                    #region Supplier Heading
                    _oPdfPCell = new PdfPCell(new Phrase("Party Name : " + oItem.ContractorName, _oFontStyleBoldUnderLine));
                    _oPdfPCell.Colspan = 12; _oPdfPCell.FixedHeight = 14f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    oPdfPTable.CompleteRow();
                    #endregion

                    #region Header Row
                    _oPdfPCell = new PdfPCell(new Phrase("SL No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("L/C No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Bank Name", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);


                    _oPdfPCell = new PdfPCell(new Phrase("P/I No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Issue Bank", _oFontStyleBold));
                    _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);


                    _oPdfPCell = new PdfPCell(new Phrase("Value", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("LDBC No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Maturity Date", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Dues", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    //_oPdfPCell = new PdfPCell(new Phrase("O.Due Rate", _oFontStyleBold));
                    //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);


                    _oPdfPCell = new PdfPCell(new Phrase("Concern Person", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);



                    oPdfPTable.CompleteRow();
                    #endregion

                    #region Insert Into Main Table
                    _oPdfPCell = new PdfPCell(oPdfPTable);
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion
                    #endregion
                }


                #region Initialize Table
                oPdfPTable = new PdfPTable(12);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.SetWidths(new float[] { 
                                                     30f,  //SL 
                                                    60f,  //L/C No
                                                    130f,  //Bank/Party Name
                                                    85f, //PI No   
                                                    100f,  //Issue Bank   
                                                    60f,  //Issue Bank   
                                                    25f,  //Issue Bank   
                                                    50f,  //Value
                                                    60f,  //LDBC  
                                                    55f,  //Maturity                                                    
                                                    25f,  //Dues
                                                    85f,  //Contact Person
                                             });
                #endregion

                nCount++;
                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ExportLCNo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.BankName_Nego, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.PINo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.BankName_Issue + "[" + oItem.BBranchName_Issue + "]", _oFontStyle));
                _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase(oItem.PINo, _oFontStyle));
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.AmountSt, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.LDBCNo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.MaturityDateSt, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase(oItem.TimeLag8.ToString(), _oFontStyle));

                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase(oItem.OverDueRate.ToString(), _oFontStyle));
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.MKTPName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


                oPdfPTable.CompleteRow();

                #region Insert Into Main Table
                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                sContractorName = oItem.ContractorName;
                sCurrencySymbol = oItem.Currency;
                nTotalAmount = nTotalAmount + oItem.Amount;
                nGrandTotalAmount = nGrandTotalAmount + oItem.Amount;
            }

            #region Initialize Table
            oPdfPTable = new PdfPTable(12);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                      30f,  //SL 
                                                    60f,  //L/C No
                                                    130f,  //Bank/Party Name
                                                    85f, //PI No   
                                                    100f,  //Issue Bank   
                                                    60f,  //Issue Bank   
                                                    25f,  //Issue Bank   
                                                    50f,  //Value
                                                    60f,  //LDBC  
                                                    55f,  //Maturity                                                    
                                                    25f,  //Dues
                                                    85f,  //Contact Person
                                             });
            #endregion

            #region Date Wise Sub Total
            _oPdfPCell = new PdfPCell(new Phrase("Sub Total :", _oFontStyleBold));
            _oPdfPCell.Colspan = 10; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nTotalAmount), _oFontStyleBold));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Grand Total
            _oPdfPCell = new PdfPCell(new Phrase("Grand Total :", _oFontStyleBold));
            _oPdfPCell.Colspan = 10; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nGrandTotalAmount), _oFontStyleBold));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        #endregion
        #region 8 OverduePayment_Bank Wise
        private void PrintBody_OverduePayment_BankWise()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
            _oFontStyleBoldUnderLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
            _oExportOutstandingDetails = _oExportOutstandingDetails.OrderBy(c => c.BankName_Nego).ToList();
            #region Initialize Table
            PdfPTable oPdfPTable = new PdfPTable(12);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                     30f,  //SL 
                                                    60f,  //L/C No
                                                    130f,  //Bank/Party Name
                                                    85f, //PI No   
                                                    100f,  //Issue Bank   
                                                    60f,  //Issue Bank   
                                                    25f,  //Issue Bank   
                                                    50f,  //Value
                                                    60f,  //LDBC  
                                                    55f,  //Maturity                                                    
                                                    25f,  //Dues
                                                    85f,  //Contact Person
                                             });
            #endregion

            string sBankName_Nego = ""; int nCount = 0; string sCurrencySymbol = "";
            double nTotalAmount = 0, nGrandTotalAmount = 0;
            foreach (ExportOutstandingDetail oItem in _oExportOutstandingDetails)
            {
                if (sBankName_Nego != oItem.BankName_Nego)
                {
                    #region Supplier Wise Sub Total
                    if (oPdfPTable.Rows.Count > 0)
                    {
                        #region Initialize Table
                        oPdfPTable = new PdfPTable(12);
                        oPdfPTable.WidthPercentage = 100;
                        oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPTable.SetWidths(new float[] { 
                                                   30f,  //SL 
                                                    60f,  //L/C No
                                                    130f,  //Bank/Party Name
                                                    85f, //PI No   
                                                    100f,  //Issue Bank   
                                                    60f,  //Issue Bank   
                                                    25f,  //Issue Bank   
                                                    50f,  //Value
                                                    60f,  //LDBC  
                                                    55f,  //Maturity                                                    
                                                    25f,  //Dues
                                                    85f,  //Contact Person
                                             });
                        #endregion

                        #region  Sub Total
                        _oPdfPCell = new PdfPCell(new Phrase("Sub Total :", _oFontStyleBold));
                        _oPdfPCell.Colspan = 10; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nTotalAmount), _oFontStyleBold));
                        _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        oPdfPTable.CompleteRow();
                        #endregion

                        #region Insert Into Main Table
                        _oPdfPCell = new PdfPCell(oPdfPTable);
                        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();
                        #endregion

                        nTotalAmount = 0; nCount = 0;
                    }
                    #endregion

                    #region Header
                    #region Blank Row
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion

                    #region Table Initialize
                    oPdfPTable = new PdfPTable(12);
                    oPdfPTable.WidthPercentage = 100;
                    oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.SetWidths(new float[] { 
                                                     30f,  //SL 
                                                    60f,  //L/C No
                                                    130f,  //Bank/Party Name
                                                    85f, //PI No   
                                                    100f,  //Issue Bank   
                                                    60f,  //Issue Bank   
                                                    25f,  //Issue Bank   
                                                    50f,  //Value
                                                    60f,  //LDBC  
                                                    55f,  //Maturity                                                    
                                                    25f,  //Dues
                                                    85f,  //Contact Person
                                             });
                    #endregion

                    #region Supplier Heading
                    _oPdfPCell = new PdfPCell(new Phrase("Bank Name : " + oItem.BankName_Nego, _oFontStyleBoldUnderLine));
                    _oPdfPCell.Colspan = 12; _oPdfPCell.FixedHeight = 14f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    oPdfPTable.CompleteRow();
                    #endregion

                    #region Header Row
                    _oPdfPCell = new PdfPCell(new Phrase("SL No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("L/C No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Party Name", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);


                    _oPdfPCell = new PdfPCell(new Phrase("P/I No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Issue Bank", _oFontStyleBold));
                    _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Value", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("LDBC No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Maturity Date", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Dues", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    //_oPdfPCell = new PdfPCell(new Phrase("O.Due Rate", _oFontStyleBold));
                    //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);


                    _oPdfPCell = new PdfPCell(new Phrase("Concern Person", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    oPdfPTable.CompleteRow();
                    #endregion

                    #region Insert Into Main Table
                    _oPdfPCell = new PdfPCell(oPdfPTable);
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion
                    #endregion
                }


                #region Initialize Table
                oPdfPTable = new PdfPTable(12);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.SetWidths(new float[] { 
                                                     30f,  //SL 
                                                    60f,  //L/C No
                                                    130f,  //Bank/Party Name
                                                    85f, //PI No   
                                                    100f,  //Issue Bank   
                                                    60f,  //Issue Bank   
                                                    25f,  //Issue Bank   
                                                    50f,  //Value
                                                    60f,  //LDBC  
                                                    55f,  //Maturity                                                    
                                                    25f,  //Dues
                                                    85f,  //Contact Person
                                             });
                #endregion

                nCount++;
                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ExportLCNo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ContractorName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.PINo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.BankName_Issue + "[" + oItem.BBranchName_Issue + "]", _oFontStyle));
                _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase(oItem.PINo, _oFontStyle));
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.AmountSt, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.LDBCNo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.MaturityDateSt, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase(oItem.TimeLag8.ToString(), _oFontStyle));

                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase(oItem.OverDueRate.ToString(), _oFontStyle));
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.MKTPName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);



                oPdfPTable.CompleteRow();

                #region Insert Into Main Table
                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                sBankName_Nego = oItem.BankName_Nego;
                sCurrencySymbol = oItem.Currency;
                nTotalAmount = nTotalAmount + oItem.Amount;
                nGrandTotalAmount = nGrandTotalAmount + oItem.Amount;
            }

            #region Initialize Table
            oPdfPTable = new PdfPTable(12);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                    30f,  //SL 
                                                    60f,  //L/C No
                                                    130f,  //Bank/Party Name
                                                    85f, //PI No   
                                                    100f,  //Issue Bank   
                                                    60f,  //Issue Bank   
                                                    25f,  //Issue Bank   
                                                    50f,  //Value
                                                    60f,  //LDBC  
                                                    55f,  //Maturity                                                    
                                                    25f,  //Dues
                                                    85f,  //Contact Person
                                             });
            #endregion

            #region Date Wise Sub Total
            _oPdfPCell = new PdfPCell(new Phrase("Sub Total :", _oFontStyleBold));
            _oPdfPCell.Colspan = 10; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nTotalAmount), _oFontStyleBold));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Grand Total
            _oPdfPCell = new PdfPCell(new Phrase("Grand Total :", _oFontStyleBold));
            _oPdfPCell.Colspan = 10; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nGrandTotalAmount), _oFontStyleBold));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        #endregion

        #region  9 DuePayment( BAcceptance = 5,  Discounted = 7,)==7--Bill State=5,7
        private void PrintBody_DuePayment_PartyWise()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
            _oFontStyleBoldUnderLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
            _oExportOutstandingDetails = _oExportOutstandingDetails.OrderBy(c => c.ContractorName).ToList();
            #region Initialize Table
            PdfPTable oPdfPTable = new PdfPTable(12);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                     30f,  //SL 
                                                    60f,  //L/C No
                                                    130f,  //Bank/Party Name
                                                    85f, //PI No   
                                                    100f,  //Issue Bank   
                                                    60f,  //Issue Bank   
                                                    25f,  //Issue Bank   
                                                    50f,  //Value
                                                    60f,  //LDBC  
                                                    55f,  //Maturity                                                    
                                                    25f,  //Dues
                                                    //25f,  //O.Due Rate
                                                    85f,  //Contact Person
                                             });
            #endregion

            string sContractorName = ""; int nCount = 0; string sCurrencySymbol = "";
            double nTotalAmount = 0, nGrandTotalAmount = 0;
            foreach (ExportOutstandingDetail oItem in _oExportOutstandingDetails)
            {
                sCurrencySymbol = oItem.Currency;
                if (sContractorName != oItem.ContractorName)
                {
                    #region Supplier Wise Sub Total
                    if (oPdfPTable.Rows.Count > 0)
                    {
                        #region Initialize Table
                        oPdfPTable = new PdfPTable(12);
                        oPdfPTable.WidthPercentage = 100;
                        oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPTable.SetWidths(new float[] { 
                                                    30f,  //SL 
                                                    60f,  //L/C No
                                                    130f,  //Bank/Party Name
                                                    85f, //PI No   
                                                    100f,  //Issue Bank   
                                                    60f,  //Issue Bank   
                                                    25f,  //Issue Bank   
                                                    50f,  //Value
                                                    60f,  //LDBC  
                                                    55f,  //Maturity                                                    
                                                    25f,  //Dues
                                                    85f,  //Contact Person
                                             });
                        #endregion

                        #region  Sub Total
                        _oPdfPCell = new PdfPCell(new Phrase("Sub Total :", _oFontStyleBold));
                        _oPdfPCell.Colspan = 10; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nTotalAmount), _oFontStyleBold));
                        _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        oPdfPTable.CompleteRow();
                        #endregion

                        #region Insert Into Main Table
                        _oPdfPCell = new PdfPCell(oPdfPTable);
                        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();
                        #endregion

                        nTotalAmount = 0; nCount = 0;
                    }
                    #endregion

                    #region Header
                    #region Blank Row
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion

                    #region Table Initialize
                    oPdfPTable = new PdfPTable(12);
                    oPdfPTable.WidthPercentage = 100;
                    oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.SetWidths(new float[] { 
                                                       30f,  //SL 
                                                    60f,  //L/C No
                                                    130f,  //Bank/Party Name
                                                    85f, //PI No   
                                                    100f,  //Issue Bank   
                                                    60f,  //Issue Bank   
                                                    25f,  //Issue Bank   
                                                    50f,  //Value
                                                    60f,  //LDBC  
                                                    55f,  //Maturity                                                    
                                                    25f,  //Dues
                                                    85f,  //Contact Person
                                                   
                                             });
                    #endregion

                    #region Supplier Heading
                    _oPdfPCell = new PdfPCell(new Phrase("Party Name : " + oItem.ContractorName, _oFontStyleBoldUnderLine));
                    _oPdfPCell.Colspan = 12; _oPdfPCell.FixedHeight = 14f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    oPdfPTable.CompleteRow();
                    #endregion

                    #region Header Row
                    _oPdfPCell = new PdfPCell(new Phrase("SL No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("L/C No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Bank Name", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);


                    _oPdfPCell = new PdfPCell(new Phrase("P/I No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Issue Bank", _oFontStyleBold));
                    _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);


                    _oPdfPCell = new PdfPCell(new Phrase("Value", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("LDBC No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Maturity Date", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Dues", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    //_oPdfPCell = new PdfPCell(new Phrase("O.Due Rate", _oFontStyleBold));
                    //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);


                    _oPdfPCell = new PdfPCell(new Phrase("Contact Person", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);



                    oPdfPTable.CompleteRow();
                    #endregion

                    #region Insert Into Main Table
                    _oPdfPCell = new PdfPCell(oPdfPTable);
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion
                    #endregion
                }


                #region Initialize Table
                oPdfPTable = new PdfPTable(12);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.SetWidths(new float[] { 
                                                     30f,  //SL 
                                                    60f,  //L/C No
                                                    130f,  //Bank/Party Name
                                                    85f, //PI No   
                                                    100f,  //Issue Bank   
                                                    60f,  //Issue Bank   
                                                    25f,  //Issue Bank   
                                                    50f,  //Value
                                                    60f,  //LDBC  
                                                    55f,  //Maturity                                                    
                                                    25f,  //Dues
                                                    85f,  //Contact Person
                                             });
                #endregion

                nCount++;
                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ExportLCNo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.BankName_Nego, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.PINo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.BankName_Issue + "[" + oItem.BBranchName_Issue + "]", _oFontStyle));
                _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase(oItem.PINo, _oFontStyle));
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.AmountSt, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.LDBCNo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.MaturityDateSt, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase(oItem.TimeLag8.ToString(), _oFontStyle));

                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase(oItem.OverDueRate.ToString(), _oFontStyle));
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.MKTPName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


                oPdfPTable.CompleteRow();

                #region Insert Into Main Table
                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                sContractorName = oItem.ContractorName;
                sCurrencySymbol = oItem.Currency;
                nTotalAmount = nTotalAmount + oItem.Amount;
                nGrandTotalAmount = nGrandTotalAmount + oItem.Amount;
            }

            #region Initialize Table
            oPdfPTable = new PdfPTable(12);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                      30f,  //SL 
                                                    60f,  //L/C No
                                                    130f,  //Bank/Party Name
                                                    85f, //PI No   
                                                    100f,  //Issue Bank   
                                                    60f,  //Issue Bank   
                                                    25f,  //Issue Bank   
                                                    50f,  //Value
                                                    60f,  //LDBC  
                                                    55f,  //Maturity                                                    
                                                    25f,  //Dues
                                                    85f,  //Contact Person
                                             });
            #endregion

            #region Date Wise Sub Total
            _oPdfPCell = new PdfPCell(new Phrase("Sub Total :", _oFontStyleBold));
            _oPdfPCell.Colspan = 10; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nTotalAmount), _oFontStyleBold));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Grand Total
            _oPdfPCell = new PdfPCell(new Phrase("Grand Total :", _oFontStyleBold));
            _oPdfPCell.Colspan = 10; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nGrandTotalAmount), _oFontStyleBold));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        #endregion
        #region 8 DuePayment Bank Wise
        private void PrintBody_DuePayment_BankWise()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
            _oFontStyleBoldUnderLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
            _oExportOutstandingDetails = _oExportOutstandingDetails.OrderBy(c => c.BankName_Nego).ToList();
            #region Initialize Table
            PdfPTable oPdfPTable = new PdfPTable(12);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                     30f,  //SL 
                                                    60f,  //L/C No
                                                    130f,  //Bank/Party Name
                                                    85f, //PI No   
                                                    100f,  //Issue Bank   
                                                    60f,  //Issue Bank   
                                                    25f,  //Issue Bank   
                                                    50f,  //Value
                                                    60f,  //LDBC  
                                                    55f,  //Maturity                                                    
                                                    25f,  //Dues
                                                    85f,  //Contact Person
                                             });
            #endregion

            string sBankName_Nego = ""; int nCount = 0; string sCurrencySymbol = "";
            double nTotalAmount = 0, nGrandTotalAmount = 0;
            foreach (ExportOutstandingDetail oItem in _oExportOutstandingDetails)
            {
                if (sBankName_Nego != oItem.BankName_Nego)
                {
                    #region Supplier Wise Sub Total
                    if (oPdfPTable.Rows.Count > 0)
                    {
                        #region Initialize Table
                        oPdfPTable = new PdfPTable(12);
                        oPdfPTable.WidthPercentage = 100;
                        oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPTable.SetWidths(new float[] { 
                                                   30f,  //SL 
                                                    60f,  //L/C No
                                                    130f,  //Bank/Party Name
                                                    85f, //PI No   
                                                    100f,  //Issue Bank   
                                                    60f,  //Issue Bank   
                                                    25f,  //Issue Bank   
                                                    50f,  //Value
                                                    60f,  //LDBC  
                                                    55f,  //Maturity                                                    
                                                    25f,  //Dues
                                                    85f,  //Contact Person
                                             });
                        #endregion

                        #region  Sub Total
                        _oPdfPCell = new PdfPCell(new Phrase("Sub Total :", _oFontStyleBold));
                        _oPdfPCell.Colspan = 10; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nTotalAmount), _oFontStyleBold));
                        _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        oPdfPTable.CompleteRow();
                        #endregion

                        #region Insert Into Main Table
                        _oPdfPCell = new PdfPCell(oPdfPTable);
                        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();
                        #endregion

                        nTotalAmount = 0; nCount = 0;
                    }
                    #endregion

                    #region Header
                    #region Blank Row
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion

                    #region Table Initialize
                    oPdfPTable = new PdfPTable(12);
                    oPdfPTable.WidthPercentage = 100;
                    oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.SetWidths(new float[] { 
                                                     30f,  //SL 
                                                    60f,  //L/C No
                                                    130f,  //Bank/Party Name
                                                    85f, //PI No   
                                                    100f,  //Issue Bank   
                                                    60f,  //Issue Bank   
                                                    25f,  //Issue Bank   
                                                    50f,  //Value
                                                    60f,  //LDBC  
                                                    55f,  //Maturity                                                    
                                                    25f,  //Dues
                                                    85f,  //Contact Person
                                             });
                    #endregion

                    #region Supplier Heading
                    _oPdfPCell = new PdfPCell(new Phrase("Bank Name : " + oItem.BankName_Nego, _oFontStyleBoldUnderLine));
                    _oPdfPCell.Colspan = 12; _oPdfPCell.FixedHeight = 14f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    oPdfPTable.CompleteRow();
                    #endregion

                    #region Header Row
                    _oPdfPCell = new PdfPCell(new Phrase("SL No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("L/C No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Party Name", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);


                    _oPdfPCell = new PdfPCell(new Phrase("P/I No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Issue Bank", _oFontStyleBold));
                    _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Value", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("LDBC No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Maturity Date", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Dues", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    //_oPdfPCell = new PdfPCell(new Phrase("O.Due Rate", _oFontStyleBold));
                    //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);


                    _oPdfPCell = new PdfPCell(new Phrase("Concern Person", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    oPdfPTable.CompleteRow();
                    #endregion

                    #region Insert Into Main Table
                    _oPdfPCell = new PdfPCell(oPdfPTable);
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion
                    #endregion
                }


                #region Initialize Table
                oPdfPTable = new PdfPTable(12);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.SetWidths(new float[] { 
                                                     30f,  //SL 
                                                    60f,  //L/C No
                                                    130f,  //Bank/Party Name
                                                    85f, //PI No   
                                                    100f,  //Issue Bank   
                                                    60f,  //Issue Bank   
                                                    25f,  //Issue Bank   
                                                    50f,  //Value
                                                    60f,  //LDBC  
                                                    55f,  //Maturity                                                    
                                                    25f,  //Dues
                                                    85f,  //Contact Person
                                             });
                #endregion

                nCount++;
                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ExportLCNo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ContractorName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.PINo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.BankName_Issue + "[" + oItem.BBranchName_Issue + "]", _oFontStyle));
                _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase(oItem.PINo, _oFontStyle));
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.AmountSt, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.LDBCNo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.MaturityDateSt, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase(oItem.TimeLag8.ToString(), _oFontStyle));

                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase(oItem.OverDueRate.ToString(), _oFontStyle));
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.MKTPName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);



                oPdfPTable.CompleteRow();

                #region Insert Into Main Table
                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                sBankName_Nego = oItem.BankName_Nego;
                sCurrencySymbol = oItem.Currency;
                nTotalAmount = nTotalAmount + oItem.Amount;
                nGrandTotalAmount = nGrandTotalAmount + oItem.Amount;
            }

            #region Initialize Table
            oPdfPTable = new PdfPTable(12);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                    30f,  //SL 
                                                    60f,  //L/C No
                                                    130f,  //Bank/Party Name
                                                    85f, //PI No   
                                                    100f,  //Issue Bank   
                                                    60f,  //Issue Bank   
                                                    25f,  //Issue Bank   
                                                    50f,  //Value
                                                    60f,  //LDBC  
                                                    55f,  //Maturity                                                    
                                                    25f,  //Dues
                                                    85f,  //Contact Person
                                             });
            #endregion

            #region Date Wise Sub Total
            _oPdfPCell = new PdfPCell(new Phrase("Sub Total :", _oFontStyleBold));
            _oPdfPCell.Colspan = 10; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nTotalAmount), _oFontStyleBold));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Grand Total
            _oPdfPCell = new PdfPCell(new Phrase("Grand Total :", _oFontStyleBold));
            _oPdfPCell.Colspan = 10; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nGrandTotalAmount), _oFontStyleBold));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        #endregion

        #region  1 Advance Delivery party Wise
        private void PrintBody_AdvanceDelivery_PartyWise()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
            _oFontStyleBoldUnderLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
            _oExportOutstandingDetails = _oExportOutstandingDetails.OrderBy(c => c.ContractorName).ToList();
            #region Initialize Table
            PdfPTable oPdfPTable = new PdfPTable(12);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                    30f,  //SL 
                                                    85f,  //P/I No
                                                    60f,  //P/I Date
                                                    120f, //Bank Name   
                                                    55f,  //P/I Qty   
                                                    55f,  //Delivery Qty   
                                                    65f,  //Yet To Del(Qty)  
                                                    55f,  //P/I Value
                                                    55f,  //Delivery Value  
                                                    65f,  //Yet To Del(Value)                                                    
                                                    25f,  //Contact Person
                                                    85f,  //Contact Person
                                             });
            #endregion

            string sContractorName = ""; int nCount = 0; string sCurrencySymbol = "";
            double nTotalAmount = 0, nGrandTotalAmount = 0;
            double nTotalDeliveryQty = 0, nGrandTotalDeliveryQty = 0;
            double nTotalQty = 0, nGrandTotalQty = 0;
            double nTotalDeliveryValue = 0, nGrandTotalDeliveryValue = 0;
            foreach (ExportOutstandingDetail oItem in _oExportOutstandingDetails)
            {
                sCurrencySymbol = oItem.Currency;
                if (sContractorName != oItem.ContractorName)
                {
                    #region Supplier Wise Sub Total
                    if (oPdfPTable.Rows.Count > 0)
                    {
                        #region Initialize Table
                        oPdfPTable = new PdfPTable(12);
                        oPdfPTable.WidthPercentage = 100;
                        oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPTable.SetWidths(new float[] { 
                                                  30f,  //SL 
                                                    85f,  //P/I No
                                                    60f,  //P/I Date
                                                    120f, //Bank Name   
                                                    55f,  //P/I Qty   
                                                    55f,  //Delivery Qty   
                                                    65f,  //Yet To Del(Qty)  
                                                    55f,  //P/I Value
                                                    55f,  //Delivery Value  
                                                    65f,  //Yet To Del(Value)                                                    
                                                    25f,  //Contact Person
                                                    85f,  //Contact Person
                                             });
                        #endregion

                        #region  Sub Total
                        _oPdfPCell = new PdfPCell(new Phrase("Sub Total :", _oFontStyleBold));
                        _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalQty), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalDeliveryQty), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalQty - nTotalDeliveryQty), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nTotalAmount), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nTotalDeliveryValue), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nTotalAmount - nTotalDeliveryValue), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                        _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        oPdfPTable.CompleteRow();
                        #endregion

                        #region Insert Into Main Table
                        _oPdfPCell = new PdfPCell(oPdfPTable);
                        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();
                        #endregion

                        nTotalAmount = 0; nCount = 0; nTotalQty = 0; nTotalDeliveryQty = 0; nTotalDeliveryValue = 0;
                    }
                    #endregion

                    #region Header
                    #region Blank Row
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion

                    #region Table Initialize
                    oPdfPTable = new PdfPTable(12);
                    oPdfPTable.WidthPercentage = 100;
                    oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.SetWidths(new float[] { 
                                                    30f,  //SL 
                                                    85f,  //P/I No
                                                    60f,  //P/I Date
                                                    120f, //Bank Name   
                                                    55f,  //P/I Qty   
                                                    55f,  //Delivery Qty   
                                                    65f,  //Yet To Del(Qty)  
                                                    55f,  //P/I Value
                                                    55f,  //Delivery Value  
                                                    65f,  //Yet To Del(Value)                                                    
                                                    25f,  //Contact Person
                                                    85f,  //Contact Person
                                                   
                                             });
                    #endregion

                    #region Supplier Heading
                    _oPdfPCell = new PdfPCell(new Phrase("Party Name : " + oItem.ContractorName, _oFontStyleBoldUnderLine));
                    _oPdfPCell.Colspan = 12; _oPdfPCell.FixedHeight = 14f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    oPdfPTable.CompleteRow();
                    #endregion

                    #region Header Row
                    _oPdfPCell = new PdfPCell(new Phrase("SL No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("P/I No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("P/I Date", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Bank Name", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("P/I Qty", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Delivered Qty", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Due Del(Qty)", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);


                    _oPdfPCell = new PdfPCell(new Phrase("P/I Value(" + sCurrencySymbol + ")", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Delivered Value", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);


                    _oPdfPCell = new PdfPCell(new Phrase("Due Del(Value)", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);


                    _oPdfPCell = new PdfPCell(new Phrase("Contact Person", _oFontStyleBold));
                    _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);



                    oPdfPTable.CompleteRow();
                    #endregion

                    #region Insert Into Main Table
                    _oPdfPCell = new PdfPCell(oPdfPTable);
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion
                    #endregion
                }


                #region Initialize Table
                oPdfPTable = new PdfPTable(12);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.SetWidths(new float[] { 
                                                    30f,  //SL 
                                                    85f,  //P/I No
                                                    60f,  //P/I Date
                                                    120f, //Bank Name   
                                                    55f,  //P/I Qty   
                                                    55f,  //Delivery Qty   
                                                    65f,  //Yet To Del(Qty)  
                                                    55f,  //P/I Value
                                                    55f,  //Delivery Value  
                                                    65f,  //Yet To Del(Value)                                                    
                                                    25f,  //Contact Person
                                                    85f,  //Contact Person
                                             });
                #endregion

                nCount++;
                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.PINo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.LCOpeningDatest, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.BBranchName_Nego, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.QtySt, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.DeliveryQty), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty - oItem.DeliveryQty), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.AmountSt, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.DeliveryValue), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Amount - oItem.DeliveryValue), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.MKTPName, _oFontStyle));
                _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


                oPdfPTable.CompleteRow();

                #region Insert Into Main Table
                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                sContractorName = oItem.ContractorName;
                sCurrencySymbol = oItem.Currency;
                nTotalAmount = nTotalAmount + oItem.Amount;
                nGrandTotalAmount = nGrandTotalAmount + oItem.Amount;
                nTotalDeliveryValue = nTotalDeliveryValue + oItem.DeliveryValue;
                nTotalQty = nTotalQty + oItem.Qty;
                nTotalDeliveryQty = nTotalDeliveryQty + oItem.DeliveryQty;
                nGrandTotalDeliveryQty = nGrandTotalDeliveryQty + oItem.DeliveryQty;
                nGrandTotalDeliveryValue = nGrandTotalDeliveryValue + oItem.DeliveryValue;
                nGrandTotalQty = nGrandTotalQty + oItem.Qty;
            }

            #region Initialize Table
            oPdfPTable = new PdfPTable(12);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                    30f,  //SL 
                                                    85f,  //P/I No
                                                    60f,  //P/I Date
                                                    120f, //Bank Name   
                                                    55f,  //P/I Qty   
                                                    55f,  //Delivery Qty   
                                                    65f,  //Yet To Del(Qty)  
                                                    55f,  //P/I Value
                                                    55f,  //Delivery Value  
                                                    65f,  //Yet To Del(Value)                                                    
                                                    25f,  //Contact Person
                                                    85f,  //Contact Person
                                             });
            #endregion

            #region Date Wise Sub Total
            _oPdfPCell = new PdfPCell(new Phrase("Sub Total :", _oFontStyleBold));
            _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalQty), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalDeliveryQty), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalQty - nTotalDeliveryQty), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nTotalAmount), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nTotalDeliveryValue), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nTotalAmount - nTotalDeliveryValue), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Grand Total
            _oPdfPCell = new PdfPCell(new Phrase("Grand Total :", _oFontStyleBold));
            _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nGrandTotalQty), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nGrandTotalDeliveryQty), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nGrandTotalQty - nGrandTotalDeliveryQty), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nGrandTotalAmount), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nGrandTotalDeliveryValue), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nGrandTotalAmount - nGrandTotalDeliveryValue), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        #endregion
        #region  1 Advance Delivery Bank Wise
        private void PrintBody_AdvanceDelivery_BankWise()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
            _oFontStyleBoldUnderLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
            _oExportOutstandingDetails = _oExportOutstandingDetails.OrderBy(c => c.BankName_Nego).ToList();
            #region Initialize Table
            PdfPTable oPdfPTable = new PdfPTable(12);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                    30f,  //SL 
                                                    85f,  //P/I No
                                                    60f,  //P/I Date
                                                    120f, //Bank Name   
                                                    55f,  //P/I Qty   
                                                    55f,  //Delivery Qty   
                                                    65f,  //Yet To Del(Qty)  
                                                    55f,  //P/I Value
                                                    55f,  //Delivery Value  
                                                    65f,  //Yet To Del(Value)                                                    
                                                    25f,  //Contact Person
                                                    85f,  //Contact Person
                                             });
            #endregion

            string sBankName_Nego = ""; int nCount = 0; string sCurrencySymbol = "";
            double nTotalAmount = 0, nGrandTotalAmount = 0;
            double nTotalDeliveryQty = 0, nGrandTotalDeliveryQty = 0;
            double nTotalQty = 0, nGrandTotalQty = 0;
            double nTotalDeliveryValue = 0, nGrandTotalDeliveryValue = 0;
            foreach (ExportOutstandingDetail oItem in _oExportOutstandingDetails)
            {
                if (sBankName_Nego != oItem.BankName_Nego)
                {
                    #region Supplier Wise Sub Total
                    if (oPdfPTable.Rows.Count > 0)
                    {
                        #region Initialize Table
                        oPdfPTable = new PdfPTable(12);
                        oPdfPTable.WidthPercentage = 100;
                        oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPTable.SetWidths(new float[] { 
                                                 30f,  //SL 
                                                    85f,  //P/I No
                                                    60f,  //P/I Date
                                                    120f, //Bank Name   
                                                    55f,  //P/I Qty   
                                                    55f,  //Delivery Qty   
                                                    65f,  //Yet To Del(Qty)  
                                                    55f,  //P/I Value
                                                    55f,  //Delivery Value  
                                                    65f,  //Yet To Del(Value)                                                    
                                                    25f,  //Contact Person
                                                    85f,  //Contact Person
                                             });
                        #endregion

                        #region  Sub Total
                        _oPdfPCell = new PdfPCell(new Phrase("Sub Total :", _oFontStyleBold));
                        _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalQty), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalDeliveryQty), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalQty - nTotalDeliveryQty), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nTotalAmount), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nTotalDeliveryValue), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nTotalAmount - nTotalDeliveryValue), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                        _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        oPdfPTable.CompleteRow();
                        #endregion

                        #region Insert Into Main Table
                        _oPdfPCell = new PdfPCell(oPdfPTable);
                        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();
                        #endregion

                        nTotalAmount = 0; nCount = 0; nTotalQty = 0; nTotalDeliveryQty = 0; nTotalDeliveryValue = 0;
                    }
                    #endregion

                    #region Header
                    #region Blank Row
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion

                    #region Table Initialize
                    oPdfPTable = new PdfPTable(12);
                    oPdfPTable.WidthPercentage = 100;
                    oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.SetWidths(new float[] { 
                                                     30f,  //SL 
                                                    85f,  //P/I No
                                                    60f,  //P/I Date
                                                    120f, //Bank Name   
                                                    55f,  //P/I Qty   
                                                    55f,  //Delivery Qty   
                                                    65f,  //Yet To Del(Qty)  
                                                    55f,  //P/I Value
                                                    55f,  //Delivery Value  
                                                    65f,  //Yet To Del(Value)                                                    
                                                    25f,  //Contact Person
                                                    85f,  //Contact Person
                                             });
                    #endregion

                    #region Supplier Heading
                    _oPdfPCell = new PdfPCell(new Phrase("Bank Name : " + oItem.BankName_Nego, _oFontStyleBoldUnderLine));
                    _oPdfPCell.Colspan = 12; _oPdfPCell.FixedHeight = 14f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    oPdfPTable.CompleteRow();
                    #endregion

                    #region Header Row
                    _oPdfPCell = new PdfPCell(new Phrase("SL No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("P/I No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("P/I Date", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Party Name", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("P/I Qty", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Delivered Qty", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Due Del(Qty)", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);


                    _oPdfPCell = new PdfPCell(new Phrase("P/I Value(" + sCurrencySymbol + ")", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Delivered Value", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);


                    _oPdfPCell = new PdfPCell(new Phrase("Due Del(Value)", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);


                    _oPdfPCell = new PdfPCell(new Phrase("Contact Person", _oFontStyleBold));
                    _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);


                    oPdfPTable.CompleteRow();
                    #endregion

                    #region Insert Into Main Table
                    _oPdfPCell = new PdfPCell(oPdfPTable);
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion
                    #endregion
                }


                #region Initialize Table
                oPdfPTable = new PdfPTable(12);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.SetWidths(new float[] { 
                                                    30f,  //SL 
                                                    85f,  //P/I No
                                                    60f,  //P/I Date
                                                    120f, //Bank Name   
                                                    55f,  //P/I Qty   
                                                    55f,  //Delivery Qty   
                                                    65f,  //Yet To Del(Qty)  
                                                    55f,  //P/I Value
                                                    55f,  //Delivery Value  
                                                    65f,  //Yet To Del(Value)                                                    
                                                    25f,  //Contact Person
                                                    85f,  //Contact Person
                                             });
                #endregion

                nCount++;
                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.PINo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.LCOpeningDatest, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ContractorName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.QtySt, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.DeliveryQty), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty - oItem.DeliveryQty), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.AmountSt, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.DeliveryValue), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Amount - oItem.DeliveryValue), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.MKTPName, _oFontStyle));
                _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);



                oPdfPTable.CompleteRow();

                #region Insert Into Main Table
                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                sBankName_Nego = oItem.BankName_Nego;
                sCurrencySymbol = oItem.Currency;
                nTotalAmount = nTotalAmount + oItem.Amount;
                nGrandTotalAmount = nGrandTotalAmount + oItem.Amount;
                nTotalDeliveryValue = nTotalDeliveryValue + oItem.DeliveryValue;
                nTotalQty = nTotalQty + oItem.Qty;
                nTotalDeliveryQty = nTotalDeliveryQty + oItem.DeliveryQty;
                nGrandTotalDeliveryQty = nGrandTotalDeliveryQty + oItem.DeliveryQty;
                nGrandTotalDeliveryValue = nGrandTotalDeliveryValue + oItem.DeliveryValue;
                nGrandTotalQty = nGrandTotalQty + oItem.Qty;
            }

            #region Initialize Table
            oPdfPTable = new PdfPTable(12);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                   30f,  //SL 
                                                    85f,  //P/I No
                                                    60f,  //P/I Date
                                                    120f, //Bank Name   
                                                    55f,  //P/I Qty   
                                                    55f,  //Delivery Qty   
                                                    65f,  //Yet To Del(Qty)  
                                                    55f,  //P/I Value
                                                    55f,  //Delivery Value  
                                                    65f,  //Yet To Del(Value)                                                    
                                                    25f,  //Contact Person
                                                    85f,  //Contact Person
                                             });
            #endregion

            #region Date Wise Sub Total
            _oPdfPCell = new PdfPCell(new Phrase("Sub Total :", _oFontStyleBold));
            _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalQty), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalDeliveryQty), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalQty - nTotalDeliveryQty), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nTotalAmount), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nTotalDeliveryValue), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nTotalAmount - nTotalDeliveryValue), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


            oPdfPTable.CompleteRow();
            #endregion

            #region Grand Total
            _oPdfPCell = new PdfPCell(new Phrase("Grand Total :", _oFontStyleBold));
            _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nGrandTotalQty), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nGrandTotalDeliveryQty), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nGrandTotalQty - nGrandTotalDeliveryQty), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nGrandTotalAmount), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nGrandTotalDeliveryValue), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nGrandTotalAmount - nGrandTotalDeliveryValue), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();

            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        #endregion

        #region  2 PendingDelivery_PartyWise
        private void PrintBody_LCInHand_PendingDelivery_PartyWise()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
            _oFontStyleBoldUnderLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
            _oExportOutstandingDetails = _oExportOutstandingDetails.OrderBy(c => c.ContractorName).ToList();
            #region Initialize Table
            PdfPTable oPdfPTable = new PdfPTable(12);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                    30f,  //SL 
                                                    85f,  //P/I No
                                                    60f,  //L/C No
                                                    55f, //L/C Date  
                                                    120f,  //Bank Name   
                                                    55f,  //P/I Qty  
                                                    55f,  //Delivery Qty  
                                                    55f,  //Yet To Del(Qty)
                                                    55f,  //P/I Value 
                                                    55f,  //Delivery Value                                                    
                                                    55f,  //Yet To Del(Value)
                                                    85f,  //Contact Person
                                             });
            #endregion

            string sContractorName = ""; int nCount = 0; string sCurrencySymbol = "";
            double nTotalAmount = 0, nGrandTotalAmount = 0;
            double nTotalDeliveryQty = 0, nGrandTotalDeliveryQty = 0;
            double nTotalQty = 0, nGrandTotalQty = 0;
            double nTotalDeliveryValue = 0, nGrandTotalDeliveryValue = 0;
            foreach (ExportOutstandingDetail oItem in _oExportOutstandingDetails)
            {
                sCurrencySymbol = oItem.Currency;
                if (sContractorName != oItem.ContractorName)
                {
                    #region Supplier Wise Sub Total
                    if (oPdfPTable.Rows.Count > 0)
                    {
                        #region Initialize Table
                        oPdfPTable = new PdfPTable(12);
                        oPdfPTable.WidthPercentage = 100;
                        oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPTable.SetWidths(new float[] { 
                                                    30f,  //SL 
                                                    85f,  //P/I No
                                                    60f,  //L/C No
                                                    55f, //L/C Date  
                                                    120f,  //Bank Name   
                                                    55f,  //P/I Qty  
                                                    55f,  //Delivery Qty  
                                                    55f,  //Yet To Del(Qty)
                                                    55f,  //P/I Value 
                                                    55f,  //Delivery Value                                                    
                                                    55f,  //Yet To Del(Value)
                                                    85f,  //Contact Person
                                             });
                        #endregion

                        #region  Sub Total
                        _oPdfPCell = new PdfPCell(new Phrase("Sub Total :", _oFontStyleBold));
                        _oPdfPCell.Colspan = 5; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalQty), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalDeliveryQty), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalQty - nTotalDeliveryQty), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nTotalAmount), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nTotalDeliveryValue), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nTotalAmount - nTotalDeliveryValue), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        oPdfPTable.CompleteRow();
                        #endregion

                        #region Insert Into Main Table
                        _oPdfPCell = new PdfPCell(oPdfPTable);
                        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();
                        #endregion

                        nTotalAmount = 0; nCount = 0; nTotalQty = 0; nTotalDeliveryQty = 0; nTotalDeliveryValue = 0;
                    }
                    #endregion

                    #region Header
                    #region Blank Row
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion

                    #region Table Initialize
                    oPdfPTable = new PdfPTable(12);
                    oPdfPTable.WidthPercentage = 100;
                    oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.SetWidths(new float[] { 
                                                      30f,  //SL 
                                                    85f,  //P/I No
                                                    60f,  //L/C No
                                                    55f, //L/C Date  
                                                    120f,  //Bank Name   
                                                    55f,  //P/I Qty  
                                                    55f,  //Delivery Qty  
                                                    55f,  //Yet To Del(Qty)
                                                    55f,  //P/I Value 
                                                    55f,  //Delivery Value                                                    
                                                    55f,  //Yet To Del(Value)
                                                    85f,  //Contact Person
                                                   
                                             });
                    #endregion

                    #region Supplier Heading
                    _oPdfPCell = new PdfPCell(new Phrase("Party Name : " + oItem.ContractorName, _oFontStyleBoldUnderLine));
                    _oPdfPCell.Colspan = 12; _oPdfPCell.FixedHeight = 14f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    oPdfPTable.CompleteRow();
                    #endregion

                    #region Header Row
                    _oPdfPCell = new PdfPCell(new Phrase("SL No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("P/I No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("L/C No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("L/C Date", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Bank Name", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("P/I Qty", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Delivered Qty", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Due Del(Qty)", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);


                    _oPdfPCell = new PdfPCell(new Phrase("P/I Value(" + sCurrencySymbol + ")", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Delivered Value", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);


                    _oPdfPCell = new PdfPCell(new Phrase("Due Del(Value)", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);


                    _oPdfPCell = new PdfPCell(new Phrase("Contact Person", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);



                    oPdfPTable.CompleteRow();
                    #endregion

                    #region Insert Into Main Table
                    _oPdfPCell = new PdfPCell(oPdfPTable);
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion
                    #endregion
                }


                #region Initialize Table
                oPdfPTable = new PdfPTable(12);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.SetWidths(new float[] { 
                                                     30f,  //SL 
                                                    85f,  //P/I No
                                                    60f,  //L/C No
                                                    55f, //L/C Date  
                                                    120f,  //Bank Name   
                                                    55f,  //P/I Qty  
                                                    55f,  //Delivery Qty  
                                                    55f,  //Yet To Del(Qty)
                                                    55f,  //P/I Value 
                                                    55f,  //Delivery Value                                                    
                                                    55f,  //Yet To Del(Value)
                                                    85f,  //Contact Person
                                             });
                #endregion

                nCount++;
                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.PINo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ExportLCNo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.LCOpeningDatest, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.BankName_Nego, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase(oItem.QtySt, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.DeliveryQty), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty - oItem.DeliveryQty), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.AmountSt, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase(oItem.DeliveryValueSt, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Currency + "" + Global.MillionFormat(oItem.Amount - oItem.DeliveryValue), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.MKTPName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


                oPdfPTable.CompleteRow();

                #region Insert Into Main Table
                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                sContractorName = oItem.ContractorName;
                sCurrencySymbol = oItem.Currency;
                nTotalAmount = nTotalAmount + oItem.Amount;
                nGrandTotalAmount = nGrandTotalAmount + oItem.Amount;
                nTotalDeliveryValue = nTotalDeliveryValue + oItem.DeliveryValue;
                nTotalQty = nTotalQty + oItem.Qty;
                nTotalDeliveryQty = nTotalDeliveryQty + oItem.DeliveryQty;
                nGrandTotalDeliveryQty = nGrandTotalDeliveryQty + oItem.DeliveryQty;
                nGrandTotalDeliveryValue = nGrandTotalDeliveryValue + oItem.DeliveryValue;
                nGrandTotalQty = nGrandTotalQty + oItem.Qty;
            }

            #region Initialize Table
            oPdfPTable = new PdfPTable(12);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                    30f,  //SL 
                                                    85f,  //P/I No
                                                    60f,  //L/C No
                                                    55f, //L/C Date  
                                                    120f,  //Bank Name   
                                                    55f,  //P/I Qty  
                                                    55f,  //Delivery Qty  
                                                    55f,  //Yet To Del(Qty)
                                                    55f,  //P/I Value 
                                                    55f,  //Delivery Value                                                    
                                                    55f,  //Yet To Del(Value)
                                                    85f,  //Contact Person
                                             });
            #endregion

            #region Date Wise Sub Total
            _oPdfPCell = new PdfPCell(new Phrase("Sub Total :", _oFontStyleBold));
            _oPdfPCell.Colspan = 5; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalQty), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalDeliveryQty), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalQty - nTotalDeliveryQty), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nTotalAmount), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nTotalDeliveryValue), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nTotalAmount - nTotalDeliveryValue), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Grand Total
            _oPdfPCell = new PdfPCell(new Phrase("Grand Total :", _oFontStyleBold));
            _oPdfPCell.Colspan = 5; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nGrandTotalQty), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nGrandTotalDeliveryQty), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nGrandTotalQty - nGrandTotalDeliveryQty), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nGrandTotalAmount), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nGrandTotalDeliveryValue), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nGrandTotalAmount - nGrandTotalDeliveryValue), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        #endregion
        #region  2 PendingDelivery Bank Wise
        private void PrintBody_LCInHand_PendingDelivery_BankWise()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
            _oFontStyleBoldUnderLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
            _oExportOutstandingDetails = _oExportOutstandingDetails.OrderBy(c => c.BankName_Nego).ToList();
            #region Initialize Table
            PdfPTable oPdfPTable = new PdfPTable(12);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                     30f,  //SL 
                                                    85f,  //P/I No
                                                    60f,  //L/C No
                                                    55f, //L/C Date  
                                                    120f,  //Bank Name   
                                                    55f,  //P/I Qty  
                                                    55f,  //Delivery Qty  
                                                    55f,  //Yet To Del(Qty)
                                                    55f,  //P/I Value 
                                                    55f,  //Delivery Value                                                    
                                                    55f,  //Yet To Del(Value)
                                                    85f,  //Contact Person
                                             });
            #endregion

            string sBankName_Nego = ""; int nCount = 0; string sCurrencySymbol = "";
            double nTotalAmount = 0, nGrandTotalAmount = 0;
            double nTotalDeliveryQty = 0, nGrandTotalDeliveryQty = 0;
            double nTotalQty = 0, nGrandTotalQty = 0;
            double nTotalDeliveryValue = 0, nGrandTotalDeliveryValue = 0;
            foreach (ExportOutstandingDetail oItem in _oExportOutstandingDetails)
            {
                if (sBankName_Nego != oItem.BankName_Nego)
                {
                    #region Supplier Wise Sub Total
                    if (oPdfPTable.Rows.Count > 0)
                    {
                        #region Initialize Table
                        oPdfPTable = new PdfPTable(12);
                        oPdfPTable.WidthPercentage = 100;
                        oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPTable.SetWidths(new float[] { 
                                                    30f,  //SL 
                                                    85f,  //P/I No
                                                    60f,  //L/C No
                                                    55f, //L/C Date  
                                                    120f,  //Bank Name   
                                                    55f,  //P/I Qty  
                                                    55f,  //Delivery Qty  
                                                    55f,  //Yet To Del(Qty)
                                                    55f,  //P/I Value 
                                                    55f,  //Delivery Value                                                    
                                                    55f,  //Yet To Del(Value)
                                                    85f,  //Contact Person
                                             });
                        #endregion

                        #region Date Wise Sub Total
                        _oPdfPCell = new PdfPCell(new Phrase("Sub Total :", _oFontStyleBold));
                        _oPdfPCell.Colspan = 5; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalQty), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalDeliveryQty), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalQty - nTotalDeliveryQty), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nTotalAmount), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nTotalDeliveryValue), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nTotalAmount - nTotalDeliveryValue), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        oPdfPTable.CompleteRow();
                        #endregion

                        #region Insert Into Main Table
                        _oPdfPCell = new PdfPCell(oPdfPTable);
                        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();
                        #endregion

                        nTotalAmount = 0; nCount = 0; nTotalQty = 0; nTotalDeliveryQty = 0; nTotalDeliveryValue = 0;
                    }
                    #endregion

                    #region Header
                    #region Blank Row
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion

                    #region Table Initialize
                    oPdfPTable = new PdfPTable(12);
                    oPdfPTable.WidthPercentage = 100;
                    oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.SetWidths(new float[] { 
                                                       30f,  //SL 
                                                    85f,  //P/I No
                                                    60f,  //L/C No
                                                    55f, //L/C Date  
                                                    120f,  //Bank Name   
                                                    55f,  //P/I Qty  
                                                    55f,  //Delivery Qty  
                                                    55f,  //Yet To Del(Qty)
                                                    55f,  //P/I Value 
                                                    55f,  //Delivery Value                                                    
                                                    55f,  //Yet To Del(Value)
                                                    85f,  //Contact Person
                                             });
                    #endregion

                    #region Supplier Heading
                    _oPdfPCell = new PdfPCell(new Phrase("Bank Name : " + oItem.BankName_Nego, _oFontStyleBoldUnderLine));
                    _oPdfPCell.Colspan = 12; _oPdfPCell.FixedHeight = 14f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    oPdfPTable.CompleteRow();
                    #endregion

                    #region Header Row
                    _oPdfPCell = new PdfPCell(new Phrase("SL No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("P/I No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("P/I Date", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Party Name", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("P/I Qty", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Delivered Qty", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Due Del(Qty)", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);


                    _oPdfPCell = new PdfPCell(new Phrase("P/I Value(" + sCurrencySymbol + ")", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Delivered Value", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);


                    _oPdfPCell = new PdfPCell(new Phrase("Due Del(Value)", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);


                    _oPdfPCell = new PdfPCell(new Phrase("Contact Person", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);


                    oPdfPTable.CompleteRow();
                    #endregion

                    #region Insert Into Main Table
                    _oPdfPCell = new PdfPCell(oPdfPTable);
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion
                    #endregion
                }


                #region Initialize Table
                oPdfPTable = new PdfPTable(12);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.SetWidths(new float[] { 
                                                       30f,  //SL 
                                                    85f,  //P/I No
                                                    60f,  //L/C No
                                                    55f, //L/C Date  
                                                    120f,  //Bank Name   
                                                    55f,  //P/I Qty  
                                                    55f,  //Delivered Qty  
                                                    55f,  //Yet To Del(Qty)
                                                    55f,  //P/I Value 
                                                    55f,  //Delivered Value                                                    
                                                    55f,  //Yet To Del(Value)
                                                    85f,  //Contact Person
                                             });
                #endregion

                nCount++;
                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.PINo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ExportLCNo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.LCOpeningDatest, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.BankName_Nego, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase(oItem.QtySt, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.DeliveryQty), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty - oItem.DeliveryQty), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.AmountSt, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase(oItem.DeliveryValueSt, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Currency + "" + Global.MillionFormat(oItem.Amount - oItem.DeliveryValue), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.MKTPName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);



                oPdfPTable.CompleteRow();

                #region Insert Into Main Table
                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                sBankName_Nego = oItem.BankName_Nego;
                sCurrencySymbol = oItem.Currency;
                nTotalAmount = nTotalAmount + oItem.Amount;
                nGrandTotalAmount = nGrandTotalAmount + oItem.Amount;
                nTotalDeliveryValue = nTotalDeliveryValue + oItem.DeliveryValue;
                nTotalQty = nTotalQty + oItem.Qty;
                nTotalDeliveryQty = nTotalDeliveryQty + oItem.DeliveryQty;
                nGrandTotalDeliveryQty = nGrandTotalDeliveryQty + oItem.DeliveryQty;
                nGrandTotalDeliveryValue = nGrandTotalDeliveryValue + oItem.DeliveryValue;
                nGrandTotalQty = nGrandTotalQty + oItem.Qty;
            }

            #region Initialize Table
            oPdfPTable = new PdfPTable(12);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                    30f,  //SL 
                                                    85f,  //P/I No
                                                    60f,  //L/C No
                                                    55f, //L/C Date  
                                                    120f,  //Bank Name   
                                                    55f,  //P/I Qty  
                                                    55f,  //Delivery Qty  
                                                    55f,  //Yet To Del(Qty)
                                                    55f,  //P/I Value 
                                                    55f,  //Delivery Value                                                    
                                                    55f,  //Yet To Del(Value)
                                                    85f,  //Contact Person
                                             });
            #endregion

            #region Date Wise Sub Total
            _oPdfPCell = new PdfPCell(new Phrase("Sub Total :", _oFontStyleBold));
            _oPdfPCell.Colspan = 5; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalQty), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalDeliveryQty), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalQty - nTotalDeliveryQty), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nTotalAmount), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nTotalDeliveryValue), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nTotalAmount - nTotalDeliveryValue), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Grand Total
            _oPdfPCell = new PdfPCell(new Phrase("Grand Total :", _oFontStyleBold));
            _oPdfPCell.Colspan = 5; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nGrandTotalQty), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nGrandTotalDeliveryQty), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nGrandTotalQty - nGrandTotalDeliveryQty), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nGrandTotalAmount), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nGrandTotalDeliveryValue), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nGrandTotalAmount - nGrandTotalDeliveryValue), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        #endregion
    }
}
