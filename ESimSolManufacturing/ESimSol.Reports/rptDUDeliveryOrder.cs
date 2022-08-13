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

    public class rptDUDeliveryOrder
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
        DUDeliveryOrder _oDUDeliveryOrder = new DUDeliveryOrder();
        List<DUDeliveryOrderDetail> _oDUDeliveryOrderDetails = new List<DUDeliveryOrderDetail>();
        List<DUDeliveryOrderDetail> _oDUDeliveryOrderDetails_Temp = new List<DUDeliveryOrderDetail>();
        BusinessUnit _oBusinessUnit = new BusinessUnit();
        DUOrderSetup _oDUOrderSetup = new DUOrderSetup();
        DUDyeingStep _oDUDyeingStep = new DUDyeingStep();
        Phrase _oPhrase = new Phrase();
        Company _oCompany = new Company();
        string _sWaterMark = "";
        string _sMUnit = "";
        float nUsagesHeight = 0;
        int _nCount = 0;
        int _nCount_P = 0;
      
        double _nTotalQty = 0;
        double _nQty = 0;

        double _nGrandTotalQty = 0;
        #endregion
        #region Report One
        public byte[] PrepareReport(DUDeliveryOrder oDUDeliveryOrder, Company oCompany, BusinessUnit oBusinessUnit, List<DUDeliveryOrderDetail> oDUDeliveryOrderDetails_Previous)
        {
            _oDUDeliveryOrder = oDUDeliveryOrder;
            _oBusinessUnit = oBusinessUnit;
            _oDUDeliveryOrderDetails = oDUDeliveryOrder.DUDeliveryOrderDetails;
            //_oDUDeliveryOrderDetails_Previous = oDUDeliveryOrderDetails_Previous;
            _oCompany = oCompany;
            if (_oDUDeliveryOrderDetails.Count>0)
            {
                _sMUnit = _oDUDeliveryOrderDetails[0].MUName;
                if (string.IsNullOrEmpty(_sMUnit))
                { _sMUnit = "LBS"; };
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
            this.PrintWaterMark(30f, 30f, 30f, 30f);
            this.PrintBody();
            _oPdfPTable.HeaderRows = 4;

            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Print Header
        private void PrintHeader()
        {
            #region CompanyHeader


            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 65f, 330.5f, 65f });

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
        #endregion

        #region Report Header
        private void ReporttHeader()
        {
            string sHeaderName = "";
            #region Proforma Invoice Heading Print
            if (_oDUDeliveryOrder.OrderType == (int)EnumOrderType.BulkOrder)
            {
                sHeaderName = "BULK-DELIVERY ORDER";
            }
            else if (_oDUDeliveryOrder.OrderType == (int)EnumOrderType.SampleOrder)
            {
                sHeaderName = "SAMPLE-DELIVERY ORDER";
            }
          
            else
            {
             
                if (!String.IsNullOrEmpty(_oDUOrderSetup.ShortName))
                {
                    _oDUOrderSetup.ShortName = _oDUOrderSetup.ShortName.ToUpper();
                    _oDUOrderSetup.ShortName = _oDUOrderSetup.ShortName.Replace("ORDER","");
                    sHeaderName = _oDUOrderSetup.ShortName + "-DELIVERY ORDER";
                }
                else
                {
                    sHeaderName = "DELIVERY ORDER";
                }
            }

            _oPdfPCell = new PdfPCell(new Phrase(sHeaderName, FontFactory.GetFont("Tahoma", 15f, iTextSharp.text.Font.BOLD)));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.FixedHeight = 20; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            if (_oDUDeliveryOrder.ApproveBy == 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Unauthorised Order", FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLDITALIC)));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.FixedHeight = 20; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.Colspan = _nTotalColumn;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }

            #endregion
        }
        #endregion


        #region Report Body
        private void PrintBody()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, 1);
            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 9f, 4);


            #region Short Info
            if (_oDUDeliveryOrder.OrderType == (int)(EnumOrderType.BulkOrder) || _oDUDeliveryOrder.OrderType == (int)(EnumOrderType.DyeingOnly))
            {
                _oPdfPCell = new PdfPCell(this.PrintHead_PI());
            }
            else
            {
                _oPdfPCell = new PdfPCell(this.PrintHead_Sample());
            }
            
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion
            #region Balank Space
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            #region Note
            if (_oDUDeliveryOrder.OrderType == (int)EnumOrderType.LoanOrder)
            {
                _oPdfPCell = new PdfPCell(this.Set_NoteLoanOrder());
            }
            else
            {
                _oPdfPCell = new PdfPCell(this.Set_Note());
            }
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion
            #region Product Details

            if (_oDUDeliveryOrder.OrderType == (int)EnumOrderType.LoanOrder)
            {
                _oPdfPCell = new PdfPCell(this.SetDUDeliveryOrderDetailLoan());
            }
            else
            {
                _oPdfPCell = new PdfPCell(this.SetDUDeliveryOrderDetail());
            }
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Note

            _oPdfPCell = new PdfPCell(this.Set_NoteTwo());
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

            #region Balank Space
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion          

            #region Balank Space
            _oPdfPCell = new PdfPCell(this.PrintFooter_C());
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            
        }
        #endregion

        private PdfPTable PrintHead_Sample()
        {
            //_oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, 1);

            PdfPTable oPdfPTable = new PdfPTable(3);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 100, 140f, 100f });

            oPdfPCell = new PdfPCell(new Phrase("DATE:", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oDUDeliveryOrder.DODateSt, _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.Colspan = 2;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("DELIVERY ORDER NO", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase( _oDUDeliveryOrder.DONoFull, FontFactory.GetFont("Tahoma", 11f, 1)));
            //oPdfPCell.Border = 0;
            oPdfPCell.Colspan = 2;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();
            if (_oDUDeliveryOrder.OrderType == (int)EnumOrderType.LoanOrder)
            {
                oPdfPCell = new PdfPCell(new Phrase("BUYER/SUPPLIER NAME", _oFontStyle));
            }
            else { oPdfPCell = new PdfPCell(new Phrase("BUYER NAME", _oFontStyle)); }
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oDUDeliveryOrder.DeliveryToName, FontFactory.GetFont("Tahoma", 11f, 1)));
            //oPdfPCell.Border = 0;
            oPdfPCell.Colspan = 2;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            _oDUDeliveryOrder.OrderTypeSt = _oDUDeliveryOrder.OrderTypeSt.Replace("Order", "");
             oPdfPCell = new PdfPCell(new Phrase(_oDUDeliveryOrder.OrderTypeSt + "  Order No", _oFontStyle));
            
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oDUDeliveryOrder.OrderNo, FontFactory.GetFont("Tahoma", 11f, 1)));
            //oPdfPCell.Border = 0;
            oPdfPCell.Colspan = 2;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            if (!String.IsNullOrEmpty(_oDUDeliveryOrder.ExportPINo))
            {
                
            oPdfPCell = new PdfPCell(new Phrase("P/I No", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oDUDeliveryOrder.ExportPINo, FontFactory.GetFont("Tahoma", 11f, 1)));
            //oPdfPCell.Border = 0;
            oPdfPCell.Colspan = 2;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();
            }


            oPdfPCell = new PdfPCell(new Phrase("MODE OF PAYMENT", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            string sTemp = "";
            string sTempTwo = "";
            if (_oDUDeliveryOrder.PaymentType == (int)EnumOrderPaymentType.CashOrCheque)
            {
                sTemp = "Cheque or Cash";
                if (!string.IsNullOrEmpty(_oDUDeliveryOrder.ExportPINo))
                {
                    sTempTwo = "MR No:" + _oDUDeliveryOrder.ExportPINo;
                }
            }
            if (_oDUDeliveryOrder.PaymentType == (int)EnumOrderPaymentType.AdjWithNextLC)
            {
                sTemp = "NEXT L/C ADJUST ";

                if (!string.IsNullOrEmpty(_oDUDeliveryOrder.ExportLCNo))
                {
                    sTempTwo = "L/C No:" + _oDUDeliveryOrder.ExportLCNo;
                }
            }
            if (_oDUDeliveryOrder.PaymentType == (int)EnumOrderPaymentType.AdjWithPI)
            {
                sTemp = "Adj With PI ";
                if (!string.IsNullOrEmpty(_oDUDeliveryOrder.ExportLCNo))
                {
                    sTempTwo = "L/C No:" + _oDUDeliveryOrder.ExportLCNo;
                }
            }
            if (_oDUDeliveryOrder.PaymentType == (int)EnumOrderPaymentType.FoC)
            {
                sTemp = "Free Of Cost";
                if (!string.IsNullOrEmpty(_oDUDeliveryOrder.ExportPINo))
                {
                    sTempTwo = "MR No:" + _oDUDeliveryOrder.ExportPINo;
                }
            }

            oPdfPCell = new PdfPCell(new Phrase(" " + sTemp + " " + sTempTwo, _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.Colspan = 2;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();
          

            return oPdfPTable;
        }
        private PdfPTable PrintHead_PI()
        {
            //_oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, 1);

            PdfPTable oPdfPTable = new PdfPTable(3);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 70, 190f, 80f });

            oPdfPCell = new PdfPCell(new Phrase("DATE:", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPCell = new PdfPCell(new Phrase(" " + _oDUDeliveryOrder.DODateSt, _oFontStyle));
            //oPdfPCell.Border = 0;
           
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" # With Acceptance" , _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("DELIVERY ORDER NO", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(_oDUDeliveryOrder.DONoFull, FontFactory.GetFont("Tahoma", 11f, 1)));
            //oPdfPCell.Border = 0;
            //oPdfPCell.Colspan = 2;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

           

            oPdfPCell = new PdfPCell(new Phrase("BUYER NAME", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oDUDeliveryOrder.DeliveryToName, FontFactory.GetFont("Tahoma", 11f, 1)));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" #With UD", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();


            oPdfPCell = new PdfPCell(new Phrase("P/I NO", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oDUDeliveryOrder.OrderNo, _oFontStyleBold));
            //oPdfPCell.Border = 0;
            //oPdfPCell.Colspan = 2;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();


            oPdfPCell = new PdfPCell(new Phrase("L/C NO", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oDUDeliveryOrder.ExportLCNo, _oFontStyleBold));
            //oPdfPCell.Border = 0;
            //oPdfPCell.Colspan = 2;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" #Without Maturity", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            return oPdfPTable;
        }
        private PdfPTable Set_Note()
        {
            //_oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, 1);

            PdfPTable oPdfPTable = new PdfPTable(1);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 595f });

            oPdfPCell = new PdfPCell(new Phrase("WE WOULD LIKE TO INFORM YOU THAT THE ABOVE MENTIONED YARN IS READY FOR DELIVERY IN OUR FACTORY. YOU CAN TAKE DELIVERY  FROM OUR FACTORY BY GIVING NECESSARY PAPER(ACCEPTANCE,U.D., MASTER L/C)", _oFontStyle));
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();
         
            return oPdfPTable;
        }
        private PdfPTable Set_NoteLoanOrder()
        {
            //_oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, 1);

            PdfPTable oPdfPTable = new PdfPTable(1);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 595f });

            oPdfPCell = new PdfPCell(new Phrase("WE WOULD LIKE TO INFORM YOU THAT THE ABOVE MENTIONED GOODS IS READY FOR DELIVERY IN OUR FACTORY. YOU CAN TAKE DELIVERY  FROM OUR FACTORY BY GIVING NECESSARY PAPER", _oFontStyle));
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            return oPdfPTable;
        }
        private PdfPTable Set_NoteTwo()
        {
            //_oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, 1);

            PdfPTable oPdfPTable = new PdfPTable(1);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 595f });

            _oPhrase = new Phrase();
            if (!String.IsNullOrEmpty(_oDUDeliveryOrder.Note))
            {
                _oPhrase.Add(new Chunk("Remarks : ", _oFontStyle));
                _oPhrase.Add(new Chunk(_oDUDeliveryOrder.Note, _oFontStyleBold));

                oPdfPCell = new PdfPCell(_oPhrase);
                //oPdfPCell.Border = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();
            }

            //oPdfPCell = new PdfPCell(new Phrase("NOTE: ", _oFontStyleBold));
            //oPdfPCell.Border = 0;
            //oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //oPdfPTable.AddCell(oPdfPCell);
            //oPdfPTable.CompleteRow();

            //oPdfPCell = new PdfPCell(new Phrase("1. Authorize person must check the goods quality as per delivery challan at the time of taking delivery from factory. ", _oFontStyle));
            //oPdfPCell.Border = 0;
            //oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //oPdfPTable.AddCell(oPdfPCell);
            //oPdfPTable.CompleteRow();
            //oPdfPCell = new PdfPCell(new Phrase("2. Your authorized person to take delivery, must submit (1) Delivery Notice (2) Authorization Letter to factory office.", _oFontStyle));
            //oPdfPCell.Border = 0;
            //oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //oPdfPTable.AddCell(oPdfPCell);
            //oPdfPTable.CompleteRow();

            return oPdfPTable;
        }
   
        private PdfPTable SetDUDeliveryOrderDetail()
        {
           
            _nTotalQty = 0;

            PdfPTable oPdfPTable = new PdfPTable(3);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 200f, 90f, 70f });
            int nProductID = 0;
            _nTotalQty = 0;
            
            _nCount = 0;

         
            if (_oDUDeliveryOrderDetails.Count > 0)
            {

                _oDUDeliveryOrderDetails = _oDUDeliveryOrderDetails.OrderBy(o => o.ProductID).ToList();

                oPdfPCell = new PdfPCell(new Phrase("DESCRIPTION OF GOODS", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);
                if (_oDUDeliveryOrder.OrderType == (int)EnumOrderType.BulkOrder || _oDUDeliveryOrder.OrderType == (int)EnumOrderType.DyeingOnly)
                {
                    oPdfPCell = new PdfPCell(new Phrase("COLOR/ORDER No", _oFontStyleBold));
                }
                else
                {
                    oPdfPCell = new PdfPCell(new Phrase("COLOR", _oFontStyleBold));
                }
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);


                oPdfPCell = new PdfPCell(new Phrase(" Qty (" + _sMUnit + ")", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPTable.CompleteRow();

                foreach (DUDeliveryOrderDetail oDUDeliveryOrderDetail in _oDUDeliveryOrderDetails)
                {
                    #region PrintDetail
                  
                        _nCount++;
                        if (nProductID != oDUDeliveryOrderDetail.ProductID)
                        {
                            if (nProductID > 0)
                            {
                                _nQty = _oDUDeliveryOrderDetails.Where(x => x.ProductID == nProductID).Sum(x => x.Qty);

                                oPdfPCell = new PdfPCell(new Phrase("Yarn Total", FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLDITALIC)));
                                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                                //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f;
                                oPdfPCell.Colspan = 2;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                                oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nQty), _oFontStyleBold));
                                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                                //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                                oPdfPTable.AddCell(oPdfPCell);
                                
                                oPdfPTable.CompleteRow();
                            }

                            _nCount_P = _oDUDeliveryOrderDetails.Where(x => x.ProductID == oDUDeliveryOrderDetail.ProductID).Count();
                            oPdfPCell = new PdfPCell(new Phrase(" " + oDUDeliveryOrderDetail.ProductName, FontFactory.GetFont("Tahoma", 11f, 1)));
                            //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f;
                            oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            //oPdfPCell.FixedHeight = 40f;
                            oPdfPCell.Rowspan = _nCount_P;
                            oPdfPTable.AddCell(oPdfPCell);
                        }

                        oPdfPCell = new PdfPCell(new Phrase(oDUDeliveryOrderDetail.ColorName, _oFontStyle));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f;
                        //oPdfPCell.FixedHeight = 40f;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oDUDeliveryOrderDetail.Qty), _oFontStyle));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f;
                        //oPdfPCell.FixedHeight = 40f;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        oPdfPTable.AddCell(oPdfPCell);
                        _nTotalQty = _nTotalQty + oDUDeliveryOrderDetail.Qty;
                        _nGrandTotalQty = _nGrandTotalQty + oDUDeliveryOrderDetail.Qty;
                        oPdfPTable.CompleteRow();
                    #endregion
                    nProductID = oDUDeliveryOrderDetail.ProductID;

                }
                if (nProductID > 0)
                {
                    _nQty = _oDUDeliveryOrderDetails.Where(x => x.ProductID == nProductID).Sum(x => x.Qty);

                    oPdfPCell = new PdfPCell(new Phrase("Yarn Total", FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLDITALIC)));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f;
                    oPdfPCell.Colspan = 2;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nQty), _oFontStyleBold));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);
                    
                    oPdfPTable.CompleteRow();
                }

                for (int i = _oDUDeliveryOrderDetails.Count + 1; i <= (50 - _oDUDeliveryOrderDetails.Count); i++)
                {
                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft =0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
                    oPdfPTable.CompleteRow();
                }
                //#region Total
                //_oPdfPCell = new PdfPCell(new Phrase("Yarn Total: ", _oFontStyleBold));
                //_oPdfPCell.Colspan = 5;
                ////_oPdfPCell.Border = 0;
                //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                //oPdfPTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalQty), _oFontStyleBold));
                //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
                ////_oPdfPCell.Border = 0;
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                //oPdfPTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
                ////_oPdfPCell.Border = 0;
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                //oPdfPTable.AddCell(_oPdfPCell);

                //oPdfPTable.CompleteRow();
                //#endregion

                #region Grand Total
                _oPdfPCell = new PdfPCell(new Phrase("TOTAL DELIVERY ORDERED QTY", _oFontStyleBold));
              
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nGrandTotalQty), _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);



                oPdfPTable.CompleteRow();
                #endregion
                #region In Word
                string sTemp = "";
                sTemp = Global.DollarWords(_nGrandTotalQty);
                if (!String.IsNullOrEmpty(sTemp))
                {
                    sTemp = sTemp.Replace("Dollar", "");
                    sTemp = sTemp.Replace("Only", "");
                    sTemp = sTemp.ToUpper();
                }
                _oPdfPCell = new PdfPCell(new Phrase("IN WORDS: " + sTemp + " " + _sMUnit + " ONLY", _oFontStyleBold));
                _oPdfPCell.Colspan =3;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(_oPdfPCell);

               


                oPdfPTable.CompleteRow();
                #endregion
                

            }
            return oPdfPTable;
        }

        private PdfPTable SetDUDeliveryOrderDetailLoan()
        {
           
            _nTotalQty = 0;

            PdfPTable oPdfPTable = new PdfPTable(4);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 200f, 90f, 30f,70f });
            int nProductID = 0;
            _nTotalQty = 0;
         
            _nCount = 0;


            if (_oDUDeliveryOrderDetails.Count > 0)
            {



                //oPdfPCell = new PdfPCell(new Phrase("Product Information", FontFactory.GetFont("Tahoma", 10f, 3)));
                //oPdfPCell.Colspan = 6;

                //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                //oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                //oPdfPTable.AddCell(oPdfPCell);
                //oPdfPTable.CompleteRow();
                _oDUDeliveryOrderDetails = _oDUDeliveryOrderDetails.OrderBy(o => o.ProductID).ToList();

                oPdfPCell = new PdfPCell(new Phrase("DESCRIPTION OF GOODS", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("BRAND/COLOR", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);


                oPdfPCell = new PdfPCell(new Phrase("Unit", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(" Qty ", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);


                oPdfPTable.CompleteRow();

                foreach (DUDeliveryOrderDetail oDUDeliveryOrderDetail in _oDUDeliveryOrderDetails)
                {



                    #region PrintDetail

                    _nCount++;
                    if (nProductID != oDUDeliveryOrderDetail.ProductID)
                    {
                        _nCount_P = _oDUDeliveryOrderDetails.Where(x => x.ProductID == oDUDeliveryOrderDetail.ProductID).Count();
                        oPdfPCell = new PdfPCell(new Phrase(" " + oDUDeliveryOrderDetail.ProductName, FontFactory.GetFont("Tahoma", 11f, 1)));
                        //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f;
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        //oPdfPCell.FixedHeight = 40f;
                        oPdfPCell.Rowspan = _nCount_P;
                        oPdfPTable.AddCell(oPdfPCell);
                    }

                    oPdfPCell = new PdfPCell(new Phrase(oDUDeliveryOrderDetail.ColorName, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f;
                    //oPdfPCell.FixedHeight = 40f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);


                    oPdfPCell = new PdfPCell(new Phrase(oDUDeliveryOrderDetail.MUName, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f;
                    //oPdfPCell.FixedHeight = 40f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oDUDeliveryOrderDetail.Qty), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f;
                    //oPdfPCell.FixedHeight = 40f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);
                    _nTotalQty = _nTotalQty + oDUDeliveryOrderDetail.Qty;
                    _nGrandTotalQty = _nGrandTotalQty + oDUDeliveryOrderDetail.Qty;
                    oPdfPTable.CompleteRow();
                    #endregion

                    nProductID = oDUDeliveryOrderDetail.ProductID;

                }
                for (int i = _oDUDeliveryOrderDetails.Count + 1; i <= (50 - _oDUDeliveryOrderDetails.Count); i++)
                {
                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
                    oPdfPTable.CompleteRow();
                }
                
                #region Grand Total
                _oPdfPCell = new PdfPCell(new Phrase("TOTAL DELIVERY ORDERED QTY", _oFontStyleBold));

                _oPdfPCell.Colspan = 3;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nGrandTotalQty), _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);



                oPdfPTable.CompleteRow();
                #endregion
                #region In Word
                string sTemp = "";
                sTemp = Global.DollarWords(_nGrandTotalQty);
                if (!String.IsNullOrEmpty(sTemp))
                {
                    sTemp = sTemp.Replace("Dollar", "");
                    sTemp = sTemp.Replace("Only", "");
                    sTemp = sTemp.ToUpper();
                    _sMUnit = _sMUnit.ToUpper();
                }
                _oPdfPCell = new PdfPCell(new Phrase("IN WORDS: " + sTemp + " " + _sMUnit + " ONLY", _oFontStyleBold));
                _oPdfPCell.Colspan = 4;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();
                #endregion

            }
            return oPdfPTable;
        }

        private PdfPTable PrintFooter()
        {
            //_oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, 1);

            PdfPTable oPdfPTable = new PdfPTable(3);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 230f, 110f, 140f });

            oPdfPCell = new PdfPCell(new Phrase("YOUR GOOD CO-OPERATION WILL APPRECIATED", _oFontStyleBold));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0.1f;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            oPdfPCell.FixedHeight = 5f; oPdfPCell.Colspan = 3; oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();


            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Name, _oFontStyleBold));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.1f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();


            return oPdfPTable;
        }
        private PdfPTable PrintFooter_C()
        {
            #region
            nUsagesHeight = CalculatePdfPTableHeight(_oPdfPTable);
            if (nUsagesHeight < 800)
            {
                nUsagesHeight = 800 - nUsagesHeight;
            }
            if (nUsagesHeight >10)
            {
                #region Blank Row


                while (nUsagesHeight < 680)
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
            #endregion


            PdfPTable oPdfPTable = new PdfPTable(3);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 197f, 197f, 197f });


            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCell.Border = 0;
            oPdfPCell.FixedHeight = 10;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCell.Border = 0;
            oPdfPCell.FixedHeight = 10;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCell.Border = 0;
            oPdfPCell.FixedHeight = 10;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();


            oPdfPCell = new PdfPCell(new Phrase(_oDUDeliveryOrder.PreaperByName, _oFontStyle));
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCell.Border = 0;
            oPdfPCell.FixedHeight = 15;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCell.Border = 0;
            oPdfPCell.FixedHeight = 15;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCell.Border = 0;
            oPdfPCell.FixedHeight = 15;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("__________________", _oFontStyle));
            oPdfPCell.Border = 0;
            //oPdfPCell.FixedHeight = 45f;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("_______________", _oFontStyle));
            oPdfPCell.Border = 0;
            //oPdfPCell.FixedHeight = 45f;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("_________________", _oFontStyle));
            oPdfPCell.Border = 0;
            //oPdfPCell.FixedHeight = 45f;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("Prepared By", _oFontStyleBold));
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("Checked By", _oFontStyleBold));
            oPdfPCell.Border = 0;

            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("Approved By", _oFontStyleBold));
            oPdfPCell.Border = 0;
            //oPdfPCell.FixedHeight = 35f;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            return oPdfPTable;
        }
        #endregion
        #region Report Two
        public byte[] PrepareReportTwo(DUDeliveryOrder oDUDeliveryOrder, Company oCompany, BusinessUnit oBusinessUnit, DUOrderSetup oDUOrderSetup, DUDyeingStep oDUDyeingStep)
        {
            _oDUDeliveryOrder = oDUDeliveryOrder;
            _oBusinessUnit = oBusinessUnit;
            _oDUDeliveryOrderDetails = oDUDeliveryOrder.DUDeliveryOrderDetails;
            _oCompany = oCompany;

            _oDUOrderSetup = oDUOrderSetup;
            _oDUDyeingStep = oDUDyeingStep;

            if (_oDUDeliveryOrderDetails.Count > 0)
            {
                _sMUnit = _oDUDeliveryOrderDetails[0].MUName;
               // _sMUnit = "kg";
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
            this.PrintWaterMark(30f, 30f, 30f, 30f);
            this.PrintBodyTwo();
            _oPdfPTable.HeaderRows = 4;

            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        #region Report Body
        private PdfPTable PrintHead_Two()
        {
            //_oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, 1);

            PdfPTable oPdfPTable = new PdfPTable(3);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 100, 140f, 100f });

            oPdfPCell = new PdfPCell(new Phrase("DATE:", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oDUDeliveryOrder.DODateSt, _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.Colspan = 2;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("DELIVERY ORDER NO", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(_oDUDeliveryOrder.DONoFull, FontFactory.GetFont("Tahoma", 11f, 1)));
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

            oPdfPCell = new PdfPCell(new Phrase(" " + _oDUDeliveryOrder.DeliveryToName, FontFactory.GetFont("Tahoma", 11f, 1)));
            //oPdfPCell.Border = 0;
            oPdfPCell.Colspan = 2;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            //oPdfPCell = new PdfPCell(new Phrase("Sample Order No", _oFontStyle));
            ////oPdfPCell.Border = 0;
            //oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //oPdfPTable.AddCell(oPdfPCell);

            //oPdfPCell = new PdfPCell(new Phrase(" " + _oDUDeliveryOrder.OrderNo, FontFactory.GetFont("Tahoma", 11f, 1)));
            ////oPdfPCell.Border = 0;
            //oPdfPCell.Colspan = 2;
            //oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //oPdfPTable.AddCell(oPdfPCell);
            //oPdfPTable.CompleteRow();

            if (!String.IsNullOrEmpty(_oDUDeliveryOrder.ExportPINo))
            {

                oPdfPCell = new PdfPCell(new Phrase("P/I No", _oFontStyle));
                //oPdfPCell.Border = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(" " + _oDUDeliveryOrder.ExportPINo, FontFactory.GetFont("Tahoma", 11f, 1)));
                //oPdfPCell.Border = 0;
                oPdfPCell.Colspan = 2;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();
            }


            oPdfPCell = new PdfPCell(new Phrase("MODE OF PAYMENT", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            string sTemp = "";
            string sTempTwo = "";
            if (_oDUDeliveryOrder.PaymentType == (int)EnumOrderPaymentType.CashOrCheque)
            {
                sTemp = "Cheque or Cash";
                if (!string.IsNullOrEmpty(_oDUDeliveryOrder.ExportPINo))
                {
                    sTempTwo = "MR No:" + _oDUDeliveryOrder.ExportPINo;
                }
            }
            if (_oDUDeliveryOrder.PaymentType == (int)EnumOrderPaymentType.AdjWithNextLC)
            {
                sTemp = "NEXT L/C ADJUST ";

                if (!string.IsNullOrEmpty(_oDUDeliveryOrder.ExportLCNo))
                {
                    sTempTwo = "L/C No:" + _oDUDeliveryOrder.ExportLCNo;
                }
            }
            if (_oDUDeliveryOrder.PaymentType == (int)EnumOrderPaymentType.AdjWithPI)
            {
                sTemp = "Adj With PI ";
                if (!string.IsNullOrEmpty(_oDUDeliveryOrder.ExportLCNo))
                {
                    sTempTwo = "L/C No:" + _oDUDeliveryOrder.ExportLCNo;
                }
            }
            if (_oDUDeliveryOrder.PaymentType == (int)EnumOrderPaymentType.FoC)
            {
                sTemp = "Free Of Cost";
                if (!string.IsNullOrEmpty(_oDUDeliveryOrder.ExportPINo))
                {
                    sTempTwo = "MR No:" + _oDUDeliveryOrder.ExportPINo;
                }
            }

            oPdfPCell = new PdfPCell(new Phrase(" " + sTemp + " " + sTempTwo, _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.Colspan = 2;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();


            return oPdfPTable;
        }
        private void PrintBodyTwo()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 9f, 1);
            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 9f, 4);


            #region Short Info
        
            _oPdfPCell = new PdfPCell(this.PrintHead_Two());
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion
            #region Balank Space
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            #region Note

            _oPdfPCell = new PdfPCell(this.Set_Note());
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion
            #region Product Details
            _oPdfPCell = new PdfPCell(this.SetDUDeliveryOrderDetailTwo());
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Note

            _oPdfPCell = new PdfPCell(this.Set_NoteTwo());
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

            #region Balank Space
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Balank Space
            _oPdfPCell = new PdfPCell(this.PrintFooter_B());
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion


        }
        private PdfPTable SetDUDeliveryOrderDetailTwo()
        {
           
            _nTotalQty = 0;

            PdfPTable oPdfPTable = new PdfPTable(4);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] {105f, 200f, 90f, 70f });
            int nProductID = 0;
            string sOrderNo = "";
            _nTotalQty = 0;
           
            _nCount = 0;
            double nTempQty = 0.0;

            if (_oDUDeliveryOrderDetails.Count > 0)
            {
                //_oDUDeliveryOrderDetails.ForEach(x => _oDUDeliveryOrderDetails.Add(x));
                _oDUDeliveryOrderDetails.ForEach(o => o.OrderNo = (string.IsNullOrEmpty(o.OrderNo)) ? _oDUDeliveryOrder.OrderNo : o.OrderNo);
                _nGrandTotalQty = (_oDUDeliveryOrderDetails.Sum(x => x.Qty));
                if (_oDUDeliveryOrder.OrderType == (int)EnumOrderType.BulkOrder || _oDUDeliveryOrder.OrderType == (int)EnumOrderType.DyeingOnly)
                {
                    _oDUDeliveryOrderDetails_Temp = _oDUDeliveryOrderDetails.GroupBy(x => new { x.OrderNo, x.ProductID, x.ProductName }, (key, grp) =>
                        new DUDeliveryOrderDetail
                        {
                            OrderNo = (string.IsNullOrEmpty(key.OrderNo)) ? _oDUDeliveryOrder.OrderNo : key.OrderNo,
                            ProductID = key.ProductID,
                            ProductName = key.ProductName,
                            ColorName = "Total",
                            Qty = grp.Sum(p => p.Qty)
                        }).ToList();

                    _oDUDeliveryOrderDetails_Temp.ForEach(x => _oDUDeliveryOrderDetails.Add(x));
                }

                _oDUDeliveryOrderDetails = _oDUDeliveryOrderDetails.OrderBy(o => o.OrderNo).ThenBy(o => o.ProductID).ToList();

                oPdfPCell = new PdfPCell(new Phrase("ORDER NO", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("YARN DESCRIPTION", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("COLOR", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);


                oPdfPCell = new PdfPCell(new Phrase(" Qty (" + _sMUnit + ")", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);



                oPdfPTable.CompleteRow();

                foreach (DUDeliveryOrderDetail oDUDeliveryOrderDetail in _oDUDeliveryOrderDetails)
                {
                    //if (oDUDeliveryOrderDetail.OrderNo != sOrderNo || nProductID != oDUDeliveryOrderDetail.ProductID)
                    //{
                    //    if (_nCount_P>1 && nProductID > 0)
                    //    {
                    //        //oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyle));
                    //        //oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //        //oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //        //oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //        //oPdfPTable.AddCell(oPdfPCell);

                    //        //oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyle));
                    //        //oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //        //oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //        //oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //        //oPdfPTable.AddCell(oPdfPCell);

                    //        nTempQty = (_oDUDeliveryOrderDetails.Where(c => c.ProductID == nProductID && c.OrderNo == sOrderNo).Sum(x => x.Qty));
                    //        oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyleBold));
                    //        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //        oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //        oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //        oPdfPTable.AddCell(oPdfPCell);

                    //        oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTempQty), _oFontStyleBold));
                    //        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //        oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //        oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //        oPdfPTable.AddCell(oPdfPCell);
                    //        oPdfPTable.CompleteRow();
                    //        _nCount_P ++;
                    //        _nCount++;
                    //    }
                    //}
                 

                    #region PrintDetail

                    //_nCount++;
                    if (oDUDeliveryOrderDetail.OrderNo != sOrderNo )
                    {
                        _nCount = _oDUDeliveryOrderDetails.Where(x => x.OrderNo == oDUDeliveryOrderDetail.OrderNo).Count();
                        oPdfPCell = new PdfPCell(new Phrase(" " + oDUDeliveryOrderDetail.OrderNo, FontFactory.GetFont("Tahoma", 11f, 1)));
                        //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f;
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        //oPdfPCell.FixedHeight = 40f;
                        oPdfPCell.Rowspan = (_nCount > 0) ? _nCount : 1;
                        oPdfPTable.AddCell(oPdfPCell);
                    }

                    if (oDUDeliveryOrderDetail.OrderNo!=sOrderNo || nProductID != oDUDeliveryOrderDetail.ProductID)
                    {
                       _nCount_P = _oDUDeliveryOrderDetails.Where(x => x.ProductID == oDUDeliveryOrderDetail.ProductID && x.OrderNo == oDUDeliveryOrderDetail.OrderNo).Count();
                       oPdfPCell = new PdfPCell(new Phrase(" " + oDUDeliveryOrderDetail.ProductName, _oFontStyle));
                        //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f;
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        //oPdfPCell.FixedHeight = 40f;
                        oPdfPCell.Rowspan = (_nCount_P > 0) ? _nCount_P : 1;
                        oPdfPTable.AddCell(oPdfPCell);
                    }

                    oPdfPCell = new PdfPCell(new Phrase(oDUDeliveryOrderDetail.ColorName, (oDUDeliveryOrderDetail.ColorName == "Total") ? _oFontStyleBold : _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f;
                    //oPdfPCell.FixedHeight = 40f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oDUDeliveryOrderDetail.Qty), (oDUDeliveryOrderDetail.ColorName == "Total") ? _oFontStyleBold : _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f;
                    //oPdfPCell.FixedHeight = 40f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);
                    _nTotalQty = _nTotalQty + oDUDeliveryOrderDetail.Qty;
                    //_nGrandTotalQty = _nGrandTotalQty + oDUDeliveryOrderDetail.Qty;
                    oPdfPTable.CompleteRow();
                    //oPdfPTable.Rows.RemoveAt(1);
                    #endregion
                   


                  

                 
                    nProductID = oDUDeliveryOrderDetail.ProductID;
                    sOrderNo = oDUDeliveryOrderDetail.OrderNo;
                }
                
           
                for (int i = _oDUDeliveryOrderDetails.Count + 1; i <= (5 - _oDUDeliveryOrderDetails.Count); i++)
                {
                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
                    oPdfPTable.CompleteRow();
                }
               

                #region Grand Total
                _oPdfPCell = new PdfPCell(new Phrase("TOTAL DELIVERY ORDERED QTY", _oFontStyleBold));

                _oPdfPCell.Colspan = 3;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nGrandTotalQty), _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);



                oPdfPTable.CompleteRow();
                #endregion
                #region In Word
                string sTemp = "";
                sTemp = Global.DollarWords(_nGrandTotalQty);
                if (!String.IsNullOrEmpty(sTemp))
                {
                    sTemp = sTemp.Replace("Dollar", "");
                    sTemp = sTemp.Replace("Only", "");
                    sTemp = sTemp.ToUpper();
                }
                _oPdfPCell = new PdfPCell(new Phrase("IN WORDS: " + sTemp + " " + _sMUnit + " ONLY", _oFontStyleBold));
                _oPdfPCell.Colspan = 4;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
                #endregion


            }
            return oPdfPTable;
        }
        private PdfPTable PrintFooter_B()
        {
           


            PdfPTable oPdfPTable = new PdfPTable(3);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 197f, 197f, 197f });


            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCell.Border = 0;
            oPdfPCell.FixedHeight = 10;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCell.Border = 0;
            oPdfPCell.FixedHeight = 10;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCell.Border = 0;
            oPdfPCell.FixedHeight = 10;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();


            oPdfPCell = new PdfPCell(new Phrase(_oDUDeliveryOrder.PreaperByName, _oFontStyle));
            oPdfPCell.Border = 0;
            oPdfPCell.MinimumHeight = 10f;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCell.Border = 0;
            oPdfPCell.MinimumHeight = 10;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(_oDUDeliveryOrder.ApproveByName, _oFontStyle));
            oPdfPCell.Border = 0;
            oPdfPCell.MinimumHeight = 10f;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCell.Border = 0;
            oPdfPCell.FixedHeight = 15;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCell.Border = 0;
            oPdfPCell.FixedHeight = 15;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCell.Border = 0;
            oPdfPCell.FixedHeight = 15;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("__________________", _oFontStyle));
            oPdfPCell.Border = 0;
            //oPdfPCell.FixedHeight = 45f;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("_______________", _oFontStyle));
            oPdfPCell.Border = 0;
            //oPdfPCell.FixedHeight = 45f;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("_________________", _oFontStyle));
            oPdfPCell.Border = 0;
            //oPdfPCell.FixedHeight = 45f;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("Prepared By", _oFontStyle));
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("Checked By", _oFontStyleBold));
            oPdfPCell.Border = 0;

            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("Approved By", _oFontStyleBold));
            oPdfPCell.Border = 0;
            //oPdfPCell.FixedHeight = 35f;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);



            oPdfPTable.CompleteRow();

            return oPdfPTable;
        }
        #endregion
    
        #endregion
        private void PrintWaterMark(float marginLeft, float marginRight, float marginTop, float marginBottom)
        {
            if (_oDUDeliveryOrder.ApproveBy == 0)
            {
                _sWaterMark = "Unauthorised";
            }
            if (_oDUDeliveryOrder.DOStatus == (int)EnumDOStatus.Cancel)
            {
                _sWaterMark = "Cancelled";
            }
            _oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
            _oDocument.SetMargins(marginLeft, marginRight, marginTop, marginBottom);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;

            ESimSolWM_Footer.WMFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            ESimSolWM_Footer.WMFontSize = 80;
            ESimSolWM_Footer.WMRotation = 45;
            ESimSolWM_Footer PageEventHandler = new ESimSolWM_Footer();
            PageEventHandler.WaterMark = _sWaterMark; //Footer print with page event handler
            //PageEventHandler.FooterNote = _oBusinessUnit.ShortName + " Concern Person: " + _oDyeingOrder.MKTPName;
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler

            _oDocument.Open();
            PdfContentByte cb = PDFWriter.DirectContent;
            _oDocument.NewPage();
        }
        private void Blank(int nFixedHeight)
        {

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Colspan = 1; _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = nFixedHeight; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
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
    }
}
