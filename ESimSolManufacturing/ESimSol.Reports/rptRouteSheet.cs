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

    public class rptRouteSheet
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyle_Bold;
        iTextSharp.text.Font _oFontStyle_BoldTwo;
        iTextSharp.text.Font _oFontStyle_Bold_UnLine;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        int _nColumn = 6;
        PdfPCell _oPdfPCell;
        Phrase _oPhrase = new Phrase();
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        RouteSheet _oRouteSheet = new RouteSheet();
        RouteSheetDetail _oRSDetail = new RouteSheetDetail();
        List<RouteSheetDetail> _oRSDetails = new List<RouteSheetDetail>();
        List<RouteSheetCombineDetail> _oRouteSheetCombineDetails = new List<RouteSheetCombineDetail>();
        Company _oCompany = new Company();
        RouteSheetCombine _oRouteSheetCombine = new RouteSheetCombine();
        BusinessUnit _oBusinessUnit = new BusinessUnit();
        RouteSheetSetup _oRouteSheetSetup = new RouteSheetSetup();
        List<DUPScheduleDetail> _oDUPScheduleDetails = new List<DUPScheduleDetail>();
        RouteSheetGrace _oRouteSheetGrace = new RouteSheetGrace();
        List<RSInQCSubStatus> _oRSInQCSubStatus = new List<RSInQCSubStatus>();
        bool _bIsDyes = false;
        double _nOrderQty = 0;
        string _sOrderType = "";
        string _sClaimOrderNo = "";
        string _sRecipeEditedBy = "";
        
        int _nNoOfPrint = 0;
        int _nRowHight = 0;
        int _nCount = 0;
        #endregion

        public byte[] PrepareReport(RouteSheet oRouteSheet, List<RouteSheetDetail> oRSDetails, RouteSheetCombine oRouteSheetCombine, BusinessUnit oBusinessUnit, bool bIsCombine, RouteSheetSetup oRouteSheetSetup, RouteSheetGrace oRouteSheetGrace)
        {
            _oRouteSheet = oRouteSheet;
            _oRSDetail = oRouteSheet.RouteSheetDetail;
            _oRSDetails = oRSDetails;
            _oBusinessUnit = oBusinessUnit;
            _oRouteSheetCombine = oRouteSheetCombine;
            _oRouteSheetSetup = oRouteSheetSetup;
            _oRouteSheetGrace = oRouteSheetGrace;
            //_sRecipeWrittenBy=sRecipeWrittenBy;
            //_sRecipeEditedBy = sRecipeEditedBy;
            //_sCombineRSNo = bIsCombine;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            _oDocument.SetMargins(10f,10f, 10f, 10f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_CENTER;

            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();

            _oPdfPTable.SetWidths(new float[] { 595f });
            #endregion

            GetsOrderInfo();
            this.ReporttHeader();
            this.ReportOrderInfo();
            PrintFooter();
            
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        public byte[] PrepareReport_Combine(RouteSheet oRouteSheet, List<RouteSheetDetail> oRSDetails, List<RouteSheetCombineDetail> oRouteSheetCombineDetails, Company oCompany, BusinessUnit oBusinessUnit, bool bIsCombine, RouteSheetSetup oRouteSheetSetup)
        {
            _oRouteSheet = oRouteSheet;
            _oRSDetail = oRouteSheet.RouteSheetDetail;
            _oRSDetails = oRSDetails;
            _oBusinessUnit = oBusinessUnit;
            _oRouteSheetCombineDetails = oRouteSheetCombineDetails;
            _oRouteSheetSetup = oRouteSheetSetup;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            _oDocument.SetMargins(10f, 10f, 10f, 10f);
            _oPdfPTable.WidthPercentage = 95;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_CENTER;

            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();

            _oPdfPTable.SetWidths(new float[] { 595f });
            #endregion

            GetsOrderInfo();
            this.ReporttHeader();
            this.ReportOrderInfo_Combine();
            //this.ReportOrderInfo_Combo();
            this.ReportBodyDyesChemical();
            PrintFooter();
            
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Header && Footer
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
            _oPdfPCell.Colspan = _nColumn; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Colspan = _nColumn; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.ExtraParagraphSpace = 10f; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }

        #endregion
        private void GetsOrderInfo()
        {
            _sOrderType = "";
            _sClaimOrderNo = "";
            if (_oRouteSheet.RouteSheetDOs.Count > 0)
            {
                _nOrderQty = _oRouteSheet.RouteSheetDOs.Select(c => c.OrderQty).Sum();
                _sOrderType = _oRouteSheet.RouteSheetDOs.Select(c => c.OrderTypeSt).FirstOrDefault();
            }
            
            if (_oRouteSheet.OrderType == (int)EnumOrderType.SampleOrder)
            {
                _sOrderType = "Sample#";
            }
            else if (_oRouteSheet.OrderType == (int)EnumOrderType.SampleOrder_Two)
            {
                _sOrderType = "Sample#";
            }
            else if (_oRouteSheet.OrderType == (int)EnumOrderType.BulkOrder)
            {
                _sOrderType = "Bulk Order#";
            }
            else if (_oRouteSheet.OrderType == (int)EnumOrderType.SaleOrder_Two)
            {
                _sOrderType = "Bulk Order#";
            }
            else if (_oRouteSheet.OrderType == (int)EnumOrderType.DyeingOnly)
            {
                _sOrderType = "Dyeing Only#";
            }
            else if (_oRouteSheet.OrderType == (int)EnumOrderType.ClaimOrder)
            {
                _sOrderType = "Claim Order#";
            }
         
            foreach (RouteSheetDO oItem in _oRouteSheet.RouteSheetDOs  )
            {
                _sClaimOrderNo =oItem.ClaimOrderNo+","+_sClaimOrderNo ;
            }
            if (!String.IsNullOrEmpty(_sClaimOrderNo ))
            {
                _sClaimOrderNo = _sClaimOrderNo.Remove((_sClaimOrderNo.Length - 1), 1);
            }
        }
        private void ReporttHeader()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
            _oFontStyle_Bold = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);

            #region  Heading Print
            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 80f, 300.5f, 80f });

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Colspan = 3; _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            #region Barcode128
            Barcode128 code128 = new Barcode128();
            code128.CodeType = Barcode.CODE128;
            code128.ChecksumText = true;
            code128.GenerateChecksum = true;
            string str = _oRouteSheet.RouteSheetID.ToString("000000000");
            code128.Code = str;
            System.Drawing.Bitmap bm = new System.Drawing.Bitmap(code128.CreateDrawingImage(System.Drawing.Color.Black, System.Drawing.Color.White));
            iTextSharp.text.Image oImag = iTextSharp.text.Image.GetInstance(bm, System.Drawing.Imaging.ImageFormat.Bmp);
            oImag.ScaleAbsolute(80f, 15f);
            #endregion

            //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //_oPdfPCell.Colspan = 2; _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 16f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(oImag);
            //_oPdfPCell.FixedHeight = 16f; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            //oPdfPTable.CompleteRow();


            //if (_nNoOfPrint > 1)
            //{
            //    _oPdfPCell = new PdfPCell(new Phrase("Copy - " + _nNoOfPrint, _oFontStyle));

            //}
            //else
            //{
            //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //}
            //_oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(oImag); //_oPdfPCell.FixedHeight = 16f;
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("DYEING CARD", FontFactory.GetFont("Tahoma", 17f, iTextSharp.text.Font.BOLD)));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            string sStr = "";
            sStr += (_nNoOfPrint > 1) ? "Copy-" + _nNoOfPrint : "";
            sStr += ((_oRouteSheet.IsReDyeing == EnumReDyeingStatus.None) ? " " : ((EnumReDyeingStatus)_oRouteSheet.IsReDyeing).ToString());
            _oPdfPCell = new PdfPCell(new Phrase(sStr, _oFontStyle_Bold));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Colspan = _nColumn; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Colspan = _nColumn; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.ExtraParagraphSpace = 10f; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }

        private void ReportOrderInfo()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL);
            _oFontStyle_Bold = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            float nUsagesHeight = 0;
            PdfPTable oPdfPTable = new PdfPTable(5);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 35f, 150f, 50f, 155f, 70f });

            oPdfPCell = new PdfPCell(new Phrase(_oRouteSheet.OperationUnitName, _oFontStyle));
            oPdfPCell.Colspan = 2; oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            //oPdfPCell = new PdfPCell(new Phrase(": " + _oRouteSheet.LotNo, _oFontStyle_Bold));
            //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            //oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Dyeing Type:" + _oRouteSheet.DyeingType + "    " + ((string.IsNullOrEmpty(_oRouteSheetGrace.Note)) ? "" : "Grace For:" + _oRouteSheetGrace.Note), _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.Colspan = 3; oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
          
            //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            //oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //oPdfPTable.AddCell(oPdfPCell);

            //oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle_Bold));
            //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;

            //oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("R/W Lot ", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(": " + _oRouteSheet.LotNo, _oFontStyle_Bold));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Lab Dip No", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            if (_oRouteSheet.PTU.LabDipDetailID<=0)
            {
                oPdfPCell = new PdfPCell(new Phrase(": " + _oRouteSheet.PTU.LabdipNo + " "+_oRouteSheet.PTU.PantonNo + " (" + ((EnumShade)_oRouteSheet.PTU.Shade).ToString() + ")", _oFontStyle));
            }
            else
            {
                oPdfPCell = new PdfPCell(new Phrase(": " + _oRouteSheet.PTU.LabdipNo + " Color No" + _oRouteSheet.PTU.ColorNo + " (" + ((EnumShade)_oRouteSheet.PTU.Shade).ToString() + ")", _oFontStyle));
            }
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Dyeing Lot No", _oFontStyle_Bold));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;

            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("Date", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(": " + _oRouteSheet.RouteSheetDateStr, _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(_sOrderType, _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            _oPhrase = new Phrase();
            if (String.IsNullOrEmpty(_sClaimOrderNo))
            {
                
                _oPhrase.Add(new Chunk(": ", _oFontStyle));
                _oPhrase.Add(new Chunk(_oRouteSheet.OrderNo, _oFontStyle_Bold));
                _oPhrase.Add(new Chunk(" (Qty: " + Global.MillionFormat(_nOrderQty) + " " + _oRouteSheetSetup .MUnit+ ")", _oFontStyle));

                //oPdfPCell = new PdfPCell(new Phrase(": "+_oRouteSheet.OrderNo +"("+Global.MillionFormat(_nOrderQty)+"Lbs)", _oFontStyle));
            }
            else
            {
                _oPhrase.Add(new Chunk(": ", _oFontStyle));
                _oPhrase.Add(new Chunk(_sClaimOrderNo, _oFontStyle_Bold));
                _oPhrase.Add(new Chunk(_oRouteSheet.OrderNo, _oFontStyle));
                //oPdfPCell = new PdfPCell(new Phrase(": " + _sClaimOrderNo + "," + _oRouteSheet.OrderNo, _oFontStyle));
            }

            oPdfPCell = new PdfPCell(_oPhrase);
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oRouteSheet.RouteSheetNo, _oFontStyle_Bold));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.Rowspan = 2;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();


            oPdfPCell = new PdfPCell(new Phrase("Buyer", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(": " + _oRouteSheet.PTU.ContractorName, _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("Machine No", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(": " + _oRouteSheet.MachineName, _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            //oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            //oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("Color", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(": " + _oRouteSheet.PTU.ColorName, _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            if (_oRouteSheet.HanksCone == (int)EumDyeingType.Hank) { _oRouteSheet.DyeingType="Bell";}

            oPdfPCell = new PdfPCell(new Phrase("No Of " + _oRouteSheet.DyeingType + ":", FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD)));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            if (_oRouteSheet.NoOfHanksCone <= 0) { _oRouteSheet.NoOfHanksCone = 1; }

            oPdfPCell = new PdfPCell(new Phrase(": " + _oRouteSheet.NoOfHanksCone + ", Weight/" + _oRouteSheet.DyeingType + ":" + Global.MillionFormat(_oRouteSheet.Qty / _oRouteSheet.NoOfHanksCone) + "" + _oRouteSheetSetup.MUnit + "," + Global.MillionFormat(_oRouteSheet.Qty * _oRouteSheetSetup.SMUnitValue / _oRouteSheet.NoOfHanksCone) + "" + _oRouteSheetSetup.MUnitTwo, _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            if (_oRouteSheetCombine.RouteSheetCombineID > 0)
            {
                oPdfPCell = new PdfPCell(new Phrase(_oRouteSheetCombine.RSNo_Combine, _oFontStyle));
            }
            else
            {
                oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            }
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("Yarn", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(": " + _oRouteSheet.ProductName, _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Weight", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            if (_oRouteSheet.QtyDye > 0)
            {
                oPdfPCell = new PdfPCell(new Phrase(": " + Global.MillionFormat(_oRouteSheet.Qty) + " + " + Global.MillionFormat(_oRouteSheet.QtyDye) + " " + _oRouteSheetSetup.MUnit + " , " + Global.MillionFormat((_oRouteSheet.Qty * _oRouteSheetSetup.SMUnitValue)) + " + " + Global.MillionFormat(_oRouteSheet.QtyDye * _oRouteSheetSetup.SMUnitValue) + " " + _oRouteSheetSetup.MUnitTwo + " ", _oFontStyle));
            }
            else
            {
                oPdfPCell = new PdfPCell(new Phrase(": " + Global.MillionFormat(_oRouteSheet.Qty) + " " + _oRouteSheetSetup.MUnit + " , " + Global.MillionFormat((_oRouteSheet.Qty * _oRouteSheetSetup.SMUnitValue)) + " " + _oRouteSheetSetup.MUnitTwo + " ", _oFontStyle));
            }

            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            if (_oRouteSheetCombine.RouteSheetCombineID >0)
            {
                oPdfPCell = new PdfPCell(new Phrase("Qty(" + _oRouteSheetSetup.MUnit + "):" + Global.MillionFormat(_oRouteSheetCombine.TotalQty), _oFontStyle));
            }
            else
            {
                oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            }

            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("Remark", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

         
            oPdfPCell = new PdfPCell(new Phrase(": " + _oRouteSheet.Note, _oFontStyle));
            oPdfPCell.Colspan = 3; oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            if (_oRouteSheetCombine.RouteSheetCombineID > 0)
            {
                oPdfPCell = new PdfPCell(new Phrase(_oRouteSheetCombine.RSNo_Combine, _oFontStyle));
            }
            else
            {
                oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            }
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();


            oPdfPCell = new PdfPCell(this.MiddlePart());
            //oPdfPCell = new PdfPCell(new Phrase(": " + _oRouteSheet.Note, _oFontStyle));
            oPdfPCell.Colspan = 5; oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            nUsagesHeight = CalculatePdfPTableHeight(_oPdfPTable);
            if (nUsagesHeight < 650)
            {
                nUsagesHeight = 650 - nUsagesHeight;
            }
            else
            {
                #region Continue
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("Continue..", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = iTextSharp.text.Rectangle.TOP_BORDER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                //_oDocument.Add(_oPdfPTable);
                _oDocument.NewPage();
                //_oPdfPTable.DeleteBodyRows();
                nUsagesHeight = 0.0f;


            }
            if (nUsagesHeight > 2)
            {
                #region Blank Row
                if (_oRouteSheetCombineDetails.Count > 6)
                {
                    while (nUsagesHeight < 650)
                    {
                        #region Table Initiate
                        oPdfPTable = new PdfPTable(7);
                        oPdfPTable.WidthPercentage = 100;
                        oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPTable.SetWidths(new float[] { 125f, 52f, 50f, 64f, 68f, 68f, 104f });


                        #endregion

                        _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.FixedHeight = 18f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.FixedHeight = 18f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.FixedHeight = 18f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.FixedHeight = 18f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.FixedHeight = 18f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.FixedHeight = 18f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.FixedHeight = 18f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


                        oPdfPTable.CompleteRow();

                        _oPdfPCell = new PdfPCell(oPdfPTable);
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();
                        nUsagesHeight = CalculatePdfPTableHeight(_oPdfPTable);
                    }
                }
                else
                {
                    while (nUsagesHeight < 650)
                    {
                        #region Table Initiate
                        oPdfPTable = new PdfPTable(6);
                        oPdfPTable.WidthPercentage = 100;
                        oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPTable.SetWidths(new float[] { 130f, 52f, 50f, 65f, 73f, 70f });


                        #endregion

                        _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.Border = 0;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.FixedHeight = 5f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.Border = 0;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.FixedHeight = 5f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.Border = 0;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.FixedHeight = 5f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.Border = 0;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.FixedHeight = 5f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.Border = 0;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.FixedHeight = 5f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.Border = 0;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.FixedHeight = 5f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        oPdfPTable.CompleteRow();

                        _oPdfPCell = new PdfPCell(oPdfPTable);
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();

                        nUsagesHeight = CalculatePdfPTableHeight(_oPdfPTable);
                    }
                }
                #endregion
            }
        }
        private void ReportBodyDyesChemical()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL);
            _oFontStyle_Bold = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            float nUsagesHeight = 0;
            PdfPTable oPdfPTable = new PdfPTable(5);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 35f, 150f, 50f, 155f, 70f });

         


            oPdfPCell = new PdfPCell(this.MiddlePart());
            //oPdfPCell = new PdfPCell(new Phrase(": " + _oRouteSheet.Note, _oFontStyle));
            oPdfPCell.Colspan = 5; oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            nUsagesHeight = CalculatePdfPTableHeight(_oPdfPTable);
            if (nUsagesHeight < 640)
            {
                nUsagesHeight = 640 - nUsagesHeight;
            }
            else
            {
                #region Continue
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("Continue..", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = iTextSharp.text.Rectangle.TOP_BORDER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                //_oDocument.Add(_oPdfPTable);
                _oDocument.NewPage();
                //_oPdfPTable.DeleteBodyRows();
                nUsagesHeight = 0.0f;


            }
            if (nUsagesHeight > 2)
            {
                #region Blank Row
                if (_oRouteSheetCombineDetails.Count > 6)
                {
                    while (nUsagesHeight < 640)
                    {
                        #region Table Initiate
                        oPdfPTable = new PdfPTable(7);
                        oPdfPTable.WidthPercentage = 100;
                        oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPTable.SetWidths(new float[] { 125f, 52f, 50f, 64f, 68f, 68f, 104f });


                        #endregion

                        _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.FixedHeight = 18f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.FixedHeight = 18f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.FixedHeight = 18f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.FixedHeight = 18f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.FixedHeight = 18f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.FixedHeight = 18f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.FixedHeight = 18f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


                        oPdfPTable.CompleteRow();

                        _oPdfPCell = new PdfPCell(oPdfPTable);
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();
                        nUsagesHeight = CalculatePdfPTableHeight(_oPdfPTable);
                    }
                }
                else
                {
                    while (nUsagesHeight < 640)
                    {
                        #region Table Initiate
                        oPdfPTable = new PdfPTable(6);
                        oPdfPTable.WidthPercentage = 100;
                        oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPTable.SetWidths(new float[] { 130f, 52f, 50f, 65f, 73f, 70f });


                        #endregion

                        _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.Border = 0;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.FixedHeight = 5f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.Border = 0;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.FixedHeight = 5f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.Border = 0;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.FixedHeight = 5f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.Border = 0;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.FixedHeight = 5f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.Border = 0;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.FixedHeight = 5f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.Border = 0;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.FixedHeight = 5f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        oPdfPTable.CompleteRow();

                        _oPdfPCell = new PdfPCell(oPdfPTable);
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();

                        nUsagesHeight = CalculatePdfPTableHeight(_oPdfPTable);
                    }
                }
                #endregion
            }
        }

        private void ReportOrderInfo_Combo()
        {
            float nUsagesHeight = 0;
            _oFontStyle = FontFactory.GetFont("Tahoma", 9.5f, iTextSharp.text.Font.NORMAL);
            _oFontStyle_Bold = FontFactory.GetFont("Tahoma", 9.5f, iTextSharp.text.Font.BOLD);

            PdfPTable oPdfPTable = new PdfPTable(5);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 40f, 130f, 50f, 150f, 90f });

            oPdfPCell = new PdfPCell(new Phrase("R/W Lot ", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(": " + _oRouteSheet.LotNo, _oFontStyle_Bold));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Lab Dip No", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            if (_oRouteSheet.PTU.LabDipDetailID <= 0)
            {
                oPdfPCell = new PdfPCell(new Phrase(": " + _oRouteSheet.PTU.LabdipNo +" "+ _oRouteSheet.PTU.PantonNo + " (" + ((EnumShade)_oRouteSheet.PTU.Shade).ToString() + ")", _oFontStyle));
            }
            else
            {
                oPdfPCell = new PdfPCell(new Phrase(": " + _oRouteSheet.PTU.LabdipNo + " Color No" + _oRouteSheet.PTU.ColorNo + " (" + ((EnumShade)_oRouteSheet.PTU.Shade).ToString() + ")", _oFontStyle));
            }
            
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(this.ShowRSNo());
            oPdfPCell.Border = 0;
            oPdfPCell.Rowspan = 8;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("Date", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(": " + _oRouteSheet.RouteSheetDateStr, _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(_sOrderType, _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            _oPhrase = new Phrase();
            if (String.IsNullOrEmpty(_sClaimOrderNo))
            {
                _oPhrase.Add(new Chunk(": ", _oFontStyle));
                _oPhrase.Add(new Chunk(_oRouteSheet.OrderNo, _oFontStyle_Bold));
                _oPhrase.Add(new Chunk(" (Qty: " + Global.MillionFormat(_nOrderQty) + "" + _oRouteSheetSetup.MUnit + ")", _oFontStyle));
                //oPdfPCell = new PdfPCell(new Phrase(": " + _oRouteSheet.OrderNo, _oFontStyle_Bold));
            }
            else
            {
                _oPhrase.Add(new Chunk(": ", _oFontStyle));
                _oPhrase.Add(new Chunk(_sClaimOrderNo, _oFontStyle_Bold));
                _oPhrase.Add(new Chunk(_oRouteSheet.OrderNo, _oFontStyle));
                //oPdfPCell = new PdfPCell(new Phrase(": " + _sClaimOrderNo + "," + _oRouteSheet.OrderNo, _oFontStyle_Bold));
            }

            oPdfPCell = new PdfPCell(_oPhrase);

            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            //oPdfPCell = new PdfPCell(new Phrase(" " + _oRouteSheet.RouteSheetNo, _oFontStyle_Bold));
            //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            //oPdfPCell.Rowspan = 4;
            //oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();


            oPdfPCell = new PdfPCell(new Phrase("Color", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(": " + _oRouteSheet.PTU.ColorName, _oFontStyle_Bold));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("Machine No", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(": " + _oRouteSheet.MachineName, _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            //oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            //oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("Yarn", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(": " +_oRouteSheet.ProductName, _oFontStyle));
            oPdfPCell.Colspan = 3; oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("Buyer", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(": " + _oRouteSheet.PTU.ContractorName, _oFontStyle_Bold));
            oPdfPCell.Colspan = 3; oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            //oPdfPCell = new PdfPCell(new Phrase("Weight", _oFontStyle));
            //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            //oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //oPdfPTable.AddCell(oPdfPCell);

            //oPdfPCell = new PdfPCell(new Phrase(": " + Global.MillionFormat(_oRouteSheet.Qty) + " LBS , " + Global.MillionFormat(Global.GetKG(_oRouteSheet.Qty, 2)) + " KG", _oFontStyle));
            //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            //oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //oPdfPTable.AddCell(oPdfPCell);

            //oPdfPCell = new PdfPCell(new Phrase(" " , _oFontStyle));
            //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            //oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();
            oPdfPCell = new PdfPCell(new Phrase("Weight", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            double nQty = 0;
            if (_oRouteSheetCombineDetails.Count <= 0)
            {
                foreach (RouteSheetCombineDetail oitem in _oRouteSheetCombineDetails)
                {
                    nQty = nQty + oitem.Qty;
                }
            }
            else
            {
                nQty = _oRouteSheet.Qty;
            }

            oPdfPCell = new PdfPCell(new Phrase(": " + Global.MillionFormat(nQty) + " " + _oRouteSheetSetup.MUnit + ", " + Global.MillionFormat(nQty * _oRouteSheetSetup.SMUnitValue) + " " + _oRouteSheetSetup.MUnitTwo, _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            //oPdfPCell = new PdfPCell(new Phrase(" " , _oFontStyle));
            //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            //oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();



            oPdfPCell = new PdfPCell(new Phrase("Remarks", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(": " + _oRouteSheet.Note, _oFontStyle));
            oPdfPCell.Colspan = 3; oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            //oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            //oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();
            if (_oRouteSheetCombineDetails.Count > 6)
            {
                oPdfPCell = new PdfPCell(this.MiddlePart());
                //oPdfPCell = new PdfPCell(new Phrase(": " + _oRouteSheet.Note, _oFontStyle));
                oPdfPCell.Colspan = 4; oPdfPCell.Border = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            else
            {
                oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                //oPdfPCell = new PdfPCell(new Phrase(": " + _oRouteSheet.Note, _oFontStyle));
                oPdfPCell.Colspan = 4; oPdfPCell.Border = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();

                oPdfPCell = new PdfPCell(this.MiddlePart());
                //oPdfPCell = new PdfPCell(new Phrase(": " + _oRouteSheet.Note, _oFontStyle));
                oPdfPCell.Colspan = 5; oPdfPCell.Border = 0; 
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();
            }

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

              nUsagesHeight = CalculatePdfPTableHeight(_oPdfPTable);
              if (nUsagesHeight < 640)
              {
                  nUsagesHeight = 640 - nUsagesHeight;
              }
              else
              {
                  #region Continue
                  _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                  _oPdfPCell = new PdfPCell(new Phrase("Continue..", _oFontStyle));
                  _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = iTextSharp.text.Rectangle.TOP_BORDER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                  _oPdfPTable.CompleteRow();
                  #endregion

                  //_oDocument.Add(_oPdfPTable);
                  _oDocument.NewPage();
                  //_oPdfPTable.DeleteBodyRows();
                  nUsagesHeight = 0.0f;

                
              }
              if (nUsagesHeight > 2)
              {
                  #region Blank Row
                  if (_oRouteSheetCombineDetails.Count > 6)
                  {
                      while (nUsagesHeight < 640)
                      {
                          #region Table Initiate
                          oPdfPTable = new PdfPTable(7);
                          oPdfPTable.WidthPercentage = 100;
                          oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                          oPdfPTable.SetWidths(new float[] { 125f, 52f, 50f, 64f, 68f, 68f, 104f });


                          #endregion

                          _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
                          _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                          _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.FixedHeight = 18f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                          _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                          _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.FixedHeight = 18f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                          _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                          _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.FixedHeight = 18f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                          _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                          _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.FixedHeight = 18f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                          _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                          _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.FixedHeight = 18f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                          _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                          _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.FixedHeight = 18f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                          _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                          _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.FixedHeight = 18f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


                          oPdfPTable.CompleteRow();

                          _oPdfPCell = new PdfPCell(oPdfPTable);
                          _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                          _oPdfPTable.CompleteRow();

                          nUsagesHeight = CalculatePdfPTableHeight(_oPdfPTable);
                      }
                  }
                  else
                  {
                      while (nUsagesHeight < 640)
                      {
                          #region Table Initiate
                          oPdfPTable = new PdfPTable(6);
                          oPdfPTable.WidthPercentage = 100;
                          oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                          oPdfPTable.SetWidths(new float[] { 130f, 52f, 50f, 65f, 73f, 70f });


                          #endregion

                          _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
                          _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                          _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.FixedHeight = 18f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                          _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                          _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.FixedHeight = 18f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                          _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                          _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.FixedHeight = 18f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                          _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                          _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.FixedHeight = 18f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                          _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                          _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.FixedHeight = 18f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                          _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                          _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.FixedHeight = 18f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                          oPdfPTable.CompleteRow();

                          _oPdfPCell = new PdfPCell(oPdfPTable);
                          _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                          _oPdfPTable.CompleteRow();

                          nUsagesHeight = CalculatePdfPTableHeight(_oPdfPTable);
                      }
                  }
                  #endregion
              }

        }


        private PdfPTable MiddlePart()
        {
            _nRowHight = 25;

            #region RouteSheetDetail
            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.NORMAL);
            _oFontStyle_BoldTwo = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
            _oFontStyle_Bold = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            if (_oRSDetails.Count >= 0 && _oRSDetails.Count < 9)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.NORMAL);
                _oFontStyle_BoldTwo = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
                _oFontStyle_Bold = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
                _nRowHight = 35;
            }
            if (_oRSDetails.Count >= 10 && _oRSDetails.Count <= 15)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL);
                _oFontStyle_BoldTwo = FontFactory.GetFont("Tahoma", 11f, iTextSharp.text.Font.BOLD);
                _oFontStyle_Bold = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
                _nRowHight = 30;
            }
            if (_oRSDetails.Count >=15 && _oRSDetails.Count <= 20)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
                _oFontStyle_BoldTwo = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
                _oFontStyle_Bold = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
                _nRowHight = 25;
            }
            else if (_oRSDetails.Count > 20 && _oRSDetails.Count < 35)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
                _oFontStyle_Bold = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
                _oFontStyle_BoldTwo = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
                _oFontStyle_Bold_UnLine = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
                _nRowHight = 12;
            }
            else if (_oRSDetails.Count >= 35 && _oRSDetails.Count < 42)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                _oFontStyle_Bold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oFontStyle_Bold_UnLine = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
                _nRowHight = 5;
            }
            else if (_oRSDetails.Count >= 42)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                _oFontStyle_Bold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oFontStyle_Bold_UnLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
                _nRowHight = 0;
            }


            PdfPTable oPdfPTableTwo = new PdfPTable(6);
            PdfPCell oPdfPCell;
            oPdfPTableTwo.SetWidths(new float[] { 130f, 52f, 50f, 65f, 73f, 70f });


            oPdfPTableTwo.AddCell(this.SetCellValue("Item Name", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, 1, 14f, _oFontStyle_Bold));
            oPdfPTableTwo.AddCell(this.SetCellValue("Amount(g/l)", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, 1, 14f, _oFontStyle_Bold));
            oPdfPTableTwo.AddCell(this.SetCellValue("Amount(%)", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, 1, 14f, _oFontStyle_Bold));
            oPdfPTableTwo.AddCell(this.SetCellValue("Total Amount", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, 1, 14f, _oFontStyle_Bold));
            oPdfPTableTwo.AddCell(this.SetCellValue("Addition(gm)", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, 1, 14f, _oFontStyle_Bold));
            oPdfPTableTwo.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, 1, 14f, _oFontStyle));
            oPdfPTableTwo.CompleteRow();
          


            foreach (RouteSheetDetail oItem in _oRSDetail.children)
            {
                int nSapn = 1;
                if (oItem.children.Count() > 0)
                {
                    nSapn = this.DepthChecker(oItem.children);
                    RecursiveChecking(oItem, ref nSapn, oPdfPTableTwo);
                }
                else
                {
                    SetInfoToGrid(oItem, ref nSapn, "", oPdfPTableTwo);
                }
            }

          

            #endregion RouteSheetDetail

            #region Summary
         
            //int[] ParentIDs = _oRSDetails.Where(x => x.ProcessName.ToUpper().Trim() == "COTTON DYEING").Select(x => x.RouteSheetDetailID).ToArray();
            //_oRSDetails = _oRSDetails.Where(x => ParentIDs.Contains(x.ParentID) && x.ProductCategoryName == "Dyes").ToList();

            //var nPercentage = _oRSDetails.Sum(x => (x.Percentage + ((x.Percentage * x.DeriveGL) / 100)));
            //var nAdditionalPercentage = _oRSDetails.Sum(x => x.AddOnePercentage + x.AddTwoPercentage + x.AddThreePercentage);
            //var nGTPercentage = nPercentage + nAdditionalPercentage;

            //var nTotal = _oRSDetails.Sum(x => (x.Percentage + ((x.Percentage * x.DeriveGL) / 100) + x.AddOnePercentage + x.AddTwoPercentage + x.AddThreePercentage));


            //oPdfPTableTwo.AddCell(this.SetCellValue("Net: " + Global.MillionFormat(nPercentage) + "%", 0, 3, Element.ALIGN_RIGHT, BaseColor.WHITE, 0, 0, _oFontStyle));
            //oPdfPTableTwo.AddCell(this.SetCellValue("Additional: " + Global.MillionFormat(nAdditionalPercentage) + "%", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, 0, 0, _oFontStyle));
            //oPdfPTableTwo.AddCell(this.SetCellValue("Total: " + Global.MillionFormat(nGTPercentage) + "%", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, 0, 0, _oFontStyle));
            //oPdfPTableTwo.CompleteRow();
            #endregion

            return oPdfPTableTwo;
        }

        int DepthChecker(List<RouteSheetDetail> oRSDetails)
        {
            int nSpan = 0;

            foreach (RouteSheetDetail oItem in oRSDetails)
            {
                if (oItem.children.Count() > 0)
                    break;
                else
                    nSpan++;
            }
            return (nSpan == 0) ? 1 : nSpan;
        }

        void RecursiveChecking(RouteSheetDetail oRSDetail, ref int nSpan, PdfPTable oPdfPTable)
        {
            if (oRSDetail.children.Count() > 0)
            {
                var bIsCottonDyeing = (oRSDetail.ProcessName.Trim().ToUpper() == "COTTON DYEING") ? true : false;
              
                var childen = oRSDetail.children.Where(x => x.Percentage > 0).ToList();
                childen.AddRange(oRSDetail.children.Where(x => x.Percentage <= 0).ToList());
                childen = childen.OrderBy(o => o.Sequence).ToList();
                oRSDetail.children = childen;

                oPdfPTable.AddCell(this.SetCellValue(oRSDetail.ProcessName, 0, 2, Element.ALIGN_LEFT, BaseColor.WHITE, 1, 16f, _oFontStyle_BoldTwo));

                oPdfPTable.AddCell(this.SetCellValue(((bIsCottonDyeing) ? oRSDetail.TempTime.Trim() : oRSDetail.Note.Trim()), 0, 4, Element.ALIGN_LEFT, BaseColor.WHITE, 1, 16f, _oFontStyle));
                oPdfPTable.CompleteRow();


                foreach (RouteSheetDetail oItem in oRSDetail.children)
                {
                    nSpan = (bIsCottonDyeing) ? 1 : nSpan;
                    if (oItem.children.Count() > 0)
                    {
                        nSpan = this.DepthChecker(oItem.children);
                        this.RecursiveChecking(oItem, ref nSpan, oPdfPTable);
                    }
                    else
                    {
                       
                        SetInfoToGrid(oItem, ref nSpan, (bIsCottonDyeing) ? "" : oRSDetail.TempTime, oPdfPTable);
                    }
                }
            }
            else
            {
             
                SetInfoToGrid(oRSDetail, ref nSpan, "", oPdfPTable);
            }
        }

        void    SetInfoToGrid(RouteSheetDetail oRSDetail, ref int nSapn, string sTempTime, PdfPTable oPdfPTable)
        {
           
            if (!string.IsNullOrEmpty(oRSDetail.SuggestLotNo))
            {
                if (_oRouteSheetSetup.DyesChemicalViewType == EnumDyesChemicalViewType.DyesChemical_and_Lot)
                {
                    oRSDetail.ProcessName = oRSDetail.ProcessName + " Lot: " + oRSDetail.SuggestLotNo;
                }
                else  if (_oRouteSheetSetup.DyesChemicalViewType == EnumDyesChemicalViewType.Lot)
                {
                    oRSDetail.ProcessName = oRSDetail.SuggestLotNo;
                }
                else if (_oRouteSheetSetup.DyesChemicalViewType == EnumDyesChemicalViewType.DyesChemical)
                {
                    oRSDetail.ProcessName = oRSDetail.ProcessName;
                }
                else
                {
                    oRSDetail.ProcessName = oRSDetail.ProcessName + " Lot: " + oRSDetail.SuggestLotNo;
                }
            }
            _bIsDyes = (oRSDetail.ProductType == EnumProductNature.Dyes) ? true : false;
            oPdfPTable.AddCell(this.SetCellValue(oRSDetail.ProcessName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, 1, _nRowHight, (_bIsDyes) ? _oFontStyle_Bold : _oFontStyle));

            oPdfPTable.AddCell(this.SetCellValue((oRSDetail.DeriveGL == 0) ? "" : Global.MillionFormatActualDigit(oRSDetail.DeriveGL), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, 1, _nRowHight, (_bIsDyes) ? _oFontStyle_Bold : _oFontStyle));

            oPdfPTable.AddCell(this.SetCellValue((oRSDetail.Percentage == 0) ? "" : Global.MillionFormatActualDigit(oRSDetail.Percentage), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, 1, _nRowHight, (_bIsDyes) ? _oFontStyle_Bold : _oFontStyle));


            double nLiquor = (oRSDetail.ForCotton) ? _oRouteSheet.TtlCotton : _oRouteSheet.TtlLiquire;
            if (oRSDetail.RecipeCalType == EnumDyeingRecipeType.General)
            {
                if (oRSDetail.GL > 0)
                {
                    oRSDetail.TotalQty = (nLiquor * oRSDetail.GL) / 1000;
                }
                else if (oRSDetail.Percentage > 0)
                {
                    if (oRSDetail.DAdjustment != 0)
                    {
                        oRSDetail.TotalQty = ((oRSDetail.Percentage + (oRSDetail.Percentage * oRSDetail.DAdjustment / 100)) * 10 * ((_oRouteSheet.Qty + _oRouteSheet.QtyDye) * _oRouteSheetSetup.SmallUnit_Cal)) / 1000;
                    }
                    else
                    {
                        oRSDetail.TotalQty = (oRSDetail.Percentage * 10 * (_oRouteSheet.Qty + _oRouteSheet.QtyDye ) * _oRouteSheetSetup.SmallUnit_Cal) / 1000;
                    }
                }
                else { oRSDetail.TotalQty = 0; }
            }

            oPdfPTable.AddCell(this.SetCellValue(oRSDetail.TotalQtyStr, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, 1, _nRowHight, (_bIsDyes) ? _oFontStyle_Bold : _oFontStyle));

            var oAddition = oRSDetail.RSDetailAdditonals.Where(x => x.InOutType == (int)EnumInOutType.Disburse).ToList();
            var oReturn = oRSDetail.RSDetailAdditonals.Where(x => x.InOutType == (int)EnumInOutType.Receive).ToList();
            if (oReturn.Count() > 0 || oAddition.Count()>0)
            {
                var sVal = oRSDetail.ConvertToStr(oAddition.Sum(x => x.Qty)) + " - " + oRSDetail.ConvertToStr(oReturn.Sum(x => x.Qty)) + " [A" + oAddition.Count() + "]";
                oPdfPTable.AddCell(this.SetCellValue(sVal, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, 1, _nRowHight, _oFontStyle));
            }
            else
            {
                oPdfPTable.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, 1, _nRowHight, _oFontStyle));
            }


            if (nSapn > 0)
            {
                if (string.IsNullOrEmpty(sTempTime)) { sTempTime = oRSDetail.TempTime;}
                oPdfPTable.AddCell(this.SetCellValue(sTempTime, nSapn, 0, Element.ALIGN_CENTER, BaseColor.WHITE, 1, _nRowHight, _oFontStyle));
                nSapn = 0;
            }
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Rowspan = nSapn;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }

        //private void PrintNote()
        //{
        //    _oPdfPTable.AddCell(this.SetCellValue("", 0, 5, Element.ALIGN_LEFT, BaseColor.WHITE, 0, 14f, _oFontStyle));
        //    _oPdfPTable.CompleteRow();

        //    _oPdfPTable.AddCell(this.SetCellValue("Macth: " + _oRouteSheet.Note, 0, 5, Element.ALIGN_LEFT, BaseColor.WHITE, 0, 14f, _oFontStyle));
        //    _oPdfPTable.CompleteRow();

        //    _oPdfPTable.AddCell(this.SetCellValue("", 0, 5, Element.ALIGN_LEFT, BaseColor.WHITE, 0, 14f, _oFontStyle));
        //    _oPdfPTable.CompleteRow();
        //}

        //private void PrintAuthorization()
        //{
        //    PdfPTable oTempTable = new PdfPTable(2);
        //    oTempTable.SetWidths(new float[] { 60f, 65f });

        //    _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, 0);

        //    oTempTable.AddCell(this.SetCellValue("Recipe written by ", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, 1, 20f, _oFontStyle));
        //    oTempTable.AddCell(this.SetCellValue(_sRecipeWrittenBy, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, 1, 20f, _oFontStyle));
        //    oTempTable.CompleteRow();

        //    _oPdfPCell = new PdfPCell(oTempTable);
        //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.FixedHeight = 14f; _oPdfPTable.AddCell(_oPdfPCell);

        //    _oPdfPTable.AddCell(this.SetCellValue("Dyebath Dropped By", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, 1, 14f, _oFontStyle));
        //    _oPdfPTable.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, 1, 14f, _oFontStyle));
        //    _oPdfPTable.AddCell(this.SetCellValue("Dyes Weighted By", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, 1, 14f, _oFontStyle));
        //    _oPdfPTable.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, 1, 14f, _oFontStyle));
        //    _oPdfPTable.CompleteRow();


        //    oTempTable = new PdfPTable(2);
        //    oTempTable.SetWidths(new float[] { 60f, 65f });

        //    oTempTable.AddCell(this.SetCellValue("Start Time", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, 1, 15f, _oFontStyle));
        //    oTempTable.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, 1, 15f, _oFontStyle));
        //    oTempTable.CompleteRow();

        //    _oPdfPCell = new PdfPCell(oTempTable);
        //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.FixedHeight = 15f; _oPdfPTable.AddCell(_oPdfPCell);

        //    _oPdfPTable.AddCell(this.SetCellValue("End Time", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, 1, 15f, _oFontStyle));
        //    _oPdfPTable.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, 1, 15f, _oFontStyle));
        //    _oPdfPTable.AddCell(this.SetCellValue("Cost/Kg(Tk.)", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, 1, 15f, _oFontStyle));
        //    _oPdfPTable.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, 1, 15f, _oFontStyle));
        //    _oPdfPTable.CompleteRow();


        //    var sValue = (_sRecipeWrittenBy != _sRecipeEditedBy && _sRecipeEditedBy != "") ? "Recipe edited by:   " + _sRecipeEditedBy : "";
        //    _oPdfPTable.AddCell(this.SetCellValue("Checked By:", 0, 2, Element.ALIGN_LEFT, BaseColor.WHITE, 1, 15f, _oFontStyle));
        //    _oPdfPTable.AddCell(this.SetCellValue(sValue, 0, 3, Element.ALIGN_LEFT, BaseColor.WHITE, 1, 15f, _oFontStyle));
        //    _oPdfPTable.CompleteRow();



        //}

        private void PrintFooter()
        {
            PdfPTable oPdfPTableTwo = new PdfPTable(6);
            PdfPCell oPdfPCell;
            oPdfPTableTwo.SetWidths(new float[] { 75f, 75f, 75f, 75f, 70f, 70f });
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);

            oPdfPTableTwo.AddCell(this.SetCellValue("", 0, 6, Element.ALIGN_CENTER, BaseColor.WHITE, 0, 10f, _oFontStyle));
            oPdfPTableTwo.CompleteRow();

            oPdfPTableTwo.AddCell(this.SetCellValue("Lab Sample", 0, 2, Element.ALIGN_CENTER, BaseColor.WHITE, 1, 0, _oFontStyle));
            oPdfPTableTwo.AddCell(this.SetCellValue("Lot Sample", 0, 2, Element.ALIGN_CENTER, BaseColor.WHITE, 1, 0, _oFontStyle));
            oPdfPTableTwo.AddCell(this.SetCellValue("Written By", 0, 2, Element.ALIGN_CENTER, BaseColor.WHITE, 1, 0, _oFontStyle));
            oPdfPTableTwo.CompleteRow();


            oPdfPTableTwo.AddCell(this.SetCellValue("", 11, 2, Element.ALIGN_CENTER, BaseColor.WHITE, 1, 70f, _oFontStyle));
            oPdfPTableTwo.AddCell(this.SetCellValue("", 11, 2, Element.ALIGN_CENTER, BaseColor.WHITE, 1, 70f, _oFontStyle));
            oPdfPTableTwo.AddCell(this.SetCellValue("", 0, 2, Element.ALIGN_CENTER, BaseColor.WHITE, 1, 15f, _oFontStyle));
            oPdfPTableTwo.CompleteRow();

            oPdfPTableTwo.AddCell(this.SetCellValue("Operator", 0, 2, Element.ALIGN_CENTER, BaseColor.WHITE, 1, 12f, _oFontStyle));
           
            oPdfPTableTwo.AddCell(this.SetCellValue("In", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, 1, 0, _oFontStyle));
            oPdfPTableTwo.AddCell(this.SetCellValue("Out", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, 1, 0, _oFontStyle));
            oPdfPTableTwo.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, 1, 15f, _oFontStyle));
            oPdfPTableTwo.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, 1, 15f, _oFontStyle));
            oPdfPTableTwo.AddCell(this.SetCellValue("Check By QC", 0, 2, Element.ALIGN_CENTER, BaseColor.WHITE, 1, 12f, _oFontStyle));
            oPdfPTableTwo.AddCell(this.SetCellValue("", 0, 2, Element.ALIGN_CENTER, BaseColor.WHITE, 1, 15, _oFontStyle));
            oPdfPTableTwo.AddCell(this.SetCellValue("Dyeing Incharge", 0, 2, Element.ALIGN_CENTER, BaseColor.WHITE, 1, 12f, _oFontStyle));

            oPdfPTableTwo.AddCell(this.SetCellValue("In", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, 1, 0, _oFontStyle));
            oPdfPTableTwo.AddCell(this.SetCellValue("Out", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, 1, 0, _oFontStyle));
            oPdfPTableTwo.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, 1, 15f, _oFontStyle));
            oPdfPTableTwo.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, 1, 15f, _oFontStyle));
            oPdfPTableTwo.AddCell(this.SetCellValue("Dyeing Master", 0, 2, Element.ALIGN_CENTER, BaseColor.WHITE, 1, 12f, _oFontStyle));
            oPdfPTableTwo.AddCell(this.SetCellValue("", 0, 2, Element.ALIGN_CENTER, BaseColor.WHITE, 1, 15f, _oFontStyle));

            _oPdfPCell = new PdfPCell(oPdfPTableTwo);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }

        private PdfPCell SetCellValue(string sName, int nRowSpan, int nColumnSpan, int align, BaseColor color, int border, float height, iTextSharp.text.Font oFontStyle)
        {
            PdfPCell oPdfPCell = new PdfPCell(new Phrase(sName, oFontStyle));
            oPdfPCell.Rowspan = (nRowSpan > 0) ? nRowSpan : 1;
            oPdfPCell.Colspan = (nColumnSpan > 0) ? nColumnSpan : 1;
            oPdfPCell.HorizontalAlignment = align;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPCell.BackgroundColor = color;
            if (border == 0)
                oPdfPCell.Border = 0;
            if (height > 0)
                oPdfPCell.MinimumHeight = height;
            return oPdfPCell;
        }
        private PdfPTable ShowRSNo()
        {
            PdfPTable oPdfPTable2 = new PdfPTable(2);
            oPdfPTable2.SetWidths(new float[] { 163f, 122f });

            _oPdfPCell = new PdfPCell(new Phrase("Dye Lot No", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Qty(" + _oRouteSheetSetup.MUnit + ")", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(_oPdfPCell);
            oPdfPTable2.CompleteRow();
            if (_oRouteSheetCombineDetails != null)
            {
                int nCount = 0;
                _oRouteSheetCombineDetails = _oRouteSheetCombineDetails.OrderBy(o => o.RouteSheetID).ToList();

                foreach (RouteSheetCombineDetail Oitem in _oRouteSheetCombineDetails)
                {
                    nCount++;
                    _oPhrase = new Phrase();
                    _oPhrase.Add(new Chunk(nCount.ToString()+". ", _oFontStyle));
                    _oPhrase.Add(new Chunk(Oitem.RouteSheetNo, _oFontStyle_Bold));
                  
                    _oPdfPCell = new PdfPCell(_oPhrase);
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(Oitem.Qty), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(_oPdfPCell);
                    oPdfPTable2.CompleteRow();
                }

            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase(_oRouteSheet.RouteSheetNo, _oFontStyle_Bold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(_oRouteSheet.Qty), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(_oPdfPCell);
                oPdfPTable2.CompleteRow();

            }
            //Also used in Beneficiary Certificate (Search "_oExportLC.DeliveryToName")
            return oPdfPTable2;
        }
        public static float CalculatePdfPTableHeight(PdfPTable table)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (Document doc = new Document(PageSize.TABLOID))
                {
                    using (PdfWriter w = PdfWriter.GetInstance(doc, ms))
                    {
                        doc.Open();
                        table.TotalWidth = 500f;
                        table.WriteSelectedRows(0, table.Rows.Count, 0, 0, w.DirectContent);

                        doc.Close();
                        return table.TotalHeight;
                    }
                }
            }
        }

        #region
        public byte[] PrepareWeighingSheet(RouteSheet oRouteSheet, List<RouteSheetDetail> oRSDetails, RouteSheetCombine oRouteSheetCombine, BusinessUnit oBusinessUnit, bool bIsCombine, RouteSheetSetup oRouteSheetSetup, List<DUPScheduleDetail> oDUPScheduleDetails, List<RouteSheetHistory> oRouteSheetHistorys, List<RSInQCSubStatus> oRSInQCSubStatus)
        {
            _oRouteSheet = oRouteSheet;
            _oRSDetail = oRouteSheet.RouteSheetDetail;
            _oRSDetails = oRSDetails;
            _oBusinessUnit = oBusinessUnit;
            _oRouteSheetCombine = oRouteSheetCombine;
            _oRouteSheetSetup=oRouteSheetSetup;
            _oDUPScheduleDetails = oDUPScheduleDetails;
            _oRSInQCSubStatus = oRSInQCSubStatus;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            _oDocument.SetMargins(5f, 5f, 05f, 20f);
            _oPdfPTable.WidthPercentage = 95;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_CENTER;

            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();

            _oPdfPTable.SetWidths(new float[] { 595f });
            #endregion
            GetsOrderInfo();
            this.ReporttHeader_WeighingSheet();
            this.ReportOrderInfo_WeighingSheet();
            this.Report_WeighingSheetDetail();
            this.Report_TotalChemical(oRouteSheetHistorys);
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        private void ReporttHeader_WeighingSheet()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oFontStyle_Bold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle_BoldTwo = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle_Bold_UnLine = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
            if (_oRSDetails.Count >= 0 && _oRSDetails.Count <=20)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.NORMAL);
                _oFontStyle_Bold = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD);
                _oFontStyle_BoldTwo = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD);
                _oFontStyle_Bold_UnLine = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
                _nRowHight = 18;
            }
            else if (_oRSDetails.Count > 20 && _oRSDetails.Count < 35)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                _oFontStyle_Bold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oFontStyle_BoldTwo = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oFontStyle_Bold_UnLine = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
                _nRowHight = 15;
            }
            else if (_oRSDetails.Count >= 35 && _oRSDetails.Count < 42)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                _oFontStyle_Bold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oFontStyle_BoldTwo = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oFontStyle_Bold_UnLine = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
                _nRowHight = 12;
            }
            else if (_oRSDetails.Count >= 42)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma",7f, iTextSharp.text.Font.NORMAL);
                _oFontStyle_Bold = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
                _oFontStyle_BoldTwo = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
                _oFontStyle_Bold_UnLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
                _nRowHight = 0;
            }


            #region  Heading Print

            GetHeadingPart();
            //_oPdfPCell = new PdfPCell(new Phrase(_oRouteSheetSetup.RSName, FontFactory.GetFont("Tahoma", 17f, iTextSharp.text.Font.BOLD)));
            //_oPdfPCell.Colspan = _nColumn; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.ExtraParagraphSpace = 10f; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();

            //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //_oPdfPCell.Colspan = _nColumn; _oPdfPCell.FixedHeight = 1f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.ExtraParagraphSpace = 10f; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();
            #endregion
        }

        private void GetHeadingPart()
        {
            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 20f, 60f, 20f});

            #region Barcode128
            Barcode128 code128 = new Barcode128();
            code128.CodeType = Barcode.CODE128;
            code128.ChecksumText = true;
            code128.GenerateChecksum = true;
            string str = _oRouteSheet.RouteSheetID.ToString("000000000");
            code128.Code = str;
            System.Drawing.Bitmap bm = new System.Drawing.Bitmap(code128.CreateDrawingImage(System.Drawing.Color.Black, System.Drawing.Color.White));
            iTextSharp.text.Image oImag = iTextSharp.text.Image.GetInstance(bm, System.Drawing.Imaging.ImageFormat.Bmp);
            oImag.ScaleAbsolute(80f, 15f);
            #endregion

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.Colspan = 3;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oImag); _oPdfPCell.Border = 0; _oPdfPCell.ExtraParagraphSpace = 10f;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oRouteSheetSetup.RSName, FontFactory.GetFont("Tahoma", 17f, iTextSharp.text.Font.BOLD))); _oPdfPCell.ExtraParagraphSpace = 10f; _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.ExtraParagraphSpace = 10f;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();

            #region push into main table
            _oPdfPCell = new PdfPCell(oPdfPTable); _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }

        private void ReportOrderInfo_WeighingSheet()
        {
            string stemp = "";
            //_oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            //_oFontStyle_Bold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            float nUsagesHeight = 0;
            PdfPTable oPdfPTable = new PdfPTable(6);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 50f, 128f, 47f, 112f, 38f,85f });

            oPdfPCell = new PdfPCell(new Phrase("Batch No", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(": " + _oRouteSheet.RouteSheetNo, _oFontStyle_Bold));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Raw Lot No", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(":"+_oRouteSheet.LotNo, _oFontStyle_Bold));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Date", _oFontStyle_Bold));
            oPdfPCell.Border = 0;  oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(":"+_oRouteSheet.RouteSheetDate.ToString("dd MMM yy mm:ss"), _oFontStyle_Bold));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("Machine", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(": " + _oRouteSheet.MachineName, _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Order", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            _oPhrase = new Phrase();
         
                _oPhrase.Add(new Chunk(": ", _oFontStyle));
                _oPhrase.Add(new Chunk(_oRouteSheet.OrderNo, _oFontStyle_Bold));
                _oPhrase.Add(new Chunk(" (Qty: " + Global.MillionFormat(_nOrderQty) + " " +_oRouteSheetSetup .MUnit+ ")", _oFontStyle));


            oPdfPCell = new PdfPCell(_oPhrase);
            oPdfPCell.Colspan = 3;
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();


            oPdfPCell = new PdfPCell(new Phrase("Weight", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            if (_oRouteSheet.QtyDye > 0)
            {
                oPdfPCell = new PdfPCell(new Phrase(": " + Global.MillionFormat(_oRouteSheet.Qty) +"+"+ Global.MillionFormat(_oRouteSheet.QtyDye) + " " + _oRouteSheetSetup.MUnit, _oFontStyle));
            }
            else
            {
                oPdfPCell = new PdfPCell(new Phrase(": " + Global.MillionFormat(_oRouteSheet.Qty) + " " + _oRouteSheetSetup.MUnit, _oFontStyle));
            }
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("Customer", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            stemp = "";
            if (_oRouteSheet.RouteSheetDOs.Count > 0)
            {
                stemp = string.Join(",", _oRouteSheet.RouteSheetDOs.Select(x => x.ContractorName).Distinct().ToList());
            }
            oPdfPCell = new PdfPCell(new Phrase(": " +stemp, _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.Colspan = 3;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("Color", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            stemp = "";
            if (_oRouteSheet.RouteSheetDOs.Count > 0)
            {
                stemp = string.Join(",", _oRouteSheet.RouteSheetDOs.Select(x => x.ColorName).Distinct().ToList());
            }
            oPdfPCell = new PdfPCell(new Phrase(": " + stemp, _oFontStyle));
            
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("Buyer", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            stemp = "";
            if (_oRouteSheet.RouteSheetDOs.Count > 0)
            {
                stemp = string.Join(",", _oRouteSheet.RouteSheetDOs.Select(x => x.DeliveryToName).Distinct().ToList());
            }
            oPdfPCell = new PdfPCell(new Phrase(": " + stemp, _oFontStyle)); ;
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.Colspan = 3;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("Lab Dip No", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            stemp = "";
            if (_oRouteSheet.RouteSheetDOs.Count > 0)
            {
                stemp = string.Join(",", _oRouteSheet.RouteSheetDOs.Select(x => x.ColorNo).Distinct().ToList());
            }

            oPdfPCell = new PdfPCell(new Phrase(": "+stemp, _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Yarn", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            stemp = "";
            if (_oRouteSheet.RouteSheetDOs.Count > 0)
            {
                stemp = string.Join(",", _oRouteSheet.RouteSheetDOs.Select(x => x.ProductName).Distinct().ToList());
                if (string.IsNullOrEmpty(stemp)) { stemp = _oRouteSheet.ProductName; }
            }
            oPdfPCell = new PdfPCell(new Phrase(": "+stemp, _oFontStyle)); 
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.Colspan = 3;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("Liquor Liter", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase(": " + _oRouteSheet.TtlLiquire + "  " + ((_oRouteSheet.Label > 0) ? "(Label:" + _oRouteSheet.Label.ToString() + ")" : ""), _oFontStyle));

            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("Match To", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            stemp = "";
            if (_oRouteSheet.RouteSheetDOs.Count > 0)
            {
                stemp = string.Join(",", _oRouteSheet.RouteSheetDOs.Select(x => x.ApproveLotNo).Distinct().ToList());
            }

            oPdfPCell = new PdfPCell(new Phrase(": " + stemp, _oFontStyle)); 
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            stemp = "";
            if (_oRouteSheet.RouteSheetDOs.Count > 0)
            {
                stemp = string.Join(",", _oRouteSheet.RouteSheetDOs.Select(x => x.RefNo).Distinct().ToList());
            }
            if (string.IsNullOrEmpty(stemp))
            {
                oPdfPCell.Colspan = 3;
            }
         //  
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            if (!string.IsNullOrEmpty(stemp))
            {

                oPdfPCell = new PdfPCell(new Phrase("Ref No: " + stemp, _oFontStyle));
                oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.Colspan = 2;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);
            }
            oPdfPTable.CompleteRow();

           // _oDUPSchedule
            oPdfPCell = new PdfPCell(new Phrase("Plan Date", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            stemp = "";
            if (_oDUPScheduleDetails.Count > 0)
            {
                stemp = _oDUPScheduleDetails[0].StartDateSt;
            }
            oPdfPCell = new PdfPCell(new Phrase(": " + stemp, _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Shift", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
           
            oPdfPCell = new PdfPCell(new Phrase(": " + _oRouteSheet.Shift, _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //oPdfPCell.Colspan = 3;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPCell = new PdfPCell(new Phrase(_oRouteSheetSetup.BatchCode, _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            stemp = "";
            if (_oDUPScheduleDetails.Count > 0)
            {
                stemp = string.Join("+", _oDUPScheduleDetails.Select(x => x.PSBatchNo).Distinct().ToList());
            }
            oPdfPCell = new PdfPCell(new Phrase(": " + stemp, _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //oPdfPCell.Colspan = 3;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            stemp = "";
            if (_oRouteSheet.RouteSheetDOs.Count > 0)
            {
                stemp = string.Join(",", _oRouteSheet.RouteSheetDOs.Select(x => x.Note).Distinct().ToList());
            }
            if (!string.IsNullOrEmpty(_oRouteSheet.Note))
            {
                stemp = stemp +"  "+  _oRouteSheet.Note;
            }
            //if (!string.IsNullOrEmpty(stemp))
            //{
                // Remarks
                oPdfPCell = new PdfPCell(new Phrase("Remarks", _oFontStyle));
                oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);
                if (!string.IsNullOrEmpty(stemp))
                {
                    oPdfPCell = new PdfPCell(new Phrase(": " + stemp, _oFontStyle));
                }
                else
                {
                      oPdfPCell = new PdfPCell(new Phrase(": " + stemp, _oFontStyle));
                }
                oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                oPdfPCell.Colspan = 3;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);

                stemp = string.Join(",", _oRouteSheet.RouteSheetDOs.Select(x => x.StyleNo).Distinct().ToList());

                if (!string.IsNullOrEmpty(stemp))
                {
                    oPdfPCell = new PdfPCell(new Phrase("Style No: " + stemp, _oFontStyle));
                }
                else
                {
                    oPdfPCell = new PdfPCell(new Phrase(": " + stemp, _oFontStyle));
                }
                oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                oPdfPCell.Colspan = 2;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPTable.CompleteRow();
           // }

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


           
        }
        private void Report_WeighingSheetDetail()
        {

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 2f; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthBottom = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(this.MiddlePart_Two());
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.BorderWidthBottom = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        private void Report_TotalChemical(List<RouteSheetHistory> oRouteSheetHistorys)
        {
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", 0, 0, BaseColor.WHITE,0,0,0, 10); //Min-Height: 10
            PdfPTable PdfPTable = new PdfPTable(4);
            PdfPTable.SetWidths(new float[] { 148.75f, 148.75f, 148.75f, 148.75f});


            ESimSolPdfHelper.AddCell(ref PdfPTable, "Recipe Written By", Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 15);
            ESimSolPdfHelper.AddCell(ref PdfPTable, (string.IsNullOrEmpty(_oRouteSheet.RecipeByName) ? "" : _oRouteSheet.RecipeByName), Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 15);
            ESimSolPdfHelper.AddCell(ref PdfPTable, "M/C Operation By", Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 15);
            if (oRouteSheetHistorys.FirstOrDefault() != null && oRouteSheetHistorys.FirstOrDefault().RouteSheetID > 0 && oRouteSheetHistorys.Where(b => (b.CurrentStatus == EnumRSState.LoadedInDyeMachine)).Count() > 0)
            {
                _sRecipeEditedBy = oRouteSheetHistorys.Where(p => p.CurrentStatus == EnumRSState.LoadedInDyeMachine).FirstOrDefault().EventEmpName;
            }
            
            ESimSolPdfHelper.AddCell(ref PdfPTable, _sRecipeEditedBy, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 15);

            ESimSolPdfHelper.AddCell(ref PdfPTable, "Recipe Checked By", Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 15);
            ESimSolPdfHelper.AddCell(ref PdfPTable, (string.IsNullOrEmpty(_oRouteSheet.ApproveByName) ? "" : _oRouteSheet.ApproveByName), Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 15);
            ESimSolPdfHelper.AddCell(ref PdfPTable, "M/C Loading By", Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 15);
            _sRecipeEditedBy = "";
            if (oRouteSheetHistorys.FirstOrDefault() != null && oRouteSheetHistorys.FirstOrDefault().RouteSheetID > 0 && oRouteSheetHistorys.Where(b => (b.CurrentStatus == EnumRSState.LoadedInDyeMachine)).Count() > 0)
            {
                _sRecipeEditedBy = oRouteSheetHistorys.Where(p => p.CurrentStatus == EnumRSState.LoadedInDyeMachine).FirstOrDefault().UserName;
            }
            
            ESimSolPdfHelper.AddCell(ref PdfPTable, _sRecipeEditedBy, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 15);

            ESimSolPdfHelper.AddCell(ref PdfPTable, "Dye Weight By", Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 15);
            _sRecipeEditedBy = "";
            if (oRouteSheetHistorys.FirstOrDefault() != null && oRouteSheetHistorys.FirstOrDefault().RouteSheetID > 0 && oRouteSheetHistorys.Where(b => (b.CurrentStatus == EnumRSState.DyesChemicalOut)).Count() > 0)
            {
                _sRecipeEditedBy = oRouteSheetHistorys.Where(p => p.CurrentStatus == EnumRSState.DyesChemicalOut).FirstOrDefault().UserName;
            }

            ESimSolPdfHelper.AddCell(ref PdfPTable, _sRecipeEditedBy, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, iTextSharp.text.Rectangle.BOX, 0, 15);
            ESimSolPdfHelper.AddCell(ref PdfPTable, "M/C Unloading By", Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 15);
            _sRecipeEditedBy = "";
            if (oRouteSheetHistorys.FirstOrDefault() != null && oRouteSheetHistorys.FirstOrDefault().RouteSheetID > 0 && oRouteSheetHistorys.Where(b => (b.CurrentStatus == EnumRSState.UnloadedFromDyeMachine)).Count() > 0)
            {
                _sRecipeEditedBy = oRouteSheetHistorys.Where(p => p.CurrentStatus == EnumRSState.UnloadedFromDyeMachine).FirstOrDefault().UserName;
            }
            ESimSolPdfHelper.AddCell(ref PdfPTable, _sRecipeEditedBy, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, iTextSharp.text.Rectangle.BOX, 0, 15);

            ESimSolPdfHelper.AddTable(ref _oPdfPTable,PdfPTable,15);

            _sRecipeEditedBy = "";
            if (_oRSInQCSubStatus.FirstOrDefault() != null && _oRSInQCSubStatus.FirstOrDefault().RouteSheetID > 0 && oRouteSheetHistorys.Count() > 0)
            {
                _sRecipeEditedBy = _oRSInQCSubStatus.FirstOrDefault().UpdateByName+" ["+_oRSInQCSubStatus.FirstOrDefault().RSSubStateStr+"]";
            }

            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL | iTextSharp.text.Font.UNDERLINE);
            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "Final Checked By :" + _sRecipeEditedBy, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER, 0, 0, 20);
        }

        private PdfPTable MiddlePart_Two()
        {
            #region RouteSheetDetail
         

            PdfPTable oPdfPTableTwo = new PdfPTable(6);
            PdfPCell oPdfPCell;
            oPdfPTableTwo.SetWidths(new float[] { 140f, 52f, 50f, 65f, 73f, 60f });


            oPdfPTableTwo.AddCell(this.SetCellValue("D/A Name", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, 1, 14f, _oFontStyle_Bold));
            oPdfPTableTwo.AddCell(this.SetCellValue("Amount(g/l)", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, 1, 14f, _oFontStyle_Bold));
            oPdfPTableTwo.AddCell(this.SetCellValue("Amount(%)", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, 1, 14f, _oFontStyle_Bold));
            oPdfPTableTwo.AddCell(this.SetCellValue("Total Weight", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, 1, 14f, _oFontStyle_Bold));
            oPdfPTableTwo.AddCell(this.SetCellValue("Addition-1(gm)", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, 1, 14f, _oFontStyle_Bold));
            oPdfPTableTwo.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, 1, 14f, _oFontStyle));
            oPdfPTableTwo.CompleteRow();


            foreach (RouteSheetDetail oItem in _oRSDetail.children)
            {
                int nSapn = 1;
                if (oItem.children.Count() > 0)
                {
                    nSapn = this.DepthChecker(oItem.children);
                    RecursiveChecking(oItem, ref nSapn, oPdfPTableTwo);
                }
                else
                {
                    SetInfoToGrid(oItem, ref nSapn, "", oPdfPTableTwo);
                }
            }
            #endregion RouteSheetDetail

            #region Summary

            //int[] ParentIDs = _oRSDetails.Where(x => x.ProcessName.ToUpper().Trim() == "COTTON DYEING").Select(x => x.RouteSheetDetailID).ToArray();
            //_oRSDetails = _oRSDetails.Where(x => ParentIDs.Contains(x.ParentID) && x.ProductCategoryName == "Dyes").ToList();

            //int[] ParentIDs = _oRSDetails.Where(x => x.ProcessName.ToUpper().Trim() == "COTTON DYEING").Select(x => x.RouteSheetDetailID).ToArray();
            _oRSDetails = _oRSDetails.Where(x => x.ProductCategoryName == "Dyes" && x.IsDyesChemical==true ).ToList();

            var nPercentage = _oRSDetails.Sum(x => (x.Percentage + ((x.Percentage * x.DeriveGL) / 100)));
            var nAdditionalPercentage = _oRSDetails.Sum(x => x.AddOnePercentage + x.AddTwoPercentage + x.AddThreePercentage);
            var nGTPercentage = nPercentage + nAdditionalPercentage;

        //    var nTotal = _oRSDetails.Sum(x => (x.Percentage + ((x.Percentage * x.DeriveGL) / 100) + x.AddOnePercentage + x.AddTwoPercentage + x.AddThreePercentage));

            oPdfPTableTwo.AddCell(this.SetCellValue("Net: " + Global.MillionFormat(nPercentage) + "%", 0, 3, Element.ALIGN_RIGHT, BaseColor.WHITE, 0, 0, _oFontStyle));
            oPdfPTableTwo.AddCell(this.SetCellValue("Additional: " + Global.MillionFormat(nAdditionalPercentage) + "%", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, 0, 0, _oFontStyle));
            oPdfPTableTwo.AddCell(this.SetCellValue("Total: " + Global.MillionFormat(nGTPercentage) + "%", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, 0, 0, _oFontStyle));
            oPdfPTableTwo.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, 0, 0, _oFontStyle));
            oPdfPTableTwo.CompleteRow();
            #endregion

            return oPdfPTableTwo;
        }

        #endregion
        #region Combine B
        public byte[] PrepareReport_Combine_B(RouteSheet oRouteSheet, List<RouteSheetDetail> oRSDetails, RouteSheetCombine oRouteSheetCombine, Company oCompany, BusinessUnit oBusinessUnit, bool bIsCombine, RouteSheetSetup oRouteSheetSetup, List<DUPScheduleDetail> oDUPScheduleDetails, List<RouteSheetHistory> oRouteSheetHistorys)
        {
            _oRouteSheet = oRouteSheet;
            _oRSDetail = oRouteSheet.RouteSheetDetail;
            _oRSDetails = oRSDetails;
            _oBusinessUnit = oBusinessUnit;
            _oRouteSheetCombine = oRouteSheetCombine;
            _oRouteSheetCombineDetails = oRouteSheetCombine.RouteSheetCombineDetails;
            _oDUPScheduleDetails = oDUPScheduleDetails;
            _oRouteSheetSetup = oRouteSheetSetup;
            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            _oDocument.SetMargins(10f, 10f, 10f, 10f);
            _oPdfPTable.WidthPercentage = 95;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_CENTER;

            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();

            _oPdfPTable.SetWidths(new float[] { 595f });
            #endregion
         
            //this.ReporttHeader();
            this.ReporttHeader_WeighingSheet();
            this.ReportScheduleInfo_Combo_B();
            this.ReportOrderInfo_Combo_B();
            this.Report_WeighingSheetDetail();
            this.Report_TotalChemical(oRouteSheetHistorys);

            //PrintFooter();

            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
     
        private void ReportOrderInfo_Combine()
        {
            PdfPTable oPdfPTable = new PdfPTable(6);
            oPdfPTable.SetWidths(new float[] { 35f, 70f, 60f, 70f ,35f, 60f });


            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Raw Lot", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, " " + _oRouteSheet.LotNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle_Bold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Machine", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oRouteSheet.MachineName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle_Bold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Date", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oRouteSheet.RouteSheetDateStr, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle_Bold);

            oPdfPTable.CompleteRow();


            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Remarks", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, " " + _oRouteSheet.Note, 0, 5, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle_Bold);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);



            
        
          var  RouteSheetDOsCon = _oRouteSheet.RouteSheetDOs.GroupBy(x => new {x.OrderNo,x.NoCode, x.ContractorID, x.ContractorName}, (key, grp) =>
                                    new RouteSheetDO
                                    {
                                        OrderNo=key.OrderNo,
                                        NoCode=key.NoCode,
                                        ContractorID = key.ContractorID,
                                        ContractorName = key.ContractorName,
                                    }).ToList();
          int nSLNo = 0;
          foreach (var oItem1 in RouteSheetDOsCon)
          {
              oPdfPTable = new PdfPTable(6);
              oPdfPTable.SetWidths(new float[] { 60f, 60f, 42f, 38f, 32f, 32f });

              nSLNo++;
              ESimSolItexSharp.SetCellValue(ref oPdfPTable, nSLNo.ToString()+". Order No:" + oItem1.OrderNoFull, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle_Bold);
              ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Buyer:" + oItem1.ContractorName, 0, 5, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle_Bold);

              oPdfPTable.CompleteRow();
              ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Yarn Type", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle_Bold);
              ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Color", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle_Bold);
              ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Color No", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle_Bold);
              ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Batch No", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle_Bold);
              ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Qty(" + _oRouteSheetSetup.MUnit + ")", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle_Bold);
              ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Qty(" + _oRouteSheetSetup.MUnitTwo + ")", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle_Bold);


              oPdfPTable.CompleteRow();
              ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);


              var oRouteSheetDOsYarnOne = _oRouteSheet.RouteSheetDOs.Where(x => x.ContractorID == oItem1.ContractorID && x.OrderNo == oItem1.OrderNo).ToList();

              var oRouteSheetDOsYarn = oRouteSheetDOsYarnOne.GroupBy(x => new { x.ProductName, x.ProductID }, (key, grp) =>
                                new
                                {
                                    ProductID = key.ProductID,
                                    ProductName = key.ProductName,
                                    Color_Grp = grp.GroupBy(p => new { p.RouteSheetID, p.ColorName, p.ColorNo, p.ApproveLotNo, p.Shade,p.PantonNo }, (color_key, color_grp) => new
                                    {
                                        ColorName = color_key.ColorName,
                                        ColorNo = color_key.ColorNo,
                                        PantonNo = color_key.PantonNo,
                                        Shade = color_key.Shade,
                                        ApproveLotNo = color_key.ApproveLotNo,
                                        RouteSheetID = color_key.RouteSheetID,
                                        Qty_RS = color_grp.Sum(p => p.Qty_RS),
                                    }).ToList(),

                                }).ToList();

              oPdfPTable = new PdfPTable(6);
              oPdfPTable.SetWidths(new float[] { 60f, 60f, 42f, 38f, 32f, 32f });

              foreach (var oItem in oRouteSheetDOsYarn)
              {
                  int nRowSpan = oItem.Color_Grp.Count();

                  ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.ProductName, nRowSpan, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                  
                  foreach (var oColor in oItem.Color_Grp)
                  {
                      ESimSolItexSharp.SetCellValue(ref oPdfPTable, oColor.ColorName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                      ESimSolItexSharp.SetCellValue(ref oPdfPTable,oColor.ColorNo+" "+ oColor.PantonNo + ((oColor.Shade>0)?" (" + ((EnumShade)oColor.Shade).ToString() + ")":""), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                     _sOrderType= _oRouteSheetCombineDetails.Where(x => x.RouteSheetID == oColor.RouteSheetID).FirstOrDefault().RouteSheetNo;
                     ESimSolItexSharp.SetCellValue(ref oPdfPTable, _sOrderType, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                     ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oColor.Qty_RS), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                     ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oColor.Qty_RS * _oRouteSheetSetup.SMUnitValue), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                      oPdfPTable.CompleteRow();
                  }
                
                 
              }
              oPdfPTable.CompleteRow();
              ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

          }

          oPdfPTable = new PdfPTable(6);
          oPdfPTable.SetWidths(new float[] { 60f, 60f, 42f, 38f, 32f, 32f });

          double nQty = 0;
          double nQtyBag = 0;
          if (_oRouteSheetCombineDetails.Count > 0)
          {
              nQtyBag = _oRouteSheetCombineDetails.Sum(x => x.NoOfHanksCone);
              nQty = _oRouteSheetCombineDetails.Sum(x => x.Qty);
          }
          else
          {
              nQty = _oRouteSheet.Qty;
              nQtyBag = _oRouteSheet.NoOfHanksCone;
          }
          ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((nQtyBag>0)?nQtyBag.ToString() + " Bag":""), 0, 2, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle_Bold);
          //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle_Bold);
          ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total Weight", 0, 2, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle_Bold);
          ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nQty), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle_Bold);
          ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nQty*_oRouteSheetSetup.SMUnitValue), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle_Bold);

          oPdfPTable.CompleteRow();
          ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);


        }
        private void ReportScheduleInfo_Combo_B()
        {
            float nUsagesHeight = 0;
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oFontStyle_Bold = FontFactory.GetFont("Tahoma",8f, iTextSharp.text.Font.BOLD);

            PdfPTable oPdfPTable = new PdfPTable(7);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 30f, 80f, 60f,60f, 80f,110f,50f });

            oPdfPCell = new PdfPCell(new Phrase("SL#", _oFontStyle_Bold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Order No", _oFontStyle_Bold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("Batch No", _oFontStyle_Bold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" Qty("+_oRouteSheetSetup.MUnit+")", _oFontStyle_Bold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);



            oPdfPCell = new PdfPCell(new Phrase("Raw Lot No", _oFontStyle_Bold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("Color", _oFontStyle_Bold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle_Bold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();
            if (_oDUPScheduleDetails.Count>0)
            {
                foreach (DUPScheduleDetail oItem in _oDUPScheduleDetails)
                {
                    _nCount++;
                    #region PrintDetail

                    oPdfPCell = new PdfPCell(new Phrase(_nCount.ToString(), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    //oPdfPCell.FixedHeight = 40f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.OrderNo, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    //oPdfPCell.FixedHeight = 40f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);



                    oPdfPCell = new PdfPCell(new Phrase(oItem.RouteSheetNo, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    //oPdfPCell.FixedHeight = 40f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);
                    if (_oRouteSheetCombineDetails.Count > 0)
                    {
                        oItem.Qty = _oRouteSheet.RouteSheetDOs.Where(x => x.RouteSheetID == oItem.RouteSheetID && x.DyeingOrderDetailID == oItem.DODID).Select(c => c.Qty_RS).Sum();
                        
                    }
                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty)+ " " + oItem.MUnit, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    //oPdfPCell.FixedHeight = 40f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.ApproveLotNo, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f;
                    //oPdfPCell.FixedHeight = 40f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.ColorName, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f;
                    //oPdfPCell.FixedHeight = 40f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(_oRouteSheetSetup.BatchCode+" "+oItem.PSBatchNo, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f;
                    //oPdfPCell.FixedHeight = 40f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);
                 
                    oPdfPTable.CompleteRow();
                    #endregion
                }
            }
            else
            {
                foreach (RouteSheetCombineDetail oItem in _oRouteSheetCombine.RouteSheetCombineDetails)
                {
                    _nCount++;
                    #region PrintDetail

                    oPdfPCell = new PdfPCell(new Phrase(_nCount.ToString(), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    //oPdfPCell.FixedHeight = 40f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.OrderNo, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    //oPdfPCell.FixedHeight = 40f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);



                    oPdfPCell = new PdfPCell(new Phrase(oItem.RouteSheetNo, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    //oPdfPCell.FixedHeight = 40f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);
                    //if (_oRouteSheetCombineDetails.Count > 0)
                    //{
                   //  oItem.Qty = _oRouteSheet.RouteSheetDOs.Where(x => x.RouteSheetID == oItem.RouteSheetID && x.DyeingOrderDetailID == oItem.DODID).Select(c => c.U).Sum();

                    //}
                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty) + " " , _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    //oPdfPCell.FixedHeight = 40f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oItem.Note = _oRouteSheet.RouteSheetDOs.Where(x => x.RouteSheetID == oItem.RouteSheetID).FirstOrDefault().ApproveLotNo;

                    oPdfPCell = new PdfPCell(new Phrase(oItem.Note, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f;
                    //oPdfPCell.FixedHeight = 40f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.ColorName, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f;
                    //oPdfPCell.FixedHeight = 40f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(_oRouteSheetSetup.BatchCode , _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f;
                    //oPdfPCell.FixedHeight = 40f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPTable.CompleteRow();
                    #endregion
                }
            }
         

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

        
         

        }
        private void ReportOrderInfo_Combo_B()
        {
            string stemp = "";
            //_oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            //_oFontStyle_Bold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            float nUsagesHeight = 0;
            PdfPTable oPdfPTable = new PdfPTable(6);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 50f, 128f, 47f, 112f, 38f, 85f });

            oPdfPCell = new PdfPCell(new Phrase("Machine", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(": " + _oRouteSheet.MachineName, _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Order", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            _oPhrase = new Phrase();

            _oPhrase.Add(new Chunk(": ", _oFontStyle));
            _oPhrase.Add(new Chunk(_oRouteSheet.OrderNo, _oFontStyle_Bold));
            if (_nOrderQty > 0)
            {
                _oPhrase.Add(new Chunk(" (Qty: " + Global.MillionFormat(_nOrderQty) + " " + _oRouteSheetSetup.MUnit + ")", _oFontStyle));
            }


            oPdfPCell = new PdfPCell(_oPhrase);
            oPdfPCell.Colspan = 3;
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();


            oPdfPCell = new PdfPCell(new Phrase("Weight", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            //_oRouteSheet.TtlLiquire = _oRouteSheet.Qty * 6;
            if (_oRouteSheet.QtyDye > 0)
            { oPdfPCell = new PdfPCell(new Phrase(": " + Global.MillionFormat(_oRouteSheet.Qty) + "+" + Global.MillionFormat(_oRouteSheet.QtyDye) + " " + _oRouteSheetSetup.MUnit, _oFontStyle_Bold)); }
            else
            {
                oPdfPCell = new PdfPCell(new Phrase(": " + Global.MillionFormat(_oRouteSheet.Qty) + " " + _oRouteSheetSetup.MUnit, _oFontStyle_Bold));
            }
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("Customer", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            stemp = "";
            if (_oRouteSheet.RouteSheetDOs.Count > 0)
            {
                stemp = string.Join(",", _oRouteSheet.RouteSheetDOs.Select(x => x.ContractorName).Distinct().ToList());
            }
            oPdfPCell = new PdfPCell(new Phrase(": " + stemp, _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.Colspan = 3;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("Color", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            stemp = "";
            if (_oRouteSheet.RouteSheetDOs.Count > 0)
            {
                stemp = string.Join(",", _oRouteSheet.RouteSheetDOs.Select(x => x.ColorName).Distinct().ToList());
            }
            oPdfPCell = new PdfPCell(new Phrase(": " + stemp, _oFontStyle));

            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("Buyer", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            stemp = "";
            if (_oRouteSheet.RouteSheetDOs.Count > 0)
            {
                stemp = string.Join(",", _oRouteSheet.RouteSheetDOs.Select(x => x.DeliveryToName).Distinct().ToList());
            }
            oPdfPCell = new PdfPCell(new Phrase(": " + stemp, _oFontStyle)); ;
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.Colspan = 3;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("Lab Dip No", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            stemp = "";
            if (_oRouteSheet.RouteSheetDOs.Count > 0)
            {
                stemp = string.Join(",", _oRouteSheet.RouteSheetDOs.Select(x => x.ColorNo).Distinct().ToList());
            }

            oPdfPCell = new PdfPCell(new Phrase(": " + stemp, _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Yarn", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            stemp = "";
            if (_oRouteSheet.RouteSheetDOs.Count > 0)
            {
                stemp = string.Join(",", _oRouteSheet.RouteSheetDOs.Select(x => x.ProductName).Distinct().ToList());
                if (string.IsNullOrEmpty(stemp)) { stemp = _oRouteSheet.ProductName; }
            }
            oPdfPCell = new PdfPCell(new Phrase(": " + stemp, _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.Colspan = 3;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("Liquor Liter", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase(": " + _oRouteSheet.TtlLiquire, _oFontStyle));

            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("Match To", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            stemp = "";
            if (_oRouteSheet.RouteSheetDOs.Count > 0)
            {
                stemp = string.Join(",", _oRouteSheet.RouteSheetDOs.Select(x => x.ApproveLotNo).Distinct().ToList());
            }

            oPdfPCell = new PdfPCell(new Phrase(": " + stemp, _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.Colspan = 3;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            // _oDUPSchedule
            oPdfPCell = new PdfPCell(new Phrase("Plan Date", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            stemp = "";
            if (_oDUPScheduleDetails.Count > 0)
            {
                stemp = _oDUPScheduleDetails[0].StartDateSt;
            }
            oPdfPCell = new PdfPCell(new Phrase(": " + stemp, _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Shift", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(": " + _oRouteSheet.Shift, _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //oPdfPCell.Colspan = 3;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            stemp = "";
            if (_oDUPScheduleDetails.Count > 0)
            {
                stemp = string.Join("+", _oDUPScheduleDetails.Select(x => x.PSBatchNo).Distinct().ToList());
            }
            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //oPdfPCell.Colspan = 3;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            stemp = "";
            if (_oRouteSheet.RouteSheetDOs.Count > 0)
            {
                stemp = string.Join(",", _oRouteSheet.RouteSheetDOs.Select(x => x.Note).Distinct().ToList());
            }
            if (!string.IsNullOrEmpty(_oRouteSheet.Note))
            {
                stemp = stemp + "  " + _oRouteSheet.Note;
            }
            //if (!string.IsNullOrEmpty(stemp))
            //{
                // Remarks
                oPdfPCell = new PdfPCell(new Phrase("Remarks", _oFontStyle));
                oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(": " + stemp, _oFontStyle));
                oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                oPdfPCell.Colspan = 3;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase( _oRouteSheetCombine.Note, _oFontStyle));
                oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                oPdfPCell.Colspan = 2;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPTable.CompleteRow();
            //}

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();



        }
        #endregion
    }

}
