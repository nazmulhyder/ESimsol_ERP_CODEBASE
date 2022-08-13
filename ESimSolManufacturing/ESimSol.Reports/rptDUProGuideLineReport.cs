using System;
using System.Data;
using ESimSol.BusinessObjects;
using ICS.Core;
using ICS.Core.Utility;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;
using ICS.Core.Framework;
using System.Linq;
using System.Web;
namespace ESimSol.Reports
{
    public class rptDUProGuideLineReport
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyleBold;
          iTextSharp.text.Font _oFontStyle_UnLine;
        public iTextSharp.text.Image _oImag { get; set; }

        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        MemoryStream _oMemoryStream = new MemoryStream();
        Company _oCompany = new Company();
        BusinessUnit _oBusinessUnit = new BusinessUnit();
        DyeingOrder _oDyeingOrder = new DyeingOrder();
        List<LotParent> _oLotParents = new List<LotParent>();
        List<DUProGuideLine> _oDUProGuideLines = new List<DUProGuideLine>();
        List<DUProGuideLineDetail> _oDUProGuideLineDetails= new List<DUProGuideLineDetail>();
        List<DUProductionStatusReport> _oDUProductionStatusReports = new List<DUProductionStatusReport>();
        #endregion

        public byte[] PrepareReport(DyeingOrder oDyeingOrder,List<DUProGuideLine> oDUProGuideLines, List<DUProGuideLineDetail> oDUProGuideLineDetails,List<DUProductionStatusReport> oDUProductionStatusReports,List<LotParent> oLotParents, Company oCompany, BusinessUnit oBusinessUnit)
        {
            _oDyeingOrder = oDyeingOrder;
            _oLotParents = oLotParents;
            _oDUProGuideLines = oDUProGuideLines;
            _oDUProGuideLineDetails = oDUProGuideLineDetails;
            _oDUProductionStatusReports = oDUProductionStatusReports;
            
            _oCompany = oCompany;
            _oBusinessUnit = oBusinessUnit;
            float _npageHeight = 842f, _nPageWidth = 595f;

            #region Page Setup
            _oDocument = new Document(new iTextSharp.text.Rectangle(_nPageWidth, _npageHeight), 0f, 0f, 0f, 0f);
            _oDocument.SetMargins(10, 10, 10, 10);
            _oPdfPTable.SetWidths(new float[] { 
               _nPageWidth
            });

            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 11f, 1);

            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();

            #endregion

            ESimSolPdfHelper.PrintHeader_Baly(ref _oPdfPTable, _oBusinessUnit, _oCompany, "Order Statement", 1);

            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            //ESimSolPdfHelper.AddCell(ref _oPdfPTable, sDateRange, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0, 0, 14, 20);

