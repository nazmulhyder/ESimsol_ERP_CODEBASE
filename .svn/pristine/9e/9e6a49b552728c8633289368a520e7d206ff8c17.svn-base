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
    public class rptFabricPattern
    {
        #region Declaration

        int _nColumn = 9;

        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(9);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        FabricPattern _oFabricPattern = new FabricPattern();
        Company _oCompany = new Company();
        int _nGapBetweenCells = 5;
        int _nWarpTotalRow = 0;
        int _nWeftTotalRow = 0;
        bool _bIsDeisplyPattern = true;

        List<FabricPatternDetail> _oWarps = new List<FabricPatternDetail>();
        List<FabricPatternDetail> _oWefts = new List<FabricPatternDetail>();
        BaseColor _oBC = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#EFF0EA"));
        BusinessUnit _oBusinessUnit = new BusinessUnit();
        #endregion

        public byte[] PrepareReport(FabricPattern oFabricPattern, Company oCompany, BusinessUnit oBU, bool bIsDeisplyPattern)
        {
            _oFabricPattern = oFabricPattern;
            _oCompany = oCompany;
            _oBusinessUnit= oBU;
            _bIsDeisplyPattern = bIsDeisplyPattern;

            #region Page Setup
            //_oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            //_oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            //_oDocument.SetMargins(10f, 10f, 10f, 10f);
            //_oPdfPTable.WidthPercentage = 100;
            //_oPdfPTable.HorizontalAlignment = Element.ALIGN_CENTER;

            //_oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            //PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            //PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            //ESimSolFooter PageEventHandler = new ESimSolFooter();
            //PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            //_oDocument.Open();

            _oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
            _oDocument.SetMargins(30f, 30f, 30f, 30f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();

            _oPdfPTable.SetWidths(new float[] { 100f, 100f, 60f, 60f, 30f, 100f, 100f, 60f, 60f });
            #endregion

            this.PrintHeader();
            this.ReporttHeader();
            this.PrintBody();
            //_oPdfPTable.HeaderRows = 5;
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
            _oPdfPCell.Colspan = _nColumn;
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();


            #region ReportHeader
            #region Blank Space
            _oFontStyle = FontFactory.GetFont("Tahoma", 5f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Colspan = _nColumn;
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion
            #endregion

            #endregion
        }
        private void ReporttHeader()
        {
            string sHeaderName = (_bIsDeisplyPattern == true) ? "Pattern Paper" : "Desk Loom Sample";

            _oPdfPCell = new PdfPCell(new Phrase(sHeaderName, FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD)));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Colspan = _nColumn;
            _oPdfPCell.Border = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase(DateTime.Now.ToString("dd-MM-yyyy hh:mm tt"), FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL)));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Colspan = _nColumn;
            _oPdfPCell.Border = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

        }
        #endregion

        #region Report Body
        private void PrintBody()
        {
            _oPdfPCell = new PdfPCell(this.InformationPart());
            _oPdfPCell.Colspan = _nColumn; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            if (_bIsDeisplyPattern)
            {
                this.Blank(25);

                #region Detail
                if (_oFabricPattern.FPDetails.Count > 0)
                {
                    _oWarps = _oFabricPattern.FPDetails.Where(o => o.IsWarp == true).ToList();
                    _oWefts = _oFabricPattern.FPDetails.Where(o => o.IsWarp == false).ToList();

                    this.CountRowsNumber();

                    _oPdfPCell = new PdfPCell(this.Pattern(_oWarps, true));
                    _oPdfPCell.Colspan = 4;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.Border = 0;
                    _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.Border = 0;
                    _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(this.Pattern(_oWefts, false));
                    _oPdfPCell.Colspan = 4;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.Border = 0;
                    _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();


                    #region Grand Total Ends

                    //int nSumWarp = _oWarps.Sum(x => x.EndsCount);
                    //if (nSumWarp > 0)
                    //{
                    //    _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
                    //    _oPdfPCell = new PdfPCell(new Phrase("Total Ends:  ", _oFontStyle));
                    //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //    _oPdfPTable.AddCell(_oPdfPCell);

                    //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //    _oPdfPTable.AddCell(_oPdfPCell);

                    //    _oPdfPCell = new PdfPCell(new Phrase(nSumWarp.ToString(), _oFontStyle));
                    //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //    _oPdfPTable.AddCell(_oPdfPCell);

                    //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //    _oPdfPTable.AddCell(_oPdfPCell);
                    //}
                    //else
                    //{
                    //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //    _oPdfPCell.Border = 0;
                    //    _oPdfPCell.Colspan = 4;
                    //    _oPdfPTable.AddCell(_oPdfPCell);
                    //}

                    //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //_oPdfPCell.Border = 0;
                    //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //_oPdfPTable.AddCell(_oPdfPCell);

                    //int nSumWeft = _oWefts.Sum(x => x.EndsCount);
                    //if (nSumWeft > 0)
                    //{
                    //    _oPdfPCell = new PdfPCell(new Phrase("Total Picks:  ", _oFontStyle));
                    //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //    _oPdfPTable.AddCell(_oPdfPCell);

                    //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //    _oPdfPTable.AddCell(_oPdfPCell);

                    //    _oPdfPCell = new PdfPCell(new Phrase(nSumWeft.ToString(), _oFontStyle));
                    //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //    _oPdfPTable.AddCell(_oPdfPCell);

                    //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //    _oPdfPTable.AddCell(_oPdfPCell);
                    //}
                    //else
                    //{
                    //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //    _oPdfPCell.Border = 0;
                    //    _oPdfPCell.Colspan = 4;
                    //    _oPdfPTable.AddCell(_oPdfPCell);
                    //}

                    //_oPdfPTable.CompleteRow();
                    #endregion
                }
            }
            #endregion
        }
        #endregion
        private void CountRowsNumber()
        {
            int nGroupNo = 0;
            #region Warp
            foreach (FabricPatternDetail oItem in _oWarps)
            {
                if (oItem.GroupNo == 0)
                {
                    _nWarpTotalRow++;
                }
                else if (oItem.GroupNo != nGroupNo)
                {
                    _nWarpTotalRow++;
                    nGroupNo = oItem.GroupNo;
                }
            }
            #endregion

            nGroupNo = 0;
            #region Weft
            foreach (FabricPatternDetail oItem in _oWefts)
            {
                if (oItem.GroupNo == 0)
                {
                    _nWeftTotalRow++;
                }
                else if (oItem.GroupNo != nGroupNo)
                {
                    _nWeftTotalRow++;
                    nGroupNo = oItem.GroupNo;
                }
            }
            #endregion

        }
        private PdfPTable AllPatterns()
        {
            PdfPTable oPdfPTable2 = new PdfPTable(3);
            oPdfPTable2.SetWidths(new float[] { 100f, 30f, 100f });
            oPdfPTable2.WidthPercentage = 100;

            _oWarps = _oFabricPattern.FPDetails.Where(o => o.IsWarp == true).ToList();
            _oWefts = _oFabricPattern.FPDetails.Where(o => o.IsWarp == false).ToList();

            _oPdfPCell = new PdfPCell(this.Pattern(_oWarps, true));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            oPdfPTable2.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            oPdfPTable2.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(this.Pattern(_oWefts, false));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            oPdfPTable2.AddCell(_oPdfPCell);
            oPdfPTable2.CompleteRow();

            return oPdfPTable2;
        }
        private PdfPTable Pattern(List<FabricPatternDetail> oFPDs, bool bIsWarp)
        {
            PdfPTable oPdfPTable1 = new PdfPTable(4);
            oPdfPTable1.SetWidths(new float[] { 100f, 100f, 60f, 60f });
            oPdfPTable1.WidthPercentage = 100;
            _oPdfPCell = new PdfPCell();

            #region Header
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase((bIsWarp == true ? "Warp" : "Weft") + " Pattern", _oFontStyle));
            _oPdfPCell.Colspan = 4;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = _oBC;
            oPdfPTable1.AddCell(_oPdfPCell);
            oPdfPTable1.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Count", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = _oBC;
            oPdfPTable1.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Color", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = _oBC;
            oPdfPTable1.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase((bIsWarp == true ? "Ends" : "Pick"), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = _oBC;
            oPdfPTable1.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Times", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = _oBC;
            oPdfPTable1.AddCell(_oPdfPCell);
            oPdfPTable1.CompleteRow();
            #endregion

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            int nGroupNo = 0;
            List<FabricPatternDetail> oTempFPDs = new List<FabricPatternDetail>();

            foreach (FabricPatternDetail oItem in oFPDs)
            {
                oTempFPDs = new List<FabricPatternDetail>();
                if (oItem.GroupNo == 0)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductShortName, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable1.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ColorName, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable1.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.EndsCount.ToString(), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable1.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable1.AddCell(_oPdfPCell);
                    oPdfPTable1.CompleteRow();
                }
                else if (oItem.GroupNo != nGroupNo)
                {
                    nGroupNo = oItem.GroupNo;
                    oTempFPDs = oFPDs.Where(o => o.GroupNo == nGroupNo).ToList();

                    string[] sProductId = string.Join(",", oTempFPDs.Select(o => o.ProductID).Distinct()).Split(',');
                    if (sProductId.Count() == 1)
                    {
                        _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductShortName, _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPTable1.AddCell(_oPdfPCell);
                    }
                    else
                    {
                        _oPdfPCell = new PdfPCell(this.SetProductName(oTempFPDs));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPTable1.AddCell(_oPdfPCell);
                    }

                    if (oTempFPDs.Count > 0)
                    {
                        _oPdfPCell = new PdfPCell(this.ColorAndEndsCount(oTempFPDs));
                        _oPdfPCell.Colspan = 2;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPTable1.AddCell(_oPdfPCell);
                    }
                    else
                    {
                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPTable1.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPTable1.AddCell(_oPdfPCell);
                    }

                    int nRepeatNo = oTempFPDs.Where(o => o.SetNo == 1).Count();
                    _oPdfPCell = new PdfPCell(new Phrase(nRepeatNo.ToString(), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable1.AddCell(_oPdfPCell);
                    oPdfPTable1.CompleteRow();
                }
            }

            int nMax = Math.Max(_nWarpTotalRow, _nWeftTotalRow);
            int nDiff = 0;
            if (bIsWarp)
            {
                if (_oWarps.Count != nMax)
                {
                    nDiff = nMax - _nWarpTotalRow;
                }
            }
            else
            {
                if (_oWefts.Count != nMax)
                {
                    nDiff = nMax - _nWeftTotalRow;
                }
            }

            if (bIsWarp)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
                int nSumWarp = _oWarps.Sum(x => x.EndsCount);
                _oPdfPCell = new PdfPCell(new Phrase("Total Ends:  ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable1.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable1.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(nSumWarp.ToString(), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable1.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable1.AddCell(_oPdfPCell);
                oPdfPTable1.CompleteRow();
            }
            else
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
                int nSumWeft = _oWefts.Sum(x => x.EndsCount);
                _oPdfPCell = new PdfPCell(new Phrase("Total Picks:  ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable1.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable1.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(nSumWeft.ToString(), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable1.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable1.AddCell(_oPdfPCell);
                oPdfPTable1.CompleteRow();
            }

            for (int i = 0; i < nDiff; i++)
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.Border = 0;
                oPdfPTable1.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.Border = 0;
                oPdfPTable1.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.Border = 0;
                oPdfPTable1.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.Border = 0;
                oPdfPTable1.AddCell(_oPdfPCell);
                oPdfPTable1.CompleteRow();
            }

            return oPdfPTable1;
        }
        private PdfPTable TableHeader() 
        {
            PdfPTable oPdfPTable = new PdfPTable(9);
            oPdfPTable.SetWidths(new float[] { 100f, 100f, 60f, 60f, 30f, 100f, 100f, 60f, 60f });

            #region Title
            _oPdfPCell = new PdfPCell(new Phrase("Warp Pattern", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.Colspan = 4;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = _oBC;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Weft Pattern", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.Colspan = 4;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = _oBC;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            #region Header

            #region Warp Header
            _oPdfPCell = new PdfPCell(new Phrase("Count", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = _oBC;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Color", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = _oBC;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Ends", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = _oBC;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Times", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = _oBC;
            oPdfPTable.AddCell(_oPdfPCell);
            #endregion

            #region Space Between Two Tables
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);
            #endregion

            #region Weft Header
            _oPdfPCell = new PdfPCell(new Phrase("Count", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = _oBC;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Color", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = _oBC;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Pick", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = _oBC;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Times", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = _oBC;
            oPdfPTable.AddCell(_oPdfPCell);
            #endregion

            oPdfPTable.CompleteRow();

            #endregion

            #endregion

            return oPdfPTable;
        }
        private PdfPTable SetProductName(List<FabricPatternDetail> oFPDs)
        {
            PdfPTable oPdfPTable = new PdfPTable(1);
            oPdfPTable.SetWidths(new float[] { 100f });
            List<FabricPatternDetail> oTempFPDs = new List<FabricPatternDetail>();
            string[] sDistinctSetNo = string.Join(",", oFPDs.Select(o => o.SetNo).Distinct()).Split(',');

            foreach (string sSetNo in sDistinctSetNo)
            {
                oTempFPDs = new List<FabricPatternDetail>();
                oTempFPDs = oFPDs.Where(o => o.SetNo == Convert.ToInt16(sSetNo)).ToList();

                _oPdfPCell = new PdfPCell(new Phrase(oTempFPDs[0].ProductShortName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();

                //if (sProductId.Count() == 1)
                //{
                //    break;
                //}
            }
          
            return oPdfPTable;
        }
        private PdfPTable ColorAndEndsCount(List<FabricPatternDetail> oFPDs)
        {
            PdfPTable oPdfPTable = new PdfPTable(2);
            oPdfPTable.SetWidths(new float[] { 100f, 60f });
            int nSetNo = 0;
            int nCount = 0;
            foreach (FabricPatternDetail oItem in oFPDs)
            {
                if (nCount == 0)
                {
                    nSetNo = oItem.SetNo;
                }

                if (oItem.SetNo == nSetNo && nCount != 0)
                {
                    break;
                }

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ColorName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.EndsCount.ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();

                nCount++;
            }
            return oPdfPTable;
        }
        private PdfPTable InformationPart()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            PdfPTable oPdfPTable = new PdfPTable(11);
            oPdfPTable.SetWidths(new float[] { 40f, 50f, 100f, 80f, 80f, 10f, 25f, 60f, 60f, 60f, 80f });

            _oPdfPCell = new PdfPCell(new Phrase("Buyer ", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oFabricPattern.BuyerName, _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Construction ", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oFabricPattern.Construction, _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Reed ", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

           
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oFabricPattern.Reed.ToString() + "/" + _oFabricPattern.Dent.ToString(), _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("R. Size ", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oFabricPattern.RepeatSize, _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Fabric ID ", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oFabricPattern.FabricNo, _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Pick ", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oFabricPattern.Pick.ToString(), _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Warp ", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oFabricPattern.Warp.ToString(), _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Ratio ", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oFabricPattern.Ratio, _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Style ", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oFabricPattern.StyleNo, _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Weave ", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oFabricPattern.WeaveName, _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Weft ", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oFabricPattern.Weft.ToString(), _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("GSM ", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oFabricPattern.GSM.ToString(), _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Fabric Design ", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oFabricPattern.FabricDesignName, _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Note ", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oFabricPattern.Note, _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.Colspan = 7; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            return oPdfPTable;
        }
        private void Blank(int nFixedHeight)
        {
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.FixedHeight = nFixedHeight;
            _oPdfPCell.Colspan = _nColumn; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
    }
}


