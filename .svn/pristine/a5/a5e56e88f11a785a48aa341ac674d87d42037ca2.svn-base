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
using ESimSol.Reports;

namespace ESimSol.BusinessObjects
{
    public class rptRouteSheetGrace
    {
        public rptRouteSheetGrace() 
        {
            DUDeliveryChallanDetails = new List<DUDeliveryChallanDetail>();
            DUReturnChallanDetails = new List<DUReturnChallanDetail>();
        }

        public List<DUReturnChallanDetail> DUReturnChallanDetails { get; set; }
        public List<DUDeliveryChallanDetail> DUDeliveryChallanDetails { get; set; }

        #region Declaration
        PdfWriter _oWriter;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();

        Company _oCompany = new Company();
        BusinessUnit _oBusinessUnit = new BusinessUnit();
        DyeingOrder _oDyeingOrder = new DyeingOrder();
        DyeingOrderDetail _oDyeingOrderDetail = new DyeingOrderDetail();
        List<DUOrderRS> _oDUOrderRSs = new List<DUOrderRS>();
        int _nColCount = 11;

        #endregion
        public byte[] PrepareReport(List<DUOrderRS> oDUOrderRSs, DyeingOrder oDyeingOrder, DyeingOrderDetail oDyeingOrderDetail, BusinessUnit oBusinessUnit, Company oCompany, string sHeader)
        {
            _oDUOrderRSs = oDUOrderRSs;
            _oDyeingOrder = oDyeingOrder;
            _oDyeingOrderDetail = oDyeingOrderDetail;

            _oCompany = oCompany;
            _oBusinessUnit = oBusinessUnit;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4); //595X842
            _oDocument.SetMargins(10f, 10f, 5f, 20f);

