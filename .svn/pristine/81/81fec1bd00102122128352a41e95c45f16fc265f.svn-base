using System.Text;
using System;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol;
using ICS.Core;
using System.Linq;
using ICS.Core.Utility;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;
using ESimSol.BusinessObjects.ReportingObject;

namespace ESimSol.Reports
{
    public class rptExportDocB
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyle_UnLine;
        iTextSharp.text.Font _oFontStyle_Bold_UnLine;
        iTextSharp.text.Font _oFontStyleBold;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPTable _oPdfPTable2 = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        Paragraph _oPdfParagraph;
        Phrase _oPhrase = new Phrase();
        iTextSharp.text.Image _oImag;
        PdfWriter _oWriter;
        MemoryStream _oMemoryStream = new MemoryStream();
        ExportCommercialDoc _oExportCommercialDoc;
        Company _oCompany = new Company();
        //ExportLC _oExportLC = new ExportLC();
        PdfContentByte _cb = null;
        PdfContentByte _cbTwo = null;
        //ContractorAddress _oContractorAddress = new ContractorAddress();
        ExportDocTnC _oEDTnC = new ExportDocTnC();
        List<ExportDocForwarding> _oExportDocForwardings = new List<ExportDocForwarding>();
        List<MasterLCMapping> _oMasterLCMappings = new List<MasterLCMapping>();
        ExportBill _oExportBill = new ExportBill();
        BusinessUnit _oBusinessUnit = new BusinessUnit();
        double _nTotalValue = 0;
        double _nTotalQty = 0;
        double _nTotalQtyKg = 0;
        double _nTotalNoOfBag = 0;
        double _nTotalNoOfBag_PREV = 1;
        double _nTotalWtPerBag = 0;
        double _nTWeight_Net = 0;
        double _nTWeight_Gross = 0;
        string _sMUName = "";
        string _sMUnit_Two = "";
        string _sTemp = "";
        int _nTotalRowCount = 0;
        int _nPrintType = 0;
        int _nTotalCopies = 0;
        float _nUsagesHeight = 0;
        float _nMaxHeight = 0;
        #endregion

