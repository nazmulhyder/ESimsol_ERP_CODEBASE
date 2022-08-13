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
    public class rptFabricTransferPackingList
    {
        #region Declaration
        int _nTotalColumn = 9;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyle_Bold;
        PdfPTable _oPdfPTable = new PdfPTable(9);
        PdfPCell _oPdfPCell;
        MemoryStream _oMemoryStream = new MemoryStream();
        FabricTransferPackingList _oFTPL = new FabricTransferPackingList();
        bool _bIsInYard = true;
        Company _oCompany = new Company();
        string _sMessage = "";
        int _nSL = 0;
        double _nTotalQty = 0;
        int _nTotalRoll = 0;
        int nRowHeight = 14;
        BaseColor _oBC = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#EFF0EA"));
        #endregion

        //public byte[] PrepareReport(FabricTransferPackingList oFTPL, bool bIsInYard, Company oCompany, string sMessage)
        //{
        //    _oFTPL = oFTPL;
        //    _bIsInYard = bIsInYard;
        //    _oCompany = oCompany;
        //    _sMessage = sMessage;

        //    #region Page Setup
        //    _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
        //    _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
        //    _oDocument.SetMargins(40f, 40f, 5f, 40f);
        //    _oPdfPTable.WidthPercentage = 100;
        //    _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

        //    _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
        //    PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
        //    PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
        //    ESimSolFooter PageEventHandler = new ESimSolFooter();
        //    PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
        //    _oDocument.Open();

        //    _oPdfPTable.SetWidths(new float[] { 40f, 100f, 100f, 40f, 100f, 100f, 40f, 100f, 100f });
        //    #endregion

        //    this.PrintHeader();
        //    this.PrintBody();
        //    _oPdfPTable.HeaderRows = 3;
        //    _oDocument.Add(_oPdfPTable);
        //    _oDocument.Close();
        //    return _oMemoryStream.ToArray();
        //}

        #region Report Header
        private void PrintHeader()
        {
            #region CompanyHeader
            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Name.ToUpper(), _oFontStyle));
            _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.FactoryAddress, _oFontStyle));
            _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

            #region ReportHeader
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(_sMessage, _oFontStyle));
            _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 5f;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        #endregion

        //#region Report Body
        //private void PrintBody()
        //{
        //    #region Single object information
        //    _oPdfPCell = new PdfPCell(this.PackingListInfo());
        //    _oPdfPCell.Colspan = _nTotalColumn;
        //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //    _oPdfPCell.Border = 0;
        //    _oPdfPTable.AddCell(_oPdfPCell);
        //    _oPdfPTable.CompleteRow();
        //    #endregion

        //    this.Blank(10);

        //    #region Table Header
        //    _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
        //    _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle));
        //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //    _oPdfPCell.BackgroundColor = _oBC;
        //    _oPdfPTable.AddCell(_oPdfPCell);

        //    _oPdfPCell = new PdfPCell(new Phrase("Roll No", _oFontStyle));
        //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //    _oPdfPCell.BackgroundColor = _oBC;
        //    _oPdfPTable.AddCell(_oPdfPCell);

        //    _oPdfPCell = new PdfPCell(new Phrase("Fab. Qty", _oFontStyle));
        //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //    _oPdfPCell.BackgroundColor = _oBC;
        //    _oPdfPTable.AddCell(_oPdfPCell);

        //    _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle));
        //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //    _oPdfPCell.BackgroundColor = _oBC;
        //    _oPdfPTable.AddCell(_oPdfPCell);

        //    _oPdfPCell = new PdfPCell(new Phrase("Roll No", _oFontStyle));
        //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //    _oPdfPCell.BackgroundColor = _oBC;
        //    _oPdfPTable.AddCell(_oPdfPCell);

        //    _oPdfPCell = new PdfPCell(new Phrase("Fab. Qty", _oFontStyle));
        //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //    _oPdfPCell.BackgroundColor = _oBC;
        //    _oPdfPTable.AddCell(_oPdfPCell);

        //    _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle));
        //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //    _oPdfPCell.BackgroundColor = _oBC;
        //    _oPdfPTable.AddCell(_oPdfPCell);

        //    _oPdfPCell = new PdfPCell(new Phrase("Roll No", _oFontStyle));
        //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //    _oPdfPCell.BackgroundColor = _oBC;
        //    _oPdfPTable.AddCell(_oPdfPCell);

        //    _oPdfPCell = new PdfPCell(new Phrase("Fab. Qty", _oFontStyle));
        //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //    _oPdfPCell.BackgroundColor = _oBC;
        //    _oPdfPTable.AddCell(_oPdfPCell);
        //    _oPdfPTable.CompleteRow();
        //    #endregion

        //    #region Detail (90 rows print)

        //    _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
        //    for (int i = 1; i <= 3; i++)
        //    {
        //        _oPdfPCell = new PdfPCell(this.MakeOneColumn());
        //        _oPdfPCell.Colspan = 3;
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //        _oPdfPCell.Border = 0;
        //        _oPdfPTable.AddCell(_oPdfPCell);
        //    }
        //    _oPdfPTable.CompleteRow();
        //    #endregion

        //    #region Total
        //    string GTConverstionQty = string.Empty;
        //    if (_bIsInYard)
        //    {
        //        GTConverstionQty = Global.MillionFormat(_nTotalQty, 2) + " yard" + " / " + Global.MillionFormat(Global.GetMeter(_nTotalQty, 2), 2) + " meter";
        //    }
        //    else
        //    {
        //        GTConverstionQty = Global.MillionFormat(_nTotalQty, 2) + " meter" + " / " + Global.MillionFormat(Global.GetYard(_nTotalQty, 2), 2) + " yard";
        //    }
        //    _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
        //    _oPdfPCell = new PdfPCell(new Phrase("G.Total : " + _nTotalRoll + " roll" + (_nTotalRoll > 1 ? "s" : "") + " and " + GTConverstionQty, _oFontStyle));
        //    _oPdfPCell.Colspan = _nTotalColumn;
        //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //    _oPdfPCell.Border = 0;
        //    _oPdfPTable.AddCell(_oPdfPCell);
        //    _oPdfPTable.CompleteRow();
        //    #endregion

        //    this.Blank(20);

        //    #region Signature
        //    _oPdfPCell = new PdfPCell(this.Signature());
        //    _oPdfPCell.Colspan = _nTotalColumn;
        //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //    _oPdfPCell.Border = 0;
        //    _oPdfPTable.AddCell(_oPdfPCell);
        //    _oPdfPTable.CompleteRow();
        //    #endregion
        //}
        //#endregion
        private PdfPTable Signature()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);

            PdfPTable oPdfPTable = new PdfPTable(4);
            oPdfPTable.SetWidths(new float[] { 100f, 100f, 100f, 100f });

            _oPdfPCell = new PdfPCell(new Phrase("_____________", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("_____________", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("_____________", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("_____________", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase("Prepared By", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Received By", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Checked By", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Approved By", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            return oPdfPTable;
        }
        private PdfPTable MakeOneColumn()
        {
            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.SetWidths(new float[] { 40f, 100f, 100f });

            for (int i = 1; i <= 31; i++)
            {
                if (i != 31)
                {
                    _nSL++;

                    _oPdfPCell = new PdfPCell(new Phrase(_nSL.ToString(), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.FixedHeight = 20f;
                    oPdfPTable.AddCell(_oPdfPCell);

                    int nIndex = _nSL - 1;

                    if (_oFTPL.FTPLDetails.Count > nIndex)
                    {
                        _nTotalRoll++;
                        _oPdfPCell = new PdfPCell(new Phrase(_oFTPL.FTPLDetails[nIndex].LotNo, _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPCell.FixedHeight = 20f;
                        oPdfPTable.AddCell(_oPdfPCell);

                        if (!_bIsInYard)
                        {
                            _oFTPL.FTPLDetails[nIndex].Qty = Global.GetMeter(_oFTPL.FTPLDetails[nIndex].Qty, 2);
                        }
                        _nTotalQty += _oFTPL.FTPLDetails[nIndex].Qty;
                        _oPdfPCell = new PdfPCell(new Phrase(_oFTPL.FTPLDetails[nIndex].QtySt, _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPCell.FixedHeight = 20f;
                        oPdfPTable.AddCell(_oPdfPCell);
                    }
                    else
                    {
                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPCell.FixedHeight = 20f;
                        oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPCell.FixedHeight = 20f;
                        oPdfPTable.AddCell(_oPdfPCell);
                    }
                    oPdfPTable.CompleteRow();
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.FixedHeight = 20f;
                    oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.FixedHeight = 20f;
                    oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.FixedHeight = 20f;
                    oPdfPTable.AddCell(_oPdfPCell);
                    oPdfPTable.CompleteRow();
                }
            }
            return oPdfPTable;
        }
        private PdfPTable PackingListInfo()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            PdfPTable oPdfPTable = new PdfPTable(6);
            oPdfPTable.SetWidths(new float[] { 60f, 100f, 100f, 150f, 100f, 100f });

            #region Row 1
            _oPdfPCell = new PdfPCell(new Phrase("FEO No : ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oFTPL.OrderNo, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Buyer/Party : ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oFTPL.BuyerName, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Date : ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oFTPL.PackingListDateSt, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Row 2
            _oPdfPCell = new PdfPCell(new Phrase("Unit : ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase((_bIsInYard == true ? "Yard" : "Meter"), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Fab Type : ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oFTPL.FabType, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Fab. Construc : ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oFTPL.Construction, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Row 3
            _oPdfPCell = new PdfPCell(new Phrase("Grey Width : ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            int decPart = Convert.ToInt32(_oFTPL.GreyWidth.Split('.')[1]);

            _oPdfPCell = new PdfPCell(new Phrase((decPart == 0) ? _oFTPL.GreyWidth.Split('.')[0] : _oFTPL.GreyWidth, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Wrap Lot : ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oFTPL.WarpLot, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Weft Lot : ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oFTPL.WeftLot, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            return oPdfPTable;
        }
        private void Blank(int nVal)
        {
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.FixedHeight = nVal;
            _oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPTable.AddCell(_oPdfPCell);
        }

        #region New

        public byte[] PrepareReportTwo(FabricTransferPackingList oFTPL, bool bIsInYard, Company oCompany, string sMessage)
        {
            _oFTPL = oFTPL;
            _bIsInYard = bIsInYard;
            _oCompany = oCompany;
            _sMessage = sMessage;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            _oDocument.SetMargins(40f, 40f, 5f, 30f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            //PageEventHandler.signatures = new List<string>(new string[] { "Prepared By", "Section Incharge", "User Department", "Audit Department", "Authorised By" });

            _oDocument.Open();

            _oPdfPTable.SetWidths(new float[] { 40f, 100f, 100f, 40f, 100f, 100f, 40f, 100f, 100f });
            #endregion

            this.PrintHeader();
            this.PrintBodyTwo();
            _oPdfPTable.HeaderRows = 0;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        private void PrintBodyTwo()
        {
            _oPdfPTable.DeleteBodyRows();

            int nCount = 0;
          

                PdfPTable oPdfPTable = Table_Packing();
                var oFDCDs = _oFTPL.FTPLDetails;//.Where(x => x.FTPLDetailID == oItem.FTPLDetailID).ToList();
                int nData = oFDCDs.Count();
                int nRows = (nData / 3) + ((nData % 3 == 0) ? 0 : 3 - (nData % 3));
                nCount = 0;
                for (int i = 0; i < nRows; i += 46)
                {
                    _oDocument.NewPage();
                    this.PrintHeader();
                    this.PackingListInfoTwo();
                   
                    SetValueToParentTable(ref nCount, 45, nData, oFDCDs);
                    _oDocument.Add(_oPdfPTable);
                    _oPdfPTable.DeleteBodyRows();
                }
                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
                _nTotalQty = _oFTPL.FTPLDetails.Select(o => System.Math.Round(o.Qty, 5)).Sum();
                _nTotalRoll = _oFTPL.FTPLDetails.Count();
                string GTConverstionQty = string.Empty;
          
                if (_bIsInYard)
                {
                    GTConverstionQty = Global.MillionFormat(_nTotalQty, 2) + " yard" + " / " + Global.MillionFormat(_oFTPL.FTPLDetails.Select(x => x.QtyInM).Sum(), 0) + " meter";
                }
                else
                {
                    GTConverstionQty = Global.MillionFormat(_oFTPL.FTPLDetails.Select(x => x.QtyInM).Sum(), 0) + " meter" + " / " + Global.MillionFormat(_nTotalQty, 2) + " yard";
                }
                ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "G.Total : " + _nTotalRoll + " roll" + (_nTotalRoll > 1 ? "s" : "") + " and " + GTConverstionQty, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle, true);

                this.Blank(22);

                #region Signature
                _oPdfPCell = new PdfPCell(this.Signature());
                _oPdfPCell.Colspan = _nTotalColumn;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.Border = 0;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

        }
        private void PackingListInfoTwo()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            PdfPTable oPdfPTable = new PdfPTable(6);
            oPdfPTable.SetWidths(new float[] { 90f, 100f, 100f, 150f, 100f, 100f });

            #region Row 1
            _oPdfPCell = new PdfPCell(new Phrase("FEO No : ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
           // _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oFTPL.OrderNo, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Buyer/Party : ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oFTPL.BuyerName, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Date : ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oFTPL.PackingListDateSt, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Row 2
            _oPdfPCell = new PdfPCell(new Phrase("Unit : ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase((_bIsInYard == true ? "Yard" : "Meter"), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Fab Type : ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oFTPL.FabType, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Fab. Construc : ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oFTPL.Construction, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Row 3
            _oPdfPCell = new PdfPCell(new Phrase("Grey Width:", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
           // _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 12f;
            oPdfPTable.AddCell(_oPdfPCell);


            int decPart = Convert.ToInt32(_oFTPL.GreyWidth.Split('.')[1]);

            _oPdfPCell = new PdfPCell(new Phrase((decPart == 0) ? _oFTPL.GreyWidth.Split('.')[0] : _oFTPL.GreyWidth, _oFontStyle));
           
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 12f;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Wrap Lot : ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 12f;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oFTPL.WarpLot, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 12f;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Weft Lot : ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 12f;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oFTPL.WeftLot, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 12f;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.Colspan = _nTotalColumn;  _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            
        }
        private static PdfPTable Table_Packing()
        {
            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                   25f, //"SL", 
                                                   55f, //"Roll No"
                                                 
                                                   40f, //"Fab. Qty"
                                             });
            return oPdfPTable;

        }

        private void MakeTableHeader(ref PdfPTable oPdfPTable)
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);

            string[] tableHeader = new string[] { "SL", "Roll No", "Fab. Qty" };
            foreach (string head in tableHeader)
            {
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, head, 0, 0, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, true, 0, _oFontStyle);
            }
            oPdfPTable.CompleteRow();
        }
        private void SetValueToParentTable(ref int nStartIndex, int nIteration, int nData, List<FabricTransferPackingListDetail> oFDCDs)
        {
            PdfPTable oPdfPTableTemp = new PdfPTable(3);
            oPdfPTableTemp.SetWidths(new float[] { 195f, 195f, 195f });
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            PdfPTable oPdfPTable = Table_Packing();
            int i = 0;

            #region Left
            MakeTableHeader(ref oPdfPTable);
            for (i = nStartIndex; i < nIteration + nStartIndex; i++)
            {
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (i + 1).ToString(), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, nRowHeight, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (i < nData) ? oFDCDs[i].LotNo : "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, nRowHeight, _oFontStyle);
                double cellvalQty = 0;
                if (i < nData)
                {
                    if ((_bIsInYard))
                        cellvalQty = oFDCDs[i].Qty;
                    else
                        cellvalQty = Math.Round(Global.GetMeter(oFDCDs[i].Qty, 0), 0);
                }
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (i < nData) ? cellvalQty.ToString() : "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, nRowHeight, _oFontStyle);
            }
            ESimSolItexSharp.PushTableInCell(ref oPdfPTableTemp, oPdfPTable, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, false);
            #endregion

            #region Middle
            nStartIndex = i;
            oPdfPTable = Table_Packing();
            MakeTableHeader(ref oPdfPTable);
            for (i = nStartIndex; i < nIteration + nStartIndex; i++)
            {
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (i + 1).ToString(), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, nRowHeight, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (i < nData) ? oFDCDs[i].LotNo : "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, nRowHeight, _oFontStyle);
                //ESimSolItexSharp.SetCellValue(ref oPdfPTable, (i < nData) ? oFDCDs[i].StyleNo : "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, nRowHeight, _oFontStyle);
                double cellvalQty = 0;
                if (i < nData)
                {
                    if ((_bIsInYard))
                        cellvalQty = oFDCDs[i].Qty;
                    else
                        cellvalQty = Math.Round(Global.GetMeter(oFDCDs[i].Qty, 0), 0);
                }
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (i < nData) ? cellvalQty.ToString() : "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, nRowHeight, _oFontStyle);
            }
            ESimSolItexSharp.PushTableInCell(ref oPdfPTableTemp, oPdfPTable, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, false);
            #endregion

            #region Middle
            nStartIndex = i;
            oPdfPTable = Table_Packing();
            MakeTableHeader(ref oPdfPTable);
            for (i = nStartIndex; i < nIteration + nStartIndex; i++)
            {
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (i + 1).ToString(), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, nRowHeight, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (i < nData) ? oFDCDs[i].LotNo : "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, nRowHeight, _oFontStyle);
                double cellvalQty = 0;
                if (i < nData)
                {
                    if ((_bIsInYard))
                        cellvalQty = oFDCDs[i].Qty;
                    else
                        cellvalQty = Math.Round(Global.GetMeter(oFDCDs[i].Qty, 0),0);
                }
               
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (i < nData) ? cellvalQty.ToString() : "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, nRowHeight, _oFontStyle);
            }
            ESimSolItexSharp.PushTableInCell(ref oPdfPTableTemp, oPdfPTable, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, false);
            nStartIndex = i;
            #endregion
            oPdfPTableTemp.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTableTemp, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, false);
        }

        #endregion
    }
}
