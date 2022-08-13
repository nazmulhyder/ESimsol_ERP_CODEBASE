
using System;
using System.Data;
using System.Linq;
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
    public class rptFNLabDipRecipe
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyle_Bold;
        int _nColumn = 12;
        PdfPTable _oPdfPTable = new PdfPTable(12);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;

        int _nOrderType = -99;
        MemoryStream _oMemoryStream = new MemoryStream();
        List<FNLabDipDetail> _oFNLabDipDetails=new  List<FNLabDipDetail>();
        List<FNLabdipShade> _oFNLabdipShades = new List<FNLabdipShade>();
        List<FNLabdipRecipe> _oFNLabdipRecipes = new List<FNLabdipRecipe>();
        Company _oCompany = new Company();
        Fabric _oFabric=new Fabric();
        BusinessUnit _oBusinessUnit = new BusinessUnit();
        #endregion

        #region PeapareReport
        public byte[] PrepareReport(List<FNLabDipDetail> oFNLabDipDetails, Company oCompany, BusinessUnit oBusinessUnit, List<FNLabdipShade> oFNLabdipShades, List<FNLabdipRecipe> oFNLabdipRecipes, Fabric oFabric)
        {
            _oFNLabDipDetails = oFNLabDipDetails;
            _oFNLabdipShades = oFNLabdipShades;
            _oFNLabdipRecipes = oFNLabdipRecipes;
            _oFabric = oFabric;

            _oCompany = oCompany;
            _oBusinessUnit = oBusinessUnit;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            _oDocument.SetMargins(5f, 5f, 5f, 5f);
            _oPdfPTable.WidthPercentage = 95;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_CENTER;

            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();

            //_oPdfPTable.SetWidths(new float[] { 28f, 30f, 60f, 20f, 45f, 45f, 40f, 50f, 50f, 42f, 52f });
            _oPdfPTable.SetWidths(new float[]   { 28f, 30f, 50f, 30f, 55f, 45f, 40f, 40f, 65f, 25f, 24f, 40f });
            #endregion

            this.PrintHeader("Recipe");
            this.PrintBody();
            _oPdfPTable.HeaderRows = 2;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        #endregion

        #region Report Header
        private void PrintHeader(string sHeader)
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
                _oImag.ScaleAbsolute(60f, 35f);
                _oPdfPCell = new PdfPCell(_oImag);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            }
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Rowspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
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
            _oPdfPCell.Border = 0; _oPdfPCell.Colspan = _nColumn; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            #endregion
            #region Title
            _oFontStyle = FontFactory.GetFont("Tahoma", 14f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(sHeader + (_oFabric.ApprovedBy == 0 ? "(unauthorized)" : ""), _oFontStyle));
            _oPdfPCell.Colspan = _nColumn;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 20f;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 8f;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        #endregion

        private void PrintBody()
        {
            PdfPTable oPdfPTableFNLabdip = new PdfPTable(9);
            oPdfPTableFNLabdip.SetWidths(new float[] { 35f, 30f, 40f, 20f, 45f, 45f, 30f, 60f, 50f });
                                                       //35f, 30f, 40f, 20f, 45f, 45f, 30f, 60f, 50f 
            PdfPTable oPdfPTableSwatch = new PdfPTable(2);
            oPdfPTableSwatch.SetWidths(new float[] { 40f, 50f });

            #region Top part

            #region FNLabdip Table
            /*------------Row-1-----------*/
            _oFontStyle_Bold = FontFactory.GetFont("Tahoma", 10f, 1);
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            //_oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Buyer ", _oFontStyle));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableFNLabdip.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase(_oFabric.BuyerName, _oFontStyle));
            _oPdfPCell.Colspan = 7; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableFNLabdip.AddCell(_oPdfPCell);
            oPdfPTableFNLabdip.CompleteRow();

            /*------------Row-2-----------*/
            //_oFontStyle_Bold = FontFactory.GetFont("Tahoma", 10f, 1);
            //_oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            //_oPdfPCell = new PdfPCell(new Phrase("Buying House ", _oFontStyle));
            //_oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableFNLabdip.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //_oPdfPCell.Colspan = 7; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableFNLabdip.AddCell(_oPdfPCell);
            //oPdfPTableFNLabdip.CompleteRow();

            /*------------Row-2-----------*/
            //_oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Order No ", _oFontStyle));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableFNLabdip.AddCell(_oPdfPCell);

            //_oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Colspan = 7; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableFNLabdip.AddCell(_oPdfPCell);
            oPdfPTableFNLabdip.CompleteRow();

            /*------------Row-3-----------*/
            //_oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Mkt Ref ", _oFontStyle));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableFNLabdip.AddCell(_oPdfPCell);

            //_oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oFabric.FabricNo, _oFontStyle));
            _oPdfPCell.Colspan = 7; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableFNLabdip.AddCell(_oPdfPCell);
            oPdfPTableFNLabdip.CompleteRow();


            /*------------Row-4-----------*/
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("Construction", _oFontStyle));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableFNLabdip.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oFabric.Construction, _oFontStyle));
            _oPdfPCell.Colspan = 7; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableFNLabdip.AddCell(_oPdfPCell);
            oPdfPTableFNLabdip.CompleteRow();


            /*------------Row-5-----------*/
            _oPdfPCell = new PdfPCell(new Phrase("Ref No ", _oFontStyle));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableFNLabdip.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oFabric.BuyerReference, _oFontStyle));
            _oPdfPCell.Colspan = 7; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableFNLabdip.AddCell(_oPdfPCell);
            oPdfPTableFNLabdip.CompleteRow();

            /*------------Row-6-----------*/
            //_oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Style No", _oFontStyle));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableFNLabdip.AddCell(_oPdfPCell);

            //_oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oFabric.StyleNo, _oFontStyle));
            _oPdfPCell.Colspan = 7; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableFNLabdip.AddCell(_oPdfPCell);
            oPdfPTableFNLabdip.CompleteRow();

            //_oFontStyle = FontFactory.GetFont("Tahoma", 7f, 0);
            //_oPdfPCell = new PdfPCell(new Phrase("Buyer Concern Person ", _oFontStyle));
            //_oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableFNLabdip.AddCell(_oPdfPCell);

            //_oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            //_oPdfPCell = new PdfPCell(new Phrase(_oFabric.ContractorCPName, _oFontStyle));
            //_oPdfPCell.Colspan = 7; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableFNLabdip.AddCell(_oPdfPCell);


            oPdfPTableFNLabdip.CompleteRow();
            #endregion

            #region Swatch Table

            _oPdfPCell = new PdfPCell(new Phrase("MKT Person", _oFontStyle));
            _oPdfPCell.Rowspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableSwatch.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oFabric.MKTPersonName, _oFontStyle_Bold));
            _oPdfPCell.Rowspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableSwatch.AddCell(_oPdfPCell);
            oPdfPTableSwatch.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Light Source (P)", _oFontStyle));
            _oPdfPCell.Rowspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableSwatch.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle_Bold));
            _oPdfPCell.Rowspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableSwatch.AddCell(_oPdfPCell);
            oPdfPTableSwatch.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Light Source (S)", _oFontStyle));
            _oPdfPCell.Rowspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableSwatch.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle_Bold));
            _oPdfPCell.Rowspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableSwatch.AddCell(_oPdfPCell);
            oPdfPTableSwatch.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Composition ", _oFontStyle));
            _oPdfPCell.Rowspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableSwatch.AddCell(_oPdfPCell);

            //_oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oFabric.ProductName, _oFontStyle));
            _oPdfPCell.Rowspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableSwatch.AddCell(_oPdfPCell);
            oPdfPTableSwatch.CompleteRow();

            #endregion

            _oPdfPCell = new PdfPCell(oPdfPTableFNLabdip);
            _oPdfPCell.Colspan = 8; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(oPdfPTableSwatch);
            _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.FixedHeight = 15f;
            _oPdfPCell.Colspan = _nColumn; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

            if (_oFNLabDipDetails.Count() > 0)
            {
                while (_oFNLabDipDetails.Count() > 0)
                {
                    List<FNLabDipDetail> oFabricDetails = new List<FNLabDipDetail>();
                    oFabricDetails = _oFNLabDipDetails.Where(x => x.FabricID == _oFNLabDipDetails.First().FabricID).ToList();
                    this.PrintProductName(oFabricDetails[0].ProductName);
                    HeaderRowsFor();
                    this.DetailTable(oFabricDetails, 0);
                    _oFNLabDipDetails.RemoveAll(x => x.FabricID == oFabricDetails.First().FabricID);
                }
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                //if (_oFabric.ISTwisted == true)
                //{
                //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                //}
                //else
                //{
                //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                //    _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                //}

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
               
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }

            #region Signature Part
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.FixedHeight = 45f;
            _oPdfPCell.Colspan = _nColumn; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("RECEIVED BY \n________________", _oFontStyle));
            _oPdfPCell.Colspan = 9; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("ATHORISED BY \n________________", _oFontStyle));
            _oPdfPCell.Colspan = 3; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 12; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            #endregion
        }
        private void HeaderRowsFor()
        {
            #region Grid Column

            //_oPdfPTable.SetWidths(new float[] { 
            //    28f, 30f, 50f,//Color
            //    30f,//Shade
            //    55f, 45f,//Dyes
            //    40f, //%
            //    40f, 65f,25f,24f //Chemical
            //    ,40f //GL
            //});

            _oPdfPCell = new PdfPCell(new Phrase("Color", _oFontStyle));
            _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Shade", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Dyes", _oFontStyle)); _oPdfPCell.Colspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("GL", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Chemical", _oFontStyle)); _oPdfPCell.Colspan = 4;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("GL", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();
            #endregion
        }
        private void PrintProductName(string sProductName)
        {
            _oPdfPCell = new PdfPCell(new Phrase("Yarn Type : " + sProductName, _oFontStyle_Bold));
            _oPdfPCell.Colspan = 12; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        private void DetailTable(List<FNLabDipDetail> oFabricDetails, int nCount)
        {
            List<FNLabdipShade> oFNLabdipShades = new List<FNLabdipShade>();
            List<FNLabdipRecipe> oFNLabdipRecipesDyes = new List<FNLabdipRecipe>();
            List<FNLabdipRecipe> oFNLabdipRecipesChamical = new List<FNLabdipRecipe>();
            double nQty_Che = 0;
            double nQty_Dyes = 0;
            foreach (FNLabDipDetail oItem in oFabricDetails)
            {
                PdfPTable oPdfPShadeDetailTable = new PdfPTable(5);
                oPdfPShadeDetailTable.SetWidths(new float[] { 30f, 100f, 40f, 154f, 40f });


                _oPdfPCell = new PdfPCell(new Phrase(" Color : " + oItem.ColorName + " \n Panton No: " + oItem.PantonNo + " \n LD No : " + oItem.LabdipNo, _oFontStyle)); _oPdfPCell.Colspan = 3;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
                #region Lab Dip Shades
                oFNLabdipShades = new List<FNLabdipShade>();
                oFNLabdipShades = _oFNLabdipShades.Where(x => x.FNLabDipDetailID == oItem.FNLabDipDetailID).ToList();
                foreach (FNLabdipShade oShadeItem in oFNLabdipShades)
                {
                    nQty_Che = 0;
                    nQty_Dyes = 0;
                    oShadeItem.ShadePercentage = 0;
                    _oPdfPCell = new PdfPCell(new Phrase(oShadeItem.ShadeStr, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPShadeDetailTable.AddCell(_oPdfPCell);

                    PdfPTable oPdfPDyesTable = new PdfPTable(2);
                    PdfPTable oPdfPChemicalTable = new PdfPTable(2);
                    oPdfPDyesTable.SetWidths(new float[] { 100f, 40f });
                    oPdfPChemicalTable.SetWidths(new float[] { 154f, 40f });

                    #region Lab Dip Dyes
                    oFNLabdipRecipesDyes = new List<FNLabdipRecipe>();
                    oFNLabdipRecipesDyes = _oFNLabdipRecipes.Where(x => x.FNLabdipShadeID == oShadeItem.FNLabdipShadeID && x.IsDyes).ToList();

                    int nType = -99;
                    oFNLabdipRecipesDyes = oFNLabdipRecipesDyes.OrderBy(x => x.FabricOrderTypeInInt).ToList();
                    foreach (FNLabdipRecipe oDyesItem in oFNLabdipRecipesDyes)
                    {
                        if (oDyesItem.FabricOrderTypeInInt != nType && _oFabric.FabricOrderType == EnumFabricRequestType.YarnSkein) 
                        {
                            _oPdfPCell = new PdfPCell(new Phrase(oDyesItem.OrderName, _oFontStyle)); _oPdfPCell.Colspan = 2;
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPDyesTable.AddCell(_oPdfPCell);
                        }

                        _oPdfPCell = new PdfPCell(new Phrase(oDyesItem.ProductName, _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPDyesTable.AddCell(_oPdfPCell);

                        if (oShadeItem.Qty > 0)
                        {
                            oDyesItem.PerTage = (oDyesItem.Qty * 100) / oShadeItem.Qty;
                        }
                        nQty_Dyes = nQty_Dyes + oDyesItem.Qty;
                        oShadeItem.ShadePercentage = oShadeItem.ShadePercentage + oDyesItem.PerTage;
                       // oShadeItem.Qty = oShadeItem.Qty + oDyesItem.Qty;
                        if (oDyesItem.IsGL)
                        { _oPdfPCell = new PdfPCell(new Phrase( Global.MillionFormat(oDyesItem.Qty) + " " + oDyesItem.IsGLSt, _oFontStyle)); ; }
                        else
                        {
                            _oPdfPCell = new PdfPCell(new Phrase(oDyesItem.PerTage.ToString("0.0000") + " " + oDyesItem.IsGLSt, _oFontStyle)); ;
                        }
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPDyesTable.AddCell(_oPdfPCell);
                        nType = oDyesItem.FabricOrderTypeInInt;
                    }
                    #endregion

                    #region Lab Dip Chamical
                    oFNLabdipRecipesDyes = new List<FNLabdipRecipe>();
                    oFNLabdipRecipesDyes = _oFNLabdipRecipes.Where(x => x.FNLabdipShadeID == oShadeItem.FNLabdipShadeID && !x.IsDyes).ToList();

                    nType = -99;
                    oFNLabdipRecipesDyes = oFNLabdipRecipesDyes.OrderBy(x => x.FabricOrderTypeInInt).ToList();
                    foreach (FNLabdipRecipe oChamicalItem in oFNLabdipRecipesDyes)
                    {
                        if (oChamicalItem.FabricOrderTypeInInt != nType && _oFabric.FabricOrderType==EnumFabricRequestType.YarnSkein)
                        {
                            _oPdfPCell = new PdfPCell(new Phrase(oChamicalItem.OrderName, _oFontStyle)); _oPdfPCell.Colspan=2;
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPChemicalTable.AddCell(_oPdfPCell);
                        }
                        _oPdfPCell = new PdfPCell(new Phrase(oChamicalItem.ProductName, _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPChemicalTable.AddCell(_oPdfPCell);
                        if (oShadeItem.Qty > 0)
                        {
                            oChamicalItem.PerTage = (oChamicalItem.Qty * 100) / oShadeItem.Qty;
                        }
                        nQty_Che = nQty_Che + oChamicalItem.Qty;
                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(Math.Round(oChamicalItem.Qty, 5)) + " " + oChamicalItem.IsGLSt, _oFontStyle));
                        
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPChemicalTable.AddCell(_oPdfPCell);
                        nType = oChamicalItem.FabricOrderTypeInInt;
                    }
                    #endregion

                    _oPdfPCell = new PdfPCell(oPdfPDyesTable); _oPdfPCell.Colspan = 2;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPShadeDetailTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(oPdfPChemicalTable); _oPdfPCell.Colspan = 2;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPShadeDetailTable.AddCell(_oPdfPCell);
                    oPdfPShadeDetailTable.CompleteRow();

                    #region Qty & Shade(%)
                    _oPdfPCell = new PdfPCell(new Phrase("Total Shade", _oFontStyle_Bold)); _oPdfPCell.Colspan = 2;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPShadeDetailTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_Dyes), _oFontStyle_Bold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPShadeDetailTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle_Bold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPShadeDetailTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(Math.Round(nQty_Che, 5)) + " GL", _oFontStyle_Bold)); _oPdfPCell.Colspan = 2;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPShadeDetailTable.AddCell(_oPdfPCell);
                    oPdfPShadeDetailTable.CompleteRow();
                    #endregion
                }
                //_oPdfPCell = new PdfPCell(oPdfPShadeTable); _oPdfPCell.Colspan = 1;
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;  _oPdfPTable.AddCell(_oPdfPCell);
                #endregion

                _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
                _oPdfPCell = new PdfPCell(oPdfPShadeDetailTable); _oPdfPCell.Colspan = 9;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPTable.CompleteRow();
            }
        }
    }
}
