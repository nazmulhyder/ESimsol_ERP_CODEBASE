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
    public class rptDeliveryChallanPackingList
    {
        #region Declaration
        int _nTotalColumn = 1;
        float _nfixedHight = 5f;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyle_UnLine;
        iTextSharp.text.Font _oFontStyleBold;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        DUDeliveryChallan _oDUDeliveryChallan = new DUDeliveryChallan();
        List<DUDeliveryChallanDetail> _oDUDeliveryChallanDetails = new List<DUDeliveryChallanDetail>();
        Company _oCompany = new Company();
        DeliverySetup _oDeliverySetup = new DeliverySetup();
        Phrase _oPhrase = new Phrase();
        int _nTitleType = 0;
        bool _bPrintFormat = false;
        #endregion
        #region Body
        public byte[] PrepareReport(DUDeliveryChallan oDUDeliveryChallan, Company oCompany, int nTitleType, bool bPrintFormat)
        {
            _oDUDeliveryChallan = oDUDeliveryChallan;
            _oDUDeliveryChallanDetails = oDUDeliveryChallan.DUDeliveryChallanDetails;
            _oCompany = oCompany;
            _nTitleType = nTitleType;
            _bPrintFormat = bPrintFormat;
            _oDeliverySetup = oDUDeliveryChallan.DeliverySetup;

            #region Page Setup
            _oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
            _oDocument.SetMargins(30f, 30f, 10f, 30f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();

            _oPdfPTable.SetWidths(new float[] { 595f });
            #endregion

            this.HeaderWithThreeFormats();
            this.ReporttHeader();
            this.PrintHead_Sample();
            this.PrintDetails();
            
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
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));

            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.PringReportHead, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            #region ReportHeader
            #region Blank Space
            _oFontStyle = FontFactory.GetFont("Tahoma", 5f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion
            #endregion

            #endregion
        }
        private void HeaderWithThreeFormats()
        {
            //_nPrintType = 1 means Normal Format
            //_nPrintType = 2 means PAD Format
            //_nPrintType = 3 means Full Image Title with logo
            if (_nTitleType == 1)
            {
                this.PrintHeader();
            }
            else if (_nTitleType == 2)
            {
                #region PAD Format
                _oFontStyle = FontFactory.GetFont("Tahoma", 15f, iTextSharp.text.Font.BOLD);
                _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.UNDERLINE);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                //_oPdfPCell.Colspan = _nColumn;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; ;
                _oPdfPCell.FixedHeight = 120f; _oPdfPCell.BorderWidth = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion
            }
            else if (_nTitleType == 3)
            {
                #region Image Format (Title with LOGO)
                if (_oDeliverySetup.Image != null)
                {
                    _oImag = iTextSharp.text.Image.GetInstance(_oDeliverySetup.Image, System.Drawing.Imaging.ImageFormat.Jpeg);
                    _oImag.ScaleAbsolute(530f, 80f);
                    _oPdfPCell = new PdfPCell(_oImag);
                    _oPdfPCell.Border = 0;
                    //_oPdfPCell.Colspan = _nColumn;
                    _oPdfPCell.FixedHeight = 80f;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                    _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();

                    _oFontStyle = FontFactory.GetFont("Tahoma", 5f, 1);
                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.FixedHeight = 10f; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                }
                else
                {
                    this.PrintHeader_Blank();
                }
                #endregion
            }

            //_oFontStyle = FontFactory.GetFont("Tahoma", 14f, 1);
            //_oPdfPCell = new PdfPCell(new Phrase(sDocHeader, _oFontStyle));
            //_oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();
        }

        private void PrintHeader_Blank()
        {
            #region ReportHeader
            _oFontStyle = FontFactory.GetFont("Tahoma", 5f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.FixedHeight = 70f; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }

        private void ReporttHeader()
        {
            //_oPdfPCell = new PdfPCell(new Phrase("", FontFactory.GetFont("Tahoma", 15f, iTextSharp.text.Font.BOLD)));
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.FixedHeight = 100f; _oPdfPCell.Border = 0;
            //_oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();

            string sHeaderName = "PACKING LIST";
            #region Proforma Invoice Heading Print
            _oPdfPCell = new PdfPCell(new Phrase(sHeaderName, FontFactory.GetFont("Tahoma", 15f, iTextSharp.text.Font.BOLD)));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        #endregion


        #region Report Body
        private void PrintHead_Sample()
        {
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL);

            PdfPTable oPdfPTable = new PdfPTable(4);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 100f, 196f, 120f, 180f });

            oPdfPCell = new PdfPCell(new Phrase("DATE", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oDUDeliveryChallan.ChallanDateSt, _oFontStyle));
            //oPdfPCell.Border = 0;

            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("CHALLAN NO", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(_oDUDeliveryChallan.ChallanNo, _oFontStyleBold));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("BUYER NAME", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oDUDeliveryChallan.ContractorName, _oFontStyleBold));
            //oPdfPCell.Border = 0;
            oPdfPCell.Colspan = 3;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            if (!String.IsNullOrEmpty(_oDUDeliveryChallan.FactoryAddress))
            {
                oPdfPCell = new PdfPCell(new Phrase("ADDRESS", _oFontStyle));
                //oPdfPCell.Border = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(" " + _oDUDeliveryChallan.FactoryAddress, _oFontStyle));
                //oPdfPCell.Border = 0;
                oPdfPCell.Colspan = 3;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();
            }

            if (_oDUDeliveryChallan.OrderType == (int)EnumOrderType.BulkOrder || _oDUDeliveryChallan.OrderType == (int)EnumOrderType.DyeingOnly)
            {
                oPdfPCell = new PdfPCell(new Phrase("PI No", _oFontStyle));
                //oPdfPCell.Border = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(" " + _oDUDeliveryChallan.OrderNos, _oFontStyle));
                //oPdfPCell.Border = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);
            }
            else
            {
                oPdfPCell = new PdfPCell(new Phrase("DO No", _oFontStyle));
                //oPdfPCell.Border = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(" " + _oDUDeliveryChallan.DONos, _oFontStyle));
                //oPdfPCell.Border = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);
            }
            
            oPdfPCell = new PdfPCell(new Phrase("CARRIER", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(_oDUDeliveryChallan.VehicleName, _oFontStyleBold));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("VEHICLE NO.", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oDUDeliveryChallan.VehicleNo, _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("DRIVER NAME", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(_oDUDeliveryChallan.ReceivedByName, _oFontStyleBold));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            if (!String.IsNullOrEmpty(_oDUDeliveryChallan.Note))
            {

                oPdfPCell = new PdfPCell(new Phrase("REMARKS:", _oFontStyle));
                //oPdfPCell.Border = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(" " + _oDUDeliveryChallan.Note, _oFontStyle));
                //oPdfPCell.Border = 0;
                oPdfPCell.Colspan = 3;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);
            }
            if (!String.IsNullOrEmpty(_oDUDeliveryChallan.DeliveryZone))
            {

                oPdfPCell = new PdfPCell(new Phrase("Delivery info:", _oFontStyle));
                //oPdfPCell.Border = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(" " + _oDUDeliveryChallan.DeliveryZone, _oFontStyle));
                //oPdfPCell.Border = 0;
                oPdfPCell.Colspan = 3;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);
            }

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }

        private void PrintDetails()
        {
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);

            _oPdfPCell = new PdfPCell(new Phrase("", FontFactory.GetFont("Tahoma", 15f, iTextSharp.text.Font.BOLD)));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.FixedHeight = 13f; _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            string sheader = "";
            if (_oDUDeliveryChallan.DUDeliveryChallanDetails.Count > 0)
            {
                 sheader = _oDUDeliveryChallan.DUDeliveryChallanDetails[0].MUnit;
            }

            PdfPTable oPdfPTable = new PdfPTable(6);
            oPdfPTable.SetWidths(new float[] { 25f, 130f, 120f, 60f, 60f, 60f });

            #region Header
            _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyleBold)); _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Color", _oFontStyleBold)); _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Lot No", _oFontStyleBold)); _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Bag No", _oFontStyleBold)); _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Qty(" + sheader + ")", _oFontStyleBold)); _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Total Qty(" + sheader + ")", _oFontStyleBold)); _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #region push main table
            _oPdfPCell = new PdfPCell(oPdfPTable); _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; 
            _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            #endregion

            int nSl = 0, nCount = 0;
            //List<DUDeliveryChallanPack> oTempDUDeliveryChallanPacks = new List<DUDeliveryChallanPack>();
            //var data = _oDUDeliveryChallan.DUDeliveryChallanDetails.GroupBy(x => new { x.ProductID, x.ProductName }, (key, grp) => new
            //{
            //    ProductID = key.ProductID,
            //    ProductName = key.ProductName,
            //    Results = grp.ToList()
            //});
            var data = _oDUDeliveryChallan.DUDeliveryChallanPacks.GroupBy(x => new { x.DUDeliveryChallanDetailID }, (key, grp) => new
            {
                DUDeliveryChallanDetailID = key.DUDeliveryChallanDetailID,
                Results = grp.ToList()
            });

            DUDeliveryChallanDetail oTempDUDeliveryChallanDetail = new DUDeliveryChallanDetail();           
            foreach (var oData in data)
            {
                nCount = 0;
                oPdfPTable = new PdfPTable(6);
                oPdfPTable.SetWidths(new float[] { 25f, 130f, 120f, 60f, 60f, 60f });
                oTempDUDeliveryChallanDetail = _oDUDeliveryChallan.DUDeliveryChallanDetails.Where(x => x.DUDeliveryChallanDetailID == oData.DUDeliveryChallanDetailID).FirstOrDefault();

                _oPdfPCell = new PdfPCell(new Phrase("Product: " + oTempDUDeliveryChallanDetail.ProductName, _oFontStyleBold)); _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.Colspan = oPdfPTable.NumberOfColumns; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((++nSl).ToString(), _oFontStyle)); _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.Rowspan = oData.Results.Count(); oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oTempDUDeliveryChallanDetail.ColorName, _oFontStyle)); _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.Rowspan = oData.Results.Count(); oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oTempDUDeliveryChallanDetail.LotNo, _oFontStyle)); _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.Rowspan = oData.Results.Count(); oPdfPTable.AddCell(_oPdfPCell);

                foreach (var oItem in oData.Results)
                {
                    nCount++;
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.BagNo.ToString(), _oFontStyle)); _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.QTY.ToString("###,0.00;(###,0.00)"), _oFontStyle)); _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(_oPdfPCell);

                    if (nCount == 1)
                    {
                        //_oPdfPCell = new PdfPCell(new Phrase(oData.Results.Sum(x => x.QTY).ToString("###,0.00;(###,0.00)"), _oFontStyle)); _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //_oPdfPCell.Rowspan = oData.Results.Count(); oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPCell = new PdfPCell(new Phrase(oTempDUDeliveryChallanDetail.Qty.ToString("###,0.00;(###,0.00)"), _oFontStyle)); _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.Rowspan = oData.Results.Count(); oPdfPTable.AddCell(_oPdfPCell);
                    }
                }

                //oPdfPTable.CompleteRow();
                #region push main table
                _oPdfPCell = new PdfPCell(oPdfPTable); _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell); //_oPdfPCell.MinimumHeight = 800f;
                _oPdfPTable.CompleteRow();
                #endregion

            }

            
        }

        #endregion
        #endregion


    }
}
