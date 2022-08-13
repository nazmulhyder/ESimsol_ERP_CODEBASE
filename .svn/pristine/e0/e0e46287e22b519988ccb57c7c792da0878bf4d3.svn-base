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
    public class rptWYRequisition
    {
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
        WYRequisition _oWYRequisition = new WYRequisition();
        List<FabricExecutionOrderYarnReceive> _oFabricExecutionOrderYarnReceives = new List<FabricExecutionOrderYarnReceive>();
        List<DUProductionStatusReport> _oDUProductionStatusReports=new List<DUProductionStatusReport>();
        List<DyeingOrderFabricDetail> _oDyeingOrderFabricDetails = new List<DyeingOrderFabricDetail>();
        List<FabricExecutionOrderYarnReceive> _oFEOYRs = new List<FabricExecutionOrderYarnReceive>();
        Company _oCompany = new Company();
        BusinessUnit _oBusinessUnit = new BusinessUnit();

        double _nTotalQty = 0;
        string _sMUnitName = "";
        #endregion
        public byte[] PrepareReport(WYRequisition oWYRequisition, Company oCompany, BusinessUnit oBusinessUnit, List<DyeingOrderFabricDetail> oDyeingOrderFabricDetails, List<FabricExecutionOrderYarnReceive> oFabricExecutionOrderYarnReceives)
        {
            _oCompany = oCompany;
            _oWYRequisition = oWYRequisition;
            _oBusinessUnit = oBusinessUnit;
            _oFabricExecutionOrderYarnReceives = oWYRequisition.FEOYSList;

            _oDyeingOrderFabricDetails = oDyeingOrderFabricDetails;
            _oFEOYRs = oFabricExecutionOrderYarnReceives;

            if (_oFabricExecutionOrderYarnReceives.Count > 0)
            {
                _sMUnitName = _oFabricExecutionOrderYarnReceives[0].UnitName;
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

            string sHeader = "Store Requisition Slip (SRS)";            if(_oWYRequisition.RequisitionType == EnumInOutType.Disburse)                 sHeader = "Store Return Memo (SRM)";
            _oPdfPCell = new PdfPCell(new Phrase(sHeader, _oFontStyle));
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
            if (_oWYRequisition.ReceivedBy == 0)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.ITALIC, BaseColor.RED);
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

            #region Report Vocher No, WYRequisition Date & Currency
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);

            _oPdfPCell = new PdfPCell(new Phrase(_oWYRequisition.RequisitionTypeSt + " No : " + _oWYRequisition.RequisitionNo, _oFontStyle)); _oPdfPCell.Colspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Req. Date : " + _oWYRequisition.IssueDate.ToString("dd MMM yy HH:mm:tt"), _oFontStyle)); _oPdfPCell.Colspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            string sBuyerNames = string.Join(", ", _oFabricExecutionOrderYarnReceives.Select(x => x.BuyerName).Distinct().ToList());
            _oPdfPCell = new PdfPCell(new Phrase("Buyer's Name : " + sBuyerNames, _oFontStyle)); _oPdfPCell.Colspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Receive Date : " + _oWYRequisition.ReceiveDateTimeSt, _oFontStyle)); _oPdfPCell.Colspan = 2; 
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase(((_oWYRequisition.RequisitionType == EnumInOutType.Receive) ? "Issue" : "Receive") + " Store : " + _oWYRequisition.IssueStoreName, _oFontStyle)); _oPdfPCell.Colspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(((_oWYRequisition.RequisitionType == EnumInOutType.Receive) ? "Receive" : "Issue") + " Store : " + _oWYRequisition.ReceiveStoreName, _oFontStyle)); _oPdfPCell.Colspan = 2;
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

            #region WYRequisition Details
            oPdfPTable = new PdfPTable(1);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 455.5f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            _oFontStyleTwo = FontFactory.GetFont("Tahoma", 8f, 1);

            _oPdfPCell = new PdfPCell(new Phrase("Requisition Details Description", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Header
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
            #endregion

            #region Data
            int nCount = 0;
            int nRawCount = 0;
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, 0);
            _oFontStyleForCC = FontFactory.GetFont("Tahoma", 8f, 0);
            foreach (FabricExecutionOrderYarnReceive oItem in _oFabricExecutionOrderYarnReceives)
            {
                oPdfPTable = new PdfPTable(7);
                oPdfPTable = GetDetails_Table();

                nCount++;
                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.DispoNo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.OrderQty), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.LotNo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.ReceiveQty), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.UnitName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Remarks, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 1; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                
                oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _nTotalQty = _nTotalQty + oItem.ReceiveQty;

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
            #endregion
            nRawCount = _oPdfPTable.Rows.Count;
            #region For Print blank raw if (30-raw count)>0
            for (int i = 0; i <= (20 - nRawCount); i++)
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

            #region Total WYRequisition Amount
            oPdfPTable = new PdfPTable(4);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 411.7f, 48f, 37.5f, 77.5f });
            //oPdfPTable.SetWidths(new float[] { 30.5f, 75f, 245.2f, 70.5f, 60f, 37.5f, 77.5f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("   Total", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("              " + Global.MillionFormat(_nTotalQty) + "   " + _sMUnitName, _oFontStyle)); _oPdfPCell.Colspan = 2;
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



            #region Warping Information
            if (_oDyeingOrderFabricDetails.Count > 0)
            {
                ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
                ESimSolPdfHelper.AddCell(ref _oPdfPTable, "Warping Information", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 0, 0, 25);
                this.PrintWarpingInfo();
            } 
            #endregion





            #region Blank Space
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            if (_oDUProductionStatusReports.Count != 0)
            {
                #region Summary Print
                oPdfPTable = new PdfPTable(7);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.SetWidths(new float[] { 25f, 218.2f, 70.6f, 70.6f, 70.6f, 70.6f, 70.6f });

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
                
                _oPdfPCell = new PdfPCell(new Phrase("Order Qty", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = _oPdfPCell.BorderWidthTop = _oPdfPCell.BorderWidthBottom = 1;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                
                _oPdfPCell = new PdfPCell(new Phrase("Rcv Qty", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = _oPdfPCell.BorderWidthTop = _oPdfPCell.BorderWidthBottom = 1;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                
                _oPdfPCell = new PdfPCell(new Phrase("Due Rcv", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = _oPdfPCell.BorderWidthTop = _oPdfPCell.BorderWidthBottom = 1;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                
                _oPdfPCell = new PdfPCell(new Phrase("SW Qty", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = _oPdfPCell.BorderWidthTop = _oPdfPCell.BorderWidthBottom = 1;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                
                _oPdfPCell = new PdfPCell(new Phrase("Yet To SW", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = _oPdfPCell.BorderWidthTop = _oPdfPCell.BorderWidthBottom = 1;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                
                #endregion

                #region
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
                int nProductID = nCount = 0;
                foreach (var oItem in _oDUProductionStatusReports)
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

                        double nQty_Order = _oDUProductionStatusReports.Where(x => x.ProductID == oItem.ProductID).Select(x => x.Qty_Order).FirstOrDefault();
                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat( nQty_Order), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        _oPdfPCell.BorderWidthTop = _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = _oPdfPCell.BorderWidthBottom = 1;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        double nQty = _oDUProductionStatusReports.Where(x => x.ProductID == oItem.ProductID).Sum(x => x.Qty_Rcv - x.Qty_Return);
                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        _oPdfPCell.BorderWidthTop = _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = _oPdfPCell.BorderWidthBottom = 1;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        double nQty_YetRC = _oDUProductionStatusReports.Where(x => x.ProductID == oItem.ProductID).Sum(x => x.Qty_YetToRcv);
                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_YetRC), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        _oPdfPCell.BorderWidthTop = _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = _oPdfPCell.BorderWidthBottom = 1;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        double nQty_SW = _oDUProductionStatusReports.Where(x => x.ProductID == oItem.ProductID).Sum(x => x.Qty_SW);
                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_SW), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        _oPdfPCell.BorderWidthTop = _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = _oPdfPCell.BorderWidthBottom = 1;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        double nQty_YetSW = _oDUProductionStatusReports.Where(x => x.ProductID == oItem.ProductID).Sum(x => x.Qty_YetToSW);
                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_YetSW), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        _oPdfPCell.BorderWidthTop = _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = _oPdfPCell.BorderWidthBottom = 1;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        
                    }
                    nProductID = oItem.ProductID;
                }

                #region

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);

                _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Colspan = 2;
                _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = _oPdfPCell.BorderWidthBottom = 1;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oDUProductionStatusReports.Sum(x => x.Qty_Order)), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.BorderWidthTop = _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = _oPdfPCell.BorderWidthBottom = 1;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oDUProductionStatusReports.Sum(x => x.Qty_Rcv-x.Qty_Return)), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.BorderWidthTop = _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = _oPdfPCell.BorderWidthBottom = 1;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oDUProductionStatusReports.Sum(x => x.Qty_YetToRcv)), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.BorderWidthTop = _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = _oPdfPCell.BorderWidthBottom = 1;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oDUProductionStatusReports.Sum(x => x.Qty_SW)), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.BorderWidthTop = _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = _oPdfPCell.BorderWidthBottom = 1;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oDUProductionStatusReports.Sum(x => x.Qty_YetToSW)), _oFontStyle));
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

            _oPdfPCell = new PdfPCell(new Phrase("__________________ \n" + " Requisition By"+"\n"+_oWYRequisition.RequisitionByName , _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("__________________  \n Approved By \n" + _oWYRequisition.ApprovedByName , _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("__________________  \n Issued By \n" + _oWYRequisition.DisburseByName, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("__________________ \n "+ ((_oWYRequisition.RequisitionType == EnumInOutType.Disburse)? "Returned" : "Received") +" By \n" + _oWYRequisition.ReceivedByName, _oFontStyle));
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
        #endregion

        public PdfPTable GetWarpingInfoTable()
        {
            PdfPTable oPdfPTable = new PdfPTable(11);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            oPdfPTable.SetWidths(new float[]{
                                                25f,//sl
                                                120f,//yarn
                                                70f,//color
                                                50f,//order qty
                                                50f,//warp/weft
                                                65f,//lot no
                                                60f,
                                                45f,//type
                                                50f,//in qty
                                                50f,//out qty
                                                55f//balance
                                               
            });
            return oPdfPTable;
        }

        private void PrintWarpingInfo()
        {
            #region Column Header
            PdfPTable oPdfPTable = GetWarpingInfoTable();
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
            string[] Headers = new string[] { "#SL", "Yarn", "Color", "Order Qty", "Warp/Weft", "Lot No", "Receive Date", "Type", "In Qty", "Out Qty", "Balance" };
            ESimSolPdfHelper.PrintHeaders(ref oPdfPTable, Headers);
            oPdfPTable.CompleteRow();
            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable);
            #endregion

            double nBalance = 0;

            #region Data List Wise Loop

            _oDyeingOrderFabricDetails = _oDyeingOrderFabricDetails.OrderBy(x => x.SLNo).ToList();

            var data = _oDyeingOrderFabricDetails.GroupBy(x => new { x.FEOSID, x.ExeNo }, (key, grp) => new
            {
                FEOSID = key.FEOSID,
                ExeNo = key.ExeNo,
                Results = grp.ToList()
            });

            foreach (var oData in data)
            {
                oPdfPTable = GetWarpingInfoTable();
                ESimSolPdfHelper.AddCell(ref oPdfPTable, "Dispo No: "+oData.ExeNo.ToString(), Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15, 0, oPdfPTable.NumberOfColumns, 0);
                oPdfPTable.CompleteRow();
                foreach (var obj in oData.Results)
                {
                    ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
                    List<FabricExecutionOrderYarnReceive> oTempFabricExecutionOrderYarnReceives = new List<FabricExecutionOrderYarnReceive>();
                    oTempFabricExecutionOrderYarnReceives = _oFEOYRs.Where(x => x.FEOSDID == obj.FEOSDID && x.WarpWeftType == obj.WarpWeftType).ToList();
                    int nRowSpan = oTempFabricExecutionOrderYarnReceives.Count();
                    if (nRowSpan <= 0) nRowSpan = 1;

                    #region DATA
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.SLNo.ToString(), Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15, nRowSpan, 0, 0);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.ProductName, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15, nRowSpan, 0, 0);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.ColorName, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15, nRowSpan, 0, 0);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(obj.QtyDyed), Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15, nRowSpan, 0, 0);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.WarpWeftTypeSt, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15, nRowSpan, 0, 0);

                    if (oTempFabricExecutionOrderYarnReceives.Count > 0)
                    {
                        int nCount = 0;
                        foreach (FabricExecutionOrderYarnReceive oItem in oTempFabricExecutionOrderYarnReceives)
                        {
                            //ESimSolPdfHelper.AddCell(ref oPdfPTable, oItem.WarpWeftTypeSt, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
                            ESimSolPdfHelper.AddCell(ref oPdfPTable, oItem.LotNo, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
                            ESimSolPdfHelper.AddCell(ref oPdfPTable, oItem.ReceiveDateInStr, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
                            ESimSolPdfHelper.AddCell(ref oPdfPTable, oItem.WYarnTypeStr, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
                            ESimSolPdfHelper.AddCell(ref oPdfPTable, (oItem.RequisitionType == EnumInOutType.Disburse) ? Global.MillionFormat(oItem.ReceiveQty) : "", Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
                            ESimSolPdfHelper.AddCell(ref oPdfPTable, (oItem.RequisitionType == EnumInOutType.Receive) ? Global.MillionFormat(oItem.ReceiveQty) : "", Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
                            if (nCount == 0)
                            {
                                nBalance= oTempFabricExecutionOrderYarnReceives.Where(x => x.RequisitionType == EnumInOutType.Receive).Sum(y => y.ReceiveQty);
                                nBalance = nBalance-oTempFabricExecutionOrderYarnReceives.Where(x => x.RequisitionType == EnumInOutType.Disburse).Sum(y => y.ReceiveQty);

                                nBalance = obj.QtyDyed - nBalance;
                                if (nBalance < 0) { nBalance = 0; }

                                ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(nBalance), Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15, nRowSpan, 0, 0);
                            }
                            oPdfPTable.CompleteRow();
                            nCount++;
                        }
                    }
                    else
                    {
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, " ", Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, " ", Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, " ", Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, " ", Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, " ", Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(obj.QtyDyed), Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
                        //ESimSolPdfHelper.AddCell(ref oPdfPTable, " ", Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
                        oPdfPTable.CompleteRow();
                    }

                    #endregion
                }
                ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable);
            }
            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 0, 0, 0, 20);
            
            #endregion
        }

    }
}