            this.PrintBody();
            this.SetReceivingInfo();
            this.PrintLots();
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Bill
        private void PrintBody()
        {
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

            #region Header
            PdfPTable oPdfHeader_Table = new PdfPTable(4);
            oPdfHeader_Table.SetWidths(new float[] {100,150,100,145 });
            ESimSolPdfHelper.AddCell(ref oPdfHeader_Table, "Order No", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfHeader_Table, _oDyeingOrder.OrderNoFull, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfHeader_Table, "Order Date", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfHeader_Table, _oDyeingOrder.OrderDateSt, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);

            ESimSolPdfHelper.AddCell(ref oPdfHeader_Table, (_oDyeingOrder.DyeingOrderType==(int)EnumOrderType.LoanOrder)?"Supplier Name":"Buyer Name", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfHeader_Table, _oDyeingOrder.ContractorName, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15, 0, 3, 10);
            ESimSolPdfHelper.AddCell(ref oPdfHeader_Table, "MKT Person", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfHeader_Table, _oDyeingOrder.MKTPName, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15, 0, 3, 10);
            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfHeader_Table);

            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0, 0, 0, 15);
            #endregion

            #region Details
            string[] Headers = new string[] {"#", 
                                             "Product", 
                                             "Qty", 
                                             };
            int nCount_Production = 1;
            foreach (var oDUPGL in _oDUProGuideLines)
            {
                ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
               
                #region Data
                PdfPTable oPdfPTable = new PdfPTable(3);
                oPdfPTable.SetWidths(new float[] { 40, 310, 145 });

                ESimSolPdfHelper.AddCell(ref oPdfPTable, "SL-" + (nCount_Production++).ToString(), Element.ALIGN_CENTER, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 0);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, oDUPGL.ReceiveStore, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.LIGHT_GRAY, 0);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, "Date: " + oDUPGL.ReceiveDateST, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.LIGHT_GRAY, 0);

                ESimSolPdfHelper.PrintHeaders(ref oPdfPTable, Headers);

                int nCount = 1;
                ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                foreach (var oItem in _oDUProGuideLineDetails.Where(x => x.DUProGuideLineID == oDUPGL.DUProGuideLineID))
                {
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, (nCount++).ToString(), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, oItem.ProductName, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(oItem.Qty), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                }
                #endregion

                ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

                #region Sub Total
                ESimSolPdfHelper.AddCell(ref oPdfPTable, "Total", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15, 0, 2, 0);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(_oDUProGuideLineDetails.Where(x => x.DUProGuideLineID == oDUPGL.DUProGuideLineID).Sum(x => x.Qty)), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                #endregion

                ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable);
                ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0,0,0,15);
            }
            #endregion

            if (_oDUProductionStatusReports.Count != 0)
            {
                #region Summary Print
                PdfPTable oPdfPTable = new PdfPTable(9);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.SetWidths(new float[] { 25f, 160f, 58.5f, 58.5f, 58.5f, 58.5f, 58.5f, 58.5f, 58.5f });

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

                _oPdfPCell = new PdfPCell(new Phrase("Transfer In", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = _oPdfPCell.BorderWidthTop = _oPdfPCell.BorderWidthBottom = 1;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Transfer Out", _oFontStyle));
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

                _oPdfPCell = new PdfPCell(new Phrase("Due SW", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = _oPdfPCell.BorderWidthTop = _oPdfPCell.BorderWidthBottom = 1;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                #endregion

                #region
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
                int nCount_SM = 0;
                foreach (var oItem in _oDUProductionStatusReports)
                {
                        nCount_SM++;
                        _oPdfPCell = new PdfPCell(new Phrase(nCount_SM.ToString(), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthLeft = _oPdfPCell.BorderWidthRight = _oPdfPCell.BorderWidthBottom = 1;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName, _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        _oPdfPCell.BorderWidthTop = _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = _oPdfPCell.BorderWidthBottom = 1;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        double nQty_Order = _oDUProductionStatusReports.Where(x => x.ProductID == oItem.ProductID).Select(x => x.Qty_Order).FirstOrDefault();
                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty_Order), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        _oPdfPCell.BorderWidthTop = _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = _oPdfPCell.BorderWidthBottom = 1;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        double nQty = _oDUProductionStatusReports.Where(x => x.ProductID == oItem.ProductID).Sum(x => x.Qty_Rcv);
                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        _oPdfPCell.BorderWidthTop = _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = _oPdfPCell.BorderWidthBottom = 1;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oDUProductionStatusReports.Where(x => x.ProductID == oItem.ProductID).Sum(x => x.Qty_Transfer_In)), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        _oPdfPCell.BorderWidthTop = _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = _oPdfPCell.BorderWidthBottom = 1;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oDUProductionStatusReports.Where(x => x.ProductID == oItem.ProductID).Sum(x => x.Qty_Transfer_Out)), _oFontStyle));
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

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oDUProductionStatusReports.Sum(x => x.Qty_Rcv)), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.BorderWidthTop = _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = _oPdfPCell.BorderWidthBottom = 1;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oDUProductionStatusReports.Sum(x => x.Qty_Transfer_In)), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.BorderWidthTop = _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = _oPdfPCell.BorderWidthBottom = 1;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oDUProductionStatusReports.Sum(x => x.Qty_Transfer_Out)), _oFontStyle));
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
        }

        private void SetReceivingInfo()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLDITALIC);
            PdfPTable oPdfPTable = new PdfPTable(5);
            oPdfPTable.SetWidths(new float[] { 50f, 200f,80f, 150f, 60f});

            #region PO Info
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Receiving Info", 0, 5, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyle_UnLine);
            oPdfPTable.CompleteRow();
            int nCount_Production = 1;
            double nQty = 0;
            foreach (var oDUPGL in _oDUProGuideLines)
            {
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "SL-" + (nCount_Production++).ToString(), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oDUPGL.ReceiveStore, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Date: " + oDUPGL.ReceiveDateST, 0, 2, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                oPdfPTable.CompleteRow();
                int nCount = 1;
                foreach (var oItem in _oDUProGuideLineDetails.Where(x => x.DUProGuideLineID == oDUPGL.DUProGuideLineID))
                {
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "SL#", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Product ", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Unit", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Lot No", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Qty", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);

                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, (nCount++).ToString(), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.ProductName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.MUnit, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.LotNo, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.Qty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    oPdfPTable.CompleteRow();
                }
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total", 0, 4, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                nQty = _oDUProGuideLines.Where(x => x.DUProGuideLineID == oDUPGL.DUProGuideLineID).Select(c => c.Qty).Sum();
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nQty), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                oPdfPTable.CompleteRow();
            }

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Grand Total", 0, 4, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            nQty = _oDUProGuideLines.Select(c => c.Qty).Sum();
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nQty), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);


            #endregion
          

        }
        private void PrintLots()
        {
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE,0,0,0,10);

            #region Details
            string[] Headers = new string[] {"#SL", 
                                             "OrderNo", 
                                             "Order Date", 
                                             "Buyer Name", 
                                             "Order Qty", 
                                             "Rcv Qty", 
                                             "Balance"
                                             };
            int nCount = 1, nLotID = -99;
            PdfPTable oPdfPTable = new PdfPTable(7);
            oPdfPTable.SetWidths(new float[] { 40, 60, 60, 145, 60, 60, 60 });
            //ESimSolPdfHelper.PrintHeaders(ref oPdfPTable, Headers);

            foreach (var oLotParent in _oLotParents)
            {
                ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

                #region Data

                if (nLotID != oLotParent.LotID)
                {
                    ESimSolPdfHelper.AddCell(ref oPdfPTable,"Lot No: "+ oLotParent.LotNo + " [" + oLotParent.ProductName +"]", Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15, 0, 7, 0);
                    oPdfPTable.CompleteRow();

                    ESimSolPdfHelper.PrintHeaders(ref oPdfPTable, Headers);

                    if (nCount > 1) 
                    {
                        ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable);

                        nCount =1;
                        oPdfPTable = new PdfPTable(7);
                        oPdfPTable.SetWidths(new float[] { 40, 60, 60, 145, 60,60, 60 });
                        //ESimSolPdfHelper.PrintHeaders(ref oPdfPTable, Headers); ESimSolPdfHelper.PrintHeaders(ref oPdfPTable, Headers);
                    }
                }

                ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                
                ESimSolPdfHelper.AddCell(ref oPdfPTable, (nCount++).ToString(), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, oLotParent.DyeingOrderNo, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, oLotParent.OrderDateSt, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, oLotParent.ContractorName, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(oLotParent.Qty_Order), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(oLotParent.Qty), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(oLotParent.Balance), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
               
                #endregion

                nLotID = oLotParent.LotID;
            }

            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable);
            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0, 0, 0, 15);

            #endregion

        }
        #endregion
    }
}
