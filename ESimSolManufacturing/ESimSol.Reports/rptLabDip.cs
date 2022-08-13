
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
    public class rptLabDip
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
        Phrase _oPhrase = new Phrase();
        Company _oCompany = new Company();
        BusinessUnit _oBusinessUnit = new BusinessUnit();
        string _sTemp = "";
        FabricSalesContractDetail _oFabricSalesContractDetail = new FabricSalesContractDetail();
        #endregion

        #region PeapareReport A
        public byte[] PrepareReport(LabDip oLabDip,  Company oCompany,BusinessUnit oBusinessUnit)
        {
            _oLabDip = oLabDip;
            _oCompany = oCompany;
            _oBusinessUnit = oBusinessUnit;
            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f); 
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            _oDocument.SetMargins(25f, 25f, 30f, 30f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_CENTER;

            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();

            //_oPdfPTable.SetWidths(new float[] { 28f, 30f, 60f, 20f, 45f, 45f, 40f, 50f, 50f, 42f, 52f });
            _oPdfPTable.SetWidths(new float[] { 28f, 30f, 50f, 20f, 65f, 45f, 40f, 40f, 65f, 25f, 42f, 22f });
            #endregion
          
            this.PrintHeader("LAB-DIP REQUEST");
            this.PrintBody();
            _oPdfPTable.HeaderRows = 2;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

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
           
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Rowspan = 2;
            oPdfPTable.AddCell(_oPdfPCell);

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
            _oFontStyle = FontFactory.GetFont("Tahoma", 14f,1);
            _oPdfPCell = new PdfPCell(new Phrase(sHeader + ((_oLabDip.ISTwisted == true) ? " (Twisting Knitting)" : "") + " " + ((_oLabDip.OrderStatus < EnumLabdipOrderStatus.Approve) ? "(unauthorized)" : ""), _oFontStyle));
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

        #region Report Body
        private void PrintBody()
        {
            PdfPTable oPdfPTableLabdip = new PdfPTable(9);
            oPdfPTableLabdip.SetWidths(new float[] { 35f, 30f, 40f, 20f, 45f, 45f, 30f, 60f, 50f });

            PdfPTable oPdfPTableSwatch = new PdfPTable(2);
            oPdfPTableSwatch.SetWidths(new float[] { 40f, 50f });

            #region Top part

            #region Labdip Table
            /*------------Row-1-----------*/
            _oFontStyle_Bold = FontFactory.GetFont("Tahoma", 10f, 1);
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            //_oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Order Date ", _oFontStyle));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableLabdip.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase(_oLabDip.OrderDateStr, _oFontStyle));
            _oPdfPCell.Colspan = 7; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableLabdip.AddCell(_oPdfPCell);
            oPdfPTableLabdip.CompleteRow();

            /*------------Row-2-----------*/
            //_oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("LabDip No ", _oFontStyle));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableLabdip.AddCell(_oPdfPCell);

            //_oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oLabDip.LabdipNo, _oFontStyle_Bold));
            _oPdfPCell.Colspan = 7; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableLabdip.AddCell(_oPdfPCell);
            oPdfPTableLabdip.CompleteRow();

            /*------------Row-3-----------*/
            //_oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Buyer Name ", _oFontStyle));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableLabdip.AddCell(_oPdfPCell);

            //_oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oLabDip.ContractorName, _oFontStyle_Bold));
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
            
            _oPdfPCell = new PdfPCell(new Phrase((_oLabDip.LabDipDetails.FirstOrDefault() != null ? _oLabDip.LabDipDetails.FirstOrDefault().KnitPlyYarnInString + " Swatch" : "Swatch Type"), _oFontStyle_Bold));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableSwatch.AddCell(_oPdfPCell);
            oPdfPTableSwatch.CompleteRow();

            
            _oPdfPCell = new PdfPCell(new Phrase((_oLabDip.LabDipDetails.FirstOrDefault() != null ? "# "+_oLabDip.LabDipDetails.FirstOrDefault().ShadeCount + " Shades" : ""), _oFontStyle));
            _oPdfPCell.Rowspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableSwatch.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase((_oLabDip.LabDipDetails.FirstOrDefault() != null ? "# " + _oLabDip.LabDipDetails.FirstOrDefault().ColorSet + " Set" : ""), _oFontStyle));
            _oPdfPCell.Rowspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableSwatch.AddCell(_oPdfPCell);
            oPdfPTableSwatch.CompleteRow();

            
            _oPdfPCell = new PdfPCell(new Phrase("Light Source", _oFontStyle));
            _oPdfPCell.Rowspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableSwatch.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oLabDip.LightSourceName, _oFontStyle_Bold));
            _oPdfPCell.Rowspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableSwatch.AddCell(_oPdfPCell);
            oPdfPTableSwatch.CompleteRow();

            #endregion

            _oPdfPCell = new PdfPCell(oPdfPTableLabdip);
            _oPdfPCell.Colspan = 8;  _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(oPdfPTableSwatch);
            _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Colspan = _nColumn; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

            #region Grid Column
            _oPdfPCell = new PdfPCell(new Phrase("#SL", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Yarn Type", _oFontStyle));
            _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Color Name", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Color No", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            if (_oLabDip.ISTwisted ==true)
            {
                _oPdfPCell = new PdfPCell(new Phrase("No of Ply", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Twist", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

               _oPdfPCell = new PdfPCell(new Phrase("Panton no", _oFontStyle));
               _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("Panton no", _oFontStyle));
                _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }
            _oPdfPCell = new PdfPCell(new Phrase("Remarks", _oFontStyle));
            _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            if(_oLabDip.LabDipDetails.Count()>0)
            {
                int nCount = 0;

                if (_oLabDip.ISTwisted == true)
                {
                    List<LabDipDetail> oTWGLDDetails = new List<LabDipDetail>();
                    List<LabDipDetail> oLDDetails = new List<LabDipDetail>();
                    List<LabDipDetail> oTempLDDetails = new List<LabDipDetail>();

                    oTWGLDDetails = _oLabDip.LabDipDetails.Where(x => x.TwistedGroup > 0).ToList();
                    oLDDetails=_oLabDip.LabDipDetails.Where(x=>x.TwistedGroup==0).ToList();

                    while (_oLabDip.LabDipDetails.Count() > 0)
                    {
                        if (oTWGLDDetails.FirstOrDefault() != null && _oLabDip.LabDipDetails.FirstOrDefault().LabDipDetailID == oTWGLDDetails.FirstOrDefault().LabDipDetailID)
                        {
                            oTempLDDetails = oTWGLDDetails.Where(x => x.TwistedGroup == oTWGLDDetails.FirstOrDefault().TwistedGroup).ToList();

                            _oLabDip.LabDipDetails.RemoveAll(x => x.TwistedGroup == oTempLDDetails.FirstOrDefault().TwistedGroup);
                            oTWGLDDetails.RemoveAll(x => x.TwistedGroup == oTempLDDetails.FirstOrDefault().TwistedGroup);

                        }
                        else if (oLDDetails.FirstOrDefault() != null && _oLabDip.LabDipDetails.FirstOrDefault().LabDipDetailID == oLDDetails.FirstOrDefault().LabDipDetailID)
                        {
                            oTempLDDetails = oLDDetails.Where(x => x.LabDipDetailID == oLDDetails.FirstOrDefault().LabDipDetailID).ToList();

                            _oLabDip.LabDipDetails.RemoveAll(x => x.LabDipDetailID == oTempLDDetails.FirstOrDefault().LabDipDetailID);
                            oLDDetails.RemoveAll(x => x.LabDipDetailID == oTempLDDetails.FirstOrDefault().LabDipDetailID);
                        }
                        this.DetailTable(oTempLDDetails, ref nCount, true);
                    }
                }
                else
                {
                    while (_oLabDip.LabDipDetails.Count() > 0)
                    {
                        List<LabDipDetail> oLabDipDetails = new List<LabDipDetail>();
                        oLabDipDetails = _oLabDip.LabDipDetails.Where(x => x.ProductID == _oLabDip.LabDipDetails.First().ProductID).ToList();
                        _oLabDip.LabDipDetails.RemoveAll(x => x.ProductID == oLabDipDetails.First().ProductID);

                        this.DetailTable(oLabDipDetails, ref nCount, false);
                       
                    }
                }
          
            }
           
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 15f; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 15f; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 15f; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 15f; _oPdfPTable.AddCell(_oPdfPCell);

                if (_oLabDip.ISTwisted==true)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 15f; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 15f; _oPdfPTable.AddCell(_oPdfPCell);
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 15f; _oPdfPTable.AddCell(_oPdfPCell);
                }
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 15f; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion

            #region Bottom part

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Colspan = _nColumn; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase("Req. Date ", _oFontStyle));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE;  _oPdfPTable.AddCell(_oPdfPCell);
           
            _oPdfPCell = new PdfPCell(new Phrase(_oLabDip.SeekingDateStr, _oFontStyle));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Buyer Ref/Master Buyer", _oFontStyle));
            _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oLabDip.BuyerRefNo + "" + _oLabDip.PH, _oFontStyle));
            _oPdfPCell.Colspan = 5; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.ShortName+ " Concern Person", _oFontStyle));
            _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
           
            _oPdfPCell = new PdfPCell(new Phrase(_oLabDip.MktPerson, _oFontStyle));
            _oPdfPCell.Colspan = _nColumn-3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Buyer Concern Person", _oFontStyle));
            _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oLabDip.ContractorCPName, _oFontStyle));
            _oPdfPCell.Colspan = _nColumn-3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            if (!string.IsNullOrEmpty(_oLabDip.DeliveryNote))
            {
                _oPdfPCell = new PdfPCell(new Phrase("Delivery Note", _oFontStyle));
                _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oLabDip.DeliveryNote, _oFontStyle));
                _oPdfPCell.Colspan = _nColumn - 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            if (!string.IsNullOrEmpty(_oLabDip.Note))
            {
                _oPdfPCell = new PdfPCell(new Phrase("Note ", _oFontStyle));
                _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oLabDip.Note, _oFontStyle));
                _oPdfPCell.Colspan = _nColumn - 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }


            #endregion

            #region Signature Part
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Colspan = _nColumn; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 20f; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase("Executive", _oFontStyle));
            _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Marketing Manager", _oFontStyle));
            _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Managing Director", _oFontStyle));
            _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 30f; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 30f; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 30f; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }

        private void DetailTable(List<LabDipDetail> oLabDipDetails,ref int nCount, bool IsTwisted)
        {
            bool IsSpanTwist = true;
            int nIteration = 0;
            int nIndex = 0;
           
            if (IsTwisted)
            {
                oLabDipDetails = oLabDipDetails.OrderBy(x => x.ProductID).ToList();

                _oPdfPCell = new PdfPCell(new Phrase((++nCount).ToString(), _oFontStyle));
                _oPdfPCell.Rowspan = oLabDipDetails.Count(); _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 40f; _oPdfPTable.AddCell(_oPdfPCell);

                foreach (LabDipDetail oItem in oLabDipDetails)
                {
                    ++nIteration;
                    ++nIndex;
                    if (nIteration == nIndex)
                    {
                        if (oItem.TwistedGroup > 0)
                        {
                            nIteration = nIteration + oLabDipDetails.Where(x => x.ProductID == oItem.ProductID).Count()-1;
                        }
                        _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName, _oFontStyle));
                        _oPdfPCell.Colspan = 3; _oPdfPCell.Rowspan = oLabDipDetails.Where(x => x.ProductID == oItem.ProductID).Count(); _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 70f; _oPdfPTable.AddCell(_oPdfPCell);
                    }
                    else
                    {
                        --nIteration;
                    }
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ColorName, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 70f; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ColorNo, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 70f; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.Combo.ToString(), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 70f; _oPdfPTable.AddCell(_oPdfPCell);

                    if (IsSpanTwist)
                    {
                        if (oItem.TwistedGroup > 0) IsSpanTwist = false;
                        _oPdfPCell = new PdfPCell(new Phrase((oItem.TwistedGroup > 0) ? "Twisted" : "", _oFontStyle));
                        _oPdfPCell.Rowspan = (oItem.TwistedGroup > 0) ? oLabDipDetails.Count() : 1; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 70f; _oPdfPTable.AddCell(_oPdfPCell);
                    }

                    _oPhrase = new Phrase();
                    _sTemp = "";
                    if (oItem.KnitPlyYarn != EnumKnitPlyYarn.None)
                    {
                        _sTemp = _sTemp + "" + oItem.KnitPlyYarnInString;
                    }

                    if (!String.IsNullOrEmpty(oItem.RGB))
                    {
                        _sTemp = _sTemp + " Style:" + oItem.RGB;
                    }
                    if (oItem.Gauge > 0)
                    {
                        _sTemp = _sTemp + " Gauge:" + oItem.Gauge;
                    }
                    if (oItem.Combo > 0)
                    {
                        _sTemp = _sTemp + " No of Ply:" + oItem.Combo.ToString();
                    }
                    if (!String.IsNullOrEmpty(oItem.PantonNo))
                    {
                        _oPhrase.Add(new Chunk(oItem.PantonNo, _oFontStyle));
                    }
                    if (!String.IsNullOrEmpty(_sTemp))
                    {
                        _oPhrase.Add(new Chunk("\n" + _sTemp, FontFactory.GetFont("Tahoma", 7.5f, 0)));
                    }
                    //_oPdfPCell = new PdfPCell(new Phrase(oItem.PantonNo, _oFontStyle));
                    _oPdfPCell = new PdfPCell(_oPhrase);
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 70f; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.RefNo, _oFontStyle));
                    _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 70f; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPTable.CompleteRow();
                }
            }
            else
            {

                _oPdfPCell = new PdfPCell(new Phrase((++nCount).ToString(), _oFontStyle));
                _oPdfPCell.Rowspan = oLabDipDetails.Count(); _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 70f; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oLabDipDetails.First().ProductName, _oFontStyle));
                _oPdfPCell.Colspan = 3; _oPdfPCell.Rowspan = oLabDipDetails.Count(); _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 70f; _oPdfPTable.AddCell(_oPdfPCell);

                foreach (LabDipDetail oItem in oLabDipDetails)
                {

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ColorName, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.MinimumHeight = 70f; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ColorNo, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.MinimumHeight = 70f; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPhrase = new Phrase();
                    _sTemp = "";
                    if (oItem.KnitPlyYarn!=EnumKnitPlyYarn.None)
                    {
                        _sTemp =_sTemp+""+ oItem.KnitPlyYarnInString;
                    }
                  
                    if (!String.IsNullOrEmpty(oItem.RGB))
                    {
                        _sTemp = _sTemp + " Style:" + oItem.RGB;
                    }
                    if (oItem.Gauge > 0)
                    {
                        _sTemp = _sTemp + " Gauge:" + oItem.Gauge;
                    }
                    if (oItem.Combo > 0)
                    {
                        _sTemp = _sTemp + " No of Ply:" + oItem.Combo.ToString();
                    }
                    if (!String.IsNullOrEmpty(oItem.PantonNo))
                    {
                        _oPhrase.Add(new Chunk(oItem.PantonNo, _oFontStyle));
                    }
                    if (!String.IsNullOrEmpty(_sTemp))
                    {
                        _oPhrase.Add(new Chunk("\n" + _sTemp, FontFactory.GetFont("Tahoma", 7.5f, 0)));
                    }
                   // _oPdfPCell = new PdfPCell(new Phrase(oItem.PantonNo, _oFontStyle));
                    _oPdfPCell = new PdfPCell(_oPhrase);
                    _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.MinimumHeight = 70f; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.RefNo, _oFontStyle));
                    _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.MinimumHeight = 70f; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPTable.CompleteRow();


                }
            }
        }
        #endregion
        #endregion

        #region PeapareReport B
        public byte[] PrepareReport_B(LabDip oLabDip, Company oCompany, BusinessUnit oBusinessUnit, FabricSalesContractDetail oFabricSalesContractDetail)
        {
            _oLabDip = oLabDip;
            _oCompany = oCompany;
            _oFabricSalesContractDetail = oFabricSalesContractDetail;
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
            _oPdfPTable.SetWidths(new float[] { 28f, 30f, 50f, 20f, 65f, 45f, 40f, 40f, 65f, 25f, 42f, 22f });
            #endregion

            this.PrintHeader("SAMPLE YARN DYEING PROGRAM");
            this.PrintBody_B();
            _oPdfPTable.HeaderRows = 2;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Body
        private void PrintBody_B()
        {
            PdfPTable oPdfPTableLabdip = new PdfPTable(9);
            oPdfPTableLabdip.SetWidths(new float[] { 35f, 30f, 40f, 20f, 45f, 45f, 30f, 60f, 50f });

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
            _oPdfPCell = new PdfPCell(new Phrase("Order No ", _oFontStyle));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableLabdip.AddCell(_oPdfPCell);

            //_oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oLabDip.OrderNo, _oFontStyle));
            _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableLabdip.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //_oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableLabdip.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableLabdip.AddCell(_oPdfPCell);


            oPdfPTableLabdip.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("LD Order No ", _oFontStyle));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableLabdip.AddCell(_oPdfPCell);

            //_oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oLabDip.LabdipNo, _oFontStyle));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableLabdip.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Mkt Ref:", _oFontStyle));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableLabdip.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oLabDip.FabricNo, _oFontStyle));
            _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableLabdip.AddCell(_oPdfPCell);


            oPdfPTableLabdip.CompleteRow();

            /*------------Row-3-----------*/
            //_oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Style No ", _oFontStyle));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableLabdip.AddCell(_oPdfPCell);

            //_oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oFabricSalesContractDetail.StyleNo, _oFontStyle));
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
            _oPdfPCell = new PdfPCell(new Phrase("Composition ", _oFontStyle));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableLabdip.AddCell(_oPdfPCell);

            //_oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oFabricSalesContractDetail.ProductName, _oFontStyle));
            _oPdfPCell.Colspan = 7; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableLabdip.AddCell(_oPdfPCell);
            oPdfPTableLabdip.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Construction ", _oFontStyle));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableLabdip.AddCell(_oPdfPCell);

            //_oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oFabricSalesContractDetail.Construction, _oFontStyle));
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

            _oPdfPCell = new PdfPCell(new Phrase("Light Source", _oFontStyle));
            _oPdfPCell.Rowspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableSwatch.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oLabDip.LightSourceName, _oFontStyle_Bold));
            _oPdfPCell.Rowspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableSwatch.AddCell(_oPdfPCell);
            oPdfPTableSwatch.CompleteRow();

            //_oPdfPCell = new PdfPCell(new Phrase("Light Source (S)", _oFontStyle));
            //_oPdfPCell.Rowspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableSwatch.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase(_oLabDip.LightSourceName, _oFontStyle_Bold));
            //_oPdfPCell.Rowspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableSwatch.AddCell(_oPdfPCell);
            //oPdfPTableSwatch.CompleteRow();


            #endregion

            _oPdfPCell = new PdfPCell(oPdfPTableLabdip);
            _oPdfPCell.Colspan = 8; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(oPdfPTableSwatch);
            _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Colspan = _nColumn; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
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
                    HeaderRowsFor_B();
                    this.DetailTable_B(oLabDipDetails, 0);
                }
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 15f; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 15f; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 15f; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 15f; _oPdfPTable.AddCell(_oPdfPCell);

                if (_oLabDip.ISTwisted == true)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 15f; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 15f; _oPdfPTable.AddCell(_oPdfPCell);
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 15f; _oPdfPTable.AddCell(_oPdfPCell);
                }
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 15f; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            
            #region Bottom part

            //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //_oPdfPCell.Colspan = _nColumn; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();


            //_oPdfPCell = new PdfPCell(new Phrase("Req. Date ", _oFontStyle));
            //_oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase(_oLabDip.SeekingDateStr, _oFontStyle));
            //_oPdfPCell.Colspan = _nColumn; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();

            //_oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.ShortName + " Concern Person", _oFontStyle));
            //_oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase(_oLabDip.MktPerson, _oFontStyle));
            //_oPdfPCell.Colspan = _nColumn - 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();

            //_oPdfPCell = new PdfPCell(new Phrase("Buyer Concern Person", _oFontStyle));
            //_oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase(_oLabDip.ContractorCPName, _oFontStyle));
            //_oPdfPCell.Colspan = _nColumn - 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();

            //_oPdfPCell = new PdfPCell(new Phrase("Note ", _oFontStyle));
            //_oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase(_oLabDip.Note, _oFontStyle));
            //_oPdfPCell.Colspan = _nColumn - 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();

            #endregion

            #region Signature Part
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Colspan = _nColumn; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.MinimumHeight = 42f; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase("RECEIVED BY", _oFontStyle));
            _oPdfPCell.Colspan = 9; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("AUTHORIZED BY", _oFontStyle));
            _oPdfPCell.Colspan = 3; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 12; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 30f; _oPdfPTable.AddCell(_oPdfPCell);

            #endregion
        }
        private void HeaderRowsFor_B()
        {
            #region Grid Column
            _oPdfPCell = new PdfPCell(new Phrase("#SL", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Color Name", _oFontStyle));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Panton No", _oFontStyle));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            //if (_oLabDip.ISTwisted == true)
            //{
            //    _oPdfPCell = new PdfPCell(new Phrase("No of Ply", _oFontStyle));
            //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            //    _oPdfPCell = new PdfPCell(new Phrase("Twist", _oFontStyle));
            //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            //    _oPdfPCell = new PdfPCell(new Phrase("Panton no", _oFontStyle));
            //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //}
            //else
            //{

            _oPdfPCell = new PdfPCell(new Phrase("LD No", _oFontStyle));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("App. Shade", _oFontStyle));
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            //}
            _oPdfPCell = new PdfPCell(new Phrase("Remarks", _oFontStyle));
            _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        private void PrintProductName(string sProductName) 
        {
            _oPdfPCell = new PdfPCell(new Phrase("Yarn Type : " + sProductName, _oFontStyle_Bold));
            _oPdfPCell.Colspan = 12; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        private void DetailTable_B(List<LabDipDetail> oLabDipDetails, int nCount)
        {
            foreach (LabDipDetail oItem in oLabDipDetails)
            {
                _oPdfPCell = new PdfPCell(new Phrase((++nCount).ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 25f; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ColorName, _oFontStyle)); _oPdfPCell.Colspan = 2;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 25f; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.PantonNo, _oFontStyle)); _oPdfPCell.Colspan = 2;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 25f; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ColorNo, _oFontStyle)); _oPdfPCell.Colspan = 2;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 25f; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); 
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 25f; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.RefNo, _oFontStyle));
                _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 25f; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPTable.CompleteRow();
            }
        }
        #endregion

        #endregion
    }
}
