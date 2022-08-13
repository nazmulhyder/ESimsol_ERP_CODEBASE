
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
    public class rptLabDipRecipe
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyle_Bold;
        int _nColumn = 12;
        PdfPTable _oPdfPTable = new PdfPTable(12);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        LabDip _oLabDip = new LabDip();
        List<LabdipShade> _oLabdipShades = new List<LabdipShade>();
        List<LabdipRecipe> _oLabdipRecipes = new List<LabdipRecipe>();
        Company _oCompany = new Company();
        Fabric _oFabric = new Fabric();
        BusinessUnit _oBusinessUnit = new BusinessUnit();
        #endregion

        #region PeapareReport
        public byte[] PrepareReport(LabDip oLabDip, Company oCompany, BusinessUnit oBusinessUnit, List<LabdipShade> oLabdipShades, List<LabdipRecipe> oLabdipRecipes, Fabric oFabric)
        {
            _oLabDip = oLabDip;
            _oLabdipShades = oLabdipShades;
            _oLabdipRecipes = oLabdipRecipes;
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
            _oPdfPTable.SetWidths(new float[] { 28f, 30f, 50f, 30f, 55f, 45f, 40f, 40f, 65f, 25f, 24f, 40f });
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
            oPdfPTable.SetWidths(new float[] { 70f, 300.5f, 80f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            if (_oCompany.CompanyLogo != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(60f, 35f);
                _oPdfPCell = new PdfPCell(_oImag);
                _oPdfPCell.FixedHeight = 80f;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
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
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.Colspan = _nColumn; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            #endregion
            #region Title
            _oFontStyle = FontFactory.GetFont("Tahoma", 14f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(sHeader, _oFontStyle));
            _oPdfPCell.Colspan = _nColumn;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            //_oPdfPCell.FixedHeight = 20f;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.ExtraParagraphSpace = 8f;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        #endregion

        private void PrintBody()
        {
            PdfPTable oPdfPTableLabdip = new PdfPTable(9);
            oPdfPTableLabdip.SetWidths(new float[] { 55f, 20f, 10f, 20f, 45f, 45f, 30f, 60f, 50f });

            PdfPTable oPdfPTableSwatch = new PdfPTable(2);
            oPdfPTableSwatch.SetWidths(new float[] { 40f, 50f });

            #region Top part

            #region Labdip Table
            /*------------Row-1-----------*/
            _oFontStyle_Bold = FontFactory.GetFont("Tahoma", 10f, 1);
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            //_oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Buyer ", _oFontStyle));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableLabdip.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase(_oLabDip.ContractorName, _oFontStyle));
            _oPdfPCell.Colspan = 7; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableLabdip.AddCell(_oPdfPCell);
            oPdfPTableLabdip.CompleteRow();

            /*------------Row-2-----------*/
            _oFontStyle_Bold = FontFactory.GetFont("Tahoma", 10f, 1);
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            //_oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Buying House ", _oFontStyle));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableLabdip.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase(_oLabDip.DeliveryToName, _oFontStyle));
            _oPdfPCell.Colspan = 7; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableLabdip.AddCell(_oPdfPCell);
            oPdfPTableLabdip.CompleteRow();

            /*------------Row-2-----------*/
            //_oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(_oLabDip.OrderReferenceTypeSt + " No ", _oFontStyle));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableLabdip.AddCell(_oPdfPCell);

            //_oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oLabDip.OrderNo, _oFontStyle_Bold));
            _oPdfPCell.Colspan = 7; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableLabdip.AddCell(_oPdfPCell);
            oPdfPTableLabdip.CompleteRow();

            /*------------Row-3-----------*/
            //_oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Lab Order No ", _oFontStyle));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableLabdip.AddCell(_oPdfPCell);

            //_oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oLabDip.LabdipNo, _oFontStyle));
            _oPdfPCell.Colspan = 7; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableLabdip.AddCell(_oPdfPCell);
            oPdfPTableLabdip.CompleteRow();



            /*------------Row-5-----------*/
            //_oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            //_oPdfPCell = new PdfPCell(new Phrase("Yarn Count ", _oFontStyle));
            //_oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableLabdip.AddCell(_oPdfPCell);

            //_oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            //_oPdfPCell = new PdfPCell(new Phrase("YC-123", _oFontStyle));
            //_oPdfPCell.Colspan = 7; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableLabdip.AddCell(_oPdfPCell);
            //oPdfPTableLabdip.CompleteRow();

            /*------------Row-6-----------*/
            //_oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Style No", _oFontStyle));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableLabdip.AddCell(_oPdfPCell);

            //_oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oFabric.StyleNo, _oFontStyle));
            _oPdfPCell.Colspan = 7; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableLabdip.AddCell(_oPdfPCell);
            oPdfPTableLabdip.CompleteRow();

            //_oFontStyle = FontFactory.GetFont("Tahoma", 7f, 0);
            //_oPdfPCell = new PdfPCell(new Phrase("Buyer Concern Person ", _oFontStyle));
            //_oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableLabdip.AddCell(_oPdfPCell);

            //_oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            //_oPdfPCell = new PdfPCell(new Phrase(_oLabDip.ContractorCPName, _oFontStyle));
            //_oPdfPCell.Colspan = 7; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableLabdip.AddCell(_oPdfPCell);


            oPdfPTableLabdip.CompleteRow();
            #endregion

            #region Swatch Table

            _oPdfPCell = new PdfPCell(new Phrase("MKT Person", _oFontStyle));
            _oPdfPCell.Rowspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableSwatch.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oLabDip.MktPerson, _oFontStyle_Bold));
            _oPdfPCell.Rowspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableSwatch.AddCell(_oPdfPCell);
            oPdfPTableSwatch.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Light Source (P)", _oFontStyle));
            _oPdfPCell.Rowspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableSwatch.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oLabDip.LightSourceName, _oFontStyle_Bold));
            _oPdfPCell.Rowspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableSwatch.AddCell(_oPdfPCell);
            oPdfPTableSwatch.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Light Source (S)", _oFontStyle));
            _oPdfPCell.Rowspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableSwatch.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oLabDip.LightSourceName, _oFontStyle_Bold));
            _oPdfPCell.Rowspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableSwatch.AddCell(_oPdfPCell);
            oPdfPTableSwatch.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Composition ", _oFontStyle));
            _oPdfPCell.Rowspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableSwatch.AddCell(_oPdfPCell);

            //_oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oFabric.ProductName, _oFontStyle));
            _oPdfPCell.Rowspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableSwatch.AddCell(_oPdfPCell);
            oPdfPTableSwatch.CompleteRow();

            #endregion

            _oPdfPCell = new PdfPCell(oPdfPTableLabdip);
            _oPdfPCell.Colspan = 8; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(oPdfPTableSwatch);
            _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.FixedHeight = 15f;
            _oPdfPCell.Colspan = _nColumn; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

            if (_oLabDip.LabDipDetails.Count() > 0)
            {
                while (_oLabDip.LabDipDetails.Count() > 0)
                {
                    List<LabDipDetail> oLabDipDetails = new List<LabDipDetail>();
                    oLabDipDetails = _oLabDip.LabDipDetails.Where(x => x.ProductID == _oLabDip.LabDipDetails.First().ProductID).ToList();
                    _oLabDip.LabDipDetails.RemoveAll(x => x.ProductID == oLabDipDetails.First().ProductID);
                    this.PrintProductName(oLabDipDetails[0].ProductName);
                    HeaderRowsFor();
                    this.DetailTable(oLabDipDetails, 0);
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

                if (_oLabDip.ISTwisted == true)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
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

            _oPdfPCell = new PdfPCell(new Phrase("%", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Chemical", _oFontStyle)); _oPdfPCell.Colspan = 4;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("GL / %", _oFontStyle));
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
        private void DetailTable(List<LabDipDetail> oLabDipDetails, int nCount)
        {
            List<LabdipShade> oLabdipShades = new List<LabdipShade>();
            List<LabdipRecipe> oLabdipRecipesDyes = new List<LabdipRecipe>();
            List<LabdipRecipe> oLabdipRecipesChamical = new List<LabdipRecipe>();
            double nQty_Che = 0;
            foreach (LabDipDetail oItem in oLabDipDetails)
            {
                PdfPTable oPdfPShadeDetailTable = new PdfPTable(5);
                oPdfPShadeDetailTable.SetWidths(new float[] { 30f, 100f, 40f, 154f, 40f });
                PdfPTable oPdfPDyesTable = new PdfPTable(2);
                //oPdfPDyesTable.SetWidths(new float[] { 100f, 40f });
                PdfPTable oPdfPChemicalTable = new PdfPTable(2);
                //oPdfPChemicalTable.SetWidths(new float[] { 154f, 40f });

                _oPdfPCell = new PdfPCell(new Phrase(" " + oItem.ColorName + " \n Color No: " + oItem.ColorNo + " \n Panton No: " + oItem.PantonNo, _oFontStyle)); _oPdfPCell.Colspan = 3;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
                #region Lab Dip Shades
                oLabdipShades = _oLabdipShades.Where(x => x.LabdipDetailID == oItem.LabDipDetailID).ToList();
                foreach (LabdipShade oShadeItem in oLabdipShades)
                {
                    oPdfPDyesTable = new PdfPTable(2);
                    oPdfPDyesTable.SetWidths(new float[] { 100f, 40f });

                    oPdfPChemicalTable = new PdfPTable(2);
                    oPdfPChemicalTable.SetWidths(new float[] { 154f, 40f });
                    nQty_Che = 0;
                    oShadeItem.ShadePercentage = 0;
                    _oPdfPCell = new PdfPCell(new Phrase(oShadeItem.ShadeStr, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPShadeDetailTable.AddCell(_oPdfPCell);

                    #region Lab Dip Dyes
                    oLabdipRecipesDyes = _oLabdipRecipes.Where(x => x.LabdipShadeID == oShadeItem.LabdipShadeID && x.IsDyes).ToList();
                    foreach (LabdipRecipe oDyesItem in oLabdipRecipesDyes)
                    {
                        _oPdfPCell = new PdfPCell(new Phrase(oDyesItem.ProductName, _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPDyesTable.AddCell(_oPdfPCell);
                        if (oShadeItem.Qty > 0)
                        {
                            oDyesItem.PerTage = (oDyesItem.Qty * 100) / oShadeItem.Qty;
                        }
                        oShadeItem.ShadePercentage = oShadeItem.ShadePercentage + oDyesItem.PerTage;
                        _oPdfPCell = new PdfPCell(new Phrase(oDyesItem.PerTage.ToString("0.00000"), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPDyesTable.AddCell(_oPdfPCell);
                    }
                    #endregion

                    #region Lab Dip Chamical
                    oLabdipRecipesDyes = _oLabdipRecipes.Where(x => x.LabdipShadeID == oShadeItem.LabdipShadeID && !x.IsDyes).ToList();
                    foreach (LabdipRecipe oChamicalItem in oLabdipRecipesDyes)
                    {
                        _oPdfPCell = new PdfPCell(new Phrase(oChamicalItem.ProductName, _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPChemicalTable.AddCell(_oPdfPCell);
                        if (oShadeItem.Qty > 0)
                        {
                            oChamicalItem.PerTage = (oChamicalItem.Qty * 100) / oShadeItem.Qty;
                        }
                        nQty_Che = nQty_Che + oChamicalItem.Qty;
                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(Math.Round(oChamicalItem.Qty, 5)) + " " + oChamicalItem.IsGLSt, _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPChemicalTable.AddCell(_oPdfPCell);
                    }
                    #endregion

                    _oPdfPCell = new PdfPCell(oPdfPDyesTable); _oPdfPCell.Colspan = 2;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPShadeDetailTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(oPdfPChemicalTable); _oPdfPCell.Colspan = 2;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPShadeDetailTable.AddCell(_oPdfPCell);
                    oPdfPShadeDetailTable.CompleteRow();

                    #region Qty & Shade(%)
                    _oPdfPCell = new PdfPCell(new Phrase("Total Shade(%) ", _oFontStyle_Bold)); _oPdfPCell.Colspan = 2;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPShadeDetailTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oShadeItem.ShadePercentage.ToString("0.00000"), _oFontStyle_Bold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPShadeDetailTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle_Bold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPShadeDetailTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(Math.Round(nQty_Che, 5)) + " GL", _oFontStyle_Bold)); _oPdfPCell.Colspan = 2;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPShadeDetailTable.AddCell(_oPdfPCell);
                    oPdfPShadeDetailTable.CompleteRow();
                    #endregion
                }
                #endregion

                _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
                _oPdfPCell = new PdfPCell(oPdfPShadeDetailTable); _oPdfPCell.Colspan = 9;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPTable.CompleteRow();
            }
        }
    }
}
