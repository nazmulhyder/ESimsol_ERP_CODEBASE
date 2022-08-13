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


    public class rptPolyMeasurementList
    {
    //    #region Declaration
    //    Document _oDocument;
    //    iTextSharp.text.Font _oFontStyle;
    //    PdfPTable _oPdfPTable = new PdfPTable(3);
    //    PdfPCell _oPdfPCell;
    //    iTextSharp.text.Image _oImag;
    //    MemoryStream _oMemoryStream = new MemoryStream();
    //    PolyMeasurement _oPolyMeasurement = new PolyMeasurement();
    //    List<PolyMeasurement> _oPolyMeasurements = new List<PolyMeasurement>();

    //    Company _oCompany = new Company();

    //    #endregion

    //    public byte[] PrepareReport(PolyMeasurement oPolyMeasurement, Company oCompany)
    //    {
    //        _oPolyMeasurements = oPolyMeasurement.ColorCategories;
    //        _oCompany = oCompany;

    //        #region Page Setup
    //        _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
    //        _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
    //        _oDocument.SetMargins(40f, 40f, 5f, 40f);
    //        _oPdfPTable.WidthPercentage = 100;
    //        _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

    //        _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
    //        PdfWriter.GetInstance(_oDocument, _oMemoryStream);
    //        _oDocument.Open();
    //        //SL No,    PolyMeasurement No, PolyMeasurement Name,   Narration,  Date,   Authorized By,  Amount,Con. Rate,   Amount
    //        _oPdfPTable.SetWidths(new float[] { 45f, 300f, 190f });
    //        #endregion

    //        this.PrintHeader();
    //        this.PrintBody();
    //        _oPdfPTable.HeaderRows = 4;
    //        _oDocument.Add(_oPdfPTable);
    //        _oDocument.Close();
    //        return _oMemoryStream.ToArray();
    //    }

    //    public PdfPTable PrepareExcel(PolyMeasurement oPolyMeasurement, Company oCompany)
    //    {
    //        _oPolyMeasurements = oPolyMeasurement.ColorCategories;
    //        _oCompany = oCompany;

    //        #region Page Setup
    //        _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
    //        _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
    //        _oDocument.SetMargins(40f, 40f, 5f, 40f);
    //        _oPdfPTable.WidthPercentage = 100;
    //        _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

    //        _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
    //        PdfWriter.GetInstance(_oDocument, _oMemoryStream);
    //        _oDocument.Open();
    //        //SL No,    PolyMeasurement No, PolyMeasurement Name,   Narration,  Date,   Authorized By,  Amount,Con. Rate,   Amount
    //        _oPdfPTable.SetWidths(new float[] { 45f, 300f, 190f });
    //        #endregion

    //        this.PrintHeader();
    //        this.PrintBody();
    //        _oPdfPTable.HeaderRows = 4;
    //        _oDocument.Add(_oPdfPTable);
    //        _oDocument.Close();
    //        return _oPdfPTable;
    //    }

    //    #region Report Header
    //    private void PrintHeader()
    //    {
    //        #region CompanyHeader
    //        if (_oCompany.CompanyLogo != null)
    //        {
    //            _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
    //            _oImag.ScaleAbsolute(160f, 35f);
    //            _oPdfPCell = new PdfPCell(_oImag);
    //            _oPdfPCell.Colspan = 3;
    //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
    //            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
    //            _oPdfPCell.Border = 0;
    //            _oPdfPCell.FixedHeight = 35;
    //            _oPdfPTable.AddCell(_oPdfPCell);
    //            _oPdfPTable.CompleteRow();
    //        }
    //        else
    //        {
    //            _oFontStyle = FontFactory.GetFont("Tahoma", 11f, 1);
    //            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));
    //            _oPdfPCell.Colspan = 3;
    //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
    //            _oPdfPCell.Border = 0;
    //            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
    //            _oPdfPCell.ExtraParagraphSpace = 0;
    //            _oPdfPTable.AddCell(_oPdfPCell);
    //            _oPdfPTable.CompleteRow();
    //        }

    //        _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
    //        _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Address + "\n" + _oCompany.Phone + ";  " + _oCompany.Email + ";  " + _oCompany.WebAddress, _oFontStyle));
    //        _oPdfPCell.Colspan = 3;
    //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
    //        _oPdfPCell.Border = 0;
    //        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
    //        _oPdfPCell.ExtraParagraphSpace = 0;
    //        _oPdfPTable.AddCell(_oPdfPCell);
    //        _oPdfPTable.CompleteRow();
    //        #endregion

    //        #region ReportHeader
    //        _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
    //        _oPdfPCell = new PdfPCell(new Phrase("Poly Measurement", _oFontStyle));
    //        _oPdfPCell.Colspan = 3;
    //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
    //        _oPdfPCell.Border = 0;
    //        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
    //        _oPdfPCell.ExtraParagraphSpace = 5f;
    //        _oPdfPTable.AddCell(_oPdfPCell);
    //        _oPdfPTable.CompleteRow();
    //        #endregion

    //    }
    //    #endregion

    //    #region Report Body
    //    private void PrintBody()
    //    {

    //        _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
    //        _oPdfPCell = new PdfPCell(new Phrase("SL No", _oFontStyle));
    //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);


    //        _oPdfPCell = new PdfPCell(new Phrase("Measurement", _oFontStyle));
    //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

    //        _oPdfPCell = new PdfPCell(new Phrase("Note", _oFontStyle));
    //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

    //        _oPdfPTable.CompleteRow();

    //        int nCount = 0;
    //        _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
    //        foreach (PolyMeasurement oItem in _oPolyMeasurements)
    //        {
    //            nCount++;
    //            _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
    //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

    //            _oPdfPCell = new PdfPCell(new Phrase(oItem.Measurement, _oFontStyle));
    //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

    //            _oPdfPCell = new PdfPCell(new Phrase(oItem.Note, _oFontStyle));
    //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

    //            _oPdfPTable.CompleteRow();
    //        }
    //    }
    //    #endregion

        #region Declaration
        int _nTotalColumn = 4;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(4);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        PolyMeasurement _oPolyMeasurement = new PolyMeasurement();
        List<PolyMeasurement> _oPolyMeasurements = new List<PolyMeasurement>();

        Company _oCompany = new Company();
        string _sMessage = "";

        #endregion

        public byte[] PrepareReport(List<PolyMeasurement> oPolyMeasurements, Company oCompany, string sMessage)
        {
            _oPolyMeasurements = oPolyMeasurements;
            _oCompany = oCompany;
            _sMessage = sMessage;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            _oDocument.SetMargins(25f, 25f, 5f, 25f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 
                                                40f,  //SL
                                                125f, //PolyMeasurement Code
                                                220f, //PolyMeasurement Name
                                                200f  //PolyMeasurement Type                                              
                                                });
            #endregion

            this.PrintHeader();
            this.PrintBody();
            _oPdfPTable.HeaderRows = 4;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Header
        private void PrintHeader()
        {
            #region CompanyHeader
            if (_oCompany.CompanyLogo != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(160f, 35f);
                _oPdfPCell = new PdfPCell(_oImag);
                _oPdfPCell.Colspan = _nTotalColumn;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 35;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            else
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 11f, 1);
                _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));
                _oPdfPCell.Colspan = _nTotalColumn;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.ExtraParagraphSpace = 0;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Address + "\n" + _oCompany.Phone + ";  " + _oCompany.Email + ";  " + _oCompany.WebAddress, _oFontStyle));
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

        #region Report Body
        private void PrintBody()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("#SL", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("PolyMeasurement Type", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Measurement", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Note", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            int nCount = 0;
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
            foreach (PolyMeasurement oItem in _oPolyMeasurements)
            {
                nCount++;
                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.PolyMeasurementTypeSt, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Measurement, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Note, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
        }
        #endregion
    }




}
