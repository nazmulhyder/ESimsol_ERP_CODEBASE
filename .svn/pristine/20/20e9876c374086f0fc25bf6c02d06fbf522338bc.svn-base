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
    public class rptSampleInvoice
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
        SampleInvoice _oSampleInvoice = new SampleInvoice();
        List<DyeingOrderDetail> _oDyeingOrderDetails = new List<DyeingOrderDetail>();
        BusinessUnit _oBusinessUnit = new BusinessUnit();
        ExportSCDO _oExportSCDO = new ExportSCDO();
        Company _oCompany = new Company();
        List<DyeingOrderReport> _oDyeingOrderReports = new List<DyeingOrderReport>();
        SampleInvoiceSetup _oSampleInvoiceSetup = new SampleInvoiceSetup();
        List<DyeingOrder> _oDyeingOrders = new List<DyeingOrder>();
        string _sMessage = "";

        int _nCount = 0;
        double _nTotalAmount = 0;
        double _nTotalQty = 0;

        double _nGrandTotalAmount = 0;
        double _nGrandTotalQty = 0;
        double _nTotalAmountCRate = 0;
        double _nGrandTotalCRate = 0;
        float nUsagesHeight = 0;
        string _sMunit = "", _sCurrency = "";
        int _nTitleType = 0;
        string _sWaterMark = "";
        #endregion

        public byte[] PrepareReport(SampleInvoice oSampleInvoice, Company oCompany, List<DyeingOrderDetail> oDyeingOrderDetails, BusinessUnit oBusinessUnit, bool bIsDebitNote, SampleInvoiceSetup oSampleInvoiceSetup, int nTitleType, List<DyeingOrder> oDyeingOrders)
        {
            _oSampleInvoice = oSampleInvoice;
            _oSampleInvoiceSetup = oSampleInvoiceSetup;
            _oDyeingOrderDetails = oDyeingOrderDetails;
            _oDyeingOrders = oDyeingOrders;
            _oCompany = oCompany;
            _oBusinessUnit = oBusinessUnit;
            _nTitleType = nTitleType;
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
            if (bIsDebitNote)
            {
               
                if (nTitleType == 1)//normal
                {
                    this.PrintHeader();
                }
                else if (nTitleType == 2)//Pad
                {
                    this.PrintHeader_Blank();
                }
                if (nTitleType == 3)//imge
                {
                    LoadCompanyTitle();
                }
                this.ReporttHeader();
                this.PrintWaterMark(30f, 30f, 30f, 30f);
                this.PrintHead_DebitNote();
                if (_oDyeingOrderDetails.Count > 0)
                {
                    this.Print_Body_DebitNote();
                }
                else
                {
                    this.Print_Body_SampleInvoice();
                }
                this.PrintFooter();
                if (_oSampleInvoice.PaymentType == (int)EnumOrderPaymentType.AdjWithNextLC)
                {
                    this.PrintFooterBuyer();
                }
            }
            else
            {
                this.PrintHeader();
                this.ReporttHeader();
                this.PrintWaterMark(30f, 30f, 30f, 30f);
                this.PrintHead_Invoice();
                if (_oDyeingOrderDetails.Count > 0)
                {
                    this.PrintBody();
                }
                else
                {
                    this.Print_Body_SampleInvoice();
                }
                this.PrintFooter();
            }
            _oPdfPTable.HeaderRows = 3;

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
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Rowspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oFontStyle = FontFactory.GetFont("Tahoma", 13f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Name, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));

            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 0);
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
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion
            #endregion

            #endregion
        }
        #endregion

        #region Report Body
        private void PrintBody()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, 1);
            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 9f, 4);

            #region Balank Space
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Short Info
            _oPdfPCell = new PdfPCell(this.SetDyeingOrderDetail());
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

            #region Balank Space
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion          

         

        }
        #endregion

        private void PrintHead_Invoice()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 9f, 1);
            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 9f, 4);

            PdfPTable oPdfPTable = new PdfPTable(3);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 100, 140f, 100f });

            oPdfPCell = new PdfPCell(new Phrase("DATE:", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oSampleInvoice.SampleInvoiceDateST, _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.Colspan = 2;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("SAMPLE INVOICE NO", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oSampleInvoice.InvoiceNo, _oFontStyleBold));
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

            oPdfPCell = new PdfPCell(new Phrase(" " + _oSampleInvoice.ContractorName, _oFontStyleBold));
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

            oPdfPCell = new PdfPCell(new Phrase(" " + _oSampleInvoice.RequirementDateST, _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.Colspan = 2;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("CONCERN PERSON", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oSampleInvoice.MKTPName, _oFontStyle));
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
            if (_oSampleInvoice.PaymentType == (int)EnumOrderPaymentType.CashOrCheque)
            {
                sTemp = "Cheque or Cash";
                if (!string.IsNullOrEmpty(_oSampleInvoice.ExportPINo))
                {
                    sTempTwo = "MR No:" + _oSampleInvoice.ExportPINo;
                }
            }
            if (_oSampleInvoice.PaymentType == (int)EnumOrderPaymentType.AdjWithNextLC)
            {
                sTemp = "NEXT L/C ADJUST ";

                if (!string.IsNullOrEmpty(_oSampleInvoice.ExportPINo))
                {
                    sTempTwo = "P/I No:" + _oSampleInvoice.ExportPINo;
                }
            }
            if (_oSampleInvoice.PaymentType == (int)EnumOrderPaymentType.AdjWithPI)
            {
                sTemp = "Adj With PI ";
                if (!string.IsNullOrEmpty(_oSampleInvoice.ExportPINo))
                {
                    sTempTwo = "P/I No:" + _oSampleInvoice.ExportPINo;
                }
            }
            if (_oSampleInvoice.PaymentType == (int)EnumOrderPaymentType.FoC)
            {
                sTemp = "Free Of Cost";
                if (!string.IsNullOrEmpty(_oSampleInvoice.ExportPINo))
                {
                    sTempTwo = "MR No:" + _oSampleInvoice.ExportPINo;
                }
            }

            oPdfPCell = new PdfPCell(new Phrase(" " + sTemp + " " + sTempTwo, _oFontStyle));


            //oPdfPCell.Border = 0;
            oPdfPCell.Colspan = 2;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

           // return oPdfPTable;
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        private void ReporttHeader()
        {
            #region Proforma Invoice Heading Print
            _oPdfPCell = new PdfPCell();
           
            if (_oSampleInvoice.ApproveBy == 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase(_oSampleInvoiceSetup.PrintName + "(Unauthorised)", FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD)));
            }
            else if (_oSampleInvoice.CurrentStatus == EnumSampleInvoiceStatus.Canceled)
            {
                _oPdfPCell = new PdfPCell(new Phrase(_oSampleInvoiceSetup.PrintName + "(Canceled)", FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD)));
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase(_oSampleInvoiceSetup.PrintName, FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD)));
            }
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
             _oPdfPCell.FixedHeight = 20; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            //if (_oSampleInvoice.ApproveBy == 0)
            //{
            //    _oPdfPCell = new PdfPCell(new Phrase("Unauthorised ", FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLDITALIC)));
            //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //    _oPdfPCell.FixedHeight = 20; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //    _oPdfPCell.Colspan = _nTotalColumn;
            //    _oPdfPTable.AddCell(_oPdfPCell);
            //    _oPdfPTable.CompleteRow();
            //}

            #endregion
        }

        private PdfPTable SetDyeingOrderDetail()
        {
            _nTotalAmount = 0;
            _nTotalQty = 0;

            PdfPTable oPdfPTable = new PdfPTable(8);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 100f, 120f, 100f, 50, 70f, 60f, 70f, 70f });
            int nProductID = 0;
            if (_oDyeingOrderDetails.Count > 0)
            {

                oPdfPCell = new PdfPCell(new Phrase("Product Information", FontFactory.GetFont("Tahoma", 10f, 3)));
                oPdfPCell.Colspan = 8;

                oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();
                _oDyeingOrderDetails = _oDyeingOrderDetails.OrderBy(o => o.ProductID).ToList();
                foreach (DyeingOrderDetail oDyeingOrderDetail in _oDyeingOrderDetails)
                {
                 
                    if (nProductID != oDyeingOrderDetail.ProductID)
                    {
                        #region Header

                        if (nProductID > 0)
                        {
                            #region Total
                            _oPdfPCell = new PdfPCell(new Phrase("Total: ", _oFontStyleBold));
                            _oPdfPCell.Colspan = 4;
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

                            _oPdfPCell = new PdfPCell(new Phrase("$" + Global.MillionFormat(_nTotalAmount), _oFontStyleBold));
                            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            //_oPdfPCell.Border = 0;
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            oPdfPTable.AddCell(_oPdfPCell);
                            if (_nTotalAmountCRate > 0)
                            {
                                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalAmountCRate), _oFontStyleBold));
                            }
                            else
                            {
                                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                            }
                            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            //_oPdfPCell.Border = 0;
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            oPdfPTable.AddCell(_oPdfPCell);

                            oPdfPTable.CompleteRow();
                            #endregion
                        }

                        oPdfPCell = new PdfPCell(new Phrase("Yarn Type: "+oDyeingOrderDetail.ProductNameCode, _oFontStyleBold));
                        oPdfPCell.Colspan = 8;
                        oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPTable.AddCell(oPdfPCell);
                        oPdfPTable.CompleteRow();

                        oPdfPCell = new PdfPCell(new Phrase("SAMPLE NO", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.GRAY;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase("COLOR", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.GRAY;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase("COLOR NO", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.GRAY;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase("SHADE", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.GRAY;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase("QTY(LBS)", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.GRAY;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase("U/P($)", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.GRAY;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);


                        oPdfPCell = new PdfPCell(new Phrase("Amount($)", _oFontStyleBold));
                        oPdfPCell.BackgroundColor = BaseColor.GRAY;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);
                        if (_oSampleInvoice.ConversionRate <= 1)
                        {
                            oPdfPCell = new PdfPCell(new Phrase("REMARKS", _oFontStyleBold));
                        }
                        else
                        {
                            oPdfPCell = new PdfPCell(new Phrase("Amount(" + _oSampleInvoice.CurrencySymbol+")", _oFontStyleBold));
                        }
                        oPdfPCell.BackgroundColor = BaseColor.GRAY;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPTable.CompleteRow();
                        #endregion
                        _nTotalQty = 0;
                        _nTotalAmount = 0;
                        _nTotalAmountCRate = 0;
                        _nCount = 0;
                    }
                    #region PrintDetail
                    //List<DyeingOrderDetail> oDyeingOrderDetails = new List<DyeingOrderDetail>();
                    //_oDyeingOrderDetails = _oDyeingOrderDetails.Where(o => o.ProductID == oChallan.ProductID).ToList();

                    //for (int i = 0; i < oDyeingOrderDetails.Count; i++)
                    //{
                        _nCount++;
                        oPdfPCell = new PdfPCell(new Phrase(oDyeingOrderDetail.OrderNo, _oFontStyle));
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

                        if (_oSampleInvoice.ConversionRate <= 1)
                        {
                            oPdfPCell = new PdfPCell(new Phrase(oDyeingOrderDetail.Note, _oFontStyle));
                            oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        }
                        else
                        {
                            _nTotalAmountCRate = _nTotalAmountCRate + (oDyeingOrderDetail.Qty * oDyeingOrderDetail.UnitPrice * _oSampleInvoice.ConversionRate);
                            _nGrandTotalCRate = _nGrandTotalCRate + (oDyeingOrderDetail.Qty * oDyeingOrderDetail.UnitPrice * _oSampleInvoice.ConversionRate);
                            oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oDyeingOrderDetail.Qty * oDyeingOrderDetail.UnitPrice * _oSampleInvoice.ConversionRate), _oFontStyle));
                            oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                     
                        oPdfPTable.AddCell(oPdfPCell);

                        _nTotalAmount = _nTotalAmount + (oDyeingOrderDetail.Qty * oDyeingOrderDetail.UnitPrice);
                        _nTotalQty = _nTotalQty + oDyeingOrderDetail.Qty;
                        _nGrandTotalAmount = _nGrandTotalAmount + (oDyeingOrderDetail.Qty * oDyeingOrderDetail.UnitPrice);
                        _nGrandTotalQty = _nGrandTotalQty + oDyeingOrderDetail.Qty;
                      
                        oPdfPTable.CompleteRow();
                    #endregion

                    nProductID = oDyeingOrderDetail.ProductID;

                }

                #region Total
                _oPdfPCell = new PdfPCell(new Phrase("Total: ", _oFontStyleBold));
                _oPdfPCell.Colspan = 4;
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

                _oPdfPCell = new PdfPCell(new Phrase("$" + Global.MillionFormat(_nTotalAmount), _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                if (_nTotalAmountCRate > 0)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalAmountCRate), _oFontStyleBold));
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                }
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();
                #endregion

                #region Grand Total
                _oPdfPCell = new PdfPCell(new Phrase("Grand Total: ", _oFontStyleBold));
                _oPdfPCell.Colspan = 4;
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

                _oPdfPCell = new PdfPCell(new Phrase("$" + Global.MillionFormat(_nGrandTotalAmount), _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                if (_nGrandTotalCRate > 0)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nGrandTotalCRate), _oFontStyleBold));
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                }
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);


                oPdfPTable.CompleteRow();
                #endregion
                #region Grand Total in Words
                if (_nGrandTotalCRate > 0)
                { _oPdfPCell = new PdfPCell(new Phrase("Amount In Words: " + Global.TakaWords(_nGrandTotalCRate), _oFontStyleBold)); }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("Amount In Words: " + Global.DollarWords(_nGrandTotalAmount), _oFontStyleBold));
                }
                _oPdfPCell.Colspan = 8;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(_oPdfPCell);

               


                oPdfPTable.CompleteRow();
                #endregion

            }
            return oPdfPTable;
        }
        private void PrintFooter()
        {
            //_oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, 1);

            PdfPTable oPdfPTable = new PdfPTable(4);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 148f, 148f, 148f, 148f });




            oPdfPCell = new PdfPCell(new Phrase("Prepared By", _oFontStyleBold));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0.5f;

            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);



            oPdfPCell = new PdfPCell(new Phrase("Accounts", _oFontStyleBold));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0.5f;

            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("Marketing Manager", _oFontStyleBold));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0.5f;
            //oPdfPCell.FixedHeight = 35f;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Managing Director", _oFontStyleBold));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0.5f;

            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("", FontFactory.GetFont("Tahoma", 9f, 0)));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0.5f;
            oPdfPCell.FixedHeight = 45f;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);



            oPdfPCell = new PdfPCell(new Phrase("", FontFactory.GetFont("Tahoma", 8f, 2)));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0.5f;
            oPdfPCell.FixedHeight = 45f;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("", FontFactory.GetFont("Tahoma", 8f, 2)));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0.5f;
            oPdfPCell.FixedHeight = 45f;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("", FontFactory.GetFont("Tahoma", 8f, 2)));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0.5f;
            oPdfPCell.FixedHeight = 45f;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        private void PrintFooterBuyer()
        {
            //_oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, 1);

            PdfPTable oPdfPTable = new PdfPTable(1);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 595f });

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.FixedHeight = 15;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("This value will be adjusted with immediate next L/C of our company, irrespective of Yarn, Buyer and Merchandiser.", _oFontStyleBold));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();


            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.FixedHeight = 15; 
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("Approved by :", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.FixedHeight = 10;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("Full Name  :", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.FixedHeight = 10;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();


            oPdfPCell = new PdfPCell(new Phrase("Seal and signature of management'", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        #region DebitNote
        private void PrintHead_DebitNote()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 9f, 1);
            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 9f, 4);
            string sTemp = "";
            PdfPTable oPdfPTable = new PdfPTable(3);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 100, 140f, 100f });

            oPdfPCell = new PdfPCell(new Phrase("DATE", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oSampleInvoice.SampleInvoiceDateST, _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.Colspan = 2;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("Debit Note No", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oSampleInvoice.InvoiceNo, _oFontStyleBold));
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

            oPdfPCell = new PdfPCell(new Phrase(" " + _oSampleInvoice.ContractorName, _oFontStyleBold));
            //oPdfPCell.Border = 0;
            oPdfPCell.Colspan = 2;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();
            if (_oDyeingOrders.Count > 0)
            {
                sTemp = _oDyeingOrders[0].MBuyer;
            }

            oPdfPCell = new PdfPCell(new Phrase("MASTER BUYER" , _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + sTemp, _oFontStyleBold));
            //oPdfPCell.Border = 0;
            oPdfPCell.Colspan = 2;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();
            if (_oDyeingOrders.Count > 0)
            {
                sTemp = _oDyeingOrders[0].StyleNo;
            }

            oPdfPCell = new PdfPCell(new Phrase("Style NO.", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + sTemp, _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.Colspan = 2;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();


            oPdfPCell = new PdfPCell(new Phrase("CONCERN PERSON", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oSampleInvoice.MKTPName, _oFontStyle));
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

            sTemp = "";
            string sTempTwo = "";
            if (_oSampleInvoice.PaymentType == (int)EnumOrderPaymentType.CashOrCheque)
            {
                sTemp = "Cheque or Cash";
                if (!string.IsNullOrEmpty(_oSampleInvoice.ExportPINo))
                {
                    sTempTwo = "MR No:" + _oSampleInvoice.ExportPINo;
                }
            }
            if (_oSampleInvoice.PaymentType == (int)EnumOrderPaymentType.AdjWithNextLC)
            {
                sTemp = "NEXT L/C ADJUST ";

                if (!string.IsNullOrEmpty(_oSampleInvoice.ExportPINo))
                {
                    sTempTwo = "P/I No:" + _oSampleInvoice.ExportPINo;
                }
            }
            if (_oSampleInvoice.PaymentType == (int)EnumOrderPaymentType.AdjWithPI)
            {
                sTemp = "Adj With PI ";
                if (!string.IsNullOrEmpty(_oSampleInvoice.ExportPINo))
                {
                    sTempTwo = "P/I No:" + _oSampleInvoice.ExportPINo;
                }
            }
            if (_oSampleInvoice.PaymentType == (int)EnumOrderPaymentType.FoC)
            {
                sTemp = "Free Of Cost";
                if (!string.IsNullOrEmpty(_oSampleInvoice.ExportPINo))
                {
                    sTempTwo = "MR No:" + _oSampleInvoice.ExportPINo;
                }
            }

            oPdfPCell = new PdfPCell(new Phrase(" " + sTemp + " " + sTempTwo, _oFontStyle));


            //oPdfPCell.Border = 0;
            oPdfPCell.Colspan = 2;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            if (_oDyeingOrders.Count > 0)
            {
                sTemp = _oDyeingOrders[0].DeliveryNote;
            }

            oPdfPCell = new PdfPCell(new Phrase("Delivery Note", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + sTemp, _oFontStyle));
            //oPdfPCell.Border = 0;
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
        }
        private void Print_Body_SampleInvoice()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 9f, 1);
            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 9f, 4);

            _nTotalQty = 0;

            PdfPTable oPdfPTable = new PdfPTable(7);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 30f, 90f, 180f, 75f, 70f, 75f, 75f });
            int nProductID = 0;
            _nTotalQty = 0;
            _nTotalAmount = 0;
            _nCount = 0;

            if (_oSampleInvoice.SampleInvoiceDetails .Count > 0)
            {

                oPdfPCell = new PdfPCell(new Phrase("SL#", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Sample No", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Product Description", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Qty", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);


                string sUnitPrice = _oSampleInvoice.RateUnit > 1 ? "U/P(" + _oSampleInvoice.CurrencySymbol + ")/" + _oSampleInvoice.RateUnit : "U/P(" + _oSampleInvoice.CurrencySymbol + ")";
                oPdfPCell = new PdfPCell(new Phrase(sUnitPrice, _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);


                oPdfPCell = new PdfPCell(new Phrase("Amount(" + _oSampleInvoice.CurrencySymbol + ")", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);
                if (_oSampleInvoice.ConversionRate <= 1)
                {
                    oPdfPCell = new PdfPCell(new Phrase("REMARKS", _oFontStyleBold));
                }
                else
                {
                    oPdfPCell = new PdfPCell(new Phrase("Amount(" + _oSampleInvoice.ExchangeCurrencySymbol + ")", _oFontStyleBold));
                }
                oPdfPCell.BackgroundColor = BaseColor.GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();

                foreach (SampleInvoiceDetail oItem in _oSampleInvoice.SampleInvoiceDetails)
                {
                    _nCount++;
                    #region PrintDetail

                    oPdfPCell = new PdfPCell(new Phrase(_nCount.ToString(), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.MinimumHeight = 30f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.OrderNo, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.MinimumHeight = 30f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.MinimumHeight = 30f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.MinimumHeight = 30f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.UnitPrice), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.MinimumHeight = 30f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty * (oItem.UnitPrice / _oSampleInvoice.RateUnit)), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.MinimumHeight = 30f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    //rate unit by default :1 
                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty * (oItem.UnitPrice/_oSampleInvoice.RateUnit) *_oSampleInvoice.ConversionRate), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.MinimumHeight = 30f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPTable.CompleteRow();
                    #endregion
                    _nTotalQty = _nTotalQty + oItem.Qty;
                    _nTotalAmount = _nTotalAmount + oItem.Qty * oItem.UnitPrice;

                }

                int nRequiredRow = 5 - (_oSampleInvoice.SampleInvoiceDetails.Count);
                for (int i = 1; i <= nRequiredRow; i++)
                {
                    #region Blank Row

                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.MinimumHeight = 10f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.MinimumHeight = 10f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.MinimumHeight = 10f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.MinimumHeight = 10f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.MinimumHeight = 10f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);


                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.MinimumHeight = 10f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.MinimumHeight = 10f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPTable.CompleteRow();
                    #endregion
                }

                #region Total
                _oPdfPCell = new PdfPCell(new Phrase("Total: ", _oFontStyleBold));
                _oPdfPCell.Colspan = 3;
                _oPdfPCell.BackgroundColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalQty), _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalAmount), _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalAmount*_oSampleInvoice.ConversionRate), _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.GRAY;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
                #endregion
                #region Total in World

                if (_oSampleInvoice.ConversionRate>1)
                { _oPdfPCell = new PdfPCell(new Phrase("Amount In Words: " + Global.TakaWords(_nTotalAmount * _oSampleInvoice.ConversionRate), _oFontStyleBold)); }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("Amount In Words: " + Global.DollarWords(_nTotalAmount), _oFontStyleBold));
                }
                _oPdfPCell.Colspan = 7;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
                #endregion
            }
            if (!string.IsNullOrEmpty(_oSampleInvoice.Remark))
            {
                _oPdfPCell = new PdfPCell(new Phrase("Remark: " + _oSampleInvoice.Remark, _oFontStyleBold));
                _oPdfPCell.Colspan = 7;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //_oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 15f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();
        }
        private void Print_Body_DebitNote()
        {
            
            _nTotalQty = 0;

            PdfPTable oPdfPTable = new PdfPTable(7);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 30f, 90f, 180f, 75f, 70f, 75f, 75f });
            int nProductID = 0;
            _nTotalQty = 0;
            _nTotalAmount = 0;
            _nCount = 0;

            if (_oDyeingOrderDetails.Count > 0)
            {
                _oDyeingOrderDetails = _oDyeingOrderDetails.OrderBy(o => o.ProductID).ToList();
            }
            List<DyeingOrderDetail> _oDyeingOrderDetails_Product = new List<DyeingOrderDetail>();
            List<DyeingOrderDetail> _oDyeingOrderDetails_Temp = new List<DyeingOrderDetail>();
            DyeingOrderDetail oDyeingOrderDetail_Temp = new DyeingOrderDetail();
            foreach (DyeingOrderDetail oItem in _oDyeingOrderDetails)
            {
                if (nProductID != oItem.ProductID)
                {
                    _oDyeingOrderDetails_Temp = _oDyeingOrderDetails.Where(o => o.ProductID == oItem.ProductID).ToList();

                    oDyeingOrderDetail_Temp = new DyeingOrderDetail();
                    oDyeingOrderDetail_Temp.OrderNo = oItem.OrderNo;
                    oDyeingOrderDetail_Temp.ProductID = oItem.ProductID;
                    oDyeingOrderDetail_Temp.ProductName = oItem.ProductName;
                    oDyeingOrderDetail_Temp.Qty = _oDyeingOrderDetails_Temp.Select(c => c.Qty).Sum();
                    _nTotalAmount = _oDyeingOrderDetails_Temp.Select(c => c.Qty * c.UnitPrice).Sum();
                    if (oDyeingOrderDetail_Temp.Qty > 0)
                    {
                        oDyeingOrderDetail_Temp.UnitPrice = _nTotalAmount / oDyeingOrderDetail_Temp.Qty;
                    }
                    else
                    {
                        oDyeingOrderDetail_Temp.UnitPrice = 0;
                    }
                    _oDyeingOrderDetails_Product.Add(oDyeingOrderDetail_Temp);
                    _nTotalQty = 0;
                    _nTotalAmount = 0;
                }
                nProductID = oItem.ProductID;
            }



            if (_oDyeingOrderDetails_Product.Count > 0)
            {
                oPdfPCell = new PdfPCell(new Phrase("SL#", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("SAMPLE NO", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("YARN TYPE", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);



                oPdfPCell = new PdfPCell(new Phrase(" Qty(LBS)", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("U/P(" + _oSampleInvoice.CurrencySymbol + ")", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);


                oPdfPCell = new PdfPCell(new Phrase("Amount(" + _oSampleInvoice.CurrencySymbol + ")", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);
                if (_oSampleInvoice.ConversionRate <= 1)
                {
                    oPdfPCell = new PdfPCell(new Phrase("REMARKS", _oFontStyleBold));
                }
                else
                {
                    oPdfPCell = new PdfPCell(new Phrase("Amount(" + _oSampleInvoice.ExchangeCurrencySymbol + ")", _oFontStyleBold));
                }
                oPdfPCell.BackgroundColor = BaseColor.GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);


                oPdfPTable.CompleteRow();

                foreach (DyeingOrderDetail oItem in _oDyeingOrderDetails_Product)
                {
                    _nCount++;


                    #region PrintDetail

                    oPdfPCell = new PdfPCell(new Phrase(_nCount.ToString(), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    oPdfPCell.MinimumHeight = 30f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.OrderNo, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    oPdfPCell.MinimumHeight = 30f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    oPdfPCell.MinimumHeight = 30f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f;
                    oPdfPCell.MinimumHeight = 30f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.UnitPrice), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f;
                    oPdfPCell.MinimumHeight = 30f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty * oItem.UnitPrice), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f;
                    oPdfPCell.MinimumHeight = 30f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty * oItem.UnitPrice * _oSampleInvoice.ConversionRate), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f;
                    oPdfPCell.MinimumHeight = 30f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPTable.CompleteRow();
                    #endregion
                    _nTotalQty = _nTotalQty + oItem.Qty;
                    _nTotalAmount = _nTotalAmount + oItem.Qty * oItem.UnitPrice;

                }

                int nRequiredRow = 5 - (_oDyeingOrderDetails_Product.Count);
                for (int i = 1; i <= nRequiredRow; i++)
                {
                    #region Blank Row

                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    oPdfPCell.MinimumHeight = 10f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    oPdfPCell.MinimumHeight = 10f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    oPdfPCell.MinimumHeight = 10f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    oPdfPCell.MinimumHeight = 10f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    oPdfPCell.MinimumHeight = 10f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);


                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f;
                    oPdfPCell.MinimumHeight = 10f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    oPdfPCell.MinimumHeight = 10f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPTable.CompleteRow();
                    #endregion
                }


                #region Total
                _oPdfPCell = new PdfPCell(new Phrase("Total: ", _oFontStyleBold));
                _oPdfPCell.Colspan = 3;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalQty), _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.GRAY;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.GRAY;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalAmount), _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.GRAY;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalAmount * _oSampleInvoice.ConversionRate), _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.GRAY;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
                #endregion
                #region Total in World

                if (_oSampleInvoice.ConversionRate > 1)
                { _oPdfPCell = new PdfPCell(new Phrase("Amount In Words: " + Global.TakaWords(_nTotalAmount * _oSampleInvoice.ConversionRate), _oFontStyleBold)); }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("Amount In Words: " + Global.DollarWords(_nTotalAmount), _oFontStyleBold));
                }
                _oPdfPCell.Colspan = 7;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
                #endregion
            }
          


            if (!string.IsNullOrEmpty(_oSampleInvoice.Remark))
            {
                _oPdfPCell = new PdfPCell(new Phrase("Remark: " + _oSampleInvoice.Remark, _oFontStyleBold));
                _oPdfPCell.Colspan = 7;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 15f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        #endregion

        #region LC Adjustment Invoice
        public byte[] PrepareReport_AdjInvoice(SampleInvoice oSampleInvoice, Company oCompany, ExportSCDO oExportSCDO, BusinessUnit oBusinessUnit, bool bIsSample)
        {
            _oSampleInvoice = oSampleInvoice;
            _oExportSCDO = oExportSCDO;
            _oCompany = oCompany;
            _oBusinessUnit = oBusinessUnit;
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
                this.ReporttHeader_AdjInvoice();
                this.PrintHead_AdjInvoice();
                this.Print_Body_AdjInvoice();
                this.PrintFooter();

            
            _oPdfPTable.HeaderRows = 4;

            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        private void ReporttHeader_AdjInvoice()
        {

            #region Proforma Invoice Heading Print
            string sTemp = "";
            if (_oSampleInvoice.InvoiceType==(int)EnumSampleInvoiceType.Adjstment_Qty)
            {
                sTemp = "L/C ADJUST-QUANTITY";
            }
            else if (_oSampleInvoice.InvoiceType == (int)EnumSampleInvoiceType.Adjstment_Value)
            {
                sTemp = "L/C ADJUST-VALUE";
            }
            else if (_oSampleInvoice.InvoiceType == (int)EnumSampleInvoiceType.Adjstment_Commission)
            {
                sTemp = "COMMISSION ADJUST";
            }
            else if (_oSampleInvoice.InvoiceType == (int)EnumSampleInvoiceType.ReturnAdjustment)
            {
                sTemp = "RETURN ADJUSTMENT";
            }
            else
            {
                sTemp = "SAMPLE ADJUSTMENT";
            }
            _oPdfPCell = new PdfPCell(new Phrase(sTemp, FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD)));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.FixedHeight = 20; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            if (_oSampleInvoice.ApproveBy == 0)
            {
                if (_oSampleInvoice.InvoiceType == (int)EnumSampleInvoiceType.Adjstment_Commission)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("Waiting For Payment Settle", FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLDITALIC)));
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("Unauthorised Invoice", FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLDITALIC)));
                }
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.FixedHeight = 20; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.Colspan = _nTotalColumn;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }

            #endregion
        }
        private void PrintHead_AdjInvoice()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 9f, 1);
            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 9f, 4);

            PdfPTable oPdfPTable = new PdfPTable(3);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 120, 140f, 80f });

            oPdfPCell = new PdfPCell(new Phrase("DATE", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oSampleInvoice.SampleInvoiceDateST, _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.Colspan = 2;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("SAMPLE ADJUSTMENT NOTE NO", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oSampleInvoice.InvoiceNo, _oFontStyleBold));
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

            oPdfPCell = new PdfPCell(new Phrase(" " + _oSampleInvoice.ContractorName, _oFontStyleBold));
            //oPdfPCell.Border = 0;
            oPdfPCell.Colspan = 2;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("P/I No", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oExportSCDO.PINo_Full, _oFontStyleBold));
            //oPdfPCell.Border = 0;
            oPdfPCell.Colspan = 2;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("L/C No", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oExportSCDO.ELCNo, _oFontStyleBold));
            //oPdfPCell.Border = 0;
            oPdfPCell.Colspan = 2;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();


            oPdfPCell = new PdfPCell(new Phrase("CONCERN PERSON", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oSampleInvoice.MKTPName, _oFontStyle));
            //oPdfPCell.Border = 0;
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
           
        }
        private void Print_Body_AdjInvoice()
        {

            PdfPTable oPdfPTable = new PdfPTable(7);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 30f, 90f, 180f, 75f, 70f, 75f, 75f });
            _nTotalQty = 0;
            _nTotalAmount = 0;
            _nCount = 0;



            if (_oSampleInvoice.SampleInvoiceDetails.Count > 0)
            {


                oPdfPCell = new PdfPCell(new Phrase("SL#", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("SAMPLE NO", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("YARN TYPE", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);



                oPdfPCell = new PdfPCell(new Phrase(" Qty(LBS)", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("U/P($)", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);


                oPdfPCell = new PdfPCell(new Phrase("Amount($)", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);
                if (_oSampleInvoice.ConversionRate <= 1)
                {
                    oPdfPCell = new PdfPCell(new Phrase("REMARKS", _oFontStyleBold));
                }
                else
                {
                    oPdfPCell = new PdfPCell(new Phrase("Amount(" + _oSampleInvoice.CurrencySymbol + ")", _oFontStyleBold));
                }
                oPdfPCell.BackgroundColor = BaseColor.GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);


                oPdfPTable.CompleteRow();

                foreach (SampleInvoiceDetail oItem in _oSampleInvoice.SampleInvoiceDetails)
                {
                    _nCount++;


                    #region PrintDetail

                    oPdfPCell = new PdfPCell(new Phrase(_nCount.ToString(), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    oPdfPCell.MinimumHeight = 30f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.OrderNo, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    oPdfPCell.MinimumHeight = 30f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    oPdfPCell.MinimumHeight = 30f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f;
                    oPdfPCell.MinimumHeight = 30f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.UnitPrice), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f;
                    oPdfPCell.MinimumHeight = 30f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty * oItem.UnitPrice), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f;
                    oPdfPCell.MinimumHeight = 30f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);
                    if (_oSampleInvoice.ConversionRate <= 1)
                    {
                        oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    }
                    else
                    {
                        oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty * oItem.UnitPrice * _oSampleInvoice.ConversionRate), _oFontStyle));
                    }
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f;
                    oPdfPCell.MinimumHeight = 30f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPTable.CompleteRow();
                    #endregion
                    _nTotalQty = _nTotalQty + oItem.Qty;
                    _nTotalAmount = _nTotalAmount + oItem.Qty * oItem.UnitPrice;

                }

                int nRequiredRow = 5 - (_oSampleInvoice.SampleInvoiceDetails.Count);
                for (int i = 1; i <= nRequiredRow; i++)
                {
                    #region Blank Row

                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    oPdfPCell.MinimumHeight = 10f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    oPdfPCell.MinimumHeight = 10f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    oPdfPCell.MinimumHeight = 10f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    oPdfPCell.MinimumHeight = 10f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    oPdfPCell.MinimumHeight = 10f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);


                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f;
                    oPdfPCell.MinimumHeight = 10f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    oPdfPCell.MinimumHeight = 10f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPTable.CompleteRow();
                    #endregion
                }


                #region Total
                _oPdfPCell = new PdfPCell(new Phrase("Total: ", _oFontStyleBold));
                _oPdfPCell.Colspan = 3;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalQty), _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.GRAY;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.GRAY;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalAmount), _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.GRAY;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);
                if (_oSampleInvoice.ConversionRate > 1)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalAmount * _oSampleInvoice.ConversionRate), _oFontStyleBold));
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                }
                _oPdfPCell.BackgroundColor = BaseColor.GRAY;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
                #endregion
                #region Total in World

                if (_oSampleInvoice.ConversionRate > 1)
                { _oPdfPCell = new PdfPCell(new Phrase("Amount In Words: " + Global.TakaWords(_nTotalAmount * _oSampleInvoice.ConversionRate), _oFontStyleBold)); }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("Amount In Words: " + Global.DollarWords(_nTotalAmount), _oFontStyleBold));
                }
                _oPdfPCell.Colspan = 7;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
                #endregion
            }
            if (!string.IsNullOrEmpty(_oSampleInvoice.Remark))
            {
                _oPdfPCell = new PdfPCell(new Phrase("Remark: " + _oSampleInvoice.Remark, _oFontStyleBold));
                _oPdfPCell.Colspan = 7;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 15f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        #endregion

        #region Dyeing Bill
        public byte[] PrepareReport_DyeingBill(SampleInvoice oSampleInvoice, Company oCompany, BusinessUnit oBusinessUnit, List<DyeingOrderReport> oDyeingOrderReports, SampleInvoiceSetup oSampleInvoiceSetup, int nTitleType)
        {
            _oSampleInvoice = oSampleInvoice;
            _oDyeingOrderReports = oDyeingOrderReports;
            _oCompany = oCompany;
            _oBusinessUnit = oBusinessUnit;
            _oSampleInvoiceSetup = oSampleInvoiceSetup;
            _nTitleType = nTitleType;
            #region Page Setup
            _oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
            _oDocument.SetMargins(35f, 35f, 10f, 20f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            //PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            //PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            //ESimSolFooter PageEventHandler = new ESimSolFooter();
            //PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler

            PdfWriter PdfWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PageEventHandler.nFontSize = 8;
            PdfWriter.PageEvent = PageEventHandler;

            _oDocument.Open();

            _oPdfPTable.SetWidths(new float[] { 595f });
            #endregion
            if (nTitleType == 1)//normal
            {
                this.PrintHeader();
            }
            else if (nTitleType == 2)//Pad
            {
                this.PrintHeader_Blank();
            }
            if (nTitleType == 3)//imge
            {
                LoadCompanyTitle();
            }
            this.ReporttHeader();
            if (_oSampleInvoice.ApproveBy == 0)
            {
                this.PrintWaterMark(35f, 35f, 10f, 20f);
            }
            if (_oSampleInvoice.ApproveBy == (int)EnumSampleInvoiceStatus.Canceled)
            {
                this.PrintWaterMark(35f, 35f, 10f, 20f);
            }
          
            this.PrintHead_DyeingBill();
            this.PrintBody_DyeingBillOrderType();
            this.PrintBody_DyeingBillSummary(_oDyeingOrderReports);
            this.PrintFooter_DyeingBill();

            _oPdfPTable.HeaderRows = 2;

            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        private void PrintHeader_Blank()
        {
            #region Proforma Invoice Heading Print
            _oFontStyle = FontFactory.GetFont("Tahoma", 15f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Colspan = 9;
            _oPdfPCell.FixedHeight = 100f; _oPdfPCell.BorderWidth = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        private void LoadCompanyTitle()
        {
            PdfPTable oPdfPTable1 = new PdfPTable(1);
            PdfPCell oPdfPCell1;
            oPdfPTable1.SetWidths(new float[] { 400f });
            iTextSharp.text.Image oImag;

            if (_oCompany.CompanyTitle != null)
            {
                oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyTitle, System.Drawing.Imaging.ImageFormat.Jpeg);
                oImag.ScaleAbsolute(530f, 65f);
                oPdfPCell1 = new PdfPCell(oImag);
                oPdfPCell1.Border = 0;
                oPdfPCell1.FixedHeight = 100f;
                oPdfPCell1.BackgroundColor = BaseColor.WHITE;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell1.VerticalAlignment = Element.ALIGN_BOTTOM;
                oPdfPTable1.AddCell(oPdfPCell1);
            }
            _oPdfPCell = new PdfPCell(oPdfPTable1);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        private void PrintHead_DyeingBill()
        {
            //_oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, 1);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.UNDERLINE);

            PdfPTable oPdfPTable = new PdfPTable(4);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 60f, 180f, 70f, 130f });
            //// 1st Row
            oPdfPCell = new PdfPCell(new Phrase("BUYER NAME", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oSampleInvoice.ContractorName, _oFontStyleBold));
            //oPdfPCell.Border = 0;
            //oPdfPCell.Colspan = 2;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("Bill No", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            //for ea _oDUDyeingOrderSteps

            oPdfPCell = new PdfPCell(new Phrase(_oSampleInvoiceSetup.Code + "-" + _oSampleInvoice.InvoiceNo, _oFontStyleBold));
            //oPdfPCell.Border = 0;
            //oPdfPCell.Colspan = 2;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();
            /// 2nd Row
            oPdfPCell = new PdfPCell(new Phrase( "Concern Person", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oSampleInvoice.ContractorPersopnnalName, _oFontStyle));
            //oPdfPCell.Border = 0;
            //oPdfPCell.Colspan = 2;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("PI No.", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);



            oPdfPCell = new PdfPCell(new Phrase(": " + _oSampleInvoice.ExportPINo, _oFontStyle));
            //oPdfPCell.Border = 0;
            //oPdfPCell.Colspan = 2;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            //// 3nd Row 

            oPdfPCell = new PdfPCell(new Phrase("Delivery Place", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.Rowspan = 3;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(_oSampleInvoice.ContractorAddress, _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.Rowspan = 3;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("Date:", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(_oSampleInvoice.SampleInvoiceDateST, _oFontStyleBold));
            //oPdfPCell.Border = 0;
            //oPdfPCell.Colspan = 2;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            //// 4th Row
            //// Use Rowspan

            oPdfPCell = new PdfPCell(new Phrase("Payment Term:", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPCell = new PdfPCell(new Phrase(_oSampleInvoice.PaymentTypeSt, _oFontStyle));
            //oPdfPCell.Border = 0;
            //oPdfPCell.Colspan = 2;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }

        private void PrintBody_DyeingBillOrderType()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            
            List<DyeingOrderReport> oDyeingOrderReports = new List<DyeingOrderReport>();
            oDyeingOrderReports = _oDyeingOrderReports.Where(o => o.DyeingOrderType==(int)EnumOrderType.SaleOrder).ToList();
            if (_oDyeingOrderReports.Count > 0)
            {
                _sMunit = _oDyeingOrderReports[0].MUName;
                _sCurrency = _oDyeingOrderReports[0].Currency;
            }
            if (oDyeingOrderReports.Count > 0)
            {
                oDyeingOrderReports = oDyeingOrderReports.OrderBy(o => o.OrderNo).ThenBy(a => a.DyeingOrderID).ToList();
                this.PrintBody_DyeingBill(oDyeingOrderReports);
            }

            oDyeingOrderReports = _oDyeingOrderReports.Where(o => o.DyeingOrderType == (int)EnumOrderType.SampleOrder).ToList();
            if (oDyeingOrderReports.Count > 0)
            {
                oDyeingOrderReports = oDyeingOrderReports.OrderBy(o => o.OrderNo).ThenBy(a => a.DyeingOrderID).ToList();
                this.PrintBody_DyeingBill(oDyeingOrderReports);
            }
            oDyeingOrderReports = _oDyeingOrderReports.Where(o => o.DyeingOrderType == (int)EnumOrderType.ClaimOrder).ToList();
            if (oDyeingOrderReports.Count > 0)
            {
                oDyeingOrderReports = oDyeingOrderReports.OrderBy(o => o.OrderNo).ThenBy(a => a.DyeingOrderID).ToList();
                this.PrintBody_DyeingBill(oDyeingOrderReports);
            }
            oDyeingOrderReports = _oDyeingOrderReports.Where(o => o.DyeingOrderType == (int)EnumOrderType.TwistOrder).ToList();
            if (oDyeingOrderReports.Count > 0)
            {
                oDyeingOrderReports = oDyeingOrderReports.OrderBy(o => o.OrderNo).ThenBy(a => a.DyeingOrderID).ThenBy(a => a.BuyerCombo).ToList();
                this.PrintBody_DyeingBill_Twisting(oDyeingOrderReports);
            }
            oDyeingOrderReports = _oDyeingOrderReports.Where(o => o.DyeingOrderType == (int)EnumOrderType.ReConing).ToList();
            if (oDyeingOrderReports.Count > 0)
            {
                oDyeingOrderReports = oDyeingOrderReports.OrderBy(o => o.OrderNo).ThenBy(a => a.DyeingOrderID).ToList();
                this.PrintBody_DyeingBill(oDyeingOrderReports);
            }
        }
        private void PrintBody_DyeingBill(List<DyeingOrderReport> oDyeingOrderReports)
        {
         
            //_oFontStyleBoldUnderLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);

         

            #region Initialize Table
            PdfPTable oPdfPTable = new PdfPTable(9);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                   //23f,  //SL
                                                    38f,  //Order No 
                                                    85f,  //Buyer Ref
                                                    60f,  //Concern Person
                                                    60f, //Color
                                                    90f,  //Type of count                                          
                                                    28f,  //Color Category                                              
                                                    35f,  //Qty
                                                    35f,  //U/P
                                                    60f,  //Value
                                                   
                                             });
            #endregion

            #region Header Name
            _oPdfPCell = new PdfPCell(new Phrase(oDyeingOrderReports[0].OrderTypeSt, FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD)));
            _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 9; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            #region Initialize Table
             oPdfPTable = new PdfPTable(9);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                   //23f,  //SL
                                                    38f,  //Order No 
                                                    85f,  //Buyer Ref
                                                    60f,  //Concern Person
                                                    60f, //Color
                                                    90f,  //Type of count                                          
                                                    28f,  //Color Category                                              
                                                    35f,  //Qty
                                                    35f,  //U/P
                                                    60f,  //Value
                                                   
                                             });
            #endregion
            int nDyeingOrderID = 0; int nCount = 0; 
            double nQty=0,nAmount=0, nTotalAmount = 0, nTotalQty = 0;



            foreach (DyeingOrderReport oItem in oDyeingOrderReports)
            {
                if (nDyeingOrderID != oItem.DyeingOrderID)
                {
                    #region Product Wise Sub Total
                    if (oPdfPTable.Rows.Count > 0)
                    {
                        #region Initialize Table
                        oPdfPTable = new PdfPTable(9);
                        oPdfPTable.WidthPercentage = 100;
                        oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPTable.SetWidths(new float[] { 
                                                   //23f,  //SL
                                                    38f,  //Order No 
                                                    85f,  //Buyer Ref
                                                    60f,  //Concern Person
                                                    60f, //Color
                                                    90f,  //Type of count                                          
                                                    28f,  //Color Category                                              
                                                    35f,  //Qty
                                                    35f,  //U/P
                                                    60f,  //Value
                                             });
                        #endregion

                        #region Order Wise Sub Total
                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                        _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                        _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(" Sub Total :", _oFontStyleBold));
                        _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(_oSampleInvoice.CurrencySymbol + "" + (nAmount).ToString("#,##0.000;(#,##0.000)"), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        oPdfPTable.CompleteRow();
                        #endregion

                        #region Insert Into Main Table
                        _oPdfPCell = new PdfPCell(oPdfPTable);
                        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();

                        #endregion

                       nQty = 0; nAmount = 0;
                    }
                    #endregion

                    #region Header

                    nUsagesHeight = CalculatePdfPTableHeight(_oPdfPTable);

                    //if (nUsagesHeight > 750)
                    //{
                    //    _oDocument.Add(_oPdfPTable);
                    //    _oDocument.NewPage();
                    //    _oPdfPTable.DeleteBodyRows();
                    //    if (_nTitleType == 1)//normal
                    //    {
                    //        this.PrintHeader();
                    //    }
                    //    else if (_nTitleType == 2)//Pad
                    //    {
                    //        this.PrintHeader_Blank();
                    //    }
                    //    if (_nTitleType == 3)//imge
                    //    {
                    //        LoadCompanyTitle();
                    //    }
                    //    this.ReporttHeader();
                    //    nUsagesHeight = 0;
                    //    _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
                    //    _oFontStyleBold = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
                    //}


                    #region Table Initialize
                    oPdfPTable = new PdfPTable(9);
                    oPdfPTable.WidthPercentage = 100;
                    oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.SetWidths(new float[] { 
                                                    //23f,  //SL
                                                    38f,  //Order No 
                                                    85f,  //Buyer Ref
                                                    60f,  //Concern Person
                                                    60f, //Color
                                                    90f,  //Type of count                                          
                                                    28f,  //Color Category                                              
                                                    35f,  //Qty
                                                    35f,  //U/P
                                                    60f,  //Value
                                             });
                    #endregion

                    //#region Product Heading
                    //_oPdfPCell = new PdfPCell(new Phrase(oItem.ContractorName, _oFontStyleBold));
                    //_oPdfPCell.Colspan = 12; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    //oPdfPTable.CompleteRow();
                    //#endregion

                    #region Header Row

                    //_oPdfPCell = new PdfPCell(new Phrase("SL No", _oFontStyle));
                    //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Order No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    if (oItem.CPName!="" && oItem.CPName != _oSampleInvoice.ContractorPersopnnalName)
                    {
                        _oPdfPCell = new PdfPCell(new Phrase("Buyer Ref", _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("Merchandiser", _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    }
                    else
                    {
                        _oPdfPCell = new PdfPCell(new Phrase("Buyer Ref", _oFontStyleBold));
                        _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    }

                    //_oPdfPCell = new PdfPCell(new Phrase("Buyer Ref", _oFontStyleBold));
                    //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    //_oPdfPCell = new PdfPCell(new Phrase("Merchandiser", _oFontStyleBold));
                    //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Color", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Yarn Type", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Shade ", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Qty " + "(" + _sMunit + ")", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("U.P.", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Value", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                  
                    oPdfPTable.CompleteRow();
                    #endregion

                    #region Insert Into Main Table
                    _oPdfPCell = new PdfPCell(oPdfPTable);
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion
                    #endregion
                }

                #region Initialize Table
                oPdfPTable = new PdfPTable(9);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.SetWidths(new float[] { 
                                                   //23f,  //SL
                                                    38f,  //Order No 
                                                    85f,  //Buyer Ref
                                                    60f,  //Concern Person
                                                    60f, //Color
                                                    90f,  //Type of count                                          
                                                    28f,  //Color Category                                              
                                                    35f,  //Qty
                                                    35f,  //U/P
                                                    60f,  //Value
                                             });
                #endregion

                nCount++;

                //_oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.OrderNo , _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                if (!String.IsNullOrEmpty(oItem.StyleNo))
                {
                    oItem.BuyerRef = oItem.BuyerRef + ", " + oItem.StyleNo;
                }

                if (oItem.CPName != _oSampleInvoice.ContractorPersopnnalName)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.BuyerRef, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.CPName, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.BuyerRef, _oFontStyle));
                    _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                }

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ColorName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName, FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL)));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                if (oItem.RGB == "" || oItem.RGB == null)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ColorNo, _oFontStyle));
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.RGB, _oFontStyle));
                }
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oSampleInvoice.CurrencySymbol + "" + System.Math.Round(oItem.UnitPrice, 4).ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                //_oPdfPCell = new PdfPCell(new Phrase(oItem.Currency + "" + Global.MillionFormatActualDigit(oItem.UnitPrice), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oSampleInvoice.CurrencySymbol + "" + (oItem.Qty * oItem.UnitPrice).ToString("#,##0.000;(#,##0.000)"), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                nQty += oItem.Qty;
                nAmount += oItem.Qty * oItem.UnitPrice;

                oPdfPTable.CompleteRow();

                #region Insert Into Main Table
                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

               
                #endregion

                nDyeingOrderID = oItem.DyeingOrderID;
                nTotalQty = nTotalQty + oItem.Qty;
                nTotalAmount += (oItem.Qty * oItem.UnitPrice);

             
            }

            #region Initialize Table
            oPdfPTable = new PdfPTable(9);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                  //23f,  //SL
                                                    38f,  //Order No 
                                                    85f,  //Buyer Ref
                                                    60f,  //Concern Person
                                                    60f, //Color
                                                    90f,  //Type of count                                          
                                                    28f,  //Color Category                                              
                                                    35f,  //Qty
                                                    35f,  //U/P
                                                    60f,  //Value
                                             });
            #endregion

            #region Order Wise Sub Total
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" Sub Total ", _oFontStyleBold));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oSampleInvoice.CurrencySymbol + "" + (nAmount).ToString("#,##0.000;(#,##0.000)"), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            #region Grand Total
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" Total ", _oFontStyleBold));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(Math.Round(nTotalQty,2)), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //nQty_Pro = _oDUProductionYetTos.Select(c => c.Qty_Prod).Sum();
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oSampleInvoice.CurrencySymbol + "" + (nTotalAmount).ToString("#,##0.000;(#,##0.000)"), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            nUsagesHeight = CalculatePdfPTableHeight(_oPdfPTable);

            //if (nUsagesHeight > 740)
            //{
            //    _oDocument.Add(_oPdfPTable);
            //    //_oDocument.NewPage();
            //    _oPdfPTable.DeleteBodyRows();
            //    this.PrintHeader();
            //    this.ReporttHeader();
            //    nUsagesHeight = 0;
            //    _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            //    _oFontStyleBold = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            //}
            #endregion
        }
        private void PrintBody_DyeingBill_Twisting(List<DyeingOrderReport> oDyeingOrderReports)
        {
            int nProductID = 0;
            int nBuyerCombo = 0;
            _nCount = 0;
            int _nCount_Raw = 0;
            string sTemp = "";

            List<DyeingOrderReport> oDyeingOrderReports_Temp = new List<DyeingOrderReport>();


            #region Initialize Table
            PdfPTable oPdfPTable = new PdfPTable(9);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                   //23f,  //SL
                                                    38f,  //Order No 
                                                    85f,  //Buyer Ref
                                                    60f,  //Concern Person
                                                    60f, //Color
                                                    90f,  //Type of count                                          
                                                    28f,  //Color Category                                              
                                                    35f,  //Qty
                                                    35f,  //U/P
                                                    60f,  //Value
                                                   
                                             });
            #endregion

            #region Header Name
            _oPdfPCell = new PdfPCell(new Phrase(oDyeingOrderReports[0].OrderTypeSt, FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD)));
            _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 9; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            #region Initialize Table
            oPdfPTable = new PdfPTable(9);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                   //23f,  //SL
                                                    38f,  //Order No 
                                                    85f,  //Buyer Ref
                                                    60f,  //Concern Person
                                                    60f, //Color
                                                    90f,  //Type of count                                          
                                                    28f,  //Color Category                                              
                                                    35f,  //Qty
                                                    35f,  //U/P
                                                    60f,  //Value
                                                   
                                             });
            #endregion
            int nDyeingOrderID = 0; int nCount = 0;
            double nQty = 0, nAmount = 0, nTotalAmount = 0, nTotalQty = 0;



            foreach (DyeingOrderReport oItem in oDyeingOrderReports)
            {
                if (nDyeingOrderID != oItem.DyeingOrderID)
                {
                    #region Product Wise Sub Total
                    if (oPdfPTable.Rows.Count > 0)
                    {
                        //#region Initialize Table
                        //oPdfPTable = new PdfPTable(9);
                        //oPdfPTable.WidthPercentage = 100;
                        //oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                        //oPdfPTable.SetWidths(new float[] { 
                        //                           //23f,  //SL
                        //                            38f,  //Order No 
                        //                            85f,  //Buyer Ref
                        //                            60f,  //Concern Person
                        //                            65f, //Color
                        //                            90f,  //Type of count                                          
                        //                            28f,  //Color Category                                              
                        //                            43f,  //Qty
                        //                            28f,  //U/P
                        //                            43f,  //Value
                        //                     });
                        //#endregion

                        #region Order Wise Sub Total
                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                        _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                        _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(" Sub Total :", _oFontStyleBold));
                        _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(_oSampleInvoice.CurrencySymbol + "" + nAmount.ToString("#,##0.000;(#,##0.000)"), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        oPdfPTable.CompleteRow();
                        #endregion

                        //#region Insert Into Main Table
                        //_oPdfPCell = new PdfPCell(oPdfPTable);
                        //_oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        //_oPdfPTable.CompleteRow();

                        //#endregion

                        nCount = 0; nQty = 0; nAmount = 0; nBuyerCombo = 0;
                    }
                    #endregion

                    #region Header


                    //#region Table Initialize
                    //oPdfPTable = new PdfPTable(9);
                    //oPdfPTable.WidthPercentage = 100;
                    //oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                    //oPdfPTable.SetWidths(new float[] { 
                    //                                //23f,  //SL
                    //                                38f,  //Order No 
                    //                                85f,  //Buyer Ref
                    //                                60f,  //Concern Person
                    //                                65f, //Color
                    //                                90f,  //Type of count                                          
                    //                                28f,  //Color Category                                              
                    //                                43f,  //Qty
                    //                                28f,  //U/P
                    //                                43f,  //Value
                    //                         });
                    //#endregion

               

                    #region Header Row

                    //_oPdfPCell = new PdfPCell(new Phrase("SL No", _oFontStyle));
                    //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Order No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                   
                    if (oItem.CPName != _oSampleInvoice.ContractorPersopnnalName)
                    {
                        _oPdfPCell = new PdfPCell(new Phrase("Buyer Ref", _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("Merchandiser", _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    }
                    else
                    {
                        _oPdfPCell = new PdfPCell(new Phrase("Buyer Ref", _oFontStyleBold));
                        _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    }
                  

                    _oPdfPCell = new PdfPCell(new Phrase("Color", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Yarn Type", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Shade ", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Qty " + "(" + _sMunit + ")", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("U.P.", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Value", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


                    oPdfPTable.CompleteRow();
                    #endregion

                    //#region Insert Into Main Table
                    //_oPdfPCell = new PdfPCell(oPdfPTable);
                    //_oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    //_oPdfPTable.CompleteRow();
                    //#endregion
                    #endregion
                }

                //#region Initialize Table
                //oPdfPTable = new PdfPTable(9);
                //oPdfPTable.WidthPercentage = 100;
                //oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                //oPdfPTable.SetWidths(new float[] { 
                //                                   //23f,  //SL
                //                                    38f,  //Order No 
                //                                    85f,  //Buyer Ref
                //                                    60f,  //Concern Person
                //                                    65f, //Color
                //                                    90f,  //Type of count                                          
                //                                    28f,  //Color Category                                              
                //                                    43f,  //Qty
                //                                    28f,  //U/P
                //                                    43f,  //Value
                //                             });
                //#endregion

                nCount++;

                //_oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.OrderNo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                if (!String.IsNullOrEmpty(oItem.StyleNo))
                {
                    oItem.BuyerRef = oItem.BuyerRef + ", " + oItem.StyleNo;
                }
                 
                   if (oItem.CPName != _oSampleInvoice.ContractorPersopnnalName)
                   {
                       _oPdfPCell = new PdfPCell(new Phrase(oItem.BuyerRef, _oFontStyle));
                       _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                       _oPdfPCell = new PdfPCell(new Phrase(oItem.CPName, _oFontStyle));
                       _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                   }
                   else
                   {
                       _oPdfPCell = new PdfPCell(new Phrase(oItem.BuyerRef, _oFontStyle));
                       _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                   }

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ColorName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName, FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL)));
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                if (nBuyerCombo != oItem.BuyerCombo && oItem.BuyerCombo > 0)
                {
                    sTemp = "";

                    _nCount_Raw = oDyeingOrderReports.Where(x => x.BuyerCombo == oItem.BuyerCombo && x.DyeingOrderID == oItem.DyeingOrderID).Count();
                    oDyeingOrderReports_Temp = oDyeingOrderReports.Where(x => x.BuyerCombo == oItem.BuyerCombo && x.DyeingOrderID == oItem.DyeingOrderID).ToList();
                    sTemp = string.Join(" + ", oDyeingOrderReports_Temp.Select(x => x.ProductName + " " + x.PantonNo).ToList());

                    _oPdfPCell = new PdfPCell(new Phrase(sTemp, _oFontStyle));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //_oPdfPCell.MinimumHeight = nMinimumHeight;
                    _oPdfPCell.Rowspan = (_nCount_Raw > 0) ? _nCount_Raw : 1;
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.BorderWidthLeft = 0.5f;
                    _oPdfPCell.BorderWidthTop = 0.5f;
                    _oPdfPCell.BorderWidthBottom = 0;
                    oPdfPTable.AddCell(_oPdfPCell);
                }
                else if (oItem.BuyerCombo == 0)
                {

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName+" " + oItem.PantonNo, _oFontStyle));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //_oPdfPCell.MinimumHeight = nMinimumHeight;
                    _oPdfPCell.Rowspan = (_nCount_Raw > 0) ? _nCount_Raw : 1;
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.BorderWidthLeft = 0.5f;
                    _oPdfPCell.BorderWidthTop = 0.5f;
                    _oPdfPCell.BorderWidthBottom = 0;
                    oPdfPTable.AddCell(_oPdfPCell);
                }

              
                 _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oSampleInvoice.CurrencySymbol + "" + System.Math.Round(oItem.UnitPrice, 4).ToString("#,##0.00;(#,##0.00)"), _oFontStyle));
                //_oPdfPCell = new PdfPCell(new Phrase(oItem.Currency + "" + Global.MillionFormatActualDigit(oItem.UnitPrice), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; 
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oSampleInvoice.CurrencySymbol + "" + (oItem.Qty * oItem.UnitPrice).ToString("#,##0.000;(#,##0.000)"), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; 
                oPdfPTable.AddCell(_oPdfPCell);


                nQty += oItem.Qty;
                nAmount += oItem.Qty * oItem.UnitPrice;
                nProductID = oItem.ProductID;
                nBuyerCombo = oItem.BuyerCombo;
                oPdfPTable.CompleteRow();

                nDyeingOrderID = oItem.DyeingOrderID;
                nTotalQty = nTotalQty + oItem.Qty;
                nTotalAmount += (oItem.Qty * oItem.UnitPrice);
                _nCount_Raw = 0;
                //nUsagesHeight = CalculatePdfPTableHeight(oPdfPTable);

                //if (nUsagesHeight > 770)
                //{
                //    _oDocument.Add(_oPdfPTable);
                //    _oDocument.NewPage();
                //    oPdfPTable.DeleteBodyRows();
                //    _oPdfPTable.DeleteBodyRows();
                //    if (_nTitleType == 1)//normal
                //    {
                //        this.PrintHeader();
                //    }
                //    else if (_nTitleType == 2)//Pad
                //    {
                //        this.PrintHeader_Blank();
                //    }
                //    if (_nTitleType == 3)//imge
                //    {
                //        LoadCompanyTitle();
                //    }
                //    this.ReporttHeader();
                //    nUsagesHeight = 0;
                //    _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
                //    _oFontStyleBold = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
                //}
            }

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            //_oPdfPCell.Rowspan = (_nCount_Raw > 0) ? _nCount_Raw : 1;
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Initialize Table
            oPdfPTable = new PdfPTable(9);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                  //23f,  //SL
                                                    38f,  //Order No 
                                                    85f,  //Buyer Ref
                                                    60f,  //Concern Person
                                                    60f, //Color
                                                    90f,  //Type of count                                          
                                                    28f,  //Color Category                                              
                                                    35f,  //Qty
                                                    35f,  //U/P
                                                    60f,  //Value
                                             });
            #endregion

            #region Order Wise Sub Total
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" Sub Total ", _oFontStyleBold));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nQty), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oSampleInvoice.CurrencySymbol + "" + (nAmount).ToString("#,##0.000;(#,##0.000)"), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            #region Grand Total
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" Total ", _oFontStyleBold));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(Math.Round(nTotalQty, 2)), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //nQty_Pro = _oDUProductionYetTos.Select(c => c.Qty_Prod).Sum();
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oSampleInvoice.CurrencySymbol + "" + (nTotalAmount).ToString("#,##0.000;(#,##0.000)"), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            nUsagesHeight = CalculatePdfPTableHeight(_oPdfPTable);

            if (nUsagesHeight > 790)
            {
                _oDocument.Add(_oPdfPTable);
                _oDocument.NewPage();
                _oPdfPTable.DeleteBodyRows();
                this.PrintHeader();
                this.ReporttHeader();
                nUsagesHeight = 0;
                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
                _oFontStyleBold = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            }
            #endregion
        }
     
      
        private void PrintBody_DyeingBillSummary(List<DyeingOrderReport> oDyeingOrderReports)
        {
            #region
            nUsagesHeight = CalculatePdfPTableHeight(_oPdfPTable);
            if (nUsagesHeight < 400)
            {
                nUsagesHeight = 400 - nUsagesHeight;
            }
            if (nUsagesHeight > 400)
            {
                #region Blank Row


                while (nUsagesHeight < 400)
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
            //_oFontStyleBoldUnderLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);

            #region Initialize Table
            PdfPTable oPdfPTable = new PdfPTable(9);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                  //23f,  //SL
                                                    38f,  //Order No 
                                                    85f,  //Buyer Ref
                                                    60f,  //Concern Person
                                                    65f, //Color
                                                    90f,  //Type of count                                          
                                                    28f,  //Color Category                                              
                                                    43f,  //Qty
                                                    28f,  //U/P
                                                    43f,  //Value
                                                   
                                             });
            #endregion

            #region Header Row

            //_oPdfPCell = new PdfPCell(new Phrase("SL No", _oFontStyle));
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Summary", _oFontStyleBold));
            _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Order Qty(" + _sMunit + ")", _oFontStyleBold));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase("Total Value", _oFontStyleBold));
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            _nTotalQty = _oDyeingOrderReports.Where(c => c.DyeingOrderType == (int)EnumOrderType.SaleOrder).Sum(x => x.Qty);
            if (_nTotalQty > 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Total Bulk Qty", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalQty) + " " + _sMunit, _oFontStyle));
                _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _nTotalAmount = _oDyeingOrderReports.Where(c => c.DyeingOrderType == (int)EnumOrderType.SaleOrder).Sum(x => (x.Qty * x.UnitPrice));
                _oPdfPCell = new PdfPCell(new Phrase(_oSampleInvoice.CurrencySymbol + " " + Global.MillionFormat(_nTotalAmount), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            _nTotalQty = _oDyeingOrderReports.Where(c => c.DyeingOrderType == (int)EnumOrderType.ClaimOrder).Sum(x => x.Qty);
            if (_nTotalQty > 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Total Demand Order Qty", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalQty) + " " + _sMunit, _oFontStyle));
                _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _nTotalAmount = _oDyeingOrderReports.Where(c => c.DyeingOrderType == (int)EnumOrderType.ClaimOrder).Sum(x => (x.Qty * x.UnitPrice));
                _oPdfPCell = new PdfPCell(new Phrase(_oSampleInvoice.CurrencySymbol + " " + Global.MillionFormat(_nTotalAmount), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            _nTotalQty = _oDyeingOrderReports.Where(c => c.DyeingOrderType == (int)EnumOrderType.SampleOrder).Sum(x => x.Qty);
            if (_nTotalQty > 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Total Sample Qty", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalQty) + " " + _sMunit, _oFontStyle));
                _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _nTotalAmount = _oDyeingOrderReports.Where(c => c.DyeingOrderType == (int)EnumOrderType.SampleOrder).Sum(x => (x.Qty * x.UnitPrice));
                _oPdfPCell = new PdfPCell(new Phrase(_oSampleInvoice.CurrencySymbol + " " + Global.MillionFormat(_nTotalAmount), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            _nTotalQty = _oDyeingOrderReports.Where(c => c.DyeingOrderType == (int)EnumOrderType.TwistOrder).Sum(x => x.Qty);
            if (_nTotalQty > 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Total Twist Order Qty", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalQty) + " " + _sMunit, _oFontStyle));
                _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _nTotalAmount = _oDyeingOrderReports.Where(c => c.DyeingOrderType == (int)EnumOrderType.TwistOrder).Sum(x => (x.Qty * x.UnitPrice));
                _oPdfPCell = new PdfPCell(new Phrase(_oSampleInvoice.CurrencySymbol + " " + Global.MillionFormat(_nTotalAmount), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            _nTotalQty = _oDyeingOrderReports.Where(c => c.DyeingOrderType == (int)EnumOrderType.ReConing).Sum(x => x.Qty);
            if (_nTotalQty > 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Total ReConing Qty", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalQty) + " " + _sMunit, _oFontStyle));
                _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _nTotalAmount = _oDyeingOrderReports.Where(c => c.DyeingOrderType == (int)EnumOrderType.ReConing).Sum(x => (x.Qty * x.UnitPrice));
                _oPdfPCell = new PdfPCell(new Phrase(_oSampleInvoice.CurrencySymbol + " " + Global.MillionFormat(_nTotalAmount), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            if (_oSampleInvoice.Charge> 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Add Charge", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                //_nTotalAmount = _oDyeingOrderReports.Where(c => c.DyeingOrderType == (int)EnumOrderType.SampleOrder).Sum(x => (x.Qty * x.UnitPrice));
                _oPdfPCell = new PdfPCell(new Phrase(_oSampleInvoice.CurrencySymbol + " " + Global.MillionFormat(_oSampleInvoice.Charge), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            if (_oSampleInvoice.Discount > 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Discount", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                //_nTotalAmount = _oDyeingOrderReports.Where(c => c.DyeingOrderType == (int)EnumOrderType.SampleOrder).Sum(x => (x.Qty * x.UnitPrice));
                _oPdfPCell = new PdfPCell(new Phrase(_oSampleInvoice.CurrencySymbol + " " + Global.MillionFormat(_oSampleInvoice.Discount), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            _nTotalQty = _oDyeingOrderReports.Select(c => c.Qty).Sum(); 
            if (_nTotalQty > 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Grand Total", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalQty) + " " + _sMunit, _oFontStyle));
                _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _nTotalAmount = _oDyeingOrderReports.Select(c => (c.Qty *c.UnitPrice)).Sum();
                _nTotalAmount = _nTotalAmount-_oSampleInvoice.Discount + _oSampleInvoice.Charge;
                _oPdfPCell = new PdfPCell(new Phrase(_oSampleInvoice.CurrencySymbol + " " + Global.MillionFormat(_nTotalAmount), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            if ((EnumOrderPaymentType)_oSampleInvoice.PaymentType == EnumOrderPaymentType.CashOrCheque)
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Equivalent Taka ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _nTotalAmount = _oDyeingOrderReports.Select(c => (c.Qty * c.UnitPrice)).Sum();
                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalAmount) + "x" + _oSampleInvoice.ConversionRate.ToString("00.00"), _oFontStyle));
                _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oSampleInvoice.ExchangeCurrencySymbol + " " + Global.TakaFormat(Math.Round((_nTotalAmount * _oSampleInvoice.ConversionRate),2)), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }
          //  _nTotalQty = _oDyeingOrderReports.Select(c => c.Qty).Sum();
            //if (_nTotalQty > 0)
            //{
            //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //    _oPdfPCell = new PdfPCell(new Phrase("Unit Price", _oFontStyleBold));
            //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalQty) + " " + _sMunit, _oFontStyle));
            //    _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            //    _nTotalAmount = _oDyeingOrderReports.Select(c => (c.Qty * c.UnitPrice)).Sum();
            //    _oPdfPCell = new PdfPCell(new Phrase(_sCurrency + " " + Global.MillionFormatActualDigit(Math.Round((_nTotalAmount / _nTotalQty),4)), _oFontStyleBold));
            //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //    _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            //    oPdfPTable.CompleteRow();
            //}
            if ((EnumOrderPaymentType)_oSampleInvoice.PaymentType == EnumOrderPaymentType.CashOrCheque)
            {
                _oPdfPCell = new PdfPCell(new Phrase("In Words ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.TakaWords(Math.Round((_nTotalAmount*_oSampleInvoice.ConversionRate),2)), _oFontStyle));
                _oPdfPCell.Colspan = 8; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("In Words", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.DollarWords(_nTotalAmount), _oFontStyle));
                _oPdfPCell.Colspan = 8; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            _oPdfPCell = new PdfPCell(new Phrase("Remarks", _oFontStyle));
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oSampleInvoice.Remark, _oFontStyle));
            _oPdfPCell.Colspan = 8; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

        }
        private void PrintFooter_DyeingBill()
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


            oPdfPCell = new PdfPCell(new Phrase(_oSampleInvoice.PreparebyName, _oFontStyle));
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


            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        #endregion

        #region Dyeing Bill V2
        public byte[] PrepareReport_DyeingBillV2(SampleInvoice oSampleInvoice, Company oCompany, BusinessUnit oBusinessUnit, List<DyeingOrderReport> oDyeingOrderReports, SampleInvoiceSetup oSampleInvoiceSetup, int nTitleType, int nPageHeight)
        {
            _oSampleInvoice = oSampleInvoice;
            _oDyeingOrderReports = oDyeingOrderReports;
            _oCompany = oCompany;
            _oBusinessUnit = oBusinessUnit;
            _oSampleInvoiceSetup = oSampleInvoiceSetup;
            _nTitleType = nTitleType;
            #region Page Setup
            _oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
            _oDocument.SetMargins(35f, 35f, 10f, 20f);
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
            if (nTitleType == 1)//normal
            {
                this.PrintHeader();
            }
            else if (nTitleType == 2)//Pad
            {
                this.PrintHeader_Blank();
            }
            if (nTitleType == 3)//imge
            {
                LoadCompanyTitle();
            }
            this.ReporttHeader();
            if (_oSampleInvoice.ApproveBy == 0)
            {
                this.PrintWaterMark(35f, 35f, 10f, 20f);
            }
            if (_oSampleInvoice.ApproveBy == (int)EnumSampleInvoiceStatus.Canceled)
            {
                this.PrintWaterMark(35f, 35f, 10f, 20f);
            }

            this.PrintHead_DyeingBill();
            this.PrintBody_DyeingBillOrderTypeV2();
            this.PrintBody_DyeingBillSummaryV2(_oDyeingOrderReports);
            this.PrintFooter_DyeingBill();

            _oPdfPTable.HeaderRows = 2;

            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        private void PrintBody_DyeingBillOrderTypeV2()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);

            List<DyeingOrderReport> oDyeingOrderReports = new List<DyeingOrderReport>();
            oDyeingOrderReports = _oDyeingOrderReports.Where(o => o.DyeingOrderType == (int)EnumOrderType.SaleOrder).ToList();
            if (_oDyeingOrderReports.Count > 0)
            {
                _sMunit = _oDyeingOrderReports[0].MUName;
                _sCurrency = _oDyeingOrderReports[0].Currency;
            }
            if (oDyeingOrderReports.Count > 0)
            {
                oDyeingOrderReports = oDyeingOrderReports.OrderBy(o => o.OrderNo).ThenBy(a => a.DyeingOrderID).ToList();
                this.PrintBody_DyeingBillV2(oDyeingOrderReports);
            }

            oDyeingOrderReports = _oDyeingOrderReports.Where(o => o.DyeingOrderType == (int)EnumOrderType.SampleOrder).ToList();
            if (oDyeingOrderReports.Count > 0)
            {
                oDyeingOrderReports = oDyeingOrderReports.OrderBy(o => o.OrderNo).ThenBy(a => a.DyeingOrderID).ToList();
                this.PrintBody_DyeingBillV2(oDyeingOrderReports);
            }
            oDyeingOrderReports = _oDyeingOrderReports.Where(o => o.DyeingOrderType == (int)EnumOrderType.ClaimOrder).ToList();
            if (oDyeingOrderReports.Count > 0)
            {
                oDyeingOrderReports = oDyeingOrderReports.OrderBy(o => o.OrderNo).ThenBy(a => a.DyeingOrderID).ToList();
                this.PrintBody_DyeingBillV2(oDyeingOrderReports);
            }
            oDyeingOrderReports = _oDyeingOrderReports.Where(o => o.DyeingOrderType == (int)EnumOrderType.TwistOrder).ToList();
            if (oDyeingOrderReports.Count > 0)
            {
                oDyeingOrderReports = oDyeingOrderReports.OrderBy(o => o.OrderNo).ThenBy(a => a.DyeingOrderID).ThenBy(a => a.BuyerCombo).ToList();
                this.PrintBody_DyeingBill_Twisting(oDyeingOrderReports);
            }
            oDyeingOrderReports = _oDyeingOrderReports.Where(o => o.DyeingOrderType == (int)EnumOrderType.ReConing).ToList();
            if (oDyeingOrderReports.Count > 0)
            {
                oDyeingOrderReports = oDyeingOrderReports.OrderBy(o => o.OrderNo).ThenBy(a => a.DyeingOrderID).ToList();
                this.PrintBody_DyeingBillV2(oDyeingOrderReports);
            }
        }
        private void PrintBody_DyeingBillV2(List<DyeingOrderReport> oDyeingOrderReports)
        {
            double nQty = 0;
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.UNDERLINE);
            string stempName = "";
            PdfPTable oPdfPTable = new PdfPTable(2);
            oPdfPTable.SetWidths(new float[] {  200f,  82f});
            if (oDyeingOrderReports.Count > 0)
            {
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oDyeingOrderReports[0].OrderTypeSt, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            }
         
            var oDyeingOrders = oDyeingOrderReports.GroupBy(x => new { x.DyeingOrderID, x.OrderNo, x.CPName }, (key, grp) =>
                               new
                               {
                                   OrderNo = key.OrderNo,
                                   DyeingOrderID = key.DyeingOrderID,
                                   CPName = key.CPName,
                               }).ToList();

             oPdfPTable = new PdfPTable(15);

            foreach (var oItem in oDyeingOrders)
            {
                var oDyeingOrderDetails = oDyeingOrderReports.Where(x => x.DyeingOrderID == oItem.DyeingOrderID).ToList();
                #region Fabric Info
                oPdfPTable = new PdfPTable(9);
                if (oItem.CPName != _oSampleInvoice.ContractorPersopnnalName)
                {
                    oPdfPTable.SetWidths(new float[] {  42f,  //Order No 
                                                    72f,  //Buyer Ref
                                                    62f,  //Concern Person
                                                    60f, //Color
                                                    95f,  //Type of count                                          
                                                    30f,  //Color Category                                              
                                                    35f,  //Qty
                                                    35f,  //U/P
                                                    45f,  //Value
                                                 });
                }
                else
                {
                    oPdfPTable.SetWidths(new float[] {  42f,  //Order No 
                                                    72f,  //Buyer Ref
                                                    47f,  //Concern Person
                                                    60f, //Color
                                                    100f,  //Type of count                                          
                                                    40f,  //Color Category                                              
                                                    45f,  //Qty
                                                    35f,  //U/P
                                                    45f,  //Value
                                                 });
                }
               
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Order No", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                if (oItem.CPName != _oSampleInvoice.ContractorPersopnnalName)
                {
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Buyer Ref", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Merchandiser", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                }
                else
                {
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Buyer Ref", 0, 2, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                }
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Color", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Yarn Type", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Shade", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Qty " + "(" + _sMunit + ")", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "U.P.", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Value", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
               
                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
                #endregion
              //  nSL = 0;
              //  oFabricWarpingPlanDetails = oFabricWarpingPlanDetails.OrderBy(o => o.StartTime).ToList();
                foreach (var oItem1 in oDyeingOrderDetails)
                {
                    oPdfPTable = new PdfPTable(9);
                    if (oItem.CPName != _oSampleInvoice.ContractorPersopnnalName)
                    {
                        oPdfPTable.SetWidths(new float[] {   42f,  //Order No 
                                                    72f,  //Buyer Ref
                                                    62f,  //Concern Person
                                                    60f, //Color
                                                    95f,  //Type of count                                          
                                                    30f,  //Color Category                                              
                                                    35f,  //Qty
                                                    35f,  //U/P
                                                    45f,  //Value
                                                 });
                    }
                    else
                    {
                        oPdfPTable.SetWidths(new float[] { 42f,  //Order No 
                                                    72f,  //Buyer Ref
                                                    47f,  //Concern Person
                                                    60f, //Color
                                                    100f,  //Type of count                                          
                                                    40f,  //Color Category                                              
                                                    45f,  //Qty
                                                    35f,  //U/P
                                                    45f,  //Value
                                                 });
                    }
               

                  ///  nSL++;
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.OrderNo, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                    if (!String.IsNullOrEmpty(oItem1.StyleNo))
                    {
                        oItem1.BuyerRef = oItem1.BuyerRef + ", " + oItem1.StyleNo;
                    }
                    if (oItem1.CPName != _oSampleInvoice.ContractorPersopnnalName)
                    {
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.BuyerRef, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.CPName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    }
                    else
                    {
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.BuyerRef, 0, 2, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    }
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.ColorName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.ProductName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL));
                    if (oItem1.RGB == "" || oItem1.RGB == null)
                    {
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.ColorNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    }
                    else
                    {
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.RGB, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    }
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem1.Qty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oSampleInvoice.CurrencySymbol + "" + System.Math.Round(oItem1.UnitPrice, 4).ToString("#,##0.00##;(#,##0.00##)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oSampleInvoice.CurrencySymbol + "" + (oItem1.Qty * oItem1.UnitPrice).ToString("#,##0.00##;(#,##0.00##)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                

                    oPdfPTable.CompleteRow();
                    ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
                }
                oPdfPTable = new PdfPTable(9);
                if (oItem.CPName != _oSampleInvoice.ContractorPersopnnalName)
                {
                    oPdfPTable.SetWidths(new float[] {   42f,  //Order No 
                                                    72f,  //Buyer Ref
                                                    62f,  //Concern Person
                                                    60f, //Color
                                                    95f,  //Type of count                                          
                                                    30f,  //Color Category                                              
                                                    35f,  //Qty
                                                    35f,  //U/P
                                                    45f,  //Value
                                                 });
                }
                else
                {
                    oPdfPTable.SetWidths(new float[] { 42f,  //Order No 
                                                    72f,  //Buyer Ref
                                                    47f,  //Concern Person
                                                    60f, //Color
                                                    100f,  //Type of count                                          
                                                    40f,  //Color Category                                              
                                                    45f,  //Qty
                                                    35f,  //U/P
                                                    45f,  //Value
                                                 });
                }

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Sub Total", 0, 6, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                nQty = oDyeingOrderDetails.Where(x => x.DyeingOrderID == oItem.DyeingOrderID).Select(c => c.Qty).Sum();
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                nQty = oDyeingOrderDetails.Where(x => x.DyeingOrderID == oItem.DyeingOrderID).Select(c => c.Qty*c.UnitPrice).Sum();
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (nQty).ToString("#,##0.00##;(#,##0.00##)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                stempName = oItem.CPName;
                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            }

            oPdfPTable = new PdfPTable(9);
            if (stempName != _oSampleInvoice.ContractorPersopnnalName)
            {
                oPdfPTable.SetWidths(new float[] { 42f,  //Order No 
                                                    72f,  //Buyer Ref
                                                    62f,  //Concern Person
                                                    60f, //Color
                                                    95f,  //Type of count                                          
                                                    30f,  //Color Category                                              
                                                    35f,  //Qty
                                                    35f,  //U/P
                                                    45f,  //Value
                                                 });
            }
            else
            {
                oPdfPTable.SetWidths(new float[] {  42f,  //Order No 
                                                    72f,  //Buyer Ref
                                                    47f,  //Concern Person
                                                    60f, //Color
                                                    100f,  //Type of count                                          
                                                    40f,  //Color Category                                              
                                                    45f,  //Qty
                                                    35f,  //U/P
                                                    45f,  //Value
                                                 });
            }
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total", 0, 6, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);

            nQty = oDyeingOrderReports.Select(c => c.Qty).Sum();
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable,"", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            nQty = oDyeingOrderReports.Select(c => c.Qty*c.UnitPrice).Sum();
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, (nQty).ToString("#,##0.00##;(#,##0.00##)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
       
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

        }
        private void PrintBody_DyeingBillSummaryV2(List<DyeingOrderReport> oDyeingOrderReports)
        {
            //#region
            //nUsagesHeight = CalculatePdfPTableHeight(_oPdfPTable);
            //if (nUsagesHeight < 400)
            //{
            //    nUsagesHeight = 400 - nUsagesHeight;
            //}
            //if (nUsagesHeight > 400)
            //{
            //    #region Blank Row


            //    while (nUsagesHeight < 400)
            //    {
            //        #region Table Initiate
            //        PdfPTable oPdfPTableTemp = new PdfPTable(4);
            //        oPdfPTableTemp.SetWidths(new float[] { 148f, 148f, 148f, 148f });

            //        #endregion

            //        _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
            //        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            //        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.FixedHeight = 18f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableTemp.AddCell(_oPdfPCell);

            //        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            //        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.FixedHeight = 18f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableTemp.AddCell(_oPdfPCell);

            //        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            //        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.FixedHeight = 18f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableTemp.AddCell(_oPdfPCell);

            //        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            //        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.FixedHeight = 18f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableTemp.AddCell(_oPdfPCell);


            //        oPdfPTableTemp.CompleteRow();

            //        _oPdfPCell = new PdfPCell(oPdfPTableTemp);
            //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //        _oPdfPTable.CompleteRow();

            //        nUsagesHeight = CalculatePdfPTableHeight(_oPdfPTable);
            //    }

            //    #endregion
            //}
            //#endregion
            PdfPTable oPdfPTable = new PdfPTable(9);

            oPdfPTable.SetWidths(new float[] {  
                                                    38f,  //Order No 
                                                    85f,  //Buyer Ref
                                                    60f,  //Concern Person
                                                    65f, //Color
                                                    90f,  //Type of count                                          
                                                    28f,  //Color Category                                              
                                                    43f,  //Qty
                                                    28f,  //U/P
                                                    43f,  //Value
                                                 });

            #region Header Row

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Summary", 0, 4, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 4, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Order Qty(" + _sMunit + ")", 0, 2, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total Value", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 4, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyle);
            oPdfPTable.CompleteRow();

            _nTotalQty = _oDyeingOrderReports.Where(c => c.DyeingOrderType == (int)EnumOrderType.SaleOrder).Sum(x => x.Qty);
            if (_nTotalQty > 0)
            {
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total Bulk Qty", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(_nTotalQty) + " " + _sMunit, 0, 2, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                _nTotalAmount = _oDyeingOrderReports.Where(c => c.DyeingOrderType == (int)EnumOrderType.SaleOrder).Sum(x => (x.Qty * x.UnitPrice));
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oSampleInvoice.CurrencySymbol + " " + Global.MillionFormat(_nTotalAmount), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 4, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyle);
                oPdfPTable.CompleteRow();
            }

            _nTotalQty = _oDyeingOrderReports.Where(c => c.DyeingOrderType == (int)EnumOrderType.ClaimOrder).Sum(x => x.Qty);
            if (_nTotalQty > 0)
            {
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total Demand Order Qty", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(_nTotalQty) + " " + _sMunit, 0, 2, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                _nTotalAmount = _oDyeingOrderReports.Where(c => c.DyeingOrderType == (int)EnumOrderType.ClaimOrder).Sum(x => (x.Qty * x.UnitPrice));
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oSampleInvoice.CurrencySymbol + " " + Global.MillionFormat(_nTotalAmount), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 4, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyle);
                oPdfPTable.CompleteRow();
            }
            _nTotalQty = _oDyeingOrderReports.Where(c => c.DyeingOrderType == (int)EnumOrderType.SampleOrder).Sum(x => x.Qty);
            if (_nTotalQty > 0)
            {

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total Sample Qty", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(_nTotalQty) + " " + _sMunit, 0, 2, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                _nTotalAmount = _oDyeingOrderReports.Where(c => c.DyeingOrderType == (int)EnumOrderType.SampleOrder).Sum(x => (x.Qty * x.UnitPrice));
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oSampleInvoice.CurrencySymbol + " " + Global.MillionFormat(_nTotalAmount), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 4, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyle);
                oPdfPTable.CompleteRow();
            
            }
            _nTotalQty = _oDyeingOrderReports.Where(c => c.DyeingOrderType == (int)EnumOrderType.TwistOrder).Sum(x => x.Qty);
            if (_nTotalQty > 0)
            {

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total Twist Order Qty", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(_nTotalQty) + " " + _sMunit, 0, 2, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                _nTotalAmount = _oDyeingOrderReports.Where(c => c.DyeingOrderType == (int)EnumOrderType.TwistOrder).Sum(x => (x.Qty * x.UnitPrice));
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oSampleInvoice.CurrencySymbol + " " + Global.MillionFormat(_nTotalAmount), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 4, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
                oPdfPTable.CompleteRow();

            }
            _nTotalQty = _oDyeingOrderReports.Where(c => c.DyeingOrderType == (int)EnumOrderType.ReConing).Sum(x => x.Qty);
            if (_nTotalQty > 0)
            {

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total ReConing Qty", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(_nTotalQty) + " " + _sMunit, 0, 2, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                _nTotalAmount = _oDyeingOrderReports.Where(c => c.DyeingOrderType == (int)EnumOrderType.ReConing).Sum(x => (x.Qty * x.UnitPrice));
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oSampleInvoice.CurrencySymbol + " " + Global.MillionFormat(_nTotalAmount), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 4, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyle);
                oPdfPTable.CompleteRow();

            }
            if (_oSampleInvoice.SampleInvoiceCharges.Count > 0)
            {

                foreach (SampleInvoiceCharge oItem in _oSampleInvoice.SampleInvoiceCharges)
                {
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.Name, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 2, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);

                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oSampleInvoice.CurrencySymbol + " " + Global.MillionFormat(oItem.Amount), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 4, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
                    oPdfPTable.CompleteRow();
                }

            }
            else { 
            
                if(_oSampleInvoice.Charge>0)
                {
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Add Charge", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 2, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);

                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oSampleInvoice.CurrencySymbol + " " + Global.MillionFormat(_oSampleInvoice.Charge), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 4, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
                    oPdfPTable.CompleteRow();
                }
                if (_oSampleInvoice.Discount > 0)
                {
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Discount", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 2, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);

                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oSampleInvoice.CurrencySymbol + " " + Global.MillionFormat(_oSampleInvoice.Discount), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 4, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
                    oPdfPTable.CompleteRow();
                }
            }

            _nTotalQty = _oDyeingOrderReports.Select(c => c.Qty).Sum();
            if (_nTotalQty > 0)
            {

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Grand Total", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(_nTotalQty) + " " + _sMunit, 0, 2, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                _nTotalAmount = _oDyeingOrderReports.Select(c => (c.Qty * c.UnitPrice)).Sum();
                _nTotalAmount = _nTotalAmount - _oSampleInvoice.Discount + _oSampleInvoice.Charge;
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oSampleInvoice.CurrencySymbol + " " + Global.MillionFormat(_nTotalAmount), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 4, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
                oPdfPTable.CompleteRow();

            }
             if ((EnumOrderPaymentType)_oSampleInvoice.PaymentType == EnumOrderPaymentType.CashOrCheque)
            {
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Equivalent Taka", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                _nTotalAmount = _oDyeingOrderReports.Select(c => (c.Qty * c.UnitPrice)).Sum();
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(_nTotalAmount) + "x" + _oSampleInvoice.ConversionRate.ToString("00.00"), 0, 2, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oSampleInvoice.ExchangeCurrencySymbol + " " + Global.TakaFormat(Math.Round((_nTotalAmount * _oSampleInvoice.ConversionRate), 2)), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 4, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
                oPdfPTable.CompleteRow();

             }
               if ((EnumOrderPaymentType)_oSampleInvoice.PaymentType == EnumOrderPaymentType.CashOrCheque)
              {
                  ESimSolItexSharp.SetCellValue(ref oPdfPTable, "In Words", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.TakaWords(Math.Round((_nTotalAmount * _oSampleInvoice.ConversionRate), 2)), 0, 8, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
                oPdfPTable.CompleteRow();
               }
               else
               {
                   ESimSolItexSharp.SetCellValue(ref oPdfPTable, "In Words", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.DollarWords(_nTotalAmount), 0, 8, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                oPdfPTable.CompleteRow();
               }
               ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Remarks", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
             ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oSampleInvoice.Remark, 0, 8, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                oPdfPTable.CompleteRow();

            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            #endregion
        }
        #endregion


        private void Blank(int nFixedHeight)
        {

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Colspan = 1; _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = nFixedHeight; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        private void PrintWaterMark(float marginLeft, float marginRight, float marginTop, float marginBottom)
        {
            if (_oSampleInvoice.ApproveBy == 0)
            {
                _sWaterMark = "Unauthorised";
            }
            if (_oSampleInvoice.ApproveBy == (int)EnumSampleInvoiceStatus.Canceled)
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
            //PageEventHandler.FooterNote = _oBusinessUnit.ShortName + " Concern Person: " + _oSampleInvoice.MKTPName;
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler

            _oDocument.Open();
            PdfContentByte cb = PDFWriter.DirectContent;
            _oDocument.NewPage();
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
