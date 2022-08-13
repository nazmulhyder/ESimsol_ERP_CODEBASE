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
    public class rptFabricTransferNote
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(10);
        int _nTotalColumn = 10;
        PdfPCell _oPdfPCell;
        MemoryStream _oMemoryStream = new MemoryStream();
        FabricTransferNote _oFTN = new FabricTransferNote();
        Company _oCompany = new Company();
        string _sMessage = "";
        bool _bIsInYard = true;
        BaseColor _oBC = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#EFF0EA"));
        #endregion

        public byte[] PrepareReport(FabricTransferNote oFTN, Company oCompany, string sMessage, bool bIsInYard)
        {
            _oFTN = oFTN;
            _oCompany = oCompany;
            _sMessage = sMessage;
            _bIsInYard = bIsInYard;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);//595 x 842
            //For Header 100
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            _oDocument.SetMargins(10f, 10f, 10f, 10f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            //ESimSolFooter PageEventHandler = new ESimSolFooter();
            //PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            PDFFooter1 PDFFooter = new PDFFooter1();
            PDFFooter.signatures = new List<string>(new string[] { "Received By", "Checked By", "Prepared By", "Store Dept.", "Authorised Signature" });
            PDFWriter.PageEvent = PDFFooter; 
            _oDocument.Open();

            _oPdfPTable.SetWidths(new float[] { 40f, 125f, 120f, 100f, 90f,100f,100f,90f, 100f, 140f });
            #endregion

            this.PrintHeader();
            this.PrintBody();
            _oPdfPTable.HeaderRows = 7;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Header
        private void PrintHeader()
        {
            #region CompanyHeader

            _oFontStyle = FontFactory.GetFont("Tahoma", 16f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.FixedHeight = 20f;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("(Weaving Unit)", _oFontStyle));
            _oPdfPCell.FixedHeight = 13f;
            _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.ExtraParagraphSpace = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.FactoryAddress, _oFontStyle));
            _oPdfPCell.FixedHeight = 13f;
            _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.ExtraParagraphSpace = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

            #region ReportHeader
            _oFontStyle = FontFactory.GetFont("Tahoma", 11f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Transfer Challan", _oFontStyle));
            _oPdfPCell.FixedHeight = 14f;
            _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.ExtraParagraphSpace = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        #endregion

        #region Report Body
        private void PrintBody()
        {
            float nCellHeight =20f;
            double nTableHeight = 0;
            double nDetailHeight = 0;
            if (_oFTN.FTNID > 0)
            {
                _oPdfPCell = new PdfPCell(this.GetFTNInfo());
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = _nTotalColumn;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                //nTableHeight += CalculatePdfPTableHeight(_oPdfPTable);
                #region Fabric Transfer Packing List print
                if (_oFTN.FTPLs.Count > 0)
                {
                    this.Blank();

                    #region Table Header
                    _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);

                    _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.BackgroundColor = _oBC;
                    _oPdfPCell.FixedHeight = 12f;
                    _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("FEO No", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.BackgroundColor = _oBC;
                    _oPdfPCell.FixedHeight = 12f;
                    _oPdfPTable.AddCell(_oPdfPCell);


                    _oPdfPCell = new PdfPCell(new Phrase("Construction", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.BackgroundColor = _oBC;
                    _oPdfPCell.FixedHeight = 12f;
                    _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Width", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.BackgroundColor = _oBC;
                    _oPdfPCell.FixedHeight = 12f;
                    _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Wrap Lot", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.BackgroundColor = _oBC;
                    _oPdfPCell.FixedHeight = 12f;
                    _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Weft Lot", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.BackgroundColor = _oBC;
                    _oPdfPCell.FixedHeight = 12f;
                    _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Color", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.BackgroundColor = _oBC;
                    _oPdfPCell.FixedHeight = 12f;
                    _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("No of Roll", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.BackgroundColor = _oBC;
                    _oPdfPCell.FixedHeight = 12f;
                    _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Qty " + (_bIsInYard ? "(Y)" : "(M)"), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.BackgroundColor = _oBC;
                    _oPdfPCell.FixedHeight = 12f;
                    _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Remark", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.BackgroundColor = _oBC;
                    _oPdfPCell.FixedHeight = 14f;
                    _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion

                    #region Detail

                    
                    int nSL = 0;
                    int nTotalRoll = 0;
                    double nTotalQty = 0;
                    float _nfixedHight = 0;

                    _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
                    //double nPageBodyHeight = 500;
                    foreach (FabricTransferPackingList oItem in _oFTN.FTPLs)
                    {
                     
                        //double height = CalculatePdfPTableHeight(_oPdfPTable);
                        //nDetailHeight = CalculatePdfPTableHeight(_oPdfPTable) - 100 - 100;
                        //if (height> 500)
                        //    nPageBodyHeight=nPageBodyHeight* (height/600);

                        //if (height > nPageBodyHeight)
                        //{
                        //    this.printTotal(nTotalRoll, nTotalQty);
                        //    this.PrintFooter();
                          
                        //}
                      
                        nSL++;
                        _oPdfPCell = new PdfPCell(new Phrase(nSL.ToString(), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPCell.BorderWidthBottom = 0;
                        _oPdfPCell.BorderWidthTop = 0;
                        _oPdfPCell.MinimumHeight = nCellHeight;
                        _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.OrderNo + ((oItem.FabType == "Yarn Dyed")?" (Y/D)":" (S/D)"), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPCell.BorderWidthBottom = 0;
                        _oPdfPCell.BorderWidthTop = 0;
                        _oPdfPCell.MinimumHeight = nCellHeight;
                        _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.Construction, _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPCell.BorderWidthBottom = 0;
                        _oPdfPCell.BorderWidthTop = 0;
                        _oPdfPCell.MinimumHeight = nCellHeight;
                        _oPdfPTable.AddCell(_oPdfPCell);

                        int decPart = Convert.ToInt32(oItem.GreyWidth.Split('.')[1]);

                        _oPdfPCell = new PdfPCell(new Phrase((decPart == 0) ? oItem.GreyWidth.Split('.')[0] : oItem.GreyWidth, _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPCell.BorderWidthBottom = 0;
                        _oPdfPCell.BorderWidthTop = 0;
                        _oPdfPCell.MinimumHeight = nCellHeight;
                        _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.WarpLot, _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPCell.BorderWidthBottom = 0;
                        _oPdfPCell.BorderWidthTop = 0;
                        _oPdfPCell.MinimumHeight = nCellHeight;
                        _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.WeftLot, _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPCell.BorderWidthBottom = 0;
                        _oPdfPCell.BorderWidthTop = 0;
                        _oPdfPCell.MinimumHeight = nCellHeight;
                        _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.Color, _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPCell.BorderWidthBottom = 0;
                        _oPdfPCell.BorderWidthTop = 0;
                        _oPdfPCell.MinimumHeight = nCellHeight;
                        _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.CountRoll.ToString(), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPCell.BorderWidthBottom = 0;
                        _oPdfPCell.BorderWidthTop = 0;
                        _oPdfPCell.MinimumHeight = nCellHeight;
                        _oPdfPTable.AddCell(_oPdfPCell);

                        oItem.TotalRollQty = (_bIsInYard ? oItem.TotalRollQty : Global.GetMeter(oItem.TotalRollQty, 2));
                        _oPdfPCell = new PdfPCell(new Phrase(Math.Round(oItem.TotalRollQty,0).ToString(), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPCell.BorderWidthBottom = 0;
                        _oPdfPCell.BorderWidthTop = 0;
                        _oPdfPCell.MinimumHeight = nCellHeight;
                        _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.Note.Replace("/n",""), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPCell.BorderWidthBottom = 0;
                        _oPdfPCell.BorderWidthTop = 0;
                        _oPdfPCell.MinimumHeight = nCellHeight;
                        _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();

                        nTotalRoll += oItem.CountRoll;
                        nTotalQty += oItem.TotalRollQty;
                      
                    }
                    #endregion

                    this.printTotal(nTotalRoll, nTotalQty);
                    this.PrintFooter();


                    //#region Extra Table Space
                    //double pageHeight = 500;
                    //nTableHeight += CalculatePdfPTableHeight(_oPdfPTable);
                    ////if (nTableHeight % 700 < pageHeight)
                    ////{
                    //double val = 800 * Math.Ceiling((nTableHeight / 800));
                    //if (nTableHeight>800)
                    //    _nfixedHight =(float)( 800 *Math.Ceiling( (nTableHeight / 800)) - nTableHeight-50); //(float)nTableHeight % 500;
                    //else
                    //    _nfixedHight = (float)(800 - nTableHeight);
                    //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //    _oPdfPCell.BorderWidthBottom = 0;
                    //    _oPdfPCell.BorderWidthTop = 0;
                    //    _oPdfPCell.FixedHeight = _nfixedHight;
                    //    _oPdfPTable.AddCell(_oPdfPCell);

                    //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //    _oPdfPCell.BorderWidthBottom = 0;
                    //    _oPdfPCell.BorderWidthTop = 0;
                    //    _oPdfPCell.FixedHeight = _nfixedHight;
                    //    _oPdfPTable.AddCell(_oPdfPCell);

                    //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //    _oPdfPCell.BorderWidthBottom = 0;
                    //    _oPdfPCell.BorderWidthTop = 0;
                    //    _oPdfPCell.FixedHeight = _nfixedHight;
                    //    _oPdfPTable.AddCell(_oPdfPCell);

                    //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //    _oPdfPCell.BorderWidthBottom = 0;
                    //    _oPdfPCell.BorderWidthTop = 0;
                    //    _oPdfPCell.FixedHeight = _nfixedHight;
                    //    _oPdfPTable.AddCell(_oPdfPCell);

                    //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //    _oPdfPCell.BorderWidthBottom = 0;
                    //    _oPdfPCell.BorderWidthTop = 0;
                    //    _oPdfPCell.FixedHeight = _nfixedHight;
                    //    _oPdfPTable.AddCell(_oPdfPCell);

                    //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //    _oPdfPCell.BorderWidthBottom = 0;
                    //    _oPdfPCell.BorderWidthTop = 0;
                    //    _oPdfPCell.FixedHeight = _nfixedHight;
                    //    _oPdfPTable.AddCell(_oPdfPCell);

                    //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //    _oPdfPCell.BorderWidthBottom = 0;
                    //    _oPdfPCell.BorderWidthTop = 0;
                    //    _oPdfPCell.FixedHeight = _nfixedHight;
                    //    _oPdfPTable.AddCell(_oPdfPCell);


                    //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //    _oPdfPCell.BorderWidthBottom = 0;
                    //    _oPdfPCell.BorderWidthTop = 0;
                    //    _oPdfPCell.FixedHeight = _nfixedHight;
                    //    _oPdfPTable.AddCell(_oPdfPCell);


                    //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //    _oPdfPCell.BorderWidthBottom = 0;
                    //    _oPdfPCell.BorderWidthTop = 0;
                    //    _oPdfPCell.FixedHeight = _nfixedHight;
                    //    _oPdfPTable.AddCell(_oPdfPCell);


                    //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //    _oPdfPCell.BorderWidthBottom = 0;
                    //    _oPdfPCell.BorderWidthTop = 0;
                    //    _oPdfPCell.FixedHeight = _nfixedHight;
                    //    _oPdfPTable.AddCell(_oPdfPCell);
                    //    _oPdfPTable.CompleteRow();
                    //    #region Total
                    //    _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);

                    //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //    _oPdfPCell.FixedHeight = nCellHeight;
                    //    _oPdfPTable.AddCell(_oPdfPCell);

                    //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //    _oPdfPCell.FixedHeight = nCellHeight;
                    //    _oPdfPTable.AddCell(_oPdfPCell);

                    //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //    _oPdfPCell.FixedHeight = nCellHeight;
                    //    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //    _oPdfPTable.AddCell(_oPdfPCell);

                    //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //    _oPdfPCell.FixedHeight = nCellHeight;
                    //    _oPdfPTable.AddCell(_oPdfPCell);

                    //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //    _oPdfPCell.FixedHeight = nCellHeight;
                    //    _oPdfPTable.AddCell(_oPdfPCell);

                    //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //    _oPdfPCell.FixedHeight = nCellHeight;
                    //    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //    _oPdfPTable.AddCell(_oPdfPCell);



                    //    _oPdfPCell = new PdfPCell(new Phrase("Total : ", _oFontStyle));
                    //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //    _oPdfPCell.FixedHeight = nCellHeight;
                    //    _oPdfPTable.AddCell(_oPdfPCell);

                    //    _oPdfPCell = new PdfPCell(new Phrase(nTotalRoll.ToString(), _oFontStyle));
                    //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //    _oPdfPCell.FixedHeight = nCellHeight;
                    //    _oPdfPTable.AddCell(_oPdfPCell);

                    //    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalQty) + " " + (_bIsInYard ? "(Y)" : "(M)"), _oFontStyle));
                    //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //    _oPdfPCell.FixedHeight = nCellHeight;
                    //    _oPdfPTable.AddCell(_oPdfPCell);

                    //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //    _oPdfPCell.FixedHeight = nCellHeight;
                    //    _oPdfPTable.AddCell(_oPdfPCell);
                    //    _oPdfPTable.CompleteRow();
                    //    #endregion
                    //    this.PrintFooter();
                    ////}
                  
                   
                    //#endregion


                }
                #endregion

                else
                {
                    _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.Colspan = _nTotalColumn;
                    _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();

                    _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.Colspan = _nTotalColumn;
                    _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();

                    _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.Colspan = _nTotalColumn;
                    _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();

                    _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.Colspan = _nTotalColumn;
                    _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                }

                
            }
            else
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = _nTotalColumn;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = _nTotalColumn;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = _nTotalColumn;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = _nTotalColumn;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 15f, 1);
                _oPdfPCell = new PdfPCell(new Phrase("Invalid Fabric Transfer Note", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = _nTotalColumn;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
        }
        #endregion
        private PdfPTable SignaturePart()
        {
            PdfPTable oPdfPTable = new PdfPTable(5);
            oPdfPTable.SetWidths(new float[] { 100f, 100f, 100f, 100f, 100f });
            float nHeight = 12f;
            #region Row 1

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.FixedHeight = nHeight;
            _oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 5;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("______________", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.FixedHeight = nHeight;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("______________", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.FixedHeight = nHeight;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("______________", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.FixedHeight = nHeight;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("______________", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.FixedHeight = nHeight;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("_________________", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.FixedHeight = nHeight;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Row 2
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("Received by", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.FixedHeight = 12f;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Checked by", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.FixedHeight = 12f;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Prepared by", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.FixedHeight = 12f;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Store Dept.", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.FixedHeight = 12f;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Authorised Signature", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.FixedHeight = 12f;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            return oPdfPTable;
        }
        private PdfPTable DriverNameAndTruckNo()
        {
            PdfPTable oPdfPTable = new PdfPTable(1);
            oPdfPTable.SetWidths(new float[] { 100f });

            #region Row 1
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("Driver's Name ............................", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.FixedHeight = 12f;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Row 2
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("Truck Name ...............................", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.FixedHeight = 12f;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);
        
            oPdfPTable.CompleteRow();
            #endregion

            return oPdfPTable;
        }
        private PdfPTable GetFTNInfo()
        {

            PdfPTable oPdfPTable = new PdfPTable(5);
            oPdfPTable.SetWidths(new float[] { 60f, 100f, 100f, 100f, 60f });

            #region Row 1
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Date : ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.FixedHeight = 12f;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oFTN.NoteDateSt, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.FixedHeight = 12f;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.FixedHeight = 12f;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Transfer Challan No : ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.FixedHeight = 12f;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oFTN.FTNNo, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.FixedHeight = 12f;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Row 2
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Address : ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.FixedHeight = 12f;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("Finishing Unit", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.FixedHeight = 12f;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.FixedHeight = 12f;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Gate Pass No : ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.FixedHeight = 12f;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oFTN.FTNNo, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.FixedHeight = 12f;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Row 3 (Remark)
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Remark : ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.FixedHeight = 12f;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oFTN.Note, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.FixedHeight = 12f;
            _oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 4;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            return oPdfPTable;
        }
        private void Blank()
        {
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 12f;
            _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
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
                        table.TotalWidth = 535f;
                        table.WriteSelectedRows(0, table.Rows.Count, 0, 0, w.DirectContent);

                        doc.Close();
                        return table.TotalHeight;
                    }
                }
            }
        }

        public void PrintFooter()
        {
            float nCellHeight = 20f;
            #region Received the fabrics in good condition
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("Received the fabrics in good condition.", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = nCellHeight;
            _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            //#region Driver name & truck no
            //_oPdfPCell = new PdfPCell(this.DriverNameAndTruckNo());
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.Border = 0;
            //_oPdfPCell.Colspan = _nTotalColumn;
            //_oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();
            //#endregion

            //#region Signature
            //_oPdfPCell = new PdfPCell(this.SignaturePart());
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.Border = 0;
            //_oPdfPCell.Colspan = _nTotalColumn;
            //_oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();
            //#endregion
        }

        public void printTotal(double nRollQty, double nTotalQty)
        {
            float nCellHeight = 20f;
            #region Total
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.FixedHeight = nCellHeight;
            _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.FixedHeight = nCellHeight;
            _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.FixedHeight = nCellHeight;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.FixedHeight = nCellHeight;
            _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.FixedHeight = nCellHeight;
            _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.FixedHeight = nCellHeight;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);



            _oPdfPCell = new PdfPCell(new Phrase("Total : ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.FixedHeight = nCellHeight;
            _oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase(nTotalRoll.ToString(), _oFontStyle));
            _oPdfPCell = new PdfPCell(new Phrase(nRollQty.ToString(), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.FixedHeight = nCellHeight;
            _oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalQty) + " " + (_bIsInYard ? "(Y)" : "(M)"), _oFontStyle));
            _oPdfPCell = new PdfPCell(new Phrase(Math.Round(nTotalQty,0).ToString(), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.FixedHeight = nCellHeight;
            _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.FixedHeight = nCellHeight;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
    }

    public class PDFFooter : PdfPageEventHelper
    {
        // write on top of document

        BaseFont bf = null;
        PdfContentByte cb;
        DateTime PrintTime = DateTime.Now;
        PdfTemplate template;
        PdfPCell _oPdfPCell;
        iTextSharp.text.Font _oFontStyle;
        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            base.OnOpenDocument(writer, document);
            PdfPTable tabFot = new PdfPTable(new float[] { 1F });
            tabFot.SpacingAfter = 10F;
            PdfPCell cell;
            tabFot.TotalWidth = 300F;
            cell = new PdfPCell(new Phrase(""));
            cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            tabFot.AddCell(cell);
            //tabFot.WriteSelectedRows(0, -1, 150, document.Top, writer.DirectContent);
            tabFot.WriteSelectedRows(0, -1, document.LeftMargin, document.RightMargin, writer.DirectContent);

        }

        // write on start of each page
        public override void OnStartPage(PdfWriter writer, Document document)
        {
            base.OnStartPage(writer, document);
        }

        // write on end of each page
        public override void OnEndPage(PdfWriter writer, Document document)
        {
            DateTime horario = DateTime.Now;
            

            base.OnEndPage(writer, document);
            PdfPTable tabFot = new PdfPTable(new float[] { 1f });
            PdfPCell cell;
            tabFot.TotalWidth = 500f;

            //cell = new PdfPCell(PrintFooter(ref oPdfPTable));
            cell = new PdfPCell(this.PrintFooter());
            cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //cell.HorizontalAlignment = Element.ALIGN_CENTER;
            tabFot.AddCell(cell);
            tabFot.WriteSelectedRows(0, -1, document.LeftMargin, document.RightMargin, writer.DirectContent);

           
            
           
        }

        //write on close of document
        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);
        }

        private PdfPTable PrintFooter()
        {
            PdfPTable _oPdfPTable = new PdfPTable(5);
            _oPdfPTable.SetWidths(new float[] { 100f, 100f, 100f, 100f, 100f });
            float nCellHeight = 20f;
            #region Received the fabrics in good condition
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("Received the fabrics in good condition.", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = nCellHeight;
            _oPdfPCell.Colspan = 5;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Driver name & truck no
            _oPdfPCell = new PdfPCell(this.DriverNameAndTruckNo());
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 5;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Signature
            _oPdfPCell = new PdfPCell(this.SignaturePart());
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 5;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            return _oPdfPTable;
        }

        private PdfPTable SignaturePart()
        {
            PdfPTable oPdfPTable = new PdfPTable(5);
            oPdfPTable.SetWidths(new float[] { 100f, 100f, 100f, 100f, 100f });
            float nHeight = 12f;
            #region Row 1

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.FixedHeight = nHeight;
            _oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 5;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("______________", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.FixedHeight = nHeight;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("______________", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.FixedHeight = nHeight;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("______________", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.FixedHeight = nHeight;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("______________", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.FixedHeight = nHeight;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("_________________", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.FixedHeight = nHeight;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Row 2
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("Received by", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.FixedHeight = 12f;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Checked by", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.FixedHeight = 12f;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Prepared by", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.FixedHeight = 12f;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Store Dept.", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.FixedHeight = 12f;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Authorised Signature", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.FixedHeight = 12f;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            return oPdfPTable;
        }
        private PdfPTable DriverNameAndTruckNo()
        {
            PdfPTable oPdfPTable = new PdfPTable(1);
            oPdfPTable.SetWidths(new float[] { 100f });

            #region Row 1
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("Driver's Name ............................", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.FixedHeight = 12f;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Row 2
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("Truck Name ...............................", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.FixedHeight = 12f;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            return oPdfPTable;
        }
    }

    public class PDFFooter1 : PdfPageEventHelper
    {
        // This is the contentbyte object of the writer
        PdfContentByte cb;
        // we will put the final number of pages in a template
        PdfTemplate template;
        // this is the BaseFont we are going to use for the header / footer
        BaseFont bf = null;
        // This keeps track of the creation time
        DateTime PrintTime = DateTime.Now;
        #region Properties
        private string _Title;
        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }

        private string _HeaderLeft;
        public string HeaderLeft
        {
            get { return _HeaderLeft; }
            set { _HeaderLeft = value; }
        }
        private string _HeaderRight;
        public string HeaderRight
        {
            get { return _HeaderRight; }
            set { _HeaderRight = value; }
        }
        private iTextSharp.text.Font _HeaderFont;
        public iTextSharp.text.Font HeaderFont
        {
            get { return _HeaderFont; }
            set { _HeaderFont = value; }
        }
        private iTextSharp.text.Font _FooterFont;
        public iTextSharp.text.Font FooterFont
        {
            get { return _FooterFont; }
            set { _FooterFont = value; }
        }
        private bool _PrintDocumentGenerator = true;
        public bool PrintDocumentGenerator
        {
            get { return _PrintDocumentGenerator; }
            set { _PrintDocumentGenerator = value; }
        }
        private bool _PrintPrintingDateTime = true;
        public bool PrintPrintingDateTime
        {
            get { return _PrintPrintingDateTime; }
            set { _PrintPrintingDateTime = value; }
        }

        public List<string> signatures { get; set; }

        #endregion
        // we override the onOpenDocument method
        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            try
            {
                PrintTime = DateTime.Now;
                bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                cb = writer.DirectContent;
                template = cb.CreateTemplate(50, 50);
            }
            catch (DocumentException de)
            {
            }
            catch (System.IO.IOException ioe)
            {
            }
        }

     
        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);
            int pageN = writer.PageNumber;
            String text = "Page " + pageN + " of ";
            float len = bf.GetWidthPoint(text, 8);
            iTextSharp.text.Rectangle pageSize = document.PageSize;
            cb.SetRGBColorFill(100, 100, 100);

            float bottomMarginTopBar = 42, bottomMarginSig = 30;

           

            //cb.BeginText();
            //cb.SetFontAndSize(bf, 10);
          
            //cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER,"Received the fabrics in good condition", pageSize.GetRight(300), pageSize.GetBottom(90), 0);
            //cb.EndText();

            cb.BeginText();
            cb.SetFontAndSize(bf, 10);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Driver's Name...................", pageSize.GetRight(580), pageSize.GetBottom(85), 0);
            cb.EndText();

            cb.BeginText();
            cb.SetFontAndSize(bf, 10);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Truck Name......................", pageSize.GetRight(580), pageSize.GetBottom(70), 0);
            cb.EndText();

            if (signatures != null && signatures.Any())
            {
                float nWidthPerSection = (document.PageSize.Width - (document.LeftMargin + document.RightMargin)) / signatures.Count();

                float nleft = 0;
                for (int i = 0; i < signatures.Count(); i++)
                {
                    string topBar = "";
                    if (signatures[i].Length < 12)
                    {
                        topBar = "____________";
                    }
                    else
                    {
                        topBar = "__";
                        signatures[i].ToList().ForEach(x => { topBar += "_"; });
                    }



                    int nSiglen = (topBar.Length > signatures[i].Length) ? topBar.Length : signatures[i].Length;
                    float nSpan = nWidthPerSection - (nSiglen * 5);
                    nleft = (document.LeftMargin * 2) + (nWidthPerSection * i) + ((nSpan > 0) ? nSpan / 2 : 0);

                    if (i == 0)
                    {
                        cb.BeginText();
                        cb.SetFontAndSize(bf, 7);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, signatures[i], pageSize.GetLeft(nleft), pageSize.GetBottom(bottomMarginSig), 0);
                        cb.EndText();

                        cb.BeginText();
                        cb.SetFontAndSize(bf, 7);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, topBar, pageSize.GetLeft(nleft), pageSize.GetBottom(bottomMarginTopBar), 0);
                        cb.EndText();
                    }
                    else if (i != 0 && i < signatures.Count() - 1)
                    {
                        cb.BeginText();
                        cb.SetFontAndSize(bf, 7);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, signatures[i], pageSize.GetLeft(nleft), pageSize.GetBottom(bottomMarginSig), 0);
                        cb.EndText();

                        cb.BeginText();
                        cb.SetFontAndSize(bf, 7);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, topBar, pageSize.GetLeft(nleft), pageSize.GetBottom(bottomMarginTopBar), 0);
                        cb.EndText();
                    }
                    else
                    {
                        cb.BeginText();
                        cb.SetFontAndSize(bf, 7);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, signatures[i], pageSize.GetLeft(nleft), pageSize.GetBottom(bottomMarginSig), 0);
                        cb.EndText();

                        cb.BeginText();
                        cb.SetFontAndSize(bf, 7);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, topBar, pageSize.GetLeft(nleft), pageSize.GetBottom(bottomMarginTopBar), 0);
                        cb.EndText();
                    }
                }
            }




            cb.BeginText();
            cb.SetFontAndSize(bf, 7);
            cb.SetTextMatrix(pageSize.GetLeft(40), pageSize.GetBottom(10));

            cb.ShowText(text);
            cb.EndText();
            cb.AddTemplate(template, pageSize.GetLeft(40) + len, pageSize.GetBottom(10));


            cb.BeginText();
            cb.SetFontAndSize(bf, 7);
            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, this.PrintDocumentGenerator ? "ESimSol generated document" : "", pageSize.GetRight((pageSize.Width / 2 - 20)), pageSize.GetBottom(10), 0);
            cb.EndText();

            cb.BeginText();
            cb.SetFontAndSize(bf, 7);
            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, this.PrintPrintingDateTime ? "Print at " + PrintTime.ToString("dd-MMM-yyyy hh:mm tt") : "", pageSize.GetRight(40), pageSize.GetBottom(10), 0);
            cb.EndText();
        }
        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);
            template.BeginText();
            template.SetFontAndSize(bf, 8);
            template.SetTextMatrix(0, 0);
            template.ShowText("" + (writer.PageNumber - 1));
            template.EndText();
        }
    }

 
}

    
