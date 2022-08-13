using System;
using System.Data;
using ESimSol.BusinessObjects;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;
using ICS.Core;
using ICS.Core.Utility;

namespace ESimSol.Reports
{
    public class rptDURequisition
    {
        public rptDURequisition() 
        {
            DUOrderSetup = new DUOrderSetup();
            LotParents = new List<LotParent>();
            DyeingOrderReports = new List<DyeingOrderReport>();
            DUProGuideLineDetails_Receive = new List<DUProGuideLineDetail>();
            DUProGuideLineDetails_Return = new List<DUProGuideLineDetail>();
            DURequisitionDetails_SRS = new List<DURequisitionDetail>();
            DURequisitionDetails_SRM = new List<DURequisitionDetail>();
        }

        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyleTwo;
        iTextSharp.text.Font _oFontStyleForCC;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPTable _oPdfPTableTemp = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        DURequisition _oDURequisition = new DURequisition();
        List<DURequisitionDetail> _oDURequisitionDetails = new List<DURequisitionDetail>();
        List<DUProductionStatusReport> _oDUProductionStatusReports=new List<DUProductionStatusReport>();
        Company _oCompany = new Company();
        BusinessUnit _oBusinessUnit = new BusinessUnit();
        DURequisitionSetup _oDURequisitionSetup = new DURequisitionSetup();

        public DUOrderSetup DUOrderSetup { set; get; }
        public List<LotParent> LotParents { set; get; }
        public List<FabricLotAssign> FabricLotAssigns { set; get; }
        public List<DyeingOrderFabricDetail> DyeingOrderFabricDetails { set; get; }
        
        public List<DyeingOrderReport> DyeingOrderReports { set; get; }
        public List<DUProGuideLineDetail> DUProGuideLineDetails_Receive { set; get; }
        public List<DUProGuideLineDetail> DUProGuideLineDetails_Return { set; get; }
        public List<DURequisitionDetail> DURequisitionDetails_SRS { set; get; }
        public List<DURequisitionDetail> DURequisitionDetails_SRM { get; set; }

        double _nTotalQty = 0;
        string _sMUnitName = "";
        #endregion

        public byte[] PrepareReport(DURequisition oDURequisition, Company oCompany, BusinessUnit oBusinessUnit, DURequisitionSetup oDURequisitionSetup, List<DUProductionStatusReport> oDUProductionStatusReports)
        {
            _oCompany = oCompany;
            _oDURequisition = oDURequisition;
            _oDURequisitionSetup = oDURequisitionSetup;
            _oBusinessUnit = oBusinessUnit;
            _oDURequisitionDetails = oDURequisition.DURequisitionDetails;
            _oDUProductionStatusReports=oDUProductionStatusReports;

            if (_oDURequisitionDetails.Count > 0)
            {
                _sMUnitName = _oDURequisitionDetails[0].MUnit;
            }

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            _oDocument.SetMargins(20f, 15f, 20f, 10f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);

            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();

            _oPdfPTable.SetWidths(new float[] { 525f });
            #endregion

            this.PrintHeader();
            this.PrintBody();
            _oPdfPTable.HeaderRows = 9;
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
            oPdfPTable.SetWidths(new float[] { 100f, 267.5f, 100f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            if (_oCompany.CompanyLogo != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(60f, 35f);
                _oPdfPCell = new PdfPCell(_oImag);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 35;
                //_oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Rowspan = 3; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; 
                oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 35;
                //_oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Rowspan = 3; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            }
            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Name, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.PringReportHead, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("Print Date: " + DateTime.Now.ToShortDateString(), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

            #region Blank Space
            //_oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            //_oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();
            #endregion

            #region ReportHeader
            oPdfPTable = new PdfPTable(3);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 100f, 267.5f, 100f });

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);
            
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(_oDURequisitionSetup.Name+" ("+_oDURequisitionSetup.ShortName+")", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 1; _oPdfPCell.BorderWidthTop = 1; _oPdfPCell.BorderWidthBottom = 1;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region ReportHeader
            if (_oDURequisition.ApprovebyID == 0)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.ITALIC);
                _oPdfPCell = new PdfPCell(new Phrase("Unauthorize Requisition", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oPdfPTableTemp.AddCell(_oPdfPCell);
                _oPdfPTableTemp.CompleteRow();
            }
            #endregion
            #region Blank Space
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            _oPdfPTableTemp.AddCell(_oPdfPCell);
            _oPdfPTableTemp.CompleteRow();
            #endregion
        }
        #endregion

        #region Report Body
        private PdfPTable GetDetails_Table() 
        {
            PdfPTable oPdfPTable = new PdfPTable(8);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 23.5f, 77f, 225.2f, 47f, 70.5f, 47f, 35.5f, 70.5f });
            return oPdfPTable;
        }
        private void PrintBody()
        {
            PdfPTable oPdfPTable = new PdfPTable(4);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 125f, 175f, 150f, 175f });

