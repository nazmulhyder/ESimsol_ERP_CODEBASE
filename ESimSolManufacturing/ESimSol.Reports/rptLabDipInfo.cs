
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
    public class rptLabDipInfo
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyleBold;
        iTextSharp.text.Font _oFontStyleBoldUnderLine;
        int _nColumn = 1;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        LabDip _oLabDip = new LabDip();

        List<LabdipRecipe> _oLabdipRecipes = new List<LabdipRecipe>();
        List<LabdipShade> _oLabdipShades = new List<LabdipShade>();
        List<LabDipDetail> _oLabDipDetails = new List<LabDipDetail>();
        List<LabdipHistory> _oLabdipHistorys = new List<LabdipHistory>();
        List<LabdipChallan> _oLabdipChallans = new List<LabdipChallan>();
        Company _oCompany = new Company();
        BusinessUnit _oBusinessUnit = new BusinessUnit();
        #endregion

        public byte[] PrepareReport(LabDip oLabDip, Company oCompany, BusinessUnit oBusinessUnit, List<LabdipShade> oLabdipShades, List<LabdipRecipe> oLabdipRecipes, List<LabdipHistory> oLabdipHistorys, List<LabdipChallan> oLabdipChallans)
        {
            _oLabDip = oLabDip;
            _oCompany = oCompany;
            _oBusinessUnit = oBusinessUnit;
            _oLabdipShades = oLabdipShades;
            _oLabdipRecipes = oLabdipRecipes;
            _oLabDipDetails = _oLabDip.LabDipDetails;
            _oLabdipHistorys = oLabdipHistorys;
            _oLabdipChallans = oLabdipChallans;
            #region Page Setup
            _oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
            _oDocument.SetMargins(20f, 20f, 10f, 10f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_CENTER;

            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();
          
            _oPdfPTable.SetWidths(new float[] { 595f });
            #endregion

            this.PrintHeader();
            this.PrintBody();
            if (_oLabDip.ISTwisted == true)
            {
                this.SetDetail_IsTwist();
            }
            else
            {
                this.SetDetail();
            }
            this.PrintHistory();

            if (_oLabdipChallans.Any())
                this.PrintLabdipDelivery();
            //_oPdfPTable.HeaderRows = 4;
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
            _oFontStyle = FontFactory.GetFont("Tahoma", 14f,1);
            _oPdfPCell = new PdfPCell(new Phrase("LAB-DIP ORDER" + ((_oLabDip.ISTwisted == true) ? " (Twisting Knitting)" : "") + " " + ((_oLabDip.OrderStatus < EnumLabdipOrderStatus.Approve) ? "(unauthorized)" : ""), _oFontStyle));
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

            _oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, 1);
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);

            #region Top part
            PdfPTable oPdfPTableLabdip = new PdfPTable(4);
            oPdfPTableLabdip.SetWidths(new float[] { 100f, 295f, 100f,100f });
            
            _oPdfPCell = new PdfPCell(new Phrase("Order Date ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableLabdip.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oLabDip.OrderDateStr, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableLabdip.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase((_oLabDip.LabDipDetails.FirstOrDefault() != null ? _oLabDip.LabDipDetails.FirstOrDefault().KnitPlyYarnInString + " Swatch" : "Swatch Type"), _oFontStyleBold));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableLabdip.AddCell(_oPdfPCell);
            oPdfPTableLabdip.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("LabDip No ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableLabdip.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oLabDip.LabdipNo, _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableLabdip.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase((_oLabDip.LabDipDetails.FirstOrDefault() != null ? "# " + _oLabDip.LabDipDetails.FirstOrDefault().ShadeCount + " Shades" : ""), _oFontStyle));
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableLabdip.AddCell(_oPdfPCell);

             _oPdfPCell = new PdfPCell(new Phrase((_oLabDip.LabDipDetails.FirstOrDefault() != null ? "# " + _oLabDip.LabDipDetails.FirstOrDefault().ColorSet + " Set" : ""), _oFontStyle));
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableLabdip.AddCell(_oPdfPCell);
            oPdfPTableLabdip.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Buyer Name ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableLabdip.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oLabDip.ContractorName, _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableLabdip.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Light Source", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableLabdip.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oLabDip.LightSourceName, _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableLabdip.AddCell(_oPdfPCell);
            oPdfPTableLabdip.CompleteRow();
               


            _oPdfPCell = new PdfPCell(oPdfPTableLabdip);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion
          


            #region Balnk Space
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight =8f; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion


            #region 2nd part
            oPdfPTableLabdip = new PdfPTable(4);
            oPdfPTableLabdip.SetWidths(new float[] { 150f, 145f, 150f, 150f });

            _oPdfPCell = new PdfPCell(new Phrase("Requirement Date ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableLabdip.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oLabDip.SeekingDateStr, _oFontStyle));
            _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableLabdip.AddCell(_oPdfPCell);
            oPdfPTableLabdip.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTableLabdip);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            oPdfPTableLabdip = new PdfPTable(4);
            oPdfPTableLabdip.SetWidths(new float[] { 200f, 195f, 150f, 150f });

            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.ShortName + " Concern Person", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableLabdip.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oLabDip.MktPerson, _oFontStyle));
            _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableLabdip.AddCell(_oPdfPCell);
            oPdfPTableLabdip.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Buyer Concern Person", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableLabdip.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oLabDip.ContractorCPName, _oFontStyle));
            _oPdfPCell.Colspan =3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableLabdip.AddCell(_oPdfPCell);
            oPdfPTableLabdip.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTableLabdip);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            if (!String.IsNullOrEmpty(_oLabDip.Note))
            {
                oPdfPTableLabdip = new PdfPTable(4);
                oPdfPTableLabdip.SetWidths(new float[] { 50f, 245f, 150f, 150f });
                _oPdfPCell = new PdfPCell(new Phrase("Note ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableLabdip.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oLabDip.Note, _oFontStyle));
                _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableLabdip.AddCell(_oPdfPCell);
                oPdfPTableLabdip.CompleteRow();

                _oPdfPCell = new PdfPCell(oPdfPTableLabdip);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }

            #endregion

        }


        private void SetDetail_IsTwist()
        {
            #region Balnk Space
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 8f; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            _oFontStyleBold = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);

            _oLabDipDetails = _oLabDipDetails.OrderBy(o => o.TwistedGroup).ToList();

            PdfPTable oPdfPTable = new PdfPTable(8);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 25f, 190f, 100f, 60f, 50f, 50f, 80f, 85f });
            oPdfPCell = new PdfPCell(new Phrase("Color Info", FontFactory.GetFont("Tahoma", 10f, 3)));
            oPdfPCell.Colspan = 8;
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            #region Grid Column
            oPdfPCell = new PdfPCell(new Phrase("#SL", _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Yarn Type", _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Color Name", _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Color No", _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("No of Ply", _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Twist", _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Panton no", _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Remarks", _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion


            int nCount = 0;
            int nProductID = 0;
            int nRowSpan = 0;
            int nRowSpan_Twist = 0;
            int nTwistGroup = 0;
            if (_oLabDipDetails.Count > 0)
            {
                foreach (LabDipDetail oLabDipDetail in _oLabDipDetails)
                {
                    nRowSpan_Twist = _oLabDipDetails.Where(P => P.TwistedGroup == oLabDipDetail.TwistedGroup).ToList().Count;


                    if (oLabDipDetail.TwistedGroup > 0 && nTwistGroup != oLabDipDetail.TwistedGroup)
                    {
                        nCount++;
                        oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                        oPdfPCell.Rowspan = (nRowSpan_Twist > 0) ? nRowSpan_Twist : 1;
                        oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPTable.AddCell(oPdfPCell);
                    }

                    if (oLabDipDetail.TwistedGroup <= 0)
                    {
                        nCount++;
                        oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                        oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPTable.AddCell(oPdfPCell);
                    }



                    //if (nProductID != oLabDipDetail.ProductID)
                    //{
                        nRowSpan = _oLabDipDetails.Where(P => P.ProductID == oLabDipDetail.ProductID).ToList() .Count ;

                        oPdfPCell = new PdfPCell(new Phrase(oLabDipDetail.ProductName, _oFontStyle));
                        //oPdfPCell.Colspan = 3;
                        //oPdfPCell.Rowspan = nRowSpan;
                        oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPTable.AddCell(oPdfPCell);
                        //oPdfPTable.CompleteRow();

                        //_oPdfPCell = new PdfPCell(oPdfPTable);
                        //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        //_oPdfPTable.AddCell(_oPdfPCell);
                        //_oPdfPTable.CompleteRow();
                    //}

                        oPdfPCell = new PdfPCell(new Phrase(oLabDipDetail.ColorName, _oFontStyle));
                    //oPdfPCell.Colspan = 2;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oLabDipDetail.ColorNo, _oFontStyle));
                    //oPdfPCell.Colspan = 2;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oLabDipDetail.Combo.ToString(), _oFontStyle));
                    //oPdfPCell.Colspan = 2;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(oPdfPCell);
                

                    if ( oLabDipDetail.TwistedGroup>0 && nTwistGroup != oLabDipDetail.TwistedGroup)
                    {
                        oPdfPCell = new PdfPCell(new Phrase((oLabDipDetail.TwistedGroup > 0) ? "Twist" : "", _oFontStyle));
                        oPdfPCell.Rowspan = (nRowSpan_Twist > 0) ? nRowSpan_Twist : 1;
                        oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPTable.AddCell(oPdfPCell);
                    }

                    if (oLabDipDetail.TwistedGroup <=0)
                    {
                        oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPTable.AddCell(oPdfPCell);
                    }
                    oPdfPCell = new PdfPCell(new Phrase(oLabDipDetail.PantonNo, _oFontStyle));
                    //oPdfPCell.Colspan = 2;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oLabDipDetail.RefNo, _oFontStyle));
                    //oPdfPCell.Colspan = 2;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPTable.CompleteRow();

                    nProductID = oLabDipDetail.ProductID;
                    nTwistGroup = oLabDipDetail.TwistedGroup;
                }

               

            }

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

          
        }
        private void SetDetail()
        {
            #region Balnk Space
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 8f; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            _oFontStyleBold = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);

            _oLabDipDetails = _oLabDipDetails.OrderBy(o => o.ProductID).ToList();

            PdfPTable oPdfPTable = new PdfPTable(6);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 25f, 190f, 110f, 60f, 80f, 125f });
            oPdfPCell = new PdfPCell(new Phrase("Color Info", FontFactory.GetFont("Tahoma", 10f, 3)));
            oPdfPCell.Colspan = 8;
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            #region Grid Column
            oPdfPCell = new PdfPCell(new Phrase("#SL", _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Yarn Type", _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Color Name", _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Color No", _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Panton no", _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Remarks", _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            int nCount = 0;
            int nProductID = 0;
            int nRowSpan = 0;

            if (_oLabDipDetails.Count > 0)
            {
                foreach (LabDipDetail oLabDipDetail in _oLabDipDetails)
                {


                    if (nProductID != oLabDipDetail.ProductID)
                    {
                        nRowSpan = _oLabDipDetails.Where(P => P.ProductID == oLabDipDetail.ProductID).ToList().Count;

                        nCount++;
                        oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                        oPdfPCell.Rowspan = nRowSpan; oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPTable.AddCell(oPdfPCell);
                       

                        oPdfPCell = new PdfPCell(new Phrase(oLabDipDetail.ProductName, _oFontStyle));
                        //oPdfPCell.Colspan = 3;
                        oPdfPCell.Rowspan = nRowSpan;
                        oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPTable.AddCell(oPdfPCell);

                    }

                    oPdfPCell = new PdfPCell(new Phrase(oLabDipDetail.ColorName, _oFontStyle));
                    //oPdfPCell.Colspan = 2;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oLabDipDetail.ColorNo, _oFontStyle));
                    //oPdfPCell.Colspan = 2;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(oPdfPCell);



                    oPdfPCell = new PdfPCell(new Phrase(oLabDipDetail.PantonNo, _oFontStyle));
                    //oPdfPCell.Colspan = 2;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oLabDipDetail.RefNo, _oFontStyle));
                    //oPdfPCell.Colspan = 2;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(oPdfPCell);
                    oPdfPTable.CompleteRow();
                    nProductID = oLabDipDetail.ProductID;
                }



            }

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


        }
        private void PrintLabdipDelivery()
        {
            #region Balnk Space
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 8f; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            _oFontStyleBold = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);

            PdfPTable oPdfPTable = new PdfPTable(4);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] {  95f, 100f, 100f, 80f});
            oPdfPCell = new PdfPCell(new Phrase("Delivery info", FontFactory.GetFont("Tahoma", 10f, 3)));
            oPdfPCell.Colspan = 8;
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

          
            oPdfPCell = new PdfPCell(new Phrase("Date", _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Challan No", _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Count", _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Prepare By", _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            foreach(LabdipChallan oItem in _oLabdipChallans)
            {
                //if (oItem.Currentstatus == EnumLabdipOrderStatus.LabdipInBuyerHand)
                //{
                    oPdfPCell = new PdfPCell(new Phrase(oItem.ChallanDateST, _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.ChallanNoFull, _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.ColorCount+"", _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.PrepareBy, _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPTable.CompleteRow();
                //}
            }

            //foreach (LabdipHistory oItem in _oLabdipHistorys)
            //{
            //    if (oItem.Currentstatus == EnumLabdipOrderStatus.LabdipInBuyerHand)
            //    {
            //        oPdfPCell = new PdfPCell(new Phrase(oItem.DateTimeSt, _oFontStyle));
            //        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            //        oPdfPCell = new PdfPCell(new Phrase(_oLabDip.ChallanNo, _oFontStyle));
            //        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            //        oPdfPCell = new PdfPCell(new Phrase(oItem.UserName, _oFontStyle));
            //        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            //        oPdfPCell = new PdfPCell(new Phrase(oItem.Note, _oFontStyle));
            //        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            //        oPdfPTable.CompleteRow();
            //    }
            //}


            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


        }
        private void PrintHistory()
        {
            #region Balnk Space
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 8f; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            _oFontStyleBold = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);

            PdfPTable oPdfPTable = new PdfPTable(6);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 25f, 70f, 100f, 100f, 40f, 60f });
            oPdfPCell = new PdfPCell(new Phrase("Operation History", FontFactory.GetFont("Tahoma", 10f, 3)));
            oPdfPCell.Colspan = 8;
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            #region Grid Column
            oPdfPCell = new PdfPCell(new Phrase("#SL", _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Date", _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Status", _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("User Name", _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Time Lag", _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Note", _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


            oPdfPTable.CompleteRow();
            #endregion
            int nCount = 0;
            bool bFlag = false;
            DateTime dStartDate = DateTime.Now;
            DateTime dEndDate = DateTime.Now;

            if (_oLabdipHistorys.Count > 0)
            {
                foreach (LabdipHistory oItem in _oLabdipHistorys)
                {
                    dEndDate = oItem.DateTime;
                    if (bFlag == true)
                    {
                        TimeSpan n = dEndDate - dStartDate;
                        if (n.Days >= 1)
                        {
                            oItem.Days = n.Days + " days";
                        }
                        else if (n.Hours > 0)
                        {
                            oItem.Days = oItem.Days + " " + n.Hours.ToString() + " hrs.";
                        }
                        else
                        {
                            oItem.Days = "";
                        }

                    }
                    else
                    {
                        oItem.Days = "";
                    }
                    dStartDate = oItem.DateTime;
                    bFlag = true;

                    nCount++;
                    oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.DateTimeSt, _oFontStyle));
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(oPdfPCell);


                    oPdfPCell = new PdfPCell(new Phrase(oItem.Currentstatus.ToString(), _oFontStyle));
                    //oPdfPCell.Colspan = 2;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.UserName, _oFontStyle));
                    //oPdfPCell.Colspan = 2;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.Days, _oFontStyle));
                    //oPdfPCell.Colspan = 2;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(oPdfPCell);


                    oPdfPCell = new PdfPCell(new Phrase(oItem.Note, _oFontStyle));
                    //oPdfPCell.Colspan = 2;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPTable.CompleteRow();

                }
            }

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


        }

        #endregion
    }
}
