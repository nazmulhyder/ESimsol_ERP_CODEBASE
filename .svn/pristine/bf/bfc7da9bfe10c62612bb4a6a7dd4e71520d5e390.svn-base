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
    public class rptFARegisterSummery
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyle_Bold;
  
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        BusinessUnit _oBusinessUnit = new BusinessUnit();
        Company _oCompany = new Company();
        
        List<FARegisterSummery> _oFARegisterSummerys = new List<FARegisterSummery>();
        Contractor _oContractor = new Contractor();
  
        #endregion

        #region FARegisterSummery LC
        public byte[] PrepareReport(List<FARegisterSummery> oFARegisterSummerys, BusinessUnit oBusinessUnit, Company oCompany, string Param)
        {
            _oFARegisterSummerys = oFARegisterSummerys;
            _oCompany = oCompany;
            _oBusinessUnit = oBusinessUnit;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
            _oDocument.SetMargins(25f, 25f, 10f, 25f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            //_oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();

            _oPdfPTable.SetWidths(new float[] { 
                                                    842f //Articale
                                                 
                                              });
            #endregion

            this.PrintHeader(Param);

            this.PrintBody(Param);
            _oPdfPTable.HeaderRows =2;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
     
        #region Report Header
        private void PrintHeader(string Param)
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
                _oImag.ScaleAbsolute(60f, 25f);
                _oPdfPCell = new PdfPCell(_oImag);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            }
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Rowspan = 3; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 18f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Name, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Rowspan = 3; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.PringReportHead, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

            #region ReportHeader
            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Schedule of Fixed Capital Assets as at " + Convert.ToDateTime(Param.Split('~')[2]).ToString("dd MMM yyyy"), _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.FixedHeight = 35f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
      
        #endregion

        #region Report Body


        private void PrintBody(string Param)
        {

            DateTime dStartDate = Convert.ToDateTime(Param.Split('~')[1]);
            DateTime dEndDate = Convert.ToDateTime(Param.Split('~')[2]);
            int nLayout = Convert.ToInt32(Param.Split('~')[4]);//1:category wise;2:Product wise
                     
            #region FAsummer print
            #region Detail Column Header

            PdfPTable oPdfPTable = new PdfPTable(10);
            oPdfPTable.SetWidths(new float[] { 25f, 120f, 80f, 80f, 80f,35f, 70f, 70f, 65f, 60f });
            _oFontStyle_Bold = FontFactory.GetFont("Tahoma", 8.0f, iTextSharp.text.Font.BOLD);
                       
            _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle_Bold));
            _oPdfPCell.Rowspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Particulars", _oFontStyle_Bold));
            _oPdfPCell.Rowspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("COST", _oFontStyle_Bold));
            _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Rate", _oFontStyle_Bold));
            _oPdfPCell.Rowspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Depraciation", _oFontStyle_Bold));
            _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Written Down Value as On\n" +dEndDate.ToString("dd.MM.yyyy"), _oFontStyle_Bold));
            _oPdfPCell.Rowspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase("AS On\n" + dStartDate.ToString("dd.MM.yyyy"), _oFontStyle_Bold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Addition During the Year", _oFontStyle_Bold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Total AS on\n" + dEndDate.ToString("dd.MM.yyyy"), _oFontStyle_Bold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("AS On\n" + dStartDate.ToString("dd.MM.yyyy"), _oFontStyle_Bold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("During the Year", _oFontStyle_Bold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Total AS on\n" + dEndDate.ToString("dd.MM.yyyy"), _oFontStyle_Bold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            #endregion
        
            #region Details Data
            int nCount = 0; int TempSubGroupHeadID = _oFARegisterSummerys[0].SubGroupHeadID;
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oFontStyle_Bold = FontFactory.GetFont("Tahoma", 8.0f, iTextSharp.text.Font.BOLD);
            foreach (FARegisterSummery oItem in _oFARegisterSummerys)
            {
                if (TempSubGroupHeadID!=oItem.SubGroupHeadID)
                {
                    nCount = 0;//Reset
                    #region  Print subtotal
                    _oPdfPCell = new PdfPCell(new Phrase("Sub-Total=", _oFontStyle_Bold));
                    _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(_oFARegisterSummerys.Where(x => x.SubGroupHeadID == TempSubGroupHeadID).Sum(t => t.AssetOpeningAmount).ToString("#,##0.00;(#,##0.00)"), _oFontStyle_Bold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(_oFARegisterSummerys.Where(x => x.SubGroupHeadID == TempSubGroupHeadID).Sum(t => t.AssetAdditionAmount).ToString("#,##0.00;(#,##0.00)"), _oFontStyle_Bold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(_oFARegisterSummerys.Where(x => x.SubGroupHeadID == TempSubGroupHeadID).Sum(t => t.TotalAssetAmount).ToString("#,##0.00;(#,##0.00)"), _oFontStyle_Bold));
                     _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle_Bold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(_oFARegisterSummerys.Where(x => x.SubGroupHeadID == TempSubGroupHeadID).Sum(t => t.DeprOpeningAmount).ToString("#,##0.00;(#,##0.00)"), _oFontStyle_Bold));
                     _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(_oFARegisterSummerys.Where(x => x.SubGroupHeadID == TempSubGroupHeadID).Sum(t => t.DeprAdditionAmount).ToString("#,##0.00;(#,##0.00)"), _oFontStyle_Bold));
                     _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(_oFARegisterSummerys.Where(x => x.SubGroupHeadID == TempSubGroupHeadID).Sum(t => t.TotalDeprAmount).ToString("#,##0.00;(#,##0.00)"), _oFontStyle_Bold));
                     _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(_oFARegisterSummerys.Where(x => x.SubGroupHeadID == TempSubGroupHeadID).Sum(t => t.EndingAssetAmount).ToString("#,##0.00;(#,##0.00)"), _oFontStyle_Bold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    oPdfPTable.CompleteRow();
                    #endregion
                    TempSubGroupHeadID = oItem.SubGroupHeadID;//set value
                }
                nCount++;
                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ReportViewLayout==1?oItem.CategoryName:oItem.ProductName, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.AssetOpeningAmount), _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.AssetAdditionAmount), _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.TotalAssetAmount), _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.DeprRate)+"%", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.DeprOpeningAmount), _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.DeprAdditionAmount), _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.TotalDeprAmount), _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.EndingAssetAmount), _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthRight = 1; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }

            //#region  Print subtotal
            //_oPdfPCell = new PdfPCell(new Phrase("Sub-Total=", _oFontStyle_Bold));
            //_oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase(_oFARegisterSummerys.Where(x => x.SubGroupHeadID == TempSubGroupHeadID).Sum(t => t.AssetOpeningAmount).ToString("#,##0.00;(#,##0.00)"), _oFontStyle_Bold));
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase(_oFARegisterSummerys.Where(x => x.SubGroupHeadID == TempSubGroupHeadID).Sum(t => t.AssetAdditionAmount).ToString("#,##0.00;(#,##0.00)"), _oFontStyle_Bold));
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase(_oFARegisterSummerys.Where(x => x.SubGroupHeadID == TempSubGroupHeadID).Sum(t => t.TotalAssetAmount).ToString("#,##0.00;(#,##0.00)"), _oFontStyle_Bold));
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle_Bold));
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase(_oFARegisterSummerys.Where(x => x.SubGroupHeadID == TempSubGroupHeadID).Sum(t => t.DeprOpeningAmount).ToString("#,##0.00;(#,##0.00)"), _oFontStyle_Bold));
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase(_oFARegisterSummerys.Where(x => x.SubGroupHeadID == TempSubGroupHeadID).Sum(t => t.DeprAdditionAmount).ToString("#,##0.00;(#,##0.00)"), _oFontStyle_Bold));
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase(_oFARegisterSummerys.Where(x => x.SubGroupHeadID == TempSubGroupHeadID).Sum(t => t.TotalDeprAmount).ToString("#,##0.00;(#,##0.00)"), _oFontStyle_Bold));
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase(_oFARegisterSummerys.Where(x => x.SubGroupHeadID == TempSubGroupHeadID).Sum(t => t.EndingAssetAmount).ToString("#,##0.00;(#,##0.00)"), _oFontStyle_Bold));
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            //oPdfPTable.CompleteRow();
            //#endregion

            //#region  Print Grand total
            //_oPdfPCell = new PdfPCell(new Phrase("Grand Total", _oFontStyle_Bold));
            //_oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase(_oFARegisterSummerys.Sum(t => t.AssetOpeningAmount).ToString("#,##0.00;(#,##0.00)"), _oFontStyle_Bold));
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase(_oFARegisterSummerys.Sum(t => t.AssetAdditionAmount).ToString("#,##0.00;(#,##0.00)"), _oFontStyle_Bold));
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase(_oFARegisterSummerys.Sum(t => t.TotalAssetAmount).ToString("#,##0.00;(#,##0.00)"), _oFontStyle_Bold));
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle_Bold));
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase(_oFARegisterSummerys.Sum(t => t.DeprOpeningAmount).ToString("#,##0.00;(#,##0.00)"), _oFontStyle_Bold));
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase(_oFARegisterSummerys.Sum(t => t.DeprAdditionAmount).ToString("#,##0.00;(#,##0.00)"), _oFontStyle_Bold));
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase(_oFARegisterSummerys.Sum(t => t.TotalDeprAmount).ToString("#,##0.00;(#,##0.00)"), _oFontStyle_Bold));
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase(_oFARegisterSummerys.Sum(t => t.EndingAssetAmount).ToString("#,##0.00;(#,##0.00)"), _oFontStyle_Bold));
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            //oPdfPTable.CompleteRow();
            //#endregion

            //#region Allocation of Depreciation
            //#region Blank space
            //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle_Bold));
            //_oPdfPCell.Colspan = 10; _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 25; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            //oPdfPTable.CompleteRow();
            //#endregion

            //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle_Bold));
            //_oPdfPCell.Colspan = 4; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("Allocation of Depreciation :", _oFontStyle_Bold));
            //_oPdfPCell.Colspan = 6; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            //oPdfPTable.CompleteRow();

            //// Manufacturing Expenses
            //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle_Bold));
            //_oPdfPCell.Colspan = 4; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("a) Manufacturing Expenses", _oFontStyle));
            //_oPdfPCell.Colspan = 4; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(2106419.05), _oFontStyle));
            //_oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle_Bold));
            //_oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            //oPdfPTable.CompleteRow();

            //// Administrative Expenses
            //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle_Bold));
            //_oPdfPCell.Colspan = 4; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("b) Administrative Expenses", _oFontStyle));
            //_oPdfPCell.Colspan = 4; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //_oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL| iTextSharp.text.Font.UNDERLINE);
            //_oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(50000.0), _oFontStyle));
            //_oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle_Bold));
            //_oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            //oPdfPTable.CompleteRow();

            //// total Expenses
            //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle_Bold));
            //_oPdfPCell.Colspan = 8; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //_oFontStyle_Bold = FontFactory.GetFont("Tahoma", 8.0f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE | iTextSharp.text.Font.UNDERLINE);
            //_oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(2156419.05), _oFontStyle_Bold));
            //_oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle_Bold));
            //_oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            //oPdfPTable.CompleteRow();
            //#endregion

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            #endregion


        }
        #endregion
        #endregion
    }
}