            #region Report Vocher No, DURequisition Date & Currency
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);

            _oPdfPCell = new PdfPCell(new Phrase(_oDURequisitionSetup.ShortName + " No : " + _oDURequisition.RequisitionNo, _oFontStyle)); _oPdfPCell.Colspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Req Date : " + _oDURequisition.ReqDate.ToString("dd MMM yy HH:mm:tt"), _oFontStyle)); _oPdfPCell.Colspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            string sBuyerNames = string.Join(", ", _oDURequisitionDetails.Select(x => x.BuyerName).Distinct().ToList());
            _oPdfPCell = new PdfPCell(new Phrase("Buyer's Name : " + sBuyerNames, _oFontStyle)); _oPdfPCell.Colspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            if (_oDURequisition.IssueDate==DateTime.MinValue)
            { _oPdfPCell = new PdfPCell(new Phrase("Issue Date :    ", _oFontStyle)); _oPdfPCell.Colspan = 2; }
            else { _oPdfPCell = new PdfPCell(new Phrase("Issue Date : " + _oDURequisition.IssueDate.ToString("dd MMM yy HH:mm:tt"), _oFontStyle)); _oPdfPCell.Colspan = 2; }
            
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Issue Store : " + _oDURequisition.IssueStore, _oFontStyle)); _oPdfPCell.Colspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Receive Store : "+_oDURequisition.ReceiveStore, _oFontStyle)); _oPdfPCell.Colspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTableTemp.AddCell(_oPdfPCell);
            _oPdfPTableTemp.CompleteRow();
            #endregion

            #region Blank Space
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region DURequisition Details
            oPdfPTable = new PdfPTable(1);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 455.5f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            _oFontStyleTwo = FontFactory.GetFont("Tahoma", 9f, 1);

            _oPdfPCell = new PdfPCell(new Phrase("Requisition Details Description", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            oPdfPTable = new PdfPTable(7);
            oPdfPTable = GetDetails_Table();

          
            _oPdfPCell = new PdfPCell(new Phrase("#SL", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Order No", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Commodity", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Order Qty", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Lot No", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Quantity", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Unit", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Remarks", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTableTemp.AddCell(_oPdfPCell);
            _oPdfPTableTemp.CompleteRow();


            int nCount = 0;
            int nRawCount = 0;
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            _oFontStyleForCC = FontFactory.GetFont("Tahoma", 8f, 0);
            foreach (DURequisitionDetail oItem in _oDURequisitionDetails)
            {
                oPdfPTable = new PdfPTable(7);
                oPdfPTable = GetDetails_Table();

                nCount++;
                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.DyeingOrderNo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty_Order), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.LotNo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.MUnit, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Note, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 1; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _nTotalQty = _nTotalQty + oItem.Qty;

                #region For Page brack if raw count >45

                if (_oPdfPTable.Rows.Count >= 45)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 1; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();

                    _oPdfPCell = new PdfPCell(new Phrase("Continued ...", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();

                    _oDocument.Add(_oPdfPTable);
                    _oDocument.NewPage();
                    _oPdfPTable.DeleteBodyRows();

                    _oPdfPCell = new PdfPCell(_oPdfPTableTemp);
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();


                    nCount = 0;


                }
                #endregion Page brack
            }
            nRawCount = _oPdfPTable.Rows.Count;

            #region For Print blank raw if (30-raw count)>0
            for (int i = 0; i <= (30 - nRawCount); i++)
            {
                oPdfPTable = new PdfPTable(7);
                oPdfPTable = GetDetails_Table();

                _oPdfPCell = new PdfPCell(new Phrase("  ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("  ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("  ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("  ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("  ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("  ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("  ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("  ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 1; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


                oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            for (int i = 1; i <= 1; i++)
            {
                oPdfPTable = new PdfPTable(7);
                oPdfPTable = GetDetails_Table();

                nCount++;

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 1; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 1; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 1; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 1; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 1; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 1; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 1; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 1; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 1; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 1; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


                oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion

            #region Total DURequisition Amount
            oPdfPTable = new PdfPTable(4);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 411.7f, 48f, 37.5f, 77.5f });
            //oPdfPTable.SetWidths(new float[] { 30.5f, 75f, 245.2f, 70.5f, 60f, 37.5f, 77.5f });


            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(" Total", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("   " + Global.MillionFormat(_nTotalQty) + " " + _sMUnitName, _oFontStyle)); _oPdfPCell.Colspan = 2;
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Blank Space
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            if (_oDUProductionStatusReports.Count != 0 && DUOrderSetup.IsInHouse)
            {
                #region Summary Print
                oPdfPTable = new PdfPTable(9);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.SetWidths(new float[] { 25f, 218.2f, 70.6f, 70.6f,   70.6f, 70.6f, 70.6f, 70.6f, 70.6f });

                #region Header
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
                _oPdfPCell = new PdfPCell(new Phrase("#SL", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.BorderWidthLeft = _oPdfPCell.BorderWidthRight = _oPdfPCell.BorderWidthTop = _oPdfPCell.BorderWidthBottom = 1;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Commodity", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = _oPdfPCell.BorderWidthTop = _oPdfPCell.BorderWidthBottom = 1;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Order No", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = _oPdfPCell.BorderWidthTop = _oPdfPCell.BorderWidthBottom = 1;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Order Qty", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = _oPdfPCell.BorderWidthTop = _oPdfPCell.BorderWidthBottom = 1;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Lot Assign", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = _oPdfPCell.BorderWidthTop = _oPdfPCell.BorderWidthBottom = 1;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Pending Assign", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = _oPdfPCell.BorderWidthTop = _oPdfPCell.BorderWidthBottom = 1;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("SRS Qty", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = _oPdfPCell.BorderWidthTop = _oPdfPCell.BorderWidthBottom = 1;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("SRM Qty", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = _oPdfPCell.BorderWidthTop = _oPdfPCell.BorderWidthBottom = 1;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Balance", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = _oPdfPCell.BorderWidthTop = _oPdfPCell.BorderWidthBottom = 1;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                #endregion

                #region
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
                int nProductID = nCount = 0;
                double nQty_LotAssign_Total = 0, nQty_YetToLotAssign_Total=0;

                var oDyeingOrderReports_Grp = _oDUProductionStatusReports.GroupBy(x => new { x.DyingOrderID, x.OrderNo, x.ProductID, x.ProductName }, (key, grp) =>
                                    new
                                    {
                                        DyingOrderID = key.DyingOrderID,
                                        OrderNo = key.OrderNo,
                                        ProductID = key.ProductID,
                                        ProductName = key.ProductName,
                                        DyeingOrderReports = grp
                                    }).ToList();

                foreach (var oItem in oDyeingOrderReports_Grp)
                {
                    if (oItem.ProductID != nProductID)
                    {
                        nCount++;
                        _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthLeft = _oPdfPCell.BorderWidthRight = _oPdfPCell.BorderWidthBottom = 1;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName, _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        _oPdfPCell.BorderWidthTop = _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = _oPdfPCell.BorderWidthBottom = 1;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.OrderNo, _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        _oPdfPCell.BorderWidthTop = _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = _oPdfPCell.BorderWidthBottom = 1;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        //ORDER QTY
                        double nQty_Order = oItem.DyeingOrderReports.Sum(x => x.Qty_Order);
                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_Order), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        _oPdfPCell.BorderWidthTop = _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = _oPdfPCell.BorderWidthBottom = 1;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        //ORDER LOT ASSIGN
                        double nQty_Lot_Assign = 0;

                        foreach (var oDOFD in DyeingOrderFabricDetails.Where(x => x.DyeingOrderID == oItem.DyingOrderID && x.ProductID == oItem.ProductID))
                            nQty_Lot_Assign += FabricLotAssigns.Where(x => x.FEOSDID == oDOFD.FEOSDID).Sum(x => x.Qty);

                        nQty_LotAssign_Total += nQty_Lot_Assign;

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_Lot_Assign), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        _oPdfPCell.BorderWidthTop = _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = _oPdfPCell.BorderWidthBottom = 1;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        //ORDER YET 2 LOT ASSIGN
                        double Qty_YetToAssign = (nQty_Order < nQty_Lot_Assign ? 0 : (nQty_Order - nQty_Lot_Assign)); 
                        nQty_YetToLotAssign_Total += Qty_YetToAssign;
                        
                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(Qty_YetToAssign), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        _oPdfPCell.BorderWidthTop = _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = _oPdfPCell.BorderWidthBottom = 1;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        //ORDER SRS
                        double nQty_SRS = DURequisitionDetails_SRS.Where(x => x.DyeingOrderID == oItem.DyingOrderID && x.ProductID == oItem.ProductID).Sum(x=>x.Qty);
                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_SRS), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        _oPdfPCell.BorderWidthTop = _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = _oPdfPCell.BorderWidthBottom = 1;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        //ORDER SRM
                        double nQty_SRM = DURequisitionDetails_SRM.Where(x => x.DyeingOrderID == oItem.DyingOrderID && x.ProductID == oItem.ProductID).Sum(x => x.Qty);
                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_SRM), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        _oPdfPCell.BorderWidthTop = _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = _oPdfPCell.BorderWidthBottom = 1;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        //ORDER BALANCE
                        double nQty_Balance = nQty_Lot_Assign + nQty_SRM - nQty_SRS;
                        if (nQty_Balance <= 0) { nQty_Balance = 0; }
                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_Balance), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        _oPdfPCell.BorderWidthTop = _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = _oPdfPCell.BorderWidthBottom = 1;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    }
                    nProductID = oItem.ProductID;
                }

                #region

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);

                _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Colspan = 3;
                _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = _oPdfPCell.BorderWidthBottom = 1;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                double nQty_Order_Total = _oDUProductionStatusReports.Sum(x => x.Qty_Order);
                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_Order_Total), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.BorderWidthTop = _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = _oPdfPCell.BorderWidthBottom = 1;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_LotAssign_Total), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.BorderWidthTop = _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = _oPdfPCell.BorderWidthBottom = 1;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_YetToLotAssign_Total), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.BorderWidthTop = _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = _oPdfPCell.BorderWidthBottom = 1;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                //ORDER SRS & SRM
                double nQty_SRS_Total = DURequisitionDetails_SRS.Sum(x => x.Qty);
                double nQty_SRM_Total = DURequisitionDetails_SRM.Sum(x => x.Qty);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_SRS_Total), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.BorderWidthTop = _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = _oPdfPCell.BorderWidthBottom = 1;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_SRM_Total), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.BorderWidthTop = _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = _oPdfPCell.BorderWidthBottom = 1;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat( nQty_LotAssign_Total + nQty_SRM_Total - nQty_SRS_Total), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.BorderWidthTop = _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = _oPdfPCell.BorderWidthBottom = 1;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                #endregion


                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 1; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                #endregion
            }
            else if (!DUOrderSetup.IsInHouse)
            {
                SetDyeingOrderStatus();
            }

            #region Narration Print
            oPdfPTable = new PdfPTable(2);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 50f, 475f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Blank Space
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 30; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region print Signature Captions
            oPdfPTable = new PdfPTable(4);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 131.25f, 131.25f, 131.25f, 131.25f });


            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);

            _oPdfPCell = new PdfPCell(new Phrase("__________________ \n" + " Requisition By"+"\n"+_oDURequisition.RequisitionByName , _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("__________________  \n Approved By \n" + _oDURequisition.ApprovedByName , _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("__________________  \n Issued By \n" + _oDURequisition.IssuedByName, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("__________________ \n Received By \n" + _oDURequisition.ReceivedByName, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();



            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);



            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //  oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            //_oPdfPCell = new PdfPCell(oPdfPTable);
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


            #endregion
        }

        private void SetDyeingOrderStatus()
        {
            if (LotParents.Count > 0)
            {
                var oDyeingOrderReports_Grp = DyeingOrderReports.GroupBy(x => new { x.DyeingOrderID, x.OrderNoFull }, (key, grp) =>
                                     new
                                     {
                                         DyeingOrderID = key.DyeingOrderID,
                                         OrderNo = key.OrderNoFull,
                                         DyeingOrderReports = grp
                                     }).ToList();

                double GT_Receive_Qty = 0, GT_Balance = 0;
                PdfPTable oPdfPTable = new PdfPTable(8);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.SetWidths(new float[] { 25.5f, 125.5f, 44.4f, 44.4f, 44.4f, 44.4f, 44.4f, 44.4f }); //130

                foreach (var oOrder in oDyeingOrderReports_Grp)
                {
                    List<LotParent> oLotParents = new List<LotParent>();
                    List<LotParent> oLotParents_In = new List<LotParent>();
                    List<LotParent> oLotParents_Out = new List<LotParent>();
                    //List<DyeingOrderReport> oDyeingOrderReports = new List<DyeingOrderReport>();
                    List<DURequisitionDetail> oDURequisitionDetails_SRM = new List<DURequisitionDetail>();
                    List<DURequisitionDetail> oDURequisitionDetails_SRS = new List<DURequisitionDetail>();
                    List<DUProGuideLineDetail> oDUProGuideLineDetails_Receive = new List<DUProGuideLineDetail>();
                    List<DUProGuideLineDetail> oDUProGuideLineDetails_Return = new List<DUProGuideLineDetail>();

                    oLotParents = LotParents.Where(p => p.DyeingOrderID == oOrder.DyeingOrderID || p.DyeingOrderID_Out == oOrder.DyeingOrderID).ToList();
                    oLotParents_In = LotParents.Where(x => x.DyeingOrderID == oOrder.DyeingOrderID).ToList();
                    oLotParents_Out = LotParents.Where(x => x.DyeingOrderID_Out == oOrder.DyeingOrderID).ToList();
                    oDURequisitionDetails_SRM = DURequisitionDetails_SRM.Where(x => x.DyeingOrderID == oOrder.DyeingOrderID).ToList();
                    oDURequisitionDetails_SRS = DURequisitionDetails_SRS.Where(x => x.DyeingOrderID == oOrder.DyeingOrderID).ToList();
                    oDUProGuideLineDetails_Return = DUProGuideLineDetails_Return.Where(x => x.DyeingOrderID == oOrder.DyeingOrderID).ToList();
                    oDUProGuideLineDetails_Receive = DUProGuideLineDetails_Receive.Where(x => x.DyeingOrderID == oOrder.DyeingOrderID).ToList();

                    var oProducts = LotParents.Where(p => p.DyeingOrderID == oOrder.DyeingOrderID || p.DyeingOrderID_Out == oOrder.DyeingOrderID).GroupBy(x => new { x.DyeingOrderID, x.ProductID }, (key, grp) =>
                                     new
                                     {
                                         DyeingOrderID = key.DyeingOrderID,
                                         ProductID = key.ProductID,
                                     }).ToList();

                    #region Summary Print V2
                 
                    _oPdfPCell = new PdfPCell(new Phrase("Party Goods Receive Notes (" + oOrder.OrderNo + ")", FontFactory.GetFont("Tahoma", 9f, 3)));
                    _oPdfPCell.Colspan = 8;
                    _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.BorderWidthBottom = 0;
                    _oPdfPCell.BackgroundColor = BaseColor.GRAY;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oPdfPTable.AddCell(_oPdfPCell);
                    oPdfPTable.CompleteRow();

                    #region Header
                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);

                    #region 1st Row

                    _oPdfPCell = new PdfPCell(new Phrase("#SL", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;  _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Commodity", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;  _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Yarn Lot", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;  _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Order Qty", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;  _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Received Qty", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("SRM Qty", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("SRS Qty", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Balance", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    #endregion

                    #endregion

                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
                    int nProductID = 0, nLotID = 0, nCount = 0;
                    double _nQTy_Total_DUE = 0;

                    foreach (var oItem in oProducts)
                    {
                        if (oItem.ProductID != nProductID)
                        {
                            if (nCount > 1)
                            {
                                #region SUB TOTAL
                                //_oPdfPCell = new PdfPCell(new Phrase("Sub Total", _oFontStyle));
                                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Colspan = 3;
                                //_oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                                //double nQty_Order = oOrder.DyeingOrderReports.Where(x => x.ProductID == nProductID).Sum(x => x.Qty);
                                //_oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_Order), _oFontStyle));
                                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //_oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                                //double nQty_GRN = oDUProGuideLineDetails_Receive.Where(x => x.ProductID == nProductID).Sum(x => x.Qty);
                                //double nQty_SRM = oDURequisitionDetails_SRM.Where(x => x.ProductID == nProductID).Sum(x => x.Qty);
                                //double nQty_T_In = oLotParents_In.Where(x => x.ProductID == nProductID && x.DyeingOrderID_Out != 0).Sum(x => x.Qty);
                                //double nQty_T_Out = oLotParents_Out.Where(x => x.ProductID == nProductID).Sum(x => x.Qty);
                                //double nQty_SW = oDURequisitionDetails_SRS.Where(x => x.ProductID == nProductID).Sum(x => x.Qty);
                                //double nQty_Return = oDUProGuideLineDetails_Return.Where(x => x.ProductID == nProductID).Sum(x => x.Qty);

                                //double nQty_Rcv = (nQty_GRN + nQty_T_In) - (nQty_T_Out + nQty_Return);
                                //double nQty_Balance = (nQty_Rcv + nQty_SRM) - nQty_SW;

                                //_oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_Rcv), _oFontStyle));
                                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //_oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                                //_oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_SRM), _oFontStyle));
                                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //_oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                                //_oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_SW), _oFontStyle));
                                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //_oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                                ////DUE
                                //_oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_Balance), _oFontStyle));
                                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //_oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);
                                #endregion
                            }

                            #region Lot Wise Status

                            #region Group By Lot Wise
                            var dataGrpList = oLotParents.Where(P => P.ProductID == oItem.ProductID).GroupBy(x => new { LotID = x.LotID }, (key, grp) => new LotParent
                            {
                                LotID = key.LotID,
                                LotNo = grp.First().LotNo,
                                ProductID = grp.First().ProductID,
                                ProductName = grp.First().ProductName,
                                DyeingOrderID = grp.First().DyeingOrderID,
                                DyeingOrderID_Out = grp.First().DyeingOrderID_Out,
                                Qty = grp.Sum(x => x.Qty)
                            });
                            #endregion

                            nCount = 0;
                            foreach (var oLotParent in dataGrpList.OrderBy(x => x.ProductID).ThenBy(x => x.LotID))
                            {
                                if (oLotParent.LotID != nLotID)
                                {
                                    #region DATA
                                    nCount++;
                                    _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                                    _oPdfPCell = new PdfPCell(new Phrase(oLotParent.ProductName, _oFontStyle));
                                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                                    //Y-Lot
                                    _oPdfPCell = new PdfPCell(new Phrase(oLotParent.LotNo, _oFontStyle));
                                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                                    //if (oItem.ProductID != nProductID)
                                    //{
                                    double nQty_Order = oOrder.DyeingOrderReports.Where(x => x.ProductID == oLotParent.ProductID).Sum(x => x.Qty);
                                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_Order), _oFontStyle));
                                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        //_oPdfPCell.Rowspan = dataGrpList.Where(x => x.ProductID == oItem.ProductID).Count();
                                        _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                                    //}

                                        double nQty_GRN = oDUProGuideLineDetails_Receive.Where(x => x.ProductID == oLotParent.ProductID && x.LotID == oLotParent.LotID).Sum(x => x.Qty);
                                        double nQty_SRM = oDURequisitionDetails_SRM.Where(x => x.ProductID == oLotParent.ProductID && x.DestinationLotID == oLotParent.LotID).Sum(x => x.Qty);
                                        double nQty_T_In = oLotParents_In.Where(x => x.ProductID == oLotParent.ProductID && x.DyeingOrderID_Out != 0 && x.LotID == oLotParent.LotID).Sum(x => x.Qty);
                                        double nQty_T_Out = oLotParents_Out.Where(x => x.ProductID == oLotParent.ProductID && x.LotID == oLotParent.LotID).Sum(x => x.Qty);
                                        double nQty_SW = oDURequisitionDetails_SRS.Where(x => x.ProductID == oLotParent.ProductID && x.LotID == oLotParent.LotID).Sum(x => x.Qty);
                                        double nQty_Return = oDUProGuideLineDetails_Return.Where(x => x.ProductID == oLotParent.ProductID && x.LotID == oLotParent.LotID).Sum(x => x.Qty);

                                    double nQty_Rcv = (nQty_GRN + nQty_T_In) - (nQty_T_Out + nQty_Return);
                                    double nQty_Balance = (nQty_Rcv + nQty_SRM) - nQty_SW;
                                    
                                    GT_Receive_Qty += nQty_Rcv;
                                    GT_Balance += nQty_Balance;

                                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_Rcv), _oFontStyle));
                                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_SRM), _oFontStyle));
                                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_SW), _oFontStyle));
                                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_Balance), _oFontStyle));
                                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                                    #endregion
                                }
                                nLotID = oLotParent.LotID;
                                nProductID = oItem.ProductID;
                            }
                            #endregion
                        }
                    }

                    //if (nCount > 1)
                    //{
                    //    #region SUB TOTAL
                    //    _oPdfPCell = new PdfPCell(new Phrase("Sub Total", _oFontStyle));
                    //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Colspan = 3;
                    //    _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    //    double nQty_Order = oDyeingOrderReports.Where(x => x.ProductID == nProductID).Sum(x => x.Qty);
                    //    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_Order), _oFontStyle));
                    //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //    _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    //    double nQty_GRN = oDUProGuideLineDetails_Receive.Where(x => x.ProductID == nProductID).Sum(x => x.Qty);
                    //    double nQty_SRM = oDURequisitionDetails_SRM.Where(x => x.ProductID == nProductID).Sum(x => x.Qty);
                    //    double nQty_T_In = oLotParents_In.Where(x => x.ProductID == nProductID && x.DyeingOrderID_Out != 0).Sum(x => x.Qty);
                    //    double nQty_T_Out = oLotParents_Out.Where(x => x.ProductID == nProductID).Sum(x => x.Qty);
                    //    double nQty_SW = oDURequisitionDetails_SRS.Where(x => x.ProductID == nProductID).Sum(x => x.Qty);
                    //    double nQty_Return = oDUProGuideLineDetails_Return.Where(x => x.ProductID == nProductID).Sum(x => x.Qty);

                    //    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_GRN), _oFontStyle));
                    //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //    _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    //    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_SRM), _oFontStyle));
                    //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //    _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    //    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_T_In), _oFontStyle));
                    //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //    _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    //    //DUE
                    //    double nBalance_DUE = nQty_Order - nQty_GRN - nQty_T_In;
                    //    nBalance_DUE = nBalance_DUE < 0 ? 0 : nBalance_DUE;

                    //    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nBalance_DUE), _oFontStyle));
                    //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //    _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    //    //I-S/W
                    //    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_SW), _oFontStyle));
                    //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //    _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    //    //T-OUT
                    //    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_T_Out), _oFontStyle));
                    //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //    _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    //    //RETURN
                    //    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_Return), _oFontStyle));
                    //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //    _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    //    //BALANCE
                    //    double nBalance_Store_SubTotal = (nQty_GRN + nQty_SRM + nQty_T_In) - nQty_SW - nQty_T_Out - nQty_Return;
                    //    nBalance_Store_SubTotal = nBalance_Store_SubTotal < 0 ? 0 : nBalance_Store_SubTotal;

                    //    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nBalance_Store_SubTotal), _oFontStyle));
                    //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //    _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);
                    //    #endregion
                    //}

                    #region Grand Total
                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);

                    //Count, OQ, GQ, SRM-Q, T-IN,        DUE, Y-Lot, I-S/W,T-OUT, RPQ, SB
                    _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Colspan = 3;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    double _Order = DyeingOrderReports.Sum(x => x.Qty);
                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_Order), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(GT_Receive_Qty), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    double _nQty_SRM = DURequisitionDetails_SRM.Sum(x => x.Qty);
                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nQty_SRM), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    double _nQty_SRS = DURequisitionDetails_SRS.Sum(x => x.Qty);
                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nQty_SRS), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(GT_Balance), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    #endregion

                    #endregion
                }
                oPdfPTable.CompleteRow();
                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 1; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
        }
        #endregion
    }
}
