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
    public class rptDevelopmentRecapSummary
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(7);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        List<DevelopmentRecapSummary> _oDevelopmentRecapSummarys = new List<DevelopmentRecapSummary>();

        Company _oCompany = new Company();
        Contractor _oContractor = new Contractor();
        string _sReportHeader = "";
        #endregion

        #region Constructor
        public rptDevelopmentRecapSummary() { }
        #endregion

        public byte[] PrepareReport(List<DevelopmentRecapSummary> oDevelopmentRecapSummarys, Company oCompany, string ReportHeader)
        {
            _oDevelopmentRecapSummarys = oDevelopmentRecapSummarys;
            _oCompany = oCompany;
            _sReportHeader = ReportHeader;
            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
            _oDocument.SetMargins(15f, 15f, 0f, 15f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();

            _oPdfPTable.SetWidths(new float[] { 96f, 134f, 134f, 134f, 134f, 134f, 134f });
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
            _oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            #region CompanyHeader
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            if (_oCompany.CompanyLogo == null)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Development Recap Summary", _oFontStyle));
                _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 12f, 1);
                _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.ExtraParagraphSpace = 0;
                _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_sReportHeader, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.Colspan = 3; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("Development Recap Summary", _oFontStyle));
                _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.Border = 0;
                _oImag.BorderColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oImag);

                _oPdfPCell = new PdfPCell(new Phrase(_sReportHeader, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.Colspan = 3; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion

            #region ReportHeader
            //_oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            //_oPdfPCell = new PdfPCell(new Phrase("Development Recap Summary", _oFontStyle));
            //_oPdfPCell.Colspan = 7;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //_oPdfPCell.Border = 0;
            //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.ExtraParagraphSpace = 5f;
            //_oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();
            #endregion
        }
        #endregion

        #region Report Body
        private void PrintBody()
        {
            _oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.BOX;
            for (int i = 0; i < _oDevelopmentRecapSummarys.Count; i++)
            {

                #region Style Image
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1, BaseColor.BLACK);
                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.FixedHeight = 100f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                if (_oDevelopmentRecapSummarys[i].StyleCoverImage != null)
                {
                    _oImag = iTextSharp.text.Image.GetInstance(_oDevelopmentRecapSummarys[i].StyleCoverImage, System.Drawing.Imaging.ImageFormat.Jpeg);
                    _oImag.Border = 1;
                    _oImag.ScaleAbsolute(105f, 96f);                    
                    _oPdfPCell = new PdfPCell(_oImag);
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                    _oPdfPCell.FixedHeight = 100f;
                    _oPdfPCell.Padding = 2f;
                    _oPdfPTable.AddCell(_oPdfPCell);
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                    _oPdfPCell.FixedHeight = 100f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }

                if (i + 1 < _oDevelopmentRecapSummarys.Count)
                {
                    if (_oDevelopmentRecapSummarys[i + 1].StyleCoverImage != null)
                    {
                        _oImag = iTextSharp.text.Image.GetInstance(_oDevelopmentRecapSummarys[i + 1].StyleCoverImage, System.Drawing.Imaging.ImageFormat.Jpeg);
                        _oImag.Border = 1;
                        _oImag.ScaleAbsolute(105f, 96f);
                        _oPdfPCell = new PdfPCell(_oImag);
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                        _oPdfPCell.FixedHeight = 100f;
                        _oPdfPCell.Padding = 2f;
                        _oPdfPTable.AddCell(_oPdfPCell);
                    }
                    else
                    {
                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.FixedHeight = 100f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    }

                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                    _oPdfPCell.FixedHeight = 100f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }

                if (i + 2 < _oDevelopmentRecapSummarys.Count)
                {
                    if (_oDevelopmentRecapSummarys[i + 2].StyleCoverImage != null)
                    {
                        _oImag = iTextSharp.text.Image.GetInstance(_oDevelopmentRecapSummarys[i + 2].StyleCoverImage, System.Drawing.Imaging.ImageFormat.Jpeg);
                        _oImag.Border = 1;
                        _oImag.ScaleAbsolute(105f, 96f);
                        _oPdfPCell = new PdfPCell(_oImag);
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                        _oPdfPCell.FixedHeight = 100f;
                        _oPdfPCell.Padding = 2f;
                        _oPdfPTable.AddCell(_oPdfPCell);
                    }
                    else
                    {
                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.FixedHeight = 100f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    }

                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                    _oPdfPCell.FixedHeight = 100f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }


                if (i + 3 < _oDevelopmentRecapSummarys.Count)
                {
                    if (_oDevelopmentRecapSummarys[i + 3].StyleCoverImage != null)
                    {
                        _oImag = iTextSharp.text.Image.GetInstance(_oDevelopmentRecapSummarys[i + 3].StyleCoverImage, System.Drawing.Imaging.ImageFormat.Jpeg);
                        _oImag.Border = 1;
                        _oImag.ScaleAbsolute(105f, 96f);
                        _oPdfPCell = new PdfPCell(_oImag);
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                        _oPdfPCell.FixedHeight = 100f;
                        _oPdfPCell.Padding = 2f;
                        _oPdfPTable.AddCell(_oPdfPCell);
                    }
                    else
                    {
                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.FixedHeight = 100f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    }

                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                    _oPdfPCell.FixedHeight = 100f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }

                if (i + 4 < _oDevelopmentRecapSummarys.Count)
                {
                    if (_oDevelopmentRecapSummarys[i + 4].StyleCoverImage != null)
                    {
                        _oImag = iTextSharp.text.Image.GetInstance(_oDevelopmentRecapSummarys[i + 4].StyleCoverImage, System.Drawing.Imaging.ImageFormat.Jpeg);
                        _oImag.Border = 1;
                        _oImag.ScaleAbsolute(105f, 96f);
                        _oPdfPCell = new PdfPCell(_oImag);
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                        _oPdfPCell.FixedHeight = 100f;
                        _oPdfPCell.Padding = 2f;
                        _oPdfPTable.AddCell(_oPdfPCell);
                    }
                    else
                    {
                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.FixedHeight = 100f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    }

                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                    _oPdfPCell.FixedHeight = 100f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }


                if (i + 5 < _oDevelopmentRecapSummarys.Count)
                {
                    if (_oDevelopmentRecapSummarys[i + 5].StyleCoverImage != null)
                    {
                        _oImag = iTextSharp.text.Image.GetInstance(_oDevelopmentRecapSummarys[i + 5].StyleCoverImage, System.Drawing.Imaging.ImageFormat.Jpeg);
                        _oImag.Border = 1;
                        _oImag.ScaleAbsolute(105f, 96f);
                        _oPdfPCell = new PdfPCell(_oImag);
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                        _oPdfPCell.FixedHeight = 100f;
                        _oPdfPCell.Padding = 2f;
                        _oPdfPTable.AddCell(_oPdfPCell);
                    }
                    else
                    {
                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.FixedHeight = 100f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    }

                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                    _oPdfPCell.FixedHeight = 100f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                #endregion

                #region SL No
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
                _oPdfPCell = new PdfPCell(new Phrase("SL No", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oDevelopmentRecapSummarys[i].DevelopmentRecapNo, _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 1 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 1].DevelopmentRecapNo : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase((i + 2 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 2].DevelopmentRecapNo : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 3 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 3].DevelopmentRecapNo : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 4 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 4].DevelopmentRecapNo : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 5 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 5].DevelopmentRecapNo : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                #endregion

                #region SL No
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
                _oPdfPCell = new PdfPCell(new Phrase("Session", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oDevelopmentRecapSummarys[i].SessionName, _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 1 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 1].SessionName : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase((i + 2 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 2].SessionName : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 3 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 3].SessionName : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 4 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 4].SessionName : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 5 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 5].SessionName : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                #endregion

                #region Style No
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
                _oPdfPCell = new PdfPCell(new Phrase("Style No", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
                _oPdfPCell = new PdfPCell(new Phrase(_oDevelopmentRecapSummarys[i].StyleNo, _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 1 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 1].StyleNo : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase((i + 2 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 2].StyleNo : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 3 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 3].StyleNo : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 4 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 4].StyleNo : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 5 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 5].StyleNo : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                #endregion

                #region Style Description
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
                _oPdfPCell = new PdfPCell(new Phrase("Style Description", _oFontStyle));
                _oPdfPCell.MinimumHeight = 25f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
                _oPdfPCell = new PdfPCell(new Phrase(_oDevelopmentRecapSummarys[i].StyleDescription, _oFontStyle));
                _oPdfPCell.MinimumHeight = 25f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 1 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 1].StyleDescription : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 25f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase((i + 2 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 2].StyleDescription : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 25f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 3 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 3].StyleDescription : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 25f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 4 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 4].StyleDescription : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 25f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 5 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 5].StyleDescription : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 25f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                #endregion

                #region Knitting Pattern
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
                _oPdfPCell = new PdfPCell(new Phrase("Knitting Pattern", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
                _oPdfPCell = new PdfPCell(new Phrase(_oDevelopmentRecapSummarys[i].KnittingPatternName, _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 1 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 1].KnittingPatternName : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase((i + 2 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 2].KnittingPatternName : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 3 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 3].KnittingPatternName : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 4 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 4].KnittingPatternName : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 5 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 5].KnittingPatternName : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                #endregion

                #region Fabrication
                _oPdfPCell = new PdfPCell(new Phrase("Fabrication", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oDevelopmentRecapSummarys[i].Fabrication, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 1 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 1].Fabrication : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase((i + 2 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 2].Fabrication : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 3 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 3].Fabrication : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 4 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 4].Fabrication : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 5 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 5].Fabrication : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                #endregion

                #region Yarn-A
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
                _oPdfPCell = new PdfPCell(new Phrase("Yarn/Count/Ply", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0, BaseColor.BLACK);
                _oPdfPCell = new PdfPCell(new Phrase(_oDevelopmentRecapSummarys[i].FabricOptionA, _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 1 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 1].FabricOptionA : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase((i + 2 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 2].FabricOptionA : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 3 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 3].FabricOptionA : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 4 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 4].FabricOptionA : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 5 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 5].FabricOptionA : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                #endregion
                
                #region Yarn-B
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
                _oPdfPCell = new PdfPCell(new Phrase("Yarn/Count/Ply", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0, BaseColor.BLACK);
                _oPdfPCell = new PdfPCell(new Phrase(_oDevelopmentRecapSummarys[i].FabricOptionB, _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 1 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 1].FabricOptionB : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase((i + 2 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 2].FabricOptionB : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 3 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 3].FabricOptionB : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 4 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 4].FabricOptionB : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 5 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 5].FabricOptionB : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                #endregion

                #region Yarn-C
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
                _oPdfPCell = new PdfPCell(new Phrase("Yarn/Count/Ply", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0, BaseColor.BLACK);
                _oPdfPCell = new PdfPCell(new Phrase(_oDevelopmentRecapSummarys[i].FabricOptionC, _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 1 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 1].FabricOptionC : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase((i + 2 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 2].FabricOptionC : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 3 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 3].FabricOptionC : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 4 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 4].FabricOptionC : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 5 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 5].FabricOptionC : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                #endregion

                #region Yarn-D
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
                _oPdfPCell = new PdfPCell(new Phrase("Yarn/Count/Ply", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0, BaseColor.BLACK);
                _oPdfPCell = new PdfPCell(new Phrase(_oDevelopmentRecapSummarys[i].FabricOptionD, _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 1 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 1].FabricOptionD : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase((i + 2 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 2].FabricOptionD : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 3 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 3].FabricOptionD : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 4 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 4].FabricOptionD : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 5 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 5].FabricOptionD : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                #endregion
                
                #region Special Finish
                _oPdfPCell = new PdfPCell(new Phrase("Special Finish", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                #endregion

                #region Gauge/Weight
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
                _oPdfPCell = new PdfPCell(new Phrase("Gauge/Weight", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oDevelopmentRecapSummarys[i].GG, _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 1 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 1].GG : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase((i + 2 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 2].GG : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 3 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 3].GG : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 4 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 4].GG : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 5 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 5].GG : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                #endregion

                #region Weight
                _oPdfPCell = new PdfPCell(new Phrase("Weight ", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oDevelopmentRecapSummarys[i].Weight, _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 1 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 1].Weight : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase((i + 2 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 2].Weight : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 3 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 3].Weight : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 4 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 4].Weight : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 5 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 5].Weight : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                #endregion

                #region Sample Type
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
                _oPdfPCell = new PdfPCell(new Phrase("Type of Sample", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
                _oPdfPCell = new PdfPCell(new Phrase(_oDevelopmentRecapSummarys[i].CollectionName, _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 1 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 1].CollectionName : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase((i + 2 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 2].CollectionName : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 3 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 3].CollectionName : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 4 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 4].CollectionName : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 5 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 5].CollectionName : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                #endregion

                #region Status
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase("Status", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase(_oDevelopmentRecapSummarys[i].DevelopmentStatus.ToString(), _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 1 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 1].DevelopmentStatus.ToString() : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase((i + 2 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 2].DevelopmentStatus.ToString() : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 3 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 3].DevelopmentStatus.ToString() : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 4 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 4].DevelopmentStatus.ToString() : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 5 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 5].DevelopmentStatus.ToString() : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                #endregion

                #region Sample Qty
                _oPdfPCell = new PdfPCell(new Phrase("Sample Qty", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oDevelopmentRecapSummarys[i].SampleQty.ToString() + " Pcs ", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 1 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 1].SampleQty.ToString() + " Pcs " : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase((i + 2 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 2].SampleQty.ToString() + " Pcs " : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 3 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 3].SampleQty.ToString() + " Pcs " : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 4 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 4].SampleQty.ToString() + " Pcs " : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 5 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 5].SampleQty.ToString() + " Pcs " : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                #endregion

                #region Unit Price
                _oPdfPCell = new PdfPCell(new Phrase("Unit Price", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oDevelopmentRecapSummarys[i].UnitPriceSt, _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 1 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i+1].UnitPriceSt : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase((i + 2 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i+2].UnitPriceSt : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 3 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i+3].UnitPriceSt : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 4 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i+4].UnitPriceSt : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 5 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i+5].UnitPriceSt : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                #endregion

                #region Inq Rcv Dt
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
                _oPdfPCell = new PdfPCell(new Phrase("Inq Rcv Dt", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oDevelopmentRecapSummarys[i].InquiryReceivedDateInString, _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 1 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 1].InquiryReceivedDateInString : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase((i + 2 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 2].InquiryReceivedDateInString : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 3 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 3].InquiryReceivedDateInString : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 4 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 4].InquiryReceivedDateInString : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 5 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 5].InquiryReceivedDateInString : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                #endregion

                #region Smpl Rcv Dt
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
                _oPdfPCell = new PdfPCell(new Phrase("Smpl Rcv Dt", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oDevelopmentRecapSummarys[i].SampleReceivedDateInString, _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 1 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 1].SampleReceivedDateInString : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase((i + 2 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 2].SampleReceivedDateInString : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 3 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 3].SampleReceivedDateInString : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 4 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 4].SampleReceivedDateInString : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 5 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 5].SampleReceivedDateInString : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                #endregion

                #region Dead Line Dt
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
                _oPdfPCell = new PdfPCell(new Phrase("Dead Line Dt", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oDevelopmentRecapSummarys[i].SendingDeadLineInString, _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 1 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 1].SendingDeadLineInString : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase((i + 2 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 2].SendingDeadLineInString : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 3 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 3].SendingDeadLineInString : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 4 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 4].SendingDeadLineInString : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 5 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 5].SendingDeadLineInString : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                #endregion

                #region Smpl Sending Dt
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
                _oPdfPCell = new PdfPCell(new Phrase("Smpl Sending Dt", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oDevelopmentRecapSummarys[i].SampleSendingDateInString, _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 1 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 1].SampleSendingDateInString : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase((i + 2 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 2].SampleSendingDateInString : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 3 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 3].SampleSendingDateInString : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 4 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 4].SampleSendingDateInString : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 5 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 5].SampleSendingDateInString : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                #endregion

                #region Color Range
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
                _oPdfPCell = new PdfPCell(new Phrase("Color Range", _oFontStyle));
                _oPdfPCell.MinimumHeight = 25f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
                _oPdfPCell = new PdfPCell(new Phrase(_oDevelopmentRecapSummarys[i].ColorRange, _oFontStyle));
                _oPdfPCell.MinimumHeight = 25f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 1 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 1].ColorRange : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 25f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase((i + 2 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 2].ColorRange : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 25f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 3 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 3].ColorRange : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 25f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 4 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 4].ColorRange : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 25f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 5 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 5].ColorRange : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 25f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                #endregion

                #region Merchandiser Name
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
                _oPdfPCell = new PdfPCell(new Phrase("Merchandiser Name", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
                _oPdfPCell = new PdfPCell(new Phrase(_oDevelopmentRecapSummarys[i].MerchandiserName, _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 1 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 1].MerchandiserName : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase((i + 2 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 2].MerchandiserName : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 3 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 3].MerchandiserName : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 4 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 4].MerchandiserName : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 5 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 5].MerchandiserName : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                #endregion

                #region Approved By
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
                _oPdfPCell = new PdfPCell(new Phrase("Approved By", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
                _oPdfPCell = new PdfPCell(new Phrase(_oDevelopmentRecapSummarys[i].ApprovedByName, _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 1 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 1].ApprovedByName : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase((i + 2 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 2].ApprovedByName : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 3 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 3].ApprovedByName : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 4 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 4].ApprovedByName : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 5 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 5].ApprovedByName : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                #endregion

                #region Prepared By
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
                _oPdfPCell = new PdfPCell(new Phrase("Prepared By", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
                _oPdfPCell = new PdfPCell(new Phrase(_oDevelopmentRecapSummarys[i].PrepareBy, _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 1 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 1].PrepareBy : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase((i + 2 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 2].PrepareBy : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 3 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 3].PrepareBy : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 4 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 4].PrepareBy : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 5 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 5].PrepareBy : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                #endregion
                
                #region Factory
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
                _oPdfPCell = new PdfPCell(new Phrase("Factory", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
                _oPdfPCell = new PdfPCell(new Phrase(_oDevelopmentRecapSummarys[i].FactoryName, _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 1 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 1].FactoryName : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase((i + 2 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 2].FactoryName : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 3 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 3].FactoryName : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 4 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 4].FactoryName : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 5 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 5].FactoryName : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 13f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                #endregion

                #region Remarks
                _oPdfPCell = new PdfPCell(new Phrase("Remarks", _oFontStyle));
                _oPdfPCell.MinimumHeight = 60f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oDevelopmentRecapSummarys[i].Remarks, _oFontStyle));
                _oPdfPCell.MinimumHeight = 60f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 1 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 1].Remarks : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 60f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase((i + 2 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 2].Remarks : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 60f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell.MinimumHeight = 60f; _oPdfPCell = new PdfPCell(new Phrase((i + 3 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 3].Remarks : "", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 4 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 4].Remarks : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 60f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((i + 5 < _oDevelopmentRecapSummarys.Count) ? _oDevelopmentRecapSummarys[i + 5].Remarks : "", _oFontStyle));
                _oPdfPCell.MinimumHeight = 60f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                #endregion

                i = i + 5;
            }
        }
        #endregion
    }
}