        #region Report Header
        private void PrintHeader()
        {
            #region CompanyHeader


            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 70f, 320.5f, 70f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            if (_oCompany.CompanyLogo != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(70f, 40f);
                _oPdfPCell = new PdfPCell(_oImag);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            }
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Rowspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oFontStyle = FontFactory.GetFont("Tahoma", 20f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Name, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));

            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.PringReportHead, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #region Blank Space
            _oFontStyle = FontFactory.GetFont("Tahoma", 5f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 5f; _oPdfPCell.Colspan = 3; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            #endregion

            #region ReportHeader
            #region Blank Space
            _oFontStyle = FontFactory.GetFont("Tahoma", 5f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 5f; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion
            #endregion

            #endregion
        }
        //private void PrintHeader()
        //{
        //    #region CompanyHeader

        //    PdfPTable oPdfPTable = new PdfPTable(3);
        //    oPdfPTable.WidthPercentage = 100;
        //    oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
        //    oPdfPTable.SetWidths(new float[] { 80f, 300.5f, 80f });

        //    _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
        //    if (_oCompany.CompanyLogo != null)
        //    {
        //        _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
        //        _oImag.ScaleAbsolute(70f, 40f);
        //        //_oImag.SetAbsolutePosition(100f, 100f);
        //        _oPdfPCell = new PdfPCell(_oImag);
        //    }
        //    else
        //    {
        //        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
        //    }
        //    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.Rowspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //    _oFontStyle = FontFactory.GetFont("Tahoma", 18f, 1);
        //    _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.BUName, _oFontStyle));
        //    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //    _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 0);
        //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));

        //    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //    oPdfPTable.CompleteRow();


        //    _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 0);
        //    _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.PringReportHead, _oFontStyle));
        //    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
        //    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //    oPdfPTable.CompleteRow();




        //    #region ReportHeader
        //    #region Blank Space
        //    _oFontStyle = FontFactory.GetFont("Tahoma", 5f, 1);
        //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
        //    _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 5f; _oPdfPCell.Colspan = 3; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //    oPdfPTable.CompleteRow();

        //    #endregion
        //    #endregion

        //    _oPdfPCell = new PdfPCell(oPdfPTable);
        //    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
        //    _oPdfPTable.CompleteRow();


        //    #endregion
        //}
        private void PrintHeader_Blank()
        {
            #region ReportHeader
            _oFontStyle = FontFactory.GetFont("Tahoma", 5f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.FixedHeight = 140f; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        private void Blank(int nHeight)
        {
            #region ReportHeader
          
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.FixedHeight = nHeight; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        private void ReportHeader(string sReportHeader)
        {
            #region CompanyHeader

            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 80f, 300.5f, 80f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 15f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(sReportHeader, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion
        }
        private void ReportHeaderTwo(string sReportHeader)
        {
            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 80f, 300.5f, 80f });
           

            #region Balnk Row
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Colspan = 3; _oPdfPCell.FixedHeight = 5f; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Report Header
            _oFontStyle = FontFactory.GetFont("Tahoma", 18, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
            _oPdfPCell = new PdfPCell(new Phrase(sReportHeader, _oFontStyle));
            _oPdfPCell.Colspan = 3; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
              oPdfPTable.CompleteRow();
            #endregion


              #region Balnk Row
              _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
              _oPdfPCell.Colspan = 3; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
              oPdfPTable.CompleteRow();
              #endregion

              _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL);
              #region Date
              if (string.IsNullOrEmpty(_oExportCommercialDoc.DocDate)) { _oPdfPCell = new PdfPCell(new Phrase("Dated:", _oFontStyle)); }
              else { _oPdfPCell = new PdfPCell(new Phrase("Dated: " + _oExportCommercialDoc.DocDate, _oFontStyle)); };
              _oPdfPCell.Colspan = 2; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


              //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
              //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

              _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
              _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
              oPdfPTable.CompleteRow();
              #endregion

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        private void HeaderWithThreeFormats(string sDocHeader)
        {
            //_nPrintType = 1 means Normal Format
            //_nPrintType = 2 means PAD Format
            //_nPrintType = 3 means Full Image Title with logo
            if (_nPrintType == 1)
            {
                this.PrintHeader();
            }
            else if (_nPrintType == 2)
            {
                #region PAD Format
                _oFontStyle = FontFactory.GetFont("Tahoma", 15f, iTextSharp.text.Font.BOLD);
                _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.UNDERLINE);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; ;
                _oPdfPCell.FixedHeight = 75f; _oPdfPCell.BorderWidth = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion
            }
            else if (_nPrintType == 3)
            {
                #region Image Format (Title with LOGO)
                if (_oCompany.CompanyTitle != null)
                {
                    _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyTitle, System.Drawing.Imaging.ImageFormat.Jpeg);
                    _oImag.ScaleAbsolute(530f, 100f);
                    _oPdfPCell = new PdfPCell(_oImag);
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.FixedHeight = 100f;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                    _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                }
                else
                {
                    this.PrintHeader_Blank();
                }
                #endregion
            }

            //_oFontStyle = FontFactory.GetFont("Tahoma", 14f, 1);
            //_oPdfPCell = new PdfPCell(new Phrase(sDocHeader, _oFontStyle));
            //_oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();
        }
        private void ReportHeaderBOE(string sReportHeader)
        {
            #region CompanyHeader

            PdfPTable oPdfPTable = new PdfPTable(1);
            oPdfPTable.WidthPercentage = 0;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 560.5f });


            _oFontStyle = FontFactory.GetFont("Tahoma", 16f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(sReportHeader, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();




            #endregion
        }
        #endregion

        #region Beneficiary Certificate
        public byte[] PrepareReport_BeneficiaryCertificate(ExportCommercialDoc oExportCommercialDoc, Company oCompany, int nPrintType, string sMUName, BusinessUnit oBusinessUnit, int nPageSize)
        {

            _oExportCommercialDoc = oExportCommercialDoc;
            _oCompany = oCompany;
            _nPrintType = nPrintType;
            _oBusinessUnit = oBusinessUnit;

            _sMUName = sMUName;
            #region Page Setup            
            _oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
            if (nPageSize == 1)
            {
                //nPageSize means "A4"
                _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595                
            }
            else
            {
                //nPageSize 2 means "Legal"
                _oDocument.SetPageSize(iTextSharp.text.PageSize.LEGAL);
            }
            _oDocument.SetMargins(40f, 25f, 30f, 30f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            //_oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PageEventHandler.PrintDocumentGenerator = false;
            PageEventHandler.PrintPrintingDateTime = false;
            _oWriter.PageEvent = PageEventHandler; //Footer print wiht page event handeler            
            _oDocument.Open();

            _oPdfPTable.SetWidths(new float[] { 595f });
            #endregion

            this.HeaderWithThreeFormats(_oExportCommercialDoc.DocHeader);
            this.ReportHeaderTwo(_oExportCommercialDoc.DocHeader);
            this.PrintBody_BeneficiaryCertificate();
            _oPdfPTable.HeaderRows = ((_oExportCommercialDoc.IsPrintHeader) ? 3 : 1);

            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Body
        private void PrintBody_BeneficiaryCertificate()
        {
            float nBlankSpace = 7f;
            if (_oBusinessUnit.BusinessUnitType == EnumBusinessUnitType.Dyeing)
            {
                if (_oExportCommercialDoc.ExportBillDetails.Count > 5 && _oExportCommercialDoc.ExportBillDetails.Count <= 10)
                {
                    nBlankSpace = 4f;
                }
                else if (_oExportCommercialDoc.ExportBillDetails.Count > 10)
                {
                    nBlankSpace = 3f;
                }
            }
            else
            {
                nBlankSpace = 2f;
            }



            #region 1st Row (ClauseOne)
            PdfPTable oPdfPTable = new PdfPTable(1);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 560f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.UNDERLINE);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            oPdfPTable = new PdfPTable(3);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 180f, 10f, 370f });

            #region Blank space
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.FixedHeight = 5f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.FixedHeight = 5f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.FixedHeight = 5f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region 3rd Row
            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.AccountOf, _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.ApplicantName, _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.ApplicantAddress, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPhrase = new Phrase();
            if (!String.IsNullOrEmpty(_oExportCommercialDoc.FactoryAddress))
            {
            
                _oPhrase.Add(new Chunk("  Factory : ", _oFontStyle));
            
                _oPhrase.Add(new Chunk(_oExportCommercialDoc.FactoryAddress, _oFontStyle));
            }

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(_oPhrase);
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            if (!string.IsNullOrEmpty(_oExportCommercialDoc.Beneficiary) && !string.IsNullOrEmpty(_oExportCommercialDoc.BUName))
            {
                #region Blank space
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.FixedHeight = 15f; _oPdfPCell.Colspan = 3; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
                #endregion

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.Beneficiary, _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.BUName, _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }


            if (!string.IsNullOrEmpty(_oExportCommercialDoc.DeliveryTo) && !string.IsNullOrEmpty(_oExportCommercialDoc.DeliveryToName))
            {
                #region Blank space
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.FixedHeight = 15f; _oPdfPCell.Colspan = 3; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
                #endregion

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.DeliveryTo, _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPhrase = new Phrase();
                _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
                _oPhrase.Add(new Chunk(_oExportCommercialDoc.DeliveryToName, _oFontStyle));
                _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
                _oPhrase.Add(new Chunk(", " + _oExportCommercialDoc.DeliveryToAddress, _oFontStyle));
                if (!string.IsNullOrEmpty(_oExportCommercialDoc.FactoryAddress))
                {
                    _oPhrase.Add(new Chunk(", Factory : ", _oFontStyleBold));
                    _oPhrase.Add(new Chunk(_oExportCommercialDoc.FactoryAddress, _oFontStyle));
                }
                _oPdfPCell = new PdfPCell(_oPhrase);
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            #endregion

            #region 4th Row
            #region Blank space
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.FixedHeight = nBlankSpace; _oPdfPCell.Colspan = 3; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.DocumentaryCreditNoDate, _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.ExportLCNoAndDate + " " + _oExportCommercialDoc.AmendmentNonDate, _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region 5th Row (Invoice No)
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.NoAndDateOfDoc))
            {
                #region Blank space
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.FixedHeight = nBlankSpace; _oPdfPCell.Colspan = 3; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
                #endregion

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.NoAndDateOfDoc, _oFontStyleBold)); //NoAndDateOfDoc == "Invoice No"
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                if (_oExportCommercialDoc.IsPrintInvoiceDate == false && _oExportCommercialDoc.DocDate!="")
                { _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.ExportBillNo_FullDate + " " + _oExportCommercialDoc.DocDate, _oFontStyleBold)); }
                else { _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.ExportBillNo_FullDate, _oFontStyleBold)); }
             
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            #endregion

            #region 6th Row (Issuing Bank)
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.IssuingBank))
            {
                #region Blank space
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.FixedHeight = nBlankSpace; _oPdfPCell.Colspan = 3; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
                #endregion

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.IssuingBank, _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.BankName_Issue, _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.BBranchName_Issue, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.BankAddress_Issue, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }

            #endregion

            #region 7th Row (Negotiation Bank)
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.NegotiatingBank))
            {
                #region Blank space
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.FixedHeight = nBlankSpace; _oPdfPCell.Colspan = 3; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
                #endregion

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.NegotiatingBank, _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.BankName_Nego, _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.BBranchName_Nego, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.BankAddress_Nego, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            #endregion

            #region 8th Row (Export LC / Master LC)
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.AllMasterLCNo))
            {
                #region Blank space
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.FixedHeight = nBlankSpace; _oPdfPCell.Colspan = 3; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
                #endregion

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.AgainstExportLC, _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.AllMasterLCNo, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            #endregion

            #region Party Info// Vat Reg
            List<ExportPartyInfoBill> oExportPartyInfoBills = new List<ExportPartyInfoBill>();
            if (_oExportCommercialDoc.ExportBillDocID > 0)
            { oExportPartyInfoBills = ExportPartyInfoBill.GetsByExportLC(_oExportCommercialDoc.ExportBillDocID, (int)EnumMasterLCType.CommercialDoc, 0); }
            else
            {
                oExportPartyInfoBills = ExportPartyInfoBill.GetsByExportLC(_oExportCommercialDoc.ExportLCID, (int)EnumMasterLCType.ExportLC, 0);
            }
        
            if (!String.IsNullOrEmpty(_oExportCommercialDoc.HSCode_Head) && !String.IsNullOrEmpty(_oExportCommercialDoc.HSCode))
            {
                ExportPartyInfoBill oExportPartyInfoBill = new ExportPartyInfoBill();
                oExportPartyInfoBill.PartyInfo = _oExportCommercialDoc.HSCode_Head;
                oExportPartyInfoBill.RefNo = _oExportCommercialDoc.HSCode;
                oExportPartyInfoBills.Add(oExportPartyInfoBill);
            }
            if (oExportPartyInfoBills.Count > 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(this.ExportPartyInfo(false, oExportPartyInfoBills));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            #endregion

            #region 8th Row (ProformaInvoiceNoAndDate)
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.ProformaInvoiceNoAndDate))
            {
                #region Blank space
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.FixedHeight = nBlankSpace; _oPdfPCell.Colspan = 3; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
                #endregion

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.ProformaInvoiceNoAndDate, _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.PINos, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }


            #endregion

            #region GarmentsQty_Head
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.GarmentsQty_Head) && !string.IsNullOrEmpty(_oExportCommercialDoc.GarmentsQty))
            {
                #region Blank space
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.FixedHeight = nBlankSpace; _oPdfPCell.Colspan = 3; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
                #endregion

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.GarmentsQty_Head, _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.GarmentsQty, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }


            #endregion

            #region CountryofOrigin
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.CountryofOrigin))
            {
                #region Blank space
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.FixedHeight = nBlankSpace; _oPdfPCell.Colspan = 3; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
                #endregion

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.CountryofOrigin, _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.CountryofOriginName, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            #endregion

            #region Carrier
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.Carrier))
            {
                #region Blank space
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.FixedHeight = nBlankSpace; _oPdfPCell.Colspan = 3; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
                #endregion

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.Carrier, _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.CarrierName, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            #endregion

            #region Truck No
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.TruckNo_Print))
            {
                #region Blank space
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.FixedHeight = nBlankSpace; _oPdfPCell.Colspan = 3; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
                #endregion

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.TruckNo_Print, _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.TruckNo, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            #endregion

            #region DriverName
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.DriverName_Print) && !string.IsNullOrEmpty(_oExportCommercialDoc.DriverName))
            {
                #region Blank space
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.FixedHeight = nBlankSpace; _oPdfPCell.Colspan = 3; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
                #endregion

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.DriverName_Print, _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.DriverName, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            #endregion

            #region Delivery Date/SellingOnAbout
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.SellingOnAbout))
            {
                #region Blank space
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.FixedHeight = 25f; _oPdfPCell.Colspan = 3; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
                #endregion

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.SellingOnAbout, _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            #endregion

            #region Blank space
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.FixedHeight = 5f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.FixedHeight = 5f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.FixedHeight = 5f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion
                       
            #region Blank space
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.FixedHeight = nBlankSpace; _oPdfPCell.Colspan = 3; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Blank space
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.FixedHeight = nBlankSpace; _oPdfPCell.Colspan = 3; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region insewrt into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region 1st Row (ClauseOne)
            oPdfPTable = new PdfPTable(1);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 560f });

          

            if (!string.IsNullOrEmpty(_oExportCommercialDoc.ClauseOne))
            {
                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.ClauseOne + ":", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion

            #region 2nd Row (Product Detail Table)
            if ((_oExportCommercialDoc.BusinessUnitType == (int)EnumBusinessUnitType.Integrated || _oExportCommercialDoc.BusinessUnitType == (int)EnumBusinessUnitType.Plastic) && _oExportCommercialDoc.ProductNature == EnumProductNature.Hanger)
            {
                
                #region Hanger
                if (_oExportCommercialDoc.IsPrintUnitPrice)
                {
                    this.HangerProductWithPrice();
                }
                else
                {   
                    _oPdfPCell = new PdfPCell(this.HangerProductWithOutPrice());
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                }                
                #endregion
            }
            else if ((_oExportCommercialDoc.BusinessUnitType == (int)EnumBusinessUnitType.Integrated || _oExportCommercialDoc.BusinessUnitType == (int)EnumBusinessUnitType.Plastic) && _oExportCommercialDoc.ProductNature == EnumProductNature.Poly)
            {
                #region Poly
                if (_oExportCommercialDoc.IsPrintUnitPrice)
                {
                    _oPdfPCell = new PdfPCell(this.PloyProductWithPrice());
                }
                else
                {
                    _oPdfPCell = new PdfPCell(this.PloyProductWithPrice());
                }
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion
            }
            else if (_oExportCommercialDoc.BusinessUnitType == (int)EnumBusinessUnitType.Dyeing  && _oExportCommercialDoc.ProductNature == EnumProductNature.Dyeing)
            {
                //#region Dyeing
                //if (_oExportCommercialDoc.IsPrintValue)
                //{
                //    _oPdfPCell = new PdfPCell(this.ProductDetails_Dyeing_Amount());
                //}
                //else if (_oExportCommercialDoc.IsPrintUnitPrice)
                //{
                //    _oPdfPCell = new PdfPCell(this.ProductDetails_Dyeing_UnitePrice());
                //}
                //else
                //{
                //    _oPdfPCell = new PdfPCell(this.ProductDetails_Dyeing_Qty());
                //}
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                //_oPdfPTable.CompleteRow();
                //#endregion

                if (_oExportCommercialDoc.GoodsDesViewType == EnumExportGoodsDesViewType.Qty_Gross_NetWeight)
                {
                    _oPdfPCell = new PdfPCell(this.ProductDetails_Dyeing_NET_GROSSWEIGHT());
                }
                else if (_oExportCommercialDoc.GoodsDesViewType == EnumExportGoodsDesViewType.Qty_UP_Ampunt)
                {
                    _oPdfPCell = new PdfPCell(this.ProductDetails_Dyeing_UnitePrice());
                }
                else if (_oExportCommercialDoc.GoodsDesViewType == EnumExportGoodsDesViewType.Qty_UP_Ampunt_Bag)
                {
                    _oPdfPCell = new PdfPCell(this.ProductDetails_Dyeing_UnitePrice());
                }
                else if (_oExportCommercialDoc.GoodsDesViewType == EnumExportGoodsDesViewType.Qty_Amount)
                {
                    _oPdfPCell = new PdfPCell(this.ProductDetails_Dyeing_Amount());
                }
                else if (_oExportCommercialDoc.GoodsDesViewType == EnumExportGoodsDesViewType.Qty)
                {
                    _oPdfPCell = new PdfPCell(this.ProductDetails_Dyeing_Qty());
                }
                else
                {
                    _oPdfPCell = new PdfPCell(this.ProductDetails_Dyeing_Qty());
                }
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            else if (_oExportCommercialDoc.BusinessUnitType == (int)EnumBusinessUnitType.Weaving || _oExportCommercialDoc.BusinessUnitType == (int)EnumBusinessUnitType.Finishing)
            {
                #region Weaving Finishing
                if (_oExportCommercialDoc.GoodsDesViewType == EnumExportGoodsDesViewType.Qty_Gross_NetWeight)
                {
                    _oPdfPCell = new PdfPCell(this.ProductDetails_WU_Qty_GrossNet());
                }
                else if (_oExportCommercialDoc.GoodsDesViewType == EnumExportGoodsDesViewType.Qty_UP_Ampunt)
                {
                    _oPdfPCell = new PdfPCell(this.ProductDetails_WU(true));
                }
                else if (_oExportCommercialDoc.GoodsDesViewType == EnumExportGoodsDesViewType.Qty_UP_Ampunt_Bag)
                {
                    _oPdfPCell = new PdfPCell(this.ProductDetails_WU(true));
                }
                else if (_oExportCommercialDoc.GoodsDesViewType == EnumExportGoodsDesViewType.Qty_Amount)
                {
                    _oPdfPCell = new PdfPCell(this.ProductDetails_WU(false));
                }
                else if (_oExportCommercialDoc.GoodsDesViewType == EnumExportGoodsDesViewType.Qty)
                {
                    _oPdfPCell = new PdfPCell(this.ProductDetails_WU_Qty());
                }
                else
                {
                    _oPdfPCell = new PdfPCell(this.ProductDetails_WU_Qty());
                }
                //if (_oExportCommercialDoc.IsPrintUnitPrice)
                //{
                //    _oPdfPCell = new PdfPCell(this.ProductDetails_WU(true));
                //}
                //else
                //{
                //    _oPdfPCell = new PdfPCell(this.ProductDetails_WU_Qty());
                //}
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion
            }      

               
            #endregion


            if (_oExportCommercialDoc.IsPrintGrossNetWeight)
            {
                if (_oExportCommercialDoc.BusinessUnitType == (int)EnumBusinessUnitType.Weaving || _oExportCommercialDoc.BusinessUnitType == (int)EnumBusinessUnitType.Finishing)
                {
                    _oPdfPCell = new PdfPCell(this.ShowNetWeight_WU());
                }
                else
                {
                    _oPdfPCell = new PdfPCell(this.ShowNetWeight_DU());
                }
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }

            #region 10th Row (Proforma Invoice)

            oPdfPTable = new PdfPTable(2);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 180f, 380f });
            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.ProformaInvoiceNoAndDate, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            #region Blank space
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.FixedHeight = 5f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.FixedHeight = 5f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            _oPdfPCell = new PdfPCell(new Phrase((!string.IsNullOrEmpty(_oExportCommercialDoc.PINos) ? _oExportCommercialDoc.PINos : ""), _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            #endregion

         

            #region Blank space
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 5f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

         
            #region 13th Row (Certification)
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.Certification))
            {
                _oPdfParagraph = new Paragraph(new Phrase(_oExportCommercialDoc.Certification, _oFontStyle));
                _oPdfParagraph.SetLeading(0f, 1.20f);
                _oPdfParagraph.Alignment = Element.ALIGN_JUSTIFIED;
                _oPdfPCell = new PdfPCell();
                _oPdfPCell.AddElement(_oPdfParagraph);
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion
            
            #region Blank space
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.FixedHeight = 5f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.FixedHeight = 5f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.FixedHeight = 5f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region 11th Row (ClauseTwo)

            oPdfPTable = new PdfPTable(1);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 560f });


            _oExportCommercialDoc.ClauseTwo = _oExportCommercialDoc.ClauseTwo.Replace("@APPLICANT", _oExportCommercialDoc.ApplicantName + "," + _oExportCommercialDoc.ApplicantAddress);
            _oExportCommercialDoc.ClauseTwo = _oExportCommercialDoc.ClauseTwo.Replace("@PINO", " " + _oExportCommercialDoc.PINos);
            _oExportCommercialDoc.ClauseTwo = _oExportCommercialDoc.ClauseTwo.Replace("@INVOICENO", " " + _oExportCommercialDoc.ExportBillNo);

            _oPdfParagraph = new Paragraph(new Phrase((!string.IsNullOrEmpty(_oExportCommercialDoc.ClauseTwo) ? _oExportCommercialDoc.ClauseTwo : ""), _oFontStyle));
            _oPdfParagraph.SetLeading(0f, 1.2f);
            _oPdfParagraph.Alignment = Element.ALIGN_JUSTIFIED;
            _oPdfPCell = new PdfPCell();
            _oPdfPCell.AddElement(_oPdfParagraph);
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); //_oExportCommercialDoc.PINos
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion
            #region 12th Row (ClauseThree)
            _oExportCommercialDoc.ClauseThree = _oExportCommercialDoc.ClauseThree.Replace("@PINO", " " + _oExportCommercialDoc.PINos);
            _oExportCommercialDoc.ClauseThree = _oExportCommercialDoc.ClauseThree.Replace("@INVOICENO", " " + _oExportCommercialDoc.ExportBillNo_Full);

            _oPdfParagraph = new Paragraph(new Phrase(_oExportCommercialDoc.ClauseThree, _oFontStyle));
            _oPdfParagraph.SetLeading(0f, 1.20f);
            _oPdfParagraph.Alignment = Element.ALIGN_JUSTIFIED;
            _oPdfPCell = new PdfPCell();
            _oPdfPCell.AddElement(_oPdfParagraph);
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            #region Blank space
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = nBlankSpace; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            #region 13th Row (ClauseFour)
            _oPdfParagraph = new Paragraph(new Phrase(_oExportCommercialDoc.ClauseFour, _oFontStyle));
            _oPdfParagraph.SetLeading(0f, 1.20f);
            _oPdfParagraph.Alignment = Element.ALIGN_JUSTIFIED;
            _oPdfPCell = new PdfPCell();
            _oPdfPCell.AddElement(_oPdfParagraph);
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Blank space
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region 14th Row (Signature Part)
            oPdfPTable = new PdfPTable(2);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 280f, 280f });


          

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.To, _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.For, _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

         
            #region Blank space
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Colspan = 2; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.FixedHeight = 40f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.Colspan = 2; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            if (String.IsNullOrEmpty(_oExportCommercialDoc.ReceiverSignature))
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            }
            else
            {
            _oPdfPCell = new PdfPCell(new Phrase("___________________", _oFontStyleBold));
            }
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            if (String.IsNullOrEmpty(_oExportCommercialDoc.AuthorisedSignature))
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("___________________", _oFontStyleBold));
            }
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.ReceiverSignature, _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.AuthorisedSignature, _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }

        #endregion
        #endregion

        #region Delivery Challan
        public byte[] PrepareReport_DeliveryChallan(ExportCommercialDoc oExportCommercialDoc, Company oCompany, int nPrintType, string sMUName, BusinessUnit oBusinessUnit, int nPageSize)
        {

            _oExportCommercialDoc = oExportCommercialDoc;
            _oCompany = oCompany;
            _nPrintType = nPrintType;
            _oBusinessUnit = oBusinessUnit;
            _nMaxHeight = 450;
            _sMUName = sMUName;
            #region Page Setup
            _oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
            if (nPageSize == 1)
            {
                //nPageSize means "A4"
                _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595                
            }
            else
            {
                //nPageSize 2 means "Legal"
                _oDocument.SetPageSize(iTextSharp.text.PageSize.LEGAL);
            }
            _oDocument.SetMargins(30f, 30f, 30f, 30f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            //_oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PageEventHandler.PrintDocumentGenerator = false;
            PageEventHandler.PrintPrintingDateTime = false;
            _oWriter.PageEvent = PageEventHandler; //Footer print wiht page event handeler            
            _oDocument.Open();

            _oPdfPTable.SetWidths(new float[] { 595f });
            #endregion

            this.HeaderWithThreeFormats(_oExportCommercialDoc.DocHeader);
            this.ReportHeader(_oExportCommercialDoc.DocHeader);
            this.PrintBodyDeliveryChallan_B();
            _oPdfPTable.HeaderRows = ((_oExportCommercialDoc.IsPrintHeader) ? 2 : 1);

            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Body
    
        private void PrintBodyDeliveryChallan_B()
        {
            #region 1st and 2nd Row ,4th, 5th, 6th And 7th Row
            PdfPTable oPdfPTable = new PdfPTable(2);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 280f, 280f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.UNDERLINE);
            _oFontStyle_Bold_UnLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);

            #region ShipmentTerm and Master LC

            _oPdfPCell = new PdfPCell(this.SetDeliveryChallan_B_Left());
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            if (!String.IsNullOrEmpty(_oExportCommercialDoc.DocumentaryCreditNoDate))
            {
                _oPdfPCell = new PdfPCell(this.SetDeliveryChallan_B_Right());
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            }
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #endregion

         
            #region Port and Delivery Info
             oPdfPTable = new PdfPTable(2);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 280f, 280f });
         
            #region ShipmentTerm 

            _oPdfPCell = new PdfPCell(this.SetShipmentTerm_DC_Left());
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            if (!String.IsNullOrEmpty(_oExportCommercialDoc.FinalDestination))
            {
                _oPdfPCell = new PdfPCell(this.SetShipmentTerm_DC_Right());
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            }
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #endregion

            #region Black Space
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region 8th Row (Product Detail Table)
            if ((_oExportCommercialDoc.BusinessUnitType == (int)EnumBusinessUnitType.Integrated || _oExportCommercialDoc.BusinessUnitType == (int)EnumBusinessUnitType.Plastic) && _oExportCommercialDoc.ProductNature == EnumProductNature.Hanger)
            {
                #region Hanger
                if (_oExportCommercialDoc.IsPrintUnitPrice)
                {
                    this.HangerProductWithPrice();
                }
                else
                {
                    _oPdfPCell = new PdfPCell(this.HangerProductWithOutPrice());
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                }
                #endregion
            }
            else if ((_oExportCommercialDoc.BusinessUnitType == (int)EnumBusinessUnitType.Integrated || _oExportCommercialDoc.BusinessUnitType == (int)EnumBusinessUnitType.Plastic) && _oExportCommercialDoc.ProductNature == EnumProductNature.Poly)
            {
                #region Poly
                if (_oExportCommercialDoc.IsPrintUnitPrice)
                {
                    _oPdfPCell = new PdfPCell(this.PloyProductWithPrice());
                }
                else
                {
                    _oPdfPCell = new PdfPCell(this.PloyProductWithPrice());
                }
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion
            }
            else if (_oExportCommercialDoc.BusinessUnitType == (int)EnumBusinessUnitType.Dyeing && _oExportCommercialDoc.ProductNature == EnumProductNature.Dyeing)
            {
                #region Dyeing
                //if (_oExportCommercialDoc.IsPrintUnitPrice)
                //{
                //    _oPdfPCell = new PdfPCell(this.ProductDetails_Dyeing_UnitePrice());
                //}
                //else
                //{
                //    _oPdfPCell = new PdfPCell(this.ProductDetails_Dyeing_Qty());
                //}
                if (_oExportCommercialDoc.GoodsDesViewType == EnumExportGoodsDesViewType.Qty_Gross_NetWeight)
                {
                    _oPdfPCell = new PdfPCell(this.ProductDetails_Dyeing_NET_GROSSWEIGHT());
                }
                else if (_oExportCommercialDoc.GoodsDesViewType == EnumExportGoodsDesViewType.Qty_UP_Ampunt)
                {
                    _oPdfPCell = new PdfPCell(this.ProductDetails_Dyeing_UnitePrice());
                }
                else if (_oExportCommercialDoc.GoodsDesViewType == EnumExportGoodsDesViewType.Qty_Amount)
                {
                    _oPdfPCell = new PdfPCell(this.ProductDetails_Dyeing_Qty());
                }
                else if (_oExportCommercialDoc.GoodsDesViewType == EnumExportGoodsDesViewType.Qty)
                {
                    _oPdfPCell = new PdfPCell(this.ProductDetails_Dyeing_Qty());
                }
                else
                {
                    _oPdfPCell = new PdfPCell(this.ProductDetails_Dyeing_Qty());
                }


                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion
            }
            else if (_oExportCommercialDoc.BusinessUnitType == (int)EnumBusinessUnitType.Weaving || _oExportCommercialDoc.BusinessUnitType == (int)EnumBusinessUnitType.Finishing)
            {
                #region Weaving
                if (_oExportCommercialDoc.GoodsDesViewType == EnumExportGoodsDesViewType.Qty_Gross_NetWeight)
                {
                    _oPdfPCell = new PdfPCell(this.ProductDetails_WU_Qty_GrossNet());
                }
                else if (_oExportCommercialDoc.GoodsDesViewType == EnumExportGoodsDesViewType.Qty_UP_Ampunt)
                {
                    _oPdfPCell = new PdfPCell(this.ProductDetails_WU(true));
                }
                else if (_oExportCommercialDoc.GoodsDesViewType == EnumExportGoodsDesViewType.Qty_Amount)
                {
                    _oPdfPCell = new PdfPCell(this.ProductDetails_WU(false));
                }
                else if (_oExportCommercialDoc.GoodsDesViewType == EnumExportGoodsDesViewType.Qty)
                {
                    _oPdfPCell = new PdfPCell(this.ProductDetails_WU_Qty());
                }
                else
                {
                    _oPdfPCell = new PdfPCell(this.ProductDetails_WU_Qty());
                }
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion
            }
            #endregion

            #region Print Gross and Net Weight 
            if (_oExportCommercialDoc.IsPrintGrossNetWeight)
            {
                if (_oExportCommercialDoc.BusinessUnitType == (int)EnumBusinessUnitType.Weaving || _oExportCommercialDoc.BusinessUnitType == (int)EnumBusinessUnitType.Finishing)
                {
                    _oPdfPCell = new PdfPCell(this.ShowNetWeight_WU());
                }
                else if (_oExportCommercialDoc.BusinessUnitType == (int)EnumBusinessUnitType.Dyeing)
                {
                    _oPdfPCell = new PdfPCell(this.ShowNetWeight_DU());
                }
                else
                {
                    _oPdfPCell = new PdfPCell(this.ShowNetWeight_DU());
                }
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion

            #region 10th Row (FREIGHT PREPAID/ORGINAM//DELIVERY)

            oPdfPTable = new PdfPTable(1);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 560f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, 1);

            if (_oExportCommercialDoc.FrightPrepaid != "" && _oExportCommercialDoc.FrightPrepaid != "N/A")
            {
                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.FrightPrepaid, FontFactory.GetFont("Tahoma", 12f, 1, BaseColor.GRAY)));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }

            if (_oExportCommercialDoc.Orginal != "" && _oExportCommercialDoc.Orginal != "N/A")
            {
                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.Orginal, FontFactory.GetFont("Tahoma", 12f, 1, BaseColor.GRAY)));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 1; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            if (_oExportCommercialDoc.DeliveryBy != "" && _oExportCommercialDoc.DeliveryBy != "N/A")
            {
                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.DeliveryBy, FontFactory.GetFont("Tahoma", 12f, 1, BaseColor.GRAY)));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

            #region Blank space
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.FixedHeight = 5f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region 9th Row


            oPdfPTable = new PdfPTable(2);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 10f, 550f });

          

            #region Balank
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.FixedHeight = 5f; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.FixedHeight = 5f; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Clauses
            //if (_oExportCommercialDoc.Wecertifythat != "" && _oExportCommercialDoc.Wecertifythat != "N/A")
            //{
            if (_oExportCommercialDoc.Certification != "" && _oExportCommercialDoc.Certification != "N/A")
            {
                _oExportCommercialDoc.Certification = _oExportCommercialDoc.Certification.Replace("@PINO", " " + _oExportCommercialDoc.PINos);
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.Certification, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            if (_oExportCommercialDoc.ClauseOne != "" && _oExportCommercialDoc.ClauseOne != "N/A")
            {
                _oExportCommercialDoc.ClauseOne = _oExportCommercialDoc.ClauseOne.Replace("@PINO", " " + _oExportCommercialDoc.PINos);
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.ClauseOne, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            if (_oExportCommercialDoc.ClauseTwo != "" && _oExportCommercialDoc.ClauseTwo != "N/A")
            {
                _oExportCommercialDoc.ClauseTwo = _oExportCommercialDoc.ClauseTwo.Replace("@PINO", " " + _oExportCommercialDoc.PINos);
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.ClauseTwo, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            if (_oExportCommercialDoc.ClauseThree != "" && _oExportCommercialDoc.ClauseThree != "N/A")
            {
                _oExportCommercialDoc.ClauseThree = _oExportCommercialDoc.ClauseThree.Replace("@PINO", " " + _oExportCommercialDoc.PINos);
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.ClauseThree, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

            #region Gross Net Weight
            //if (_oExportCommercialDoc.IsPrintGrossNetWeight)
            //{
            //    if (_oExportCommercialDoc.BusinessUnitType == (int)EnumBusinessUnitType.Weaving || _oExportCommercialDoc.BusinessUnitType == (int)EnumBusinessUnitType.Finishing)
            //    {
            //        _oPdfPCell = new PdfPCell(this.ShowNetWeight_WU());
            //    }
            //    else
            //    {
            //        _oPdfPCell = new PdfPCell(this.ShowNetWeight_DU());
            //    }
            //    _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //    _oPdfPTable.CompleteRow();
            //}
            //if (_oExportCommercialDoc.IsPrintGrossNetWeight)
            //{
            //    if (_oExportCommercialDoc.BusinessUnitType == (int)EnumBusinessUnitType.Weaving || _oExportCommercialDoc.BusinessUnitType == (int)EnumBusinessUnitType.Finishing)
            //    {
            //        _oPdfPCell = new PdfPCell(this.ShowNetWeight_WU());
            //    }
            //    else
            //    {
            //        _oPdfPCell = new PdfPCell(this.ShowNetWeight_DU());
            //    }
            //    _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //    _oPdfPTable.CompleteRow();
            //}
            #endregion


            //#region Certification
            //if (_oExportCommercialDoc.Certification != "" && _oExportCommercialDoc.Certification != "N/A")
            //{
            //    _oPdfPCell = new PdfPCell(new Phrase("Certification:", _oFontStyleBold));
            //    _oExportCommercialDoc.Certification = _oExportCommercialDoc.Certification.Replace("@PINO", " " + _oExportCommercialDoc.PINos);
            //    _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


            //    _oPdfParagraph = new Paragraph(new Phrase(_oExportCommercialDoc.Certification, _oFontStyle));
            //    _oPdfParagraph.SetLeading(0, 1.20f);
            //    _oPdfParagraph.Alignment = Element.ALIGN_JUSTIFIED;
            //    _oPdfPCell = new PdfPCell();
            //    _oPdfPCell.AddElement(_oPdfParagraph);
            //    _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            //    oPdfPTable.CompleteRow();
            //}
            //#endregion

            //_oPdfPCell = new PdfPCell(oPdfPTable);
            //_oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();

            #region Clause Four
            if (_oExportCommercialDoc.ClauseFour != "" && _oExportCommercialDoc.ClauseFour != "N/A")
            {
                #region Blank space
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.FixedHeight = 5f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                oPdfPTable = new PdfPTable(1);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.SetWidths(new float[] { 560f });

             


                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.ClauseFour, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion
           

            #region Blank space
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 5f; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region ReceiverSignature
            oPdfPTable = new PdfPTable(2);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 280f, 280f });

            #region Blank space
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 5f; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            if (!string.IsNullOrEmpty(_oExportCommercialDoc.To) || !string.IsNullOrEmpty(_oExportCommercialDoc.ReceiverSignature))
            {
                _oPdfPCell = new PdfPCell(this.To());
                //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            }

            if (_oExportCommercialDoc.For != "")
            {
                _oPdfPCell = new PdfPCell(this.For());
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0;_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            }
            oPdfPTable.CompleteRow();
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0;_oPdfPCell.FixedHeight = 3f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0;  _oPdfPCell.FixedHeight = 3f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion
            #endregion

        }
        private PdfPTable SetDeliveryChallan_B_Left()
        {
            PdfPTable oPdfPTable2 = new PdfPTable(2);
            oPdfPTable2.SetWidths(new float[] { 15f, 265f });
         
            #region Beneficiary
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.Beneficiary))
            {

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.Beneficiary, _oFontStyle_UnLine));
                _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable2.AddCell(_oPdfPCell);
                oPdfPTable2.CompleteRow();

                if (!string.IsNullOrEmpty(_oExportCommercialDoc.BUName))
                {
                    #region Balnk Space
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyleBold));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    #endregion

                    _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.BUName, _oFontStyleBold));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    oPdfPTable2.CompleteRow();

                }
                if (!string.IsNullOrEmpty(_oExportCommercialDoc.BUAddress))
                {
                    #region Balnk Space
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyleBold));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    #endregion

                    _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.BUAddress, _oFontStyle));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    oPdfPTable2.CompleteRow();

                }

            }
            #endregion
            #region AccountOf
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.AccountOf))
            {

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.AccountOf, _oFontStyle_UnLine));
                _oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable2.AddCell(_oPdfPCell);
                oPdfPTable2.CompleteRow();

                if (!string.IsNullOrEmpty(_oExportCommercialDoc.ApplicantName))
                {
                    #region Balnk Space
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyleBold));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    #endregion

                    _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.ApplicantName, _oFontStyleBold));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    oPdfPTable2.CompleteRow();

                }
                if (!string.IsNullOrEmpty(_oExportCommercialDoc.ApplicantAddress_Full))
                {
                    #region Balnk Space
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyleBold));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    #endregion

                    _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.ApplicantAddress_Full, _oFontStyle));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    oPdfPTable2.CompleteRow();

                }

            }
            #endregion
            
            #region NotifyParty
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.NotifyParty))
            {
                string sTemp = "";
                string sTempTwo = "";
                if (_oExportCommercialDoc.NotifyBy != EnumNotifyBy.None)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.NotifyParty, _oFontStyle_UnLine));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.Colspan = 2;
                    _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    oPdfPTable2.CompleteRow();
                    if (_oExportCommercialDoc.NotifyBy == EnumNotifyBy.Bank)
                    {
                        sTemp = _oExportCommercialDoc.IssuingBank + "\n" + _oExportCommercialDoc.BBranchName_Issue + "\n" + _oExportCommercialDoc.BankAddress_Issue;
                    }
                    else if (_oExportCommercialDoc.NotifyBy == EnumNotifyBy.Party)
                    {
                        sTemp = _oExportCommercialDoc.ApplicantName + "\n" + _oExportCommercialDoc.ApplicantAddress;
                    }
                    else if (_oExportCommercialDoc.NotifyBy == EnumNotifyBy.Party_Bank)
                    {
                        sTemp = _oExportCommercialDoc.ApplicantName + "\n" + _oExportCommercialDoc.ApplicantAddress;
                        sTempTwo = _oExportCommercialDoc.BankName_Issue + "\n" + _oExportCommercialDoc.BBranchName_Issue + "\n" + _oExportCommercialDoc.BankAddress_Issue;
                    }
                    else if (_oExportCommercialDoc.NotifyBy == EnumNotifyBy.ThirdParty_Bank)
                    {
                        sTemp = _oExportCommercialDoc.DeliveryToName + "\n" + _oExportCommercialDoc.DeliveryToAddress;
                        sTempTwo = _oExportCommercialDoc.BankName_Issue + "\n" + _oExportCommercialDoc.BBranchName_Issue + "\n" + _oExportCommercialDoc.BankAddress_Issue;
                    }
                    else if (_oExportCommercialDoc.NotifyBy == EnumNotifyBy.ThirdParty)
                    {
                        sTemp = _oExportCommercialDoc.DeliveryToName + "\n" + _oExportCommercialDoc.DeliveryToAddress;
                    }
                    if (!string.IsNullOrEmpty(sTemp))
                    {
                        #region Balnk Space
                        if (!string.IsNullOrEmpty(sTempTwo))
                        {
                            _oPdfPCell = new PdfPCell(new Phrase("A)", _oFontStyle));
                        }
                        else
                        {
                            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        }
                        _oPdfPCell.Border = 0;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPTable2.AddCell(_oPdfPCell);
                        #endregion

                        _oPdfPCell = new PdfPCell(new Phrase(sTemp, _oFontStyle));
                        _oPdfPCell.Border = 0;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPTable2.AddCell(_oPdfPCell);
                        oPdfPTable2.CompleteRow();

                    }
                    if (!string.IsNullOrEmpty(sTempTwo))
                    {
                        #region Balnk Space
                        _oPdfPCell = new PdfPCell(new Phrase("B)", _oFontStyle));
                        _oPdfPCell.Border = 0;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPTable2.AddCell(_oPdfPCell);
                        #endregion

                        _oPdfPCell = new PdfPCell(new Phrase(sTempTwo, _oFontStyle));
                        _oPdfPCell.Border = 0;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPTable2.AddCell(_oPdfPCell);
                        oPdfPTable2.CompleteRow();

                    }
                }

            }
            #endregion
            #region NegotiatingBank
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.NegotiatingBank))
            {
                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.NegotiatingBank, _oFontStyle_UnLine));
                _oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable2.AddCell(_oPdfPCell);
                oPdfPTable2.CompleteRow();
                //_sTemp = _oExportCommercialDoc.BankName_Nego;
                //if (_oExportCommercialDoc.OrderOfBankType == EnumBankType.IssueBank) { _sTemp = _oExportCommercialDoc.BankName_Issue; }
                //if (_oExportCommercialDoc.OrderOfBankType == EnumBankType.Nego_Bank) { _sTemp = _oExportCommercialDoc.BankName_Nego; }
                //if (_oExportCommercialDoc.OrderOfBankType == EnumBankType.Forwarding_Bank) { _sTemp = _oExportCommercialDoc.BankName_Forwarding; }
                //if (_oExportCommercialDoc.OrderOfBankType == EnumBankType.Endorse_Bank) { _sTemp = _oExportCommercialDoc.BankName_Endorse; }


                if (!string.IsNullOrEmpty(_oExportCommercialDoc.BankName_Nego))
                {
                    #region Balnk Space
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyleBold));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    #endregion

                    _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.BankName_Nego, _oFontStyleBold));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    oPdfPTable2.CompleteRow();
                }

              //  _oExportCommercialDoc.BBranchName_Nego = _oExportCommercialDoc.BBranchName_Nego + "\n" + _oExportCommercialDoc.BankAddress_Nego; 
                _sTemp = _oExportCommercialDoc.BBranchName_Nego + "\n" + _oExportCommercialDoc.BankAddress_Nego;
                //if (_oExportCommercialDoc.OrderOfBankType == EnumBankType.IssueBank) { _sTemp = _oExportCommercialDoc.BBranchName_Issue + "\n" + _oExportCommercialDoc.BankAddress_Issue; }
                //if (_oExportCommercialDoc.OrderOfBankType == EnumBankType.Nego_Bank || _oExportCommercialDoc.OrderOfBankType == EnumBankType.None) { _sTemp = _oExportCommercialDoc.BBranchName_Nego + "\n" + _oExportCommercialDoc.BankAddress_Nego; }
                //if (_oExportCommercialDoc.OrderOfBankType == EnumBankType.Forwarding_Bank) { _sTemp = _oExportCommercialDoc.BBranchName_Forwarding + "\n" + _oExportCommercialDoc.BankAddress_Forwarding; }
                //if (_oExportCommercialDoc.OrderOfBankType == EnumBankType.Endorse_Bank) { _sTemp = _oExportCommercialDoc.BBranchName_Endorse + "\n" + _oExportCommercialDoc.BankAddress_Endorse; }

                if (!string.IsNullOrEmpty(_oExportCommercialDoc.BBranchName_Nego))
                {
                    #region Balnk Space
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyleBold));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    #endregion
                    _oPdfPCell = new PdfPCell(new Phrase(_sTemp, _oFontStyle));
                    //_oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.BBranchName_Nego + "\n" + _oExportCommercialDoc.BankAddress_Nego, _oFontStyle));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    oPdfPTable2.CompleteRow();

                }

            }
            #endregion
            #region ToTheOrderOf
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.ToTheOrderOf))
            {
                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.ToTheOrderOf, _oFontStyle_UnLine));
                _oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable2.AddCell(_oPdfPCell);
                oPdfPTable2.CompleteRow();
                _sTemp = _oExportCommercialDoc.BankName_Nego;
                if (_oExportCommercialDoc.OrderOfBankType == EnumBankType.IssueBank) { _sTemp = _oExportCommercialDoc.BankName_Issue; }
                if (_oExportCommercialDoc.OrderOfBankType == EnumBankType.Nego_Bank) { _sTemp = _oExportCommercialDoc.BankName_Nego; }
                if (_oExportCommercialDoc.OrderOfBankType == EnumBankType.Forwarding_Bank) { _sTemp = _oExportCommercialDoc.BankName_Forwarding; }
                if (_oExportCommercialDoc.OrderOfBankType == EnumBankType.Endorse_Bank) { _sTemp = _oExportCommercialDoc.BankName_Endorse; }

             

                if (!string.IsNullOrEmpty(_oExportCommercialDoc.BankName_Nego))
                {
                    #region Balnk Space
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyleBold));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    #endregion

                    _oPdfPCell = new PdfPCell(new Phrase(_sTemp, _oFontStyleBold));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    oPdfPTable2.CompleteRow();
                }
                if (!string.IsNullOrEmpty(_oExportCommercialDoc.BBranchName_Nego))
                {
                    #region Balnk Space
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyleBold));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    #endregion
                    _sTemp = _oExportCommercialDoc.BBranchName_Issue + "\n" + _oExportCommercialDoc.BankAddress_Issue;
                    //  _oExportCommercialDoc.BBranchName_Nego = _oExportCommercialDoc.BBranchName_Nego + "\n" + _oExportCommercialDoc.BankAddress_Nego; 
                    if (_oExportCommercialDoc.OrderOfBankType == EnumBankType.IssueBank) { _sTemp = _oExportCommercialDoc.BBranchName_Issue + "\n" + _oExportCommercialDoc.BankAddress_Issue; }
                    if (_oExportCommercialDoc.OrderOfBankType == EnumBankType.Nego_Bank || _oExportCommercialDoc.OrderOfBankType == EnumBankType.None) { _sTemp = _oExportCommercialDoc.BBranchName_Nego + "\n" + _oExportCommercialDoc.BankAddress_Nego; }
                    if (_oExportCommercialDoc.OrderOfBankType == EnumBankType.Forwarding_Bank) { _sTemp = _oExportCommercialDoc.BBranchName_Forwarding + "\n" + _oExportCommercialDoc.BankAddress_Forwarding; }
                    if (_oExportCommercialDoc.OrderOfBankType == EnumBankType.Endorse_Bank) { _sTemp = _oExportCommercialDoc.BBranchName_Endorse + "\n" + _oExportCommercialDoc.BankAddress_Endorse; }


                    _oPdfPCell = new PdfPCell(new Phrase(_sTemp, _oFontStyle));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    oPdfPTable2.CompleteRow();

                }

            }
            #endregion
            #region TermsofPayment
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.TermsofPayment))
            {
                _oPhrase = new Phrase();
                #region CountryofOrigin
                if (!string.IsNullOrEmpty(_oExportCommercialDoc.TermsofPayment))
                {
                    _oPhrase.Add(new Chunk(_oExportCommercialDoc.TermsofPayment + " : ", _oFontStyle_UnLine));
                    _oPhrase.Add(new Chunk(_oExportCommercialDoc.LCTermsName, _oFontStyle));
                }
                #endregion

                _oPdfPCell = new PdfPCell(_oPhrase);
                _oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable2.AddCell(_oPdfPCell);


            }
            #endregion
            #region TermsofPayment
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.CTPApplicant))
            {
                _oPhrase = new Phrase();
                #region CountryofOrigin
                //if (!string.IsNullOrEmpty(_oExportCommercialDoc.CTPApplicant))
                //{
                //    _oPhrase.Add(new Chunk("Terms of shipment" + " : ", _oFontStyle_UnLine));
                //    _oPhrase.Add(new Chunk(_oExportCommercialDoc.CTPApplicant, _oFontStyle));
                //}
                #endregion

                _oPdfPCell = new PdfPCell(_oPhrase);
                _oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable2.AddCell(_oPdfPCell);


            }
            #endregion
            #region CountryofOrigin
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.CountryofOrigin))
            {
                _oPhrase = new Phrase();
                #region CountryofOrigin
                if (!string.IsNullOrEmpty(_oExportCommercialDoc.CountryofOrigin))
                {
                    _oPhrase.Add(new Chunk(_oExportCommercialDoc.CountryofOrigin + " : ", _oFontStyle_UnLine));
                    _oPhrase.Add(new Chunk(_oExportCommercialDoc.CountryofOriginName, _oFontStyle));
                }
                #endregion

                _oPdfPCell = new PdfPCell(_oPhrase);
                _oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable2.AddCell(_oPdfPCell);


            }
            #endregion

            return oPdfPTable2;
        }
        private PdfPTable SetDeliveryChallan_B_Right()
        {
            PdfPTable oPdfPTable2 = new PdfPTable(2);
            oPdfPTable2.SetWidths(new float[] { 15f, 265f });
            #region BillNo
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.ChallanNo))
            {

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.ExportBill_ChallanNo + "       DT: " + _oExportCommercialDoc.DocDate, _oFontStyleBold));
                _oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.FixedHeight = 25;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable2.AddCell(_oPdfPCell);
                oPdfPTable2.CompleteRow();
            }
            #endregion
            #region CommercialInvoiceNo
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.NoAndDateOfDoc))
            {

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.NoAndDateOfDoc, _oFontStyle_UnLine));
                _oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable2.AddCell(_oPdfPCell);
                oPdfPTable2.CompleteRow();

                if (!string.IsNullOrEmpty(_oExportCommercialDoc.ExportBillNo_FullDate))
                {
                    #region Balnk Space
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyleBold));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    #endregion
                    if (_oExportCommercialDoc.IsPrintInvoiceDate == false && _oExportCommercialDoc.DocDate != "")
                    {
                        _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.ExportBillNo_FullDate + " " + _oExportCommercialDoc.DocDate, _oFontStyleBold));
                    }
                    else
                    { _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.ExportBillNo_FullDate, _oFontStyleBold)); }
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.FixedHeight = 20;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    oPdfPTable2.CompleteRow();

                }

            }
            #endregion
            #region DocumentaryCreditNoDate (LC No)
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.DocumentaryCreditNoDate))
            {

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.DocumentaryCreditNoDate, _oFontStyle_UnLine));
                _oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable2.AddCell(_oPdfPCell);
                oPdfPTable2.CompleteRow();

                if (!string.IsNullOrEmpty(_oExportCommercialDoc.ExportLCNoAndDate))
                {
                    #region Balnk Space
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyleBold));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    #endregion

                    _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.ExportLCNoAndDate + "" + _oExportCommercialDoc.AmendmentNonDate, _oFontStyleBold));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    oPdfPTable2.CompleteRow();

                }

            }
            #endregion
            #region ProformaInvoiceNoAndDate
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.ProformaInvoiceNoAndDate))
            {

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.ProformaInvoiceNoAndDate, _oFontStyle_UnLine));
                _oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable2.AddCell(_oPdfPCell);
                oPdfPTable2.CompleteRow();

                if (!string.IsNullOrEmpty(_oExportCommercialDoc.PINos))
                {
                    #region Balnk Space
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyleBold));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    #endregion

                    _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.PINos, _oFontStyleBold));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    oPdfPTable2.CompleteRow();

                }

            }

            #endregion
            
            #region Delivery To
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.DeliveryTo) && !string.IsNullOrEmpty(_oExportCommercialDoc.DeliveryToName))
            {

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.DeliveryTo, _oFontStyle_UnLine));
                _oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable2.AddCell(_oPdfPCell);
                oPdfPTable2.CompleteRow();

                if (!string.IsNullOrEmpty(_oExportCommercialDoc.DeliveryToName))
                {
                    #region Balnk Space
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyleBold));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    #endregion

                    _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.DeliveryToName, _oFontStyleBold));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    oPdfPTable2.CompleteRow();

                    _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.DeliveryToAddress, _oFontStyle));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    oPdfPTable2.CompleteRow();
                }

            }
            #endregion
            #region IssuingBank
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.IssuingBank))
            {
                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.IssuingBank, _oFontStyle_UnLine));
                _oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable2.AddCell(_oPdfPCell);
                oPdfPTable2.CompleteRow();

                if(!string.IsNullOrEmpty(_oExportCommercialDoc.BBranchName_Endorse))
                {
                    _oExportCommercialDoc.BankName_Issue = _oExportCommercialDoc.BankName_Endorse;
                    _oExportCommercialDoc.BBranchName_Issue = _oExportCommercialDoc.BBranchName_Endorse;
                    _oExportCommercialDoc.BankAddress_Issue = _oExportCommercialDoc.BankAddress_Endorse;
                }

                if (!string.IsNullOrEmpty(_oExportCommercialDoc.BankName_Issue))
                {
                    #region Balnk Space
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyleBold));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    #endregion

                    _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.BankName_Issue, _oFontStyleBold));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    oPdfPTable2.CompleteRow();
                }
                if (!string.IsNullOrEmpty(_oExportCommercialDoc.BBranchName_Issue))
                {
                    #region Balnk Space
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyleBold));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    #endregion

                    _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.BBranchName_Issue + "\n" + _oExportCommercialDoc.BankAddress_Issue, _oFontStyle));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    oPdfPTable2.CompleteRow();

                }

            }
            #endregion
            #region Master LC/ AgainstExportLC
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.AgainstExportLC))
            {
                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.AgainstExportLC, _oFontStyle_UnLine));
                _oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable2.AddCell(_oPdfPCell);
                oPdfPTable2.CompleteRow();

                if (!string.IsNullOrEmpty(_oExportCommercialDoc.AllMasterLCNo))
                {
                    #region Balnk Space
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyleBold));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    #endregion

                    if (_oExportCommercialDoc.AllMasterLCNo.Length > 250)
                    {
                        _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.AllMasterLCNo, FontFactory.GetFont("Tahoma", 7f, 0)));
                    }
                    else
                    {
                        _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.AllMasterLCNo, FontFactory.GetFont("Tahoma",8f, 0)));
                    }
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    oPdfPTable2.CompleteRow();
                }
               

            }
            #endregion
            #region  Other Info
            List<ExportPartyInfoBill> oExportPartyInfoBills = new List<ExportPartyInfoBill>();
            if (_oExportCommercialDoc.ExportBillDocID > 0)
            { oExportPartyInfoBills = ExportPartyInfoBill.GetsByExportLC(_oExportCommercialDoc.ExportBillDocID, (int)EnumMasterLCType.CommercialDoc, 0); }
            else
            {
                oExportPartyInfoBills = ExportPartyInfoBill.GetsByExportLC(_oExportCommercialDoc.ExportLCID, (int)EnumMasterLCType.ExportLC, 0);
            }

            if (!String.IsNullOrEmpty(_oExportCommercialDoc.HSCode_Head) && !String.IsNullOrEmpty(_oExportCommercialDoc.HSCode))
            {
                ExportPartyInfoBill oExportPartyInfoBill = new ExportPartyInfoBill();
                oExportPartyInfoBill.PartyInfo = _oExportCommercialDoc.HSCode_Head;
                oExportPartyInfoBill.RefNo = _oExportCommercialDoc.HSCode;
                oExportPartyInfoBills.Add(oExportPartyInfoBill);
            }
            if (oExportPartyInfoBills.Count > 0)
                {
                    #region Balnk Space
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyleBold));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    #endregion

                    _oPdfPCell = new PdfPCell(this.ExportPartyInfo(false, oExportPartyInfoBills));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    oPdfPTable2.CompleteRow();
                }

            //}
            #endregion
            return oPdfPTable2;
        }
        private PdfPTable SetShipmentTerm_DC_Left()
        {
            PdfPTable oPdfPTable2 = new PdfPTable(2);
            oPdfPTable2.SetWidths(new float[] { 100f, 400f });

            #region PortofLoading
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.PortofLoading))
            {
                _oPhrase = new Phrase();

                #region Port Of Loading
                if (!string.IsNullOrEmpty(_oExportCommercialDoc.PortofLoading))
                {
                    _oPhrase.Add(new Chunk(_oExportCommercialDoc.PortofLoading + " : ", _oFontStyleBold));

                    _oPhrase.Add(new Chunk(_oExportCommercialDoc.PortofLoadingName, _oFontStyle));
                }
                #endregion

                _oPdfPCell = new PdfPCell(_oPhrase);
                _oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable2.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.PortofLoadingAddress, _oFontStyle));
                _oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable2.AddCell(_oPdfPCell);
                oPdfPTable2.CompleteRow();
            }
            #endregion



            #region Carrier
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.Carrier))
            {
                _oPhrase = new Phrase();

                #region Port Of Loading
                if (!string.IsNullOrEmpty(_oExportCommercialDoc.Carrier))
                {
                    _oPhrase.Add(new Chunk(_oExportCommercialDoc.Carrier + " : ", _oFontStyleBold));
                }
                #endregion

                #region CarrierName
                if (!string.IsNullOrEmpty(_oExportCommercialDoc.Carrier))
                {
                    _oPhrase.Add(new Chunk(_oExportCommercialDoc.CarrierName, _oFontStyle));
                }
                #endregion

                _oPdfPCell = new PdfPCell(_oPhrase);
                _oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.BorderWidthTop = 0.5f;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable2.AddCell(_oPdfPCell);

                oPdfPTable2.CompleteRow();
            }
            #endregion
            #region Truck Receipt No
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.TRNo))
            {
                _oPhrase = new Phrase();
                #region TRNo
                if (!string.IsNullOrEmpty(_oExportCommercialDoc.TRNo))
                {
                    _oPhrase.Add(new Chunk("Receipt/Challan No:", _oFontStyleBold));
                    _oPhrase.Add(new Chunk(_oExportCommercialDoc.TRNo + " ", _oFontStyle));
                    if (!string.IsNullOrEmpty(_oExportCommercialDoc.TRDate))
                    {
                        _oPhrase.Add(new Chunk(" DT: " + _oExportCommercialDoc.TRDate, _oFontStyle));
                    }
                }
                #endregion

                _oPdfPCell = new PdfPCell(_oPhrase);
                _oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable2.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable2.AddCell(_oPdfPCell);
                oPdfPTable2.CompleteRow();
            }
            #endregion
            #region ShippingMark
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.ShippingMark))
            {
                _oPhrase = new Phrase();

                #region ShippingMark
                if (!string.IsNullOrEmpty(_oExportCommercialDoc.ShippingMark))
                {
                    _oPhrase.Add(new Chunk(_oExportCommercialDoc.ShippingMark + " : ", _oFontStyleBold));
                }
                #endregion

                #region CarrierName
                if (!string.IsNullOrEmpty(_oExportCommercialDoc.ShippingMark) && !string.IsNullOrEmpty(_oExportCommercialDoc.ShippingMarkName))
                {
                    _oPhrase.Add(new Chunk(_oExportCommercialDoc.ShippingMarkName, _oFontStyle));
                }
                #endregion

                _oPdfPCell = new PdfPCell(_oPhrase);
                _oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.BorderWidthTop = 0.5f;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable2.AddCell(_oPdfPCell);
                oPdfPTable2.CompleteRow();
            }
            #endregion

            #region SellingOnAbout
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.SellingOnAbout))
            {
                _oPhrase = new Phrase();

                #region SellingOnAbout
                if (!string.IsNullOrEmpty(_oExportCommercialDoc.SellingOnAbout))
                {
                    _oPhrase.Add(new Chunk(_oExportCommercialDoc.SellingOnAbout, _oFontStyleBold));
                }
                #endregion

                #region CarrierName
                if (!string.IsNullOrEmpty(_oExportCommercialDoc.SellingOnAbout) && !string.IsNullOrEmpty(_oExportCommercialDoc.SellingOnAboutName))
                {
                    _oPhrase.Add(new Chunk(_oExportCommercialDoc.SellingOnAboutName, _oFontStyle));
                }
                #endregion

                _oPdfPCell = new PdfPCell(_oPhrase);
                _oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.BorderWidthTop = 0.5f;
                _oPdfPCell.FixedHeight = 25f;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable2.AddCell(_oPdfPCell);

                oPdfPTable2.CompleteRow();
            }
            #endregion

            return oPdfPTable2;
        }
        private PdfPTable SetShipmentTerm_DC_Right()
        {
            PdfPTable oPdfPTable2 = new PdfPTable(2);
            oPdfPTable2.SetWidths(new float[] { 100f, 400f });

            #region FinalDestination
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.FinalDestination))
            {
                _oPhrase = new Phrase();

                #region Port Of Loading
                if (!string.IsNullOrEmpty(_oExportCommercialDoc.FinalDestination))
                {
                    _oPhrase.Add(new Chunk(_oExportCommercialDoc.FinalDestination + " ", _oFontStyleBold));

                    _oPhrase.Add(new Chunk(_oExportCommercialDoc.FinalDestinationName + " ", _oFontStyle));

                }
                #endregion

                _oPdfPCell = new PdfPCell(_oPhrase);
                _oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable2.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable2.AddCell(_oPdfPCell);
                oPdfPTable2.CompleteRow();
            }
            #endregion
            #region DriverName_Print
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.DriverName_Print))
            {
                _oPhrase = new Phrase();

                #region DriverName
                if (!string.IsNullOrEmpty(_oExportCommercialDoc.DriverName))
                {
                    _oPhrase.Add(new Chunk(_oExportCommercialDoc.DriverName_Print + ":", _oFontStyleBold));

                    _oPhrase.Add(new Chunk(_oExportCommercialDoc.DriverName + " ", _oFontStyle));


                #endregion

                    _oPdfPCell = new PdfPCell(_oPhrase);
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.Colspan = 2;
                    _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.Colspan = 2;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    oPdfPTable2.CompleteRow();
                }
            }
            #endregion
            #region TruckNo_Print
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.TruckNo_Print))
            {
                _oPhrase = new Phrase();

                #region Port Of Loading
                if (!string.IsNullOrEmpty(_oExportCommercialDoc.TruckNo))
                {
                    _oPhrase.Add(new Chunk(_oExportCommercialDoc.TruckNo_Print + ":", _oFontStyleBold));
                    _oPhrase.Add(new Chunk(_oExportCommercialDoc.TruckNo + " ", _oFontStyle));
                }
                #endregion

                _oPdfPCell = new PdfPCell(_oPhrase);
                _oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable2.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable2.AddCell(_oPdfPCell);
                oPdfPTable2.CompleteRow();
            }
            #endregion
          
            

            return oPdfPTable2;
        }
        private PdfPTable ProductDetails()
        {
            PdfPTable oPdfPTable1 = new PdfPTable(6);
            oPdfPTable1.WidthPercentage = 100;
            oPdfPTable1.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable1.SetWidths(new float[] { 50f, 80f, 175f, 80f, 100f, 100f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, 1);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.SL, _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.MarkSAndNos, _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.DescriptionOfGoods, _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.NetWeight + (!string.IsNullOrEmpty(_sMUName) ? "(" + _sMUName + ")" : ""), _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.GrossWeight + (!string.IsNullOrEmpty(_sMUName) ? "(" + _sMUName + ")" : ""), _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);
            oPdfPTable1.CompleteRow();

            List<ExportBillDetail> oExportBillDetails = new List<ExportBillDetail>();
            oExportBillDetails = _oExportCommercialDoc.ExportBillDetails;

            int nSL = 0;
            double nTotalBags = 0;
            double nGrossWeight = 0, nTotalGrossWeight = 0;
            _nTotalRowCount = 0;
            foreach (ExportBillDetail oitem in oExportBillDetails)
            {
                nSL++;
                _oPdfPCell = new PdfPCell(new Phrase(nSL.ToString(), _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oitem.NoOfBag + "", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oitem.ProductName + " " + oitem.ProductDesciption, _oFontStyle));
                _oPdfPCell.Colspan = 2; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oitem.Qty), _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

                nGrossWeight = oitem.Qty * 1.024; //Hard Code
                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nGrossWeight), _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

                oPdfPTable1.CompleteRow();

                _nTotalRowCount++;
                _nTotalValue = _nTotalValue + (oitem.Qty * oitem.UnitPrice);
                _nTotalQty = _nTotalQty + oitem.Qty;

                nTotalGrossWeight += nGrossWeight;
                nTotalBags += oitem.NoOfBag;
            }

            for (int i = _nTotalRowCount + 1; i <= 15; i++)
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Colspan = 2; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

                oPdfPTable1.CompleteRow();
            }
            _oPdfPCell = new PdfPCell(new Phrase("Total : ", _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(nTotalBags.ToString() + " " + (nTotalBags > 1 ? "BAGS" : "BAG"), _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.Colspan = 2; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalQty) + " " + _sMUName, _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalGrossWeight) + " " + _sMUName, _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);
            oPdfPTable1.CompleteRow();

            return oPdfPTable1;
        }

      
        private PdfPTable ProductDetails_Dyeing_UnitePrice()
        {

            PdfPTable oPdfPTable = new PdfPTable(6);
            oPdfPTable = new PdfPTable(6);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 60f, 200,  100f, 65f, 65f, 75f });
            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.MarkSAndNos, _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.DescriptionOfGoods, _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.Header_Color, _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.QtyInKg, _oFontStyleBold));
            _oPdfPCell.Border = 0;  _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.UnitPriceDes, _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.ValueDes, _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


            oPdfPTable.CompleteRow();
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.TextWithGoodsRow))
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.TextWithGoodsRow, FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.ITALIC)));
                _oPdfPCell.Colspan = 2; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase("", FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.ITALIC)));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.CTPApplicant, FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.ITALIC)));
                _oPdfPCell.Colspan = 3; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();
            }

            List<ExportBillDetail> oExportBillDetails = new List<ExportBillDetail>();
            oExportBillDetails = _oExportCommercialDoc.ExportBillDetails;

            oExportBillDetails = oExportBillDetails.GroupBy(x => new { x.ProductID, x.ProductName, x.StyleNo, x.ColorInfo, x.BuyerReference, x.ProductDesciption, x.UnitPrice, x.MUnitID, x.IsDeduct }, (key, grp) =>
                                            new ExportBillDetail
                                            {
                                                ProductID = key.ProductID,
                                                ProductName = key.ProductName,
                                                StyleNo = key.StyleNo,
                                                ColorInfo = key.ColorInfo,
                                                BuyerReference = key.BuyerReference,
                                                ProductDesciption = key.ProductDesciption,
                                                Qty = grp.Sum(p => p.Qty),
                                                NoOfBag = grp.Sum(p => p.NoOfBag),
                                                WtPerBag = grp.Sum(p => p.WtPerBag),
                                                UnitPrice = key.UnitPrice,
                                                MUnitID = key.MUnitID,
                                                IsDeduct = key.IsDeduct
                                            }).ToList();

            int nSL = 0;
            _nTotalRowCount = 0;
            foreach (ExportBillDetail oitem in oExportBillDetails)
            {
                nSL++;
                if (_oExportCommercialDoc.WeightPerBag <= 0)
                { _oExportCommercialDoc.WeightPerBag = 100; }
                _nTotalNoOfBag = _nTotalNoOfBag + Math.Floor(oitem.NoOfBag);
                _oPdfPCell = new PdfPCell(new Phrase(_nTotalNoOfBag_PREV + "-" + (_nTotalNoOfBag).ToString(), _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _nTotalNoOfBag_PREV = _nTotalNoOfBag_PREV + Math.Floor(oitem.NoOfBag);

                _oPdfPCell = new PdfPCell(new Phrase(oitem.ProductDesciption + " " + oitem.ProductName , _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase(oitem.StyleRef, _oFontStyle));
                //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight =0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                if (string.IsNullOrEmpty(oitem.ColorInfo)) { oitem.ColorInfo = _oExportCommercialDoc.TextWithGoodsCol; }
                _oPdfPCell = new PdfPCell(new Phrase(oitem.ColorInfo, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(Math.Round(oitem.Qty,4)), _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.Currency + "" + System.Math.Round(oitem.UnitPrice, 4).ToString("#,##0.00##;(#,##0.00##)"), _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oitem.Qty * oitem.UnitPrice), _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();




                _nTotalRowCount++;
                _nTotalValue = _nTotalValue + (oitem.Qty * oitem.UnitPrice);
                _nTotalQty = _nTotalQty + oitem.Qty;
                _nTotalQtyKg = _nTotalQtyKg + oitem.Qty;
                //_nTotalNoOfBag = _nTotalNoOfBag + oitem.NoOfBag;
                _nTotalWtPerBag = _nTotalWtPerBag + oitem.WtPerBag;

                _nUsagesHeight = CalculatePdfPTableHeight(_oPdfPTable);
                _nUsagesHeight = _nUsagesHeight + CalculatePdfPTableHeight(oPdfPTable);
                if (_nUsagesHeight > 560)
                {

                 
                    _oPdfPCell = new PdfPCell(oPdfPTable);

                    _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();

                    _nUsagesHeight = 0;
                    _oDocument.Add(_oPdfPTable);
                    _oDocument.NewPage();

                    oPdfPTable.DeleteBodyRows();
                    _oPdfPTable.DeleteBodyRows();

                    this.HeaderWithThreeFormats(_oExportCommercialDoc.DocHeader);
                    this.ReportHeader(_oExportCommercialDoc.DocHeader);
                    oPdfPTable = new PdfPTable(6);
                    oPdfPTable.SetWidths(new float[] { 50f, 80f, 175f, 80f, 100f, 100f });

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                    _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                    _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                    _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                    _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                    _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop =  0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    oPdfPTable.CompleteRow();

                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                    _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                }

            }

            oPdfPTable.CompleteRow();
            for (int i = _nTotalRowCount + 1; i <= 5; i++)
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.ASPERPI))
            {
                _oExportCommercialDoc.ASPERPI = _oExportCommercialDoc.ASPERPI.Replace("@PINO", " " + _oExportCommercialDoc.PINos);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                if (_oExportCommercialDoc.ASPERPI.Length > 700)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.ASPERPI, FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL)));
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.ASPERPI, FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL)));
                }
                _oPdfPCell.Colspan = 2; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }


            #region Total

            _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 3; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(Math.Round(_nTotalQty, 4)) + " " + _oExportCommercialDoc.MUName, _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.Currency + "" + Global.MillionFormat(_nTotalValue), _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion
            #region AmountInWord
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.AmountInWord))
            {
                _oPhrase = new Phrase();
                _oPhrase.Add(new Chunk(_oExportCommercialDoc.AmountInWord + ": ", _oFontStyle));
                _oPhrase.Add(new Chunk("US " + Global.DollarWords(_nTotalValue), _oFontStyleBold));
                _oPdfPCell = new PdfPCell(_oPhrase);
                _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 6; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            #endregion
                     


            return oPdfPTable;
        }
        private PdfPTable ProductDetails_Dyeing_Amount()
        {

            PdfPTable oPdfPTable = new PdfPTable(6);
            oPdfPTable = new PdfPTable(6);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 30f, 210f, 100f, 75f, 50f, 90f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.SL, _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.DescriptionOfGoods, _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Color", _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.QtyInKg, _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.Bag_Name, _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.ValueDes, _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            List<ExportBillDetail> oExportBillDetails = new List<ExportBillDetail>();
            oExportBillDetails = _oExportCommercialDoc.ExportBillDetails;
            int nSL = 0;
            _nTotalRowCount = 0;
            foreach (ExportBillDetail oitem in oExportBillDetails)
            {
                nSL++;

                _oPdfPCell = new PdfPCell(new Phrase(nSL.ToString(), _oFontStyle));
                _oPdfPCell.Border = 0;  _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oitem.ProductName + " " + oitem.ProductDesciption, _oFontStyle));
                _oPdfPCell.Border = 0;  _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oitem.ColorInfo, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oitem.Qty), _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                oitem.WtPerBag = 40;
                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(Math.Floor(Math.Round(oitem.Qty / oitem.WtPerBag, 0))), _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.Currency + "" + Global.MillionFormat(oitem.Qty * oitem.UnitPrice), _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();
                _nTotalRowCount++;
                _nTotalValue = _nTotalValue + (oitem.Qty * oitem.UnitPrice);
                _nTotalQty = _nTotalQty + oitem.Qty;
                _nTotalQtyKg = _nTotalQtyKg + oitem.Qty;
                _nTotalNoOfBag = _nTotalNoOfBag + (oitem.Qty / oitem.WtPerBag);
                _nTotalNoOfBag = Math.Floor(Math.Round(_nTotalNoOfBag, 0));
            }

            oPdfPTable.CompleteRow();
            for (int i = _nTotalRowCount + 1; i <= 6; i++)
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0;  _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }

          


            _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyleBold));
            _oPdfPCell.Colspan = 3; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalQty), _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalNoOfBag), _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.Currency + "" + Global.MillionFormat(_nTotalValue), _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            if (_oExportCommercialDoc.AmountInWord != "" && _oExportCommercialDoc.AmountInWord != "N/A")
            {
                _oPdfPCell = new PdfPCell(new Phrase("(" + _oExportCommercialDoc.AmountInWord + ": US " + Global.DollarWords(_nTotalValue) + ")", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 7; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }


            return oPdfPTable;
        }
        
        private PdfPTable ProductDetails_Dyeing_NET_GROSSWEIGHT()
        {

            PdfPTable oPdfPTable = new PdfPTable(6);
          //  oPdfPTable = new PdfPTable(4);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 70f, 200, 65f, 70f,70f,70f });
            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.MarkSAndNos, _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.DescriptionOfGoods, _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.Header_Color, _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.QtyInKg, _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.NetWeight + "\n(" + _oExportCommercialDoc.MUName + ")", _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.GrossWeight + "\n(" + _oExportCommercialDoc.MUName+")", _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


            oPdfPTable.CompleteRow();

            //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
           
            //_oPdfPCell = new PdfPCell(new Phrase("DYEING CHARGE FOR 100% EXPORT ORIENTED READYMADE GARMENTS INDUSTRIES", FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.ITALIC)));
            //_oPdfPCell.Colspan = 2; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            
            //_oPdfPCell = new PdfPCell(new Phrase("", FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.ITALIC)));
            //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            ////_oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.CTPApplicant, FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.ITALIC)));
            //// _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


            //oPdfPTable.CompleteRow();

            List<ExportBillDetail> oExportBillDetails = new List<ExportBillDetail>();
            oExportBillDetails = _oExportCommercialDoc.ExportBillDetails;

            oExportBillDetails = oExportBillDetails.GroupBy(x => new { x.ProductID, x.ProductName, x.StyleNo, x.ColorInfo, x.BuyerReference, x.ProductDesciption, x.UnitPrice, x.MUnitID,x.IsDeduct }, (key, grp) =>
                                            new ExportBillDetail
                                            {
                                                ProductID = key.ProductID,
                                                ProductName = key.ProductName,
                                                StyleNo = key.StyleNo,
                                                ColorInfo = key.ColorInfo,
                                                BuyerReference = key.BuyerReference,
                                                ProductDesciption = key.ProductDesciption,
                                                Qty = grp.Sum(p => p.Qty),
                                                NoOfBag = grp.Sum(p => p.NoOfBag),
                                                WtPerBag = grp.Sum(p => p.WtPerBag),
                                                UnitPrice = key.UnitPrice,
                                                MUnitID = key.MUnitID,
                                                IsDeduct = key.IsDeduct
                                            }).ToList();

            int nSL = 0;
            _nTotalRowCount = 0;
            _nTotalNoOfBag_PREV = 1;
            foreach (ExportBillDetail oitem in oExportBillDetails)
            {
                nSL++;
                if (_oExportCommercialDoc.WeightPerBag <= 0)
                { _oExportCommercialDoc.WeightPerBag = 100; }
                _nTotalNoOfBag = _nTotalNoOfBag + Math.Floor(oitem.NoOfBag);
                _oPdfPCell = new PdfPCell(new Phrase(_nTotalNoOfBag_PREV + "-" + (_nTotalNoOfBag).ToString(), _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _nTotalNoOfBag_PREV = 1 + Math.Floor(oitem.NoOfBag);

                _oPdfPCell = new PdfPCell(new Phrase(oitem.ProductDesciption + " " + oitem.ProductName , _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase(oitem.StyleRef, _oFontStyle));
                //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight =0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                //if (string.IsNullOrEmpty(oitem.ColorInfo)) { oitem.ColorInfo = "Average"; }
                _oPdfPCell = new PdfPCell(new Phrase(oitem.ColorInfo, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(Math.Round(oitem.Qty,4)), _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(Math.Round(oitem.Qty, 4)), _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(Math.Round(oitem.Qty + (oitem.Qty *_oExportCommercialDoc.GrossWeightPTage), 4)), _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


                //_oPdfPCell = new PdfPCell(new Phrase(System.Math.Round(oitem.UnitPrice, 4).ToString("#,##0.00##;(#,##0.00##)"), _oFontStyle));
                //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oitem.Qty * oitem.UnitPrice), _oFontStyle));
                //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();
                _nTotalRowCount++;
                _nTotalValue = _nTotalValue + (oitem.Qty * oitem.UnitPrice);
                _nTotalQty = _nTotalQty + oitem.Qty;
                _nTotalQtyKg = _nTotalQtyKg + (oitem.Qty + (oitem.Qty * _oExportCommercialDoc.GrossWeightPTage));
                //_nTotalNoOfBag = _nTotalNoOfBag + oitem.NoOfBag;
                _nTotalWtPerBag = _nTotalWtPerBag + oitem.WtPerBag;
            }

            oPdfPTable.CompleteRow();
            for (int i = _nTotalRowCount + 1; i <= 5; i++)
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.ASPERPI))
            {
                _oExportCommercialDoc.ASPERPI = _oExportCommercialDoc.ASPERPI.Replace("@PINO", " " + _oExportCommercialDoc.PINos);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.ASPERPI, FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL)));
                _oPdfPCell.Colspan = 2; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }


            #region Total

            _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 3; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(Math.Round(_nTotalQty, 4)) + " " + _oExportCommercialDoc.MUName, _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(Math.Round(_nTotalQty, 4)) + " " + _oExportCommercialDoc.MUName, _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(Math.Round(_nTotalQtyKg, 4)) + " " + _oExportCommercialDoc.MUName, _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion
            #region AmountInWord
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.AmountInWord))
            {
                _oPhrase = new Phrase();
                _oPhrase.Add(new Chunk(_oExportCommercialDoc.AmountInWord + ": ", _oFontStyle));
                _oPhrase.Add(new Chunk("US " + Global.DollarWords(_nTotalValue), _oFontStyleBold));
                _oPdfPCell = new PdfPCell(_oPhrase);
                _oPdfPCell.Border = 0; _oPdfPCell.Colspan =6; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            #endregion



            return oPdfPTable;
        }
        private PdfPTable ProductDetails_Dyeing_Qty()
        {

            PdfPTable oPdfPTable = new PdfPTable(4);
            oPdfPTable = new PdfPTable(4);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 70f, 205, 70f, 70f });
            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.MarkSAndNos, _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.DescriptionOfGoods, _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.Header_Color, _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.QtyInKg, _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


            //_oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.UnitPriceDes, _oFontStyleBold));
            //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.ValueDes, _oFontStyleBold));
            //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //Average
            oPdfPTable.CompleteRow();
            //DYEING CHARGE FOR 100% EXPORT ORIENTED READYMADE GARMENTS INDUSTRIES
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.TextWithGoodsRow))
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.TextWithGoodsRow, FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.ITALIC)));
                _oPdfPCell.Colspan = 2; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase("", FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.ITALIC)));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                //_oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.CTPApplicant, FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.ITALIC)));
                // _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }

            List<ExportBillDetail> oExportBillDetails = new List<ExportBillDetail>();
            oExportBillDetails = _oExportCommercialDoc.ExportBillDetails;

            oExportBillDetails = oExportBillDetails.GroupBy(x => new { x.ProductID, x.ProductName, x.StyleNo, x.ColorInfo, x.BuyerReference, x.ProductDesciption, x.UnitPrice, x.MUnitID, x.IsDeduct }, (key, grp) =>
                                            new ExportBillDetail
                                            {
                                                ProductID = key.ProductID,
                                                ProductName = key.ProductName,
                                                StyleNo = key.StyleNo,
                                                ColorInfo = key.ColorInfo,
                                                BuyerReference = key.BuyerReference,
                                                ProductDesciption = key.ProductDesciption,
                                                Qty = grp.Sum(p => p.Qty),
                                                NoOfBag = grp.Sum(p => p.NoOfBag),
                                                WtPerBag = grp.Sum(p => p.WtPerBag),
                                                UnitPrice = key.UnitPrice,
                                                MUnitID = key.MUnitID,
                                                IsDeduct = key.IsDeduct
                                            }).ToList();

            int nSL = 0;
            _nTotalRowCount = 0;
            foreach (ExportBillDetail oitem in oExportBillDetails)
            {
                nSL++;
                if (_oExportCommercialDoc.WeightPerBag <= 0)
                { _oExportCommercialDoc.WeightPerBag = 100; }
                _nTotalNoOfBag = _nTotalNoOfBag + Math.Floor(oitem.NoOfBag);
                _oPdfPCell = new PdfPCell(new Phrase(_nTotalNoOfBag_PREV + "-" + (_nTotalNoOfBag).ToString(), _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _nTotalNoOfBag_PREV = 1 + Math.Floor(oitem.NoOfBag);

                _oPdfPCell = new PdfPCell(new Phrase(oitem.ProductDesciption + " " + oitem.ProductName , _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase(oitem.StyleRef, _oFontStyle));
                //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight =0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                if (string.IsNullOrEmpty(oitem.ColorInfo)) { oitem.ColorInfo = _oExportCommercialDoc.TextWithGoodsCol; }
                _oPdfPCell = new PdfPCell(new Phrase(oitem.ColorInfo, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(Math.Round(oitem.Qty, 4)), _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase(System.Math.Round(oitem.UnitPrice, 4).ToString("#,##0.00##;(#,##0.00##)"), _oFontStyle));
                //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oitem.Qty * oitem.UnitPrice), _oFontStyle));
                //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();
                _nTotalRowCount++;
                _nTotalValue = _nTotalValue + (oitem.Qty * oitem.UnitPrice);
                _nTotalQty = _nTotalQty + oitem.Qty;
                _nTotalQtyKg = _nTotalQtyKg + oitem.Qty;
                //_nTotalNoOfBag = _nTotalNoOfBag + oitem.NoOfBag;
                _nTotalWtPerBag = _nTotalWtPerBag + oitem.WtPerBag;
            }

            oPdfPTable.CompleteRow();
            for (int i = _nTotalRowCount + 1; i <= 5; i++)
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.ASPERPI))
            {
                _oExportCommercialDoc.ASPERPI = _oExportCommercialDoc.ASPERPI.Replace("@PINO", " " + _oExportCommercialDoc.PINos);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                if (_oExportCommercialDoc.ASPERPI.Length > 150)
                {

                    _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.ASPERPI, FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL)));
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.ASPERPI, FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL)));
                }
                _oPdfPCell.Colspan = 2; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }


            #region Total

            _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 3; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(Math.Round(_nTotalQty, 4)) + " " + _oExportCommercialDoc.MUName, _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.Currency + "" + Global.MillionFormat(_nTotalValue), _oFontStyleBold));
            //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion
            #region AmountInWord
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.AmountInWord))
            {
                _oPhrase = new Phrase();
                _oPhrase.Add(new Chunk(_oExportCommercialDoc.AmountInWord + ": ", _oFontStyle));
                _oPhrase.Add(new Chunk("US " + Global.DollarWords(_nTotalValue), _oFontStyleBold));
                _oPdfPCell = new PdfPCell(_oPhrase);
                _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 4; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            #endregion



            return oPdfPTable;
        }
        private PdfPTable ProductDetails_WU(bool bShowAmount)
        {
            #region Weaving
            PdfPTable oPdfPTable1 = new PdfPTable(7);
            oPdfPTable1.SetWidths(new float[] { 70f, 180,80f,80f,80f, 65f, 65f });


            List<ExportBillDetail> oExportBillDetails = new List<ExportBillDetail>();
            List<ExportBillDetail> oExportBillDetailsTemp = new List<ExportBillDetail>();
            oExportBillDetails = _oExportCommercialDoc.ExportBillDetails;
         

         

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.MarkSAndNos, _oFontStyleBold));
            _oPdfPCell.Border = 0;  _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.DescriptionOfGoods, _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.Header_Style, _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.Header_Color, _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);
          
            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.QtyInKg, _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

         
             _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.UnitPriceDes, _oFontStyleBold));
             _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);
          
             _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.ValueDes, _oFontStyleBold));
             _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

        
            oPdfPTable1.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("ROLL No", _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase("FABRICS FOR 100% EXPORT ORIENTED READYMADE GARMENTS INDUSTRY", FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.ITALIC)));
            _oPdfPCell.Colspan = 3; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.CTPApplicant, FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.ITALIC)));
            _oPdfPCell.Colspan = 3; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);
            
            oPdfPTable1.CompleteRow();

            int nSL = 0;
            _nTotalRowCount = 0;
            var sTemp = "";
            string sConstruction = "";
            string sConst="";
            int nProductID = 0;
            string sProcessTypeName = "";
            string sFabricWeaveName = "";
            string sFabricWidth = "";
            string sFinishTypeName = "";
            int _nCount_Raw=0;
            int _nCount_Raw_Style = 0;
            string sStyleNo = "";
             if (_oExportCommercialDoc.ProductPrintType == EnumExcellColumn.B)
            {
                oExportBillDetailsTemp = oExportBillDetails.GroupBy(x => new { x.ProductID, x.ProductName, x.FabricNo, x.Construction, x.ProcessTypeName, x.FabricWeaveName, x.FabricWidth, x.FinishTypeName, x.StyleNo, x.BuyerReference, x.Shrinkage, x.Weight, x.ColorInfo, x.UnitPrice, x.MUnitID, x.IsDeduct }, (key, grp) =>
                                                  new ExportBillDetail
                                                  {
                                                      ProductID = key.ProductID,
                                                      ProductName = key.ProductName,
                                                      FabricNo = key.FabricNo,
                                                      Construction = key.Construction,
                                                      ProcessTypeName = key.ProcessTypeName,
                                                      FabricWeaveName = key.FabricWeaveName,
                                                      FabricWidth = key.FabricWidth,
                                                      FinishTypeName = key.FinishTypeName,
                                                      StyleNo = key.StyleNo,
                                                      BuyerReference = key.BuyerReference,
                                                      Shrinkage = key.Shrinkage,
                                                      Weight = key.Weight,
                                                      ColorInfo = key.ColorInfo,
                                                      Qty = grp.Sum(p => p.Qty),
                                                      NoOfBag = grp.Sum(p => p.NoOfBag),
                                                      WtPerBag = grp.Sum(p => p.WtPerBag),
                                                      UnitPrice = key.UnitPrice,
                                                      MUnitID = key.MUnitID,
                                                      IsDeduct = key.IsDeduct
                                                  }).ToList();
            }
            else
            {
                oExportBillDetailsTemp = oExportBillDetails.GroupBy(x => new { x.ProductID, x.ProductName, x.Construction, x.ProcessTypeName, x.FabricWeaveName, x.FabricWidth, x.FinishTypeName, x.StyleNo, x.ColorInfo, x.BuyerReference, x.UnitPrice, x.MUnitID, x.IsDeduct}, (key, grp) =>
                                                 new ExportBillDetail
                                                 {
                                                     ProductID = key.ProductID,
                                                     ProductName = key.ProductName,
                                                     Construction = key.Construction,
                                                     ProcessTypeName = key.ProcessTypeName,
                                                     FabricWeaveName = key.FabricWeaveName,
                                                     FabricWidth = key.FabricWidth,
                                                     FinishTypeName = key.FinishTypeName,
                                                     StyleNo = key.StyleNo,
                                                     ColorInfo = key.ColorInfo,
                                                     BuyerReference = key.BuyerReference,
                                                     Qty = grp.Sum(p => p.Qty),
                                                     NoOfBag = grp.Sum(p => p.NoOfBag),
                                                     WtPerBag = grp.Sum(p => p.WtPerBag),
                                                     UnitPrice = key.UnitPrice,
                                                     MUnitID = key.MUnitID,
                                                     IsDeduct = key.IsDeduct
                                                 }).ToList();
            }
            foreach (ExportBillDetail oItem in oExportBillDetailsTemp)
            {
                if (String.IsNullOrEmpty(oItem.BuyerReference))
                {
                    oItem.StyleNo = oItem.StyleNo;
                }
                else
                {
                    oItem.StyleNo = oItem.StyleNo + " " + oItem.BuyerReference;
                }
            }
            oExportBillDetailsTemp = oExportBillDetailsTemp.OrderBy(o => o.ProductID).ThenBy(a => a.Construction).ThenBy(a => a.ProcessTypeName).ThenBy(a => a.FabricWeaveName).ThenBy(a => a.FabricWidth).ThenBy(a => a.FinishTypeName).ThenBy(b => b.StyleNo).ThenBy(c => c.ColorInfo).ToList();


            foreach (ExportBillDetail oitem in oExportBillDetailsTemp)
            {
                nSL++;
                if (_oExportCommercialDoc.WeightPerBag <= 0)
                { _oExportCommercialDoc.WeightPerBag = 100; }
                if (!string.IsNullOrEmpty(oitem.Construction))
                {
                    _nTotalNoOfBag = _nTotalNoOfBag + Math.Floor(oitem.NoOfBag);
                }
                _oPdfPCell = new PdfPCell(new Phrase(_nTotalNoOfBag_PREV + "-" + (_nTotalNoOfBag).ToString(), _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);
                _nTotalNoOfBag_PREV =1+ _nTotalNoOfBag;

                if (nProductID != oitem.ProductID || sConstruction != oitem.Construction || sProcessTypeName != oitem.ProcessTypeName || sFabricWeaveName != oitem.FabricWeaveName || sFabricWidth != oitem.FabricWidth || sFinishTypeName != oitem.FinishTypeName)/// nProcessType != (int)oItem.ProcessType || nFabricWeave != (int)oItem.FabricWeave)
                {
                   
                    ////// Format B///
                    if (_oExportCommercialDoc.ProductPrintType == EnumExcellColumn.B)
                    {
                        sTemp = "";
                        sConst = "";
                        if (!string.IsNullOrEmpty(oitem.FabricWeaveName))
                        {
                            oitem.ProductName = "Comp:" + oitem.ProductName + ", Weave: " + oitem.FabricWeaveName;
                        }
                        if (!String.IsNullOrEmpty(oitem.FabricNo))
                        {
                            oitem.ProductName = "Article: " + oitem.FabricNo + "\n" + oitem.ProductName;
                        }
                        if (oitem.IsDeduct)
                        {
                            oitem.ProductName = oitem.ProductName + "(Deduct)";
                        }
                        if (!string.IsNullOrEmpty(oitem.Construction))
                        {
                            sConst = sConst + "Const: " + oitem.Construction;
                        }
                        if (!string.IsNullOrEmpty(oitem.FabricWidth))
                        {
                            sTemp = sTemp + "Width : " + oitem.FabricWidth;
                        }
                        if (!string.IsNullOrEmpty(oitem.Weight))
                        {
                            sTemp = sTemp + " Weight : " + oitem.Weight;
                        }
                        if (!String.IsNullOrEmpty(oitem.ProcessTypeName))
                        {
                            sTemp = sTemp + "\nProcess: " + oitem.ProcessTypeName;
                        }
                        if (!String.IsNullOrEmpty(oitem.FinishTypeName))
                        {
                            sTemp = sTemp + ", Finish: " + oitem.FinishTypeName;
                        }
                        if (!string.IsNullOrEmpty(oitem.Shrinkage))
                        {
                            sTemp = sTemp + "\nShrinkage: " + oitem.Shrinkage;
                        }
                        if (!string.IsNullOrEmpty(oitem.ProductDescription))
                        {
                            sTemp = sTemp + "\n" + oitem.ProductDescription;
                        }
                    }
                    else
                    {
                        sTemp = "";
                        sConst = "";
                        if (oitem.ProcessTypeName != "")
                        {
                            oitem.ProductName = oitem.ProductName + ", " + oitem.ProcessTypeName;
                        }
                        oitem.ProductName = oitem.ProductName + " Fabrics";
                        if (!string.IsNullOrEmpty(oitem.Construction))
                        {
                            sConst = sConst + "Const: " + oitem.Construction;
                        }

                        if (oitem.FabricWeaveName != "")
                        {
                            sTemp = sTemp + " " + oitem.FabricWeaveName;
                        }

                        if (!string.IsNullOrEmpty(oitem.FabricWidth))
                        {
                            sTemp = sTemp + ", Width : " + oitem.FabricWidth;
                        }
                        if (oitem.FinishTypeName != "")
                        {
                            sTemp = sTemp + ", Finish Type : " + oitem.FinishTypeName;
                        }
                    }
                    //////

                    _nCount_Raw = oExportBillDetailsTemp.Where(x => x.ProductID == oitem.ProductID && x.Construction == oitem.Construction && x.ProcessTypeName == oitem.ProcessTypeName && x.FabricWeaveName == oitem.FabricWeaveName && x.FabricWidth == oitem.FabricWidth && x.FinishTypeName == oitem.FinishTypeName && x.SLNo>=0).Count();
                   

                    _oPhrase = new Phrase();
                    _oPhrase.Add(new Chunk(oitem.ProductName + ",\n" + sConst + "\n" + sTemp, _oFontStyle));
                    _oPdfPCell = new PdfPCell(_oPhrase);
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.Rowspan = _nCount_Raw;
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.BorderWidthLeft = 0.5f;
                    _oPdfPCell.BorderWidthTop = 0.5f;
                    _oPdfPCell.BorderWidthRight = 0.5f;
                    _oPdfPCell.BorderWidthBottom = 0;
                    oPdfPTable1.AddCell(_oPdfPCell);
                }
                if (nProductID != oitem.ProductID || sConstruction != oitem.Construction || sProcessTypeName != oitem.ProcessTypeName || sFabricWeaveName != oitem.FabricWeaveName || sFabricWidth != oitem.FabricWidth || sFinishTypeName != oitem.FinishTypeName || sStyleNo != oitem.StyleNo)
                {
                    _nCount_Raw_Style = oExportBillDetailsTemp.Where(x => x.ProductID == oitem.ProductID && x.Construction == oitem.Construction && x.ProcessTypeName == oitem.ProcessTypeName && x.FabricWeaveName == oitem.FabricWeaveName && x.FabricWidth == oitem.FabricWidth && x.FinishTypeName == oitem.FinishTypeName && x.StyleNo == oitem.StyleNo && x.SLNo>=0).Count();
                   // _nCount_Raw_Style = oExportBillDetailsTemp.Where(x => x.ProductID == oitem.ProductID && x.Construction == oitem.Construction && x.StyleNo == oitem.StyleNo).Count();
                    _oPdfPCell = new PdfPCell(new Phrase(oitem.StyleNo , _oFontStyle));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f;
                    _oPdfPCell.Rowspan = _nCount_Raw_Style;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable1.AddCell(_oPdfPCell);
                }
                //if (nProductID != oitem.ProductID || sConstruction != oitem.Construction || sStyleNo != oitem.StyleNo)/// nProcessType != (int)oItem.ProcessType || nFabricWeave != (int)oItem.FabricWeave)
                //{
                    _oPdfPCell = new PdfPCell(new Phrase(oitem.ColorInfo, _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);
                
                _oPdfPCell = new PdfPCell(new Phrase( Global.MillionFormatActualDigit(System.Math.Round(oitem.Qty,2))+" "+_oExportCommercialDoc.MUName , _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

               //_oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.Currency + "" + System.Math.Round(oitem.UnitPrice, 4).ToString("#,##0.00##;(#,##0.00##)"), _oFontStyle));
            //   _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.Currency + "" + Global.MillionFormat(oitem.UnitPrice), _oFontStyle));
               _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.Currency + "" + System.Math.Round(oitem.UnitPrice, 4).ToString("#,##0.00##;(#,##0.00##)"), _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);
                if (oitem.IsDeduct)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("(" + _oExportCommercialDoc.Currency + "" + Global.MillionFormat(oitem.Qty * oitem.UnitPrice) + ")", _oFontStyle));
                    
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.Currency + "" + Global.MillionFormat(oitem.Qty * oitem.UnitPrice), _oFontStyle));
                }
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);


                oPdfPTable1.CompleteRow();
                _nTotalRowCount++;
               if (oitem.IsDeduct) { _nTotalValue = _nTotalValue - (oitem.Qty * oitem.UnitPrice); }
                else { _nTotalValue = _nTotalValue + (oitem.Qty * oitem.UnitPrice); }
               if (!string.IsNullOrEmpty(oitem.Construction))
               {
                   _nTotalQty = _nTotalQty + oitem.Qty;
                   _nTotalQtyKg = _nTotalQtyKg + oitem.QtyTwo;
                   //_nTotalNoOfBag = _nTotalNoOfBag + oitem.NoOfBag;
                   _nTotalWtPerBag = _nTotalWtPerBag + oitem.WtPerBag;
               }
                _sMUnit_Two = oitem.MUName;
                nProductID = oitem.ProductID;
                sConstruction = oitem.Construction;
                sProcessTypeName = oitem.ProcessTypeName;
                sFabricWeaveName = oitem.FabricWeaveName;
                sFabricWidth = oitem.FabricWidth;
                sFinishTypeName = oitem.FinishTypeName;
                sStyleNo = oitem.StyleNo;

                oitem.SLNo=-1;

                _nUsagesHeight = CalculatePdfPTableHeight(_oPdfPTable);
                _nUsagesHeight =_nUsagesHeight+ CalculatePdfPTableHeight(oPdfPTable1);
                if (_nUsagesHeight > 650)
                {
                  
                    _nCount_Raw=0;
                      _nCount_Raw_Style=0;
                      nProductID = 0;
                    _oPdfPCell = new PdfPCell(oPdfPTable1);
                  
                    _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();

                    _nUsagesHeight = 0;
                    _oDocument.Add(_oPdfPTable);
                    _oDocument.NewPage();
                   
                    oPdfPTable1.DeleteBodyRows();
                    _oPdfPTable.DeleteBodyRows();

                    this.HeaderWithThreeFormats(_oExportCommercialDoc.DocHeader);
                    this.ReportHeader(_oExportCommercialDoc.DocHeader);
                     oPdfPTable1 = new PdfPTable(7);
                    oPdfPTable1.SetWidths(new float[] { 70f, 180, 80f, 80f, 80f, 65f, 65f });
                
                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                    _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                }
            }

            if (!string.IsNullOrEmpty(_oExportCommercialDoc.ASPERPI))
            {
                _oExportCommercialDoc.ASPERPI = _oExportCommercialDoc.ASPERPI.Replace("@PINO", " " + _oExportCommercialDoc.PINos);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.ASPERPI, FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL)));
                _oPdfPCell.Colspan = 4; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);
                oPdfPTable1.CompleteRow();
            }

          
            #region Total

            _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 4; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(Math.Round(_nTotalQty,2))+" "+_oExportCommercialDoc.MUName , _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);
         
            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.Currency+""+Global.MillionFormat(_nTotalValue), _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);
           
            oPdfPTable1.CompleteRow();
            #endregion

            #region Claim Sattle
            if (_oExportCommercialDoc.ExportClaimSettles.Count > 0)
            {
                foreach (ExportClaimSettle oitemClaim in _oExportCommercialDoc.ExportClaimSettles)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oitemClaim.SettleName + " ", _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 5; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

                    //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

                    //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

                    //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);


                    //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);
                    if (oitemClaim.InOutType == EnumInOutType.Receive) { _nTotalValue = _nTotalValue + oitemClaim.Amount; } else { _nTotalValue = _nTotalValue - oitemClaim.Amount; }

                    _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.Currency + "" + Global.MillionFormat(oitemClaim.Amount), _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

                    oPdfPTable1.CompleteRow();
                }

                #region Total

                _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 4; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.Currency + "" + Global.MillionFormat(_nTotalValue), _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

                oPdfPTable1.CompleteRow();
                #endregion
            }
            #endregion  Claim Sattle

            #region AmountInWord
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.AmountInWord))
            {
                _oPhrase = new Phrase();
                _oPhrase.Add(new Chunk(_oExportCommercialDoc.AmountInWord + ": ", _oFontStyle));
                _oPhrase.Add(new Chunk("US "+Global.DollarWords(_nTotalValue), _oFontStyleBold));
                _oPdfPCell = new PdfPCell(_oPhrase);
                _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 7; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);
                oPdfPTable1.CompleteRow();
            }
            #endregion
          
            #endregion We

            return oPdfPTable1;
        }
        private PdfPTable ProductDetails_WU_UP_Amount()
        {
            #region Weaving
            PdfPTable oPdfPTable1 = new PdfPTable(6);
            oPdfPTable1.SetWidths(new float[] { 180, 80f, 80f, 80f, 65f, 65f });


            List<ExportBillDetail> oExportBillDetails = new List<ExportBillDetail>();
            List<ExportBillDetail> oExportBillDetailsTemp = new List<ExportBillDetail>();
            oExportBillDetails = _oExportCommercialDoc.ExportBillDetails;




            //_oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.MarkSAndNos, _oFontStyleBold));
            //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.DescriptionOfGoods, _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.Header_Style, _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.Header_Color, _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.QtyInKg, _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.UnitPriceDes, _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.ValueDes, _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);


            oPdfPTable1.CompleteRow();

            //_oPdfPCell = new PdfPCell(new Phrase("ROLL No", _oFontStyleBold));
            //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase("FABRICS FOR 100% EXPORT ORIENTED READYMADE GARMENTS INDUSTRY", FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.ITALIC)));
            _oPdfPCell.Colspan = 3; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.CTPApplicant, FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.ITALIC)));
            _oPdfPCell.Colspan = 3; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

            oPdfPTable1.CompleteRow();

            int nSL = 0;
            _nTotalRowCount = 0;
            var sTemp = "";
            string sConstruction = "";
            string sConst = "";
            int nProductID = 0;
            string sProcessTypeName = "";
            string sFabricWeaveName = "";
            string sFabricWidth = "";
            string sFinishTypeName = "";
            int _nCount_Raw = 0;
            int _nCount_Raw_Style = 0;
            string sStyleNo = "";
             if (_oExportCommercialDoc.ProductPrintType == EnumExcellColumn.B)
            {
                oExportBillDetailsTemp = oExportBillDetails.GroupBy(x => new { x.ProductID, x.ProductName, x.FabricNo, x.Construction, x.ProcessTypeName, x.FabricWeaveName, x.FabricWidth, x.FinishTypeName, x.StyleNo, x.BuyerReference, x.Shrinkage, x.Weight, x.ColorInfo, x.UnitPrice, x.MUnitID, x.IsDeduct }, (key, grp) =>
                                                  new ExportBillDetail
                                                  {
                                                      ProductID = key.ProductID,
                                                      ProductName = key.ProductName,
                                                      FabricNo = key.FabricNo,
                                                      Construction = key.Construction,
                                                      ProcessTypeName = key.ProcessTypeName,
                                                      FabricWeaveName = key.FabricWeaveName,
                                                      FabricWidth = key.FabricWidth,
                                                      FinishTypeName = key.FinishTypeName,
                                                      StyleNo = key.StyleNo,
                                                      BuyerReference = key.BuyerReference,
                                                      Shrinkage = key.Shrinkage,
                                                      Weight = key.Weight,
                                                      ColorInfo = key.ColorInfo,
                                                      Qty = grp.Sum(p => p.Qty),
                                                      NoOfBag = grp.Sum(p => p.NoOfBag),
                                                      WtPerBag = grp.Sum(p => p.WtPerBag),
                                                      UnitPrice = key.UnitPrice,
                                                      MUnitID = key.MUnitID,
                                                      IsDeduct = key.IsDeduct
                                                  }).ToList();
            }
            else
            {

            oExportBillDetailsTemp = oExportBillDetails.GroupBy(x => new { x.ProductID, x.ProductName, x.Construction, x.ProcessTypeName, x.FabricWeaveName, x.FabricWidth, x.FinishTypeName, x.StyleNo, x.ColorInfo, x.BuyerReference, x.UnitPrice, x.MUnitID,x.IsDeduct }, (key, grp) =>
                                              new ExportBillDetail
                                              {
                                                  ProductID = key.ProductID,
                                                  ProductName = key.ProductName,
                                                  Construction = key.Construction,
                                                  ProcessTypeName = key.ProcessTypeName,
                                                  FabricWeaveName = key.FabricWeaveName,
                                                  FabricWidth = key.FabricWidth,
                                                  FinishTypeName = key.FinishTypeName,
                                                  StyleNo = key.StyleNo,
                                                  ColorInfo = key.ColorInfo,
                                                  BuyerReference = key.BuyerReference,
                                                  Qty = grp.Sum(p => p.Qty),
                                                  NoOfBag = grp.Sum(p => p.NoOfBag),
                                                  WtPerBag = grp.Sum(p => p.WtPerBag),
                                                  UnitPrice = key.UnitPrice,
                                                  MUnitID = key.MUnitID,
                                                  IsDeduct = key.IsDeduct
                                              }).ToList();
             }
            foreach (ExportBillDetail oItem in oExportBillDetailsTemp)
            {
                if (String.IsNullOrEmpty(oItem.BuyerReference))
                {
                    oItem.StyleNo = oItem.StyleNo;
                }
                else
                {
                    oItem.StyleNo = oItem.StyleNo + " " + oItem.BuyerReference;
                }
            }
            oExportBillDetailsTemp = oExportBillDetailsTemp.OrderBy(o => o.ProductID).ThenBy(a => a.Construction).ThenBy(a => a.ProcessTypeName).ThenBy(a => a.FabricWeaveName).ThenBy(a => a.FabricWidth).ThenBy(a => a.FinishTypeName).ThenBy(b => b.StyleNo).ThenBy(c => c.ColorInfo).ToList();


            foreach (ExportBillDetail oitem in oExportBillDetailsTemp)
            {
                nSL++;
                if (_oExportCommercialDoc.WeightPerBag <= 0)
                { _oExportCommercialDoc.WeightPerBag = 100; }
                if (!string.IsNullOrEmpty(oitem.Construction))
                {
                    _nTotalNoOfBag = _nTotalNoOfBag + Math.Floor(oitem.NoOfBag);
                }
                //_oPdfPCell = new PdfPCell(new Phrase(_nTotalNoOfBag_PREV + "-" + (_nTotalNoOfBag).ToString(), _oFontStyle));
                //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);
                _nTotalNoOfBag_PREV = 1 + _nTotalNoOfBag;

                if (nProductID != oitem.ProductID || sConstruction != oitem.Construction || sProcessTypeName != oitem.ProcessTypeName || sFabricWeaveName != oitem.FabricWeaveName || sFabricWidth != oitem.FabricWidth || sFinishTypeName != oitem.FinishTypeName)/// nProcessType != (int)oItem.ProcessType || nFabricWeave != (int)oItem.FabricWeave)
                //if (nProductID != oitem.ProductID || sConstruction != oitem.Construction)/// nProcessType != (int)oItem.ProcessType || nFabricWeave != (int)oItem.FabricWeave)
                {
                  
                    if (_oExportCommercialDoc.ProductPrintType == EnumExcellColumn.B)
                    {
                        sTemp = "";
                        sConst = "";
                        if (!string.IsNullOrEmpty(oitem.FabricWeaveName))
                        {
                            oitem.ProductName = "Comp:" + oitem.ProductName + ", Weave: " + oitem.FabricWeaveName;
                        }
                        if (!String.IsNullOrEmpty(oitem.FabricNo))
                        {
                            oitem.ProductName = "Article: " + oitem.FabricNo + "\n" + oitem.ProductName;
                        }
                        if (oitem.IsDeduct)
                        {
                            oitem.ProductName = oitem.ProductName + "(Deduct)";
                        }
                        if (!string.IsNullOrEmpty(oitem.Construction))
                        {
                            sConst = sConst + "Const: " + oitem.Construction;
                        }
                        if (!string.IsNullOrEmpty(oitem.FabricWidth))
                        {
                            sTemp = sTemp + "Width : " + oitem.FabricWidth;
                        }
                        if (!string.IsNullOrEmpty(oitem.Weight))
                        {
                            sTemp = sTemp + " Weight : " + oitem.Weight;
                        }
                        if (!String.IsNullOrEmpty(oitem.ProcessTypeName))
                        {
                            sTemp = sTemp + "\nProcess: " + oitem.ProcessTypeName;
                        }
                        if (!String.IsNullOrEmpty(oitem.FinishTypeName))
                        {
                            sTemp = sTemp + ", Finish: " + oitem.FinishTypeName;
                        }
                        if (!string.IsNullOrEmpty(oitem.Shrinkage))
                        {
                            sTemp = sTemp + "\nShrinkage: " + oitem.Shrinkage;
                        }
                        if (!string.IsNullOrEmpty(oitem.ProductDescription))
                        {
                            sTemp = sTemp + "\n" + oitem.ProductDescription;
                        }
                    }
                    else
                    {
                        sTemp = "";
                        sConst = "";
                        if (oitem.ProcessTypeName != "")
                        {
                            oitem.ProductName = oitem.ProductName + ", " + oitem.ProcessTypeName;
                        }
                        oitem.ProductName = oitem.ProductName + " Fabrics";
                        if (!string.IsNullOrEmpty(oitem.Construction))
                        {
                            sConst = sConst + "Const: " + oitem.Construction;
                        }

                        if (oitem.FabricWeaveName != "")
                        {
                            sTemp = sTemp + " " + oitem.FabricWeaveName;
                        }

                        if (!string.IsNullOrEmpty(oitem.FabricWidth))
                        {
                            sTemp = sTemp + ", Width : " + oitem.FabricWidth;
                        }
                        if (oitem.FinishTypeName != "")
                        {
                            sTemp = sTemp + ", Finish Type : " + oitem.FinishTypeName;
                        }
                    }
                    _nCount_Raw = oExportBillDetailsTemp.Where(x => x.ProductID == oitem.ProductID && x.Construction == oitem.Construction && x.ProcessTypeName == oitem.ProcessTypeName && x.FabricWeaveName == oitem.FabricWeaveName && x.FabricWidth == oitem.FabricWidth && x.FinishTypeName == oitem.FinishTypeName && x.SLNo >= 0).Count();


                    _oPhrase = new Phrase();
                    _oPhrase.Add(new Chunk(oitem.ProductName + ",\n" + sConst + "\n" + sTemp, _oFontStyle));
                    _oPdfPCell = new PdfPCell(_oPhrase);
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.Rowspan = _nCount_Raw;
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.BorderWidthLeft = 0.5f;
                    _oPdfPCell.BorderWidthTop = 0.5f;
                    _oPdfPCell.BorderWidthRight = 0.5f;
                    _oPdfPCell.BorderWidthBottom = 0;
                    oPdfPTable1.AddCell(_oPdfPCell);
                }
                if (nProductID != oitem.ProductID || sConstruction != oitem.Construction || sProcessTypeName != oitem.ProcessTypeName || sFabricWeaveName != oitem.FabricWeaveName || sFabricWidth != oitem.FabricWidth || sFinishTypeName != oitem.FinishTypeName || sStyleNo != oitem.StyleNo)
                {
                    _nCount_Raw_Style = oExportBillDetailsTemp.Where(x => x.ProductID == oitem.ProductID && x.Construction == oitem.Construction && x.ProcessTypeName == oitem.ProcessTypeName && x.FabricWeaveName == oitem.FabricWeaveName && x.FabricWidth == oitem.FabricWidth && x.FinishTypeName == oitem.FinishTypeName && x.StyleNo == oitem.StyleNo && x.SLNo >= 0).Count();
                    // _nCount_Raw_Style = oExportBillDetailsTemp.Where(x => x.ProductID == oitem.ProductID && x.Construction == oitem.Construction && x.StyleNo == oitem.StyleNo).Count();
                    _oPdfPCell = new PdfPCell(new Phrase(oitem.StyleNo, _oFontStyle));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f;
                    _oPdfPCell.Rowspan = _nCount_Raw_Style;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable1.AddCell(_oPdfPCell);
                }
                //if (nProductID != oitem.ProductID || sConstruction != oitem.Construction || sStyleNo != oitem.StyleNo)/// nProcessType != (int)oItem.ProcessType || nFabricWeave != (int)oItem.FabricWeave)
                //{
                _oPdfPCell = new PdfPCell(new Phrase(oitem.ColorInfo, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(System.Math.Round(oitem.Qty, 2)) + " " + _oExportCommercialDoc.MUName, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

              
                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.Currency + "" + System.Math.Round(oitem.UnitPrice, 4).ToString("#,##0.00##;(#,##0.00##)"), _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);
                if (oitem.IsDeduct)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("(" + _oExportCommercialDoc.Currency + "" + Global.MillionFormat(oitem.Qty * oitem.UnitPrice) + ")", _oFontStyle));

                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.Currency + "" + Global.MillionFormat(oitem.Qty * oitem.UnitPrice), _oFontStyle));
                }
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);


                oPdfPTable1.CompleteRow();
                _nTotalRowCount++;
                if (oitem.IsDeduct) { _nTotalValue = _nTotalValue - (oitem.Qty * oitem.UnitPrice); }
                else { _nTotalValue = _nTotalValue + (oitem.Qty * oitem.UnitPrice); }
                if (!string.IsNullOrEmpty(oitem.Construction))
                {
                    
                    _nTotalQty = _nTotalQty + oitem.Qty;
                    _nTotalQtyKg = _nTotalQtyKg + oitem.QtyTwo;
                    //_nTotalNoOfBag = _nTotalNoOfBag + oitem.NoOfBag;
                    _nTotalWtPerBag = _nTotalWtPerBag + oitem.WtPerBag;
                }
                _sMUnit_Two = oitem.MUName;
                nProductID = oitem.ProductID;
                sConstruction = oitem.Construction;
                sProcessTypeName = oitem.ProcessTypeName;
                sFabricWeaveName = oitem.FabricWeaveName;
                sFabricWidth = oitem.FabricWidth;
                sFinishTypeName = oitem.FinishTypeName;
                sStyleNo = oitem.StyleNo;
                // For Page Brack
                #region 
                oitem.SLNo = -1;
                _nUsagesHeight = CalculatePdfPTableHeight(_oPdfPTable);
                _nUsagesHeight = _nUsagesHeight + CalculatePdfPTableHeight(oPdfPTable1);
               
                if (_nUsagesHeight > 700)
                {
                    nSL--;
                    _nCount_Raw = 0;
                    _nCount_Raw_Style = 0;
                    nProductID = 0;
                    _oPdfPCell = new PdfPCell(oPdfPTable1);

                    _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();

                    _nUsagesHeight = 0;
                    _oDocument.Add(_oPdfPTable);
                    _oDocument.NewPage();

                    oPdfPTable1.DeleteBodyRows();
                    _oPdfPTable.DeleteBodyRows();

                    this.HeaderWithThreeFormats(_oExportCommercialDoc.DocHeader);
                    this.ReportHeader(_oExportCommercialDoc.DocHeader);
                     oPdfPTable1 = new PdfPTable(6);
                    oPdfPTable1.SetWidths(new float[] { 180, 80f, 80f, 80f, 65f, 65f });

                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                    _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

                }
                #endregion
            }

            if (!string.IsNullOrEmpty(_oExportCommercialDoc.ASPERPI))
            {
                _oExportCommercialDoc.ASPERPI = _oExportCommercialDoc.ASPERPI.Replace("@PINO", " " + _oExportCommercialDoc.PINos);

                //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.ASPERPI, FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL)));
                _oPdfPCell.Colspan = 4; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);
                oPdfPTable1.CompleteRow();
            }


            #region Total

            _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 3; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(Math.Round(_nTotalQty, 2)) + " " + _oExportCommercialDoc.MUName, _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.Currency + "" + Global.MillionFormat(_nTotalValue), _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

            oPdfPTable1.CompleteRow();
            #endregion
            #region Claim Sattle
            if (_oExportCommercialDoc.ExportClaimSettles.Count > 0)
            {
                foreach (ExportClaimSettle oitemClaim in _oExportCommercialDoc.ExportClaimSettles)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oitemClaim.SettleName + " ", _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 4; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

                    //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

                    //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

                    //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);


                    //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);
                    if (oitemClaim.InOutType == EnumInOutType.Receive) { _nTotalValue = _nTotalValue + oitemClaim.Amount; } else { _nTotalValue = _nTotalValue - oitemClaim.Amount; }

                    _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.Currency + "" + Global.MillionFormat(oitemClaim.Amount), _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

                    oPdfPTable1.CompleteRow();
                }

                #region Total

                _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 3; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.Currency + "" + Global.MillionFormat(_nTotalValue), _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

                oPdfPTable1.CompleteRow();
                #endregion
            }
            #endregion  Claim Sattle

            #region AmountInWord
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.AmountInWord))
            {
                _oPhrase = new Phrase();
                _oPhrase.Add(new Chunk(_oExportCommercialDoc.AmountInWord + ": ", _oFontStyle));
                _oPhrase.Add(new Chunk("US " + Global.DollarWords(_nTotalValue), _oFontStyleBold));
                _oPdfPCell = new PdfPCell(_oPhrase);
                _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 6; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);
                oPdfPTable1.CompleteRow();
            }
            #endregion

            #endregion We

            return oPdfPTable1;
        }
        private PdfPTable ProductDetails_WU_Qty()
        {
            #region Weaving
            PdfPTable oPdfPTable1 = new PdfPTable(6);
            oPdfPTable1.SetWidths(new float[] {30f, 180,100f,100f, 80f, 60f});


            List<ExportBillDetail> oExportBillDetails = new List<ExportBillDetail>();
            List<ExportBillDetail> oExportBillDetailsTemp = new List<ExportBillDetail>();
            oExportBillDetails = _oExportCommercialDoc.ExportBillDetails;
         
         
            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.SL, _oFontStyleBold));
            _oPdfPCell.Rowspan = 2; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.DescriptionOfGoods, _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.Header_Style, _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.Header_Color, _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.QtyInKg, _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);
         
            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.Bag_Name, _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);


            oPdfPTable1.CompleteRow();

            //_oPdfPCell = new PdfPCell(new Phrase("ROLL No", _oFontStyleBold));
            //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase("FABRICS FOR 100% EXPORT ORIENTED READYMADE GARMENTS INDUSTRY",  FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.ITALIC)));
            _oPdfPCell.Colspan = 3; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);
            
            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.CTPApplicant, FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.ITALIC)));
            _oPdfPCell.Colspan = 2; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

            oPdfPTable1.CompleteRow();

            int nSL = 0;
            _nTotalRowCount = 0;
            var sTemp = "";
            string sConstruction = "";
            string sConst = "";
            int nProductID = 0;
            string sProcessTypeName = "";
            string sFabricWeaveName = "";
            string sFabricWidth = "";
            string sFinishTypeName = "";
            int _nCount_Raw = 0;
            int _nCount_Raw_Style = 0;
            string sStyleNo = "";
             if (_oExportCommercialDoc.ProductPrintType == EnumExcellColumn.B)
            {
                oExportBillDetailsTemp = oExportBillDetails.GroupBy(x => new { x.ProductID, x.ProductName, x.FabricNo, x.Construction, x.ProcessTypeName, x.FabricWeaveName, x.FabricWidth, x.FinishTypeName, x.StyleNo, x.BuyerReference, x.Shrinkage, x.Weight, x.ColorInfo, x.UnitPrice, x.MUnitID, x.IsDeduct }, (key, grp) =>
                                                  new ExportBillDetail
                                                  {
                                                      ProductID = key.ProductID,
                                                      ProductName = key.ProductName,
                                                      FabricNo = key.FabricNo,
                                                      Construction = key.Construction,
                                                      ProcessTypeName = key.ProcessTypeName,
                                                      FabricWeaveName = key.FabricWeaveName,
                                                      FabricWidth = key.FabricWidth,
                                                      FinishTypeName = key.FinishTypeName,
                                                      StyleNo = key.StyleNo,
                                                      BuyerReference = key.BuyerReference,
                                                      Shrinkage = key.Shrinkage,
                                                      Weight = key.Weight,
                                                      ColorInfo = key.ColorInfo,
                                                      Qty = grp.Sum(p => p.Qty),
                                                      NoOfBag = grp.Sum(p => p.NoOfBag),
                                                      WtPerBag = grp.Sum(p => p.WtPerBag),
                                                      UnitPrice = key.UnitPrice,
                                                      MUnitID = key.MUnitID,
                                                      IsDeduct = key.IsDeduct
                                                  }).ToList();
            }
            else
            {

            oExportBillDetailsTemp = oExportBillDetails.GroupBy(x => new { x.ProductID, x.ProductName, x.Construction, x.ProcessTypeName, x.FabricWeaveName, x.FabricWidth, x.FinishTypeName, x.StyleNo, x.BuyerReference, x.ColorInfo, x.UnitPrice, x.MUnitID, x.IsDeduct }, (key, grp) =>
                                              new ExportBillDetail
                                              {
                                                  ProductID = key.ProductID,
                                                  ProductName = key.ProductName,
                                                  Construction = key.Construction,
                                                  ProcessTypeName = key.ProcessTypeName,
                                                  FabricWeaveName = key.FabricWeaveName,
                                                  FabricWidth = key.FabricWidth,
                                                  FinishTypeName = key.FinishTypeName,
                                                  StyleNo = key.StyleNo,
                                                  BuyerReference = key.BuyerReference,
                                                  ColorInfo = key.ColorInfo,
                                                  Qty = grp.Sum(p => p.Qty),
                                                  NoOfBag = grp.Sum(p => p.NoOfBag),
                                                  WtPerBag = grp.Sum(p => p.WtPerBag),
                                                  UnitPrice = key.UnitPrice,
                                                  MUnitID = key.MUnitID,
                                                  IsDeduct = key.IsDeduct,
                                              }).ToList();
             }
            foreach (ExportBillDetail oItem in oExportBillDetailsTemp)
            {
                if (String.IsNullOrEmpty(oItem.BuyerReference))
                {
                    oItem.StyleNo = oItem.StyleNo;
                }
                else
                {
                    oItem.StyleNo = oItem.StyleNo + " " + oItem.BuyerReference;
                }
            }
            oExportBillDetailsTemp = oExportBillDetailsTemp.OrderBy(o => o.ProductID).ThenBy(a => a.Construction).ThenBy(a => a.ProcessTypeName).ThenBy(a => a.FabricWeaveName).ThenBy(a => a.FabricWidth).ThenBy(a => a.FinishTypeName).ThenBy(b => b.StyleNo).ThenBy(c => c.ColorInfo).ToList();


            foreach (ExportBillDetail oitem in oExportBillDetailsTemp)
            {
                //if (_oExportCommercialDoc.WeightPerBag <= 0)
                //{ _oExportCommercialDoc.WeightPerBag = 100; }
                if (!string.IsNullOrEmpty(oitem.Construction))
                {
                    _nTotalNoOfBag = _nTotalNoOfBag + Math.Floor(oitem.NoOfBag);
                   
                }

                if (nProductID != oitem.ProductID || sConstruction != oitem.Construction || sProcessTypeName != oitem.ProcessTypeName || sFabricWeaveName != oitem.FabricWeaveName || sFabricWidth != oitem.FabricWidth || sFinishTypeName != oitem.FinishTypeName)/// nProcessType != (int)oItem.ProcessType || nFabricWeave != (int)oItem.FabricWeave)
            //    if (nProductID != oitem.ProductID || sConstruction != oitem.Construction)/// nProcessType != (int)oItem.ProcessType || nFabricWeave != (int)oItem.FabricWeave)
                {

                    nSL++;

                    if (_oExportCommercialDoc.ProductPrintType == EnumExcellColumn.B)
                    {
                        sTemp = "";
                        sConst = "";
                        if (!string.IsNullOrEmpty(oitem.FabricWeaveName))
                        {
                            oitem.ProductName = "Comp:" + oitem.ProductName + ", Weave: " + oitem.FabricWeaveName;
                        }
                        if (!String.IsNullOrEmpty(oitem.FabricNo))
                        {
                            oitem.ProductName = "Article: " + oitem.FabricNo + "\n" + oitem.ProductName;
                        }
                        if (oitem.IsDeduct)
                        {
                            oitem.ProductName = oitem.ProductName + "(Deduct)";
                        }
                        if (!string.IsNullOrEmpty(oitem.Construction))
                        {
                            sConst = sConst + "Const: " + oitem.Construction;
                        }
                        if (!string.IsNullOrEmpty(oitem.FabricWidth))
                        {
                            sTemp = sTemp + "Width : " + oitem.FabricWidth;
                        }
                        if (!string.IsNullOrEmpty(oitem.Weight))
                        {
                            sTemp = sTemp + " Weight : " + oitem.Weight;
                        }
                        if (!String.IsNullOrEmpty(oitem.ProcessTypeName))
                        {
                            sTemp = sTemp + "\nProcess: " + oitem.ProcessTypeName;
                        }
                        if (!String.IsNullOrEmpty(oitem.FinishTypeName))
                        {
                            sTemp = sTemp + ", Finish: " + oitem.FinishTypeName;
                        }
                        if (!string.IsNullOrEmpty(oitem.Shrinkage))
                        {
                            sTemp = sTemp + "\nShrinkage: " + oitem.Shrinkage;
                        }
                        if (!string.IsNullOrEmpty(oitem.ProductDescription))
                        {
                            sTemp = sTemp + "\n" + oitem.ProductDescription;
                        }
                    }
                    else
                    {
                        sTemp = "";
                        sConst = "";
                        if (oitem.ProcessTypeName != "")
                        {
                            oitem.ProductName = oitem.ProductName + ", " + oitem.ProcessTypeName;
                        }
                        oitem.ProductName = oitem.ProductName + " Fabrics";
                        if (!string.IsNullOrEmpty(oitem.Construction))
                        {
                            sConst = sConst + "Const: " + oitem.Construction;
                        }

                        if (oitem.FabricWeaveName != "")
                        {
                            sTemp = sTemp + " " + oitem.FabricWeaveName;
                        }

                        if (!string.IsNullOrEmpty(oitem.FabricWidth))
                        {
                            sTemp = sTemp + ", Width : " + oitem.FabricWidth;
                        }
                        if (oitem.FinishTypeName != "")
                        {
                            sTemp = sTemp + ", Finish Type : " + oitem.FinishTypeName;
                        }
                    }
                    _nCount_Raw = oExportBillDetailsTemp.Where(x => x.ProductID == oitem.ProductID && x.ProcessTypeName == oitem.ProcessTypeName && x.Construction == oitem.Construction && x.FabricWeaveName == oitem.FabricWeaveName && x.FabricWidth == oitem.FabricWidth && x.FinishTypeName == oitem.FinishTypeName && x.SLNo >= 0).Count();
                    
                    _oPdfPCell = new PdfPCell(new Phrase(nSL.ToString(), _oFontStyle));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f;
                    _oPdfPCell.BorderWidthTop = 0.5f;
                    _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.BorderWidthBottom = 0;
                    _oPdfPCell.Rowspan = _nCount_Raw;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable1.AddCell(_oPdfPCell);

                    _oPhrase = new Phrase();
                    _oPhrase.Add(new Chunk(oitem.ProductName + ",\n" + sConst + "\n" + sTemp, _oFontStyle));
                    _oPdfPCell = new PdfPCell(_oPhrase);
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.Rowspan = _nCount_Raw;
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.BorderWidthLeft = 0.5f;
                    _oPdfPCell.BorderWidthTop = 0.5f;
                    _oPdfPCell.BorderWidthRight = 0.5f;
                    _oPdfPCell.BorderWidthBottom = 0;
                    oPdfPTable1.AddCell(_oPdfPCell);
                }
                if (nProductID != oitem.ProductID || sConstruction != oitem.Construction || sProcessTypeName != oitem.ProcessTypeName || sFabricWeaveName != oitem.FabricWeaveName || sFabricWidth != oitem.FabricWidth || sFinishTypeName != oitem.FinishTypeName || sStyleNo != oitem.StyleNo)
                {
                    _nCount_Raw_Style = oExportBillDetailsTemp.Where(x => x.ProductID == oitem.ProductID && x.Construction == oitem.Construction && x.ProcessTypeName == oitem.ProcessTypeName && x.FabricWeaveName == oitem.FabricWeaveName && x.FabricWidth == oitem.FabricWidth && x.FinishTypeName == oitem.FinishTypeName && x.StyleNo == oitem.StyleNo && x.SLNo >= 0).Count();
                    
                    _oPdfPCell = new PdfPCell(new Phrase(oitem.StyleNo, _oFontStyle));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f;
                    _oPdfPCell.Rowspan = _nCount_Raw_Style;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable1.AddCell(_oPdfPCell);
                }
              
                    _oPdfPCell = new PdfPCell(new Phrase(oitem.ColorInfo, _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

                    if (oitem.IsDeduct)
                    {
                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(oitem.Qty) , _oFontStyle));
                    }
                    else
                    { _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(oitem.Qty) + " " + _oExportCommercialDoc.MUName, _oFontStyle)); }
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((Math.Floor(oitem.NoOfBag)).ToString(), _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

               

                oPdfPTable1.CompleteRow();
                _nTotalRowCount++;
                if (oitem.IsDeduct) { _nTotalValue = _nTotalValue - (oitem.Qty * oitem.UnitPrice); }
                else { _nTotalValue = _nTotalValue + (oitem.Qty * oitem.UnitPrice); }
                if (!string.IsNullOrEmpty(oitem.Construction))
                {
                    _nTotalQty = _nTotalQty + oitem.Qty;
                    _nTotalQtyKg = _nTotalQtyKg + oitem.QtyTwo;
                    //_nTotalNoOfBag = _nTotalNoOfBag + oitem.NoOfBag;
                    _nTotalWtPerBag = _nTotalWtPerBag + oitem.WtPerBag;
                }
                //_nTotalValue = _nTotalValue + (oitem.Qty * oitem.UnitPrice);
                //_nTotalQty = _nTotalQty + oitem.Qty;
                //_nTotalQtyKg = _nTotalQtyKg + oitem.QtyTwo;
                ////_nTotalNoOfBag = _nTotalNoOfBag + oitem.NoOfBag;
                //_nTotalWtPerBag = _nTotalWtPerBag + oitem.WtPerBag;
                _sMUnit_Two = oitem.MUName;
                nProductID = oitem.ProductID;
                sConstruction = oitem.Construction;
                sProcessTypeName = oitem.ProcessTypeName;
                sFabricWeaveName = oitem.FabricWeaveName;
                sFabricWidth = oitem.FabricWidth;
                sFinishTypeName = oitem.FinishTypeName;
                sStyleNo = oitem.StyleNo;
                #region
                oitem.SLNo = -1;
                _nUsagesHeight = CalculatePdfPTableHeight(_oPdfPTable);
                _nUsagesHeight = _nUsagesHeight+CalculatePdfPTableHeight(oPdfPTable1);
                if (_nUsagesHeight > 700)
                {
                    nSL--;
                    _nCount_Raw = 0;
                    _nCount_Raw_Style = 0;
                    nProductID = 0;
                    _oPdfPCell = new PdfPCell(oPdfPTable1);

                    _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();

                    _nUsagesHeight = 0;
                    _oDocument.Add(_oPdfPTable);
                    _oDocument.NewPage();

                    oPdfPTable1.DeleteBodyRows();
                    _oPdfPTable.DeleteBodyRows();

                    this.HeaderWithThreeFormats(_oExportCommercialDoc.DocHeader);
                    this.ReportHeader(_oExportCommercialDoc.DocHeader);
                    oPdfPTable1 = new PdfPTable(6);
                    oPdfPTable1.SetWidths(new float[] { 30f, 180, 100f, 100f, 80f, 60f });
                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                    _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

                }
                #endregion
            }

            if (!string.IsNullOrEmpty(_oExportCommercialDoc.ASPERPI))
            {
                _oExportCommercialDoc.ASPERPI = _oExportCommercialDoc.ASPERPI.Replace("@PINO", " " + _oExportCommercialDoc.PINos);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.ASPERPI, FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL)));
                _oPdfPCell.Colspan = 3; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);
                oPdfPTable1.CompleteRow();
            }
            #region Total

            _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 4; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(_nTotalQty) + "" + _oExportCommercialDoc.MUName, _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_nTotalNoOfBag.ToString(), _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.Currency + "" + Global.MillionFormat(_nTotalValue), _oFontStyleBold));
            //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

            oPdfPTable1.CompleteRow();
            #endregion

            
            #endregion We

            return oPdfPTable1;
        }
        private PdfPTable ProductDetails_WU_Qty_PackingListDetail()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

            #region Weaving
            PdfPTable oPdfPTable1 = new PdfPTable(8);
            oPdfPTable1.SetWidths(new float[] { 30f, 180, 100f, 100f, 60f, 40f,50f,50f });

            List<ExportBillDetail> oExportBillDetails = new List<ExportBillDetail>();
            List<ExportBillDetail> oExportBillDetailsTemp = new List<ExportBillDetail>();
            oExportBillDetails = _oExportCommercialDoc.ExportBillDetails;


            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.SL, _oFontStyleBold));
            _oPdfPCell.Rowspan = 2; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.DescriptionOfGoods, _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.Header_Style, _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.Header_Color, _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.QtyInKg, _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.Bag_Name, _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.NetWeight, _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.GrossWeight, _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);


            oPdfPTable1.CompleteRow();

            //_oPdfPCell = new PdfPCell(new Phrase("ROLL No", _oFontStyleBold));
            //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase("FABRICS FOR 100% EXPORT ORIENTED READYMADE GARMENTS INDUSTRY", FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.ITALIC)));
            _oPdfPCell.Colspan = 3; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.CTPApplicant, FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.ITALIC)));
            _oPdfPCell.Colspan = 2; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

            oPdfPTable1.CompleteRow();

            int nSL = 0;
            _nTotalRowCount = 0;
            var sTemp = "";
          
            string sConst = "";
            int nProductID = 0;
            string sConstruction = "";
            string sProcessTypeName = "";
            string sFabricWeaveName = "";
            string sFabricWidth = "";
            string sFinishTypeName = "";
            int _nCount_Raw = 0;
            int _nCount_Raw_Style = 0;
            string sStyleNo = "";
            if (_oExportCommercialDoc.ProductPrintType == EnumExcellColumn.B)
            {
                oExportBillDetailsTemp = oExportBillDetails.GroupBy(x => new { x.ProductID, x.ProductName,x.FabricNo, x.Construction, x.ProcessTypeName, x.FabricWeaveName, x.FabricWidth, x.FinishTypeName, x.StyleNo, x.BuyerReference, x.Shrinkage, x.Weight, x.ColorInfo, x.UnitPrice, x.MUnitID,x.IsDeduct }, (key, grp) =>
                                                  new ExportBillDetail
                                                  {
                                                      ProductID = key.ProductID,
                                                      ProductName = key.ProductName,
                                                      FabricNo = key.FabricNo,
                                                      Construction = key.Construction,
                                                      ProcessTypeName = key.ProcessTypeName,
                                                      FabricWeaveName = key.FabricWeaveName,
                                                      FabricWidth = key.FabricWidth,
                                                      FinishTypeName = key.FinishTypeName,
                                                      StyleNo = key.StyleNo,
                                                      BuyerReference = key.BuyerReference,
                                                      Shrinkage = key.Shrinkage,
                                                      Weight = key.Weight,
                                                      ColorInfo = key.ColorInfo,
                                                      Qty = grp.Sum(p => p.Qty),
                                                      NoOfBag = grp.Sum(p => p.NoOfBag),
                                                      WtPerBag = grp.Sum(p => p.WtPerBag),
                                                      UnitPrice = key.UnitPrice,
                                                      MUnitID = key.MUnitID,
                                                      IsDeduct = key.IsDeduct
                                                  }).ToList();
            }
            else
            {

                oExportBillDetailsTemp = oExportBillDetails.GroupBy(x => new { x.ProductID, x.ProductName, x.Construction, x.ProcessTypeName, x.FabricWeaveName, x.FabricWidth, x.FinishTypeName, x.StyleNo, x.BuyerReference, x.ColorInfo, x.UnitPrice, x.MUnitID, x.IsDeduct }, (key, grp) =>
                                                  new ExportBillDetail
                                                  {
                                                      ProductID = key.ProductID,
                                                      ProductName = key.ProductName,
                                                      Construction = key.Construction,
                                                      ProcessTypeName = key.ProcessTypeName,
                                                      FabricWeaveName = key.FabricWeaveName,
                                                      FabricWidth = key.FabricWidth,
                                                      FinishTypeName = key.FinishTypeName,
                                                      StyleNo = key.StyleNo,
                                                      ColorInfo = key.ColorInfo,
                                                      BuyerReference = key.BuyerReference,
                                                      Qty = grp.Sum(p => p.Qty),
                                                      NoOfBag = grp.Sum(p => p.NoOfBag),
                                                      WtPerBag = grp.Sum(p => p.WtPerBag),
                                                      UnitPrice = key.UnitPrice,
                                                      MUnitID = key.MUnitID,
                                                      IsDeduct = key.IsDeduct
                                                  }).ToList();
            }
            foreach (ExportBillDetail oItem in oExportBillDetailsTemp)
            {
                if (String.IsNullOrEmpty(oItem.BuyerReference))
                {
                    oItem.StyleNo = oItem.StyleNo;
                }
                else
                {
                    oItem.StyleNo = oItem.StyleNo + " " + oItem.BuyerReference;
                }
            }
            oExportBillDetailsTemp = oExportBillDetailsTemp.OrderBy(o => o.ProductID).ThenBy(a => a.Construction).ThenBy(a => a.ProcessTypeName).ThenBy(a => a.FabricWeaveName).ThenBy(a => a.FabricWidth).ThenBy(a => a.FinishTypeName).ThenBy(b => b.StyleNo).ThenBy(c => c.ColorInfo).ToList();

            foreach (ExportBillDetail oitem in oExportBillDetailsTemp)
            {
                if (_oExportCommercialDoc.WeightPerBag <= 0)
                { _oExportCommercialDoc.WeightPerBag = 100; }
                if (!string.IsNullOrEmpty(oitem.Construction))
                {
                    _nTotalNoOfBag = _nTotalNoOfBag + oitem.NoOfBag;
                }

                if (nProductID != oitem.ProductID || sConstruction != oitem.Construction || sProcessTypeName != oitem.ProcessTypeName || sFabricWeaveName != oitem.FabricWeaveName || sFabricWidth != oitem.FabricWidth || sFinishTypeName != oitem.FinishTypeName)/// nProcessType != (int)oItem.ProcessType || nFabricWeave != (int)oItem.FabricWeave)
                {

                    nSL++;

                   
                    if (_oExportCommercialDoc.ProductPrintType == EnumExcellColumn.B)
                    {
                        sTemp = "";
                        sConst = "";
                        if (!string.IsNullOrEmpty(oitem.FabricWeaveName))
                        {
                            oitem.ProductName = "Comp:" + oitem.ProductName + ", Weave: " + oitem.FabricWeaveName;
                        }
                        if (!String.IsNullOrEmpty(oitem.FabricNo))
                        {
                            oitem.ProductName = "Article: " + oitem.FabricNo + "\n" + oitem.ProductName;
                        }
                        if (oitem.IsDeduct)
                        {
                            oitem.ProductName = oitem.ProductName + "(Deduct)";
                        }
                        if (!string.IsNullOrEmpty(oitem.Construction))
                        {
                            sConst = sConst + "Const: " + oitem.Construction;
                        }
                        if (!string.IsNullOrEmpty(oitem.FabricWidth))
                        {
                            sTemp = sTemp + "Width : " + oitem.FabricWidth;
                        }
                        if (!string.IsNullOrEmpty(oitem.Weight))
                        {
                            sTemp = sTemp + " Weight : " + oitem.Weight;
                        }
                        if (!String.IsNullOrEmpty(oitem.ProcessTypeName))
                        {
                            sTemp = sTemp + "\nProcess: " + oitem.ProcessTypeName;
                        }
                        if (!String.IsNullOrEmpty(oitem.FinishTypeName))
                        {
                            sTemp = sTemp + ", Finish: " + oitem.FinishTypeName;
                        }
                        if (!string.IsNullOrEmpty(oitem.Shrinkage))
                        {
                            sTemp = sTemp + "\nShrinkage: " + oitem.Shrinkage;
                        }
                        if (!string.IsNullOrEmpty(oitem.ProductDescription))
                        {
                            sTemp = sTemp + "\n" + oitem.ProductDescription;
                        }
                    }
                    else
                    {
                        sTemp = "";
                        sConst = "";
                        if (oitem.ProcessTypeName != "")
                        {
                            oitem.ProductName = oitem.ProductName + ", " + oitem.ProcessTypeName;
                        }
                        oitem.ProductName = oitem.ProductName + " Fabrics";
                        if (!string.IsNullOrEmpty(oitem.Construction))
                        {
                            sConst = sConst + "Const: " + oitem.Construction;
                        }

                        if (oitem.FabricWeaveName != "")
                        {
                            sTemp = sTemp + " " + oitem.FabricWeaveName;
                        }

                        if (!string.IsNullOrEmpty(oitem.FabricWidth))
                        {
                            sTemp = sTemp + ", Width : " + oitem.FabricWidth;
                        }
                        if (oitem.FinishTypeName != "")
                        {
                            sTemp = sTemp + ", Finish Type : " + oitem.FinishTypeName;
                        }
                    }
                    _nCount_Raw = oExportBillDetailsTemp.Where(x => x.ProductID == oitem.ProductID && x.ProcessTypeName == oitem.ProcessTypeName && x.Construction == oitem.Construction && x.FabricWeaveName == oitem.FabricWeaveName && x.FabricWidth == oitem.FabricWidth && x.FinishTypeName == oitem.FinishTypeName && x.SLNo >= 0).Count();

                    _oPdfPCell = new PdfPCell(new Phrase(nSL.ToString(), _oFontStyle));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f;
                    _oPdfPCell.BorderWidthTop = 0.5f;
                    _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.BorderWidthBottom = 0;
                    _oPdfPCell.Rowspan = _nCount_Raw;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable1.AddCell(_oPdfPCell);

                    _oPhrase = new Phrase();
                    _oPhrase.Add(new Chunk(oitem.ProductName + ",\n" + sConst + "\n" + sTemp, _oFontStyle));
                    _oPdfPCell = new PdfPCell(_oPhrase);
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.Rowspan = _nCount_Raw;
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.BorderWidthLeft = 0.5f;
                    _oPdfPCell.BorderWidthTop = 0.5f;
                    _oPdfPCell.BorderWidthRight = 0.5f;
                    _oPdfPCell.BorderWidthBottom = 0;
                    oPdfPTable1.AddCell(_oPdfPCell);
                }
                if (nProductID != oitem.ProductID || sConstruction != oitem.Construction || sProcessTypeName != oitem.ProcessTypeName || sFabricWeaveName != oitem.FabricWeaveName || sFabricWidth != oitem.FabricWidth || sFinishTypeName != oitem.FinishTypeName || sStyleNo != oitem.StyleNo)
                {
                    _nCount_Raw_Style = oExportBillDetailsTemp.Where(x => x.ProductID == oitem.ProductID && x.Construction == oitem.Construction && x.ProcessTypeName == oitem.ProcessTypeName && x.FabricWeaveName == oitem.FabricWeaveName && x.FabricWidth == oitem.FabricWidth && x.FinishTypeName == oitem.FinishTypeName && x.StyleNo == oitem.StyleNo && x.SLNo >= 0).Count();

                    _oPdfPCell = new PdfPCell(new Phrase(oitem.StyleNo, _oFontStyle));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f;
                    _oPdfPCell.Rowspan = _nCount_Raw_Style;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable1.AddCell(_oPdfPCell);
                }

                _oPdfPCell = new PdfPCell(new Phrase(oitem.ColorInfo, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(oitem.Qty) + " " + _oExportCommercialDoc.MUName, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

              //  _oPdfPCell = new PdfPCell(new Phrase((Math.Floor(oitem.Qty / _oExportCommercialDoc.WeightPerBag)).ToString(), _oFontStyle));
                _oPdfPCell = new PdfPCell(new Phrase(oitem.NoOfBag.ToString(), _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(Math.Round(oitem.WtPerBag,2)), _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(Math.Round((oitem.WtPerBag+(oitem.WtPerBag * 0.01)), 2)), _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);


                oPdfPTable1.CompleteRow();
                _nTotalRowCount++;
                if (oitem.IsDeduct) { _nTotalValue = _nTotalValue - (oitem.Qty * oitem.UnitPrice); }
                else { _nTotalValue = _nTotalValue + (oitem.Qty * oitem.UnitPrice); }
                if (!string.IsNullOrEmpty(oitem.Construction))
                {
                _nTotalQty = _nTotalQty + oitem.Qty;
                _nTotalQtyKg = _nTotalQtyKg + oitem.QtyTwo;
                _nTotalWtPerBag = _nTotalWtPerBag + oitem.WtPerBag;
                 _nTWeight_Net += (oitem.WtPerBag);
                 _nTWeight_Gross = _nTWeight_Gross + Math.Round(oitem.WtPerBag +(oitem.WtPerBag * 0.01), 2);
                }
                _sMUnit_Two = oitem.MUName;
                nProductID = oitem.ProductID;
                sConstruction = oitem.Construction;
                sProcessTypeName = oitem.ProcessTypeName;
                sFabricWeaveName = oitem.FabricWeaveName;
                sFabricWidth = oitem.FabricWidth;
                sFinishTypeName = oitem.FinishTypeName;
                sStyleNo = oitem.StyleNo;

                #region
                oitem.SLNo = -1;

                _nUsagesHeight = CalculatePdfPTableHeight(_oPdfPTable);
                _nUsagesHeight = _nUsagesHeight + CalculatePdfPTableHeight(oPdfPTable1);
                if (_nUsagesHeight > 700)
                {
                    nSL--;
                    _nCount_Raw = 0;
                    _nCount_Raw_Style = 0;
                    nProductID = 0;
                    _oPdfPCell = new PdfPCell(oPdfPTable1);

                    _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();

                    _nUsagesHeight = 0;
                    _oDocument.Add(_oPdfPTable);
                    _oDocument.NewPage();

                    oPdfPTable1.DeleteBodyRows();
                    _oPdfPTable.DeleteBodyRows();

                    this.HeaderWithThreeFormats(_oExportCommercialDoc.DocHeader);
                    this.ReportHeader(_oExportCommercialDoc.DocHeader);
                    oPdfPTable1 = new PdfPTable(8);
                    oPdfPTable1.SetWidths(new float[] { 30f, 180, 100f, 100f, 60f, 40f, 50f, 50f });
                    _oPdfPCell = new PdfPCell();
                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                    _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

                }
                #endregion
            }

            if (!string.IsNullOrEmpty(_oExportCommercialDoc.ASPERPI))
            {
                _oExportCommercialDoc.ASPERPI = _oExportCommercialDoc.ASPERPI.Replace("@PINO", " " + _oExportCommercialDoc.PINos);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.ASPERPI, FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL)));
                _oPdfPCell.Colspan = 5; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);
                oPdfPTable1.CompleteRow();
            }
            #region Total

            _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 4; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(_nTotalQty) + " " + _oExportCommercialDoc.MUName, _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_nTotalNoOfBag.ToString(), _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTWeight_Net), _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTWeight_Gross) + " kg", _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);


            //_oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.Currency + "" + Global.MillionFormat(_nTotalValue), _oFontStyleBold));
            //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

            oPdfPTable1.CompleteRow();
            #endregion
            
            #endregion We

            return oPdfPTable1;
        }
        private PdfPTable ProductDetails_WU_Qty_GrossNet()
        {
            #region Weaving
            PdfPTable oPdfPTable1 = new PdfPTable(7);
            oPdfPTable1.SetWidths(new float[] {180, 100f, 100f,80f,30f,70f,70f });


            List<ExportBillDetail> oExportBillDetails = new List<ExportBillDetail>();
            List<ExportBillDetail> oExportBillDetailsTemp = new List<ExportBillDetail>();
            oExportBillDetails = _oExportCommercialDoc.ExportBillDetails;


            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.DescriptionOfGoods, _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.Header_Style, _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.Header_Color, _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.QtyInKg, _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.Bag_Name, _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.NetWeight, _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.GrossWeight, _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);


            oPdfPTable1.CompleteRow();


            int nSL = 0;
            _nTotalRowCount = 0;
            var sTemp = "";
            string sConstruction = "";
            string sConst = "";
            int nProductID = 0;
            string sProcessTypeName = "";
            string sFabricWeaveName = "";
            string sFabricWidth = "";
            string sFinishTypeName = "";
            int _nCount_Raw = 0;
            int _nCount_Raw_Style = 0;
            string sStyleNo = "";
            if (_oExportCommercialDoc.ProductPrintType == EnumExcellColumn.B)
            {
                oExportBillDetailsTemp = oExportBillDetails.GroupBy(x => new { x.FabricNo, x.ProductID, x.ProductName, x.Construction, x.ProcessTypeName, x.FabricWeaveName, x.FabricWidth, x.FinishTypeName, x.StyleNo, x.BuyerReference, x.Shrinkage, x.Weight, x.ColorInfo, x.UnitPrice, x.MUnitID, x.IsDeduct }, (key, grp) =>
                                                  new ExportBillDetail
                                                  {
                                                      ProductID = key.ProductID,
                                                      ProductName = key.ProductName,
                                                      FabricNo = key.FabricNo,
                                                      Construction = key.Construction,
                                                      ProcessTypeName = key.ProcessTypeName,
                                                      FabricWeaveName = key.FabricWeaveName,
                                                      FabricWidth = key.FabricWidth,
                                                      FinishTypeName = key.FinishTypeName,
                                                      StyleNo = key.StyleNo,
                                                      BuyerReference = key.BuyerReference,
                                                      Shrinkage = key.Shrinkage,
                                                      Weight = key.Weight,
                                                      ColorInfo = key.ColorInfo,
                                                      Qty = grp.Sum(p => p.Qty),
                                                      NoOfBag = grp.Sum(p => p.NoOfBag),
                                                      WtPerBag = grp.Sum(p => p.WtPerBag),
                                                      UnitPrice = key.UnitPrice,
                                                      MUnitID = key.MUnitID,
                                                      IsDeduct = key.IsDeduct
                                                  }).ToList();
            }
            else
            {
                oExportBillDetailsTemp = oExportBillDetails.GroupBy(x => new { x.ProductID, x.ProductName, x.Construction, x.ProcessTypeName, x.FabricWeaveName, x.FabricWidth, x.FinishTypeName, x.StyleNo, x.BuyerReference, x.ColorInfo, x.UnitPrice, x.MUnitID, x.IsDeduct }, (key, grp) =>
                                                new ExportBillDetail
                                                {
                                                    ProductID = key.ProductID,
                                                    ProductName = key.ProductName,
                                                    Construction = key.Construction,
                                                    ProcessTypeName = key.ProcessTypeName,
                                                    FabricWeaveName = key.FabricWeaveName,
                                                    FabricWidth = key.FabricWidth,
                                                    FinishTypeName = key.FinishTypeName,
                                                    StyleNo = key.StyleNo,
                                                    BuyerReference = key.BuyerReference,
                                                    ColorInfo = key.ColorInfo,
                                                    Qty = grp.Sum(p => p.Qty),
                                                    NoOfBag = grp.Sum(p => p.NoOfBag),
                                                    WtPerBag = grp.Sum(p => p.WtPerBag),
                                                    UnitPrice = key.UnitPrice,
                                                    MUnitID = key.MUnitID,
                                                    IsDeduct = key.IsDeduct
                                                }).ToList();
            }
            foreach (ExportBillDetail oItem in oExportBillDetailsTemp)
            {
                if (String.IsNullOrEmpty(oItem.BuyerReference))
                {
                    oItem.StyleNo = oItem.StyleNo;
                }
                else
                {
                    oItem.StyleNo = oItem.StyleNo + " " + oItem.BuyerReference;
                }
            }
            oExportBillDetailsTemp = oExportBillDetailsTemp.OrderBy(o => o.ProductID).ThenBy(a => a.Construction).ThenBy(a => a.ProcessTypeName).ThenBy(a => a.FabricWeaveName).ThenBy(a => a.FabricWidth).ThenBy(a => a.FinishTypeName).ThenBy(b => b.StyleNo).ThenBy(c => c.ColorInfo).ToList();


            foreach (ExportBillDetail oitem in oExportBillDetailsTemp)
            {
              
                if (nProductID != oitem.ProductID || sConstruction != oitem.Construction || sProcessTypeName != oitem.ProcessTypeName || sFabricWeaveName != oitem.FabricWeaveName || sFabricWidth != oitem.FabricWidth || sFinishTypeName != oitem.FinishTypeName)/// nProcessType != (int)oItem.ProcessType || nFabricWeave != (int)oItem.FabricWeave)
                //    if (nProductID != oitem.ProductID || sConstruction != oitem.Construction)/// nProcessType != (int)oItem.ProcessType || nFabricWeave != (int)oItem.FabricWeave)
                {

                    nSL++;

                  
                    if (_oExportCommercialDoc.ProductPrintType == EnumExcellColumn.B)
                    {
                        sTemp = "";
                        sConst = "";
                        if (!string.IsNullOrEmpty(oitem.FabricWeaveName))
                        {
                            oitem.ProductName = "Comp:" + oitem.ProductName + ", Weave: " + oitem.FabricWeaveName;
                        }
                        if (!String.IsNullOrEmpty(oitem.FabricNo))
                        {
                            oitem.ProductName = "Article: " + oitem.FabricNo + "\n" + oitem.ProductName;
                        }
                        if (oitem.IsDeduct)
                        {
                            oitem.ProductName = oitem.ProductName + "(Deduct)";
                        }
                        if (!string.IsNullOrEmpty(oitem.Construction))
                        {
                            sConst = sConst + "Const: " + oitem.Construction;
                        }
                        if (!string.IsNullOrEmpty(oitem.FabricWidth))
                        {
                            sTemp = sTemp + "Width : " + oitem.FabricWidth;
                        }
                        if (!string.IsNullOrEmpty(oitem.Weight))
                        {
                            sTemp = sTemp + " Weight : " + oitem.Weight;
                        }
                        if (!String.IsNullOrEmpty(oitem.ProcessTypeName))
                        {
                            sTemp = sTemp + "\nProcess: " + oitem.ProcessTypeName;
                        }
                        if (!String.IsNullOrEmpty(oitem.FinishTypeName))
                        {
                            sTemp = sTemp + ", Finish: " + oitem.FinishTypeName;
                        }
                        if (!string.IsNullOrEmpty(oitem.Shrinkage))
                        {
                            sTemp = sTemp + "\nShrinkage: " + oitem.Shrinkage;
                        }
                        if (!string.IsNullOrEmpty(oitem.ProductDescription))
                        {
                            sTemp = sTemp + "\n" + oitem.ProductDescription;
                        }
                    }
                    else
                    {
                        sTemp = "";
                        sConst = "";
                        if (oitem.ProcessTypeName != "")
                        {
                            oitem.ProductName = oitem.ProductName + ", " + oitem.ProcessTypeName;
                        }
                        oitem.ProductName = oitem.ProductName + " Fabrics";
                        if (!string.IsNullOrEmpty(oitem.Construction))
                        {
                            sConst = sConst + "Const: " + oitem.Construction;
                        }

                        if (oitem.FabricWeaveName != "")
                        {
                            sTemp = sTemp + " " + oitem.FabricWeaveName;
                        }

                        if (!string.IsNullOrEmpty(oitem.FabricWidth))
                        {
                            sTemp = sTemp + ", Width : " + oitem.FabricWidth;
                        }
                        if (oitem.FinishTypeName != "")
                        {
                            sTemp = sTemp + ", Finish Type : " + oitem.FinishTypeName;
                        }
                    }
                    _nCount_Raw = oExportBillDetailsTemp.Where(x => x.ProductID == oitem.ProductID && x.ProcessTypeName == oitem.ProcessTypeName && x.Construction == oitem.Construction && x.FabricWeaveName == oitem.FabricWeaveName && x.FabricWidth == oitem.FabricWidth && x.FinishTypeName == oitem.FinishTypeName && x.SLNo >= 0).Count();

                    //_oPdfPCell = new PdfPCell(new Phrase(nSL.ToString(), _oFontStyle));
                    //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f;
                    //_oPdfPCell.BorderWidthTop = 0.5f;
                    //_oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.BorderWidthBottom = 0;
                    //_oPdfPCell.Rowspan = _nCount_Raw;
                    //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //oPdfPTable1.AddCell(_oPdfPCell);

                    _oPhrase = new Phrase();
                    _oPhrase.Add(new Chunk(oitem.ProductName + ",\n" + sConst + "\n" + sTemp, _oFontStyle));
                    _oPdfPCell = new PdfPCell(_oPhrase);
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.Rowspan = _nCount_Raw;
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.BorderWidthLeft = 0.5f;
                    _oPdfPCell.BorderWidthTop = 0.5f;
                    _oPdfPCell.BorderWidthRight = 0.5f;
                    _oPdfPCell.BorderWidthBottom = 0;
                    oPdfPTable1.AddCell(_oPdfPCell);
                }
                if (nProductID != oitem.ProductID || sConstruction != oitem.Construction || sProcessTypeName != oitem.ProcessTypeName || sFabricWeaveName != oitem.FabricWeaveName || sFabricWidth != oitem.FabricWidth || sFinishTypeName != oitem.FinishTypeName || sStyleNo != oitem.StyleNo)
                {
                    _nCount_Raw_Style = oExportBillDetailsTemp.Where(x => x.ProductID == oitem.ProductID && x.Construction == oitem.Construction && x.ProcessTypeName == oitem.ProcessTypeName && x.FabricWeaveName == oitem.FabricWeaveName && x.FabricWidth == oitem.FabricWidth && x.FinishTypeName == oitem.FinishTypeName && x.StyleNo == oitem.StyleNo && x.SLNo >= 0).Count();

                    _oPdfPCell = new PdfPCell(new Phrase(oitem.StyleNo, _oFontStyle));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f;
                    _oPdfPCell.Rowspan = _nCount_Raw_Style;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable1.AddCell(_oPdfPCell);
                }

                _oPdfPCell = new PdfPCell(new Phrase(oitem.ColorInfo, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(oitem.Qty) + " " + _oExportCommercialDoc.MUName, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((Math.Floor(oitem.NoOfBag)).ToString(), _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((Global.MillionFormat(oitem.WtPerBag)).ToString(), _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((Global.MillionFormat(oitem.WtPerBag+(oitem.WtPerBag * 0.01))).ToString(), _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);



                oPdfPTable1.CompleteRow();
                _nTotalRowCount++;
                _nTotalValue = _nTotalValue + (oitem.Qty * oitem.UnitPrice);
                _nTotalQty = _nTotalQty + oitem.Qty;
                _nTotalQtyKg = _nTotalQtyKg + oitem.QtyTwo;
                _nTotalNoOfBag = _nTotalNoOfBag + oitem.NoOfBag;
                _nTotalWtPerBag = _nTotalWtPerBag + oitem.WtPerBag;
                _sMUnit_Two = oitem.MUName;
                nProductID = oitem.ProductID;
                sConstruction = oitem.Construction;
                sProcessTypeName = oitem.ProcessTypeName;
                sFabricWeaveName = oitem.FabricWeaveName;
                sFabricWidth = oitem.FabricWidth;
                sFinishTypeName = oitem.FinishTypeName;
                sStyleNo = oitem.StyleNo;
                #region
                oitem.SLNo = -1;
                _nUsagesHeight = CalculatePdfPTableHeight(_oPdfPTable);
                _nUsagesHeight = _nUsagesHeight + CalculatePdfPTableHeight(oPdfPTable1);
                if (_nUsagesHeight > 700)
                {
                    nSL--;
                    _nCount_Raw = 0;
                    _nCount_Raw_Style = 0;
                    nProductID = 0;
                    _oPdfPCell = new PdfPCell(oPdfPTable1);

                    _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();

                    _nUsagesHeight = 0;
                    _oDocument.Add(_oPdfPTable);
                    _oDocument.NewPage();

                    oPdfPTable1.DeleteBodyRows();
                    _oPdfPTable.DeleteBodyRows();

                    this.HeaderWithThreeFormats(_oExportCommercialDoc.DocHeader);
                    this.ReportHeader(_oExportCommercialDoc.DocHeader);
                     oPdfPTable1 = new PdfPTable(7);
                    oPdfPTable1.SetWidths(new float[] { 180, 100f, 100f, 80f, 30f, 70f, 70f });

                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                    _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

                }
                #endregion

            }

            if (!string.IsNullOrEmpty(_oExportCommercialDoc.ASPERPI))
            {
                _oExportCommercialDoc.ASPERPI = _oExportCommercialDoc.ASPERPI.Replace("@PINO", " " + _oExportCommercialDoc.PINos);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.ASPERPI, FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL)));
                _oPdfPCell.Colspan = 4; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);
                oPdfPTable1.CompleteRow();
            }
            #region Total

            _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 3; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(_nTotalQty) + "" + _oExportCommercialDoc.MUName, _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_nTotalNoOfBag.ToString(), _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalWtPerBag) + " kgs", _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase((Global.MillionFormat(_nTotalWtPerBag+(_nTotalWtPerBag * 0.01)))+" kgs", _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);



            oPdfPTable1.CompleteRow();
            #endregion

            

            #endregion We

            return oPdfPTable1;
        }

        //private void ProductDetails_WU1(bool bShowAmount)
        //{

        //    List<ExportBillDetail> oExportBillDetailColors = new List<ExportBillDetail>();
        //    List<ExportBillDetail> oExportBillDetailStyleNo = new List<ExportBillDetail>();

        //    #region  Details
        //    #region Detail Column Header
        //    _oFontStyle = FontFactory.GetFont("Tahoma", 8.0f, 1);

        //    PdfPTable oPdfPTable1 = new PdfPTable(7);
        //    oPdfPTable1.SetWidths(new float[] { 70f, 180, 80f, 80f, 80f, 65f, 65f });


        //    List<ExportBillDetail> oExportBillDetails = new List<ExportBillDetail>();
        //    List<ExportBillDetail> oExportBillDetailsTemp = new List<ExportBillDetail>();
        //    oExportBillDetails = _oExportCommercialDoc.ExportBillDetails;




        //    _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.MarkSAndNos, _oFontStyleBold));
        //    _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

        //    _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.DescriptionOfGoods, _oFontStyleBold));
        //    _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

        //    _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.Header_Style, _oFontStyleBold));
        //    _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

        //    _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.Header_Color, _oFontStyleBold));
        //    _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

        //    _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.QtyInKg, _oFontStyleBold));
        //    _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);


        //    _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.UnitPriceDes, _oFontStyleBold));
        //    _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

        //    _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.ValueDes, _oFontStyleBold));
        //    _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);


        //    oPdfPTable1.CompleteRow();

        //    _oPdfPCell = new PdfPCell(new Phrase("ROLL No", _oFontStyleBold));
        //    _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);
        //    _oPdfPCell = new PdfPCell(new Phrase("FABRICS FOR 100% EXPORT ORIENTED READYMADE GARMENTS INDUSTRY", FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.ITALIC)));
        //    _oPdfPCell.Colspan = 3; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);
        //    _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.CTPApplicant, FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.ITALIC)));
        //    _oPdfPCell.Colspan = 3; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);

        //    oPdfPTable1.CompleteRow();

        //    int nSL = 0;
        //    _nTotalRowCount = 0;
        //    var sTemp = "";
        //    string sConstruction = "";
        //    string sConst = "";
        //    int nProductID = 0;
        //    string sProcessTypeName = "";
        //    string sFabricWeaveName = "";
        //    string sFabricWidth = "";
        //    string sFinishTypeName = "";
        //    int _nCount_Raw = 0;
        //    int _nCount_Raw_Style = 0;
        //    string sStyleNo = "";
        //    if (_oExportCommercialDoc.ProductPrintType == EnumExcellColumn.B)
        //    {
        //        oExportBillDetailsTemp = oExportBillDetails.GroupBy(x => new { x.ProductID, x.ProductName, x.FabricNo, x.Construction, x.ProcessTypeName, x.FabricWeaveName, x.FabricWidth, x.FinishTypeName, x.StyleNo, x.BuyerReference, x.Shrinkage, x.Weight, x.ColorInfo, x.UnitPrice, x.MUnitID, x.IsDeduct }, (key, grp) =>
        //                                          new ExportBillDetail
        //                                          {
        //                                              ProductID = key.ProductID,
        //                                              ProductName = key.ProductName,
        //                                              FabricNo = key.FabricNo,
        //                                              Construction = key.Construction,
        //                                              ProcessTypeName = key.ProcessTypeName,
        //                                              FabricWeaveName = key.FabricWeaveName,
        //                                              FabricWidth = key.FabricWidth,
        //                                              FinishTypeName = key.FinishTypeName,
        //                                              StyleNo = key.StyleNo,
        //                                              BuyerReference = key.BuyerReference,
        //                                              Shrinkage = key.Shrinkage,
        //                                              Weight = key.Weight,
        //                                              ColorInfo = key.ColorInfo,
        //                                              Qty = grp.Sum(p => p.Qty),
        //                                              NoOfBag = grp.Sum(p => p.NoOfBag),
        //                                              WtPerBag = grp.Sum(p => p.WtPerBag),
        //                                              UnitPrice = key.UnitPrice,
        //                                              MUnitID = key.MUnitID,
        //                                              IsDeduct = key.IsDeduct
        //                                          }).ToList();
        //    }
        //    else
        //    {
        //        oExportBillDetailsTemp = oExportBillDetails.GroupBy(x => new { x.ProductID, x.ProductName, x.Construction, x.ProcessTypeName, x.FabricWeaveName, x.FabricWidth, x.FinishTypeName, x.StyleNo, x.ColorInfo, x.BuyerReference, x.UnitPrice, x.MUnitID, x.IsDeduct }, (key, grp) =>
        //                                         new ExportBillDetail
        //                                         {
        //                                             ProductID = key.ProductID,
        //                                             ProductName = key.ProductName,
        //                                             Construction = key.Construction,
        //                                             ProcessTypeName = key.ProcessTypeName,
        //                                             FabricWeaveName = key.FabricWeaveName,
        //                                             FabricWidth = key.FabricWidth,
        //                                             FinishTypeName = key.FinishTypeName,
        //                                             StyleNo = key.StyleNo,
        //                                             ColorInfo = key.ColorInfo,
        //                                             BuyerReference = key.BuyerReference,
        //                                             Qty = grp.Sum(p => p.Qty),
        //                                             NoOfBag = grp.Sum(p => p.NoOfBag),
        //                                             WtPerBag = grp.Sum(p => p.WtPerBag),
        //                                             UnitPrice = key.UnitPrice,
        //                                             MUnitID = key.MUnitID,
        //                                             IsDeduct = key.IsDeduct
        //                                         }).ToList();
        //    }
        //    foreach (ExportBillDetail oItem in oExportBillDetailsTemp)
        //    {
        //        if (String.IsNullOrEmpty(oItem.BuyerReference))
        //        {
        //            oItem.StyleNo = oItem.StyleNo;
        //        }
        //        else
        //        {
        //            oItem.StyleNo = oItem.StyleNo + " " + oItem.BuyerReference;
        //        }
        //    }
        //    oExportBillDetailsTemp = oExportBillDetailsTemp.OrderBy(o => o.ProductID).ThenBy(a => a.Construction).ThenBy(a => a.ProcessTypeName).ThenBy(a => a.FabricWeaveName).ThenBy(a => a.FabricWidth).ThenBy(a => a.FinishTypeName).ThenBy(b => b.StyleNo).ThenBy(c => c.ColorInfo).ToList();
        //       #endregion

        //    bool bFlag = false;
        //    bool bFlagTwo = false;
        //    bool bIsNewPage = false;
        //    foreach (ExportBillDetail oitem in oExportBillDetailsTemp)
        //    {
        //        oExportBillDetailStyleNo = new List<ExportBillDetail>();
        //        //oExportPIDetailColor = _oExportPIDetails.Where(x => x.ProductID == oItem.ProductID && x.ProductName == oItem.ProductName && x.Construction == oItem.Construction && x.ProcessTypeName == oItem.ProcessTypeName && x.FabricWeaveName == oItem.FabricWeaveName && x.FabricWidth == oItem.FabricWidth && x.FinishTypeName == oItem.FinishTypeName && x.FinishType == oItem.FinishType && x.MUnitID == oItem.MUnitID && x.IsDeduct == oItem.IsDeduct).ToList();
        //        oExportBillDetailStyleNo = oExportBillDetails.Where(x => x.ProductID == oitem.ProductID && x.ProductName == oitem.ProductName && x.Construction == oitem.Construction && x.ProcessTypeName == oitem.ProcessTypeName && x.ProcessTypeName == oitem.ProcessTypeName && x.FabricWeaveName == oitem.FabricWeaveName && x.FabricWeaveName == oitem.FabricWeaveName && x.FabricWidth == oitem.FabricWidth && x.FinishTypeName == oitem.FinishTypeName && x.Weight == oitem.Weight && x.Shrinkage == oitem.Shrinkage && x.IsDeduct == oitem.IsDeduct).ToList();
        //        bFlag = true;

        //        if (_oExportCommercialDoc.ProductPrintType == EnumExcellColumn.B)
        //        {
        //            sTemp = "";
        //            sConst = "";
        //            if (!string.IsNullOrEmpty(oitem.FabricWeaveName))
        //            {
        //                oitem.ProductName = "Comp:" + oitem.ProductName + ", Weave: " + oitem.FabricWeaveName;
        //            }
        //            if (!String.IsNullOrEmpty(oitem.FabricNo))
        //            {
        //                oitem.ProductName = "Article: " + oitem.FabricNo + "\n" + oitem.ProductName;
        //            }
        //            if (oitem.IsDeduct)
        //            {
        //                oitem.ProductName = oitem.ProductName + "(Deduct)";
        //            }
        //            if (!string.IsNullOrEmpty(oitem.Construction))
        //            {
        //                sConst = sConst + "Const: " + oitem.Construction;
        //            }
        //            if (!string.IsNullOrEmpty(oitem.FabricWidth))
        //            {
        //                sTemp = sTemp + "Width : " + oitem.FabricWidth;
        //            }
        //            if (!string.IsNullOrEmpty(oitem.Weight))
        //            {
        //                sTemp = sTemp + " Weight : " + oitem.Weight;
        //            }
        //            if (!String.IsNullOrEmpty(oitem.ProcessTypeName))
        //            {
        //                sTemp = sTemp + "\nProcess: " + oitem.ProcessTypeName;
        //            }
        //            if (!String.IsNullOrEmpty(oitem.FinishTypeName))
        //            {
        //                sTemp = sTemp + ", Finish: " + oitem.FinishTypeName;
        //            }
        //            if (!string.IsNullOrEmpty(oitem.Shrinkage))
        //            {
        //                sTemp = sTemp + "\nShrinkage: " + oitem.Shrinkage;
        //            }
        //            if (!string.IsNullOrEmpty(oitem.ProductDescription))
        //            {
        //                sTemp = sTemp + "\n" + oitem.ProductDescription;
        //            }
        //        }
        //        else
        //        {
        //            sTemp = "";
        //            sConst = "";
        //            if (oitem.ProcessTypeName != "")
        //            {
        //                oitem.ProductName = oitem.ProductName + ", " + oitem.ProcessTypeName;
        //            }
        //            oitem.ProductName = oitem.ProductName + " Fabrics";
        //            if (!string.IsNullOrEmpty(oitem.Construction))
        //            {
        //                sConst = sConst + "Const: " + oitem.Construction;
        //            }

        //            if (oitem.FabricWeaveName != "")
        //            {
        //                sTemp = sTemp + " " + oitem.FabricWeaveName;
        //            }

        //            if (!string.IsNullOrEmpty(oitem.FabricWidth))
        //            {
        //                sTemp = sTemp + ", Width : " + oitem.FabricWidth;
        //            }
        //            if (oitem.FinishTypeName != "")
        //            {
        //                sTemp = sTemp + ", Finish Type : " + oitem.FinishTypeName;
        //            }
        //        }


        //        _oPhrase = new Phrase();
        //        _oPhrase.Add(new Chunk(oitem.ProductName + ",\n" + sConst + "\n" + sTemp, FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL)));
        //        //_oPhrase.Add(new Chunk(" ["+oItem.ExportQuality+"]", FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL)));
        //        _oPdfPCell = new PdfPCell(_oPhrase);

        //        //_oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName + ",\n" + sConst + "\n" + sTemp, FontFactory.GetFont("Tahoma", 7f, 0)));
        //        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
        //        _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
        //        //_oPdfPCell.Rowspan = _nCount_Raw;
        //        _oPdfPCell.Border = 0;
        //        _oPdfPCell.BorderWidthLeft = 0.5f;
        //        _oPdfPCell.BorderWidthTop = 0.5f;
        //        _oPdfPCell.BorderWidthBottom = 0;
        //        _oPdfPTable.AddCell(_oPdfPCell);

        //        //_oPdfPCell = new PdfPCell(new Phrase(oitem.ExportQuality, _oFontStyle));
        //        //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //        //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //        //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //        ////_oPdfPCell.Rowspan = _nCount_Raw;
        //        //_oPdfPCell.Border = 0;
        //        //_oPdfPCell.BorderWidthLeft = 0.5f;
        //        //_oPdfPCell.BorderWidthTop = 0.5f;
        //        //_oPdfPCell.BorderWidthBottom = 0;
        //        //_oPdfPTable.AddCell(_oPdfPCell);


        //        oExportBillDetailStyleNo = oExportBillDetailStyleNo.GroupBy(x => new { x.StyleNo }, (key, grp) =>
        //                                         new ExportBillDetail
        //                                         {
        //                                             StyleNo = key.StyleNo,
        //                                         }).ToList();

        //        oExportBillDetailStyleNo = oExportBillDetailStyleNo.OrderBy(o => o.ProductID).ThenBy(b => b.StyleNo).ThenBy(c => c.ColorInfo).ToList();
        //        foreach (ExportBillDetail oItem2 in oExportBillDetailStyleNo)
        //        {
        //            //oExportPIDetailColors = _oExportPIDetails.Where(x => x.StyleNo == oItem2.StyleNo).ToList();
        //            oExportBillDetailColors = oExportBillDetails.Where(x => x.ProductID == oitem.ProductID && x.StyleNo == oItem2.StyleNo && x.Construction == oitem.Construction && x.ProcessTypeName == oitem.ProcessTypeName && x.ProcessTypeName == oitem.ProcessTypeName && x.FabricWeaveName == oitem.FabricWeaveName && x.FabricWeaveName == oitem.FabricWeaveName && x.FabricWidth == oitem.FabricWidth && x.FinishTypeName == oitem.FinishTypeName && x.FinishTypeName == oitem.FinishTypeName && x.IsDeduct == oitem.IsDeduct).ToList();
        //            if (!bFlag)
        //            {
        //                //if (bIsNewPage)
        //                //{
        //                //    _oPhrase = new Phrase();
        //                //    _oPhrase.Add(new Chunk(oItem.ProductName + ",\n" + sConst + "\n" + sTemp, FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL)));
        //                //    //_oPhrase.Add(new Chunk(" ["+oItem.ExportQuality+"]", FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL)));
        //                //    _oPdfPCell = new PdfPCell(_oPhrase);
        //                //}
        //                //else
        //                //{
        //                //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
        //                //}

        //                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
        //                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //                _oPdfPCell.Border = 0;
        //                _oPdfPCell.BorderWidthLeft = 0.5f;
        //                if (bIsNewPage)
        //                {
        //                    _oPdfPCell.BorderWidthTop = 0.5f;
        //                }
        //                else
        //                {
        //                    _oPdfPCell.BorderWidthTop = 0;
        //                }
        //                _oPdfPCell.BorderWidthBottom = 0;
        //                _oPdfPTable.AddCell(_oPdfPCell);

        //                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
        //                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //                _oPdfPCell.Border = 0;
        //                _oPdfPCell.BorderWidthLeft = 0.5f;
        //                if (bIsNewPage)
        //                {
        //                    _oPdfPCell.BorderWidthTop = 0.5f;
        //                }
        //                else
        //                {
        //                    _oPdfPCell.BorderWidthTop = 0;
        //                }
        //                _oPdfPCell.BorderWidthBottom = 0;
        //                _oPdfPTable.AddCell(_oPdfPCell);
        //            }

        //            bFlag = false;

        //            //_nCount_Raw = oExportPIDetailColors.Where(x => x.StyleNo == oItem2.StyleNo).Count();
        //            _oPdfPCell = new PdfPCell(new Phrase(oItem2.StyleNo, _oFontStyle));
        //            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //            _oPdfPCell.Border = 0;
        //            _oPdfPCell.BorderWidthLeft = 0.5f;
        //            _oPdfPCell.BorderWidthTop = 0.5f;
        //            _oPdfPCell.BorderWidthBottom = 0;
        //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //            _oPdfPTable.AddCell(_oPdfPCell);

        //            oExportBillDetailColors = oExportBillDetailColors.OrderBy(o => o.ProductID).ThenBy(b => b.StyleNo).ThenBy(c => c.ColorInfo).ToList();
        //            bFlagTwo = true;
        //            foreach (ExportBillDetail oItem3 in oExportBillDetailColors)
        //            {
        //                if (!bFlagTwo)
        //                {
        //                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
        //                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //                    _oPdfPCell.Border = 0;
        //                    _oPdfPCell.BorderWidthLeft = 0.5f;
        //                    _oPdfPCell.BorderWidthTop = 0;
        //                    _oPdfPCell.BorderWidthBottom = 0;
        //                    _oPdfPTable.AddCell(_oPdfPCell);

        //                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
        //                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //                    _oPdfPCell.Border = 0;
        //                    _oPdfPCell.BorderWidthLeft = 0.5f;
        //                    _oPdfPCell.BorderWidthTop = 0;
        //                    _oPdfPCell.BorderWidthBottom = 0;
        //                    _oPdfPTable.AddCell(_oPdfPCell);

        //                    //_nCount_Raw = oExportPIDetailColors.Where(x => x.StyleNo == oItem2.StyleNo).Count();
        //                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
        //                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //                    _oPdfPCell.Border = 0;
        //                    _oPdfPCell.BorderWidthLeft = 0.5f;
        //                    _oPdfPCell.BorderWidthTop = 0;
        //                    _oPdfPCell.BorderWidthBottom = 0;
        //                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //                    _oPdfPTable.AddCell(_oPdfPCell);

        //                }
        //                bFlagTwo = false;
        //                _oPdfPCell = new PdfPCell(new Phrase(oItem3.ColorInfo, _oFontStyle));
        //                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //                //_oPdfPCell.MinimumHeight = 40f;
        //                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //                _oPdfPTable.AddCell(_oPdfPCell);

        //                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem3.Qty), _oFontStyle));
        //                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //                //_oPdfPCell.MinimumHeight = 40f;
        //                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //                _oPdfPTable.AddCell(_oPdfPCell);

        //                _oPdfPCell = new PdfPCell(new Phrase(System.Math.Round(oItem3.UnitPrice, 4).ToString("#,##0.00##;(#,##0.00##)"), _oFontStyle));
        //                //_oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(oItem.UnitPrice), _oFontStyle));
        //                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //                //_oPdfPCell.MinimumHeight = 40f;
        //                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //                _oPdfPTable.AddCell(_oPdfPCell);
        //                if (oItem.IsDeduct)
        //                {
        //                    //_oPdfPCell = new PdfPCell(new Phrase(oItem.AmountSt, _oFontStyle));
        //                    _oPdfPCell = new PdfPCell(new Phrase("(" + _oExportPI.Currency + "" + Global.MillionFormat(oItem3.Qty * oItem3.UnitPrice) + ")", _oFontStyle));
        //                }
        //                else
        //                {
        //                    _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.Currency + "" + Global.MillionFormat(oItem3.Qty * oItem3.UnitPrice), _oFontStyle));
        //                }

        //                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //                //_oPdfPCell.MinimumHeight = 40f;
        //                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;

        //                _oPdfPTable.AddCell(_oPdfPCell);
        //                _oPdfPTable.CompleteRow();

        //                bIsNewPage = false;
        //                _nUsagesHeight = CalculatePdfPTableHeight(_oPdfPTable);
        //                if (_nUsagesHeight > 775)
        //                {
        //                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
        //                    _oFontStyle_Bold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);


        //                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
        //                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //                    _oPdfPCell.Colspan = _nTotalColumn;
        //                    _oPdfPCell.Border = 0;
        //                    _oPdfPCell.BorderWidthLeft = 0;
        //                    _oPdfPCell.BorderWidthRight = 0;
        //                    _oPdfPCell.BorderWidthTop = 0.5f;
        //                    _oPdfPCell.BorderWidthBottom = 0;
        //                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //                    _oPdfPTable.AddCell(_oPdfPCell);
        //                    _oPdfPTable.CompleteRow();

        //                    _nUsagesHeight = 0;
        //                    _oDocument.Add(_oPdfPTable);
        //                    _oDocument.NewPage();
        //                    //_oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
        //                    //_oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
        //                    //_oDocument.SetMargins(35f, 15f, 5f, 30f);
        //                    //oPdfPTable.DeleteBodyRows();
        //                    _oPdfPTable.DeleteBodyRows();
        //                    _nTotalColumn = 7;
        //                    _oPdfPTable = new PdfPTable(_nTotalColumn);
        //                    _oPdfPTable.SetWidths(new float[] { 200f, 55f, 125f, 125f, 70f, 70f, 75f });
        //                    _oPdfPTable.WidthPercentage = 100;
        //                    _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

        //                    this.PrintHeader();
        //                    this.ReporttHeader();
        //                    //isNewPage = true;
        //                    //this.WaterMark(35f, 15f, 5f, 30f);
        //                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
        //                    _oFontStyle_Bold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
        //                    bIsNewPage = true;
        //                }
        //            }

        //        }

        //        //oPdfPTable = new PdfPTable(7);
        //        //oPdfPTable.SetWidths(new float[] { 200f, 55f, 125f, 125f, 70f, 70f, 75f });



        //        //if (oItem.IsDeduct) { nTotalValue = nTotalValue - oItem2.Qty * oItem2.UnitPrice; }
        //        //else { nTotalValue = nTotalValue + oItem.Qty * oItem.UnitPrice; nTotalQty += oItem.Qty; }

        //        //oItem.OrderSheetDetailID = -1;

        //        //sTotalWidth = oItem.FabricWidth;
        //        //sMUName = oItem.MUName;
        //        //nProductID = oItem.ProductID;
        //        //nProcessType = (int)oItem.ProcessType;

        //        //sConstruction = oItem.Construction;
        //        //sStyleNo = oItem.StyleNo;
        //        //sBuyerReference = oItem.BuyerReference;

        //        //sFabricWeaveName = oItem.FabricWeaveName;
        //        //sFabricWidth = oItem.FabricWidth;
        //        //sFinishTypeName = oItem.FinishTypeName;

        //        //_nUsagesHeight = CalculatePdfPTableHeight(_oPdfPTable);


        //        //_oPdfPCell = new PdfPCell(_oPdfPTable);
        //        //_oPdfPCell.Colspan = _nTotalColumn; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
        //        //_oPdfPTable.CompleteRow();

        //    }
        //    nTotalQty = _oExportPIDetails.Where(c => c.MUnitID > 1).Sum(x => x.Qty);
        //    //nTotalQty = _oExportPIDetails.Sum(x => x.Qty);
        //    nTotalValue = _oExportPIDetails.Where(c => c.IsDeduct == false).Sum(x => x.Qty * x.UnitPrice);
        //    nTotalValue = nTotalValue - _oExportPIDetails.Where(c => c.IsDeduct == true).Sum(x => x.Qty * x.UnitPrice);
        //    //oPdfPTable = new PdfPTable(7);
        //    //oPdfPTable.SetWidths(new float[] { 200f, 55f, 125f, 125f, 70f, 70f, 75f });

        //    _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyle_Bold));
        //    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //    //_oPdfPCell.MinimumHeight = 40f;
        //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //    _oPdfPTable.AddCell(_oPdfPCell);

        //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
        //    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //    //_oPdfPCell.MinimumHeight = 40f;
        //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //    _oPdfPTable.AddCell(_oPdfPCell);

        //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
        //    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //    //_oPdfPCell.MinimumHeight = 40f;
        //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //    _oPdfPTable.AddCell(_oPdfPCell);

        //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
        //    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //    //_oPdfPCell.MinimumHeight = 40f;
        //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //    _oPdfPTable.AddCell(_oPdfPCell);

        //    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalQty) + (_bIsInYard ? " Y" : "M"), _oFontStyle_Bold));
        //    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //    //_oPdfPCell.MinimumHeight = 40f;
        //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //    _oPdfPTable.AddCell(_oPdfPCell);

        //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
        //    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //    //_oPdfPCell.MinimumHeight = 40f;
        //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //    _oPdfPTable.AddCell(_oPdfPCell);

        //    _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.Currency + "" + Global.MillionFormat(nTotalValue), _oFontStyle_Bold));
        //    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //    //_oPdfPCell.MinimumHeight = 40f;
        //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //    _oPdfPTable.AddCell(_oPdfPCell);


        //    if (_oExportPI.CurrencyID == _oCompany.BaseCurrencyID)/// Hard code-Please Change it Mamun
        //    {
        //        _oPdfPCell = new PdfPCell(new Phrase("In Word :  " + Global.TakaWords(nTotalValue), _oFontStyle));
        //    }
        //    else
        //    {
        //        _oPdfPCell = new PdfPCell(new Phrase("In Word :  " + Global.DollarWords(nTotalValue), _oFontStyle));
        //    }
        //    _oPdfPCell.Colspan = 7; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
        //    _oPdfPTable.CompleteRow();

        //    //_oPdfPCell = new PdfPCell(oPdfPTable);
        //    //_oPdfPCell.Colspan = _nTotalColumn; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
        //    //_oPdfPTable.CompleteRow();


        //    #endregion

        //    #endregion


        //}
      
        #endregion

        #endregion

        #region CommercialInvoice
        public byte[] PrepareReport_CommercialInvoice(ExportCommercialDoc oExportCommercialDoc, Company oCompany, int nPrintType, string sMUName, BusinessUnit oBusinessUnit, int nPageSize)
        {

            _oExportCommercialDoc = oExportCommercialDoc;
            _oCompany = oCompany;
            _oBusinessUnit = oBusinessUnit;
            _nPrintType = nPrintType;
            _nMaxHeight = 500;
            _sMUName = sMUName;
            #region Page Setup
            _oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
            if (nPageSize == 1)
            {
                //nPageSize means "A4"
                _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595                
            }
            else
            {
                //nPageSize 2 means "Legal"
                _oDocument.SetPageSize(iTextSharp.text.PageSize.LEGAL);
            }
            _oDocument.SetMargins(30f, 30f, 30f, 30f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            //_oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PageEventHandler.PrintDocumentGenerator = false;
            PageEventHandler.PrintPrintingDateTime = false;
            _oWriter.PageEvent = PageEventHandler; //Footer print wiht page event handeler            
            _oDocument.Open();

            _oPdfPTable.SetWidths(new float[] { 595f });
            #endregion

            this.HeaderWithThreeFormats(_oExportCommercialDoc.DocHeader);
            this.ReportHeader(_oExportCommercialDoc.DocHeader);
            this.PrintBody_CommercialInvoice();
            _oPdfPTable.HeaderRows = ((_oExportCommercialDoc.IsPrintHeader) ? 2 : 1);

            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Body
        private void PrintBody_CommercialInvoice()
        {
            #region 1st and 2nd Row ,4th, 5th, 6th And 7th Row
            PdfPTable oPdfPTable = new PdfPTable(2);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 280f, 280f });
            if (_oExportCommercialDoc.ExportBillDetails.Count < 20)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.UNDERLINE);
                _oFontStyle_Bold_UnLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
            }
            else {
                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
                _oFontStyleBold = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
                _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.UNDERLINE);
                _oFontStyle_Bold_UnLine = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
            }
            #region ShipmentTerm and Master LC

            _oPdfPCell = new PdfPCell(this.SetCommercialInvoice_Left());
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            if (!String.IsNullOrEmpty(_oExportCommercialDoc.DocumentaryCreditNoDate))
            {
                _oPdfPCell = new PdfPCell(this.SetCommercialInvoice_Right());
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            }
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #endregion

            #region Black Space
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 5; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region 8th Row (Product Detail Table)
            if ((_oExportCommercialDoc.BusinessUnitType == (int)EnumBusinessUnitType.Integrated || _oExportCommercialDoc.BusinessUnitType == (int)EnumBusinessUnitType.Plastic) && _oExportCommercialDoc.ProductNature == EnumProductNature.Hanger)
            {
                #region Hanger
                if (_oExportCommercialDoc.IsPrintUnitPrice)
                {
                    this.HangerProductWithPrice();
                }
                else
                {
                    _oPdfPCell = new PdfPCell(this.HangerProductWithOutPrice());
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                }
                #endregion
            }
            else if ((_oExportCommercialDoc.BusinessUnitType == (int)EnumBusinessUnitType.Integrated || _oExportCommercialDoc.BusinessUnitType == (int)EnumBusinessUnitType.Plastic) && _oExportCommercialDoc.ProductNature == EnumProductNature.Poly)
            {
                #region Poly
                if (_oExportCommercialDoc.IsPrintUnitPrice)
                {
                    _oPdfPCell = new PdfPCell(this.PloyProductWithPrice());
                }
                else
                {
                    _oPdfPCell = new PdfPCell(this.PloyProductWithPrice());
                }
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion
            }
            else if (_oExportCommercialDoc.BusinessUnitType == (int)EnumBusinessUnitType.Dyeing )
            {
                #region Dyeing
                //if (_oExportCommercialDoc.IsPrintUnitPrice)
                //{
                //    _oPdfPCell = new PdfPCell(this.ProductDetails_Dyeing_UnitePrice());
                //}
                //else
                //{
                //    _oPdfPCell = new PdfPCell(this.ProductDetails_Dyeing_Qty());
                //}
                if (_oExportCommercialDoc.GoodsDesViewType == EnumExportGoodsDesViewType.Qty_Gross_NetWeight)
                {
                    _oPdfPCell = new PdfPCell(this.ProductDetails_Dyeing_NET_GROSSWEIGHT());
                }
                else if (_oExportCommercialDoc.GoodsDesViewType == EnumExportGoodsDesViewType.Qty_UP_Ampunt)
                {
                    _oPdfPCell = new PdfPCell(this.ProductDetails_Dyeing_UnitePrice());
                }
                else if (_oExportCommercialDoc.GoodsDesViewType == EnumExportGoodsDesViewType.Qty_UP_Ampunt_Bag)
                {
                    _oPdfPCell = new PdfPCell(this.ProductDetails_Dyeing_UnitePrice());
                }
                else if (_oExportCommercialDoc.GoodsDesViewType == EnumExportGoodsDesViewType.Qty_Amount)
                {
                    _oPdfPCell = new PdfPCell(this.ProductDetails_Dyeing_Qty());
                }
                else if (_oExportCommercialDoc.GoodsDesViewType == EnumExportGoodsDesViewType.Qty)
                {
                    _oPdfPCell = new PdfPCell(this.ProductDetails_Dyeing_Qty());
                }
                else
                {
                    _oPdfPCell = new PdfPCell(this.ProductDetails_Dyeing_UnitePrice());
                }
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion
            }
            else if (_oExportCommercialDoc.BusinessUnitType == (int)EnumBusinessUnitType.Weaving || _oExportCommercialDoc.BusinessUnitType == (int)EnumBusinessUnitType.Finishing)
            {
                #region Weaving
                if (_oExportCommercialDoc.GoodsDesViewType == EnumExportGoodsDesViewType.Qty_Gross_NetWeight)
                {
                    _oPdfPCell = new PdfPCell(this.ProductDetails_WU_Qty_PackingListDetail());
                }
                else if (_oExportCommercialDoc.GoodsDesViewType == EnumExportGoodsDesViewType.Qty_UP_Ampunt_Bag)
                {
                    _oPdfPCell = new PdfPCell(this.ProductDetails_WU(true));
                }
                else if (_oExportCommercialDoc.GoodsDesViewType == EnumExportGoodsDesViewType.Qty_UP_Ampunt)
                {
                    _oPdfPCell = new PdfPCell(this.ProductDetails_WU_UP_Amount());
                }
                else if (_oExportCommercialDoc.GoodsDesViewType == EnumExportGoodsDesViewType.Qty_Amount)
                {
                    _oPdfPCell = new PdfPCell(this.ProductDetails_WU(false));
                }
                else if (_oExportCommercialDoc.GoodsDesViewType == EnumExportGoodsDesViewType.Qty)
                {
                    _oPdfPCell = new PdfPCell(this.ProductDetails_WU_Qty());
                }
                else
                {
                    _oPdfPCell = new PdfPCell(this.ProductDetails_WU(true));
                }

                //if (_oExportCommercialDoc.IsPrintUnitPrice)
                //{
                //    _oPdfPCell = new PdfPCell(this.ProductDetails_WU(true));
                //}
                //else
                //{
                //    if (_oExportCommercialDoc.DocumentType == EnumDocumentType.Packing_List_Detail)
                //    {
                //        _oPdfPCell = new PdfPCell(this.ProductDetails_WU_Qty_PackingListDetail());
                //    }
                //    else
                //    {
                //        _oPdfPCell = new PdfPCell(this.ProductDetails_WU_Qty());
                //    }
                //}
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion
            }      
            #endregion

            #region 10th Row (FREIGHT PREPAID/ORGINAM//DELIVERY)

            oPdfPTable = new PdfPTable(1);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 560f });

         

            if (_oExportCommercialDoc.FrightPrepaid != "" && _oExportCommercialDoc.FrightPrepaid != "N/A")
            {
                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.FrightPrepaid, FontFactory.GetFont("Tahoma", 12f, 1, BaseColor.GRAY)));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }

            if (_oExportCommercialDoc.Orginal != "" && _oExportCommercialDoc.Orginal != "N/A")
            {
                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.Orginal, FontFactory.GetFont("Tahoma", 12f, 1, BaseColor.GRAY)));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 1; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            if (_oExportCommercialDoc.DeliveryBy != "" && _oExportCommercialDoc.DeliveryBy != "N/A")
            {
                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.DeliveryBy, FontFactory.GetFont("Tahoma", 12f, 1, BaseColor.GRAY)));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

            #region Blank space
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.FixedHeight = 5f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Gross Net Weight
            if (_oExportCommercialDoc.IsPrintGrossNetWeight)
            {
                if (_oExportCommercialDoc.BusinessUnitType == (int)EnumBusinessUnitType.Weaving || _oExportCommercialDoc.BusinessUnitType == (int)EnumBusinessUnitType.Finishing)
                {
                    _oPdfPCell = new PdfPCell(this.ShowNetWeight_WU());
                }
                else
                {
                    _oPdfPCell = new PdfPCell(this.ShowNetWeight_DU());
                }
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion

            #region 9th Row

            oPdfPTable = new PdfPTable(2);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 10f, 550f });

      

            #region Blank Space
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.FixedHeight = 2f; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.FixedHeight = 2f; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Clauses
         
            //if (_oExportCommercialDoc.Wecertifythat != "" && _oExportCommercialDoc.Wecertifythat != "N/A")
            //{
            if (_oExportCommercialDoc.Certification != "" && _oExportCommercialDoc.Certification != "N/A")
            {
                _oExportCommercialDoc.Certification = _oExportCommercialDoc.Certification.Replace("@PINO", " " + _oExportCommercialDoc.PINos);
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase( _oExportCommercialDoc.Certification, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            if (_oExportCommercialDoc.ClauseOne != "" && _oExportCommercialDoc.ClauseOne != "N/A")
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.ClauseOne, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            if (_oExportCommercialDoc.ClauseTwo != "" && _oExportCommercialDoc.ClauseTwo != "N/A")
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.ClauseTwo, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            if (_oExportCommercialDoc.ClauseThree != "" && _oExportCommercialDoc.ClauseThree != "N/A")
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.ClauseThree, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

          


            //#region Certification
            //if (_oExportCommercialDoc.Certification != "" && _oExportCommercialDoc.Certification != "N/A")
            //{
            //    //if (!string.IsNullOrEmpty(_oExportCommercialDoc.Certification_Entry))
            //    //{
            //    //    _oExportCommercialDoc.Certification = _oExportCommercialDoc.Certification_Entry;
            //    //    _oPdfPCell = new PdfPCell(new Phrase("Certification:", _oFontStyleBold));
            //    //    _oExportCommercialDoc.Certification = _oExportCommercialDoc.Certification.Replace("@PINO", " " + _oExportCommercialDoc.PINos);
            //    //    _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            //    //}
            //    //else
            //    //{
            //    //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            //    //    _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //    //}
            //    _oExportCommercialDoc.Certification = _oExportCommercialDoc.Certification.Replace("@PINO", " " + _oExportCommercialDoc.PINos);
            //    _oPdfParagraph = new Paragraph(new Phrase(_oExportCommercialDoc.Certification, _oFontStyle));
            //    _oPdfParagraph.SetLeading(0, 1.20f);
            //    _oPdfParagraph.Alignment = Element.ALIGN_JUSTIFIED;
            //    _oPdfPCell = new PdfPCell();
            //    _oPdfPCell.AddElement(_oPdfParagraph);
            //    _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            //    oPdfPTable.CompleteRow();

            //}
            //#endregion

            //_oPdfPCell = new PdfPCell(oPdfPTable);
            //_oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();

            #region Clause Four
            if (_oExportCommercialDoc.ClauseFour != "" && _oExportCommercialDoc.ClauseFour != "N/A")
            {
                #region Blank space
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.FixedHeight = 5f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                oPdfPTable = new PdfPTable(1);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.SetWidths(new float[] { 560f });

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.ClauseFour, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion

            #region Blank space
            if (_oExportCommercialDoc.ExportBillDetails.Count < 20)
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 5f; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion
            #region ReceiverSignature
            oPdfPTable = new PdfPTable(2);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 280f, 280f });

            #region Blank space
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0;
            if (_oExportCommercialDoc.ExportBillDetails.Count > 20)
            {
                _oPdfPCell.FixedHeight = 15f;
            }
            else
            {
                _oPdfPCell.FixedHeight = 20f;
            }
                _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            if (!string.IsNullOrEmpty(_oExportCommercialDoc.To) || !string.IsNullOrEmpty(_oExportCommercialDoc.ReceiverSignature))
            {
                _oPdfPCell = new PdfPCell(this.To());
                //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            }

            if (_oExportCommercialDoc.For != "")
            {
                _oPdfPCell = new PdfPCell(this.For());
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            }
            oPdfPTable.CompleteRow();
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 3f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.FixedHeight = 3f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion
            //#region ReceiverSignature
            //oPdfPTable = new PdfPTable(2);
            //oPdfPTable.WidthPercentage = 100;
            //oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            //oPdfPTable.SetWidths(new float[] { 280f, 280f });
                        
            //_oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        
            //if (_oExportCommercialDoc.For != "")
            //{
            //    _oPdfPCell = new PdfPCell(this.For());
            //    _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            //}
            //else
            //{
            //    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            //    _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //}
            //oPdfPTable.CompleteRow();
            //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.FixedHeight = 3f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);




            //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.FixedHeight = 3f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            //oPdfPTable.CompleteRow();

            //_oPdfPCell = new PdfPCell(oPdfPTable);
            //_oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();

            //#endregion

            #endregion

        }
        #endregion
        #endregion

        #region Report Submition
        public byte[] PrepareReport_Submition(ExportCommercialDoc oExportCommercialDoc, Company oCompany, int nPrintType, ExportBill oExportBill, BusinessUnit oBusinessUnit, int nPageSize)
        {

            _oExportCommercialDoc = oExportCommercialDoc;
            _oCompany = oCompany;
            _nPrintType = nPrintType;
            _oExportBill = oExportBill;
            _oBusinessUnit = oBusinessUnit;
            #region Page Setup
            _oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
            if (nPageSize == 1)
            {
                //nPageSize means "A4"
                _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595                
            }
            else
            {
                //nPageSize 2 means "Legal"
                _oDocument.SetPageSize(iTextSharp.text.PageSize.LEGAL);
            }
            _oDocument.SetMargins(60f, 40f, 30f, 30f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            //_oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);

            _oDocument.Open();

            _oPdfPTable.SetWidths(new float[] { 595f });
            #endregion

            this.HeaderWithThreeFormats(_oExportCommercialDoc.DocHeader);
            this.Print_BankSubmition();

            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        private void Print_BankSubmition()
        {
            if (_oExportCommercialDoc.FontSize_Normal > 0)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", _oExportCommercialDoc.FontSize_Normal, iTextSharp.text.Font.NORMAL);
                _oFontStyleBold = FontFactory.GetFont("Tahoma", _oExportCommercialDoc.FontSize_Normal, iTextSharp.text.Font.BOLD);
                _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", _oExportCommercialDoc.FontSize_Normal, iTextSharp.text.Font.UNDERLINE);
                _oFontStyle_Bold_UnLine = FontFactory.GetFont("Tahoma", _oExportCommercialDoc.FontSize_Normal, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
            }
            else
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.UNDERLINE);
                _oFontStyle_Bold_UnLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
            }

            #region Blank spacc

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

         

            #region Blank spacc
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            if (!string.IsNullOrEmpty(_oExportCommercialDoc.GRPNoDate))
            {
                _oExportCommercialDoc.GRPNoDate = _oExportCommercialDoc.GRPNoDate;
                if (!string.IsNullOrEmpty(_oExportCommercialDoc.DocDate)) { _oExportCommercialDoc.GRPNoDate = _oExportCommercialDoc.GRPNoDate + "  " + _oExportCommercialDoc.DocDate; }
                else { _oExportCommercialDoc.GRPNoDate = _oExportCommercialDoc.GRPNoDate + "                  ."; }
            }
            //if (string.IsNullOrEmpty(_oExportCommercialDoc.DocDate)) { _oPdfPCell = new PdfPCell(new Phrase("Dated:", _oFontStyle)); }
            //else { _oPdfPCell = new PdfPCell(new Phrase("Dated: " + _oExportCommercialDoc.DocDate, _oFontStyle)); };

            #region Manager Print
            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.GRPNoDate, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Manager Print
            _oPdfPCell = new PdfPCell(new Phrase("To", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Doc
            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.NegotiatingBank, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Advice Bank Name
            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.BankName_Nego, _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Branch Name
            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.BBranchName_Nego, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Bank Address
            PdfPTable oPdfPTable = new PdfPTable(2);
            oPdfPTable.SetWidths(new float[] { 120f, 150f, });

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.BankAddress_Nego, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            //_oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.BankAddress_Nego, _oFontStyle));
            //_oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 15f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();
            #endregion

            #region Blank spacc

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 20f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Subject
             oPdfPTable = new PdfPTable(3);
            oPdfPTable.SetWidths(new float[] { 68f, 530f,15f });

            _oExportCommercialDoc.Certification = _oExportCommercialDoc.Certification.Replace("@AMOUNTLC", _oExportCommercialDoc.Currency + "" + Global.MillionFormat(_oExportCommercialDoc.Amount_LC));
            _oExportCommercialDoc.Certification = _oExportCommercialDoc.Certification.Replace("@AMOUNT", _oExportCommercialDoc.Currency + "" + Global.MillionFormat(_oExportCommercialDoc.Amount_Bill));
            _oExportCommercialDoc.Certification = _oExportCommercialDoc.Certification.Replace("@LCNO", _oExportCommercialDoc.ExportLCNoAndDate + "  " + _oExportCommercialDoc.AmendmentNonDate);
            _oExportCommercialDoc.Certification = _oExportCommercialDoc.Certification.Replace("@INVOICENO", _oExportCommercialDoc.ExportBillNo_Full);
            _oExportCommercialDoc.Certification = _oExportCommercialDoc.Certification.Replace("@BANK_ISSUE", _oExportCommercialDoc.BankName_Issue + ", " + _oExportCommercialDoc.BBranchName_Issue + ", " + _oExportCommercialDoc.BankAddress_Issue);

            _oPdfPCell = new PdfPCell(new Phrase("SUBJECT:", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //_oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
            _oPdfParagraph = new Paragraph(new Phrase(_oExportCommercialDoc.Certification , _oFontStyle));
            _oPdfParagraph.SetLeading(0f, 1.25f);
            _oPdfParagraph.Alignment = iTextSharp.text.Element.ALIGN_JUSTIFIED;
            _oPdfPCell = new PdfPCell();
            _oPdfPCell.AddElement(_oPdfParagraph);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_JUSTIFIED; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
         
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Blank spacc

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 25f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Dear sir
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.ASPERPI))
            {
                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.ASPERPI, _oFontStyle));
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("Dear Sir,", _oFontStyle));
            }
            _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Message
            _oExportCommercialDoc.ClauseOne = _oExportCommercialDoc.ClauseOne.Replace("@LCNO", _oExportCommercialDoc.ExportLCNoAndDate);
            _oExportCommercialDoc.ClauseOne = _oExportCommercialDoc.ClauseOne.Replace("@INVOICENO", _oExportCommercialDoc.ExportBillNo_Full);
            _oExportCommercialDoc.ClauseOne = _oExportCommercialDoc.ClauseOne.Replace("@AMOUNTLC", _oExportCommercialDoc.Currency + "" + Global.MillionFormat(_oExportCommercialDoc.Amount_LC));
            _oExportCommercialDoc.ClauseOne = _oExportCommercialDoc.ClauseOne.Replace("@AMOUNT", _oExportCommercialDoc.Currency + "" + Global.MillionFormat(_oExportCommercialDoc.Amount_Bill));
            _oExportCommercialDoc.ClauseOne = _oExportCommercialDoc.ClauseOne.Replace("@INWORD", "US " + Global.DollarWords(_oExportCommercialDoc.Amount_Bill));

            _oPdfParagraph = new Paragraph(new Phrase(_oExportCommercialDoc.ClauseOne, _oFontStyle));
            _oPdfParagraph.SetLeading(0f, 1.25f);
            _oPdfPCell.AddElement(_oPdfParagraph);
            _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            if (!string.IsNullOrEmpty(_oExportCommercialDoc.TextWithGoodsRow))
            {
                #region Blank spacc
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 20f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion


                _oPdfParagraph = new Paragraph(new Phrase(_oExportCommercialDoc.TextWithGoodsRow, _oFontStyle));
                _oPdfParagraph.SetLeading(0f, 1.25f);
                _oPdfPCell.AddElement(_oPdfParagraph);
                _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #region Enclosed
            oPdfPTable = new PdfPTable(5);
            oPdfPTable.SetWidths(new float[] { 30f,5f, 250f, 10f, 200f });

        
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Colspan = 5; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            int nCount = 0;

            List<ExportDocForwarding> oExportDocForwardings = new List<ExportDocForwarding>();
            oExportDocForwardings = ExportDocForwarding.Gets(_oExportCommercialDoc.ExportLCID, (int)EnumMasterLCType.ExportLC, 0);
            oExportDocForwardings = oExportDocForwardings.Where(d => d.DocumentType != EnumDocumentType.Bank_Forwarding && d.DocumentType != EnumDocumentType.Bank_Submission).ToList();
            foreach (ExportDocForwarding oItem in oExportDocForwardings)
            {
                if (oItem.Name_Print.ToUpper() != "BANK SUBMISSION")
                {
                    nCount++;
                 
                    _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString() + ")", _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                                    

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.Name_Print.ToUpper(), _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


                    if (oItem.Copies <= 0)
                    {

                        _oPdfPCell = new PdfPCell(new Phrase("1 Original   +   " + oItem.Copies + " Copy   ", _oFontStyle));
                        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    }
                    else if (oItem.Copies == 1)
                    {

                        _oPdfPCell = new PdfPCell(new Phrase("1 Original   +   " + oItem.Copies + " Copy   ", _oFontStyle));
                        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    }
                    else
                    {
                        _oPdfPCell = new PdfPCell(new Phrase("1 Original   +   " + oItem.Copies + " Copies   ", _oFontStyle));
                        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    }
                    oPdfPTable.CompleteRow();
                }
            }

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #endregion
           

            #region Blank spacc
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 20f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region ClauseTwo
            _oPdfParagraph = new Paragraph(new Phrase(_oExportCommercialDoc.ClauseTwo, _oFontStyle));
            _oPdfParagraph.SetLeading(0f, 1.25f);
            _oPdfParagraph.Alignment = iTextSharp.text.Element.ALIGN_JUSTIFIED;
            _oPdfPCell = new PdfPCell();
            _oPdfPCell.AddElement(_oPdfParagraph);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_JUSTIFIED; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region ClauseThree
            if (!String.IsNullOrEmpty(_oExportCommercialDoc.ClauseThree))
            {
                #region Blank space
                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                _oPdfParagraph = new Paragraph(new Phrase(_oExportCommercialDoc.ClauseThree, _oFontStyle));
                _oPdfParagraph.SetLeading(0f, 1.25f);
                _oPdfParagraph.Alignment = iTextSharp.text.Element.ALIGN_JUSTIFIED;
                _oPdfPCell = new PdfPCell();
                _oPdfPCell.AddElement(_oPdfParagraph);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_JUSTIFIED; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion

            #region ClauseFour
            if (!String.IsNullOrEmpty(_oExportCommercialDoc.ClauseFour))
            {
                #region Blank space
                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                _oPdfParagraph = new Paragraph(new Phrase(_oExportCommercialDoc.ClauseFour, _oFontStyle));
                _oPdfParagraph.SetLeading(0f, 1.25f);
                _oPdfParagraph.Alignment = iTextSharp.text.Element.ALIGN_JUSTIFIED;
                _oPdfPCell = new PdfPCell();
                _oPdfPCell.AddElement(_oPdfParagraph);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_JUSTIFIED; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion

            #region Blank Row
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region To
            if (!String.IsNullOrEmpty(_oExportCommercialDoc.To))
            {
                #region Blank space
                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                _oPdfParagraph = new Paragraph(new Phrase(_oExportCommercialDoc.To, _oFontStyle));
                _oPdfParagraph.SetLeading(0f, 1.25f);
                _oPdfParagraph.Alignment = iTextSharp.text.Element.ALIGN_JUSTIFIED;
                _oPdfPCell = new PdfPCell();
                _oPdfPCell.AddElement(_oPdfParagraph);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_JUSTIFIED; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion


            #region For
            if (!String.IsNullOrEmpty(_oExportCommercialDoc.For))
            {
                #region Blank space
                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight =10f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.For + " " + _oExportCommercialDoc.BUName, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_JUSTIFIED; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion


            #region AuthorisedSignature
            if (!String.IsNullOrEmpty(_oExportCommercialDoc.AuthorisedSignature))
            {
                #region Blank space
                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 38f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                _oPdfPCell = new PdfPCell(new Phrase("____________________________", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.AuthorisedSignature, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_JUSTIFIED; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion
            
        }

        #endregion

        #region ContryOfOrgin
        public byte[] PrepareReport_CertificateOfORGIN(ExportCommercialDoc oExportCommercialDoc, Company oCompany, int nPrintType, string sMUName, BusinessUnit oBusinessUnit, int nPageSize)
        {

            _oExportCommercialDoc = oExportCommercialDoc;
            _oCompany = oCompany;
            _oBusinessUnit = oBusinessUnit;
            _nPrintType = nPrintType;
            _nMaxHeight = 450;
            _sMUName = sMUName;
            #region Page Setup
            _oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
            if (nPageSize == 1)
            {
                //nPageSize means "A4"
                _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595                
            }
            else
            {
                //nPageSize 2 means "Legal"
                _oDocument.SetPageSize(iTextSharp.text.PageSize.LEGAL);
            }
            _oDocument.SetMargins(30f, 30f, 30f, 30f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            //_oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);

            _oDocument.Open();

            _oPdfPTable.SetWidths(new float[] { 595f });
            #endregion

            this.HeaderWithThreeFormats(_oExportCommercialDoc.DocHeader);
            this.ReportHeader(_oExportCommercialDoc.DocHeader);
            this.PrintBody_CertificateOfORGIN();

            _oPdfPTable.HeaderRows = ((_oExportCommercialDoc.IsPrintHeader) ? 5 : 1);

            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Body
        private void PrintBody_CertificateOfORGIN()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.UNDERLINE);
            _oFontStyle_Bold_UnLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);

            #region 1st Row
            PdfPTable oPdfPTable = new PdfPTable(2);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 280f, 280f });

            #region 1St Row

            _oPdfPCell = new PdfPCell(this.SetDeliveryChallan_Left());
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            if (!String.IsNullOrEmpty(_oExportCommercialDoc.AccountOf))
            {
                _oPdfPCell = new PdfPCell(this.SetDeliveryChallan_Right());
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            }
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

            #endregion


            #region 2nd Row
            oPdfPTable = new PdfPTable(2);
            oPdfPTable.SetWidths(new float[] { 280f, 280f });
            if (!String.IsNullOrEmpty(_oExportCommercialDoc.NegotiatingBank) || !String.IsNullOrEmpty(_oExportCommercialDoc.IssuingBank))
            {
                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.NegotiatingBank, _oFontStyle_UnLine));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.IssuingBank, _oFontStyle_UnLine));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            if (!String.IsNullOrEmpty(_oExportCommercialDoc.NegotiatingBank) || !String.IsNullOrEmpty(_oExportCommercialDoc.IssuingBank))
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
                _oPdfPCell = new PdfPCell(new Phrase("" + _oExportCommercialDoc.BankName_Nego, _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.BankName_Issue, _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.BBranchName_Nego + "\n" + _oExportCommercialDoc.BankAddress_Nego, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.BBranchName_Issue + "\n" + _oExportCommercialDoc.BankAddress_Issue, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase("\n", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("\n", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region 3rd Row
            oPdfPTable = new PdfPTable(2);
            oPdfPTable.SetWidths(new float[] { 280f, 280f });

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.DocumentaryCreditNoDate, _oFontStyle_UnLine));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.NoAndDateOfDoc, _oFontStyle_UnLine));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            //string sAVersionAndDate = "";
            //if (_oExportLC.VersionNoInInt > 0)
            //{
            //    sAVersionAndDate = ",Amendment No: " + _oExportLC.VersionNoInInt + "(" + _oExportLC.AmendmentDateSt + ")";
            //}

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.ExportLCNoAndDate + "" + _oExportCommercialDoc.AmendmentNonDate, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.ExportBillNo_FullDate, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase((!string.IsNullOrEmpty(_oExportCommercialDoc.AllMasterLCNo) ? "Master LC : " : ""), _oFontStyle_UnLine));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase((!string.IsNullOrEmpty(_oExportCommercialDoc.AllMasterLCNo) ? _oExportCommercialDoc.AllMasterLCNo : ""), _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.ProformaInvoiceNoAndDate, _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            _oPhrase = new Phrase();

            if (!string.IsNullOrEmpty(_oExportCommercialDoc.AllExportLCNo))
            {
                _oPhrase.Add(new Chunk(_oExportCommercialDoc.AgainstExportLC + " : \n \n", _oFontStyle_UnLine));
                _oPhrase.Add(new Chunk(_oExportCommercialDoc.AllExportLCNo, _oFontStyle));
            }

            _oPdfPCell = new PdfPCell(_oPhrase);
            _oPdfPCell.Border = 0; _oPdfPCell.Rowspan = 4; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.PINos, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.Rowspan = 4; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            if (_oExportCommercialDoc.Term != "" && _oExportCommercialDoc.Term != "N/A")
            {
                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.Term, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            }
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

            #region 4th Row
            oPdfPTable = new PdfPTable(2);
            oPdfPTable.SetWidths(new float[] { 280f, 280f });

            if ((_oExportCommercialDoc.PortofLoading != "" && _oExportCommercialDoc.PortofLoading != "N/A") || (_oExportCommercialDoc.FinalDestination != "" && _oExportCommercialDoc.FinalDestination != "N/A"))
            {
                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.PortofLoading, _oFontStyle_UnLine));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.FinalDestination, _oFontStyle_UnLine));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
                _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);

                _oPdfPCell = new PdfPCell(new Phrase("" + _oExportCommercialDoc.PortofLoadingName, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.FinalDestinationName, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }

            _oPdfPCell = new PdfPCell(oPdfPTable);


            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(this.SetShipmentTerm());
            _oPdfPCell.Colspan = 2; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            #endregion

            #region Blank space
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion
                     

            #region 6th Row (Product Detail Table)
            if ((_oExportCommercialDoc.BusinessUnitType == (int)EnumBusinessUnitType.Integrated || _oExportCommercialDoc.BusinessUnitType == (int)EnumBusinessUnitType.Plastic) && _oExportCommercialDoc.ProductNature == EnumProductNature.Hanger)
            {
                #region Hanger
                if (_oExportCommercialDoc.IsPrintUnitPrice)
                {
                    _oPdfPCell = new PdfPCell(this.HangerProductWithPrice());
                }
                else
                {
                    _oPdfPCell = new PdfPCell(this.HangerProductWithPrice());
                }
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion
            }
            else if ((_oExportCommercialDoc.BusinessUnitType == (int)EnumBusinessUnitType.Integrated || _oExportCommercialDoc.BusinessUnitType == (int)EnumBusinessUnitType.Plastic) && _oExportCommercialDoc.ProductNature == EnumProductNature.Poly)
            {
                #region Poly
                if (_oExportCommercialDoc.IsPrintUnitPrice)
                {
                    _oPdfPCell = new PdfPCell(this.PloyProductWithPrice());
                }
                else
                {
                    _oPdfPCell = new PdfPCell(this.PloyProductWithPrice());
                }
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion
            }
            else if (_oExportCommercialDoc.BusinessUnitType == (int)EnumBusinessUnitType.Dyeing && _oExportCommercialDoc.ProductNature == EnumProductNature.Dyeing)
            {
                #region Dyeing
                if (_oExportCommercialDoc.IsPrintUnitPrice && _oExportCommercialDoc.IsPrintValue)
                {
                    _oPdfPCell = new PdfPCell(this.ProductDetails_Dyeing_UnitePrice());
                }
                else if (_oExportCommercialDoc.IsPrintValue)
                {
                    _oPdfPCell = new PdfPCell(this.ProductDetails_Dyeing_Amount());
                }
                else
                {
                    _oPdfPCell = new PdfPCell(this.ProductDetails_Dyeing_Qty());
                }
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion
            }
            else if (_oExportCommercialDoc.BusinessUnitType == (int)EnumBusinessUnitType.Weaving || _oExportCommercialDoc.BusinessUnitType == (int)EnumBusinessUnitType.Finishing)
            {
                #region Dyeing
                if (_oExportCommercialDoc.IsPrintUnitPrice && _oExportCommercialDoc.IsPrintValue)
                {
                    _oPdfPCell = new PdfPCell(this.ProductDetails_WU(true));
                }
                else if (_oExportCommercialDoc.IsPrintValue)
                {
                    _oPdfPCell = new PdfPCell(this.ProductDetails_WU(true));
                }
                else
                {
                    _oPdfPCell = new PdfPCell(this.ProductDetails_WU(false));
                }
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion
            }  
            #endregion

            #region 7th Row
            oPdfPTable = new PdfPTable(2);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 100f, 460f });

          
            if (_oExportCommercialDoc.AmountInWord != "" && _oExportCommercialDoc.AmountInWord != "N/A")
            {
                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.AmountInWord, _oFontStyleBold));
                _oPdfPCell.Border = 0;
                _oPdfPCell.BorderWidthTop = 0;
                _oPdfPCell.BorderWidthBottom = 0;
                _oPdfPCell.BorderWidthLeft = 1;
                _oPdfPCell.BorderWidthRight = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.DollarWords(_oExportCommercialDoc.Amount_Bill), _oFontStyleBold));
                _oPdfPCell.Border = 0;
                _oPdfPCell.BorderWidthTop = 0;
                _oPdfPCell.BorderWidthBottom = 0;
                _oPdfPCell.BorderWidthRight = 1;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Blank space

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.FixedHeight = 15f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region 8th Row
            oPdfPTable = new PdfPTable(1);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 560f });

         

            if (_oExportCommercialDoc.Wecertifythat != "" && _oExportCommercialDoc.Wecertifythat != "N/A")
            {
                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.Wecertifythat, _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }

            if (_oExportCommercialDoc.Certification != "" && _oExportCommercialDoc.Certification != "N/A")
            {
                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.Certification, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }

            if (_oExportCommercialDoc.ClauseOne != "" && _oExportCommercialDoc.ClauseOne != "N/A")
            {
                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.ClauseOne, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            if (_oExportCommercialDoc.ClauseTwo != "" && _oExportCommercialDoc.ClauseTwo != "N/A")
            {
                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.ClauseTwo, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            if (_oExportCommercialDoc.ClauseThree != "" && _oExportCommercialDoc.ClauseThree != "N/A")
            {
                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.ClauseThree, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            if (_oExportCommercialDoc.ClauseFour != "" && _oExportCommercialDoc.ClauseFour != "N/A")
            {
                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.ClauseFour, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

     

            #region PrintGrossNetWeight and Authorised Signatuse
            oPdfPTable = new PdfPTable(1);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 560f });

          

            if (_oExportCommercialDoc.IsPrintGrossNetWeight)
            {
                _oPdfPCell = new PdfPCell(this.ShowNetWeight_DU());
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }


            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

            #region Blank space
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 20f; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region ReceiverSignature
            oPdfPTable = new PdfPTable(2);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 280f, 280f });

            //if (_oExportCommercialDoc.IsPrintGrossNetWeight)
            //{
            //    _oPdfPCell = new PdfPCell(this.ShowNetWeight_DU());
            //    _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            //}
            //else
            //{
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //}
            if (_oExportCommercialDoc.For != "")
            {
                _oPdfPCell = new PdfPCell(this.For());
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            }
            oPdfPTable.CompleteRow();
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.FixedHeight = 3f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);




            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.FixedHeight = 3f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

        }

        #endregion
        #endregion

        #region BillOfExchange
        public byte[] PrepareReport_BillOfExchange(ExportCommercialDoc oExportCommercialDoc, Company oCompany, int nPrintType, BusinessUnit oBusinessUnit, int nPageSize)
        {
            _oExportCommercialDoc = oExportCommercialDoc;
            _oCompany = oCompany;
            _nPrintType = nPrintType;
            _oBusinessUnit = oBusinessUnit;

            #region Page Setup
            _oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
            if (nPageSize == 1)
            {
                //nPageSize means "A4"
                _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595                
            }
            else
            {
                //nPageSize 2 means "Legal"
                _oDocument.SetPageSize(iTextSharp.text.PageSize.LEGAL);
            }
            _oDocument.SetMargins(30f, 30f, 50f, 20f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();

            _cb = _oWriter.DirectContent;            
            #endregion
            
            //BaseFont oBaseFont = BaseFont.CreateFont(BaseFont.TIMES_BOLD, BaseFont.CP1252, false);
            //AddWaterMark(_cb, "1", oBaseFont, 300f, -10f, new BaseColor(70, 70, 255), new iTextSharp.text.Rectangle(842f, 595f), new iTextSharp.text.Rectangle(842f, 595f));

            #region New Code
            #region 1st Part
            PdfPTable oPdfPTable2 = new PdfPTable(2);
            oPdfPTable2.SetWidths(new float[] { 400f, 30f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 17f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.DocHeader, _oFontStyle));
            _oPdfPCell.Colspan = 2; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(_oPdfPCell);

            #region (1,2 Caption)
            _oPdfPCell = new PdfPCell(new Phrase("1", FontFactory.GetFont("Tahoma", 18f, iTextSharp.text.Font.BOLDITALIC, BaseColor.GRAY)));
            _oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 2;
            _oPdfPCell.BorderWidthTop = 0;
            _oPdfPCell.BorderWidthBottom = 0;
            _oPdfPCell.BorderWidthLeft = 0.5f;
            _oPdfPCell.BorderWidthRight = 0.5f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable2.AddCell(_oPdfPCell);
            oPdfPTable2.CompleteRow();
            #endregion



            _oPdfPCell = new PdfPCell(this.BOELeftPart(true));
            _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable2.AddCell(_oPdfPCell);

           

            _oFontStyle = FontFactory.GetFont("Tahoma", 17f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.BUName.ToUpper(), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Rotation = 90;
            _oPdfPCell.BorderWidth = 1;
            oPdfPTable2.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(oPdfPTable2);
            _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();            
            #endregion

            this.Blank(60);

            #region 2nd Part
            oPdfPTable2 = new PdfPTable(2);
            oPdfPTable2.SetWidths(new float[] { 400f, 30f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 17f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.DocHeader, _oFontStyle));
            _oPdfPCell.Colspan = 2; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(_oPdfPCell);

            #region (1,2 Caption)
            _oPdfPCell = new PdfPCell(new Phrase("2", FontFactory.GetFont("Tahoma", 18f, iTextSharp.text.Font.BOLDITALIC, BaseColor.GRAY)));
            _oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 2;
            _oPdfPCell.BorderWidthTop = 0;
            _oPdfPCell.BorderWidthBottom = 0;
            _oPdfPCell.BorderWidthLeft = 0.5f;
            _oPdfPCell.BorderWidthRight = 0.5f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable2.AddCell(_oPdfPCell);
            oPdfPTable2.CompleteRow();
            #endregion

            //Page Brack For Baly
            _oDocument.Add(_oPdfPTable);
            _oDocument.NewPage();
            _oPdfPTable.DeleteBodyRows();
            //////

            _oPdfPCell = new PdfPCell(this.BOELeftPart(false));
            _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable2.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 17f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Name.ToUpper(), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Rotation = 90;
            _oPdfPCell.BorderWidth = 1;
            oPdfPTable2.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(oPdfPTable2);
            _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            #endregion

            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Body

        private PdfPTable BOELeftPart(bool bIsTwo)
        {
            _oPdfPTable2 = new PdfPTable(1);
            _oPdfPTable2.SetWidths(new float[] { 200f });
            #region 2nd Raw

            PdfPTable oPdfPTable = new PdfPTable(2);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 100f, 460f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.UNDERLINE);
            _oFontStyle_Bold_UnLine = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);


          


            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 2;
            _oPdfPCell.FixedHeight = 10f;
            _oPdfPCell.BorderWidthTop = 0.5f;
            _oPdfPCell.BorderWidthBottom = 0;
            _oPdfPCell.BorderWidthLeft = 0.5f;
            _oPdfPCell.BorderWidthRight = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.NoAndDateOfDoc, _oFontStyle));
            _oPdfPCell.Border = 0;
            //_oPdfPCell.FixedHeight = 5f;
            _oPdfPCell.BorderWidthTop = 0;
            _oPdfPCell.BorderWidthBottom = 0;
            _oPdfPCell.BorderWidthLeft = 0.5f;
            _oPdfPCell.BorderWidthRight = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell);
            if (_oExportCommercialDoc.IsPrintInvoiceDate)
            {
                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.ExportBill_ChallanNo + "                                           " + "Date:" +_oExportCommercialDoc.ExportBillDate, _oFontStyleBold));
            }
            else  if (!String.IsNullOrEmpty(_oExportCommercialDoc.DocDate))
            {
                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.ExportBill_ChallanNo + "                                           " + "Date:" + _oExportCommercialDoc.DocDate, _oFontStyleBold));
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.ExportBill_ChallanNo + "                                           " + "Date:", _oFontStyleBold));
            }
            _oPdfPCell.Border = 0;
            _oPdfPCell.BorderWidthTop = 0;
            //_oPdfPCell.FixedHeight = 5f;
            _oPdfPCell.BorderWidthBottom = 0;
            _oPdfPCell.BorderWidthRight = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 2;
            _oPdfPCell.FixedHeight = 8f;
            _oPdfPCell.BorderWidthTop = 0;
            _oPdfPCell.BorderWidthBottom = 0;
            _oPdfPCell.BorderWidthLeft = 0.5f;
            _oPdfPCell.BorderWidthRight = 0.5f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.AmountInWord, _oFontStyle));
            _oPdfPCell.Border = 0;
            //_oPdfPCell.FixedHeight = 5f;
            _oPdfPCell.BorderWidthTop = 0;
            _oPdfPCell.BorderWidthBottom = 0;
            _oPdfPCell.BorderWidthLeft = 0.5f;
            _oPdfPCell.BorderWidthRight = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("US " + _oExportCommercialDoc.Currency + "" + Global.MillionFormat(_oExportCommercialDoc.Amount_Bill) + "\n(US " + Global.DollarWords(_oExportCommercialDoc.Amount_Bill) + ")", _oFontStyleBold));
            _oPdfPCell.Border = 0;
            //_oPdfPCell.FixedHeight = 5f;
            _oPdfPCell.BorderWidthTop = 0;
            _oPdfPCell.BorderWidthBottom = 0;
            _oPdfPCell.BorderWidthLeft = 0;
            _oPdfPCell.BorderWidthRight = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.Border = 0;
            _oPdfPCell.MinimumHeight = 5f;
            _oPdfPCell.Colspan = 2;
            _oPdfPCell.BorderWidthTop = 0;
            _oPdfPCell.BorderWidthBottom = 0;
            _oPdfPCell.BorderWidthLeft = 0.5f;
            _oPdfPCell.BorderWidthRight = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable2.AddCell(_oPdfPCell);
            _oPdfPTable2.CompleteRow();



            #endregion

            #region 1st Raw

            oPdfPTable = new PdfPTable(2);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 130f, 410f });


            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.Colspan = 2;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BorderWidthTop = 0.5f;
            _oPdfPCell.BorderWidthBottom = 0;
            _oPdfPCell.BorderWidthLeft = 0.5f;
            _oPdfPCell.BorderWidthRight = 0;
            _oPdfPCell.FixedHeight = 5f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.IssuingBank, _oFontStyle));
            _oPdfPCell.Rowspan = 2;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BorderWidthTop = 0;
            _oPdfPCell.BorderWidthBottom = 0;
            _oPdfPCell.BorderWidthLeft = 0.5f;
            _oPdfPCell.BorderWidthRight = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.BankName_Issue, _oFontStyleBold));
            _oPdfPCell.Colspan = 3;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BorderWidthTop = 0f;
            _oPdfPCell.BorderWidthBottom = 0;
            _oPdfPCell.BorderWidthLeft = 0;
            _oPdfPCell.BorderWidthRight = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.BBranchName_Issue + ", " + _oExportCommercialDoc.BankAddress_Issue, _oFontStyle));
            _oPdfPCell.Border = 0;
            _oPdfPCell.BorderWidthTop = 0;
            _oPdfPCell.BorderWidthBottom = 0;
            _oPdfPCell.BorderWidthLeft = 0;
            _oPdfPCell.BorderWidthRight = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.Colspan = 2;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BorderWidthTop = 0;
            _oPdfPCell.BorderWidthBottom = 0;
            _oPdfPCell.BorderWidthLeft = 0.5f;
            _oPdfPCell.BorderWidthRight = 0;
            _oPdfPCell.FixedHeight = 5f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.DocumentaryCreditNoDate, _oFontStyle));
            _oPdfPCell.Border = 0;
            _oPdfPCell.MinimumHeight = 15f;
            _oPdfPCell.BorderWidthTop = 0.5f;
            _oPdfPCell.BorderWidthBottom = 0;
            _oPdfPCell.BorderWidthLeft = 0.5f;
            _oPdfPCell.BorderWidthRight = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.ExportLCNoAndDate + "" + _oExportCommercialDoc.AmendmentNonDate, _oFontStyle));
            _oPdfPCell.Border = 0;
            _oPdfPCell.MinimumHeight = 15f;
            _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0;
            _oPdfPCell.BorderWidthLeft = 0;
            _oPdfPCell.BorderWidthRight = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            #region Export LC and Master LC
            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.AgainstExportLC, _oFontStyle));
            _oPdfPCell.Border = 0;
            _oPdfPCell.BorderWidthTop = 0;
            _oPdfPCell.BorderWidthBottom = 0;
            _oPdfPCell.BorderWidthLeft = 0.5f;
            _oPdfPCell.BorderWidthRight = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(this.SetMasterAndExportLC(true));
            _oPdfPCell.Border = 0;
            //_oPdfPCell.FixedHeight = 15f;
            _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0;
            _oPdfPCell.BorderWidthLeft = 0;
            _oPdfPCell.BorderWidthRight = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            #endregion



            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable2.AddCell(_oPdfPCell);

            #endregion

            

            #region (Other Options)
            oPdfPTable = new PdfPTable(2);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 260f, 300f });



            List<ExportPartyInfoBill> oExportPartyInfoBills = new List<ExportPartyInfoBill>();
            if (_oExportCommercialDoc.ExportBillDocID > 0)
            { oExportPartyInfoBills = ExportPartyInfoBill.GetsByExportLC( _oExportCommercialDoc.ExportBillDocID, (int)EnumMasterLCType.CommercialDoc, 0); }
            else
            {
                oExportPartyInfoBills = ExportPartyInfoBill.GetsByExportLC( _oExportCommercialDoc.ExportLCID, (int)EnumMasterLCType.ExportLC, 0);
            }
            if (!String.IsNullOrEmpty(_oExportCommercialDoc.HSCode_Head) && !String.IsNullOrEmpty(_oExportCommercialDoc.HSCode))
            {
                ExportPartyInfoBill oExportPartyInfoBill = new ExportPartyInfoBill();
                oExportPartyInfoBill.PartyInfo = _oExportCommercialDoc.HSCode_Head;
                oExportPartyInfoBill.RefNo = _oExportCommercialDoc.HSCode;
                oExportPartyInfoBills.Add(oExportPartyInfoBill);
            }
            if (oExportPartyInfoBills.Count > 0)
            {
                _oPdfPCell = new PdfPCell(this.ExportPartyInfo(false, oExportPartyInfoBills));
                _oPdfPCell.Border = 0;
                _oPdfPCell.BorderWidthLeft = 0.5f;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable2.AddCell(_oPdfPCell);
                _oPdfPTable2.CompleteRow();
            }
            #endregion
            #region Blank space

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0;
            _oPdfPCell.BorderWidthTop = 0;
            _oPdfPCell.BorderWidthBottom = 0;
            _oPdfPCell.BorderWidthLeft = 0.5f;
            _oPdfPCell.FixedHeight = 5f;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable2.AddCell(_oPdfPCell);
            _oPdfPTable2.CompleteRow();

            #endregion
            #region 3rd  Raw

            oPdfPTable = new PdfPTable(2);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 100f, 460f });

          

            if (bIsTwo)
            {
                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.LCTermsName_Full + " " + _oExportCommercialDoc.ClauseOne, _oFontStyle));
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.LCTermsName_Full + " " + _oExportCommercialDoc.ClauseTwo, _oFontStyle));
            }
            _oPdfPCell.Colspan = 2;
             _oPdfPCell.MinimumHeight = 20f;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BorderWidthTop = 0.5f;
            _oPdfPCell.BorderWidthBottom = 0;
            _oPdfPCell.BorderWidthLeft = 0.5f;
            _oPdfPCell.BorderWidthRight = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();

          

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 2;
            _oPdfPCell.FixedHeight = 5f;
            _oPdfPCell.BorderWidthTop = 0.5f;
            _oPdfPCell.BorderWidthBottom = 0;
            _oPdfPCell.BorderWidthLeft = 0.5f;
            _oPdfPCell.BorderWidthRight = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.NegotiatingBank, _oFontStyle));
            _oPdfPCell.Rowspan = 2;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BorderWidthTop = 0;
            _oPdfPCell.BorderWidthBottom = 0;
            _oPdfPCell.BorderWidthLeft = 0.5f;
            _oPdfPCell.BorderWidthRight = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.BankName_Nego, _oFontStyleBold));
            _oPdfPCell.Border = 0;
            _oPdfPCell.BorderWidthTop = 0;
            _oPdfPCell.BorderWidthBottom = 0;
            _oPdfPCell.BorderWidthLeft = 0;
            _oPdfPCell.BorderWidthRight = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.BBranchName_Nego + "," + _oExportCommercialDoc.BankAddress_Nego, _oFontStyle));
            _oPdfPCell.Border = 0;
            _oPdfPCell.BorderWidthTop = 0;
            _oPdfPCell.BorderWidthBottom = 0;
            _oPdfPCell.BorderWidthLeft = 0;
            _oPdfPCell.BorderWidthRight = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 2;
            _oPdfPCell.FixedHeight = 5f;
            _oPdfPCell.BorderWidthTop = 0;
            _oPdfPCell.BorderWidthBottom = 0.5f;
            _oPdfPCell.BorderWidthLeft = 0.5f;
            _oPdfPCell.BorderWidthRight = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable2.AddCell(_oPdfPCell);
            _oPdfPTable2.CompleteRow();


            #endregion

            #region Blank space

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0;
            _oPdfPCell.BorderWidthTop = 0;
            _oPdfPCell.BorderWidthBottom = 0;
            _oPdfPCell.BorderWidthLeft = 0.5f;
            _oPdfPCell.FixedHeight = 10f;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable2.AddCell(_oPdfPCell);
            _oPdfPTable2.CompleteRow();

            #endregion

            #region 4th  Raw
            oPdfPTable = new PdfPTable(2);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 260f, 300f });

         
            _oPhrase = new Phrase();


            //_oFontStyle = FontFactory.GetFont("Tahoma", 11f, 0);
            //_oPhrase.Add(new Chunk(_oExportCommercialDoc.AccountOf + " ", _oFontStyle));
            //_oFontStyle = FontFactory.GetFont("Tahoma", 11f, 1);
            //_oPhrase.Add(new Chunk(_oExportCommercialDoc.ApplicantName + ", Address : ", _oFontStyle));
            ////_oFontStyle = FontFactory.GetFont("Tahoma", 11f, 0);
            ////_oPhrase.Add(new Chunk(_oExportCommercialDoc.ApplicantName, _oFontStyle));

            //if (!String.IsNullOrEmpty(_oExportCommercialDoc.ApplicantAddress))
            //{
            //    _oFontStyle = FontFactory.GetFont("Tahoma", 11f, 0);
            //    _oPhrase.Add(new Chunk(_oExportCommercialDoc.ApplicantAddress, _oFontStyle));
            //}
            _sTemp = _oExportCommercialDoc.Certification;
            if (_sTemp.Contains("@APPLICANT"))
            {
                _sTemp = _sTemp.Replace("@APPLICANT", "");
                _oPhrase.Add(new Chunk(_sTemp + " ", FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL)));
                _oPhrase.Add(new Chunk(_oExportCommercialDoc.ApplicantName + ", " + _oExportCommercialDoc.ApplicantAddress_Full, _oFontStyleBold));
                //_oPhrase.Add(new Chunk(_oExportCommercialDoc.ApplicantName + " " + _oExportCommercialDoc.ApplicantAddress_Full, _oFontStyleBold));
            }
            else
            {
                _oPhrase.Add(new Chunk(_oExportCommercialDoc.Certification + " ", FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL)));
                if (_oExportCommercialDoc.IsPrintInvoiceDate == false && _oExportCommercialDoc.DocDate != "")
                {
                    _oPhrase.Add(new Chunk(_oExportCommercialDoc.ExportBillNo_FullDate + " " + _oExportCommercialDoc.DocDate, _oFontStyleBold));
                }
                else
                {
                    _oPhrase.Add(new Chunk(_oExportCommercialDoc.ExportBillNo_FullDate, _oFontStyleBold));
                }
            }
           
            _oPdfPCell = new PdfPCell(_oPhrase);
            _oPdfPCell.Border = 0;
            _oPdfPCell.BorderWidthLeft = 0.5f;
            _oPdfPCell.Colspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            //_oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.AgainstExportLC + " " + _oExportCommercialDoc.MasterLCNo + " & " + _oExportCommercialDoc.LCTermsName, _oFontStyle));
            //_oPdfPCell.Colspan = 2;
            //_oPdfPCell.FixedHeight = 10f;
            //_oPdfPCell.Border = 0;
            //_oPdfPCell.BorderWidthTop = 0;
            //_oPdfPCell.BorderWidthBottom = 0;
            //_oPdfPCell.BorderWidthLeft = 1;
            //_oPdfPCell.BorderWidthRight = 0;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //_oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //oPdfPTable.AddCell(_oPdfPCell);
            //oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable2.AddCell(_oPdfPCell);
            _oPdfPTable2.CompleteRow();

            #endregion
            
            #region Blank spacc
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0;
            _oPdfPCell.BorderWidthTop = 0;
            _oPdfPCell.BorderWidthBottom = 0;
            _oPdfPCell.BorderWidthLeft = 0.5f;
            _oPdfPCell.FixedHeight = 20f;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable2.AddCell(_oPdfPCell);
            _oPdfPTable2.CompleteRow();
            #endregion
            
            //#region (Other Options)
            //oPdfPTable = new PdfPTable(2);
            //oPdfPTable.WidthPercentage = 100;
            //oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            //oPdfPTable.SetWidths(new float[] { 260f, 300f });




            #region 5th  Raw

            oPdfPTable = new PdfPTable(2);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 280f, 280f });

            //_oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            //_oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            //_oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.UNDERLINE);
            //_oFontStyle_Bold_UnLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);





            #region 
            _oPdfPCell = new PdfPCell(this.SetBOE_To());
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(this.SetBOE_ACC());
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            if (!String.IsNullOrEmpty(_oExportCommercialDoc.For))
            {
                _oPdfPCell = new PdfPCell(this.SetBOE_For());
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            }
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable2.AddCell(_oPdfPCell);
            _oPdfPTable2.CompleteRow();
            #endregion


            #endregion

            return _oPdfPTable2;
        }

        public static void AddWaterMark(PdfContentByte dc, string text, BaseFont font, float fontSize, float angle, BaseColor color, iTextSharp.text.Rectangle realPageSize, iTextSharp.text.Rectangle rect = null)
        {
            var gstate = new PdfGState { FillOpacity = 0.1f, StrokeOpacity = 0.3f };
            dc.SaveState();
            dc.SetGState(gstate);
            dc.SetColorFill(color);
            dc.BeginText();
            dc.SetFontAndSize(font, fontSize);
            var ps = rect ?? realPageSize; /*dc.PdfDocument.PageSize is not always correct*/
            var x = (ps.Right + ps.Left) / 2;
            var y = (ps.Bottom + ps.Top) / 2;
            dc.ShowTextAligned(Element.ALIGN_CENTER, text, x, y, angle);
            dc.EndText();
            dc.RestoreState();
        }

        #endregion
        #endregion

        #region Bank Forwarding

        private void Header_Bank_Forwarding(string sDocHeader)
        {
            #region PAD Format
            _oFontStyle = FontFactory.GetFont("Tahoma", 15f, iTextSharp.text.Font.BOLD);
            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 15f, 4);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; ;
            _oPdfPCell.FixedHeight = 150f; _oPdfPCell.BorderWidth = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion


            _oFontStyle = FontFactory.GetFont("Tahoma", 14f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        public byte[] PrepareReport_Bank_Forwarding(ExportCommercialDoc oExportCommercialDoc, Company oCompany, int nPrintType, List<ExportDocForwarding> oExportDocForwardings, BusinessUnit oBusinessUnit, int nPageSize)
        {

            _oExportCommercialDoc = oExportCommercialDoc;
            _oCompany = oCompany;
            _nPrintType = nPrintType;
            _oBusinessUnit = oBusinessUnit;
            _oExportDocForwardings = oExportDocForwardings;

            #region Page Setup
            _oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
            if (nPageSize == 1)
            {
                //nPageSize means "A4"
                _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595                
            }
            else
            {
                //nPageSize 2 means "Legal"
                _oDocument.SetPageSize(iTextSharp.text.PageSize.LEGAL);
            }
            _oDocument.SetMargins(40f, 40f, 30f, 30f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;


            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);

            _oDocument.Open();

            _oPdfPTable.SetWidths(new float[] { 595f });
            #endregion

            //this.Header_Bank_Forwarding(_oExportCommercialDoc.DocHeader);
            this.HeaderWithThreeFormats("");
            this.PrintBody_Bank_Forwarding();
            _oPdfPTable.HeaderRows = ((_oExportCommercialDoc.IsPrintHeader) ? 5 : 1);

            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Body (Bank Forwarding)
        private void PrintBody_Bank_Forwarding()
        {
            #region 1st Row

            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.SetWidths(new float[] { 100f, 100f, 100f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 11f, iTextSharp.text.Font.NORMAL);
            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.UNDERLINE);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oFontStyle_Bold_UnLine = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle_Bold_UnLine));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase((string.IsNullOrEmpty(_oExportCommercialDoc.CarrierName) ? "" : _oExportCommercialDoc.CarrierName), _oFontStyle_Bold_UnLine));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 20f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 20f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 20f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

            this.Blank(10);

            #region 2nd Row
            oPdfPTable = new PdfPTable(4);
            oPdfPTable.SetWidths(new float[] { 280f, 30f, 95f, 160f });

        

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.IssuingBank, _oFontStyleBold));
            _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.NoAndDateOfDoc, _oFontStyle));
            _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            if (_oExportCommercialDoc.SendToBankDate == DateTime.MinValue)
            {
                _oPdfPCell = new PdfPCell(new Phrase(": " + _oExportCommercialDoc.BillNo + "         /" + DateTime.Today.ToString("yyyy"), _oFontStyle));
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase(": " + _oExportCommercialDoc.BillNo + "         /" + _oExportCommercialDoc.SendToBankDate.ToString("yyyy"), _oFontStyle));
            }
            _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.BankName_Forwarding, _oFontStyleBold));
            _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase("Dated", _oFontStyle));
            _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            if (_oExportCommercialDoc.SendToBankDate != DateTime.MinValue)
            {
                _oPdfPCell = new PdfPCell(new Phrase(": " + _oExportCommercialDoc.SendToBankDate.ToString("dd MMM yyyy"), _oFontStyle));
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase(": " + "", _oFontStyle));
            }
            _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.BBranchName_Forwarding + "\n" + _oExportCommercialDoc.BankAddress_Forwarding, _oFontStyle));
            _oPdfPCell.Rowspan = 4; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.Account, _oFontStyle));
            _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase(": "+_oExportCommercialDoc.CurrencyName+"" + _oExportCommercialDoc.Currency +""+ Global.MillionFormat(_oExportCommercialDoc.Amount_Bill), _oFontStyle));
            _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            //_oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.IssuingBank, _oFontStyle));
            //_oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.TermsofPayment, _oFontStyle));
            _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase(": " + _oExportCommercialDoc.LCTermsName, _oFontStyle));
            _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            //_oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.IssuingBank, _oFontStyle));
            //_oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.DocumentaryCreditNoDate, _oFontStyle));
            _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase(": " + _oExportCommercialDoc.ExportLCNo, _oFontStyle));
            _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            //_oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.IssuingBank, _oFontStyle));
            //_oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase("Dated", _oFontStyle));
            _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase(": " + _oExportCommercialDoc.LCOpeningDate, _oFontStyle));
            _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.Beneficiary, _oFontStyle_Bold_UnLine));
            _oPdfPCell.MinimumHeight = 23f; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPCell = new PdfPCell(new Phrase("Dated", _oFontStyle));
            //_oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.AccountOf, _oFontStyle_Bold_UnLine));
            _oPdfPCell.MinimumHeight = 23f; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.Colspan = 2; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Name, _oFontStyleBold));
            _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPCell = new PdfPCell(new Phrase("Dated", _oFontStyle));
            //_oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.ApplicantName, _oFontStyleBold));
            _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.Colspan = 2; _oPdfPCell.BorderWidthTop =0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Address, _oFontStyle));
             _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.Account, _oFontStyle));
            //_oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.ApplicantAddress_Full, _oFontStyle));
            _oPdfPCell.Colspan = 2; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            this.Blank(10);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.ClauseOne, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #region Enclosed
            oPdfPTable = new PdfPTable(5);
            oPdfPTable.SetWidths(new float[] { 30f, 5f, 250f, 10f, 200f });


            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Colspan = 5; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            int nCount = 0;

            List<ExportBankForwarding> oExportBankForwardings = new List<ExportBankForwarding>();
            oExportBankForwardings = ExportBankForwarding.Gets(_oExportCommercialDoc.ExportBillID, 0);
            //oExportBankForwardings = oExportBankForwardings.Where(d => d.DocumentType != EnumDocumentType.Bank_Forwarding && d.DocumentType != EnumDocumentType.Bank_Submission).ToList();
            foreach (ExportBankForwarding oItem in oExportBankForwardings)
            {
                if ((oItem.Copies_Original + oItem.Copies) > 0)
                {
                    nCount++;

                    _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString() + ")", _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);



                    _oPdfPCell = new PdfPCell(new Phrase(oItem.Name_Print, _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(((oItem.Copies_Original <= 0) ? "" : oItem.Copies_Original.ToString()) + " " + ((oItem.Copies <= 0) ? "" : "+ "+oItem.Copies.ToString()), _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                }

                oPdfPTable.CompleteRow();
            }
         
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            this.Blank(8);
            #region 3rd Row LC Table
            oPdfPTable = new PdfPTable(2);
            oPdfPTable.SetWidths(new float[] { 100f,300f });

            //_oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.ClauseTwo, _oFontStyle));
            _oPdfPCell.MinimumHeight = 55f; _oPdfPCell.Colspan = 2; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.ClauseThree, _oFontStyle));
           _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.ClauseFour, _oFontStyle));
            _oPdfPCell.MinimumHeight = 20f; _oPdfPCell.Colspan = 2; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
         

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            this.Blank(10);



            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.ReceiverCluse, _oFontStyle));
            _oPdfPCell.MinimumHeight = 40f; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.CTPApplicant, _oFontStyle));
            _oPdfPCell.MinimumHeight = 40f; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            this.Blank(20);

            #region 6th Row (Signature)
            oPdfPTable = new PdfPTable(3);
            oPdfPTable.SetWidths(new float[] { 200f, 100f, 200f });

            //_oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.AuthorisedSignature, _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.ReceiverSignature, _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }

        #endregion
        #endregion

        #region Supporting Functions
        private string GetCopiesNumber(string sDoc_Name)
        {
            foreach (ExportDocForwarding oItem in _oExportDocForwardings)
            {
                if (oItem.Name_Print.ToUpper() == sDoc_Name)
                {
                    _nTotalCopies += oItem.Copies;
                    return oItem.Copies.ToString();
                }
            }
            return "";
        }
        private PdfPTable Bank_ForwardClauses()
        {
            PdfPTable oPdfPTable2 = new PdfPTable(1);
            oPdfPTable2.SetWidths(new float[] { 200f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 8f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(_oPdfPCell);
            oPdfPTable2.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("    PLEASE CREDIT OR REMIT THE PROCEED BT : (Tick marked)", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(_oPdfPCell);
            oPdfPTable2.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("          1." + _oExportCommercialDoc.ClauseOne, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(_oPdfPCell);
            oPdfPTable2.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("          2." + _oExportCommercialDoc.ClauseTwo, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(_oPdfPCell);
            oPdfPTable2.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("          3." + _oExportCommercialDoc.ClauseThree, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(_oPdfPCell);
            oPdfPTable2.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("    " + _oExportCommercialDoc.ClauseFour, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(_oPdfPCell);
            oPdfPTable2.CompleteRow();

            return oPdfPTable2;
        }
     
        private PdfPTable ForwardingBank()
        {
            PdfPTable oPdfPTable2 = new PdfPTable(1);
            oPdfPTable2.SetWidths(new float[] { 200f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);

            if (!string.IsNullOrEmpty(_oExportCommercialDoc.BankName_Forwarding))
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.BankName_Forwarding, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(_oPdfPCell);
                oPdfPTable2.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.BBranchName_Forwarding, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(_oPdfPCell);
                oPdfPTable2.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.BankAddress_Forwarding, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(_oPdfPCell);
                oPdfPTable2.CompleteRow();
            }
            return oPdfPTable2;
        }
        private PdfPTable NegotiateBank()
        {
            PdfPTable oPdfPTable2 = new PdfPTable(1);
            oPdfPTable2.SetWidths(new float[] { 200f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);

            if (!string.IsNullOrEmpty(_oExportCommercialDoc.BankName_Issue))
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.BankName_Issue, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(_oPdfPCell);
                oPdfPTable2.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.BBranchName_Issue, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(_oPdfPCell);
                oPdfPTable2.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.BankAddress_Issue, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(_oPdfPCell);
                oPdfPTable2.CompleteRow();
            }
            return oPdfPTable2;
        }
        private PdfPTable ExportPartyInfo(bool bIsBillOfExchange, List<ExportPartyInfoBill> oExportPartyInfoBills)
        {
            PdfPTable oPdfPTable3 = new PdfPTable(1);
            oPdfPTable3.SetWidths(new float[] { 200f });
         
            _oPhrase = new Phrase();
            int nCount = 0;
            foreach (ExportPartyInfoBill oItem in oExportPartyInfoBills)
            {
                if (nCount > 0)
                {
                    if (nCount == oExportPartyInfoBills.Count - 1)
                    {
                        if (!string.IsNullOrEmpty(oItem.RefNo))
                        {
                            _oPhrase.Add(new Chunk(" AND ", _oFontStyle));
                        }
                    }
                    else
                    {
                        _oPhrase.Add(new Chunk(", ", _oFontStyle));
                    }
                }
                if (!string.IsNullOrEmpty(oItem.RefNo))
                {
                    _oPhrase.Add(new Chunk(oItem.PartyInfo + " : ", _oFontStyle));
                    _oPhrase.Add(new Chunk(oItem.RefNo, _oFontStyle));

                    if (!string.IsNullOrEmpty(oItem.RefDate))
                    {
                        _oPhrase.Add(new Chunk(" DATE " + oItem.RefDate, _oFontStyle));
                    }
                }
                nCount++;
            }
        

            if (!string.IsNullOrEmpty(_oExportCommercialDoc.SpecialNote))
            {
                _oPhrase.Add(new Chunk("  " + _oExportCommercialDoc.SpecialNote , _oFontStyleBold));
            }
            _oPdfPCell = new PdfPCell(_oPhrase);
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable3.AddCell(_oPdfPCell);
            oPdfPTable3.CompleteRow();
            return oPdfPTable3;
        }
       
        private PdfPTable SetShipmentTerm()
        {
            PdfPTable oPdfPTable2 = new PdfPTable(2);
            oPdfPTable2.SetWidths(new float[] { 100f, 400f });

            #region PortofLoading
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.PortofLoading))
            {
                _oPhrase = new Phrase();

                #region Port Of Loading
                if (!string.IsNullOrEmpty(_oExportCommercialDoc.PortofLoading))
                {
                    _oPhrase.Add(new Chunk(_oExportCommercialDoc.PortofLoading + " : ", _oFontStyleBold));

                    _oPhrase.Add(new Chunk(_oExportCommercialDoc.PortofLoadingName, _oFontStyle));
                }
                #endregion

                _oPdfPCell = new PdfPCell(_oPhrase);
                _oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable2.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.PortofLoadingAddress, _oFontStyle));
                _oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable2.AddCell(_oPdfPCell);
                oPdfPTable2.CompleteRow();
            }
            #endregion

            #region FinalDestination
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.FinalDestination))
            {
                _oPhrase = new Phrase();

                #region Port Of Loading
                if (!string.IsNullOrEmpty(_oExportCommercialDoc.FinalDestination))
                {
                    _oPhrase.Add(new Chunk(_oExportCommercialDoc.FinalDestination + " : ", _oFontStyleBold));

                    _oPhrase.Add(new Chunk(_oExportCommercialDoc.FinalDestinationName + " : ", _oFontStyle));

                }
                #endregion

                _oPdfPCell = new PdfPCell(_oPhrase);
                _oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable2.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable2.AddCell(_oPdfPCell);
                oPdfPTable2.CompleteRow();
            }
            #endregion

            #region CountryofOrigin
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.CountryofOrigin))
            {
                _oPhrase = new Phrase();

                #region Port Of Loading
                if (!string.IsNullOrEmpty(_oExportCommercialDoc.CountryofOrigin))
                {
                    _oPhrase.Add(new Chunk(_oExportCommercialDoc.CountryofOrigin + " : ", _oFontStyleBold));
                    _oPhrase.Add(new Chunk(_oExportCommercialDoc.CountryofOriginName, _oFontStyle));

                }
                #endregion

                _oPdfPCell = new PdfPCell(_oPhrase);
                _oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable2.AddCell(_oPdfPCell);


            }
            #endregion

            #region Carrier
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.Carrier))
            {
                _oPhrase = new Phrase();

                #region Port Of Loading
                if (!string.IsNullOrEmpty(_oExportCommercialDoc.Carrier))
                {
                    _oPhrase.Add(new Chunk(_oExportCommercialDoc.Carrier + " : ", _oFontStyleBold));
                }
                #endregion

                #region CarrierName
                if (!string.IsNullOrEmpty(_oExportCommercialDoc.Carrier))
                {
                    _oPhrase.Add(new Chunk(_oExportCommercialDoc.CarrierName, _oFontStyle));
                }
                #endregion

                _oPdfPCell = new PdfPCell(_oPhrase);
                _oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.BorderWidthTop = 0.5f;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable2.AddCell(_oPdfPCell);

                oPdfPTable2.CompleteRow();
            }
            #endregion

            #region ShippingMark
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.ShippingMark))
            {
                _oPhrase = new Phrase();

                #region ShippingMark
                if (!string.IsNullOrEmpty(_oExportCommercialDoc.ShippingMark))
                {
                    _oPhrase.Add(new Chunk(_oExportCommercialDoc.ShippingMark+" : ", _oFontStyleBold));
                }
                #endregion

                #region CarrierName
                if (!string.IsNullOrEmpty(_oExportCommercialDoc.ShippingMark) && !string.IsNullOrEmpty(_oExportCommercialDoc.ShippingMarkName))
                {
                    _oPhrase.Add(new Chunk(_oExportCommercialDoc.ShippingMarkName, _oFontStyle));
                }
                #endregion

                _oPdfPCell = new PdfPCell(_oPhrase);
                _oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.BorderWidthTop = 0.5f;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable2.AddCell(_oPdfPCell);
                oPdfPTable2.CompleteRow();
            }
            #endregion

            #region SellingOnAbout
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.SellingOnAbout))
            {
                _oPhrase = new Phrase();

                #region SellingOnAbout
                if (!string.IsNullOrEmpty(_oExportCommercialDoc.SellingOnAbout))
                {
                    _oPhrase.Add(new Chunk(_oExportCommercialDoc.SellingOnAbout, _oFontStyleBold));
                }
                #endregion

                #region CarrierName
                if (!string.IsNullOrEmpty(_oExportCommercialDoc.SellingOnAbout) && !string.IsNullOrEmpty(_oExportCommercialDoc.SellingOnAboutName))
                {
                    _oPhrase.Add(new Chunk(_oExportCommercialDoc.SellingOnAboutName, _oFontStyle));
                }
                #endregion

                _oPdfPCell = new PdfPCell(_oPhrase);
                _oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.BorderWidthTop = 0.5f;
                _oPdfPCell.FixedHeight = 25f;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable2.AddCell(_oPdfPCell);

                oPdfPTable2.CompleteRow();
            }
            #endregion

            #region TruckNo
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.TruckNo_Print))
            {
                _oPhrase = new Phrase();

                #region TruckNo
                if (!string.IsNullOrEmpty(_oExportCommercialDoc.TruckNo))
                {
                    _oPhrase.Add(new Chunk(_oExportCommercialDoc.TruckNo_Print + ": ", _oFontStyleBold));
                }
                #endregion

                #region CarrierName
                if (!string.IsNullOrEmpty(_oExportCommercialDoc.TruckNo))
                {
                    _oPhrase.Add(new Chunk(_oExportCommercialDoc.TruckNo, _oFontStyle));
                }
                #endregion

                _oPdfPCell = new PdfPCell(_oPhrase);
                _oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.BorderWidthTop = 0.5f;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable2.AddCell(_oPdfPCell);

                oPdfPTable2.CompleteRow();
            }
            #endregion

            return oPdfPTable2;
        }
        private PdfPTable PortOfLoading()
        {
            PdfPTable oPdfPTable2 = new PdfPTable(1);
            oPdfPTable2.SetWidths(new float[] { 100f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.UNDERLINE);

            _oPhrase = new Phrase();

            #region Port Of Loading
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.PortofLoading))
            {
                _oPhrase.Add(new Chunk(_oExportCommercialDoc.PortofLoading + " : \n \n", _oFontStyle_UnLine));
            }
            #endregion

            #region Port Of Loading Name
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.PortofLoadingName))
            {
                _oPhrase.Add(new Chunk(_oExportCommercialDoc.PortofLoadingName, _oFontStyle));
            }
            #endregion

            _oPdfPCell = new PdfPCell(_oPhrase);
            _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable2.AddCell(_oPdfPCell);
            oPdfPTable2.CompleteRow();

            return oPdfPTable2;
        }
        private PdfPTable SetMasterAndExportLC(bool bIsBOE)
        {

            PdfPTable oPdfPTable2 = new PdfPTable(2);
            oPdfPTable2.SetWidths(new float[] { 100f, 400f });
            //if (bIsBOE)
            //{
            //    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            //    _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            //}
            //else
            //{
            //    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            //    _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            //}

            #region Export LC
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.AllMasterLCNo))
            {
                _oPdfParagraph = new Paragraph(new Phrase(_oExportCommercialDoc.AllMasterLCNo, _oFontStyle));
                _oPdfParagraph.SetLeading(0f, 1.2f);
                _oPdfParagraph.Alignment = Element.ALIGN_JUSTIFIED;
                _oPdfPCell = new PdfPCell();
                _oPdfPCell.AddElement(_oPdfParagraph);

                //_oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.AllMasterLCNo, _oFontStyleBold));
                _oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable2.AddCell(_oPdfPCell);

                oPdfPTable2.CompleteRow();
            }
            #endregion


            return oPdfPTable2;
        }
  
    
        private PdfPTable ShowNetWeight_DU()
        {
            PdfPTable oPdfPTable2 = new PdfPTable(3);
            oPdfPTable2.SetWidths(new float[] { 80f, 80f, 280f });

            if (_oExportCommercialDoc.IsPrintGrossNetWeight)
            {

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.NetWeight + ": ", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase((Global.MillionFormatActualDigit(Math.Round(_nTotalQty,4))) + " " + _sMUName, _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(_oPdfPCell);
                oPdfPTable2.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.GrossWeight + ": ", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(_nTotalQty + Math.Round((_nTotalQty *_oExportCommercialDoc.GrossWeightPTage ), 3)) + " " + _sMUName, _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(_oPdfPCell);
                oPdfPTable2.CompleteRow();

                _nTotalNoOfBag = _oExportCommercialDoc.ExportBillDetails.Sum(x => x.NoOfBag);
                if (_nTotalNoOfBag > 0)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("Total : ", _oFontStyleBold));
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(_oPdfPCell);
                    _oPdfPCell = new PdfPCell(new Phrase( _nTotalNoOfBag.ToString() + " " + _oExportCommercialDoc.Bag_Name, _oFontStyleBold));
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(_oPdfPCell);
                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(_oPdfPCell);
                    oPdfPTable2.CompleteRow();
                }

            }
            //Also used in Beneficiary Certificate (Search "_oExportLC.DeliveryToName")
            return oPdfPTable2;
        }
        private PdfPTable ShowNetWeight_WU()
        {
            PdfPTable oPdfPTable2 = new PdfPTable(2);
            oPdfPTable2.SetWidths(new float[] { 80f, 120f });

             if (_oExportCommercialDoc.IsPrintGrossNetWeight)
            {

                _oPdfPCell = new PdfPCell(new Phrase("TOTAL QTY =", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(_nTotalQty) + " " + _oExportCommercialDoc.MUName, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(_oPdfPCell);

                oPdfPTable2.CompleteRow();

               _nTotalNoOfBag= _oExportCommercialDoc.ExportBillDetails.Sum(x => x.NoOfBag);
              
                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.NoOfBag + " =", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase((Math.Round((Math.Round(_nTotalNoOfBag)), 0)).ToString() + " Roll", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(_oPdfPCell);
                oPdfPTable2.CompleteRow();

                _nTWeight_Net = _oExportCommercialDoc.ExportBillDetails.Sum(x => x.WtPerBag);

                _nTWeight_Gross = (_nTWeight_Net + _nTWeight_Net * 0.01);

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.NetWeight + " =", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase((Global.MillionFormat(_nTWeight_Net)) + " kg", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(_oPdfPCell);
                oPdfPTable2.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.GrossWeight + " =", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTWeight_Gross) + " kg", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(_oPdfPCell);
                oPdfPTable2.CompleteRow();

                
            }
            //Also used in Beneficiary Certificate (Search "_oExportLC.DeliveryToName")
            return oPdfPTable2;
        }
        private PdfPTable For()
        {
            PdfPTable oPdfPTable2 = new PdfPTable(1);
            oPdfPTable2.SetWidths(new float[] { 200f });

            if (_oExportCommercialDoc.For != "" && _oExportCommercialDoc.For != "N/A")
            {
                if (_oExportCommercialDoc.ForCaptionInDubleLine)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.For, _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(_oPdfPCell);
                    oPdfPTable2.CompleteRow();

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(_oPdfPCell);
                    oPdfPTable2.CompleteRow();
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.For, _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(_oPdfPCell);
                    oPdfPTable2.CompleteRow();
                }

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 45f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(_oPdfPCell);
                oPdfPTable2.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase("_____________________________", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(_oPdfPCell);
                oPdfPTable2.CompleteRow();

                if (String.IsNullOrEmpty(_oExportCommercialDoc.AuthorisedSignature))
                {
                    _oExportCommercialDoc.AuthorisedSignature = "";
                }
                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.AuthorisedSignature, _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(_oPdfPCell);
                oPdfPTable2.CompleteRow();

            }
            return oPdfPTable2;
        }
        private PdfPTable To()
        {
            PdfPTable oPdfPTable2 = new PdfPTable(1);
            oPdfPTable2.SetWidths(new float[] { 200f });

            if (!string.IsNullOrEmpty(_oExportCommercialDoc.To))
            {
                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.To, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(_oPdfPCell);
                oPdfPTable2.CompleteRow();
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(_oPdfPCell);
                oPdfPTable2.CompleteRow();
            }

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 45f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(_oPdfPCell);
            oPdfPTable2.CompleteRow();
            if (!String.IsNullOrEmpty(_oExportCommercialDoc.ReceiverSignature))
            {
                _oPdfPCell = new PdfPCell(new Phrase("_____________________________", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(_oPdfPCell);
                oPdfPTable2.CompleteRow();


                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.ReceiverSignature, _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(_oPdfPCell);
                oPdfPTable2.CompleteRow();

                if (!String.IsNullOrEmpty(_oExportCommercialDoc.ReceiverCluse))
                {
                    _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.ReceiverCluse, _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(_oPdfPCell);
                    oPdfPTable2.CompleteRow();
                }
            }
            return oPdfPTable2;
        }
        private PdfPTable SetCommercialInvoice_Left()
        {

            PdfPTable oPdfPTable2 = new PdfPTable(2);
            oPdfPTable2.SetWidths(new float[] { 15f, 265f });
            #region Beneficiary
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.Beneficiary))
            {

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.Beneficiary, _oFontStyle_UnLine));
                _oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable2.AddCell(_oPdfPCell);
                oPdfPTable2.CompleteRow();

                if (!string.IsNullOrEmpty(_oExportCommercialDoc.BUName))
                {
                    #region Balnk Space
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyleBold));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    #endregion

                    _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.BUName, _oFontStyleBold));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    oPdfPTable2.CompleteRow();

                }
                if (!string.IsNullOrEmpty(_oExportCommercialDoc.BUAddress))
                {
                    #region Balnk Space
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyleBold));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    #endregion

                    _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.BUAddress, _oFontStyle));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    oPdfPTable2.CompleteRow();

                }

            }
            #endregion
            #region AccountOf
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.AccountOf))
            {

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.AccountOf, _oFontStyle_UnLine));
                _oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable2.AddCell(_oPdfPCell);
                oPdfPTable2.CompleteRow();

                if (!string.IsNullOrEmpty(_oExportCommercialDoc.ApplicantName))
                {
                    #region Balnk Space
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyleBold));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    #endregion

                    _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.ApplicantName, _oFontStyleBold));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    oPdfPTable2.CompleteRow();

                }
                if (!string.IsNullOrEmpty(_oExportCommercialDoc.ApplicantAddress_Full))
                {
                    #region Balnk Space
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyleBold));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    #endregion

                    _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.ApplicantAddress_Full, _oFontStyle));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    oPdfPTable2.CompleteRow();

                }

            }
            #endregion
            #region NotifyParty
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.NotifyParty))
            {
                string sTemp = "";
                string sTempTwo = "";
                if (_oExportCommercialDoc.NotifyBy != EnumNotifyBy.None)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.NotifyParty, _oFontStyle_UnLine));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.Colspan = 2;
                    _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    oPdfPTable2.CompleteRow();
                    if (_oExportCommercialDoc.NotifyBy == EnumNotifyBy.Bank)
                    {
                        sTemp = _oExportCommercialDoc.IssuingBank + "\n" + _oExportCommercialDoc.BBranchName_Issue + "\n" + _oExportCommercialDoc.BankAddress_Issue;
                    }
                    else if (_oExportCommercialDoc.NotifyBy == EnumNotifyBy.Party)
                    {
                        sTemp = _oExportCommercialDoc.ApplicantName + "\n" + _oExportCommercialDoc.ApplicantAddress;
                    }
                    else if (_oExportCommercialDoc.NotifyBy == EnumNotifyBy.Party_Bank)
                    {
                        sTemp = _oExportCommercialDoc.ApplicantName + "\n" + _oExportCommercialDoc.ApplicantAddress;
                        sTempTwo = _oExportCommercialDoc.BankName_Issue + "\n" + _oExportCommercialDoc.BBranchName_Issue + "\n"+ _oExportCommercialDoc.BankAddress_Issue;
                    }
                    else if (_oExportCommercialDoc.NotifyBy == EnumNotifyBy.ThirdParty_Bank)
                    {
                        sTemp = _oExportCommercialDoc.DeliveryToName + "\n" + _oExportCommercialDoc.DeliveryToAddress;
                        sTempTwo = _oExportCommercialDoc.BankName_Issue + "\n" + _oExportCommercialDoc.BBranchName_Issue + "\n" + _oExportCommercialDoc.BankAddress_Issue;
                    }
                    else if (_oExportCommercialDoc.NotifyBy == EnumNotifyBy.ThirdParty)
                    {
                        sTemp = _oExportCommercialDoc.DeliveryToName + "\n" + _oExportCommercialDoc.DeliveryToAddress;
                    }

                    if (!string.IsNullOrEmpty(sTemp))
                    {
                        #region Balnk Space
                        _oPdfPCell = new PdfPCell(new Phrase("A)", _oFontStyle));
                        _oPdfPCell.Border = 0;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPTable2.AddCell(_oPdfPCell);
                        #endregion

                        _oPdfPCell = new PdfPCell(new Phrase(sTemp, _oFontStyle));
                        _oPdfPCell.Border = 0;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPTable2.AddCell(_oPdfPCell);
                        oPdfPTable2.CompleteRow();

                    }
                    if (!string.IsNullOrEmpty(sTempTwo))
                    {
                        #region Balnk Space
                        _oPdfPCell = new PdfPCell(new Phrase("B)", _oFontStyle));
                        _oPdfPCell.Border = 0;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPTable2.AddCell(_oPdfPCell);
                        #endregion

                        _oPdfPCell = new PdfPCell(new Phrase(sTempTwo, _oFontStyle));
                        _oPdfPCell.Border = 0;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPTable2.AddCell(_oPdfPCell);
                        oPdfPTable2.CompleteRow();

                    }
                }
                
            }
            #endregion
            
            #region NegotiatingBank
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.NegotiatingBank))
            {
                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.NegotiatingBank, _oFontStyle_UnLine));
                _oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable2.AddCell(_oPdfPCell);
                oPdfPTable2.CompleteRow();

                if (!string.IsNullOrEmpty(_oExportCommercialDoc.BankName_Nego))
                {
                    #region Balnk Space
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyleBold));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    #endregion

                    _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.BankName_Nego, _oFontStyleBold));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    oPdfPTable2.CompleteRow();
                }
                if (!string.IsNullOrEmpty(_oExportCommercialDoc.BBranchName_Nego))
                {
                    #region Balnk Space
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyleBold));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    #endregion

                    _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.BBranchName_Nego + "\n" + _oExportCommercialDoc.BankAddress_Nego, _oFontStyle));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    oPdfPTable2.CompleteRow();

                }

            }
            #endregion
            #region ToTheOrderOf
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.ToTheOrderOf))
            {
                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.ToTheOrderOf, _oFontStyle_UnLine));
                _oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable2.AddCell(_oPdfPCell);
                oPdfPTable2.CompleteRow();

                if (_oExportCommercialDoc.OrderOfBankType == EnumBankType.IssueBank) { _oExportCommercialDoc.BankName_Nego = _oExportCommercialDoc.BankName_Issue; }
                if (_oExportCommercialDoc.OrderOfBankType == EnumBankType.Nego_Bank) { _oExportCommercialDoc.BankName_Nego = _oExportCommercialDoc.BankName_Nego; }
                if (_oExportCommercialDoc.OrderOfBankType == EnumBankType.Forwarding_Bank) { _oExportCommercialDoc.BankName_Nego = _oExportCommercialDoc.BankName_Forwarding; }
                if (_oExportCommercialDoc.OrderOfBankType == EnumBankType.Endorse_Bank) { _oExportCommercialDoc.BankName_Nego = _oExportCommercialDoc.BankName_Endorse; }

                //  _oExportCommercialDoc.BBranchName_Nego = _oExportCommercialDoc.BBranchName_Nego + "\n" + _oExportCommercialDoc.BankAddress_Nego; 
                if (_oExportCommercialDoc.OrderOfBankType == EnumBankType.IssueBank) { _oExportCommercialDoc.BBranchName_Nego = _oExportCommercialDoc.BBranchName_Issue + "\n" + _oExportCommercialDoc.BankAddress_Issue; }
                if (_oExportCommercialDoc.OrderOfBankType == EnumBankType.Nego_Bank || _oExportCommercialDoc.OrderOfBankType == EnumBankType.None) { _oExportCommercialDoc.BBranchName_Nego = _oExportCommercialDoc.BBranchName_Nego + "\n" + _oExportCommercialDoc.BankAddress_Nego; }
                if (_oExportCommercialDoc.OrderOfBankType == EnumBankType.Forwarding_Bank) { _oExportCommercialDoc.BBranchName_Nego = _oExportCommercialDoc.BBranchName_Forwarding + "\n" + _oExportCommercialDoc.BankAddress_Forwarding; }
                if (_oExportCommercialDoc.OrderOfBankType == EnumBankType.Endorse_Bank) { _oExportCommercialDoc.BBranchName_Nego = _oExportCommercialDoc.BBranchName_Endorse + "\n" + _oExportCommercialDoc.BankAddress_Endorse; }



                if (!string.IsNullOrEmpty(_oExportCommercialDoc.BankName_Nego))
                {
                    #region Balnk Space
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyleBold));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    #endregion

                    _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.BankName_Nego, _oFontStyleBold));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    oPdfPTable2.CompleteRow();
                }
                if (!string.IsNullOrEmpty(_oExportCommercialDoc.BBranchName_Nego))
                {
                    #region Balnk Space
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyleBold));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    #endregion

                    _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.BBranchName_Nego , _oFontStyle));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    oPdfPTable2.CompleteRow();

                }

            }
            #endregion

            #region PortofLoading
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.PortofLoading))
            {
                _oPhrase = new Phrase();

                #region Port Of Loading
                if (!string.IsNullOrEmpty(_oExportCommercialDoc.PortofLoading))
                {
                    _oPhrase.Add(new Chunk(_oExportCommercialDoc.PortofLoading + " : ", _oFontStyle_UnLine));

                    _oPhrase.Add(new Chunk(_oExportCommercialDoc.PortofLoadingName, _oFontStyle));
                }
                #endregion

                _oPdfPCell = new PdfPCell(_oPhrase);
                _oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable2.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.PortofLoadingAddress, _oFontStyle));
                _oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable2.AddCell(_oPdfPCell);
                oPdfPTable2.CompleteRow();
            }
            #endregion

            #region FinalDestination
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.FinalDestination))
            {
                _oPhrase = new Phrase();

                #region Port Of Loading
                if (!string.IsNullOrEmpty(_oExportCommercialDoc.FinalDestination))
                {
                    _oPhrase.Add(new Chunk(_oExportCommercialDoc.FinalDestination + " : ", _oFontStyle_UnLine));
                    _oPhrase.Add(new Chunk(_oExportCommercialDoc.FinalDestinationName, _oFontStyle));

                }
                #endregion

                _oPdfPCell = new PdfPCell(_oPhrase);
                _oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable2.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable2.AddCell(_oPdfPCell);
                oPdfPTable2.CompleteRow();
            }
            #endregion

            #region CountryofOrigin
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.CountryofOrigin))
            {
                _oPhrase = new Phrase();

                #region Port Of Loading
                if (!string.IsNullOrEmpty(_oExportCommercialDoc.CountryofOrigin))
                {
                    _oPhrase.Add(new Chunk(_oExportCommercialDoc.CountryofOrigin + " : ", _oFontStyle));
                    _oPhrase.Add(new Chunk(_oExportCommercialDoc.CountryofOriginName, _oFontStyleBold));

                }
                #endregion

                _oPdfPCell = new PdfPCell(_oPhrase);
                _oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable2.AddCell(_oPdfPCell);


            }
            #endregion
            #region GRPNoDate
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.GRPNoDate))
            {
                _oPhrase = new Phrase();

                #region Port Of Loading
                if (!string.IsNullOrEmpty(_oExportCommercialDoc.GRPNoDate))
                {
                    _oPhrase.Add(new Chunk("GRP : ", _oFontStyle));
                    _oPhrase.Add(new Chunk(_oExportCommercialDoc.GRPNoDate, _oFontStyleBold));

                }
                #endregion

                _oPdfPCell = new PdfPCell(_oPhrase);
                _oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable2.AddCell(_oPdfPCell);


            }
            #endregion

            #region Carrier
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.Carrier))
            {
                _oPhrase = new Phrase();

                #region Port Of Loading
                if (!string.IsNullOrEmpty(_oExportCommercialDoc.Carrier))
                {
                    _oPhrase.Add(new Chunk(_oExportCommercialDoc.Carrier + " : ", _oFontStyle));
                }
                #endregion

                #region CarrierName
                if (!string.IsNullOrEmpty(_oExportCommercialDoc.Carrier))
                {
                    _oPhrase.Add(new Chunk(_oExportCommercialDoc.CarrierName, _oFontStyle));
                }
                #endregion

                _oPdfPCell = new PdfPCell(_oPhrase);
                _oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.BorderWidthTop = 0.5f;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable2.AddCell(_oPdfPCell);

                oPdfPTable2.CompleteRow();
            }
            #endregion

            #region ShippingMark
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.ShippingMark))
            {
                _oPhrase = new Phrase();

                #region ShippingMark
                if (!string.IsNullOrEmpty(_oExportCommercialDoc.ShippingMark))
                {
                    _oPhrase.Add(new Chunk(_oExportCommercialDoc.ShippingMark + " : ", _oFontStyle_UnLine));
                }
                #endregion

                #region CarrierName
                if (!string.IsNullOrEmpty(_oExportCommercialDoc.ShippingMark) && !string.IsNullOrEmpty(_oExportCommercialDoc.ShippingMarkName))
                {
                    _oPhrase.Add(new Chunk(_oExportCommercialDoc.ShippingMarkName, _oFontStyle));
                }
                #endregion

                _oPdfPCell = new PdfPCell(_oPhrase);
                _oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.BorderWidthTop = 0.5f;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable2.AddCell(_oPdfPCell);
                oPdfPTable2.CompleteRow();
            }
            #endregion

            #region SellingOnAbout
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.SellingOnAbout))
            {
                _oPhrase = new Phrase();

                #region SellingOnAbout
                if (!string.IsNullOrEmpty(_oExportCommercialDoc.SellingOnAbout))
                {
                    _oPhrase.Add(new Chunk(_oExportCommercialDoc.SellingOnAbout, _oFontStyle));
                }
                #endregion

                #region CarrierName
                if (!string.IsNullOrEmpty(_oExportCommercialDoc.SellingOnAbout) && !string.IsNullOrEmpty(_oExportCommercialDoc.SellingOnAboutName))
                {
                    _oPhrase.Add(new Chunk(_oExportCommercialDoc.SellingOnAboutName, _oFontStyle));
                }
                #endregion

                _oPdfPCell = new PdfPCell(_oPhrase);
                _oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.BorderWidthTop = 0.5f;
                _oPdfPCell.FixedHeight = 25f;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable2.AddCell(_oPdfPCell);

                oPdfPTable2.CompleteRow();
            }
            #endregion

            #region TruckNo
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.TruckNo_Print))
            {
                _oPhrase = new Phrase();

                #region TruckNo
                if (!string.IsNullOrEmpty(_oExportCommercialDoc.TruckNo))
                {
                    _oPhrase.Add(new Chunk(_oExportCommercialDoc.TruckNo_Print + ": ", _oFontStyle));
                }
                #endregion

                #region CarrierName
                if (!string.IsNullOrEmpty(_oExportCommercialDoc.TruckNo))
                {
                    _oPhrase.Add(new Chunk(_oExportCommercialDoc.TruckNo, _oFontStyle));
                }
                #endregion

                _oPdfPCell = new PdfPCell(_oPhrase);
                _oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.BorderWidthTop = 0.5f;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable2.AddCell(_oPdfPCell);

                oPdfPTable2.CompleteRow();
            }
            #endregion

            return oPdfPTable2;
        }
        private PdfPTable SetCommercialInvoice_Right()
        {

            PdfPTable oPdfPTable2 = new PdfPTable(2);
            oPdfPTable2.SetWidths(new float[] { 15f, 265f });
            #region NoAndDateOfDoc
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.NoAndDateOfDoc))
            {

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.NoAndDateOfDoc, _oFontStyle_UnLine));
                _oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable2.AddCell(_oPdfPCell);
                oPdfPTable2.CompleteRow();

                if (!string.IsNullOrEmpty(_oExportCommercialDoc.ExportBillNo_Full))
                {
                    #region Balnk Space
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyleBold));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    #endregion
                    if (_oExportCommercialDoc.IsPrintInvoiceDate == false && _oExportCommercialDoc.DocDate != "")
                    {
                        _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.ExportBillNo_FullDate + " " + _oExportCommercialDoc.DocDate, _oFontStyleBold));
                    }
                    else
                    {
                        _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.ExportBillNo_FullDate, _oFontStyleBold));
                    }
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.FixedHeight =30;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    oPdfPTable2.CompleteRow();

                }


            }
            #endregion
            #region DocumentaryCreditNoDate (LC No)
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.DocumentaryCreditNoDate))
            {

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.DocumentaryCreditNoDate, _oFontStyle_UnLine));
                _oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable2.AddCell(_oPdfPCell);
                oPdfPTable2.CompleteRow();

                if (!string.IsNullOrEmpty(_oExportCommercialDoc.ExportLCNoAndDate))
                {
                    #region Balnk Space
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyleBold));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    #endregion

                    _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.ExportLCNoAndDate + "" + _oExportCommercialDoc.AmendmentNonDate, _oFontStyleBold));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    oPdfPTable2.CompleteRow();

                }

            }
            #endregion
            #region ProformaInvoiceNoAndDate
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.ProformaInvoiceNoAndDate))
            {

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.ProformaInvoiceNoAndDate, _oFontStyle_UnLine));
                _oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable2.AddCell(_oPdfPCell);
                oPdfPTable2.CompleteRow();

                if (!string.IsNullOrEmpty(_oExportCommercialDoc.PINos))
                {
                    #region Balnk Space
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyleBold));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    #endregion

                    _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.PINos, _oFontStyle));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    oPdfPTable2.CompleteRow();

                }

            }

            #endregion
            #region Delivery To
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.DeliveryTo) && !string.IsNullOrEmpty(_oExportCommercialDoc.DeliveryToName))
            {

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.DeliveryTo, _oFontStyle_UnLine));
                _oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable2.AddCell(_oPdfPCell);
                oPdfPTable2.CompleteRow();

                if (!string.IsNullOrEmpty(_oExportCommercialDoc.DeliveryToName))
                {
                    #region Balnk Space
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyleBold));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    #endregion

                    _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.DeliveryToName, _oFontStyleBold));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    oPdfPTable2.CompleteRow();

                    _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.DeliveryToAddress, _oFontStyle));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    oPdfPTable2.CompleteRow();
                }

            }
            #endregion
            #region NegotiatingBank
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.IssuingBank))
            {
                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.IssuingBank, _oFontStyle_UnLine));
                _oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable2.AddCell(_oPdfPCell);
                oPdfPTable2.CompleteRow();

                if (!string.IsNullOrEmpty(_oExportCommercialDoc.BankName_Issue))
                {
                    #region Balnk Space
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyleBold));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    #endregion

                    _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.BankName_Issue, _oFontStyleBold));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    oPdfPTable2.CompleteRow();
                }
                if (!string.IsNullOrEmpty(_oExportCommercialDoc.BBranchName_Issue))
                {
                    #region Balnk Space
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyleBold));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    #endregion

                    _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.BBranchName_Issue + "\n" + _oExportCommercialDoc.BankAddress_Issue, _oFontStyle));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    oPdfPTable2.CompleteRow();

                }

            }
            #endregion
            #region Master LC/ AgainstExportLC
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.AgainstExportLC))
            {
                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.AgainstExportLC, _oFontStyle_UnLine));
                _oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable2.AddCell(_oPdfPCell);
                oPdfPTable2.CompleteRow();

                if (!string.IsNullOrEmpty(_oExportCommercialDoc.AllMasterLCNo))
                {
                    #region Balnk Space
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    #endregion

                    if (_oExportCommercialDoc.AllMasterLCNo.Length > 250)
                    {
                        _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.AllMasterLCNo, FontFactory.GetFont("Tahoma", 8f, 0)));
                    }
                    else
                    {
                        _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.AllMasterLCNo, FontFactory.GetFont("Tahoma", 8f, 0)));
                    }
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    oPdfPTable2.CompleteRow();
                }


            }
            #endregion
            #region  Other Info
            List<ExportPartyInfoBill> oExportPartyInfoBills = new List<ExportPartyInfoBill>();
            if (_oExportCommercialDoc.ExportBillDocID > 0)
            { oExportPartyInfoBills = ExportPartyInfoBill.GetsByExportLC( _oExportCommercialDoc.ExportBillDocID, (int)EnumMasterLCType.CommercialDoc, 0); }
            else
            {
                oExportPartyInfoBills = ExportPartyInfoBill.GetsByExportLC( _oExportCommercialDoc.ExportLCID, (int)EnumMasterLCType.ExportLC, 0);
            }
            if (!String.IsNullOrEmpty(_oExportCommercialDoc.HSCode_Head) && !String.IsNullOrEmpty(_oExportCommercialDoc.HSCode))
            {
                ExportPartyInfoBill oExportPartyInfoBill = new ExportPartyInfoBill();
                oExportPartyInfoBill.PartyInfo = _oExportCommercialDoc.HSCode_Head;
                oExportPartyInfoBill.RefNo = _oExportCommercialDoc.HSCode;
                oExportPartyInfoBills.Add(oExportPartyInfoBill);
            }

            if (oExportPartyInfoBills.Count > 0)
                {
                    #region Balnk Space
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyleBold));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    #endregion

                    _oPdfPCell = new PdfPCell(this.ExportPartyInfo(false, oExportPartyInfoBills));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    oPdfPTable2.CompleteRow();
                }

            #endregion
                #region TermsofPayment
                if (!string.IsNullOrEmpty(_oExportCommercialDoc.TermsofPayment))
                {
                    _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.TermsofPayment + " " + _oExportCommercialDoc.LCTermsName, _oFontStyle));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.Colspan = 2;
                    _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    oPdfPTable2.CompleteRow();


                }
                #endregion
            return oPdfPTable2;
        }
        private PdfPTable SetDeliveryChallan_Left()
        {
            PdfPTable oPdfPTable2 = new PdfPTable(2);
            oPdfPTable2.SetWidths(new float[] { 15f, 265f });
            #region Beneficiary
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.Beneficiary))
            {

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.Beneficiary, _oFontStyleBold));
                _oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable2.AddCell(_oPdfPCell);
                oPdfPTable2.CompleteRow();

                if (!string.IsNullOrEmpty(_oExportCommercialDoc.BUName))
                {
                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.BUName, _oFontStyleBold));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    oPdfPTable2.CompleteRow();
                }
                if (!string.IsNullOrEmpty(_oExportCommercialDoc.BUAddress))
                {
                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.BUAddress, _oFontStyle));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    oPdfPTable2.CompleteRow();
                }
            }
            #endregion

            return oPdfPTable2;
        }
        private PdfPTable SetDeliveryChallan_Right()
        {
            PdfPTable oPdfPTable2 = new PdfPTable(2);
            oPdfPTable2.SetWidths(new float[] { 15f, 265f });

            #region AccountOf
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.AccountOf))
            {
                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.AccountOf, _oFontStyleBold));
                _oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable2.AddCell(_oPdfPCell);
                oPdfPTable2.CompleteRow();

                if (!string.IsNullOrEmpty(_oExportCommercialDoc.ApplicantName))
                {
                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.ApplicantName, _oFontStyleBold));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    oPdfPTable2.CompleteRow();
                }
                if (!string.IsNullOrEmpty(_oExportCommercialDoc.ApplicantName))
                {
                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.ApplicantAddress_Full, _oFontStyle));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    oPdfPTable2.CompleteRow();
                }
            }
            #endregion
            #region Delivery To
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.DeliveryTo) && !string.IsNullOrEmpty(_oExportCommercialDoc.DeliveryToName))
            {

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.DeliveryTo, _oFontStyleBold));
                _oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable2.AddCell(_oPdfPCell);
                oPdfPTable2.CompleteRow();

                if (!string.IsNullOrEmpty(_oExportCommercialDoc.DeliveryToName))
                {
                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.DeliveryToName, _oFontStyleBold));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    oPdfPTable2.CompleteRow();

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.DeliveryToAddress, _oFontStyle));
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable2.AddCell(_oPdfPCell);
                    oPdfPTable2.CompleteRow();
                }
            }
            #endregion
            return oPdfPTable2;
        }
        private PdfPTable HangerProductWithPrice()
        {
            List<ExportBillDetail> oExportBillDetails = new List<ExportBillDetail>();
            oExportBillDetails = _oExportCommercialDoc.ExportBillDetails;
            PdfPTable oDetailPdfPTable = new PdfPTable(6);
            oDetailPdfPTable.SetWidths(new float[] { 25f, 190f, 130f, 60f, 70f, 60f });

            if (oExportBillDetails != null && oExportBillDetails.Count > 0)
            {
                #region  Accessories For 100% Export Oriented Readymade Garments Industry
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("Accessories For 100% Export Oriented Readymade Garments Industry", _oFontStyle));
                _oPdfPCell.Colspan = 6; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);
                #endregion

                #region Heading
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("#SL", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Description Of Goods", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Style Desc", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Qty (" + oExportBillDetails[0].MUName + ")", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oExportBillDetails[0].RateUnit > 1 ? "Price/" + oExportBillDetails[0].RateUnit + " (" + oExportBillDetails[0].MUName + ")" : "Price/" + oExportBillDetails[0].MUName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Amount(" + _oExportCommercialDoc.Currency + ")", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);
                oDetailPdfPTable.CompleteRow();

                #region Update In Main Table
                _oPdfPCell = new PdfPCell(oDetailPdfPTable);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion
                #endregion


                int nTempCount = 0; double nTotalAmount = 0, nTotalQty = 0;
                string sTempGoodDescription = "";
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                foreach (ExportBillDetail oItem in oExportBillDetails)
                {
                    oDetailPdfPTable = new PdfPTable(6);
                    oDetailPdfPTable.SetWidths(new float[] { 25f, 190f, 130f, 60f, 70f, 60f });

                    _oPdfPCell = new PdfPCell(new Phrase((++nTempCount).ToString(), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);
                    //make dynamic good description
                    if (oItem.IsApplySizer)
                    {
                        sTempGoodDescription = oItem.ProductName;
                    }
                    else
                    {
                        if (oItem.ProductDescription != null)
                        {
                            sTempGoodDescription = oItem.ProductDescription + "\n";
                        }
                        sTempGoodDescription += "HANGER REF. # " + oItem.ProductName;
                        if (oItem.SizeName != "") { sTempGoodDescription += ", SIZE: " + oItem.SizeName; }
                        if (oItem.ColorID != 0) { sTempGoodDescription += "\nCOLOR: " + oItem.ColorName; }
                        if (oItem.ModelReferenceID != 0) { sTempGoodDescription += ", MODEL: " + oItem.ModelReferenceName; }
                    }
                    _oPdfPCell = new PdfPCell(new Phrase(sTempGoodDescription, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.StyleNo, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(oItem.Qty), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

                    //_oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.UnitPrice), _oFontStyle));
                    _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.Currency + "" + System.Math.Round(oItem.UnitPrice, 4).ToString("#,##0.00##;(#,##0.00##)"), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.Currency + " " + Global.MillionFormat(oItem.Amount), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

                    nTotalQty += oItem.Qty;
                    nTotalAmount += oItem.Amount;
                    oDetailPdfPTable.CompleteRow();

                    #region Update In Main Table
                    _oPdfPCell = new PdfPCell(oDetailPdfPTable);
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion
                }
                
                #region Total
                oDetailPdfPTable = new PdfPTable(6);
                oDetailPdfPTable.SetWidths(new float[] { 25f, 190f, 130f, 60f, 70f, 60f });

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyle));
                _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(nTotalQty), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.Currency + " " + Global.MillionFormat(nTotalAmount), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);
                oDetailPdfPTable.CompleteRow();

                #region Update In Main Table
                _oPdfPCell = new PdfPCell(oDetailPdfPTable);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion
                #endregion

                #region amount in word
                oDetailPdfPTable = new PdfPTable(6);
                oDetailPdfPTable.SetWidths(new float[] { 25f, 190f, 130f, 60f, 70f, 60f });

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                string sAmountInWord = "Amount In Word : ";
                if (_oExportCommercialDoc.Currency == "$")
                {
                    sAmountInWord += "US " + Global.DollarWords(nTotalAmount);
                }
                else if (_oExportCommercialDoc.Currency == "TK")
                {
                    sAmountInWord += Global.TakaWords(nTotalAmount);
                }
                else if (_oExportCommercialDoc.Currency == "GBP")
                {
                    sAmountInWord += Global.PoundWords(nTotalAmount);
                }
                _oPdfPCell = new PdfPCell(new Phrase(sAmountInWord, _oFontStyle));
                _oPdfPCell.Colspan = 6; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);
                oDetailPdfPTable.CompleteRow();

                #region Update In Main Table
                _oPdfPCell = new PdfPCell(oDetailPdfPTable);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion
                #endregion
            }

            return oDetailPdfPTable;
        }
        private PdfPTable HangerProductWithOutPrice()
        {
            List<ExportBillDetail> oExportBillDetails = new List<ExportBillDetail>();
            oExportBillDetails = _oExportCommercialDoc.ExportBillDetails;
            PdfPTable oDetailPdfPTable = new PdfPTable(4);
            oDetailPdfPTable.SetWidths(new float[] { 70f, 70f, 300f, 95f});

            if (oExportBillDetails != null && oExportBillDetails.Count > 0)
            {
                #region Heading
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("NO(S) OF PACKAGES", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("QUANTITY", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("DESCRIPTION", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("NET & GROSS WEIGHT", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);
                oDetailPdfPTable.CompleteRow();
                #endregion

                #region Total Packages/ Gross Weight/ Net Weight
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.NoOfPackages, _oFontStyle));
                _oPdfPCell.Rowspan = oExportBillDetails.Count + 1; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

                #region Category Wise Total
                List<ExportBillDetail> oTempExportBillDetails = new List<ExportBillDetail>();
                IEnumerable<int>  aProductCategoryIDs = oExportBillDetails.Select(x => x.ProductCategoryID).Distinct();
                foreach (int nProductCategoryID in aProductCategoryIDs)
                {
                    ExportBillDetail oExportBillDetail = new ExportBillDetail();
                    oExportBillDetail.ProductCategoryID = nProductCategoryID;
                    oExportBillDetail.Qty = oExportBillDetails.Where(d => d.ProductCategoryID == nProductCategoryID).Sum(d => d.Qty);
                    oExportBillDetail.ProductCategoryName = oExportBillDetails.Where(d => d.ProductCategoryID == nProductCategoryID).ToArray()[0].ProductCategoryName;
                    oExportBillDetail.MUName = oExportBillDetails.Where(d => d.ProductCategoryID == nProductCategoryID).ToArray()[0].MUName;
                    oTempExportBillDetails.Add(oExportBillDetail);
                }

                string sProductCatagoryName = "";
                _oPhrase = new Phrase();                
                foreach (ExportBillDetail oItem in oTempExportBillDetails)
                {
                    sProductCatagoryName = oItem.ProductCategoryName.Split('(')[0];
                    _oPhrase.Add(new Chunk(sProductCatagoryName + " = " + Global.MillionFormatActualDigit(oItem.Qty) + " " + oItem.MUName + "\n\n\n", _oFontStyle));
                }
                _oPdfPCell = new PdfPCell(_oPhrase);
                _oPdfPCell.Rowspan = oExportBillDetails.Count + 1; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);
                #endregion

                

                _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.UNDERLINE);
                _oPdfPCell = new PdfPCell(new Phrase("Accessories For 100% Export Oriented Readymade Garments Industry:", _oFontStyle_UnLine));
                _oPdfPCell.FixedHeight = 20f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;  _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

                _oPhrase = new Phrase();
                _oPhrase.Add(new Chunk("NET WEIGHT", _oFontStyleBold));
                _oPhrase.Add(new Chunk("\n", _oFontStyle));
                _oPhrase.Add(new Chunk(_oExportCommercialDoc.PlasticNetWeight, _oFontStyle));
                _oPhrase.Add(new Chunk("\n\n&\n\n", _oFontStyleBold));
                _oPhrase.Add(new Chunk("GROSS WEIGHT", _oFontStyleBold));
                _oPhrase.Add(new Chunk("\n", _oFontStyle));
                _oPhrase.Add(new Chunk(_oExportCommercialDoc.PlasticGrossWeight, _oFontStyle));
                _oPdfPCell = new PdfPCell(_oPhrase);
                _oPdfPCell.Rowspan = oExportBillDetails.Count + 1; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);
                oDetailPdfPTable.CompleteRow();
                #endregion


                int nTempCount = 0; double nTotalQty = 0;
                string sTempGoodDescription = "";
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                foreach (ExportBillDetail oItem in oExportBillDetails)
                {   
                    //make dynamic good description
                    _oPhrase = new Phrase();
                    if (oItem.IsApplySizer)
                    {
                        sTempGoodDescription = oItem.ProductName;
                        _oPhrase.Add(new Chunk(sTempGoodDescription, _oFontStyleBold));
                        sTempGoodDescription = "";
                    }
                    else
                    {
                        if (oItem.ProductDescription != null)
                        {
                            sTempGoodDescription = oItem.ProductDescription + "\n";
                            _oPhrase.Add(new Chunk(sTempGoodDescription, _oFontStyleBold));
                        }

                        sTempGoodDescription = "Hanger Ref No : " + oItem.ProductName;
                        if (oItem.SizeName != "") { sTempGoodDescription += ", Size : " + oItem.SizeName; }
                        if (oItem.ColorID != 0) { sTempGoodDescription += " Color : " + oItem.ColorName; }                        
                    }
                    sTempGoodDescription += "\nQuantity      : " + Global.MillionFormatActualDigit(oItem.Qty) + "  " + oItem.MUName;
                    _oPhrase.Add(new Chunk(sTempGoodDescription, _oFontStyle));

                    _oPdfParagraph = new Paragraph(_oPhrase);
                    _oPdfParagraph.SetLeading(0f, 1.2f);
                    _oPdfParagraph.Alignment = Element.ALIGN_LEFT;
                    _oPdfPCell = new PdfPCell();
                    _oPdfPCell.AddElement(_oPdfParagraph);
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);
                    oDetailPdfPTable.CompleteRow();
                }
            }
            return oDetailPdfPTable;
        }
        private PdfPTable PloyProductWithPrice()
        {
            List<ExportBillDetail> oExportBillDetails = new List<ExportBillDetail>();
            oExportBillDetails = _oExportCommercialDoc.ExportBillDetails;

            PdfPTable oDetailPdfPTable = new PdfPTable(8);
            oDetailPdfPTable.SetWidths(new float[] {
                                                    25f,//SL: 
                                                    110f, //Item Name
                                                    125f,//measeurement
                                                    60f, //Item description
                                                    45f, //Color Qty
                                                    50f,//qty
                                                    60f,//Price
                                                    60f});//amount

            if (oExportBillDetails != null && oExportBillDetails.Count > 0)
            {
                #region  Accessories For 100% Export Oriented Readymade Garments Industry
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("Accessories For 100% Export Oriented Readymade Garments Industry", _oFontStyle));
                _oPdfPCell.Colspan = 8; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);
                #endregion

                #region Heading
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("#SL", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Item Name", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Measurement", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Style Desc", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Color Qty", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Qty (" + oExportBillDetails[0].MUName + ")", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oExportBillDetails[0].RateUnit > 1 ? "Price/" + oExportBillDetails[0].RateUnit + " (" + oExportBillDetails[0].MUName + ")" : "Price/" + oExportBillDetails[0].MUName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Amount(" + _oExportCommercialDoc.Currency + ")", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

                oDetailPdfPTable.CompleteRow();
                #endregion

                int nTempCount = 0; double nTotalAmount = 0, nTotalQty = 0;

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                foreach (ExportBillDetail oItem in oExportBillDetails)
                {
                    _oPdfPCell = new PdfPCell(new Phrase((++nTempCount).ToString(), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.Measurement, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.StyleNo, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ColorQty.ToString(), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(oItem.Qty), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

                    //_oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.UnitPrice), _oFontStyle));
                    _oPdfPCell = new PdfPCell(new Phrase(System.Math.Round(oItem.UnitPrice, 4).ToString("#,##0.00##;(#,##0.00##)"), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.Currency + " " + Global.MillionFormat(oItem.Amount), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

                    nTotalQty += oItem.Qty;
                    nTotalAmount += oItem.Amount;
                    oDetailPdfPTable.CompleteRow();
                }
                #region Total
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyle));
                _oPdfPCell.Colspan = 5; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(nTotalQty), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.Currency + " " + Global.MillionFormat(nTotalAmount), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);
                oDetailPdfPTable.CompleteRow();
                #endregion
                #region amount in word
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                string sAmountInWord = "Amount In Word : ";
                if (_oExportCommercialDoc.Currency == "$")
                {
                    sAmountInWord += "US " + Global.DollarWords(nTotalAmount);
                }
                else if (_oExportCommercialDoc.Currency == "TK")
                {
                    sAmountInWord += Global.TakaWords(nTotalAmount);
                }
                else if (_oExportCommercialDoc.Currency == "GBP")
                {
                    sAmountInWord += Global.PoundWords(nTotalAmount);
                }
                _oPdfPCell = new PdfPCell(new Phrase(sAmountInWord, _oFontStyle));
                _oPdfPCell.Colspan = 8; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);
                oDetailPdfPTable.CompleteRow();
                #endregion

            }
            return oDetailPdfPTable;
        }
        private PdfPTable SetBOE_To()
        {

            PdfPTable oPdfPTable2 = new PdfPTable(2);
            oPdfPTable2.SetWidths(new float[] { 22f, 258f });
            #region NoAndDateOfDoc
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.To))
            {
                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.To, _oFontStyle));
                _oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable2.AddCell(_oPdfPCell);
                oPdfPTable2.CompleteRow();

                if (string.IsNullOrEmpty(_oExportCommercialDoc.BankName_Forwarding))
                {
                    _oExportCommercialDoc.BankName_Forwarding = _oExportCommercialDoc.BankName_Issue;
                    _oExportCommercialDoc.BBranchName_Forwarding = _oExportCommercialDoc.BBranchName_Issue;
                    _oExportCommercialDoc.BankAddress_Forwarding = _oExportCommercialDoc.BankAddress_Forwarding;
                }

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle_UnLine));
                _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable2.AddCell(_oPdfPCell);

                _oPhrase = new Phrase();
                _oPhrase.Add(new Chunk(_oExportCommercialDoc.BankName_Forwarding, _oFontStyleBold));
                if (!String.IsNullOrEmpty(_oExportCommercialDoc.BBranchName_Forwarding))
                {
                    _oPhrase.Add(new Chunk("\n" + _oExportCommercialDoc.BBranchName_Forwarding, _oFontStyle));
                }
                if (!String.IsNullOrEmpty(_oExportCommercialDoc.BankAddress_Forwarding))
                {
                    _oPhrase.Add(new Chunk("\n" + _oExportCommercialDoc.BankAddress_Forwarding, _oFontStyle));
                }
             
                Paragraph oPdfParagraph;
                oPdfParagraph = new Paragraph(_oPhrase);
                oPdfParagraph.SetLeading(0f, 1f);
                oPdfParagraph.SpacingBefore=0.2f;
                oPdfParagraph.Alignment = Element.ALIGN_LEFT;
              
                _oPdfPCell.AddElement(oPdfParagraph);
                _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable2.AddCell(_oPdfPCell);
                oPdfPTable2.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.FixedHeight = 15;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable2.AddCell(_oPdfPCell);
                oPdfPTable2.CompleteRow();

            }
          
            #endregion
            return oPdfPTable2;
        }
        private PdfPTable SetBOE_ACC()
        {

            PdfPTable oPdfPTable2 = new PdfPTable(2);
            oPdfPTable2.SetWidths(new float[] { 25f, 255f });
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 2;
            _oPdfPCell.FixedHeight = 5;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable2.AddCell(_oPdfPCell);
            oPdfPTable2.CompleteRow();
            #region AccountOf

            if (!string.IsNullOrEmpty(_oExportCommercialDoc.AccountOf))
            {
                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.AccountOf, _oFontStyle));
                _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable2.AddCell(_oPdfPCell);

                _oPhrase = new Phrase();

                _oPhrase.Add(new Chunk(_oExportCommercialDoc.ApplicantName, _oFontStyleBold));
                if (!String.IsNullOrEmpty("\n" + _oExportCommercialDoc.ApplicantAddress))
                {
                    _oPhrase.Add(new Chunk("\n" + _oExportCommercialDoc.ApplicantAddress, _oFontStyle));
                }
                _oPdfPCell = new PdfPCell(_oPhrase);
                _oPdfPCell.Border = 0;
                //_oPdfPCell.Colspan = 2;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable2.AddCell(_oPdfPCell);
                oPdfPTable2.CompleteRow();

            }
            #endregion
            return oPdfPTable2;
        }
        private PdfPTable SetBOE_For()
        {
            PdfPTable oPdfPTable2 = new PdfPTable(2);
            oPdfPTable2.SetWidths(new float[] { 15f, 265f });
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 2;
            _oPdfPCell.FixedHeight = 5;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable2.AddCell(_oPdfPCell);
            oPdfPTable2.CompleteRow();
            #region For
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.For))
            {
                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.For, _oFontStyle));
                _oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable2.AddCell(_oPdfPCell);
                oPdfPTable2.CompleteRow();
            }
            #endregion
            #region AuthorisedSignature
            if (!string.IsNullOrEmpty(_oExportCommercialDoc.AuthorisedSignature))
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.FixedHeight = 42;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable2.AddCell(_oPdfPCell);
                oPdfPTable2.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase(_oExportCommercialDoc.AuthorisedSignature, _oFontStyle));
                _oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable2.AddCell(_oPdfPCell);
                oPdfPTable2.CompleteRow();
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.FixedHeight = 40;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable2.AddCell(_oPdfPCell);
                oPdfPTable2.CompleteRow();
            }
            #endregion
            return oPdfPTable2;
        }
        public static float CalculatePdfPTableHeight(PdfPTable table)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (Document doc = new Document(PageSize.TABLOID))
                {
                    using (PdfWriter w = PdfWriter.GetInstance(doc, ms))
                    {
                        doc.Open();
                        table.TotalWidth = 500f;
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