            _oPdfPTable = new PdfPTable(1);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _oWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PageEventHandler.PrintDocumentGenerator = true;
            PageEventHandler.PrintPrintingDateTime = true;
            _oWriter.PageEvent = PageEventHandler; //Footer print wiht page event handeler            
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] {595});
            #endregion

            #region Report Body & Header
            oCompany.Name = _oBusinessUnit.Name;
            
            ESimSolPdfHelper.PrintHeader_Baly(ref _oPdfPTable, oBusinessUnit, oCompany, sHeader, 0);
          
            this.PrintBody(1);
            this.PrintDeliveryChallan();
            _oPdfPTable.HeaderRows = 1;
            #endregion

            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        public PdfPTable GetDetailsTable() 
        {
            PdfPTable oPdfPTable = new PdfPTable(_nColCount);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[]{
                                                20f,
                                                60f,
                                                80f,
                                                95f,// BUYER --5
                                                45f, 

                                                45f,
                                                45f,
                                                45f,
                                                45f,

                                                45f,
                                                45f
            });
            return oPdfPTable;
        }
      
        #region Report Body
        private void PrintBody(int eReportLayout)
        {
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0,0,0,20);

            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

            PdfPTable oPdfPTable = GetDetailsTable();

            #region Column Header
            PdfPTable oPdfPTable_Head = new PdfPTable(4);
            oPdfPTable_Head.WidthPercentage = 100;
            oPdfPTable_Head.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable_Head.SetWidths(new float[]{
                                                80f,
                                                120f,
                                                95f,// BUYER --5
                                                220f
            });

            #region 1st Row
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            string[] excelCaptios = new string[] {  "Order No ",        " " + _oDyeingOrder.OrderNoFull,        "Buyer Name ",      " " +_oDyeingOrder.ContractorName, 
                                                    "Order Date ",      " " +_oDyeingOrder.OrderDateSt,         "Product Name ",    " " + _oDyeingOrderDetail.ProductName, 
                                                    "Color & Shade ",   " " + _oDyeingOrderDetail.ColorName + "["+  _oDyeingOrderDetail.ShadeSt +"]", 
                                                    "Order Qty ",       " " +Global.MillionFormat(_oDyeingOrderDetail.Qty)+" "+_oDyeingOrderDetail.MUnit};
            for (var i=0; i < excelCaptios.Length; i += 2)
            {
                ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                ESimSolPdfHelper.AddCell(ref oPdfPTable_Head, excelCaptios[i], Element.ALIGN_RIGHT, 15);

                ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                ESimSolPdfHelper.AddCell(ref oPdfPTable_Head, excelCaptios[i+1], Element.ALIGN_LEFT, 15);
            }
            #endregion

            oPdfPTable_Head.CompleteRow();
            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable_Head);

            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15, 0, 0, 10);

            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "#SL", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Batch No", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Batch Date", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Batch Status", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);

            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Raw Issue", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Packing Qty", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Recycle Qty", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Westage Qty", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);

            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Process Gain/Loss", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Delivery Qty", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Return Qty", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
           
            _oPdfPTable.CompleteRow();
            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable);
            #endregion

            string sNumberFormate = "#,##0.00;(#,##0.00)";

            #region Date Wise Loop

            int nCount = 0;
            double nQty_GainLoss = 0, nQty_GainLoss_Total = 0;
            foreach (var obj in _oDUOrderRSs)
            {
                #region DATA
                ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                oPdfPTable = new PdfPTable(10);
                oPdfPTable = GetDetailsTable();

                ESimSolPdfHelper.AddCell(ref oPdfPTable, (++nCount).ToString(), Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.RouteSheetNo, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.RSDateSt, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.RSStateSt, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
             
                ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(obj.Qty), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(obj.PackingQty), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(obj.RecycleQty), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(obj.WastageQty), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);

                //ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(obj.FinishQty), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                //ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(obj.QtyDC), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                //ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(obj.BalanceQty), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);

                nQty_GainLoss = (obj.FinishQty  > 0 ) ? (obj.Qty - obj.FinishQty) : 0;
                nQty_GainLoss_Total += nQty_GainLoss;

                ESimSolPdfHelper.AddCell(ref oPdfPTable, (nQty_GainLoss).ToString(sNumberFormate), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);

                ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(obj.QtyDC), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(obj.QtyRC), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15); oPdfPTable.CompleteRow();
                ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable);
                #endregion
            }

            #region Grand Total (Layout Wise)
            oPdfPTable = new PdfPTable(11);
            oPdfPTable = GetDetailsTable();
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, " Grand Total ", Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15, 0, 4, 0);

            ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(_oDUOrderRSs.Sum(x => x.Qty)), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(_oDUOrderRSs.Sum(x => x.PackingQty)), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(_oDUOrderRSs.Sum(x => x.RecycleQty)), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(_oDUOrderRSs.Sum(x => x.WastageQty)), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, nQty_GainLoss_Total.ToString(sNumberFormate), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
           
            ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(_oDUOrderRSs.Sum(x => x.QtyDC)), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(_oDUOrderRSs.Sum(x => x.QtyRC)), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
           
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            oPdfPTable.CompleteRow();
            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable);
            #endregion

            #endregion
        }
        #endregion

        private void PrintDeliveryChallan()
        {
            iTextSharp.text.Font _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

            string _sCurrency = "", _sMUName = "";
            double _nTotalAmount = 0, _nTotalQty = 0, _nGrandTotalAmount = 0, _nGrandTotalQty = 0, _nTotalYetToDC = 0, _nTotalDC = 0, _nGrandTotalDC = 0, _nGrandTotalYetToDC = 0, _nCount = 0;

            #region Delivery challan

            PdfPTable oPdfPTable = new PdfPTable(8);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 100f, 75f, 130f, 60, 60f, 75f, 70f, 70f });
            int nProductID = 0;

            _nTotalAmount = 0;
            _nTotalQty = 0;
            _nGrandTotalAmount = 0;
            _nGrandTotalQty = 0;
            nProductID = 0;
            if (DUDeliveryChallanDetails.Count > 0)
            {
                oPdfPCell = new PdfPCell(new Phrase("Delivery Challan Info", FontFactory.GetFont("Tahoma", 10f, 3)));
                oPdfPCell.Colspan = 8;

                oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                oPdfPCell.BackgroundColor = BaseColor.GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();
                DUDeliveryChallanDetails = DUDeliveryChallanDetails.OrderBy(o => o.ProductID).ToList();
                foreach (DUDeliveryChallanDetail oDCDetail in DUDeliveryChallanDetails)
                {
                    //oDyeingOrderDetail.ProductID = 1;
                    //oDyeingOrderDetail.ProductNameCode = "100 ";
                    if (nProductID != oDCDetail.ProductID)
                    {
                        #region Header

                        if (nProductID > 0)
                        {
                            #region Total
                            _oPdfPCell = new PdfPCell(new Phrase("Total: ", _oFontStyleBold));
                            _oPdfPCell.Colspan = 5;
                            //_oPdfPCell.Border = 0;
                            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            oPdfPTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalQty), _oFontStyleBold));
                            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            //_oPdfPCell.Border = 0;
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            oPdfPTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            //_oPdfPCell.Border = 0;
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            oPdfPTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase(_sCurrency + "" + Global.MillionFormat(_nTotalAmount), _oFontStyleBold));
                            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            //_oPdfPCell.Border = 0;
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            oPdfPTable.AddCell(_oPdfPCell);



                            oPdfPTable.CompleteRow();
                            #endregion
                        }

                        oPdfPCell = new PdfPCell(new Phrase("Yarn Type: " + oDCDetail.ProductName, _oFontStyleBold));
                        oPdfPCell.Colspan = 8;
                        oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPTable.AddCell(oPdfPCell);
                        oPdfPTable.CompleteRow();

                        oPdfPCell = new PdfPCell(new Phrase("Date", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase("Challan No", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase("COLOR", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase("Lot", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase("SHADE", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase(" Qty (" + _sMUName + ")", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase("Unit Price(" + _sCurrency + ")", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);


                        oPdfPCell = new PdfPCell(new Phrase("Amount(" + _sCurrency + ")", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);



                        oPdfPTable.CompleteRow();
                        #endregion
                        _nTotalQty = 0;
                        _nTotalAmount = 0;
                        _nCount = 0;
                    }
                    #region PrintDetail
                    //List<DyeingOrderDetail> oDyeingOrderDetails = new List<DyeingOrderDetail>();
                    //_oDyeingOrderDetails = _oDyeingOrderDetails.Where(o => o.ProductID == oChallan.ProductID).ToList();

                    //for (int i = 0; i < oDyeingOrderDetails.Count; i++)
                    //{
                    _nCount++;
                    oPdfPCell = new PdfPCell(new Phrase(oDCDetail.ChallanDate, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("DC " + oDCDetail.ChallanNo, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oDCDetail.ColorName, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oDCDetail.LotNo, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(((EnumShade)oDCDetail.Shade).ToString(), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(oPdfPCell);



                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oDCDetail.Qty), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oDCDetail.UnitPrice), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oDCDetail.Qty * oDCDetail.UnitPrice), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPTable.CompleteRow();


                    _nTotalAmount = _nTotalAmount + (oDCDetail.Qty * oDCDetail.UnitPrice);
                    _nTotalQty = _nTotalQty + oDCDetail.Qty;
                    _nGrandTotalAmount = _nGrandTotalAmount + (oDCDetail.Qty * oDCDetail.UnitPrice);
                    _nGrandTotalQty = _nGrandTotalQty + oDCDetail.Qty;

                    #endregion

                    nProductID = oDCDetail.ProductID;


                    #region Return Challan

                    List<DUReturnChallanDetail> oDUReturnChallanDetails = new List<DUReturnChallanDetail>();
                    oDUReturnChallanDetails = DUReturnChallanDetails.Where(o => o.DUDeliveryChallanDetailID == oDCDetail.DUDeliveryChallanDetailID).ToList();
                    if (oDUReturnChallanDetails.Count > 0)
                    {
                        for (int j = 0; j < oDUReturnChallanDetails.Count; j++)
                        {
                            _nCount++;


                            oPdfPCell = new PdfPCell(new Phrase(oDUReturnChallanDetails[j].RCDate, _oFontStyle));
                            oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                            oPdfPTable.AddCell(oPdfPCell);

                            oPdfPCell = new PdfPCell(new Phrase("RC " + oDUReturnChallanDetails[j].RCNo, _oFontStyle));
                            oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                            oPdfPTable.AddCell(oPdfPCell);

                            oPdfPCell = new PdfPCell(new Phrase(oDCDetail.ColorName, _oFontStyle));
                            oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                            oPdfPTable.AddCell(oPdfPCell);

                            oPdfPCell = new PdfPCell(new Phrase(oDUReturnChallanDetails[j].LotNo, _oFontStyle));
                            oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                            oPdfPTable.AddCell(oPdfPCell);

                            oPdfPCell = new PdfPCell(new Phrase(((EnumShade)oDCDetail.Shade).ToString(), _oFontStyle));
                            oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                            oPdfPTable.AddCell(oPdfPCell);

                            oPdfPCell = new PdfPCell(new Phrase("(" + Global.MillionFormat(oDUReturnChallanDetails[j].Qty) + ")", _oFontStyle));
                            oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            oPdfPTable.AddCell(oPdfPCell);

                            oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oDCDetail.UnitPrice), _oFontStyle));
                            oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            oPdfPTable.AddCell(oPdfPCell);

                            oPdfPCell = new PdfPCell(new Phrase("(" + Global.MillionFormat(oDUReturnChallanDetails[j].Qty * oDCDetail.UnitPrice) + ")", _oFontStyle));
                            oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            oPdfPTable.AddCell(oPdfPCell);

                            oPdfPTable.CompleteRow();
                            _nTotalAmount = _nTotalAmount - (oDUReturnChallanDetails[j].Qty * oDCDetail.UnitPrice);
                            _nTotalQty = _nTotalQty - oDUReturnChallanDetails[j].Qty;
                            _nGrandTotalAmount = _nGrandTotalAmount - (oDUReturnChallanDetails[j].Qty * oDCDetail.UnitPrice);
                            _nGrandTotalQty = _nGrandTotalQty - oDUReturnChallanDetails[j].Qty;
                        }
                    }
                    #endregion Return Challan

                }

                #region Total
                _oPdfPCell = new PdfPCell(new Phrase("Total: ", _oFontStyleBold));
                _oPdfPCell.Colspan = 5;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalQty), _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_sCurrency + "" + Global.MillionFormat(_nTotalAmount), _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);



                oPdfPTable.CompleteRow();
                #endregion

                #region Grand Total
                _oPdfPCell = new PdfPCell(new Phrase("Grand Total: ", _oFontStyleBold));
                _oPdfPCell.Colspan = 5;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nGrandTotalQty) + "" + _sMUName, _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);



                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_sCurrency + "" + Global.MillionFormat(_nGrandTotalAmount), _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();
                #endregion
            }

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion Delivery challan
        }
    }
}
