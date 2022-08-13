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

    public class rptDUHardWindingReport
    {
        #region Declaration
        int count, num;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyleBold;
        
        PdfPTable _oPdfPTable = new PdfPTable(1);
        //PdfPTable _oPdfPTableDetail = new PdfPTable(5);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        DUHardWindingReport _oDUHardWindingReport = new DUHardWindingReport();
        List<DUHardWindingReport> _oDUHardWindingReports = new List<DUHardWindingReport>();
        Company _oCompany = new Company();
        List<DUHardWinding> _oDUHardWindings = new List<DUHardWinding>();
        BusinessUnit _oBusinessUnit = new BusinessUnit();
        List<FabricBeamFinish> _oFabricBeamFinishs = new List<FabricBeamFinish>();
        string sReportHeader = "";
        string _sDateRange = "";
        #endregion

        public byte[] PrepareReport(List<DUHardWindingReport> oDUHardWindingReports, Company oCompany)
        {
            _oDUHardWindingReports = oDUHardWindingReports;
            _oCompany = oCompany;
            _sDateRange = "";

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
            _oDocument.SetMargins(25f, 25f, 5f, 25f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);

            PdfWriter PdfWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);

            ESimSolFooter PageEventHandler = new ESimSolFooter();

            //PageEventHandler.signatures = signatureList;
            PageEventHandler.nFontSize = 9;
            PdfWriter.PageEvent = PageEventHandler; //Footer print with page event handler

            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 842f });
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
            #region Company & Report Header
            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 100f, 450f, 250f });

            #region Company Name & Report Header
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
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
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
            _oPdfPCell = new PdfPCell(new Phrase("Hard Winding Report", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Company Address & Date Range
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Address, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(_sDateRange, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Company Phone Number
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Phone + ";  " + _oCompany.Email + ";  " + _oCompany.WebAddress, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Blank Row
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 1f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Blank Row
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #endregion
        }
        #endregion

        #region Report Body
        private void PrintBody()
        {
            this.GetDUHardWindingReportTable();
        }
        #endregion

        private void GetDUHardWindingReportTable()
        {
            PdfPTable oDUHardWindingReportTable = new PdfPTable(14);
            oDUHardWindingReportTable.WidthPercentage = 100;
            oDUHardWindingReportTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oDUHardWindingReportTable.SetWidths(new float[] { 25f, 80f, 80f, 80f, 80f, 80f, 80f, 80f, 80f, 80f, 80f, 80f, 80f, 80f});

            _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD);

            #region Header
            _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; 
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDUHardWindingReportTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Date", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDUHardWindingReportTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("H/W (In) + H/W (Out)", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.Colspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDUHardWindingReportTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Reprocess (In)", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDUHardWindingReportTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Reprocess (Out)", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDUHardWindingReportTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Greige Prod.", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDUHardWindingReportTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDUHardWindingReportTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("L/O", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDUHardWindingReportTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("G. Total", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDUHardWindingReportTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Bream Comm.", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDUHardWindingReportTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Beam T/F", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDUHardWindingReportTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Warping", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDUHardWindingReportTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Beam Stock", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDUHardWindingReportTable.AddCell(_oPdfPCell);

            oDUHardWindingReportTable.CompleteRow();
            #endregion

            #region push into main table
            _oPdfPCell = new PdfPCell(oDUHardWindingReportTable); _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Initialize Table
            oDUHardWindingReportTable = new PdfPTable(14);
            oDUHardWindingReportTable.WidthPercentage = 100;
            oDUHardWindingReportTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oDUHardWindingReportTable.SetWidths(new float[] { 25f, 80f, 80f, 80f, 80f, 80f, 80f, 80f, 80f, 80f, 80f, 80f, 80f, 80f });
            #endregion

            _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.NORMAL);

            #region Data
            double nQtyHWIn = 0, nQtyHWOut = 0, nQtyReHWIn = 0, nQtyReHWOut = 0, nQtyGreige = 0, nTotalQty = 0, nQty_LO = 0, nGrandTotalQty = 0, nBeamCom = 0, nBeamTF = 0, nWarping = 0, nBeamStock = 0;
            foreach (DUHardWindingReport oItem in _oDUHardWindingReports)
            {
                count++;
                _oPdfPCell = new PdfPCell(new Phrase(count.ToString("00"), _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDUHardWindingReportTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.StartDateInString, _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDUHardWindingReportTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.QtyHWIn.ToString("#,##0.00;(#,##0.00)"), _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDUHardWindingReportTable.AddCell(_oPdfPCell);
                nQtyHWIn += oItem.QtyHWIn;

                _oPdfPCell = new PdfPCell(new Phrase(oItem.QtyHWOut.ToString("#,##0.00;(#,##0.00)"), _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDUHardWindingReportTable.AddCell(_oPdfPCell);
                nQtyHWOut += oItem.QtyHWOut;

                _oPdfPCell = new PdfPCell(new Phrase(oItem.QtyReHWIn.ToString("#,##0.00;(#,##0.00)"), _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDUHardWindingReportTable.AddCell(_oPdfPCell);
                nQtyReHWIn += oItem.QtyReHWIn;

                _oPdfPCell = new PdfPCell(new Phrase(oItem.QtyReHWOut.ToString("#,##0.00;(#,##0.00)"), _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDUHardWindingReportTable.AddCell(_oPdfPCell);
                nQtyReHWOut += oItem.QtyReHWOut;

                _oPdfPCell = new PdfPCell(new Phrase(oItem.QtyGreige.ToString("#,##0.00;(#,##0.00)"), _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDUHardWindingReportTable.AddCell(_oPdfPCell);
                nQtyGreige += oItem.QtyGreige;

                _oPdfPCell = new PdfPCell(new Phrase(oItem.TotalQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDUHardWindingReportTable.AddCell(_oPdfPCell);
                nTotalQty += oItem.TotalQty;

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Qty_LO.ToString("#,##0.00;(#,##0.00)"), _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDUHardWindingReportTable.AddCell(_oPdfPCell);
                nQty_LO += oItem.Qty_LO;

                _oPdfPCell = new PdfPCell(new Phrase(oItem.GrandTotalQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDUHardWindingReportTable.AddCell(_oPdfPCell);
                nGrandTotalQty += oItem.GrandTotalQty;

                _oPdfPCell = new PdfPCell(new Phrase(oItem.BeamCom.ToString("#,##0.00;(#,##0.00)"), _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDUHardWindingReportTable.AddCell(_oPdfPCell);
                nBeamCom += oItem.BeamCom;

                _oPdfPCell = new PdfPCell(new Phrase(oItem.BeamTF.ToString("#,##0.00;(#,##0.00)"), _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDUHardWindingReportTable.AddCell(_oPdfPCell);
                nBeamTF += oItem.BeamTF;

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Warping.ToString("#,##0.00;(#,##0.00)"), _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDUHardWindingReportTable.AddCell(_oPdfPCell);
                nWarping += oItem.Warping;

                _oPdfPCell = new PdfPCell(new Phrase(oItem.BeamStock.ToString("#,##0.00;(#,##0.00)"), _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDUHardWindingReportTable.AddCell(_oPdfPCell);
                nBeamStock += oItem.BeamStock;

                oDUHardWindingReportTable.CompleteRow();

                #region push into main table
                _oPdfPCell = new PdfPCell(oDUHardWindingReportTable); _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                #region Initialize Table
                oDUHardWindingReportTable = new PdfPTable(14);
                oDUHardWindingReportTable.WidthPercentage = 100;
                oDUHardWindingReportTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oDUHardWindingReportTable.SetWidths(new float[] { 25f, 80f, 80f, 80f, 80f, 80f, 80f, 80f, 80f, 80f, 80f, 80f, 80f, 80f });
                #endregion

            }
            #endregion

            _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD);
            #region Total
            _oPdfPCell = new PdfPCell(new Phrase("Total : ", _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.Colspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDUHardWindingReportTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(nQtyHWIn.ToString("#,##0.00;(#,##0.00)"), _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDUHardWindingReportTable.AddCell(_oPdfPCell);
            
            _oPdfPCell = new PdfPCell(new Phrase(nQtyHWOut.ToString("#,##0.00;(#,##0.00)"), _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDUHardWindingReportTable.AddCell(_oPdfPCell);
            
            _oPdfPCell = new PdfPCell(new Phrase(nQtyReHWIn.ToString("#,##0.00;(#,##0.00)"), _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDUHardWindingReportTable.AddCell(_oPdfPCell);
            
            _oPdfPCell = new PdfPCell(new Phrase(nQtyReHWOut.ToString("#,##0.00;(#,##0.00)"), _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDUHardWindingReportTable.AddCell(_oPdfPCell);
            
            _oPdfPCell = new PdfPCell(new Phrase(nQtyGreige.ToString("#,##0.00;(#,##0.00)"), _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDUHardWindingReportTable.AddCell(_oPdfPCell);
            
            _oPdfPCell = new PdfPCell(new Phrase(nTotalQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDUHardWindingReportTable.AddCell(_oPdfPCell);
            
            _oPdfPCell = new PdfPCell(new Phrase(nQty_LO.ToString("#,##0.00;(#,##0.00)"), _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDUHardWindingReportTable.AddCell(_oPdfPCell);
            
            _oPdfPCell = new PdfPCell(new Phrase(nGrandTotalQty.ToString("#,##0.00;(#,##0.00)"), _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDUHardWindingReportTable.AddCell(_oPdfPCell);
            
            _oPdfPCell = new PdfPCell(new Phrase(nBeamCom.ToString("#,##0.00;(#,##0.00)"), _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDUHardWindingReportTable.AddCell(_oPdfPCell);
            
            _oPdfPCell = new PdfPCell(new Phrase(nBeamTF.ToString("#,##0.00;(#,##0.00)"), _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDUHardWindingReportTable.AddCell(_oPdfPCell);
            
            _oPdfPCell = new PdfPCell(new Phrase(nWarping.ToString("#,##0.00;(#,##0.00)"), _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDUHardWindingReportTable.AddCell(_oPdfPCell);
            
            _oPdfPCell = new PdfPCell(new Phrase(nBeamStock.ToString("#,##0.00;(#,##0.00)"), _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDUHardWindingReportTable.AddCell(_oPdfPCell);
            
            oDUHardWindingReportTable.CompleteRow();
            #endregion

            #region push into main table
            _oPdfPCell = new PdfPCell(oDUHardWindingReportTable); _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Initialize Table
            oDUHardWindingReportTable = new PdfPTable(14);
            oDUHardWindingReportTable.WidthPercentage = 100;
            oDUHardWindingReportTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oDUHardWindingReportTable.SetWidths(new float[] { 25f, 80f, 80f, 80f, 80f, 80f, 80f, 80f, 80f, 80f, 80f, 80f, 80f, 80f });
            #endregion

        }


        public byte[] PrepareReport_Detail(DUHardWindingReport oDUHardWindingReport, List<DUHardWinding> oDUHardWindings, List<FabricExecutionOrderYarnReceive> oFabricExecutionOrderYarnReceives, Company oCompany, BusinessUnit oBusinessUnit, List<FabricBeamFinish> oFabricBeamFinishs)
        {
            _oDUHardWindings = oDUHardWindings;
            _oCompany = oCompany;
            _oBusinessUnit = oBusinessUnit;
            _oFabricBeamFinishs = oFabricBeamFinishs;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
            _oDocument.SetMargins(25f, 25f, 10f, 25f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);

            PdfWriter PdfWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);

            ESimSolFooter PageEventHandler = new ESimSolFooter();

            //PageEventHandler.signatures = signatureList;
            PageEventHandler.nFontSize = 9;
            PdfWriter.PageEvent = PageEventHandler; //Footer print with page event handler

            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 842f });
            #endregion
            List<DUHardWinding> oDUHardWindingsTwo = new List<DUHardWinding>();
            this.PrintHeaderTwo();
            if (oDUHardWindingReport.ReportType == 1)
            {
                oDUHardWindingsTwo = _oDUHardWindings.Where(x => x.IsInHouse == true).ToList();
                if (oDUHardWindingsTwo.Count > 0)
                {
                    this.ReportHeader_Detail("Dyed Yarn Receive in H/W", "In House", " Receive Date " + oDUHardWindingReport.StartDate.ToString("dd MMM yyyy  hh:mm tt"));
                    this.SetSummary_Detail(oDUHardWindingsTwo);
                }
              
                _oDocument.Add(_oPdfPTable);
                _oDocument.NewPage();
                _oPdfPTable.DeleteBodyRows();
            
                oDUHardWindingsTwo = _oDUHardWindings.Where(x => x.IsInHouse == false).ToList();
                if (oDUHardWindings.Count > 0)
                {
                    this.PrintHeaderTwo();
                    this.ReportHeader_Detail("Dyed Yarn Receive in H/W", "Out House", " Receive Date:" + oDUHardWindingReport.StartDate.ToString("dd MMM yyyy hh:mm tt"));
                    this.SetSummary_Detail(oDUHardWindingsTwo);
                }
            }
            if (oDUHardWindingReport.ReportType == 2)
            {
                oDUHardWindingsTwo = _oDUHardWindings.Where(x => x.IsInHouse == true && x.IsRewinded==true).ToList();
                if (oDUHardWindingsTwo.Count > 0)
                {
                    this.ReportHeader_Detail("Re Process in H/W", "In House", " Receive Date:" + oDUHardWindingReport.StartDate.ToString("dd MMM yyyy"));
                    this.SetSummary_Detail(oDUHardWindingsTwo);
                }
                _oDocument.Add(_oPdfPTable);
                _oDocument.NewPage();
                _oPdfPTable.DeleteBodyRows();
                oDUHardWindingsTwo = _oDUHardWindings.Where(x => x.IsInHouse == false && x.IsRewinded == true).ToList();
                if (oDUHardWindings.Count > 0)
                {
                    this.PrintHeaderTwo();
                    this.ReportHeader_Detail("Re Process in H/W", "Out House", " Receive Date:" + oDUHardWindingReport.StartDate.ToString("dd MMM yyyy"));
                    this.SetSummary_Detail(oDUHardWindingsTwo);
                }
            }
            if (oDUHardWindingReport.ReportType == 6)
            {
              
                if (oFabricExecutionOrderYarnReceives.Count > 0)
                {
                    this.ReportHeader_Detail("Beam T/F", "In House", " T/F Date:" + oDUHardWindingReport.StartDate.ToString("dd MMM yyyy"));
                    this.SetWURequsation(oFabricExecutionOrderYarnReceives);
                }
            }
            if (oDUHardWindingReport.ReportType == 8)
            {

                if (oFabricExecutionOrderYarnReceives.Count > 0)
                {
                    this.ReportHeader_Detail("Left Over", " ", " Req. Date:" + oDUHardWindingReport.StartDate.ToString("dd MMM yyyy"));
                    this.SetWURequsationLeftOver(oFabricExecutionOrderYarnReceives);
                }
            }

            if (oDUHardWindingReport.ReportType == 4)//Beam Com
            {
                if (_oFabricBeamFinishs.Count > 0)
                {
                    this.ReportHeader_Detail("Beam Complete", "In House", " Comp. Date:" + oDUHardWindingReport.StartDate.ToString("dd MMM yyyy"));
                    this.SetFabricBeamFinish();
                }
            }
            if (oDUHardWindingReport.ReportType == 7)//Beam Stock
            {
                if (_oFabricBeamFinishs.Count > 0)
                {
                    this.ReportHeader_Detail("Beam Stock", "In House", " Comp. Date:" + oDUHardWindingReport.StartDate.ToString("dd MMM yyyy"));
                    this.SetFabricBeamFinishForBeamStock();
                }
            }

            _oPdfPTable.HeaderRows = 2;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        private void PrintHeaderTwo()
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
                _oImag.ScaleAbsolute(70f, 40f);
                _oPdfPCell = new PdfPCell(_oImag);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            }
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Rowspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oFontStyle = FontFactory.GetFont("Tahoma", 20f, 1);
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
            _oPdfPCell.Border = 0;  _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            #region ReportHeader
            #region Blank Space
            _oFontStyle = FontFactory.GetFont("Tahoma", 5f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion
            #endregion

            #endregion
        }
        private void ReportHeader_Detail( string sReportHead,string sOrderType, string sDate)
        {
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.SetWidths(new float[] { 170f,200f, 170f });
            #region 
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, sReportHead, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable,  sDate, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, sOrderType, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, FontFactory.GetFont("Tahoma", 11f, iTextSharp.text.Font.BOLDITALIC));
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Reporting Date:" + DateTime.Now.ToString("dd MMM yyyy hh:mm tt"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, false, 0, _oFontStyle);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            #endregion


        }
        private void SetSummary_Detail(List<DUHardWinding> oDUHardWindings)
        {
            int nSLNo = 0;
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            PdfPTable oPdfPTable = new PdfPTable(7);
            oPdfPTable.SetWidths(new float[] { 30f, 100f, 80f, 130f, 130f, 90f, 80f });
            
            #region Heder Info
            oPdfPTable = new PdfPTable(7);
            oPdfPTable.SetWidths(new float[] { 30f, 100f, 70f, 140f, 130f, 90f, 80f });
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "SL#", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Recd Date", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Order No", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Customer", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Yarn Type", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Batch/Lot No", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Qty", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);

            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            #endregion

            foreach (var oItem1 in oDUHardWindings)
            {
                nSLNo++;
                oPdfPTable = new PdfPTable(7);
                oPdfPTable.SetWidths(new float[] { 30f, 100f, 70f, 140f, 130f, 90f, 80f });
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, nSLNo.ToString(), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.ReceiveDate.ToString("dd MMM yy HH:mm"), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.DyeingOrderNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.ContractorName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.ProductName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.LotNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem1.Qty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
               
                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            }

            #region Heder Info
            oPdfPTable = new PdfPTable(7);
            oPdfPTable.SetWidths(new float[] { 30f, 100f, 70f, 140f, 130f, 90f, 80f });
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total", 0, 5, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oDUHardWindings.Sum(x => x.Qty)), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);

            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            #endregion
           

        }
        private void SetWURequsation(List<FabricExecutionOrderYarnReceive> oFabricExecutionOrderYarnReceives)
        {
            int nSLNo = 0;
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            PdfPTable oPdfPTable = new PdfPTable(6);
            oPdfPTable.SetWidths(new float[] { 30f,  80f, 130f, 130f, 90f, 80f });

            #region Heder Info
            oPdfPTable = new PdfPTable(6);
            oPdfPTable.SetWidths(new float[] { 30f,  70f, 140f, 130f, 90f, 80f });
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "SL#", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Recd Date", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Order No", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Customer", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Yarn Type", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Batch/Lot No", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "T/F", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);

            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            #endregion

            var data = oFabricExecutionOrderYarnReceives.GroupBy(x => new { x.FSCDID, x.DispoNo,x.WYRequisitionID }, (key, grp) => new
            {
                FSCDID = key.FSCDID,
                WYRequisitionID = key.WYRequisitionID,
                DispoNo = key.DispoNo,
                TFLength = grp.Max(x=>x.TFLength),
                Results = grp.ToList()
            });

            double nTotalTFLength = 0;
              int nFSCDID = -99,nWYRequisitionID=99;

              data = data.OrderBy(x => x.FSCDID).ThenBy(x => x.WYRequisitionID);

            foreach (var oData in data)
            {
              
                nSLNo++;
                oPdfPTable = new PdfPTable(6);
                oPdfPTable.SetWidths(new float[] { 30f, 70f, 140f, 130f, 90f, 80f });

                foreach (var oItem1 in oData.Results)
                {
                    if (nFSCDID != oData.FSCDID || nWYRequisitionID != oData.WYRequisitionID)//sDispoNo != oData.DispoNo
                    {
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, nSLNo.ToString(), oData.Results.Count(), 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, oData.DispoNo, oData.Results.Count(), 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    }
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.BuyerName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.ProductName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.LotNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    if (nFSCDID != oData.FSCDID || nWYRequisitionID != oData.WYRequisitionID)//sDispoNo != oData.DispoNo
                    {
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oData.TFLength), oData.Results.Count(), 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                        nTotalTFLength += oData.TFLength;
                    }
                    oPdfPTable.CompleteRow();
                    nFSCDID = oData.FSCDID;
                    nWYRequisitionID= oData.WYRequisitionID;
                    //sDispoNo = oData.DispoNo;
                }
                
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            }

            //foreach (var oItem1 in oFabricExecutionOrderYarnReceives)
            //{
            //    nSLNo++;
            //    oPdfPTable = new PdfPTable(6);
            //    oPdfPTable.SetWidths(new float[] { 30f, 70f, 140f, 130f, 90f, 80f });

            //    ESimSolItexSharp.SetCellValue(ref oPdfPTable, nSLNo.ToString(), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            //    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.DispoNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            //    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.BuyerName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            //    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.ProductName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            //    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.LotNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            //    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem1.TFLength), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);

            //    oPdfPTable.CompleteRow();
            //    ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            //}

            #region Footer Info
            oPdfPTable = new PdfPTable(6);
            oPdfPTable.SetWidths(new float[] { 30f, 70f, 140f, 130f, 90f, 80f });
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total", 0, 4, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);

            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oFabricExecutionOrderYarnReceives.Sum(x => x.TFLength)), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nTotalTFLength), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);

            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            #endregion


        }

        private void SetFabricBeamFinish()
        {
            int nSLNo = 0;
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            PdfPTable oPdfPTable = new PdfPTable(8);
            oPdfPTable.SetWidths(new float[] { 30f, 80f, 80f, 100f, 50f, 60f, 50f, 60f });

            #region Heder Info
            oPdfPTable = new PdfPTable(8);
            oPdfPTable.SetWidths(new float[] { 30f, 80f, 80f, 100f, 50f, 60f, 50f, 60f });
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "SL#", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Ready Date", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Order No", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Customer", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Count", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Ready Mtr.", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Beam No", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Balance", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);

            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            #endregion

            foreach (var oItem1 in _oFabricBeamFinishs)
            {
                nSLNo++;
                oPdfPTable = new PdfPTable(8);
                oPdfPTable.SetWidths(new float[] { 30f, 80f, 80f, 100f, 50f, 60f, 50f, 60f });
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, nSLNo.ToString(), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.ReadyDateInString, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.ExeNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.CustomerName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Count", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem1.LengthFinish), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.BeamNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);

                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            }

            #region Heder Info
            oPdfPTable = new PdfPTable(8);
            oPdfPTable.SetWidths(new float[] { 30f, 80f, 80f, 100f, 50f, 60f, 50f, 60f });
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total", 0, 4, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(_oFabricBeamFinishs.Sum(x => x.LengthFinish)), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);

            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            #endregion


        }

        private void SetFabricBeamFinishForBeamStock()
        {
            int nSLNo = 0;
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            PdfPTable oPdfPTable = new PdfPTable(9);
            oPdfPTable.SetWidths(new float[] { 25f, 60f, 70f, 130f, 40f, 60f, 60f, 60f, 60f });

            #region Heder Info
            oPdfPTable = new PdfPTable(9);
            oPdfPTable.SetWidths(new float[] { 25f, 60f, 70f, 130f, 40f, 60f, 60f, 60f, 60f });
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "SL#", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Ready Date", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Order No", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Customer", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Count", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Ready Mtr.", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Transfer Mtr.", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Beam No", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Balance", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);

            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            #endregion

            var data = _oFabricBeamFinishs.GroupBy(x => new { x.FEOSID }, (key, grp) => new
            {
                nFEOSID = key.FEOSID, 
                Results = grp.ToList()
            });
            double nTotalBalance = 0, nTotalTFlength = 0;
            int nFEOSID = 0;
            foreach (var oData in data)
            {
                oPdfPTable = new PdfPTable(9);
                oPdfPTable.SetWidths(new float[] { 25f, 60f, 70f, 130f, 40f, 60f, 60f, 60f, 60f });
                foreach (var oItem1 in oData.Results)
                {
                    nSLNo++;
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, nSLNo.ToString(), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.ReadyDateInString, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.ExeNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.CustomerName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Count", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem1.LengthFinish), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    if (nFEOSID != oData.nFEOSID)
                    {
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem1.TFlength), oData.Results.Count, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                        nTotalTFlength += oItem1.TFlength;
                    }
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.BeamNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    if (nFEOSID != oData.nFEOSID)
                    {
                        double nBalance = _oFabricBeamFinishs.Where(x => x.FEOSID == oData.nFEOSID).Sum(y => y.LengthFinish) - oItem1.TFlength;
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nBalance), oData.Results.Count, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                        nTotalBalance += nBalance;
                    }
                    oPdfPTable.CompleteRow();
                    nFEOSID = oData.nFEOSID;
                }

                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
                
            }

            //int nFEOSID = -999;
            //double nTotalBalance = 0, nTotalTFlength = 0;
            //_oFabricBeamFinishs = _oFabricBeamFinishs.OrderBy(x => x.FEOSID).ToList();
            //foreach (var oItem1 in _oFabricBeamFinishs)
            //{
            //    nSLNo++;
            //    if (oItem1.FEOSID != nFEOSID)
            //    {
            //        oPdfPTable = new PdfPTable(9);
            //        oPdfPTable.SetWidths(new float[] { 25f, 60f, 70f, 130f, 40f, 60f, 60f, 60f, 60f });
            //    }
                
            //    ESimSolItexSharp.SetCellValue(ref oPdfPTable, nSLNo.ToString(), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            //    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.ReadyDateInString, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            //    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.ExeNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            //    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.CustomerName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            //    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Count", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            //    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem1.LengthFinish), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
            //    if (oItem1.FEOSID != nFEOSID)
            //    {
            //        int nRowCount = _oFabricBeamFinishs.Where(x => x.FEOSID == oItem1.FEOSID).Count();

            //        //_oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem1.TFlength), _oFontStyle)); _oPdfPCell.Rowspan = nRowCount;
            //        //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


            //        ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem1.TFlength), nRowCount, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
            //        nTotalTFlength += oItem1.TFlength;
            //    }
                
            //    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.BeamNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            //    if (oItem1.FEOSID != nFEOSID)
            //    {
            //        double nBalance = _oFabricBeamFinishs.Where(x => x.FEOSID == oItem1.FEOSID).Sum(y => y.LengthFinish) - oItem1.TFlength;
            //        int nRowCount = _oFabricBeamFinishs.Where(x => x.FEOSID == oItem1.FEOSID).Count();

            //        //_oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nBalance), _oFontStyle)); _oPdfPCell.Rowspan = nRowCount;
            //        //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //        ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nBalance), nRowCount, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
            //        nTotalBalance += nBalance;
            //    }

                
            //    oPdfPTable.CompleteRow();
            //    if (oItem1.FEOSID != nFEOSID && nFEOSID != -999)
            //    ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            //    nFEOSID = oItem1.FEOSID;
            //}

            //nTotalTFlength += _oFabricBeamFinishs[_oFabricBeamFinishs.Count - 1].TFlength;
            //nTotalBalance += _oFabricBeamFinishs.Where(x => x.FEOSID == _oFabricBeamFinishs[_oFabricBeamFinishs.Count - 1].FEOSID).Sum(y => y.LengthFinish) - _oFabricBeamFinishs[_oFabricBeamFinishs.Count - 1].TFlength;
            //ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

            #region Heder Info
            oPdfPTable = new PdfPTable(9);
            oPdfPTable.SetWidths(new float[] { 25f, 60f, 70f, 130f, 40f, 60f, 60f, 60f, 60f });
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total", 0, 4, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(_oFabricBeamFinishs.Sum(x => x.LengthFinish)), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(_oFabricBeamFinishs.Sum(x => x.TFlength)), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nTotalTFlength), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nTotalBalance), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);

            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            #endregion


        }

        private void SetWURequsationLeftOver(List<FabricExecutionOrderYarnReceive> oFabricExecutionOrderYarnReceives)
        {
            int nSLNo = 0;
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            PdfPTable oPdfPTable = new PdfPTable(6);
            oPdfPTable.SetWidths(new float[] { 30f, 80f, 130f, 130f, 90f, 80f });

            #region Heder Info
            oPdfPTable = new PdfPTable(6);
            oPdfPTable.SetWidths(new float[] { 30f, 70f, 140f, 130f, 90f, 80f });
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "SL#", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Recd Date", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Order No", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Customer", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Yarn Type", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Batch/Lot No", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Qty(kg)", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);

            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            #endregion

            var data = oFabricExecutionOrderYarnReceives.GroupBy(x => new { x.FSCDID, x.DispoNo, x.WYRequisitionID }, (key, grp) => new
            {
                FSCDID = key.FSCDID,
                WYRequisitionID = key.WYRequisitionID,
                DispoNo = key.DispoNo,
                TFLength = grp.Max(x => x.TFLength),
                Results = grp.ToList()
            });

            double nTotalTFLength = 0;
            int nFSCDID = -99, nWYRequisitionID = 99;

            data = data.OrderBy(x => x.FSCDID).ThenBy(x => x.WYRequisitionID);

            foreach (var oData in data)
            {
               
                nSLNo++;
                oPdfPTable = new PdfPTable(6);
                oPdfPTable.SetWidths(new float[] { 30f, 70f, 140f, 130f, 90f, 80f });

                foreach (var oItem1 in oData.Results)
                {
                    if (nFSCDID != oData.FSCDID || nWYRequisitionID != oData.WYRequisitionID)//sDispoNo != oData.DispoNo
                    {
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, nSLNo.ToString(), oData.Results.Count(), 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, oData.DispoNo, oData.Results.Count(), 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    }
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.BuyerName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.ProductName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.LotNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem1.ReceiveQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    //if (nFSCDID != oData.FSCDID || nWYRequisitionID != oData.WYRequisitionID)//sDispoNo != oData.DispoNo
                    //{
                    //    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oData.TFLength), oData.Results.Count(), 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    nTotalTFLength += oItem1.ReceiveQty;
                    //}
                    oPdfPTable.CompleteRow();
                    nFSCDID = oData.FSCDID;
                    nWYRequisitionID = oData.WYRequisitionID;
                    //sDispoNo = oData.DispoNo;
                }

                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            }

            //foreach (var oItem1 in oFabricExecutionOrderYarnReceives)
            //{
            //    nSLNo++;
            //    oPdfPTable = new PdfPTable(6);
            //    oPdfPTable.SetWidths(new float[] { 30f, 70f, 140f, 130f, 90f, 80f });

            //    ESimSolItexSharp.SetCellValue(ref oPdfPTable, nSLNo.ToString(), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            //    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.DispoNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            //    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.BuyerName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            //    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.ProductName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            //    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.LotNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            //    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem1.TFLength), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);

            //    oPdfPTable.CompleteRow();
            //    ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            //}

            #region Footer Info
            oPdfPTable = new PdfPTable(6);
            oPdfPTable.SetWidths(new float[] { 30f, 70f, 140f, 130f, 90f, 80f });
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total", 0, 4, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);

            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oFabricExecutionOrderYarnReceives.Sum(x => x.TFLength)), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nTotalTFLength), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);

            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            #endregion


        }
    }
}
