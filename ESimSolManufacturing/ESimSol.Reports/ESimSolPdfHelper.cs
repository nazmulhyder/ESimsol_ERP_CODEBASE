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
using System;

namespace ESimSol.Reports
{
    public class ESimSolPdfHelper
    {
        #region Declaration
        public static iTextSharp.text.Font FontStyle;
        public static List CheckMark = new ZapfDingbatsList(52);

        static PdfPCell _oPdfPCell;
        static iTextSharp.text.Image _oImag;
        static BusinessUnit _oBusinessUnit = new BusinessUnit();
        #endregion

        #region Report Header
        /*
         PdfPTable oPdfHeader = new PdfPTable(1);
         ESimSolPdfHelper.PrintHeader(ref _oPdfPTable, oBusinessUnit,oCompany, sRowHeader[{Header, CompanyName, Audi, Address, Service}]);
         ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfHeader,iTextSharp.text.Rectangle.NO_BORDER,0,_nColumn);
         */
        public static void PrintHeader_Baly(ref PdfPTable oPdfPTable_Main, BusinessUnit oBusinessUnit, Company oCompany, string sReportHeader, int nColspan)
        {
            #region CompanyHeader
            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.SetWidths(new float[] { 80f, 340f, 40f });

            #region LOGO
            FontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            if (oCompany.CompanyLogo != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(60f, 40f);
                _oPdfPCell = new PdfPCell(_oImag);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", FontStyle));
            }
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Rowspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
           #endregion

            FontStyle = FontFactory.GetFont("Tahoma", 20f, iTextSharp.text.Font.BOLD);
            AddCell(ref oPdfPTable, oBusinessUnit.Name, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
            AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
            oPdfPTable.CompleteRow();

            FontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            AddCell(ref oPdfPTable, oBusinessUnit.PringReportHead, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
            AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
            oPdfPTable.CompleteRow();
            #endregion

            #region ReportHeader
            FontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
            AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
            AddCell(ref oPdfPTable, sReportHeader, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER,0,0,25);
            AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
            #endregion

            #region Insert Into Main Table
            AddTable(ref oPdfPTable_Main, oPdfPTable, iTextSharp.text.Rectangle.NO_BORDER, 0, nColspan);
            #endregion
        }

        public static void PrintHeader_Baly(ref PdfPTable oPdfPTable_Main, Company oCompany, string sReportHeader, int nColspan)
        {
            #region CompanyHeader
            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.SetWidths(new float[] { 80f, 340f, 40f });

            #region LOGO
            FontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            if (oCompany.CompanyLogo != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(60f, 40f);
                _oPdfPCell = new PdfPCell(_oImag);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", FontStyle));
            }
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Rowspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            #endregion

            FontStyle = FontFactory.GetFont("Tahoma", 20f, iTextSharp.text.Font.BOLD);
            AddCell(ref oPdfPTable, oCompany.Name, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
            AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
            oPdfPTable.CompleteRow();

            FontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            AddCell(ref oPdfPTable, oCompany.PringReportHead, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
            AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
            oPdfPTable.CompleteRow();
            #endregion

            #region ReportHeader
            FontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
            AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
            AddCell(ref oPdfPTable, sReportHeader, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER, 0, 0, 25);
            AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
            #endregion

            #region Insert Into Main Table
            AddTable(ref oPdfPTable_Main, oPdfPTable, iTextSharp.text.Rectangle.NO_BORDER, 0, nColspan);
            #endregion
        }
        public static void EasyPrintHeader(ref PdfPTable oPdfPTable_Main, Company oCompany, string sReportHeader, int nColspan, float nFontCompany, float nFontAddress, float nFontHeader)
        {
            #region CompanyHeader
            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.SetWidths(new float[] { 80f, 340f, 40f });

            #region LOGO
            FontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            if (oCompany.CompanyLogo != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(60f, 40f);
                _oPdfPCell = new PdfPCell(_oImag);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", FontStyle));
            }
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Rowspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            #endregion

            FontStyle = FontFactory.GetFont("Tahoma", nFontCompany, iTextSharp.text.Font.BOLD);
            AddCell(ref oPdfPTable, oCompany.Name, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
            AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
            oPdfPTable.CompleteRow();

            FontStyle = FontFactory.GetFont("Tahoma", nFontAddress, iTextSharp.text.Font.BOLD);
            AddCell(ref oPdfPTable, oCompany.PringReportHead, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
            AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
            oPdfPTable.CompleteRow();
            #endregion

            #region ReportHeader
            FontStyle = FontFactory.GetFont("Tahoma", nFontHeader, iTextSharp.text.Font.BOLD);
            AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
            AddCell(ref oPdfPTable, sReportHeader, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER, 0, 0, 25);
            AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
            #endregion

            #region Insert Into Main Table
            AddTable(ref oPdfPTable_Main, oPdfPTable, iTextSharp.text.Rectangle.NO_BORDER, 0, nColspan);
            #endregion
        }
        public static void PrintHeader_Sticker(ref PdfPTable oPdfPTable_Main, Company oCompany, string sReportHeader, int nColspan)
        {
            #region CompanyHeader
            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.SetWidths(new float[] { 80f, 340f, 40f });

            #region LOGO
            FontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            if (oCompany.CompanyLogo != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(35f, 20f);
                _oPdfPCell = new PdfPCell(_oImag);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", FontStyle));
            }
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Rowspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            #endregion

            FontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            AddCell(ref oPdfPTable, oCompany.Name, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
            AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);

            if (!string.IsNullOrEmpty(sReportHeader))
            {
                FontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
                AddCell(ref oPdfPTable, sReportHeader, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
                AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
                oPdfPTable.CompleteRow();
            }
            #endregion

            //#region ReportHeader
            //if (!string.IsNullOrEmpty(sReportHeader))
            //{
            //    FontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
            //    AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
            //    AddCell(ref oPdfPTable, sReportHeader, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.ANNOTATION, 0, 0, 25);
            //    AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
            //}
            //#endregion

            #region Insert Into Main Table
            oPdfPTable.CompleteRow();
            AddTable(ref oPdfPTable_Main, oPdfPTable, iTextSharp.text.Rectangle.NO_BORDER, 0, nColspan);
            #endregion
        }

        public static void PrintHeader_Sticker_With_BarCode(ref PdfPTable oPdfPTable_Main, Company oCompany, string sReportHeader, string sBarCode, int nColspan)
        {
            #region CompanyHeader
            PdfPTable oPdfPTable = new PdfPTable(4);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.SetWidths(new float[] { 10f, 70f, 340f, 40f });
            FontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);

            _oPdfPCell = new PdfPCell(new Phrase(" ", FontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = oPdfPTable.NumberOfColumns; _oPdfPCell.FixedHeight = 3;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            //AddCell(ref oPdfPTable, " ", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER, 0, oPdfPTable.NumberOfColumns, 5);

            #region Barcode128
            Barcode128 code128 = new Barcode128();
            code128.CodeType = Barcode.CODE128;
            code128.ChecksumText = true;
            code128.GenerateChecksum = true;
            code128.Code = sBarCode;
            System.Drawing.Bitmap bm = new System.Drawing.Bitmap(code128.CreateDrawingImage(System.Drawing.Color.Black, System.Drawing.Color.White));
            iTextSharp.text.Image oImag = iTextSharp.text.Image.GetInstance(bm, System.Drawing.Imaging.ImageFormat.Bmp);
            oImag.ScaleAbsolute(65f, 12f);
            #endregion

            AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);

            _oPdfPCell = new PdfPCell(oImag); _oPdfPCell.Border = 0; _oPdfPCell.ExtraParagraphSpace = 10f; _oPdfPCell.Rowspan = 2;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            //AddCell(ref oPdfPTable, sBarCode, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
            FontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            AddCell(ref oPdfPTable, oCompany.Name, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
            AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);

            if (!string.IsNullOrEmpty(sReportHeader))
            {
                FontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
                AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
                AddCell(ref oPdfPTable, sReportHeader, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
                AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
                oPdfPTable.CompleteRow();
            }
            #endregion

            //#region ReportHeader
            //if (!string.IsNullOrEmpty(sReportHeader))
            //{
            //    FontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
            //    AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
            //    AddCell(ref oPdfPTable, sReportHeader, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.ANNOTATION, 0, 0, 25);
            //    AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
            //}
            //#endregion

            #region Insert Into Main Table
            oPdfPTable.CompleteRow();
            AddTable(ref oPdfPTable_Main, oPdfPTable, iTextSharp.text.Rectangle.NO_BORDER, 0, nColspan);
            #endregion
        }
        public static void PrintHeader_Sticker_With_BarCodeForNum(ref PdfPTable oPdfPTable_Main, Company oCompany, string sReportHeader, int nBarCode, int nColspan)
        {
            #region CompanyHeader
            PdfPTable oPdfPTable = new PdfPTable(4);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.SetWidths(new float[] { 10f, 70f, 340f, 40f });
            FontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);

            _oPdfPCell = new PdfPCell(new Phrase(" ", FontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = oPdfPTable.NumberOfColumns; _oPdfPCell.FixedHeight = 3;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            //AddCell(ref oPdfPTable, " ", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER, 0, oPdfPTable.NumberOfColumns, 5);

            #region Barcode128
            Barcode128 code128 = new Barcode128();
            code128.CodeType = Barcode.CODE128;
            code128.ChecksumText = true;
            code128.GenerateChecksum = true;
            string str = nBarCode.ToString("000000000");
            code128.Code = str;
            System.Drawing.Bitmap bm = new System.Drawing.Bitmap(code128.CreateDrawingImage(System.Drawing.Color.Black, System.Drawing.Color.White));
            iTextSharp.text.Image oImag = iTextSharp.text.Image.GetInstance(bm, System.Drawing.Imaging.ImageFormat.Bmp);
            oImag.ScaleAbsolute(65f, 12f);
            #endregion

            AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);

            _oPdfPCell = new PdfPCell(oImag); _oPdfPCell.Border = 0; _oPdfPCell.ExtraParagraphSpace = 10f; _oPdfPCell.Rowspan = 2;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            //AddCell(ref oPdfPTable, sBarCode, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
            FontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            AddCell(ref oPdfPTable, oCompany.Name, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
            AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);

            if (!string.IsNullOrEmpty(sReportHeader))
            {
                FontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
                AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
                AddCell(ref oPdfPTable, sReportHeader, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
                AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
                oPdfPTable.CompleteRow();
            }
            #endregion

            //#region ReportHeader
            //if (!string.IsNullOrEmpty(sReportHeader))
            //{
            //    FontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
            //    AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
            //    AddCell(ref oPdfPTable, sReportHeader, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.ANNOTATION, 0, 0, 25);
            //    AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER);
            //}
            //#endregion

            #region Insert Into Main Table
            oPdfPTable.CompleteRow();
            AddTable(ref oPdfPTable_Main, oPdfPTable, iTextSharp.text.Rectangle.NO_BORDER, 0, nColspan);
            #endregion
        }
        public static void PrintHeader_Audi(ref PdfPTable oPdfPTable, BusinessUnit oBusinessUnit, Company oCompany, string[] sRowHeader, int nColspan)
        {
            PdfPTable oPdfPTableComDetail = new PdfPTable(2);
            oPdfPTableComDetail.SetWidths(new float[] { 525f, 70f });

            #region 1st Row

            FontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            AddCell(ref oPdfPTableComDetail, sRowHeader[0], Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0);//"Spare Parts Request Form"
           
            #region CompanyLogo
            _oImag = iTextSharp.text.Image.GetInstance(oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);

            if (_oImag!= null)
            {
                _oImag.ScaleAbsolute(40f, 25f);
                _oPdfPCell = new PdfPCell(_oImag);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                _oPdfPCell.Border = 0;
                _oPdfPCell.MinimumHeight = 35f;
                oPdfPTableComDetail.AddCell(_oPdfPCell);
            }
            else
                AddCell(ref oPdfPTableComDetail, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0);
            #endregion

             #endregion
            
            #region 2nd Row
            if (sRowHeader.Length > 1)
            {
                FontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
                AddCell(ref oPdfPTableComDetail, sRowHeader[1], Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0);

                FontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.RED);
                AddCell(ref oPdfPTableComDetail, sRowHeader[2], Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0);


                FontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
                AddCell(ref oPdfPTableComDetail, sRowHeader[3], Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0);

                FontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.RED);
                AddCell(ref oPdfPTableComDetail, sRowHeader[4], Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0);
            }
            #endregion

            #region Insert Into Main Table
            AddTable(ref oPdfPTable, oPdfPTableComDetail,iTextSharp.text.Rectangle.NO_BORDER, 0, nColspan);
            #endregion
        }
        #endregion

        #region Report Header Details
        /*
         *ESimSolPdfHelper.PrintHeaderInfo(ref _oPdfPTable, TableHeader,TableFooter, sDetails[{1stRow},{1stRowValue},{2ndRow}],{2ndRowValue});
         *sDetails[0][0]="Contractor",sDetails[0][1]="Date"
         *sDetails[1][0]=ContractorName,sDetails[1][1]=DateSt
         */
        public static void PrintHeaderInfo(ref PdfPTable oPdfPTable, string TableHeader, string TableFooter, string[][] sDetails)
        {
            PdfPTable oPdfPTableRD = new PdfPTable(sDetails[0].Length);
            oPdfPTableRD.DefaultCell.Border = iTextSharp.text.Rectangle.BOX;

            int nTotalColumn = sDetails[0].Length;
            float[] nWidths = new float[nTotalColumn];
            for (int i = 0; i < nTotalColumn; i++)
            {
                nWidths[i] = 595 / nTotalColumn;
			}
            oPdfPTableRD.SetWidths(nWidths);

            FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            AddCell(ref oPdfPTableRD, "" , Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0, 0, nTotalColumn, 15);
            if (!string.IsNullOrEmpty(TableHeader)) AddCell(ref oPdfPTableRD, TableHeader, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, nTotalColumn, 15);

            #region Data
            for (int i = 0; i < sDetails.Length; i++)
            {
                for (int j = 0; j < sDetails[i].Length; j++)
                {
                    int nColspan = 1;
                    while(sDetails[i][j].Equals("~Merge")) 
                    {
                        nColspan++; j++;
                    }

                    if (i % 2 == 0)
                    {
                        FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

                        AddCell(ref oPdfPTableRD, sDetails[i][j], Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.LIGHT_GRAY, iTextSharp.text.Rectangle.BOX, 0, nColspan, 10);
                    }
                    else 
                    {
                        FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                        AddCell(ref oPdfPTableRD, sDetails[i][j], Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, nColspan, 10);
                    }
                }
                oPdfPTableRD.CompleteRow();
            }
            #endregion

            FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            if (!string.IsNullOrEmpty(TableFooter)) AddCell(ref oPdfPTableRD, TableFooter, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, nTotalColumn, 15);
            AddCell(ref oPdfPTableRD, "", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0, 0, nTotalColumn, 15);

            AddTable(ref oPdfPTable, oPdfPTableRD, iTextSharp.text.Rectangle.NO_BORDER,0,0);
        }
        #endregion

        #region HELPER
        
        #region NEW PAGE DECLARE
        public static void NewPageDeclaration(Document oDocument, PdfPTable oPdfPTable)
        {
            oDocument.Add(oPdfPTable);
            oDocument.NewPage();
            oPdfPTable.DeleteBodyRows();
        }
        #endregion

        #region ADD CELL
        public static void AddCell(ref PdfPTable oPdfPTable, string sData)
        {
            AddCell(ref oPdfPTable, sData, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 0);
        }

        //public static void AddCell(ref PdfPTable oPdfPTable, string sData, iTextSharp.text.Rectangle oRectangle)
        //{
        //    AddCell(ref oPdfPTable, sData, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, oRectangle.Border, 0, 0, 0);
        //}
        public static void AddCell(ref PdfPTable oPdfPTable, string sData, BaseColor oBaseColor)
        {
            AddCell(ref oPdfPTable, sData, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, oBaseColor, iTextSharp.text.Rectangle.BOX, 0, 0, 0);
        }
        public static void AddCell(ref PdfPTable oPdfPTable, string sData, int oHorizontal)
        {
            AddCell(ref oPdfPTable, sData, oHorizontal, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 0);
        }
        public static void AddParagraph(ref PdfPTable oPdfPTable, Phrase oPhrase, int oHorizontal, int oVertical, BaseColor oBaseColor, int nBorder, int nRowspan, int nColspan, float FixedHeight)
        {
            Paragraph _oPdfParagraph;
            _oPdfParagraph = new Paragraph(oPhrase);
            _oPdfParagraph.SetLeading(0.5f, 1.5f);
            _oPdfParagraph.Alignment = Element.ALIGN_JUSTIFIED;
            _oPdfPCell = new PdfPCell();
            _oPdfPCell.AddElement(_oPdfParagraph);
            _oPdfPCell.Border = nBorder;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = oBaseColor;

            if (FixedHeight > 0) { _oPdfPCell.FixedHeight = FixedHeight; }

            oPdfPTable.AddCell(_oPdfPCell);
        }
        public static void AddCell(ref PdfPTable oPdfPTable, string sData, int oHorizontal, int oVertical)
        {
            AddCell(ref oPdfPTable, sData, oHorizontal, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 0);
        }
        public static void AddCell(ref PdfPTable oPdfPTable, string sData, int oHorizontal, int oVertical, BaseColor oBaseColor, int nBorder)
        {
            AddCell(ref oPdfPTable, sData, oHorizontal, oVertical, oBaseColor, nBorder, 0, 0, 0);
        }
        public static void AddCell(ref PdfPTable oPdfPTable, double nData, int oHorizontal, int oVertical, BaseColor oBaseColor, int nBorder)
        {
            AddCell(ref oPdfPTable, Global.MillionFormat(nData), oHorizontal, oVertical, oBaseColor, nBorder, 0, 0, 0);
        }
        public static void AddCell(ref PdfPTable oPdfPTable, double nData, int oHorizontal, int oVertical, BaseColor oBaseColor, int nBorder, int nRowspan, int nColspan, int MinimumHeight)
        {
            AddCell(ref oPdfPTable, Global.MillionFormat(nData), oHorizontal, oVertical, oBaseColor, nBorder, nRowspan, nColspan, MinimumHeight);
        }
        public static void AddCell(ref PdfPTable oPdfPTable, string sHeader, int oHorizontal, int oVertical, BaseColor oBaseColor, int nBorder, int nRowspan, int nColspan, int MinimumHeight)
        {
            if (FontStyle == null) throw new System.ArgumentException("Null Font ArgumentException For: '" + sHeader + "' From AddCell [1001]");
            if (sHeader == null) throw new System.ArgumentException("Null Data ArgumentException From AddCell [1002]");

            _oPdfPCell = new PdfPCell(new Phrase(sHeader, FontStyle));
            _oPdfPCell.HorizontalAlignment = oHorizontal;
            _oPdfPCell.VerticalAlignment = oVertical;
            _oPdfPCell.BackgroundColor = oBaseColor;
            _oPdfPCell.Border = nBorder;
            _oPdfPCell.Colspan = (nColspan > 0 ? nColspan : 1);
            _oPdfPCell.Rowspan = (nRowspan > 0 ? nRowspan : 1);
            if (MinimumHeight > 0) { _oPdfPCell.MinimumHeight = MinimumHeight; }
            oPdfPTable.AddCell(_oPdfPCell);
        }
        public static void AddCell(ref PdfPTable oPdfPTable, string sHeader, int oHorizontal, int oVertical, BaseColor oBaseColor, int nBorder, int nRowspan, int nColspan, double MinimumHeight)
        {
            if (FontStyle == null) throw new System.ArgumentException("Null Font ArgumentException For: '" + sHeader + "' From AddCell [1001]");
            //if (sHeader == null) throw new System.ArgumentException("Null Data ArgumentException From AddCell [1002]");

            _oPdfPCell = new PdfPCell(new Phrase(sHeader, FontStyle));
            _oPdfPCell.HorizontalAlignment = oHorizontal;
            _oPdfPCell.VerticalAlignment = oVertical;
            _oPdfPCell.BackgroundColor = oBaseColor;
            _oPdfPCell.Border = nBorder;
            _oPdfPCell.Colspan = (nColspan > 0 ? nColspan : 1);
            _oPdfPCell.Rowspan = (nRowspan > 0 ? nRowspan : 1);
            if (MinimumHeight > 0) { _oPdfPCell.MinimumHeight = (float)MinimumHeight; }
            oPdfPTable.AddCell(_oPdfPCell);
        }
        public static void AddCell(ref PdfPTable oPdfPTable, string sHeader, int oHorizontal, int oVertical, BaseColor oBaseColor, int nBorder, int nRowspan, int nColspan, float MinimumHeight, float FixedHeight)
        {
            if (FontStyle == null) throw new System.ArgumentException("Null Font ArgumentException For: '" + sHeader + "' From AddCell [1002]");
            if (sHeader == null) throw new System.ArgumentException("Null Data ArgumentException From AddCell [1002]");
          
            _oPdfPCell = new PdfPCell(new Phrase(sHeader, FontStyle));
            _oPdfPCell.HorizontalAlignment = oHorizontal;
            _oPdfPCell.VerticalAlignment = oVertical;
            _oPdfPCell.BackgroundColor = oBaseColor;
            _oPdfPCell.Border = nBorder;
            _oPdfPCell.Colspan = (nColspan > 0 ? nColspan : 1);
            _oPdfPCell.Rowspan = (nRowspan > 0 ? nRowspan : 1);
            if (MinimumHeight > 0) { _oPdfPCell.MinimumHeight = MinimumHeight; }
            else if (FixedHeight > 0) { _oPdfPCell.FixedHeight = FixedHeight; }
            oPdfPTable.AddCell(_oPdfPCell);
        }
        public static void AddCell(ref PdfPTable oPdfPTable, PdfPCell oPdfPCell, int oHorizontal, int oVertical, BaseColor oBaseColor, int nBorder, int nRowspan, int nColspan, float MinimumHeight, float FixedHeight)
        {
            if (FontStyle == null) throw new System.ArgumentException("Null Font ArgumentException For: Phrase From AddCell [1002]");
            if (oPdfPCell == null) throw new System.ArgumentException("Null Data ArgumentException From AddCell [1002]");

            _oPdfPCell = new PdfPCell();
            _oPdfPCell = oPdfPCell;
            _oPdfPCell.HorizontalAlignment = oHorizontal;
            _oPdfPCell.VerticalAlignment = oVertical;
            _oPdfPCell.BackgroundColor = oBaseColor;
            _oPdfPCell.Border = nBorder;
            _oPdfPCell.Colspan = (nColspan > 0 ? nColspan : 1);
            _oPdfPCell.Rowspan = (nRowspan > 0 ? nRowspan : 1);
            if (MinimumHeight > 0) { _oPdfPCell.MinimumHeight = MinimumHeight; }
            else if (FixedHeight > 0) { _oPdfPCell.FixedHeight = FixedHeight; }
            oPdfPTable.AddCell(_oPdfPCell);
        }
        public static void PrintHeaders(ref PdfPTable oPdfPTable, string[] sData)
        {
            foreach (var oitem in sData)
                AddCell(ref oPdfPTable, oitem, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.LIGHT_GRAY, 15);
        }  
        public static void AddCells(ref PdfPTable oPdfPTable, string[] sData, int oHorizontal, int nBorder)
        {
            foreach (var oitem in sData)
                AddCell(ref oPdfPTable, oitem, oHorizontal, Element.ALIGN_MIDDLE, BaseColor.WHITE, nBorder);
        }
        public static void AddCells(ref PdfPTable oPdfPTable, string[] sData, int oHorizontal, int nBorder, BaseColor bBaseColor)
        {
            foreach (var oitem in sData)
                AddCell(ref oPdfPTable, oitem, oHorizontal, Element.ALIGN_MIDDLE, bBaseColor, nBorder);
        }
        #endregion

        #region AddCellMark
        public static void AddCellMark(ref PdfPTable oPdfPTable, string sData)
        {
            AddCellMark(ref oPdfPTable, sData, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 0,13);
        }
        public static void AddCellMark(ref PdfPTable oPdfPTable, string sHeader, int oHorizontal, int oVertical, BaseColor oBaseColor, int nBorder, int nRowspan, int nColspan, int MinimumHeight, int FixedHeight)
        {
            if (FontStyle == null) throw new System.ArgumentException("Null Font ArgumentException For: '" + sHeader + "' From AddCell [1001]");
            if (sHeader == null) throw new System.ArgumentException("Null Data ArgumentException From AddCellMark [1001]");

            _oPdfPCell = new PdfPCell(new Phrase("", FontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            CheckMark.Add(" " + sHeader);
            _oPdfPCell.AddElement(CheckMark);

            _oPdfPCell.HorizontalAlignment = oHorizontal;
            _oPdfPCell.VerticalAlignment = oVertical;
            _oPdfPCell.BackgroundColor = oBaseColor;
            _oPdfPCell.Border = nBorder;
            _oPdfPCell.Colspan = (nColspan > 0 ? nColspan : 1);
            _oPdfPCell.Rowspan = (nRowspan > 0 ? nRowspan : 1);
            if (MinimumHeight > 0) { _oPdfPCell.MinimumHeight = MinimumHeight; }
            else if (FixedHeight > 0) { _oPdfPCell.FixedHeight = FixedHeight; }
            oPdfPTable.AddCell(_oPdfPCell);
        }
        #endregion

        #region ADD TABLE
        public static void AddTable(ref PdfPTable MainTable, PdfPTable RefTable)
        {
            AddTable(ref MainTable, RefTable, 15);
        }
        public static void AddTable(ref PdfPTable MainTable, PdfPTable RefTable, int Border)
        {
            if (RefTable == null) throw new System.ArgumentException("Null Table ArgumentException From ESimSolPdfHelper. [1001]");

            _oPdfPCell = new PdfPCell(RefTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = Border;
            MainTable.AddCell(_oPdfPCell);
        }
      
        public static void AddTable(ref PdfPTable MainTable, PdfPTable RefTable, int Border, int nRowspan, int nColspan) 
        {
            if (RefTable == null) throw new System.ArgumentException("Null Table ArgumentException From ESimSolPdfHelper. [1002]");

            AddTable(ref MainTable, RefTable, Border, nRowspan, nColspan, 0, true);
        }
        public static void AddTable(ref PdfPTable MainTable, PdfPTable RefTable, int Border, int nRowspan, int nColspan, int height, bool isComplete)
        {
            if (RefTable == null) throw new System.ArgumentException("Null Table ArgumentException From ESimSolPdfHelper. [1002]");

            _oPdfPCell = new PdfPCell(RefTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Colspan = (nColspan > 0 ? nColspan : 1);
            _oPdfPCell.Rowspan = (nRowspan > 0 ? nRowspan : 1);
            if (height > 0) { _oPdfPCell.FixedHeight = height; }
            _oPdfPCell.Border = Border;
            MainTable.AddCell(_oPdfPCell);

            if(isComplete)
            MainTable.CompleteRow();
        }
        #endregion

        #region Working On PaddingCell
        
        //public static void AddPaddingCell(ref PdfPTable oPdfPTable, string sHeader,BaseColor oBaseColor, int Padding, int nColspan, int nRowspan, int MinimumHeight)
        //{
        //    _oPdfPCell = new PdfPCell(new Phrase(sHeader, FontStyle));
        //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //    _oPdfPCell.VerticalAlignment = Element.ALIGN_CENTER;
        //    _oPdfPCell.BackgroundColor = BaseColor.WHITE;

        //    _oPdfPCell.Padding = Padding;

        //    _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX;
        //    _oPdfPCell.Colspan = (nColspan > 0 ? nColspan : 1);
        //    _oPdfPCell.Rowspan = (nRowspan > 0 ? nRowspan : 1);
        //    if (MinimumHeight > 0) { _oPdfPCell.MinimumHeight = MinimumHeight; }
        //    oPdfPTable.AddCell(_oPdfPCell);
        //}
        
        #endregion

        #region CUSTOM FONT
        public static void SetCustomFont(string fontName, float fFontSize)
        {
            FontStyle=GetCustomFont( fontName, fFontSize);
        }
        public static iTextSharp.text.Font GetCustomFont(string fontName, float fFontSize)
        {
            string fontpath = System.Web.Hosting.HostingEnvironment.MapPath("~/fonts/");
            BaseFont customfont = BaseFont.CreateFont(fontpath + fontName + ".ttf", BaseFont.CP1252, BaseFont.EMBEDDED);
            iTextSharp.text.Font oFont = new iTextSharp.text.Font(customfont, fFontSize);
            return oFont;
        }
        public static iTextSharp.text.Font GetCustomFont(string fontName, float fFontSize, BaseColor oBaseColor)
        {
            string fontpath = System.Web.Hosting.HostingEnvironment.MapPath("~/fonts/");
            BaseFont customfont = BaseFont.CreateFont(fontpath + fontName + ".ttf", BaseFont.CP1252, BaseFont.EMBEDDED);
            iTextSharp.text.Font oFont = new iTextSharp.text.Font(customfont, fFontSize);
            oFont.Color=oBaseColor;
            return oFont;
        }
        #endregion

        #region PDF CALCULATE HIGHT
        public static float CalculatePdfPTableHeight(PdfPTable table)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (Document doc = new Document(PageSize.TABLOID))
                {
                    using (PdfWriter w = PdfWriter.GetInstance(doc, ms))
                    {
                        doc.Open();
                        table.TotalWidth = 535f;
                        table.WriteSelectedRows(0, table.Rows.Count, 0, 0, w.DirectContent);

                        doc.Close();
                        return table.TotalHeight;
                    }
                }
            }
        }
        #endregion

        #region Base Color
        public static BaseColor Custom_BaseColor(int[] rgb)
        {
            if (rgb.Length == 3)
                return new BaseColor(rgb[0], rgb[1], rgb[2]);
            else 
                return new BaseColor(255,255,255);
        }
        public static BaseColor GetRandomColor(int nCount)
        {
            //int mode= nCount % 4;
            //BaseColor result=ESimSolPdfHelper.Custom_BaseColor(new int[] { 17, 127, 117 });
            //switch(mode)
            //{
            //    case 0: result= ESimSolPdfHelper.Custom_BaseColor(new int[] { 67, 127, 117 }); break;
            //    case 1: result = ESimSolPdfHelper.Custom_BaseColor(new int[] { 47, 117, 127 }); break;
            //    case 2: result= ESimSolPdfHelper.Custom_BaseColor(new int[] { 67, 100, 14 }); break;
            //    case 3: result = ESimSolPdfHelper.Custom_BaseColor(new int[] { 50, 127, 142 }); break;
            //    case 4: result = ESimSolPdfHelper.Custom_BaseColor(new int[] { 40, 107, 102 }); break;
            //}
            return BaseColor.WHITE;
        }
        #endregion

        #endregion

        #region Footer
        public static void ApprovalHead(ref PdfPTable oPdfPTable, List<ApprovalHead> oApprovalHeads, List<ApprovalHistory> oApprovalHistorys, float nTableWidth, float nBlankRowHeight)
        {
            string[] signatureList = new string[oApprovalHeads.Count + 1];
            string[] dataList = new string[oApprovalHeads.Count + 1];

            for (int i = 0; i < oApprovalHeads.Count; i++)
            {
                signatureList[i] = (oApprovalHeads[i].Name);
                dataList[i] = (oApprovalHistorys.Where(x => x.ApprovalHeadID == oApprovalHeads[i].ApprovalHeadID).Select(x => x.SendToPersonName).FirstOrDefault());
            }

            #region Authorized Signature
            _oPdfPCell = new PdfPCell(GetSignature(nTableWidth, dataList, signatureList, nBlankRowHeight)); _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 20; //_oPdfPCell.Colspan = 9;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion
        }
        public static void Signature(ref PdfPTable oPdfPTable, string[] signatureList, string[] dataList, float nTableWidth, float nBlankRowHeight, int ColSpan)
        {
            #region Authorized Signature
            _oPdfPCell = new PdfPCell(GetSignature(nTableWidth, dataList, signatureList, nBlankRowHeight)); _oPdfPCell.Border = 0;
            //_oPdfPCell.FixedHeight = 20; 
            _oPdfPCell.Colspan = ColSpan;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion
        }
        public static PdfPTable GetSignature(float nTableWidth, string[] oData, string[] oSignatureSetups, float nBlankRowHeight)
        {
            iTextSharp.text.Font _oFontStyle;
            PdfPCell _oPdfPCell;
            int nSignatureCount = oSignatureSetups.Length;
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            if (nSignatureCount <= 0)
            {
                #region Blank Table
                PdfPTable oPdfPTable = new PdfPTable(1);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.SetWidths(new float[] { nTableWidth });

                if (nBlankRowHeight <= 0)
                {
                    nBlankRowHeight = 10f;
                }
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.FixedHeight = nBlankRowHeight; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                return oPdfPTable;
                #endregion
            }
            else
            {

                PdfPTable oPdfPTable = new PdfPTable(nSignatureCount);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

                int nColumnCount = -1;
                float[] columnArray = new float[nSignatureCount];
                foreach (string oItem in oSignatureSetups)
                {
                    nColumnCount++;
                    columnArray[nColumnCount] = nTableWidth / nSignatureCount;
                }
                oPdfPTable.SetWidths(columnArray);

                #region Blank Row
                if (nBlankRowHeight > 0)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                    _oPdfPCell.Colspan = nSignatureCount; _oPdfPCell.FixedHeight = nBlankRowHeight; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                }
                #endregion

                if (oData!=null && oData.Length == oSignatureSetups.Length)
                {
                    nColumnCount = 0;
                    for (int i = 0; i < oSignatureSetups.Length; i++)
                    {
                        if (nSignatureCount == 1)
                        {
                            _oPdfPCell = new PdfPCell(new Phrase(" " + oData[i] + "\n_________________\n" + oSignatureSetups[i], _oFontStyle));
                            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        }
                        else if (nSignatureCount == 2)
                        {
                            if (nColumnCount == 0)
                            {

                                _oPdfPCell = new PdfPCell(new Phrase(" " + oData[i] + "\n_________________\n" + oSignatureSetups[i], _oFontStyle));
                                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                            }
                            else
                            {
                                _oPdfPCell = new PdfPCell(new Phrase(oData[i] + "\n_________________\n" + oSignatureSetups[i] + " ", _oFontStyle));
                                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                            }
                        }
                        else
                        {
                            _oPdfPCell = new PdfPCell(new Phrase(oData[i] + "\n_________________\n" + oSignatureSetups[i], _oFontStyle));
                            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        }
                        nColumnCount++;
                    }
                }
                oPdfPTable.CompleteRow();
                return oPdfPTable;
            }
        }
        #endregion

        #region String Helper
        public static void FindAndReplace(ref string sData, string sFind, string sReplace)
        {
            if (sData.ToUpper().Contains(sFind.ToUpper()))
            {
                sData = sData.Replace(sFind, sReplace);
            }
        }
        public static void FindAndReplace1(ref string sData, string sFind, string sReplace)
        {
            if (sData.ToUpper().Contains(sFind.ToUpper()))
            {
                sData = sData.Replace(sFind, sReplace);
            }
        }
        #endregion
    }
}
