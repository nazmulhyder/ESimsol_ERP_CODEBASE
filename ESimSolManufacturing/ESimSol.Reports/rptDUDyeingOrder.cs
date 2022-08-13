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
    public class rptDUDyeingOrder
    {
        #region Declaration
        int _nTotalColumn = 1;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyle_UnLine;
        iTextSharp.text.Font _oFontStyleBold;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        ExportSCDO _oExportSCDO = new ExportSCDO();
        List<DyeingOrderDetail> _oDyeingOrderDetails = new List<DyeingOrderDetail>();
      
  

        List<DUDeliveryChallan> _oDUDeliveryChallans = new List<DUDeliveryChallan>();
        List<DUDeliveryChallanDetail> _oDUDeliveryChallanDetails = new List<DUDeliveryChallanDetail>();
        List<DUDeliveryOrderDetail> _oDUDeliveryOrderDetails = new List<DUDeliveryOrderDetail>();
        //List<ReturnChallanDetail> _oDUReturnChallanDetails = new List<ReturnChallanDetail>();

        BusinessUnit _oBusinessUnit = new BusinessUnit();
        DyeingOrder _oDyeingOrder = new DyeingOrder();
        Company _oCompany = new Company();
        string _sMessage = "";

        int _nCount = 0;
        double _nTotalClaim = 0;
        double _nTotalAmount = 0;
        double _nTotalQty = 0;
        double _nTotalAdjQty = 0;
        double _nTotalAdjValue = 0;
        double _nTotalNetDyeingQty = 0;
        double _nTotalActualValue = 0;
        double _nGrandTotalAmount = 0;
        double _nGrandTotalQty = 0;

        double _nTotalPOQty = 0;
        double _nTotalDOQty = 0;
        double _nTotalYetToDO = 0;
        double _nTotalYetToDC = 0;
        double _nGrandTotalYetToDC = 0;
        double _nTotalQty_RC = 0;
        double _nTotalDC = 0;
        double _nGrandTotalDC = 0;
        #endregion

        public byte[] PrepareReport(DyeingOrder oDyeingOrder, ExportSCDO oExportSCDO, Company oCompany,BusinessUnit oBusinessUnit)
        {
            _oDyeingOrder = oDyeingOrder;
            _oExportSCDO = oExportSCDO;
            _oDyeingOrderDetails = oExportSCDO.DyeingOrderDetails;
           

            _oDUDeliveryChallans = oExportSCDO.DUDeliveryChallans;
            _oDUDeliveryChallanDetails = oExportSCDO.DUDeliveryChallanDetails;
            _oDUDeliveryOrderDetails = oExportSCDO.DUDeliveryOrderDetails;
            //_oDUReturnChallanDetails = oLCDeliveryReport.ReturnChallanDetails;

            _oCompany = oCompany;
            _oBusinessUnit = oBusinessUnit;
            #region Page Setup
            _oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
            _oDocument.SetMargins(10f, 10f, 20f,20f);
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
            this.PrintHead_Sample();
            //this.PrintBalance();
            //this.PrintBody();
            this.SetExportSC();
           
            this.SetDyeingOrder();
            this.SetDeliveryOrder();
            this.SetDeliveryChallan();
            this.SetClaimOrder();
            this.SetDeliveryOrder_Claim();
            this.SetDeliveryChallan_Claim();
            //_oPdfPTable.HeaderRows = 1;
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
            _oFontStyle = FontFactory.GetFont("Tahoma", 18f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Name, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));

            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.PringReportHead, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
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
        #endregion
        private void ReporttHeader()
        {

            #region Proforma Invoice Heading Print

            _oPdfPCell = new PdfPCell(new Phrase("Order Management", FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD)));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.FixedHeight = 20; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            #endregion
        }
    
        private void PrintHead_Sample()
        {
            
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.UNDERLINE);

            PdfPTable oPdfPTable = new PdfPTable(3);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 100, 140f, 100f });

            oPdfPCell = new PdfPCell(new Phrase("DATE:", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oDyeingOrder.OrderDateSt, _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.Colspan = 2;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase(_oDyeingOrder.OrderType +" NO", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oDyeingOrder.OrderNoFull, FontFactory.GetFont("Tahoma", 11f, 1)));
            //oPdfPCell.Border = 0;
            oPdfPCell.Colspan = 2;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("BUYER NAME", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oDyeingOrder.ContractorName, FontFactory.GetFont("Tahoma", 11f, 1)));
            //oPdfPCell.Border = 0;
            oPdfPCell.Colspan = 2;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("REQUIREMENT DATE", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            if (_oDyeingOrderDetails.Count > 0)
            {
                oPdfPCell = new PdfPCell(new Phrase(_oDyeingOrderDetails[0].DeliveryDateSt, _oFontStyle));
            }
            else
            {
                oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            }

            //oPdfPCell.Border = 0;
            oPdfPCell.Colspan = 2;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.ShortName+" CONCERN PERSON", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oDyeingOrder.MKTPName, _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.Colspan = 2;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("BUYER CONCERN PERSON", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);



            oPdfPCell = new PdfPCell(new Phrase(" " + _oDyeingOrder.ContactPersonnelName, _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.Colspan = 2;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("MODE OF PAYMENT", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            string sTemp = "";
            string sTempTwo = "";
            if (_oDyeingOrder.PaymentType == (int)EnumOrderPaymentType.CashOrCheque)
            {
                sTemp = "Cheque or Cash";
                if (!string.IsNullOrEmpty(_oDyeingOrder.ExportPINo))
                {
                    sTempTwo = "MR No:" + _oDyeingOrder.ExportPINo;
                }
            }
            if (_oDyeingOrder.PaymentType == (int)EnumOrderPaymentType.AdjWithNextLC)
            {
                sTemp = "NEXT L/C ADJUST ";

                if (!string.IsNullOrEmpty(_oDyeingOrder.ExportPINo))
                {
                    sTempTwo = "P/I No:" + _oDyeingOrder.ExportPINo;
                }
            }
            if (_oDyeingOrder.PaymentType == (int)EnumOrderPaymentType.AdjWithPI)
            {
                sTemp = "Adj With PI ";
                if (!string.IsNullOrEmpty(_oDyeingOrder.ExportPINo))
                {
                    sTempTwo = "P/I No:" + _oDyeingOrder.ExportPINo;
                }
            }
            if (_oDyeingOrder.PaymentType == (int)EnumOrderPaymentType.FoC)
            {
                sTemp = "Free Of Cost";
                if (!string.IsNullOrEmpty(_oDyeingOrder.ExportPINo))
                {
                    sTempTwo = "MR No:" + _oDyeingOrder.ExportPINo;
                }
            }

            oPdfPCell = new PdfPCell(new Phrase(" " + sTemp + " " + sTempTwo, _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.Colspan = 2;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
          
        }
        //private void PrintBalance()
        //{
        //    //_oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, 1);
        //    PdfPTable oPdfPTable = new PdfPTable(8);
        //    PdfPCell oPdfPCell;
        //    oPdfPTable.SetWidths(new float[] { 60f, 88f, 60f, 86f, 60f, 86f, 60f, 86f });



        //    oPdfPCell = new PdfPCell(new Phrase("PI vs PO:", _oFontStyle));
        //    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
        //    oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //    oPdfPTable.AddCell(oPdfPCell);

        //    oPdfPCell = new PdfPCell(new Phrase(" " + _oExportSCDO.YetToDO + " " + _oExportSCDO.MUName, _oFontStyleBold));
        //    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
        //    //oPdfPCell.Colspan = 5;
        //    oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
        //    oPdfPTable.AddCell(oPdfPCell);

        //    oPdfPCell = new PdfPCell(new Phrase("PI vs DO:", _oFontStyle));
        //    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
        //    oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //    oPdfPTable.AddCell(oPdfPCell);

        //    oPdfPCell = new PdfPCell(new Phrase(" " + Global.MillionFormat((_oExportSCDO.TotalQty - _oExportSCDO.Qty_DO)) + " " + _oExportSCDO.MUName, _oFontStyleBold));
        //    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
        //    //oPdfPCell.Colspan = 5;
        //    oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
        //    oPdfPTable.AddCell(oPdfPCell);

        //    oPdfPCell = new PdfPCell(new Phrase("PI vs DC:", _oFontStyle));
        //    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
        //    oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //    oPdfPTable.AddCell(oPdfPCell);

        //    oPdfPCell = new PdfPCell(new Phrase(" " + Global.MillionFormat((_oExportSCDO.TotalQty + _oExportSCDO.Qty_Claim - _oExportSCDO.Qty_DC)) + " " + _oExportSCDO.MUName, _oFontStyleBold));
        //    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
        //    oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
        //    oPdfPTable.AddCell(oPdfPCell);

        //    oPdfPCell = new PdfPCell(new Phrase("DO vs DC:", _oFontStyle));
        //    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
        //    oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //    oPdfPTable.AddCell(oPdfPCell);

        //    oPdfPCell = new PdfPCell(new Phrase(" " + Global.MillionFormat(_oExportSCDO.Qty_DO - _oExportSCDO.Qty_DC) + " " + _oExportSCDO.MUName, _oFontStyleBold));
        //    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
        //    oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
        //    oPdfPTable.AddCell(oPdfPCell);

        //    oPdfPTable.CompleteRow();

        //    _oPdfPCell = new PdfPCell(oPdfPTable);
        //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //    _oPdfPTable.AddCell(_oPdfPCell);
        //    _oPdfPTable.CompleteRow();
        //}
     
        private void SetExportSC()
        {
            double nQty_DC = 0;
            double nQty_Claim = 0;
            _nTotalAmount = 0;
            _nTotalQty = 0;
            int nProductID = 0;
            //OrderBy(o => o.ProductID).ToList();
            _oDyeingOrderDetails = _oDyeingOrderDetails.OrderBy(o => o.ProductID).ToList();

            PdfPTable oPdfPTable = new PdfPTable(10);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] {25f,  50f, 40f, 50f, 50f, 50f, 50f, 50f, 50f, 50f });
            if (_oDyeingOrderDetails.Count > 0)
            {

                oPdfPCell = new PdfPCell(new Phrase("Yarn Type Detail", FontFactory.GetFont("Tahoma", 10f, 3)));
                oPdfPCell.Colspan = 12;
                oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();

                oPdfPCell = new PdfPCell(new Phrase("SL#", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);

                //oPdfPCell = new PdfPCell(new Phrase("Code", _oFontStyleBold));
                //oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                //oPdfPTable.AddCell(oPdfPCell);

                //oPdfPCell = new PdfPCell(new Phrase("Product Name", _oFontStyleBold));
                //oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                //oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(" Qty(" + _oExportSCDO.MUName + ")", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(" U/P(" + _oExportSCDO.Currency + ")", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Amount(" + _oExportSCDO.Currency + ")", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Claim Qty", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);
              

                //oPdfPCell = new PdfPCell(new Phrase("BPO Qty", _oFontStyleBold));
                //oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                //oPdfPTable.AddCell(oPdfPCell);
                //oPdfPCell = new PdfPCell(new Phrase("YetTo(" + _oExportSCDO.MUName + ")", _oFontStyleBold));
                //oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                //oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("DO Qty", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPCell = new PdfPCell(new Phrase("YetTo(" + _oExportSCDO.MUName + ")", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("DC Qty", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPCell = new PdfPCell(new Phrase("YetTo(" + _oExportSCDO.MUName + ")", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("RC(" + _oExportSCDO.MUName + ")", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                _nCount = 0;

                foreach (DyeingOrderDetail oItem in _oDyeingOrderDetails)
                {
                    #region PrintDetail

                    if (nProductID != oItem.ProductID)
                    {
                        _nCount++;
                        oPdfPCell = new PdfPCell(new Phrase(_nCount.ToString() + ". Yarn: " + oItem.ProductNameCode, _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.Colspan = 12;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPTable.AddCell(oPdfPCell);
                        oPdfPTable.CompleteRow();



                        oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPTable.AddCell(oPdfPCell);


                        //2
                        _nTotalQty = (_oDyeingOrderDetails.Where(c => c.ProductID == oItem.ProductID).Sum(x => x.Qty));
                        _nTotalAmount = (_oDyeingOrderDetails.Where(c => c.ProductID == oItem.ProductID).Sum(x => (x.Qty*x.UnitPrice)));

                        oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalQty), _oFontStyle));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        oPdfPTable.AddCell(oPdfPCell);
                        //3
                        if(_nTotalQty>0)
                        {
                        oPdfPCell = new PdfPCell(new Phrase(_oExportSCDO.Currency + "" + Global.MillionFormat(_nTotalAmount/_nTotalQty), _oFontStyle));
                        }
                        else
                        {
                            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        }
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        oPdfPTable.AddCell(oPdfPCell);

                        //4
                        oPdfPCell = new PdfPCell(new Phrase(_oExportSCDO.Currency + "" + Global.MillionFormat(_nTotalAmount), _oFontStyle));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        oPdfPTable.AddCell(oPdfPCell);

                        //5 Claim Qty

                        nQty_Claim = (_oExportSCDO.DUClaimOrderDetails.Where(c => c.ProductID == oItem.ProductID).Sum(x => x.Qty));

                        oPdfPCell = new PdfPCell(new Phrase(_oExportSCDO.Currency + "" + Global.MillionFormat(nQty_Claim), _oFontStyle)); //_nTotalClaim
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        oPdfPTable.AddCell(oPdfPCell);
                        //_nTotalClaim = _nTotalClaim + nTempQty;



                        _nTotalDOQty = (_oExportSCDO.DUDeliveryOrderDetails.Where(c => c.ProductID == oItem.ProductID).Sum(x => x.Qty));
                        _nTotalDOQty = _nTotalDOQty + (_oExportSCDO.DUDeliveryOrderDetails_Claim.Where(c => c.ProductID == oItem.ProductID).Sum(x => x.Qty));

                        oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalDOQty), _oFontStyle));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalQty-_nTotalDOQty), _oFontStyle));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        oPdfPTable.AddCell(oPdfPCell);

                        
                        _oExportSCDO.Qty_DC = (_oExportSCDO.DUReturnChallanDetails.Where(c => c.ProductID == oItem.ProductID).Sum(x => x.Qty));
                        _oExportSCDO.Qty_DC = _oExportSCDO.Qty_DC+(_oExportSCDO.DUReturnChallanDetails_Claim.Where(c => c.ProductID == oItem.ProductID).Sum(x => x.Qty));

                        oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oExportSCDO.Qty_DC), _oFontStyle));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalQty - _oExportSCDO.Qty_DC), _oFontStyle));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        oPdfPTable.AddCell(oPdfPCell);

                        _nTotalQty_RC = (_oExportSCDO.DUReturnChallanDetails.Where(c => c.ProductID == oItem.ProductID).Sum(x => x.Qty));
                        
                        oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalQty_RC), _oFontStyle));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPTable.CompleteRow();
                    }
                    #endregion
                    nProductID = oItem.ProductID;
                }
                #region Total
                _oPdfPCell = new PdfPCell(new Phrase("SUM", _oFontStyleBold));
                //_oPdfPCell.Colspan = 3;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(_oPdfPCell);

                

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oDyeingOrder.Qty), _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oExportSCDO.Currency + "" + Global.MillionFormat(_oDyeingOrder.Amount), _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oExportSCDO.Qty_Claim), _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                _nTotalDOQty = _oExportSCDO.DUDeliveryOrderDetails.Select(c => c.Qty).Sum();
                _nTotalDOQty =_nTotalDOQty+ _oExportSCDO.DUDeliveryOrderDetails_Claim.Select(c => c.Qty).Sum();


                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalDOQty), _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat((_oDyeingOrder.Qty + _oExportSCDO.Qty_Claim )- _nTotalDOQty), _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oExportSCDO.Qty_DC), _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalQty + _oExportSCDO.Qty_Claim - _oExportSCDO.Qty_DC), _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalQty_RC), _oFontStyleBold));
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
        }

        //#region Report Body
        //private void PrintBody()
        //{
        //    _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
        //    _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, 1);
        //    _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 9f, 4);

        //    #region SetDyeingOrderDetail
        //    _oPdfPCell = new PdfPCell(this.SetDyeingOrderDetail());
        //    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
        //    _oPdfPTable.CompleteRow();
        //    #endregion

        //    //#region Balank Space
        //    //_oPdfPCell = new PdfPCell(this.PrintFooter());
        //    //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
        //    //_oPdfPTable.CompleteRow();
        //    //#endregion

        //}
        //#endregion
        private void SetClaimOrder()
        {
            #region Delivery Order
            _nTotalAmount = 0;
            _nTotalQty = 0;
            _nGrandTotalDC = 0;
            _nGrandTotalYetToDC = 0;
            
            PdfPTable oPdfPTable = new PdfPTable(8);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 90f, 80f, 140f, 60, 60f, 70f, 70f, 70f });
            int nProductID = 0;

            if (_oExportSCDO.DUClaimOrderDetails.Count > 0)
            {
                _nTotalAmount = 0;
                _nTotalQty = 0;
                _nGrandTotalAmount = 0;
                _nGrandTotalQty = 0;


                oPdfPCell = new PdfPCell(new Phrase("Claim Order info", FontFactory.GetFont("Tahoma", 10f, 3)));
                oPdfPCell.Colspan = 8;

                oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                oPdfPCell.BackgroundColor = BaseColor.GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();
                _oDUDeliveryOrderDetails = _oDUDeliveryOrderDetails.OrderBy(o => o.ProductID).ToList();
                foreach (DUClaimOrderDetail oCODetail in _oExportSCDO.DUClaimOrderDetails)
                {
                    if (nProductID != oCODetail.ProductID)
                    {
                        #region Header

                        if (nProductID > 0)
                        {
                            #region Total
                            _oPdfPCell = new PdfPCell(new Phrase("Total: ", _oFontStyleBold));
                            _oPdfPCell.Colspan = 3;
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

                            _oPdfPCell = new PdfPCell(new Phrase(_oExportSCDO.Currency + "" + Global.MillionFormat(_nTotalAmount), _oFontStyleBold));
                            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            //_oPdfPCell.Border = 0;
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            oPdfPTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalDC), _oFontStyleBold));
                            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            //_oPdfPCell.Border = 0;
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            oPdfPTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalYetToDC), _oFontStyleBold));
                            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            //_oPdfPCell.Border = 0;
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            oPdfPTable.AddCell(_oPdfPCell);

                            oPdfPTable.CompleteRow();
                            #endregion
                        }

                        oPdfPCell = new PdfPCell(new Phrase("Yarn Type: " + oCODetail.ProductCode + "" + oCODetail.ProductName, _oFontStyleBold));
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

                        oPdfPCell = new PdfPCell(new Phrase("BCO No", _oFontStyleBold));
                        //oPdfPCell.Colspan = 2;
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase("Color", _oFontStyleBold));
                        //oPdfPCell.Colspan = 2;
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase(" Qty (" + _oExportSCDO.MUName + ")", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);

                        //oPdfPCell = new PdfPCell(new Phrase("U/P(" + _oExportSCDO.Currency + ")", _oFontStyleBold));
                        //oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        //oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        //oPdfPTable.AddCell(oPdfPCell);


                        //oPdfPCell = new PdfPCell(new Phrase("Amount(" + _oExportSCDO.Currency + ")", _oFontStyleBold));
                        //oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        //oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        //oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase("DO Qty  ", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase(" Pending DO ", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase("Delivery Qty  ", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase(" Balance D/C ", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);


                        oPdfPTable.CompleteRow();
                        #endregion
                        _nTotalQty = 0;
                        _nTotalAmount = 0;
                        _nTotalYetToDC = 0;
                        _nTotalDC = 0;
                        _nCount = 0;
                    }
                    #region PrintDetail
                    //List<DyeingOrderDetail> oDyeingOrderDetails = new List<DyeingOrderDetail>();
                    //_oDyeingOrderDetails = _oDyeingOrderDetails.Where(o => o.ProductID == oChallan.ProductID).ToList();

                    //for (int i = 0; i < oDyeingOrderDetails.Count; i++)
                    //{
                    _nCount++;
                    oPdfPCell = new PdfPCell(new Phrase(oCODetail.Date, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oCODetail.ClaimOrderNo, _oFontStyle));
                    //oPdfPCell.Colspan = 2;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(oPdfPCell);


                    oPdfPCell = new PdfPCell(new Phrase(oCODetail.ColorName, _oFontStyle));
                    //oPdfPCell.Colspan = 2;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(oPdfPCell);

                    //oPdfPCell = new PdfPCell(new Phrase(oDODetail.ColorName, _oFontStyle));
                    //oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //oPdfPTable.AddCell(oPdfPCell);

                    //oPdfPCell = new PdfPCell(new Phrase(oDODetail.LotNo, _oFontStyle));
                    //oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //oPdfPTable.AddCell(oPdfPCell);

                    //oPdfPCell = new PdfPCell(new Phrase(((EnumShade)oDODetail.Shade).ToString(), _oFontStyle));
                    //oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //oPdfPTable.AddCell(oPdfPCell);



                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oCODetail.Qty), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(oPdfPCell);

               

                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oCODetail.DOQty), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oCODetail.Qty - oCODetail.DOQty), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(oPdfPCell);


                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oCODetail.DOQty), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oCODetail.DOQty), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(oPdfPCell);


                    _nTotalYetToDC = _nTotalYetToDC + (oCODetail.Qty - oCODetail.DOQty);
                   
                    _nTotalQty = _nTotalQty + oCODetail.Qty;
                    
                    _nGrandTotalQty = _nGrandTotalQty + oCODetail.Qty;
                    _nTotalDC = _nTotalDC + oCODetail.DOQty;
                    _nGrandTotalDC = _nGrandTotalDC + oCODetail.DOQty;
                    _nGrandTotalYetToDC = _nGrandTotalYetToDC + ((oCODetail.Qty - oCODetail.DOQty));

                    oPdfPTable.CompleteRow();
                    #endregion

                    nProductID = oCODetail.ProductID;

                }

                #region Total
                _oPdfPCell = new PdfPCell(new Phrase("Total: ", _oFontStyleBold));
                _oPdfPCell.Colspan = 3;
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

                _oPdfPCell = new PdfPCell(new Phrase(_oExportSCDO.Currency + "" + Global.MillionFormat(_nTotalAmount), _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalDC), _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalYetToDC), _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();
                #endregion

                #region Grand Total
                _oPdfPCell = new PdfPCell(new Phrase("Grand Total: ", _oFontStyleBold));
                _oPdfPCell.Colspan = 3;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nGrandTotalQty), _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);



                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oExportSCDO.Currency + "" + Global.MillionFormat(_nGrandTotalAmount), _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nGrandTotalDC), _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nGrandTotalYetToDC), _oFontStyleBold));
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
        private void SetDyeingOrder()
        {
            _nTotalAmount = 0;
            _nTotalQty = 0;
            PdfPTable oPdfPTable = new PdfPTable(8);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 100f, 75f, 130f, 60, 60f, 75f, 70f, 70f });
            int nProductID = 0;
            #region Dyeing Order
            if (_oDyeingOrderDetails.Count > 0)
            {

                oPdfPCell = new PdfPCell(new Phrase("Production Order Detail", FontFactory.GetFont("Tahoma", 10f, 3)));
                oPdfPCell.Colspan = 8;

                oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                oPdfPCell.BackgroundColor = BaseColor.GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oDyeingOrderDetails = _oDyeingOrderDetails.OrderBy(o => o.ProductID).ToList();
                foreach (DyeingOrderDetail oDyeingOrderDetail in _oDyeingOrderDetails)
                {
                    //oDyeingOrderDetail.ProductID = 1;
                    //oDyeingOrderDetail.ProductNameCode = "100 ";
                    if (nProductID != oDyeingOrderDetail.ProductID)
                    {
                        #region Header

                        if (nProductID > 0)
                        {
                            #region Total

                            oPdfPTable = new PdfPTable(8);
                            oPdfPTable.SetWidths(new float[] { 100f, 75f, 130f, 60, 60f, 75f, 70f, 70f });

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

                            _oPdfPCell = new PdfPCell(new Phrase(_oExportSCDO.Currency + "" + Global.MillionFormat(_nTotalAmount), _oFontStyleBold));
                            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            //_oPdfPCell.Border = 0;
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            oPdfPTable.AddCell(_oPdfPCell);
                            oPdfPTable.CompleteRow();

                            _oPdfPCell = new PdfPCell(oPdfPTable);
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            _oPdfPTable.AddCell(_oPdfPCell);
                            _oPdfPTable.CompleteRow();
                            #endregion
                        }

                        oPdfPTable = new PdfPTable(8);
                        oPdfPTable.SetWidths(new float[] { 100f, 75f, 130f, 60, 60f, 75f, 70f, 70f });

                        oPdfPCell = new PdfPCell(new Phrase("Yarn Type: " + oDyeingOrderDetail.ProductNameCode, _oFontStyleBold));
                        oPdfPCell.Colspan = 8;
                        oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPTable.AddCell(oPdfPCell);
                        oPdfPTable.CompleteRow();

                        oPdfPCell = new PdfPCell(new Phrase("Order No", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase("Lab-Dip No", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase("COLOR", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase("COLOR NO", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase("SHADE", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase(" Qty (" + _oExportSCDO.MUName + ")", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase("Unit Price(" + _oExportSCDO.Currency + ")", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);


                        oPdfPCell = new PdfPCell(new Phrase("Amount(" + _oExportSCDO.Currency + ")", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);
                        oPdfPTable.CompleteRow();

                        _oPdfPCell = new PdfPCell(oPdfPTable);
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();

                        #endregion
                        _nTotalQty = 0;
                        _nTotalAmount = 0;
                        _nCount = 0;
                    }
                    #region PrintDetail

                    oPdfPTable = new PdfPTable(8);
                    oPdfPTable.SetWidths(new float[] { 100f, 75f, 130f, 60, 60f, 75f, 70f, 70f });

                    _nCount++;
                    oPdfPCell = new PdfPCell(new Phrase(oDyeingOrderDetail.OrderNo, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oDyeingOrderDetail.LabdipNo, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oDyeingOrderDetail.ColorName, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oDyeingOrderDetail.ColorNo, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oDyeingOrderDetail.ShadeSt, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(oPdfPCell);



                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oDyeingOrderDetail.Qty), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oDyeingOrderDetail.UnitPrice), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oDyeingOrderDetail.Qty * oDyeingOrderDetail.UnitPrice), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(oPdfPCell);




                    _nTotalAmount = _nTotalAmount + (oDyeingOrderDetail.Qty * oDyeingOrderDetail.UnitPrice);
                    _nTotalQty = _nTotalQty + oDyeingOrderDetail.Qty;
                    _nGrandTotalAmount = _nGrandTotalAmount + (oDyeingOrderDetail.Qty * oDyeingOrderDetail.UnitPrice);
                    _nGrandTotalQty = _nGrandTotalQty + oDyeingOrderDetail.Qty;
                    oPdfPTable.CompleteRow();

                    _oPdfPCell = new PdfPCell(oPdfPTable);
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion

                    nProductID = oDyeingOrderDetail.ProductID;

                }

                #region Total

                oPdfPTable = new PdfPTable(8);
                oPdfPTable.SetWidths(new float[] { 100f, 75f, 130f, 60, 60f, 75f, 70f, 70f });

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

                _oPdfPCell = new PdfPCell(new Phrase(_oExportSCDO.Currency + "" + Global.MillionFormat(_nTotalAmount), _oFontStyleBold));
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

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nGrandTotalQty) + "" + _oExportSCDO.MUName, _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);



                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oExportSCDO.Currency + "" + Global.MillionFormat(_nGrandTotalAmount), _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);


                oPdfPTable.CompleteRow();
                #endregion
                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

            }
            #endregion Dyeing Order
            //_oPdfPCell = new PdfPCell(oPdfPTable);
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();
        }
       
        private void SetDeliveryOrder()
        {
            #region Delivery Order
            _nTotalAmount = 0;
            _nTotalQty = 0;
            PdfPTable oPdfPTable = new PdfPTable(8);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 100f, 75f, 130f, 60, 60f, 75f, 70f, 70f });
            int nProductID = 0;

            if (_oDUDeliveryOrderDetails.Count > 0)
            {
                _nTotalAmount = 0;
                _nTotalQty = 0;
                _nGrandTotalAmount = 0;
                _nGrandTotalQty = 0;


                oPdfPCell = new PdfPCell(new Phrase("Delivery Order info", FontFactory.GetFont("Tahoma", 10f, 3)));
                oPdfPCell.Colspan = 8;

                oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                oPdfPCell.BackgroundColor = BaseColor.GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();
                _oDUDeliveryOrderDetails = _oDUDeliveryOrderDetails.OrderBy(o => o.ProductID).ToList();
                foreach (DUDeliveryOrderDetail oDODetail in _oDUDeliveryOrderDetails)
                {
                    //oDyeingOrderDetail.ProductID = 1;
                    //oDyeingOrderDetail.ProductNameCode = "100 ";
                    if (nProductID != oDODetail.ProductID)
                    {
                        #region Header

                        if (nProductID > 0)
                        {
                            #region Total
                            _oPdfPCell = new PdfPCell(new Phrase("Total: ", _oFontStyleBold));
                            _oPdfPCell.Colspan = 3;
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

                            _oPdfPCell = new PdfPCell(new Phrase(_oExportSCDO.Currency + "" + Global.MillionFormat(_nTotalAmount), _oFontStyleBold));
                            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            //_oPdfPCell.Border = 0;
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            oPdfPTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalDC), _oFontStyleBold));
                            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            //_oPdfPCell.Border = 0;
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            oPdfPTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalYetToDC), _oFontStyleBold));
                            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            //_oPdfPCell.Border = 0;
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            oPdfPTable.AddCell(_oPdfPCell);

                            oPdfPTable.CompleteRow();
                            #endregion
                        }

                        oPdfPCell = new PdfPCell(new Phrase("Yarn Type: " + oDODetail.ProductNameCode, _oFontStyleBold));
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

                        oPdfPCell = new PdfPCell(new Phrase("DO No", _oFontStyleBold));
                        oPdfPCell.Colspan = 2;
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);


                        oPdfPCell = new PdfPCell(new Phrase(" Qty (" + _oExportSCDO.MUName + ")", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase("U/P(" + _oExportSCDO.Currency + ")", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);


                        oPdfPCell = new PdfPCell(new Phrase("Amount(" + _oExportSCDO.Currency + ")", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase("Delivery Qty  ", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase(" Balance D/C ", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);


                        oPdfPTable.CompleteRow();
                        #endregion
                        _nTotalQty = 0;
                        _nTotalAmount = 0;
                        _nTotalYetToDC = 0;
                        _nTotalDC = 0;
                        _nCount = 0;
                    }
                    #region PrintDetail
                    //List<DyeingOrderDetail> oDyeingOrderDetails = new List<DyeingOrderDetail>();
                    //_oDyeingOrderDetails = _oDyeingOrderDetails.Where(o => o.ProductID == oChallan.ProductID).ToList();

                    //for (int i = 0; i < oDyeingOrderDetails.Count; i++)
                    //{
                    _nCount++;
                    oPdfPCell = new PdfPCell(new Phrase(oDODetail.DeliveryDateSt, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oDODetail.OrderNo, _oFontStyle));
                    oPdfPCell.Colspan = 2;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(oPdfPCell);

                    //oPdfPCell = new PdfPCell(new Phrase(oDODetail.ColorName, _oFontStyle));
                    //oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //oPdfPTable.AddCell(oPdfPCell);

                    //oPdfPCell = new PdfPCell(new Phrase(oDODetail.LotNo, _oFontStyle));
                    //oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //oPdfPTable.AddCell(oPdfPCell);

                    //oPdfPCell = new PdfPCell(new Phrase(((EnumShade)oDODetail.Shade).ToString(), _oFontStyle));
                    //oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //oPdfPTable.AddCell(oPdfPCell);



                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oDODetail.Qty), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oDODetail.UnitPrice), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oDODetail.Qty * oDODetail.UnitPrice), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oDODetail.Qty_DC), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oDODetail.YetToDC), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(oPdfPCell);


                    _nTotalYetToDC = _nTotalYetToDC + oDODetail.YetToDC;
                    _nTotalAmount = _nTotalAmount + (oDODetail.Qty * oDODetail.UnitPrice);
                    _nTotalQty = _nTotalQty + oDODetail.Qty;
                    _nGrandTotalAmount = _nGrandTotalAmount + (oDODetail.Qty * oDODetail.UnitPrice);
                    _nGrandTotalQty = _nGrandTotalQty + oDODetail.Qty;
                    _nTotalDC = _nTotalDC + oDODetail.Qty_DC;
                    _nGrandTotalDC = _nGrandTotalDC + oDODetail.Qty_DC;
                    _nGrandTotalYetToDC = _nGrandTotalYetToDC + oDODetail.YetToDC;

                    oPdfPTable.CompleteRow();
                    #endregion

                    nProductID = oDODetail.ProductID;

                }

                #region Total
                _oPdfPCell = new PdfPCell(new Phrase("Total: ", _oFontStyleBold));
                _oPdfPCell.Colspan = 3;
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

                _oPdfPCell = new PdfPCell(new Phrase(_oExportSCDO.Currency + "" + Global.MillionFormat(_nTotalAmount), _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalDC), _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalYetToDC), _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();
                #endregion

                #region Grand Total
                _oPdfPCell = new PdfPCell(new Phrase("Grand Total: ", _oFontStyleBold));
                _oPdfPCell.Colspan = 3;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nGrandTotalQty), _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);



                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oExportSCDO.Currency + "" + Global.MillionFormat(_nGrandTotalAmount), _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nGrandTotalDC), _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nGrandTotalYetToDC), _oFontStyleBold));
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
        private void SetDeliveryChallan()
        {

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
            if (_oDUDeliveryChallanDetails.Count > 0)
            {


                oPdfPCell = new PdfPCell(new Phrase("Delivery Challan info", FontFactory.GetFont("Tahoma", 10f, 3)));
                oPdfPCell.Colspan = 8;

                oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                oPdfPCell.BackgroundColor = BaseColor.GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();
                _oDUDeliveryChallanDetails = _oDUDeliveryChallanDetails.OrderBy(o => o.ProductID).ToList();
                foreach (DUDeliveryChallanDetail oDCDetail in _oDUDeliveryChallanDetails)
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

                            _oPdfPCell = new PdfPCell(new Phrase(_oExportSCDO.Currency + "" + Global.MillionFormat(_nTotalAmount), _oFontStyleBold));
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

                        oPdfPCell = new PdfPCell(new Phrase(" Qty (" + _oExportSCDO.MUName + ")", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase("Unit Price(" + _oExportSCDO.Currency + ")", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);


                        oPdfPCell = new PdfPCell(new Phrase("Amount(" + _oExportSCDO.Currency + ")", _oFontStyleBold));
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

                    oPdfPCell = new PdfPCell(new Phrase("DC "+oDCDetail.ChallanNo, _oFontStyle));
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
                    oDUReturnChallanDetails = _oExportSCDO.DUReturnChallanDetails.Where(o => o.DUDeliveryChallanDetailID == oDCDetail.DUDeliveryChallanDetailID).ToList();
                    if (oDUReturnChallanDetails.Count > 0)
                    {
                        for (int j = 0; j < oDUReturnChallanDetails.Count; j++)
                        {
                            _nCount++;
                         

                            oPdfPCell = new PdfPCell(new Phrase(oDUReturnChallanDetails[j].RCDate, _oFontStyle));
                            oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                            oPdfPTable.AddCell(oPdfPCell);

                            oPdfPCell = new PdfPCell(new Phrase("RC "+oDUReturnChallanDetails[j].RCNo, _oFontStyle));
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

                _oPdfPCell = new PdfPCell(new Phrase(_oExportSCDO.Currency + "" + Global.MillionFormat(_nTotalAmount), _oFontStyleBold));
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

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nGrandTotalQty) + "" + _oExportSCDO.MUName, _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);



                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oExportSCDO.Currency + "" + Global.MillionFormat(_nGrandTotalAmount), _oFontStyleBold));
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
        private void SetDeliveryOrder_Claim()
        {
            #region Delivery Order Claim
            _nTotalAmount = 0;
            _nTotalQty = 0;
            PdfPTable oPdfPTable = new PdfPTable(8);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 100f, 75f, 130f, 60, 60f, 75f, 70f, 70f });
            int nProductID = 0;

            if (_oExportSCDO.DUDeliveryOrderDetails_Claim .Count > 0)
            {
                _nTotalAmount = 0;
                _nTotalQty = 0;
                _nGrandTotalAmount = 0;
                _nGrandTotalQty = 0;


                oPdfPCell = new PdfPCell(new Phrase("Claim Delivery Order info", FontFactory.GetFont("Tahoma", 10f, 3)));
                oPdfPCell.Colspan = 8;

                oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                oPdfPCell.BackgroundColor = BaseColor.GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();
                _oExportSCDO.DUDeliveryOrderDetails_Claim = _oExportSCDO.DUDeliveryOrderDetails_Claim.OrderBy(o => o.ProductID).ToList();
                foreach (DUDeliveryOrderDetail oDODetail in _oExportSCDO.DUDeliveryOrderDetails_Claim)
                {
                    //oDyeingOrderDetail.ProductID = 1;
                    //oDyeingOrderDetail.ProductNameCode = "100 ";
                    if (nProductID != oDODetail.ProductID)
                    {
                        #region Header

                        if (nProductID > 0)
                        {
                            #region Total
                            _oPdfPCell = new PdfPCell(new Phrase("Total: ", _oFontStyleBold));
                            _oPdfPCell.Colspan = 3;
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

                            _oPdfPCell = new PdfPCell(new Phrase(_oExportSCDO.Currency + "" + Global.MillionFormat(_nTotalAmount), _oFontStyleBold));
                            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            //_oPdfPCell.Border = 0;
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            oPdfPTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalDC), _oFontStyleBold));
                            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            //_oPdfPCell.Border = 0;
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            oPdfPTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalYetToDC), _oFontStyleBold));
                            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            //_oPdfPCell.Border = 0;
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            oPdfPTable.AddCell(_oPdfPCell);

                            oPdfPTable.CompleteRow();
                            #endregion
                        }

                        oPdfPCell = new PdfPCell(new Phrase("Yarn Type: " + oDODetail.ProductNameCode, _oFontStyleBold));
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

                        oPdfPCell = new PdfPCell(new Phrase("DO No", _oFontStyleBold));
                        oPdfPCell.Colspan = 2;
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);


                        oPdfPCell = new PdfPCell(new Phrase(" Qty (" + _oExportSCDO.MUName + ")", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase("U/P(" + _oExportSCDO.Currency + ")", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);


                        oPdfPCell = new PdfPCell(new Phrase("Amount(" + _oExportSCDO.Currency + ")", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase("Delivery Qty  ", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase(" Balance D/C", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);


                        oPdfPTable.CompleteRow();
                        #endregion
                        _nTotalQty = 0;
                        _nTotalAmount = 0;
                        _nTotalYetToDC = 0;
                        _nTotalDC = 0;
                        _nCount = 0;
                    }
                    #region PrintDetail
                    //List<DyeingOrderDetail> oDyeingOrderDetails = new List<DyeingOrderDetail>();
                    //_oDyeingOrderDetails = _oDyeingOrderDetails.Where(o => o.ProductID == oChallan.ProductID).ToList();

                    //for (int i = 0; i < oDyeingOrderDetails.Count; i++)
                    //{
                    _nCount++;
                    oPdfPCell = new PdfPCell(new Phrase(oDODetail.DeliveryDateSt, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oDODetail.OrderNo, _oFontStyle));
                    oPdfPCell.Colspan = 2;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(oPdfPCell);

                    //oPdfPCell = new PdfPCell(new Phrase(oDODetail.ColorName, _oFontStyle));
                    //oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //oPdfPTable.AddCell(oPdfPCell);

                    //oPdfPCell = new PdfPCell(new Phrase(oDODetail.LotNo, _oFontStyle));
                    //oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //oPdfPTable.AddCell(oPdfPCell);

                    //oPdfPCell = new PdfPCell(new Phrase(((EnumShade)oDODetail.Shade).ToString(), _oFontStyle));
                    //oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //oPdfPTable.AddCell(oPdfPCell);



                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oDODetail.Qty), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oDODetail.UnitPrice), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oDODetail.Qty * oDODetail.UnitPrice), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oDODetail.Qty_DC), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oDODetail.YetToDC), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(oPdfPCell);


                    _nTotalYetToDC = _nTotalYetToDC + oDODetail.YetToDC;
                    _nTotalAmount = _nTotalAmount + (oDODetail.Qty * oDODetail.UnitPrice);
                    _nTotalQty = _nTotalQty + oDODetail.Qty;
                    _nGrandTotalAmount = _nGrandTotalAmount + (oDODetail.Qty * oDODetail.UnitPrice);
                    _nGrandTotalQty = _nGrandTotalQty + oDODetail.Qty;
                    _nTotalDC = _nTotalDC + oDODetail.Qty_DC;
                    _nGrandTotalDC = _nGrandTotalDC + oDODetail.Qty_DC;
                    _nGrandTotalYetToDC = _nGrandTotalYetToDC + oDODetail.YetToDC;

                    oPdfPTable.CompleteRow();
                    #endregion

                    nProductID = oDODetail.ProductID;

                }

                #region Total
                _oPdfPCell = new PdfPCell(new Phrase("Total: ", _oFontStyleBold));
                _oPdfPCell.Colspan = 3;
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

                _oPdfPCell = new PdfPCell(new Phrase(_oExportSCDO.Currency + "" + Global.MillionFormat(_nTotalAmount), _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalDC), _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalYetToDC), _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();
                #endregion

                #region Grand Total
                _oPdfPCell = new PdfPCell(new Phrase("Grand Total: ", _oFontStyleBold));
                _oPdfPCell.Colspan = 3;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nGrandTotalQty), _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);



                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oExportSCDO.Currency + "" + Global.MillionFormat(_nGrandTotalAmount), _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nGrandTotalDC), _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nGrandTotalYetToDC), _oFontStyleBold));
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
        private void SetDeliveryChallan_Claim()
        {

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
            if (_oExportSCDO.DUDeliveryChallanDetails_Claim.Count > 0)
            {


                oPdfPCell = new PdfPCell(new Phrase("Claim Delivery Challan info", FontFactory.GetFont("Tahoma", 10f, 3)));
                oPdfPCell.Colspan = 8;

                oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                oPdfPCell.BackgroundColor = BaseColor.GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();
                _oExportSCDO.DUDeliveryChallanDetails_Claim = _oExportSCDO.DUDeliveryChallanDetails_Claim.OrderBy(o => o.ProductID).ToList();
                foreach (DUDeliveryChallanDetail oDCDetail in _oExportSCDO.DUDeliveryChallanDetails_Claim)
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

                            _oPdfPCell = new PdfPCell(new Phrase(_oExportSCDO.Currency + "" + Global.MillionFormat(_nTotalAmount), _oFontStyleBold));
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

                        oPdfPCell = new PdfPCell(new Phrase(" Qty (" + _oExportSCDO.MUName + ")", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase("Unit Price(" + _oExportSCDO.Currency + ")", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);


                        oPdfPCell = new PdfPCell(new Phrase("Amount(" + _oExportSCDO.Currency + ")", _oFontStyleBold));
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
                    oDUReturnChallanDetails = _oExportSCDO.DUReturnChallanDetails_Claim.Where(o => o.DUDeliveryChallanDetailID == oDCDetail.DUDeliveryChallanDetailID).ToList();
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

                _oPdfPCell = new PdfPCell(new Phrase(_oExportSCDO.Currency + "" + Global.MillionFormat(_nTotalAmount), _oFontStyleBold));
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

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nGrandTotalQty) + "" + _oExportSCDO.MUName, _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);



                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oExportSCDO.Currency + "" + Global.MillionFormat(_nGrandTotalAmount), _oFontStyleBold));
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
        //private PdfPTable SetDyeingOrderDetail()
        //{
        //    _nTotalAmount = 0;
        //    _nTotalQty = 0;

        //    PdfPTable oPdfPTable = new PdfPTable(8);
        //    PdfPCell oPdfPCell;
        //    oPdfPTable.SetWidths(new float[] { 100f, 75f, 130f, 60, 60f, 75f, 70f, 70f });

        //    int nProductID = 0;
        //    #region Dyeing Order
        //    if (_oDyeingOrderDetails.Count > 0)
        //    {

        //        oPdfPCell = new PdfPCell(new Phrase("Production Order Detail", FontFactory.GetFont("Tahoma", 10f, 3)));
        //        oPdfPCell.Colspan = 8;

        //        oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
        //        oPdfPCell.BackgroundColor = BaseColor.GRAY;
        //        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //        oPdfPTable.AddCell(oPdfPCell);
        //        oPdfPTable.CompleteRow();
        //        _oDyeingOrderDetails = _oDyeingOrderDetails.OrderBy(o => o.ProductID).ToList();
        //        foreach (DyeingOrderDetail oDyeingOrderDetail in _oDyeingOrderDetails)
        //        {
        //            //oDyeingOrderDetail.ProductID = 1;
        //            //oDyeingOrderDetail.ProductNameCode = "100 ";
        //            if (nProductID != oDyeingOrderDetail.ProductID)
        //            {
        //                #region Header

        //                if (nProductID > 0)
        //                {
        //                    #region Total
        //                    _oPdfPCell = new PdfPCell(new Phrase("Total: ", _oFontStyleBold));
        //                    _oPdfPCell.Colspan = 5;
        //                    //_oPdfPCell.Border = 0;
        //                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                    oPdfPTable.AddCell(_oPdfPCell);

        //                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalQty), _oFontStyleBold));
        //                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //                    //_oPdfPCell.Border = 0;
        //                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                    oPdfPTable.AddCell(_oPdfPCell);

        //                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
        //                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //                    //_oPdfPCell.Border = 0;
        //                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                    oPdfPTable.AddCell(_oPdfPCell);

        //                    _oPdfPCell = new PdfPCell(new Phrase(_oExportSCDO.Currency + "" + Global.MillionFormat(_nTotalAmount), _oFontStyleBold));
        //                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //                    //_oPdfPCell.Border = 0;
        //                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                    oPdfPTable.AddCell(_oPdfPCell);



        //                    oPdfPTable.CompleteRow();
        //                    #endregion
        //                }

        //                oPdfPCell = new PdfPCell(new Phrase("Yarn Type: " + oDyeingOrderDetail.ProductNameCode, _oFontStyleBold));
        //                oPdfPCell.Colspan = 8;
        //                oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
        //                oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
        //                oPdfPTable.AddCell(oPdfPCell);
        //                oPdfPTable.CompleteRow();

        //                oPdfPCell = new PdfPCell(new Phrase("Order No", _oFontStyleBold));
        //                oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
        //                oPdfPTable.AddCell(oPdfPCell);

        //                oPdfPCell = new PdfPCell(new Phrase("Lab-Dip No", _oFontStyleBold));
        //                oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //                oPdfPTable.AddCell(oPdfPCell);

        //                oPdfPCell = new PdfPCell(new Phrase("COLOR", _oFontStyleBold));
        //                oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //                oPdfPTable.AddCell(oPdfPCell);

        //                oPdfPCell = new PdfPCell(new Phrase("COLOR NO", _oFontStyleBold));
        //                oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //                oPdfPTable.AddCell(oPdfPCell);

        //                oPdfPCell = new PdfPCell(new Phrase("SHADE", _oFontStyleBold));
        //                oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //                oPdfPTable.AddCell(oPdfPCell);

        //                oPdfPCell = new PdfPCell(new Phrase(" Qty (" + _oExportSCDO.MUName + ")", _oFontStyleBold));
        //                oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //                oPdfPTable.AddCell(oPdfPCell);

        //                oPdfPCell = new PdfPCell(new Phrase("Unit Price(" + _oExportSCDO.Currency + ")", _oFontStyleBold));
        //                oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //                oPdfPTable.AddCell(oPdfPCell);


        //                oPdfPCell = new PdfPCell(new Phrase("Amount(" + _oExportSCDO.Currency + ")", _oFontStyleBold));
        //                oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //                oPdfPTable.AddCell(oPdfPCell);



        //                oPdfPTable.CompleteRow();
        //                #endregion
        //                _nTotalQty = 0;
        //                _nTotalAmount = 0;
        //                _nCount = 0;
        //            }
        //            #region PrintDetail
        //            //List<DyeingOrderDetail> oDyeingOrderDetails = new List<DyeingOrderDetail>();
        //            //_oDyeingOrderDetails = _oDyeingOrderDetails.Where(o => o.ProductID == oChallan.ProductID).ToList();

        //            //for (int i = 0; i < oDyeingOrderDetails.Count; i++)
        //            //{
        //            _nCount++;
        //            oPdfPCell = new PdfPCell(new Phrase(oDyeingOrderDetail.OrderNo, _oFontStyle));
        //            oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
        //            oPdfPTable.AddCell(oPdfPCell);

        //            oPdfPCell = new PdfPCell(new Phrase(oDyeingOrderDetail.LabdipNo, _oFontStyle));
        //            oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
        //            oPdfPTable.AddCell(oPdfPCell);

        //            oPdfPCell = new PdfPCell(new Phrase(oDyeingOrderDetail.ColorName, _oFontStyle));
        //            oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
        //            oPdfPTable.AddCell(oPdfPCell);

        //            oPdfPCell = new PdfPCell(new Phrase(oDyeingOrderDetail.ColorNo, _oFontStyle));
        //            oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //            oPdfPTable.AddCell(oPdfPCell);

        //            oPdfPCell = new PdfPCell(new Phrase(oDyeingOrderDetail.ShadeSt, _oFontStyle));
        //            oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
        //            oPdfPTable.AddCell(oPdfPCell);



        //            oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oDyeingOrderDetail.Qty), _oFontStyle));
        //            oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //            oPdfPTable.AddCell(oPdfPCell);

        //            oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oDyeingOrderDetail.UnitPrice), _oFontStyle));
        //            oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //            oPdfPTable.AddCell(oPdfPCell);

        //            oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oDyeingOrderDetail.Qty * oDyeingOrderDetail.UnitPrice), _oFontStyle));
        //            oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //            oPdfPTable.AddCell(oPdfPCell);




        //            _nTotalAmount = _nTotalAmount + (oDyeingOrderDetail.Qty * oDyeingOrderDetail.UnitPrice);
        //            _nTotalQty = _nTotalQty + oDyeingOrderDetail.Qty;
        //            _nGrandTotalAmount = _nGrandTotalAmount + (oDyeingOrderDetail.Qty * oDyeingOrderDetail.UnitPrice);
        //            _nGrandTotalQty = _nGrandTotalQty + oDyeingOrderDetail.Qty;
        //            oPdfPTable.CompleteRow();
        //            #endregion

        //            nProductID = oDyeingOrderDetail.ProductID;

        //        }

        //        #region Total
        //        _oPdfPCell = new PdfPCell(new Phrase("Total: ", _oFontStyleBold));
        //        _oPdfPCell.Colspan = 5;
        //        //_oPdfPCell.Border = 0;
        //        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //        oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalQty), _oFontStyleBold));
        //        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //        //_oPdfPCell.Border = 0;
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //        oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
        //        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //        //_oPdfPCell.Border = 0;
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //        oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase(_oExportSCDO.Currency + "" + Global.MillionFormat(_nTotalAmount), _oFontStyleBold));
        //        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //        //_oPdfPCell.Border = 0;
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //        oPdfPTable.AddCell(_oPdfPCell);



        //        oPdfPTable.CompleteRow();
        //        #endregion

        //        #region Grand Total
        //        _oPdfPCell = new PdfPCell(new Phrase("Grand Total: ", _oFontStyleBold));
        //        _oPdfPCell.Colspan = 5;
        //        //_oPdfPCell.Border = 0;
        //        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //        oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nGrandTotalQty) + "" + _oExportSCDO.MUName, _oFontStyleBold));
        //        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //        //_oPdfPCell.Border = 0;
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //        oPdfPTable.AddCell(_oPdfPCell);



        //        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
        //        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //        //_oPdfPCell.Border = 0;
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //        oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase(_oExportSCDO.Currency + "" + Global.MillionFormat(_nGrandTotalAmount), _oFontStyleBold));
        //        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //        //_oPdfPCell.Border = 0;
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //        oPdfPTable.AddCell(_oPdfPCell);




        //        oPdfPTable.CompleteRow();
        //        #endregion


        //    }
        //    #endregion Dyeing Order

        //    #region Delivery Order
        //    nProductID = 0;
        //    if (_oDUDeliveryOrderDetails.Count > 0)
        //    {
        //        _nTotalAmount = 0;
        //        _nTotalQty = 0;
        //        _nGrandTotalAmount = 0;
        //        _nGrandTotalQty = 0;


        //        oPdfPCell = new PdfPCell(new Phrase("Delivery Order info", FontFactory.GetFont("Tahoma", 10f, 3)));
        //        oPdfPCell.Colspan = 8;

        //        oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
        //        oPdfPCell.BackgroundColor = BaseColor.GRAY;
        //        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //        oPdfPTable.AddCell(oPdfPCell);
        //        oPdfPTable.CompleteRow();
        //        _oDUDeliveryOrderDetails = _oDUDeliveryOrderDetails.OrderBy(o => o.ProductID).ToList();
        //        foreach (DUDeliveryOrderDetail oDODetail in _oDUDeliveryOrderDetails)
        //        {
        //            //oDyeingOrderDetail.ProductID = 1;
        //            //oDyeingOrderDetail.ProductNameCode = "100 ";
        //            if (nProductID != oDODetail.ProductID)
        //            {
        //                #region Header

        //                if (nProductID > 0)
        //                {
        //                    #region Total
        //                    _oPdfPCell = new PdfPCell(new Phrase("Total: ", _oFontStyleBold));
        //                    _oPdfPCell.Colspan = 3;
        //                    //_oPdfPCell.Border = 0;
        //                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                    oPdfPTable.AddCell(_oPdfPCell);

        //                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalQty), _oFontStyleBold));
        //                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //                    //_oPdfPCell.Border = 0;
        //                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                    oPdfPTable.AddCell(_oPdfPCell);

        //                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
        //                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //                    //_oPdfPCell.Border = 0;
        //                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                    oPdfPTable.AddCell(_oPdfPCell);

        //                    _oPdfPCell = new PdfPCell(new Phrase(_oExportSCDO.Currency + "" + Global.MillionFormat(_nTotalAmount), _oFontStyleBold));
        //                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //                    //_oPdfPCell.Border = 0;
        //                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                    oPdfPTable.AddCell(_oPdfPCell);

        //                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalDC), _oFontStyleBold));
        //                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //                    //_oPdfPCell.Border = 0;
        //                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                    oPdfPTable.AddCell(_oPdfPCell);

        //                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalYetToDC), _oFontStyleBold));
        //                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //                    //_oPdfPCell.Border = 0;
        //                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                    oPdfPTable.AddCell(_oPdfPCell);

        //                    oPdfPTable.CompleteRow();
        //                    #endregion
        //                }

        //                oPdfPCell = new PdfPCell(new Phrase("Yarn Type: " + oDODetail.ProductNameCode, _oFontStyleBold));
        //                oPdfPCell.Colspan = 8;
        //                oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
        //                oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
        //                oPdfPTable.AddCell(oPdfPCell);
        //                oPdfPTable.CompleteRow();

        //                oPdfPCell = new PdfPCell(new Phrase("Date", _oFontStyleBold));
        //                oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
        //                oPdfPTable.AddCell(oPdfPCell);

        //                oPdfPCell = new PdfPCell(new Phrase("DO No", _oFontStyleBold));
        //                oPdfPCell.Colspan = 2;
        //                oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //                oPdfPTable.AddCell(oPdfPCell);


        //                oPdfPCell = new PdfPCell(new Phrase(" Qty (" + _oExportSCDO.MUName + ")", _oFontStyleBold));
        //                oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //                oPdfPTable.AddCell(oPdfPCell);

        //                oPdfPCell = new PdfPCell(new Phrase("U/P(" + _oExportSCDO.Currency + ")", _oFontStyleBold));
        //                oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //                oPdfPTable.AddCell(oPdfPCell);


        //                oPdfPCell = new PdfPCell(new Phrase("Amount(" + _oExportSCDO.Currency + ")", _oFontStyleBold));
        //                oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //                oPdfPTable.AddCell(oPdfPCell);

        //                oPdfPCell = new PdfPCell(new Phrase("Delivery Qty  ", _oFontStyleBold));
        //                oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //                oPdfPTable.AddCell(oPdfPCell);

        //                oPdfPCell = new PdfPCell(new Phrase(" Yet To D/C ", _oFontStyleBold));
        //                oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //                oPdfPTable.AddCell(oPdfPCell);


        //                oPdfPTable.CompleteRow();
        //                #endregion
        //                _nTotalQty = 0;
        //                _nTotalAmount = 0;
        //                _nTotalYetToDC = 0;
        //                _nTotalDC = 0;
        //                _nCount = 0;
        //            }
        //            #region PrintDetail
        //            //List<DyeingOrderDetail> oDyeingOrderDetails = new List<DyeingOrderDetail>();
        //            //_oDyeingOrderDetails = _oDyeingOrderDetails.Where(o => o.ProductID == oChallan.ProductID).ToList();

        //            //for (int i = 0; i < oDyeingOrderDetails.Count; i++)
        //            //{
        //            _nCount++;
        //            oPdfPCell = new PdfPCell(new Phrase(oDODetail.DeliveryDateSt, _oFontStyle));
        //            oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
        //            oPdfPTable.AddCell(oPdfPCell);

        //            oPdfPCell = new PdfPCell(new Phrase(oDODetail.OrderNo, _oFontStyle));
        //            oPdfPCell.Colspan = 2;
        //            oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
        //            oPdfPTable.AddCell(oPdfPCell);

        //            //oPdfPCell = new PdfPCell(new Phrase(oDODetail.ColorName, _oFontStyle));
        //            //oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //            //oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
        //            //oPdfPTable.AddCell(oPdfPCell);

        //            //oPdfPCell = new PdfPCell(new Phrase(oDODetail.LotNo, _oFontStyle));
        //            //oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //            //oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //            //oPdfPTable.AddCell(oPdfPCell);

        //            //oPdfPCell = new PdfPCell(new Phrase(((EnumShade)oDODetail.Shade).ToString(), _oFontStyle));
        //            //oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //            //oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
        //            //oPdfPTable.AddCell(oPdfPCell);



        //            oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oDODetail.Qty), _oFontStyle));
        //            oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //            oPdfPTable.AddCell(oPdfPCell);

        //            oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oDODetail.UnitPrice), _oFontStyle));
        //            oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //            oPdfPTable.AddCell(oPdfPCell);

        //            oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oDODetail.Qty * oDODetail.UnitPrice), _oFontStyle));
        //            oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //            oPdfPTable.AddCell(oPdfPCell);

        //            oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oDODetail.Qty_DC), _oFontStyle));
        //            oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //            oPdfPTable.AddCell(oPdfPCell);

        //            oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oDODetail.YetToDC), _oFontStyle));
        //            oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //            oPdfPTable.AddCell(oPdfPCell);


        //            _nTotalYetToDC = _nTotalYetToDC + oDODetail.YetToDC;
        //            _nTotalAmount = _nTotalAmount + (oDODetail.Qty * oDODetail.UnitPrice);
        //            _nTotalQty = _nTotalQty + oDODetail.Qty;
        //            _nGrandTotalAmount = _nGrandTotalAmount + (oDODetail.Qty * oDODetail.UnitPrice);
        //            _nGrandTotalQty = _nGrandTotalQty + oDODetail.Qty;
        //            _nTotalDC = _nTotalDC + oDODetail.Qty_DC;
        //            _nGrandTotalDC = _nGrandTotalDC + oDODetail.Qty_DC;
        //            _nGrandTotalYetToDC = _nGrandTotalYetToDC + oDODetail.YetToDC;

        //            oPdfPTable.CompleteRow();
        //            #endregion

        //            nProductID = oDODetail.ProductID;

        //        }

        //        #region Total
        //        _oPdfPCell = new PdfPCell(new Phrase("Total: ", _oFontStyleBold));
        //        _oPdfPCell.Colspan = 3;
        //        //_oPdfPCell.Border = 0;
        //        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //        oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalQty), _oFontStyleBold));
        //        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //        //_oPdfPCell.Border = 0;
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //        oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
        //        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //        //_oPdfPCell.Border = 0;
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //        oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase(_oExportSCDO.Currency + "" + Global.MillionFormat(_nTotalAmount), _oFontStyleBold));
        //        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //        //_oPdfPCell.Border = 0;
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //        oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalDC), _oFontStyleBold));
        //        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //        //_oPdfPCell.Border = 0;
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //        oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalYetToDC), _oFontStyleBold));
        //        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //        //_oPdfPCell.Border = 0;
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //        oPdfPTable.AddCell(_oPdfPCell);

        //        oPdfPTable.CompleteRow();
        //        #endregion

        //        #region Grand Total
        //        _oPdfPCell = new PdfPCell(new Phrase("Grand Total: ", _oFontStyleBold));
        //        _oPdfPCell.Colspan = 3;
        //        //_oPdfPCell.Border = 0;
        //        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //        oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nGrandTotalQty), _oFontStyleBold));
        //        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //        //_oPdfPCell.Border = 0;
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //        oPdfPTable.AddCell(_oPdfPCell);



        //        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
        //        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //        //_oPdfPCell.Border = 0;
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //        oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase(_oExportSCDO.Currency + "" + Global.MillionFormat(_nGrandTotalAmount), _oFontStyleBold));
        //        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //        //_oPdfPCell.Border = 0;
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //        oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nGrandTotalDC), _oFontStyleBold));
        //        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //        //_oPdfPCell.Border = 0;
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //        oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nGrandTotalYetToDC), _oFontStyleBold));
        //        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //        //_oPdfPCell.Border = 0;
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //        oPdfPTable.AddCell(_oPdfPCell);

        //        oPdfPTable.CompleteRow();
        //        #endregion
        //    }
        //    #endregion Delivery challan

        //    #region Delivery challan
        //    _nTotalAmount = 0;
        //    _nTotalQty = 0;
        //    _nGrandTotalAmount = 0;
        //    _nGrandTotalQty = 0;
        //    nProductID = 0;
        //    if (_oDUDeliveryChallanDetails.Count > 0)
        //    {


        //        oPdfPCell = new PdfPCell(new Phrase("Delivery info", FontFactory.GetFont("Tahoma", 10f, 3)));
        //        oPdfPCell.Colspan = 8;

        //        oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
        //        oPdfPCell.BackgroundColor = BaseColor.GRAY;
        //        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //        oPdfPTable.AddCell(oPdfPCell);
        //        oPdfPTable.CompleteRow();
        //        _oDUDeliveryChallanDetails = _oDUDeliveryChallanDetails.OrderBy(o => o.ProductID).ToList();
        //        foreach (DUDeliveryChallanDetail oDCDetail in _oDUDeliveryChallanDetails)
        //        {
        //            //oDyeingOrderDetail.ProductID = 1;
        //            //oDyeingOrderDetail.ProductNameCode = "100 ";
        //            if (nProductID != oDCDetail.ProductID)
        //            {
        //                #region Header

        //                if (nProductID > 0)
        //                {
        //                    #region Total
        //                    _oPdfPCell = new PdfPCell(new Phrase("Total: ", _oFontStyleBold));
        //                    _oPdfPCell.Colspan = 5;
        //                    //_oPdfPCell.Border = 0;
        //                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                    oPdfPTable.AddCell(_oPdfPCell);

        //                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalQty), _oFontStyleBold));
        //                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //                    //_oPdfPCell.Border = 0;
        //                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                    oPdfPTable.AddCell(_oPdfPCell);

        //                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
        //                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //                    //_oPdfPCell.Border = 0;
        //                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                    oPdfPTable.AddCell(_oPdfPCell);

        //                    _oPdfPCell = new PdfPCell(new Phrase(_oExportSCDO.Currency + "" + Global.MillionFormat(_nTotalAmount), _oFontStyleBold));
        //                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //                    //_oPdfPCell.Border = 0;
        //                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                    oPdfPTable.AddCell(_oPdfPCell);



        //                    oPdfPTable.CompleteRow();
        //                    #endregion
        //                }

        //                oPdfPCell = new PdfPCell(new Phrase("Yarn Type: " + oDCDetail.ProductName, _oFontStyleBold));
        //                oPdfPCell.Colspan = 8;
        //                oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
        //                oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
        //                oPdfPTable.AddCell(oPdfPCell);
        //                oPdfPTable.CompleteRow();

        //                oPdfPCell = new PdfPCell(new Phrase("Date", _oFontStyleBold));
        //                oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
        //                oPdfPTable.AddCell(oPdfPCell);

        //                oPdfPCell = new PdfPCell(new Phrase("Challan No", _oFontStyleBold));
        //                oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //                oPdfPTable.AddCell(oPdfPCell);

        //                oPdfPCell = new PdfPCell(new Phrase("COLOR", _oFontStyleBold));
        //                oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //                oPdfPTable.AddCell(oPdfPCell);

        //                oPdfPCell = new PdfPCell(new Phrase("Lot", _oFontStyleBold));
        //                oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //                oPdfPTable.AddCell(oPdfPCell);

        //                oPdfPCell = new PdfPCell(new Phrase("SHADE", _oFontStyleBold));
        //                oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //                oPdfPTable.AddCell(oPdfPCell);

        //                oPdfPCell = new PdfPCell(new Phrase(" Qty (" + _oExportSCDO.MUName + ")", _oFontStyleBold));
        //                oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //                oPdfPTable.AddCell(oPdfPCell);

        //                oPdfPCell = new PdfPCell(new Phrase("Unit Price(" + _oExportSCDO.Currency + ")", _oFontStyleBold));
        //                oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //                oPdfPTable.AddCell(oPdfPCell);


        //                oPdfPCell = new PdfPCell(new Phrase("Amount(" + _oExportSCDO.Currency + ")", _oFontStyleBold));
        //                oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //                oPdfPTable.AddCell(oPdfPCell);



        //                oPdfPTable.CompleteRow();
        //                #endregion
        //                _nTotalQty = 0;
        //                _nTotalAmount = 0;
        //                _nCount = 0;
        //            }
        //            #region PrintDetail
        //            //List<DyeingOrderDetail> oDyeingOrderDetails = new List<DyeingOrderDetail>();
        //            //_oDyeingOrderDetails = _oDyeingOrderDetails.Where(o => o.ProductID == oChallan.ProductID).ToList();

        //            //for (int i = 0; i < oDyeingOrderDetails.Count; i++)
        //            //{
        //            _nCount++;
        //            oPdfPCell = new PdfPCell(new Phrase(oDCDetail.ChallanDate, _oFontStyle));
        //            oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
        //            oPdfPTable.AddCell(oPdfPCell);

        //            oPdfPCell = new PdfPCell(new Phrase(oDCDetail.ChallanNo, _oFontStyle));
        //            oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
        //            oPdfPTable.AddCell(oPdfPCell);

        //            oPdfPCell = new PdfPCell(new Phrase(oDCDetail.ColorName, _oFontStyle));
        //            oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
        //            oPdfPTable.AddCell(oPdfPCell);

        //            oPdfPCell = new PdfPCell(new Phrase(oDCDetail.LotNo, _oFontStyle));
        //            oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //            oPdfPTable.AddCell(oPdfPCell);

        //            oPdfPCell = new PdfPCell(new Phrase(((EnumShade)oDCDetail.Shade).ToString(), _oFontStyle));
        //            oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
        //            oPdfPTable.AddCell(oPdfPCell);



        //            oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oDCDetail.Qty), _oFontStyle));
        //            oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //            oPdfPTable.AddCell(oPdfPCell);

        //            oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oDCDetail.UnitPrice), _oFontStyle));
        //            oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //            oPdfPTable.AddCell(oPdfPCell);

        //            oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oDCDetail.Qty * oDCDetail.UnitPrice), _oFontStyle));
        //            oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //            oPdfPTable.AddCell(oPdfPCell);




        //            _nTotalAmount = _nTotalAmount + (oDCDetail.Qty * oDCDetail.UnitPrice);
        //            _nTotalQty = _nTotalQty + oDCDetail.Qty;
        //            _nGrandTotalAmount = _nGrandTotalAmount + (oDCDetail.Qty * oDCDetail.UnitPrice);
        //            _nGrandTotalQty = _nGrandTotalQty + oDCDetail.Qty;
        //            oPdfPTable.CompleteRow();
        //            #endregion

        //            nProductID = oDCDetail.ProductID;

        //        }

        //        #region Total
        //        _oPdfPCell = new PdfPCell(new Phrase("Total: ", _oFontStyleBold));
        //        _oPdfPCell.Colspan = 5;
        //        //_oPdfPCell.Border = 0;
        //        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //        oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalQty), _oFontStyleBold));
        //        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //        //_oPdfPCell.Border = 0;
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //        oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
        //        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //        //_oPdfPCell.Border = 0;
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //        oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase(_oExportSCDO.Currency + "" + Global.MillionFormat(_nTotalAmount), _oFontStyleBold));
        //        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //        //_oPdfPCell.Border = 0;
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //        oPdfPTable.AddCell(_oPdfPCell);



        //        oPdfPTable.CompleteRow();
        //        #endregion

        //        #region Grand Total
        //        _oPdfPCell = new PdfPCell(new Phrase("Grand Total: ", _oFontStyleBold));
        //        _oPdfPCell.Colspan = 5;
        //        //_oPdfPCell.Border = 0;
        //        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //        oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nGrandTotalQty) + "" + _oExportSCDO.MUName, _oFontStyleBold));
        //        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //        //_oPdfPCell.Border = 0;
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //        oPdfPTable.AddCell(_oPdfPCell);



        //        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
        //        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //        //_oPdfPCell.Border = 0;
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //        oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase(_oExportSCDO.Currency + "" + Global.MillionFormat(_nGrandTotalAmount), _oFontStyleBold));
        //        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //        //_oPdfPCell.Border = 0;
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //        oPdfPTable.AddCell(_oPdfPCell);

        //        oPdfPTable.CompleteRow();
        //        #endregion
        //    }
        //    #endregion Delivery challan

        //    //#region SetDyeingOrderDetail
        //    //_oPdfPCell = new PdfPCell(oPdfPTable);
        //    //_oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
        //    //_oPdfPTable.CompleteRow();
        //    //#endregion
        //    return oPdfPTable;
        //}
      
     
     
       
     
        private PdfPTable PrintFooter()
        {
            //_oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, 1);

            PdfPTable oPdfPTable = new PdfPTable(4);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 148f, 148f, 148f, 148f });

          

        
            oPdfPCell = new PdfPCell(new Phrase("Prepared By", FontFactory.GetFont("Tahoma", 8f, 2)));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0.5f;
            oPdfPCell.FixedHeight = 35f;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

       

            oPdfPCell = new PdfPCell(new Phrase("Sr. Executive", FontFactory.GetFont("Tahoma", 8f, 2)));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0.5f;
            oPdfPCell.FixedHeight = 35f;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("MKT. Manager", FontFactory.GetFont("Tahoma", 8f, 2)));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0.5f;
            oPdfPCell.FixedHeight = 35f;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Managing Director", FontFactory.GetFont("Tahoma", 8f, 2)));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0.5f;
            oPdfPCell.FixedHeight = 35f;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

           

            oPdfPTable.CompleteRow();

            //oPdfPCell = new PdfPCell(new Phrase("", FontFactory.GetFont("Tahoma", 8f, 2)));
            //oPdfPCell.Border = 0;
            //oPdfPCell.Colspan = 4;
            //oPdfPCell.FixedHeight = 35f;
            //oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //oPdfPTable.AddCell(oPdfPCell);

            //oPdfPTable.CompleteRow();

            return oPdfPTable;
        }
        private void Blank(int nFixedHeight)
        {

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Colspan = 1; _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = nFixedHeight; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }

    }
}
