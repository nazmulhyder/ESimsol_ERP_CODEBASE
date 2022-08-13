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
    public class rptFabricSalesContract
    {
        #region Declaration
        int _nTotalColumn = 1;
        float _nfixedHight = 5f;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyle_UnLine;
        bool _bIsLog = false;
        iTextSharp.text.Font _oFontStyleBold;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        Phrase _oPhrase = new Phrase();
        iTextSharp.text.Image _oImag;
        iTextSharp.text.Image _oPreparedImag;
        iTextSharp.text.Image _oApprovedImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        FabricSalesContract _oFabricSalesContract = new FabricSalesContract();
        List<FabricSalesContractDetail> _oFabricSalesContractDetails = new List<FabricSalesContractDetail>();
        List<FabricDeliverySchedule> _oFebricDeliverySchedules = new List<FabricDeliverySchedule>();
        BusinessUnit _oBusinessUnit = new BusinessUnit();
        List<FabricSalesContractNote> _oFabricSalesContractNotes = new List<FabricSalesContractNote>();
        List<FabricSalesContractDetail> _discountFabricSalesContractDetail = new List<FabricSalesContractDetail>();
        Company _oCompany = new Company();
        List<FabricSCHistory> _oFabricSCHistorys = new List<FabricSCHistory>();
        List<ExportPI> _oExportPIs = new List<ExportPI>();
        FabricOrderSetup _oFabricOrderSetup = new FabricOrderSetup();
        string _sMUnit = "";
        bool _bIsRnd = false;
        int _nCount = 0;
        int _nCount_P = 0;
        double _nTotalAmount = 0;
        double _nTotalQty = 0;
        double _nTotalDSQty = 0;
        float _nUsagesHeight = 0;
        #endregion
        float nUsagesHeight = 0;
        #region Production Request

        #region Print Header
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

        private void ReporttHeader()
        {
            string sHeaderName = "";
            #region Proforma Invoice Heading Print
            if (_oFabricSalesContract.OrderType == (int)EnumFabricRequestType.HandLoom)
            {
                sHeaderName = "Handloom Development Request";
            }
            else if (_oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Sample)
            {
                sHeaderName = "Sample Development Request";
            }
            else if (_oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Bulk)
            {
                sHeaderName = "Bulk Production Request";
            }
            else if (_oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Buffer)
            {
                sHeaderName = "Buffer Production Request";
            }
            else if (_oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Analysis)
            {
                sHeaderName = "Analysis Request";
            }
            else if (_oFabricSalesContract.OrderType == (int)EnumFabricRequestType.CAD)
            {
                sHeaderName = "CAD Request";
            }
            else if (_oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Color)
            {
                sHeaderName = "Color Standard Request";
            }
            else if (_oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Labdip)
            {
                sHeaderName = "Labdip Request";
            }
            else if (_oFabricSalesContract.OrderType == (int)EnumFabricRequestType.YarnSkein)
            {
                sHeaderName = "Yarn Skein Request";
            }
            else if (_oFabricSalesContract.OrderType == (int)EnumFabricRequestType.SampleFOC)
            {
                sHeaderName = "Sample Development Request";
            }
            else if (_oFabricSalesContract.OrderType == (int)EnumFabricRequestType.StockSale)
            {
                sHeaderName = "Stock Sale Order";
            }
            else
            {
                sHeaderName = "PRODUCTION REQUEST";
            }

            if (_oFabricSalesContract.IsPrint)
            {
                sHeaderName = sHeaderName + " (Print)";
            }

            _oPdfPCell = new PdfPCell(new Phrase(sHeaderName, FontFactory.GetFont("Tahoma", 15f, iTextSharp.text.Font.BOLD)));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE;

            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            if (_oFabricSalesContract.ApproveBy == 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Unauthorised Order", FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLDITALIC)));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.FixedHeight = 20; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.Colspan = _nTotalColumn;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            else if (_oFabricSalesContract.ApproveBy != 0 && _oFabricSalesContract.CurrentStatus == EnumFabricPOStatus.Cancel)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Cancel Order", FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLDITALIC)));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.FixedHeight = 20; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.Colspan = _nTotalColumn;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion
        }
        private void ReporttHeader_B()
        {
            string sHeaderName = "";
            #region Proforma Invoice Heading Print
            if (_oFabricSalesContract.IsPrint)
            {
                sHeaderName = sHeaderName + " (Print)";
            }

            _oPdfPCell = new PdfPCell(new Phrase(_oFabricOrderSetup.POPrintName, FontFactory.GetFont("Tahoma", 15f, iTextSharp.text.Font.BOLD)));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            if (_oFabricSalesContract.ApproveBy == 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase("( "+ _oFabricSalesContract.OrderTypeSt + " )", FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLDITALIC)));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
               _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.Colspan = _nTotalColumn;
                _oPdfPCell.Border = 0;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            else if (_oFabricSalesContract.ApproveBy != 0 && _oFabricSalesContract.CurrentStatus == EnumFabricPOStatus.Cancel)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Cancel Order" + " ) " + _oFabricSalesContract.OrderTypeSt + " )", FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLDITALIC)));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.MinimumHeight = 20; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.Colspan = _nTotalColumn;
                _oPdfPCell.Border = 0;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion
        }
        private void ReporttHeader_SC()
        {
            string sHeaderName = "";
            #region Proforma Invoice Heading Print

            sHeaderName = "Sales Contract";

            _oPdfPCell = new PdfPCell(new Phrase(sHeaderName, FontFactory.GetFont("Tahoma", 15f, iTextSharp.text.Font.BOLD)));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE;

            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            if (_oFabricSalesContract.ApproveBy == 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Unauthorised Order", FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLDITALIC)));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.FixedHeight = 20; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.Colspan = _nTotalColumn;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            else if (_oFabricSalesContract.ApproveBy != 0 && _oFabricSalesContract.CurrentStatus == EnumFabricPOStatus.Cancel)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Cancel Order", FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLDITALIC)));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.FixedHeight = 20; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.Colspan = _nTotalColumn;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion
        }
        #endregion


        public byte[] PrepareReport(FabricSalesContract oFabricSalesContract, Company oCompany, BusinessUnit oBusinessUnit, List<FabricDeliverySchedule> oFebricDeliverySchedules, List<FabricSalesContractNote> oFabricSalesContractNotes, List<FabricSCHistory> oFabricSCHistorys, bool bIsRnd, bool bIsLog)
        {
            _bIsLog = bIsLog;
            _oFabricSalesContract = oFabricSalesContract;
            _oFebricDeliverySchedules = oFebricDeliverySchedules;
            _oFabricSalesContractNotes = oFabricSalesContractNotes;
            _oBusinessUnit = oBusinessUnit;
            _bIsRnd = bIsRnd;
            _oFabricSalesContractDetails = oFabricSalesContract.FabricSalesContractDetails;
            _oCompany = oCompany;
            _oFabricSCHistorys = oFabricSCHistorys;
            _sMUnit = "Y";
            if (_oFabricSalesContractDetails.Count > 0)
            {
                _sMUnit = _oFabricSalesContractDetails[0].MUName;
              
            }

            #region Page Setup
            _oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
            _oDocument.SetMargins(30f, 30f, 30f, 30f);
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

            this.PrintHeader();
            this.ReporttHeader();
            this.PrintHead_Bulk(bIsLog);
            this.PrintWaterMark();
            if (_oFabricSalesContract.OrderType == (int)EnumFabricRequestType.StockSale || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Bulk || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Sample || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.SampleFOC || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Buffer)
            {
                this.Print_Body_BulkSample();
            }
            else
            {
                this.Print_Body();
            }
            if (_oFabricSalesContractNotes.Count > 0)
            {
                this.Print_Instruction();
            }
            if (_oFebricDeliverySchedules.Count > 0)
            {
                this.Print_DeliverySchedule();
            }
            this.PrintFooter();
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Body
        //private void PrintHead_Bulk(bool bIsLog)
        //{
        //    /////////////////////Water Mark
        //    _oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
        //    _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
        //    _oDocument.SetMargins(30f, 30f, 30f, 30f);
        //    _oPdfPTable.WidthPercentage = 100;
        //    _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

        //    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
        //    PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
        //    PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;

        //    if (bIsLog)
        //    {
        //        ESimSolWM_Footer.WMFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        //        ESimSolWM_Footer.WMFontSize = 100;
        //        ESimSolWM_Footer.WMRotation = 45;
        //        ESimSolWM_Footer PageEventHandler = new ESimSolWM_Footer();
        //        PageEventHandler.WaterMark = "HISTORY"; //Footer print with page event handler
        //        PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
        //    }
        //    else
        //    {
        //        ESimSolFooter PageEventHandler = new ESimSolFooter();
        //        PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
        //    }
        //    _oDocument.Open();
        //    PdfContentByte cb = PDFWriter.DirectContent;
        //    _oDocument.NewPage();
        //    ///////////////////////////////////

        //    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
        //    _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, 1);
        //    _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 8f, 4);


        //    PdfPTable oPdfPTable = new PdfPTable(5);
        //    PdfPCell oPdfPCell;
        //    oPdfPTable.SetWidths(new float[] { 50f, 180f, 150f, 80f, 146f });

        //    ///Col1
        //    oPdfPCell = new PdfPCell(new Phrase("DATE", _oFontStyle));
        //    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
        //    oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
        //    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.Border = 0;
        //    oPdfPTable.AddCell(oPdfPCell);
        //    ///Col2
        //    oPdfPCell = new PdfPCell(new Phrase(": " + _oFabricSalesContract.ApprovedDateSt + "(Approve Date)", _oFontStyleBold));
        //    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
        //    //oPdfPCell.Colspan = 3;
        //    oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
        //    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.Border = 0;
        //    oPdfPTable.AddCell(oPdfPCell);

        //    //if (_oFabricSalesContract.ConImage == null)
        //    //{
        //    //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
        //    //}
        //    //else
        //    //{
        //    //    _oImag = iTextSharp.text.Image.GetInstance(_oFabricSalesContract.ConImage, System.Drawing.Imaging.ImageFormat.Jpeg);
        //    //    _oImag.ScaleAbsolute(60f, 30f);
        //    //    _oPdfPCell = new PdfPCell(_oImag);
        //    //}
        //    ///Col3
        //    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
        //    oPdfPCell.Border = 0;
        //    oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.Border = 0;
        //    oPdfPTable.AddCell(oPdfPCell);

        //    ///Col 4+5
        //    oPdfPCell = new PdfPCell(new Phrase("Issue To: " + _oFabricSalesContract.ContractorName, _oFontStyleBold));
        //    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
        //    oPdfPCell.Colspan = 2;
        //    oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.Border = 0;
        //    oPdfPTable.AddCell(oPdfPCell);
        //    oPdfPTable.CompleteRow();

        //    ///Col 1
        //    if (_oFabricSalesContract.OrderType != (int)EnumFabricRequestType.StockSale)
        //    {
        //        oPdfPCell = new PdfPCell(new Phrase("PO No", _oFontStyle));
        //    }
        //    else
        //    {
        //        oPdfPCell = new PdfPCell(new Phrase("Sale Order No", _oFontStyle));
        //    }
        //    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
        //    oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
        //    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.Border = 0;
        //    oPdfPTable.AddCell(oPdfPCell);
        //    ///Col 2
        //    oPdfPCell = new PdfPCell(new Phrase(": " + _oFabricSalesContract.SCNoFull, _oFontStyleBold));
        //    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
        //    oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
        //    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.Border = 0;
        //    oPdfPTable.AddCell(oPdfPCell);

        //    ///Col 3
        //    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
        //    oPdfPCell.Border = 0;
        //    oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.Border = 0;
        //    oPdfPTable.AddCell(oPdfPCell);

        //    ///Col 4+5
        //    ///
        //    //if (_oFabricSalesContract.OrderType == (int)EnumFabricRequestType.StockSale || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Bulk || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Sample || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.SampleFOC)
        //    //{
        //    //    oPdfPCell = new PdfPCell(new Phrase(_oFabricSalesContract.BuyerName, FontFactory.GetFont("Tahoma", 11f, 1)));
        //    //}
        //    //else
        //    //{
        //    //    oPdfPCell = new PdfPCell(new Phrase(_oFabricSalesContract.ContractorName, FontFactory.GetFont("Tahoma", 11f, 1)));
        //    //}
        //    oPdfPCell = new PdfPCell(new Phrase("Buyer Name: " + _oFabricSalesContract.BuyerName, _oFontStyleBold));
        //    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
        //    oPdfPCell.Colspan = 2;
        //    //oPdfPCell.Rowspan = 3;
        //    oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.Border = 0;
        //    oPdfPTable.AddCell(oPdfPCell);
        //    oPdfPTable.CompleteRow();
        //    //
        //    if (_oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Bulk)
        //    {
        //        ///Col 1
        //        oPdfPCell = new PdfPCell(new Phrase("P/I No", _oFontStyle));
        //        oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
        //        oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //        oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
        //        oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.Border = 0;
        //        oPdfPTable.AddCell(oPdfPCell);

        //        ///Col 2
        //        oPdfPCell = new PdfPCell(new Phrase(": " + _oFabricSalesContract.PINo, _oFontStyleBold));
        //        oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
        //        oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //        oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
        //        oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.Border = 0;
        //        oPdfPTable.AddCell(oPdfPCell);
        //    }
        //    else
        //    {
        //        ///Col 1
        //        oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
        //        oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
        //        oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //        oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
        //        oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.Border = 0;
        //        oPdfPTable.AddCell(oPdfPCell);

        //        ///Col 2
        //        oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
        //        oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
        //        oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //        oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
        //        oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.Border = 0;
        //        oPdfPTable.AddCell(oPdfPCell);
        //    }

        //    ///Col 3
        //    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
        //    oPdfPCell.Border = 0;
        //    oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.Border = 0;
        //    oPdfPTable.AddCell(oPdfPCell);

        //    //Col 4+5
        //    if (_oFabricSalesContract.OrderType == (int)EnumFabricRequestType.StockSale || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Sample || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Bulk)
        //    {
        //        oPdfPCell = new PdfPCell(new Phrase("Local Buyer:"));
        //        oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
        //    }
        //    else
        //    {
        //        oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
        //        oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
        //    }

        //    oPdfPCell.Colspan = 2;
        //    oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.Border = 0;
        //    oPdfPTable.AddCell(oPdfPCell);
        //    oPdfPTable.CompleteRow();

        //    //if (_oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Bulk)
        //    //{
        //    //    ///Col 1
        //    //    oPdfPCell = new PdfPCell(new Phrase("L/C No", _oFontStyle));
        //    //    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0.5f;
        //    //    oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //    //    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
        //    //    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //    //    oPdfPTable.AddCell(oPdfPCell);
        //    //    ///Col 2
        //    //    oPdfPCell = new PdfPCell(new Phrase(": ", _oFontStyleBold));
        //    //    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0.5f;
        //    //    oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //    //    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
        //    //    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //    //    oPdfPTable.AddCell(oPdfPCell);
        //    //}
        //    //else
        //    //{
        //    ///Col 1
        //    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
        //    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
        //    oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
        //    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.Border = 0;
        //    oPdfPTable.AddCell(oPdfPCell);
        //    ///Col 2
        //    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
        //    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
        //    oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
        //    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.Border = 0;
        //    oPdfPTable.AddCell(oPdfPCell);


        //    ///Col 3
        //    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
        //    oPdfPCell.Border = 0;
        //    oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.Border = 0;
        //    oPdfPTable.AddCell(oPdfPCell);
        //    //Col 4+5
        //    if (_oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Sample || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Bulk || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.StockSale)
        //    {
        //        oPdfPCell = new PdfPCell(new Phrase(_oFabricSalesContract.ContractorName, FontFactory.GetFont("Tahoma", 11f, 1)));
        //    }
        //    else
        //    {
        //        oPdfPCell = new PdfPCell(new Phrase("", FontFactory.GetFont("Tahoma", 11f, 1)));
        //    }
        //    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
        //    oPdfPCell.Colspan = 2;
        //    oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.Border = 0;
        //    oPdfPTable.AddCell(oPdfPCell);
        //    oPdfPTable.CompleteRow();
        //    //Col 1
        //    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
        //    oPdfPCell.Border = 0;
        //    oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.Border = 0;
        //    oPdfPTable.AddCell(oPdfPCell);
        //    //Col 2
        //    oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyleBold));
        //    oPdfPCell.Border = 0;
        //    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
        //    //oPdfPCell.Colspan = 3;
        //    oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.Border = 0;
        //    oPdfPTable.AddCell(oPdfPCell);

        //    //Col 3
        //    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
        //    oPdfPCell.Border = 0;
        //    oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.Border = 0;
        //    oPdfPTable.AddCell(oPdfPCell);
        //    //Col 4+5
        //    if (_oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Sample || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Bulk || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.StockSale)
        //    {
        //        oPdfPCell = new PdfPCell(new Phrase(_oFabricSalesContract.Con_Address, _oFontStyle));
        //    }
        //    else
        //    {
        //        oPdfPCell = new PdfPCell(new Phrase("", FontFactory.GetFont("Tahoma", 11f, 1)));
        //    }
        //    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
        //    oPdfPCell.Colspan = 2;
        //    oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.Border = 0;
        //    oPdfPTable.AddCell(oPdfPCell);
        //    oPdfPTable.CompleteRow();





        //    _oPdfPCell = new PdfPCell(oPdfPTable);
        //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPCell.Border = 0;
        //    _oPdfPCell.Border = 0;
        //    _oPdfPTable.AddCell(_oPdfPCell);
        //    _oPdfPTable.CompleteRow();

        //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
        //    _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //    _oPdfPCell.Border = 0;
        //    _oPdfPTable.AddCell(_oPdfPCell);
        //    _oPdfPTable.CompleteRow();


        //}

        private void PrintHead_Bulk(bool bIsLog)
        {
            /////////////////////Water Mark
            _oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
            _oDocument.SetMargins(30f, 30f, 30f, 30f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;

            if (bIsLog)
            {
                ESimSolWM_Footer.WMFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                ESimSolWM_Footer.WMFontSize = 100;
                ESimSolWM_Footer.WMRotation = 45;
                ESimSolWM_Footer PageEventHandler = new ESimSolWM_Footer();
                PageEventHandler.WaterMark = "HISTORY"; //Footer print with page event handler
                PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            }
            else
            {
                ESimSolFooter PageEventHandler = new ESimSolFooter();
                PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            }
            _oDocument.Open();
            PdfContentByte cb = PDFWriter.DirectContent;
            _oDocument.NewPage();
            ///////////////////////////////////

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, 1);
            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 8f, 4);


            PdfPTable oPdfPTable = new PdfPTable(5);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 50f, 180f, 150f, 80f, 146f });

            ///Col1
            oPdfPCell = new PdfPCell(new Phrase("DATE", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPTable.AddCell(oPdfPCell);
            ///Col2
            oPdfPCell = new PdfPCell(new Phrase(": " + _oFabricSalesContract.ApprovedDateSt + "(Approve Date)", _oFontStyleBold));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            //oPdfPCell.Colspan = 3;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPTable.AddCell(oPdfPCell);

            //if (_oFabricSalesContract.ConImage == null)
            //{
            //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //}
            //else
            //{
            //    _oImag = iTextSharp.text.Image.GetInstance(_oFabricSalesContract.ConImage, System.Drawing.Imaging.ImageFormat.Jpeg);
            //    _oImag.ScaleAbsolute(60f, 30f);
            //    _oPdfPCell = new PdfPCell(_oImag);
            //}
            ///Col3
            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            ///Col 4+5
            oPdfPCell = new PdfPCell(new Phrase("End Buyer:"));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.Colspan = 2;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            ///Col 1
            if (_oFabricSalesContract.OrderType != (int)EnumFabricRequestType.StockSale)
            {
                oPdfPCell = new PdfPCell(new Phrase("PO No", _oFontStyle));
            }
            else
            {
                oPdfPCell = new PdfPCell(new Phrase("Sale Order No", _oFontStyle));
            }
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPTable.AddCell(oPdfPCell);
            ///Col 2
            oPdfPCell = new PdfPCell(new Phrase(": " + _oFabricSalesContract.SCNoFull, _oFontStyleBold));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPTable.AddCell(oPdfPCell);

            ///Col 3
            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            ///Col 4+5
            ///
            if (_oFabricSalesContract.OrderType == (int)EnumFabricRequestType.StockSale || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Bulk || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Sample || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.SampleFOC || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Buffer)
            {
                oPdfPCell = new PdfPCell(new Phrase(_oFabricSalesContract.BuyerName, FontFactory.GetFont("Tahoma", 11f, 1)));
            }
            else
            {
                oPdfPCell = new PdfPCell(new Phrase(_oFabricSalesContract.ContractorName, FontFactory.GetFont("Tahoma", 11f, 1)));
            }
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.Colspan = 2;
            //oPdfPCell.Rowspan = 3;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();
            //
            if (_oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Bulk)
            {
                ///Col 1
                oPdfPCell = new PdfPCell(new Phrase("P/I No", _oFontStyle));
                oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(oPdfPCell);

                ///Col 2
                oPdfPCell = new PdfPCell(new Phrase(": " + _oFabricSalesContract.PINo, _oFontStyleBold));
                oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(oPdfPCell);
            }
            else
            {
                ///Col 1
                oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(oPdfPCell);

                ///Col 2
                oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(oPdfPCell);
            }

            ///Col 3
            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            //Col 4+5
            if (_oFabricSalesContract.OrderType == (int)EnumFabricRequestType.StockSale || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Sample || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Bulk || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Buffer)
            {
                oPdfPCell = new PdfPCell(new Phrase("Local Buyer:"));
                oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            }
            else
            {
                oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            }

            oPdfPCell.Colspan = 2;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            //if (_oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Bulk)
            //{
            //    ///Col 1
            //    oPdfPCell = new PdfPCell(new Phrase("L/C No", _oFontStyle));
            //    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0.5f;
            //    oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            //    oPdfPTable.AddCell(oPdfPCell);
            //    ///Col 2
            //    oPdfPCell = new PdfPCell(new Phrase(": ", _oFontStyleBold));
            //    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0.5f;
            //    oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            //    oPdfPTable.AddCell(oPdfPCell);
            //}
            //else
            //{
            ///Col 1
            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPTable.AddCell(oPdfPCell);
            ///Col 2
            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPTable.AddCell(oPdfPCell);


            ///Col 3
            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            //Col 4+5
            if (_oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Buffer || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Sample || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Bulk || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.StockSale)
            {
                oPdfPCell = new PdfPCell(new Phrase(_oFabricSalesContract.ContractorName, FontFactory.GetFont("Tahoma", 11f, 1)));
            }
            else
            {
                oPdfPCell = new PdfPCell(new Phrase("", FontFactory.GetFont("Tahoma", 11f, 1)));
            }
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.Colspan = 2;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();
            //Col 1
            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            //Col 2
            oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyleBold));
            oPdfPCell.Border = 0;
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            //oPdfPCell.Colspan = 3;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            //Col 3
            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            //Col 4+5
            if (_oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Buffer || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Sample || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Bulk || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.StockSale || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Local_Bulk || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Local_Sample)
            {
                oPdfPCell = new PdfPCell(new Phrase(_oFabricSalesContract.Con_Address, _oFontStyle));
            }
            else
            {
                oPdfPCell = new PdfPCell(new Phrase("", FontFactory.GetFont("Tahoma", 11f, 1)));
            }
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.Colspan = 2;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();





            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


        }
        private void PrintHead_Bulk_B(bool bIsLog)
        {
            /////////////////////Water Mark
            _oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
            _oDocument.SetMargins(30f, 30f, 30f, 30f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;

            if (bIsLog)
            {
                ESimSolWM_Footer.WMFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                ESimSolWM_Footer.WMFontSize = 100;
                ESimSolWM_Footer.WMRotation = 45;
                ESimSolWM_Footer PageEventHandler = new ESimSolWM_Footer();
                PageEventHandler.WaterMark = "HISTORY"; //Footer print with page event handler
                PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            }
            else
            {
                ESimSolFooter PageEventHandler = new ESimSolFooter();
                PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            }
            _oDocument.Open();
            PdfContentByte cb = PDFWriter.DirectContent;
            _oDocument.NewPage();
            ///////////////////////////////////

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, 1);
            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 8f, 4);


            PdfPTable oPdfPTable = new PdfPTable(5);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 50f, 180f, 150f, 80f, 146f });

            ///Col1
            oPdfPCell = new PdfPCell(new Phrase("DATE", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPTable.AddCell(oPdfPCell);
            ///Col2
            if (_oFabricSalesContract.ApproveBy!=0)
            {
            oPdfPCell = new PdfPCell(new Phrase(": " + _oFabricSalesContract.ApprovedDateSt + "(Approve Date)", _oFontStyleBold));
            }
            else
            {
                oPdfPCell = new PdfPCell(new Phrase(": " + _oFabricSalesContract.SCDateSt + "( PO Date)", _oFontStyleBold));
            }
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            //oPdfPCell.Colspan = 3;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPTable.AddCell(oPdfPCell);

            //if (_oFabricSalesContract.ConImage == null)
            //{
            //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //}
            //else
            //{
            //    _oImag = iTextSharp.text.Image.GetInstance(_oFabricSalesContract.ConImage, System.Drawing.Imaging.ImageFormat.Jpeg);
            //    _oImag.ScaleAbsolute(60f, 30f);
            //    _oPdfPCell = new PdfPCell(_oImag);
            //}
            ///Col3
            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            ///Col 4+5
            oPdfPCell = new PdfPCell(new Phrase("End Buyer:"));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.Colspan = 2;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            ///Col 1
            if (_oFabricSalesContract.OrderType != (int)EnumFabricRequestType.StockSale)
            {
                oPdfPCell = new PdfPCell(new Phrase("PO No", _oFontStyle));
            }
            else
            {
                oPdfPCell = new PdfPCell(new Phrase("Sale Order No", _oFontStyle));
            }
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPTable.AddCell(oPdfPCell);
            ///Col 2
            oPdfPCell = new PdfPCell(new Phrase(": " + _oFabricSalesContract.SCNoFull, _oFontStyleBold));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPTable.AddCell(oPdfPCell);

            ///Col 3
            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            ///Col 4+5
            ///
            if (_oFabricSalesContract.OrderType == (int)EnumFabricRequestType.StockSale || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Bulk || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Sample || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.SampleFOC || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Buffer)
            {
                oPdfPCell = new PdfPCell(new Phrase(_oFabricSalesContract.BuyerName, FontFactory.GetFont("Tahoma", 11f, 1)));
            }
            else
            {
                oPdfPCell = new PdfPCell(new Phrase(_oFabricSalesContract.ContractorName, FontFactory.GetFont("Tahoma", 11f, 1)));
            }
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.Colspan = 2;
            //oPdfPCell.Rowspan = 3;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();
            //PI No
            if (_oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Bulk)
            {
                ///Col 1
                oPdfPCell = new PdfPCell(new Phrase("P/I No", _oFontStyle));
                oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(oPdfPCell);

                ///Col 2
                oPdfPCell = new PdfPCell(new Phrase(": " + _oFabricSalesContract.PINo, _oFontStyleBold));
                oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(oPdfPCell);
            }
            else
            {
                ///Col 1
                oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(oPdfPCell);

                ///Col 2
                oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(oPdfPCell);
            }

            ///Col 3
            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            //Col 4+5
            if (_oFabricSalesContract.OrderType == (int)EnumFabricRequestType.StockSale || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Sample || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Bulk || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Buffer)
            {
                oPdfPCell = new PdfPCell(new Phrase("Local Buyer:"));
                oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            }
            else
            {
                oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            }

            oPdfPCell.Colspan = 2;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();
             string sTemp = "";
            //LC No
            if (_oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Bulk)
            {
                ///Col 1
                oPdfPCell = new PdfPCell(new Phrase("L/C No", _oFontStyle));
                oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0.5f;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(oPdfPCell);

               
                if (_oExportPIs.Count > 0)
                {
                    sTemp = string.Join("+", _oExportPIs.Select(x => x.ExportLCNo).Distinct().ToList());
                }
                oPdfPCell = new PdfPCell(new Phrase(": " + sTemp, _oFontStyleBold));
                oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0.5f;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(oPdfPCell);



                ///Col 3
                oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                oPdfPCell.Border = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);
                //Col 4+5
                if (_oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Sample || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Bulk || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.StockSale || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Buffer)
                {
                    oPdfPCell = new PdfPCell(new Phrase(_oFabricSalesContract.ContractorName, FontFactory.GetFont("Tahoma", 11f, 1)));
                }
                else
                {
                    oPdfPCell = new PdfPCell(new Phrase("", FontFactory.GetFont("Tahoma", 11f, 1)));
                }
                oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                oPdfPCell.Colspan = 2;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            //if (_oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Bulk)
            //{
            //    ///Col 1
            //    oPdfPCell = new PdfPCell(new Phrase("L/C No", _oFontStyle));
            //    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0.5f;
            //    oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            //    oPdfPTable.AddCell(oPdfPCell);
            //    ///Col 2
            //    oPdfPCell = new PdfPCell(new Phrase(": ", _oFontStyleBold));
            //    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0.5f;
            //    oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            //    oPdfPTable.AddCell(oPdfPCell);
            //}
            //else
            //{
            ///Col 1
            else
            {
                oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(oPdfPCell);
                ///Col 2
                oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(oPdfPCell);


                ///Col 3
                oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                oPdfPCell.Border = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);
                //Col 4+5
                if (_oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Sample || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Bulk || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.StockSale || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Buffer)
                {
                    oPdfPCell = new PdfPCell(new Phrase(_oFabricSalesContract.ContractorName, FontFactory.GetFont("Tahoma", 11f, 1)));
                }
                else
                {
                    oPdfPCell = new PdfPCell(new Phrase("", FontFactory.GetFont("Tahoma", 11f, 1)));
                }
                oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                oPdfPCell.Colspan = 2;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            //Col 1
            oPdfPCell = new PdfPCell(new Phrase("Status", _oFontStyle));
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            //Col 2
            if (_oFabricSalesContract.CurrentStatus <= EnumFabricPOStatus.RequestForApprove)
            { oPdfPCell = new PdfPCell(new Phrase(": Waiting for Approval ", _oFontStyleBold)); }
            else { oPdfPCell = new PdfPCell(new Phrase(":"+_oFabricSalesContract.CurrentStatusSt, _oFontStyleBold)); }
            oPdfPCell.Border = 0;
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            //oPdfPCell.Colspan = 3;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            //Col 3
            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            //Col 4+5
            ContactPersonnel oContactPersonnel = new ContactPersonnel();
            if (_oFabricSalesContract.ContactPersonnelID > 0)
            {
                oContactPersonnel = oContactPersonnel.Get(_oFabricSalesContract.ContactPersonnelID, 0);
                sTemp = oContactPersonnel.Name;
            }
            if (_oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Sample || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Bulk || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.StockSale || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Buffer)
            {
                oPdfPCell = new PdfPCell(new Phrase(_oFabricSalesContract.Con_Address + (string.IsNullOrEmpty(sTemp)?"":"C.Person: " + sTemp), _oFontStyle));
            }
            else
            {

                if (!string.IsNullOrEmpty(sTemp))
                {

                    oPdfPCell = new PdfPCell(new Phrase("C.Person: " + sTemp, FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL)));
                }
                else
                {
                    oPdfPCell = new PdfPCell(new Phrase("", FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL)));
                }
            }
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.Colspan = 2;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();





            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


        }
        private void Print_Body_BulkSample()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 8f, 4);
            _nTotalQty = 0;
            string sTemp = "";
            PdfPTable oPdfPTable = new PdfPTable(10);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 30f, 170f, 90f, 80f, 100f, 75f, 90f, 80f, 60f, 150f });
            _nTotalQty = 0;
            _nCount = 0;
            //SL, DoG,MKT Ref,Approve Ref ,Buyer Ref/Style, Weave, Design & Pattern, Color, FType, Qty,Remarks

            if (_oFabricSalesContractDetails.Count > 0)
            {
                // _oFabricSalesContractDetails = _oFabricSalesContractDetails.OrderBy(o => o.ProductID).ToList();

                oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Description of Goods", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("MKT Ref", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                if (_oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Sample || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.SampleFOC || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Bulk || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.StockSale || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Buffer)
                {
                    oPdfPCell = new PdfPCell(new Phrase("Approve Ref", _oFontStyleBold));
                }
                else
                {
                    oPdfPCell = new PdfPCell(new Phrase("H/L Ref", _oFontStyleBold));
                }

                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Buyer Ref/Style", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Weave", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);




                oPdfPCell = new PdfPCell(new Phrase("Color", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Finish Type", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);


                //oPdfPCell = new PdfPCell(new Phrase("CW", _oFontStyleBold));
                //oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                //oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                //oPdfPTable.AddCell(oPdfPCell);
                if (_oFabricSalesContract.OrderType == (int)EnumFabricRequestType.StockSale || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Sample || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Bulk || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.SampleFOC || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Buffer)
                {
                    oPdfPCell = new PdfPCell(new Phrase(" Qty (" + _sMUnit + ")", _oFontStyleBold));
                }
                else
                {
                    oPdfPCell = new PdfPCell(new Phrase("Size", _oFontStyleBold));
                }
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Remarks", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();


                foreach (FabricSalesContractDetail oFabricSalesContractDetail in _oFabricSalesContractDetails)
                {
                    if (!_bIsRnd)
                    {
                        oFabricSalesContractDetail.ExeNo = "";
                    }

                    oPdfPTable = new PdfPTable(10);
                    oPdfPTable.SetWidths(new float[] { 30f, 170f, 90f, 80f, 100f, 75f, 90f, 80f, 60f, 150f });


                    if (oFabricSalesContractDetail.IsProduction || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.StockSale)
                    {
                        #region PrintDetail
                        _nCount++;

                        _oPdfPCell = new PdfPCell(new Phrase(_nCount.ToString(), _oFontStyle));
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPCell.MinimumHeight = 40f;
                        _oPdfPCell.Rowspan = 2;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        oPdfPTable.AddCell(_oPdfPCell);

                        //if (nProductID != oFabricSalesContractDetail.ProductID)
                        //{
                        sTemp = "";
                        if (!String.IsNullOrEmpty(oFabricSalesContractDetail.ProductName))
                        {
                            sTemp = sTemp + "" + oFabricSalesContractDetail.ProductName.Trim();
                        }
                        if (!String.IsNullOrEmpty(oFabricSalesContractDetail.ProductName))
                        {
                            sTemp = sTemp + "\n";
                        }
                        if (!String.IsNullOrEmpty(oFabricSalesContractDetail.ProcessTypeName))
                        {
                            sTemp = sTemp + "" + oFabricSalesContractDetail.ProcessTypeName.Trim();
                        }
                        if (!String.IsNullOrEmpty(oFabricSalesContractDetail.FabricDesignName))
                        {
                            sTemp = sTemp + ",";
                        }
                        if (!String.IsNullOrEmpty(oFabricSalesContractDetail.FabricDesignName))
                        {
                            sTemp = sTemp + "" + oFabricSalesContractDetail.FabricDesignName.Trim();
                        }
                        if (!String.IsNullOrEmpty(oFabricSalesContractDetail.FabricDesignName))
                        {
                            sTemp = sTemp + "\n";
                        }
                        if (!String.IsNullOrEmpty(oFabricSalesContractDetail.Construction))
                        {
                            sTemp = sTemp + "Const:" + oFabricSalesContractDetail.Construction.Trim();
                        }
                        if (!String.IsNullOrEmpty(oFabricSalesContractDetail.Construction))
                        {
                            sTemp = sTemp + "\n";
                        }
                        if (_oFabricSalesContract.OrderType != (int)EnumFabricRequestType.HandLoom)
                        {
                            if (!String.IsNullOrEmpty(oFabricSalesContractDetail.FabricWidth))
                            {
                                sTemp = sTemp + "CW:" + oFabricSalesContractDetail.FabricWidth.Trim();
                            }
                        }
                        // Col 2
                        //_nCount_P = _oFabricSalesContractDetails.Where(x => x.ProductID == oFabricSalesContractDetail.ProductID).Count();
                        _oPdfPCell = new PdfPCell(new Phrase(sTemp, _oFontStyle));
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _oPdfPCell.Rowspan = 2;
                        _oPdfPCell.Border = 0;
                        _oPdfPCell.BorderWidthLeft = 0.5f;
                        _oPdfPCell.BorderWidthTop = 0.5f;
                        _oPdfPCell.BorderWidthBottom = 0;
                        oPdfPTable.AddCell(_oPdfPCell);

                        //}

                        //else
                        //{
                        //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        //    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        //    _oPdfPCell.MinimumHeight = 40f;
                        //    _oPdfPCell.Border = 0;
                        //    _oPdfPCell.BorderWidthLeft = 0.5f;
                        //    _oPdfPCell.BorderWidthTop = 0;
                        //    _oPdfPCell.BorderWidthBottom = 0;
                        //    //_oPdfPCell.Rowspan = 17;
                        //    oPdfPTable.AddCell(_oPdfPCell);
                        //}
                        // Col 3
                        _oPdfPCell = new PdfPCell(new Phrase(oFabricSalesContractDetail.FabricNo, _oFontStyle));
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        if (string.IsNullOrEmpty(oFabricSalesContractDetail.ExeNo))
                        {
                            _oPdfPCell.Rowspan = 2;
                        }
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        oPdfPTable.AddCell(_oPdfPCell);
                        // Col 4
                        _oPdfPCell = new PdfPCell(new Phrase(oFabricSalesContractDetail.HLReference, _oFontStyle));
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        if (string.IsNullOrEmpty(oFabricSalesContractDetail.ExeNo))
                        {
                            _oPdfPCell.Rowspan = 2;
                        }
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        oPdfPTable.AddCell(_oPdfPCell);

                        // Col 5
                        sTemp = "";
                        if (!String.IsNullOrEmpty(oFabricSalesContractDetail.BuyerReference))
                        {
                            sTemp = sTemp + "" + oFabricSalesContractDetail.BuyerReference.Trim();
                        }
                        if (!String.IsNullOrEmpty(oFabricSalesContractDetail.BuyerReference))
                        {
                            sTemp = sTemp + ";\n";
                        }
                        if (!String.IsNullOrEmpty(oFabricSalesContractDetail.StyleNo))
                        {
                            sTemp = sTemp + "" + oFabricSalesContractDetail.StyleNo.Trim();
                        }
                        _oPdfPCell = new PdfPCell(new Phrase(sTemp, _oFontStyle));
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPCell.Rowspan = 2;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        oPdfPTable.AddCell(_oPdfPCell);

                        // Col 6
                        _oPdfPCell = new PdfPCell(new Phrase(oFabricSalesContractDetail.FabricWeaveName, _oFontStyle));
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPCell.Rowspan = 2;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        oPdfPTable.AddCell(_oPdfPCell);



                        // Col 7
                        _oPdfPCell = new PdfPCell(new Phrase(oFabricSalesContractDetail.ColorInfo, _oFontStyle));
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPCell.Rowspan = 2;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        oPdfPTable.AddCell(_oPdfPCell);


                        // Col 8
                        _oPdfPCell = new PdfPCell(new Phrase(oFabricSalesContractDetail.FinishTypeName, _oFontStyle));
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPCell.Rowspan = 2;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        oPdfPTable.AddCell(_oPdfPCell);

                        // Col 8
                        if (_oFabricSalesContract.OrderType == (int)EnumFabricRequestType.StockSale || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Sample || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Bulk || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.SampleFOC || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Buffer)
                        {
                            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(Math.Round(oFabricSalesContractDetail.Qty, 0)), _oFontStyle));
                        }
                        else
                        {
                            _oPdfPCell = new PdfPCell(new Phrase(oFabricSalesContractDetail.Size, _oFontStyle));
                        }
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPCell.Rowspan = 2;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        oPdfPTable.AddCell(_oPdfPCell);

                        // Col 10
                        if (oFabricSalesContractDetail.Status == EnumPOState.Hold)
                        {
                            sTemp = _oFabricSCHistorys.Where(m => m.FabricSCDetailID == oFabricSalesContractDetail.FabricSalesContractDetailID && m.FSCDStatusInt == (int)EnumPOState.Hold).First().Note;
                            oFabricSalesContractDetail.Note = sTemp + "\n" + oFabricSalesContractDetail.Note;
                        }
                        Paragraph oPdfParagraph;
                        if (oFabricSalesContractDetail.Note == null) { oFabricSalesContractDetail.Note = ""; }
                        if (oFabricSalesContractDetail.Note.Length > 50)
                        {
                            oPdfParagraph = new Paragraph(new Phrase(oFabricSalesContractDetail.Note, FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL)));
                        }
                        else
                        {
                            oPdfParagraph = new Paragraph(new Phrase(oFabricSalesContractDetail.Note, _oFontStyle));
                        }
                        oPdfParagraph.SetLeading(0f, 1f);
                        oPdfParagraph.Alignment = Element.ALIGN_LEFT;
                        _oPdfPCell = new PdfPCell();
                        _oPdfPCell.AddElement(oPdfParagraph);
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        //_oPdfPCell.MinimumHeight = 40f;
                        _oPdfPCell.Rowspan = 2;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        oPdfPTable.AddCell(_oPdfPCell);


                        _nTotalAmount = _nTotalAmount + (oFabricSalesContractDetail.Qty * oFabricSalesContractDetail.UnitPrice);
                        _nTotalQty = _nTotalQty + oFabricSalesContractDetail.Qty;

                        oPdfPTable.CompleteRow();
                        if (!string.IsNullOrEmpty(oFabricSalesContractDetail.ExeNo))
                        {
                            _oPdfPCell = new PdfPCell(new Phrase("Dispo No:" + oFabricSalesContractDetail.ExeNo, FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLDITALIC)));
                            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            //_oPdfPCell.MinimumHeight = 40f;
                            _oPdfPCell.Colspan = 2;
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            oPdfPTable.AddCell(_oPdfPCell);
                            oPdfPTable.CompleteRow();
                        }
                        #endregion



                        _oPdfPCell = new PdfPCell(oPdfPTable);
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();
                    }
                }
                if (_oFabricSalesContract.OrderType == (int)EnumFabricRequestType.SampleFOC || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Sample || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Bulk || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.StockSale)
                {
                    #region Total
                    oPdfPTable = new PdfPTable(10);
                    oPdfPTable.SetWidths(new float[] { 30f, 170f, 90f, 80f, 100f, 75f, 90f, 80f, 60f, 150f });
                    if (_oFabricSalesContractDetails.Count > 1)
                    {
                        _oPdfPCell = new PdfPCell(new Phrase("Total: ", _oFontStyleBold));
                        _oPdfPCell.Colspan = 8;
                        //_oPdfPCell.Border = 0;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(Math.Round(_nTotalQty, 0)), _oFontStyleBold));
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        //_oPdfPCell.Border = 0;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        //_oPdfPCell.Border = 0;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        oPdfPTable.AddCell(_oPdfPCell);
                        oPdfPTable.CompleteRow();

                        _oPdfPCell = new PdfPCell(oPdfPTable);
                        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();
                    }
                    #endregion
                }

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

            }
        }
        private void Print_Body()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 8f, 4);

            _nTotalQty = 0;

            PdfPTable oPdfPTable = new PdfPTable(9);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 28f, 155f, 75f, 80f, 60f, 100f, 95f, 60f, 100f });
            int nProductID = 0;
            _nTotalQty = 0;
            _nCount = 0;
            string sTemp = "";

            if (_oFabricSalesContractDetails.Count > 0)
            {
                // _oFabricSalesContractDetails = _oFabricSalesContractDetails.OrderBy(o => o.ProductID).ToList();

                oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Description of Goods", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("MKT Ref", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Buyer Ref\n/Style", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                //oPdfPCell = new PdfPCell(new Phrase("Size", _oFontStyleBold));
                //oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                //oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                //oPdfPTable.AddCell(oPdfPCell);


                //oPdfPCell = new PdfPCell(new Phrase("Style No", _oFontStyleBold));
                //oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                //oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                //oPdfPTable.AddCell(oPdfPCell);


                oPdfPCell = new PdfPCell(new Phrase("Weave", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);



                oPdfPCell = new PdfPCell(new Phrase("Design \n& Pattern", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Color", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Finish Type", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Remarks", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();


                foreach (FabricSalesContractDetail oFabricSalesContractDetail in _oFabricSalesContractDetails)
                {
                    oPdfPTable = new PdfPTable(9);
                    oPdfPTable.SetWidths(new float[] { 28f, 155f, 75f, 80f, 60f, 100f, 95f, 60f, 100f });


                    #region PrintDetail
                    _nCount++;

                    _oPdfPCell = new PdfPCell(new Phrase(_nCount.ToString(), _oFontStyle));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.MinimumHeight = 40f;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(_oPdfPCell);

                    //if (nProductID != oFabricSalesContractDetail.ProductID)

                    //{
                    sTemp = "";
                    if (!String.IsNullOrEmpty(oFabricSalesContractDetail.ProductName))
                    {
                        sTemp = sTemp + "" + oFabricSalesContractDetail.ProductName.Trim();
                    }
                    if (!String.IsNullOrEmpty(oFabricSalesContractDetail.ProductName))
                    {
                        sTemp = sTemp + "\n";
                    }
                    if (!String.IsNullOrEmpty(oFabricSalesContractDetail.ProcessTypeName))
                    {
                        sTemp = sTemp + "" + oFabricSalesContractDetail.ProcessTypeName.Trim();
                    }
                    if (!String.IsNullOrEmpty(oFabricSalesContractDetail.FabricDesignName))
                    {
                        sTemp = sTemp + ",";
                    }
                    if (!String.IsNullOrEmpty(oFabricSalesContractDetail.FabricDesignName))
                    {
                        sTemp = sTemp + "" + oFabricSalesContractDetail.FabricDesignName.Trim();
                    }
                    if (!String.IsNullOrEmpty(oFabricSalesContractDetail.FabricDesignName))
                    {
                        sTemp = sTemp + "\n";
                    }
                    if (!String.IsNullOrEmpty(oFabricSalesContractDetail.Construction))
                    {
                        sTemp = sTemp + "Const:" + oFabricSalesContractDetail.Construction.Trim();
                    }
                    if (!String.IsNullOrEmpty(oFabricSalesContractDetail.Construction))
                    {
                        sTemp = sTemp + "\n";
                    }
                    if (_oFabricSalesContract.OrderType != (int)EnumFabricRequestType.HandLoom)
                    {
                        if (!String.IsNullOrEmpty(oFabricSalesContractDetail.FabricWidth))
                        {
                            sTemp = sTemp + "CW:" + oFabricSalesContractDetail.FabricWidth.Trim();
                        }
                    }
                    if (!String.IsNullOrEmpty(oFabricSalesContractDetail.Weight))
                    {
                        sTemp = sTemp + "Weight:" + oFabricSalesContractDetail.Weight.Trim();
                    }
                    if (!String.IsNullOrEmpty(oFabricSalesContractDetail.Weight))
                    {
                        sTemp = sTemp + "\n";
                    }
                    if (!String.IsNullOrEmpty(oFabricSalesContractDetail.FabricWeaveName))
                    {
                        sTemp = sTemp + "Weave:" + oFabricSalesContractDetail.FabricWeaveName.Trim();
                    }
                    if (!String.IsNullOrEmpty(oFabricSalesContractDetail.FabricWeaveName))
                    {
                        sTemp = sTemp + "\n";
                    }
                    if (!String.IsNullOrEmpty(oFabricSalesContractDetail.Shrinkage))
                    {
                        sTemp = sTemp + "Shrinkage:" + oFabricSalesContractDetail.Shrinkage.Trim();
                    }
                    if (!String.IsNullOrEmpty(oFabricSalesContractDetail.Shrinkage))
                    {
                        sTemp = sTemp + "\n";
                    }
                    //_nCount_P = _oFabricSalesContractDetails.Where(x => x.ProductID == oFabricSalesContractDetail.ProductID).Count();
                    _oPdfPCell = new PdfPCell(new Phrase(sTemp, _oFontStyle));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.MinimumHeight = 40f;
                    //_oPdfPCell.Rowspan = 5;
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.BorderWidthLeft = 0.5f;
                    _oPdfPCell.BorderWidthTop = 0.5f;
                    _oPdfPCell.BorderWidthBottom = 0;
                    oPdfPTable.AddCell(_oPdfPCell);



                    _oPdfPCell = new PdfPCell(new Phrase(oFabricSalesContractDetail.FabricNo, _oFontStyle));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.MinimumHeight = 40f;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(_oPdfPCell);

                    sTemp = "";
                    if (!String.IsNullOrEmpty(oFabricSalesContractDetail.BuyerReference))
                    {
                        sTemp = sTemp + "" + oFabricSalesContractDetail.BuyerReference.Trim();
                    }
                    if (!String.IsNullOrEmpty(oFabricSalesContractDetail.BuyerReference))
                    {
                        sTemp = sTemp + ";\n";
                    }
                    if (!String.IsNullOrEmpty(oFabricSalesContractDetail.StyleNo))
                    {
                        sTemp = sTemp + "" + oFabricSalesContractDetail.StyleNo.Trim();
                    }

                    _oPdfPCell = new PdfPCell(new Phrase(sTemp, _oFontStyle));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.MinimumHeight = 40f;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(_oPdfPCell);




                    _oPdfPCell = new PdfPCell(new Phrase(oFabricSalesContractDetail.FabricWeaveName, _oFontStyle));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.MinimumHeight = 40f;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(_oPdfPCell);



                    _oPdfPCell = new PdfPCell(new Phrase(oFabricSalesContractDetail.DesignPattern, _oFontStyle));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.MinimumHeight = 40f;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(_oPdfPCell);


                    // Col 7
                    _oPdfPCell = new PdfPCell(new Phrase(oFabricSalesContractDetail.ColorInfo, _oFontStyle));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.MinimumHeight = 40f;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(_oPdfPCell);

                    // Col 8
                    _oPdfPCell = new PdfPCell(new Phrase(oFabricSalesContractDetail.FinishTypeName, _oFontStyle));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.MinimumHeight = 40f;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(_oPdfPCell);



                    // Col 9
                    if (oFabricSalesContractDetail.Status == EnumPOState.Hold)
                    {
                        oFabricSalesContractDetail.Note = _oFabricSCHistorys.Where(m => m.FabricSCDetailID == oFabricSalesContractDetail.FabricSalesContractDetailID && m.FSCDStatusInt == (int)EnumPOState.Hold).First().Note;
                        oFabricSalesContractDetail.Note = sTemp + "\n" + oFabricSalesContractDetail.Note;
                    }

                    Paragraph oPdfParagraph;
                    if (oFabricSalesContractDetail.Note == null) { oFabricSalesContractDetail.Note = ""; }
                    if (oFabricSalesContractDetail.Note.Length > 50)
                    {
                        oPdfParagraph = new Paragraph(new Phrase(oFabricSalesContractDetail.Note, FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL)));
                    }
                    else
                    {
                        oPdfParagraph = new Paragraph(new Phrase(oFabricSalesContractDetail.Note, _oFontStyle));
                    }
                    oPdfParagraph.SetLeading(0f, 1f);
                    oPdfParagraph.Alignment = Element.ALIGN_LEFT;
                    _oPdfPCell = new PdfPCell();
                    _oPdfPCell.AddElement(oPdfParagraph);
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.MinimumHeight = 40f;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(_oPdfPCell);


                    _nTotalAmount = _nTotalAmount + (oFabricSalesContractDetail.Qty * oFabricSalesContractDetail.UnitPrice);
                    _nTotalQty = _nTotalQty + oFabricSalesContractDetail.Qty;

                    oPdfPTable.CompleteRow();
                    #endregion

                    nProductID = oFabricSalesContractDetail.ProductID;

                    _oPdfPCell = new PdfPCell(oPdfPTable);
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                }

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

            }
        }
        private void Print_DeliverySchedule()
        {  var oFebricDeliverySchedules = _oFebricDeliverySchedules.Where(p => p.IsGrey == false).ToList();
        if (oFebricDeliverySchedules.Count > 0)
        {
            #region Total
            PdfPTable oPdfPTable = new PdfPTable(6);
            oPdfPTable.SetWidths(new float[] { 90f, 40f, 40f, 40f, 85f, 90f });

            _oPdfPCell = new PdfPCell(new Phrase("Fabric Delivery Detail ", _oFontStyleBold));
            //_oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 6;
            _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Delivery ", _oFontStyleBold));
            //_oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Date", _oFontStyleBold));
            //_oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);
            if (_oFabricSalesContract.OrderType == (int)EnumFabricRequestType.SampleFOC || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Sample || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Bulk || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.StockSale || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Buffer)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Qty(" + _sMUnit + ")", _oFontStyleBold));
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("Set(s)", _oFontStyleBold));
            }
            //_oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("%", _oFontStyleBold));
            //_oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("Note", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Colspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("Address", _oFontStyleBold));
            //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
            ////_oPdfPCell.Border = 0;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            double dTotalForDeliveryDetail = oFebricDeliverySchedules.Sum(x => x.Qty);

            foreach (var oItem in oFebricDeliverySchedules)
            {
                _oPdfPCell = new PdfPCell(new Phrase(oItem.Name, _oFontStyle));
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.DeliveryDateSt, _oFontStyle));
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(Math.Round(oItem.Qty, 0)), _oFontStyle));
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                if (_nTotalQty > 0)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(Math.Round((oItem.Qty * 100 / _nTotalQty), 0)), _oFontStyle));
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                }
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase(oItem.Note, _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();
                if (!oItem.IsFoc)
                {
                    _nTotalDSQty = _nTotalDSQty + oItem.Qty;
                }



            }

            _oPdfPCell = new PdfPCell(new Phrase("Total ", _oFontStyleBold));
            _oPdfPCell.Colspan = 2;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(_oPdfPCell);

            if (_oFabricSalesContract.OrderType == (int)EnumFabricRequestType.SampleFOC || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Sample || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Bulk || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.StockSale || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Buffer)
            {
                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(Math.Round(_nTotalDSQty, 0)) + " " + _sMUnit + "", _oFontStyleBold));
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase(dTotalForDeliveryDetail.ToString(), _oFontStyleBold));
            }


            //_oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            //_oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Colspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
            ////_oPdfPCell.Border = 0;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion
        }
        }
        private void Print_GreyReceiveSchedule()
        {
            var oFebricDeliverySchedules = _oFebricDeliverySchedules.Where(p => p.IsGrey == true).ToList();
            if (oFebricDeliverySchedules.Count > 0)
            {
                #region Total
                PdfPTable oPdfPTable = new PdfPTable(6);
                oPdfPTable.SetWidths(new float[] { 90f, 40f, 40f, 40f, 85f, 90f });

                _oPdfPCell = new PdfPCell(new Phrase("Grey Receive Detail", _oFontStyleBold));
                //_oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = 6;
                _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase("Delivery ", _oFontStyleBold));
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Date", _oFontStyleBold));
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(_oPdfPCell);
                if (_oFabricSalesContract.OrderType == (int)EnumFabricRequestType.SampleFOC || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Sample || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Bulk || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.StockSale || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Buffer)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("Qty(" + _sMUnit + ")", _oFontStyleBold));
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("Set(s)", _oFontStyleBold));
                }
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("%", _oFontStyleBold));
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase("Note", _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase("Address", _oFontStyleBold));
                //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
                ////_oPdfPCell.Border = 0;
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                //oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();
                double dTotalForDeliveryDetail = oFebricDeliverySchedules.Sum(x => x.Qty);

                foreach (var oItem in oFebricDeliverySchedules)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.Name, _oFontStyle));
                    //_oPdfPCell.Border = 0;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.DeliveryDateSt, _oFontStyle));
                    //_oPdfPCell.Border = 0;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(Math.Round(oItem.Qty, 0)), _oFontStyle));
                    //_oPdfPCell.Border = 0;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(_oPdfPCell);

                    if (_nTotalQty > 0)
                    {
                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(Math.Round((oItem.Qty * 100 / _nTotalQty), 0)), _oFontStyle));
                    }
                    else
                    {
                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    }
                    //_oPdfPCell.Border = 0;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oPdfPTable.AddCell(_oPdfPCell);


                    _oPdfPCell = new PdfPCell(new Phrase(oItem.Note, _oFontStyle));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.Colspan = 2;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(_oPdfPCell);

                    oPdfPTable.CompleteRow();
                    if (!oItem.IsFoc)
                    {
                        _nTotalDSQty = _nTotalDSQty + oItem.Qty;
                    }



                }

                _oPdfPCell = new PdfPCell(new Phrase("Total ", _oFontStyleBold));
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(_oPdfPCell);

                if (_oFabricSalesContract.OrderType == (int)EnumFabricRequestType.SampleFOC || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Sample || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Bulk || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.StockSale || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Buffer)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(Math.Round(_nTotalDSQty, 0)) + " " + _sMUnit + "", _oFontStyleBold));
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase(dTotalForDeliveryDetail.ToString(), _oFontStyleBold));
                }


                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
                ////_oPdfPCell.Border = 0;
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                //oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                #endregion
            }
        }
        private void Print_Instruction()
        {
            string sNotes = "";

            #region



            PdfPTable oPdfPTable = new PdfPTable(2);
            oPdfPTable.SetWidths(new float[] { 90f, 400f });

            _oPdfPCell = new PdfPCell(new Phrase("Special Instruction(s)", _oFontStyle));
            _oPdfPCell.Colspan = 2;
            _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            if (!String.IsNullOrEmpty(_oFabricSalesContract.EndUse))
            {
                _oPdfPCell = new PdfPCell(new Phrase("End Use", _oFontStyle));
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase(_oFabricSalesContract.EndUse, _oFontStyle));
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            if (!String.IsNullOrEmpty(_oFabricSalesContract.LightSourceName))
            {

                _oPdfPCell = new PdfPCell(new Phrase("Light Source:", _oFontStyle));
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPhrase = new Phrase();
                if (!String.IsNullOrEmpty(_oFabricSalesContract.LightSourceName) && !String.IsNullOrEmpty(_oFabricSalesContract.LightSourceNameTwo))
                {
                    _oPhrase.Add(new Chunk("Primary:", _oFontStyle));
                }
                _oPhrase.Add(new Chunk(_oFabricSalesContract.LightSourceName, _oFontStyleBold));
                if (!String.IsNullOrEmpty(_oFabricSalesContract.LightSourceNameTwo))
                {
                    _oPhrase.Add(new Chunk(" Secondary:", _oFontStyle));
                    _oPhrase.Add(new Chunk(_oFabricSalesContract.LightSourceNameTwo, _oFontStyleBold));
                }
                _oPdfPCell = new PdfPCell(_oPhrase);
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();

            }
            if (!String.IsNullOrEmpty(_oFabricSalesContract.QualityParameters))
            {
                _oPdfPCell = new PdfPCell(new Phrase("Quality Parameters", _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase(_oFabricSalesContract.QualityParameters, _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            if (!String.IsNullOrEmpty(_oFabricSalesContract.GarmentWash))
            {
                _oPdfPCell = new PdfPCell(new Phrase("Garments Wash Type", _oFontStyleBold));
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase(_oFabricSalesContract.GarmentWash, _oFontStyle));
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            if (!String.IsNullOrEmpty(_oFabricSalesContract.QtyTolarance))
            {
                _oPdfPCell = new PdfPCell(new Phrase("Quantity Tolerance", _oFontStyle));
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase(_oFabricSalesContract.QtyTolarance, _oFontStyle));
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            if (_oFabricSalesContractNotes.Count > 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Special Note(s)", _oFontStyle));
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(_oPdfPCell);
                _nCount = 0;
                foreach (FabricSalesContractNote oItem in _oFabricSalesContractNotes)
                {
                    _nCount++;
                    sNotes = sNotes + _nCount.ToString() + ". " + oItem.Note + "\n";
                }


                _oPdfPCell = new PdfPCell(new Phrase(sNotes, _oFontStyle));
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            if (_oFabricSalesContract.CurrentStatus == EnumFabricPOStatus.Cancel)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Cancel Reason:", _oFontStyle));
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(_oPdfPCell);
                _nCount = 0;
                _oFabricSalesContract.Note = _oFabricSCHistorys.Where(m => m.FabricSCID == _oFabricSalesContract.FabricSalesContractID && m.FSCStatus == EnumFabricPOStatus.Cancel).First().Note;

                _oPdfPCell = new PdfPCell(new Phrase(_oFabricSalesContract.Note, _oFontStyle));
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }


            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion
        }
        private void PrintFooter()
        {

            nUsagesHeight = CalculatePdfPTableHeight(_oPdfPTable);
            if (nUsagesHeight < 640)
            {
                nUsagesHeight = 640 - nUsagesHeight;
            }
            if (nUsagesHeight > 2)
            {
                #region Blank Row


                while (nUsagesHeight < 640)
                {
                    #region Table Initiate
                    PdfPTable oPdfPTableTemp = new PdfPTable(4);
                    oPdfPTableTemp.SetWidths(new float[] { 148f, 148f, 148f, 148f });

                    #endregion

                    _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.FixedHeight = 18f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableTemp.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.FixedHeight = 18f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableTemp.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.FixedHeight = 18f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableTemp.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.FixedHeight = 18f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableTemp.AddCell(_oPdfPCell);


                    oPdfPTableTemp.CompleteRow();

                    _oPdfPCell = new PdfPCell(oPdfPTableTemp);
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();

                    nUsagesHeight = CalculatePdfPTableHeight(_oPdfPTable);
                }

                #endregion
            }

            //_oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, 1);
            #region Footer
            PdfPTable oPdfPTable = new PdfPTable(7);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 8f, 23f, 8f, 22f, 8f, 23f, 8f });

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            oPdfPCell.Border = 0;
            oPdfPCell.FixedHeight = 4f;
            oPdfPCell.Colspan = oPdfPTable.NumberOfColumns;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();


            oPdfPCell = new PdfPCell(new Phrase("Account Holder", _oFontStyleBold));
            oPdfPCell.Border = 0;
            oPdfPCell.Colspan = oPdfPTable.NumberOfColumns;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase(_oFabricSalesContract.MKTGroupName, _oFontStyleBold));
            oPdfPCell.Border = 0;
            oPdfPCell.FixedHeight = 45f;
            oPdfPCell.Colspan = oPdfPTable.NumberOfColumns;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            #region Img
            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPTable.AddCell(oPdfPCell);

            if (_oFabricSalesContract.PreparedBySignature == null)
            {
                oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.Border = 0;  oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPTable.AddCell(oPdfPCell);
            }
            else
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oFabricSalesContract.PreparedBySignature, System.Drawing.Imaging.ImageFormat.Jpeg);
                oPdfPCell.Border = 0; _oImag.ScaleAbsolute(45f, 35f);
                oPdfPCell = new PdfPCell(_oImag); oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(oPdfPCell);
            }

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPTable.AddCell(oPdfPCell);

            if (_oFabricSalesContract.CheckBySignature == null)
            {
                oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPTable.AddCell(oPdfPCell);
            }
            else
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oFabricSalesContract.CheckBySignature, System.Drawing.Imaging.ImageFormat.Jpeg);
                oPdfPCell.Border = 0; _oImag.ScaleAbsolute(45f, 35f);
                oPdfPCell = new PdfPCell(_oImag); oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(oPdfPCell);
            }

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPTable.AddCell(oPdfPCell);

            if (_oFabricSalesContract.ApprovedBySignature == null)
            {
                oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPTable.AddCell(oPdfPCell);
            }
            else
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oFabricSalesContract.ApprovedBySignature, System.Drawing.Imaging.ImageFormat.Jpeg);
                oPdfPCell.Border = 0; _oImag.ScaleAbsolute(45f, 35f);
                oPdfPCell = new PdfPCell(_oImag); oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(oPdfPCell);
            }

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            #endregion

            #region Name & Designation
            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPTable.AddCell(oPdfPCell);

            Phrase oPhrase = new Phrase();
            Chunk oChunk2 = new Chunk(_oFabricSalesContract.PreapeByName, _oFontStyle);
            oPhrase.Add(oChunk2);
            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL);
            if(!string.IsNullOrEmpty(_oFabricSalesContract.PreapeByDesignation))
            {
                Chunk oChunk3 = new Chunk("\n" + _oFabricSalesContract.PreapeByDesignation, _oFontStyle);
                oPhrase.Add(oChunk3);
            }
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            //oPdfPCell = new PdfPCell(new Phrase(_oFabricSalesContract.PreapeByName + ((string.IsNullOrEmpty(_oFabricSalesContract.PreapeByDesignation)) ? "" : ("\n" + _oFabricSalesContract.PreapeByDesignation)), _oFontStyle));
            oPdfPCell = new PdfPCell(oPhrase);
            oPdfPCell.Border = 0; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPTable.AddCell(oPdfPCell);

            oPhrase = new Phrase();
            oChunk2 = new Chunk(_oFabricSalesContract.CheckedByName, _oFontStyle);
            oPhrase.Add(oChunk2);
            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL);
            if (!string.IsNullOrEmpty(_oFabricSalesContract.CheckByDesignation))
            {
                Chunk oChunk3 = new Chunk("\n" + _oFabricSalesContract.CheckByDesignation, _oFontStyle);
                oPhrase.Add(oChunk3);
            }
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            oPdfPCell = new PdfPCell(oPhrase);
            oPdfPCell.Border = 0; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPTable.AddCell(oPdfPCell);

            oPhrase = new Phrase();
            oChunk2 = new Chunk(_oFabricSalesContract.ApproveByName, _oFontStyle);
            oPhrase.Add(oChunk2);
            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL);
            if (!string.IsNullOrEmpty(_oFabricSalesContract.ApproveByDesignation))
            {
                Chunk oChunk3 = new Chunk("\n" + _oFabricSalesContract.ApproveByDesignation, _oFontStyle);
                oPhrase.Add(oChunk3);
            }
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            //oPdfPCell = new PdfPCell(new Phrase(_oFabricSalesContract.ApproveByName + ((string.IsNullOrEmpty(_oFabricSalesContract.ApproveByDesignation)) ? "" : ("\n" + _oFabricSalesContract.ApproveByDesignation)), _oFontStyle));
            oPdfPCell = new PdfPCell(oPhrase);
            oPdfPCell.Border = 0; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            #region Caption
            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Prepared By", _oFontStyleBold));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 1; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Checked By", _oFontStyleBold));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 1; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Authorised Signature", _oFontStyleBold));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 1; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            #endregion

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        private void PrintFooter_B()
        {

            nUsagesHeight = CalculatePdfPTableHeight(_oPdfPTable);
            if (nUsagesHeight < 640)
            {
                nUsagesHeight = 640 - nUsagesHeight;
            }
            if (nUsagesHeight > 2)
            {
                #region Blank Row


                while (nUsagesHeight < 640)
                {
                    #region Table Initiate
                    PdfPTable oPdfPTableTemp = new PdfPTable(4);
                    oPdfPTableTemp.SetWidths(new float[] { 148f, 148f, 148f, 148f });

                    #endregion

                    _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.FixedHeight = 18f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableTemp.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.FixedHeight = 18f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableTemp.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.FixedHeight = 18f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableTemp.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.FixedHeight = 18f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableTemp.AddCell(_oPdfPCell);


                    oPdfPTableTemp.CompleteRow();

                    _oPdfPCell = new PdfPCell(oPdfPTableTemp);
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();

                    nUsagesHeight = CalculatePdfPTableHeight(_oPdfPTable);
                }

                #endregion
            }

            #region Footer
            PdfPTable oPdfPTable = new PdfPTable(7);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 8f, 23f, 8f, 22f, 8f, 23f, 8f });

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            oPdfPCell.Border = 0;
            oPdfPCell.FixedHeight = 4f;
            oPdfPCell.Colspan = oPdfPTable.NumberOfColumns;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();


            oPdfPCell = new PdfPCell(new Phrase("Account Holder", _oFontStyleBold));
            oPdfPCell.Border = 0;
            oPdfPCell.Colspan = oPdfPTable.NumberOfColumns;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase(_oFabricSalesContract.MKTGroupName, _oFontStyleBold));
            oPdfPCell.Border = 0;
            oPdfPCell.FixedHeight = 45f;
            oPdfPCell.Colspan = oPdfPTable.NumberOfColumns;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            #region Img
            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPTable.AddCell(oPdfPCell);

            if (_oFabricSalesContract.PreparedBySignature == null)
            {
                oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPTable.AddCell(oPdfPCell);
            }
            else
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oFabricSalesContract.PreparedBySignature, System.Drawing.Imaging.ImageFormat.Jpeg);
                oPdfPCell.Border = 0; _oImag.ScaleAbsolute(45f, 35f);
                oPdfPCell = new PdfPCell(_oImag); oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(oPdfPCell);
            }

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPTable.AddCell(oPdfPCell);

            if (_oFabricSalesContract.CheckBySignature == null)
            {
                oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPTable.AddCell(oPdfPCell);
            }
            else
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oFabricSalesContract.CheckBySignature, System.Drawing.Imaging.ImageFormat.Jpeg);
                oPdfPCell.Border = 0; _oImag.ScaleAbsolute(45f, 35f);
                oPdfPCell = new PdfPCell(_oImag); oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(oPdfPCell);
            }

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPTable.AddCell(oPdfPCell);

            if (_oFabricSalesContract.ApprovedBySignature == null)
            {
                oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPTable.AddCell(oPdfPCell);
            }
            else
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oFabricSalesContract.ApprovedBySignature, System.Drawing.Imaging.ImageFormat.Jpeg);
                oPdfPCell.Border = 0; _oImag.ScaleAbsolute(45f, 35f);
                oPdfPCell = new PdfPCell(_oImag); oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(oPdfPCell);
            }

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            #endregion

            #region Name & Designation
            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPTable.AddCell(oPdfPCell);

            Phrase oPhrase = new Phrase();
            Chunk oChunk2 = new Chunk(_oFabricSalesContract.PreapeByName, _oFontStyle);
            oPhrase.Add(oChunk2);
            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL);
            if (!string.IsNullOrEmpty(_oFabricSalesContract.PreapeByDesignation))
            {
                Chunk oChunk3 = new Chunk("\n" + _oFabricSalesContract.PreapeByDesignation, _oFontStyle);
                oPhrase.Add(oChunk3);
            }
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            //oPdfPCell = new PdfPCell(new Phrase(_oFabricSalesContract.PreapeByName + ((string.IsNullOrEmpty(_oFabricSalesContract.PreapeByDesignation)) ? "" : ("\n" + _oFabricSalesContract.PreapeByDesignation)), _oFontStyle));
            oPdfPCell = new PdfPCell(oPhrase);
            oPdfPCell.Border = 0; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPTable.AddCell(oPdfPCell);

            oPhrase = new Phrase();
            oChunk2 = new Chunk(_oFabricSalesContract.CheckedByName, _oFontStyle);
            oPhrase.Add(oChunk2);
            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL);
            if (!string.IsNullOrEmpty(_oFabricSalesContract.CheckByDesignation))
            {
                Chunk oChunk3 = new Chunk("\n" + _oFabricSalesContract.CheckByDesignation, _oFontStyle);
                oPhrase.Add(oChunk3);
            }
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            oPdfPCell = new PdfPCell(oPhrase);
            oPdfPCell.Border = 0; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPTable.AddCell(oPdfPCell);

            oPhrase = new Phrase();
            oChunk2 = new Chunk(_oFabricSalesContract.ApproveByName, _oFontStyle);
            oPhrase.Add(oChunk2);
            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL);
            if (!string.IsNullOrEmpty(_oFabricSalesContract.ApproveByDesignation))
            {
                Chunk oChunk3 = new Chunk("\n" + _oFabricSalesContract.ApproveByDesignation, _oFontStyle);
                oPhrase.Add(oChunk3);
            }
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            //oPdfPCell = new PdfPCell(new Phrase(_oFabricSalesContract.ApproveByName + ((string.IsNullOrEmpty(_oFabricSalesContract.ApproveByDesignation)) ? "" : ("\n" + _oFabricSalesContract.ApproveByDesignation)), _oFontStyle));
            oPdfPCell = new PdfPCell(oPhrase);
            oPdfPCell.Border = 0; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            #region Caption
            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Prepared By", _oFontStyleBold));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 1; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Checked By", _oFontStyleBold));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 1; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Authorised Signature", _oFontStyleBold));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 1; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            #endregion

            #region Old Footer
            //PdfPTable oPdfPTable = new PdfPTable(3);
            //PdfPCell oPdfPCell;
            //oPdfPTable.SetWidths(new float[] { 197f, 197f, 197f });

            //oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            //oPdfPCell.Border = 0;
            //oPdfPCell.FixedHeight = 4f;
            //oPdfPCell.Colspan = 3;
            //oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //oPdfPTable.AddCell(oPdfPCell);




            //oPdfPTable.CompleteRow();

            //oPdfPCell = new PdfPCell(new Phrase("Account Holder", _oFontStyleBold));
            //oPdfPCell.Border = 0;
            //oPdfPCell.Colspan = 3;

            //oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //oPdfPTable.AddCell(oPdfPCell);


            //oPdfPTable.CompleteRow();
            //oPdfPCell = new PdfPCell(new Phrase(_oFabricSalesContract.MKTGroupName, _oFontStyleBold));
            //oPdfPCell.Border = 0;
            //oPdfPCell.FixedHeight = 35f;
            //oPdfPCell.Colspan = 3;
            //oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //oPdfPTable.AddCell(oPdfPCell);


            ////oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            ////oPdfPCell.Border = 0;
            ////oPdfPCell.FixedHeight = 35f;
            ////oPdfPCell.BackgroundColor = BaseColor.WHITE;
            ////oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            ////oPdfPTable.AddCell(oPdfPCell);


            ////oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            ////oPdfPCell.Border = 0;
            ////oPdfPCell.FixedHeight = 35f;
            ////oPdfPCell.BackgroundColor = BaseColor.WHITE;
            ////oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            ////oPdfPTable.AddCell(oPdfPCell);

            ////oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            ////oPdfPCell.Border = 0;
            ////oPdfPCell.FixedHeight = 35f;
            ////oPdfPCell.BackgroundColor = BaseColor.WHITE;
            ////oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            ////oPdfPTable.AddCell(oPdfPCell);

            //oPdfPTable.CompleteRow();

            //oPdfPCell = new PdfPCell(new Phrase(_oFabricSalesContract.PreapeByName, _oFontStyle_UnLine));
            ////oPdfPCell.FixedHeight = 30f;
            ////oPdfPCell.Colspan = 2;
            //oPdfPCell.Border = 0;
            //oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //oPdfPTable.AddCell(oPdfPCell);

            //oPdfPCell = new PdfPCell(new Phrase("________", _oFontStyle_UnLine));
            ////oPdfPCell.FixedHeight = 30f;
            ////oPdfPCell.Colspan = 2;
            //oPdfPCell.Border = 0;
            //oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //oPdfPTable.AddCell(oPdfPCell);


            //oPdfPCell = new PdfPCell(new Phrase(_oFabricSalesContract.ApproveByName, _oFontStyle_UnLine));
            ////oPdfPCell.FixedHeight = 30f;
            ////oPdfPCell.Colspan = 2;
            //oPdfPCell.Border = 0;
            //oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //oPdfPTable.AddCell(oPdfPCell);

            //oPdfPTable.CompleteRow();

            //oPdfPCell = new PdfPCell(new Phrase("Prepared By", _oFontStyleBold));
            ////oPdfPCell.FixedHeight = 5f;
            ////oPdfPCell.Colspan = 2;
            //oPdfPCell.Border = 0;
            //oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //oPdfPTable.AddCell(oPdfPCell);

            //oPdfPCell = new PdfPCell(new Phrase("Checked By", _oFontStyleBold));
            ////oPdfPCell.FixedHeight = 5f;
            ////oPdfPCell.Colspan = 2;
            //oPdfPCell.Border = 0;
            //oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //oPdfPTable.AddCell(oPdfPCell);



            //oPdfPCell = new PdfPCell(new Phrase("Authorised Signature", _oFontStyleBold));
            ////oPdfPCell.FixedHeight = 5f;
            ////oPdfPCell.Colspan = 2;
            //oPdfPCell.Border = 0;
            //oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //oPdfPTable.AddCell(oPdfPCell);

            //oPdfPTable.CompleteRow();
            #endregion


            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        #endregion

        #endregion
        #region Sales Contract
        public byte[] PrepareReport_SalesContract(FabricSalesContract oFabricSalesContract, Company oCompany, BusinessUnit oBusinessUnit, List<FabricDeliverySchedule> oFebricDeliverySchedules, List<FabricSalesContractNote> oFabricSalesContractNotes)
        {
            _oFabricSalesContract = oFabricSalesContract;
            _oFebricDeliverySchedules = oFebricDeliverySchedules;
            _oFabricSalesContractNotes = oFabricSalesContractNotes;
            _oBusinessUnit = oBusinessUnit;
            _oFabricSalesContractDetails = oFabricSalesContract.FabricSalesContractDetails;
            //_oFabricSCHistorys = oFabricSCHistorys;
            _oCompany = oCompany;
            if (_oFabricSalesContractDetails.Count > 0)
            {
                _sMUnit = _oFabricSalesContractDetails[0].MUName;
                _sMUnit = "Y";
            }

            #region Page Setup
            _oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
            _oDocument.SetMargins(30f, 30f, 30f, 30f);
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

            this.PrintHeader();
            this.ReporttHeader_SC();

            this.PrintHead_Bulk(false);
            if (_oFabricSalesContract.OrderType == (int)EnumFabricRequestType.StockSale || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Bulk || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Sample || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.SampleFOC || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Buffer)
            {
                this.Print_Body_SalesContract();
            }
            else
            {
                this.Print_Body();
            }

            if (_oFabricSalesContractNotes.Count > 0)
            {
                this.Print_Instruction();
            }
            if (_oFebricDeliverySchedules.Count > 0)
            {
                this.Print_DeliverySchedule();
            }
            this.PrintFooter();
            //_oPdfPTable.HeaderRows = 4;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        private void Print_Body_SalesContract()
        {
            _nTotalQty = 0;
            string sTemp = "";
            PdfPTable oPdfPTable = new PdfPTable(10);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 30f, 160f, 90f, 80f, 100f, 75f, 80f, 80f, 80f, 150f });
            int nProductID = 0;
            _nTotalQty = 0;
            _nCount = 0;
            //SL, DoG,MKT Ref,Approve Ref ,Buyer Ref/Style, Weave, Design & Pattern, Color, FType, Qty,Remarks

            if (_oFabricSalesContractDetails.Count > 0)
            {
                _oFabricSalesContractDetails = _oFabricSalesContractDetails.OrderBy(o => o.ProductID).ToList();

                oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Description of Goods", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("MKT Ref", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                //if (_oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Sample || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.SampleFOC || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Bulk)
                //{
                //    oPdfPCell = new PdfPCell(new Phrase("Approve Ref", _oFontStyleBold));
                //}
                //else
                //{
                //    oPdfPCell = new PdfPCell(new Phrase("H/L Ref", _oFontStyleBold));
                //}

                //oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                //oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                //oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Buyer Ref/Style", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Weave", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);




                oPdfPCell = new PdfPCell(new Phrase("Color", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                //oPdfPCell = new PdfPCell(new Phrase("Finish Type", _oFontStyleBold));
                //oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                //oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                //oPdfPTable.AddCell(oPdfPCell);



                if (_oFabricSalesContract.OrderType == (int)EnumFabricRequestType.StockSale || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Sample || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Bulk || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.SampleFOC || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Buffer)
                {
                    oPdfPCell = new PdfPCell(new Phrase(" Qty (" + _sMUnit + ")", _oFontStyleBold));
                }
                else
                {
                    oPdfPCell = new PdfPCell(new Phrase("Size", _oFontStyleBold));
                }
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);


                oPdfPCell = new PdfPCell(new Phrase("UP(" + _oFabricSalesContract.Currency + "/" + _sMUnit + ")", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Amount(" + _oFabricSalesContract.Currency + ")", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Remarks", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();


                foreach (FabricSalesContractDetail oFabricSalesContractDetail in _oFabricSalesContractDetails)
                {
                    oPdfPTable = new PdfPTable(10);
                    oPdfPTable.SetWidths(new float[] { 30f, 160f, 90f, 80f, 100f, 75f, 80f, 80f, 80f, 150f });


                    #region PrintDetail
                    _nCount++;

                    _oPdfPCell = new PdfPCell(new Phrase(_nCount.ToString(), _oFontStyle));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.MinimumHeight = 40f;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(_oPdfPCell);

                    //if (nProductID != oFabricSalesContractDetail.ProductID)
                    //{
                    sTemp = "";
                    if (!String.IsNullOrEmpty(oFabricSalesContractDetail.ProductName))
                    {
                        sTemp = sTemp + "" + oFabricSalesContractDetail.ProductName.Trim();
                    }
                    if (!String.IsNullOrEmpty(oFabricSalesContractDetail.ProductName))
                    {
                        sTemp = sTemp + "\n";
                    }
                    if (!String.IsNullOrEmpty(oFabricSalesContractDetail.ProcessTypeName))
                    {
                        sTemp = sTemp + "" + oFabricSalesContractDetail.ProcessTypeName.Trim();
                    }
                    if (!String.IsNullOrEmpty(oFabricSalesContractDetail.FabricDesignName))
                    {
                        sTemp = sTemp + ",";
                    }
                    if (!String.IsNullOrEmpty(oFabricSalesContractDetail.FabricDesignName))
                    {
                        sTemp = sTemp + "" + oFabricSalesContractDetail.FabricDesignName.Trim();
                    }
                    if (!String.IsNullOrEmpty(oFabricSalesContractDetail.FabricDesignName))
                    {
                        sTemp = sTemp + "\n";
                    }
                    if (!String.IsNullOrEmpty(oFabricSalesContractDetail.Construction))
                    {
                        sTemp = sTemp + "Const:" + oFabricSalesContractDetail.Construction.Trim();
                    }
                    if (!String.IsNullOrEmpty(oFabricSalesContractDetail.Construction))
                    {
                        sTemp = sTemp + "\n";
                    }
                    if (_oFabricSalesContract.OrderType != (int)EnumFabricRequestType.HandLoom)
                    {
                        if (!String.IsNullOrEmpty(oFabricSalesContractDetail.FabricWidth))
                        {
                            sTemp = sTemp + "CW:" + oFabricSalesContractDetail.FabricWidth.Trim();
                        }
                    }
                    if (!String.IsNullOrEmpty(oFabricSalesContractDetail.Weight))
                    {
                        sTemp = sTemp + "Weight:" + oFabricSalesContractDetail.Weight.Trim();
                    }
                    if (!String.IsNullOrEmpty(oFabricSalesContractDetail.Weight))
                    {
                        sTemp = sTemp + "\n";
                    }
                    if (!String.IsNullOrEmpty(oFabricSalesContractDetail.FabricWeaveName))
                    {
                        sTemp = sTemp + "Weave:" + oFabricSalesContractDetail.FabricWeaveName.Trim();
                    }
                    if (!String.IsNullOrEmpty(oFabricSalesContractDetail.FabricWeaveName))
                    {
                        sTemp = sTemp + "\n";
                    }
                    if (!String.IsNullOrEmpty(oFabricSalesContractDetail.Shrinkage))
                    {
                        sTemp = sTemp + "Shrinkage:" + oFabricSalesContractDetail.Shrinkage.Trim();
                    }
                    if (!String.IsNullOrEmpty(oFabricSalesContractDetail.Shrinkage))
                    {
                        sTemp = sTemp + "\n";
                    }

                    // Col 2
                    //_nCount_P = _oFabricSalesContractDetails.Where(x => x.ProductID == oFabricSalesContractDetail.ProductID).Count();
                    _oPdfPCell = new PdfPCell(new Phrase(sTemp, _oFontStyle));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.MinimumHeight = 40f;
                    //_oPdfPCell.Rowspan = 5;
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.BorderWidthLeft = 0.5f;
                    _oPdfPCell.BorderWidthTop = 0.5f;
                    _oPdfPCell.BorderWidthBottom = 0;
                    oPdfPTable.AddCell(_oPdfPCell);

                    //}

                    //else
                    //{
                    //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    //    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //    _oPdfPCell.MinimumHeight = 40f;
                    //    _oPdfPCell.Border = 0;
                    //    _oPdfPCell.BorderWidthLeft = 0.5f;
                    //    _oPdfPCell.BorderWidthTop = 0;
                    //    _oPdfPCell.BorderWidthBottom = 0;
                    //    //_oPdfPCell.Rowspan = 17;
                    //    oPdfPTable.AddCell(_oPdfPCell);
                    //}
                    // Col 3
                    _oPdfPCell = new PdfPCell(new Phrase(oFabricSalesContractDetail.FabricNo, _oFontStyle));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.MinimumHeight = 40f;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(_oPdfPCell);
                    // Col 4
                    //_oPdfPCell = new PdfPCell(new Phrase(oFabricSalesContractDetail.HLReference, _oFontStyle));
                    //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //_oPdfPCell.MinimumHeight = 40f;
                    //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //oPdfPTable.AddCell(_oPdfPCell);

                    // Col 5
                    sTemp = "";
                    if (!String.IsNullOrEmpty(oFabricSalesContractDetail.BuyerReference))
                    {
                        sTemp = sTemp + "" + oFabricSalesContractDetail.BuyerReference.Trim();
                    }
                    if (!String.IsNullOrEmpty(oFabricSalesContractDetail.BuyerReference))
                    {
                        sTemp = sTemp + ";\n";
                    }
                    if (!String.IsNullOrEmpty(oFabricSalesContractDetail.StyleNo))
                    {
                        sTemp = sTemp + "" + oFabricSalesContractDetail.StyleNo.Trim();
                    }
                    _oPdfPCell = new PdfPCell(new Phrase(sTemp, _oFontStyle));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.MinimumHeight = 40f;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(_oPdfPCell);

                    // Col 6
                    _oPdfPCell = new PdfPCell(new Phrase(oFabricSalesContractDetail.FabricWeaveName, _oFontStyle));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.MinimumHeight = 40f;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(_oPdfPCell);



                    // Col 7
                    _oPdfPCell = new PdfPCell(new Phrase(oFabricSalesContractDetail.ColorInfo, _oFontStyle));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.MinimumHeight = 40f;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(_oPdfPCell);


                    // Col 8
                    //_oPdfPCell = new PdfPCell(new Phrase(oFabricSalesContractDetail.FinishTypeName, _oFontStyle));
                    //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //_oPdfPCell.MinimumHeight = 40f;
                    //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //oPdfPTable.AddCell(_oPdfPCell);

                    // Col 8
                    if (_oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Sample || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Bulk || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.SampleFOC || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.StockSale || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Buffer)
                    {
                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(Math.Round(oFabricSalesContractDetail.Qty, 0)), _oFontStyle));
                    }
                    else
                    {
                        _oPdfPCell = new PdfPCell(new Phrase(oFabricSalesContractDetail.Size, _oFontStyle));
                    }
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.MinimumHeight = 40f;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(_oPdfPCell);

                    // Col 8
                    if (_oFabricSalesContract.OrderType == (int)EnumFabricRequestType.StockSale || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Sample || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Bulk || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.SampleFOC || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Buffer)
                    {
                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(Math.Round(oFabricSalesContractDetail.UnitPrice, 2)), _oFontStyle));
                    }
                    else
                    {
                        _oPdfPCell = new PdfPCell(new Phrase(oFabricSalesContractDetail.Size, _oFontStyle));
                    }
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.MinimumHeight = 40f;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(_oPdfPCell);

                    if (_oFabricSalesContract.OrderType == (int)EnumFabricRequestType.StockSale || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Sample || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Bulk || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.SampleFOC || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Buffer)
                    {
                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(Math.Round((oFabricSalesContractDetail.Qty * oFabricSalesContractDetail.UnitPrice), 2)), _oFontStyle));
                    }
                    else
                    {
                        _oPdfPCell = new PdfPCell(new Phrase(oFabricSalesContractDetail.Size, _oFontStyle));
                    }
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.MinimumHeight = 40f;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(_oPdfPCell);

                    // Col 10
                    if (oFabricSalesContractDetail.Status == EnumPOState.Hold)
                    {
                        oFabricSalesContractDetail.Note = _oFabricSCHistorys.Where(m => m.FabricSCDetailID == oFabricSalesContractDetail.FabricSalesContractDetailID && m.FSCDStatusInt == (int)EnumPOState.Hold).First().Note;
                        oFabricSalesContractDetail.Note = sTemp + "\n" + oFabricSalesContractDetail.Note;
                    }

                    Paragraph oPdfParagraph;
                    if (oFabricSalesContractDetail.Note == null) { oFabricSalesContractDetail.Note = ""; }
                    if (oFabricSalesContractDetail.Note.Length > 50)
                    {
                        oPdfParagraph = new Paragraph(new Phrase(oFabricSalesContractDetail.Note, FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL)));
                    }
                    else
                    {
                        oPdfParagraph = new Paragraph(new Phrase(oFabricSalesContractDetail.Note, _oFontStyle));
                    }
                    oPdfParagraph.SetLeading(0f, 1f);
                    oPdfParagraph.Alignment = Element.ALIGN_LEFT;
                    _oPdfPCell = new PdfPCell();
                    _oPdfPCell.AddElement(oPdfParagraph);
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.MinimumHeight = 40f;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(_oPdfPCell);


                    _nTotalAmount = _nTotalAmount + (oFabricSalesContractDetail.Qty * oFabricSalesContractDetail.UnitPrice);
                    _nTotalQty = _nTotalQty + oFabricSalesContractDetail.Qty;

                    oPdfPTable.CompleteRow();
                    #endregion

                    nProductID = oFabricSalesContractDetail.ProductID;

                    _oPdfPCell = new PdfPCell(oPdfPTable);
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                }
                if (_oFabricSalesContract.OrderType == (int)EnumFabricRequestType.StockSale || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.SampleFOC || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Sample || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Bulk || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.StockSale || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Buffer)
                {
                    #region Total
                    oPdfPTable = new PdfPTable(10);
                    oPdfPTable.SetWidths(new float[] { 30f, 160f, 90f, 80f, 100f, 75f, 80f, 80f, 80f, 150f });
                    if (_oFabricSalesContractDetails.Count > 1)
                    {
                        _oPdfPCell = new PdfPCell(new Phrase("Total: ", _oFontStyleBold));
                        _oPdfPCell.Colspan = 6;
                        //_oPdfPCell.Border = 0;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(Math.Round(_nTotalQty, 2)), _oFontStyleBold));
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        //_oPdfPCell.Border = 0;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        //_oPdfPCell.Border = 0;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(Math.Round(_nTotalAmount, 2)), _oFontStyleBold));
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        //_oPdfPCell.Border = 0;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        //_oPdfPCell.Border = 0;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        oPdfPTable.AddCell(_oPdfPCell);

                        oPdfPTable.CompleteRow();

                        _oPdfPCell = new PdfPCell(oPdfPTable);
                        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();
                    }
                    #endregion
                }

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

            }
        }
        #endregion
        private void Blank(int nFixedHeight)
        {
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Colspan = 1; _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = nFixedHeight; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        #region Print Two
        public byte[] PrepareReport_B(FabricSalesContract oFabricSalesContract, Company oCompany, BusinessUnit oBusinessUnit, List<FabricDeliverySchedule> oFebricDeliverySchedules, List<FabricSalesContractNote> oFabricSalesContractNotes, List<FabricSCHistory> oFabricSCHistorys, FabricOrderSetup oFabricOrderSetup, bool bIsRnd, bool bIsLog, List<ExportPI> oExportPIs)
        {
            _bIsLog = bIsLog;
            _oFabricSalesContract = oFabricSalesContract;
            _oFebricDeliverySchedules = oFebricDeliverySchedules;
            _oFabricSalesContractNotes = oFabricSalesContractNotes;
            _oFabricSCHistorys = oFabricSCHistorys;
            _oBusinessUnit = oBusinessUnit;
            _oFabricSalesContractDetails = oFabricSalesContract.FabricSalesContractDetails;
            _oCompany = oCompany;
            _bIsRnd = bIsRnd;
            _oFabricOrderSetup = oFabricOrderSetup;
            _sMUnit = "Y";
            if (_oFabricSalesContractDetails.Count > 0)
            {
                _sMUnit = _oFabricSalesContractDetails[0].MUName;
               // _sMUnit = "Y";
            }
            _oExportPIs = oExportPIs;
            #region Page Setup
            _oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
            _oDocument.SetMargins(30f, 30f, 30f, 30f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            //ESimSolFooter PageEventHandler = new ESimSolFooter();

            //ESimSolWM_Footer.WMFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            ////ESimSolWM_Footer.WMFont.c
            //ESimSolWM_Footer.WMFontSize = 48;
            //ESimSolWM_Footer.WMRotation = 280;
            //ESimSolWM_Footer PageEventHandler = new ESimSolWM_Footer();
            //PageEventHandler.Title = "AKRAM 001"; //Footer print with page event handler
            //PageEventHandler.WaterMark = "AKRAM 001"; //Footer print with page event handler
            //PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler

            //ESimSolWM_Footer PageEventHandler = new ESimSolWM_Footer();
            //PageEventHandler.Title = "AKRAM";
            //PageEventHandler.WaterMark = "AKRAM";
            //PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();

            _oPdfPTable.SetWidths(new float[] { 595f });
            #endregion

            this.PrintHeader();
            this.ReporttHeader_B();
            this.PrintWaterMark();
            this.PrintHead_Bulk_B(bIsLog);
            //if (_oFabricSalesContract.OrderType == (int)EnumFabricRequestType.StockSale || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Bulk || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Sample || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.SampleFOC)
            //{
            //    this.Print_Body_BulkSample_B();
            //}
            //else
            //{
            //    this.Print_Body_B();
            //}
            this.PrintBodyProduct_WUOne();

            //this.Print_Instruction();
            //this.Print_DeliverySchedule();
            if (_oFabricSalesContractNotes.Count > 0)
            {
                this.Print_Instruction();
            }
            if (_oFebricDeliverySchedules.Count > 0)
            {
                this.Print_DeliverySchedule();
                this.Print_GreyReceiveSchedule();
            }
            this.PrintFooter_B();
            //_oPdfPTable.HeaderRows = 4;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        private void Print_Body_BulkSample_B()
        {
            _nTotalQty = 0;
            string sTemp = "";
            PdfPTable oPdfPTable = new PdfPTable(10);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 30f, 170f, 90f, 80f, 100f, 75f, 90f, 80f, 60f, 150f });

            _nTotalQty = 0;
            _nCount = 0;
            string sConstruction = "";
            string sConst = "";
            int nProductID = 0;
            string sProcessTypeName = "";
            string sFabricWeaveName = "";
            string sFabricWidth = "";
            string sFinishTypeName = "";
            int _nCount_Raw = 0;
            int _nCount_Raw_Style = 0;
            //SL, DoG,MKT Ref,Approve Ref ,Buyer Ref/Style, Weave, Design & Pattern, Color, FType, Qty,Remarks

            if (_oFabricSalesContractDetails.Count > 0)
            {
                // _oFabricSalesContractDetails = _oFabricSalesContractDetails.OrderBy(o => o.ProductID).ToList();

                oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Description of Goods", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("MKT Ref", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                if (_oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Sample || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.SampleFOC || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Bulk || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.StockSale || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Buffer)
                {
                    oPdfPCell = new PdfPCell(new Phrase("Approve Ref", _oFontStyleBold));
                }
                else
                {
                    oPdfPCell = new PdfPCell(new Phrase("H/L Ref", _oFontStyleBold));
                }

                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Buyer Ref/Style", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Lab Dip", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);


                oPdfPCell = new PdfPCell(new Phrase("Color", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Finish Type", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);


                //oPdfPCell = new PdfPCell(new Phrase("CW", _oFontStyleBold));
                //oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                //oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                //oPdfPTable.AddCell(oPdfPCell);
                if (_oFabricSalesContract.OrderType == (int)EnumFabricRequestType.StockSale || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Sample || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Bulk || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.SampleFOC || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Buffer)
                {
                    oPdfPCell = new PdfPCell(new Phrase(" Qty (" + _sMUnit + ")", _oFontStyleBold));
                }
                else
                {
                    oPdfPCell = new PdfPCell(new Phrase("Size", _oFontStyleBold));
                }
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Remarks", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                //_oFabricSalesContractDetails = _oFabricSalesContractDetails.OrderBy(o => o.ProductID).ThenBy(a => a.Construction).ThenBy(a => a.ProcessTypeName).ThenBy(a => a.FabricWeaveName).ThenBy(a => a.FabricWidth).ThenBy(a => a.FinishTypeName).ThenBy(b => b.StyleNo).ThenBy(c => c.ColorInfo).ToList();

                List<FabricSalesContractDetail> oFabricSalesContractDetails = new List<FabricSalesContractDetail>();

                oFabricSalesContractDetails = _oFabricSalesContractDetails.GroupBy(x => new { x.ProductID, x.ProductName, x.Construction, x.FabricID, x.FabricNo, x.ProcessTypeName, x.FabricWeaveName, x.FabricWidth, x.FinishTypeName, x.MUnitID, x.Weight, x.Shrinkage }, (key, grp) =>
                                                new FabricSalesContractDetail
                                                {
                                                    ProductID = key.ProductID,
                                                    ProductName = key.ProductName,
                                                    FabricNo = key.FabricNo,
                                                    FabricID = key.FabricID,
                                                    Construction = key.Construction,
                                                    ProcessTypeName = key.ProcessTypeName,
                                                    FabricWeaveName = key.FabricWeaveName,
                                                    FabricWidth = key.FabricWidth,
                                                    FinishTypeName = key.FinishTypeName,
                                                    Qty = grp.Sum(p => p.Qty),
                                                    MUnitID = key.MUnitID,
                                                    Weight = key.Weight,
                                                    Shrinkage = key.Shrinkage
                                                }).ToList();

                foreach (FabricSalesContractDetail oFabricSalesContractDetail in oFabricSalesContractDetails)
                {
                    oPdfPTable = new PdfPTable(10);
                    oPdfPTable.SetWidths(new float[] { 30f, 170f, 90f, 80f, 100f, 75f, 90f, 80f, 60f, 150f });

                    if (oFabricSalesContractDetail.IsProduction || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.StockSale)
                    {
                        #region PrintDetail
                        _nCount++;

                        _oPdfPCell = new PdfPCell(new Phrase(_nCount.ToString(), _oFontStyle));
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        oPdfPTable.AddCell(_oPdfPCell);


                        sTemp = "";
                        if (!String.IsNullOrEmpty(oFabricSalesContractDetail.ProductName))
                        {
                            sTemp = sTemp + "" + oFabricSalesContractDetail.ProductName.Trim();
                        }
                        if (!String.IsNullOrEmpty(oFabricSalesContractDetail.ProductName))
                        {
                            sTemp = sTemp + "\n";
                        }
                        if (!String.IsNullOrEmpty(oFabricSalesContractDetail.ProcessTypeName))
                        {
                            sTemp = sTemp + "" + oFabricSalesContractDetail.ProcessTypeName.Trim();
                        }
                        if (!String.IsNullOrEmpty(oFabricSalesContractDetail.FabricDesignName))
                        {
                            sTemp = sTemp + ",";
                        }
                        if (!String.IsNullOrEmpty(oFabricSalesContractDetail.FabricDesignName))
                        {
                            sTemp = sTemp + "" + oFabricSalesContractDetail.FabricDesignName.Trim();
                        }
                        if (!String.IsNullOrEmpty(oFabricSalesContractDetail.FabricDesignName))
                        {
                            sTemp = sTemp + "\n";
                        }
                        if (!String.IsNullOrEmpty(oFabricSalesContractDetail.Construction))
                        {
                            sTemp = sTemp + "Const:" + oFabricSalesContractDetail.Construction.Trim();
                        }
                        if (!String.IsNullOrEmpty(oFabricSalesContractDetail.Construction))
                        {
                            sTemp = sTemp + "\n";
                        }
                        if (_oFabricSalesContract.OrderType != (int)EnumFabricRequestType.HandLoom)
                        {
                            if (!String.IsNullOrEmpty(oFabricSalesContractDetail.FabricWidth))
                            {
                                sTemp = sTemp + "CW:" + oFabricSalesContractDetail.FabricWidth.Trim();
                            }
                        }
                        if (!String.IsNullOrEmpty(oFabricSalesContractDetail.FabricWeaveName))
                        {
                            sTemp = sTemp + "\n";
                        }
                        if (!String.IsNullOrEmpty(oFabricSalesContractDetail.FabricWeaveName))
                        {
                            sTemp = sTemp + "Weave:" + oFabricSalesContractDetail.FabricWeaveName.Trim();
                        }
                        if (_oFabricSalesContractDetails.FirstOrDefault() != null && _oFabricSalesContractDetails.FirstOrDefault().FabricSalesContractDetailID > 0 && _oFabricSalesContractDetails.Where(x => x.ProductID == oFabricSalesContractDetail.ProductID && x.ProductName == oFabricSalesContractDetail.ProductName && x.Construction == oFabricSalesContractDetail.Construction && x.FabricID == oFabricSalesContractDetail.FabricID && x.FabricNo == oFabricSalesContractDetail.FabricNo && x.ProcessTypeName == oFabricSalesContractDetail.ProcessTypeName && x.FabricWeaveName == oFabricSalesContractDetail.FabricWeaveName && x.FabricWidth == oFabricSalesContractDetail.FabricWidth && x.FinishTypeName == oFabricSalesContractDetail.FinishTypeName && x.MUnitID == oFabricSalesContractDetail.MUnitID).Count() > 0)
                        {
                            oFabricSalesContractDetail.Weight = _oFabricSalesContractDetails.Where(x => x.ProductID == oFabricSalesContractDetail.ProductID && x.ProductName == oFabricSalesContractDetail.ProductName && x.Construction == oFabricSalesContractDetail.Construction && x.FabricID == oFabricSalesContractDetail.FabricID && x.FabricNo == oFabricSalesContractDetail.FabricNo && x.ProcessTypeName == oFabricSalesContractDetail.ProcessTypeName && x.FabricWeaveName == oFabricSalesContractDetail.FabricWeaveName && x.FabricWidth == oFabricSalesContractDetail.FabricWidth && x.FinishTypeName == oFabricSalesContractDetail.FinishTypeName && x.MUnitID == oFabricSalesContractDetail.MUnitID).FirstOrDefault().Weight;
                            oFabricSalesContractDetail.Shrinkage = _oFabricSalesContractDetails.Where(x => x.ProductID == oFabricSalesContractDetail.ProductID && x.ProductName == oFabricSalesContractDetail.ProductName && x.Construction == oFabricSalesContractDetail.Construction && x.FabricID == oFabricSalesContractDetail.FabricID && x.FabricNo == oFabricSalesContractDetail.FabricNo && x.ProcessTypeName == oFabricSalesContractDetail.ProcessTypeName && x.FabricWeaveName == oFabricSalesContractDetail.FabricWeaveName && x.FabricWidth == oFabricSalesContractDetail.FabricWidth && x.FinishTypeName == oFabricSalesContractDetail.FinishTypeName && x.MUnitID == oFabricSalesContractDetail.MUnitID).FirstOrDefault().Shrinkage;
                        }
                        if (!String.IsNullOrEmpty(oFabricSalesContractDetail.Weight))
                        {
                            sTemp = sTemp + "\nWeight:" + oFabricSalesContractDetail.Weight.Trim();
                        }
                        if (!String.IsNullOrEmpty(oFabricSalesContractDetail.Shrinkage))
                        {
                            sTemp = sTemp + "\nShrinkage:" + oFabricSalesContractDetail.Shrinkage.Trim();
                        }
                        // Col 2
                        _oPdfPCell = new PdfPCell(new Phrase(sTemp, _oFontStyle));
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        //_oPdfPCell.MinimumHeight = 40f;
                        _oPdfPCell.Border = 0;
                        //_oPdfPCell.Colspan = _nCount_Raw;
                        _oPdfPCell.BorderWidthLeft = 0.5f;
                        _oPdfPCell.BorderWidthTop = 0.5f;
                        _oPdfPCell.BorderWidthBottom = 0;
                        oPdfPTable.AddCell(_oPdfPCell);

                        // Col 3
                        _oPdfPCell = new PdfPCell(this.ProductDetails(oFabricSalesContractDetail));
                        ////_oPdfPCell = new PdfPCell(new Phrase(oFabricSalesContractDetail.FabricNo, _oFontStyle));
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPCell.Colspan = 8;
                        //_oPdfPCell.Border = 0;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        oPdfPTable.AddCell(_oPdfPCell);

                        //// Col 4
                        //_oPdfPCell = new PdfPCell(new Phrase(oFabricSalesContractDetail.HLReference, _oFontStyle));
                        //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        ////_oPdfPCell.MinimumHeight = 40f;
                        //if (string.IsNullOrEmpty(oFabricSalesContractDetail.ExeNo))
                        //{
                        //    _oPdfPCell.Rowspan = 2;
                        //}
                        //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        //oPdfPTable.AddCell(_oPdfPCell);




                        //// Col 5
                        //sTemp = "";
                        //if (!String.IsNullOrEmpty(oFabricSalesContractDetail.BuyerReference))
                        //{
                        //    sTemp = sTemp + "" + oFabricSalesContractDetail.BuyerReference.Trim();
                        //}
                        //if (!String.IsNullOrEmpty(oFabricSalesContractDetail.BuyerReference))
                        //{
                        //    sTemp = sTemp + ";\n";
                        //}
                        //if (!String.IsNullOrEmpty(oFabricSalesContractDetail.StyleNo))
                        //{
                        //    sTemp = sTemp + "" + oFabricSalesContractDetail.StyleNo.Trim();
                        //}
                        //_oPdfPCell = new PdfPCell(new Phrase(sTemp, _oFontStyle));
                        //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        ////_oPdfPCell.MinimumHeight = 40f;
                        //_oPdfPCell.Rowspan = 2;
                        //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        //oPdfPTable.AddCell(_oPdfPCell);

                        //// Col 6
                        //_oPdfPCell = new PdfPCell(new Phrase(oFabricSalesContractDetail.LDNo + "" + ((EnumShade)oFabricSalesContractDetail.ShadeID).ToString(), _oFontStyle));
                        //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        ////_oPdfPCell.MinimumHeight = 40f;
                        //_oPdfPCell.Rowspan = 2;
                        //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        //oPdfPTable.AddCell(_oPdfPCell);

                        //// Col 7
                        //_oPdfPCell = new PdfPCell(new Phrase(oFabricSalesContractDetail.ColorInfo, _oFontStyle));
                        //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        ////_oPdfPCell.MinimumHeight = 40f;
                        //_oPdfPCell.Rowspan = 2;
                        //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        //oPdfPTable.AddCell(_oPdfPCell);


                        //// Col 8
                        //_oPdfPCell = new PdfPCell(new Phrase(oFabricSalesContractDetail.FinishTypeName, _oFontStyle));
                        //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        ////_oPdfPCell.MinimumHeight = 40f;
                        //_oPdfPCell.Rowspan = 2;
                        //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        //oPdfPTable.AddCell(_oPdfPCell);

                        //// Col 8
                        //if (_oFabricSalesContract.OrderType == (int)EnumFabricRequestType.StockSale || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Sample || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Bulk || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.SampleFOC)
                        //{
                        //    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(Math.Round(oFabricSalesContractDetail.Qty, 0)), _oFontStyle));
                        //}
                        //else
                        //{
                        //    _oPdfPCell = new PdfPCell(new Phrase(oFabricSalesContractDetail.Size, _oFontStyle));
                        //}
                        //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        ////_oPdfPCell.MinimumHeight = 40f;
                        //_oPdfPCell.Rowspan = 2;
                        //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        //oPdfPTable.AddCell(_oPdfPCell);

                        //// Col 10

                        //if (oFabricSalesContractDetail.Status == EnumPOState.Hold)
                        //{
                        //    oFabricSalesContractDetail.Note = _oFabricSCHistorys.Where(m => m.FabricSCDetailID == oFabricSalesContractDetail.FabricSalesContractDetailID && m.FSCDStatusInt == (int)EnumPOState.Hold).First().Note;
                        //    oFabricSalesContractDetail.Note = sTemp + "\n" + oFabricSalesContractDetail.Note;
                        //}

                        //Paragraph oPdfParagraph;
                        //if (oFabricSalesContractDetail.Note == null) { oFabricSalesContractDetail.Note = ""; }
                        //if (oFabricSalesContractDetail.Note.Length > 50)
                        //{
                        //    oPdfParagraph = new Paragraph(new Phrase(oFabricSalesContractDetail.Note, FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL)));
                        //}
                        //else
                        //{
                        //    oPdfParagraph = new Paragraph(new Phrase(oFabricSalesContractDetail.Note, _oFontStyle));
                        //}
                        //oPdfParagraph.SetLeading(0f, 1f);
                        //oPdfParagraph.Alignment = Element.ALIGN_LEFT;
                        //_oPdfPCell = new PdfPCell();
                        //_oPdfPCell.AddElement(oPdfParagraph);
                        //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        ////_oPdfPCell.MinimumHeight = 40f;
                        //_oPdfPCell.Rowspan = 2;
                        //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        //oPdfPTable.AddCell(_oPdfPCell);


                        //_nTotalAmount = _nTotalAmount + (oFabricSalesContractDetail.Qty * oFabricSalesContractDetail.UnitPrice);
                        //_nTotalQty = _nTotalQty + oFabricSalesContractDetail.Qty;

                        //oPdfPTable.CompleteRow();
                        //// Col 3
                        //if (!string.IsNullOrEmpty(oFabricSalesContractDetail.ExeNo))
                        //{
                        //    _oPdfPCell = new PdfPCell(new Phrase("Dispo No:" + oFabricSalesContractDetail.ExeNo, FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLDITALIC)));
                        //    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        //    //_oPdfPCell.MinimumHeight = 40f;
                        //    _oPdfPCell.Colspan = 2;
                        //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        //    oPdfPTable.AddCell(_oPdfPCell);
                        //    oPdfPTable.CompleteRow();
                        //}
                        #endregion

                        _oPdfPCell = new PdfPCell(oPdfPTable);
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();
                    }
                }

                if (_oFabricSalesContract.OrderType == (int)EnumFabricRequestType.SampleFOC || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Sample || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Bulk || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.StockSale || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Buffer)
                {
                    #region Total
                    oPdfPTable = new PdfPTable(10);
                    oPdfPTable.SetWidths(new float[] { 30f, 170f, 90f, 80f, 100f, 75f, 90f, 80f, 60f, 150f });
                    if (_oFabricSalesContractDetails.Count > 1)
                    {
                        _oPdfPCell = new PdfPCell(new Phrase("Total: ", _oFontStyleBold));
                        _oPdfPCell.Colspan = 8;
                        //_oPdfPCell.Border = 0;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPTable.AddCell(_oPdfPCell);
                        _nTotalQty = _oFabricSalesContractDetails.Sum(x => x.Qty);
                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(Math.Round(_nTotalQty, 0)) + " " + _sMUnit, _oFontStyleBold));
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        //_oPdfPCell.Border = 0;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        //_oPdfPCell.Border = 0;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        oPdfPTable.AddCell(_oPdfPCell);
                        oPdfPTable.CompleteRow();

                        _oPdfPCell = new PdfPCell(oPdfPTable);
                        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();
                    }
                    #endregion
                }

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
        }
        private PdfPTable ProductDetails(FabricSalesContractDetail oFabricSCDetail)
        {
            #region Weaving
            PdfPTable oPdfPTable = new PdfPTable(8);
            oPdfPTable.SetWidths(new float[] { 90f, 80f, 100f, 75f, 90f, 80f, 60f, 150f });
            string sTemp = "";
            List<FabricSalesContractDetail> oFabricSalesContractDetails = new List<FabricSalesContractDetail>();
            oFabricSalesContractDetails = _oFabricSalesContractDetails.Where(x => x.ProductID == oFabricSCDetail.ProductID && x.ProductName == oFabricSCDetail.ProductName && x.Construction == oFabricSCDetail.Construction && x.FabricID == oFabricSCDetail.FabricID && x.FabricNo == oFabricSCDetail.FabricNo && x.ProcessTypeName == oFabricSCDetail.ProcessTypeName && x.FabricWeaveName == oFabricSCDetail.FabricWeaveName && x.FabricWidth == oFabricSCDetail.FabricWidth && x.FinishTypeName == oFabricSCDetail.FinishTypeName && x.MUnitID == oFabricSCDetail.MUnitID).ToList();
            foreach (FabricSalesContractDetail oFabricSalesContractDetail in oFabricSalesContractDetails)
            {
                if (!_bIsRnd)
                {
                    oFabricSalesContractDetail.ExeNo = "";
                }
                // Col 1
                _oPdfPCell = new PdfPCell(new Phrase(oFabricSalesContractDetail.FabricNo, _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.MinimumHeight = 40f;
                if (string.IsNullOrEmpty(oFabricSalesContractDetail.ExeNo))
                {
                    _oPdfPCell.Rowspan = 2;
                }
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                // Col 2
                _oPdfPCell = new PdfPCell(new Phrase(oFabricSalesContractDetail.HLReference, _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.MinimumHeight = 40f;
                if (string.IsNullOrEmpty(oFabricSalesContractDetail.ExeNo))
                {
                    _oPdfPCell.Rowspan = 2;
                }
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                // Col 3
                sTemp = "";
                if (!String.IsNullOrEmpty(oFabricSalesContractDetail.BuyerReference))
                {
                    sTemp = sTemp + "" + oFabricSalesContractDetail.BuyerReference.Trim();
                }
                if (!String.IsNullOrEmpty(oFabricSalesContractDetail.BuyerReference))
                {
                    sTemp = sTemp + ";\n";
                }
                if (!String.IsNullOrEmpty(oFabricSalesContractDetail.StyleNo))
                {
                    sTemp = sTemp + "" + oFabricSalesContractDetail.StyleNo.Trim();
                }
                _oPdfPCell = new PdfPCell(new Phrase(sTemp, _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.MinimumHeight = 40f;
                _oPdfPCell.Rowspan = 2;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                // Col 4
                _oPdfPCell = new PdfPCell(new Phrase(oFabricSalesContractDetail.LDNo + " " + ((oFabricSalesContractDetail.ShadeID <= 0) ? "" : ((EnumShade)oFabricSalesContractDetail.ShadeID).ToString()), _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.MinimumHeight = 40f;
                _oPdfPCell.Rowspan = 2;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                // Col 5
                _oPdfPCell = new PdfPCell(new Phrase(oFabricSalesContractDetail.ColorInfo, _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.MinimumHeight = 40f;
                _oPdfPCell.Rowspan = 2;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);


                // Col 6
                _oPdfPCell = new PdfPCell(new Phrase(oFabricSalesContractDetail.FinishTypeName, _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.MinimumHeight = 40f;
                _oPdfPCell.Rowspan = 2;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                // Col 7
                if (_oFabricSalesContract.OrderType == (int)EnumFabricRequestType.StockSale || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Sample || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Bulk || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.SampleFOC || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Buffer)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(Math.Round(oFabricSalesContractDetail.Qty, 0)), _oFontStyle));
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase(oFabricSalesContractDetail.Size, _oFontStyle));
                }
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.MinimumHeight = 40f;
                _oPdfPCell.Rowspan = 2;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                // Col 8

                if (oFabricSalesContractDetail.Status == EnumPOState.Hold)
                {
                    oFabricSalesContractDetail.Note = _oFabricSCHistorys.Where(m => m.FabricSCDetailID == oFabricSalesContractDetail.FabricSalesContractDetailID && m.FSCDStatusInt == (int)EnumPOState.Hold).First().Note;
                    oFabricSalesContractDetail.Note = sTemp + "\n" + oFabricSalesContractDetail.Note;
                }

                Paragraph oPdfParagraph;
                if (oFabricSalesContractDetail.Note == null) { oFabricSalesContractDetail.Note = ""; }
                if (oFabricSalesContractDetail.Note.Length > 50)
                {
                    oPdfParagraph = new Paragraph(new Phrase(oFabricSalesContractDetail.Note, FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL)));
                }
                else
                {
                    oPdfParagraph = new Paragraph(new Phrase(oFabricSalesContractDetail.Note, _oFontStyle));
                }
                oPdfParagraph.SetLeading(0f, 1f);
                oPdfParagraph.Alignment = Element.ALIGN_LEFT;
                _oPdfPCell = new PdfPCell();
                _oPdfPCell.AddElement(oPdfParagraph);
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.MinimumHeight = 40f;
                _oPdfPCell.Rowspan = 2;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                //_nTotalAmount = _nTotalAmount + (oFabricSalesContractDetail.Qty * oFabricSalesContractDetail.UnitPrice);
                //_nTotalQty = _nTotalQty + oFabricSalesContractDetail.Qty;
                oPdfPTable.CompleteRow();
                // ExeNo
                if (!string.IsNullOrEmpty(oFabricSalesContractDetail.ExeNo))
                {
                    _oPdfPCell = new PdfPCell(new Phrase("Dispo No:" + oFabricSalesContractDetail.ExeNo, FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLDITALIC)));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //_oPdfPCell.MinimumHeight = 40f;
                    _oPdfPCell.Colspan = 2;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(_oPdfPCell);
                    oPdfPTable.CompleteRow();
                }

            }
            #endregion

            return oPdfPTable;
        }
        private void PrintBodyProduct_WUOne()
        {
            #region  Details
            #region Detail Column Header
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 8f, 4);
          
            if (_oFabricSalesContractDetails.Count > 0)
            {
                _sMUnit = _oFabricSalesContractDetails[0].MUName;
                
            }

            PdfPTable oPdfPTable = new PdfPTable(9);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 160f, 100f, 95f, 90f, 80f, 75f, 62f,62f, 95f });

            oPdfPCell = new PdfPCell(new Phrase("Description of Goods", _oFontStyleBold));
            //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            //oPdfPCell = new PdfPCell(new Phrase("MKT Ref", _oFontStyleBold));
            //oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Style/Buyer Ref", _oFontStyleBold));
            oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("Lab Dip", _oFontStyleBold));
            oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Color", _oFontStyleBold));
            oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Finish Type", _oFontStyleBold));
            oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Approve Ref", _oFontStyleBold));
            oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            if (_oFabricSalesContract.OrderType == (int)EnumFabricRequestType.StockSale || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Sample || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Bulk || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.SampleFOC || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Buffer)
            {
                oPdfPCell = new PdfPCell(new Phrase(" Qty (" + _sMUnit + ")", _oFontStyleBold));
            }
            else
            {
                oPdfPCell = new PdfPCell(new Phrase("Size", _oFontStyleBold));
            }
            oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            if (_oFabricSalesContract.OrderType == (int)EnumFabricRequestType.StockSale || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Sample || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Bulk || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.SampleFOC || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Buffer)
            {
                oPdfPCell = new PdfPCell(new Phrase(" Qty (M)", _oFontStyleBold));
            }
            else
            {
                oPdfPCell = new PdfPCell(new Phrase("Qty", _oFontStyleBold));
            }
            oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Remarks", _oFontStyleBold));
            oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Colspan = _nTotalColumn; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

            string sTemp = "";
         
            oPdfPTable = new PdfPTable(9);
            oPdfPTable.SetWidths(new float[] { 160f, 100f, 95f, 90f, 80f, 75f, 62f, 62f, 95f });

            //_oFabricSalesContractDetails = _oFabricSalesContractDetails.GroupBy(x => new { x.FabricNo, x.ProductID, x.ProductName, x.Construction, x.ProcessType, x.ProcessTypeName, x.FabricWeave, x.FabricWeaveName, x.FabricWidth, x.FinishTypeName, x.FinishType, x.StyleNo, x.ColorInfo, x.UnitPrice, x.BuyerReference, x.MUnitID, x.IsDeduct }, (key, grp) =>
            //                                     new FabricSalesContractDetail
            //                                     {
            //                                         FabricNo = key.FabricNo,
            //                                         ProductID = key.ProductID,
            //                                         ProductName = key.ProductName,
            //                                         Construction = key.Construction,
            //                                         ProcessType = key.ProcessType,
            //                                         ProcessTypeName = key.ProcessTypeName,
            //                                         FabricWeave = key.FabricWeave,
            //                                         FabricWeaveName = key.FabricWeaveName,
            //                                         FabricWidth = key.FabricWidth,
            //                                         FinishTypeName = key.FinishTypeName,
            //                                         FinishType = key.FinishType,
            //                                         StyleNo = key.StyleNo,
            //                                         ColorInfo = key.ColorInfo,
            //                                         BuyerReference = key.BuyerReference,
            //                                         Qty = grp.Sum(p => p.Qty),
            //                                         UnitPrice = key.UnitPrice,
            //                                         MUnitID = key.MUnitID,
            //                                         IsDeduct = key.IsDeduct
            //                                     }).ToList();

            foreach (FabricSalesContractDetail oItem in _oFabricSalesContractDetails)
            {
                if (String.IsNullOrEmpty(oItem.BuyerReference))
                {
                    oItem.StyleNo = oItem.StyleNo;
                }
                else
                {
                    oItem.StyleNo = oItem.StyleNo + " " + oItem.BuyerReference;
                }
            }
            _oFabricSalesContractDetails = _oFabricSalesContractDetails.OrderBy(o => o.ProductID).ThenBy(a => a.Construction).ThenBy(a => a.ProcessType).ThenBy(a => a.FabricWeaveName).ThenBy(a => a.FabricWidth).ThenBy(a => a.FinishTypeName).ThenBy(b => b.StyleNo).ThenBy(c => c.ColorInfo).ToList();
            //_oExportPIDetails = _oExportPIDetails.OrderBy(o => o.ProductID).ToList();
            #region Details Data
            int nCount = 0; double nTotalQty = 0; double nTotalValue = 0;


            List<FabricSalesContractDetail> oFabricSalesContractDetailFab = new List<FabricSalesContractDetail>();
            List<FabricSalesContractDetail> oFabricSalesContractDetailColors = new List<FabricSalesContractDetail>();
            List<FabricSalesContractDetail> oFabricSalesContractDetailStyleNo = new List<FabricSalesContractDetail>();

            oFabricSalesContractDetailFab = _oFabricSalesContractDetails.GroupBy(x => new { x.FabricID, x.FabricNo, x.ProductID, x.ProductName, x.Construction, x.ProcessType, x.ProcessTypeName, x.FabricWeave, x.FabricWeaveName, x.FabricWidth, x.FinishTypeName, x.FinishType,x.YarnType, x.SCDetailType }, (key, grp) =>
                                                 new FabricSalesContractDetail
                                                 {
                                                     FabricNo = key.FabricNo,
                                                     FabricID = key.FabricID,
                                                     ProductID = key.ProductID,
                                                     ProductName = key.ProductName,
                                                     Construction = key.Construction,
                                                     ProcessType = key.ProcessType,
                                                     ProcessTypeName = key.ProcessTypeName,
                                                     FabricWeave = key.FabricWeave,
                                                     FabricWeaveName = key.FabricWeaveName,
                                                     FabricWidth = key.FabricWidth,
                                                     FinishTypeName = key.FinishTypeName,
                                                     FinishType = key.FinishType,
                                                     YarnType = key.YarnType,
                                                     //ColorInfo = key.ColorInfo,
                                                     //BuyerReference = key.BuyerReference,
                                                     //Qty = grp.Sum(p => p.Qty),
                                                     //UnitPrice = key.UnitPrice,
                                                     //MUnitID = key.MUnitID,

                                                     SCDetailType = key.SCDetailType
                                                 }).ToList();
            bool bFlag = false;
            bool bFlagTwo = false;
            bool bIsNewPage = false;
            foreach (FabricSalesContractDetail oItem in oFabricSalesContractDetailFab)
            {
                oFabricSalesContractDetailStyleNo = new List<FabricSalesContractDetail>();
                //oExportPIDetailColor = _oExportPIDetails.Where(x => x.ProductID == oItem.ProductID && x.ProductName == oItem.ProductName && x.Construction == oItem.Construction && x.ProcessTypeName == oItem.ProcessTypeName && x.FabricWeaveName == oItem.FabricWeaveName && x.FabricWidth == oItem.FabricWidth && x.FinishTypeName == oItem.FinishTypeName && x.FinishType == oItem.FinishType && x.MUnitID == oItem.MUnitID && x.IsDeduct == oItem.IsDeduct).ToList();
                oFabricSalesContractDetailStyleNo = _oFabricSalesContractDetails.Where(x => x.FabricNo == oItem.FabricNo && x.ProductID == oItem.ProductID && x.ProductName == oItem.ProductName && x.Construction == oItem.Construction && x.ProcessType == oItem.ProcessType && x.ProcessTypeName == oItem.ProcessTypeName && x.FabricWeave == oItem.FabricWeave && x.FabricWeaveName == oItem.FabricWeaveName && x.FabricWidth == oItem.FabricWidth && x.FinishTypeName == oItem.FinishTypeName && x.FinishType == oItem.FinishType && x.SCDetailType == oItem.SCDetailType && x.YarnType == oItem.YarnType).ToList();
                bFlag = true;
              
                sTemp = "";
                if (!String.IsNullOrEmpty(oItem.FabricNo))
                {
                    sTemp = sTemp + "Mkt Ref:" + oItem.FabricNo.Trim();
                   
                }
                if (!String.IsNullOrEmpty(oItem.ProductName))
                {
                    sTemp = sTemp + "\n";
                    sTemp = sTemp + "" + oItem.ProductName.Trim();
                }
                //if (!String.IsNullOrEmpty(oItem.ProductName))
                //{
                //    sTemp = sTemp + "\n";
                //}
                if (!String.IsNullOrEmpty(oItem.ProcessTypeName))
                {
                    sTemp = sTemp + " " + oItem.ProcessTypeName.Trim();
                }
                if (!String.IsNullOrEmpty(oItem.FabricDesignName))
                {
                    sTemp = sTemp + ",";
                }
                if (!String.IsNullOrEmpty(oItem.FabricDesignName))
                {
                    sTemp = sTemp + " " + oItem.FabricDesignName.Trim();
                }
                //if (!String.IsNullOrEmpty(oItem.FabricDesignName))
                //{
                //    sTemp = sTemp + "\n";
                //}
                if (!String.IsNullOrEmpty(oItem.Construction))
                {
                    sTemp = sTemp + "\n";
                    sTemp = sTemp + "Const:" + oItem.Construction.Trim();
                }
              
                if (_oFabricSalesContract.OrderType != (int)EnumFabricRequestType.HandLoom)
                {
                    if (!String.IsNullOrEmpty(oItem.FabricWidth))
                    {
                        sTemp = sTemp + "\n";
                        sTemp = sTemp + "CW:" + oItem.FabricWidth.Trim();
                    }
                }
                //if (!String.IsNullOrEmpty(oItem.FabricWeaveName))
                //{
                //    sTemp = sTemp + "\n";
                //}
                if (!String.IsNullOrEmpty(oItem.FabricWeaveName))
                {
                    sTemp = sTemp + "\n";
                    sTemp = sTemp + "Weave:" + oItem.FabricWeaveName.Trim();
                }
                //if (_oFabricSalesContractDetails.FirstOrDefault() != null && _oFabricSalesContractDetails.FirstOrDefault().FabricSalesContractDetailID > 0 && _oFabricSalesContractDetails.Where(x => x.ProductID == oItem.ProductID && x.ProductName == oItem.ProductName && x.Construction == oItem.Construction && x.FabricID == oItem.FabricID && x.FabricNo == oItem.FabricNo && x.ProcessTypeName == oItem.ProcessTypeName && x.FabricWeaveName == oItem.FabricWeaveName && x.FabricWidth == oItem.FabricWidth && x.FinishTypeName == oItem.FinishTypeName  && x.Weight == oItem.Weight && x.Shrinkage == oItem.Shrinkage).Count() > 0)
                //{
                oItem.Weight = _oFabricSalesContractDetails.Where(x => x.ProductID == oItem.ProductID && oItem.FabricNo == oItem.FabricNo && x.ProductName == oItem.ProductName && x.Construction == oItem.Construction && x.FabricID == oItem.FabricID && x.FabricNo == oItem.FabricNo && x.ProcessTypeName == oItem.ProcessTypeName && x.FabricWeaveName == oItem.FabricWeaveName && x.FabricWidth == oItem.FabricWidth && x.FinishTypeName == oItem.FinishTypeName && x.YarnType == oItem.YarnType).FirstOrDefault().Weight;
                oItem.Shrinkage = _oFabricSalesContractDetails.Where(x => x.ProductID == oItem.ProductID && oItem.FabricNo == oItem.FabricNo && x.ProductName == oItem.ProductName && x.Construction == oItem.Construction && x.FabricID == oItem.FabricID && x.FabricNo == oItem.FabricNo && x.ProcessTypeName == oItem.ProcessTypeName && x.FabricWeaveName == oItem.FabricWeaveName && x.FabricWidth == oItem.FabricWidth && x.FinishTypeName == oItem.FinishTypeName && x.YarnType == oItem.YarnType).FirstOrDefault().Shrinkage;
                //}
                if (!String.IsNullOrEmpty(oItem.Weight))
                {
                    sTemp = sTemp + "\nWeight:" + oItem.Weight.Trim();
                }
                if (!String.IsNullOrEmpty(oItem.Shrinkage))
                {
                    sTemp = sTemp + "\nShrinkage:" + oItem.Shrinkage.Trim();
                }
                if (!String.IsNullOrEmpty(oItem.YarnType))
                {
                    sTemp = sTemp + "\nYarnType:" + oItem.YarnType.Trim();
                }


                _oPhrase = new Phrase();
                _oPhrase.Add(new Chunk(sTemp, FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL)));
                //_oPhrase.Add(new Chunk(" ["+oItem.ExportQuality+"]", FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL)));
                _oPdfPCell = new PdfPCell(_oPhrase);

                //_oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName + ",\n" + sConst + "\n" + sTemp, FontFactory.GetFont("Tahoma", 7f, 0)));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                //_oPdfPCell.Rowspan = _nCount_Raw;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BorderWidthLeft = 0.5f;
                _oPdfPCell.BorderWidthTop = 0.5f;
                _oPdfPCell.BorderWidthBottom = 0;
                _oPdfPCell.Rowspan = 2;
                oPdfPTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase(oItem.FabricNo, _oFontStyle));
                //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                ////_oPdfPCell.Rowspan = _nCount_Raw;
                //_oPdfPCell.Border = 0;
                //_oPdfPCell.BorderWidthLeft = 0.5f;
                //_oPdfPCell.BorderWidthTop = 0.5f;
                //_oPdfPCell.BorderWidthBottom = 0;
                //oPdfPTable.AddCell(_oPdfPCell);


                oFabricSalesContractDetailStyleNo = oFabricSalesContractDetailStyleNo.GroupBy(x => new { x.StyleNo }, (key, grp) =>
                                                 new FabricSalesContractDetail
                                                 {
                                                     StyleNo = key.StyleNo,
                                                 }).ToList();

                oFabricSalesContractDetailStyleNo = oFabricSalesContractDetailStyleNo.OrderBy(o => o.ProductID).ThenBy(b => b.StyleNo).ThenBy(c => c.ColorInfo).ToList();
                foreach (FabricSalesContractDetail oItem2 in oFabricSalesContractDetailStyleNo)
                {
                    //oExportPIDetailColors = _oExportPIDetails.Where(x => x.StyleNo == oItem2.StyleNo).ToList();
                    oFabricSalesContractDetailColors = _oFabricSalesContractDetails.Where(x => x.FabricNo == oItem.FabricNo && x.ProductID == oItem.ProductID && x.StyleNo == oItem2.StyleNo && x.Construction == oItem.Construction && x.ProcessType == oItem.ProcessType && x.ProcessTypeName == oItem.ProcessTypeName && x.FabricWeave == oItem.FabricWeave && x.FabricWeaveName == oItem.FabricWeaveName && x.FabricWidth == oItem.FabricWidth && x.FinishTypeName == oItem.FinishTypeName && x.FinishType == oItem.FinishType && x.SCDetailType == oItem.SCDetailType && x.YarnType == oItem.YarnType).ToList();
                    if (!bFlag)
                    {
                        //if (bIsNewPage)
                        //{
                        //    _oPhrase = new Phrase();
                        //    _oPhrase.Add(new Chunk(oItem.ProductName + ",\n" + sConst + "\n" + sTemp, FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL)));
                        //    //_oPhrase.Add(new Chunk(" ["+oItem.ExportQuality+"]", FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL)));
                        //    _oPdfPCell = new PdfPCell(_oPhrase);
                        //}
                        //else
                        //{
                        //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        //}

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));

                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _oPdfPCell.Border = 0;
                        _oPdfPCell.Rowspan = 2;
                        _oPdfPCell.BorderWidthLeft = 0.5f;
                        if (bIsNewPage)
                        {
                            _oPdfPCell.BorderWidthTop = 0.5f;
                        }
                        else
                        {
                            _oPdfPCell.BorderWidthTop = 0;
                        }
                        _oPdfPCell.BorderWidthBottom = 0;
                        oPdfPTable.AddCell(_oPdfPCell);

                        //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        //_oPdfPCell.Border = 0;
                        //_oPdfPCell.BorderWidthLeft = 0.5f;
                        //if (bIsNewPage)
                        //{
                        //    _oPdfPCell.BorderWidthTop = 0.5f;
                        //}
                        //else
                        //{
                        //    _oPdfPCell.BorderWidthTop = 0;
                        //}
                        //_oPdfPCell.BorderWidthBottom = 0;
                        //oPdfPTable.AddCell(_oPdfPCell);
                    }

                    bFlag = false;

                    //_nCount_Raw = oExportPIDetailColors.Where(x => x.StyleNo == oItem2.StyleNo).Count();
                    _oPdfPCell = new PdfPCell(new Phrase(oItem2.StyleNo, _oFontStyle));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.BorderWidthLeft = 0.5f;
                    _oPdfPCell.BorderWidthTop = 0.5f;
                    _oPdfPCell.BorderWidthBottom = 0;
                    _oPdfPCell.Rowspan = 2;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(_oPdfPCell);

                    oFabricSalesContractDetailColors = oFabricSalesContractDetailColors.OrderBy(o => o.SLNo).ToList();
                    bFlagTwo = true;
                    foreach (FabricSalesContractDetail oItem3 in oFabricSalesContractDetailColors)
                    {
                          nCount++;
                        if (!_bIsRnd)
                        { oItem3.ExeNo = ""; }
                        if (!bFlagTwo)
                        {
                            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            _oPdfPCell.Rowspan = 2;
                            _oPdfPCell.Border = 0;
                            _oPdfPCell.BorderWidthLeft = 0.5f;
                            _oPdfPCell.BorderWidthTop = 0;
                            _oPdfPCell.BorderWidthBottom = 0;
                            oPdfPTable.AddCell(_oPdfPCell);

                            //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                            //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            //_oPdfPCell.Border = 0;
                            //_oPdfPCell.BorderWidthLeft = 0.5f;
                            //_oPdfPCell.BorderWidthTop = 0;
                            //_oPdfPCell.BorderWidthBottom = 0;
                            //oPdfPTable.AddCell(_oPdfPCell);

                            //_nCount_Raw = oExportPIDetailColors.Where(x => x.StyleNo == oItem2.StyleNo).Count();
                            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            _oPdfPCell.Border = 0;
                            _oPdfPCell.Rowspan = 2;
                            _oPdfPCell.BorderWidthLeft = 0.5f;
                            _oPdfPCell.BorderWidthTop = 0;
                            _oPdfPCell.BorderWidthBottom = 0;
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            oPdfPTable.AddCell(_oPdfPCell);
                        }
                        bFlagTwo = false;
                        //if (oItem3.ShadeID > 0)
                        //{
                        //    oItem3.LDNo = oItem3.LDNo + " Opt:'" + ((EnumShade)oItem3.ShadeID).ToString() + "'";
                        //}

                        _oPdfPCell = new PdfPCell(new Phrase(oItem3.LDNo, _oFontStyle));
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        //_oPdfPCell.Rowspan = 2;
                        if (string.IsNullOrEmpty(oItem3.ExeNo))
                        {
                            _oPdfPCell.Rowspan = 2;
                        }
                        //_oPdfPCell.MinimumHeight = 40f;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem3.ColorInfo, _oFontStyle));
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        //_oPdfPCell.MinimumHeight = 40f;
                        //_oPdfPCell.Rowspan = 2;
                        if (string.IsNullOrEmpty(oItem3.ExeNo))
                        {
                            _oPdfPCell.Rowspan = 2;
                        }
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem3.FinishTypeName, _oFontStyle));
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPCell.Rowspan = 2;
                        //_oPdfPCell.MinimumHeight = 40f;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        oPdfPTable.AddCell(_oPdfPCell);
                        if (oItem3.ShadeID > 0)
                        {
                            oItem3.HLReference = oItem3.HLReference + " Opt:'" + ((EnumShade)oItem3.ShadeID).ToString() + "'";
                        }

                        _oPdfPCell = new PdfPCell(new Phrase(oItem3.HLReference, _oFontStyle));
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPCell.Rowspan = 2;
                        //_oPdfPCell.MinimumHeight = 40f;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        oPdfPTable.AddCell(_oPdfPCell);

                        // Col 7
                        if (_oFabricSalesContract.OrderType == (int)EnumFabricRequestType.StockSale || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Sample || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Bulk || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.SampleFOC || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Buffer)
                        {
                            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(Math.Round(oItem3.Qty,2)), _oFontStyle));
                        }
                        else
                        {
                            _oPdfPCell = new PdfPCell(new Phrase(oItem3.Size, _oFontStyle));
                        }
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        //_oPdfPCell.MinimumHeight = 40f;
                        _oPdfPCell.Rowspan = 2;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        oPdfPTable.AddCell(_oPdfPCell);

                        // Col 8
                        if (_oFabricSalesContract.OrderType == (int)EnumFabricRequestType.StockSale || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Sample || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Bulk || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.SampleFOC || _oFabricSalesContract.OrderType == (int)EnumFabricRequestType.Buffer)
                        {
                            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(Math.Round(oItem3.QtyTwo, 2)), _oFontStyle));
                        }
                        else
                        {
                            _oPdfPCell = new PdfPCell(new Phrase(oItem3.Size, _oFontStyle));
                        }
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        //_oPdfPCell.MinimumHeight = 40f;
                        _oPdfPCell.Rowspan = 2;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        oPdfPTable.AddCell(_oPdfPCell);

                        // Col 9
                        if (oItem3.Status == EnumPOState.Hold && _oFabricSCHistorys.Count>0)
                        {
                            oItem3.Note = _oFabricSCHistorys.Where(m => m.FabricSCDetailID == oItem3.FabricSalesContractDetailID && m.FSCDStatusInt == (int)EnumPOState.Hold).First().Note;
                            oItem3.Note = sTemp + "\n" + oItem3.Note;
                        }

                        Paragraph oPdfParagraph;
                        if (oItem3.Note == null) { oItem3.Note = ""; }
                        if (oItem3.Note.Length > 50)
                        {
                            oPdfParagraph = new Paragraph(new Phrase(oItem3.Note, FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL)));
                        }
                        else
                        {
                            oPdfParagraph = new Paragraph(new Phrase(oItem3.Note, _oFontStyle));
                        }
                        oPdfParagraph.SetLeading(0f, 1f);
                        oPdfParagraph.Alignment = Element.ALIGN_LEFT;
                        _oPdfPCell = new PdfPCell();
                        _oPdfPCell.AddElement(oPdfParagraph);
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        //_oPdfPCell.MinimumHeight = 40f;
                        //_oPdfPCell.Rowspan = 2;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _oPdfPCell.Rowspan = 2;
                        oPdfPTable.AddCell(_oPdfPCell);
                        oPdfPTable.CompleteRow();
                        if (!string.IsNullOrEmpty(oItem3.ExeNo))
                        {
                            _oPdfPCell = new PdfPCell(new Phrase("Dispo No:" + oItem3.ExeNo, FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLDITALIC)));
                            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            //_oPdfPCell.MinimumHeight = 40f;
                            _oPdfPCell.Colspan = 2;
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            oPdfPTable.AddCell(_oPdfPCell);
                            oPdfPTable.CompleteRow();
                        }

                        bIsNewPage = false;
                  //  _nUsagesHeight=  (int)ESimSolPdfHelper.CalculatePdfPTableHeight(_oPdfPTable) + (int)ESimSolPdfHelper.CalculatePdfPTableHeight(oPdfPTable);
                    _nUsagesHeight =  CalculatePdfPTableHeight(_oPdfPTable);
                    _nUsagesHeight = _nUsagesHeight + CalculatePdfPTableHeight(oPdfPTable);
                      //  _nUsagesHeight = CalculatePdfPTableHeight(oPdfPTable);
                    if (nCount>10)
                        {
                            nCount = 0;
                            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

                     

                            _oPdfPCell = new PdfPCell(oPdfPTable);
                            _oPdfPCell.Border = 0;
                         
                            _oPdfPCell.BorderWidthLeft = 0;
                            _oPdfPCell.BorderWidthRight = 0;
                            //_oPdfPCell.BorderWidthTop = 0.5f;
                            _oPdfPCell.BorderWidthBottom = 0.5f;
                            _oPdfPCell.Colspan = _nTotalColumn; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                            _oPdfPTable.CompleteRow();

                            //_nUsagesHeight = 0;
                            _oDocument.Add(_oPdfPTable);
                            _oDocument.NewPage();
                            //_oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
                            //_oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
                            //_oDocument.SetMargins(35f, 15f, 5f, 30f);
                            oPdfPTable.DeleteBodyRows();
                            _oPdfPTable.DeleteBodyRows();
                            //_nTotalColumn = 7;
                            //_oPdfPTable = new PdfPTable(_nTotalColumn);
                            //_oPdfPTable.SetWidths(new float[] { 200f, 55f, 125f, 125f, 70f, 70f, 75f });
                            //_oPdfPTable.WidthPercentage = 100;
                            //_oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

                            this.PrintHeader();
                            this.ReporttHeader();
                            //isNewPage = true;
                            //this.WaterMark(35f, 15f, 5f, 30f);
                            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                            bIsNewPage = true;
                        }
                    }
                }
            }

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Colspan = _nTotalColumn; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            oPdfPTable = new PdfPTable(9);
            oPdfPTable.SetWidths(new float[] { 160f, 100f, 95f, 90f, 80f, 75f, 62f, 62f, 95f });

            nTotalQty = _oFabricSalesContractDetails.Where(c => c.SCDetailType == EnumSCDetailType.None).Sum(x => x.Qty);

            _oPdfPCell = new PdfPCell(new Phrase("Total ", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.MinimumHeight = 40f;
            _oPdfPCell.Colspan = 6;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalQty) + "\n" + _sMUnit, _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.MinimumHeight = 40f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPTable.AddCell(_oPdfPCell);

            nTotalQty = _oFabricSalesContractDetails.Where(c => c.SCDetailType == EnumSCDetailType.None).Sum(x => x.QtyTwo);
            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalQty) + "\n" + "M", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.MinimumHeight = 40f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.MinimumHeight = 40f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            _oPhrase = new Phrase();
            if (!string.IsNullOrEmpty(_oFabricSalesContract.Note))
            {
                _oPhrase.Add(new Chunk("Remarks : ", _oFontStyleBold));
                _oPhrase.Add(new Chunk(_oFabricSalesContract.Note, _oFontStyle));
              
                _oPdfPCell = new PdfPCell(_oPhrase);
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.MinimumHeight = 40f;
                _oPdfPCell.Colspan = 8;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Colspan = _nTotalColumn; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            #endregion

            #endregion


        }
        private void Print_Body_B()
        {
            _nTotalQty = 0;

            PdfPTable oPdfPTable = new PdfPTable(9);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 28f, 155f, 75f, 80f, 60f, 100f, 95f, 60f, 100f });
            int nProductID = 0;
            _nTotalQty = 0;
            _nCount = 0;
            string sTemp = "";

            if (_oFabricSalesContractDetails.Count > 0)
            {
                // _oFabricSalesContractDetails = _oFabricSalesContractDetails.OrderBy(o => o.ProductID).ToList();

                oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Description of Goods", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("MKT Ref", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Buyer Ref\n/Style", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                //oPdfPCell = new PdfPCell(new Phrase("Size", _oFontStyleBold));
                //oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                //oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                //oPdfPTable.AddCell(oPdfPCell);


                //oPdfPCell = new PdfPCell(new Phrase("Style No", _oFontStyleBold));
                //oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                //oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                //oPdfPTable.AddCell(oPdfPCell);


                oPdfPCell = new PdfPCell(new Phrase("Lab Dip No", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);



                oPdfPCell = new PdfPCell(new Phrase("Design \n& Pattern", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Color", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Finish Type", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Remarks", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                //oExportBillDetailsTemp = oExportBillDetailsTemp.OrderBy(o => o.ProductID).ThenBy(a => a.Construction).ThenBy(a => a.ProcessTypeName).ThenBy(a => a.FabricWeaveName).ThenBy(a => a.FabricWidth).ThenBy(a => a.FinishTypeName).ThenBy(b => b.StyleNo).ThenBy(c => c.ColorInfo).ToList();


                foreach (FabricSalesContractDetail oFabricSalesContractDetail in _oFabricSalesContractDetails)
                {
                    oPdfPTable = new PdfPTable(9);
                    oPdfPTable.SetWidths(new float[] { 28f, 155f, 75f, 80f, 60f, 100f, 95f, 60f, 100f });


                    #region PrintDetail
                    _nCount++;

                    _oPdfPCell = new PdfPCell(new Phrase(_nCount.ToString(), _oFontStyle));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.MinimumHeight = 40f;
                    _oPdfPCell.Rowspan = 2;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(_oPdfPCell);

                    //if (nProductID != oFabricSalesContractDetail.ProductID)

                    //{
                    sTemp = "";
                    if (!String.IsNullOrEmpty(oFabricSalesContractDetail.ProductName))
                    {
                        sTemp = sTemp + "" + oFabricSalesContractDetail.ProductName.Trim();
                    }
                    if (!String.IsNullOrEmpty(oFabricSalesContractDetail.ProductName))
                    {
                        sTemp = sTemp + "\n";
                    }
                    if (!String.IsNullOrEmpty(oFabricSalesContractDetail.ProcessTypeName))
                    {
                        sTemp = sTemp + "" + oFabricSalesContractDetail.ProcessTypeName.Trim();
                    }
                    if (!String.IsNullOrEmpty(oFabricSalesContractDetail.FabricDesignName))
                    {
                        sTemp = sTemp + ",";
                    }
                    if (!String.IsNullOrEmpty(oFabricSalesContractDetail.FabricDesignName))
                    {
                        sTemp = sTemp + "" + oFabricSalesContractDetail.FabricDesignName.Trim();
                    }
                    if (!String.IsNullOrEmpty(oFabricSalesContractDetail.FabricDesignName))
                    {
                        sTemp = sTemp + "\n";
                    }
                    if (!String.IsNullOrEmpty(oFabricSalesContractDetail.Construction))
                    {
                        sTemp = sTemp + "Const:" + oFabricSalesContractDetail.Construction.Trim();
                    }
                    if (!String.IsNullOrEmpty(oFabricSalesContractDetail.FabricWidth))
                    {
                        sTemp = sTemp + "\n";
                    }
                    if (_oFabricSalesContract.OrderType != (int)EnumFabricRequestType.HandLoom)
                    {
                        if (!String.IsNullOrEmpty(oFabricSalesContractDetail.FabricWidth))
                        {
                            sTemp = sTemp + "CW:" + oFabricSalesContractDetail.FabricWidth.Trim();
                        }
                    }
                    if (!String.IsNullOrEmpty(oFabricSalesContractDetail.FabricWeaveName))
                    {
                        sTemp = sTemp + "\n";
                    }
                    if (!String.IsNullOrEmpty(oFabricSalesContractDetail.FabricWeaveName))
                    {
                        sTemp = sTemp + "Weave:" + oFabricSalesContractDetail.FabricWeaveName.Trim();
                    }

                    if (!String.IsNullOrEmpty(oFabricSalesContractDetail.Weight))
                    {
                        sTemp = sTemp + "\nWeight:" + oFabricSalesContractDetail.Weight.Trim();
                    }
                    if (!String.IsNullOrEmpty(oFabricSalesContractDetail.Shrinkage))
                    {
                        sTemp = sTemp + "\nShrinkage:" + oFabricSalesContractDetail.Shrinkage.Trim();
                    }

                    //_nCount_P = _oFabricSalesContractDetails.Where(x => x.ProductID == oFabricSalesContractDetail.ProductID).Count();
                    _oPdfPCell = new PdfPCell(new Phrase(sTemp, _oFontStyle));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.MinimumHeight = 40f;
                    _oPdfPCell.Rowspan = 2;
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.BorderWidthLeft = 0.5f;
                    _oPdfPCell.BorderWidthTop = 0.5f;
                    _oPdfPCell.BorderWidthBottom = 0;
                    oPdfPTable.AddCell(_oPdfPCell);



                    _oPdfPCell = new PdfPCell(new Phrase(oFabricSalesContractDetail.FabricNo, _oFontStyle));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.MinimumHeight = 40f;
                    _oPdfPCell.Rowspan = 2;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(_oPdfPCell);

                    sTemp = "";
                    if (!String.IsNullOrEmpty(oFabricSalesContractDetail.BuyerReference))
                    {
                        sTemp = sTemp + "" + oFabricSalesContractDetail.BuyerReference.Trim();
                    }
                    if (!String.IsNullOrEmpty(oFabricSalesContractDetail.BuyerReference))
                    {
                        sTemp = sTemp + ";\n";
                    }
                    if (!String.IsNullOrEmpty(oFabricSalesContractDetail.StyleNo))
                    {
                        sTemp = sTemp + "" + oFabricSalesContractDetail.StyleNo.Trim();
                    }

                    _oPdfPCell = new PdfPCell(new Phrase(sTemp, _oFontStyle));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.MinimumHeight = 40f;
                    _oPdfPCell.Rowspan = 2;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(_oPdfPCell);



                    _oPdfPCell = new PdfPCell(new Phrase(oFabricSalesContractDetail.LDNo + "" + ((EnumShade)oFabricSalesContractDetail.ShadeID).ToString(), _oFontStyle));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.MinimumHeight = 40f;
                    _oPdfPCell.Rowspan = 2;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(_oPdfPCell);



                    _oPdfPCell = new PdfPCell(new Phrase(oFabricSalesContractDetail.DesignPattern, _oFontStyle));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.MinimumHeight = 40f;
                    _oPdfPCell.Rowspan = 2;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(_oPdfPCell);


                    // Col 7
                    _oPdfPCell = new PdfPCell(new Phrase(oFabricSalesContractDetail.ColorInfo, _oFontStyle));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.MinimumHeight = 40f;
                    _oPdfPCell.Rowspan = 2;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(_oPdfPCell);

                    // Col 8
                    _oPdfPCell = new PdfPCell(new Phrase(oFabricSalesContractDetail.FinishTypeName, _oFontStyle));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.MinimumHeight = 40f;
                    _oPdfPCell.Rowspan = 2;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(_oPdfPCell);

                    // Col 9
                    if (oFabricSalesContractDetail.Status == EnumPOState.Hold)
                    {
                        oFabricSalesContractDetail.Note = _oFabricSCHistorys.Where(m => m.FabricSCDetailID == oFabricSalesContractDetail.FabricSalesContractDetailID && m.FSCDStatusInt == (int)EnumPOState.Hold).First().Note;
                        oFabricSalesContractDetail.Note = sTemp + "\n" + oFabricSalesContractDetail.Note;
                    }
                    Paragraph oPdfParagraph;
                    if (oFabricSalesContractDetail.Note == null) { oFabricSalesContractDetail.Note = ""; }
                    if (oFabricSalesContractDetail.Note.Length > 50)
                    {
                        oPdfParagraph = new Paragraph(new Phrase(oFabricSalesContractDetail.Note, FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL)));
                    }
                    else
                    {
                        oPdfParagraph = new Paragraph(new Phrase(oFabricSalesContractDetail.Note, _oFontStyle));
                    }
                    oPdfParagraph.SetLeading(0f, 1f);
                    oPdfParagraph.Alignment = Element.ALIGN_LEFT;
                    _oPdfPCell = new PdfPCell();
                    _oPdfPCell.AddElement(oPdfParagraph);
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.MinimumHeight = 40f;
                    _oPdfPCell.Rowspan = 2;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(_oPdfPCell);


                    _nTotalAmount = _nTotalAmount + (oFabricSalesContractDetail.Qty * oFabricSalesContractDetail.UnitPrice);
                    _nTotalQty = _nTotalQty + oFabricSalesContractDetail.Qty;

                    oPdfPTable.CompleteRow();
                    #endregion

                    nProductID = oFabricSalesContractDetail.ProductID;

                    _oPdfPCell = new PdfPCell(oPdfPTable);
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                }

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

            }
        }
        #endregion
        #region Print Local
        public byte[] PrepareReport_Local(FabricSalesContract oFabricSalesContract, Company oCompany, BusinessUnit oBusinessUnit, List<FabricDeliverySchedule> oFebricDeliverySchedules, List<FabricSalesContractNote> oFabricSalesContractNotes, List<FabricSCHistory> oFabricSCHistorys, FabricOrderSetup oFabricOrderSetup, bool bIsRnd, bool bIsLog)
        {
            _oFabricSalesContract = oFabricSalesContract;
            _oFebricDeliverySchedules = oFebricDeliverySchedules;
            _oFabricSalesContractNotes = oFabricSalesContractNotes;
            _oFabricSCHistorys = oFabricSCHistorys;
            _oBusinessUnit = oBusinessUnit;
            _oFabricSalesContractDetails = oFabricSalesContract.FabricSalesContractDetails;
            _oCompany = oCompany;
            _bIsRnd = bIsRnd;
            _oFabricOrderSetup = oFabricOrderSetup;
            if (_oFabricSalesContractDetails.Count > 0)
            {
                _sMUnit = _oFabricSalesContractDetails[0].MUName;
                _sMUnit = "Y";
            }

            #region Page Setup
            _oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
            _oDocument.SetMargins(30f, 30f, 30f, 30f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;

            _oDocument.Open();

            _oPdfPTable.SetWidths(new float[] { 595f });
            #endregion

            this.PrintHeader();
            this.ReporttHeader_B();
            this.PrintBlankRow();
            this.PrintHead_Bulk(bIsLog);
            this.PrintWaterMark();
            this.Print_Body_Local();
            this.PrintRemarks();
            this.PrintBlankRow();
            this.Print_Instruction();
            this.Print_DeliverySchedule();
            this.PrintFooter();
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        public void PrintRemarks()
        {
            PdfPTable oPdfPTable = new PdfPTable(2);
            oPdfPTable.SetWidths(new float[] { 80f, 673f });
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);

            _oPdfPCell = new PdfPCell(new Phrase("Remarks :", FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD)));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(oPdfPTable);

            _oPdfPCell = new PdfPCell(new Phrase(_oFabricSalesContract.Note, FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL)));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(oPdfPTable);

            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        private void PrintBlankRow()
        {
            PdfPTable oPdfPTable = new PdfPTable(1);
            oPdfPTable.SetWidths(new float[] { 753f });
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.FixedHeight = 20;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(oPdfPTable);

            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        private void PrintWaterMark()
        {
            _oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
            _oDocument.SetMargins(30f, 30f, 30f, 30f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            if (_oFabricSalesContract.ApproveBy == 0)
            {
                ESimSolWM_Footer.WMFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                ESimSolWM_Footer.WMFontSize = 75;
                ESimSolWM_Footer.WMRotation = 45;
                ESimSolWM_Footer PageEventHandler = new ESimSolWM_Footer();
                PageEventHandler.WaterMark = "Unauthorised"; //Footer print with page event handler
                PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            }
            else if (_oFabricSalesContract.ApproveBy != 0 && _oFabricSalesContract.CurrentStatus == EnumFabricPOStatus.Cancel)
            {
                ESimSolWM_Footer.WMFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                ESimSolWM_Footer.WMFontSize = 75;
                ESimSolWM_Footer.WMRotation = 45;
                ESimSolWM_Footer PageEventHandler = new ESimSolWM_Footer();
                PageEventHandler.WaterMark = "Canceled"; //Footer print with page event handler
                PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            }
            else if (_bIsLog == true)
            {
                ESimSolWM_Footer.WMFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                ESimSolWM_Footer.WMFontSize = 75;
                ESimSolWM_Footer.WMRotation = 45;
                ESimSolWM_Footer PageEventHandler = new ESimSolWM_Footer();
                PageEventHandler.WaterMark = "Unauthorised"; //Footer print with page event handler
                PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            }
            _oDocument.Open();
            PdfContentByte cb = PDFWriter.DirectContent;
            _oDocument.NewPage();
        }
        private void Print_Body_Local()
        {
            PdfPTable oPdfPTable = new PdfPTable(9);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 28f, 170f, 75f, 80f, 70f, 100f, 75f, 50f, 82f });
            _nTotalQty = 0;
            _nCount = 0;

            if (_oFabricSalesContractDetails.Count > 0)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
                oPdfPCell = new PdfPCell(new Phrase("#SL", _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Description of Goods", _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Dispo No", _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Color", _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Lab Dip", _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Finish Type", _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Quantity", _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Rate", _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Amount", _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);


                oPdfPCell = new PdfPCell(oPdfPTable);
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(oPdfPCell);
                _oPdfPTable.CompleteRow();

                int nCount = 0;
                double dGrandTotal = 0;
                double dDiscount = 0;
                double dTotal = 0;
                double dQty = 0;
                string sTemp = "";
                foreach (FabricSalesContractDetail oFabricSalesContractDetail in _oFabricSalesContractDetails)
                {
                    if (oFabricSalesContractDetail.SCDetailType == EnumSCDetailType.DeductCharge)
                    {
                        _discountFabricSalesContractDetail.Add(oFabricSalesContractDetail);
                    }
                    else
                    {
                        oPdfPTable = new PdfPTable(9);
                        oPdfPTable.SetWidths(new float[] { 28f, 170f, 75f, 80f, 70f, 100f, 75f, 50f, 82f });
                        nCount++;
                        oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                        #region GetProductDescription
                        sTemp = "";
                        if (!String.IsNullOrEmpty(oFabricSalesContractDetail.ProductName))
                        {
                            sTemp = sTemp + "" + oFabricSalesContractDetail.ProductName.Trim();
                        }
                        if (!String.IsNullOrEmpty(oFabricSalesContractDetail.ProductName))
                        {
                            sTemp = sTemp + "\n";
                        }
                        if (!String.IsNullOrEmpty(oFabricSalesContractDetail.ProcessTypeName))
                        {
                            sTemp = sTemp + "" + oFabricSalesContractDetail.ProcessTypeName.Trim();
                        }
                        if (!String.IsNullOrEmpty(oFabricSalesContractDetail.FabricDesignName))
                        {
                            sTemp = sTemp + ",";
                        }
                        if (!String.IsNullOrEmpty(oFabricSalesContractDetail.FabricDesignName))
                        {
                            sTemp = sTemp + "" + oFabricSalesContractDetail.FabricDesignName.Trim();
                        }
                        if (!String.IsNullOrEmpty(oFabricSalesContractDetail.Construction))
                        {
                            sTemp = sTemp + "\n";
                        }
                        if (!String.IsNullOrEmpty(oFabricSalesContractDetail.Construction))
                        {
                            sTemp = sTemp + "Const:" + oFabricSalesContractDetail.Construction.Trim();
                        }
                    
                        if (!String.IsNullOrEmpty(oFabricSalesContractDetail.FabricWeaveName))
                        {
                            sTemp = sTemp + "\n Weave Type:" + oFabricSalesContractDetail.FabricWeaveName;
                        }
                        if (!String.IsNullOrEmpty(oFabricSalesContractDetail.Weight))
                        {
                            sTemp = sTemp + "\n Weight:" + oFabricSalesContractDetail.Weight;
                        }
                        if (!String.IsNullOrEmpty(oFabricSalesContractDetail.Shrinkage))
                        {
                            sTemp = sTemp + "\n Shrinkage:" + oFabricSalesContractDetail.Shrinkage;
                        }
                        if (_oFabricSalesContract.OrderType != (int)EnumFabricRequestType.HandLoom)
                        {
                            if (!String.IsNullOrEmpty(oFabricSalesContractDetail.FabricWidth))
                            {
                                sTemp = sTemp + "CW:" + oFabricSalesContractDetail.FabricWidth.Trim();
                            }
                        }
                        #endregion
                        #region

                        oPdfPCell = new PdfPCell(new Phrase(sTemp, _oFontStyle));
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase(oFabricSalesContractDetail.ExeNo, _oFontStyle));
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase(oFabricSalesContractDetail.ColorInfo, _oFontStyle));
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase(oFabricSalesContractDetail.LDNo, _oFontStyle));
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase(oFabricSalesContractDetail.FinishTypeName, _oFontStyle));
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase(oFabricSalesContractDetail.Qty.ToString(), _oFontStyle));
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
                        dQty = dQty + oFabricSalesContractDetail.Qty;

                        oPdfPCell = new PdfPCell(new Phrase(oFabricSalesContractDetail.UnitPrice.ToString(), _oFontStyle));
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase(oFabricSalesContractDetail.AmountSt, _oFontStyle));
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
                        dGrandTotal = dGrandTotal + oFabricSalesContractDetail.Amount;

                        dTotal = dTotal + oFabricSalesContractDetail.Amount;
                        oPdfPCell = new PdfPCell(oPdfPTable);
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(oPdfPCell);
                        _oPdfPTable.CompleteRow();
                    }
                }
                GetTotalRow(dTotal, dQty);
                double discount = DiscountPrint();
              //  if (discount>0)
                GetGrandTotalRow(dGrandTotal - discount);
            }
        }
        private double DiscountPrint()
        {
            PdfPTable oPdfPTable = new PdfPTable(1);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 753f });
            _nTotalQty = 0;
            _nCount = 0;
            double dDiscount = 0;

            if (_discountFabricSalesContractDetail.Count > 0)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
                oPdfPCell = new PdfPCell(new Phrase("Special Discount :", _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPCell.Border = 0; oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(oPdfPTable);
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(oPdfPCell);
                _oPdfPTable.CompleteRow();

                double dQty = 0;
                _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
                foreach (FabricSalesContractDetail oFabricSalesContractDetail in _discountFabricSalesContractDetail)
                {

                     oPdfPTable = new PdfPTable(9);
                    oPdfPTable.SetWidths(new float[] { 28f, 170f, 75f, 80f, 70f, 100f, 75f, 50f, 82f });

                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oFabricSalesContractDetail.ProductName, _oFontStyle));
                    oPdfPCell.Colspan = 5; oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oFabricSalesContractDetail.Qty.ToString(), _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
                    dQty = dQty + oFabricSalesContractDetail.Qty;

                    oPdfPCell = new PdfPCell(new Phrase(oFabricSalesContractDetail.UnitPrice.ToString(), _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    if (oFabricSalesContractDetail.SCDetailType == EnumSCDetailType.DeductCharge)
                    {
                        oPdfPCell = new PdfPCell(new Phrase("(" + oFabricSalesContractDetail.AmountSt + ")", _oFontStyle));
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
                        dDiscount = dDiscount + oFabricSalesContractDetail.Amount;
                    }
                    else
                    {
                        oPdfPCell = new PdfPCell(new Phrase(oFabricSalesContractDetail.AmountSt, _oFontStyle));
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
                        dDiscount = dDiscount - oFabricSalesContractDetail.Amount;
                    }
                    oPdfPCell = new PdfPCell(oPdfPTable);
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(oPdfPCell);
                    _oPdfPTable.CompleteRow();
                }
                //GetTotalRowDiscount(dDiscount);       //if need to show total discount
            }
            return dDiscount;
        }
        private void GetTotalRowDiscount(double dDiscount)
        {
            PdfPTable oPdfPTable = new PdfPTable(2);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 653f, 100f });
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);

            #region Sub Total
            oPdfPCell = new PdfPCell(new Phrase("Total Discount:", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            dDiscount = System.Math.Round(dDiscount, 2);
            oPdfPCell = new PdfPCell(new Phrase(dDiscount.ToString(), _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            #endregion
            oPdfPCell = new PdfPCell(oPdfPTable);
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        private void GetTotalRow(double dTotal, double dQty)
        {
            PdfPTable oPdfPTable = new PdfPTable(9);
            oPdfPTable.SetWidths(new float[] { 28f, 170f, 75f, 80f, 70f, 100f, 75f, 50f, 82f });
            PdfPCell oPdfPCell;
            //oPdfPTable.SetWidths(new float[] { 333f, 75f, 50f, 82f });
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);

            #region Total Qty
            oPdfPCell = new PdfPCell(new Phrase("Total :", _oFontStyle));
            oPdfPCell.Colspan = 6; oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(dQty.ToString(), _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            #endregion

            #region SUb Total
            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            dTotal = System.Math.Round(dTotal, 2);
            oPdfPCell = new PdfPCell(new Phrase(dTotal.ToString(), _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            #endregion

            oPdfPCell = new PdfPCell(oPdfPTable);
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        private void GetDiscountRow(double dDiscount)
        {
            PdfPTable oPdfPTable = new PdfPTable(2);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 500f, 82f });
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase("Discount:", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(dDiscount.ToString(), _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(oPdfPTable);
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        private void GetGrandTotalRow(double dGrandTotal)
        {
            PdfPTable oPdfPTable = new PdfPTable(9);
            oPdfPTable.SetWidths(new float[] { 28f, 170f, 75f, 80f, 70f, 100f, 75f, 50f, 82f });

            //PdfPTable oPdfPTable = new PdfPTable(2);
            PdfPCell oPdfPCell;
           // oPdfPTable.SetWidths(new float[] { 500f, 82f });
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase("Total:", _oFontStyle));
            oPdfPCell.Colspan = 8; oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            dGrandTotal = System.Math.Round(dGrandTotal, 2);
            oPdfPCell = new PdfPCell(new Phrase(_oFabricSalesContract.Currency+""+dGrandTotal.ToString(), _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(oPdfPTable);
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
                        #endregion
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
    }
}
