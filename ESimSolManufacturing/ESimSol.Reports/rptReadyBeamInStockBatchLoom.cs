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
    public class rptReadyBeamInStockBatchLoom
    {
        #region Declaration
        int _nTotalColumn = 2;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyleBold;
        PdfPTable _oPdfPTable = new PdfPTable(2);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        List<FabricBatchLoom> _oFabricBatchLooms = new List<FabricBatchLoom>();
        Company _oCompany = new Company();
        List<TextileSubUnit> _oTextileSubUnits = new List<TextileSubUnit>();
        List<FabricMachine> _oFabricMachines = new List<FabricMachine>();
        double _nTotalEnds = 0;
        int _nStopLooms = 0;
        int _nCount = 1, nItem = 0, nSide = 1;
        #endregion

        public byte[] PrepareReport(List<FabricBatchLoom> oFabricBatchLooms, double nTotalEnds, Company oCompany, List<TextileSubUnit> oTextileSubUnits, List<FabricMachine> oFabricMachines)
        {
            _oFabricBatchLooms = oFabricBatchLooms;
            _oCompany = oCompany;
            _nTotalEnds = nTotalEnds;
            _oTextileSubUnits = oTextileSubUnits;
            //_nStopLooms = nStopLooms;
            _oFabricMachines = oFabricMachines;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            _oDocument.SetMargins(10f, 10f, 15f, 15f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            PdfWriter PdfWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            //ESimSolFooter PageEventHandler = new ESimSolFooter();
            //PageEventHandler.nFontSize = 9;
            //PdfWriter.PageEvent = PageEventHandler; 
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 297f, 297f });
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
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));

            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.PringReportHead, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Colspan = 2; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            #region ReportHeader
            #region Blank Space
            _oFontStyle = FontFactory.GetFont("Tahoma", 5f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Colspan = 2;
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion
            #endregion

            #endregion
        }
        private void PrintHeaderDetail(string sReportHead, string sOrderType, string sDate)
        {
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.SetWidths(new float[] { 170f, 200f, 170f });
            #region
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, sReportHead, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, sDate, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, sOrderType, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, FontFactory.GetFont("Tahoma", 11f, iTextSharp.text.Font.BOLDITALIC));
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Reporting Date:" + DateTime.Now.ToString("dd MMM yyyy hh:mm tt"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, false, 0, _oFontStyle);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            #endregion


        }
        #endregion

        #region Report Body
        private void PrintBody()
        {
            this.PrintHeaderDetail("Daily Shed Position Report", " ", " ");
            this.SetData();
        }
        #endregion

        private void SetData()
        {
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);

            var data = _oFabricBatchLooms.GroupBy(x => new { x.BuyerID, x.BuyerName, x.FSCDID, x.FEONo }, (key, grp) => new //x.FMID, x.FabricMachineName
                {
                    BuyerID = key.BuyerID,
                    BuyerName = key.BuyerName,
                    FSCDID = key.FSCDID,
                    FEONo = key.FEONo,
                    //FMID = key.FMID,
                    //FabricMachineName = key.FabricMachineName,
                    Results = grp.ToList()
                }).ToList();

            int nRowCount = 0, nGrantedRow = 52;
            int n = 0, nMasterRow = 0;
            string sBuyerName = "";
            PdfPTable oFirstTable = new PdfPTable(10);
            oFirstTable.WidthPercentage = 100;
            //foreach (var oData in data)
            while ( n < data.Count())
            {
                //PdfPTable oFirstTable = new PdfPTable(10);
                //oFirstTable.WidthPercentage = 100;
                oFirstTable.SetWidths(new float[] { 25f, 15f, 7.5f, 7.5f, 7.5f, 7.5f, 7.5f, 7.5f, 7.5f, 8f });
                //int nSpan = (data[n].Results.Count / 7) + 1;
                double nRes = (data[n].Results.Count / 7.00);
                int nSpan = Convert.ToInt32(Math.Ceiling(nRes));
                if ((nRowCount + nSpan + 1) <= nGrantedRow) //******
                {
                    if (nRowCount == 0)
                    {
                        #region header
                        _oPdfPCell = new PdfPCell(new Phrase("Customer/Quality", _oFontStyleBold));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oFirstTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("Dispo No", _oFontStyleBold));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oFirstTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("Loom No", _oFontStyleBold)); _oPdfPCell.Colspan = 7;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oFirstTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyleBold));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oFirstTable.AddCell(_oPdfPCell);

                        oFirstTable.CompleteRow();
                        #endregion
                    }

                    //nRowCount += nSpan + 1;   //******
                    nRowCount += nSpan;

                    if (sBuyerName != data[n].BuyerName)    //******
                    {
                        _oPdfPCell = new PdfPCell(new Phrase(data[n].BuyerName, _oFontStyleBold)); _oPdfPCell.Colspan = oFirstTable.NumberOfColumns;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oFirstTable.AddCell(_oPdfPCell);
                        oFirstTable.CompleteRow();
                        nRowCount++;    //******                        
                    }

                    _oPdfPCell = new PdfPCell(new Phrase(data[n].Results[0].Construction, _oFontStyle)); _oPdfPCell.Rowspan = nSpan; 
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oFirstTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(data[n].FEONo, _oFontStyle)); _oPdfPCell.Rowspan = nSpan; 
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oFirstTable.AddCell(_oPdfPCell);

                    #region loom Print
                    int nCount = 0;
                    for (int i = 0; i < data[n].Results.Count; i++)
                    {
                        if (nCount == 7)
                        {
                            _oPdfPCell = new PdfPCell(new Phrase("7", _oFontStyle)); 
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; oFirstTable.AddCell(_oPdfPCell);
                            nCount = 0;
                        }

                        if (data[n].Results[i].RemainingQty <= _nTotalEnds)
                        {
                            _oPdfPCell = new PdfPCell(new Phrase(data[n].Results[i].MachineCode, _oFontStyle)); _oPdfPCell.BackgroundColor = (new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ff8080"))); 
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; oFirstTable.AddCell(_oPdfPCell);
                        }
                        else
                        {
                            _oPdfPCell = new PdfPCell(new Phrase(data[n].Results[i].MachineCode, _oFontStyle)); _oPdfPCell.BackgroundColor = BaseColor.WHITE; 
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; oFirstTable.AddCell(_oPdfPCell);
                        }
                        nCount++;
                    }
                    if (nCount == 7)
                    {
                        _oPdfPCell = new PdfPCell(new Phrase("7", _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; oFirstTable.AddCell(_oPdfPCell);
                    }
                    else if (nCount < 7)
                    {
                        int a = nCount;
                        while (nCount != 7)
                        {
                            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle)); 
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oFirstTable.AddCell(_oPdfPCell);
                            nCount++;
                        }
                        _oPdfPCell = new PdfPCell(new Phrase(a.ToString(), _oFontStyle)); 
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; oFirstTable.AddCell(_oPdfPCell);
                    }
                    #endregion

                    sBuyerName = data[n].BuyerName;
                    n++;                    
                }
                else
                {
                    while (nRowCount != nGrantedRow)
                    {
                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle)); _oPdfPCell.Colspan = oFirstTable.NumberOfColumns; 
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oFirstTable.AddCell(_oPdfPCell);
                        oFirstTable.CompleteRow();
                        nRowCount++;
                    }
                }

                if (nRowCount == nGrantedRow)
                {
                    nMasterRow++;
                    #region push into main table
                    _oPdfPCell = new PdfPCell(oFirstTable); _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Border = 0;
                    _oPdfPTable.AddCell(_oPdfPCell);
                    #endregion
                    if (nMasterRow % 2 == 0)
                    {
                        _oPdfPTable.CompleteRow();

                        #region footer
                        _oPdfPCell = new PdfPCell(new Phrase("Running Looms: " + _oFabricBatchLooms.Count().ToString("00"), _oFontStyleBold)); 
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = 15; _oPdfPCell.Colspan = 2; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();

                        foreach (TextileSubUnit oTU in _oTextileSubUnits)
                        {
                            //_oPdfPCell = new PdfPCell(new Phrase(oTU.Name + ": " + _oFabricBatchLooms.Where(x => x.TSUID == oTU.TSUID).Count().ToString("00"), _oFontStyleBold)); 
                            _oPdfPCell = new PdfPCell(new Phrase(oTU.Name + ": " + _oFabricMachines.Where(x => x.TSUID == oTU.TSUID).Count().ToString("00"), _oFontStyleBold));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = 15; _oPdfPCell.Colspan = 2; _oPdfPTable.AddCell(_oPdfPCell);
                            _oPdfPTable.CompleteRow();
                        }

                        //_oPdfPCell = new PdfPCell(new Phrase("STOP LOOMS: " + _nStopLooms.ToString("00"), _oFontStyleBold)); 
                        _oPdfPCell = new PdfPCell(new Phrase("STOP LOOMS: " + (_oFabricMachines.Count() - _oFabricBatchLooms.Count()).ToString("00"), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = 15; _oPdfPCell.Colspan = 2; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();

                        //_oPdfPCell = new PdfPCell(new Phrase("TOTAL LOOMS: " + (_oFabricBatchLooms.Count() + _nStopLooms).ToString("00"), _oFontStyleBold)); 
                        _oPdfPCell = new PdfPCell(new Phrase("TOTAL LOOMS: " + _oFabricMachines.Count().ToString("00"), _oFontStyleBold)); 
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = 15; _oPdfPCell.Colspan = 2; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();
                        #endregion

                        #region New Page Declare
                        _oPdfPTable.HeaderRows = 3;
                        _oDocument.Add(_oPdfPTable);
                        _oDocument.NewPage();
                        _oPdfPTable.DeleteBodyRows();
                        #endregion
                    }

                    nRowCount = 0;
                    oFirstTable = new PdfPTable(10);
                    oFirstTable.WidthPercentage = 100;
                }

            }

            if (nRowCount != nGrantedRow)
            {
                #region push into main table
                if (nMasterRow % 2 != 0)
                {
                    while (nRowCount != nGrantedRow)
                    {
                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle)); _oPdfPCell.Colspan = oFirstTable.NumberOfColumns; _oPdfPCell.Border = 0;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oFirstTable.AddCell(_oPdfPCell);
                        oFirstTable.CompleteRow();
                        nRowCount++;
                    }
                }
                _oPdfPCell = new PdfPCell(oFirstTable); _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Border = 0;
                _oPdfPTable.AddCell(_oPdfPCell);
                if (nMasterRow % 2 == 0)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle)); _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Border = 0;
                    _oPdfPTable.AddCell(_oPdfPCell);
                }
                _oPdfPTable.CompleteRow();
                //_oDocument.Add(_oPdfPTable);
                #endregion

                #region footer
                _oPdfPCell = new PdfPCell(new Phrase("Running Looms: " + _oFabricBatchLooms.Count().ToString("00"), _oFontStyleBold)); 
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = 15; _oPdfPCell.Colspan = 2; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                foreach (TextileSubUnit oTU in _oTextileSubUnits)
                {
                    //_oPdfPCell = new PdfPCell(new Phrase(oTU.Name + ": " + _oFabricBatchLooms.Where(x => x.TSUID == oTU.TSUID).Count().ToString("00"), _oFontStyleBold)); 
                    _oPdfPCell = new PdfPCell(new Phrase(oTU.Name + ": " + _oFabricMachines.Where(x => x.TSUID == oTU.TSUID).Count().ToString("00"), _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = 15; _oPdfPCell.Colspan = 2; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                }

                //_oPdfPCell = new PdfPCell(new Phrase("STOP LOOMS: " + _nStopLooms.ToString("00"), _oFontStyleBold)); 
                _oPdfPCell = new PdfPCell(new Phrase("STOP LOOMS: " + (_oFabricMachines.Count() - _oFabricBatchLooms.Count()).ToString("00"), _oFontStyleBold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = 15; _oPdfPCell.Colspan = 2; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                //_oPdfPCell = new PdfPCell(new Phrase("TOTAL LOOMS: " + (_oFabricBatchLooms.Count() + _nStopLooms).ToString("00"), _oFontStyleBold)); 
                _oPdfPCell = new PdfPCell(new Phrase("TOTAL LOOMS: " + _oFabricMachines.Count().ToString("00"), _oFontStyleBold)); 
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = 15; _oPdfPCell.Colspan = 2; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

            }




            #region Previous Code
            //while (nItem < data.Count())
            //{
            //    if (nSide % 2 != 0)
            //    {
            //        PdfPTable oFirstTable = new PdfPTable(10);
            //        oFirstTable.WidthPercentage = 100;
            //        oFirstTable.SetWidths(new float[] { 25f, 15f, 7.5f, 7.5f, 7.5f, 7.5f, 7.5f, 7.5f, 7.5f, 8f });

            //        #region header
            //        _oPdfPCell = new PdfPCell(new Phrase("Customer/Quality", _oFontStyleBold));
            //        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oFirstTable.AddCell(_oPdfPCell);

            //        _oPdfPCell = new PdfPCell(new Phrase("Dispo No", _oFontStyleBold));
            //        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oFirstTable.AddCell(_oPdfPCell);

            //        _oPdfPCell = new PdfPCell(new Phrase("Loom No", _oFontStyleBold)); _oPdfPCell.Colspan = 7;
            //        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oFirstTable.AddCell(_oPdfPCell);

            //        _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyleBold));
            //        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oFirstTable.AddCell(_oPdfPCell);

            //        oFirstTable.CompleteRow();
            //        #endregion

            //        while (_nCount <= 50 && nItem < data.Count())
            //        {
            //            int nSpan = (data[nItem].Results.Count / 7) + 1;
            //            #region NaAtle
            //            if ((_nCount + nSpan + 1) > 50)
            //            {
            //                while (_nCount <= 50)
            //                {
            //                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            //                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oFirstTable.AddCell(_oPdfPCell);
            //                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            //                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oFirstTable.AddCell(_oPdfPCell);
            //                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            //                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oFirstTable.AddCell(_oPdfPCell);
            //                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            //                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oFirstTable.AddCell(_oPdfPCell);
            //                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            //                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oFirstTable.AddCell(_oPdfPCell);
            //                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            //                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oFirstTable.AddCell(_oPdfPCell);
            //                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            //                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oFirstTable.AddCell(_oPdfPCell);
            //                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            //                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oFirstTable.AddCell(_oPdfPCell);
            //                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            //                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oFirstTable.AddCell(_oPdfPCell);
            //                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            //                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oFirstTable.AddCell(_oPdfPCell);
            //                    oFirstTable.CompleteRow();

            //                }
            //                break;
            //            }
            //            #endregion
            //            #region 1st
            //            _oPdfPCell = new PdfPCell(new Phrase(data[nItem].BuyerName, _oFontStyleBold)); _oPdfPCell.Colspan = oFirstTable.NumberOfColumns;
            //            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oFirstTable.AddCell(_oPdfPCell);
            //            oFirstTable.CompleteRow();
            //            _nCount++;


            //            #region Data
            //            _oPdfPCell = new PdfPCell(new Phrase(data[nItem].Results[0].Construction, _oFontStyle)); _oPdfPCell.Rowspan = nSpan;
            //            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oFirstTable.AddCell(_oPdfPCell);

            //            _oPdfPCell = new PdfPCell(new Phrase(data[nItem].FEONo, _oFontStyle)); _oPdfPCell.Rowspan = nSpan;
            //            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oFirstTable.AddCell(_oPdfPCell);

            //            int n = 0;
            //            for (int i = 0; i < data[nItem].Results.Count; i++)
            //            {
            //                if (n == 7)
            //                {
            //                    _oPdfPCell = new PdfPCell(new Phrase("7", _oFontStyle));
            //                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; oFirstTable.AddCell(_oPdfPCell);
            //                    n = 0;
            //                }
            //                if(data[nItem].Results[i].RemainingQty <= _nTotalEnds)
            //                {
            //                    _oPdfPCell = new PdfPCell(new Phrase(data[nItem].Results[i].MachineCode, _oFontStyle)); _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            //                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; oFirstTable.AddCell(_oPdfPCell);
            //                }
            //                else
            //                {
            //                    _oPdfPCell = new PdfPCell(new Phrase(data[nItem].Results[i].MachineCode, _oFontStyle)); _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; oFirstTable.AddCell(_oPdfPCell);
            //                }
            //                n++;
            //            }
            //            if (n < 7)
            //            {
            //                int a = n;
            //                while (n != 7)
            //                {
            //                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            //                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oFirstTable.AddCell(_oPdfPCell);
            //                    n++;
            //                }
            //                _oPdfPCell = new PdfPCell(new Phrase(a.ToString(), _oFontStyle));
            //                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; oFirstTable.AddCell(_oPdfPCell);
            //            }

            //            _nCount += nSpan;
            //            #endregion

            //            nItem++;
            //            #endregion
            //        }
                    
            //        nSide++;
            //        #region push into main table
            //        _oPdfPCell = new PdfPCell(oFirstTable); _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Border = 0;
            //        _oPdfPTable.AddCell(_oPdfPCell);
            //        #endregion
            //    }
            //    else
            //    {
            //        PdfPTable oFirstTable = new PdfPTable(10);
            //        oFirstTable.WidthPercentage = 100;
            //        oFirstTable.SetWidths(new float[] { 25f, 15f, 7.5f, 7.5f, 7.5f, 7.5f, 7.5f, 7.5f, 7.5f, 8f });

            //        #region header
            //        _oPdfPCell = new PdfPCell(new Phrase("Customer/Quality", _oFontStyleBold));
            //        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oFirstTable.AddCell(_oPdfPCell);

            //        _oPdfPCell = new PdfPCell(new Phrase("Dispo No", _oFontStyleBold));
            //        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oFirstTable.AddCell(_oPdfPCell);

            //        _oPdfPCell = new PdfPCell(new Phrase("Loom No", _oFontStyleBold)); _oPdfPCell.Colspan = 7;
            //        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oFirstTable.AddCell(_oPdfPCell);

            //        _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyleBold));
            //        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oFirstTable.AddCell(_oPdfPCell);

            //        oFirstTable.CompleteRow();
            //        #endregion

            //        while (_nCount <= 50 && nItem < data.Count())
            //        {
            //            int nSpan = (data[nItem].Results.Count / 7) + 1;
            //            #region NaAtle
            //            if ((_nCount + nSpan + 1) > 50)
            //            {
            //                while (_nCount <= 50)
            //                {
            //                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            //                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oFirstTable.AddCell(_oPdfPCell);
            //                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            //                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oFirstTable.AddCell(_oPdfPCell);
            //                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            //                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oFirstTable.AddCell(_oPdfPCell);
            //                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            //                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oFirstTable.AddCell(_oPdfPCell);
            //                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            //                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oFirstTable.AddCell(_oPdfPCell);
            //                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            //                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oFirstTable.AddCell(_oPdfPCell);
            //                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            //                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oFirstTable.AddCell(_oPdfPCell);
            //                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            //                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oFirstTable.AddCell(_oPdfPCell);
            //                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            //                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oFirstTable.AddCell(_oPdfPCell);
            //                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            //                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oFirstTable.AddCell(_oPdfPCell);
            //                    oFirstTable.CompleteRow();

            //                }
            //                break;
            //            }
            //            #endregion
            //            #region 1st
            //            _oPdfPCell = new PdfPCell(new Phrase(data[nItem].BuyerName, _oFontStyleBold)); _oPdfPCell.Colspan = oFirstTable.NumberOfColumns;
            //            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oFirstTable.AddCell(_oPdfPCell);
            //            oFirstTable.CompleteRow();
            //            _nCount++;


            //            #region Data
            //            _oPdfPCell = new PdfPCell(new Phrase(data[nItem].Results[0].Construction, _oFontStyle)); _oPdfPCell.Rowspan = nSpan;
            //            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oFirstTable.AddCell(_oPdfPCell);

            //            _oPdfPCell = new PdfPCell(new Phrase(data[nItem].FEONo, _oFontStyle)); _oPdfPCell.Rowspan = nSpan;
            //            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oFirstTable.AddCell(_oPdfPCell);

            //            int n = 0;
            //            for (int i = 0; i < data[nItem].Results.Count; i++)
            //            {
            //                if (n == 7)
            //                {
            //                    _oPdfPCell = new PdfPCell(new Phrase("7", _oFontStyle));
            //                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; oFirstTable.AddCell(_oPdfPCell);
            //                    n = 0;
            //                }
            //                _oPdfPCell = new PdfPCell(new Phrase(data[nItem].Results[i].MachineCode, _oFontStyle));
            //                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; oFirstTable.AddCell(_oPdfPCell);
            //                n++;
            //            }
            //            if (n < 7)
            //            {
            //                int a = n;
            //                while (n != 7)
            //                {
            //                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            //                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oFirstTable.AddCell(_oPdfPCell);
            //                    n++;
            //                }
            //                _oPdfPCell = new PdfPCell(new Phrase(a.ToString(), _oFontStyle));
            //                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; oFirstTable.AddCell(_oPdfPCell);
            //            }

            //            _nCount += nSpan;
            //            #endregion

            //            nItem++;
            //            #endregion
            //        }

            //        nSide++;
            //        #region push into main table
            //        _oPdfPCell = new PdfPCell(oFirstTable); _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Border = 0;
            //        _oPdfPTable.AddCell(_oPdfPCell);
            //        #endregion
            //        _oPdfPTable.CompleteRow();
            //    }
            //    if (nItem == data.Count())
            //    {
            //        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle)); _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Border = 0;
            //        _oPdfPTable.AddCell(_oPdfPCell);
            //        _oPdfPTable.CompleteRow();
            //    }

            //    #region footer
            //    _oPdfPCell = new PdfPCell(new Phrase("Running Looms: " + _oFabricBatchLooms.Count().ToString("00"), _oFontStyle)); _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //    _oPdfPCell.Border = 15; _oPdfPCell.Colspan = 2;
            //    _oPdfPTable.AddCell(_oPdfPCell);
            //    _oPdfPTable.CompleteRow();

            //    foreach (TextileSubUnit oTU in _oTextileSubUnits)
            //    {
            //        _oPdfPCell = new PdfPCell(new Phrase(oTU.Name + ": " + _oFabricBatchLooms.Where(x=>x.TSUID == oTU.TSUID).Count().ToString("00"), _oFontStyle)); _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //        _oPdfPCell.Border = 15; _oPdfPCell.Colspan = 2;
            //        _oPdfPTable.AddCell(_oPdfPCell);
            //        _oPdfPTable.CompleteRow();
            //    }

            //    _oPdfPCell = new PdfPCell(new Phrase("STOP LOOMS: " + _nStopLooms.ToString("00"), _oFontStyle)); _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //    _oPdfPCell.Border = 15; _oPdfPCell.Colspan = 2;
            //    _oPdfPTable.AddCell(_oPdfPCell);
            //    _oPdfPTable.CompleteRow();

            //    _oPdfPCell = new PdfPCell(new Phrase("TOTAL LOOMS: " + (_oFabricBatchLooms.Count() + _nStopLooms).ToString("00"), _oFontStyle)); _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //    _oPdfPCell.Border = 15; _oPdfPCell.Colspan = 2;
            //    _oPdfPTable.AddCell(_oPdfPCell);
            //    _oPdfPTable.CompleteRow();
            //    #endregion

            //    #region New Page Declare
            //    _oDocument.Add(_oPdfPTable);
            //    _oDocument.NewPage();
            //    _oPdfPTable.DeleteBodyRows();
            //    #endregion
            //}
            #endregion  //Previous Code

            
            



        }

        
    }
}
